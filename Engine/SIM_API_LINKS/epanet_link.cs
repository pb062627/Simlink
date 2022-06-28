    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data;
using SIM_API_LINKS.DAL;
using System.Runtime.InteropServices;
using System.Threading;
using Nini.Config;
using System.Text.RegularExpressions;

namespace SIM_API_LINKS
{
    public class EPANET_link : simlink
    {
        #region MEMBERS

        //SP 18-Feb-2016 Provide options for reading EPANET results - determines other events which are required
        //Private var to determine how to process the results
        private ProcessEPANETResultsMethod _nProcessEPANETResultsMethod;
        private enum ProcessEPANETResultsMethod
        {
            BinaryOUTFile = 0,
            EPANETdll = 1
        }

        private const string _StatusForClosed = "Closed";
        private const string _StatusForOpen = "Open";
        private const string _DefaultPipeDiameterForClosed = "1";
        private const string _EPANETCMDEXE = "epanet2d.exe";
        private const string _RUNEPANETBAT = "run_epanet.bat";
        //private const ProcessEPANETResultsMethod nProcessEPANETResultsMethod = ProcessEPANETResultsMethod.EPANETdll; //currently hardcoded to use EPANET dll but perhaps need a XML config or database reference to change this

        #endregion

        #region DICT INFO       //store SP 8-Mar-2016 - Similar to SWMM
        private const int _nFieldDict_PIPE_DIAMETER = 259;            //used when retrieve TS detail (MEV)
        private const int _nFieldDict_PIPE_STATUS = 262;
        private const int _nFieldDict_TANK_DIAMETER = 251;
        private const int _nFieldDict_TANK_VOLUMECURVE = 253;
        private const int _nFieldDict_PUMP_STATUS = 283;
        private const int _nFieldDict_PATTERN_START = 412;
        private const int _nFieldDict_StartDate = 413; //fictitous variable types
        private const int _nFieldDict_StartTime = 414; //fictitous variable types
        private const int _nFieldDict_EndDate = 415; //fictitous variable types
        private const int _nFieldDict_EndTime = 416; //fictitous variable types
        private const int _nFieldDict_StartClockTime_Time = 417;
        private const int _nFieldDict_StartClockTime_AMPM = 418;

        #endregion

        #region EPANET_OutVars
        private static long offset0; private static long StartPos; private static long EPANET_Nperiods; private static long errCode; private static long magic2; private static long magic1;
        private static long version; private static long EPANET_FlowUnits; private static long EPANET_Nnodes; private static long EPANET_NReservoirsAndTanks; private static long EPANET_Nlinks; private static long EPANET_Npumps;
        private static long EPANET_Nvalves; private static long EPANET_Nwq_option; private static long _nNodeVarCount; private static long _nLinkVarCount;
         private static long EPANET_Nnode_for_tracking; private static long SysVars; private static long EPANET_StartDate; private static long EPANET_ReportStep; private static long BytesPerPeriod;
        private static int nRecordSize = 4; private static BinaryReader b;      //constant  
        #endregion

        #region INIT

        public override void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_EPANET;
            _sTS_FileNameRoot = "EPANET_TS.h5";
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
            InitNavigationDict();
        }

        public override bool InitializeConn_ByFileCon(string sFile, string sDelim = "=")
        {
            bool bValid;
            Dictionary<string, string> dict = InitializeConnDict_ByFileCon(sFile, out bValid, sDelim);
            if (bValid)
                InitializeModelLinkage(dict["conn"], Convert.ToInt32(dict["dbtype"]), _bIsSimCondor);
            return bValid;
        }



