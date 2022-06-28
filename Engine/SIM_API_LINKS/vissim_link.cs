using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using Nini.Config;
using System.Text.RegularExpressions;
using VissimWrapper;

namespace SIM_API_LINKS
{
    public enum vissim_SimulationMethod
    {
        API = 1,
        BatchFile = 2
    }

    //SP 22-Jul-2016 Convert from ExtendTimeUnits enum to IntervalType enum
    public static class VissimTimeUnitsExtensions
    {
        public static IntervalType ToIntervalType(this VissimTimeUnits value)
        {
            switch (value)
            {
                case VissimTimeUnits.Seconds:
                    return IntervalType.Second;
                case VissimTimeUnits.Minutes:
                    return IntervalType.Minute;
                case VissimTimeUnits.Hours:
                    return IntervalType.Hour;
                case VissimTimeUnits.Days:
                    return IntervalType.Day;
                case VissimTimeUnits.Months:
                    return IntervalType.Month;
                case VissimTimeUnits.Years:
                    return IntervalType.Year;
                default:
                    return IntervalType.Second;
            }
        }
    }

    public class vissim_link : simlink
    {
        #region MEMBERS

        private const string _RUNVISSIMBAT = "run_vissim.bat";
        private const string _VISSIMCMDEXE = "VISSIM90.exe";
        private vissim_SimulationMethod _smProcessVissimResultsMethod;
        private const vissim_SimulationMethod _smExecuteVissimResultsMethod = vissim_SimulationMethod.API; //currently hardcoded 
        VissimObject VSInstance = new VissimObject();

        #endregion

        #region DICT INFO    

        private const int _nFieldDict_LINK = 1;
        private const int _nFieldDict_NODE = 2;
        private const int _nFieldDict_AREA = 3;

        #endregion

        #region INIT

        public override void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_Vissim;
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
            InitNavigationDict();
        }


