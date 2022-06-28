using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using InfoWorksLib;
using Nini.Config;
using ICM_BinaryFileReader;



namespace SIM_API_LINKS
{
    /// <summary>
    /// ICM Wrapper
    /// Initiated 11/14/18 by MET
    /// </summary>
    public class icm : simlink
    {
        public bool _bIW_IsInitialized = false;
        public string _spBaseNetwork;
        public string _spBaseNetworkShortDisplayName;
        public string _sMasterDatabase;
        // move to simlink for ease of init and supporting other simulators  public int _nSimID;
        public string _sSimSP;
        public string _sRunSP;
        public int _nCommitID;                  // new in ICM wrap
        public DAL.DBContext _dbContextMODEL;       //iw needs to be able to interact with its model as well.
        public bool _bSKIP_IW_Init = false;                //used in specific cases when IW license not neededd
        public bool _bTestForIW_MaxFileSize = true;         //if true, each iteration (whether opt or processeg) will check for file size and compact and repair if needed.
        public cohortSpecIW _cohortSpecIW = new cohortSpecIW(); // cohort spec extension for iw
        private string _sIExchangePath = @"C:\Program Files\Innovyze Workgroup Client 8.5\IExchange.exe";        // effectively a constant- potentially adapt future
        private string _sRubyPath = @"C:\simlink\scripts";
        private string _sICM_Settings = @"C:\simlink\settings";
        private string _sResults_Summary = @"C:\simlink\icm_output\icm_output.bin";
        private string _sResults_TS = @"C:\simlink\icm_output\icm_output_ts.bin";
        private ICM_BinaryFileReader.ICMBinReader _icm_bin_reader_summary;                           // initialize during results read
        private ICM_BinaryFileReader.ICMBinReader _icm_bin_reader_ts;
        #region INIT
        /// <summary>
        /// Read ICM specific data... Throw error if value not found
        /// Call base configuration
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public override bool InitializeByConfig(IConfigSource config)
        {
            try
            {
                _sMasterDatabase = config.Configs["icm"].GetString("model_path");
                _sRunSP = config.Configs["icm"].GetString("run_path");
                _nSimID = config.Configs["icm"].GetInt("sim", -1);
                //   _bSKIP_IW_Init = HelperGetSkipInit(config.Configs["simlink"].GetString("iwSkip", "FALSE"));
                base.InitializeByConfig(config);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error intializing Simlink ICM configuration: {0}", ex.Message));
            }

            return true;
        }

