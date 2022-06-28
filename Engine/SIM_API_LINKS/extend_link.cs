using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using ExtendSimWrapper;
using Nini.Config;
//using System.Reflection;

namespace SIM_API_LINKS
{
    //SP 25-Jul-2016 moved to ExtendSimInterface Project
    /*public enum ExtendExecuteCommandType
    {
        MenuCommand,
        OpenExtendFile,
        GetSimulationPhase,
        GetRunParameter,
        GetBlockNumber,
        GetDBDBIndex,
        GetDBTableIndex,
        GetDBFieldIndex,
        GetDBRecordIndex,
        SaveModelAs,
        SaveModel,
        RunSimulation
    }
    
    public enum ExtendMenuCommandType
    {
        Close,
        ExitOrQuit
    }

    public enum ExtendPhaseType
    {
        SimStart = 11,
        SimFinish = 12
    }

    public enum ExtendRunParameter
    {
        EndTime = 1,
        StartTime = 2,
        NumberSims = 3,
        NumberSteps = 4,
        TimeUnits = 8
    }

    public enum ExtendTimeUnits
    {
        Generic = 1,
        Milliseconds = 2,
        Seconds = 3, 
        Minutes = 4,
        Hours = 5,
        Days = 6,
        Weeks = 7,
        Months = 8,
        Years = 9
    }*/

    public enum ExtendSim_SimulationMethod
    {
        API = 1,
        BatchFile = 2
    }

    //SP 22-Jul-2016 Convert from ExtendTimeUnits enum to IntervalType enum
    public static class ExtendTimeUnitsExtensions
    {
        public static IntervalType ToIntervalType(this ExtendTimeUnits value)
        {
            switch (value)
            {
                case ExtendTimeUnits.Seconds:
                    return IntervalType.Second;
                case ExtendTimeUnits.Minutes:
                    return IntervalType.Minute;
                case ExtendTimeUnits.Hours:
                    return IntervalType.Hour;
                case ExtendTimeUnits.Days:
                    return IntervalType.Day;
                case ExtendTimeUnits.Months:
                    return IntervalType.Month;
                case ExtendTimeUnits.Years:
                    return IntervalType.Year;
                default:
                    return IntervalType.Second;
            }
        }
    }

    public class extend_link : simlink
    {
        #region DICT INFO    

        private const string _nFieldDict_DATABASE = "DATABASE";            //used when retrieve TS detail (MEV)
        private const string _nFieldDict_BLOCK = "BLOCK";

        #endregion

        #region Simlink

        private const string _RUNEXTENDSIMBAT = "run_ExtendSim.bat";
        private const string _ExtendSimEXE = "ExtendSimWrapper.exe";

        private ExtendSim_SimulationMethod _smProcessExtendSimResultsMethod;
        private const ExtendSim_SimulationMethod smExecuteExtendSimSimulationMethod = ExtendSim_SimulationMethod.API; //currently hardcoded to use API but to be transferred to XML config or database reference to change this

        public override void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_ExtendSim;
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
            InitNavigationDict();
        }