        //SP 21-Jun-2017 Created Navigation table for Vissim
        protected override void InitNavigationDict()
        {
            string sSQL = "SELECT tlkpModelAttribute.ID AS VarType_FK, tlkpModelAttributeSection.SectionName, tlkpModelAttribute.FieldName, tlkpModelAttribute.ModelTypeID_FK, tlkpModelAttribute.FieldINP_ColNo, tlkpModelAttribute.FieldINP_RowNo, tlkpModelAttribute.FieldAPI_Code, tlkpModelAttribute.IsResult "
                    + "FROM (tlkpModelAttribute INNER JOIN tlkpModelType ON tlkpModelAttribute.ModelTypeID_FK = tlkpModelType.ModelTypeID) "
                    + "INNER JOIN tlkpModelAttributeSection ON tlkpModelAttributeSection.SectionID = tlkpModelAttribute.SectionID_FK WHERE not tlkpModelAttribute.IsResult AND tlkpModelType.ModelTypeID = " + _nActiveModelTypeID.ToString() + ";";
            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int nVarType_FK = Convert.ToInt32(dr["VarType_FK"].ToString());
                simlinkTableHelper slTH = new simlinkTableHelper(dr, _dbContext.GetTrueBitByContext());
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
                _smProcessVissimResultsMethod = _smExecuteVissimResultsMethod;

                base.LoadReference_EG_Datasets();               //  must follow _dsEG_ResultTS_Request
                InitTS_Vars();                                  //met 11/13/16- include direct call for now.
                SetTSDetails();                                 // load simulation/reporting timesereis information
                LoadAndInitDV_TS();                             //load any reference TS information needed for DV and/or tblElementXREF
                SetTS_FileName(_nActiveBaselineScenarioID);

                //SP 21-Jun-2017 TODO - should be finding the block number of every element once here
                //DetermineElementIndex(ref _dsEG_ResultTS_Request);

                //SP 15-Feb-2017 - Extract External Data at EG level for AUXEG retrieve codes
                EGGetExternalData();

                LoadScenarioDatasets(_nActiveBaselineScenarioID, 100, true);
            }
            catch (Exception ex)
            {
                _logInitEG.AddString(string.Format("Error in initializing EG for Vissim. Error: {0}", ex.Message), Logging._nLogging_Level_1);
            }
            finally
            {
                _logInitEG.WriteLogFile();
            }
        }

        
        private void SetTSDetails(DateTime dtStartSim) //SP 18-Apr-2017 - allow passing in a date
        {
            //get the report interval duration that will be referenced by the TimeStamps
            VSInstance.VISSIM_OpenModel(_sActiveModelLocation);
            VissimTimeUnits vtuTimeUnits = (VissimTimeUnits)Enum.Parse(typeof(VissimTimeUnits), VSInstance.VISSIM_GetRunTimeParameter().ToString());

            //save and close the model
            //VSInstance.VISSIM_CloseModel(); No need to close the model, the next time a model is opened, this model is closed

            DateTime dtRPT = DateTime.Parse("1/1/2000"); //arbitrary
            _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtRPT, vtuTimeUnits.ToIntervalType(), 1);

            //SP 21-Sep-2016 needed for dssUtil
            _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtRPT, vtuTimeUnits.ToIntervalType(), 1);
        }


        private void SetTSDetails() //SP 18-Apr-2017 - allow passing in a date
        {
            SetTSDetails(DateTime.Parse("1/1/1900"));
        }

        //SP 21-Jun-2017 Copied from EPANET may be needed later for real time modeling - TODO
        public override void SetSimTimeSeries(bool bCreateModelChanges, DateTime dtSimStart, DateTime dtSimEnd, int nTS_Interval_Sec, DateTime dtRptStart = default(DateTime), DateTime dtRptEnd = default(DateTime), int nInterval_Sec_rpt = -1)
        {
            base.SetSimTimeSeries(bCreateModelChanges, dtSimStart, dtSimEnd, nTS_Interval_Sec, dtRptStart, dtRptEnd, nInterval_Sec_rpt);
            if (bCreateModelChanges)
            {
                // manually push the pattern start time
                /*InsertModelValList(-1, _nFieldDict_StartDate, _nActiveScenarioID, _tsdSimDetails._dtStartTimestamp.ToString("MM/dd/yyyy"), "", "START_DATE", -1, -1);
                InsertModelValList(-1, _nFieldDict_StartTime, _nActiveScenarioID, _tsdSimDetails._dtStartTimestamp.ToString("HH:mm:ss"), "", "START_TIME", -1, -1);
                InsertModelValList(-1, _nFieldDict_EndDate, _nActiveScenarioID, _tsdSimDetails._dtEndTimestamp.ToString("MM/dd/yyyy"), "", "END_DATE", -1, -1);
                InsertModelValList(-1, _nFieldDict_EndTime, _nActiveScenarioID, _tsdSimDetails._dtEndTimestamp.ToString("HH:mm:ss"), "", "END_TIME", -1, -1);

                InsertModelValList(-1, _nFieldDict_PATTERN_START, _nActiveScenarioID, dtSimStart.ToString("HH:mm"), "", "Pattern Start", -1, -1);
                InsertModelValList(-1, _nFieldDict_StartClockTime_Time, _nActiveScenarioID, dtSimStart.ToString("hh"), "", "Start ClockTime", -1, -1);
                InsertModelValList(-1, _nFieldDict_StartClockTime_AMPM, _nActiveScenarioID, dtSimStart.ToString("tt"), "", "Start ClockTime", -1, -1);
                WriteResultsToDB(_nActiveScenarioID);*/
            }
        }

        //SP 21-Jun-2017 Copied from EPANET may be needed later for real time modeling - TODO - PROBABLY JUST TEMPORARY - ONLY MODIFIED in Element_LABEL = "SCADA"
        /*public override void PreProcessTimeseriesData(TimeSeries.TimeSeriesDetail dtFirstSim, DateTime dtSimStart)
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
        }*/

        private DataSet LoadResultSummaryDS(int nEvalID)
        {
            string sql = "SELECT tblResultVar.Result_ID, tblResultVar.Result_Label, tblResultVar.EvaluationGroup_FK, '' as val, " +
                "tblResultVar.VarResultType_FK, tlkpModelAttributeSection.SectionName, tlkpModelAttribute.FieldName, tblResultVar.ElementID_FK, " +
                "tblResultVar.Element_Label " +
                "FROM ((tblResultVar INNER JOIN tlkpModelAttribute ON tblResultVar.VarResultType_FK = tlkpModelAttribute.ID) " +
                "inner join tlkpModelAttributeSection ON tlkpModelAttributeSection.SectionID = tlkpModelAttribute.SectionID_FK) INNER JOIN tlkpModelType ON tlkpModelType.ModelTypeID = tlkpModelAttribute.ModelTypeID_FK " +
                "WHERE (((EvaluationGroup_FK)=" + nEvalID + ") AND tlkpModelAttribute.IsResult AND tlkpModelType.ModelTypeName = 'Vissim') ORDER BY Element_Label;";

            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        public override double[,] GetNetworkTS_Data(int nElementID, int nVarType_FK, string sElementLabel = "NOTHING", string sFileLocation = "NOTHING")
        {
            //todo
            return null;
        }


        // met 12/27/16: modified base and so added datetime here
        public override string[] DSS_GetParts(DataRow dr, int nScenarioID, string sDateDSS_Format, string sIntervalDSS_Format, DateTime dtStart, DateTime dtEnd, string sScenarioLabel = "")
        {
            string[] sParts = new string[6];
            sParts[0] = dr["Result_Label"].ToString();
            sParts[1] = dr["Element_Label"].ToString();     //b part
            sParts[2] = dr["FieldName"].ToString();
            sParts[3] = sDateDSS_Format;
            sParts[4] = sIntervalDSS_Format;
            sParts[5] = nScenarioID.ToString() + " " + sScenarioLabel;
            return sParts;
        }

        #endregion


        #region RunProcessing

        public override int ProcessScenarioWRAP(string sDNA, int nScenarioID, int nScenStartAct, int nScenEndAct, bool bCreateIntDNA = true)
        {
            if (bCreateIntDNA)
                sDNA = ConvertDNAtoInt(sDNA);       //round any doubles to int

            int nReturn = ProcessScenario(_nActiveProjID, _nActiveEvalID, _nActiveReferenceEvalID, _sActiveModelLocation, nScenarioID, nScenStartAct, nScenEndAct, sDNA);
            return nReturn;
        }

        public override int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINPX_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1", string sLabel = "DEFAULT")
        {
            string sPath = ""; string sTargetPath = ""; string sTarget_INPX = ""; string sIncomingINPX = ""; string sTarget_INPX_FileName; string sINIFile; string sSummaryFile = ""; string sOUT = "";
            int nCurrentLoc = nScenStartAct; string sTS_Filename = "";

            if (_scenDeleteSpec == DeleteScenDetails.BeforeRun)
                ScenDS_ClearAfterScenario(nScenarioID); //SP 18-Jul-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

            if (true)     //we should have a valid ScenarioID at this point.
            {
                try
                {
                    if (_bIsOptimization || (nScenarioID == -1))           //nScenarioID  = -1
                    {
                        //SP 10-Jun-2016 TODO Ideally want to shift writing / reading from the database out of this routine - increased precision of datetime as there are now more than one able to be processed per second
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

                        //SP 8-Jul-2017 this may not be needed if we are calling SetupScenarioFiles in the lines below - consider removing
                        ScenarioPrepareFilenames(nScenarioID, nEvalID, sINPX_File, out sTargetPath, out sIncomingINPX, out sTarget_INPX, out sTS_Filename);

                        //if ((nScenarioID != _nActiveBaselineScenarioID) && (nScenarioID != -1))        //met 7/3/14: for now, don't load if optimization... todo; consider appropriate loading if seeeding (probably not worth the effort) 
                        //SP 14-Jun-2016 - even if Optimization, ScenarioID would be set here so original comment from 7/3/14 no longer holds anyway
                        LoadScenarioDatasets(nScenarioID, nScenStartAct, nScenarioID == _nActiveBaselineScenarioID);                       //, sTS_Filename);           //loads datasets needed for the scenario if not starting from beginning (in which case ds are constructed through process);

                        sPath = System.IO.Path.GetDirectoryName(sINPX_File);
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
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineFileSetup) && (nScenEndAct >= CommonUtilities.nScenLCBaselineFileSetup))
                    {
                        _log.AddString("Vissim File Setup Begin: ", Logging._nLogging_Level_1);      //log begin scenario step
                        //SP 8-Jul-2017 try using SetupScenarioFiles new common function within Simlink class
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        CommonUtilities.CopyDirectoryAndSubfolders(sPath, sTargetPath, true); 

  //                      SetupScenarioFiles(sIncomingINPX, nScenarioID);     // bojangles- need the four arg version..., out sTargetPath, out sTarget_INPX);

                        SetupScenarioFiles(sIncomingINPX, nScenarioID, out sTargetPath, out sTarget_INPX);
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
                        _log.AddString("VISSIM File Update Begin: ", Logging._nLogging_Level_1);      //log begin scenario step
                        Update_INPX(sTarget_INPX, nScenarioID);
                        System.IO.File.Delete(sIncomingINPX);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineModified;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        bool bIsUNC = false; string sVISSIM_EXE = ""; string sBAT = "";
                        if (sTarget_INPX.Substring(0, 2) == @"\\") { bIsUNC = true; }         //explicit paths required if UNC
                        if (bIsUNC)
                        {
                            sVISSIM_EXE = _VISSIMCMDEXE + " -q -s \"" + sTarget_INPX + "\""; //SP 12-Oct-2017 //- q Enables the Quick mode during simulation //-s closes the model after simulation
                            sBAT = System.IO.Path.GetDirectoryName(sTarget_INPX) + "\\" + _RUNVISSIMBAT;
                        }
                        else
                        {
                            sVISSIM_EXE = _VISSIMCMDEXE + " -q -s \"" + sTarget_INPX + "\""; //SP 12-Oct-2017 //- q Enables the Quick mode during simulation //-s closes the model after simulation
                            sBAT = _RUNVISSIMBAT;
                        }

                        //create batch file information for running the program
                        string[] s = new string[] { sVISSIM_EXE };
                        string sBat = System.IO.Path.Combine(sTargetPath, _RUNVISSIMBAT);

                        //create the string for the .bat
                        s[0] = "cd %~dp0 \r\n" + s[0];
                        File.WriteAllLines(sBat, s);

                        //SP 8-Aug-16 Copied from SWMM
                        if (_compute_env == ComputeEnvironment.LocalMachine)
                        {
                            //SP 18-Feb-2016 No need to the run the batch file if extracting results using the VISSIM COM methods
                            if (_smExecuteVissimResultsMethod != vissim_SimulationMethod.API)
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
                                _dictHPC["transfer_input_files"] = sBat + "," + sTarget_INPX; //SP 10-Aug-2016 Needs to have full file path for Condor
                            else
                                _dictHPC["transfer_input_files"] = _hpc._EnvConfig.GetString("", "").ToLower();

                            //SP 10-Aug-2016 files required for output
                            if (_hpc._EnvConfig.GetString("transfer_output_files", "").ToLower() == "standard")
                                _dictHPC["transfer_output_files"] = System.IO.Path.GetFileName(sOUT) + "," + System.IO.Path.GetFileName(sSummaryFile); //SP 10-Aug-2016 Needs to have file path removed for Condor
                            else
                                _dictHPC["transfer_output_files"] = _hpc._EnvConfig.GetString("", "").ToLower();

                            // 1: Create the job file
                            XmlConfigSource xmlJobSpec = CreateHPC_JobSpec("vissim_" + nScenarioID, nScenarioID, _RUNVISSIMBAT, sTargetPath); //SP 10-Aug-2016 changed the executable to run_EPANET.bat instead of specifying the EPANET2d.exe
                            // 2: Perform any sim/wrapper/platform specific requirements

                            // 3: Submit the job           
                            _hpc.SubmitJob(xmlJobSpec.Configs["Job"]);

                            //now, tell simlink what you've done....    
                            // 100: 
                            UpdateScenarioStamp(nScenarioID, CommonUtilities.nScenLCModelExecuted);

                            // 200:
                            return CommonUtilities.nScenLCModelResultsRead;
                        }
                    }

                    /*if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        //SP 24-Jul-2017 Read summary data directly after running the simulation and getting the TS results
                        _log.AddString("VISSIM Results Read Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        //ensure the model is open
                        ReadSummaryData(sTarget_INPX, nScenarioID);
                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
                    }*/

                    //SP 9-Oct-2017 Used stage 'nScenLCModelExecuted' of simlink to read timeseries results as occurs in conjunction with runnin model - needs to occur before summary data is read
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted)) 
                    {
                        switch (_smExecuteVissimResultsMethod)
                        {
                            case vissim_SimulationMethod.API:
                                _log.AddString("Reading results using VISSIM API", Logging._nLogging_Level_1, false);
                                long nNumberReportingPeriods = 0;
                                double[][,] _dResultTS_Vals_VISSIMdll = ReadOUTData(sTarget_INPX, ref nNumberReportingPeriods, nScenarioID); //reads both timeseries and summary data

                                //SP 16-Feb-2016 - replace the global array _dResultTS_Vals with the EPANETdll results (15-Feb-2017 - for primary variables only)
                                foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()))
                                {
                                    int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())];
                                    _dResultTS_Vals[nIndex] = _dResultTS_Vals_VISSIMdll[nIndex];
                                }

                                break;

                            default:
                                _log.AddString("Processing VISSIM results method unknown", Logging._nLogging_Level_1);
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

                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted; //SP 9-Oct-2017 Used stage 'nScenLCModelExecuted' of simlink to read timeseries results as occurs in conjunction with runnin model - needs to occur before summary data is read
                    }

                    //read the summary results
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        _log.AddString("VISSIM Read summary results begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        ReadSummaryData(VSInstance, sTarget_INPX, nScenarioID);
                        _log.AddString("VISSIM Read summary results completed: ", Logging._nLogging_Level_2);      //log begin scenario step
                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
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
                    return 0;
                }
            }
        }


        //utility function to set the filenames that are needed
        //todo: 22-Jun-2017 use the SimLink standard functions in CommnUtilities to standardize what is copied across
        private void ScenarioPrepareFilenames(int nScenarioID, int nEvalID, string sINPX_File, out string sTargetPath, out string sIncomingINPX, out string sTarget_INPX, out string sTS_Filename)
        {
            string sPath = System.IO.Path.GetDirectoryName(sINPX_File);
            sTargetPath = sPath.Substring(0, sPath.LastIndexOf("\\")) + "\\" + nEvalID.ToString() + "\\" + nScenarioID.ToString();
            string sTarget_MOX_FileName = System.IO.Path.GetFileNameWithoutExtension(sINPX_File) + "_" + nScenarioID.ToString() + System.IO.Path.GetExtension(sINPX_File);       //append scenario name (good for gathering up the files into a single folder if needed)
            sIncomingINPX = System.IO.Path.Combine(sTargetPath, System.IO.Path.GetFileName(sINPX_File));
            sTarget_INPX = System.IO.Path.Combine(sTargetPath, sTarget_MOX_FileName);
            sTS_Filename = sTargetPath + "\\" + CommonUtilities.GetSimLinkFileName("Vissim_TS.H5", nScenarioID);
        }


        #endregion

        #region MODIFY

        //Update MOX file
        public string Update_INPX(string sINPX_File, int nScenarioID)
        {
            Debug.Print("begin Vissim_Update_INPX");
            if (File.Exists(sINPX_File))
            {
                Debug.Print("INPX Exists");

                //TODO SP 9-Jul-2017 find a way to close the model without closing VISSIM
                //create and open a new instance of the model
                //VissimObject VSInstanceForUpdatingModel = new VissimObject();
                VSInstance.VISSIM_OpenModel(sINPX_File, true);

                try
                {
                    IEnumerable<simLinkModelChangeHelper> ModelChangesIEnum = Updates_GetChangeDS(nScenarioID);       //bojangles...  needs to be from mem!!!

                    foreach (simLinkModelChangeHelper slmCurrent in ModelChangesIEnum)
                    {
                        Debug.Print("Begin: " + slmCurrent._sElementName);
                        try
                        {
                            //find type of element from SectionName
                            VissimElements vsElement = VISSIMHelper_GetVissimObject(slmCurrent._sSectionName);
                            if (vsElement == VissimElements.VAP)
                            {
                                //SP 2-Oct-2017 an element in section type VAP is a text file and cannot be modified through COM - therefore special case of modifying the text file
                                string sVAPFileFullPath = Path.GetDirectoryName(sINPX_File) + "\\" + slmCurrent._sElementName;
                                Update_VAP(sVAPFileFullPath, nScenarioID, "CONST", slmCurrent._sFieldName, slmCurrent._sVal, slmCurrent._nColumnNo, slmCurrent._nRowNo);
                            }
                            else
                            {
                                VSInstance.VISSIM_PokeVal(slmCurrent._sElementName, slmCurrent._sFieldName, slmCurrent._sVal, vsElement);
                            }
                        }
                        catch (Exception ex)
                        {
                            string sMsg = "Error writing result to Vissim Model " + sINPX_File + " msg: " + ex.Message;
                            _log.AddString(sMsg, Logging._nLogging_Level_3, false);
                            Console.WriteLine(sMsg);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error with modification of Vissim model. " + ex.Message);
                    return "didn't work";
                }

                //save new updated model
                VSInstance.VISSIM_SaveModel(sINPX_File);
                //VSInstance.VISSIM_CloseModel(); //No need to close the whole application, when a new model is opened it will replace the currently opened model
                return "this worked";
            }
            else
            {
                return "didn't work";
            }
        }

        //SP 8-Mar-2016 - Additional EPANET required changes based on a DV value - Modify a model change value based on a DV value
        protected override void ModifyModelChanges_SpecialCase
            (int DV_ID_FK, int TableFieldKey, int ScenID, ref string val, string note, string ElName, int ElId, int nDV_Option)
        {


        }

        //SP 8-Mar-2016 - Additional EPANET required changes based on a DV value - create a new model change based on a DV value
        protected override void AdditionalRequiredModelChanges_SpecialCase(int DV_ID_FK, int TableFieldKey, int ScenID, string val, string note,
            string ElName, int ElId, int nDV_Option)
        {

        }

        //SP 6-Oct-2017 Copied from SWMM
        public string Update_VAP(string sVAP_File, int nScenarioID, string sVAPSectionName, string sVAPFieldName, string sVAPValue, int sVAPColumnNumber, int sVAPRowNumber, string sOptionalOutput_TextFile = "nothing")
        {
            Debug.Print("begin VISSIM_Update_VAP");
            if (File.Exists(sVAP_File))
            {
                Debug.Print("VAP Exists");
                StreamReader fileMEX = null;
                try
                {
                    string[] sTextFile_ALL = File.ReadAllLines(sVAP_File);

                    List<string> listTextFile_ALL = new List<string>();

                    //initialize variables for each loop
                    string sCurrentSectionName = "none";
                    int nCurrentWriteLine = 0;
                    int nFileTotalRows; int nCurrentChange = 0;
                    int nSectionBeginLine = 0;
                    nFileTotalRows = sTextFile_ALL.Length;

                    if (sTextFile_ALL[nCurrentWriteLine].IndexOf(" ") > 0)                   //check whether we have data row
                    {
                        string sIDName = "";

                        //test for whether we are using a "system/scenario specific var (eg option table)
                        //or (more standard) an element name.
                        string sFindElementNameOrField = sVAPFieldName;

                        sIDName = UpdateHelper_AdvanceToCurrent_ID(ref sTextFile_ALL, sFindElementNameOrField, ref nCurrentWriteLine, nSectionBeginLine, sCurrentSectionName); //BYM
                        Debug.Print("Found CurrentID: " + nCurrentChange);
                        if (sIDName != "No_ID_Found")
                        {
                            string[] sbufDATA =  sTextFile_ALL[nCurrentWriteLine].Trim().Replace(",", "|,").Split(new Char[] { ' ', '|' });             

                            int nLastRow = 1;                                                                      //met 6/18/2012: modified to support multiple row values. Val of 1 is the default.
                            int nCurrentRow = 1;
                            //met 1/4/2013: modified to check thta the next ID has the right type.
                            if (sIDName == sVAPFieldName)
                            {
                                bool bCorrectRow = true;
                                if (bCorrectRow)
                                {
                                    nCurrentRow = sVAPRowNumber;
                                    if (nCurrentRow > nLastRow)                                                         // if this is true, we need to get the new row.
                                    {
                                        nCurrentWriteLine += nCurrentRow - nLastRow;
                                        sbufDATA = CommonUtilities.RemoveRepeatingChar(sTextFile_ALL[nCurrentWriteLine]).Split(' ');                      //remove the repeating spaces so the split works (occurs in HELPER_SWMM_AdvanceToCurrent_ID for normal workflow)
                                    }

                                    int index = sVAPColumnNumber;
                                    sbufDATA[index - 1] = sVAPValue;
                                }


                            }
                            sTextFile_ALL[nCurrentWriteLine] = String.Join(" ", sbufDATA);
                        }
                        else
                        {
                            nCurrentChange++;  //nothing found- move on to the next change.  TODO L log this change
                        }
                    }
                    else
                    {
                        nCurrentWriteLine++;
                    }


                    string sOUT;
                    if (sOptionalOutput_TextFile == "nothing")
                        sOUT = sVAP_File;
                    else
                        sOUT = sOptionalOutput_TextFile;

                    Debug.Print("Write: " + sOUT);
                    File.WriteAllLines(sOUT, sTextFile_ALL);              //overwrite the file initially passed

                }
                catch (Exception ex)
                {
                    //      /   _log.AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                    int n = 1;
                }
                finally
                {
                    if (fileMEX != null)
                        fileMEX.Close();
                }
            }

            return "crap";
        }

        //SP 6-Oct-2017 Copied from SWMM
        public string UpdateHelper_AdvanceToCurrent_ID(ref string[] sTEXT_File, string sFindID, ref int nCurrentFilePosition, int nSectionStartIndex, string sSectionName)
        {
            int nStartingPosition = nCurrentFilePosition; string sbuf; int nID_Index = 0;
            string sReturn = "No_ID_Found"; bool bFound = false; bool bNewSection = false;
            while ((nCurrentFilePosition < sTEXT_File.Length) && (!bFound))
            {
                nID_Index = 0;        //typical case

                sbuf = UpdateHelper_GetIDFromDataRow(sTEXT_File[nCurrentFilePosition].ToString(), nID_Index, sSectionName);
                if (sbuf.Trim().Length == 0)        //met 7/9/14- subsequent call erroring out due to no len. test
                {
                    nCurrentFilePosition++; //increment, but not necessarily new section.
                }
                else
                {
                    if (sbuf == sFindID)
                    {
                        sReturn = sFindID;          //we have found the ID exit loop
                        sTEXT_File[nCurrentFilePosition] = CommonUtilities.RemoveRepeatingChar(sTEXT_File[nCurrentFilePosition]);
                        bFound = true;
                    }
                    else
                    {
                        nCurrentFilePosition++;
                    }
                }

            }
            if (!bFound)
            {
                return "";
            }
            return sReturn;
        }

        //SP 6-Oct-2017 Copied from SWMM
        public string UpdateHelper_GetIDFromDataRow(string sbuf, int nID_Column = 0, string sSectionName = "NOTHING")
        {
            sbuf = sbuf.Trim();     

            int nSpaceIndex = -1;
            if (nID_Column == -1) 
            {
                return CleanIDField(sbuf);
            }

            for (int i = 0; i < nID_Column; i++)
            {
                nSpaceIndex = sbuf.IndexOf(" ");
                if (nSpaceIndex > 0)
                {
                    sbuf = sbuf.Substring(nSpaceIndex + 1, sbuf.Length - nSpaceIndex - 1);
                }
                else
                {
                    return "No_ID_Found";
                }
            }

            nSpaceIndex = sbuf.IndexOf(" ");
            if (nSpaceIndex >= 0)
            {
                return CleanIDField(sbuf.Substring(0, nSpaceIndex));
            }
            else
            {               //no comma found, this is not a typical data row
                return "No_ID_Found";
            }
        }

        private string CleanIDField(string sID)
        {
            string sReturn = sID.Substring(sID.IndexOf(' ') + 1, sID.Length - sID.IndexOf(' ') - 1).Trim();
            return CleanMEXString(sReturn);
        }

        private string CleanMEXString(string sbuf)
        {
            char[] nogood = { ' ' };
            return sbuf.TrimStart(nogood).TrimEnd(nogood);
        }

        #region UpdateHelpers

        //for Vissim read model changes table
        private IEnumerable<simLinkModelChangeHelper> Updates_GetChangeDS(int nScenarioID)
        {
            IEnumerable<simLinkModelChangeHelper> ModelChangesList = from ModelChanges in _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.MEV)
                                                                   .Where(x => x._nScenarioID == nScenarioID).AsEnumerable()               //which performance to characterize
                                                                     join VISSIM_StructureDict in _dictSL_TableNavigation.AsEnumerable()
                                                                     on ModelChanges._nVarType_FK equals VISSIM_StructureDict.Key
                                                                     orderby ModelChanges._nElementID
                                                                     orderby ModelChanges._nRecordID
                                                                     select new simLinkModelChangeHelper
                                                                     {
                                                                         _sVal = ModelChanges._sVal,
                                                                         _sElementName = ModelChanges._sElementName,
                                                                         _nElementID = ModelChanges._nElementID,
                                                                         _nRecordID = ModelChanges._nRecordID,
                                                                         _sSectionName = VISSIM_StructureDict.Value._sSectionName,
                                                                         _sFieldName = VISSIM_StructureDict.Value._sFieldName,
                                                                         _nVarType_FK = VISSIM_StructureDict.Value._nVarType_FK,
                                                                         _nRowNo = VISSIM_StructureDict.Value._nRowNo,
                                                                         _nColumnNo = VISSIM_StructureDict.Value._nColumnNo,
                                                                         _sQualifier1 = "-1",                  //todo : figure out how the qulifier info is obtained (or if there is a better way to do this)
                                                                         _nQual1Pos = -1
                                                                     };
            return ModelChangesList.Cast<simLinkModelChangeHelper>();
        }






        #endregion



        #endregion

        #region OUT

        //SP 21-Jul-2016 Modified this from EPANET to extract the Vissim Data
        private double[][,] ReadOUTData(string sTarget_INPX, ref long nNumberReportingPeriods, int nScenarioID)
        {
            //initiate and open the inpx file
            int nTimeStep = 0;
            _log.AddString(string.Format("  opening Vissim model {0}", sTarget_INPX), Logging._nLogging_Level_2);

            // open the model
            VSInstance.VISSIM_OpenModel(sTarget_INPX, true);

            _log.AddString("  Reading run time parameters", Logging._nLogging_Level_1, false);

            //clear all data relating to existing simulations
            VSInstance.VISSIM_ClearAllExistingSimulations();

            int nDurationInSeconds = 0;
            nDurationInSeconds = VSInstance.VISSIM_GetRunTimeParameter();

            //initial pre-loaded value is the reporting step 
            int nReportingTimeStep = 0;
            nReportingTimeStep = Math.Max(VSInstance.VISSIM_GetReportingStep(), 900);

            nNumberReportingPeriods = nDurationInSeconds / nReportingTimeStep;

            _log.AddString(string.Format("  Finished reading run time parameters. Duration = {0}, ReportStep = {1}, NumberPeriods = {2}",
                nDurationInSeconds, nReportingTimeStep, nNumberReportingPeriods), Logging._nLogging_Level_2);

            //retrieving the output results by stepping through to next breakpoint - SP 22-Jun-17
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
            }


            int nTimeStepCounter = 0;
            nTimeStep = nReportingTimeStep;
            do
            {
                VSInstance.VISSIM_RunSimulationToNextBreakPoint(nTimeStep);

                //SP 04-Apr-2016 - correction to ensure only reporting steps are recorded in TS memory arrays
                //for each time step - if it's a reporting time step, get the required node or link value
                if (nTimeStep % nReportingTimeStep == 0 && nTimeStep > 0)
                {
                    _log.AddString(string.Format("  Reading results for timestep = {0}", nTimeStep), Logging._nLogging_Level_2, false);

                    foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString())) //SP 15-Feb-2017 only want to retrieve primary TS
                    {
                        string sFieldName = dr["FieldName"].ToString();
                        string sSectionName = dr["SectionName"].ToString();
                        int nBeginPeriodNo = Convert.ToInt32(dr["BeginPeriodNo"]); //SP 1-Dec-2016 Incorporating TS BeginPeriodNo
                        int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())]; //SP 15-Feb-2017 Get index for storing TS in memory for the dr from dictionary

                        string sElementLabel = dr["Element_Label"].ToString();
                        VissimElements vsElementType = VISSIMHelper_GetVissimObject(sSectionName);

                        object oAttributeValueNode = VSInstance.VISSIM_Request(sElementLabel, sFieldName, vsElementType);
                        dreturn[nIndex][nTimeStepCounter - (nBeginPeriodNo - 1), 0] = Convert.ToDouble(oAttributeValueNode);
                        _log.AddString(string.Format("    {0}: ResultsTSLabel {1}, FieldName {2}, Value {3}", Enum.GetName(typeof(VissimElements), (int)vsElementType), 
                            dr["Result_Label"].ToString(), sFieldName, oAttributeValueNode), Logging._nLogging_Level_3, false);
                        
                    }

                    nTimeStepCounter++;
                }

                //continue to next time step
                nTimeStep += nReportingTimeStep;
            } while (nTimeStep > 0 && nTimeStepCounter < nNumberReportingPeriods);

            //save the model with simulation results
            VSInstance.VISSIM_SaveModel(sTarget_INPX);

            _log.AddString(string.Format("  closing Vissim model {0}", sTarget_INPX), Logging._nLogging_Level_2);
            //VSInstance.VISSIM_CloseModel(); //No need to close the whole application, when a new model is opened it will replace the currently opened model

            return dreturn;
        }

        //SP June 2017
        public void ReadSummaryData(VissimObject VS, string sTarget_INPX, int nScenarioID)
        {
            _log.AddString(string.Format("  opening Vissim model {0} to read Summary Data", sTarget_INPX), Logging._nLogging_Level_2);
            // open the model
            VSInstance.VISSIM_OpenModel(sTarget_INPX, true);

            try
            {
                int nResults = _dsEG_ResultSummary_Request.Tables[0].Rows.Count;

                string[] sVals = new string[nResults];
                int[] nIDs = new int[nResults];
                string sCurrentElementName;
                int nDataVals = 0;

                foreach (DataRow dr in _dsEG_ResultSummary_Request.Tables[0].Rows)
                {
                    //encapsulate results read in try/catch to help pinpoint errors 
                    string sLogIdentifier = "";
                    try
                    {
                        string sFieldName = dr["FieldName"].ToString();
                        string sSectionName = dr["SectionName"].ToString();
                        nIDs[nDataVals] = Convert.ToInt32(dr["Result_ID"].ToString());
                        string sElementLabel = dr["Element_Label"].ToString();
                        VissimElements vsElementType = VISSIMHelper_GetVissimObject(sSectionName);

                        object oAttributeValueNode = VS.VISSIM_Request(sElementLabel, sFieldName, vsElementType);
                        sVals[nDataVals] = oAttributeValueNode.ToString();
                        _log.AddString(string.Format("    {0}: ResultsSummary {1}, FieldName {2}, Value {3}", Enum.GetName(typeof(VissimElements), (int)vsElementType),
                            dr["Result_Label"].ToString(), sFieldName, oAttributeValueNode), Logging._nLogging_Level_3, false);

                        //get a label name to be stored with the summary data
                        sCurrentElementName = sSectionName + ": " + sElementLabel + " " + string.Join(",", sFieldName);

                        //add the Summary Result vals to the DS
                        ResultSummaryHelper_AddValToDS(nScenarioID, Convert.ToInt32(dr["Result_ID"].ToString()), nIDs[nDataVals], sCurrentElementName, Convert.ToDouble(sVals[nDataVals]), -1);

                        nDataVals++;
                    }
                    catch (Exception ex)
                    {
                        string sMsg = "Error reading result: " + sLogIdentifier + " msg: " + ex.Message;
                        _log.AddString(sMsg, Logging._nLogging_Level_3, false);
                        Console.WriteLine(sMsg);
                    }
                }

                _log.AddString(string.Format("  Finished reading summary data"), Logging._nLogging_Level_2);

            }
            catch (Exception ex)
            {
                _log.AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
            }
        }

        //SP 28-Feb-2017 Modified to keep consistent with Secondary and AUX requests. SQL Server struggles with merge if datasets are not consistent
        //SP 21-Apr-2017 adjusted to only retrieve with retrievecode = primary. Previously relying on join with VarTypeResult
        private DataSet ReadOut_GetDataSet(int nEvalId)
        {
            string sqlFD = "SELECT tblResultTS.ResultTS_ID, tblResultTS.Result_Label, tblResultTS.Element_Label, tlkpModelAttributeSection.SectionName, tlkpModelAttribute.FieldName, tblResultTS.BeginPeriodNo,"
                            + " tblResultTS.ElementIndex, tblResultTS.EvaluationGroup_FK, tblResultTS.RetrieveCode, SQN, CustomFunction, FunctionArgs, RefTS_ID_FK, FunctionID_FK, UseQuickParse"
                            + " FROM (((tblResultTS INNER JOIN tlkpModelAttribute ON tblResultTS.VarResultType_FK = tlkpModelAttribute.ID)"
                            + " INNER JOIN tlkpModelType ON tlkpModelType.ModelTypeID = tlkpModelAttribute.ModelTypeID_FK)"
                            + " INNER JOIN tlkpModelAttributeSection ON tlkpModelAttributeSection.SectionID = tlkpModelAttribute.SectionID_FK)"
                            + " LEFT OUTER JOIN tblFunctions ON tblResultTS.FunctionID_FK = tblFunctions.FunctionID"
                            + " WHERE (((EvaluationGroup_FK)=" + nEvalId + ") AND tlkpModelAttribute.IsResult AND tlkpModelType.ModelTypeName = 'VISSIM')";
            DataSet dsFD = _dbContext.getDataSetfromSQL(sqlFD);
            return dsFD;
        }

        VissimElements VISSIMHelper_GetVissimObject(string sSectionName)
        {
            switch (sSectionName.ToUpper())
            {
                case "NODE":
                    return VissimElements.Node;
                case "LINK":
                    return VissimElements.Link;
                case "DESSPEEDDECISION":
                    return VissimElements.DesSpeedDecision;
                default:
                    return (VissimElements)Enum.Parse(typeof(VissimElements), sSectionName);
            }
        }

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
            int nVarType_FK = -1; //_nFieldDict_StartDate;                //todo: better not to hard code to IDs
            int nVarType_FK2 = -1; // _nFieldDict_StartTime;
            if (!bIsStartTime)
            {
                nVarType_FK = -1; // _nFieldDict_EndDate;
                nVarType_FK2 = -1; // _nFieldDict_EndTime;
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


        //standard override
        public override DataSet EGDS_GetResultTS(int nEvalID, bool bIncludeAux = false)
        {
            return ReadOut_GetDataSet(nEvalID);
        }


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

        #endregion
    }
}