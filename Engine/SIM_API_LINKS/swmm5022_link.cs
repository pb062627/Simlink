using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data;
using SIM_API_LINKS.DAL;
using System.Globalization;
using Nini.Config;

namespace SIM_API_LINKS
{
    public class swmmReportHelper
    {
        public bool _bUpdateIndexEachIteration = false; //todo: if inserts, this would be necessary
        public bool _bInputInclude;
        public bool _bCONTROLSInclude;
       // public bool _bSUBCATCHMENTS_ALL;
        public RPT_OUT_TYPE _rptSubcatchments;       //load all, none, or some in OUT file (important for indices)
        public List<string> _lstSubcatchments = new List<string>();
        public RPT_OUT_TYPE _rptNodes;
        public List<string> _lstNodes = new List<string>();
        public RPT_OUT_TYPE _rptLinks;
        public List<string> _lstLinks = new List<string>();

        public enum RPT_OUT_TYPE
        {
            ALL,
            NONE,
            SOME
        }
    }



    public class swmm5022_link : simlink
    {
        #region MEMBERS
        #region SWMM_OutVars
        private static long offset0; private static long StartPos; private static long SWMM_Nperiods; private static long errCode; private static long magic2; private static long magic1;
        private static long version; private static long SWMM_FlowUnits; private static long SWMM_Nsubcatch; private static long SWMM_Nnodes; private static long SWMM_Nlinks; private static long SWMM_Npolluts;
        private static long SubcatchVars; private static long NodeVars; private static long LinkVars; private static long SysVars; private static long SWMM_StartDate; private static long SWMM_ReportStep; private static long BytesPerPeriod;
        private static int nRecordSize = 4; private static BinaryReader b;      //constant  
        #endregion

        public Dictionary<string, string> _dictSWMM_LID_Usage = new Dictionary<string, string>();
        public bool _bUpdateSubcatchmentsFromLID;
        public bool _bLID_Subarea_ScaleByFootprint = true;     //set to false to scale by tributary area.
        private bool _bOUT_ReadAll = true;
        public swmmReportHelper _swmmReportHelper = new swmmReportHelper();
        private bool _bTS_IndicesLoaded = false;
        public bool _bIsSWMM51 = false;
        public string _sHSF_XCOHORT;        // for use in cohort runs of type SCENARIO_INIT
        private bool _bScenSaveSecondaryAndAuxTS = true;


        #region DICT INFO       //store 
        private const int _nFieldDict_RAINGAGE_TIMESERIES = 111;            //used when retrieve TS detail (MEV)
        private const int _nFieldDict_RAINGAGE_DATASOURCE = 110;
        private const int _nFieldDict_RAINGAGE_STATIONID = 486; 
        private const int _nFieldDict_RAINGAGE_NAME = 106;
        private const int _nFieldDict_SUBAREA_TOTAL_AREA = 187;
        private const int _nFieldDict_SUBAREA_PERC_IMPERVIOUS = 188;
        private const int _nFieldDict_LID_USAGE_ID = 342;               
        #endregion

        #endregion


        /// <summary>
        /// Class description
        /// </summary>
        public string ClassDescription
        {
            get { return "swmm5022 simlink"; }
        }

        #region INIT

        public override void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_SWMM;
            _sTS_FileNameRoot = "SWMM_TS.h5";
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
   //moved to initEG becaueys this is EG specific         if (true)
     //                    PopulateLID_UsageDictionary();                      //   maybe todo: consider loading only if it will be needed..

            if (LoadSetupData())
                InitNavigationDict();
            InitHPC_Dict();
            if (_bIsSimCondor)
            {
               // do anything you need to do for specific platform
               _htc._htcPlatformSpecActive =  _htc.SyncGetPlatformItem("SWMM5");
            }
        }

        public override bool InitializeConn_ByFileCon(string sFile, string sDelim = "=")
        {
            bool bValid;
            Dictionary<string, string> dict = InitializeConnDict_ByFileCon(sFile, out bValid, sDelim);
            if (bValid)
                InitializeModelLinkage(dict["conn"], Convert.ToInt32(dict["dbtype"]), _bIsSimCondor);
            return bValid;
        }

