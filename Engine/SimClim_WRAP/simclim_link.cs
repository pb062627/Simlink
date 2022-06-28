using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

//using CLIMsystems.Scenario.Climate.Constraints;
using CLIMsystems.FileFormats.Common;
using CLIMsystems.FileFormats.Legacy.SimCLIM2006;
using CLIMsystems.ScenarioGenerator;
using CLIMsystems.Licensing.Component;
using CLIMsystems.FileFormats.Legacy.Adapters;
using CLIMsystems.Scenario.Climate;
using System.IO;
using CLIMsystems;
using CLIMsystems.FileFormats.Common.EmissionScenarios;
using CLIMsystems.Scenario.Climate.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CLIMsystems.FileFormats.Legacy.SimCLIM2006.Idrisi;
using CLIMsystems.FileFormats.Legacy.Pattern;
using CLIMsystems.Scenario;
using CLIMsystems.Scenario.Data;
using CLIMsystems.FileFormats.Legacy.Idrisi;


//met 12/13/12: 
namespace SIM_API_LINKS
{
    public class simclim_link : simlink
    {

        public static Dictionary<string, string> _dictSC_DomainCodes = new Dictionary<string, string>();        //holds data from tlkpSimClimDictionary for easy access

        public string Delete_This = @"C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\LOGS";
        private string _sSimClimIntermediateCacheLocation = @"C:\Program Files\SimLink\Extensions\SimClim\Data";
        private string _sClimDataLocation = @"C:\Users\Mason\Documents\SimClim\Data";
        private bool _bCacheIntermediateResults;

        public const int _nSimClim_ResultsCodePrecip = 2;
        public const int _nSimClim_ResultsCodePrecipPerturb = 13;

        //data for working with a specific area file
        public bool bAreaLoaded = false;
        public string sSCArea_Path;
        public double _dSCArea_LongMin = -1;
        public double _dSCArea_LongMax = -1;
        public double _dSCArea_LatMin = -1;
        public double _dSCArea_LatMax = -1;
        public double _dSCArea_Resolution = -1;
        public double _dSCArea_FlagValue = -9990.00;

        private int _nMAX_YEAR = 2100;
        private int _nBASE_YEAR = 1990;
        private int _nYearInterval = 5;         // frequency of polling the SimClim API - todo: set through UI
        private int _nLoopsMUST_BE_REPLACED = 3;


        private List<string> _list_Ensemble_GCMs = new List<string>();
        private int _nActiveGCMList = -666;       //the current Ensemble in _EnsembleGCM_List

        #region CLIMSYSTEMS_VARS

        string sRasterProjectArea = "NONE";         // 
        SimCLIMArea _simclimArea;
        //added based upon V02 of the API
        // *new* Ensemble Reader is a new class for generating ensemble values
        EnsemblePatternReader _ensembleReader;

        // *new* Create a data source object which ensemble values will be read from
        IPatternDataSource _ensembleDataSource;
        ScenarioYear _scenYear = ScenarioYear.AR4;      //default  
        ISpatialScenarioGenerator _generator;
        IPatternDataSource _baselineDataSource;
        //api v02  IEnsembleDataSource _patternDataSource;
        CookieIdrisiImage _cookieImage;
        DataVariable _climVar;          // API V02      DataVariable _climVar;
        int _yearSC;
        MonthList _months_SC;
        ScenarioResultType _resultTypeSC;
        Sensitivity _sensitivitySC;
        IEmissionScenarioItem _emissionScenarioSC;
        IScenarioResults _results, _resultsLow = null, _resultsHigh = null;
        List<IPatternInfo> _selectedPatterns;
        #endregion


        string _sMOF_Dir = @"C:\Users\Mason\Documents\Optimization\SimLink\SimClimLink\Data\Emission Scenarios";
        string _sMOF = @"NOT_USED";          //RCP45.mof";      //todo IMPORTANT: this needs to be handled better.
        int? _lowPercentile, _highPercentile;
        int _nActiveScenarioID = -1;                //  MET : a given Scenario has only one perc definition , nActiveScenarioID_Low = -1, nActiveScenarioID_High = -1;
        int _nMidPercentile = -1;

        string _licRegistrant = "tyler.jantzen@ch2m.com";                        // dev only
        string _licKey = "57C2281A9BAC2096C77CE7F4F9A38F55";                     // dev only

        bool bScenarioResultsLoaded = false;                                     // can pull vals if this is true
        bool bSC_ScenarioIsDirty_vs_File = false;                               // unused presently; to help identify when something has chagned vs the version stored on disk

        DateTime _dtEndDateTime;             //used by SimClim to end analysis; todo: optional compare against linked model end time


        //SC Object init and destroy
        #region SC_RMG
        public override void InitializeModelLinkage(string _sCONN, int nDB_Type, bool bSimCondor = false, string sDelimiter = "", int nLogLevel = 1)
        {     //was SimClimLink_Init
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_SimClim;
            base.InitializeModelLinkage(_sCONN, nDB_Type, bSimCondor, sDelimiter, nLogLevel);                       //init underyling simlink object
            if (_dictSC_DomainCodes.Count == 0) { InitSC_DomainDictionary(); }
            SC_InitializeNonClimate(nLogLevel);
        }
        public override void InitializeEG(int nEvalID)
        {
            base.InitializeEG(nEvalID);
            nEvalID = GetReferenceEvalID();                                         //get correct EG for loading datasets
            SetTSDetails();
            _dsEG_ResultTS_Request = LoadTSData_Request(nEvalID);

            int nLoops = CountSimClimYearLoops();
            _dResultTS_Vals = InitTS_InMemArray(nLoops, _dsEG_ResultTS_Request.Tables[0].Rows.Count); //SP 15-Feb-2017 this should be all TS so kept as _dsEG_ResultTS_Request

            base.LoadReference_EG_Datasets();               //  must follow _dsEG_ResultTS_Request
            SetTSREPO_HDF5(true);

            //SP 15-Feb-2017 - Extract External Data at EG level for AUXEG retrieve codes
            EGGetExternalData();

            if (_dsEG_ResultTS_Request.Tables[0].Rows.Count > 0)
            {
                int nScenStart = Convert.ToInt32(_dsEG_ResultTS_Request.Tables[0].Rows[0]["ScenStart"]);
                //int nScenEnd = Convert.ToInt32(_dsEG_ResultTS_Request.Tables[0].Rows[0]["ScenEnd"]);
                LoadScenarioDatasets(-1, nScenStart);         //simclim loads all scenarios within an EG at once.
            }



        }


        private void SetTSDetails()
        {

            string sSQL = "SELECT TS_StartDate, TS_EndDate, TS_Interval_Unit, TS_Interval"
                            + " FROM tblEvaluationGroup WHERE (((EvaluationID)=" + GetReferenceEvalID() + "))";

            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);