        protected override void InitNavigationDict()
        {
            string sSQL = "SELECT tlkpEPANET_TableDictionary.TableName, tlkpEPANET_TableDictionary.KeyColumn, tlkpEPANETFieldDictionary.FieldName, tlkpEPANETFieldDictionary.ID AS VarType_FK, tlkpEPANET_TableDictionary.ID AS [TableID],"
                    + " FieldINP_Number, IsScenarioSpecific, RowNo, SectionNumber, SectionName,  tlkpEPANETFieldDictionary.API_Update, tlkpEPANETFieldDictionary.EPANET_CSharp_LibInt"             // todo: figure out how to get this info- rarely used, Qualifier1,  Qual1Pos"        
                    + " FROM tlkpEPANETFieldDictionary INNER JOIN tlkpEPANET_TableDictionary ON tlkpEPANETFieldDictionary.TableName_FK = tlkpEPANET_TableDictionary.ID;";
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
                simlinkTableHelper slTH = new simlinkTableHelper(dr, _dbContext.GetTrueBitByContext(), true);   //nVarType_FK, sKeyFieldName, nTableID, sFieldName, sTableName);
                                //new simlinkTableHelper(dr, _dbContext.GetTrueBitByContext());
                _dictSL_TableNavigation.Add(nVarType_FK, slTH);
            }
        }

        public override void InitializeEG(int nEvalID)
        {
            base.InitializeEG(nEvalID);

            try
            {
                nEvalID = GetReferenceEvalID();                                         //get correct EG for loading datasets
                _dsEG_ResultSummary_Request = LoadResultSummaryDS(nEvalID);
                //_dsEG_ResultTS_Request = EGDS_GetResultTS(nEvalID); //SP 15-Feb-2017 Called in parent routine

                //testing UI functions SP 16-Feb-2016
                //PerformanceDeleteLinkedRecord(372, 5, 99999);

                //SP 18-Feb-2016 set the results method - by default will use EPANETdll if nothing is passed into InitializeEG
                //Possibly create a reference to database through evaluation group at a later stage
                //if not computing locally then must read results from the OUT file
                if (_compute_env == ComputeEnvironment.LocalMachine)
                    _nProcessEPANETResultsMethod = ProcessEPANETResultsMethod.EPANETdll;
                else
                    _nProcessEPANETResultsMethod = ProcessEPANETResultsMethod.BinaryOUTFile;

                //met 3/21/14: this should now be obsolete....          _dsEG_ResultSummary_Request.Tables[0].Columns["val"].ReadOnly = false;                  //used to store vals
                base.LoadReference_EG_Datasets();               //  must follow _dsEG_ResultTS_Request
                InitTS_Vars();              //met 11/13/16- include direct call for now.
                SetTSDetails();                                 // load simulation/reporting timesereis information
                LoadAndInitDV_TS();                             //load any reference TS information needed for DV and/or tblElementXREF
                SetTS_FileName(_nActiveBaselineScenarioID);

                //SP 15-Feb-2017 moved to after LoadReference_EG_Datasets so ResultTS is read in
                //SP 15-Feb-2016 - determine element Index for each ResultTS and updated _dsEG_ResultTS_Request dataframe - EPANET specific
                DetermineElementIndex(ref _dsEG_ResultTS_Request);

                //SP 18-Nov-2016 - TODO find a better place for this. Moved from LoadReference_EG_Datasets as it's needed after TS is initialised which is called in each derived class
                //EGDS_GetTS_AuxDetails(_nActiveBaselineScenarioID);  // 8/15/14 //SP 15-Feb-2017 AUX details now retrieved when EGDS_GetTS_Details is called

                //SP 15-Feb-2017 - Extract External Data at EG level for AUXEG retrieve codes
                EGGetExternalData();

                LoadScenarioDatasets(_nActiveBaselineScenarioID, 100, true);
            }
            catch (Exception ex)
            {
                _logInitEG.AddString(string.Format("Error initializing EG for EPANET: {0}", ex.Message), Logging._nLogging_Level_1);
            }
            finally
            {
                //SP 3-Oct-2016 testing whether having 3 log files will work - EPANET only for now but can extend. Scenario independent log
                _logInitEG.WriteLogFile();
            }
        }

        //SP 15-Feb-2016 - determine element Index for each ResultTS and updated _dsEG_ResultTS_Request dataframe - EPANET specific
        private void DetermineElementIndex(ref DataSet dsInput_dsEG_ResultTS_Request)
        {
            //open the original inp model  - no need to open the hydraulic solver
            string sTargetPath = _sActiveModelLocation.Substring(0, _sActiveModelLocation.LastIndexOf("\\"));
            string sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(_sActiveModelLocation) + ".RPT";
            ENopen(_sActiveModelLocation, sSummaryFile, "");

            try
            {

                foreach (DataRow dr in dsInput_dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()))
                {
                    string sEPANETFeatureType = dr["FeatureType"].ToString();
                    string sEPANETElement_Label = dr["Element_Label"].ToString();
                    int nCalcElementIndex = -1;
                    int nErrorCode = 0;

                    //call EPANET functions to get the index of each node / link and save back to the dataframe
                    switch (sEPANETFeatureType.ToUpper())
                    {
                        case "NODE":
                            nErrorCode = ENgetnodeindex(sEPANETElement_Label, ref nCalcElementIndex);
                            if (nErrorCode == 0)
                            {
                                dr["ElementIndex"] = nCalcElementIndex.ToString();
                            }

                            break;

                        case "LINK":
                            nErrorCode = ENgetlinkindex(sEPANETElement_Label, ref nCalcElementIndex);
                            if (nErrorCode == 0)
                            {
                                dr["ElementIndex"] = nCalcElementIndex.ToString();
                            }
                            break;
                    }
                }

                ENclose();
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error in determining Element Index. Error: {0}", ex.Message), Logging._nLogging_Level_1);
                throw; //SP 3-Oct-2016 ideally want to catch this error outside this main routine. 
            }
        }


        private void SetTSDetails(DateTime dtStartSim) //SP 18-Apr-2017 - allow passing in a date
        {
            //todo
            //SP 18-Feb-2016 - read and populate time step interval from .inp file
            //open the original inp model  - no need to open the hydraulic solver
            string sTargetPath = _sActiveModelLocation.Substring(0, _sActiveModelLocation.LastIndexOf("\\"));
            string sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(_sActiveModelLocation) + ".RPT";
            ENopen(_sActiveModelLocation, sSummaryFile, "");

            //get the report interval duration that will be referenced by the TimeStamps
            int nReportingTimeStep = 0;
            int nErrorCode = ENgettimeparam(CommonUtilities.EN_REPORTSTEP, ref nReportingTimeStep);
            if (nErrorCode != 0)
                nReportingTimeStep = 1800; //default of 30 min time steps

            int nDurationInSeconds = 0;
            nErrorCode = ENgettimeparam(CommonUtilities.EN_DURATION, ref nDurationInSeconds);
            if (nErrorCode != 0)
                nDurationInSeconds = 86400; //default of 1 day simulation

            DateTime dtRPT = dtStartSim; //SP 18-Apr-2017 - allow passing in a date

            _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtRPT, IntervalType.Second, nReportingTimeStep, dtRPT.AddSeconds(nDurationInSeconds));
                
            //SP 10-Aug-2016 needed for dssUtil
            _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtRPT, IntervalType.Second, nReportingTimeStep, dtRPT.AddSeconds(nDurationInSeconds));

            ENclose();
        }

        
        private void SetTSDetails() //SP 18-Apr-2017 - allow passing in a date
        {
            SetTSDetails(DateTime.Parse("1/1/1900"));
        }

        //SP 27-Dec-2016 Copied from SWMM
        public override void SetSimTimeSeries(bool bCreateModelChanges, DateTime dtSimStart, DateTime dtSimEnd, int nTS_Interval_Sec, DateTime dtRptStart = default(DateTime), DateTime dtRptEnd = default(DateTime), int nInterval_Sec_rpt = -1)
        {
            base.SetSimTimeSeries(bCreateModelChanges, dtSimStart, dtSimEnd, nTS_Interval_Sec, dtRptStart, dtRptEnd, nInterval_Sec_rpt);
            if (bCreateModelChanges)
            {
                // manually push the pattern start time
                InsertModelValList(-1, _nFieldDict_StartDate, _nActiveScenarioID, _tsdSimDetails._dtStartTimestamp.ToString("MM/dd/yyyy"), "", "START_DATE", -1, -1);
                InsertModelValList(-1, _nFieldDict_StartTime, _nActiveScenarioID, _tsdSimDetails._dtStartTimestamp.ToString("HH:mm:ss"), "", "START_TIME", -1, -1);
                InsertModelValList(-1, _nFieldDict_EndDate, _nActiveScenarioID, _tsdSimDetails._dtEndTimestamp.ToString("MM/dd/yyyy"), "", "END_DATE", -1, -1);
                InsertModelValList(-1, _nFieldDict_EndTime, _nActiveScenarioID, _tsdSimDetails._dtEndTimestamp.ToString("HH:mm:ss"), "", "END_TIME", -1, -1);

                InsertModelValList(-1, _nFieldDict_PATTERN_START, _nActiveScenarioID, dtSimStart.ToString("HH:mm"), "", "Pattern Start", -1, -1);
                InsertModelValList(-1, _nFieldDict_StartClockTime_Time, _nActiveScenarioID, dtSimStart.ToString("hh"), "", "Start ClockTime", -1, -1);
                InsertModelValList(-1, _nFieldDict_StartClockTime_AMPM, _nActiveScenarioID, dtSimStart.ToString("tt"), "", "Start ClockTime", -1, -1);
                WriteResultsToDB(_nActiveScenarioID);
            }
        }

        //SP 28-Dec-2016 Modify TimeSeriesData for Demo purposes to simulate RealTime data - PROBABLY JUST TEMPORARY - ONLY MODIFIED in Element_LABEL = "SCADA"
        public override void PreProcessTimeseriesData(TimeSeries.TimeSeriesDetail dtFirstSim, DateTime dtSimStart)
        {
            //truncate the time series data to remove rows. Calculate the Period Number
            int nPeriodNumber = dtFirstSim.GetTSPeriodNumberFromDateTime(dtSimStart) - 1;
            if (nPeriodNumber < 0)
                nPeriodNumber = 0;

            foreach (ExternalData ex in _lstExternalDataDestinations.Where(x => x._bIsInput).ToList())
            {
                //check the resultTS - only modify if ElementID = SCADA
                if (_dsEG_ResultTS_Request.Tables[0].Select("ResultTS_ID =" + (ex._nTSRecordID).ToString()).First()["Element_Label"].ToString().ToUpper() == "SCADA")
                {

                    int nIndex = _dictResultTS_Indices[ex._nTSRecordID];
                    //only if the current TS dataset is large
                    if (nPeriodNumber < _dResultTS_Vals[nIndex].Length)
                    {

                        //modify the TS Vals - clear out everything in future
                        double[,] tmpResultTS = new double[1, 1];
                        tmpResultTS[0, 0] = _dResultTS_Vals[nIndex][nPeriodNumber, 0];

                        //save back to ResultsTS
                        _dResultTS_Vals[nIndex] = tmpResultTS;
                    }
                }
            }
        }
        
        private DataSet LoadResultSummaryDS(int nEvalID)
        {
            //rm feature type
            // not set up for reading from INP becaues this is not helpful for htis model type
            string sql = "SELECT Result_ID, Element_Label, Result_Label, VarResultType_FK,  FieldName, 'NA' as TableName, -1 as SectionNumber, ColumnNo, EvaluationGroup_FK, ElementID_FK, -1.234 as val"
                        +" FROM (tblResultVar INNER JOIN tlkpEPANET_ResultsFieldDictionary ON tblResultVar.VarResultType_FK = tlkpEPANET_ResultsFieldDictionary.ResultsFieldID)"
            + " WHERE (((EvaluationGroup_FK)=" + nEvalID + ")) ORDER BY ColumnNo, Element_Label;";
            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }
 
        public override double[,] GetNetworkTS_Data(int nElementID, int nVarType_FK, string sElementLabel = "NOTHING", string sFileLocation = "NOTHING")
        {
            //todo

            return null;
        }

        #endregion
        #region RunProcessing

        //ProcessEvaluationGroup(int nEvalID, int nModelTypeID, int nRefScenarioID = -1, bool bSkipDictionaryPopulate = false)
        //copied from SWMM met 3/20/14
        
        //SP 5-Aug-2016 use the standard virtual function in SimlinkScenario - TODO can probably remove this routine for all other link classes
        /*public override void ProcessEvaluationGroup(string[] astrScenario2Run)
        {
            DataSet dsEvals = ProcessEG_GetGS_Initialize(_nActiveEvalID, astrScenario2Run);       //, nRefScenarioID);
            //now performed in init... LoadReferenceDatasets();            //initialize datasets

            //SP 25-Jul-2016 now can be read from config
            //string sLogPath = System.IO.Path.GetDirectoryName(_sActiveModelLocation);
            //sLogPath = sLogPath.Substring(0, sLogPath.LastIndexOf("\\")) + "\\LOGS";

            foreach (DataRow dr in dsEvals.Tables[0].Rows)
            {
                _log.Initialize("logEG_" + _nActiveEvalID.ToString() + "_" + dr["ScenarioID"].ToString(), _log._nLogging_ActiveLogLevel, _sLogLocation_Simlink);   //begin a logging session
                int nProjID = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["ProjID"].ToString());
                string sFileLocation = dsEvals.Tables[0].Rows[0]["ModelFile_Location"].ToString();         //todo: proj/sFile should only be set once; minor time penalty so no issue
                int nScenarioID = Convert.ToInt32(dr["ScenarioID"].ToString());
                int nScenStart = Convert.ToInt32(dr["ScenStart"].ToString());
                int nScenEnd = Convert.ToInt32(dr["ScenEnd"].ToString());
                string sDNA = dr["DNA"].ToString();

                try
                {
                    //delete scenario data from DB
                    if (true) //SP TODO 5-Aug-2016 to be read from config    
                        DeleteScenariosWrapper(false, _nActiveEvalID, nScenStart, nScenEnd, nScenarioID.ToString());

                    ProcessScenario(nProjID, _nActiveEvalID, _nActiveReferenceEvalID, sFileLocation, nScenarioID, nScenStart, nScenEnd, sDNA);
                }
                catch (Exception ex)               
                {
                    //todo: log the issue
                }

            }
            
            
     //       base.ProcessEvaluationGroup();



        }*/

        public override int ProcessScenarioWRAP(string sDNA, int nScenarioID, int nScenStartAct, int nScenEndAct, bool bCreateIntDNA = true)
        {
            if (bCreateIntDNA)
                sDNA = ConvertDNAtoInt(sDNA);       //round any doubles to int

            int nReturn = ProcessScenario(_nActiveProjID, _nActiveEvalID, _nActiveReferenceEvalID, _sActiveModelLocation, nScenarioID, nScenStartAct, nScenEndAct, sDNA);
            return nReturn;
        }

        public override int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINP_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1", string sLabel = "DEFAULT")
        {
            string sPath=""; string sTargetPath=""; string sTarget_INP=""; string sIncomingINP=""; string sTarget_INP_FileName; string sINIFile; string sSummaryFile=""; string sOUT="";
            int nCurrentLoc = nScenStartAct; string sTS_Filename = "";

            if (_scenDeleteSpec == DeleteScenDetails.BeforeRun)
                ScenDS_ClearAfterScenario(nScenarioID); //SP 18-Jul-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

            if (true)     //we should have a valid ScenarioID at this point.
            {
                try
                {
                    if (_bIsOptimization || (nScenarioID == -1))           //nScenarioID  = -1
                    {
                        //SP 10-Jun-2016 TODO Ideally want to shift writing / reading from the database out of this routine - increased precision of datetime as there are not more than one able to be processed per second
                        sLabel = GetScenarioLabel(sLabel);
                        nScenarioID = InsertScenario(nEvalID, nProjID, sLabel, "", sDNA);
                        _nActiveScenarioID = nScenarioID;

                        //SP 27-Jul-2016 need to initiate an instance of log file for Simlink - only now is the scenario ID known
                        _log.Initialize("logEG_" + _nActiveEvalID.ToString() + "_" + _nActiveScenarioID, _log._nLogging_ActiveLogLevel, _sLogLocation_Simlink);   //begin a logging session

                    }

                    //SP 15-Feb-2017 ExtractExternalData for RetrieveCode = AUX
                    // MET the following retrieval of external data and loading of datasets only occurs if we are not doing a "pure insert"
                    // added 3/8/17. May require better control if there are cases where you DO want to get external data once the scenario is inserted.
                    // this also avoids duplicating this call in the case where we have two successive calls to process_scenario (from realtime)
                    if (nScenEndAct * nScenStartAct != 1)
                    {
                        // only get external data if you need to run the model
                        // todo: better control of this?
                        if (nScenStartAct <= CommonUtilities.nScenLCBaselineFileSetup)
                        {
                            ScenarioGetExternalData();
                            PreProcessTimeseriesData(_tsdSimDetails, _tsdSimDetails._dtStartTimestamp); //SP 21-Apr-2017 Temporary for real time SCADA processing  -output reasons
                        }
                        ScenarioPrepareFilenames(nScenarioID, nEvalID, sINP_File, out sTargetPath, out sIncomingINP, out sTarget_INP, out sSummaryFile, out sOUT, out sINIFile, out sTS_Filename);

                        //if ((nScenarioID != _nActiveBaselineScenarioID) && (nScenarioID != -1))        //met 7/3/14: for now, don't load if optimization... todo; consider appropriate loading if seeeding (probably not worth the effort) 
                        //SP 14-Jun-2016 - even if Optimization, ScenarioID would be set here so original comment from 7/3/14 no longer holds anyway
                        LoadScenarioDatasets(nScenarioID, nScenStartAct, nScenarioID == _nActiveBaselineScenarioID);                       //, sTS_Filename);           //loads datasets needed for the scenario if not starting from beginning (in which case ds are constructed through process);

                        sPath = System.IO.Path.GetDirectoryName(sINP_File);
                    }


                    bool bContinue = true;              //identifies whether to continue
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModElementExist) && (nScenEndAct >= CommonUtilities.nScenLCModElementExist))       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
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

                        //SP 15-Jun-2016 no longer needed
                        /*if (_dbContext._DBContext_DBTYPE==0)      //if access
                        {
                            _dbContext.OpenCloseDBConnection(); 
                        }*/
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineFileSetup) && (nScenEndAct >= CommonUtilities.nScenLCBaselineFileSetup))
                    {
                        _log.AddString("EPANET File Setup Begin: ", Logging._nLogging_Level_1);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        CommonUtilities.CopyDirectoryAndSubfolders(sPath, sTargetPath, true);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;          //

                        // COPIED FROM SWMM FOR NOW - process data for input into model file
                        if (nScenStartAct <= CommonUtilities.nScenLCModElementExist)
                        {
                            ProcessNonDV_FileDependency(sTargetPath, nScenarioID);
                            _log.UpdateFileOutName("logEG_" + nEvalID.ToString() + "_" + nScenarioID.ToString());
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
                        _log.AddString("EPANET File Update Begin: ", Logging._nLogging_Level_1);      //log begin scenario step
                        Update_INP(sIncomingINP, nScenarioID, sTarget_INP);
                        System.IO.File.Delete(sIncomingINP);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineModified;

                        /* met todo: verify if INI stuff for EPANET
                            * string sTargetINI = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sINIFile) + "_" + nScenarioID.ToString() + ".ini";
                                    if (File.Exists(sINIFile) && !File.Exists(sTargetINI))
                                    {      //there may not be one in the root file, but update if there is.
                                        File.Move(sINIFile, sTargetINI);
                                    }

                            * */
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        bool bIsUNC = false; string sEPANET_EXE = ""; string sBAT = "";
                        if (sTarget_INP.Substring(0, 2) == @"\\") { bIsUNC = true; }         //explicit paths required if UNC
                        if (bIsUNC)
                        {
                            sEPANET_EXE = _EPANETCMDEXE + " " + sTarget_INP + " " + sSummaryFile + " " + sOUT;
                            sBAT = System.IO.Path.GetDirectoryName(sTarget_INP) + "\\" + _RUNEPANETBAT;
                        }
                        else
                        {
                            sEPANET_EXE = _EPANETCMDEXE + " " + System.IO.Path.GetFileName(sTarget_INP) + " " + System.IO.Path.GetFileName(sSummaryFile) + " " + System.IO.Path.GetFileName(sOUT);
                            sBAT = _RUNEPANETBAT;
                        }

                        //create batch file information for running the program
                        string[] s = new string[] { sEPANET_EXE };
                        string sBat = System.IO.Path.Combine(sTargetPath, _RUNEPANETBAT);

                        //create the string for the .bat
                        s[0] = "cd %~dp0 \r\n" + s[0];
                        File.WriteAllLines(sBat, s);

                        //SP 8-Aug-16 Copied from SWMM
                        if (_compute_env == ComputeEnvironment.LocalMachine)
                        {
                            //SP 18-Feb-2016 No need to the run the batch file if extracting results using the EPANET dll
                            if (_nProcessEPANETResultsMethod != ProcessEPANETResultsMethod.EPANETdll)
                                CommonUtilities.RunBatchFile(sBat);
                        }
                        else
                        {
                            /*SP 10-Aug-2016 NOTE CURRENTLY WITH CONDOR THE BASE MODEL FILE LOCATION MUST BE LOCAL OR THE NETWORK MUST BE MAPPED LOCALLY TO AVOID PATH STARTING WITH \\*/

                            //three steps then to execute
                            //05: set sim/wrapper/platform specific requirements
                            //todo: need way to say to transfer ALL (just set to 'all') - where to define this? environment?  depdnent files may be needed..?
                            //jk 26-Aug-2016 not passing paths to EXE files around anymore...
                            //string sEPANETCMDEXEFullPath = GetFullPath(_EPANETCMDEXE); //SP 10-Aug-2016 Needs to have full file path for Condor

                            if (_hpc._EnvConfig.GetString("transfer_input_files", "").ToLower() == "standard")
                                _dictHPC["transfer_input_files"] = sBat + "," + sTarget_INP; //SP 10-Aug-2016 Needs to have full file path for Condor
                            else
                                _dictHPC["transfer_input_files"] = _hpc._EnvConfig.GetString("", "").ToLower();

                            //SP 10-Aug-2016 files required for output
                            if (_hpc._EnvConfig.GetString("transfer_output_files", "").ToLower() == "standard")
                                _dictHPC["transfer_output_files"] = System.IO.Path.GetFileName(sOUT) + "," + System.IO.Path.GetFileName(sSummaryFile); //SP 10-Aug-2016 Needs to have file path removed for Condor
                            else
                                _dictHPC["transfer_output_files"] = _hpc._EnvConfig.GetString("", "").ToLower();

                            // 1: Create the job file
                            XmlConfigSource xmlJobSpec = CreateHPC_JobSpec("epanet_" + nScenarioID, nScenarioID, _RUNEPANETBAT, sTargetPath); //SP 10-Aug-2016 changed the executable to run_EPANET.bat instead of specifying the EPANET2d.exe
                            // 2: Perform any sim/wrapper/platform specific requirements

                            // 3: Submit the job           
                            _hpc.SubmitJob(xmlJobSpec.Configs["Job"]);

                            //now, tell simlink what you've done....    
                            // 100: 
                            UpdateScenarioStamp(nScenarioID, CommonUtilities.nScenLCModelExecuted);

                            // 200:
                            return CommonUtilities.nScenLCModelResultsRead;
                        }


                        /*if (_bIsSimCondor) //run the SWMM job as a Condor job.
                        {               //note: 
                            _htc = new CIRRUS_HTC_NS.CIRRUS_HTC();  //cirrus htc2.0 no more mod-specific   _htc = new CIRRUS_HTC_NS.HTC_SWMM();
                            _htc.PreProcessCondorJob(Directory.GetFiles(sTargetPath), null);          //grab all files in the TargetPath location
                            _htc.SubmitCondorJob();
                        }
                        else
                        {   //run within SimLink



                            //sim2_note: significant code removed here from previous exercises
                        }
                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted;*/
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        _log.AddString("EPANET Results Read Begin: ", Logging._nLogging_Level_1);      //log begin scenario step
                        //       ResultsReadReport(sSummaryFile, nReferenceEvalID, nScenarioID);
                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                        switch (_nProcessEPANETResultsMethod)
                        {
                            case ProcessEPANETResultsMethod.BinaryOUTFile:
                                _log.AddString("Reading results from binary outfile", Logging._nLogging_Level_1, false);
                                ReadOUTData(nReferenceEvalID, nScenarioID, sOUT);   //read data into memory
                                //WriteOutputToCSVForTesting(_dResultTS_Vals, System.IO.Path.GetDirectoryName(sTarget_INP), "ResultsFromOUTFile.csv",
                                //    SWMM_Nperiods);
                                break;

                            case ProcessEPANETResultsMethod.EPANETdll: //SP 16-Feb-2016 - read from the EPANET dll
                                _log.AddString("Reading results using EPANETdll", Logging._nLogging_Level_1, false);
                                long nNumberReportingPeriods = 0;
                                double[][,] _dResultTS_Vals_EPANETdll = ReadOUTDataUsingEPANETdll(sTarget_INP, sSummaryFile, sOUT, ref nNumberReportingPeriods);
                                //WriteOutputToCSVForTesting(_dResultTS_Vals_EPANETdll, System.IO.Path.GetDirectoryName(sTarget_INP),
                                //    "ResultsFromEPANETdll.csv", nNumberReportingPeriods);

                                //SP 16-Feb-2016 - replace the global array _dResultTS_Vals with the EPANETdll results (15-Feb-2017 - for primary variables only)
                                foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()))
                                {
                                    int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())];
                                    _dResultTS_Vals[nIndex] = _dResultTS_Vals_EPANETdll[nIndex];
                                }

                                break;

                            default:
                                _log.AddString("Processing EPANET results method unknown", Logging._nLogging_Level_1);
                                break;
                        }

                        _log.AddString("Finished reading results", Logging._nLogging_Level_1);

                        //1/5/15: updated code to read TS above, and then write below if requested
                        if ((_tsRepo == TSRepository.HDF5) && (_IntermediateStorageSpecification._bResultTS))           //at present, this is the only supported TS repo
                        {
                            try
                            {
                                _log.AddString("Creating HDF5 timeseries data", Logging._nLogging_Level_1);
                                //split into batches for writing to HDF5
                                _hdf5 = new hdf5_wrap();
                                _hdf5.hdfCheckOrCreateH5(sTS_Filename);
                                _hdf5.hdfOpen(sTS_Filename, false, true);
                                WriteTimeSeriesToRepo(); //SP 15-Feb-2017 Changed to use the generic Simlink function WriteTimeSeriesToRepo(_hdf5, sTS_Filename);
                                _hdf5.hdfClose();
                                _log.AddString("Finished HDF5 timeseries data", Logging._nLogging_Level_1);
                            }
                            catch (Exception ex)
                            {
                                _log.AddString(string.Format("Error writing timeseries to HDF5. Error message returned: {0}", ex.Message), Logging._nLogging_Level_1);
                                throw;
                            }
            }


                nCurrentLoc = CommonUtilities.nScenLModelResultsTS_Read;
                    }

                    _log.AddString("Starting processing results, events and performance metrics", Logging._nLogging_Level_1);
                    ProcessScenario_COMMON(nReferenceEvalID, nScenarioID, nCurrentLoc, nScenEndAct, sTS_Filename);        //call base function to perform modeltype independent actions
                    if (_slXMODEL != null)
                    {

                    }

                    _log.AddString("Updating scenario", Logging._nLogging_Level_1);
                    
                    // SP - Copied from SWMM If we are only inserting scenario, don't update (we don't have a scen datail
                    if (nScenEndAct * nScenStartAct != 1)
                    {
                        UpdateScenarioStamp(nScenarioID, nCurrentLoc);  //store the time the scenario is completed, along with the "stage" of the Life Cycle //SP 10-Jun-2016 now only changes in memory
                                                                        //SP 14-Jun-2016 Collated all routines writing back to the DB to the end of the ProcessScenario routine
                                                                        //SP 10-Jun-2016 TODO Ideally want to shift writing / reading from the database out of this routine - initial step, integrate into one routine at the end of process scenario

                        _log.AddString("Writing results to DB", Logging._nLogging_Level_1);
                        WriteResultsToDB(nScenarioID);
                    }

                    //SP 14-Jun-2016 moved this to after writing back to the DB //18-Jul-2016 moved again!! clear at the start of the routine - _dsSCEN_PerformanceDetails is needed to obtain the objective value for the optimization
                    //ScenDS_ClearAfterScenario(nScenarioID);

                    //_log.WriteLogFile();   //SP 3-Oct-2016 Now performed outside in inherited scenario loop                         //file only written if >0 lines to be written
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
                    //_log.WriteLogFile(); //SP 3-Oct-2016 Now performed outside in inherited scenario loop
                    return 0;   //TODO: refine based upon code succes met 6/12/2012
                }
            }
        }

        
        //utility function to set the filenames that are needed
        //todo: use the SimLink naming functions in CommnUtilities to make this work beter
        private void ScenarioPrepareFilenames(int nScenarioID, int nEvalID, string sINP_File, out string sTargetPath, out string sIncomingINP, out string sTarget_INP, out string sSummaryFile, out string sOUT, out string sINIFile, out string sTS_Filename)
        {
            string sPath = System.IO.Path.GetDirectoryName(sINP_File);
            sTargetPath = sPath.Substring(0, sPath.LastIndexOf("\\")) + "\\" + nEvalID.ToString() + "\\" + nScenarioID.ToString();
            string sTarget_INP_FileName = System.IO.Path.GetFileNameWithoutExtension(sINP_File) + "_" + nScenarioID.ToString() + System.IO.Path.GetExtension(sINP_File);       //append scenario name (good for gathering up the files into a single folder if needed)
            sIncomingINP = System.IO.Path.Combine(sTargetPath, System.IO.Path.GetFileName(sINP_File));
            sTarget_INP = System.IO.Path.Combine(sTargetPath, sTarget_INP_FileName);
            sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP_FileName) + ".RPT";
            sOUT = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP_FileName) + ".OUT";
            sINIFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sINP_File) + ".ini";
            sTS_Filename = sTargetPath + "\\" + CommonUtilities.GetSimLinkFileName("EPANET_TS.H5", nScenarioID);                  //todo: handle for different TS repository?
        }


        #endregion

        #region MODIFY

        //met 3/19/14: synthesis of swmm update_inp and epanet from Collin
        //drop the API stuff in round 1; 
        public string Update_INP(string sINP_File, int nScenarioID, string sOptionalOutput_TextFile = "nothing", bool bCleanTargetOfRepeating = true)
        {
            Thread tThread = Thread.CurrentThread;

            _log.AddString("begin EPANET_Update_INP", Logging._nLogging_Level_2);
            if (File.Exists(sINP_File))
            {
                Debug.Print("INP Exists");
                StreamReader fileMEX = null;
                try
                {
                    string[] sTextFile_ALL = File.ReadAllLines(sINP_File);

                    //string sql = "SELECT ElementName, Val, ScenarioID_FK, TableName, FieldName, SectionNumber, SectionName, FieldINP_Number, TableClass, ElementID, KeyColumn FROM qryRMG001_SWMM_ModelChanges WHERE (((ScenarioID_FK)=@Scenario)) ORDER BY SectionNumber, ElementName;";         //BYM2012
                    // DataSet dsRMG_Changes = null;

                    IEnumerable<simLinkModelChangeHelper> ModelChangesIEnum = Updates_GetChangeDS(nScenarioID);       //bojangles...  needs to be from mem!!!
                    int nTotalChanges = ModelChangesIEnum.Count();
                    //met 1/2/2013: placed inside a double loop
                    //round 1: inserts
                    //round 2: updates
                    //while this can probably be improved, looping over the vars doesn't take long, and I believe this will cause less looping within a given section
                    //todo: optimize loop
                    //tood: include better sec


                    int nListOffset = 0;                   //used to keep track of the insert position. a value >=0 indicates that there was at LEAST one insert
                    //
                    List<string> listTextFile_ALL = new List<string>();

                    for (int i = 0; i <= 1; i++)
                    {         //loop through twice

                        //initialize variables for each loop
                        bool bIsInsert = false; bool bInCurrentSection = false;
                        if (i == 0) { bIsInsert = true; } else { bIsInsert = false; }       //first loop is insert
                        string sCurrentSectionName = "none";
                        int nCurrentWriteLine = 0;
                        int nFileTotalRows; int nCurrentChange = 0;
                        int nSectionLine = 0; //BYM2012
                        //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 1");

                        _log.AddString("Total Changes: " + nTotalChanges, Logging._nLogging_Level_2, false);

                        if ((!bIsInsert) && (nListOffset > 0))             //update loop, and at lest one insert; need to convert list back
                        {
                            sTextFile_ALL = listTextFile_ALL.ToArray();         //
                        }
                        nFileTotalRows = sTextFile_ALL.Length;

                        //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 1.1");

                        simLinkModelChangeHelper slmCurrent = new simLinkModelChangeHelper(); //SP 7-June-2016 I think there is something in this line which is causing a conflict with multithreading
                        if (nTotalChanges > 0)
                            slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);

                        //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 1.2");

                        while (nCurrentChange < nTotalChanges)
                        //foreach (simLinkModelChangeHelper slmCurrent in ModelChangesIEnum)
                        {
                            _log.AddString("Begin change to element: " + slmCurrent._sElementName + " fieldname = " + slmCurrent._sFieldName, Logging._nLogging_Level_3, false);
                            //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 2");

                            //bojangles 1/20/14: need to set whether insert on slmCurrrent; not set through query

                            //SP 28-Sep-2016 check whether this current change is a Rule or Control - if so, split out separate lines and implement separately
                            if (slmCurrent._nVarType_FK == _nEPANET_FieldDict_CONTROLS || slmCurrent._nVarType_FK == _nEPANET_FieldDict_RULES)
                            {

                                string[] RulesSubLines_Element = slmCurrent._sElementName.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] RulesSubLines_Val = slmCurrent._sVal.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                                for (int iLine = 0; iLine < RulesSubLines_Element.Count(); iLine++)
                                {
                                    simLinkModelChangeHelper sub_slmCurrent = slmCurrent;
                                    sub_slmCurrent._sVal = RulesSubLines_Val[iLine];
                                    sub_slmCurrent._sElementName = RulesSubLines_Element[iLine];

                                    //now use the sub_slmCurrent to make adjustments

                                    //met 1/2/2013: check that we do ONLY inserts on inserts loop and ONLY updates on update loop
                                    if (bIsInsert == sub_slmCurrent._bIsInsert)
                                    {
                                        if (sub_slmCurrent._nVarType_FK == -1)        //perform insert (don't go through all the "section name" stuff
                                        {
                                            _log.AddString("Table FieldID = -1; is there an issue?", Logging._nLogging_Level_3);

                                        }
                                        else                  //general case. standard update being performed
                                        {
                                            //SP 27-Sept-2016 Store the current line in the text file
                                            int nStartingLine = nCurrentWriteLine;

                                            sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                            //BYM   
                                            while ((sCurrentSectionName != sub_slmCurrent._sSectionName) && (nCurrentWriteLine < nFileTotalRows))
                                            {                                            
                                                sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                        //BYM
                                                nCurrentWriteLine++;
                                                nSectionLine = nCurrentWriteLine;
                                                bInCurrentSection = false;
                                            }

                                            //SP 27-Sep-2016 to avoid the user knowing the order of the DVs, loop through from the start again until reach the required section
                                            if (sCurrentSectionName != sub_slmCurrent._sSectionName)
                                            {
                                                nCurrentWriteLine = 0;
                                                while ((sCurrentSectionName != sub_slmCurrent._sSectionName) && (nCurrentWriteLine < nStartingLine))
                                                {
                                                    sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                        //BYM
                                                    nCurrentWriteLine++;
                                                    nSectionLine = nCurrentWriteLine;
                                                    bInCurrentSection = false;
                                                }
                                            }

                                            if (sCurrentSectionName == sub_slmCurrent._sSectionName)
                                            {
                                                bInCurrentSection = true;

                                                if (sTextFile_ALL[nCurrentWriteLine].IndexOf(" ") > 0)                   //check whether we have data row
                                                {

                                                    bool bIsScenarioLevelVar = false; string sFindElementNameOrField;
                                                    sFindElementNameOrField = UpdateHelper_GetElementNameOrField(sub_slmCurrent, ref bIsScenarioLevelVar); //this will return the RULE / CONTROL without semicolon
                                                    string sTrimmedElementName = Regex.Replace(sFindElementNameOrField, @"\s+", string.Empty, RegexOptions.Multiline);

                                                    string sTimmedIDName = UpdateHelper_AdvanceToCurrent_ID(ref sTextFile_ALL, sTrimmedElementName, ref nCurrentWriteLine, nSectionLine, sCurrentSectionName); //this will return the RULE / CONTROL without semicolon and removed spaces

                                                    _log.AddString("Found CurrentID: " + nCurrentChange, Logging._nLogging_Level_3, false);
                                                    if (sTimmedIDName != "No_ID_Found")
                                                    {
                                                        //SP 28-Sep-2016 if writing controls or rules, don't split the row into an array
                                                        string[] sbufDATA = new string[1];
                                                        sbufDATA[0] = sTextFile_ALL[nCurrentWriteLine];

                                                        int nLastRow = 1;                                                                      //met 6/18/2012: modified to support multiple row values. Val of 1 is the default.
                                                        int nCurrentRow = 1;

                                                        //met 1/4/2013: modified to check thta the next ID has the right type.


                                                        //activate / deactivate the row if it is the correct line
                                                        if ((sTimmedIDName == sTrimmedElementName) && (bIsInsert == Convert.ToBoolean(sub_slmCurrent._bIsInsert)))
                                                        {
                                                            bool bCorrectRow = true;

                                                            if (bCorrectRow)
                                                            {

                                                                nCurrentRow = sub_slmCurrent._nRowNo;
                                                                if (nCurrentRow > nLastRow)                                                         // if this is true, we need to get the new row.
                                                                {
                                                                    nCurrentWriteLine += nCurrentRow - nLastRow;

                                                                    //SP 28-Sep-2016 if writing controls or rules, don't split the row into an array
                                                                    sbufDATA[0] = sTextFile_ALL[nCurrentWriteLine];;
                                                                }

                                                                sbufDATA[0] = sub_slmCurrent._sVal;
                                                            }
                                                        }
                                                        //when finished making changes to the line
                                                        sTextFile_ALL[nCurrentWriteLine] = String.Join(" ", sbufDATA);
                                                    }
                                                }
                                                else
                                                {
                                                    nCurrentWriteLine++;
                                                }
                                            }
                                            else                                       //didn't find the proper section name
                                            {
                                                _log.AddString("MODEL CHANGE NOT TRANSLATED TO INP", Logging._nLogging_Level_3);
                                                //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 15");
                                                nCurrentChange++;       //met 5/17/2012 skip over to avoid infinite loop. this most likely happens if tblMEV has multiple elements...
                                                if (nCurrentChange < nTotalChanges)
                                                    slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                            }
                                        }       //end else
                                    }
                                }

                                nCurrentChange++;       //change not applicable for current loop (insert/update)
                                if (nCurrentChange < nTotalChanges)
                                    slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);

                            }
                            else //this is the normal case of updating the INP file for model element values that are not rules
                            {
                                //met 1/2/2013: check that we do ONLY inserts on inserts loop and ONLY updates on update loop
                                if (bIsInsert == slmCurrent._bIsInsert)
                                {
                                    //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 3");
                                    //MET 4/13/2012: 
                                    if (slmCurrent._nVarType_FK == -1)        //perform insert (don't go through all the "section name" stuff
                                    {
                                        _log.AddString("Table FieldID = -1; is there an issue?", Logging._nLogging_Level_3, false);
                                        /* met 11/12/13: I BELIEVE  that this early approach to insertions is not needed and now we use tblElementLibrary
                                         * 
                                         * 
                                        cu.cuFILE_InsertTextChars(ref sTextFile_ALL, Convert.ToInt32(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["Val"].ToString()), ref connRMG);
                                        nCurrentChange++;
                                        if (nCurrentChange == nTotalChanges) { break; }    // no more changes, so break out of loop

                                        Debug.Print(nScenarioID + ": " + nCurrentChange);      //met 3/1/2012   figure out why the file is sometimes not getting written.
                               
                                         * /
                                         */
                                    }
                                    else                  //general case. standard update being performed
                                    {
                                        //SP 27-Sept-2016 Store the current line in the text file
                                        int nStartingLine = nCurrentWriteLine;

                                        //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 4");
                                        sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                            //BYM   
                                        while ((sCurrentSectionName != slmCurrent._sSectionName) && (nCurrentWriteLine < nFileTotalRows))
                                        {
                                            //sCurrentSectionName = MOUSE_CheckSectionName(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName);                                                      //BYM
                                            sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                        //BYM
                                            nCurrentWriteLine++;
                                            nSectionLine = nCurrentWriteLine;
                                            bInCurrentSection = false;
                                        }

                                        //SP 27-Sep-2016 to avoid the user knowing the order of the DVs, loop through from the start again until reach the required section
                                        if (sCurrentSectionName != slmCurrent._sSectionName)
                                        {
                                            nCurrentWriteLine = 0;
                                            while ((sCurrentSectionName != slmCurrent._sSectionName) && (nCurrentWriteLine < nStartingLine))
                                            {
                                                sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                        //BYM
                                                nCurrentWriteLine++;
                                                nSectionLine = nCurrentWriteLine;
                                                bInCurrentSection = false;
                                            }
                                        }

                                        if (sCurrentSectionName == slmCurrent._sSectionName)
                                        {
                                            //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 5");
                                            if (bIsInsert)      //don't need to find right location; insert now  (new 1/2/2013)
                                            {
                                                if (nListOffset == 0)
                                                {
                                                    listTextFile_ALL = sTextFile_ALL.ToList();  //create the list. we only want to do this if needed, because it takes considerable time.
                                                }
                                                if (!bInCurrentSection)         //first time we navigate to new section
                                                {                               //only advance below until we leave section.
                                                    //SP 01-Aug-2016 removed the ref from this function for sTextFile_ALL - need to confirm it still works
                                                    UpdateHelper_AdvanceToData(sTextFile_ALL, ref nCurrentWriteLine);     //advance to data section (some have 2 header, some 3- this is easiest.
                                                    bInCurrentSection = true;
                                                }

                                                //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 6");
                                                listTextFile_ALL.Insert(nCurrentWriteLine + nListOffset + 1, slmCurrent._sVal);         //met: off by one: nCurrentWriteLine + nListOffset-1,
                                                //   nCurrentWriteLine++;                      met: think this is not needed. offset handles
                                                nCurrentChange++;
                                                slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                                nListOffset++;              //this counter keeps track of how many additional inserts there are
                                            }
                                            else
                                            {
                                                //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 7");
                                                //case : update loopo.
                                                bInCurrentSection = true;

                                                if (GetWhitespaceCharacter(sTextFile_ALL[nCurrentWriteLine]) > 0) //SP 5-Oct-2016 Include searching for a tab. Previously: sTextFile_ALL[nCurrentWriteLine].IndexOf(" ") > 0)                   //check whether we have data row
                                                {
                                                    string sIDName = "";

                                                    //TODO:!!  need to store the position of the SectionName. This allows
                                                    //there is a chance it may work right now, but it should not be considered implemented

                                                    //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 8");
                                                    //test for whether we are using a "system/scenario specific var (eg option table)
                                                    //or (more standard) an element name.
                                                    bool bIsScenarioLevelVar = false; string sFindElementNameOrField;
                                                    sFindElementNameOrField = UpdateHelper_GetElementNameOrField(slmCurrent, ref bIsScenarioLevelVar);

                                                    sIDName = UpdateHelper_AdvanceToCurrent_ID(ref sTextFile_ALL, sFindElementNameOrField, ref nCurrentWriteLine, nSectionLine, sCurrentSectionName); //BYM

                                                    _log.AddString("Found CurrentID: " + nCurrentChange, Logging._nLogging_Level_3, false);
                                                    if (sIDName != "No_ID_Found")
                                                    {
                                                        //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 9");

                                                        //SP 3-Aug-2016 modified to account for spaces or tabls //TODO is this change required for any other models?             //met 4/18/2013 drop leading zero
                                                        //BYM string[] sbufDATA = sTextFile_ALL[nCurrentWriteLine].Split(',');

                                                        string[] sbufDATA = new string[1];
                                                        sbufDATA = sTextFile_ALL[nCurrentWriteLine].Trim().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                                                        int nbufDATARow = 1;                                                                      //met 6/18/2012: modified to support multiple row values. Val of 1 is the default.
                                                        int nCurrentRow = slmCurrent._nRowNo;

                                                        //met 1/4/2013: modified to check thta the next ID has the right type //SP 18-Nov-2016 and is still part of the same line.
                                                        while ((sIDName == UpdateHelper_GetElementNameOrField(slmCurrent, ref bIsScenarioLevelVar)) && (bIsInsert == Convert.ToBoolean(slmCurrent._bIsInsert))
                                                          && nCurrentRow == slmCurrent._nRowNo)
                                                        {
                                                            bool bCorrectRow = true;

                                                            //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 10");
                                                            if (bCorrectRow)
                                                            {

                                                                //nCurrentRow = slmCurrent._nRowNo;
                                                                if (nCurrentRow > nbufDATARow)                                                         // if this is true, we need to get the new row.
                                                                {
                                                                    //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 11");
                                                                     nCurrentWriteLine += nCurrentRow - 1;
                                                                    
                                                                    //SP 3-Aug-2016 modified to account for spaces or tabls //TODO is this change required for any other models?                     //remove the repeating spaces so the split works (occurs in HELPER_SWMM_AdvanceToCurrent_ID for normal workflow)
                                                                    sbufDATA = CommonUtilities.RemoveRepeatingChar(sTextFile_ALL[nCurrentWriteLine]).Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                                                                    //SP 18-NOv-2016 need to set the last row to be the relative row we are currently updating so that sbufDATA is not overwritten
                                                                    nbufDATARow = nCurrentRow;
                                                                }

                                                                int index = slmCurrent._nFieldNumber;
                                                                sbufDATA[index - 1] = slmCurrent._sVal;
                                                                nCurrentChange++;
                                                                //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 12");
                                                                if (nCurrentChange < nTotalChanges)
                                                                    slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);

                                                                if (nCurrentChange == nTotalChanges) { break; }    // no more changes, so break out of loop

                                                                Debug.Print(nScenarioID + ": " + nCurrentChange);      //met 3/1/2012   figure out why the file is sometimes not getting written.
                                                            }


                                                        }

                                                        //BYM sTextFile_ALL[nCurrentWriteLine] = String.Join(",", sbufDATA);
                                                        //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 13");
                                                        sTextFile_ALL[nCurrentWriteLine] = String.Join(" ", sbufDATA);

                                                        //after written back - adjust back to base line that is being assessed to
                                                        nCurrentWriteLine -= nCurrentRow - 1;
                                                    }
                                                    else
                                                    {
                                                        //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 14");
                                                        nCurrentChange++;  //nothing found- move on to the next change.  TODO L log this change
                                                        if (nCurrentChange < nTotalChanges)
                                                            slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                                    }
                                                }
                                                else
                                                {
                                                    nCurrentWriteLine++;
                                                }
                                            }
                                        }
                                        else                                       //didn't find the proper section name
                                        {
                                            _log.AddString("MODEL CHANGE NOT TRANSLATED TO INP", Logging._nLogging_Level_3);
                                            //Console.WriteLine("Thread: " + tThread.Name + ". Scenario ID: " + nScenarioID + ". Location 15");
                                            nCurrentChange++;       //met 5/17/2012 skip over to avoid infinite loop. this most likely happens if tblMEV has multiple elements...
                                            if (nCurrentChange < nTotalChanges)
                                                slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                        }
                                    }       //end else
                                }
                                else
                                {               //end if   if (bIsInsert == Convert.ToBoolean(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["IsInsertFeature"]))
                                    nCurrentChange++;       //change not applicable for current loop (insert/update)
                                    if (nCurrentChange < nTotalChanges)
                                        slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                }
                            }
                        }
                    }


                
                    //write out 

                string sOUT;
                if (sOptionalOutput_TextFile == "nothing")
                {
                    sOUT = sINP_File;
                }
                else
                {
                    sOUT = sOptionalOutput_TextFile;

                }

                _log.AddString("Write: " + sOUT, Logging._nLogging_Level_2);
                File.WriteAllLines(sOUT, sTextFile_ALL);              //overwrite the file initially passed
                }

                catch (Exception ex)
                {
                    _log.AddString(string.Format("Error encountered updating the file {0}: ", sINP_File, ex.Message), Logging._nLogging_Level_1);
                    throw; //need this to be handled by the ProcessScenario routine - don't want to proceed if you have not successfully updated the inp
                }
                finally
                {
                    if (fileMEX != null)
                        fileMEX.Close();
                }
                    return "crap";
            }
            else 
            { 
                return "crap"; 
            }
        }