        /// <summary>
        /// function to set the base dictionary to be used for sending to hpc
        /// todo: potentially read from the config
        /// </summary>
        private void InitHPC_Dict()
        {
     // on _hpc.EnvConfig       _dictHPC.Add("has_requirements", "has_swmm");
            if(!_dictHPC.ContainsKey("transfer_input_files"))
                _dictHPC.Add("transfer_input_files", "");           //override in process scenario
        }
        protected override void InitNavigationDict()
        {
            string sSQL = "SELECT tlkpSWMMTableDictionary.TableName, tlkpSWMMTableDictionary.KeyColumn, tlkpSWMMFieldDictionary.FieldName, tlkpSWMMFieldDictionary.ID AS VarType_FK, tlkpSWMMTableDictionary.ID AS [TableID],"
                    + " FieldINP_Number, IsScenarioSpecific, RowNo, SectionNumber, SectionName"             // todo: figure out how to get this info- rarely used, Qualifier1,  Qual1Pos"        
                    + " FROM tlkpSWMMFieldDictionary INNER JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID;";
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
                _bTS_IndicesLoaded = false;             // met 1/3/17: make sure we ts indices are reset if needed

                if (LoadSetupData())
                {
                    if (_dictSWMM_LID_Usage.Count == 0)       //only load this first time around
                    {
                        try
                        {
                            PopulateLID_UsageDictionary(_nActiveBaselineScenarioID);
                        }
                        catch (Exception ex)
                        {
                            _log.AddString("Error loading SWMM LID dictionary", Logging._nLogging_Level_2, false, true);
                            // do not throw - not usually mattering.
                        }

                    }
                
                    _dsEG_ResultSummary_Request = LoadResultSummaryDS(nEvalID);
                    //_dsEG_ResultTS_Request = ReadOut_GetDataSet(nEvalID,false); //SP 15-Feb-17 Performed in standard Simlink InitializeEG routine
                    _dsEG_ResultSummary_Request.Tables[0].Columns["val"].ReadOnly = false;                  //used to store vals
                    base.LoadReference_EG_Datasets();               //  must follow _dsEG_ResultTS_Request
                    SetTSDetails();                                 // load simulation/reporting timesereis information
                    LoadAndInitDV_TS();                             //load any reference TS information needed for DV and/or tblElementXREF
                }

                InitTS_Vars();
                //SP 18-Nov-2016 - TODO find a better place for this. Moved from LoadReference_EG_Datasets as it's needed after TS is initialised which is called in each derived class
                //EGDS_GetTS_AuxDetails(_nActiveBaselineScenarioID);  // 8/15/14 //SP 15-Feb-2017 AUX details now retrieved when EGDS_GetTS_Details is called

                //if the baseline is already executed, collect any info (may be needed for scoring, functions etc).

                SetTS_FileName(_nActiveBaselineScenarioID,"NOTHING",true);    //don't update the filename...
                                                                              //pass false, even though 3rd arg actuallly true, because we don't want to kill those datasets.
                                                                              //may be mor complicated- issues.... met 5/30/14
                                                                              //changed back... ? solves one issue, creates another?

                //SP 15-Feb-2017 - Extract External Data at EG level for AUXEG retrieve codes
                EGGetExternalData();

                if (LoadSetupData())
                    LoadScenarioDatasets(_nActiveBaselineScenarioID, 100, true);       //Load any datasets for the baseline, if applicable
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error initializing EG: " + ex.Message);
            }
        }

        // updated 9/21/16 to support end dates
        public override void SetTSDetails()
        {
            string sSQL =   "SELECT START_DATE, START_TIME, REPORT_START_DATE, REPORT_START_TIME, REPORT_STEP, WET_STEP, end_date, end_time"
                            +" FROM tblSWMM_RunSettings"
                            + " WHERE (((ModelVersion)=" + _nActiveBaselineScenarioID + "))";      // replaced _nActiveReferenceEG_BaseScenarioID  - can differ across eg with common ref...
            var usCulture = "en-US";
            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime dtSim = DateTime.Parse(ds.Tables[0].Rows[0]["START_DATE"] + " " + ds.Tables[0].Rows[0]["START_TIME"], new CultureInfo(usCulture, false));
                DateTime dtEnd = DateTime.Parse(ds.Tables[0].Rows[0]["end_date"] + " " + ds.Tables[0].Rows[0]["end_time"], new CultureInfo(usCulture, false));
                DateTime dtRPT = DateTime.Parse(ds.Tables[0].Rows[0]["REPORT_START_DATE"] + " " + ds.Tables[0].Rows[0]["REPORT_START_TIME"], new CultureInfo(usCulture, false));
                int nSecInterval = TimeSeries.CONVERT_GetSecFromHHMM(ds.Tables[0].Rows[0]["REPORT_STEP"].ToString());
                _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtRPT, IntervalType.Second, nSecInterval);
                _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtSim, IntervalType.Second, nSecInterval, dtEnd);
            }
            else
            {
                try
                {
                    //SP 11-Sept-2017 read Sim time and details from the INP;
                    _log.AddString(string.Format("SWMM timeseries info not in tblSWMM_RunSettings; retrieving from inp file {0}", _sActiveModelLocation),
                        Logging._nLogging_Level_1, false, true);
                    DateTime dtSim = DateTime.Parse(ReadINPOption_FieldValue(_sActiveModelLocation, "START_DATE") + " " + ReadINPOption_FieldValue(_sActiveModelLocation, "START_TIME"), new CultureInfo(usCulture, false));
                    DateTime dtEnd = DateTime.Parse(ReadINPOption_FieldValue(_sActiveModelLocation, "END_DATE") + " " + ReadINPOption_FieldValue(_sActiveModelLocation, "END_TIME"), new CultureInfo(usCulture, false));
                    DateTime dtRPT = DateTime.Parse(ReadINPOption_FieldValue(_sActiveModelLocation, "REPORT_START_DATE") + " " + ReadINPOption_FieldValue(_sActiveModelLocation, "REPORT_START_TIME"), new CultureInfo(usCulture, false));
                    int nSecInterval = TimeSeries.CONVERT_GetSecFromHHMM(ReadINPOption_FieldValue(_sActiveModelLocation, "REPORT_STEP"));

                    _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtRPT, IntervalType.Second, nSecInterval);
                    _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtSim, IntervalType.Second, nSecInterval, dtEnd);
                }
                catch (Exception ex)
                {
                    //if an error reading from the inp, use dummy values
                    _log.AddString("SWMM timeseries info could not be read from inp file. Setting to dummy values time step interval 3600 and start time 1/1/2000. Critical if performing event definition.", 
                        Logging._nLogging_Level_1, false, true);
                    _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(Convert.ToDateTime("1/1/2000"), IntervalType.Second, 3600); //SP 26-Jun-2017 This is where the 3600 is set. Need to read from hydraulic model. MET to advise how!
                    _tsdSimDetails = new TimeSeries.TimeSeriesDetail(Convert.ToDateTime("1/1/2000"), IntervalType.Second, 3600, Convert.ToDateTime("1/2/2000")); 
                }

                // add default time
                // log the issue.
            }

        }

        /// <summary>
        /// Added on election day of 2016. I hope this is a good day.
        /// </summary>
        /// <param name="bCreateModelChanges"></param>
        /// <param name="dtSimStart"></param>
        /// <param name="dtSimEnd"></param>
        /// <param name="nTS_Interval_Sec"></param>
        /// <param name="dtRptStart"></param>
        /// <param name="dtRptEnd"></param>
        /// <param name="nInterval_Sec_rpt"></param>
        public override void SetSimTimeSeries(bool bCreateModelChanges, DateTime dtSimStart, DateTime dtSimEnd, int nTS_Interval_Sec, DateTime dtRptStart = default(DateTime), DateTime dtRptEnd = default(DateTime), int nInterval_Sec_rpt = -1)
        {
            base.SetSimTimeSeries(bCreateModelChanges, dtSimStart, dtSimEnd, nTS_Interval_Sec, dtRptStart ,  dtRptEnd ,  nInterval_Sec_rpt);
            if (bCreateModelChanges)
            {
                // manually push the results
                //bojangles: we almost never code 
                InsertModelValList(-1, 377, _nActiveScenarioID, _tsdSimDetails._dtStartTimestamp.ToString("MM/dd/yyyy"), "", "START_DATE", -1, -1);
                InsertModelValList(-1, 378, _nActiveScenarioID, _tsdSimDetails._dtStartTimestamp.ToString("HH:mm:ss"), "", "START_TIME", -1, -1);
                InsertModelValList(-1, 379, _nActiveScenarioID, _tsdSimDetails._dtStartTimestamp.ToString("MM/dd/yyyy"), "", "REPORT_START_DATE", -1, -1);
                InsertModelValList(-1, 380, _nActiveScenarioID, _tsdSimDetails._dtStartTimestamp.ToString("HH:mm:ss"), "", "REPORT_START_TIME", -1, -1);
                InsertModelValList(-1, 381, _nActiveScenarioID, _tsdSimDetails._dtEndTimestamp.ToString("MM/dd/yyyy"), "", "END_DATE", -1, -1);
                InsertModelValList(-1, 382, _nActiveScenarioID, _tsdSimDetails._dtEndTimestamp.ToString("HH:mm:ss"), "", "END_TIME", -1, -1);

                WriteResultsToDB(_nActiveScenarioID);
            }
        }

        //8/15/14: made an override. allows this to be called from simlink class if not initialized by earlier step
        public override void SetTS_FileName(int nScenarioID, string sPath = "NOTHING", bool bSetHDF = true)
        {
            string sTargetPath = sPath;
            if (sTargetPath == "NOTHING")
            {
                sTargetPath = Path.GetDirectoryName(_sActiveModelLocation) + "\\";
                sTargetPath = CommonUtilities.GetSimLinkDirectory(sTargetPath, nScenarioID, _nActiveEvalID, true);
            }
            if (bSetHDF)
                _hdf5._sHDF_FileName = sTargetPath + "\\" + CommonUtilities.GetSimLinkFileName(_sTS_FileNameRoot, nScenarioID);
        }
        #region SPECIAL_CASE



        //MET 5/14/17: migrate special case code to this location... not sure how it iwas being called before.
        protected virtual void ModifyModelChanges_SpecialCase
            (int DV_ID_FK, int TableFieldKey, int ScenID, ref string val, string note, string ElName, int ElId, int nDV_Option)
        {
            //do nothing in generic case
        }


        //met 4/7/14: override in base class
        // todo: make generalized special case manager
                //for now, this just calls LID special case manager
        //public override Dictionary<int, string>       GetNewModVals_SPECIALCASE

        //met 4/9/14: getting this call below to work was VERY challenging
        //many configurations of attempts let to ACCESS VIOLATION errors.
        //oddly, it seemed to depend on the number of vars sent (on basis of MUCH testing).
        //thus, the stupid pack into nvals.
        public override Dictionary<int, string> GetNewModVals_SPECIALCASE(string sElementLabel, double dValFromOptionList, int[] nPacked, double dParam1)   //, int nElementID)     //, double dAggregateVal = -1)
        {
            int nCode = nPacked[1]; //this is so dumb
            int nElementID = nPacked[0];
            double dAggregateVal = -1;  
            //todo: add more options (pipe sizing, pumping, etc etc as the need arises (try to handle as much as possible generally)    
          //  int nElementID = -666;
            Dictionary<int, string> dictReturn = SpecialCase_LID(nCode, dParam1, nElementID, sElementLabel, dValFromOptionList);        //sElementLabel, sOptionList_c_Val, -1);
          //  return dictReturn;


            return dictReturn;
        }


        public override int ReturnNum()     //int nCode, double dParam1)    //, int nElementID, string sElementLabel, string sOptionList_c_Val, double dAggregateVal = -1)
        {
            return -21;
        }

                // met 1/4/2013
        // 1st special case function to do "generalized specific" processing for LID . done for OC

        //TODO: i envision a tblDV_SpecialCase that holds some of the field keys allowing some customization of this without code change.
        //dAggregateVal must be passed if using type 3 (which distributes an amount of LID based upon the AREA of the subcatchment compared to the total for that ElementList
       
            //sim2 4/4/2013: minor modifications for sim2 compatability, but no progres towards more adaptable implementation
        public  Dictionary<int, string> SpecialCase_LID(int nCode, double dParam1, int nElementID, string sElementLabel, double dOptionList_c_Val, double dAggregateVal = -1){
            Dictionary<int, string> dictResultVals = new Dictionary<int,string>();
            double dPercImpervious;
            double dTotalArea; double dAreaImpervious; double dOptionVal;
            double dRES_LID_AREA; double dRES_Impervious_Managed; double dImperviousRouteToPervious;
            
            double dUnitConversion = 43560; double dIMP_Perc_AlreadyManaged; double dUnAllocatedImperv;
            dTotalArea = dUnitConversion * Convert.ToDouble(GetNetworkAttribute(_nFieldDict_SUBAREA_TOTAL_AREA,_nActiveBaselineScenarioID,nElementID));                //todo: replace hard coded key values with pull from to-be-created special case table
            dAggregateVal = dUnitConversion * dAggregateVal;
            dPercImpervious = Convert.ToDouble(GetNetworkAttribute(_nFieldDict_SUBAREA_PERC_IMPERVIOUS,_nActiveBaselineScenarioID,nElementID));         //  rmgDB_link.rmgHelper_GetModelElement_ByID(nElementID, 188, 1));

     /*       if (sElementLabel == "3960-L" || (sElementLabel == "3756-L"))
            {
                sElementLabel=sElementLabel;
            }*/

            dIMP_Perc_AlreadyManaged =LID_GetManagedPERC(sElementLabel, _nActiveBaselineScenarioID);
            dImperviousRouteToPervious = GetImperviousRouteToPervious(nElementID, _nActiveBaselineScenarioID);
          //  cu.cuLogging_AddString("NOTICE: hard coded baseline scenario value of: 7344", cu.nLogging_Level_1);
            dAreaImpervious = dTotalArea * (dPercImpervious )  / 100;
            dOptionVal = dOptionList_c_Val;     // Convert.ToDouble(sOptionList_c_Val);       //this must be convertible to a double
            dIMP_Perc_AlreadyManaged = dIMP_Perc_AlreadyManaged * (100 - dImperviousRouteToPervious) / 100;// *(100 - dImperviousRouteToPervious);     //LID already managed must be represented in terms of UNROUTED impervious
            dUnAllocatedImperv = 100 - dIMP_Perc_AlreadyManaged - dImperviousRouteToPervious;
            double dAreaImperviousUnManaged = dAreaImpervious * dUnAllocatedImperv / 100;
           //todo: make this more adaptive, beautiful and complicated.

            switch (nCode)
            {
                case 1:             //LID element as percentage of impervious area                                                           
                    dRES_LID_AREA = dAreaImpervious * dOptionVal / 100;     // sOptionList_c_Val expected to be a # like 5, 10, 25  (%)
                    dRES_Impervious_Managed = dParam1 * dOptionVal;
                    dictResultVals.Add(346, dRES_LID_AREA.ToString());
                    dictResultVals.Add(349, dRES_Impervious_Managed.ToString());
                    break;
                case 2:             //LNC 4/10/14: calc the amount MANAGED, and then calculate LID from that (inverse of option 1)
                    dRES_Impervious_Managed = dAreaImpervious * dOptionVal / 100;       //NB: managed imp is understood 
                    if (dRES_Impervious_Managed > dAreaImperviousUnManaged)
                        dRES_Impervious_Managed = 0.99 * dAreaImperviousUnManaged;      //  -> met 8/12/14 dUnAllocatedImperv;            //ensure that we do not manage more than 100%

                    double dPercImpManaged = (dRES_Impervious_Managed / dAreaImpervious) * 100;     //calculate (adjusted) % imp managed (based upon OVERALL IMPERVIOUS)
                    dPercImpManaged = dPercImpManaged / ((100-dImperviousRouteToPervious) / 100);                 // convert to SWMM expected percentage, which is in relation to the imperviousness not managed << 8/14/14- NO- it is imperviousd not routed. 
                    dRES_LID_AREA = dRES_Impervious_Managed / dParam1;                              //area of LID unit

                    dictResultVals.Add(346, dRES_LID_AREA.ToString());
                    dictResultVals.Add(349, dPercImpManaged.ToString());
                    break;
                case 200:             //LID a size eg 10 ac)
                    //  NOT TESTED NOT TESTED met 1/8/13

                    dRES_LID_AREA = 43560 * dOptionVal;     // sOptionList_c_Val expected to be a # like 5, 10, 25  (ac)
                    dRES_Impervious_Managed = Math.Round((100 * (dRES_LID_AREA) / dAreaImpervious), 4);

                    if (dRES_Impervious_Managed > 100)          //impervious area managed turns out to be GT 100
                    {
                        dRES_Impervious_Managed = 100;
                        dOptionVal = dRES_Impervious_Managed * dAreaImpervious / (100 * dParam1);       //update dOptionVal
                        dRES_LID_AREA = 43560 * dOptionVal;
                    //    rmgDB_link.cu.cuLogging_AddString("Requested LID coverage for element " + sElementLabel + " exceeded 100% impervious; revised to manage 100%", cu.nLogging_Level_2);
                    }
                    dictResultVals.Add(346, dRES_LID_AREA.ToString());
                    dictResultVals.Add(349, dRES_Impervious_Managed.ToString());
                    break;
                case 3:         //prorate based on AREA 
                    if (dAggregateVal>=0)
                    {
                        dRES_LID_AREA = 43560 * dOptionVal * (dTotalArea / dAggregateVal);
                        dRES_Impervious_Managed = Math.Round((100 * (dParam1 * dRES_LID_AREA) / dAreaImpervious), 4);

                        if (dRES_Impervious_Managed > dUnAllocatedImperv)          //impervious area managed turns out to be GT 100
                        {
                            dRES_Impervious_Managed = dUnAllocatedImperv - 0.1;         // try to avoid a dumb rounding error
                            dOptionVal = dRES_Impervious_Managed * (dAreaImpervious / dUnitConversion) / (100 * dParam1);       //update dOptionVal
                            dRES_LID_AREA = 43560 * dOptionVal;
                           // rmgDB_link.cu.cuLogging_AddString("Requested LID coverage for element " + sElementLabel + " exceeded 100% impervious; revised to manage 100%", cu.nLogging_Level_2);
                        }
                        dictResultVals.Add(346, dRES_LID_AREA.ToString());
                        dictResultVals.Add(349, dRES_Impervious_Managed.ToString());

                    }
                    else{
                     //    rmgDB_link.cu.cuLogging_AddString("Special case LID distribution failed; Aggregate value LT 0 passed", cu.nLogging_Level_1);       //log the error
                    }
                    break;
            }
        return dictResultVals;
    }

                //some of the impervious area is already managed; we grab this quantity for a given subcatchment

        //todo: store in SWMM member variable and don't reload each time
       private double LID_GetManagedPERC(string sSubcatchment, int nScenarioID)
       {
           double dReturn = 0.0;      //default value if no LID currently managing the subcatchment
           string sql = "SELECT tblSWMM_LID_Usage.Subcatchment, Sum(tblSWMM_LID_Usage.FromImprv) AS SumOfFromImprv"
                        + " FROM tblSWMM_LID_Usage WHERE "
                        + "(((tblSWMM_LID_Usage.ModelVersion)=" + nScenarioID + ") AND ((tblSWMM_LID_Usage.Subcatchment)='" + sSubcatchment + "'))"
                        +" GROUP BY tblSWMM_LID_Usage.Subcatchment";
           DataSet dsMyDs = _dbContext.getDataSetfromSQL(sql);
           if (dsMyDs.Tables[0].Rows.Count > 0)
           {
               dReturn = Convert.ToDouble(dsMyDs.Tables[0].Rows[0]["SumOfFromImprv"].ToString());
           }
           return dReturn;
       }

        //untested...
        private double GetImperviousRouteToPervious(int nSubareaID, int nScenarioID)
       {
           double dReturn = 0.0;      //default value if no LID currently managing the subcatchment
           string sql = "SELECT SubareaID, Subarea, PctRouted, ModelVersion FROM tblSWMM_Subareas" + 
                        " WHERE (((ModelVersion)=" + nScenarioID + ") AND ((SubareaID)=" + nSubareaID + "))";

           DataSet dsMyDs = _dbContext.getDataSetfromSQL(sql);
           if (dsMyDs.Tables[0].Rows.Count > 0)
           {
               dReturn = Convert.ToDouble(dsMyDs.Tables[0].Rows[0]["PctRouted"].ToString());
           }
           return dReturn;
       }



        #endregion



        //met 11/6/2013: added to help support LID usage automation.
        //current limitation: does not use ModelVersion
        private void PopulateLID_UsageDictionary(int nScenarioID)
        {
            string sSQL = "SELECT tblSWMM_LID_Usage.LIDUsageID, tblSWMM_LID_Usage.Subcatchment, tblSWMM_LID_Usage.LID_Process, tblSWMM_LID_Usage.ModelVersion"
                            + " FROM tblSWMM_LID_Usage"
                            + " WHERE (ModelVersion = " + nScenarioID +")";

            DataSet ds_Updates = _dbContext.getDataSetfromSQL(sSQL);

            _dictSWMM_LID_Usage = new Dictionary<string, string>();
            foreach (DataRow drow in ds_Updates.Tables[0].Rows)
            {          //ds_Updates is a misnomer; just copied from elsewhere
                _dictSWMM_LID_Usage.Add(drow["LIDUsageID"].ToString(), drow["Subcatchment"].ToString() + "_" + drow["LID_Process"].ToString());
            }
        }

        //met 4/10/14
        /*
Updating parameters of subarea LID elements is challenging, because to identify the proper row, one needs to know 1) the subarea name and 2)the LID unit name.  This is fine if the LID exists in the model initally, because one reference the SimLink backend and grab this information (populating the _dictSWMM_LID_Usage). However, if it is being inserted, then the dictionary is not populated.
Near term resolution: on insert, add the subarea name and the lid type to _dictSWMM_LID_Usage.
This only works if you are inserting max one LID unit per subarea.
Long term resolution: modify SimLinkDetail class to include the SecondaryRecordId.
This will allow us see that the nElementID =-1, ok, it's an insert, go grab the name of the LID from the detail whose primary key equals the current slm secondary id. This is a bit more involved and so has not been implemented
        */


        private void LID_Dict_InsertNew(string sLID_UsageInsert)
        {
            try
            {
                sLID_UsageInsert = CommonUtilities.RemoveRepeatingChar(sLID_UsageInsert);
                string[] s = sLID_UsageInsert.Split(' ');
                string sSubarea = s[0];
                string sLID = s[1];

                if (_dictSWMM_LID_Usage.ContainsKey(sSubarea)==false)           // add new key  met 7/12/14
                    _dictSWMM_LID_Usage.Add(sSubarea, sSubarea + "_" + sLID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding LID USAGE to dictionary; multiple insert types on one subcatchment not yet supported");
                //todo: log the issue
                _bScenarioIsValid = false;
            }


        }


        //TS information for DV/MEV can be defined in one of two places
            //DV -
            // tblElementXREF 

        //todo: first part of this should happen on SimLink, and then call derived class to get the data.
        //met 1/20/13: moved to simlink.cs when implementing isis
/*
        private void LoadAndInitDV_TS()
        {
            DataRow[] drTS = _dsEG_DecisionVariables.Tables[0].Select("IsTS = " + _dbContext.GetTrueBitByContext());
            DataRow[] drXREF = _dsEG_XMODEL_LINKS.Tables[0].Select("IsDV_Link = 0");
            int nDV_TS_Count = drTS.Count() + drXREF.Count();
            _dMEV_Vals = new double[nDV_TS_Count, 2][,];
            _sMEV_GroupID = new string[nDV_TS_Count];
            int nCounter = 0;
            foreach (DataRow dr in drTS)
            {
                _dMEV_Vals[nCounter, 0] = GetNetworkData(Convert.ToInt32(dr["ElementID_FK"]), Convert.ToInt32(dr["VarType_FK"]));
                nCounter++;
            }
            foreach (DataRow dr in drXREF)
            {
                _dMEV_Vals[nCounter, 0] = GetNetworkData(Convert.ToInt32(dr["RefID"]), Convert.ToInt32(dr["RefTypeID"]));
                string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.MEV, dr["RefTypeID"].ToString(), dr["RefID"].ToString(), _nActiveBaselineScenarioID.ToString());
                _sMEV_GroupID[nCounter] = sGroupID;         //track it.
                nCounter++;
            }
        }*/

        //current limitations
            //1: assume that the TS is store in .dat file, not in 
            //2: 
        public override double[,] GetNetworkTS_Data(int nElementID, int nVarType_FK, string sElementLabel = "NOTHING", string sFileLocation = "NOTHING")
        {
            double[,] dReturn = null;
            int nRecordID = -1; 
            string sVal = GetSimLinkDetail(SimLinkDataType_Major.Network,nRecordID,nVarType_FK,_nActiveReferenceEG_BaseScenarioID,nElementID);
            sVal = sVal.Replace("\"", "");
            switch(nVarType_FK){
                case _nFieldDict_RAINGAGE_TIMESERIES:
                    //todo: avoid two db calls by writing custom function
                    string sStationID = GetSimLinkDetail(SimLinkDataType_Major.Network, nRecordID, _nFieldDict_RAINGAGE_STATIONID, _nActiveReferenceEG_BaseScenarioID, nElementID);    //int nStationID = 4;
                    dReturn = DatFile_ReadTimeSeries(sVal, sStationID, _tsdResultTS_SIM);
                    break;
                default:
                    //todo: log this issue
                    break;
            }

            return dReturn;
        }

        #endregion

        #region IMPORT / READ
        #region INP_IMPORT
        public void ReadINP_ToDB(string FileName, int nScenario, int nProjID)
        {
            if (File.Exists(FileName))
            {
                StreamReader file = null;
                string sFirstChar = ""; string sbuf = ""; string sConcat = "";


                DataSet dsRS = new DataSet();
                
                //todoV2
                string sqlRS = "select * from tblSWMM_RunSettings where (SWMM_ID<0)";       //return empty dataset
                dsRS = _dbContext.getDataSetfromSQL(sqlRS);
                DataRow rowRunSettings = dsRS.Tables[0].NewRow();
                try
                {
                    file = new StreamReader(FileName);
                    while (!file.EndOfStream)
                    {
                        if (sFirstChar != "[")
                        {
                            sbuf = file.ReadLine();     // eat a single comment line
            //                if (_bIsSWMM51 && sbuf!="[TITLE)")
               //                 sbuf = file.ReadLine();         //read a second line
                        }
                        //test for special cases which must be handled uniquely
                        //met 2/13/2012 added snowpacks as a table to be skipped for now
                        if (sbuf == "[TITLE]" || sbuf == "[OPTIONS]" || sbuf == "[REPORT]" || sbuf == "[TAGS]" || sbuf == "[MAP]" || sbuf == "[LID_CONTROLS]" || sbuf == "[SNOWPACKS]" || sbuf == "[CONTROLS]" || sbuf == "[TEMPERATURE]" || sbuf == "[TRANSECTS]")
                        {
                            sFirstChar = "X";
                            sConcat = "";
                            switch (sbuf)
                            {
                                case "[TITLE]":
                                    while (sFirstChar != "[")
                                    {
                                        sbuf = file.ReadLine();
                                        if (sbuf.Length > 0) { sConcat = sConcat + sbuf + "\n"; }    //add the title string into a single str
                                        sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                    }
                                    rowRunSettings["Title"] = sConcat;
                                    break;

                                case "[REPORT]":
                                    //allow code to execute through to Options; same processing
                                    while (sFirstChar != "[")
                                    {
                                        sbuf = file.ReadLine();
                                        sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                        if (sbuf.Trim().Length > 2 & sFirstChar != "[")
                                        {
                                            SWMM_ImportNameValuePair(sbuf, ref rowRunSettings);
                                        }
                                    }

                                    break;
                                case "[OPTIONS]":
                                    while (sFirstChar != "[")
                                    {
                                        sbuf = file.ReadLine();
                                        sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                        if (sbuf.Trim().Length > 2 & sFirstChar != "[" & sFirstChar != ";")
                                        {
                                            SWMM_ImportNameValuePair(sbuf, ref rowRunSettings);
                                        }
                                    }

                                    break;
                                case "[TAGS]":
                                    sbuf = file.ReadLine();                                                                     //BYM Feb 15, 2012
                                    sFirstChar = CommonUtilities.GetFirstChar(sbuf);                                                         //BYM Feb 15, 2012
                                    ReadINP_SkipSection(ref file, ref sFirstChar, ref sbuf);      //skip over section          //BYM Feb 15, 2012
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
                                    SWMM_ImportNameValuePair(sbuf, ref rowRunSettings);
                                    sbuf = file.ReadLine();
                                    break;
                                case "[LID_CONTROLS]":
                                    sbuf = file.ReadLine();
                                    sbuf = file.ReadLine();

                                    while (sFirstChar != "[")
                                    {
                                        sbuf = file.ReadLine();
                                        sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                        if (sbuf.Trim().Length > 2 & sFirstChar != "[")         //test that we have some real data
                                        {
                                            ReadINP_ReadLIDControl(ref file, sbuf, nScenario);
                                        }
                                    }
                                    break;
                                case "[SNOWPACKS]":     //skip
                                    sbuf = file.ReadLine();
                                    sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                    ReadINP_SkipSection(ref file, ref sFirstChar, ref sbuf);      //skip over section
                                    break;
                                case "[CONTROLS]":     //skip this section for now, BYM Feb 2012
                                    sbuf = file.ReadLine();
                                    sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                    ReadINP_SkipSection(ref file, ref sFirstChar, ref sbuf);      //skip over section
                                    break;
                                case "[TEMPERATURE]":     //skip this section for now
                                    sbuf = file.ReadLine();
                                    sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                    ReadINP_SkipSection(ref file, ref sFirstChar, ref sbuf);      //skip over section
                                    break;
                                case "[TRANSECTS]":     //skip this section for now
                                    sbuf = file.ReadLine();
                                    sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                    ReadINP_SkipSection(ref file, ref sFirstChar, ref sbuf);      //skip over section
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            if (sbuf.Length > 0)
                            {
                                 new DataSet();
                                char[] nogood = { '[', ']', ';', 'Z', '\t', ' ' };
                                string sTableName = "tblSWMM_" + sbuf.TrimStart(nogood).TrimEnd(nogood);
                                string sSQL_SectionType = DBContext.GetSimpleSQLfromTableName(sTableName);
                                DataSet ds_SectionType =  _dbContext.getDataSetfromSQL(sSQL_SectionType);
                                                             
                                sFirstChar = "X";
                                while ((sFirstChar != "[") & !file.EndOfStream)
                                {
                                    sbuf = file.ReadLine();
                                    sFirstChar = CommonUtilities.GetFirstChar(sbuf);

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
                                        ds_SectionType.Tables[0].Rows.Add(ds_SectionType.Tables[0].NewRow());       //add a new row to the datatable
                                        while (sbuf.Length > 0)
                                        {

                                            i = i + 1;  //increment (skipping the first ID field
                                            if (sbuf.IndexOf(" ") > 0)
                                            {
                                                sEntry = sbuf.Substring(0, sbuf.IndexOf(" "));
                                                sbuf = sbuf.Substring(sbuf.IndexOf(" ") + 1, sbuf.Length - sbuf.IndexOf(" ") - 1).Trim();
                                            }
                                            else
                                            {
                                                sEntry = sbuf;
                                                sbuf = "";
                                            }
                                            ds_SectionType.Tables[0].Rows[ds_SectionType.Tables[0].Rows.Count - 1][i] = sEntry;
                                            ds_SectionType.Tables[0].Rows[ds_SectionType.Tables[0].Rows.Count - 1]["ModelVersion"] = nScenario;
                                        }//end inner while: decoding the sbuf string
                                    }
                                }//end while
                                _dbContext.InsertOrUpdateDBByDataset(true, ds_SectionType, sSQL_SectionType, true, false);        //sim2 note: orig version does not pull back pk
                            }
                        }//end else

                    }   //end while    
                    rowRunSettings["ModelVersion"] = nScenario;         //now finalize the creation of the Runsetting record specific to this SWMM file
                    dsRS.Tables[0].Rows.Add(rowRunSettings);

                    _dbContext.InsertOrUpdateDBByDataset(true, dsRS, sqlRS, true, false);

                    //create lookup tables for the appropriate columns
                    //bckV01: may consider for future - a good idea but not the way SimLink works now....  cu.insertOptionList_MODEL_Lookups(nScenario, 1, nProjID, connMod, connRMG);
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }

                //        }
                //   }

            }



        }

        public void ReadINP_SkipSection(ref StreamReader file, ref string sFirstChar, ref string sbuf)
        {
            while ((sFirstChar != "[") & !file.EndOfStream)
            {
                sbuf = file.ReadLine();
                sFirstChar = CommonUtilities.GetFirstChar(sbuf);
            }
        }

        //SP 11-Sept-17 Factored this out of SWMM_ImportNameValuePair! TODO is called from SWMM_ImportNameValuePair so can incorporate this function at some stage.
        public void SWMM_ReturnNameValuePair(string sNameValPair, ref string sFieldName, ref string sVal)
        {
            if (sNameValPair.Length > 3)       //minimum possible entry
            {
                sFieldName = sNameValPair.Substring(0, sNameValPair.IndexOf(" "));
                if (sFieldName == "INPUT")
                {                               // check for MS Access reserved keywords
                    sFieldName = sFieldName + "_";
                }
                sVal = sNameValPair.Substring(sNameValPair.IndexOf(" ") + 1, sNameValPair.Length - sNameValPair.IndexOf(" ") - 1).Trim();
            }
        }


        public void SWMM_ImportNameValuePair(string sNameValPair, ref DataRow dr)
        {
            if (sNameValPair.Length > 3)       //minimum possible entry
            {
                string sFieldName; string sVal;
                sFieldName = sNameValPair.Substring(0, sNameValPair.IndexOf(" "));
                if (sFieldName == "INPUT")
                {                               // check for MS Access reserved keywords
                    sFieldName = sFieldName + "_";
                }
                sVal = sNameValPair.Substring(sNameValPair.IndexOf(" ") + 1, sNameValPair.Length - sNameValPair.IndexOf(" ") - 1).Trim();
                try
                {
                    dr[sFieldName] = sVal;
                }
                catch (Exception ex){
                    _log.AddString(string.Format("{0} field not found. Most likely a SWMM version issue. In most cases, this will not cause simlink to fail", sFieldName), Logging._nLogging_Level_1, true, true);
                }
            }
        }

        //read an LID control into the db (it could be one of five types
        //provides a good example of how to use the TLKP to assign values
        public void ReadINP_ReadLIDControl(ref System.IO.StreamReader file, string sbuf, int nScenario)
        {
            string sFirstChar = "x"; string sTableName = "x";
            string sLID_Type; string sLID_Label;
            // read first line, and get a data adapater with the correct fields
            //  sbuf = file.ReadLine();
            sLID_Label = sbuf.Substring(0, sbuf.IndexOf(" "));
            sbuf = sbuf.Substring(sbuf.IndexOf(" "));
            sLID_Type = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
            sFirstChar = CommonUtilities.GetFirstChar(sbuf);


            switch (sLID_Type)
            {
                case "BC":
                    sTableName = "tblSWMM_LID_Bioretention";
                    break;
                case "IT":
                    sTableName = "tblSWMM_LID_InfiltrationTrench";
                    break;
                case "PP":
                    sTableName = "tblSWMM_LID_PorousPavement";
                    break;
                case "RB":
                    sTableName = "tblSWMM_LID_RainBarrel";
                    break;
                case "VS":
                    sTableName = "tblSWMM_LID_VegetativeSwale";
                    break;
            }


            //sim2 todo: avoid going back to the db each time for this...  use a dicitonary?

            //get a datatable that shows the fields for a given LID in ascending row/field_inp
            //SP 4-Mar-2016 TODO remove reliance on Query 'qryUI_Component001_TableFieldDict' in access to ensure compatible with SQL Server 
            string sqlFD = "SELECT FieldName FROM qryUI_Component001_TableFieldDict WHERE (((qryUI_Component001_TableFieldDict.InINP)=1) AND (TableName=@TableName))"
                                    + " ORDER BY RowNo, FieldINP_Number;";
            DataSet dsFD = _dbContext.getDataSetfromSQL(sqlFD);

            //get an empty datable for given LID
            string sqlLID = DBContext.GetSimpleSQLfromTableName(sTableName); //"select * from " + sTableName;
            DataSet dsLID = _dbContext.getDataSetfromSQL(sqlLID);

            string sEntry; int i = 2; bool bExit = false;
            dsLID.Tables[0].Rows.Add(dsLID.Tables[0].NewRow());       //add a new row to the datatable

            //set row specific variables
            dsLID.Tables[0].Rows[dsLID.Tables[0].Rows.Count - 1]["ModelVersion"] = nScenario;
            dsLID.Tables[0].Rows[dsLID.Tables[0].Rows.Count - 1]["Label"] = sLID_Label;
            dsLID.Tables[0].Rows[dsLID.Tables[0].Rows.Count - 1]["LID_Type"] = sLID_Type;

            while (!bExit)         //there is an empty line between each LID control entry.
            {
                sbuf = file.ReadLine();
                if (sbuf.Trim().Length == 0) { bExit = true; }              //exit the loop if the output line is clear.
                sbuf = sbuf.Substring(sbuf.IndexOf(" ") + 1, sbuf.Length - sbuf.IndexOf(" ") - 1).Trim();   //trim off the first column which is the label; this is already known



                while (sbuf.Length > 0)
                {

                    i = i + 1;  //increment (skipping the first ID field AND the label field for LID
                    if (sbuf.IndexOf(" ") > 0)
                    {
                        sEntry = sbuf.Substring(0, sbuf.IndexOf(" "));
                        sbuf = sbuf.Substring(sbuf.IndexOf(" ") + 1, sbuf.Length - sbuf.IndexOf(" ") - 1).Trim();
                    }
                    else
                    {
                        sEntry = sbuf;
                        sbuf = "";
                    }
                    dsLID.Tables[0].Rows[dsLID.Tables[0].Rows.Count - 1][i] = sEntry;       //this should be more explicit; depends on the table being set right; could bum out;
                }

            }
            _dbContext.InsertOrUpdateDBByDataset(true, dsLID, sqlLID, true, false);
        }

        #endregion

        #region ReadINPOptions
        //SP 11-Sep-2017 return single value from INP
        public string ReadINPOption_FieldValue(string FileName, string sFieldNametoReturn)
        {
            string sReturn = "";
            if (File.Exists(FileName))
            {
                StreamReader file = null;
                string sFirstChar = ""; string sbuf = ""; string sConcat = "";

                try
                {
                    file = new StreamReader(FileName);
                    while (!file.EndOfStream)
                    {
                        if (sFirstChar != "[")
                        {
                            sbuf = file.ReadLine();     // eat a single comment line
                                                        //                if (_bIsSWMM51 && sbuf!="[TITLE)")
                                                        //                 sbuf = file.ReadLine();         //read a second line
                        }
                        //test for special cases which must be handled uniquely
                        //met 2/13/2012 added snowpacks as a table to be skipped for now
                        if (sbuf == "[TITLE]" || sbuf == "[OPTIONS]" || sbuf == "[REPORT]" || sbuf == "[TAGS]" || sbuf == "[MAP]" || sbuf == "[LID_CONTROLS]" || sbuf == "[SNOWPACKS]" || sbuf == "[CONTROLS]" || sbuf == "[TEMPERATURE]" || sbuf == "[TRANSECTS]")
                        {
                            sFirstChar = "X";
                            sConcat = "";
                            switch (sbuf)
                            {
                                case "[OPTIONS]":
                                    while (sFirstChar != "[")
                                    {
                                        sbuf = file.ReadLine();
                                        sFirstChar = CommonUtilities.GetFirstChar(sbuf);
                                        if (sbuf.Trim().Length > 2 & sFirstChar != "[" & sFirstChar != ";")
                                        {
                                            string sFieldName = "";
                                            string sVal = "";
                                            SWMM_ReturnNameValuePair(sbuf, ref sFieldName, ref sVal);
                                            if (sFieldNametoReturn.ToUpper() == sFieldName.ToUpper())
                                                return sVal;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        /*else
                        {
                            if (sbuf.Length > 0)
                            {
                                char[] nogood = { '[', ']', ';', 'Z', '\t', ' ' };
                                sFirstChar = "X";
                                while ((sFirstChar != "[") & !file.EndOfStream)
                                {
                                    sbuf = file.ReadLine();
                                    sFirstChar = CommonUtilities.GetFirstChar(sbuf);

                                    if (sbuf.Length <= 1 || sFirstChar == ";" || sFirstChar == "[")
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        string sEntry; int i = 0;
                                        while (sbuf.Length > 0)
                                        {

                                            i = i + 1;  //increment (skipping the first ID field
                                            if (sbuf.IndexOf(" ") > 0)
                                            {
                                                sEntry = sbuf.Substring(0, sbuf.IndexOf(" "));
                                                sbuf = sbuf.Substring(sbuf.IndexOf(" ") + 1, sbuf.Length - sbuf.IndexOf(" ") - 1).Trim();
                                            }
                                            else
                                            {
                                                sEntry = sbuf;
                                                sbuf = "";
                                            }
                                        }//end inner while: decoding the sbuf string
                                    }
                                }//end while
                            }
                        }//end else*/
                    }
                }
                catch (Exception ex)
                {
                    //do something
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            return sReturn;
        }


        #endregion

                                    #region DAT_IMPORT


        public double[,] DatFile_ReadTimeSeries(string sFileName, string sStationID, TimeSeries.TimeSeriesDetail tsdSim)
        {
            if (File.Exists(sFileName))
            {
                StreamReader file = new StreamReader(sFileName);
                string sBuf;
                using (file)
                {
                    bool bFoundSection = DatFile_AdvanceToStationID(ref file, out sBuf, sStationID);                            //find correct station (col 1)
                    bFoundSection = DatFile_AdvanceToTargetDate(ref file, ref sBuf, tsdSim._dtStartTimestamp, sStationID);      //advance to date of interest
                    List<TimeSeries> lstTS = new List<TimeSeries>();
                    if (bFoundSection)
                    {
                        string[] sVals = null;
                        while (!file.EndOfStream && DatFile_IsInStationID(sBuf, sStationID, ref sVals))
                        {
                            DateTime dt = new DateTime(Convert.ToInt32(sVals[1]), Convert.ToInt32(sVals[2]), Convert.ToInt32(sVals[3]), Convert.ToInt32(sVals[4]),Convert.ToInt32(sVals[5]),0);
                            lstTS.Add(new TimeSeries(dt, Convert.ToDouble(sVals[6].Trim())));
                            sBuf = file.ReadLine();  
                        }
                        double[,] dTS_Filled = TimeSeries.CreateStandardIntervalTimeSeries(lstTS, tsdSim);
                        return dTS_Filled;
                    }
                    else{
                        //log that the station was not found.
                    }

                }
            }
            else
            {
                //log the issue
            }
            //log
            return null;
        }

        private bool DatFile_AdvanceToStationID(ref StreamReader file, out string sBuf, string  sTargetStation)
        {
            bool bFoundSection = false;
            sBuf = "";
            while (!file.EndOfStream && !bFoundSection)
            {
                sBuf = file.ReadLine();
                string[] sVals = null;
                if (DatFile_IsInStationID(sBuf, sTargetStation, ref sVals))
                    bFoundSection = true;
            }
            return bFoundSection;
        }
      
        //.dat may contain data preceding the sim period; this is not needed; reduce data reqs by grabbing data that falls after start period
        //todo: consider end period also
        private bool DatFile_AdvanceToTargetDate(ref StreamReader file, ref string sBuf, DateTime dtStartTimestamp, string sStationID)
        {
            bool bFoundSection = false;
           // sBuf = "";
            string[] sVals = null;
            while (!file.EndOfStream && !bFoundSection && DatFile_IsInStationID(sBuf, sStationID, ref sVals))
            {
                DateTime dtLine = new DateTime(Convert.ToInt32(sVals[1]), Convert.ToInt32(sVals[2]), Convert.ToInt32(sVals[3]), Convert.ToInt32(sVals[4]),Convert.ToInt32(sVals[5]),0);
                if (dtLine >= dtStartTimestamp)
                    bFoundSection = true;
                else
                    sBuf = file.ReadLine();
            }
            return bFoundSection;
        }
        
        
        //return true if buffer is identified (Col 1) as in taget statsion id.
        //return sVals[] because is needed in main function
        private bool DatFile_IsInStationID(string sBuf, string sTargetStation, ref string[] sVals)
        {
            sVals = sBuf.Split('\t');
            if (sVals[0].Trim() == sTargetStation)
                return true;
            else
                return false;
        }

        #endregion

        #region RESULTS_READ
        #region OUT
        #region TESTING

        //test function met 6/19/14 funnny bidness
        public void TestReadOut(ref hdf5_wrap hdf, string sOutFile, int nRecords)
        {
            string[] sVars = new string[] { "Node", "Subcatchment", "Link" };
            int[] nResultTypes = new int[]{6,6,5};
            SWMM_OpenOutFile(sOutFile);
            for (int ii = 0; ii < 3; ii++) { 
                int nVarType = SWMM_GetOUT_VarType(sVars[ii]);
                int nNumberOfResults = nResultTypes[ii];
                for (int j = 0; j < nRecords; j++)
                {
                    for (int kResultIndex =0; kResultIndex<nNumberOfResults; kResultIndex++)
                    {
                        int nResultIndex = kResultIndex;
                        double[,] dvals = new double[SWMM_Nperiods, 1];                         //hold the current TS Record
                        int nRecordIndex = j;
                        double dCatchError = -1;
                        string sGroupID = sVars[ii] + "_" + nRecordIndex + "_" + nResultIndex;
                        hdf.hdfWriteDataSeries(dvals, sGroupID, "1");
                        if (sGroupID == "Node_0_1")
                            sGroupID = "Node_0_1";

                        //  double[,] dVals = new double[SWMM_Nperiods, 1];
                        for (int i = 0; i < SWMM_Nperiods; i++)
                        {
                            dvals[i, 0] = GetSWMMSeries(nVarType, nRecordIndex, nResultIndex, i + 1);
                        }
                    }
                }
            }
        }


        #endregion

        #region SWMM_Output_VarsDefine

        //
        /// <summary>
        /// Reads rpt file to determine TS Element indices
        /// Returns true if successful (even if some results are not found)
        /// False if error- most likely to RPT file not being complete (Eg error in baseline run
        /// </summary>
        /// <param name="sRPT_File"></param>
        /// <returns></returns>
        public bool TS_SetResultIndices(string sRPT_File, bool bUpdateDBwIndices = false)
        {
            try{
                Dictionary<string, int> dictIndices = TS_LoadIndicesFromRPT(sRPT_File);

                // met 11/13/16: if the "non-combined" result ts is null, then set it based upon the combined
                // this is not ideal, and is fallout from having 3-4 ds of different kinds
                // added ts_code = 1 for result_ts, 2 for secondary, 3 for aux. This is diff than aux result code.  not in db; add in sql. added to epanet.
                //SP 15-Feb-2017 not certain that ts_code is different from RetrieveCode anymore - combining no longer required as all TS type are now stored in _dsEG_ResultTS_Request
                /*if (_dsEG_ResultTS_Request == null)
                {
                    _dsEG_ResultTS_Request = new DataSet();
                    DataTable dt = _dsEG_TS_Combined_Request.Tables[0].Copy(); //SP 15-Feb-2017 _dsEG_ResultTS_Request is now the combined ResultTS dataset
                    _dsEG_ResultTS_Request.Tables.Add(dt);
                    for (int i = _dsEG_ResultTS_Request.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        if (Convert.ToInt32(_dsEG_ResultTS_Request.Tables[0].Rows[i]["ts_code"].ToString()) != 1)  
                        {
                            _dsEG_ResultTS_Request.Tables[0].Rows[i].Delete();
                        }
                    }
                    _dsEG_ResultTS_Request.Tables[0].AcceptChanges();
                 }*/


                for (int i = 0; i < _dsEG_ResultTS_Request.Tables[0].Rows.Count;i++ )
                {
                    //met_rt2 - only process if it's a type 1 results
                        //better: iterate over a filtered return of the ds.
                    if (Convert.ToInt32(_dsEG_ResultTS_Request.Tables[0].Rows[i]["RetrieveCode"]) == (int)RetrieveCode.Primary)
                    {

                        string sSearch = _dsEG_ResultTS_Request.Tables[0].Rows[i]["FeatureType"].ToString() + _dsEG_ResultTS_Request.Tables[0].Rows[i]["Element_Label"].ToString();
                        if (dictIndices.ContainsKey(sSearch))
                        {
                            _dsEG_ResultTS_Request.Tables[0].Rows[i]["ElementIndex"] = dictIndices[sSearch].ToString();
                        }
                        else
                        {
                            _dsEG_ResultTS_Request.Tables[0].Rows[i]["ElementIndex"] = "-1";    //flag not to look at
                            Console.WriteLine("TS Results not found for element: " + _dsEG_ResultTS_Request.Tables[0].Rows[i]["Element_Label"].ToString());
                            //todo : log the issue
                        }
                    }
                }
                if (bUpdateDBwIndices)
                {
                    //todo: push the vals back to db
                }

                return true;
            }
            catch (Exception ex){
                Console.WriteLine("Error Setting up TS Indices- Check whether rpt file is complete.");
                return false;
            }

        }

        //todo: filter on ds_ResultTs to not load dict for those
        private Dictionary<string, int> TS_LoadIndicesFromRPT(string sRPT_File)
        {
            Dictionary<string, int> dictIndices = new Dictionary<string, int>();
            StreamReader file = new StreamReader(sRPT_File);
            bool bFound = false;
            if (_swmmReportHelper._rptSubcatchments!= swmmReportHelper.RPT_OUT_TYPE.NONE){
                bFound = TS_LoadAllIndices(ref file, ref dictIndices, "Subcatchment Runoff Summary", "Subcatchment", _swmmReportHelper._rptSubcatchments, _swmmReportHelper._lstSubcatchments);

                // if there are no subcatchments, problem: need to seek stream origin
                file.DiscardBufferedData();
                file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin); 
            }
            if (_swmmReportHelper._rptNodes != swmmReportHelper.RPT_OUT_TYPE.NONE)
            {       // change to Node Depth Summary for 5.022... 
                // 11/17/17: back to node depth summary ... need to figure out why this  keeps changing
                bFound = TS_LoadAllIndices(ref file, ref dictIndices, "Node Depth Summary", "Node", _swmmReportHelper._rptNodes, _swmmReportHelper._lstNodes);
            }
            if (_swmmReportHelper._rptLinks != swmmReportHelper.RPT_OUT_TYPE.NONE)
            {       // "Link Flow Summary"
                bFound = TS_LoadAllIndices(ref file, ref dictIndices, "Link Flow Summary", "Link", _swmmReportHelper._rptLinks, _swmmReportHelper._lstLinks);
            }
            
            file.Close();
            return dictIndices;
        }

        /// <summary>
        /// Reads the SWMM indices from the report file.
        /// Runs into issus with no subcatchemnts, so 11/14/16: add ability to signal defeat...
        /// </summary>
        /// <param name="file"></param>
        /// <param name="dictIndices"></param>
        /// <param name="sFindText"></param>
        /// <param name="sPrefix"></param>
        /// <param name="rptType"></param>
        /// <param name="lstVals"></param>
        /// <returns></returns>
        private bool TS_LoadAllIndices(ref StreamReader file, ref Dictionary<string, int> dictIndices, string sFindText, string sPrefix, swmmReportHelper.RPT_OUT_TYPE rptType, List<string> lstVals =null)
        {
            string sBuf = file.ReadLine().Trim();
            bool bFound = false;
            while (!file.EndOfStream && sBuf != sFindText)
            {
                sBuf = file.ReadLine().Trim();
                if (sBuf == sFindText)
                    bFound = true;
            }
            if (file.EndOfStream)               //met 11/14/16: this is to 
            {
                Console.WriteLine("failed to find: " + sFindText);
                return bFound;
            }

            //bojangles: needs to be set dynamically to swmm version
            //changed from 7 to 4 for 5.0.1.1
            // met- back to 7... let's write some real fucking code here!!
            // todo: get this figured out to work across versions you shithead!
            for (int i = 0; i < 7; i++)
            {
                file.ReadLine();
            }
            if (rptType == swmmReportHelper.RPT_OUT_TYPE.ALL)
            {
                ReadAllIndices(ref file, ref dictIndices, sPrefix);
            }
            else{
                ReadSelectIndices(ref file, ref dictIndices, sPrefix, lstVals);
            }
            return bFound;
        }
        public void ReadAllIndices(ref StreamReader file, ref Dictionary<string, int> dictIndices, string sPrefix)
        {
            bool bExit = false;
            int nIndex = 0; string sElementName="";
            while (!bExit)
            {
                string sBuf = file.ReadLine().Trim();
                if (sBuf.Length == 0)
                {
                    bExit = true;
                }
                else
                {
                    sElementName = sBuf.Substring(0, sBuf.IndexOf(' '));
                    try
                    {

                        if (!dictIndices.ContainsKey(sPrefix + sElementName))
                            dictIndices.Add(sPrefix + sElementName, nIndex);
                        nIndex++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ISSUE!");

                    }
                }
            }
        }

        public void ReadSelectIndices(ref StreamReader file, ref Dictionary<string, int> dictIndices, string sPrefix, List<string> lstVals)
        {
            bool bExit = false;
            int nIndex = 0; string sElementName = "";
            while (!bExit)
            {
                string sBuf = file.ReadLine().Trim();
                if (sBuf.Length == 0)
                {
                    bExit = true;
                }
                else
                {
                    sElementName = sBuf.Substring(0, sBuf.IndexOf(' '));
                    if(lstVals.Contains(sElementName)){
                        if (!dictIndices.ContainsKey(sPrefix + sElementName))             //not 100% sure this is required here  
                            dictIndices.Add(sPrefix + sElementName, nIndex);
                        nIndex++;
                    }                 
                }
            }
        }

        private void TS_ProcessElementType()
        {


        }

        /// <summary>
        /// Init TS params based upon report file
        /// Updated 7/24/17: make possible to use the active RPT, instead of base model (for instances like PUSH when geometries vary between models)
        /// </summary>
        /// <param name="sRPT_File"></param>
        /// <param name="bUseCurrentRPT"></param>
        public void Initialize_SWMM_TS_ReportRequest(string sRPT_File, bool bUseCurrentRPT = false)             //ref StreamReader file)
        {
            string sModelFile = _sActiveModelLocation;
            if (bUseCurrentRPT)
                sModelFile = sRPT_File.Replace(".RPT", ".INP");

            ReadINP_RPT_Section(sModelFile);
            TS_SetResultIndices(sRPT_File);      //todo: probably better to read the baseline?
        }
        
 
        /// <summary>
        /// reads [REPORT] into data structure to help identify SWMM TS indices
        /// assumes INPUT/CONTROLS/SUB/NODES/LINK order
        /// 
        /// big note: currently doing this by dyn ref to FILE, not db backend- when db backend more robust, may consider switch
        /// 
        /// 
        /// </summary>
        /// <param name="file"></param>
        public void ReadINP_RPT_Section(string sReportFile)             //ref StreamReader file)
        {
            StreamReader file = new StreamReader(sReportFile);      // met 7/24/17 _sActiveModelLocation);

                string sBuf = file.ReadLine();
                while (!file.EndOfStream && (sBuf.Trim() != "[REPORT]"))
                {
                    sBuf = file.ReadLine();
                }
                if (file.EndOfStream)
                {
                    //todo : log the issue
                    Console.WriteLine("Report section not found! Defaults used...");
                }
                sBuf = file.ReadLine(); //.Split(' ')[1].Trim();       //INPUT val

                if(sBuf.Substring(0,1)==";")            // added check 8/15/16: associated with swmm version 5.1
                    sBuf = file.ReadLine();                 

                sBuf = CommonUtilities.RemoveRepeatingChar(sBuf);
                sBuf = sBuf.Split(' ')[1].Trim(); 
                if (sBuf == "NO") { _swmmReportHelper._bInputInclude = false; }
                else { _swmmReportHelper._bInputInclude = true; }
                sBuf = file.ReadLine();                          //CONTROLS val
                sBuf = CommonUtilities.RemoveRepeatingChar(sBuf);
                sBuf = sBuf.Split(' ')[1].Trim();     
                if (sBuf == "NO") { _swmmReportHelper._bInputInclude = false; }
                else { _swmmReportHelper._bCONTROLSInclude = true; }
                ////

                sBuf = CommonUtilities.RemoveRepeatingChar(file.ReadLine());
                string sVal = sBuf.Split(' ')[1].Trim();
                if (sVal == "ALL")
                {
                    _swmmReportHelper._rptSubcatchments = swmmReportHelper.RPT_OUT_TYPE.ALL;
                    sBuf = CommonUtilities.RemoveRepeatingChar(file.ReadLine()); //read next line- similar to what will happen if reading specifics
                }
                else if (sVal == "NONE")
                {
                    _swmmReportHelper._rptSubcatchments = swmmReportHelper.RPT_OUT_TYPE.NONE;
                    sBuf = CommonUtilities.RemoveRepeatingChar(file.ReadLine()); //read next line- similar to what will happen if reading specifics
                }
                else
                {
                    _swmmReportHelper._rptSubcatchments = swmmReportHelper.RPT_OUT_TYPE.SOME;
                    TS_Indices_ReadList(ref file, ref _swmmReportHelper._lstSubcatchments, ref  sBuf, "NODES");
                }

                sVal = sBuf.Split(' ')[1].Trim();
                if (sVal == "ALL")
                {
                    _swmmReportHelper._rptNodes = swmmReportHelper.RPT_OUT_TYPE.ALL;
                    sBuf = CommonUtilities.RemoveRepeatingChar(file.ReadLine()); //read next line- similar to what will happen if reading specifics
                }
                else if (sVal == "NONE")
                {
                    _swmmReportHelper._rptNodes = swmmReportHelper.RPT_OUT_TYPE.NONE;
                    sBuf = CommonUtilities.RemoveRepeatingChar(file.ReadLine()); //read next line- similar to what will happen if reading specifics
                }
                else
                {
                    _swmmReportHelper._rptNodes = swmmReportHelper.RPT_OUT_TYPE.SOME;
                    TS_Indices_ReadList(ref file, ref _swmmReportHelper._lstNodes, ref sBuf, "LINKS");  //last param is stop_string
                }
            /////////////           LINKS                   //////////////////////////
                sVal = sBuf.Split(' ')[1].Trim();
                if (sVal == "ALL")
                {
                    _swmmReportHelper._rptLinks = swmmReportHelper.RPT_OUT_TYPE.ALL;
              // no more char to eat      sBuf = CommonUtilities.RemoveRepeatingChar(file.ReadLine()); //read next line- similar to what will happen if reading specifics
                }
                else if (sVal == "NONE")
                {
                    _swmmReportHelper._rptLinks = swmmReportHelper.RPT_OUT_TYPE.NONE;
                    sBuf = CommonUtilities.RemoveRepeatingChar(file.ReadLine()); //read next line- similar to what will happen if reading specifics
                }
                else
                {
                    _swmmReportHelper._rptLinks = swmmReportHelper.RPT_OUT_TYPE.SOME;
                    TS_Indices_ReadList(ref file, ref _swmmReportHelper._lstLinks, ref sBuf, "[TAGS]");
                }
                file.Close();
            }
        

        // read multi-line list into the specified list element
        public void TS_Indices_ReadList(ref StreamReader file, ref List<string> lstElement, ref string sBuf, string sStopString)
        {
            bool bEndLoop = false;
            while (!bEndLoop)
            {
                string[] sVals = CommonUtilities.RemoveRepeatingChar(sBuf).Split(' ');
                if ((sBuf.Trim().Length == 0) || (sVals[0].Trim() == sStopString))
                {
                    bEndLoop = true;
                }
                else
                {
                    TS_Indices_LoadList(ref lstElement, sVals);
                    sBuf = file.ReadLine();
                }
            }

        }
        public void TS_Indices_LoadList(ref List<string> lstElement, string[] sVals)
        {
            for (int i = 1; i < sVals.Length; i++) // skip the first value 
            {
                lstElement.Add(sVals[i]);
            }
        }

        #endregion


        /// <summary>
        /// override that allows a direct call to read output if needed.
        /// allows user to override the name of the output file...
        /// </summary>
        /// <param name="nEvalId"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="sOutFile"></param>
        /// <returns></returns>
        public override long ReadTimeSeriesOutput(int nEvalID, int nScenarioID, string sOutFile="UNDEFINED",long nStartIndex=0,long nEndIndex=-1,string sDatasetLabel="1", string sHDF_Name="UNDEFINED")
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
            SetTS_FileName(nScenarioID, Path.GetDirectoryName(sOutFile),false);            
            _hdf5.hdfCheckOrCreateH5(_hdf5._sHDF_FileName);
            _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
            long l = ReadOUTData(_nActiveReferenceEvalID, nScenarioID, sOutFile, nStartIndex, nEndIndex, sDatasetLabel);
            _hdf5.hdfClose();
            return l;
        }

        /// <summary>
        /// Takes a list of storms (and potentially even different input files)
        /// Loop over and extract into one or more HDF file
        /// </summary>
        /// <param name="sCSV_Location"></param>
        /// <param name="sHDFfile"></param>

        public void ReadStormsToHDF5(string sCSV_Location, string sHDFfile="UNDEFINED"){
            DataTable dt = DBContext.GetDataTableFromCsv(sCSV_Location, true);
            foreach (DataRow dr in dt.Rows){
                long lStartIndex = Convert.ToInt64(dr["start_index"].ToString());
                long lEndIndex = Convert.ToInt64(dr["end_index"].ToString());
                string sFile = dr["input_file"].ToString().Replace(".inp",".out");
                string sEvent = dr["Event"].ToString();
                ReadTimeSeriesOutput(-1,-1, sFile, lStartIndex, lEndIndex, sEvent, sHDFfile);
            }
        }



        public long 
            ReadOUTData(int nEvalId, int nScenarioID, string sOutFile, long nStartIndex = 0, long nEndIndex = -1, string sDatasetLabel = "1")           //met 7/31/2011 db pass arg change string sConnRMG)
        {
            long nResult;
            //string sOutFile = "C:\\Users\\mthroneb\\Documents\\Optimization\\Models\\GI_SENS\\v03\\DotOutTest\\swmm_gi_v003_DotOut.out";

            SWMM_OpenOutFile(sOutFile);
            //now we know how many rows to cast the array to.
            int nTS_Records = _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()).Count(); //SP 15-Feb-2017 Primary only
            if (nEndIndex == -1)
                nEndIndex = SWMM_Nperiods;
            long nPeriods = nEndIndex - nStartIndex;

            //performed in SimLInk init       _dResultTS_Vals = new double[nTS_Records][,];
            //performed in SimLInk init       _sTS_GroupID = new string[nTS_Records];                 //store group array which is needed for HDF write
            int nCounter = 0; //SP 15-Feb-2017 Now only used as counter, not index of TS in memory 
            foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString())) //SP 15-Feb-2017 Require primary only
            {
                // Console.WriteLine("reading element {0}", nCounter);
                int nVal = Convert.ToInt32(dr["ElementIndex"] /*SP 15-Feb-2017 removed need for nCounter - _dsEG_ResultTS_Request.Tables[0].Rows[nCounter]["ElementIndex"]*/);
                int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())]; //SP 15-Feb-2017 get index for saving the TS back

                if (nVal != -1)
                {
                    double[,] dvals = new double[nPeriods, 1];                         //hold the current TS Record
                    int nVarType = SWMM_GetOUT_VarType(dr["FeatureType"].ToString());
                    int nResultCode = Convert.ToInt32(dr["ResultIndex"].ToString());
                    int nElementIndex = Convert.ToInt32(dr["ElementIndex"].ToString());
                    double dCatchError = -1;

                    int ncount = 0;
                    for (long i = nStartIndex; i < nEndIndex; i++)
                    {
                        dvals[ncount, 0] = GetSWMMSeries(nVarType, nElementIndex, nResultCode, i + 1);      // use i (period) for the last index
                        ncount++;
                        //  sw_OUT.WriteLine(dr["ResultTS_ID"].ToString() + ", " + i + ", " + nScenarioID.ToString() + "," + GetSWMMSeries(nVarType, nElementIndex, nResultCode, i + 1));

                        if (dCatchError == -666.66)
                        {
                            break;          //met 3/7/2012     exit if the value was not found. this allows us to close and not hang upt he file.
                        }

                    }
                    //     _hdf5_SWMM must be initialized prior to this function call.
                    string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_ID"].ToString(), "SKIP", "SKIP");
                    

                    _sTS_GroupID[nIndex] = sGroupID; //SP 15-Feb-2017 use nIndex instead of nCounter for saving the TS back - should normally be same. Safer

                    if (dCatchError == -666.66)
                    {
                        break;          //met 3/7/2012     exit if the value was not found. this allows us to close and not hang upt he file.
                    }

                    
                    _dResultTS_Vals[nIndex] = dvals;              // add current TS to jagged array  //SP 15-Feb-2017 use nIndex instead of nCounter for saving the TS back - should normally be same. Safer
                    if (nCounter % _nTS_WriteMessageToConsoleFreq == 0)
                        Console.WriteLine("Record {0} read: {1}", nCounter, System.DateTime.Now.ToString());

                    nCounter++;
                }
                else
                {
                    if (true)
                        Console.WriteLine("element {0} ts index not set", dr["Element_Label"].ToString());
                    _log.AddString("element " + dr["Element_Label"].ToString() + " not set", Logging._nLogging_Level_1, true);
                    _dResultTS_Vals[nIndex] = new double[nPeriods, 1]; //SP 15-Feb-2017 use nIndex instead of nCounter for saving the TS back - should normally be same. Safer
                    nCounter++;
                }
            }

            if (true)                                           //optional write to REPO; otherwise keep for event processing
                WriteTimeSeriesToRepo(sDatasetLabel);
            //
            CloseSWMM_OutFile();



            return -1;
        }

        public override void WriteTimeSeriesToRepo(string sDatasetLabel = "1")
        {
            base.WriteTimeSeriesToRepo(sDatasetLabel);
        }