        public override void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            try
            {
                _nActiveModelTypeID = CommonUtilities._nModelTypeID_IW;
                _sTS_FileNameRoot = "IW_TS.h5";
                base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
                InitNavigationDict();

                if (!Directory.Exists(_sRubyPath))
                {
                    throw new Exception(string.Format("Ruby scripts not found at {0}. These are required for ICM execution.", _sRubyPath));
                }


                if (_bIsSimCondor)
                {
                    // do anything you need to do for specific platform
                    _htc._htcPlatformSpecActive = _htc.SyncGetPlatformItem("SWMM5");
                }

                // initialize the IW object.
                if (!_bIW_IsInitialized && !_bSKIP_IW_Init)         //debug testing skip iw init
                {
                    try
                    {
                        //_iw = new InfoWorksLib.InfoWorks();                                               // MET-ICM not sure if there is an ICM platform
                        //_iw.InitForTest(0, "", "");
                        //_bIW_IsInitialized = true;
                        //Console.WriteLine("IW license obtained");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to get IW license. Simlink will not be able to execute any IW API functions");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Unable to load ICM library: {0}", ex.Message), Logging._nLogging_Level_2, false, true);
            }
        }

        /// <summary>
        /// Create ICM navigation dictionary
        /// This uses CS variables for now... but it might need to have its own variable list in the future
        /// 
        /// 1/11/2019
        /// </summary>
        protected override void InitNavigationDict()
        {
            string sSQL = "SELECT tlkpIWTableDictionary.TableName, tlkpIWTableDictionary.KeyColumn, tlkpIWFieldDictionary.FieldName, tlkpIWFieldDictionary.ID AS VarType_FK, tlkpIWTableDictionary.ID AS [TableID]"
                    + " FROM tlkpIWFieldDictionary INNER JOIN tlkpIWTableDictionary ON tlkpIWFieldDictionary.TableName_FK = tlkpIWTableDictionary.ID;";
            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int nVarType_FK = Convert.ToInt32(dr["VarType_FK"].ToString());
                // /ot used - can delete if swmmth works...  
                //   int nVarType_FK = Convert.ToInt32(dr["VarType_FK"].ToString());
                string sKeyFieldName = dr["KeyColumn"].ToString();
                int nTableID = Convert.ToInt32(dr["TableID"].ToString());
                string sFieldName = dr["FieldName"].ToString();
                string sTableName = dr["TableName"].ToString();
                simlinkTableHelper slTH = new simlinkTableHelper(nVarType_FK, sKeyFieldName, nTableID, sFieldName, sTableName);
             //  simlinkTableHelper slTH = new simlinkTableHelper(dr, _dbContext.GetTrueBitByContext(), true);   //nVarType_FK, sKeyFieldName, nTableID, sFieldName, sTableName);     int nVarType_FK, string sKeyFieldName, int nTableID, string sFieldName, string sTableName)
                                                                                                                //new simlinkTableHelper(dr, _dbContext.GetTrueBitByContext());
                _dictSL_TableNavigation.Add(nVarType_FK, slTH);
            }
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="nEvalID"></param>
        public override void InitializeEG(int nEvalID)
        {
            try
            {
                base.InitializeEG(nEvalID);

                nEvalID = GetReferenceEvalID();                                         //get correct EG for loading datasets
                _dsEG_ResultSummary_Request = LoadResultSummaryDS(nEvalID);
                //_dsEG_ResultTS_Request = ReadOut_GetDataSet(nEvalID); //SP 15-Feb-2017 called in parent routine
                _dsEG_ResultSummary_Request.Tables[0].Columns["val"].ReadOnly = false;                  //used to store vals
                base.LoadReference_EG_Datasets();               //  must follow _dsEG_ResultTS_Request
                SetTSDetails();                                 // load simulation/reporting timesereis information
                LoadAndInitDV_TS();                             //load any reference TS information needed for DV and/or tblElementXREF


                //icm update the JSON
                // copy the ICM JSON to the standard location.
                File.Copy(_sActiveModelLocation, @"C:\simlink\settings\settings.json", true);

                //if the baseline is already executed, collect any info (may be needed for scoring, functions etc).

                SetTS_FileName(_nActiveBaselineScenarioID);

                //SP 15-Feb-2017 - Extract External Data at EG level for AUXEG retrieve codes
                EGGetExternalData();

                //pass false, even though 3rd arg actuallly true, because we don't want to kill those datasets.
                //may be mor complicated- issues.... met 5/30/14
                //changed back... ? solves one issue, creates another?
                LoadScenarioDatasets(_nActiveBaselineScenarioID, 100, true);       //Load any datasets for the baseline, if applicable

                // now do a couple things to get the iw ready
                if (!_bSKIP_IW_Init)
                {
                  //icm  _iw.MasterDatabase = _sMasterDatabase;
                  //icm  Believe no init work needed?
              /*      if (_nSimID == -1)
                    {
                        Console.WriteLine("must set simid on eg");
                        //todo: handle the error.
                    }
                    else
                    {
                        _sSimSP = IW_CreateScriptingPath(_nSimID, "Sim", _iw.MasterDatabase);
                    }
                    _sRunSP = _iw.get_Parent(_sSimSP);
                    _spBaseNetwork = _iw.get_Value(_sRunSP, "Network");*/
                }
            }
            catch (Exception ex)
            {
                string sMessage = string.Format("Error initializing EG {0}: {1}", nEvalID, ex.Message);
                throw new Exception(sMessage);
            }
        }


        #endregion



        #region PROCESS SCENARIO

        public override int ProcessScenarioWRAP(string sDNA, int nScenarioID, int nScenStartAct, int nScenEndAct, bool bCreateIntDNA = true)
        {
            if (bCreateIntDNA)
                sDNA = ConvertDNAtoInt(sDNA);       //round any doubles to int
            int nReturn = ProcessScenario(_nActiveProjID, _nActiveEvalID, _nActiveReferenceEvalID, _sActiveModelLocation, nScenarioID, nScenStartAct, nScenEndAct, sDNA);
            return nReturn;
        }

        /// <summary>
        /// 
        /// Modified MET 1/11/19 - make this an override of base simlink function, and sLabel with default option...
        /// </summary>
        /// <returns></returns>
        public override int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINP_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1", string sLabel ="DEFAULT")
        {
            string sPath; string sTargetPath; string sScenarioSettingsFile = ""; string sResultsFile = ""; string sResultsFileTS = ""; string sINIFile; string sSummaryFile; string sOUT;
            int nCurrentLoc = nScenStartAct; string sTS_Filename = ""; string iwTheSim = ""; string sUpdateCSV = ""; string sUpdateJSON = "";
            string spNewNetwork = "NOT_SET";
            string iwNewRun = "";
            int nScenarioID_IWM = GetScenarioForName(nScenarioID);      //  use this everywhere the scen id is used to generate a name.
            bool bResultsJustCreated = false;                           // set to true to avoid copying files back


            ScenDS_ClearAfterScenario(nScenarioID); //SP 18-Jul-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

            if (true)           // met 7/3/14       nScenarioID != -1)     //we should have a valid ScenarioID at this point.
            {
                try
                {
                    if (_bIsOptimization || (nScenarioID == -1))           //nScenarioID  = -1
                    {
                        nScenarioID = InsertScenario(nEvalID, nProjID, System.DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss.fff"), "", sDNA);       //pass the current date time to enable easy retrieval of PK (should be better wya to do this)
                        _nActiveScenarioID = nScenarioID;
                    }

                    //SP 15-Feb-2017 ExtractExternalData for RetrieveCode = AUX
             //icm bojangles       ScenarioGetExternalData();

                    ScenarioPrepareFilenames(nScenarioID, nEvalID, out sTargetPath);                //todo-  add into retrieve filename routine
                    ScenarioRetrieveFilenames(nScenarioID, nEvalID, out sScenarioSettingsFile, out sResultsFile, out sResultsFileTS, out sUpdateCSV, out sUpdateJSON);
                    SetTS_FileName(nScenarioID, sTargetPath);       //_hdf5._sHDF_FileName = sTS_Filename;    //met 1/16/14 - sl object should know it's repository

                    //if ((nScenarioID != _nActiveBaselineScenarioID) && (nScenarioID != -1))        //met 7/3/14: for now, don't load if optimization... todo; consider appropriate loading if seeeding (probably not worth the effort) 
                    //SP 14-Jun-2016 - even if Optimization, ScenarioID would be set here so original comment from 7/3/14 no longer holds anyway
                    LoadScenarioDatasets(nScenarioID, nScenStartAct, nScenarioID == _nActiveBaselineScenarioID);                       //, sTS_Filename);           //loads datasets needed for the scenario if not starting from beginning (in which case ds are constructed through process);

                    Console.WriteLine("Begin process scenario {0}, steps {1}:{2}", nScenarioID, nScenStartAct, nScenEndAct);

                    sPath = System.IO.Path.GetDirectoryName(Path.GetDirectoryName(_sActiveModelLocation));
                    bool bContinue = true;              //identifies whether to continue
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModElementExist) && (nScenEndAct >= CommonUtilities.nScenLCModElementExist) && bContinue)       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
                    {
                        if (sDNA != "-1")
                        {           //not an optimization run, no DNA is passed
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
                        _log.AddString("IW Directory Setup Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        string spRunGroupToUse = "";
                        /*icm 
                         * 
                         * MET 11/15/18: Commmenting out cohort as we get started
                         * 
                         * if (_bIsCohort)
                    << ROUGHLY 30 lines removed relating to cohort... figure out in future
                                             */
                        // step 1: Create the scenario
                        string sScenarioName = string.Format("{0}_{1}", nScenarioID, System.DateTime.Now.ToString("MM.dd.yyyy_H.mm.ss"));
                        UpdateScenarioSettings(sScenarioName, sScenarioSettingsFile);
                        bool bSuccess = CreateScenario();
                        if (!bSuccess)
                        {
                            _log.AddString("Error creating new scenario", Logging._nLogging_Level_3, true, true);
                            return -1;
                        }
                    }

                    //XMODEL: consider adding handler for this?
                    //lots of potential for complexity in this... first cut keep simple
                    if (_slXMODEL != null)
                    {
                        ExecuteLinkedSimLinkPrecursor();        //check and evaluate any linked runs...
                        XMODEL_ProcessLinkedRecords(nScenarioID);                //primary data linkage
                        XMODEL_PlatformSpecificFollowup(nScenarioID);
                        //now, must write out the TS  (do for raingage, ET, LEVEL


                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineModified) && (nScenEndAct >= CommonUtilities.nScenLCBaselineModified))
                    {
                        _log.AddString("IW File Update Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        string sNewNetworkName = nScenarioID.ToString() + "_" + DateTime.Now;
                        sNewNetworkName = CommonUtilities.RMV_FixFilename(sNewNetworkName, ".");         //make sure the filename meets certain standards
                        bool bUpdateNetwork = true;
                        if (_bIsCohort)
                        {
                            if (_bIsLeadEGInCohort)
                            {

                            }
                            else
                            {           // in cohort, but not in lead. run group should already be created.
                                bUpdateNetwork = false;
                            }
                        }

                        if (bUpdateNetwork)     // general case: you are making the new network
                        {
                     //       string sUpdateCSV = Path.Combine(sTargetPath, "update_" + nScenarioID.ToString() + ".csv");
                            if (_bScenUpdateFileUserDefined)
                            {
                                sUpdateCSV = CreateStringFromCode(nScenarioID, out sNewNetworkName, true);             //chance for user to overwrite the provided file
                            }
                            else
                                IW_WriteNetworkCSV(nScenarioID, 2, sUpdateCSV);             //only write the CSV if needed.
                            object[] files = new object[1];
                            files[0] = sUpdateCSV;                  // _sMasterDatabase;                    //iw api appears to requires this
                    //        spNewNetwork = _iw.Import(_spBaseNetwork, sNewNetworkName, "Network", "CSVUP", files);
                     //       _iw.CheckIn(spNewNetwork);
                            if (_bIsCohort)
                                _cohortSpecIW._dictNetwork.Add(_nScenSQN, spNewNetwork);      // add the sp to the dictionary
                        }
                        else
                        {   //special case: use the lead cohort EG versino of the scripting path.
                            spNewNetwork = _cohortSpecIW._dictNetwork[_nScenSQN];          // use the stored scripting path
                        }
                        //icm           _iw.set_Value(iwNewRun, "Network", spNewNetwork);
                        //step 1 write the CSV

                        //               Update_INP(sIncomingINP, nScenarioID, sTarget_INP);

                        // write the location of the update file to .json, so the ruby scripts know what to update

                        WriteUpdateCSV_Settings(sUpdateCSV, nScenarioID, sUpdateJSON);

                        UpdateNetworkScripRun();
                        StoreUpdatedScenarioJSON(sScenarioSettingsFile);

                        nCurrentLoc = CommonUtilities.nScenLCBaselineModified;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        _log.AddString("Model execution begin: " + System.DateTime.Now.ToString(), Logging._nLogging_Level_2);
                        bool bRunSuccess = RunSimulation();
                        bResultsJustCreated = true;

                        // now results are generated, so copy to target location
                        File.Copy(_sResults_Summary, sResultsFile, true);
                        File.Copy(_sResults_TS, sResultsFileTS, true);

                        if (!bRunSuccess)
                        {
                            throw new Exception(string.Format("Error executing simulation- exciting scenario process routine"));
                        }

                        // deleted a bunch of IW CS stuff...
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        _log.AddString("IW Results Read Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                                                                                                   // maybe?        string sPRN_OUT = Path.Combine(sTargetPath, "iw" + nScenarioID + ".prn");
                                                                                                   /*icm                  if (!_bSKIP_IW_Init)
                                                                                                                         _iw.Export(iwTheSim, "PRN", sPRN_OUT);              //if this is skipped, you must already have it somehow.
                                                                                                                     IW_Read_PRN_File(nEvalID, sPRN_OUT, nScenarioID);
                                                                                             */
                        // if not sure results in output dir are from correct run, copy them to the right location
                        if (!bResultsJustCreated)
                            File.Copy(sResultsFile, _sResults_Summary, true);         // copy the data from the Simlink results file to the working version   

                        _icm_bin_reader_summary = new ICMBinReader(_sResults_Summary, false);
                       // _icm_bin_reader_summary.ReadSummary_debug();

                        double[] dVals = _icm_bin_reader_summary.ReadSummaryData(_dsEG_ResultSummary_Request.Tables[0]);
                        PushSummaryResultsToDetail(dVals);

                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                   //     ICMBinReader icm_results = new ICMBinReader(sResultsFile ,false);

                        if (_dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()).Count() > 0)        //5/15/14- skip HDF5 create if no results //SP 15-Feb-2017 Primary only
                        {
                            // untested- ICM ts
                            if(!bResultsJustCreated)
                                File.Copy(sResultsFileTS, _sResults_TS);         // copy the data from the Simlink results file to the working version   
                            _icm_bin_reader_ts = new ICMBinReader(sResultsFileTS, false);

                            if (_tsRepo == TSRepository.HDF5)           //at present, this is the only supported TS repo
                            {
                                //  _hdf5 = new hdf5_wrap();
                                _hdf5.hdfCheckOrCreateH5(_hdf5._sHDF_FileName);
                                _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
                                //iw do this                       ReadOUTData(nReferenceEvalID, nScenarioID, sOUT);
                                _hdf5.hdfClose();
                            }
                        }
                        nCurrentLoc = CommonUtilities.nScenLModelResultsTS_Read;
                    }

                    ProcessScenario_COMMON(nReferenceEvalID, nScenarioID, nCurrentLoc, nScenEndAct);        //call base function to perform modeltype independent actions

                    if (_slXMODEL != null)
                    {

                    }
                    //clear scenario following execute
                    UpdateScenarioStamp(nScenarioID, nCurrentLoc);                 //store the time the scenario is completed, along with the "stage" of the Life Cycle
                    //SP 14-Jun-2016 Collated all routines writing back to the DB to the end of the ProcessScenario routine
                    //SP 10-Jun-2016 TODO Ideally want to shift writing / reading from the database out of this routine - initial step, integrate into one routine at the end of process scenario
                    WriteResultsToDB(nScenarioID);

                    //SP 14-Jun-2016 moved this to after writing back to the DB //18-Jul-2016 moved again!! clear at the start of the routine - _dsSCEN_PerformanceDetails is needed to obtain the objective value for the optimization
                    //ScenDS_ClearAfterScenario(nScenarioID);

                    _log.WriteLogFile();                            //file only written if >0 lines to be written
                    return nCurrentLoc;
                }
                catch (Exception ex)                //log the error
                {
                    //bool bWriteIfFail = true;       //consider adding to config //SP 5-Aug-2016 Added to config 'db_writeonfail'
                    if (_bDBWriteOnFail) //SP 5-Aug-2016 TODO this structure with catching exceptions and writing to DB on fail should be changed for all classes
                    {
                        WriteResultsToDB(nScenarioID);
                    }
                    _log.AddString(string.Format("Process Scenario Error. Last stage {0}.  Error: {1}", nCurrentLoc, ex.Message), Logging._nLogging_Level_1);
                    _bScenarioIsValid = false;
                    _log.WriteLogFile();
                    return 0;   //TODO: refine based upon code succes met 6/12/2012
                }
            }
        }

        /// <summary>
        /// Take the list of dVals from summary report, and store them as details
        /// 
        /// Updated 2/12/18 to call existing function
        /// </summary>
        /// <param name="dVals"></param>
        private void PushSummaryResultsToDetail(double[] dVals)
        {
            try
            {
                for (int i = 0; i < dVals.Length; i++)
                {
                    //(int nScenarioID, int nResultID_FK, int nElementID, string sCurrentElementName, double dVal, int nVarType_FK)

                    ResultSummaryHelper_AddValToDS(_nActiveScenarioID,
                         Convert.ToInt32(_dsEG_ResultSummary_Request.Tables[0].Rows[i]["Result_ID"].ToString()),
                         Convert.ToInt32(_dsEG_ResultSummary_Request.Tables[0].Rows[i]["ElementID_FK"].ToString()),
                         _dsEG_ResultSummary_Request.Tables[0].Rows[i]["Element_Label"].ToString(),
                         dVals[i],
                         Convert.ToInt32(_dsEG_ResultSummary_Request.Tables[0].Rows[i]["VarResultType_FK"].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error storing result summary data: {0}", ex.Message));
            }
        }
        #endregion

        /// <summary>
        /// Retrieve filename for specific type of scenario support file.
        /// 
        /// Updated to include the path the CSV for ICM update - 1/11/2019
        /// </summary>
        // met 4/29/14: removed ts set from this- h
        private void ScenarioRetrieveFilenames(int nScenarioID, int nEvalID, out string sScenarioSettings, out string sResultBin, out string sResultBinTS, out string sUpdateFile, out string sUpdateJSON)
        {
            string sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, nEvalID, true);   //met 4/29/14- was being done manually. confirm and delete prev line
            sScenarioSettings = Path.Combine(sTargetPath, "scenario_settings.json");
            sResultBin = Path.Combine(sTargetPath, "hyd_out.bin");
            sResultBinTS = Path.Combine(sTargetPath, "hyd_out_ts.bin");
            sResultBinTS = Path.Combine(sTargetPath, "hyd_out_ts.bin");
            sUpdateFile = Path.Combine(sTargetPath, string.Format("update_{0}.csv", nScenarioID));
            sUpdateJSON = Path.Combine(sTargetPath, "update_settings.json");
        }

        // functionality to run iexchange 
        // these are set up as individual functions, which call a ruby script and hopefully provide some feedback
        // in the future, it may be possible to combine functions for  fewer IE.exe roundtrips- not sure if that is an issue at this stage
        // returns true if successful, 
        #region IEXCHANGE

        /// <summary>
        /// Ruby scripts will throw certain FAIL conditions in their stdout
        /// Here we check the stdout to see whether the script has been succesful.
        /// </summary>
        /// <param name="sStdout"></param>
        /// <returns></returns>
        private bool CheckForFail(string sStdout)
        {
            int nFail = sStdout.IndexOf("FAIL");
            int nError = sStdout.ToLower().IndexOf("error");        // this is thrown if license is in use
            if (nFail < 0  && nError < 0)
                return true;
            else if (nError>0)
            {
                Console.WriteLine("ICM Exchange license not available");        // not 100% sure- but that is likely the case
                return false;
            }
            else
            {
                if (sStdout.IndexOf("FAIL:IEXCHANGE") >= 0)
                    Console.WriteLine("Unable to get IExchange license. Tell Anjulie to stop hogging it.");
                if (sStdout.IndexOf("FAIL:SETTINGS") >= 0)
                    Console.WriteLine(@"Unable to find one of the necessary settings files- Check that these are defined in c:\simlink\settings.");
                if (sStdout.IndexOf("FAIL:NETWORK") >= 0)
                    Console.WriteLine("Unable to open network. Most likely this results from a) You have a local ICM db open- close when running Simlink  or b) No base network defined in settings.");
                if (sStdout.IndexOf("FAIL:SCENARIO") >= 0)
                    Console.WriteLine("Unable to create new scenario");

                return false;
            }

        }

        private bool CreateScenario()
        {
            try
            {
                string sCmd = @"C:\simlink\scripts\create_scenario.bat";        // call with bat file due to issue getting string to work GetExchangeCommand("create_scenario.rb");
                string sStdout = CommonUtilities.RunProcess(sCmd);
                bool bReturn = CheckForFail(sStdout);
                if (!bReturn)
                    throw new Exception("- Failure creating scenario from Ruby script. Check if ICM Exchange license is available, and that correct IExchange.exe is called in .bat wrapper for Ruby");
                Console.WriteLine(sStdout);
                return bReturn;
            }
            catch (Exception ex)
            {
                string sMessage = string.Format("Exception thrown when executing ICM create_scenario script: {0}", ex.Message);
                throw new Exception(sMessage);
            }
        }

        /// <summary>
        /// Run Javascript run to update network based upon .json in c:\simlink\settings
        /// </summary>
        /// <returns></returns>
        private bool UpdateNetworkScripRun()
        {
            try
            {
                string sCmd = @"C:\simlink\scripts\update_network.bat";        // call with bat file due to issue getting string to work GetExchangeCommand("create_scenario.rb");
                string sStdout = CommonUtilities.RunProcess(sCmd);
                bool bReturn = CheckForFail(sStdout);
                if (!bReturn)
                    throw new Exception("- Check if ICM Exchange license is available");
                Console.WriteLine(sStdout);
                return bReturn;
            }
            catch (Exception ex)
            {
                string sMessage = string.Format("Exception thrown when executing ICM network update script: {0}", ex.Message);
                throw new Exception(sMessage);
            }

        }


        /// <summary>
        /// Call the run scenario
        /// </summary>
        /// <returns></returns>
        private bool RunSimulation()
        {
            try
            {
                string sCmd = @"C:\simlink\scripts\run_sim.bat";        // call with bat file due to issue getting string to work GetExchangeCommand("create_scenario.rb");
                string sStdout = CommonUtilities.RunProcess(sCmd);
                bool bReturn = CheckForFail(sStdout);
                Console.WriteLine(sStdout);
                return bReturn;
            }
            catch (Exception ex)
            {
                string sMessage = string.Format("Exception thrown when executing ICM create_scenario script: {0}", ex.Message);
                throw new Exception(sMessage);
            }
        }

        /// <summary>
        /// Write the scenario.json file 
        /// </summary>
        /// <param name="sScenarioName"></param>
        private void UpdateScenarioSettings(string sScenarioName, string sScenarioSettingsFile)
        {
            try
            {
         //       string sScenarioJSON_Local = Path.Combine(sScenarioSettingsFile, "scenario.json");
                using (StreamWriter writetext = new StreamWriter(sScenarioSettingsFile))
                {
                    writetext.WriteLine("{" + string.Format("\"scenario\":\"{0}\", \"scenario_commit_id\":\"-1\"", sScenarioName) + "}");                     //Format("{\"scenario\":\"{0}\"", sScenarioName));
                }
                // Copy the Scenario version of the scenario settings to local
                File.Copy(sScenarioSettingsFile, @"C:\simlink\settings\scenario.json", true);
            }
            catch (Exception ex)
            {
                string sMessage = string.Format("Exception thrown when executing ICM create_scenario script: {0}", ex.Message);
                throw new Exception(sMessage);
            }
        }

        /// <summary>
        /// Store the location of the CSV to be used for ICM network update
        /// 
        /// 1/11/19
        /// </summary>
        /// <param name="sScenarioName"></param>
        /// <param name="sScenarioSettingsFile"></param>
        private void WriteUpdateCSV_Settings(string sUpdatePath, int nScenarioID,  string sFile)
        {
            try
            {
                //       string sScenarioJSON_Local = Path.Combine(sScenarioSettingsFile, "scenario.json");
                using (StreamWriter writetext = new StreamWriter(sFile))
                {
                    sUpdatePath = sUpdatePath.Replace(@"\", @"\\");
                    writetext.WriteLine("{" + string.Format("\"update_csv\":\"{0}\", \"simlink_scenario_id\":\"{1}\"", sUpdatePath, nScenarioID  ) + "}");                     //Format("{\"scenario\":\"{0}\"", sScenarioName));
                }
                // Copy the Scenario version of the scenario settings to local
                File.Copy(sFile, @"C:\simlink\settings\update.json", true);
            }
            catch (Exception ex)
            {
                string sMessage = string.Format("Exception thrown when executing ICM create_scenario script: {0}", ex.Message);
                throw new Exception(sMessage);
            }
        }


        /// <summary>
        /// After updating the scenario and retrieving the new commit ID, you need to store that .json (in case you want to run that model later
        /// </summary>
        /// <param name="sScenarioSettingsFile"></param>
        private void StoreUpdatedScenarioJSON(string sScenarioSettingsFile)
        {
            string sScenarioJSON_Local = Path.Combine(_sICM_Settings, "scenario.json");
            File.Copy(sScenarioJSON_Local, sScenarioSettingsFile, true);
        }

        #endregion


        //utility function to set the filenames that are needed
        //todo: use the SimLink naming functions in CommnUtilities to make this work beter
        // met 4/29/14: removed ts set from this- h
        private void ScenarioPrepareFilenames(int nScenarioID, int nEvalID, out string sTargetPath)
        {
            sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, nEvalID, true);   //met 4/29/14- was being done manually. confirm and delete prev line           
        }

        /// <summary>
        /// Create an IExchange commande line call of time EXE RB /icm
        /// </summary>
        /// <param name="sRubyFilename"></param>
        /// <returns></returns>
        private string GetExchangeCommand(string sRubyFilename)
        {
            return string.Format(@"""{0}"" {1} /icm", _sIExchangePath, Path.Combine(_sRubyPath, sRubyFilename));
        }

        #region NETWORK UPDates

        #region UpdateCSV

        /// <summary>
        /// version to be created which links to in memory storage...
        /// 
        /// Moved the original versino of the code here..
        /// </summary>/*
        /*public void IW_WriteNetworkCSV_OLD)
        {
            string sql = "SELECT ElementName, Val, ScenarioID_FK, TableName, FieldName "
                        + " FROM (tblModElementVals INNER JOIN tlkpIWFieldDictionary ON tblModElementVals.TableFieldKey_FK = tlkpIWFieldDictionary.ID) INNER JOIN tlkpIWTableDictionary ON tlkpIWFieldDictionary.TableName_FK = tlkpIWTableDictionary.ID"
                        + " WHERE (((ScenarioID_FK)=@Scenario)) ORDER BY ScenarioID_FK, TableName, FieldName;";         //met 4/1v0/16: removed and (IsScenarioSpecific=0) from where clause

        List<DAL.DBContext_Parameter> lstParam = new List<DAL.DBContext_Parameter>();
        DAL.DBContext_Parameter param = new DAL.DBContext_Parameter("@Scenario", SIM_API_LINKS.DAL.DataTypeSL.INTEGER, nScenarioID);
        lstParam.Add(param);

            DataSet dsTDict = _dbContext.getDataSetfromSQL(sql, lstParam);

            try
            {
                //grab the data table, which will indicate how to loop through
                int i = 0; int nWriteHeader = 1; string sCheckPrev = "";
                foreach (DataRow dr in dsTDict.Tables[0].Rows)
                {
                    if (dr["TableName"].ToString() + dr["FieldName"].ToString() != sCheckPrev)
                    {
                        nWriteHeader = 1;
                    }
                    if (nWriteHeader == 1)
                    {
                        WriteCSV_Header(dr["TableName"].ToString(), dr["FieldName"].ToString(), ref file_INP_Out);
    nWriteHeader = 0;
                    }
IW_WriteRow_to_csv(dr["TableName"].ToString(), dr["ElementName"].ToString(), dr["val"].ToString(), ref file_INP_Out);
sCheckPrev = dr["TableName"].ToString() + dr["FieldName"].ToString();     // this is used to see whether we need to repost the header
                }

                // MET 8/12/2011 get rid of oledbstuff in trying to resolve memory leak
                //   ''  drModVals.Close();
                //       drModVals.Dispose();
                //     cmd.Dispose();
            }
        }
        */

        /// <summary>
        /// Write a CSV for updating the network
        /// 
        /// In general, the way this was originally written, many years ago, is BAD because it requires the record to be written to the database prior to running this.
        /// 
        /// Options: 
        ///     1- Rewrite and take out the database dependency
        ///     2- Write the ModelElementVal to the db when it is generated... (MAYBE quicker, probably worse in the long run)
        /// The way this was 
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <param name="nActiveModelType"></param>
        /// <param name="fileBaseIWM"></param>
        public void IW_WriteNetworkCSV(int nScenarioID, int nActiveModelType, string fileBaseIWM)  //int nProjID, int nIWRunDI,
        {
            IEnumerable<simLinkModelChangeHelper> changes = Updates_GetChangeDS(nScenarioID);

            //todo change this to a request like swmm/epanet
            System.IO.StreamWriter file_INP_Out = new System.IO.StreamWriter(fileBaseIWM);
            try
            {
                //grab the data table, which will indicate how to loop through
                int i = 0; int nWriteHeader = 1; string sCheckPrev = "";
                foreach (simLinkModelChangeHelper my_change in changes) // dr in dsTDict.Tables[0].Rows)
                {
                    if (my_change._sTableName + my_change._sFieldName != sCheckPrev)        //dr["TableName"].ToString() + dr["FieldName"].ToString() != sCheckPrev)
                    {
                        nWriteHeader = 1;
                    }
                    if (nWriteHeader == 1)
                    {
                        WriteCSV_Header(my_change._sTableName, my_change._sFieldName, ref file_INP_Out);
                        nWriteHeader = 0;
                    }
                    IW_WriteRow_to_csv(my_change._sTableName, my_change._sElementName, my_change._sVal, ref file_INP_Out);
                    sCheckPrev = my_change._sTableName + my_change._sFieldName;     // this is used to see whether we need to repost the header
                }

                // MET 8/12/2011 get rid of oledbstuff in trying to resolve memory leak
                //   ''  drModVals.Close();
                //       drModVals.Dispose();
                //     cmd.Dispose();
            }
            catch (Exception ex)
            {
                string sMessage = string.Format("Unable to write Infoworks update file: {0}", ex.Message);
                throw new Exception(sMessage);
            }
            finally
            {
                if (file_INP_Out != null)
                    file_INP_Out.Close();
            }
        }

        /// <summary>
        /// copied from swmm 4/10/16
        /// goal is to get this versino working instead of previous version based upon the stored data in simlink. 
        ///     //to enable quick start, for now we use the old approach
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <returns></returns>
        private IEnumerable<simLinkModelChangeHelper> Updates_GetChangeDS(int nScenarioID)
        {
            IEnumerable<simLinkModelChangeHelper> ModelChangesList = from ModelChanges in _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.MEV)
                                                                   .Where(x => x._nScenarioID == nScenarioID).AsEnumerable()               //which performance to characterize
                                                                     join ModelWrapper_DICT in _dictSL_TableNavigation.AsEnumerable()
                                                                     on ModelChanges._nVarType_FK equals
                                                                     ModelWrapper_DICT.Key
                                                                     orderby ModelWrapper_DICT.Value._nSectionNumber, ModelChanges._nElementID
                                                                     //        orderby ModelChanges._nElementID
                                                                     //           orderby ModelChanges._nElementID
                                                                     //        orderby ModelChanges._nRecordID
                                                                     select new simLinkModelChangeHelper

                                                                     {
                                                                         _sVal = ModelChanges._sVal,
                                                                         _sElementName = ModelChanges._sElementName,
                                                                         _nElementID = ModelChanges._nElementID,
                                                                         _nRecordID = ModelChanges._nRecordID,
                                                                         _nSectionNumber = ModelWrapper_DICT.Value._nSectionNumber,
                                                                         _sSectionName = ModelWrapper_DICT.Value._sSectionName,
                                                                         _nTableID = ModelWrapper_DICT.Value._nTableID,
                                                                         _sTableName = ModelWrapper_DICT.Value._sTableName,
                                                                         _sFieldName = ModelWrapper_DICT.Value._sFieldName,
                                                                         _nFieldNumber = ModelWrapper_DICT.Value._nFieldNumber,
                                                                         _nRowNo = ModelWrapper_DICT.Value._nRowNo,
                                                                         _nVarType_FK = ModelWrapper_DICT.Value._nVarType_FK,
                                                                         _bIsScenarioSpecific = ModelWrapper_DICT.Value._bIsScenarioSpecific,
                                                                         _sQualifier1 = "-1",                  //todo : figure out how the qulifier info is obtained (or if there is a better way to do this)
                                                                         _nQual1Pos = -1,
                                                                         _bIsInsert = ModelChanges._bIsInsert
                                                                     }
                              ;
            return ModelChangesList.Cast<simLinkModelChangeHelper>();
        }

        public void WriteCSV_Header(string sTableName, string sAttribute, ref System.IO.StreamWriter filestream)
        {
            filestream.WriteLine("****" + sTableName);
            if (sTableName == "hw_subcatchment" || sTableName == "hw_node")
            {

                if (sTableName == "hw_subcatchment")
                {
                    filestream.WriteLine("subcatchment_id, " + sAttribute);
                }
                else
                {
                    filestream.WriteLine("node_id, " + sAttribute);
                }
            }
            else
            {
                filestream.WriteLine("us_node_id, link_suffix, " + sAttribute);
            }

        }



        public void IW_WriteRow_to_csv(string sTableName, string sElementName, string val, ref System.IO.StreamWriter filestream)
        {
            if (sTableName == "hw_subcatchment" || sTableName == "hw_node")
            {
                filestream.WriteLine(sElementName + ", " + val);
            }
            else
            {
                string sLine = sElementName.Substring(0, sElementName.Length - 2) + ", " + sElementName.Substring(sElementName.Length - 1, 1) + ", " + val;
                filestream.WriteLine(sLine);
            }

        }
        #endregion

        #endregion



        ///
                // data copied from iw.cs
                //

        ///
        #region RESULTS

        /// <summary>
        /// Get results request linked to IW table dictionary
        /// 
        /// //untested
        /// </summary>
        /// <param name="nEvalID"></param>
        /// <returns></returns>
        private DataSet LoadResultSummaryDS(int nEvalID)
        {
            //SP 4-Mar-2016 - Requires Testing after changing from using the Query in accesso 
            //met 4/10/16: modify to remove reference to result table dictionary, which doesn't really exist for iw
            //remove sectionnumber ordering... could cause problem?
            string sql = "SELECT tblResultVar.Result_ID, tblResultVar.Result_Label, tblResultVar.EvaluationGroup_FK, '' as val,  " +
                "tblResultVar.VarResultType_FK, tblResultVar.ElementID_FK, " +
                "tblResultVar.Element_Label, " +
                "tlkpIWResults_FieldDictionary.FieldName, tlkpIWResults_FieldDictionary.FeatureType as TableName," +        // add tablename so works more like other things
                "tlkpIWResults_FieldDictionary.FeatureType, tlkpIWResults_FieldDictionary.ColumnNo, tlkpIWResults_FieldDictionary.AltColumnNo " +
                "FROM (tblResultVar INNER JOIN tlkpIWResults_FieldDictionary ON " +
                "tblResultVar.VarResultType_FK = tlkpIWResults_FieldDictionary.ResultsFieldID) " +
                " WHERE (((EvaluationGroup_FK)=" + nEvalID + ")) ORDER BY ColumnNo, Element_Label;";

            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server
            /*"SELECT Result_ID, Element_Label, Result_Label, VarResultType_FK, FeatureType, FieldName, TableName, SectionNumber, ColumnNo, EvaluationGroupID, ElementID_FK, -1.234 as val FROM qryResultSummary001_SWMM"
                + " WHERE (((EvaluationGroupID)=" + nEvalID + ")) ORDER BY SectionNumber, ColumnNo, Element_Label;";*/

            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        /// <summary>
        /// Read TS request
        /// met: todo- fix the iw table (just copied from swmm 4/8/16)
        /// </summary>
        /// <param name="nEvalId"></param>
        /// <returns></returns>
        //SP 28-Feb-2017 NEEDS TESTING WITH THE ADDITIONAL FIELDS - Modified to keep consistent with Secondary and AUX requests. SQL Server struggles with merge if datasets are not consistent
        private DataSet ReadOut_GetDataSet(int nEvalId)
        {
            string sqlFD = "SELECT ResultTS_ID, Result_Label, ElementIndex, ColumnNo as ResultIndex, FeatureType, VarResultType_FK, Element_Label, -1 as val, BeginPeriodNo, "
                            + "SQN, CustomFunction, FunctionArgs, RefTS_ID_FK, FunctionID_FK, UseQuickParse"
                           + " FROM (tblResultTS INNER JOIN tlkpIWResults_FieldDictionary ON tblResultTS.VarResultType_FK = tlkpIWResults_FieldDictionary.ResultsFieldID) LEFT OUTER JOIN tblFunctions ON tblResultTS.FunctionID_FK = tblFunctions.FunctionID"
                           + " WHERE (((EvaluationGroup_FK)=" + nEvalId + "))";
            /* met 5/31/14: redone to 1) use virtual/override  2) not depend on query    string sqlFD = "SELECT ResultTS_ID, Result_Label, ResultIndex,ElementIndex, FeatureType"
                            + " FROM qryResultTS001_SWMM_OUT WHERE (((EvaluationGroup_FK)=" + nEvalId + "))";  */
            DataSet dsFD = _dbContext.getDataSetfromSQL(sqlFD);
            return dsFD;
        }

        #endregion


    }
}