#region UpdateHelpers
                //todo: this should be modified to pull directly from memory as well if needed.
        //met changed to 'Qual1Pos' because of SQL SERVER error
        private IEnumerable<simLinkModelChangeHelper> Updates_GetChangeDS(int nScenarioID)
        {
        //    Dictionary <int, swmmTableHelper> dictLocal = _dictSL_TableNavigation.OfType<swmmTableHelper>();
            
       //     IEnumerable<simlinkTableHelper>

            IEnumerable<simLinkModelChangeHelper> ModelChangesList =  from ModelChanges in _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.MEV)
                                                                    .Where(x => x._nScenarioID == nScenarioID).AsEnumerable()               //which performance to characterize
                              join EPANET_DICT in _dictSL_TableNavigation.AsEnumerable()
                              on ModelChanges._nVarType_FK  equals
                              EPANET_DICT.Key
                                   orderby EPANET_DICT.Value._nSectionNumber
                                   orderby ModelChanges._nElementID
                                   orderby ModelChanges._nRecordID
                              select new simLinkModelChangeHelper
                              {
                                  _sVal = ModelChanges._sVal,
                                  _sElementName = ModelChanges._sElementName,
                                  _nElementID = ModelChanges._nElementID,
                                  _nRecordID = ModelChanges._nRecordID,
                                  _nSectionNumber = EPANET_DICT.Value._nSectionNumber,
                                  _sSectionName = EPANET_DICT.Value._sSectionName,
                                  _nTableID = EPANET_DICT.Value._nTableID,
                                  _sTableName = EPANET_DICT.Value._sTableName,
                                  _sFieldName = EPANET_DICT.Value._sFieldName,
                                  _nFieldNumber = EPANET_DICT.Value._nFieldNumber,
                                  _nRowNo = EPANET_DICT.Value._nRowNo,
                                  _nVarType_FK = EPANET_DICT.Value._nVarType_FK,
                                  _bIsScenarioSpecific = EPANET_DICT.Value._bIsScenarioSpecific,
                                  _sQualifier1 = "-1",                  //todo : figure out how the qulifier info is obtained (or if there is a better way to do this)
                                  _nQual1Pos = -1
                              };
                return ModelChangesList.Cast<simLinkModelChangeHelper>();
        //    return null;
          /*  if ("TODO: reference Mem storage approach" != "true"){
                string sql = "SELECT ElementName, Val, ScenarioID_FK, TableName, FieldName, SectionNumber, SectionName, FieldINP_Number, ElementID, KeyColumn, TableFieldKey_FK, DV_ID_FK, IsScenarioSpecific, RowNo, Qualifier1, Qual1Pos, IsInsertFeature"
                                + " FROM qryMEV001_SWMM WHERE ((ScenarioID_FK)=" + nScenarioID + ") ORDER BY SectionNumber, ElementName, Model_ID;";                     //BYM2012
                 DataSet dsModelChanges = _dbContext.getDataSetfromSQL(sql);
                return dsModelChanges;
               }
            else{
                return null;
            }*/
        }




        // helper to increment nCurrentWriteLine to not 
        //formerly : HELPER_SWMM_AdvanceToData
        private void UpdateHelper_AdvanceToData(string[] sTextFile_ALL, ref int nCurrentWriteLine)
        {
            bool bExit = false;
            int nCountList = sTextFile_ALL.Length;
            int nMaximumPossibleNumberHeaderLines = 3; //SP 6-Jul-2016 not always possible
            int nLastLineToCheck = Math.Min(nCountList, nCurrentWriteLine + nMaximumPossibleNumberHeaderLines);

            while (!bExit && (nCurrentWriteLine < nLastLineToCheck))
            {
                nCurrentWriteLine++;                            //index whether found or not - 
                //SP 1-Aug-2016 if empty line then found end of section so also exit out
                if (sTextFile_ALL[nCurrentWriteLine].IndexOf(";;--") >= 0 || sTextFile_ALL[nCurrentWriteLine] == "")
                {
                    bExit = true;
                }
            }
        }


        //met 3/18/14: updated from SWMM case
        public string UpdateHelper_GetElementNameOrField(simLinkModelChangeHelper slm, ref bool bIsScenarioLevelVar)
        {
            string sFindElementNameOrField;
            if (!slm._bIsScenarioSpecific)             //met 1/17/14      //Convert.ToInt32(dr["IsScenarioSpecific"].ToString()) == 0)             //met 4/16/2012: 
            {
                sFindElementNameOrField = slm._sElementName;       //typical case
                bIsScenarioLevelVar = true;
            }
            else
            {
                sFindElementNameOrField = slm._sFieldName;              // dr["FieldName"].ToString();           //find the field name for summmary variables. 
                bIsScenarioLevelVar = false;
            }

            return sFindElementNameOrField;

        }

        //formerly HELPER_SWMM_AdvanceToCurrent_ID
        public string UpdateHelper_AdvanceToCurrent_ID(ref string[] sTEXT_File, string sFindID, ref int nCurrentFilePosition, int nSectionStartIndex, string sSectionName)
        {
            int nStartingPosition = nCurrentFilePosition; string sbuf; int nID_Index = 0;
            string sReturn = "No_ID_Found"; bool bFound = false;
            while ((nCurrentFilePosition < sTEXT_File.Length) && (!bFound))
            {
                if (UpdateHelper_IsNewMEX_Section(sTEXT_File[nCurrentFilePosition].ToString())) { break; }  //exit the while if we hit a new section

                //SP 28-Sep-2016 Controls and Rules need to be searched for using the entire string - not just one index
                if ((sSectionName == "CONTROLS") || (sSectionName == "RULES"))
                {
                    sbuf = Regex.Replace(sTEXT_File[nCurrentFilePosition].ToString().Replace(";", ""), @"\s+", string.Empty, RegexOptions.Multiline);
                }
                else if (sSectionName == "TIMES") //times quite often contain spaces in their ID
                {
                    nID_Index = 0;
                    sTEXT_File[nCurrentFilePosition] = CommonUtilities.RemoveRepeatingChar(sTEXT_File[nCurrentFilePosition]);                                  //BYM2012
                    sbuf = UpdateHelper_GetIDFromDataRow(sTEXT_File[nCurrentFilePosition].ToString(), nID_Index, sSectionName, sFindID.Split(' ').Length);
                }
                else
                {
                    nID_Index = 0;
                    sTEXT_File[nCurrentFilePosition] = CommonUtilities.RemoveRepeatingChar(sTEXT_File[nCurrentFilePosition]);                                  //BYM2012
                    sbuf = UpdateHelper_GetIDFromDataRow(sTEXT_File[nCurrentFilePosition].ToString(), nID_Index, sSectionName);
                }

                if (sbuf == sFindID)
                {
                    sReturn = sFindID;          //we have found the ID exit loop
                    bFound = true;
                }
                else
                {
                    nCurrentFilePosition++;
                }
            }
            if (!bFound)
            {
                for (int i = nSectionStartIndex; i < nStartingPosition; i++)
                {
                    nCurrentFilePosition = i;

                    //SP 28-Sep-2016 Controls and Rules need to be searched for using the entire string - not just one index
                    if ((sSectionName == "CONTROLS") || (sSectionName == "RULES"))
                    {
                        sbuf = Regex.Replace(sTEXT_File[nCurrentFilePosition].ToString().Replace(";", ""), @"\s+", string.Empty, RegexOptions.Multiline);
                    }
                    else
                    {
                        sbuf = UpdateHelper_GetIDFromDataRow(sTEXT_File[nCurrentFilePosition].ToString(), nID_Index, sSectionName, sFindID.Split(' ').Length);                //BYM2012
                    }

                    if (sbuf == sFindID)
                    {
                        sReturn = sFindID;
                        bFound = true;          //we have found the ID exit loop
                        break;
                    }
                    else
                    {
                        nCurrentFilePosition++;
                    }
                }
            }
            return sReturn;
        }

        //formerly: UpdateHelper_IsNewMEX_Section
        public bool UpdateHelper_IsNewMEX_Section(string s, bool bExcludeInteriorTables = true)
        {
            if (s.Trim() == "")
            {
                return false;
            }
            else if (s.Trim().Substring(0, 1) == "[")
            {
                if (bExcludeInteriorTables)
                {
                    bool bInteriorTable = false;
                    if (s.IndexOf("LogicCondition") > 0) { bInteriorTable = true; }
                    if (s.IndexOf("ControlFunction") > 0) { bInteriorTable = true; }
                    if (s.IndexOf("ControlledDevice") > 0) { bInteriorTable = true; }
                    if (bInteriorTable)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        //met 11/6/2013: added means to make a special ID for LID_USAGE
        public string UpdateHelper_GetIDFromDataRow(string sbuf, int nID_Column = 0, string sSectionName ="NOTHING", int nNthSpace = 1)
        {
            sbuf = sbuf.Trim();         //met 4/18/2013: first  blank was causing problems.

            int nSpaceIndex = -1;
            if (nID_Column == -1)           //this is a special case where no comma exists; just send back cleaned id field.
            {
                return CleanIDField(sbuf, nNthSpace);
            }

            if (sSectionName=="LID_USAGE")      //concatenate first two columns
            {
                nSpaceIndex = sbuf.IndexOf(" ");
                string sFirstPart = sbuf.Substring(0, nSpaceIndex);
                string sSecondPart = sbuf.Substring(nSpaceIndex).Trim();
                sSecondPart = sSecondPart.Substring(0,sSecondPart.IndexOf(" "));
                sbuf = sFirstPart+"_"+ sSecondPart;
                return sbuf;                            //return the concat of subcatchment && LID_Process
            }
            else
            {
                for (int i = 0; i < nID_Column; i++)
                {
                    //BYM2012 nCommaIndex = sbuf.IndexOf(",");
                    nSpaceIndex = GetWhitespaceCharacter(sbuf, nNthSpace); //SP 5-Oct-2016 Include searching for a tab
                    if (nSpaceIndex > 0)
                    {
                        sbuf = sbuf.Substring(nSpaceIndex + 1, sbuf.Length - nSpaceIndex - 1);
                    }
                    else
                    {
                        return "No_ID_Found";
                    }
                }
           
                //BYM nCommaIndex = sbuf.IndexOf(",");
                nSpaceIndex = GetWhitespaceCharacter(sbuf, nNthSpace); //SP 5-Oct-2016 Include searching for a tab
                if (nSpaceIndex >= 0)
                {
                    return CleanIDField(sbuf.Substring(0, nSpaceIndex), nNthSpace);
                }
                else
                {               //no comma found, this is not a typical data row
                    return "No_ID_Found";
                }
            }
        }
        private string CleanIDField(string sID, int nnthSpace = 1)
        {
            //BYM string sReturn = sID.Substring(sID.IndexOf('=') + 1, sID.Length - sID.IndexOf('=') - 1).Trim();
            string sReturn = sID.Substring(GetNthIndex(sID, " ", nnthSpace) + 1, sID.Length - GetNthIndex(sID, " ", nnthSpace) - 1).Trim();
            return CleanMEXString(sReturn);
        }

        private string CleanMEXString(string sbuf)
        {
            //BYM2012 char[] nogood = { '\'' };
            char[] nogood = { ' ' };
            return sbuf.TrimStart(nogood).TrimEnd(nogood);
        }

        //SP 8-Mar-2016 - Additional EPANET required changes based on a DV value - Modify a model change value based on a DV value
        protected override void ModifyModelChanges_SpecialCase
            (int DV_ID_FK, int TableFieldKey, int ScenID, ref string val, string note, string ElName, int ElId, int nDV_Option)
        {

            //if diameter for a pipe is to be changed, ensure the status of the pipe is also set
            if (TableFieldKey == _nFieldDict_PIPE_DIAMETER)
            {
                //default pipe diameter to be 1 if option has been set to 0 by user - setting pipe to be closed with a non-zero diameter
                if (Convert.ToDouble(val) == 0.0)
                    val = _DefaultPipeDiameterForClosed;
            }

            //if pipe / pump initial status may need to convert 0-1 to closed-open
            if (TableFieldKey == _nFieldDict_PUMP_STATUS)
            {
                double dval;
                //check if number can be converted to double
                if (Double.TryParse(val, out dval))
                {
                    if (dval == 0.0)
                        val = _StatusForClosed;
                    else
                        val = _StatusForOpen;
                }
            }

        }

        //SP 8-Mar-2016 - Additional EPANET required changes based on a DV value - create a new model change based on a DV value
        protected override void AdditionalRequiredModelChanges_SpecialCase(int DV_ID_FK, int TableFieldKey, int ScenID, string val, string note,
            string ElName, int ElId, int nDV_Option)
        {
            //if diameter for a pipe is to be changed, ensure the status of the pipe is also set
            if (TableFieldKey == _nFieldDict_PIPE_DIAMETER)
            {
                //default pipe diameter to be 1 if option has been set to 0 by user - setting pipe to be closed with a non-zero diameter
                if (val == _DefaultPipeDiameterForClosed)
                {
                    //Insert a new model change to ensure the status is changed to closed
                    InsertModelValList(DV_ID_FK, _nFieldDict_PIPE_STATUS, ScenID, "Closed", note, ElName,
                        ElId, nDV_Option);

                }
                else
                {
                    //ensure the status of the pipe is open
                    InsertModelValList(DV_ID_FK, _nFieldDict_PIPE_STATUS, ScenID, "Open", note, ElName,
                        ElId, nDV_Option);

                }
            }

            //if tank diameter is changed, ensure Volume Curve is blank otherwise this overrides it
            if (TableFieldKey == _nFieldDict_TANK_DIAMETER)
            {
                //for tank diameter to be effective, must have volume curve = 0
                //Insert a new model change to ensure the status is changed to closed
                InsertModelValList(DV_ID_FK, _nFieldDict_TANK_VOLUMECURVE, ScenID, "", note, ElName,
                    ElId, nDV_Option);
            }
        }

#endregion



        #endregion

        #region OUT

        public long ReadOUTData(int nEvalId, int nScenarioID, string sOutFile)           //met 7/31/2011 db pass arg change string sConnRMG)
        {
            long nResult;
            //string sOutFile = "C:\\Users\\mthroneb\\Documents\\Optimization\\Models\\GI_SENS\\v03\\DotOutTest\\swmm_gi_v003_DotOut.out";

            EPANET_OpenOutFile(sOutFile);
            //now we know how many rows to cast the array to.
            int nTS_Records = _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()).Count();

            //performed in SimLInk init       _dResultTS_Vals = new double[nTS_Records][,];
            //performed in SimLInk init       _sTS_GroupID = new string[nTS_Records];                 //store group array which is needed for HDF write
            //int nCounter = 0; //SP 15-feb-2017 no longer used - get index of each TS from the dataset using _dictResultTS_Indices instead
            foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString())) //SP 15-Feb-2017 only get the primary TS from the dataset
            {
                double[,] dvals = new double[EPANET_Nperiods, 1];        //   double[,] dVals = new double[SWMM_Nperiods, 1];              //hold the current TS Record
              //  double[,] dvals = new double[700000, 1]; 
                int nVarType = EPANET_GetOUT_VarType(dr["FeatureType"].ToString());
                int nResultCode = Convert.ToInt32(dr["ResultIndex"].ToString());
                int nElementIndex = Convert.ToInt32(dr["ElementIndex"].ToString());
                
                //for testing only
                string NodeName = dr["Element_Label"].ToString();

                double dCatchError = -1;

                
                for (int i = 0; i < EPANET_Nperiods; i++)
             //   for (int i = 0; i < 700000; i++)
                {
                    dvals[i, 0] = GetEPANETSeries(nVarType, nElementIndex, nResultCode, i + 1);
                 //  dvals[i, 0] = GetSWMMSeries2(0, 0, 0, i + 1);             // keep for debug!!

                    if (dCatchError == -666.66)
                    {
                        break;          //met 3/7/2012     exit if the value was not found. this allows us to close and not hang upt he file.
                    }

                }
                
                //     _hdf5_SWMM must be initialized prior to this function call.
                string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_ID"].ToString(), "SKIP", "SKIP");

                int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())]; //SP 15-Feb-2017 get index for saving the TS back
                _sTS_GroupID[nIndex] = sGroupID;

                if (dCatchError == -666.66)
                {
                    break;          //met 3/7/2012     exit if the value was not found. this allows us to close and not hang upt he file.
                }

                _dResultTS_Vals[nIndex] = dvals;              // add current TS to jagged array //SP 15-Feb-2017 changed nCounter to nIndex - tidier rather than assuming an order
                //nCounter++; //SP 15-feb-2017 no longer used
            }
 //intermediate transition           if (true)                                           //optional write to REPO; otherwise keep for event processing
            //intermediate transition      - handle outside function               WriteTimeSeriesToRepo();
            //
            CloseEPANET_OutFile();



            return -1;
        }

        public void CloseEPANET_OutFile()
        {
            if (b != null)
            {
                b.Close();
                b = null;//
            }
        }

        //SP 28-Feb-2017 Modified to keep consistent with Secondary and AUX requests. SQL Server struggles with merge if datasets are not consistent
        //SP 21-Apr-2017 adjusted to only retrieve with retrievecode = primary. Previously relying on join with VarTypeResult
        private DataSet ReadOut_GetDataSet(int nEvalId)
        {
            string sqlFD = "SELECT tblResultTS.ResultTS_ID, tblResultTS.Result_Label, tblResultTS.Element_Label, tlkpEPANET_ResultsFieldDictionary.FeatureType, VarResultType_FK, tlkpEPANET_ResultsFieldDictionary.FieldName, tlkpEPANET_ResultsFieldDictionary.ColumnNo AS ResultIndex,"
                            + " tlkpEPANET_ResultsFieldDictionary.EPANET_CODE1, tblResultTS.ElementIndex, tblResultTS.EvaluationGroup_FK, FieldName as FieldLabel, tblResultTS.BeginPeriodNo, RetrieveCode, AuxID_FK, SQN, CustomFunction, FunctionArgs, RefTS_ID_FK, FunctionID_FK, UseQuickParse" //, 1 as ts_code" //SP 15-Feb-2017 removed to ensure no other references for testing. Removed reference to IsSecondary
                            + " FROM (tblResultTS INNER JOIN tlkpEPANET_ResultsFieldDictionary ON tblResultTS.VarResultType_FK = tlkpEPANET_ResultsFieldDictionary.ResultsFieldID) LEFT OUTER JOIN tblFunctions ON tblResultTS.FunctionID_FK = tblFunctions.FunctionID"
                            + " WHERE (((EvaluationGroup_FK)=" + nEvalId + ") and RetrieveCode = "+ ((int)RetrieveCode.Primary).ToString() + ")";
            DataSet dsFD = _dbContext.getDataSetfromSQL(sqlFD);
            return dsFD;
        }

        //SP 15-Feb-2017 No reason to have a separate function here - keep standard and use Simlink
        /*private void WriteTimeSeriesToRepo(hdf5_wrap hdf5, string sTS_Filename)
        {
            int nbatch_size = 15000;
            int nStart = 0;

            //only write out "primary" TS... secondary  //SP 15-Feb-2017 Now writing out all TS - Note for MET: any issues with this?
            int nTSCount = _dsEG_ResultTS_Request.Tables[0].Rows.Count ;          
            int nEnd = Math.Min(nStart + nbatch_size, nTSCount);


            while (nStart < nTSCount)
            {
                //open HDF5
                _hdf5.hdfOpen(sTS_Filename, false, true);

                if (true)
                {
                    for (int i = nStart; i < nEnd; i++)
                    {
                        _hdf5.hdfWriteDataSeries(_dResultTS_Vals[i], _sTS_GroupID[i], "1");
                        //Console.WriteLine(string.Format("Written group {0}", i));
                    }
                }

                //Close HDF5
                _hdf5.hdfClose();

                nStart = nEnd;
                nEnd = Math.Min(nStart + nbatch_size, nTSCount);
            }

            //write auxilliary TS to HDF5 - SP 15-Feb-2017 now Aux are all part of _dsEG_ResultTS_Request
            /*nbatch_size = 15000;
            nStart = 0;
            int nAuxCountStart = _dsEG_ResultTS_Request.Tables[0].Rows.Count + _dsEG_SecondaryTS_Request.Tables[0].Rows.Count;
            int nAUXTSCount = _dsEG_TS_AUX_Request.Tables[0].Rows.Count;          //only write out Auxilliary for debugging purposes 
            nEnd = Math.Min(nStart + nbatch_size, nAUXTSCount);
            while (nStart < nAUXTSCount)
            {
                //open HDF5
                _hdf5.hdfOpen(sTS_Filename, false, true);

                if (true)
                {
                    for (int i = nStart + nAuxCountStart; i < nEnd + nAuxCountStart; i++)
                    {
                        _hdf5.hdfWriteDataSeries(_dResultTS_Vals[i], _sTS_GroupID[i], "1");
                        //Console.WriteLine(string.Format("Written group {0}", i));
                    }
                }

                //Close HDF5
                _hdf5.hdfClose();

                nStart = nEnd;
                nEnd = Math.Min(nStart + nbatch_size, nPrimaryTSCount);
            }
        }*/

        public int EPANET_GetOUT_VarType(string sVarType)
        {
            switch (sVarType)
            {
                case "Node":
                    return 0;           //met 3/22/14- guessing
                case "Link":
                    return 1;
                default:
                    return -1;
            }
        }
        
        public void EPANET_OpenOutFile(string sOutFile)
        {
            string sFileName = sOutFile;
            b = new BinaryReader(File.Open(sFileName, FileMode.Open));      //open the streamreader; this needs to be closed later

            long offset;
            long length = (long)b.BaseStream.Length;

            b.BaseStream.Seek(length - 3 * nRecordSize, SeekOrigin.Begin);      //changed to '2' from 5 length - 5 * nRecordSize, SeekOrigin.Begin)
        //    offset0 = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
          //  StartPos = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_Nperiods = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            errCode = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            magic2 = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);

            //
            b.BaseStream.Seek(0, SeekOrigin.Begin);
            magic1 = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);

            version = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_Nnodes = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_NReservoirsAndTanks = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_Nlinks = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_Npumps = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_Nvalves = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_Nwq_option = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_Nnode_for_tracking = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            EPANET_FlowUnits = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);

            //calc from inspection
            int nBulkOffset = 888;
            int nNodeFactor = 36;       //20
            int nLinkFactor = 52;       //36
            int nTankFactor = 8;            // 20;        //8
            int nPumpFactor = 0;        // 28;        //28
            int nValveFactor = 0;       // 52;          //

            //prolog offset- by burdensome trial and error
            offset = nBulkOffset + nNodeFactor * EPANET_Nnodes + nLinkFactor * EPANET_Nlinks + nTankFactor * EPANET_NReservoirsAndTanks + nPumpFactor * EPANET_Npumps;  // +SWMM_Nvalves * nValveFactor;   // +4 + 24; //24 is       // chg 852 to 858 
            offset += 28 * EPANET_Npumps + 4; //energy offset
            offset -= nRecordSize;              //by inspection

            //      offset += SWMM_Npumps * SWMM_Nperiods*nRecordSize;
      //      offset = 2004;
            StartPos = offset;
            offset = offset;            // offset0 + offset;
            b.BaseStream.Seek(offset, SeekOrigin.Begin);            //dubious?

            //SubcatchVars = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
           // b.BaseStream.Seek(SubcatchVars * nRecordSize, SeekOrigin.Current);
            _nNodeVarCount = 4;       // 4 results vars?
            _nLinkVarCount = 8;
                
                
              //  BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
          ///  b.BaseStream.Seek(NodeVars * nRecordSize, SeekOrigin.Current);
         //   int LinkVars = 8;    BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
       //     b.BaseStream.Seek(LinkVars * nRecordSize, SeekOrigin.Current);
        //    SysVars = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