/*        //SP 15-Feb-2017 - overriden function for SWMM to encapsulate the condition :
        //  if (_bSaveSecondaryAndAuxTS && nScenStartAct <= CommonUtilities.nScenLCModelExecuted || nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read)
        protected override void WriteSecondaryAndAUXTimeSeriesToRepo(string sFilePath, string sDatasetLabel = "1")
        {
            //_bScenSaveSecondaryAndAuxTS set within ProcessScenario if there has been a substantive change
            if (_bScenSaveSecondaryAndAuxTS)
            {

                _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);           //met 4/25/17: Datasetlabel was being used as filename...
=======
                _hdf5.hdfOpen(sFilePath, false, true);
>>>>>>> 0b9545632df5cfbbd64c701437645f97b514bd25
                WriteTimeSeriesToRepo(new[] { RetrieveCode.Secondary, RetrieveCode.Aux, RetrieveCode.AuxEG }); //SP 15-Feb-2017 Refactored function - WriteSecondaryAndAuxTS_ToRepo();                
                _hdf5.hdfClose();
            }
        }
*/

        //standard override
        public override DataSet EGDS_GetResultTS(int nEvalID, bool bIncludeAux = false)
        {
            return ReadOut_GetDataSet(nEvalID, bIncludeAux);
        }

        /*  public override DataSet EGDS_GetSplint(int nEvalID)
          {

          }

         */

        // updated 9/21/16 to enable user to request whether they wish to return all TS, or just non-aux (the default, more typical case)
        // met 2/1/17: exlude secondary results explicitly
        //SP 28-Feb-2017 NEEDS TESTING WITH THE ADDITIONAL FIELDS - Modified to keep consistent with Secondary and AUX requests. SQL Server struggles with merge if datasets are not consistent 
        private DataSet ReadOut_GetDataSet(int nEvalId, bool bIncludeAux)
        {
            string sqlFD = "SELECT ResultTS_ID, Result_Label, ElementIndex, ColumnNo as ResultIndex, FeatureType, SWMM_ALIAS as FieldLabel, VarResultType_FK, Element_Label, RetrieveCode, AuxID_FK," //, 1 as ts_code"  // 11/11/13 - add ts_code for quick filter, SP 15-Feb-2017 should not be able to use RetrieveCode
                           + " SQN, CustomFunction, FunctionArgs, RefTS_ID_FK, FunctionID_FK, UseQuickParse"
                            + " FROM (tblResultTS INNER JOIN tlkpSWMMResults_FieldDictionary ON tblResultTS.VarResultType_FK = tlkpSWMMResults_FieldDictionary.ResultsFieldID) LEFT OUTER JOIN tblFunctions ON tblResultTS.FunctionID_FK = tblFunctions.FunctionID"
                           + " WHERE (((EvaluationGroup_FK)=" + nEvalId + ") and (RetrieveCode = " + ((int)RetrieveCode.Primary).ToString() + "))"; //+ " WHERE (((EvaluationGroup_FK)=" + nEvalId + ") and (IsSecondary=0)"; //SP 28-Feb-2017 remove reference to IsSecondary
           
            // met 5/5/17: above code was getting aux info twice...
            // this happends in secondary (merged into ts_request).
            // therefore, this was adjusted to ONLY get the primary
 /*           if (bIncludeAux)                //
                sqlFD += ")";
            else
                sqlFD += "and (RetrieveCode not in (" + ((int)RetrieveCode.Aux).ToString() + ", " + ((int)RetrieveCode.AuxEG).ToString() + ")))"; //sqlFD += "and (IsAux=0))"; //SP 28-Feb-2017 removed reference to IsAux
            /* met 5/31/14: redone to 1) use virtual/override  2) not depend on query    string sqlFD = "SELECT ResultTS_ID, Result_Label, ResultIndex,ElementIndex, FeatureType"
                            + " FROM qryResultTS001_SWMM_OUT WHERE (((EvaluationGroup_FK)=" + nEvalId + "))";  */
            DataSet dsFD = _dbContext.getDataSetfromSQL(sqlFD);
            return dsFD;
        }

        // item types (from SWMM interfacing guide
        public int SWMM_GetOUT_VarType(string sVarType)
        {
            switch (sVarType)
            {
                case "Node":
                    return 1;
                    break;

                case "Subcatchment":
                    return 0;
                    break;
                case "Sys":
                    return 3;
                    break;
                default:
                    return 2;
                    break;
            }
        }

        public void CloseSWMM_OutFile()
        {
            if (b != null)
            {
                b.Close();
                b = null;//
            }
        }

        public void SWMM_OpenOutFile(string sOutFile)
        {
            string sFileName = sOutFile;
            b = new BinaryReader(File.Open(sFileName, FileMode.Open));      //open the streamreader; this needs to be closed later

            long offset;
            long length = (long)b.BaseStream.Length;

            b.BaseStream.Seek(length - 5 * nRecordSize, SeekOrigin.Begin);
            offset0 = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            StartPos = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            SWMM_Nperiods = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            errCode = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            magic2 = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);

            b.BaseStream.Seek(0, SeekOrigin.Begin);
            magic1 = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);

            version = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            SWMM_FlowUnits = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            SWMM_Nsubcatch = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            SWMM_Nnodes = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            SWMM_Nlinks = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            SWMM_Npolluts = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);

            offset = (SWMM_Nsubcatch + 2) * nRecordSize + (3 * SWMM_Nnodes + 4) * nRecordSize + (5 * SWMM_Nlinks + 6) * nRecordSize;
            offset = offset0 + offset;
            b.BaseStream.Seek(offset, SeekOrigin.Begin);            //dubious?

            SubcatchVars = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            b.BaseStream.Seek(SubcatchVars * nRecordSize, SeekOrigin.Current);
            NodeVars = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            b.BaseStream.Seek(NodeVars * nRecordSize, SeekOrigin.Current);
            LinkVars = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            b.BaseStream.Seek(LinkVars * nRecordSize, SeekOrigin.Current);
            SysVars = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);

            offset = StartPos - 3 * nRecordSize;
            b.BaseStream.Seek(offset, SeekOrigin.Begin);
            SWMM_StartDate = BitConverter.ToInt32(b.ReadBytes(sizeof(double)), 0);
            SWMM_ReportStep = BitConverter.ToInt32(b.ReadBytes(nRecordSize), 0);
            BytesPerPeriod = 2 * nRecordSize + (SWMM_Nsubcatch * SubcatchVars + SWMM_Nnodes * NodeVars + SWMM_Nlinks * LinkVars + SysVars) * nRecordSize;

        }
        public double GetSWMMSeries(int iType, int iIndex, int vIndex, long nPeriod)
        {
            try
            {

                long offset = 0;
                offset = StartPos + (nPeriod - 1) * BytesPerPeriod + 2 * nRecordSize;         //removed the period reference because this will go through all the data.
                switch (iType)
                {
                    case 0:                 //"SUBCATCH":
                        offset += nRecordSize * (iIndex * SubcatchVars + vIndex);                   //offset += Convert.ToInt32(nRecordSize) * (iIndex * Convert.ToInt32(SubcatchVars) + vIndex);
                        break;
                    case 1:                 //"NODE":
                        offset += nRecordSize * (SWMM_Nsubcatch * SubcatchVars + iIndex * NodeVars + vIndex);                 //offset += Convert.ToInt32(nRecordSize) * (iIndex * Convert.ToInt32(SubcatchVars) + vIndex);
                        break;
                    case 2:                 //"LINK":
                        offset += nRecordSize * (SWMM_Nsubcatch * SubcatchVars + SWMM_Nnodes * NodeVars + iIndex * LinkVars + vIndex);
                        break;
                    case 3:                 //"SYS":
                        offset += nRecordSize * (SWMM_Nsubcatch * SubcatchVars + SWMM_Nnodes * NodeVars + SWMM_Nlinks * LinkVars + vIndex);
                        break;
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
        #endregion
        #region RPT

        private DataSet LoadResultSummaryDS(int nEvalID)
        {
            //SP 4-Mar-2016 - Requires Testing after changing from using the Query in access
            string sql = "SELECT tblResultVar.Result_ID, tblResultVar.Result_Label, tblResultVar.EvaluationGroup_FK AS EvaluationGroupID, "+
                "tblResultVar.VarResultType_FK, tblResultVar.ElementID_FK, "+
                "tblResultVar.Element_Label, -1.234 as val, " +
                "tlkpSWMMResults_TableDictionary.TableName, tlkpSWMMResults_FieldDictionary.FieldName, "+
                "tlkpSWMMResults_FieldDictionary.FeatureType, tlkpSWMMResults_FieldDictionary.ColumnNo, tlkpSWMMResults_TableDictionary.SectionNumber "+
                "FROM (tblResultVar INNER JOIN tlkpSWMMResults_FieldDictionary ON "+
                "tblResultVar.VarResultType_FK = tlkpSWMMResults_FieldDictionary.ResultsFieldID) "+
                "INNER JOIN tlkpSWMMResults_TableDictionary ON tlkpSWMMResults_FieldDictionary.TableID_FK = tlkpSWMMResults_TableDictionary.ResultTableID " +
                " WHERE (((EvaluationGroup_FK)=" + nEvalID + ")) ORDER BY SectionNumber, ColumnNo, Element_Label;";

            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server
            /*"SELECT Result_ID, Element_Label, Result_Label, VarResultType_FK, FeatureType, FieldName, TableName, SectionNumber, ColumnNo, EvaluationGroupID, ElementID_FK, -1.234 as val FROM qryResultSummary001_SWMM"
                + " WHERE (((EvaluationGroupID)=" + nEvalID + ")) ORDER BY SectionNumber, ColumnNo, Element_Label;";*/

            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        // the function below is for reading ALL results into a datatable; 
        // see vba in .mdb for example of reading specific results; it is very quick for all table (for pilot) so don't necessarily see need at this point.

        //BYM Jan 2012
        //met 3/2/2012: pasted in Baha's function, and replaced HELPER_MOUSE with HELPER_SWMM.
        //todo: general common utilties TEXT file processor functions that do not need to be called from a specific function
        public void ResultsReadReport(string sRPTfile, int nEvalID, int nScenarioID)
        {
            if(_dsEG_ResultSummary_Request != null)
            {
                if (File.Exists(sRPTfile))
                {
                    StreamReader fileINP = null;
                    try
                    {
                        string[] sResults_RPT = File.ReadAllLines(sRPTfile);
                        int nResults = _dsEG_ResultSummary_Request.Tables[0].Rows.Count;

                        string[] sVals = new string[nResults];
                        int[] nIDs = new int[nResults];                     //MET 1/15/2011
                        int count2 = 0;
                        bool bIsCorrectResultSection = false; bool bIsSummaryResult;
                        int nCurrentFilePosition = 32;      //skip header bullshit. probably should advance through file for this.
                        string sCurrentTableName; string sCurrentElementName; int nCurrentResultCol;
                        int nDataVals = 0;
                        string sLastTableName = "set in code below";
                    
                        foreach (DataRow dr in _dsEG_ResultSummary_Request.Tables[0].Rows)                                    //now populate the  counter that will tell us how to read the file in...?
                        {
                            //encapsulate results read in try/catch to help pinpoint errors 
                            string sLogIdentifier = "";
                            try
                            {
                                int nVarType_FK = Convert.ToInt32(dr["VarResultType_FK"].ToString());

                                sCurrentTableName = dr["TableName"].ToString();

                                if (dr["FeatureType"].ToString() != "System")             //met 4/16/2012: 
                                {
                                    sCurrentElementName = dr["Element_Label"].ToString();       //typical case
                                    bIsSummaryResult = false;
                                }
                                else
                                {
                                    sCurrentElementName = dr["FieldName"].ToString();           //find the field name for summmary variables. 
                                    bIsSummaryResult = true;
                                }
                                sLogIdentifier = sCurrentTableName + "," + sCurrentElementName;
                                nCurrentResultCol = Convert.ToInt32(dr["ColumnNo"].ToString());              //Column numbers are indexed from 1 (in the tlkp KEY)
                                if (sCurrentTableName != sLastTableName)
                                {
                                    nCurrentFilePosition = HELPER_SWMMResults_GetTablePosition(sResults_RPT, sCurrentTableName, nCurrentFilePosition);
                                    bIsCorrectResultSection = true;
                                }

                                if (HELPER_SWMMResults_FindElementName(sResults_RPT, sCurrentElementName, sCurrentTableName, ref nCurrentFilePosition, bIsSummaryResult))
                                {      //this function loops to the actual data entry
                                    sVals[nDataVals] = HELPER_SWMMResults_GetResultVal(sResults_RPT[nCurrentFilePosition], nCurrentResultCol);
                                    _dsEG_ResultSummary_Request.Tables[0].Rows[nDataVals]["val"] = Convert.ToDouble(sVals[nDataVals]);      //sim2: store thate val

                                    if (string.IsNullOrEmpty(dr["ElementID_FK"].ToString()))        //test for null (bad problem set up- but don't want to die here)
                                    {
                                        nIDs[nDataVals] = -111666;         //ElementID_FK was not filled out.
                                    }
                                    else
                                    {
                                        nIDs[nDataVals] = Convert.ToInt32(dr["ElementID_FK"].ToString());
                                    }
                                }
                                else
                                {
                                    sVals[nDataVals] = "-666";                    //value was not found. nCurrentFilePosition is set to where it was at beginning
                                    nIDs[nDataVals] = -666;
                                }

                                if (Convert.ToInt32(dr["Result_ID"].ToString()) == 8689)
                                {
                                    int n = 1;
                                }


                                ResultSummaryHelper_AddValToDS(nScenarioID, Convert.ToInt32(dr["Result_ID"].ToString()), nIDs[nDataVals], sCurrentElementName, Convert.ToDouble(sVals[nDataVals]), nVarType_FK);        //add new val to the Result dataset

                                nDataVals++;
                                sLastTableName = sCurrentTableName;
                            }
                            catch (Exception ex)
                            {
                                string sMsg = "Error reading result: " + sLogIdentifier + " msg: " + ex.Message;
                                _log.AddString(sMsg, Logging._nLogging_Level_3, false);
                                Console.WriteLine(sMsg);
                            }

                        }

                        // now insert the records into the database. SP 21-Jul-2016 Moved this out of here to WriteResultsToDB - write back to the database at the end only - missed this previously!
                        //ResultsSummary_WriteToRepo(nScenarioID);

                    }

                    catch (Exception ex)
                    {
                        _log.AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                    }
                    finally
                    {
                        if (fileINP != null)
                            fileINP.Close();
                    }
                }
            }
            else
            {
                _log.AddString("skip result summary read due to null request (likely simlink_lite)", Logging._nLogging_Level_2, true, true);
            }
        }





        public string HELPER_SWMMResults_GetResultVal(string sLineHTML, int nCurrentResultCol)
        {
            string sNoHTML = sLineHTML;
       //not used in SWMM     sNoHTML = cu.StripTagsCharArray(sLineHTML);
             sNoHTML = System.Text.RegularExpressions.Regex.Replace(sNoHTML, @"\s+", " ");
            string[] sbuf = sNoHTML.Trim().Split(' ');
            string sReturn = sbuf[nCurrentResultCol - 1];               //the columns are indexed from 1 in tlkpMouseResults_FieldDictionary
            // check whether there is an scientific numeral througwn in there.
            if (sReturn.Contains("e"))
            {
                try
                {
                    sReturn = double.Parse(sReturn).ToString();
                }
                catch (Exception ex)
                {
                    _log.AddString("error parsing double: " + sReturn, Logging._nLogging_Level_3);
                }
            }
            
            
            
            // if (nCurrentResultCol==sbuf.Length){
            //    sReturn=sReturn.Substring(0,sReturn.Length-10);
            // }
            return sReturn;
        }
        public int HELPER_SWMMResults_GetTablePosition(string[] sResults_RPT, string sCurrentTableName, int nCurrentFilePosition)
        {
            int nStartingPosition = nCurrentFilePosition;
            int nReturnVal = -1;
            while ((nCurrentFilePosition < sResults_RPT.Length) && (nReturnVal < 0))
            {
                if (sResults_RPT[nCurrentFilePosition].ToString().IndexOf(sCurrentTableName) >= 0)
                {      //please don't name your nodes Link Discharge Table
                    nReturnVal = nCurrentFilePosition;
                }
                else
                {
                    nCurrentFilePosition++;
                }
            }
            if (nReturnVal < 0)
            {
                return nStartingPosition;
            }
            else
            {
                return nCurrentFilePosition;
            }
        }

        //this gets us to the correct line of the file.
        //met 4/16/2012: modified to also read sumary information. minor change needed because the summary field name is not followed by "    "
        public bool HELPER_SWMMResults_FindElementName(string[] sResults_RPT, string sCurrentElementName, string sCurrentTableName, ref int nCurrentFilePosition, bool bIsSummaryData)
        {
            int nStartingPosition = nCurrentFilePosition;
            int nReturnVal = -1;
            bool bElementFound = false;
            while ((nCurrentFilePosition < sResults_RPT.Length) && (!bElementFound))
            {
                if (!HELPER_SWMMResults_IsNewResultsTable(sResults_RPT[nCurrentFilePosition].ToString(), sCurrentTableName))
                {
                    if (sResults_RPT[nCurrentFilePosition].ToString().Trim().IndexOf(sCurrentElementName) == 0)         //met 5/14/2012 tightened code to ensure exact match at beginning of line   old : (sResults_RPT[nCurrentFilePosition].ToString().IndexOf(sCurrentElementName) > 0)
                    {      //please don't name your nodes Link Discharge Table
                        int nCharAfterString = Convert.ToInt32(sResults_RPT[nCurrentFilePosition].ToString().Trim().IndexOf(sCurrentElementName)) + sCurrentElementName.Length;                                                   //BYM

                        if ((sResults_RPT[nCurrentFilePosition].ToString().Trim().IndexOf(" ") == nCharAfterString) || bIsSummaryData)                     // MET 10/16/2012: changed to look for single space following name..       prevent quitting when not the actual record name (eg the ds node on a link)
                        {
                            bElementFound = true;
                        }
                        else
                        {                                                       //only index on the else because there could be several data points on a given record
                            nCurrentFilePosition++;
                        }
                    }
                    else
                    {
                        nCurrentFilePosition++;                                      //only index on the else because there could be several data points on a given record
                    }
                }
                else
                {
                    break;                          // have crossed into new table; exit.
                }
            }

            if (!bElementFound)
            {                                     //be nice and check whether person populated database incorrectly .  this should really be in a function
                nCurrentFilePosition = HELPER_SWMMResults_GetTablePosition(sResults_RPT, sCurrentTableName, 59);            //get to the write table
                for (int j = nCurrentFilePosition; j <= nStartingPosition; j++)
                {        // start at 50. shouldn't be any data before then. again, this could be improved.
                    if (!HELPER_SWMMResults_IsNewResultsTable(sResults_RPT[nCurrentFilePosition].ToString(), sCurrentTableName))
                    {
                        if (sResults_RPT[nCurrentFilePosition].ToString().IndexOf(sCurrentElementName) > 0)
                        {      //please don't name your nodes Link Discharge Table
                            int nCharAfterString = Convert.ToInt32(sResults_RPT[nCurrentFilePosition].ToString().Trim().IndexOf(sCurrentElementName)) + sCurrentElementName.Length;   
                            if ((sResults_RPT[nCurrentFilePosition].ToString().Trim().IndexOf(" ") == nCharAfterString) || bIsSummaryData) //BYM: I need to find a better flag   met 5/14/17: don't know what orig code was doing but was missing special case stuff
                            {   // the html should close the tag
                                bElementFound = true;
                                break;
                            }
                            else
                            {                                                       //only index on the else because there could be several data points on a given record
                                nCurrentFilePosition++;
                            }
                        }
                        else
                        {
                            nCurrentFilePosition++;                                      //only index on the else because there could be several data points on a given record
                        }
                    }
                }
            }
            return bElementFound;
        }
        private bool HELPER_SWMMResults_IsNewResultsTable(string sLine, string sTable)
        {
            if (sLine.ToString().IndexOf("Summary") >= 0 && sLine.ToString().IndexOf(sTable) < 0)
                return true;
            else
                return false;
            //return (sLine.ToString().IndexOf("Summary") >= 0);                                         //BYM: I need to find a better flag
        }

        #endregion

        #region TS_INDEX
        
        // assumes EG is already set
        public void UpdateResultTS_Indices(){
            string sPath; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName; string sINIFile; string sSummaryFile; string sOUT;string sTS_Filename = "";
            ScenarioPrepareFilenames(_nActiveBaselineScenarioID, _nActiveEvalID, _sActiveModelLocation, out  sTargetPath, out sIncomingINP, out sTarget_INP, out  sSummaryFile, out  sOUT, out  sINIFile);
            bool bReportAll =true;
            DataSet dsResultTS = GetResultTS_DS();



        }

        private DataSet GetResultTS_DS(){
            string sSQL = "SELECT tblResultTS.Result_Label, tblResultTS.Element_Label, tblResultTS.VarResultType_FK, tblResultTS.ElementIndex, tblResultTS.ElementID_FK, tlkpSWMMResults_FieldDictionary.IsOutFileVar, tlkpSWMMResults_FieldDictionary.FeatureType"
                        +" FROM tblResultTS INNER JOIN tlkpSWMMResults_FieldDictionary ON tblResultTS.VarResultType_FK = tlkpSWMMResults_FieldDictionary.ResultsFieldID"
                        + " WHERE (((tlkpSWMMResults_FieldDictionary.IsOutFileVar)=" + _dbContext.GetTrueBitByContext() 
                        + ") AND ((tblResultTS.EvaluationGroup_FK)="+ _nActiveEvalID + "))"
                        + " ORDER BY tblResultTS.Element_Label";

            return null;
        }
        #endregion


        #endregion

        #endregion

        #region MODIFY
        //11/12/2013: updated to be consistent with DAL
        //public string SWMM_Update_INP(string sINP_File, int nScenarioID, string sOptionalOutput_TextFile = "nothing", bool bUpdateCRF = true) //BYM2012
        // met 4/19/2012: updated to support "scenario specific" edits to INP (eg Option table)

        public string Update_INP(string sINP_File, int nScenarioID, string sOptionalOutput_TextFile = "nothing", bool bCleanTargetOfRepeating = true)
        {
            Debug.Print("begin SWMM_Update_INP");
            if (File.Exists(sINP_File))
            {
                Debug.Print("INP Exists");
                StreamReader fileMEX = null;
                string sLogForException = "";
                try
                {
                    string[] sTextFile_ALL = File.ReadAllLines(sINP_File);

                    //string sql = "SELECT ElementName, Val, ScenarioID_FK, TableName, FieldName, SectionNumber, SectionName, FieldINP_Number, TableClass, ElementID, KeyColumn FROM qryRMG001_SWMM_ModelChanges WHERE (((ScenarioID_FK)=@Scenario)) ORDER BY SectionNumber, ElementName;";         //BYM2012
                    // DataSet dsRMG_Changes = null;

                    IEnumerable<simLinkModelChangeHelper> ModelChangesIEnum = Updates_GetChangeDS(nScenarioID);       //bojangles...  needs to be from mem!!!

                    /*        if (true)       //stupid debug
                            {
                                foreach (simLinkModelChangeHelper slm in ModelChangesIEnum)
                                {
                                    string sOut = slm._s + ", " + slm._nSectionNumber + ", " + slm._sElementName + ", " + slm._sVal;
                                    Debug.Print(sOut);
                                }

                            }
                     * */

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
                        string sCurrentSectionName = "none"; string sCheckSectionName = ""; string sCurrentElementName = "none";
                        int nCurrentWriteLine = 0;
                        int nFileTotalRows; int nCurrentChange = 0;
                        int nSectionBeginLine = 0; //BYM2012    -met renamed to make more clear


                        Debug.Print("Total Changes: " + nTotalChanges);

                        if ((!bIsInsert) && (nListOffset > 0))             //update loop, and at lest one insert; need to convert list back
                        {
                            sTextFile_ALL = listTextFile_ALL.ToArray();         //
                        }
                        nFileTotalRows = sTextFile_ALL.Length;

                        simLinkModelChangeHelper slmCurrent = new simLinkModelChangeHelper();
                        if (nTotalChanges > 0)
                            slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                        while (nCurrentChange < nTotalChanges)

                        //foreach (simLinkModelChangeHelper slmCurrent in ModelChangesIEnum)
                        {

                            //met 1/2/2013: check that we do ONLY inserts on inserts loop and ONLY updates on update loop
                            if (bIsInsert == slmCurrent._bIsInsert)
                            {

                                if (slmCurrent._nVarType_FK == 342)             //debug only
                                {
                                    int nDEBUG = 1;
                                }

                                //MET 4/13/2012: 
                                if (slmCurrent._nVarType_FK == -1)        //perform insert (don't go through all the "section name" stuff
                                {
                                    Debug.Print("Table FieldID = -1; is there an issue?");
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
                                {   // go find right section; return BAD_DATA if not found 

                                    // met 8/6/17: Special case- hot start file.
                                    if (slmCurrent._nVarType_FK == -100)
                                    {
                                        slmCurrent._sSectionName = "FILES";
                                    }
                                    sLogForException = string.Format("id {0} field: {1} val {2}", slmCurrent._sElementName, slmCurrent._sFieldName, slmCurrent._sVal);
                                    sCurrentSectionName = AdvanceToCurrentINPSection(ref sTextFile_ALL, ref nCurrentWriteLine, ref nSectionBeginLine, sCurrentSectionName, slmCurrent._sSectionName, nFileTotalRows, out bInCurrentSection);

                                    if (sCurrentSectionName == slmCurrent._sSectionName)
                                    {
                                        if (bIsInsert)      //don't need to find right location; insert now  (new 1/2/2013)
                                        {
                                            if (nListOffset == 0)
                                            {
                                                listTextFile_ALL = sTextFile_ALL.ToList();  //create the list. we only want to do this if needed, because it takes considerable time.
                                            }
                                            if (!bInCurrentSection)         //first time we navigate to new section
                                            {                               //only advance below until we leave section.
                                                UpdateHelper_AdvanceToData(ref sTextFile_ALL, ref nCurrentWriteLine);     //advance to data section (some have 2 header, some 3- this is easiest.
                                                bInCurrentSection = true;
                                            }

                                            listTextFile_ALL.Insert(nCurrentWriteLine + nListOffset + 1, slmCurrent._sVal);
                                            if (slmCurrent._nVarType_FK == _nFieldDict_LID_USAGE_ID)
                                            {
                                                LID_Dict_InsertNew(slmCurrent._sVal);       //new LID usage- needs to be added to the DICT
                                            }
                                            //met: off by one: nCurrentWriteLine + nListOffset-1,
                                            //   nCurrentWriteLine++;                      met: think this is not needed. offset handles
                                            nCurrentChange++;
                                            nListOffset++;              //this counter keeps track of how many additional inserts there are

                                            if (nCurrentChange < nTotalChanges)
                                            {
                                                slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                            }

                                        }
                                        // added special section for dealing with hot start.
                                        else if (slmCurrent._nVarType_FK == 499)
                                        {
                                            _log.AddString(string.Format("Replacing hotstart with {0}", _sUserFileUpdateKey), Logging._nLogging_Level_2, false, true);
                                            _log.AddString("Note to user: hotstart replacement only works with there is a single 'use' declartion in [files] card. To be improved", Logging._nLogging_Level_1, false, true);
                                            string sID = UpdateHelper_AdvanceToCurrent_ID(ref sTextFile_ALL, "USE", ref nCurrentWriteLine, nSectionBeginLine, sCurrentSectionName);
                                            sTextFile_ALL[nCurrentWriteLine] = "USE HOTSTART " + _sUserFileUpdateKey;  // todo: handle case where multiple interfacing files.
                                            nCurrentChange++;
                                            if (nCurrentChange < nTotalChanges)
                                                slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                        }
                                        else
                                        {                   //case : update loopo.
                                            bInCurrentSection = true;

                                            if (sTextFile_ALL[nCurrentWriteLine].IndexOf(" ") > 0)                   //check whether we have data row
                                            {
                                                string sIDName = "";

                                                //TODO:!!  need to store the position of the SectionName. This allows
                                                //there is a chance it may work right now, but it should not be considered implemented


                                                //test for whether we are using a "system/scenario specific var (eg option table)
                                                //or (more standard) an element name.
                                                bool bIsScenarioLevelVar = false; string sFindElementNameOrField;
                                                sFindElementNameOrField = UpdateHelper_GetElementNameOrField(slmCurrent, ref bIsScenarioLevelVar);

                                                sIDName = UpdateHelper_AdvanceToCurrent_ID(ref sTextFile_ALL, sFindElementNameOrField, ref nCurrentWriteLine, nSectionBeginLine, sCurrentSectionName); //BYM
                                                Debug.Print("Found CurrentID: " + nCurrentChange);
                                                if (sIDName != "No_ID_Found")
                                                {
                                                    //BYM string[] sbufDATA = sTextFile_ALL[nCurrentWriteLine].Split(',');
                                                    string[] sbufDATA = sTextFile_ALL[nCurrentWriteLine].Trim().Split(' ');             //met 4/18/2013 drop leading zero

                                                    int nLastRow = 1;                                                                      //met 6/18/2012: modified to support multiple row values. Val of 1 is the default.
                                                    int nCurrentRow = 1;
                                                    int nCurrentTableID = slmCurrent._nTableID;                                         //check that we are on the same table  met 10/21/14
                                                    //met 1/4/2013: modified to check thta the next ID has the right type.
                                                    while ((sIDName == UpdateHelper_GetElementNameOrField(slmCurrent, ref bIsScenarioLevelVar)) && (bIsInsert == Convert.ToBoolean(slmCurrent._bIsInsert)) && (nCurrentTableID == slmCurrent._nTableID))        //added check that same table applies.
                                                    {
                                                        bool bCorrectRow = true;

                                                        //LID USAGE- can be multiple different types on a single subcatchment
                                                        if (slmCurrent._sQualifier1 != "-1")
                                                        {
                                                            int nCol_Qual = slmCurrent._nQual1Pos;     // Convert.ToInt32(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["Qualifier1POS"].ToString());
                                                            if (nCol_Qual > -1)
                                                            {
                                                                bCorrectRow = false;
                                                                while (!bCorrectRow && sbufDATA[0] == sIDName)
                                                                {
                                                                    if (sbufDATA[nCol_Qual] == slmCurrent._nQual1Pos.ToString())
                                                                    {
                                                                        bCorrectRow = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        nCurrentWriteLine++;
                                                                        sbufDATA = CommonUtilities.RemoveRepeatingChar(sTextFile_ALL[nCurrentWriteLine]).Split(' ');
                                                                    }

                                                                }
                                                            }
                                                            else
                                                            {
                                                                // log   _log.AddString("Potential error- DV has Qualifer but does not define column position", Logging._nLogging_Level_2);
                                                            }
                                                        }


                                                        if (bCorrectRow)
                                                        {
                                                            if (slmCurrent._sElementName == "P_DPS3_AB-12ftWoodScrew-B")
                                                            {
                                                                int n=1;
                                                            }
                                                            if (slmCurrent._nRecordID == 3024)
                                                            {
                                                                int nn = 1;
                                                            }

                                                            nCurrentRow = slmCurrent._nRowNo;
                                                            if (nCurrentRow > nLastRow)                                                         // if this is true, we need to get the new row.
                                                            {
                                                                nCurrentWriteLine += nCurrentRow - nLastRow;
                                                                sbufDATA = CommonUtilities.RemoveRepeatingChar(sTextFile_ALL[nCurrentWriteLine]).Split(' ');                      //remove the repeating spaces so the split works (occurs in HELPER_SWMM_AdvanceToCurrent_ID for normal workflow)
                                                            }

                                                            int index = slmCurrent._nFieldNumber;

                                                            // met update hydrograph section - added 11/6/17: two diff syntax to manage!! wild.
                                                            if(IsHydrographs(slmCurrent._sFieldName)){
                                                                UpdateINP_Hydrograph(ref slmCurrent, ref nCurrentWriteLine,ref sTextFile_ALL);
                                                                bCorrectRow = false;        // no longer on correct row
                                                                // at this point, the change is successfully made, or FAILED (and logged) so advance the counter.
                                                                nCurrentChange++;
                                                                if (nCurrentChange < nTotalChanges)
                                                                    slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);

                                                                bCorrectRow=false;  // after update, no longer on correct row.
                                                                                //unlike other updates, this does not handle multiple row updates at the same time.
                                                            }
                                                            // met 10/25/17: add code to deal with 1:many case
                                                            // this is driven by need to mulitply pump curves.
                                                            // In this case, we want to apply the change to all the records below that have this case.

                                                            else if (IsSWMM_1toMany(slmCurrent._sFieldName))
                                                            {
                                                                bool bIsFirstPass = true;
                                                                double dVal = -1;
                                                                try
                                                                {
                                                                    while (sbufDATA[0].Trim() == sIDName)
                                                                    {
                                                                        if (bIsFirstPass)   // only log the issue once
                                                                        {
                                                                            _log.AddString(string.Format("Adjusting SWMM table, multiplier: table {0}, multiplier {1}", sIDName, slmCurrent._sVal), Logging._nLogging_Level_3, true, false);
                                                                        }
                                                                        dVal = Convert.ToDouble(sbufDATA[index - 1]);

                                                                        dVal = dVal * Convert.ToDouble(slmCurrent._sVal);
                                                                        sbufDATA[index - 1] = dVal.ToString();
                                                                        sTextFile_ALL[nCurrentWriteLine] = String.Join(" ", sbufDATA);  // add back the corrected value
                                                                        if (bIsFirstPass)
                                                                        {
                                                                            index--;        // only the first row has an extra name...
                                                                            bIsFirstPass = false;
                                                                        }
                                                                        
                                                                        nCurrentWriteLine++;
                                                                        sbufDATA = CommonUtilities.RemoveRepeatingChar(sTextFile_ALL[nCurrentWriteLine]).Split(' ');
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    _log.AddString(string.Format("Error adjusting SWMM table multiplier, : table {0}, multiplier {1}", sIDName, slmCurrent._sVal), Logging._nLogging_Level_1, true, false);
                                                                }
                                                                bCorrectRow = false;        // no longer on correct row
                                                                // at this point, the change is successfully made, or FAILED (and logged) so advance the counter.
                                                                nCurrentChange++;
                                                                if (nCurrentChange < nTotalChanges)
                                                                    slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                                            }
                                                            else
                                                            {   //original code path (pre 1toMany fix on 10/26/17
                                                                // this advances to the next option
                                                                sbufDATA[index - 1] = slmCurrent._sVal;
                                                                nCurrentChange++;
                                                                sTextFile_ALL[nCurrentWriteLine] = String.Join(" ", sbufDATA);  //moved from XAX below
                                                                if (nCurrentChange < nTotalChanges)
                                                                    slmCurrent = ModelChangesIEnum.ElementAt(nCurrentChange);
                                                            }
                                                            if (nCurrentChange == nTotalChanges) { break; }    // no more changes, so break out of loop
                                                            Debug.Print(nScenarioID + ": " + nCurrentChange);      //met 3/1/2012   figure out why the file is sometimes not getting written.
                                                        }
                                                    }
           //XAX issue for NOLA ?                                         if (!IsSWMM_1toMany(slmCurrent._sFieldName))    // 1 to many handled in the loop above... is the exception.
                            //                        {
                             //                           sTextFile_ALL[nCurrentWriteLine] = String.Join(" ", sbufDATA);
                              //                      }


                                                    int aa = 1;
                                                }
                                                else
                                                {
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
                                        Debug.Print("MODEL CHANGE NOT TRANSLATED TO INP: " + nScenarioID + " :" + slmCurrent._sElementName + ", " + slmCurrent._sVal);
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
                    //standard update changes are made at this point. now we need to back and insert any values.
                    //               HELPER_Mouse_InsertNewElementFeatures(ref sTextFile_ALL, nScenarioID);

                    string sOUT;
                    if (sOptionalOutput_TextFile == "nothing")
                    {
                        sOUT = sINP_File;
                    }
                    else
                    {
                        sOUT = sOptionalOutput_TextFile;

                    }

                    Debug.Print("Write: " + sOUT);
                    File.WriteAllLines(sOUT, sTextFile_ALL);              //overwrite the file initially passed

                }
                catch (Exception ex)
                {

                    _log.AddString("Error updating swmm file " + sLogForException, Logging._nLogging_Level_1);
                    _log.AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                    int n = 1;
                }
                finally
                {
                    if (fileMEX != null)
                        fileMEX.Close();
                }
            }
            else
            {
                _log.AddString("Note: baseline file is not found in correct location. Check reference file location.", Logging._nLogging_Level_1, true, true);
            }

            return "crap";
        }

        /// <summary>
        /// helper function to identify if var type is a hydrograph (better: get from tlkp?!)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool IsHydrographs(string sFieldName){
            string[] sKeys = new string[]{"R1","R2", "R3","T1","T2", "T3","K1","K2", "K3"};
            int pos = Array.IndexOf(sKeys, sFieldName);
            if (pos > -1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Code to update a block of rtk values
        /// </summary>
        /// <param name="slmCurrent"></param>
        /// <param name="nCurrentWriteLine"></param>
        /// <param name="sTextFile_ALL"></param>
        /// <param name="sbufDATA"></param>
        private void UpdateINP_Hydrograph(ref simLinkModelChangeHelper slmCurrent, ref int nCurrentWriteLine, ref string[] sTextFile_ALL, bool bMultiplyVal = false)  //, ref string sbufDATA,
        {
            if (slmCurrent._nVarType_FK <= 50)
            {
                _log.AddString(string.Format("Begin handling old-type RTK (12 col), element {0}", slmCurrent._sElementName), Logging._nLogging_Level_2, true, false);
                nCurrentWriteLine++;    //eat one row of header
                for (int i = 0; i < 12; i++)  // loop over 12 mo
                {
                    string[] sVals = CommonUtilities.RemoveRepeatingChar(sTextFile_ALL[nCurrentWriteLine]).Split(' ');         //sTextFile_ALL[nCurrentWriteLine].Split(' ');
                    if (!bMultiplyVal)
                        sVals[slmCurrent._nFieldNumber] = slmCurrent._sVal;
                    else
                    {
                        double dVal = Convert.ToDouble(slmCurrent._sVal) * Convert.ToDouble(sVals[slmCurrent._nFieldNumber]);
                        sVals[slmCurrent._nFieldNumber] = dVal.ToString();
                    }
                    sTextFile_ALL[nCurrentWriteLine] = string.Join(" ", sVals);
                    nCurrentWriteLine++;
                }
            }
            else
            {
                _log.AddString(string.Format("New type RTK not yet handled"), Logging._nLogging_Level_Debug, true, false);
            }
        }

        /// <summary>
        /// Return true if the swmm field value to change is in a special list.
        /// </summary>
        /// <param name="sField"></param>
        /// <returns></returns>
        private bool IsSWMM_1toMany(string sField)
        {
            var lstSWMM_1toMany = new List<string>();
            lstSWMM_1toMany.Add("XValue");                         // note: it would be better if this is defined on field in tlkpswmm vartype
            lstSWMM_1toMany.Add("YValue");
            lstSWMM_1toMany.Add("ADD_ADDTL_HERE");
            if (lstSWMM_1toMany.Contains(sField))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //met 6/11/14: catch case where INP sections out of order
        //7/11/14: added bIsNewSectionName
        private string AdvanceToCurrentINPSection(ref string[] sTextFile_ALL, ref int nCurrentWriteLine, ref int nSectionBeginLine,  string sCurrentSectionName, string sTargetSectionName, int nFileTotalRows, out bool bAlreadyInCurrentSection)
        {
            int nStartingWriteLine = nCurrentWriteLine;
            //1: check current section; if target, return it
            sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");
            if (sCurrentSectionName == sTargetSectionName)
            {
                bAlreadyInCurrentSection = true;
                return sCurrentSectionName;
            }
            //2: search to EOF 
            while ((sCurrentSectionName != sTargetSectionName) && (nCurrentWriteLine < nFileTotalRows))
            {
                //sCurrentSectionName = MOUSE_CheckSectionName(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName);                                                      //BYM
                sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                        //BYM
                nCurrentWriteLine++;
                nSectionBeginLine = nCurrentWriteLine;
             //   bInCurrentSection = false;
            }
            if (sCurrentSectionName == sTargetSectionName)
            {
                bAlreadyInCurrentSection = false;
                return sCurrentSectionName;
            }
            //3: search from BOF

            nCurrentWriteLine = 0;
            while ((sCurrentSectionName != sTargetSectionName) && (nCurrentWriteLine < nStartingWriteLine))
            {
                //sCurrentSectionName = MOUSE_CheckSectionName(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName);                                                      //BYM
                sCurrentSectionName = CommonUtilities.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                        //BYM
                nCurrentWriteLine++;
                nSectionBeginLine = nCurrentWriteLine;
              //  bInCurrentSection = false;
            }

            if (sCurrentSectionName == sTargetSectionName)
            {
                bAlreadyInCurrentSection = false;
                return sCurrentSectionName;
            }
            else
            {
                bAlreadyInCurrentSection = true;
                return CommonUtilities._sBAD_DATA;          //not found! check INP FILe 

            }
        }

        //try to address sort error
 /*       private IEnumerable<simLinkModelChangeHelper> Updates_GetChangeDS2(int nScenarioID)
        {
            string sql = "SELECT ElementName, Val, ScenarioID_FK, TableName, FieldName, SectionNumber, SectionName, FieldINP_Number, ElementID, KeyColumn, TableFieldKey_FK, DV_ID_FK, IsScenarioSpecific, RowNo, Qualifier1, Qual1Pos, IsInsertFeature"
                               + " FROM qryMEV001_SWMM WHERE ((ScenarioID_FK)=" + nScenarioID + ") ORDER BY SectionNumber, ElementName, Model_ID;";                     //BYM2012
            DataSet dsModelChanges = _dbContext.getDataSetfromSQL(sql);
            IEnumerable<simLinkModelChangeHelper> ModelChangesList;
            foreach (DataRow dr in dsModelChanges.Tables[0].Rows)
            {
         //       ModelChangesList.OrderBy()


            }
        }*/ 

        //todo: this should be modified to pull directly from memory as well if needed.
        //met changed to 'Qual1Pos' because of SQL SERVER error
        private IEnumerable<simLinkModelChangeHelper> Updates_GetChangeDS(int nScenarioID)
        {
        //    Dictionary <int, swmmTableHelper> dictLocal = _dictSL_TableNavigation.OfType<swmmTableHelper>();
            
       //     IEnumerable<simlinkTableHelper>

            IEnumerable<simLinkModelChangeHelper> ModelChangesList =  from ModelChanges in _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.MEV)
                                                                    .Where(x => x._nScenarioID == nScenarioID).AsEnumerable()               //which performance to characterize
                              join SWMM_DICT in _dictSL_TableNavigation.AsEnumerable()
                              on ModelChanges._nVarType_FK  equals
                              SWMM_DICT.Key
                                   orderby SWMM_DICT.Value._nSectionNumber, ModelChanges._nElementID
                           //        orderby ModelChanges._nElementID
                        //           orderby ModelChanges._nElementID
                           //        orderby ModelChanges._nRecordID
                              select new simLinkModelChangeHelper
                            
                              {
                                  _sVal = ModelChanges._sVal,
                                  _sElementName = ModelChanges._sElementName,
                                  _nElementID = ModelChanges._nElementID,
                                  _nRecordID = ModelChanges._nRecordID,
                                  _nSectionNumber = SWMM_DICT.Value._nSectionNumber,
                                  _sSectionName = SWMM_DICT.Value._sSectionName,
                                  _nTableID = SWMM_DICT.Value._nTableID,
                                  _sTableName = SWMM_DICT.Value._sTableName,
                                  _sFieldName = SWMM_DICT.Value._sFieldName,
                                  _nFieldNumber = SWMM_DICT.Value._nFieldNumber,
                                  _nRowNo = SWMM_DICT.Value._nRowNo,
                                  _nVarType_FK = SWMM_DICT.Value._nVarType_FK,
                                  _bIsScenarioSpecific = SWMM_DICT.Value._bIsScenarioSpecific,
                                  _sQualifier1 = "-1",                  //todo : figure out how the qulifier info is obtained (or if there is a better way to do this)
                                  _nQual1Pos = -1,
                                  _bIsInsert = ModelChanges._bIsInsert
                              }
                              ;
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
        private void UpdateHelper_AdvanceToData(ref string[] sTextFile_ALL, ref int nCurrentWriteLine)
        {
            bool bExit = false;
            int nCountList = sTextFile_ALL.Length;
            while (!bExit && (nCurrentWriteLine < nCountList))
            {
                nCurrentWriteLine++;                            //index whether found or not
                if (sTextFile_ALL[nCurrentWriteLine].IndexOf(";;--") >= 0)
                {
                    bExit = true;
                }
            }
        }


        //helper function (formerly SWMM_INP_Helper_GetElementNameOrField)
        //generally returns element name, except in cases where we're dealing with a scenario specific table (eg options)
        //also sets the bool in case needed later
        public string UpdateHelper_GetElementNameOrField(simLinkModelChangeHelper slm, ref bool bIsScenarioLevelVar)
        {
            string sFindElementNameOrField;
            if (!slm._bIsScenarioSpecific)             //met 1/17/14      //Convert.ToInt32(dr["IsScenarioSpecific"].ToString()) == 0)             //met 4/16/2012: 
            {
                sFindElementNameOrField = slm._sElementName;       //typical case
                bIsScenarioLevelVar = true;

                if (slm._sSectionName == "LID_USAGE")
                {
                    if (_dictSWMM_LID_Usage.ContainsKey(slm._nElementID.ToString()))
                    {
                        sFindElementNameOrField = _dictSWMM_LID_Usage[slm._nElementID.ToString()];
                    }
                    else
                    {
                        sFindElementNameOrField = _dictSWMM_LID_Usage[slm._sElementName.ToString()];
                    }
                    
                }

            }
            else
            {
                sFindElementNameOrField = slm._sFieldName;              // dr["FieldName"].ToString();           //find the field name for summmary variables. 
                bIsScenarioLevelVar = false;
            }
            return sFindElementNameOrField;

        }

        //met 5/14/2014
        private bool IsNewSectionName(string sBuf)
        {
            if (sBuf.Length == 0)
                return false;
            else if (sBuf.Trim().Substring(0,1) == "[")
                return true;
            else
                return false;
        }

        //formerly HELPER_SWMM_AdvanceToCurrent_ID
        //5/14/2014: check for goto new section.
        public string UpdateHelper_AdvanceToCurrent_ID(ref string[] sTEXT_File, string sFindID, ref int nCurrentFilePosition, int nSectionStartIndex, string sSectionName)
        {
            int nStartingPosition = nCurrentFilePosition; string sbuf; int nID_Index = 0;
            string sReturn = "No_ID_Found"; bool bFound = false; bool bNewSection = false;
            while ((nCurrentFilePosition < sTEXT_File.Length) && (!bFound) && (!bNewSection))
            {
                nID_Index = 0;        //typical case
                 
                //sim2: only change if ID is found              sTEXT_File[nCurrentFilePosition] = CommonUtilities.RemoveRepeatingChar(sTEXT_File[nCurrentFilePosition]);                                  //BYM2012
                

                if (!IsNewSectionName(sTEXT_File[nCurrentFilePosition].Trim()))
                {     //CHECK FOR NEW SECTION 
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
                else
                {
                    bNewSection = true;
                }
}
            if (!bFound)
            {
                for (int i = nSectionStartIndex; i < nStartingPosition; i++)
                {
                    nCurrentFilePosition = i;
                                   //BYM2012
                    string sFirstChar= sTEXT_File[nCurrentFilePosition].Trim().Substring(0,1);

                    if (!IsNewSectionName(sTEXT_File[nCurrentFilePosition].Trim()))
                    {     //CHECK FOR NEW SECTION 
                        sbuf = UpdateHelper_GetIDFromDataRow(sTEXT_File[nCurrentFilePosition].ToString(), nID_Index, sSectionName); 
                        if (sbuf == sFindID)
                        {
                            sReturn = sFindID;
                            sTEXT_File[nCurrentFilePosition] = CommonUtilities.RemoveRepeatingChar(sTEXT_File[nCurrentFilePosition]);
                            bFound = true;          //we have found the ID exit loop
                            break;
                        }
                        else
                        {
                            nCurrentFilePosition++;
                        }
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
        public string UpdateHelper_GetIDFromDataRow(string sbuf, int nID_Column = 0, string sSectionName ="NOTHING")
        {
            sbuf = sbuf.Trim();         //met 4/18/2013: first  blank was causing problems.

            int nSpaceIndex = -1;
            if (nID_Column == -1)           //this is a special case where no comma exists; just send back cleaned id field.
            {
                return CleanIDField(sbuf);
            }

            if (sSectionName=="LID_USAGE")      //concatenate first two columns
            {
                if (sbuf=="")
                {
                    return "No_ID_Found";
                }
                
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
           
            //BYM nCommaIndex = sbuf.IndexOf(",");
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
        }
        private string CleanIDField(string sID)
        {
            //BYM string sReturn = sID.Substring(sID.IndexOf('=') + 1, sID.Length - sID.IndexOf('=') - 1).Trim();
            string sReturn = sID.Substring(sID.IndexOf(' ') + 1, sID.Length - sID.IndexOf(' ') - 1).Trim();
            return CleanMEXString(sReturn);
        }

        private string CleanMEXString(string sbuf)
        {
            //BYM2012 char[] nogood = { '\'' };
            char[] nogood = { ' ' };
            return sbuf.TrimStart(nogood).TrimEnd(nogood);
        }

        //todo: write the actual file
        private void WriteTimeSeriesDatFile(string sFullPathName, double[,] dTS_Vals, TimeSeries.TimeSeriesDetail tsdResultTS, string sStationID, string sFirstLine)
        {
            List<string> lstOut = new List<string>();
            lstOut.Add(sFirstLine);
            DateTime dtCurrentTime = tsdResultTS._dtStartTimestamp;
            int nSecInterval = TimeSeries.GetSecInterval(tsdResultTS._tsIntervalType,tsdResultTS._nTSInterval);
            for (int i = 0; i < dTS_Vals.GetLength(0); i++)
            {
                string sDateString_SWMM_DAT = TimeSeries.Convert_DateTimeToSWMM_DAT_String(dtCurrentTime);
                string sLine = sStationID + "\t" + sDateString_SWMM_DAT + "\t" + dTS_Vals[i,0];
                if (dTS_Vals[i, 0] != 0 )
                    lstOut.Add(sLine);
                dtCurrentTime = dtCurrentTime.AddSeconds(nSecInterval);
            }
            File.WriteAllLines(sFullPathName, lstOut.ToArray());   
        }  
        #endregion
        #region RunProcessing

        //ProcessEvaluationGroup(int nEvalID, int nModelTypeID, int nRefScenarioID = -1, bool bSkipDictionaryPopulate = false)
        //SP 5-Aug-2016 Can now use the virtual procedure in SimlinkScenario.cs - commented out to avoid making changes to this specific class when can now be using virtual class
        /*public override void ProcessEvaluationGroup(string[] astrScenarioId2Run)
        {
            DataSet dsEvals = ProcessEG_GetGS_Initialize(_nActiveEvalID, astrScenarioId2Run);       //, nRefScenarioID);
            //now performed in init... LoadReferenceDatasets();            //initialize datasets

            //SP 25-Jul-2016 now can be read from config
            //string sLogPath = System.IO.Path.GetDirectoryName(_sActiveModelLocation);
            //sLogPath = sLogPath.Substring(0, sLogPath.LastIndexOf("\\")) + "\\LOGS";

            foreach (DataRow dr in dsEvals.Tables[0].Rows)
            {
                ProcessScenario(dr, _sLogLocation_Simlink);
                _log.WriteLogFile(true);            
            }
            // now process the remainder of the condor loop
            if (_compute_env != ComputeEnvironment.LocalMachine)
            {
                //test 1: wait for all jobs to come bac (Easiest to get from hpc wrap
                //test 2 (future): loop over jobs and check, as time reqs may vary 

                //test1
                //todo : put in soe testing, or breaks, or something. 
                _hpc.WaitForAllJobs();
                foreach (DataRow dr in dsEvals.Tables[0].Rows)
                {
                    ProcessScenario(dr, _sLogLocation_Simlink);
                    _log.WriteLogFile(true);            //writes a new log file- better to write to the first one.  not critical path
                }
            }
        }*/




     /*   private void ProcessLinkedScenarios()
        {
            _slXMODEL.ProcessEvaluationGroup();

        }*/


        /// <summary>
        /// wraps process sceanario (which is not a virtualized function)
        /// code was triggering a weird memory error when that function was called (as an override to function in simlink.cs)
        /// this is needed for optimization
        /// 
        /// </summary>
        /// <param name="sDNA"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="nScenStartAct"></param>
        /// <param name="nScenEndAct"></param>
        /// <returns></returns>

        public override int ProcessScenarioWRAP(string sDNA, int nScenarioID, int nScenStartAct, int nScenEndAct, bool bCreateIntDNA=true)
        {
            if (bCreateIntDNA)
                sDNA = ConvertDNAtoInt(sDNA);       //round any doubles to int
            int nReturn = ProcessScenario(_nActiveProjID, _nActiveEvalID, _nActiveReferenceEvalID, _sActiveModelLocation, nScenarioID, nScenStartAct, nScenEndAct, sDNA);
            return nReturn;
        }


        //  8/16/15: added override of simlink versino of this function...
        public override int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINP_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, 
            string sDNA = "-1", string sLabel="DEFAULT")
        {
            string sPath; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName; string sINIFile; string sSummaryFile; string sOUT;
            int nCurrentLoc = nScenStartAct; string sTS_Filename = ""; bool bSetHSF_RefLocation = false;

            if (_scenDeleteSpec == DeleteScenDetails.BeforeRun)
                ScenDS_ClearAfterScenario(nScenarioID); //SP 18-Jul-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

            if (true)           // met 7/3/14       nScenarioID != -1)     //we should have a valid ScenarioID at this point.
            {
                try
                {

                    if (_bIsOptimization || (nScenarioID == -1))           //nScenarioID  = -1
                    {
                        sLabel = GetScenarioLabel(sLabel);
                        nScenarioID = InsertScenario(nEvalID, nProjID, sLabel, "", sDNA);       //pass the current date time to enable easy retrieval of PK (should be better wya to do this)
                        _nActiveScenarioID = nScenarioID;
                        _log.Initialize("logEG_" + _nActiveEvalID.ToString() + "_" + _nActiveScenarioID, _log._nLogging_ActiveLogLevel, _sLogLocation_Simlink);
                    }

                    if (!_bSpecialSimlinkNaming)         // typical case: use simlink naming
                        ScenarioPrepareFilenames(nScenarioID, nEvalID, sINP_File, out sTargetPath, out sIncomingINP, out sTarget_INP, out sSummaryFile, out sOUT, out sINIFile);
                    else
                    {
                        // perform special file naming.
                        ScenarioPrepareFilenames_SpecialCase(nScenarioID, nEvalID, sINP_File, out sTargetPath, out sIncomingINP, out sTarget_INP, out sSummaryFile, out sOUT, out sINIFile);
                        //todo: consider whether to throw flag skipping step 3.
                        //user can request > 3, but may forget to.
                    }
                    SetTS_FileName(nScenarioID, sTargetPath);       //_hdf5._sHDF_FileName = sTS_Filename;    //met 1/16/14 - sl object should know it's repository
                    sPath = System.IO.Path.GetDirectoryName(sINP_File);

                    // the following retrieval of external data and loading of datasets only occurs if we are not doing a "pure insert"
                    // added 3/8/17. May require better control if there are cases where you DO want to get external data once the scenario is inserted.
                    // this also avoids duplicating this call in the case where we have two successive calls to process_scenario (from realtime)
                    if (nScenEndAct * nScenStartAct != 1)
                    {
                        // only get external data if you need to run the model
                        // todo: better control of this?
                        if (nScenStartAct <= CommonUtilities.nScenLCBaselineFileSetup)
                        {
                            //SP 15-Feb-2017 ExtractExternalData for RetrieveCode = AUX   //  met 3/8/17: move and embed in IF statement so 
                            ScenarioGetExternalData();
                        }
                        
                        if (!_bIsSimLinkLite)            // do not have scen details in this case so limited options
                            LoadScenarioDatasets(nScenarioID, nScenStartAct, nScenarioID == _nActiveBaselineScenarioID);
                    }

                    bool bContinue = true;              //identifies whether to continue
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModElementExist) && (nScenEndAct >= CommonUtilities.nScenLCModElementExist) && bContinue)       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
                    {
                        if (sDNA != "-1")
                        {           //not an optimization run, no DNA is passed
                            nScenarioID = DistribDNAToScenario(sDNA, nEvalID, nReferenceEvalID, nProjID, 1, -1, nScenarioID);
                            if (nScenarioID == -1) { bContinue = false; }       // some failure in the DNA distribution
                            if (_sUserFileUpdateKey.Length>3)
                            {
                                // met 8/7/14: added a quick/dirty way to insert a new hot start record.
                                // this avoids having to have multiple duplicate baseline files with just that one modification.
                                // todo: this could be managed much better
                                if (_sUserFileUpdateKey.Substring(_sUserFileUpdateKey.Length - 3, 3).ToLower() == "hsf")
                                {
                                    InsertModelValList(-1, 499, nScenarioID, _sUserFileUpdateKey,"","hotstart", -1, -1, CommonUtilities._sDATA_UNDEFINED, false);
                                }
                            }


                            else
                            {
                                nCurrentLoc = CommonUtilities.nScenLCModElementExist;
                            }
                        }
                        else
                        {
                            nCurrentLoc = CommonUtilities.nScenLCModElementExist;          //
                        }

                        //SP 15-Jun-2016 no longer needed - tested with EPANET and IW
                        /*if (_dbContext._DBContext_DBTYPE==0)      //if access
                        {
                            _dbContext.OpenCloseDBConnection();
                        }*/
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineFileSetup) && (nScenEndAct >= CommonUtilities.nScenLCBaselineFileSetup))
                    {
                        _log.AddString("SWMM File Setup Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        CommonUtilities.CopyDirectoryAndSubfolders(sPath, sTargetPath, true);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;          //

                        // manage a hotstart file if needed (RT only at this point- added 11/3/17
                        if (_sHotStart_ToCopy != "UNDEFINED")
                        {
                            try
                            {
                                File.Copy(_sHotStart_ToCopy, _sHotStart_ToUse,true);
                                _log.AddString(string.Format("Copied ref hotstart {0} to location {1}", _sHotStart_ToCopy, _sHotStart_ToUse), Logging._nLogging_Level_3, false, true);  //sucess - let them know
                            } 
                            catch (Exception ex)
                            {
                                _log.AddString(string.Format("Error copying ref hotstart {0} to location {1}", _sHotStart_ToCopy, _sHotStart_ToUse), Logging._nLogging_Level_1, false, true);
                            }
                        }


                        // perform a "pickup" of "special files for writing (that could be overwritten otheriwse
                        if (nScenStartAct <= CommonUtilities.nScenLCModElementExist)
                        {       // only do if mods are requested
                            ProcessNonDV_FileDependency(sTargetPath);

                        }

                        bSetHSF_RefLocation = ManageHotStartFiles(sTargetPath);     // manage hot start files (either from last sim, or cross-sim)       
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
                        _log.AddString("SWMM File Update Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        Update_INP(sIncomingINP, nScenarioID, sTarget_INP);
                        System.IO.File.Delete(sIncomingINP);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineModified;

                        string sTargetINI = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sINIFile) + "_" + nScenarioID.ToString() + ".ini";
                        if (File.Exists(sINIFile) && !File.Exists(sTargetINI))
                        {      //there may not be one in the root file, but update if there is.
                            File.Move(sINIFile, sTargetINI);
                        }
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        bool bIsUNC = false; string sSWMM_EXE = ""; string sBAT = "";
                        if (sTarget_INP.Substring(0, 2) == @"\\") { bIsUNC = true; }         //explicit paths required if UNC
                        if (bIsUNC && _compute_env == ComputeEnvironment.LocalMachine)      //added compute env swithch 8/16/16: if running condor, needs to be relative references.
                        {
                            sSWMM_EXE = "swmm5.exe " + sTarget_INP + " " + sSummaryFile + " " + sOUT;
                            sBAT = System.IO.Path.GetDirectoryName(sTarget_INP) + "\\" + "run_swmm5.bat";
                        }
                        else
                        {
                            sSWMM_EXE = "swmm5.exe " + System.IO.Path.GetFileName(sTarget_INP) + " " + System.IO.Path.GetFileName(sSummaryFile) + " " + System.IO.Path.GetFileName(sOUT);
                            sBAT = "run_swmm5.bat";
                        }

                        //create batch file information for running the program
                        string[] s = new string[] { sSWMM_EXE };
                        string _sSWMM_BAT = "run_swmm5.bat";
                        string sBat = System.IO.Path.Combine(sTargetPath, _sSWMM_BAT);

                        // assume that the swmm.cs must write the .bat    (met 9/14/14)
                        s[0] = "cd %~dp0 \r\n" + s[0];                                                  //debug   s[0] = "cd %~dp0 \r\n echo path %path% \r\n" + s[0];
                        File.WriteAllLines(sBat, s);

                        if (_compute_env == ComputeEnvironment.LocalMachine)
                        {
                            Directory.SetCurrentDirectory(sTargetPath);                 // 12/13/16: was failing with local run- working dir not getting set despite trying in the .bat 
                            string a = Directory.GetCurrentDirectory();
                            CommonUtilities.RunBatchFile(sBat);
                        }
                        else
                        {
                            //three steps then to execute
                            //05: set sim/wrapper/platform specific requirements
                            //todo: need way to say to transfer ALL (just set to 'all') - where to define this? environment?  depdnent files may be needed..?
                            if (_hpc._EnvConfig.GetString("transfer_input_files", "").ToLower() == "standard")
                                _dictHPC["transfer_input_files"] = _sSWMM_BAT + "," + Path.GetFileName(sTarget_INP);
                            else
                                _dictHPC["transfer_input_files"] = _hpc._EnvConfig.GetString("", "").ToLower();
                            // 1: Create the job file
                            XmlConfigSource xmlJobSpec = CreateHPC_JobSpec("swmm_" + nScenarioID, nScenarioID, _sSWMM_BAT, sTargetPath);
                            // 2: Perform any sim/wrapper/platform specific requirements

                            // 3: Submit the job           
                            _hpc.SubmitJob(xmlJobSpec.Configs["Job"]);

                            //now, tell simlink what you've done....    
                            // 100: 
                            UpdateScenarioStamp(nScenarioID, CommonUtilities.nScenLCModelExecuted);

                            // 200:
                            return CommonUtilities.nScenLCModelResultsRead;
                        }
                        /*
                         * iteration 1: send the job to its hpc environment if needed.
                         * this is the most time consuming step
                         * subsequent efforts may consider serializing the simlink object or sending out simlink agents to perform whole steps in the cloud
                                                if (_bIsSimCondor) //run the SWMM job as a Condor job.
                                                {               //note: 
                                                    _htc._htcJobSpecActive = CreateSubmitJob(sBat, sTargetPath,System.IO.Path.GetFileName(sTarget_INP),  bIsUNC);
                                                    _htc.ProcessCondorJobAndSync();
                                                //    _htc.PreProcessCondorJob(Directory.GetFiles(sTargetPath), null);          //grab all files in the TargetPath location
                                                  //  _htc.SubmitCondorJob();
                                                }
                                                else
                                                {   //run within SimLink

                                                    CommonUtilities.RunBatchFile(sBat);

                                                    //sim2_note: significant code removed here from previous exercises
                                                }
                         * 
                         * 
                         */
                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted;
                    }

                    if (bSetHSF_RefLocation)
                    {
                        bSetHSF_RefLocation = false;    //reset the bool
                        try
                        {
                            var directory = new DirectoryInfo(sTargetPath);
                            var myFile = directory.GetFiles("*.hsf")
                                    .OrderByDescending(f => f.LastWriteTime)
                                    .First();

                            _sHSF_XCOHORT = Path.Combine(directory.ToString(), myFile.ToString());
                        }
                        catch (Exception ex)
                        {
                            _log.AddString(string.Format("Error setting hot start reference for scenario {0}", _nActiveScenarioID), Logging._nLogging_Level_1, false, true);
                        }
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        _log.AddString("SWMM Results Read Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        ResultsReadReport(sSummaryFile, nReferenceEvalID, nScenarioID);
                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                        bool bIsScenSpecificBaseRPT = true;

                        if (!_bTS_IndicesLoaded || bIsScenSpecificBaseRPT)            //first time through, load indices
                        {                                   //for now we are assuming no inserts- so stay same for each
                            Initialize_SWMM_TS_ReportRequest(sSummaryFile,bIsScenSpecificBaseRPT);
                            if (_dictResultTS_Indices != null)
                            {
                                _log.AddString(string.Format("{0} ts indices loaded", _dictResultTS_Indices.Count), Logging._nLogging_Level_1, false, true);
                            }
                            else
                            {
                                _log.AddString("ts dict null; check rpt file row count offsets.", Logging._nLogging_Level_1, false, true);
                            }
                            _bTS_IndicesLoaded = true;
                        }


                        if (_dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()).Count() > 0)        //5/15/14- skip HDF5 create if no results //SP 15-Feb-2017 only if primary
                        {
                            //MET 12/9/2013 - manage HDF OUTSIDE the readOUT logic... - it shouldn't be troubled with knowing about where the data will go.
                            if (_tsRepo == TSRepository.HDF5)           //at present, this is the only supported TS repo
                            {
                                try
                                {
                                    //  _hdf5 = new hdf5_wrap();
                                    _hdf5.hdfCheckOrCreateH5(_hdf5._sHDF_FileName);
                                    _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
                                    ReadOUTData(nReferenceEvalID, nScenarioID, sOUT);
                                    _hdf5.hdfClose();
                                }
                                catch (Exception ex)
                                {
                                    _log.AddString(string.Format("Error writing timeseries to HDF5. Error message returned: {0}", ex.Message), Logging._nLogging_Level_1);
                                    throw; 
                                }
                            }
                        }
                        nCurrentLoc = CommonUtilities.nScenLModelResultsTS_Read;
                    }

                    // SP 15-Feb-2017 Moved to before ProcessScenario_COMMON so WriteTimeSeriesToRepo for Secondary and AUX could be achieved in Simlink Class 
                    //this conditional is a litte arbitrary: don't want to send results if we  didn't do seomthing substantive.
                    if (nScenStartAct <= CommonUtilities.nScenLCModelExecuted || nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read)
                        _bScenSaveSecondaryAndAuxTS = true;
                    else
                        _bScenSaveSecondaryAndAuxTS = false;

                    ProcessScenario_COMMON(nReferenceEvalID, nScenarioID, nCurrentLoc, nScenEndAct);        //call base function to perform modeltype independent actions
                                                                                                            //note: if _bIsOptimization, the objective function is set in ProcessScenario_COMMON

                    if (_slXMODEL != null)
                    {

                    }
                    if (_bUseHotStart_CrossScenario)
                        _nScenarioHotStart = nScenarioID;       // store the scenario ID for the next use if needed
                                                                //clear scenario following execute
                    
                    // If we are only inserting scenario, don't update (we don't have a scen datail
                    if (nScenEndAct * nScenStartAct != 1)
                        UpdateScenarioStamp(nScenarioID, nCurrentLoc);                 //store the time the scenario is completed, along with the "stage" of the Life Cycle
                                                                                   //SP 14-Jun-2016 Collated all routines writing back to the DB to the end of the ProcessScenario routine
                                                                                   //SP 10-Jun-2016 TODO Ideally want to shift writing / reading from the database out of this routine - initial step, integrate into one routine at the end of process scenario
                    WriteResultsToDB(nScenarioID);

                    //SP 14-Jun-2016 moved this to after writing back to the DB //18-Jul-2016 moved again!! clear at the start of the routine - _dsSCEN_PerformanceDetails is needed to obtain the objective value for the optimization
                    if (_scenDeleteSpec == DeleteScenDetails.AfterRun)
                        ScenDS_ClearAfterScenario(nScenarioID); //SP 18-Jul-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

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
                    Console.WriteLine("Process Scenario Error. Last stage {0}.  Error: {1}", nCurrentLoc, ex.Message);
                    _log.AddString("Process scenario error stage: " + nCurrentLoc, Logging._nLogging_Level_1);
                    _log.AddString("Exception: " + ex.Message + " : ", Logging._nLogging_Level_1);
                    _bScenarioIsValid = false;
                    _log.WriteLogFile();
                    return 0;   //TODO: refine based upon code succes met 6/12/2012
                }
            }
        }

        /// <summary>
        /// Get hotstart from previous run...
        /// </summary>
        /// <param name="sTargetPath"></param>
        private bool ManageHotStartFiles(string sTargetPath){
            bool bReturn = false;
            if (IsInCohort())
            {
                if (_cohortSpec._cohortType == CohortType.SCENARIO_INIT)        // cohort run : ALL ABOUT THE HSF
                {
                    if (!IsLeadEGInCohort(_nActiveEvalID))          // first pass, do NOT grab HSF across EG
                    {
                        string sNewHSF = Path.Combine(sTargetPath, Path.GetFileName(_sHSF_XCOHORT));
                        //replace string _OUT to _IN. For now, no other options are performed.
                        sNewHSF = sNewHSF.Replace("_OUT", "_IN");
                        if (File.Exists(_sHSF_XCOHORT))
                            File.Copy(_sHSF_XCOHORT, sNewHSF);          // copy the old hotstart file 
                        else
                            _log.AddString(string.Format("Hotstart file {0} not found for init/run cohort type", _sHSF_XCOHORT), Logging._nLogging_Level_1, false, true);
                        return false;
                    }
                    else
                    {
                        // lead EG in cohort.
                        bReturn = true;         // tell the main function that you need to set the _sHSF_XCOHORT var
                    }
                }
                return bReturn;
            }
            // if you get here, need to check whether you need previous runs hot start
            if (_bUseHotStart_CrossScenario)                    //UNTESTED as of 12/26/16
            {
                if (_nScenarioHotStart == -1)
                {
                    // first run, no scenario set, so just use .hsf which should be provided
                }
                else
                {
                    string sHotStartDir = CommonUtilities.GetSimLinkDirectory(sTargetPath, _nActiveScenarioID, _nActiveEvalID, true);
                    string[] sHSFs = Directory.GetFiles(sHotStartDir, "*.hsf");

                    // copy HSF (should be just one, but could be more) from reference dir to current working dir.
                    foreach (string sHSF in sHSFs)
                    {
                        FileInfo fiHSF = new FileInfo(sHSF);
                        fiHSF.CopyTo(sTargetPath, true);
                    }
                }
            }
            return bReturn;
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
            int nVarType_FK = 127;                //todo: better not to hard code to IDs
            int nVarType_FK2 = 128;
            if (!bIsStartTime) 
            {
                nVarType_FK = 129;
                nVarType_FK2 = 130;
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


        protected override void ProcessNonDV_FileDependency(string sFilePath, int nScenID = -1, int nRefEvalID = -1)
        {
           // foreach (DataRow dr in _dsEG_SupportingFileSpec.Tables[0].Select("DataType_Code > 0 and IsInput")) 
            // met 12/22/16: move to using the list of external data destinations
            foreach (external_csv ex in _lstExternalDataDestinations.Where(x => x._bIsInput))// todo: filter on type ex csv? done auto?        //"DataType_Code > 0 and IsInput")
            {
                string sLabel = ex._sFilename;
                int nRecordID = ex._nTSRecordID;

                if (nRecordID == 64211)
                {
                    int n = 1;
                }
          //      string sLabel = dr["Filename"].ToString();          // todo: default val of -1 uses Result ElementName, val of -2 uses ResultLabel, Filename overwrites
                //int nCode = Convert.ToInt32(dr["DataType_Code"].ToString());
              //  ExternalDataType nCode = (ExternalDataType)Convert.ToInt32(dr["DataType_Code"].ToString()); //value of 1 is writing to DAT file 
          //      int nRecordID = Convert.ToInt32(dr["RecordID_FK"].ToString());

                switch(ex._externaldatatype){
                    case ExternalDataType.CSV:
                        WriteDatFile(nRecordID, sFilePath, sLabel);
                        break;
                    default:
                        WriteDatFile(nRecordID, sFilePath, sLabel);
                        break;
                }
            }
        }

        private void WriteDatFile(int nRecordID, string sBasePath, string sLabel, bool bIncludeScenarioInFilename=false)
        {
            string sFilename = Path.Combine(sBasePath, sLabel) +".dat";
            DateTime dtToWrite = _tsdSimDetails._dtStartTimestamp;        // ASSUME start at sim start.
            try
            {
            int nIndex = _dictResultTS_Indices[nRecordID];


                int nRecords = _dResultTS_Vals[nIndex].GetLength(0);         // all TS thus far do so
                TimeSpan tStep = new TimeSpan(0, 0, (int)_tsdSimDetails._nTSIntervalInSeconds);                                                           // may not work for all ts in future (thus need to support record level TS definition!?! see schema- been thinking about it

                if (File.Exists(sFilename))
                    File.Delete(sFilename);     //rm file if exists in baseline set up 

                using (StreamWriter writer = new StreamWriter(sFilename))
                {
                    for (int i = 0; i < nRecords; i++)
                    {
                        writer.WriteLine(dtToWrite.ToString("MM/dd/yyyy") + '\t' + dtToWrite.ToString("HH:mm") + '\t' + _dResultTS_Vals[nIndex][i, 0]);
                        dtToWrite += tStep;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error writing DAT file for ID {0}; record not found", nRecordID.ToString()), Logging._nLogging_Level_1, true, true);

            }
        }


        public EventHandler _evhSimLink;

        //public void SetBorgDelegate(){
        //    //_optWrapper._evhProcessScenario += _evhSimLink;
        //    //_optWrapper._methodProcessScenario = ProcessScenario_BORG(double[] , double[] , double[] );
        //    _optWrapper._delProcessScenario = new Optimization.Optimization.delProcessScenario(ProcessScenario_BORG);
        //}

        public void UpdateProgress(){


    }

        //hard-coded dummy function to test delegate
        public override void ProcessScenario_BORG(double[] vars, double[] objs, double[] constrs){
            objs[0] = Math.Pow(vars[0], 2);
            objs[1] = -1 * Math.Pow((vars[1] - 2), 2);
            objs[2] = vars[0] * vars[1];
        }

        /// <summary>
        /// Delegate for use with optimization....
        /// </summary>
        /// <param name="vars"></param>
        /// <param name="objs"></param>
        /// <param name="constrs"></param>
        public override void ProcessScenario(double[] vars, double[] objs, double[] constrs)
        {
           // add calls to process scenario....
            string sDNA = string.Join(_sDNA_Delimiter, vars);          
            int nReturn = ProcessScenarioWRAP(sDNA, -1, -1, 100);

            //SP 10-Oct-2016 Write log file after finished processing each scenario processing
            _log.WriteLogFile();

            LoadObjectives(objs);           //load the objective values to send back...
        }

        //utility function to set the filenames that are needed
        //todo: use the SimLink naming functions in CommnUtilities to make this work beter
        // met 4/29/14: removed ts set from this- h
        private void ScenarioPrepareFilenames(int nScenarioID, int nEvalID, string sINP_File, out string sTargetPath, out string sIncomingINP, out string sTarget_INP, out string sSummaryFile, out string sOUT, out string sINIFile)
        {
            string sPath = System.IO.Path.GetDirectoryName(sINP_File);
         //   sTargetPath = sPath.Substring(0, sPath.LastIndexOf("\\")) + "\\" + nEvalID.ToString() + "\\" + nScenarioID.ToString();
            sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, nEvalID, true);   //met 4/29/14- was being done manually. confirm and delete prev line
            string sTarget_INP_FileName = System.IO.Path.GetFileNameWithoutExtension(sINP_File) + "_" + nScenarioID.ToString() + System.IO.Path.GetExtension(sINP_File);       //append scenario name (good for gathering up the files into a single folder if needed)
            sIncomingINP = System.IO.Path.Combine(sTargetPath, System.IO.Path.GetFileName(sINP_File));
            sTarget_INP = System.IO.Path.Combine(sTargetPath, sTarget_INP_FileName);
            sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP_FileName) + ".RPT";
            sOUT = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP_FileName) + ".OUT";
            sINIFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sINP_File) + ".ini";

            
        }

        /// <summary>
        /// MET 11/22/16: se 
        /// </summary>

        private void ScenarioPrepareFilenames_SpecialCase(int nScenarioID, int nEvalID, string sINP_File, out string sTargetPath, out string sIncomingINP, out string sTarget_INP, out string sSummaryFile, out string sOUT, out string sINIFile)
        {
            // phase 1: 
            string sOut = "NOT";
            sTarget_INP = CreateStringFromCode(nScenarioID, out sOut, false);
            sIncomingINP = sTarget_INP;
            sTargetPath = Path.GetDirectoryName(sTarget_INP);
            sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP) + ".RPT";
            sOUT = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP) + ".OUT";
            sINIFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sINP_File) + ".ini";
        }


        #endregion

        #region HPC - WRAP

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
            if(_log._nLogging_ActiveLogLevel>=Logging._nLogging_Level_3)        // save if we are in debug situation
                icsJob.Save(sXMLPath);
            return icsJob;
        }

        /// <summary>
        /// Perform any functions specific to the compute environment itself
        /// plz don't mkae func names this long in the future!
        /// </summary>
        /// <param name="?"></param>
        /// <param name="dictParams"></param>
        private void CreateHPC_JobSpecPerformComputeEnvSpecificFunctions(ref XmlConfigSource jobConfigReturn, Dictionary<string,string> dictParams)
        {
            switch (_compute_env)
            {
                case ComputeEnvironment.Condor:
                    jobConfigReturn.Configs["Job"].Set("requirements", _hpc._EnvConfig.GetString("CondorRequirements",""));     // assume this is not varying by scenario - constant
                    if (dictParams.ContainsKey("transfer_input_files"))
                        jobConfigReturn.Configs["Job"].Set("transfer_input_files", dictParams["transfer_input_files"]);         //changes by scenario- set in process scenario
                // and so on
                        
                    break;
                case ComputeEnvironment.AWS:

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

        #region File Manip
        public override void CreateBatchFile_ByEval(int nEvalID, int nRunsInBatch = -1, int nCohortID = -1, string sEXE_NAME = "swmm5.exe", string sOutputPath = "DEFAULT")
        {
            string sPath; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName; string sRef_INP; int nScenarioID; int nBatchCounter; int nSWMM_RecordCounter; int nLoops;
            string sql = "";
            if (nCohortID<0)
            {  //met 5/14- changed code becasue wasn't working with eg... not sure why. not good in long run.
              sql = "SELECT tblScenario.ScenarioID, tblEvaluationGroup.EvaluationID, " +
                                                "[tblEvaluationGroup].[ModelFileLocation] AS ModelFile_Location, " +
                                                "tblScenario.DNA, IIf([tblEvaluationGroup].[ReferenceEvalID_FK]=-1,[tblEvaluationGroup].[EvaluationID]," +
                                                "[tblEvaluationGroup].[ReferenceEvalID_FK]) AS ReferenceEvalID_FK " +
                                                "FROM (tblProj INNER JOIN tblEvaluationGroup ON tblProj.ProjID = tblEvaluationGroup.ProjID_FK) " +
                                                "INNER JOIN tblScenario ON tblEvaluationGroup.EvaluationID = tblScenario.EvalGroupID_FK " +
                                                "WHERE (((EvaluationID) = " + nEvalID + ") and (HasBeenRun=0)) ORDER BY ScenarioID;";
            }
            else
            {
                sql = "SELECT tblScenario.ScenarioID, tblEvaluationGroup.EvaluationID, " +
                                                "[tblEvaluationGroup].[ModelFileLocation] AS ModelFile_Location, " +
                                                "tblScenario.DNA, IIf([tblEvaluationGroup].[ReferenceEvalID_FK]=-1,[tblEvaluationGroup].[EvaluationID]," +
                                                "[tblEvaluationGroup].[ReferenceEvalID_FK]) AS ReferenceEvalID_FK " +
                                                "FROM (tblProj INNER JOIN tblEvaluationGroup ON tblProj.ProjID = tblEvaluationGroup.ProjID_FK) " +
                                                "INNER JOIN tblScenario ON tblEvaluationGroup.EvaluationID = tblScenario.EvalGroupID_FK " +
                                                "WHERE (((EvaluationID) in (select evaluationid from tblevaluationgroup where (CohortID = " + nCohortID + ")))" +
                                                " and ((HasBeenRun)=0)) ORDER BY ScenarioID;";

            }
            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server    
            /*"SELECT  ScenarioID, EvaluationID, ModelFile_Location,  DNA, ReferenceEvalID_FK"
                    + " FROM  qrySimLink_ScenarioEvalProj"
                    + " WHERE (((EvaluationID) = "  + nEvalID + ") and (HasBeenRun=0)) ORDER BY ScenarioID;";*/

            DataSet dsEvals = _dbContext.getDataSetfromSQL(sql);


                if (dsEvals.Tables[0].Rows.Count > 0)
            {
                if (nRunsInBatch == -1) { nRunsInBatch = dsEvals.Tables[0].Rows.Count; }

                sRef_INP = dsEvals.Tables[0].Rows[0]["ModelFile_Location"].ToString();
                sPath = System.IO.Path.GetDirectoryName(sRef_INP);
                if (sOutputPath == "DEFAULT")
                {           //use default setting
                    sOutputPath = sPath.Substring(0, sPath.LastIndexOf("\\")) + "\\";
                }
                else
                {
                    //todo: check for "\\" and add if not present...
                }
                double d;
                d = dsEvals.Tables[0].Rows.Count / nRunsInBatch;
                nLoops = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dsEvals.Tables[0].Rows.Count) / Convert.ToDouble(nRunsInBatch)));
                nSWMM_RecordCounter = 0;
                for (int i = 1; i <= nLoops; i++)
                {
                    string sBatchFileOut = sOutputPath + "eg" + nEvalID.ToString() + "_v" + i + "_swmm_run3.bat";
             //   use this if can't access run pth        string sBatchFileOut = @"C:\a\" + "eg" + nEvalID.ToString() + "_v" + i + "_swmm.bat";
                    System.IO.StreamWriter fileBatchOut = new System.IO.StreamWriter(sBatchFileOut);

                    for (int j = 1; j <= nRunsInBatch; j++)
                    {
                        nScenarioID = Convert.ToInt32(dsEvals.Tables[0].Rows[nSWMM_RecordCounter]["ScenarioID"].ToString());
                        nEvalID = Convert.ToInt32(dsEvals.Tables[0].Rows[nSWMM_RecordCounter]["EvaluationID"].ToString());      //met 12/30/16: can change if in cohort.
                        sTargetPath = sPath.Substring(0, sPath.LastIndexOf("\\")) + "\\" + nEvalID.ToString() + "\\" + nScenarioID.ToString();
                        sTarget_INP_FileName = System.IO.Path.GetFileNameWithoutExtension(sRef_INP) + "_" + nScenarioID.ToString() + System.IO.Path.GetExtension(sRef_INP);       //append scenario name (good for gathering up the files into a single folder if needed)
                        sIncomingINP = System.IO.Path.Combine(sTargetPath, System.IO.Path.GetFileName(sRef_INP));
                        sTarget_INP = System.IO.Path.Combine(sTargetPath, sTarget_INP_FileName);
                        string sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP_FileName) + ".RPT";
                        string sOUT = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP_FileName) + ".OUT";
                        fileBatchOut.WriteLine(sEXE_NAME + " " + sTarget_INP + " " + sSummaryFile + " " + sOUT);
                        nSWMM_RecordCounter++;
                        if (nSWMM_RecordCounter == dsEvals.Tables[0].Rows.Count) { break; }
                    }
                    fileBatchOut.Close();
                }
            }
        }


        #endregion



        #region SWMM SPECIFIC OVERRIDES
        //override in derived classes
        protected override void XMODEL_PlatformSpecificFollowup(int nScenarioID)
        {
            var LinkedModelChanges = _lstSimLinkDetail                            //step 1: get any simLinkdetails from the list
                                .Where(x => x._nScenarioID == nScenarioID)
                                .Where(x => x._nRecordID == CommonUtilities._nDV_ID_Code_LinkedData)
                                .Where(x => x._slDataType == SimLinkDataType_Major.MEV);
            foreach (var modelChange in LinkedModelChanges.ToList())
            {
                int nIndexOfMEV = Array.IndexOf(_sMEV_GroupID, modelChange._sVal);      // modelChange._sVal holds the key, and is overwritten subsequently
                if (nIndexOfMEV < 0)
                    _log.AddString("XMODEL timeseries not found: " + modelChange._sVal, Logging._nLogging_Level_1);


                double[,] dTS_Vals = _dMEV_Vals[nIndexOfMEV, 1];     //grab the perturbed time series.
                //current limitations: only supports .dat for specific data types
                switch (modelChange._nVarType_FK)           
                {
                    case _nFieldDict_RAINGAGE_TIMESERIES:
                        XMODEL_RaingageDataSource_Process(nScenarioID, modelChange, dTS_Vals, _tsdResultTS_SIM);
                        break;
                    case -1:        //todo: Evaporation and Level

                        break;
                }

            }
        }

        private void XMODEL_RaingageDataSource_Process(int nScenarioID, simlinkDetail ModelChange,double[,] dTS_Vals, TimeSeries.TimeSeriesDetail tsdResultTS)
        {
            string sFileNameOut = CommonUtilities.GetSimLinkFileName("RainGage_Perturbed", nScenarioID) + ".dat";    // fine to just say file name as we will write to folder location
            string sFullPathName = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, _nActiveEvalID, true);
            sFullPathName = sFullPathName +"\\"+sFileNameOut;

            int nRecordID = -1;
            string sElementName = GetSimLinkDetail(SimLinkDataType_Major.Network, nRecordID, _nFieldDict_RAINGAGE_NAME, _nActiveReferenceEG_BaseScenarioID, ModelChange._nElementID);
            //first update the list.
            _lstSimLinkDetail.Where(x => x._nScenarioID == nScenarioID)
                                .Where(x => x._nRecordID == CommonUtilities._nDV_ID_Code_LinkedData)
                                .Where(x => x._slDataType == SimLinkDataType_Major.MEV)
                                .Select(u => { u._sVal = sFileNameOut; u._sElementName = sElementName; return u; }).ToList();

            
            string sStationID = GetSimLinkDetail(SimLinkDataType_Major.Network, nRecordID, _nFieldDict_RAINGAGE_STATIONID, _nActiveReferenceEG_BaseScenarioID, ModelChange._nElementID);    //NOTE: Would be better to set the ID value in the initialization phase...

            WriteTimeSeriesDatFile(sFullPathName, dTS_Vals, tsdResultTS, sStationID, ";Rainfall (inch)");              //todo: units!
        }


        #endregion
    }
}