            DateTime dtSim = DateTime.Parse(ds.Tables[0].Rows[0]["TS_StartDate"].ToString());
            //  DateTime dtRPT = DateTime.Parse(ds.Tables[0].Rows[0]["TS_StartDate"].ToString());
            int nTS_Interval = Convert.ToInt32(ds.Tables[0].Rows[0]["TS_Interval"].ToString());
            IntervalType interval = TimeSeries.GetTSIntervalType(Convert.ToInt32(ds.Tables[0].Rows[0]["TS_Interval_Unit"].ToString()));
            _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtSim, IntervalType.Year, nTS_Interval);
            _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtSim, IntervalType.Year, nTS_Interval);
            _dtEndDateTime = DateTime.Parse(ds.Tables[0].Rows[0]["TS_EndDate"].ToString());

        }

        //todo: move this to simlink...
        private void SetTSREPO_HDF5(bool bCreateIfNoExist = true)
        {
            string sHDF_Filename = CommonUtilities.GetSimLinkFileName("SimClimTS.h5", _nActiveEvalID);       //todo: handle scenario specific simclim?

            sHDF_Filename = Path.Combine(Path.GetDirectoryName(_sActiveModelLocation), sHDF_Filename);
            _hdf5._sHDF_FileName = sHDF_Filename;

            if (bCreateIfNoExist)
                _hdf5.hdfCheckOrCreateH5(sHDF_Filename);
        }


        //initalize any other members of the class that are needed but do not relate directly to climate
        public void SC_InitializeNonClimate(int nLogLevel = 1)
        {
            //sim2.2        cu.cuLOGGING_Init(cu.RMV_FixFilename(System.DateTime.Now.ToString()) + "_LOG.log", 1, Delete_This);

            //     cu.sLogging_Directory = Delete_This;
            //      cu.sLogging_FileToWrite = Path.Combine(Delete_This, cu.RMV_FixFilename(System.DateTime.Now.ToString()) + "_LOG.log");
        }

        public void SC_CloseObject()
        {
            //sim2.2         cu.cuLOGGING_WriteLogFile();

        }

        private void InitSC_DomainDictionary()
        {
            string sql = "SELECT Val, Domain_Cat, ValLabel FROM tlkpSimClimDictionary";
            DataSet dsMyDs = _dbContext.getDataSetfromSQL(sql);

            foreach (DataRow drow in dsMyDs.Tables[0].Rows)
            {
                string sKey = drow["Domain_Cat"].ToString() + "_" + drow["VAL"].ToString();
                _dictSC_DomainCodes.Add(sKey, drow["ValLabel"].ToString());
            }
        }

        public static double[,] scGetSimClimOutput(ref hdf5_wrap hdfSC, string sGroupID, string sDataSetID = "1")
        {
            double[,] dVals = TimeSeries.tsGetTimeSeries(ref hdfSC, sGroupID, true);
            return dVals;
        }
        /*      
              //met 7/6/2013: replaced original function which did HDF creation in TS; now responsibility of calling function to manage that
              public static double[,] scGetSimClimOutput(int nEvalID, int nScenarioID, string sBaseModelLocation, int nResultsCode)
              {
                  string sHDF = cu.GetSimLinkFullFilePath(sBaseModelLocation, CommonUtilities._nModelTypeID_SimClim, nEvalID, nScenarioID);    
                  double[,] dVals = TimeSeries.tsGetTimeSeries(CommonUtilities._nModelTypeID_SimClim,nEvalID,nScenarioID, sBaseModelLocation,nResultsCode);
                  return dVals;
              }

         * 
         * */
        #endregion


        // process sim Clim evaluations and scenarios
        #region SimClim_EXECUTE


        //rtrn typ jagged array for ease of data push to HDF5
        private double[][,] InitTS_InMemArray(int nPeriodCount, int nResultsRecordCount)
        {
            double[][,] _dResultTS_Vals = new double[nResultsRecordCount][,];
            _sTS_GroupID = new string[nResultsRecordCount];
            for (int i = 0; i < nResultsRecordCount; i++)
            {
                _dResultTS_Vals[i] = new double[nPeriodCount, 13];
            }
            return _dResultTS_Vals;
        }

        public void WriteResultsToFile(string sFileOut, int nEvalID)
        {
            int[,] nScenarioInfo;
            double[][,] _dResultTS_Vals = scCompileResults(nEvalID, out nScenarioInfo);
            StreamWriter file = new StreamWriter(sFileOut);
            for (int i = 0; i < nScenarioInfo.GetLength(0); i++)
            {
                string sPrefix = nScenarioInfo[i, 0].ToString() + "," + nScenarioInfo[i, 1].ToString() + ",";
                int nJaggedRows = _dResultTS_Vals[i].GetLength(0);
                int nJaggedCols = _dResultTS_Vals[i].GetLength(1);

                for (int j = 0; j < nJaggedRows; j++)
                {
                    string sLine = "";
                    for (int k = 0; k < nJaggedCols; k++)
                    {
                        sLine = sLine + _dResultTS_Vals[i][j, k].ToString() + ",";
                    }
                    sLine = sPrefix + sLine;
                    file.WriteLine(sLine);
                }

            }
            file.Close();
        }

        //met sim2.0 1/13/2013: this is only needed if starting at midstream
        //pull all SC results back into a jagged array
        public double[][,] scCompileResults(int nEvalID, out int[,] nIDs)
        {
            hdf5_wrap hdf = new hdf5_wrap();
            string sFilePath = CommonUtilities.GetSimLinkDirectory(_sSimClimIntermediateCacheLocation + "\\EVAL\\File.model", 666, nEvalID, false) + "\\";
            sFilePath = sFilePath + CommonUtilities.GetSimLinkFileName("SimClimTS.h5", nEvalID);
            hdf.hdfOpen(sFilePath, true, false);
            DataTable dtResults = _dsEG_ResultTS_Request.Tables[0];         //sim2.0: grab the table for minimum change.. scRMG_GetStationResultRequest(nEvalID);
            double[][,] _dResultTS_Vals = new double[dtResults.Rows.Count][,];
            int[,] nID_Return = new int[dtResults.Rows.Count, 2];
            int i = 0;
            foreach (DataRow dr in dtResults.Rows)
            {
                int nResultID = Convert.ToInt32(dr["ResultTS_ID"].ToString());
                int nScenarioID = Convert.ToInt32(dr["ScenarioID"].ToString());
                string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, nResultID.ToString(), "SKIP", nScenarioID.ToString());
                double[,] dSeries = hdf.hdfGetDataSeries(sGroupID, "1");
                _dResultTS_Vals[i] = dSeries;
                nID_Return[i, 0] = nScenarioID;
                nID_Return[i, 1] = nResultID;
                i++;
            }

            nIDs = nID_Return;
            return _dResultTS_Vals;
        }

        //todo: XMODEL specifications need to be defined.
        public override void ProcessEvaluationGroup()
        {
            if (_dsEG_ResultTS_Request.Tables[0].Rows.Count > 0)
            {
                int nScenStart = Convert.ToInt32(_dsEG_ResultTS_Request.Tables[0].Rows[0]["ScenStart"]);
                int nScenEnd = Convert.ToInt32(_dsEG_ResultTS_Request.Tables[0].Rows[0]["ScenEnd"]);
                bool bHasBeen = Convert.ToBoolean(_dsEG_ResultTS_Request.Tables[0].Rows[0]["HasBeenRun"]);
                if (!bHasBeen && (nScenStart <= CommonUtilities.nScenLCModelExecuted) && (nScenEnd >= CommonUtilities.nScenLCModelExecuted))
                {
                    ExecuteSimClimScenario();
                    UpdateScenarioStamps();
                }
            }
        }


        //this could probably be done much quicker / more elegantly using LINQ
        private void UpdateScenarioStamps()
        {
            List<int> lstScenariosToUpdate = new List<int>();        // list of which scenarios need to be update
            string sTrueBit = _dbContext.GetTrueBitByContext().ToString();
            for (int i = 0; i < _dsEG_ResultTS_Request.Tables[0].Rows.Count; i++)
            {
                if (_dsEG_ResultTS_Request.Tables[0].Rows[i]["HasBeenRun"] != sTrueBit)
                {
                    _dsEG_ResultTS_Request.Tables[0].Rows[i]["HasBeenRun"] = true;
                    if (!lstScenariosToUpdate.Contains(Convert.ToInt32(_dsEG_ResultTS_Request.Tables[0].Rows[i]["ScenarioID"])))
                        lstScenariosToUpdate.Add(Convert.ToInt32(_dsEG_ResultTS_Request.Tables[0].Rows[i]["ScenarioID"]));
                }
            }

            foreach (int n in lstScenariosToUpdate)
            {
                UpdateScenarioStamp(n, CommonUtilities.nScenLCModelResultsRead);
            }
        }

        //quick and dirty code to 
        /* NOT USED this is code optimization just gt working for now      private void PruneDataTableRequest(ref DataTable dt){
                  for (int i = dt.Rows.Count - 1; i >= 0; i--)
                  {
                      DataRow dr = dt.Rows[i];
                      if (dr["HasBeenRun"] == _dbContext.GetTrueBitByContext().ToString())
                          dt.Rows.Remove(dr);           // dr.Delete();
                  } 

              }
      */

        //load TS data for previously executed SimClim runs
        //note -if TS is added for previously run scenario, the scenario needs to be rerun (or fancy footwork by somebody who understands)

        /*protected void LoadScenarioDatasets()
        {
            DataTable dtSC_ResultsRequest = _dsEG_ResultTS_Request.Tables[0];
            int nRowCount = dtSC_ResultsRequest.Rows.Count;
            if (nRowCount>0)
            {
                _hdf5.hdfOpen(_hdf5._sHDF_FileName);
                for (int i = 0; i < dtSC_ResultsRequest.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dtSC_ResultsRequest.Rows[i]["HasBeenRun"]))
                    {
                        string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS,
                            dtSC_ResultsRequest.Rows[i]["ResultTS_ID"].ToString(), "SKIP",
                            dtSC_ResultsRequest.Rows[i]["ScenarioID"].ToString());

                        _dResultTS_Vals[i] = _hdf5.hdfGetDataSeries(sGroupID, "1");
                        _sTS_GroupID[i] = sGroupID;
                    }
                }
                _hdf5.hdfClose();
            }
            else{
                _log.AddString("no ResultTS requested for linked SimClim object", Logging._nLogging_Level_2);
            }
        }   */

        //this does not accord with "processScenario" type of approach to linked methods
        //this is because we want to completely minimize the number of times that we go to the SC grid ...
        //removed nEvalID - need to consider how this handles multiple scenarios 

        public bool ExecuteSimClimScenario()
        {
            bool bReturn = true; bool bFirstPass = true;
            // step 1: get the data
            DataTable dtSC_ResultsRequest = _dsEG_ResultTS_Request.Tables[0];       //met 1/3/2013: removed 
            // PruneDataTableRequest(ref dtSC_ResultsRequest);

            int nResultsRecordCount = EvalCountScenarios(ref dtSC_ResultsRequest);                         //

            if (nResultsRecordCount > 0)
            {
                int nStartYear = scHELPER_GetYearFromDate(_tsdSimDetails._dtStartTimestamp.ToString(), true);     // met 1/17 updated based on tsd set during init....  scHELPER_GetYearFromDate(dtSC_ResultsRequest.Rows[0]["TS_StartDate"].ToString(), true);
                int nEndYear = scHELPER_GetYearFromDate(_dtEndDateTime.ToString(), false);
                int nYearInterval = _tsdSimDetails._nTSInterval;    //Convert.ToInt32 (Math.Round(Convert.ToDouble(dtSC_ResultsRequest.Rows[0]["TS_Interval"].ToString()),0));         // we are assuming the interval is provided in years- may not be the case! anyway.
                int nCurrentYear = nStartYear;
                //sim2: performed on initEG now.. int nLoops = Convert.ToInt32(Math.Floor(Convert.ToDouble((nEndYear-nStartYear)/nYearInterval)));
                // sim2 if (((nEndYear - nStartYear) % nYearInterval) == 0) { nLoops++; }       //increment by one if mod is zero


                double dVal, dVal_Low = -666.666, dVal_High = -666.666, dLat = -666.666, dLong = -666.666;
                int nCol = -1, nRow = -1;
                int? nResultsCode;          ///0: LOW  1 or null : MEDIAN     2 HIGH

                /////////////temp function build only   ///////////////////////                 //replaced by a handler to get the 1:Many info on these
                List<string> lstMonths = new List<string>(); lstMonths.Add("Jan");

                //////////////////end temporary section
                int nPeriodNo = 0;

                //size data array  MET sim2.0 done on init now:  double[][,] _dResultTS_Vals = InitTS_InMemArray(_nLoopsMUST_BE_REPLACED, nResultsRecordCount);       
                //   Random r = new Random();    //testing only   
                //       new double[dtSC_ResultsRequest.Rows.Count][,];        //jagged array holds separate results for each pass
                int[,] arrSC_IDs = scHELPER_ScenarioData(dtSC_ResultsRequest, nResultsRecordCount);                           //  new double[dtSC_ResultsRequest.Rows.Count,2];         //index 0: SC_Scenario  index 1: ResultTSID_FK

                //load the group arrays for referencing the data- todo move into own function
                int nCounter = 0;
                foreach (DataRow dr_SC2 in dtSC_ResultsRequest.Rows)
                {
                    string sResultID = dr_SC2["ResultTS_ID"].ToString();  //VarResultType_FK
                    string sStationID = "SKIP";             // dr_SC["StationID"].ToString();
                    string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, sResultID, sStationID, dr_SC2["ScenarioID"].ToString());
                    // string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, sVarType, sStationID, dr_SC["ScenarioID"].ToString());
                    _sTS_GroupID[nCounter] = sGroupID;
                    bFirstPass = false;
                    nCounter++;
                }
                ///


                while ((nCurrentYear <= nEndYear) && (nCurrentYear <= _nMAX_YEAR))
                {            //outer year loop- requiers separate 
                    //   double[,] arrSC_Results = new double[nLoops, 13];       //hold values for all SC runs
                    int nLastSimClimScenarioID = -1; int nLastResultTS_ID = -1; string sLastSC_Area = "NOTHING"; //vars to track if we need to get another ... always need new at begin new loop
                    string sASCII_Primary = "NOTHING"; string sASCII_Low = "NOTHING"; string sASCII_High = "NOTHING";
                    int nPercentileLow = -1; int nPercentileHigh = -1;

                    int drCounter = 0;
                    int nMonthCounter = 0;

                    //todo: pad months before TS start; however, for now this works (just takes longer).
                    for (nMonthCounter = 1; nMonthCounter <= 12; nMonthCounter++)
                    {   //bojangles change backt to 12
                        lstMonths = GetMonth(nMonthCounter - 1);
                        bool bFirstMonthPass = true;
                        drCounter = 0;
                        foreach (DataRow dr_SC in dtSC_ResultsRequest.Rows)
                        {
                            if (!Convert.ToBoolean(dr_SC["HasBeenRun"].ToString()))
                            {      //met 1/14/14- skip runs previously loaded.
                                int nGCM = Convert.ToInt32(dr_SC["GCM_Link"].ToString());       // negative: just GCM positive Ensemble  -666: not set.
                                if (nGCM != _nActiveGCMList)
                                {
                                    _list_Ensemble_GCMs = GetEnsembleList(nGCM);               // go get the list of ensemebles
                                    _nActiveGCMList = nGCM;
                                }

                                // load the identifier array on first pass
                                /* met 7/14/14                         if (bFirstPass)
                                                         {
                                                             string sResultID = dr_SC["ResultTS_ID"].ToString();  //VarResultType_FK
                                                             string sStationID = "SKIP";             // dr_SC["StationID"].ToString();
                                                             string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, sResultID, sStationID, dr_SC["ScenarioID"].ToString());
                                                            // string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, sVarType, sStationID, dr_SC["ScenarioID"].ToString());
                                                             _sTS_GroupID[drCounter] = sGroupID;
                                                             bFirstPass = false;
                                                         }           */


                                //check if new (1): SimClim Scenario (diff month/sens/sres et)  (2)area   (3) results variable

                                // loop over each scenario (low/high are managed as part of that scenario)
                                if (((Convert.ToInt32(dr_SC["ScenarioID"].ToString()) == nLastSimClimScenarioID) && (dr_SC["SCData_Path"].ToString() == sLastSC_Area) && (Convert.ToInt32(dr_SC["VarResultType_FK"].ToString()) == nLastResultTS_ID) && (!bFirstMonthPass)))
                                {
                                    //same data can be used.
                                }
                                else
                                {           //must go get new data
                                    string sSensitivity = _dictSC_DomainCodes["SENSITIVITY_" + dr_SC["ClimateSensitivity"].ToString()];
                                    nPercentileLow = Convert.ToInt32(dr_SC["PercentileLow"]);
                                    nPercentileHigh = Convert.ToInt32(dr_SC["PercentileHigh"]);
                                    string sArea = Path.Combine(_sClimDataLocation, dr_SC["SCData_Path"].ToString());
                                    InitializeClimateRequest(nCurrentYear, dr_SC["SC_FieldName"].ToString(), sSensitivity, lstMonths, _list_Ensemble_GCMs, Convert.ToBoolean(dr_SC["IsFromBaseline"].ToString()), sArea, Convert.ToInt32(dr_SC["SRES_Projection"].ToString()), nPercentileLow, nPercentileHigh);
                                    bool bSuccessScenarioGenerate = CalculateSC_Pattern();
                                    nLastSimClimScenarioID = Convert.ToInt32(dr_SC["ScenarioID"].ToString());
                                    sLastSC_Area = dr_SC["SCData_Path"].ToString();
                                    nLastResultTS_ID = Convert.ToInt32(dr_SC["VarResultType_FK"].ToString());


                                    bool bIntermediateSave = SC_Helper_CheckWhetherSaveData(Convert.ToInt32(dr_SC["StoreIntermediateResultsCode"].ToString()), nCurrentYear, nStartYear, nEndYear);
                                    if (false) //bIntermediateSave
                                    {
                                        string sRasterFileName = "CreateRasterFilename eg 2150_1_2.txt";  //[year]_[vartype]_[percentilecode]
                                        //todo (8/21/2013: modify ascii raster to pull from relevant datta (eg cookie raster, results, _results_low, _results_high etc.
                                        scCreateASCII_Raster(sRasterFileName);
                                    }

                                }
                                dLat = Convert.ToDouble(dr_SC["Latitude"].ToString());
                                dLong = Convert.ToDouble(dr_SC["Longitude"].ToString());
                                //        nResultsCode = Convert.ToInt32(dr_SC["ElementIndex"].ToString());

                                //at this point, we have the correct pattern (may be the part from the old one

                                scGetRequestedGridCell(ref nCol, ref nRow, dLong, dLat);

                                dVal = scGetResults(nCol, nRow, 1);
                                _dResultTS_Vals[drCounter][nPeriodNo, nMonthCounter] = dVal;
                                drCounter++;

                                if (nPercentileLow != -1)                //handle Percentile Low if requested
                                {
                                    dVal = scGetResults(nCol, nRow, 0);
                                    _dResultTS_Vals[drCounter][nPeriodNo, nMonthCounter] = dVal;
                                    drCounter++;
                                }
                                if (nPercentileHigh != -1)                //handle Percentile High if requested
                                {
                                    dVal = scGetResults(nCol, nRow, 2);
                                    _dResultTS_Vals[drCounter][nPeriodNo, nMonthCounter] = dVal;
                                    drCounter++;
                                }
                                // r


                                bFirstMonthPass = false;
                            }   //end if HasBeenRun
                        }       //end for   dr_SC in dtSC_ResultsRequest.Rows
                    }
                    SC_Helper_SetYear(ref _dResultTS_Vals, nPeriodNo, nResultsRecordCount, nCurrentYear);
                    //                 _dResultTS_Vals[drCounter][nPeriodNo, 0] = nCurrentYear;          //store the year.
                    nCurrentYear += nYearInterval;
                    nPeriodNo++;                                                            //current approach assumes 
                }
                InsertTS_Results();     //_dResultTS_Vals, arrSC_IDs, _nActiveEvalID);
            }
            return bReturn;
        }

        //counts the total # of scenarios, including perc_low and perc_high requested by user                   MET 8/13/2013
        private int EvalCountScenarios(ref DataTable dtSC_ResultsRequest)
        {
            int nRecordCount = 0;
            foreach (DataRow dr in dtSC_ResultsRequest.Rows)
            {
                nRecordCount++;
                if (Convert.ToInt32(dr["ScenarioLow"].ToString()) != -1) { nRecordCount++; }
                if (Convert.ToInt32(dr["ScenarioHigh"].ToString()) != -1) { nRecordCount++; }
            }
            return nRecordCount;
        }


        //set the year for each result slice
        private void SC_Helper_SetYear(ref double[][,] _dResultTS_Vals, int nCounter, int nDim, int nCurrentYear)
        {
            for (int i = 0; i < nDim; i++)
            {
                _dResultTS_Vals[i][nCounter, 0] = nCurrentYear;
            }
        }

        //archived 6/20/2013: previous version that wrote to database; now goes to hdf5 like other ts information for standardization (also suits the weird monthly format of the data.
        // met 1/13/14  removed reference3 to arrSC_IDs and evalID
        public void InsertTS_Results()      //double[][,] arrSC_Results, int[,] arrSC_IDs, int nEvalID)
        {
            int nSeries = _dResultTS_Vals.GetLength(0); // arrSC_Results.GetLength(0); string sFilePath = "";
            //    int nScenarioID = -1;  int nResultID = -1; 
            //    int nInsertCounter = 0;     //initial insert location
            // hdf5_wrap _hdf5_SC = new hdf5_wrap();
            // met 8/13/2013: use ONE ts file per EG - easier to keep open
            // sFilePath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, nEvalID, false) + "\\";
            //  sFilePath = sFilePath + CommonUtilities.GetSimLinkFileName("SimClimTS.h5", nEvalID);
            _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);

            for (int i = 0; i < nSeries; i++)
            {
                double[,] dArrToInsert = _dResultTS_Vals[i];
                // nScenarioID = arrSC_IDs[i,0]; 
                // nResultID = arrSC_IDs[i,1];
                // string sGroupName = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, nResultID.ToString(),"SKIP",nScenarioID.ToString());

                //todo: this writes the vals already pulled as well; seems like no problem with that. very fast compred to simclim run
                _hdf5.hdfWriteDataSeries(dArrToInsert, _sTS_GroupID[i], "1");
            }
            _hdf5.hdfClose();
        }

        //

