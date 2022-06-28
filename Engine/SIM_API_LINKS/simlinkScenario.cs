using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace SIM_API_LINKS
{

    public enum CohortType
    {
        UNDEFINED = -1,
        PARALLEL_EVENTS,        //typical case
        SCENARIO_INIT           // first scenario 
    }
    
    // used to store basic cohort information
    public class cohortSpec
    {
        public CohortType _cohortType = CohortType.PARALLEL_EVENTS;
        public bool _bHasSummaryEG = false;         //whether the last EG is a simlink XMODEL for summary
        public Dictionary<int, List<int>> _dictScenXREF = new Dictionary<int, List<int>>();
        public Dictionary<int, int> _dictEG_Order = new Dictionary<int, int>();
        public Dictionary<int, int> _dictScenSummary_Order = new Dictionary<int, int>();    //used to track which order the scen in a summary eg are in... used to get right scenarios from ref datasets
        public bool _bIsRegular = true; // todo: a test whether the cohort is well formed (same number of scen in each)
        
    }
    
    
    /// <summary>
    /// added to support processeing cohorts, eg, and scenarios (during cohort addition)
    /// </summary>
    public partial class simlink
    {
        #region EG
         


        // copied from swmm 7/31/16 
        // todo: use only this for all classes (nothign special about swmm process eg)
        /// <param name="astrScenarioId2Run"></param>
        public virtual void ProcessEvaluationGroup(string[] astrScenarioId2Run)                //int nEvalID, int nModelTypeID, int nRefScenarioID = -1, bool bSkipDictionaryPopulate = false, int nSingleScenario = -1, string sOptionalFileLocation = "NOTHING")
        {
            DataSet dsEvals = ProcessEG_GetGS_Initialize(_nActiveEvalID, astrScenarioId2Run);       //, nRefScenarioID);
            //now performed in init... LoadReferenceDatasets();            //initialize datasets

            //SP 25-Jul-2016 now can be read from config
            //string sLogPath = System.IO.Path.GetDirectoryName(_sActiveModelLocation);
            //sLogPath = sLogPath.Substring(0, sLogPath.LastIndexOf("\\")) + "\\LOGS";
            _nLoopNumber = 0;
            foreach (DataRow dr in dsEvals.Tables[0].Rows)
            {
                try
                {
                    ProcessScenario(dr, _sLogLocation_Simlink);
                    _nLoopNumber++;
                }
                catch (Exception ex)
                {
                    _log.AddString(string.Format("Error in processing evaluation group {0}. Error: {1}", dr["ScenarioID"].ToString(), ex.Message), Logging._nLogging_Level_1);
                }
                finally
                {
                    _log.WriteLogFile(true);
                }

                if (_nLoopsOpenCloseDB > 0)   // only check if this is a positive val
                {
                    if (_nLoopsOpenCloseDB == _nLoopNumber)
                    {
                        _dbContext.OpenCloseDBConnection();
                        _nLoopNumber = 0;
                        _log.AddString("Opening and closing the data connection for speed. Set Loops in XML to avoid or modify frequency",Logging._nLogging_Level_2,true,true);
                    }
                }

            }
            // now process the remainder of the condor loop
            if (_compute_env != ComputeEnvironment.LocalMachine)
            {
                //test 1: wait for all jobs to come bac (Easiest to get from hpc wrap
                //test 2: loop over jobs and check, as time reqs may vary 

                /*
                //test1
                //todo : put in soe testing, or breaks, or something. 
                _hpc.WaitForAllJobs();
                foreach (DataRow dr in dsEvals.Tables[0].Rows)
                {
                    ProcessScenario(dr, _sLogLocation_Simlink);
                    _log.WriteLogFile(true);            //writes a new log file- better to write to the first one.  not critical path
                }
                */

                //test2
                while (!_hpc.areAllJobsComplete())
                {
                    try
                    {
                        int jobScenarioID = 1;  // _hpc.WaitForFirstJob(returnJobUserID: true);        //  1;  // bojangles, MET don't commit.. 
                        //Which row holds the scenario information
                        DataRow dr = dsEvals.Tables[0].Select("ScenarioID = " + jobScenarioID.ToString()).First();
                        ProcessScenario(dr, _sLogLocation_Simlink);
                    }
                    catch (Exception ex)
                    {
                        _log.AddString(string.Format("Error in retrieving results. Error: {0}", ex.Message), Logging._nLogging_Level_1);
                    }
                    finally
                    {
                        _log.WriteLogFile(true);            //writes a new log file- better to write to the first one.  not critical path
                    }
                }

            }
        }

        //SP 8-Sept-2017 - simple for now, can be expanded upon by passing in dictionary args through CLI for passing into the ExportTimeseriestoDSSByEval function
        public virtual void ConvertEGCompletedResultsToDSS()               
        {
            try
            {
                //TODO - log file hasn't been initialized yet - therefore currently not writing
                _log.AddString(string.Format("Attempting to write results to DSS for Evaluation group {0}", _nActiveEvalID), Logging._nLogging_Level_1);
                ExportTimeseriestoDSSByEval(_nActiveEvalID, _sWriteEGToDSSLocation, false);
                _log.AddString(string.Format("Completed writing results to DSS"), Logging._nLogging_Level_1);
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Failed writing results to DSS for Evaluation group {0}", _nActiveEvalID), Logging._nLogging_Level_1);
            }
        }
        #endregion

        #region scenario

        /// <summary>
        /// read the datarow and run a scenario. return the last step reached by the scenario
        ///     moved from swmm 7/31/16
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public int ProcessScenario(DataRow dr, string sLogPath)
        {
            try
            {
                _log.Initialize("logEG_" + _nActiveEvalID.ToString() + "_" + dr["ScenarioID"].ToString(), _log._nLogging_ActiveLogLevel, sLogPath);   //begin a logging session
                int nProjID = _nActiveProjID;           // or get from dr Convert.ToInt32(dsEvals.Tables[0].Rows[0]["ProjID"].ToString());
                string sFileLocation = dr["ModelFile_Location"].ToString();         //todo: proj/sFile should only be set once; minor time penalty so no issue
                int nScenarioID = Convert.ToInt32(dr["ScenarioID"].ToString());
                
                //SP 21-Dec-2016 In each class, if ScenarioID = -1, _nActiveScenarioID is set to Scenario after insert, however if processing an existing scenario, nActiveScenario is not set. Do here?
                _nActiveScenarioID = nScenarioID;

                int nScenStart = Convert.ToInt32(dr["ScenStart"].ToString());
                int nScenEnd = Convert.ToInt32(dr["ScenEnd"].ToString());
                string sDNA = dr["DNA"].ToString();

                if (_bClearScenarioDetails) //SP 8-Aug-2016 added to config   
                    DeleteScenariosWrapper(false, _nActiveEvalID, nScenStart, nScenEnd, nScenarioID.ToString());

                _log.AddString("", Logging._nLogging_Level_1, false);
                _log.AddString(string.Format("----Process Scenario {0}----", dr["ScenarioID"].ToString()), Logging._nLogging_Level_1, false);
                int nCurrentStep = ProcessScenario(nProjID, _nActiveEvalID, _nActiveReferenceEvalID, sFileLocation, nScenarioID, nScenStart, nScenEnd, sDNA);

                dr["ScenStart"] = nCurrentStep.ToString();      //record step reached, for subsequent processing
                return nCurrentStep;
            }
            catch (Exception ex)
            {
                //todo: log the issue
                _log.AddString(string.Format("Error in processing evaluation group {0}. Error: {1}", dr["ScenarioID"].ToString(), ex.Message), Logging._nLogging_Level_1);
                throw;
                return -1;
            }
        }
            
            /// <summary>
        /// Delegate for use with optimization....
        /// </summary>
        /// <param name="vars"></param>
        /// <param name="objs"></param>
        /// <param name="constrs"></param>
        public virtual void ProcessScenario(double[] vars, double[] objs, double[] constrs)
        {
            // override in cases? or default has some clever way of calling a function in an external lib?
            int n = 1;

            //SP 10-Oct-2016 Write log file after finished processing each scenario processing
            _log.WriteLogFile();
        }
        public virtual void ProcessScenario_BORG(double[] vars, double[] objs, double[] constrs)
        {
            objs[0] = Math.Pow(vars[0], 2);
            objs[1] = -1 * Math.Pow((vars[1] - 2), 2);
            objs[2] = vars[0] * vars[1];
        }


              //SP 22-Jun-2016 - original taken from EPANET and modified for ExtendSim
        // met 7/31/16: added simlink process scenario following extendsim... deleted most "stuff"
            //purpose: be able to perform calcs/aggregation on results across a cohort....

        // note 12/28/16: add label
        public virtual int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sInputFile, int nScenarioID = -1, int nScenStartAct = 1,
            int nScenEndAct = 100, string sDNA = "-1", string sLabel="DEFAULT")
        {
            string sPath = ""; 
            string sTargetPath; 
            string sTarget_MOX; 
            string sIncomingMOX;
            int nCurrentLoc = nScenStartAct; 
            string sTS_Filename = "";

            ScenDS_ClearAfterScenario(nScenarioID); //SP 9-Mar-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

            try
            {
                if (true)     //we should have a valid ScenarioID at this point.
                {
                    if (_bIsOptimization || (nScenarioID == -1))           //nScenarioID  = -1
                    {
                        if (sLabel == "DEFAULT")
                        {
                            sLabel = System.DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss.fff");       // set base label only if not provided by caller
                        }
                        
                        //SP 10-Jun-2016 TODO Ideally want to shift writing / reading from the database out of this routine - increased precision of datetime as there are not more than one able to be processed per second
                        nScenarioID = InsertScenario(nEvalID, nProjID, sLabel, "", sDNA);       //pass the current date time to enable easy retrieval of PK (should be better wya to do this)
                        _nActiveScenarioID = nScenarioID;
                    }

                    //    ScenarioPrepareFilenames(nScenarioID, nEvalID, sMOXLocation, out sTargetPath, out sIncomingMOX, out sTarget_MOX, out sTS_Filename);
                    //LoadScenarioDatasets(nScenarioID, nScenStartAct, nScenarioID == _nActiveBaselineScenarioID);                       //, sTS_Filename);           //loads datasets needed for the scenario if not starting from beginning (in which case ds are constructed through process);

                    //SP 15-Feb-2017 ExtractExternalData for RetrieveCode = AUX
                    ScenarioGetExternalData();

                    sPath = System.IO.Path.GetDirectoryName(sInputFile);
                    bool bContinue = true;              //identifies whether to continue
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModElementExist) && (nScenEndAct >= CommonUtilities.nScenLCModElementExist))       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
                    {
                        if (sDNA != "-1")
                        {
                            //not an optimization run, no DNA is passed
                            nScenarioID = DistribDNAToScenario(sDNA, nEvalID, nReferenceEvalID, nProjID, 1, -1, nScenarioID);
                            if (nScenarioID == -1) { bContinue = false; }       // some failure in the DNA distribution
                            else
                            {
                                nCurrentLoc = CommonUtilities.nScenLCModElementExist;
                            }
                            _log.UpdateFileOutName("logEG_" + nEvalID.ToString() + "_" + nScenarioID.ToString());
                        }
                        else
                        {
                            nCurrentLoc = CommonUtilities.nScenLCModElementExist;          //
                        }
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineFileSetup) && (nScenEndAct >= CommonUtilities.nScenLCBaselineFileSetup))
                    {
                        nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineModified) && (nScenEndAct >= CommonUtilities.nScenLCBaselineModified))
                    {
                        nCurrentLoc = CommonUtilities.nScenLCBaselineModified;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                        nCurrentLoc = CommonUtilities.nScenLModelResultsTS_Read;
                    }
                    ProcessScenario_COMMON(nReferenceEvalID, nScenarioID, nCurrentLoc, nScenEndAct, sTS_Filename);        //call base function to perform modeltype independent actions
                    if (_slXMODEL != null)
                    {
                    }
                    UpdateScenarioStamp(nScenarioID, nCurrentLoc);  //store the time the scenario is completed, along with the "stage" of the Life Cycle //SP 10-Jun-2016 now only changes in memory
                    WriteResultsToDB(nScenarioID);
                    //_log.WriteLogFile();               //SP 3-Oct-2016 written in the inherited loop, allows for catching exceptions             //file only written if >0 lines to be written
                }
                return nCurrentLoc;
            }

            catch (Exception ex)                //log the error
            {
                _log.AddString("Exception: " + ex.Message + " : ", Logging._nLogging_Level_1);
                return 0;   //TODO: refine based upon code succes met 6/12/2012
            }
        }    

        public virtual int ProcessScenario(int nScenarioID, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1")
        {
            return -1;
        }
        #endregion

        #region cohort

        protected bool IsLeadEGInCohort(int nEvalID)
        {
            return _cohortSpec._dictEG_Order[nEvalID] == 0;
        }

        protected bool IsInCohort()
        {
            return _cohortSpec != null;
        }

        // return true if valid cohort of type scenario init
        public bool IsCohortTypeInit()
        {
            if (_cohortSpec != null)
            {
                if (_cohortSpec._cohortType == CohortType.SCENARIO_INIT)
                    return true;
                else
                    return false;
            }            
           return false;
        }


        /// <summary>
        /// Return a cohort
        /// </summary>
        /// <param name="nEvalID"></param>
        /// <returns></returns>
        private int HelperGetCohortIDFromEG(int nEvalID)
        {
            int nReturn = -1;
            string sSQL = "SELECT CohortID from tblEvaluationGroup where (EvaluationID = " + nEvalID + ")";
            DataSet dsCohort = _dbContext.getDataSetfromSQL(sSQL);
            if (dsCohort.Tables[0].Rows.Count >= 1)
            {
                nReturn = Convert.ToInt32(dsCohort.Tables[0].Rows[0][0]);     
            }
            return nReturn;
        }


        public DataSet EG_GetCohortDS(int nEvalID, int nProjID, bool bFirstArgIsCohort =false)
        {
            int nCohortID = nEvalID;        // met10312017- this is a set up weird- adding some 
            if(!bFirstArgIsCohort)
                nCohortID = HelperGetCohortIDFromEG(nEvalID);   // go get the cohort based on eg if you need to.
            
            if (nCohortID == -1)        // not a real CohortID
            {
                nCohortID = -1234543;       // set to a bad val sure to return nothing (empty DS)

            }
            string sSQL = "SELECT EvaluationID, EvaluationLabel, ModelFileLocation, ModelType_ID, ReferenceEvalID_FK, ScenarioID_Baseline_FK, IntermediateResultCode, IsModFileUserDefined, ModFileKey, CohortID, CohortSQN, CohortType, IsXModel"
            + " FROM tblEvaluationGroup"
            + " WHERE (((CohortID)=" + nCohortID + ") AND ((ProjID_FK)=" + nProjID + "))"
            + " ORDER BY CohortSQN";

            DataSet dsCohort = _dbContext.getDataSetfromSQL(sSQL);
            return dsCohort;
        }

        /// <summary>   added met 7/30/16
        /// Step through a series of EG in order.
        ///     todo: last EG can be an xmodel type simlink to do performance characteristics on this stuff...
        ///
        /// 
        ///     COHORT version 1:
        ///         user sets up identical scen
        ///         scen performed identical to normal means
        ///     COHORT version 2:
        ///         MEV generated for "lead" EG (cohortsqn==1)... ref by others
        ///     COHORT version 3:
        ///         cohort manages setting up dup sims etc...
        ///         
        /// Update 12/26/16: Modified to use new member dataset: _dsEG_Cohort
        /// </summary>
        /// <param name="nProjID"></param>
        /// <param name="nCohortID"></param>
        public void ProcessCohort(int nProjID, int nCohortID, int nSQN =-1)
        {
            InitCohort();                               // go through and define data structure that is needed for the scenarios of EG to be referenceable
            foreach (DataRow drEG in _dsEG_Cohort.Tables[0].Rows)
            {
                bool bIsXModel = Convert.ToBoolean(drEG["IsXModel"].ToString());
                int nEvalID = Convert.ToInt32(drEG["EvaluationID"].ToString());
                if (!bIsXModel )         // question: is Xmodel what we really mean here? or do we mean synthetic?>?
                {
                    if (true)      //switch if needed no NOT run the data retrieval step
                    {
                        // step 1: Initialize the EG
                        // todo: if same REF EG , then one should just ACTIVATE, not INIT. Not yet implemented!!
                        InitializeEG(nEvalID);
                        // step 2: process EG
                        ProcessEvaluationGroup(new string[] { });
                        _bIsLeadEGInCohort = false;
                    }
                }
                else
                {
                    // met testing function 11/23/16
                        // this should be paratmerized once the range of cohort processing needs is further thought through
                    if (true)
                    {
                        _dResultTS_Vals = SynthTimeSeries(nCohortID, nSQN);        
                        string sTargetPath = Path.GetDirectoryName(_sActiveModelLocation);
                        SetTS_FileName(15489, sTargetPath);
                        _hdf5.hdfCheckOrCreateH5(_hdf5._sHDF_FileName);
                        _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
                        WriteTimeSeriesToRepo();
                        _hdf5.hdfClose();
                    }
                    else
                    {

                        // xmodel: v1 we just init and run the xmodel that isneeded
                        //init and execute here
                        //PURPOSE: roll up some stats across these EG that have been evaluated.
                        _slXMODEL = CommonUtilities.GetSimLinkObject(Convert.ToInt32(drEG["ModelType_ID"].ToString()));
                        //      _slXMODEL.InitializeModelLinkage(_dbContext._sConnectionString, (int)_dbContext._DBContext_DBTYPE);  //init to same connection
                        _slXMODEL._dbContext = _dbContext;
                        _slXMODEL.InitializeModelLinkage("alreadyset", -1);
                        _slXMODEL.InitializeEG(nEvalID);
                        _slXMODEL._lstSimLinkDetail = _lstSimLinkDetail;    //bad: dup in memory- also possible list mod issues...  todo: consider ways to avoid when xmodel is refined more
                        _slXMODEL._cohortSpec = _cohortSpec;        //set the cohort spec for xmodel to use
                        _slXMODEL.ProcessEvaluationGroup(new string[] { });
                    }
                }
            }
        }

        /// <summary>
        /// Return a list of scenarios belong to a certain generation of a cohort.
        /// 
        /// NOTE: This requires that SQN be incremented if more than one cohort in a cohort. otherwise default of -1
        /// </summary>
        /// <param name="nCohortID"></param>
        /// <param name="nSQN"></param>
        /// <returns></returns>
        private int[] GetScenListByCohort(int nCohortID, int nSQN){
            string sSQL = "select scenarioID from tblScenario where ((EvalGroupID_FK in "
                           + "(select EvaluationID from tblEvaluationGroup where (CohortID = " + nCohortID + ")))"
                           + " AND (SQN = " + nSQN + "))";
            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
            int[] nReturn = new int[ds.Tables[0].Rows.Count];
            for(int i=0;i<ds.Tables[0].Rows.Count;i++){
                nReturn[i] = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
            }
            return nReturn;
        }

        // return a list of scenarios between two ranges
            //11/21/17: baby due date, add eval id filter
        private int[] GetScenListByRange(int nScenarioID_Low, int nScenarioID_High, int nEvalID)
        {
            string sSQL = "select scenarioID from tblScenario where "
                +"(scenarioID between " + nScenarioID_Low.ToString() +" and " + nScenarioID_High.ToString()
                +" and EvalGroupID_FK = " + nEvalID + ")";


            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
            int[] nReturn = new int[ds.Tables[0].Rows.Count];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                nReturn[i] = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
            }
            return nReturn;
        }


        /// <summary>
        /// Loop over the array of EG a cohort, read a resultTS, and use the count of records to get the stitched count 
        /// Phase 1: Assuem these all have the same RefEvalID 
        ///     4/2/17: update to work if not in cohort.
        /// 
        /// </summary>
        /// <param name="nScenList"></param>
        /// <returns></returns>
        int GetSyntRecordCount(int[] nScenList)
        {
            int nCounter = 0;
            int nRunningTotal = 0;
            int nEvalID_Orig = _nActiveEvalID;                                          // store this to set back to for continuing
            int nEvalID = _nActiveEvalID;
            bool bIsXModel = false;
            DataRow drEG = _dsEG_Cohort.Tables[0].NewRow();
            foreach (int nScenarioID in nScenList)                          //foreach (DataRow drEG in _dsEG_Cohort.Tables[0].Rows)                          //foreach (int nScenarioID in nScenList)            
            {
                if (_bIsCohort)             // set cohort specific aspects of the synth  (updated 4/2/17 to work outside the synth setting)
                {
                    drEG = _dsEG_Cohort.Tables[0].Rows[nCounter];
                    bIsXModel = Convert.ToBoolean(drEG["IsXModel"].ToString());
                    nEvalID = Convert.ToInt32(drEG["EvaluationID"].ToString());
                    SetActiveEvalID(nCounter, EvalActivationCode.Cohort, nCounter);
                }


                if (!bIsXModel)         // the last IsXmodel EG is for synth; don't process
                {
                    string sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, nEvalID, true);
                    SetTS_FileName(nScenarioID, sTargetPath);
                    EGDS_GetTS_Details(nScenarioID);
                    //SP 13-Oct-2017 changed to get the first non-null value. Can be null if only getting secondary and AUX vars
                    int nLocalCount = _dResultTS_Vals.First(x => x != null).GetLength(0);          // bojangles: better to use the EG and make sure that this is not an aux. TODO SP 15-Feb-2017 Confirm this is as intended with change to single DS for timeseries
                    nRunningTotal += nLocalCount;
                }
                nCounter++;
            }

            SetActiveEvalID(nEvalID_Orig);                              // reset to the orig eval id for continuing
            
            //reset the TS to the first scenario... MET 9/21/17  - this was done for eg 172 (170) for mill creek.
            if (!_bIsCohort)        // this has NOT been checked... needs to be verified.
            {
                if (nScenList.Length > 0)
                {
                    SetTS_FileName(nScenList[0], CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenList[0], nEvalID, true));
                    EGDS_GetTS_Details(nScenList[0]);           
                }
            }
            return nRunningTotal;
        }

        // todo: add functionality for many scenarios
        public double[][,] SynthTimeSeries(int nCohortID, int nSQN = -1, bool bUseCohort=true)
        {
            int[] nScenList;
            if (bUseCohort)
                nScenList = GetScenListByCohort(nCohortID, nSQN);     // list of scenarios in this SQN of this cohort     
            else
                nScenList = GetScenListByRange(nCohortID, nSQN, _nActiveEvalID);  // low and higher range
            int nTS_Count = _dsEG_ResultTS_Request.Tables[0].Rows.Count;  // todo: need to support the multiple ts type //SP 15-Feb-2017 As per METs comment, kept this as the combined ds;
            double[][,] dSynth = new double[nTS_Count][,];
            int nRowOffset = 0;
            bool bFirstPass=true;           
            string sTargetPath="";
            int nCounter = 0;
            bool bIsXModel = false;
            int nEvalID = _nActiveEvalID;

            // update the looping over scenarios to support BOTH user defined scnearios and scenlist (which are almost identical)
            // commented as synth2; remove once performance proved in both modes.
            foreach (int nScenarioID in nScenList)  
//synth2            foreach (DataRow drEG in _dsEG_Cohort.Tables[0].Rows)                          //foreach (int nScenarioID in nScenList)            
            {
                //processing slightly unique for cohort... set cohort level vars
                if (_bIsCohort)
                {
                    bIsXModel = Convert.ToBoolean(_dsEG_Cohort.Tables[0].Rows[nCounter]["IsXModel"].ToString());
                    nEvalID = Convert.ToInt32(_dsEG_Cohort.Tables[0].Rows[nCounter]["EvaluationID"].ToString());
                }
                if (!bIsXModel)         // the last IsXmodel EG is for synth; don't process
                {

                    if (bFirstPass)
                    {
                        InitializeEG(nEvalID);      //bojangles!
                    }
                    else
                    {
                        if (_bIsCohort)
                            SetActiveEvalID(nEvalID, EvalActivationCode.Cohort, nCounter);      // Set simlink vars for the EG we are working with  (ONLY NEEDED if working with cohort)
                    }

                    if (false)
                    {
                        sTargetPath = Path.GetDirectoryName(_sActiveModelLocation);  // better way? .  dr[""].ToString();
                        //have not tested this yet...  _dResultTS_Vals = new double[nTS_Count][,];     // init here?
                    }
                    else
                    {
                        sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, nEvalID, true);
                    }
                    // set the TS information on the simlink object itself 
                    SetTS_FileName(nScenarioID, sTargetPath);
                    EGDS_GetTS_Details(nScenarioID);                            // get timeseries arrays
                                                                                //met 5/14/17   int nIndex = 0;     
                    //SP 13-Oct-2017 changed to get the first non-null value. Can be null if only getting secondary and AUX vars
                    int nRecordCount_Component = _dResultTS_Vals.First(x => x != null).GetLength(0);      //SP 15-Feb-2017 TODO Consider getting nIndex from dictIndices for each TS  met 5/14/17 finally got around to this...  

                    _log.AddString(string.Format("Retrieve {0} records from {1}", nRecordCount_Component.ToString(), _hdf5._sHDF_FileName), Logging._nLogging_Level_2, true, true);
                    if (bFirstPass)     // now create empy ts arrays 
                    {
                        int nCountRecords = GetSyntRecordCount(nScenList);      // support irregular sized batches intervals                

                        for (int i = 0; i < nTS_Count; i++)
                        {
                            double[,] d = new double[nCountRecords, 1];         // initialize an empty array of the proper length needed to accomodate all results
                            dSynth[i] = d;
                        }
                        bFirstPass = false;
                    }
                    int nIndex = -1;
                    foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Rows)       // todo:  .Select("RetrieveCode = " + (int)RetrieveCode.Primary))       // todo = 
                    {
                        // transfer the timeseries data to the synth ts
                        nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())];

                        int nResultID = Convert.ToInt32(dr["ResultTS_ID"].ToString());
                        if (nResultID==65187){
                            int n = 1;
                        }

                        for (int i = 0; i < nRecordCount_Component; i++)
                        {

                            // test for null below... 
                            if (_dResultTS_Vals[nIndex] != null) //SP 13-Oct-2017
                                dSynth[nIndex][i + nRowOffset, 0] = _dResultTS_Vals[nIndex][i, 0];          /// todo: support for more than 2 dimensions //SP 15-Feb-2017 TODO Consider getting nIndex from dictIndices for each TS
                        }
                     //   nIndex++;
                    }
                    // set the row offset for the next guy
                    nRowOffset += nRecordCount_Component;              // assumes all ts are of same length
                    nCounter++;     // counter for etting the right EG
                }
            }
            return dSynth;
        }

        #endregion

    }
}