//
       //     offset = StartPos - 3 * nRecordSize;
           // b.BaseStream.Seek(offset, SeekOrigin.Begin);
        //    SWMM_StartDate = BitConverter.ToInt32(b.ReadBytes(sizeof(double)), 0);
          //  SWMM_ReportStep = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            BytesPerPeriod =  ( EPANET_Nnodes * _nNodeVarCount + _nLinkVarCount * EPANET_Nlinks) * 1 * nRecordSize;
           // BytesPerPeriod = 2 * nRecordSize + (SWMM_Nsubcatch * SubcatchVars + SWMM_Nnodes * NodeVars + SWMM_Nlinks * LinkVars + SysVars) * nRecordSize;

        }
        
        public double  GetEPANETSeries2(int iType, int iIndex, int vIndex, int nPeriod){
            long offset = 0;
     //debug       offset = 2552000;       //hartford fuck debug
            offset = offset + (nPeriod - 1) * nRecordSize;// +2 * nRecordSize;
            b.BaseStream.Seek(offset, SeekOrigin.Begin);
            float fVal;     

            fVal = BitConverter.ToSingle(b.ReadBytes(nRecordSize), 0);
            return Convert.ToDouble(fVal);
        }
        
        
        public double GetEPANETSeries(int iType, int iIndex, int vIndex, int nPeriod)
        {
            try
            {

                long offset = 0;
                offset = StartPos + (nPeriod - 1) * BytesPerPeriod;// +2 * nRecordSize;         //removed the period reference because this will go through all the data.
                switch (iType)
                {
                    case 0:                 // //"NODE":
                        offset += nRecordSize * (iIndex + EPANET_Nnodes * vIndex); //met by inspection, this is different var usage than swmm5022                   //offset += Convert.ToInt32(nRecordSize) * (iIndex * Convert.ToInt32(SubcatchVars) + vIndex);
                        break;
                    case 1:                 //"LINK":
                        offset += nRecordSize * (EPANET_Nnodes * _nNodeVarCount + vIndex * EPANET_Nlinks + iIndex);                 //offset += Convert.ToInt32(nRecordSize) * (iIndex * Convert.ToInt32(SubcatchVars) + vIndex);
                        break;
                /*    case 2:                 //"LINK":
                        offset += nRecordSize * (SWMM_Nsubcatch * SubcatchVars + SWMM_Nnodes * NodeVars + iIndex * LinkVars + vIndex);
                        break;
                    case 3:                 //"SYS":
                        offset += nRecordSize * (SWMM_Nsubcatch * SubcatchVars + SWMM_Nnodes * NodeVars + SWMM_Nlinks * LinkVars + vIndex);
                        break;*/
                    default:
                        return 0;
                }

                b.BaseStream.Seek(offset, SeekOrigin.Begin);
                float fVal;

                fVal = BitConverter.ToSingle(b.ReadBytes(nRecordSize), 0);

                //for (int i = 0; i < 21; i++)
                //{
                //    fVal = BitConverter.ToSingle(b.ReadBytes(nRecordSize), 0);
                //    b.BaseStream.Seek(BytesPerPeriod, SeekOrigin.Current);
                //}

                return Convert.ToDouble(fVal);
            }
            catch (Exception ex)
            {
                return -666.66;     //element not found.
                //   _log.AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
            }



        }


        //SP 16-Feb-2016 - Read output using the EPANET dll
        private double[][,] ReadOUTDataUsingEPANETdll(string sTarget_INP, string sSummaryFile, string sOUT, ref long nNumberReportingPeriods)
        {

            //initiate and open the inp file and hydraulic solver
            int nTimeStep = 0;
            _log.AddString(string.Format("  opening hydraulic model {0}", sTarget_INP), Logging._nLogging_Level_2, false);
            
            ENopen(sTarget_INP, sSummaryFile, sOUT);
            ENopenH();
            ENinitH(0);

            _log.AddString("  Reading run time parameters", Logging._nLogging_Level_1, false);

            int nDurationInSeconds = 0;
            int ErrorCode = ENgettimeparam(CommonUtilities.EN_DURATION, ref nDurationInSeconds);

            int nReportingTimeStep = 0;
            int ErrorCode1 = ENgettimeparam(CommonUtilities.EN_REPORTSTEP, ref nReportingTimeStep);

            if (ErrorCode == 0 && ErrorCode1 == 0)
                nNumberReportingPeriods = nDurationInSeconds / nReportingTimeStep;

            _log.AddString(string.Format("  Finished reading run time parameters. Duration = {0}, ReportStep = {1}, NumberPeriods = {2}",
                nDurationInSeconds, nReportingTimeStep, nNumberReportingPeriods), Logging._nLogging_Level_2, false);

            //retrieving the output results through dll - SP 15-Feb-2016
            //needed to reference the global array _dResultTS_Vals as it has already been set to the required length
            double[][,] dreturn = new double[_dResultTS_Vals.Length][,];
            
            //SP 1-Dec-2016 Initialise all dvals - but then adjust for known BeginPeriodNos 
            for (int i = 0; i < dreturn.Length; i++)
            {
                double[,] dvals = new double[nNumberReportingPeriods, 1];
                dreturn[i] = dvals;
            }

            //Moved to after we know how many periods each TS result contains
            //int nNodeLinkCounter = 0; ////SP 15-Feb-2017 Get index for the dr from dictionary
            foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString())) //SP 15-Feb-2017 only populating primary TS
            {
                int nBeginPeriodNo = Convert.ToInt32(dr["BeginPeriodNo"]); 
                //SP 1-Dec-2016 RedimArray after knowing period start no
                double[,] dRedimedVals = new double[nNumberReportingPeriods - (nBeginPeriodNo - 1), 1];
                int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())]; //SP 15-Feb-2017 Get index for the dr from dictionary
                dreturn[nIndex] = dRedimedVals;
                //nNodeLinkCounter++; //SP 15-Feb-2017 Get index for the dr from dictionary
            }


            int nTimeStepCounter = 0;
            do
            {
                ENrunH(ref nTimeStep);
                //nNodeLinkCounter = 0; //SP 15-Feb-2017 Get index for the dr from dictionary

                //SP 04-Apr-2016 - correction to ensure only reporting steps are recorded in TS memory arrays
                //for each time step - if it's a reporting time step, get the required node or link value
                if (nTimeStep % nReportingTimeStep == 0 && nTimeStep > 0)
                {
                    _log.AddString(string.Format("  Reading results for timestep = {0}", nTimeStep), Logging._nLogging_Level_2, false);

                    foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString())) //SP 15-Feb-2017 only want to retrieve primary TS
                    {
                        string sEPANETFeatureType = dr["FeatureType"].ToString();
                        string sParamCode = dr["EPANET_CODE1"].ToString();
                        string sElementIndex = dr["ElementIndex"].ToString();
                        int nBeginPeriodNo = Convert.ToInt32(dr["BeginPeriodNo"]); //SP 1-Dec-2016 Incorporating TS BeginPeriodNo
                        int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())]; //SP 15-Feb-2017 Get index for storing TS in memory for the dr from dictionary
                        float ftmpval = 0;

                        if (nTimeStepCounter >= nBeginPeriodNo - 1)
                        {
                            //call EPANET functions to get the required result of each node / link and save back to the value _nreturn
                            switch (sEPANETFeatureType.ToUpper())
                            {
                                case "NODE":
                                    ENgetnodevalue(Convert.ToInt32(sElementIndex), Convert.ToInt32(sParamCode), ref ftmpval);
                                    dreturn[nIndex][nTimeStepCounter - (nBeginPeriodNo - 1), 0] = ftmpval; //SP 15-Feb-2017 Use nindex for storing TS in memory
                                    _log.AddString(string.Format("    NODE: ResultsTSLabel {0}, ParamCode {1}, Value {2}", dr["Result_Label"].ToString(), sParamCode, ftmpval), Logging._nLogging_Level_3, false);
                                    break;

                                case "LINK":
                                    //SP 6-Oct-2016 headloss returns actual headloss, need to convert to unit headloss - temp until can find an more elegant fix
                                    if (Convert.ToInt32(sParamCode) == CommonUtilities.EN_HEADLOSS)
                                    {
                                        float ftmplenval = 0;
                                        ENgetlinkvalue(Convert.ToInt32(sElementIndex), Convert.ToInt32(sParamCode), ref ftmpval);
                                        ENgetlinkvalue(Convert.ToInt32(sElementIndex), Convert.ToInt32(CommonUtilities.EN_LENGTH), ref ftmplenval);
                                        dreturn[nIndex][nTimeStepCounter - (nBeginPeriodNo - 1), 0] = ftmpval / ftmplenval * 1000; //SP 15-Feb-2017 Use nindex for storing TS in memory
                                        _log.AddString(string.Format("    LINK: ResultsTSLabel {0}, ParamCode {1}, Value {2}", dr["Result_Label"].ToString(), sParamCode, dreturn[nIndex][nTimeStepCounter, 0]),
                                            Logging._nLogging_Level_3, false);
                                        break;
                                    }
                                    else
                                    {
                                          ENgetlinkvalue(Convert.ToInt32(sElementIndex), Convert.ToInt32(sParamCode), ref ftmpval);
                                        dreturn[nIndex][nTimeStepCounter - (nBeginPeriodNo - 1), 0] = ftmpval; //SP 15-Feb-2017 Use nindex for storing TS in memory
                                        _log.AddString(string.Format("    LINK: ResultsTSLabel {0}, ParamCode {1}, Value {2}", dr["Result_Label"].ToString(), sParamCode, ftmpval), Logging._nLogging_Level_3, false);
                                        break;
                                    }
                            }
                        }

                        //nNodeLinkCounter++; //SP 15-Feb-2017 Get index for the dr from dictionary
                    }

                    nTimeStepCounter++;
                }

                //continue to next time step
                ENnextH(ref nTimeStep);
            } while (nTimeStep > 0 && nTimeStepCounter < nNumberReportingPeriods);

            _log.AddString(string.Format("  closing hydraulic model {0}", sTarget_INP), Logging._nLogging_Level_2, false);
            ENcloseH();
            ENclose();

            return dreturn;
        }

        //SP 1-Mar-2017 This should now be handled in the tblSupportFileSpec
        /*private void WriteOutputToCSVForTesting(double[][,] dTSResultValues, string sPathToSaveTo, string sFileName, long nNumberReportingPeriods)
        {
            StringBuilder sb2 = new StringBuilder();

            string[] sHeadings2 = new string[dTSResultValues.Length];
            for (int nindex = 0; nindex < dTSResultValues.Length; nindex++)
            {
                sHeadings2[nindex] = _sTS_GroupID[nindex];
            }
            sb2.AppendLine(string.Join(",", sHeadings2.ToArray()));

            for (int i = 0; i < nNumberReportingPeriods; i++)
            {
                double[] dForTimePeriod = new double[dTSResultValues.Length];
                for (int index = 0; index < dTSResultValues.Length; index++)
                    dForTimePeriod[index] = dTSResultValues[index][i, 0];
                sb2.AppendLine(string.Join(",", dForTimePeriod.ToArray()));
            }

            string sfilePath2 = sPathToSaveTo + "\\" + sFileName;
            File.WriteAllText(sfilePath2, sb2.ToString());
        }*/


        #endregion


        #region OptimizationDelegates
        
        public override void ProcessScenario(double[] vars, double[] objs, double[] constrs)
        {
            //Preload objective with a large cost in case the EPANET model does not function
            for (int i = 0; i < objs.Count(); i++)
                objs[i] = 999999999999;
            
            // add calls to process scenario....
            string sDNA = string.Join(_sDNA_Delimiter, vars);
            int nReturn = ProcessScenarioWRAP(sDNA, _nActiveScenarioID, -1, 100);

            //SP 10-Oct-2016 Write log file after finished processing each scenario processing
            _log.WriteLogFile();

            LoadObjectives(objs);           //load the objective values to send back...
        }

        #endregion


        #region ModifyEPANETINPFileForFireFlow

        /// <summary>
        /// takes string arg and parses into a dictionary
        /// </summary>
        /// <param name="sVal"></param>
        /// <returns></returns>
        //SP 6-Jul-2016 Decouple fireflow node and fireflow information from config file
        //tuple of 2 values - node and fireflow demand
        public static Dictionary<string, double> GetFFDemandsAndNodesFromString(string sVal)
        {
            Dictionary<string, double> dictReturn = new Dictionary<string, double>(); ;
            
            if (sVal != "")
            {
                sVal = sVal.Replace("{", "").Replace("}", "");
                string[] sTuples = sVal.Split('^');

                //values will always be 2 - node and a demand
                for (int i = 0; i < sTuples.Length; i++)
                {
                    string[] sVals = sTuples[i].Split(',');

                    dictReturn.Add(Convert.ToString(sVals[0]), Convert.ToDouble(sVals[1]));
                }

                return dictReturn;
            }
            else
            {
                return dictReturn;
            }

        }

        //SP 6-Jul-2016 Read config file to determine how to modify EPANET model for fireflow
        public void GetEPANETFileModsFromConfiguration(string sXMLFileLocation, ref Dictionary<string, double> FireFlowNodes, ref int nHourToAnalyseFireflow,
            ref string sFireflowPatternPreffix)
        {
            //read required input files from config    
            IConfigSource configXML = null;
            try
            {
                configXML = new XmlConfigSource(sXMLFileLocation);
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("xml {0} to modify EPANET model for fireflow not found", sXMLFileLocation), Logging._nLogging_Level_1);
                Console.WriteLine("Press your favorite key to continue");
                Console.ReadKey();
                return;                 //nothing works wout config, so end.
            }

            string sFFAnalysisHour = configXML.Configs["FireFlowConversion"].GetString("ffanalysishour", "8"); //default value of 8am if nothing else specified
            string sNodesAndDemands = configXML.Configs["FireFlowConversion"].GetString("ffnodesanddemands", ""); //default value of "" if nothing else specified
            sFireflowPatternPreffix = configXML.Configs["FireFlowConversion"].GetString("ffpatternpreffix", "FireFlowPattern_"); //default value of "FireFlowPattern_" if nothing else specified

            FireFlowNodes = GetFFDemandsAndNodesFromString(sNodesAndDemands);
            nHourToAnalyseFireflow = Convert.ToInt16(sFFAnalysisHour);
        }


        /// <summary>
        /// retrieve start/end date
        /// 
        ///     todo: handle case where date OR time changed, but not both.... not supported currenty.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bIsStartTime"></param>
        /// <returns></returns>
        protected override DateTime GetSimStartEndMEV(DateTime dt, bool bIsStartTime)
        {
            int nVarType_FK = _nFieldDict_StartDate;                //todo: better not to hard code to IDs
            int nVarType_FK2 = _nFieldDict_StartTime;
            if (!bIsStartTime)
            {
                nVarType_FK = _nFieldDict_EndDate;
                nVarType_FK2 = _nFieldDict_EndTime;
            }
            if (_lstSimLinkDetail.Count > 0)
            {
                simlinkDetail detail = _lstSimLinkDetail.Where(x => x._nScenarioID == _nActiveScenarioID && x._slDataType == SimLinkDataType_Major.MEV && x._nVarType_FK == nVarType_FK).FirstOrDefault();
                simlinkDetail detail2 = _lstSimLinkDetail.Where(x => x._nScenarioID == _nActiveScenarioID && x._slDataType == SimLinkDataType_Major.MEV && x._nVarType_FK == nVarType_FK2).FirstOrDefault();
                if (detail != null && detail2 != null)
                {
                    string sDate = detail._sVal + " " + detail2._sVal;
                    dt = Convert.ToDateTime(sDate);
                }
            }
            return dt;
        }

        //SP 29-Jun-2016 - Modify EPANET file to append fireflow scenarios onto the end
        public string ModifyEPANETINPFileForFireFlow(string sBaseINP_File, string sBaseRTP_File, string sXMLFFConfigFileLocation)
        {          
            Debug.Print("begin CreateEPANETINPFileWithFireFlow");

            if (File.Exists(sBaseINP_File))
            {
                Debug.Print("Base INP Exists");
                try
                {
                    //create an output file
                    string sTarget_INP = sBaseINP_File.Substring(0, sBaseINP_File.Length - Path.GetExtension(sBaseINP_File).Length) 
                        + "_Fireflow_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".inp";

                    //make the required modifications
                    int nHourToAnalyseFireflow = -1;
                    Dictionary<string, double> dictFireFlowNodes = new Dictionary<string, double>();
                    Dictionary<string, double> dictStorageTankInitialHead = new Dictionary<string, double>();
                    string sFireflowPatternPreffix = "";

                    //populate dictionary with nodes and fireflow demands, hour of fireflow analysis and pattern preffix
                    GetEPANETFileModsFromConfiguration(sXMLFFConfigFileLocation, ref dictFireFlowNodes, ref nHourToAnalyseFireflow, ref sFireflowPatternPreffix);

                    int nNumberOfFireflowNodesToAnalyse = dictFireFlowNodes.Count;

                    //initiate and open the inp file 
                    ENopen(sBaseINP_File, sBaseRTP_File, "");

                    //get required time data
                    int nCurrentDuration = -1;
                    int nreturn = ENgettimeparam(CommonUtilities.EN_DURATION, ref nCurrentDuration);
                    int nReportStepDuration = -1;
                    nreturn = ENgettimeparam(CommonUtilities.EN_REPORTSTEP, ref nReportStepDuration);
                    int nPatternStepDuration = -1;
                    nreturn = ENgettimeparam(CommonUtilities.EN_PATTERNSTEP, ref nPatternStepDuration);
                    
                    //calculate the period for fireflow
                    int nPeriodForFireflow = nHourToAnalyseFireflow * 60 * 60 / nPatternStepDuration + 1;
                    int nPeriodsInModifiedEPANETFile = nCurrentDuration / nPatternStepDuration + nNumberOfFireflowNodesToAnalyse;
                    int nPeriodsInBaseEPANETFile = nCurrentDuration / nPatternStepDuration;


                    //extend the existing patterns for the fireflow periods
                    //find the number of current patterns
                    int nCurrentPatternCount = -1;
                    nreturn = ENgetcount(CommonUtilities.EN_PATCOUNT, ref nCurrentPatternCount);

                    for (int nPatternIndex = 1; nPatternIndex <= nCurrentPatternCount; nPatternIndex++)
                    {
                        //set the length of the new required pattern
                        float[] dPattern = new float[nPeriodsInModifiedEPANETFile];

                        //copy existing pattern
                        for (int iPeriod = 0; iPeriod < nPeriodsInBaseEPANETFile; iPeriod++)
                        {
                            //periods in EPANET are indexed from 1
                            nreturn = ENgetpatternvalue(nPatternIndex, iPeriod + 1, ref dPattern[iPeriod]);

                            //append new pattern values - if it's the hour of the fireflow
                            if (iPeriod == nPeriodForFireflow - 1)
                            {
                                for (int i = nPeriodsInBaseEPANETFile; i <= nPeriodsInModifiedEPANETFile - 1; i++)
                                {
                                    dPattern[i] = dPattern[iPeriod];
                                }
                            }
                        }

                        //save modified pattern back to inp
                        ENsetpattern(nPatternIndex, dPattern, nPeriodsInModifiedEPANETFile);
                    }


                    //add a new demand pattern for each fireflow node
                    //create a list of strings to insert into the [Demands] section
                    List<string> lst_sFireflowDemands = new List<string>();
                    int nAppendedPeriod = 1;
                    foreach (KeyValuePair<string, double> ent_FireflowNode in dictFireFlowNodes)
                    {
                        string PatternName = sFireflowPatternPreffix + ent_FireflowNode.Key;
                        int patIndex = -1;
                        nreturn = ENaddpattern(PatternName);
                        nreturn = ENgetpatternindex(PatternName, ref patIndex);

                        //set new pattern factors
                        float[] dNewPattern = new float[nPeriodsInModifiedEPANETFile];
                        nreturn = ENsetpattern(patIndex, dNewPattern, nPeriodsInModifiedEPANETFile);
                        nreturn = ENsetpatternvalue(patIndex, nPeriodsInBaseEPANETFile + nAppendedPeriod, 1);
                        nAppendedPeriod++;

                        //SP 5-Jul-16 Only way to add additional demand categories is by modifying the EPANET text file
                        lst_sFireflowDemands.Add(string.Format("{0} {1} {2}", ent_FireflowNode.Key, ent_FireflowNode.Value, PatternName));
                    }

                    //add a new demand category for each fireflow node - can only assign one demand pattern per node this way
                    //find the node index
                    /*string sNodeName = "147";
                    int nCalcElementIndex = -1;
                    float dFireflow = 1000;
                    float nPatIndex = patIndex;
                    nreturn = ENgetnodeindex(sNodeName, ref nCalcElementIndex);

                    nreturn = ENsetnodevalue(nCalcElementIndex, CommonUtilities.EN_BASEDEMAND, dFireflow);
                    nreturn = ENsetnodevalue(nCalcElementIndex, CommonUtilities.EN_PATTERN, nPatIndex);*/

                    //extend the EPS to continue throughout the fireflow
                     //set the new duration to the additional number of periods required for fireflow
                    nreturn = ENsettimeparam(CommonUtilities.EN_DURATION, Convert.ToInt32(nCurrentDuration + nNumberOfFireflowNodesToAnalyse * nReportStepDuration));


                    //SP 6-Jul-2016 TODO - reset the tanks to be at their initial level
                    //add a reservoir to each tank connected with a wide / short pipe. Label accordingly so that the pipes and reservoirs can be set
                    //get the list of storagetanks in model
                    int nNodeCount = -1;
                    nreturn = ENgetcount(CommonUtilities.EN_NODECOUNT, ref nNodeCount);
                    if (nreturn == 0)
                    {
                        for (int i = 1; i <= nNodeCount; i++)
                        {
                            int nTypeCode = -1;
                            nreturn = ENgetnodetype(i, ref nTypeCode);

                            if (nTypeCode == CommonUtilities.EN_TANK)
                            {
                                //add name to the list
                                StringBuilder sTankName = new StringBuilder(CommonUtilities.EN_MAXSTRINGLENGTH);
                                nreturn = ENgetnodeid(i, sTankName);

                                //get the initial head of the tank
                                float fElevation = -1;
                                nreturn = ENgetnodevalue(i, CommonUtilities.EN_ELEVATION, ref fElevation);

                                float fInitialLevel = -1;
                                nreturn = ENgetnodevalue(i, CommonUtilities.EN_TANKLEVEL, ref fInitialLevel);

                                double fInitialHead = Convert.ToDouble(fInitialLevel) + Convert.ToDouble(fElevation);
                                //add all the storage tanks to dictionary
                                dictStorageTankInitialHead.Add(sTankName.ToString(), fInitialHead); 
                            }
                        }
                    }

                    nreturn = ENsaveinpfile(sTarget_INP);
                    ENclose();

                    //-----------now make the remaining changes manually to the text file------------
                    string[] sTextFile_ALL = File.ReadAllLines(sTarget_INP);
                    List<string> listTextFile_ALL = new List<string>();

                    string sCurrentSectionName = "none";
                    
                    int nSectionLine = 0;
                    bool bInCurrentSection = false;
                    int nListOffset = 0;                   //used to keep track of the insert position. a value >=0 indicates that there was at LEAST one insert
                    int nFileTotalRows = sTextFile_ALL.Length;

                    //create the list
                    listTextFile_ALL = sTextFile_ALL.ToList();

                    int nCurrentWriteLine = 0; //now ordering of sections does not matter
                    //-------get through to [RESERVOIRS] Section----------
                    sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                    while ((sCurrentSectionName != "RESERVOIRS") && (nCurrentWriteLine < nFileTotalRows))
                    {
                        sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                        nCurrentWriteLine++;
                        nSectionLine = nCurrentWriteLine;
                        bInCurrentSection = false;
                    }

                    if (!bInCurrentSection)         //first time we navigate to new section
                    {                               //only advance below until we leave section.
                        UpdateHelper_AdvanceToData(listTextFile_ALL.ToArray(), ref nCurrentWriteLine);     //advance to data section (some have 2 header, some 3- this is easiest.
                        bInCurrentSection = true;
                    }

                    //add a new reservoir for each tank
                    foreach (KeyValuePair<string, double> ent_StorageTanks in dictStorageTankInitialHead)
                    {
                        listTextFile_ALL.Insert(nCurrentWriteLine /*+ nListOffset*/ +1, GetResNameFromTankName(ent_StorageTanks.Key) + " " + ent_StorageTanks.Value.ToString());
                        nListOffset++;              //this counter keeps track of how many additional inserts there are
                    }
                    //-----------------------------------------------------


                    nCurrentWriteLine = 0; //now ordering of sections does not matter
                    //-------get through to [COORDINATES] Section----------
                    sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                    while ((sCurrentSectionName != "COORDINATES") && (nCurrentWriteLine < nFileTotalRows))
                    {
                        sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                        nCurrentWriteLine++;
                        nSectionLine = nCurrentWriteLine;
                        bInCurrentSection = false;
                    }

                    if (!bInCurrentSection)         //first time we navigate to new section
                    {                               //only advance below until we leave section.
                        UpdateHelper_AdvanceToData(listTextFile_ALL.ToArray(), ref nCurrentWriteLine);     //advance to data section (some have 2 header, some 3- this is easiest.
                        bInCurrentSection = true;
                    }

                    //add new reservoir coordinates - requires an entry in Coordinates section
                    int nStorageTank = 0;
                    foreach (KeyValuePair<string, double> ent_StorageTanks in dictStorageTankInitialHead)
                    {
                        listTextFile_ALL.Insert(nCurrentWriteLine /*+ nListOffset*/ +1, GetResNameFromTankName(ent_StorageTanks.Key) + " 1000 " + (nStorageTank * 1000 + 1000));
                        nListOffset++;              //this counter keeps track of how many additional inserts there are
                        nStorageTank++;
                    }
                    //-----------------------------------------------------


                    nCurrentWriteLine = 0; //now ordering of sections does not matter
                    //-------get through to [PIPES] Section----------
                    sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                    while ((sCurrentSectionName != "PIPES") && (nCurrentWriteLine < nFileTotalRows))
                    {
                        sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                        nCurrentWriteLine++;
                        nSectionLine = nCurrentWriteLine;
                        bInCurrentSection = false;
                    }

                    if (!bInCurrentSection)         //first time we navigate to new section
                    {                               //only advance below until we leave section.
                        UpdateHelper_AdvanceToData(listTextFile_ALL.ToArray(), ref nCurrentWriteLine);     //advance to data section (some have 2 header, some 3- this is easiest.
                        bInCurrentSection = true;
                    }

                    //add a new pipe linking new reservoirs and each tank - ensure this is closed.
                    foreach (KeyValuePair<string, double> ent_StorageTanks in dictStorageTankInitialHead)
                    {
                        listTextFile_ALL.Insert(nCurrentWriteLine /*+ nListOffset*/ + 1, 
                            string.Format("{0} {1} {2} {3} {4} {5} {6} Closed", 
                                GetPipeNameFromTankName(ent_StorageTanks.Key), 
                                GetResNameFromTankName(ent_StorageTanks.Key),
                                ent_StorageTanks.Key,
                                10, //length
                                100, //diameter
                                200, // roughness
                                0)); //minor losses
                        nListOffset++;              //this counter keeps track of how many additional inserts there are
                    }
                    //-----------------------------------------------------


                    nCurrentWriteLine = 0; //now ordering of sections does not matter
                    //-------get through to [CONTROLS] Section----------
                    sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                    while ((sCurrentSectionName != "CONTROLS") && (nCurrentWriteLine < nFileTotalRows))
                    {
                        sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                        nCurrentWriteLine++;
                        nSectionLine = nCurrentWriteLine;
                        bInCurrentSection = false;
                    }

                    if (!bInCurrentSection)         //first time we navigate to new section
                    {                               //only advance below until we leave section.
                        UpdateHelper_AdvanceToData(listTextFile_ALL.ToArray(), ref nCurrentWriteLine);     //advance to data section (some have 2 header, some 3- this is easiest.
                        bInCurrentSection = true;
                    }

                    //get hours:minutes from the number of seconds
                    TimeSpan time = TimeSpan.FromSeconds(nCurrentDuration + 50);
                    string sCurrentDuration_HoursMinutes = string.Format("{0}:{1}", time.TotalHours.ToString("00"), time.Minutes.ToString("00"));
                    
                    //add a new pipe linking new reservoirs and each tank - ensure this is closed.
                    foreach (KeyValuePair<string, double> ent_StorageTanks in dictStorageTankInitialHead)
                    {
                        listTextFile_ALL.Insert(nCurrentWriteLine + 1,
                            string.Format("LINK {0} open at TIME {1}", GetPipeNameFromTankName(ent_StorageTanks.Key), sCurrentDuration_HoursMinutes));
                        nListOffset++;              //this counter keeps track of how many additional inserts there are
                    }
                    //-----------------------------------------------------


                    nCurrentWriteLine = 0; //now ordering of sections does not matter
                    //------get through to [Demands] Section----------
                    sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                    while ((sCurrentSectionName != "DEMANDS") && (nCurrentWriteLine < nFileTotalRows))
                    {
                        sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(listTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
                        nCurrentWriteLine++;
                        nSectionLine = nCurrentWriteLine;
                        bInCurrentSection = false;
                    }

                    if (!bInCurrentSection)         //first time we navigate to new section
                    {                               //only advance below until we leave section.
                        UpdateHelper_AdvanceToData(listTextFile_ALL.ToArray(), ref nCurrentWriteLine);     //advance to data section (some have 2 header, some 3- this is easiest.
                        bInCurrentSection = true;
                    }

                    //write demand line for each fireflow node
                    foreach (string sFireflowDemands in lst_sFireflowDemands)
                    {
                        listTextFile_ALL.Insert(nCurrentWriteLine /*+ nListOffset*/ +1, sFireflowDemands);       
                        nListOffset++;              //this counter keeps track of how many additional inserts there are
                    }
                    //-----------------------------------------------------

                    


                    //convert the list back to an array for writing back to file
                    sTextFile_ALL = listTextFile_ALL.ToArray();
                    Debug.Print("Write: " + sTarget_INP);
                    File.WriteAllLines(sTarget_INP, sTextFile_ALL);              //overwrite the file initially passed
                }

                catch (Exception ex)
                {
                    _log.AddString("Failed to create EPANET model with fireflow components. Exception: " + ex.Message, Logging._nLogging_Level_1);
                    return "Failed to create EPANET model with fireflow components";
                }
            }

            return "Successful creation of EPANET model with fireflow components";
        }

        private string GetResNameFromTankName(string sTankName)
        {
            return sTankName + "_Res";
        }

        private string GetPipeNameFromTankName(string sTankName)
        {
            return sTankName + "_Pipe";
        }

        //SP 5-Oct-2016 find nth whitespace character in string - test in EPANET to start with. TODO Probably use everywhere
        //SP 27-Dec-2016 Added functionality to check for nth space if a ID contains a space in it - CAREFUL - NEED TO TEST THOROUGHLY FOR EPANET
        private int GetWhitespaceCharacter(string sAnyString, int nSpaceNumber = 1)
        {
            int nSpaceIndex = int.MaxValue;
            int nTabIndex = int.MaxValue;

            //if (sAnyString.IndexOf(" ") > 0)
            if (GetNthIndex(sAnyString, " ", nSpaceNumber) > 0)
            {
                //nSpaceIndex = sAnyString.IndexOf(" ");
                nSpaceIndex = GetNthIndex(sAnyString, " ", nSpaceNumber);
            }

            //if (sAnyString.IndexOf("\t") > 0)
            if (GetNthIndex(sAnyString, "\t", nSpaceNumber) > 0)
            {
                //nTabIndex = sAnyString.IndexOf("\t");
                nTabIndex = GetNthIndex(sAnyString, "\t", nSpaceNumber);
            }

            int nFirstWhiteSpace = Math.Min(nSpaceIndex, nTabIndex);
            if (nFirstWhiteSpace == int.MaxValue)
                return -1;
            else
                return nFirstWhiteSpace;
        }

        //SP 27-Dec-2016 Get nth Whitespace character
        public int GetNthIndex(string s, string t, int n)
        {
            int nindex = -1;
            string sSubString = s;
            if (s.Length > 0)
            {
                nindex = 0;
                for (int i = 0; i < n; i++)
                {
                    nindex = s.IndexOf(t, nindex + 1);
                }
            }

            return nindex;
        }

        #endregion


        #region ImportEPANETINPFile


        public void EPANET_ImportNameValuePair(string sNameValPair, ref DataRow dr)                                             // takes a pair of valus, and puts the second into the column named the first of a datarow.
        {
            if (sNameValPair.Length > 3)       //minimum possible entry
            {
                string sFieldName; string sVal;
                sFieldName = sNameValPair.Substring(0, sNameValPair.IndexOf(" "));
                if (sFieldName.Substring(sFieldName.Length - 1, 1) == ":")
                {
                    sFieldName = sFieldName.Substring(0, sFieldName.Length - 1);        //drop the : in Scenario: EPANET
                }


                sVal = sNameValPair.Substring(sNameValPair.IndexOf(" ") + 1, sNameValPair.Length - sNameValPair.IndexOf(" ") - 1).Trim();
                dr[sFieldName] = sVal;
            }
        }


        //standard override
        public override DataSet EGDS_GetResultTS(int nEvalID, bool bIncludeAux = false)
        {
            return ReadOut_GetDataSet(nEvalID);
        }

        //SP 29-Jun-2016 TODO this needs to be tested - has been brought in from epanet_Link3.cs
        /*public void EPANET_ReadINP_ToDB(string FileName, int nScenario, int nProjID, string sConnRMG)
        {
            if (File.Exists(FileName))
            {
                StreamReader file = null;
                string sFirstChar = ""; string sbuf = ""; string sConcat = "";

                sConnRMG = cu.getModelSpecificConnectionString(5);

                using (OleDbConnection conn = new OleDbConnection(sConnRMG))
                {
                    conn.Open();
                    DataSet dsRS = new DataSet();
                    OleDbDataAdapter daRS = cu.SWMM_GetTableByElementType(ref dsRS, "tblEPANET_RunSettings", "Empty", -1, -1, conn);
                    DataRow rowRunSettings = dsRS.Tables[0].NewRow();
                    try
                    {
                        file = new StreamReader(FileName);
                        while (!file.EndOfStream)
                        {
                            if (sFirstChar != "[")
                            {
                                sbuf = file.ReadLine();
                            }
                            //test for special cases which must be handled uniquely
                            if (sbuf == "[TITLE]" || sbuf == "[OPTIONS]" || sbuf == "[REPORT]" || sbuf == "[TAGS]" || sbuf == "[MAP]")
                            {
                                sFirstChar = "X";
                                sConcat = "";
                                switch (sbuf)
                                {
                                    case "[TITLE]":
                                        while (sFirstChar != "[")
                                        {
                                            int jj = 0; //dummy;do better
                                            sbuf = file.ReadLine();
                                            if ((sbuf.Length > 0) && (sbuf.Substring(0, 1) == "S"))
                                            {
                                                EPANET_ImportNameValuePair(sbuf, ref rowRunSettings);
                                                jj = 100;
                                                //sConcat = sConcat + sbuf + "\n"; 
                                            }    //add the title string into a single str
                                            sFirstChar = cu.GetFirstChar(sbuf);
                                        }
                                        //         rowRunSettings["Title"] = sConcat;
                                        break;

                                    case "[REPORT]":
                                        //allow code to execute through to Options; same processing
                                        while (sFirstChar != "[")
                                        {
                                            sbuf = file.ReadLine();
                                            sFirstChar = cu.GetFirstChar(sbuf);
                                            if (sbuf.Trim().Length > 2 & sFirstChar != "[")
                                            {
                                                EPANET_ImportNameValuePair(sbuf, ref rowRunSettings);
                                            }
                                        }

                                        break;
                                    case "[OPTIONS]":
                                        while (sFirstChar != "[")
                                        {
                                            sbuf = file.ReadLine();
                                            sFirstChar = cu.GetFirstChar(sbuf);
                                            if (sbuf.Trim().Length > 2 & sFirstChar != "[")
                                            {
                                                EPANET_ImportNameValuePair(sbuf, ref rowRunSettings);
                                            }
                                        }

                                        break;
                                    case "[TAGS]":
                                        break;
                                    case "[MAP]": //parse this @#$@#$ing line to get the coordinates
                                        sbuf = file.ReadLine();
                                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
                                        rowRunSettings["MAP_X1"] = sbuf.Substring(0, sbuf.IndexOf(" "));
                                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
                                        rowRunSettings["MAP_Y1"] = sbuf.Substring(0, sbuf.IndexOf(" "));
                                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
                                        rowRunSettings["MAP_X2"] = sbuf.Substring(0, sbuf.IndexOf(" "));
                                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
                                        rowRunSettings["MAP_Y2"] = sbuf;
                                        sbuf = file.ReadLine();
                                        EPANET_ImportNameValuePair(sbuf, ref rowRunSettings);
                                        sbuf = file.ReadLine();
                                        break;
                                }
                            }
                            else
                            {
                                if (sbuf.Length > 0)
                                {
                                    DataSet ds = new DataSet();
                                    char[] nogood = { '[', ']', ';', 'Z', '\t', ' ' };
                                    string sTableName = "tblEPANET_" + sbuf.TrimStart(nogood).TrimEnd(nogood);
                                    OleDbDataAdapter da = cu.SWMM_GetTableByElementType(ref ds, sTableName, "Empty", -1, -1, conn);
                                    sFirstChar = "X";
                                    while ((sFirstChar != "[") & !file.EndOfStream)
                                    {
                                        sbuf = file.ReadLine();
                                        sbuf = sbuf.Trim();
                                        sFirstChar = cu.GetFirstChar(sbuf);

                                        if (sbuf.Length <= 1 || sFirstChar == ";" || sFirstChar == "[")
                                        {
                                            //do nothing
                                        }
                                        else
                                        {
                                            //SWMM_ELEMENT_FillDT(ref ds.Tables[0], sbuf);

                                            // read in the SWMM Section Block into the proper table
                                            //TODO: This should be tightened up to use the tlkpFieldDictoinary, not assuming columns are in right order! for now it works.
                                            string sEntry; int i = 0;
                                            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());       //add a new row to the datatable
                                            while (sbuf.Length > 0)
                                            {

                                                i = i + 1;  //increment (skipping the first ID field
                                                if (sbuf.IndexOf(" ") > 0)
                                                {
                                                    int nIndex = sbuf.IndexOf(" ");
                                                    int nIndex2 = sbuf.IndexOf("\t");
                                                    if ((nIndex2 > 0) && (nIndex2 < nIndex))    //if there is a tab space instead of a space, use that.
                                                    {
                                                        nIndex = nIndex2;
                                                    }
                                                    sEntry = sbuf.Substring(0, nIndex);
                                                    sbuf = sbuf.Substring(nIndex + 1, sbuf.Length - nIndex - 1).Trim();
                                                    sbuf = sbuf.TrimStart(nogood);
                                                }
                                                else
                                                {
                                                    sEntry = sbuf;
                                                    sbuf = "";
                                                }
                                                ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][i] = sEntry;
                                                ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["ModelVersion"] = nScenario;
                                            }//end inner while: decoding the sbuf string
                                        }
                                    }//end while
                                    da.InsertCommand = new OleDbCommandBuilder(da).GetInsertCommand();
                                    da.Update(ds);
                                }
                            }//end else

                        }   //end while    
                        rowRunSettings["ModelVersion"] = nScenario;         //now finalize the creation of the Runsetting record specific to this SWMM file
                        daRS.InsertCommand = new OleDbCommandBuilder(daRS).GetInsertCommand();
                        daRS.Update(dsRS);

                        //create lookup tables for the appropriate columns
                        cu.insertOptionList_MODEL_Lookups(nScenario, 3, nProjID, conn, conn);
                    }
                    finally
                    {
                        if (file != null)
                            file.Close();
                    }

                }

            }
        }*/




        #endregion


        //SP 9-Aug-2016 TODO - HPC-Wrap region copied from SWMM and bits added for EPANET but think there is nothing special here for dervied class specifics - Consider a better place to store this once confirmed
        #region HPC - WRAP


        //SP 10-Aug-2016 Get full path from environment variables - moved to common
        /*public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return "";
        }*/


        /// <summary>
        /// Creates a general job specification file
        /// Needs to work with any of our run environements
        /// </summary>
        /// <param name="sJobName"></param>
        /// <param name="sEXE"></param>
        /// <param name="sActiveDir"></param>
        /// <param name="sINPFileToTransfer"></param>
        /// <param name="bIsUNC"></param>
        /// <returns></returns>
        private XmlConfigSource CreateHPC_JobSpec(string sJobName, int nScenarioID, string sEXE, string sActiveDir)
        {
            string sXMLPath = Path.Combine(sActiveDir, "job_spec.xml");             // for now, no need to make unique
            XmlConfigSource icsJob = new Nini.Config.XmlConfigSource();
            icsJob.AddConfig("Job");
            icsJob.Configs["Job"].Set("JobName", sJobName);
            icsJob.Configs["Job"].Set("RunCommand", sEXE);
            icsJob.Configs["Job"].Set("BasePath", sActiveDir);
            icsJob.Configs["Job"].Set("ScenarioID", nScenarioID);
            CreateHPC_JobSpecPerformComputeEnvSpecificFunctions(ref icsJob, _dictHPC);
            if (_log._nLogging_ActiveLogLevel >= Logging._nLogging_Level_3)        // save if we are in debug situation
                icsJob.Save(sXMLPath);
            return icsJob;
        }

        /// <summary>
        /// Perform any functions specific to the compute environment itself
        /// plz don't mkae func names this long in the future!
        /// </summary>
        /// <param name="?"></param>
        /// <param name="dictParams"></param>
        private void CreateHPC_JobSpecPerformComputeEnvSpecificFunctions(ref XmlConfigSource jobConfigReturn, Dictionary<string, string> dictParams)
        {
            switch (_compute_env)
            {
                case ComputeEnvironment.Condor:
                    jobConfigReturn.Configs["Job"].Set("requirements", _hpc._EnvConfig.GetString("CondorRequirements", ""));     // assume this is not varying by scenario - constant
                    if (dictParams.ContainsKey("transfer_input_files"))
                        jobConfigReturn.Configs["Job"].Set("transfer_input_files", dictParams["transfer_input_files"]);         //changes by scenario- set in process scenario
                    if (dictParams.ContainsKey("transfer_output_files"))
                        jobConfigReturn.Configs["Job"].Set("transfer_output_files", dictParams["transfer_output_files"]);         //changes by scenario- set in process scenario
                    // and so on

                    break;
                case ComputeEnvironment.AWS:
                    if (dictParams.ContainsKey("transfer_input_files"))
                        jobConfigReturn.Configs["Job"].Set("transfer_input_files", dictParams["transfer_input_files"]);         //changes by scenario- set in process scenario
                    if (dictParams.ContainsKey("transfer_output_files"))
                        jobConfigReturn.Configs["Job"].Set("transfer_output_files", dictParams["transfer_output_files"]);         //changes by scenario- set in process scenario
                    // and so on
                    break;

                case ComputeEnvironment.LocalViaHPC:

                    break;
            }

        }

        // SimLink must perform part of Cirrus contract to run CirrusHTC 
        // met 7/11/16: this will soon be outdated; retained as reminder of how this was initially performed
        #region CIRRUS

        //todo: consider as override 
        private CIRRUS_HTC_NS.HTC_JOB_SPEC CreateSubmitJob(string sBat, string sActiveDir, string sINPFileToTransfer, bool bIsUNC)
        {
            CIRRUS_HTC_NS.HTC_JOB_SPEC job = new CIRRUS_HTC_NS.HTC_JOB_SPEC();
            job._htcSubmitSpec = new CIRRUS_HTC_NS.HTC_SUBMIT_SPEC();
            job._sActiveHTCDir = sActiveDir;
            job._htcSubmitSpec._sRequirement = "(HasSWMM == true)";
            job._htcSubmitSpec._sExecutable = sBat;
            job._IsUNC = bIsUNC;
            job._htcSubmitSpec._sTransfer_input_files = sINPFileToTransfer;                //todo: this could be a list; separate with commas

            //believe fields below are not used
            //    job._sCUST_ModelRunName = sBat;
            //job._sCUST_Requirements = "(HasSWMM = true)";

            return job;
        }

        #endregion
        #endregion

        #region ReadIndependentDataToHDF

        /*public void ReadCSVDataToHDF5(string sCSV_Location, string sHDFfile = "UNDEFINED")
        {
            DataTable dt = DBContext.GetDataTableFromCsv(sCSV_Location, true);
            foreach (DataRow dr in dt.Rows)
            {
                long lStartIndex = Convert.ToInt64(dr["start_index"].ToString());
                long lEndIndex = Convert.ToInt64(dr["end_index"].ToString());
                string sFile = dr["input_file"].ToString().Replace(".inp", ".out");
                string sEvent = dr["Event"].ToString();
                ReadTimeSeriesFromCSV(-1, -1, sFile, lStartIndex, lEndIndex, sEvent, sHDFfile);
            }
        }*/
            

                /// <summary>
        /// override that allows a direct call to read output if needed.
        /// allows user to override the name of the output file...
        /// </summary>
        /// <param name="nEvalId"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="sOutFile"></param>
        /// <returns></returns>
        /*public override long ReadTimeSeriesFromCSV(int nEvalID, int nScenarioID, string sOutFile="UNDEFINED",long nStartIndex=0,long nEndIndex=-1,string sDatasetLabel="1", string sHDF_Name="UNDEFINED")
        {
            bool bUseSimlinkName = false;

            if (sOutFile == "UNDEFINED")           // if user does not provide filename, then create using standard naming
            {
                bUseSimlinkName = true;
                sOutFile = _sActiveModelLocation.Replace(".inp", ".out");
                sOutFile = CommonUtilities.GetSimLinkFileName(sOutFile, nScenarioID);
            }
            else
            {
                // need to set the active model location...
                _sActiveModelLocation = sOutFile.Replace(".out", ".inp");
            }

            string sRPT = sOutFile.Replace(".out",".rpt");      
            if (!_bTS_IndicesLoaded)            //first time through, load indices
            {                                   //for now we are assuming no inserts- so stay same for each
                Initialize_SWMM_TS_ReportRequest(sRPT);
                _bTS_IndicesLoaded = true;
            }

            if (sHDF_Name != "UNDEFINED")
            {
                _hdf5._sHDF_FileName = sHDF_Name;
            }

            // now set the hdf filename   - for now place in same dir ... 
            SetTS_FileName(nScenarioID, Path.GetDirectoryName(sOutFile));            
            _hdf5.hdfCheckOrCreateH5(_hdf5._sHDF_FileName);
            _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
            long l = ReadOUTData(_nActiveReferenceEvalID, nScenarioID, sOutFile, nStartIndex, nEndIndex, sDatasetLabel);
            _hdf5.hdfClose();
            return l;
        }*/

        #endregion

        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetlinkindex")]
        public extern static int ENgetlinkindex(string sLabel, ref int nCode);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetnodeindex")]
        public extern static int ENgetnodeindex(string sLabel, ref int nCode);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENopen")]
        public extern static int ENopen(string s1, string s2, string s3);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENclose")]
        public extern static int ENclose();
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENopenH")]
        public extern static int ENopenH();
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENcloseH")]
        public extern static int ENcloseH();
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENinitH")]
        public extern static int ENinitH(int StartingTime);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENrunH")]
        public extern static int ENrunH(ref int TimeStep);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetnodevalue")]
        public extern static int ENgetnodevalue(int iIndex, int eCodeForOutput, ref float dVal);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetnodetype")]
        public extern static int ENgetnodetype(int iIndex, ref int nTypeCode);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetnodeid")]
        public extern static int ENgetnodeid(int iIndex, [MarshalAs(UnmanagedType.LPStr), Out]StringBuilder id);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetlinkvalue")]
        public extern static int ENgetlinkvalue(int iIndex, int eCodeForOutput, ref float dVal);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENnextH")]
        public extern static int ENnextH(ref int TimeStep);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgettimeparam")]
        public extern static int ENgettimeparam(int eCodeForOutput, ref int dVal);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENsettimeparam")]
        public extern static int ENsettimeparam(int nCode, int dVal);

        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENaddpattern")]
        public extern static int ENaddpattern(string sPatternName);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetpatternindex")]
        public extern static int ENgetpatternindex(string sPatternName, ref int nIndex);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetpatternvalue")]
        public extern static int ENgetpatternvalue(int nIndex, int nPeriod, ref float dVal);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetpatternlen")]
        public extern static int ENgetpatternlen(int nIndex, ref int dLen);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENsetpattern")]
        public extern static int ENsetpattern(int nIndex, float [] dFactors, int nNumberOfFactors);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENsetpatternvalue")]
        public extern static int ENsetpatternvalue(int nIndex, int nPeriod, float dVal);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENsetnodevalue")]
        public extern static int ENsetnodevalue(int nIndex, int nCode, float dVal);
        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENsaveinpfile")]
        public extern static int ENsaveinpfile(string sFileName);

        [DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENgetcount")]
        public extern static int ENgetcount(int nCode, ref int nCount);

        //[DllImport(@"C:\Program Files (x86)\EPANET2\epanet2.dll", EntryPoint = "ENSolveH")]
        //public extern static long ENSolveH();
    /*    [DllImport("C:\\Program Files\\EPA_SWMM_5022\\swmm5.dll")]
        extern static long swmm_report();
        [DllImport("C:\\Program Files\\EPA_SWMM_5022\\swmm5.dll")]
        extern static long swmm_close();
        [DllImport("C:\\Program Files\\EPA_SWMM_5022\\swmm5.dll")]
        extern static long OpenSwmmOutFile(string sOutFile);
        */
    }
}