// commented out- use simlink version of this function now
        ////met 9/3/2013: add ability to bring in related station data
        ///// <summary>
        ///// 9/21/16: added resultid as first arg, and optional switch to create the TS record. many times you may not wish to create the TS record
        ///// </summary>
        ///// <param name="nEvalID"></param>
        ///// <param name="nScenarioID"></param>
        ///// <param name="nVarType"></param>
        ///// <param name="nStationID"></param>
        ///// <param name="lstTS"></param>
        ///// <param name="sLabel"></param>
        ///// <param name="bIsAux"></param>
        //public void ImportResultsTimeSeries(int nResultID, int nEvalID, int nScenarioID, int nVarType, int nStationID, List<TimeSeries> lstTS, TimeSeries.TimeSeriesDetail tsDtl, string sLabel, bool bIsAux = true)
        //{
        //    // 1: Store a result record
        //    if (nResultID==-1)      //val of -1 indicates that you wnat to creat the record
        //        nResultID = InsertTS(nEvalID, nVarType, nStationID, tsDtl, sLabel, bIsAux);
        //    // 2: get name of the TS Container
        //    string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, nResultID.ToString(), "SKIP", nScenarioID.ToString());
        //    // 3: Store the TS data

        //    _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
        //    _hdf5.hdfWriteDataSeries(TimeSeries.tsTimeSeriesTo2DArray(lstTS), sGroupID, "1");
        //    _hdf5.hdfClose();
        //}


        //updated 8/13/2013 MET to asupport revised approach for
        private int[,] scHELPER_ScenarioData(DataTable dt, int nScenarioCount)
        {
            int[,] dArr = new int[nScenarioCount, 2];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                dArr[i, 0] = Convert.ToInt32(dr["ScenarioID"]);
                dArr[i, 1] = Convert.ToInt32(dr["ResultTS_ID"]);
                i++;

                // now manage for low
                if (Convert.ToInt32(dr["ScenarioLow"].ToString()) != -1)
                {
                    dArr[i, 0] = Convert.ToInt32(dr["ScenarioLow"]);
                    dArr[i, 1] = Convert.ToInt32(dr["ResultTS_ID"]);
                    i++;
                }
                // high
                if (Convert.ToInt32(dr["ScenarioHigh"].ToString()) != -1)
                {
                    dArr[i, 0] = Convert.ToInt32(dr["ScenarioHigh"]);
                    dArr[i, 1] = Convert.ToInt32(dr["ResultTS_ID"]);
                    i++;
                }

            }
            return dArr;
        }

        //met todo: consider best way of storing multiple months. right now we store for whole year. may want to provide greater flexibility about what to store out.
        private bool SC_Helper_CheckWhetherSaveData(int nStoreIntermediateResultsCode, int nCurrentYear, int nStartYear, int nEndYear)
        {
            bool bReturn = false;
            switch (nStoreIntermediateResultsCode)
            {
                case 1:
                    bReturn = false;
                    break;
                case 2:
                    if ((nCurrentYear == nStartYear) || (nCurrentYear == nEndYear))
                    {
                        bReturn = true;
                    }
                    else
                    {
                        bReturn = false;
                    }
                    break;

                case 3:
                    bReturn = true;
                    break;
            }
            return bReturn;
        }

        //utiliyt function to avoid flipping erros on bad data
        private int scHELPER_GetYearFromDate(string sDate, bool bIsStart)
        {
            try
            {
                return DateTime.Parse(sDate).Year;
            }
            catch (Exception ex)
            {
                //     cu.cuLogging_AddString("Bad start or end time for SimClim Analysis; default value being used (1990 start 2050 end)",cu.nLogging_Level_1);
                if (bIsStart)
                {
                    return _nBASE_YEAR;
                }
                else
                {
                    return 2050;
                }
            }
        }

        //
        private List<string> GetEnsembleList(int nGCM_CODE)
        {
            List<string> lstReturn = new List<string>();
            string sql;
            if (nGCM_CODE > 0)
            {
                sql = "SELECT tblEnsemble.EnsembleID, tblEnsemble.ProjID_FK, tblEnsembleXREF.GCM_ID_FK, qrySimClim001_GCM.ValLabel"
                    + " FROM qrySimClim001_GCM INNER JOIN (tblEnsembleXREF INNER JOIN tblEnsemble ON tblEnsembleXREF.EnsembleID_FK = tblEnsemble.EnsembleID) "
                    + "ON qrySimClim001_GCM.Val = tblEnsembleXREF.GCM_ID_FK"
                    + " WHERE (((tblEnsemble.EnsembleID)=" + nGCM_CODE + "));";
            }
            else
            {
                sql = "SELECT Val, ValLabel FROM qrySimClim001_GCM"
                    + " WHERE (((Val)=" + nGCM_CODE + "));";
            }

            DataSet dsMyDs = _dbContext.getDataSetfromSQL(sql);

            foreach (DataRow drGCM in dsMyDs.Tables[0].Rows)
            {
                lstReturn.Add(drGCM["ValLabel"].ToString());
            }

            return lstReturn;
        }


        //met 1/3/2013: changed name from scRMG_GetStationResultRequest
        private DataSet LoadTSData_Request(int nEvalID)
        {

            string sql_select = "SELECT EvaluationID, ScenarioID, SimClimScenarioID, StationID, VarResultType_FK, ResultTS_ID, Latitude, Longitude, Elevation, SCData_Path, ElementIndex, "
                                + "TS_StartDate, TS_EndDate, TS_Interval, TS_Interval_Unit, SRES_Projection, ClimateSensitivity, SC_FieldName, GCM_Link, Percentile, IsFromBaseline, Months, StoreIntermediateResultsCode, IsPrimaryScen,"
                                + " PercentileLow, PercentileHigh, ScenarioLow, ScenarioHigh, ScenStart, ScenEnd, HasBeenRun"
                                //+ " FROM qrySimClim002_LinkToResultTS WHERE ((EvaluationID=" + nEvalID + ") AND (IsAux = 0)) ORDER BY SimClimScenarioID, VarResultType_FK, SCData_Path";
                                + " FROM qrySimClim002_LinkToResultTS WHERE (EvaluationID=" + nEvalID + ") ORDER BY SimClimScenarioID, VarResultType_FK, SCData_Path";

            DataSet dsMyDs = _dbContext.getDataSetfromSQL(sql_select);
            return dsMyDs;
        }

        #endregion


        //region to return SimClim type objects based on passed strings
        #region SimClimify

        //read the file DEM.txt into the member vars
        //requires that _simclimArea  is set
        private bool scInitializeDEM_Info()
        {
            bool bSuccess = true;
            string sAreaPath = _simclimArea.Path;
            string sFile = Path.Combine(sAreaPath, "Cookie.DOC");
            if (File.Exists(sFile))
            {
                using (StreamReader reader = new StreamReader(sFile))
                {
                    string sBuf; string sKey = ""; string sPair = "";
                    while ((sBuf = reader.ReadLine()) != null)
                    {
                        ParseDEM_Line(sBuf, ref sKey, ref sPair);                //set the values from the file
                        if (sKey != "SKIP")
                        {
                            switch (sKey)
                            {
                                case "columns":
                                    //track var or not?
                                    break;
                                case "rows":
                                    //track var or not?
                                    break;
                                case "min. X":
                                    _dSCArea_LongMin = Convert.ToDouble(sPair);
                                    break;
                                case "max. X":
                                    _dSCArea_LongMax = Convert.ToDouble(sPair);
                                    break;
                                case "min. Y":
                                    _dSCArea_LatMin = Convert.ToDouble(sPair);
                                    break;
                                case "max. Y":
                                    _dSCArea_LatMax = Convert.ToDouble(sPair);
                                    break;
                                case "resolution":
                                    _dSCArea_Resolution = Convert.ToDouble(sPair);
                                    break;
                                case "flag value":
                                    _dSCArea_FlagValue = -9990.0;                                   // no value was given in global... let's just assume this for now                   Convert.ToDouble(sPair);
                                    break;
                            }
                        }
                    }
                }
                bAreaLoaded = true;
            }
            else
            {
                bAreaLoaded = false;
            }
            return bAreaLoaded;
        }



        //how to parse the line
        private void ParseDEM_Line(string sBuf, ref string sKey, ref string sPair)
        {
            sKey = "SKIP";
            int nIndex = sBuf.IndexOf(':');
            if (nIndex > 0)
            {
                sKey = sBuf.Substring(0, nIndex - 1).Trim();
                sPair = sBuf.Substring(nIndex + 1).Trim();
            }
        }

        private int CountSimClimYearLoops()
        {
            int nYearEnd = _dtEndDateTime.Year;
            int nYearStart = _tsdSimDetails._dtStartTimestamp.Year;
            int nYearInterval = _tsdSimDetails._nTSInterval;            // simclim must be yearly so don't need to check that
            int nLoops = Convert.ToInt32(Math.Floor(Convert.ToDouble((nYearEnd - nYearStart) / nYearInterval)));
            if (((nYearEnd - nYearStart) % nYearInterval) == 0) { nLoops++; }
            return nLoops++;
        }

        //sets the values passed to it based on interpolation of the '
        // met 9/13/2013: updated interp function; believe this is from upperleft corner- row was being calc'd wrong.

        public void scGetRequestedGridCell(ref int nCol, ref int nRow, double dRequest_Longitude, double dRequestLatitude)
        {
            if (IsInRange(dRequest_Longitude, _dSCArea_LongMin, _dSCArea_LongMax) && IsInRange(dRequest_Longitude, _dSCArea_LongMin, _dSCArea_LongMax))
            {
                nCol = Convert.ToInt32(Math.Round((dRequest_Longitude - _dSCArea_LongMin) / _dSCArea_Resolution));
                nRow = Convert.ToInt32(Math.Round((_dSCArea_LatMax - dRequestLatitude) / _dSCArea_Resolution));
            }
            else
            {
                // value not found
                nCol = -1;
                nRow = -1;
            }
        }

        //0 : low ensemble results  1 'median'  2 high
        public double scGetResults(int nCol, int nRow, int? nResultsCode = 1)
        {
            double dReturn = _dSCArea_FlagValue + 1.234;            //increment a bit to distinguish from other error flags

            switch (nResultsCode)
            {
                case 0:
                    if (_resultsLow != null) { dReturn = _resultsLow[nCol, nRow]; }
                    break;
                case 1:
                    if (_results != null) { dReturn = _results[nCol, nRow]; }
                    break;
                case 2:
                    if (_resultsHigh != null) { dReturn = _resultsHigh[nCol, nRow]; }
                    break;
            }
            return dReturn;
        }

        //overloaded function that can be called from COM and does not require special types.
        //todo: public bool InitializeClimateRequest(int nYear, string sClimVar, string sSensitivity, int[]nMonthList
        //not implemented yet
        public bool InitializeClimateRequest(int nYear, string sClimVar, string sSensitivity, List<string> lstMonths, List<string> lstPatterns, bool bFromBaseline, string sArea, int nMOF_Code, int? nPercLow, int? nPercHigh)
        {
            try
            {
                _yearSC = nYear;
                _sensitivitySC = getSC_Sensitivity(sSensitivity);

                _months_SC = GetSelectedMonths(lstMonths);
                _resultTypeSC = GetResultType(bFromBaseline);
                SetEnsemblePercentileValues(nPercLow, nPercHigh);
                _simclimArea = SimCLIMArea.Create(sArea, DataVariables.Climate);               //_simclimArea = SimCLIMArea.Create(sArea);
                _emissionScenarioSC = getSC_EmissionsScenario(nMOF_Code);
                _climVar = getSC_DataVariable(sClimVar);
                scInitializeDEM_Info();
                _cookieImage = IdrisiImageFactory.LoadCookie(Path.Combine(_simclimArea.Path, "cookie.img"));
                _baselineDataSource = SimCLIMPatternDataSourceAdapter.Create(_cookieImage, _simclimArea.Baseline.Path, _climVar);
                //    baselineDataSource = SimCLIMPatternDataSourceAdapter.Create(cookieImage, _simclimArea.Baseline.Path, climVar);               
                _selectedPatterns = GetSelectedPatterns(lstPatterns);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public void test1()
        {
            IPatternDataSource baselineDataSource;

            CookieIdrisiImage cookieImage;
            DataVariable climVar;

            //set area stuff
            string folder = @"C:\Users\Mason\Documents\SimClim\Data\Illinois";
            _simclimArea = SimCLIMArea.Create(folder, DataVariables.Climate);

            // Update UI based with info from specified area
            //    UpdatePatternList();
            //  UpdateVariableList();
            //   areaFolderPath.Text = folder;

            //  get clim var
            climVar = getSC_DataVariable("Precip");
            cookieImage = IdrisiImageFactory.LoadCookie(Path.Combine(_simclimArea.Path, "cookie.img"));
            baselineDataSource = SimCLIMPatternDataSourceAdapter.Create(cookieImage, _simclimArea.Baseline.Path, climVar);
        }



        public string scGetASCIIRaster_BaselineArea()
        {
            string sReturn = "NONE";
            string sRasterFileName = Path.Combine(_simclimArea.Path, "cookie_raster.txt");
            if (File.Exists(sRasterFileName))
            {
                sReturn = sRasterFileName;
            }
            else
            {
                scCreateASCII_Raster(sRasterFileName);
                sReturn = sRasterFileName;
            }
            return sReturn;
        }



        //ESRI 
        private void scCreateASCII_Raster(string sRasterFileName)
        {
            List<string> lstRaster = new List<string>();
            lstRaster.Add("ncols       " + _baselineDataSource.Height);
            lstRaster.Add("nrows       " + _baselineDataSource.Width);
            lstRaster.Add("xllcorner       " + _dSCArea_LongMin);
            lstRaster.Add("yllcorner       " + _dSCArea_LatMin);
            lstRaster.Add("cellsize       " + _dSCArea_Resolution);
            lstRaster.Add("NODATA_value    -9999");

            int nRow = _baselineDataSource.Height;
            int nCol = _baselineDataSource.Width;

            for (int i = 0; i < nRow; i++)
            {
                string sBuf = "";
                for (int j = 0; j < nCol; j++)
                {
                    sBuf = sBuf + _cookieImage.Data[i, j] + " ";
                }
                lstRaster.Add(sBuf);
            }


            File.WriteAllLines(sRasterFileName, lstRaster.ToArray()); //write out the raster


            scCreateProjectionFile(sRasterFileName.Replace(".txt", ".prj"));
        }

        //create a projection file 
        private void scCreateProjectionFile(string sProjectionFileName)
        {
            List<string> lstProjection = new List<string>();
            lstProjection.Add("Projection    GEOGRAPHIC");
            lstProjection.Add("Datum         NAD83");
            lstProjection.Add("Spheroid      GRS80");
            lstProjection.Add("Units         DD");
            lstProjection.Add("Zunits        NO");
            lstProjection.Add("Parameters    ");

            File.WriteAllLines(sProjectionFileName, lstProjection.ToArray()); //write out the raster
        }



        //apiv02- note this is for ensemble only

        public bool CalculateSC_Pattern()
        {
            try
            {
                getEnsemblePatternSource();      //ensemble only; todo- review scenario workflow
                _ensembleDataSource = new EnsemblePatternReaderDataSource(_cookieImage, _ensembleReader);

                _generator = ScenarioGeneratorFactory.CreateSpatialScenarioGenerator(_scenYear, GetLicenseInfo());
                //          _patternDataSource = getEnsemblePatterSource();
                //            _patternDataSource.Calculate();                          //be careful to avoid cached data
                setSC_GeneratorVals();
                SC_GeneratorExecute();
                return true;
            }
            catch (Exception ex)
            {
                //log the info
                Debug.WriteLine("Error in SC Generate Process :" + ex.Message);
                return false;
            }

        }

        //met 8/21/2013: this code is basically from the test API from Clim; maybe should be modified to work with SimLink approach?
        private void SC_GeneratorExecute()
        {
            _results = _generator.Execute();


            /*   API V02 figure out how to handle percentile
               // Generate results for Low Percentile
               if (_lowPercentile != null)
               {
                   _generator.Pattern = _patternDataSource.LowPercentileData;
                   _resultsLow = _generator.Execute();
               }

               // Generate results for High Percentile
               if (_highPercentile != null)
               {
                   _generator.Pattern = _patternDataSource.HighPercentileData;
                   _resultsHigh = _generator.Execute();
               }
             * 
             * */
        }

        private void setSC_GeneratorVals()
        {
            _generator.Baseline = _baselineDataSource;
            _generator.Year = _yearSC;
            _generator.Variable = _climVar;
            _generator.EmissionScenario = _emissionScenarioSC;
            _generator.Months = _months_SC;
            _generator.ResultType = _resultTypeSC;
            _generator.Sensitivity = _sensitivitySC;

            // Generate results for Ensemble values
            _generator.Pattern = _ensembleDataSource;
        }

        // get a Sensitivity object based on string stored in SimLink*
        private Sensitivity getSC_Sensitivity(string sSensitivity)
        {
            Sensitivity s = Sensitivity.Mid;

            switch (sSensitivity.ToLower())
            {
                case "low":
                    s = Sensitivity.Low;
                    break;
                case "mid":
                    s = Sensitivity.Mid;
                    break;
                case "high":
                    s = Sensitivity.High;
                    break;
            }
            return s;
        }


        // get a climvar object given a string
        private DataVariable getSC_DataVariable(string sClimVar)
        {
            DataVariable cvReturn = _simclimArea.Baseline.SupportedVariables.First();  // = new DataVariable();
            bool bFound = false;
            foreach (var cv in _simclimArea.Baseline.SupportedVariables)      //was climatevar
            {
                if (cv.Name.ToString().ToLower() == sClimVar.ToLower())
                {
                    cvReturn = cv;
                    bFound = true;
                    break;
                }
            }
            if (!bFound) { }                                        //log it//}
            return cvReturn;

        }

        // get an Emmissions Scenario
        //VAL	ValLabel    fILENAME (join SimClim index with filenames per index.csv (in the .mof folder)
        //1	Baseline        EmisScen.mof   
        //2	SRES A1B        EmisSce0.mof
        //3	SRES AF1        EmisSce1.mof
        //4	SRES A1T        EmisSce2.mof
        //5	SRES A2         EmisSce3.mof
        //6	SRES B1         EmisSce4.mof
        //7	SRES B2         EmisSce5.mof

        private IEmissionScenarioItem getSC_EmissionsScenario(int nEmissionsScenario)
        {
            if (!string.IsNullOrEmpty(_sMOF))
            {
                // Load the emission scenario data using the MOFIndexEntry class directory (normally you'd use
                // MOFIndex and select one from there, or use the XML based Emission Scenario File type (EmsScenFile class))

                // MOF files are an (older) format used by SimCLIM. The EmsScenFile XML format is newer and is
                // used by SimCLIM for ArcGIS
                string sFileName = "";
                switch (nEmissionsScenario)
                {
                    case 1:
                        sFileName = "EmisScen.mof ";
                        break;
                    case 2:
                        sFileName = "EmisSce0.mof";
                        break;
                    case 3:
                        sFileName = "EmisSce1.mof";
                        break;
                    case 4:
                        sFileName = "EmisSce2.mof";
                        break;
                    case 5:
                        sFileName = "EmisSce3.mof";
                        break;
                    case 6:
                        sFileName = "EmisSce4.mof";
                        break;
                    case 7:
                        sFileName = "EmisSce5.mof";
                        break;
                }


                return new MOFIndexEntry(_sMOF_Dir, sFileName, Path.Combine(_sMOF_Dir, sFileName));
            }
            throw new Exception("Emission Scenario not loaded");
        }



        //return pattern objects corresponding to those with names in lstPatterns 
        private List<IPatternInfo> GetSelectedPatterns(List<string> lstPatterns)
        {
            List<IPatternInfo> patternList = new List<IPatternInfo>();

            foreach (var pattern in _simclimArea.Patterns)
            {
                if (lstPatterns.Contains(pattern.Name))
                {
                    patternList.Add(pattern);
                }
                else
                {
                    Debug.WriteLine(pattern.Name + " not found");
                }
            }

            if (patternList.Count != lstPatterns.Count)
            {             //significant potential error- could easily drop a climate pattern info
                //   cu.cuLogging_AddString("Possible data error: Requested clim patterns = " + lstPatterns.Count + "; Clim data patterns found = " + patternList.Count, cu.nLogging_Level_1);
            }

            return patternList;
        }

        //return Monthlist objects corresponding to those with names in lstMonths 
        private MonthList GetSelectedMonths(List<string> lstMonths)
        {
            List<IPatternInfo> patternList = new List<IPatternInfo>();
            MonthList monthlistReturn = new MonthList();

            foreach (string sMonth in lstMonths)
            {
                Month m = Month.Jan;
                switch (sMonth.Substring(0, 3).ToLower())
                {
                    case "jan":
                        m = Month.Jan;
                        break;
                    case "feb":
                        m = Month.Feb;
                        break;
                    case "mar":
                        m = Month.Mar;
                        break;
                    case "apr":
                        m = Month.Apr;
                        break;
                    case "may":
                        m = Month.May;
                        break;
                    case "jun":
                        m = Month.Jun;
                        break;
                    case "jul":
                        m = Month.Jul;
                        break;
                    case "aug":
                        m = Month.Aug;
                        break;
                    case "sep":
                        m = Month.Sep;
                        break;
                    case "oct":
                        m = Month.Oct;
                        break;
                    case "nov":
                        m = Month.Nov;
                        break;
                    case "dec":
                        m = Month.Dec;
                        break;
                }
                monthlistReturn.Add(m);
            }
            return monthlistReturn;
        }

        //overloaded function takes zero indexed month 
        //return Monthlist objects corresponding to those with names in lstMonths 
        private MonthList GetSelectedMonths(int nMonth)
        {
            List<IPatternInfo> patternList = new List<IPatternInfo>();
            MonthList monthlistReturn = new MonthList();


            Month m = Month.Jan;
            switch (nMonth)
            {
                case 0:
                    m = Month.Jan;
                    break;
                case 1:
                    m = Month.Feb;
                    break;
                case 2:
                    m = Month.Mar;
                    break;
                case 3:
                    m = Month.Apr;
                    break;
                case 4:
                    m = Month.May;
                    break;
                case 5:
                    m = Month.Jun;
                    break;
                case 6:
                    m = Month.Jul;
                    break;
                case 7:
                    m = Month.Aug;
                    break;
                case 8:
                    m = Month.Sep;
                    break;
                case 9:
                    m = Month.Oct;
                    break;
                case 10:
                    m = Month.Nov;
                    break;
                case 11:
                    m = Month.Dec;
                    break;
                    monthlistReturn.Add(m);
            }
            return monthlistReturn;
        }

        //return a 
        //todo: consider sidestepping intermediate list and going integer or monthlist approach
        private List<String> GetMonth(int nMonth)
        {
            List<string> lstSingleMonth = new List<string>();

            switch (nMonth)
            {
                case 0:
                    lstSingleMonth.Add("Jan");
                    break;
                case 1:
                    lstSingleMonth.Add("Feb");
                    break;
                case 2:
                    lstSingleMonth.Add("Mar");
                    break;
                case 3:
                    lstSingleMonth.Add("Apr");
                    break;
                case 4:
                    lstSingleMonth.Add("May");
                    break;
                case 5:
                    lstSingleMonth.Add("Jun");
                    break;
                case 6:
                    lstSingleMonth.Add("Jul");
                    break;
                case 7:
                    lstSingleMonth.Add("Aug");
                    break;
                case 8:
                    lstSingleMonth.Add("Sep");
                    break;
                case 9:
                    lstSingleMonth.Add("Oct");
                    break;
                case 10:
                    lstSingleMonth.Add("Nov");
                    break;
                case 11:
                    lstSingleMonth.Add("Dec");
                    break;
            }
            return lstSingleMonth;
        }


        //set some baselin stuff
        private void GetBaselineInfo()
        {
            _cookieImage = IdrisiImageFactory.LoadCookie(Path.Combine(_simclimArea.Path, "cookie.img"));
            _baselineDataSource = SimCLIMPatternDataSourceAdapter.Create(_cookieImage, _simclimArea.Baseline.Path, _climVar);
        }

        //updated for API v02
        private void getEnsemblePatternSource()
        {
            // *new* Create and setup the Ensemble Pattern Reader
            _ensembleReader = new EnsemblePatternReader();
            _ensembleReader.Months = _months_SC;
            _ensembleReader.Percentile = 50;        //bojangles nt.Parse(percentileText.Text);
            foreach (IPatternInfo patternInfo in _selectedPatterns)
                _ensembleReader.Patterns.Add(patternInfo.Path);
            _ensembleReader.Calculate();
        }

        /* API V01
        private IEnsembleDataSource getEnsemblePatterSource()
        {
            _patternDataSource = new EnsembleDataSource(_cookieImage.NumCols, _cookieImage.NumRows);
            _patternDataSource.Months = _months_SC;
            _patternDataSource.LowPercentile = _lowPercentile;
            _patternDataSource.HighPercentile = _highPercentile;
            foreach (IPatternInfo patternInfo in _selectedPatterns)
                _patternDataSource.Patterns.Add(SimCLIMPatternDataSourceAdapter.Create(_cookieImage, (patternInfo).Path, _climVar));

            return _patternDataSource;
        }
        */


        //ensemble percentile values
        private void SetEnsemblePercentileValues(int? nLow, int? nHigh)
        {
            _lowPercentile = nLow;
            _highPercentile = nHigh;


            /*          if (int.TryParse(nLow.ToString, out l))                       //orig from matt code
                          _lowPercentile = l;

                      if (int.TryParse(highPercentileTxt.Text, out h))
                          _highPercentile = h;*/
        }

        private ScenarioResultType GetResultType(bool bIsChangeFromBaseline)
        {
            if (!bIsChangeFromBaseline)
                return ScenarioResultType.Projections;
            else
                return ScenarioResultType.ChangesFromBaseline;
        }

        private IComponentLicenseInfo GetLicenseInfo()
        {
            var licenseInfo = new ComponentLicenseInfo(_licRegistrant, _licKey);
            return licenseInfo;
        }

        #endregion


        #region SC_Processes

        #endregion

        // various garabage
        #region SC Utility

        //utility to test for request within range
        private bool IsInRange(double dRequest, double dMin, double dMax)
        {
            return ((dRequest >= dMin) && (dRequest <= dMax));
        }

        #endregion
    }
}