        //SP 22-Jul-2016 Created Navigation table for ExtendSim
        protected override void InitNavigationDict()
        {
            string sSQL = "SELECT tlkpExtend_StructureDictionary.StructureID AS VarType_FK, tlkpExtend_StructureDictionary.StructureName "
                    + "FROM tlkpExtend_StructureDictionary;";
            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int nVarType_FK = Convert.ToInt32(dr["VarType_FK"].ToString());
                string sStructureName = dr["StructureName"].ToString();
                simlinkTableHelper slTH = new simlinkTableHelper(nVarType_FK, sStructureName); 
                _dictSL_TableNavigation.Add(nVarType_FK, slTH);
            }
        }


        public override void InitializeEG(int nEvalID)
        {
            base.InitializeEG(nEvalID);
            nEvalID = GetReferenceEvalID();                                         //get correct EG for loading datasets
            _dsEG_ResultSummary_Request = LoadResultSummaryDS(nEvalID);
            //_dsEG_ResultTS_Request = ReadOut_GetDataSet(nEvalID); //SP 15-Feb-2017 called in parent routine
            _smProcessExtendSimResultsMethod = smExecuteExtendSimSimulationMethod;

            base.LoadReference_EG_Datasets();               //  must follow _dsEG_ResultTS_Request
            InitTS_Vars();              //met 11/13/16- include direct call for now.
            SetTSDetails();                                 // load simulation/reporting timesereis information
            LoadAndInitDV_TS();                             //load any reference TS information needed for DV and/or tblElementXREF
            SetTS_FileName(_nActiveBaselineScenarioID);
            //SP 18-Nov-2016 - TODO find a better place for this. Moved from LoadReference_EG_Datasets as it's needed after TS is initialised which is called in each derived class
            //EGDS_GetTS_AuxDetails(_nActiveBaselineScenarioID);  // 8/15/14 //SP 15-Feb-2017 AUX details now retrieved when EGDS_GetTS_Details is called
            
            //SP 15-Feb-2017 - Extract External Data at EG level for AUXEG retrieve codes
            EGGetExternalData();

            LoadScenarioDatasets(_nActiveBaselineScenarioID, 100, true);
        }

        //SP 22-Jul-16 Modified from EPANET class
        private void SetTSDetails()
        {
            //get the report interval duration that will be referenced by the TimeStamps
            ExtendSimFunctions.EXTEND_OpenExtendInstance(_sActiveModelLocation);
            ExtendTimeUnits etuTimeUnits = (ExtendTimeUnits)Enum.Parse(typeof(ExtendTimeUnits), ExtendSimFunctions.EXTEND_GetRunTimeParameter(ExtendRunParameter.TimeUnits).ToString());
            
            //save and close the model
            ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.SaveModel });
            //System.Threading.Thread.Sleep(_nSaveWaitTime); //ensure the model saves before closing  - now built into ExtendSimFunctions
            ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.Close });
            //ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.ExitOrQuit });

            DateTime dtRPT = DateTime.Parse("1/1/2000"); //arbitrary
            _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtRPT, etuTimeUnits.ToIntervalType(), 1);

            //SP 21-Sep-2016 needed for dssUtil
            _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtRPT, etuTimeUnits.ToIntervalType(), 1);
        }


        //standard override
        public override DataSet EGDS_GetResultTS(int nEvalID, bool bIncludeAux = false)
        {
            return ReadOut_GetDataSet(nEvalID);
        }


        //SP 21-Jul-2016 Modified to account for the required specification data for ExtendSim Element_Label
        //SP 28-Feb-2017 NEEDS TESTING - Modified to keep consistent with Secondary and AUX requests. SQL Server struggles with merge if datasets are not consistent
        private DataSet ReadOut_GetDataSet(int nEvalId)
        {
            string sqlFD = "SELECT tblResultTS.ResultTS_ID, tblResultTS.Result_Label, tblResultTS.Element_Label, tlkpExtend_StructureDictionary.StructureName, tblResultTS.BeginPeriodNo,"
                            + " tblResultTS.ElementIndex, tblResultTS.EvaluationGroup_FK, tblResultTS.RetrieveCode, SQN, CustomFunction, FunctionArgs, RefTS_ID_FK, FunctionID_FK, UseQuickParse" //SP 28-Feb-2017 no longer a field called IsSecondary - needs to be calculated from RetrieveCode
                            + " FROM (tblResultTS INNER JOIN tlkpExtend_StructureDictionary ON tblResultTS.VarResultType_FK = tlkpExtend_StructureDictionary.StructureID) LEFT OUTER JOIN tblFunctions ON tblResultTS.FunctionID_FK = tblFunctions.FunctionID"
                            + " WHERE (((EvaluationGroup_FK)=" + nEvalId + "))";
            DataSet dsFD = _dbContext.getDataSetfromSQL(sqlFD);
            return dsFD;
        }


        // met 12/27/16: modified base and so added datetime here
        public override string[] DSS_GetParts(DataRow dr, int nScenarioID, string sDateDSS_Format, string sIntervalDSS_Format, DateTime dtStart, DateTime dtEnd, string sScenarioLabel = "")
        {
            string[] sParts = new string[6];
            sParts[0] = dr["Result_Label"].ToString();
            sParts[1] = dr["Element_Label"].ToString();     //b part
            sParts[2] = dr["StructureName"].ToString();
            sParts[3] = sDateDSS_Format;
            sParts[4] = sIntervalDSS_Format;
            sParts[5] = nScenarioID.ToString() + " " + sScenarioLabel;
            return sParts;
        }

        //SP 22-Jun-2016 consider putting this in Simlink.cs class - seems very similar for all classes. MET Completed - now in SimlinkScenario.cs
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
                string sFileLocation = dsEvals.Tables[0].Rows[0]["ModelFile_Location"].ToString();
                int nScenarioID = Convert.ToInt32(dr["ScenarioID"].ToString());
                int nScenStart = Convert.ToInt32(dr["ScenStart"].ToString());
                int nScenEnd = Convert.ToInt32(dr["ScenEnd"].ToString());
                string sDNA = dr["DNA"].ToString();

                try
                {
                    ProcessScenario(nProjID, _nActiveEvalID, _nActiveReferenceEvalID, sFileLocation, nScenarioID, nScenStart, nScenEnd, sDNA);
                }
                catch (Exception ex)
                {
                    //todo: log the issue
                }
            }
        }*/

        private void ScenarioPrepareFilenames(int nScenarioID, int nEvalID, string sMOX_File, out string sTargetPath, out string sIncomingMOX, out string sTarget_MOX, out string sTS_Filename)
        {
            string sPath = System.IO.Path.GetDirectoryName(sMOX_File);
            sTargetPath = sPath.Substring(0, sPath.LastIndexOf("\\")) + "\\" + nEvalID.ToString() + "\\" + nScenarioID.ToString();
            string sTarget_MOX_FileName = System.IO.Path.GetFileNameWithoutExtension(sMOX_File) + "_" + nScenarioID.ToString() + System.IO.Path.GetExtension(sMOX_File);       //append scenario name (good for gathering up the files into a single folder if needed)
            sIncomingMOX = System.IO.Path.Combine(sTargetPath, System.IO.Path.GetFileName(sMOX_File));
            sTarget_MOX = System.IO.Path.Combine(sTargetPath, sTarget_MOX_FileName);
            sTS_Filename = sTargetPath + "\\" + CommonUtilities.GetSimLinkFileName("ExtendSim_TS.H5", nScenarioID);      
        }

        //for ExtendSim just read model changes table
        private IEnumerable<simLinkModelChangeHelper> Updates_GetChangeDS(int nScenarioID)
        {
            IEnumerable<simLinkModelChangeHelper> ModelChangesList = from ModelChanges in _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.MEV)
                                                                   .Where(x => x._nScenarioID == nScenarioID).AsEnumerable()               //which performance to characterize
                                                                   join EXTEND_StructureDict in _dictSL_TableNavigation.AsEnumerable()
                                                                   on ModelChanges._nVarType_FK equals EXTEND_StructureDict.Key
                                                                     orderby ModelChanges._nElementID
                                                                     orderby ModelChanges._nRecordID
                                                                     select new simLinkModelChangeHelper
                                                                     {
                                                                         _sVal = ModelChanges._sVal,
                                                                         _sElementName = ModelChanges._sElementName,
                                                                         _nElementID = ModelChanges._nElementID,
                                                                         _nRecordID = ModelChanges._nRecordID,
                                                                         _sStructureName = EXTEND_StructureDict.Value._sStructureName,
                                                                         _nVarType_FK = EXTEND_StructureDict.Value._nVarType_FK,
                                                                         _sQualifier1 = "-1",                  //todo : figure out how the qulifier info is obtained (or if there is a better way to do this)
                                                                         _nQual1Pos = -1
                                                                     };
            return ModelChangesList.Cast<simLinkModelChangeHelper>();
        }

        //Update MOX file
        public string Update_MOX(string sMOX_File, int nScenarioID, string sOptionalOutput_TextFile = "nothing", bool bCleanTargetOfRepeating = true)
        {
            Debug.Print("begin ExtendSim_Update_MOX");
            if (File.Exists(sMOX_File))
            {
                Debug.Print("MOX Exists");
                try
                {
                    IEnumerable<simLinkModelChangeHelper> ModelChangesIEnum = Updates_GetChangeDS(nScenarioID);       //bojangles...  needs to be from mem!!!

                    //while (nCurrentChange < nTotalChanges)
                    foreach (simLinkModelChangeHelper slmCurrent in ModelChangesIEnum)
                    {
                        //slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                        Debug.Print("Begin: " + slmCurrent._sElementName);
                        try
                        {
                            //decouple the Element Label which contains the parameters for reading with ExtendSim
                            string[] sArrElement_Label = slmCurrent._sElementName.ToString().Split(';').ToArray();
                            //trim all elements of array
                            sArrElement_Label = sArrElement_Label.Select(x => x.Trim()).Where(x => x != "").ToArray();

                            //retrieve all DB values - check if a DB or Block structure
                            string StructureName = slmCurrent._sStructureName.ToUpper();
                            if (StructureName == "DATABASE")
                            {
                                if (sArrElement_Label.Count() == 5)
                                {
                                    //Example of setting a table value
                                    //EXTEND_SetDBData_FromNames("Chevron", "Storage Data", "Scenario Capacity", "ID", "Brininstool", "4000000");
                                    ExtendSimFunctions.EXTEND_SetDBData_FromNames(sArrElement_Label[0], sArrElement_Label[1], sArrElement_Label[2], sArrElement_Label[3], sArrElement_Label[4], slmCurrent._sVal);
                                }
                                else
                                {
                                    _log.AddString(string.Format("Exception: Require 5 arguments. {0} found in string Element_Label for Result_ID {1}",
                                        sArrElement_Label.Count(), slmCurrent._nElementID), Logging._nLogging_Level_1);
                                }
                            }
                            else if (StructureName == "BLOCK")
                            {
                                if (sArrElement_Label.Count() == 3)
                                {
                                    //example of setting a block value
                                    //EXTEND_SetBlockProperty_FromNames("V01-Storage", "Brininstool", "LossPercent", "5");
                                    ExtendSimFunctions.EXTEND_SetBlockProperty_FromNames(sArrElement_Label[0], sArrElement_Label[1], sArrElement_Label[2], slmCurrent._sVal);
                                }
                                else
                                {
                                    _log.AddString(string.Format("Exception: Require 3 arguments. {0} found in string Element_Label for Result_ID {1}",
                                        sArrElement_Label.Count(), slmCurrent._nElementID), Logging._nLogging_Level_1);
                                }
                            }
                            else
                            {
                                //do nothing
                            }
                        }
                        catch (Exception ex)
                        {
                            string sMsg = "Error writing result to ExtendSim Model " + sMOX_File + " msg: " + ex.Message;
                            _log.AddString(sMsg, Logging._nLogging_Level_3, false);
                            Console.WriteLine(sMsg);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error with modification of ExtendSim model. " + ex.Message);
                    return "crap";
                }
                return "this worked";
            }
            else
            {
                return "crap";
            }
        }


        //SP 15-Feb-2017 Use standard Simlink generic function to write time series to repo
        /*private void WriteTimeSeriesToRepo()
        {
            int nPrimaryTSCount = _dsEG_ResultTS_Request.Tables[0].Rows.Count;          //only write out "primary" TS... secondary 
            if (true)
            {
                for (int i = 0; i < nPrimaryTSCount; i++)
                {
                    if (_dResultTS_Vals[i] != null)
                        _hdf5.hdfWriteDataSeries(_dResultTS_Vals[i], _sTS_GroupID[i], "1");
                }
            }
        }*/

        //SP 21-Jul-2016 Modifed to account for the DB structure
        public void ReadSummaryData(string sMOX_File, int nEvalID, int nScenarioID)
        {
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
                        //decouple the Element Label which contains the parameters for reading with ExtendSim
                        string [] sArrElement_Label = dr["Element_Label"].ToString().Split(';').ToArray();
                        //trim all elements of array
                        sArrElement_Label = sArrElement_Label.Select(x => x.Trim()).Where(x => x != "").ToArray();

                        //retrieve all DB values - check if a DB or Block structure
                        string StructureName = dr["StructureName"].ToString().ToUpper();
                        if (StructureName == "DATABASE")
                        {
                            if (sArrElement_Label.Count() == 5)
                            {
                                //Example of reading a table value
                                //var dTimeSeriesValues = EXTEND_GetDBData_FromNames("Chevron", "Storage Volumes", "Hayhurst SE", "Time", "1", "30");
                                sVals[nDataVals] = ExtendSimFunctions.EXTEND_GetDBData_FromNames(sArrElement_Label[0], sArrElement_Label[1], sArrElement_Label[2], sArrElement_Label[3],
                                    sArrElement_Label[4]).ToString();
                            }
                            else
                            {
                                _log.AddString(string.Format("Exception: Require 5 arguments. {0} found in string Element_Label for Result_ID {1}", 
                                    sArrElement_Label.Count(), dr["Result_ID"].ToString()), Logging._nLogging_Level_1);
                            }
                        }
                        else if (StructureName == "BLOCK")
                        {
                            if (sArrElement_Label.Count() == 3)
                            {
                                //example of retrieving a block value
                                //var dBlockProperty = EXTEND_GetBlockProperty_FromNames("V01-Storage", "Brininstool", "Capacity");
                                sVals[nDataVals] = ExtendSimFunctions.EXTEND_GetBlockProperty_FromNames(sArrElement_Label[0], sArrElement_Label[1], sArrElement_Label[2]).ToString();
                            }
                            else
                            {
                                _log.AddString(string.Format("Exception: Require 3 arguments. {0} found in string Element_Label for Result_ID {1}",
                                    sArrElement_Label.Count(), dr["Result_ID"].ToString()), Logging._nLogging_Level_1);
                            }
                        }
                        else
                        {
                            //do nothing
                        }

                        //get a label name to be stored with the summary data
                        sCurrentElementName = StructureName + ": " + string.Join(",", sArrElement_Label);
                        
                        //add the Summary Result vals to the DS
                        ResultSummaryHelper_AddValToDS(nScenarioID, Convert.ToInt32(dr["Result_ID"].ToString()), nIDs[nDataVals], sCurrentElementName, Convert.ToDouble(sVals[nDataVals]),-1); 

                        nDataVals++;
                    }
                    catch (Exception ex)
                    {
                        string sMsg = "Error reading result: " + sLogIdentifier + " msg: " + ex.Message;
                        _log.AddString(sMsg, Logging._nLogging_Level_3, false);
                        Console.WriteLine(sMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
            }
        }

        //SP 21-Jul-2016 Modified this from EPANET to extract the ExtendSim Data
        public void ReadOUTData(int nEvalID, int nScenarioID, int nNumberTimePeriods)
        {
            try
            {
                //int nTS_Records = _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()).Count(); //SP 15-Feb-2017 Primary only
                //string[] sVals = new string[nTS_Records];
                
                // int nCounter = 0; SP 15-Feb-2017 now get the index for each dr in TS dataset
                foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString())) //SP 15-Feb-2017 Require primary only
                {
                    int nBeginPeriodNo = Convert.ToInt32(dr["BeginPeriodNo"]);
                    //SP 1-Dec-2016 calculate array length after knowing period start no
                    double[,] dvals = new double[nNumberTimePeriods - (nBeginPeriodNo - 1), 1];
                    
                    //for testing only
                    string NodeName = dr["Element_Label"].ToString();

                    //encapsulate results read in try/catch to help pinpoint errors 
                    string sLogIdentifier = "";
                    try
                    {
                        //decouple the Element Label which contains the parameters for reading with ExtendSim
                        string[] sArrElement_Label = dr["Element_Label"].ToString().Split(';').ToArray();
                        //trim all elements of array
                        sArrElement_Label = sArrElement_Label.Select(x => x.Trim()).Where(x => x != "").ToArray();

                        //retrieve all DB values - check if a DB or Block structure
                        string StructureName = dr["StructureName"].ToString().ToUpper();
                        string [,] oTimeSeriesData = null;

                        if (StructureName == "DATABASE")
                        {
                            if (sArrElement_Label.Count() == 3)
                            {
                                //Example of reading a table value
                                //var dTimeSeriesValues = EXTEND_GetDBData_FromNames("Chevron", "Storage Volumes", "Hayhurst SE", "Time", "1", "30");
                                //TimeSeries data should return a null
                                oTimeSeriesData = ExtendSimFunctions.EXTEND_GetDBData_FromNames(sArrElement_Label[0], sArrElement_Label[1], sArrElement_Label[2], 1, nNumberTimePeriods);
                            }
                            else
                            {
                                _log.AddString(string.Format("Exception: Require 3 arguments. {0} found in string Element_Label for Result_ID {1}",
                                    sArrElement_Label.Count(), dr["Result_ID"].ToString()), Logging._nLogging_Level_1);
                            }
                        }
                        else if (StructureName == "BLOCK") //no current way of retrieving the TS data without explicitly transferring to a table in ExtendSim
                        {
                            //example of retrieving a block value
                            //var dBlockProperty = EXTEND_GetBlockProperty_FromNames("V01-Storage", "Brininstool", "Capacity");
                        }
                        else
                        {
                            //do nothing
                        }

                        for (int i = 0; i < nNumberTimePeriods; i++)
                        {
                            dvals[i, 0] = double.Parse(oTimeSeriesData[i, 0], System.Globalization.CultureInfo.InvariantCulture); //SP 21-Jul-2016 Assumption for now that only going to obtain a single column and is of numeric type
                        }

                        //_hdf5_ExtendSim must be initialized prior to this function call.
                        string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_ID"].ToString(), "SKIP", "SKIP");
                        int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())]; //SP 15-Feb-2017 get index for saving the TS back
                        _sTS_GroupID[nIndex] = sGroupID; //SP 15-Feb-2017 use nIndex instead of nCounter for saving the TS back - should normally be same. Safer

                        _dResultTS_Vals[nIndex] = dvals;              // add current TS to jagged array //SP 15-Feb-2017 use nIndex instead of nCounter for saving the TS back - should normally be same. Safer
                        //nCounter++; SP 15-Feb-2017 now get the index for each dr in TS dataset
                    }
                    catch (Exception ex)
                    {
                        string sMsg = "Error reading result: " + sLogIdentifier + " msg: " + ex.Message;
                        _log.AddString(sMsg, Logging._nLogging_Level_3, false);
                        Console.WriteLine(sMsg);
                    }
                }
                
            }
            catch (Exception ex)
            {
                _log.AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
            }
        }



        //SP 22-Jun-2016 - original taken from EPANET and modified for ExtendSim
        public override int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sMOXLocation, int nScenarioID = -1, int nScenStartAct = 1,
            int nScenEndAct = 100, string sDNA = "-1", string sLabel = "DEFAULT")
        {
            string sPath = ""; 
            string sTargetPath; 
            string sTarget_MOX; 
            string sIncomingMOX;
            int nCurrentLoc = nScenStartAct; 
            string sTS_Filename = "";

            ScenDS_ClearAfterScenario(nScenarioID); //SP 9-Mar-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

            if (true)     //we should have a valid ScenarioID at this point.
            {
                try
                {

                    if (_bIsOptimization || (nScenarioID == -1))           //nScenarioID  = -1
                    {
                        //SP 10-Jun-2016 TODO Ideally want to shift writing / reading from the database out of this routine - increased precision of datetime as there are not more than one able to be processed per second
                        nScenarioID = InsertScenario(nEvalID, nProjID, System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), "", sDNA);       //pass the current date time to enable easy retrieval of PK (should be better wya to do this)
                        _nActiveScenarioID = nScenarioID;
                    }

                    //SP 15-Feb-2017 ExtractExternalData for RetrieveCode = AUX
                    ScenarioGetExternalData();

                    ScenarioPrepareFilenames(nScenarioID, nEvalID, sMOXLocation, out sTargetPath, out sIncomingMOX, out sTarget_MOX, out sTS_Filename);

                    //if ((nScenarioID != _nActiveBaselineScenarioID) && (nScenarioID != -1))        //met 7/3/14: for now, don't load if optimization... todo; consider appropriate loading if seeeding (probably not worth the effort) 
                    //SP 14-Jun-2016 - even if Optimization, ScenarioID would be set here so original comment from 7/3/14 no longer holds anyway
                    LoadScenarioDatasets(nScenarioID, nScenStartAct, nScenarioID == _nActiveBaselineScenarioID);                       //, sTS_Filename);           //loads datasets needed for the scenario if not starting from beginning (in which case ds are constructed through process);

                    sPath = System.IO.Path.GetDirectoryName(sMOXLocation);
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
                        _log.AddString("ExtendSim File Setup Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        CommonUtilities.CopyDirectoryAndSubfolders(sPath, sTargetPath, true);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;          //
                    }

                    //XMODEL: consider adding handler for this?
                    //lots of potential for complexity in this... first cut keep simple
                    /*if (_slXMODEL != null)
                    {
                        ExecuteLinkedSimLinkPrecursor();        //check and evaluate any linked runs...
                        XMODEL_ProcessLinkedRecords(nScenarioID);                //primary data linkage
                        XMODEL_PlatformSpecificFollowup(nScenarioID);
                        //now, must write out the TS  (do for raingage, ET, LEVEL
                    }*/

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineModified) && (nScenEndAct >= CommonUtilities.nScenLCBaselineModified))
                    {
                        _log.AddString("ExtendSim File Update Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        
                        //if the file exists, then open it, otherwise start from the base file
                        if (!System.IO.File.Exists(sTarget_MOX))
                            ExtendSimFunctions.EXTEND_OpenExtendInstance(sIncomingMOX);
                        else
                            ExtendSimFunctions.EXTEND_OpenExtendInstance(sTarget_MOX);

                        Update_MOX(sIncomingMOX, nScenarioID);
                        //save the model, uncertain which step we are going to start and finish at
                        ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.SaveModelAs, new object[] { sTarget_MOX });
                        //System.Threading.Thread.Sleep(_nSaveWaitTime); //ensure the model saves before closing  - now built into ExtendSimFunctions
                        ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.Close });

                        //close ExtendSim at this point
                        //ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.Close });
                        //ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.ExitOrQuit }); //ensure all models are closed down - especially the existing
                        
                        //System.IO.File.Delete(sIncomingMOX);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineModified;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        //SP 25-Jul-2016 - this will require the path for this ExtendSimInterface.exe to be included in the path variables
                        bool bIsUNC = false; 
                        string sExtendSim_EXE = ""; 
                        string sBAT = "";

                        //SP 25-Jul-2016 - unsure what this is doing
                        if (sTarget_MOX.Substring(0, 2) == @"\\") 
                            { bIsUNC = true; }

                        sExtendSim_EXE = _ExtendSimEXE + @" """ + sTarget_MOX + @"""";
                        if (bIsUNC)
                        {
                            sBAT = System.IO.Path.GetDirectoryName(sTarget_MOX) + "\\" + _RUNEXTENDSIMBAT;
                        }
                        else
                        {
                            sBAT = _RUNEXTENDSIMBAT;
                        }

                        //create batch file information for running the program
                        string[] s = new string[] { sExtendSim_EXE };
                        string sBat = System.IO.Path.Combine(sTargetPath, _RUNEXTENDSIMBAT);
                        
                        //run within SimLink - create the batch file
                        s[0] = "cd %~dp0 \r\n" + s[0];
                        File.WriteAllLines(sBat, s);

                        //SP 31-Aug-16 Copied from EPANET
                        if (_compute_env == ComputeEnvironment.LocalMachine)
                        {
                            //different method if running through the API
                            if (_smProcessExtendSimResultsMethod == ExtendSim_SimulationMethod.BatchFile)
                                CommonUtilities.RunBatchFile(sBat);
                            else if (_smProcessExtendSimResultsMethod == ExtendSim_SimulationMethod.API)
                            {
                                //ensure the model is open
                                ExtendSimFunctions.EXTEND_OpenExtendInstance(sTarget_MOX);

                                //execute the ExtendSim model
                                ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.RunSimulation);
                                //SP 24-Jun-2016 TODO maybe get simulation phase if you can keep passing back control to Simlink - then may be possible to step 1 by 1 getting all values along the way
                                //int nGetPhase = (int)EXTEND_Execute(ExtendExecuteCommandType.GetSimulationPhase);

                                //save the model after simulation have been run, uncertain which step we are going to start and finish at
                                ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.SaveModel);
                                //System.Threading.Thread.Sleep(_nSaveWaitTime); //ensure the model saves before closing - now built into ExtendSimFunctions
                                ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.Close });
                            }
                        }
                        else
                        {
                            /*SP 10-Aug-2016 NOTE CURRENTLY WITH CONDOR THE BASE MODEL FILE LOCATION MUST BE LOCAL OR THE NETWORK MUST BE MAPPED LOCALLY TO AVOID PATH STARTING WITH \\*/

                            //three steps then to execute
                            //05: set sim/wrapper/platform specific requirements
                            //todo: need way to say to transfer ALL (just set to 'all') - where to define this? environment?  depdnent files may be needed..?
                            if (_hpc._EnvConfig.GetString("transfer_input_files", "").ToLower() == "standard")
                                _dictHPC["transfer_input_files"] = sBat + "," + sTarget_MOX + "," + _ExtendSimEXE; //SP 10-Aug-2016 Needs to have full file path for Condor
                            else
                                _dictHPC["transfer_input_files"] = _hpc._EnvConfig.GetString("", "").ToLower();

                            //SP 10-Aug-2016 files required for output
                            if (_hpc._EnvConfig.GetString("transfer_output_files", "").ToLower() == "standard")
                                _dictHPC["transfer_output_files"] = System.IO.Path.GetFileName(sTarget_MOX); //SP 10-Aug-2016 Needs to have file path removed for Condor
                            else
                                _dictHPC["transfer_output_files"] = _hpc._EnvConfig.GetString("", "").ToLower();

                            // 1: Create the job file
                            XmlConfigSource xmlJobSpec = CreateHPC_JobSpec("extendsim_" + nScenarioID, nScenarioID, _RUNEXTENDSIMBAT, sTargetPath); //SP 10-Aug-2016 changed the executable to run_EPANET.bat instead of specifying the EPANET2d.exe
                            // 2: Perform any sim/wrapper/platform specific requirements

                            // 3: Submit the job           
                            _hpc.SubmitJob(xmlJobSpec.Configs["Job"]);

                            //now, tell simlink what you've done....    
                            // 100: 
                            UpdateScenarioStamp(nScenarioID, CommonUtilities.nScenLCModelExecuted);

                            // 200:
                            return CommonUtilities.nScenLCModelResultsRead;
                        }

                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        _log.AddString("ExtendSim Results Read Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        //ensure the model is open
                        ExtendSimFunctions.EXTEND_OpenExtendInstance(sTarget_MOX);
                        
                        ReadSummaryData(sTarget_MOX, nReferenceEvalID, nScenarioID);
                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;

                        ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.SaveModel });
                        //System.Threading.Thread.Sleep(_nSaveWaitTime); //ensure the model saves before closing  - now built into ExtendSimFunctions
                        ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.Close });
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                        //ensure the model is open
                        ExtendSimFunctions.EXTEND_OpenExtendInstance(sTarget_MOX);
                        
                        //get # time steps - this will allow retrieving the time series data from the tables in ExtendSim
                        int nNumberOfTimeSteps = Convert.ToInt32(ExtendSimFunctions.EXTEND_GetRunTimeParameter(ExtendRunParameter.NumberSteps));
                        //TODO the following are not needed but can be incorporated later
                        //int nStartTime = Convert.ToInt32(EXTEND_GetRunTimeParameter(ExtendRunParameter.StartTime));
                        //int nEndTime = Convert.ToInt32(EXTEND_GetRunTimeParameter(ExtendRunParameter.EndTime));
                        //int nNumberSims = Convert.ToInt32(EXTEND_GetRunTimeParameter(ExtendRunParameter.NumberSims));
                        //object sTimeUnits = EXTEND_GetRunTimeParameter(ExtendRunParameter.TimeUnits);
                        
                        ReadOUTData(nEvalID, nScenarioID, nNumberOfTimeSteps);

                        //1/5/15: updated code to read TS above, and then write below if requested
                        if ((_tsRepo == TSRepository.HDF5) && (_IntermediateStorageSpecification._bResultTS))           //at present, this is the only supported TS repo
                        {
                            try
                            {
                                _hdf5 = new hdf5_wrap();
                                _hdf5.hdfCheckOrCreateH5(sTS_Filename);
                                _hdf5.hdfOpen(sTS_Filename, false, true);
                                WriteTimeSeriesToRepo();
                                _hdf5.hdfClose();
                            }
                            catch (Exception ex)
                            {
                                _log.AddString(string.Format("Error writing timeseries to HDF5. Error message returned: {0}", ex.Message), Logging._nLogging_Level_1);
                                throw;
                            }
                        }

                        nCurrentLoc = CommonUtilities.nScenLModelResultsTS_Read;
                        ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.SaveModel });
                        //System.Threading.Thread.Sleep(_nSaveWaitTime); //ensure the model saves before closing  - now built into ExtendSimFunctions
                        ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.Close });
                    }

                    //close model
                    //EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object [] {ExtendMenuCommandType.ExitOrQuit});
                    //ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.SaveModel });
                    //ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.Close });
                    //ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.ExitOrQuit });

                    ProcessScenario_COMMON(nReferenceEvalID, nScenarioID, nCurrentLoc, nScenEndAct, sTS_Filename);        //call base function to perform modeltype independent actions

                    if (_slXMODEL != null)
                    {

                    }

                    UpdateScenarioStamp(nScenarioID, nCurrentLoc);  //store the time the scenario is completed, along with the "stage" of the Life Cycle //SP 10-Jun-2016 now only changes in memory
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
                    if (_bDBWriteOnFail)
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

        #endregion


        //SP 25-Jul-2016 moved to ExtendSimInterface Project
        /*#region ExtendApplicationFunctions

        public object EXTEND_OpenExtendInstance(string sExtendModelPath)
        {
            Guid ExtendCLSID = new Guid("E167B362-7044-11d2-99DE-00C0230406DF");
            Type ExtendType = Type.GetTypeFromCLSID(ExtendCLSID, true);
            object objExtend = null;

            objExtend = EXTEND_getActiveExtendAPP();

            if (objExtend == null)
                objExtend = Activator.CreateInstance(ExtendType);

            string sExtendCommand;
            sExtendCommand = GetExtendCommand(ExtendExecuteCommandType.OpenExtendFile, new object[] {sExtendModelPath});

            object[] paramExtend = new object[1];
            paramExtend[0] = sExtendCommand;

            object sReturn = objExtend.GetType().InvokeMember("Execute", BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty, null, objExtend, paramExtend);
            return sReturn;
        }

        public object EXTEND_getActiveExtendAPP()
        {
            try
            {
                object objExtend = new object();
                objExtend = System.Runtime.InteropServices.Marshal.GetActiveObject("Extend.application");        //get an instance of extend
                return objExtend;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object EXTEND_ReturnExtendInstance(string sExtendPath)
        {
            Guid ExtendCLSID = new Guid("E167B362-7044-11d2-99DE-00C0230406DF");
            Type ExtendType = Type.GetTypeFromCLSID(ExtendCLSID, true);
            object ExtendInstance = Activator.CreateInstance(ExtendType);
            return ExtendInstance;
        }

        #endregion 



        #region HelperFunctionArguments

        //SP 24-Jun-2016 get menu command type
        public int GetMenuCommand (ExtendMenuCommandType eMCT)
        {
            switch (eMCT)
            {
                case ExtendMenuCommandType.ExitOrQuit:
                    return 1;
                case ExtendMenuCommandType.Close:
                    return 4;
                default:
                    return 1; //run simulation
            }
        }

        //SP 24-Jun-2016 get ExtendSim command
        public string GetExtendCommand(ExtendExecuteCommandType eCT, object [] oParams, string tmpStorageLocation = sGlobalStorageLoc0_General)
        {
            switch (eCT)
            {
                case ExtendExecuteCommandType.MenuCommand:
                    return string.Format("ExecuteMenuCommand({0});", GetMenuCommand((ExtendMenuCommandType)oParams[0]));
                case ExtendExecuteCommandType.OpenExtendFile:
                    return string.Format("OpenExtendFile(\"{0}\");", oParams[0].ToString());
                case ExtendExecuteCommandType.SaveModelAs:
                    return string.Format("SaveModelAs(\"{0}\");", oParams[0].ToString());
                case ExtendExecuteCommandType.SaveModel:
                    return string.Format("SaveModel();");
                case ExtendExecuteCommandType.RunSimulation:
                    return string.Format("RunSimulation(False);");
                case ExtendExecuteCommandType.GetSimulationPhase:
                    return string.Format("{0} = GetSimulationPhase();", tmpStorageLocation);
                case ExtendExecuteCommandType.GetRunParameter:
                    return string.Format("{0} = GetRunParameter({1});", tmpStorageLocation, (int)oParams[0]);
                case ExtendExecuteCommandType.GetBlockNumber:
                    return string.Format("integer b, NoBlks; string CurrentBlkName, CurrentBlkLbl; NoBlks= NumBlocks(); for(b=0; b<NoBlks;  b++) " + 
                        "{{CurrentBlkName=BlockName(b); CurrentBlkLbl=GetBlockLabel(b); if(StringCase(CurrentBlkName, true)==StringCase(\"{0}\", true) " + 
                        "and StringCase(CurrentBlkLbl, true)==StringCase(\"{1}\", true)) {{{2}=b; b=NoBlks;}}}}",
                        oParams[0].ToString(), oParams[1].ToString(), tmpStorageLocation);
                case ExtendExecuteCommandType.GetDBDBIndex:
                    return string.Format("{0} = DBDatabaseGetIndex(\"{1}\");", tmpStorageLocation, oParams[0].ToString());
                case ExtendExecuteCommandType.GetDBTableIndex:
                    return string.Format("{0} = DBTableGetIndex({1}, \"{2}\");", tmpStorageLocation, (int)oParams[0], oParams[1].ToString());
                case ExtendExecuteCommandType.GetDBFieldIndex:
                    return string.Format("{0} = DBFieldGetIndex({1}, {2}, \"{3}\");", tmpStorageLocation, (int)oParams[0], (int)oParams[1], oParams[2].ToString());
                case ExtendExecuteCommandType.GetDBRecordIndex:
                    return string.Format("{0} = DBRecordFind({1}, {2}, {3}, 1, True, \"{4}\");", tmpStorageLocation, (int)oParams[0], (int)oParams[1], (int)oParams[2], oParams[3].ToString());
                default:
                    return "";
            }
        }

        
        //Get ItemString for modifying data in DB
        public string EXTEND_getItemString_DB(int nDBIndex, int nTableIndex, int nRecordStart, int nFieldStart, int nRecordEnd = 0, int nFieldEnd = 0)
        {
            if (nRecordEnd == 0)
                nRecordEnd = nRecordStart;

            if (nFieldEnd == 0)
                nFieldEnd = nFieldStart;

            return string.Format("DB:#{0}:{1}:{2}:{3}:{4}:{5}", nDBIndex, nTableIndex, nRecordStart, nFieldStart, nRecordEnd, nFieldEnd);
        }


        //Get ItemString for modifying data in Object
        public string EXTEND_getItemString_Object(string sVariableName, int nBlockNumber, int nRowStart = 0, int nColStart = 0, int nRowEnd = 0, int nColEnd = 0)
        {
            return string.Format("{0}:#{1}:{2}:{3}:{4}:{5}", sVariableName, nBlockNumber, nRowStart, nColStart, nRowEnd, nColEnd);
        }

        #endregion



        #region ExtendSimAsServerAutomationFunctions


        //get the table indexes from the names
        private void EXTEND_GetDBIndexes(string sDBName, string sDBTable, string sDBReturnFieldName, string sDBRecordFieldName, string sDBRecord, string sDBRecordEnd,
            ref int nDBDBIndex, ref int nDBTableIndex, ref int nDBReturnFieldIndex, ref int nDBRecordFieldIndex, ref int nDBRecordStartIndex, ref int nDBRecordEndIndex)
        {
            //this sets Global0 parameter in ExtendSim to be the returned value
            var nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBDBIndex, new object[] { sDBName });
            string sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            nDBDBIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

            //this sets Global0 parameter in ExtendSim to be the returned value
            nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBTableIndex, new object[] { nDBDBIndex, sDBTable });
            sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            nDBTableIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

            //this sets Global0 parameter in ExtendSim to be the returned value
            nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBFieldIndex, new object[] { nDBDBIndex, nDBTableIndex, sDBReturnFieldName });
            sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            nDBReturnFieldIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

            if (sDBRecordFieldName != "")
            {
                //this sets Global0 parameter in ExtendSim to be the returned value
                nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBFieldIndex, new object[] { nDBDBIndex, nDBTableIndex, sDBRecordFieldName });
                sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
                nDBRecordFieldIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

                //this sets Global0 parameter in ExtendSim to be the returned value
                nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBRecordIndex, new object[] { nDBDBIndex, nDBTableIndex, nDBRecordFieldIndex, sDBRecord });
                sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
                nDBRecordStartIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

                //this sets Global0 parameter in ExtendSim to be the returned value
                nDBRecordEndIndex = nDBRecordStartIndex;
                if (sDBRecordEnd != "")
                {
                    nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBRecordIndex, new object[] { nDBDBIndex, nDBTableIndex, nDBRecordFieldIndex, sDBRecordEnd });
                    sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
                    nDBRecordEndIndex = Convert.ToInt32(EXTEND_Request("System", sItem));
                }
            }
            else
            {
                //don't modify the nRecordStartIndex nor the nRecordEndIndex as they should be dictated by time periods in simulation and not specified by user
            }
        }


        public object EXTEND_GetDBData_FromNames(string sDBName, string sDBTable, string sDBReturnFieldName, string sDBRecordFieldName, string sDBRecord, string sDBRecordEnd = "")
        {
            //if don't want an array, sRecordEnd will be set to the start record # in EXTEND_getItemString_DB
            
            int nDBDBIndex = -1;
            int nDBTableIndex = -1;
            int nRecordFieldIndex = -1; //not used in the request or poke - just used to find the start and end rows
            int nReturnFieldIndex = -1;
            int nRecordStartIndex = -1;
            int nDBRecordEndIndex = -1;

            EXTEND_GetDBIndexes(sDBName, sDBTable, sDBReturnFieldName, sDBRecordFieldName, sDBRecord, sDBRecordEnd,
                ref nDBDBIndex, ref nDBTableIndex, ref nReturnFieldIndex, ref nRecordFieldIndex, ref nRecordStartIndex, ref nDBRecordEndIndex);

            //return a string to be used with 'request' to obtain the required data from the tables in ExtendSim
            string sDBRequest = EXTEND_getItemString_DB(nDBDBIndex, nDBTableIndex, nRecordStartIndex, nReturnFieldIndex, nDBRecordEndIndex);
            return EXTEND_Request("System", sDBRequest);
        }

        //for time series, just get the start row and end row
        public string[,] EXTEND_GetDBData_FromNames(string sDBName, string sDBTable, string sDBReturnFieldName, int nStartTimeRow, int nEndTimeRow)
        {
            //if don't want an array, sRecordEnd will be set to the start record # in EXTEND_getItemString_DB
            int nDBDBIndex = -1;
            int nDBTableIndex = -1;
            int nRecordFieldIndex = -1; //not used in the request or poke - just used to find the start and end rows
            int nReturnFieldIndex = -1;
            int nRecordStartIndex = nStartTimeRow;
            int nDBRecordEndIndex = nEndTimeRow;

            EXTEND_GetDBIndexes(sDBName, sDBTable, sDBReturnFieldName, "", "", "",
                ref nDBDBIndex, ref nDBTableIndex, ref nReturnFieldIndex, ref nRecordFieldIndex, ref nRecordStartIndex, ref nDBRecordEndIndex);

            //return a string to be used with 'request' to obtain the required data from the tables in ExtendSim
            string sDBRequest = EXTEND_getItemString_DB(nDBDBIndex, nDBTableIndex, nRecordStartIndex, nReturnFieldIndex, nDBRecordEndIndex);
            return (string[,])EXTEND_Request("System", sDBRequest);
        }


        //set a table value through names
        public void EXTEND_SetDBData_FromNames(string sDBName, string sDBTable, string sDBReturnFieldName, string sDBRecordFieldName, string sDBRecord, string sNewTableValue)
        {
            //if don't want an array, sRecordEnd will be set to the start record # in EXTEND_getItemString_DB

            int nDBDBIndex = -1;
            int nDBTableIndex = -1;
            int nRecordFieldIndex = -1;
            int nReturnFieldIndex = -1;
            int nRecordStartIndex = -1;
            int nDBRecordEndIndex = -1;

            //for poking, only allow one record to be modified at once - record end will be set to record start
            string sDBRecordEnd = "";

            EXTEND_GetDBIndexes(sDBName, sDBTable, sDBReturnFieldName, sDBRecordFieldName, sDBRecord, sDBRecordEnd,
                ref nDBDBIndex, ref nDBTableIndex, ref nReturnFieldIndex, ref nRecordFieldIndex, ref nRecordStartIndex, ref nDBRecordEndIndex);

            //return a string to be used with 'poke' to obtain the required data from the tables in ExtendSim - simple, keep to single values for now
            string sDBPoke = EXTEND_getItemString_DB(nDBDBIndex, nDBTableIndex, nRecordStartIndex, nReturnFieldIndex);
            EXTEND_PokeVal("System", sNewTableValue, sDBPoke);
        }


        //get Item string for Objects for Request and Pokes from Names
        private string EXTEND_getItemString_Object_FromNames(string sBlockName, string sBlockLabel, string sBlockProperty)
        {
            int nBlockNumber = EXTEND_GetBlockNumber(sBlockName, sBlockLabel);
            //get the request string
            return EXTEND_getItemString_Object(sBlockProperty, nBlockNumber);
        }

        //Get the property value of a block from names
        public object EXTEND_GetBlockProperty_FromNames(string sBlockName, string sBlockLabel, string sBlockProperty)
        {
            string sItem = EXTEND_getItemString_Object_FromNames(sBlockName, sBlockLabel, sBlockProperty);
            return EXTEND_Request("System", sItem); //SP 24-Jun-2016 if this is a DB call the result is likely to be an array - process this array
        }


        //Set the property value of a block from names
        public void EXTEND_SetBlockProperty_FromNames(string sBlockName, string sBlockLabel, string sBlockProperty, string sNewPropertyValue)
        {
            string sItem = EXTEND_getItemString_Object_FromNames(sBlockName, sBlockLabel, sBlockProperty);
            EXTEND_PokeVal("System", sNewPropertyValue, sItem);
        }



        //get the property value of a block
        private int EXTEND_GetBlockNumber(string sBlockName, string sBlockLabel)
        {
            //this sets Global0 parameter in ExtendSim to be the block number of interest
            var nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetBlockNumber, new object [] {sBlockName, sBlockLabel});
            string sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            return Convert.ToInt32(EXTEND_Request("System", sItem));
        }

        public object EXTEND_GetRunTimeParameter(ExtendRunParameter eRTP)
        {
            //this sets Global0 parameter in ExtendSim to be the run parameter of interest
            var nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetRunParameter, new object [] {eRTP});
            string sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            return EXTEND_Request("System", sItem);
        }


        public object EXTEND_Request(string sTopic, string sItem)
        {
            object objExtend = new object();
            objExtend = System.Runtime.InteropServices.Marshal.GetActiveObject("Extend.application");        //get an instance of extend

            object[] paramExtend = new object[2];
            paramExtend[0] = sTopic;
            paramExtend[1] = sItem;
            //object vOutput = new object();
            object sReturn = objExtend.GetType().InvokeMember("Request", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty, null, objExtend, paramExtend);
            return sReturn;

        }


        public object EXTEND_Execute(ExtendExecuteCommandType eCT = ExtendExecuteCommandType.MenuCommand, object[] oParams = null, string tmpStorageLocation = sGlobalStorageLoc0_General)
        {
            object objExtend = new object();
            objExtend = System.Runtime.InteropServices.Marshal.GetActiveObject("Extend.application");        //get an instance of extend

            string sExtendCommand;
            sExtendCommand = GetExtendCommand(eCT, oParams, tmpStorageLocation);

            object[] paramExtend = new object[1];
            paramExtend[0] = sExtendCommand;

            object sReturn = objExtend.GetType().InvokeMember("Execute", BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty, null, objExtend, paramExtend);
            return sReturn;
        }


        public void EXTEND_PokeVal(string sTopic, string sVAL, string sItem)           //      for now just get object within the function, object ExtendApp)
        {
            object objExtend = new object();
            objExtend = System.Runtime.InteropServices.Marshal.GetActiveObject("Extend.application");        //get an instance of extend

            //string sTopic = "System";
            object[] paramExtend = new object[3];
            paramExtend[0] = sTopic;
            paramExtend[1] = sItem;
            paramExtend[2] = sVAL;
            object sReturn = objExtend.GetType().InvokeMember("Poke", BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty, null, objExtend, paramExtend);
        }


        #endregion*/

        #region Results

        //SP 21-Jul-2016 TODO - modify this according to the DB structure for storing the inputs for poking and retrieving DB values for ExtendSim
        //Initially, store the required string in Element_Label
        private DataSet LoadResultSummaryDS(int nEvalID)
        {
            string sql = "SELECT tblResultVar.Result_ID, tblResultVar.Result_Label, tblResultVar.EvaluationGroup_FK, '' as val, " +
                "tblResultVar.VarResultType_FK, tlkpExtend_StructureDictionary.StructureName, tblResultVar.ElementID_FK, " +
                "tblResultVar.Element_Label " +
                "FROM tblResultVar INNER JOIN tlkpExtend_StructureDictionary ON tblResultVar.VarResultType_FK = tlkpExtend_StructureDictionary.StructureID " +
                "WHERE (((EvaluationGroup_FK)=" + nEvalID + ")) ORDER BY Element_Label;";

            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        #endregion

        #region HPC 
        //SP 31-Aug-2016 TODO I think these functions are standard across all classes - perhaps we should put in base class
        
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
