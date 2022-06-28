using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using InfoWorksLib;
using Nini.Config;

namespace SIM_API_LINKS
{
    //extend cohort specification to support some "nicer" run grouping in infoworks...
    public class cohortSpecIW      /// could not make derived as compiler would not recgonize this as a necesisit..:cohortSpec
    {
        public Dictionary<int, string> _dictRunGroupSQN = new Dictionary<int, string>();     // store the run groups (Created by lead run group)
        public Dictionary<int, string> _dictNetwork = new Dictionary<int, string>();
        
    }

    public class iw : simlink
    {
        /// <summary>
        /// iw wrapper adapted from old iw_link.cs 
        /// 
        /// major improvements needed
        ///     - MEV- get from in-memory list
        ///     - dbContextModel- evaluate whether this is really needed..
        /// </summary>

        public InfoWorksLib.InfoWorks _iw;
        public bool _bIW_IsInitialized = false;
        public string _spBaseNetwork;
        public string _spBaseNetworkShortDisplayName;
        public string _sMasterDatabase;
        // move to simlink for ease of init and supporting other simulators  public int _nSimID;
        public string _sSimSP;
        public string _sRunSP;
        public DAL.DBContext _dbContextMODEL;       //iw needs to be able to interact with its model as well.
        public bool _bSKIP_IW_Init = false;                //used in specific cases when IW license not neededd
        public bool _bTestForIW_MaxFileSize = true;         //if true, each iteration (whether opt or processeg) will check for file size and compact and repair if needed.
        public cohortSpecIW _cohortSpecIW = new cohortSpecIW(); // cohort spec extension for iw

        #region INIT

        public override bool InitializeByConfig(IConfigSource config)
        {
            _sMasterDatabase = config.Configs["simlink"].GetString("iwm", "UNDEFINED");
            _nSimID = config.Configs["simlink"].GetInt("sim", -1);
            _bSKIP_IW_Init = HelperGetSkipInit(config.Configs["simlink"].GetString("iwSkip", "FALSE"));

            InitializeIW_ModelLinkage("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _sMasterDatabase, 0);                     //todo: add ability to manage the compaction ratio.  add ability to link to sqlserver

            base.InitializeByConfig(config);

            return true;
        }

        private bool HelperGetSkipInit(string sVal)
        {
            if (sVal.ToLower() == "true" || sVal.ToLower() == "y" || sVal.ToLower() == "yes")
                return true;
            else
                return false;
        }

        public override void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_IW;
            _sTS_FileNameRoot = "IW_TS.h5";
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
            InitNavigationDict();
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
                    _iw = new InfoWorksLib.InfoWorks();
                    _iw.InitForTest(0, "", "");
                    _bIW_IsInitialized = true;
                    Console.WriteLine("IW license obtained");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to get IW license. Simlink will not be able to execute any IW API functions");
                }
            }
        }


        /// <summary>
        /// new function to link to the iw model db, which is needed (currently) for creating scripting paths etc etc
        /// unlike all other simlink wrappers, the model itself has a dbContext. This is probably not good, but can be improved in the future if needed (and possible)
        /// 
        /// set dCR_Ratio to -1 to avoid the CR operation...
        /// 
        /// MET ICM - not needed in icm... Generally can't connect to db directly
        /// </summary>
        public void InitializeIW_ModelLinkage(string sConn, int nDB_Type, double dCR_Ratio = .85)
        {
            _dbContextMODEL = new DAL.DBContext();
            bool bIsValidConnection = _dbContextMODEL.Init(sConn, DAL.DBContext.GetDBContextType(nDB_Type), dCR_Ratio);
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
                    _iw.MasterDatabase = _sMasterDatabase;
                    if (_nSimID == -1)
                    {
                        Console.WriteLine("must set simid on eg");
                        //todo: handle the error.
                    }
                    else
                    {
                        _sSimSP = IW_CreateScriptingPath(_nSimID, "Sim", _iw.MasterDatabase);
                    }
                    _sRunSP = _iw.get_Parent(_sSimSP);
                    _spBaseNetwork = _iw.get_Value(_sRunSP, "Network");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public override void CloseModelLinkage()
        {
            base.CloseModelLinkage();
            _dbContextMODEL.Close();
        }
        /// <summary>
        /// todo; pull from an IW data table...
        /// </summary>
        public override void SetTSDetails()
        {

            DateTime dtSim = new DateTime(2000, 1, 1);
            DateTime dtRPT = dtSim;
            int nSecInterval = 300;
            _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtRPT, IntervalType.Second, nSecInterval);
            _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtSim, IntervalType.Second, nSecInterval);
        }
        #endregion


        #region IW general scripting and import
        public string IW_CreateScriptingPath(int nComponentID, string sTable, string fileBaseIWM)
        {
            string sp = "";

            sp = GetValueFromGeneralTable(sTable, "Name", "(ID = " + nComponentID.ToString() + ")").ToString();
            int nNewIndex = 0; string sUpperTable = "";
            switch (sTable)
            {
                case "Run":
                    sp = ">RUN~" + sp;    //get scripting path for the element 
                    sUpperTable = "Project";

                    break;
                case "Project":
                    sp = ">RUNG~" + sp;
                    sUpperTable = "[Catchment Group]";
                    break;

                case "[Catchment Group]":
                    sp = ">CG~" + sp;
                    break;
                case "Sim":
                    sp = ">SIM~" + sp;
                    sUpperTable = "Run";
                    break;

                case "Network":
                    sp = "this isn't implemented yet!";
                    break;
            }
            if (sTable == "[Catchment Group]")      //works slightly different for catchment group
            {
                object sObj = GetValueFromGeneralTable(sTable, "Parent", "(ID = " + nComponentID.ToString() + ")");
                if (sObj.ToString() != "")            //value in parent fields indicates nested CG (atypical, but frequent). 
                {
                    sp = IW_CreateScriptingPath(Convert.ToInt32(sObj), sTable, fileBaseIWM) + sp;       //get the parent cg (these can be infinitely nested)
                }
                //ELSE: do nothing- this is how we get out of the loop.
            }
            else
            {
                nNewIndex = Convert.ToInt32(GetValueFromGeneralTable(sTable, sUpperTable, "(ID = " + nComponentID.ToString() + ")"));
                sp = IW_CreateScriptingPath(nNewIndex, sUpperTable, fileBaseIWM) + sp;
            }


            return sp;
        }

        /// <summary>
        /// generalized from previous version in cu; 
        /// this approach is not generally used ; should be considered for different approach
        /// 
        /// met changed 4/10/15: assume this is linking to the model (via dbcontextmodel itself... may not be case for everything?
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sKeyFieldName"></param>
        /// <param name="sWhereClause"></param>
        /// <returns></returns>
        public object GetValueFromGeneralTable(string sTableName, string sKeyFieldName, string sWhereClause)
        {
            string sql = "select " + sKeyFieldName + " from " + sTableName + " where " + sWhereClause;
            DataSet dsMyDs = _dbContextMODEL.getDataSetfromSQL(sql);
            //    dsMyDs.Tables.Add(oTable);
            //   dsMyDs.Tables[0].Load(dr);
            if (dsMyDs.Tables[0].Rows.Count > 0)
            {
                return dsMyDs.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return -667;
            }
        }

        //utility function to set the filenames that are needed
        //todo: use the SimLink naming functions in CommnUtilities to make this work beter
        // met 4/29/14: removed ts set from this- h
        private void ScenarioPrepareFilenames(int nScenarioID, int nEvalID, out string sTargetPath)
        {
            sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, nEvalID, true);   //met 4/29/14- was being done manually. confirm and delete prev line           
        }

        public string ReadIW_NetworkToDB(string fileBaseIWM, int nSimID, int nScenarioID, int nProjID)  //int nProjID, int nIWRunDI,
        {
            if (File.Exists(fileBaseIWM))
            {
                try
                {
                    //grab the data table, which will indicate how to loop through
                    string sql_Tdict = "SELECT TableName, ComponentName, TableClass, ComponentName_Alias, KeyColumn FROM tlkpIWTableDictionary ORDER BY TableClass;";
                    string sImportStatus = "Imported the following objects \r\n";

                    DataTable oTable2 = new DataTable();
                    DataSet dsTDict = _dbContext.getDataSetfromSQL(sql_Tdict);                  //met 7/12/16: updated from _dbContextMODEL  (how could that have been?)


                    string sql_table = "";
                    //////////////now get the NETWORK ID for the specified SIM (you could use an IW instance for this, but not doing that allows you to do this a) faster b) without a license
                    string sql_Network = "select Network FROM Run INNER JOIN Sim ON Run.ID = Sim.Run where (Sim.ID=@Sim)";
                    List<DAL.DBContext_Parameter> lstParam = new List<DAL.DBContext_Parameter>();
                    DAL.DBContext_Parameter param = new DAL.DBContext_Parameter("@Sim", SIM_API_LINKS.DAL.DataTypeSL.INTEGER, nScenarioID);
                    lstParam.Add(param);
                    DataSet dsNet = _dbContextMODEL.getDataSetfromSQL(sql_Network, lstParam);
                    int nNetworkID = Convert.ToInt32(dsNet.Tables[0].Rows[0]["Network"].ToString());

                    //////////////////////
                    foreach (DataRow dr_Dict in dsTDict.Tables[0].Rows)
                    {

                        // WAS r_Dict["TableClass"].ToString()=="3"this indicates a "regular" network data table
                        //get proper select statement based on data type (could go in web config for easier update / less code real estate
                        switch ("hw_subcatchment")                  //dr_Dict["TableName"].ToString())
                        {
                            case "hw_sluice":
                                sql_table = "SELECT hw_sluice.*, -1 as ModelVersion, '' as Description FROM hw_sluice INNER JOIN Network_sluice ON (hw_sluice.link_suffix = Network_sluice.link_suffix) AND (hw_sluice.us_node_id = Network_sluice.us_node_id) AND (hw_sluice.Version = Network_sluice.Version) WHERE (((Network_sluice.NetworkID)=@NetworkID));";
                                break;
                            case "hw_pump":
                                sql_table = "SELECT hw_pump.*, -1 as ModelVersion, '' as Description FROM hw_pump INNER JOIN Network_pump ON (hw_pump.link_suffix = Network_pump.link_suffix) AND (hw_pump.us_node_id = Network_pump.us_node_id) AND (hw_pump.Version = Network_pump.Version) WHERE (((Network_pump.NetworkID)=@NetworkID));";
                                break;
                            case "hw_subcatchment":
                                sql_table = "SELECT -1 as subID, hw_subcatchment.subcatchment_id, total_area, x, y, area_absolute_1, area_absolute_2, wastewater_profile, trade_profile, population, rainfall_profile, node_id, contributing_area, catchment_dimension, -1 as ModelVersion, '' as Description,area_measurement_type,excluded,hw_subcatchment.Version FROM hw_subcatchment INNER JOIN Network_subcatchment ON  (hw_subcatchment.subcatchment_id = Network_subcatchment.subcatchment_id) AND (hw_subcatchment.Version = Network_subcatchment.Version) WHERE (((Network_subcatchment.NetworkID)=@NetworkID));";
                                break;
                            case "hw_weir":
                                sql_table = "SELECT hw_weir.*, -1 as ModelVersion, '' as Description FROM hw_weir INNER JOIN Network_weir ON (hw_weir.link_suffix = Network_weir.link_suffix) AND (hw_weir.us_node_id = Network_weir.us_node_id) AND (hw_weir.Version = Network_weir.Version) WHERE (((Network_weir.NetworkID)=@NetworkID));";
                                break;
                            case "hw_node":
                                sql_table = "SELECT hw_node.*, -1 as ModelVersion, '' as Description FROM hw_node INNER JOIN Network_node ON (hw_node.node_id = Network_node.node_id) AND (hw_node.Version = Network_node.Version) WHERE (((Network_node.NetworkID)=@NetworkID));";
                                break;
                            case "hw_conduit":
                                sql_table = "SELECT hw_conduit.*, -1 as ModelVersion, '' as Description FROM hw_conduit INNER JOIN Network_conduit ON (hw_conduit.link_suffix = Network_conduit.link_suffix) AND (hw_conduit.us_node_id = Network_conduit.us_node_id) AND (hw_conduit.Version = Network_conduit.Version) WHERE (((Network_conduit.NetworkID)=@NetworkID));";
                                break;
                            case "Run":
                                //because the wallingford bozos had many spaces in the field names, we need to "convert" through this sql call. i can't imagine why they did that
                                sql_table = "SELECT Run.ID, Run.Name, Run.Inflow, Run.Level, Run.[Trade Waste] as Trade_Waste, Run.[Waste Water] as Waste_Water, Run.Network, Sim.[Rainfall Event] as Rainfall_Event, Run.[Ground Infiltration] as Ground_Infiltration, Sim.ID as Sim, -1 as ModelVersion, '' as Description FROM Run INNER JOIN Sim ON Run.ID = Sim.Run where (Sim.ID=@sim);";
                                break;
                            case "tblComponent":
                                DataRow drRunDef = iwGetRunComponents();
                                sql_table = "SELECT [RTC Scenario].ID as ComponentType_FK, [RTC Scenario].Name as ComponentLabel, [RTC Scenario].Comment AS Description, 'RTC' AS Type, " + Convert.ToInt32(drRunDef["RTC Scenario"].ToString()) + " as VarType_FK, -1 AS ModelVersion FROM [RTC Scenario] UNION"
                                    + " SELECT Inflow.ID, Inflow.Name, Inflow.Comment AS Description, 'Inflow' AS Type, " + Convert.ToInt32(drRunDef["Inflow"].ToString()) + " as VarType_FK, -1 AS ModelVersion FROM Inflow UNION "
                                    + " SELECT [Level].ID, Level.Name, [Level].Comment AS Description, 'Level' AS Type, " + Convert.ToInt32(drRunDef["Level"].ToString()) + " as VarType_FK, -1 AS ModelVersion FROM [Level] UNION"
                                    + " SELECT [Rainfall Event].ID, [Rainfall Event].Name, [Rainfall Event].Comment AS Description, 'Rainfall Event' AS Type, " + Convert.ToInt32(drRunDef["Rainfall Event"].ToString()) + " as VarType_FK, -1 AS ModelVersion FROM [Rainfall Event] UNION"
                                    + " SELECT [Trade Waste].ID, [Trade Waste].Name, [Trade Waste].Comment AS Description, 'Trade Waste' AS Type, " + Convert.ToInt32(drRunDef["Trade Waste"].ToString()) + " as VarType_FK, -1 AS ModelVersion FROM [Trade Waste] UNION"
                                    + " SELECT [Waste Water].ID, [Waste Water].Name, [Waste Water].Comment AS Description, 'Waste Water' AS Type, " + Convert.ToInt32(drRunDef["Waste Water"].ToString()) + " as VarType_FK, -1 AS ModelVersion FROM [Waste Water] UNION"
                                    + " SELECT [Ground Infiltration].ID, [Ground Infiltration].Name, [Ground Infiltration].Comment AS Description, 'Ground Infiltration' AS Type, " + Convert.ToInt32(drRunDef["Ground Infiltration"].ToString()) + " as VarType_FK, -1 AS ModelVersion FROM [Ground Infiltration] UNION"
                                    + " SELECT [Network].ID, Network.Name, [Network].Comment AS Description, 'Network' AS Type, " + Convert.ToInt32(drRunDef["Network"].ToString()) + " as VarType_FK, -1 AS ModelVersion FROM [Network];";
                                break;
                            default:
                                break;
                        }
                        List<DAL.DBContext_Parameter> lstParam2 = new List<DAL.DBContext_Parameter>();
                        //                   DAL.DBContext_Parameter paramSim = new DAL.DBContext_Parameter("@Sim", SIM_API_LINKS.DAL.DataTypeSL.INTEGER, nSimID);
                        DAL.DBContext_Parameter paramNetwork = new DAL.DBContext_Parameter("@NetworkID", SIM_API_LINKS.DAL.DataTypeSL.INTEGER, nNetworkID);
                        //               lstParam2.Add(paramSim);
                        lstParam2.Add(paramNetwork);
                        DataSet dsOrigIWM = _dbContextMODEL.getDataSetfromSQL(sql_table, lstParam2);
                        //  ***********  MET **********  unclear quite what this is doing...  figure out if you need the components
                        /*
                        DataSet dsNewIWM = new DataSet();
                        OleDbDataAdapter daNewIWM;
                        if (dr_Dict["TableName"].ToString() == "Run")       //can't pull all records from table run; or don't want to, at least.
                        {
                            daNewIWM = cu.getDataAdapterfromSQL("SELECT RunID, Name, Inflow, Level_,  Trade_Waste,  Waste_Water, Network, Rainfall_Event,  Ground_Infiltration, Sim, ModelVersion FROM Run;", connIW);
                        }
                        else
                        {
                            daNewIWM = cu.SWMM_GetTableByElementType(ref dsNewIWM, dr_Dict["TableName"].ToString(), "Empty", -1, -1, connIW);
                        }

                        daNewIWM.Fill(dsNewIWM);
                         * 
                         * */


                        string sSQL_for_insert = "SELECT  subID, subcatchment_id, total_area, x, y, area_absolute_1, area_absolute_2, wastewater_profile, trade_profile, population, rainfall_profile, node_id, contributing_area, catchment_dimension, -1 as ModelVersion, '' as Description,area_measurement_type,excluded,Version FROM hw_subcatchment";        //met cheat- how was this ppossibly working before?
                        int i = 0;
                        //  set the dr for each record...
                        DataSet dsNewIWM = dsOrigIWM.Clone();
                        dsNewIWM.Tables[0].Columns["ModelVersion"].ReadOnly = false;
                        foreach (DataRow dr in dsOrigIWM.Tables[0].Rows)
                        {
                            //dsNewIWM.Tables[0].Rows.Add(dr);
                            dsNewIWM.Tables[0].ImportRow(dr);

                            //  dsNewIWM.Tables[0].Rows[i]["ModelVersion"] = nScenarioID;
                            //     dsNewIWM.Tables[0].Rows[i].set
                            dsNewIWM.Tables[0].Rows[i].SetAdded();
                            dsNewIWM.Tables[0].Rows[i]["ModelVersion"] = nScenarioID;
                            i = i + 1;
                        }
                        //         sImportStatus = sImportStatus + dr_Dict["ComponentName"].ToString() + ": " + i.ToString() + "\r\n";
                        //       daNewIWM.InsertCommand = new OleDbCommandBuilder(daNewIWM).GetInsertCommand();
                        //     daNewIWM.Update(dsNewIWM);

                        _dbContext.InsertOrUpdateDBByDataset(true, dsNewIWM, sSQL_for_insert);              //sql_table);
                    }

                }


              //      if (true)
                //      {      //create the option lists for the run definition components 
                // not a huge priority                        insertOptionList_RunComponents(nScenarioID, 2, nProjID);
                //     }

                    /*
                    for (int i = 0; i < dsMyDs.Tables[0].Columns.Count; i++)
                    {         //go through and set the columns/ NOTE: ImportRow and setting it equal to the row FAILED
                        newRecord[i] = dsMyDs.Tables[0].Rows[0][i];
                    }
                    newRecord["ParentID"] = dsMyDs.Tables[0].Rows[0][row["KeyColumn"].ToString()];     //reference the elementID of the "parent" record- helps to track records

                    dsMyDs.Tables[0].Rows.Add(newRecord);
                    dsMyDs.Tables[0].Rows[1][row["FieldName"].ToString()] = row["val"].ToString();          //set the new value
                    dsMyDs.Tables[0].Rows[1]["ModelVersion"] = row["ScenarioID_FK"].ToString();             //set the new scenario
                    da.InsertCommand = new OleDbCommandBuilder(da).GetInsertCommand();
                    da.Update(dsMyDs);*/

                finally
                {

                }
            }
            return "";
        }

        //grab the IDs of the field types 
        private DataRow iwGetRunComponents()
        {
            string sql = "select * from qryUI_Component002_RunComponents";
            DataSet dsSP = _dbContext.getDataSetfromSQL(sql);
            return dsSP.Tables[0].Rows[0];
        }

        public void testc()
        {
            string s = _iw.BrowseForObjects("Selection List", "Selection a Selection list", ">0", 0, 0);
            int nSimID = 175;
            string sSim = IW_CreateScriptingPath(nSimID, "Sim", _iw.MasterDatabase);
            _iw.ExportResults(sSim, "CSV", @"C:\a\test34.csv", "ds_flow", s, 0, 0);


        }


        #endregion
        #region RunProcessing
        public override void ProcessEvaluationGroup(string[] astrScenarioId2Run)
        {
            DataSet dsEvals = ProcessEG_GetGS_Initialize(_nActiveEvalID, astrScenarioId2Run);       //, nRefScenarioID);
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
                string sFileIWM_NEW = dr["ModelFile_Location"].ToString();                 //sequence (for cohort)
                _nScenarioID_AltFramework = Convert.ToInt32(dr["AltScenarioID"].ToString());            //use this scenario ID for iwm tracking if desire
                _nScenSQN = Convert.ToInt32(dr["sqn"].ToString());
                if (_bTestForIW_MaxFileSize && !_bSKIP_IW_Init)
                {
                    bool bReInit = false;
                    if (_dbContextMODEL.RectifyTestMeetsLimit())
                    {
                        _iw = null;
                        _iw = new InfoWorksLib.InfoWorks();
                        bReInit = true;
                        string sFileCurrent = _dbContextMODEL.GetFilenameFromConnectionString();
                        sFileIWM_NEW = CommonUtilities.RMV_FixFilename(System.DateTime.Now.ToString()) + ".iwm";
                        sFileIWM_NEW = Path.Combine(Path.GetDirectoryName(sFileCurrent), sFileIWM_NEW);
                        File.Copy(sFileCurrent, sFileIWM_NEW);
                        string sConnNew = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sFileIWM_NEW;
                        _dbContextMODEL.Close();
                        _dbContextMODEL.Init(sConnNew, DAL.DB_Type.OLEDB);
                    }
                    bool bNewDBDoesNotMeetSizeLimits = false;
                    bool bCR_Performed = _dbContextMODEL.RectifySizeLimitation(out bNewDBDoesNotMeetSizeLimits);

                    if (bNewDBDoesNotMeetSizeLimits)
                    {
                        Console.WriteLine("db cannot be compacted. figure out a solution");
                        break;
                    }

                    if (bReInit)
                    {
                        _sMasterDatabase = sFileIWM_NEW;
                        _iw.InitForTest(0, "", "");
                        _iw.MasterDatabase = sFileIWM_NEW;              //set to the new master database
                    }
                    bReInit = false;
                }

                try
                {
                    ProcessScenario(nProjID, _nActiveEvalID, _nActiveReferenceEvalID, sFileLocation, nScenarioID, nScenStart, nScenEnd, sDNA);
                    _log.WriteLogFile();
                }
                catch (Exception ex)
                {
                    //todo: log the issue
                }

            }
        }
        /// <summary>
        /// wraps process sceanario (which is not a virtualized function)
        /// code was triggering a weird memory error when that function was called (as an override to function in simlink.cs)
        /// this is needed for optimization
        /// </summary>

        /// <returns></returns>

        public override int ProcessScenarioWRAP(string sDNA, int nScenarioID, int nScenStartAct, int nScenEndAct, bool bCreateIntDNA = true)
        {
            if (bCreateIntDNA)
                sDNA = ConvertDNAtoInt(sDNA);       //round any doubles to int
            int nReturn = ProcessScenario(_nActiveProjID, _nActiveEvalID, _nActiveReferenceEvalID, _sActiveModelLocation, nScenarioID, nScenStartAct, nScenEndAct, sDNA);
            return nReturn;
        }
        /// <summary>
        /// Create a new run group at the same level as existing 
        /// 
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <returns></returns>
        private string CreateRunGroup(int nScenarioID)
        {
            string spRunGroup = _iw.get_Parent(_sRunSP);    // base runs scripting path

            string sCG = _iw.get_Parent(spRunGroup);
            string sNewRunGroupName = CommonUtilities.RMV_FixFilename(nScenarioID.ToString() +"_" + DateTime.Now,".");
            string sRG_Name = _iw.get_ShortDisplayName(spRunGroup);
            string sSQL = "SELECT Archived, [Catchment Group], Comment, CreationGUID, ModificationGUID, ModifiedBy, Name, WhenCreated, WhenModified FROM Project where (name = '" + sRG_Name + "')";
            DataSet ds = _dbContextMODEL.getDataSetfromSQL(sSQL);        //search on cg too?
            string sSQL_Return = CreateRunGroupSQL(sNewRunGroupName, ds.Tables[0]);
            _dbContextMODEL.ExecuteNonQuerySQL(sSQL_Return);
            string[] sTest = new string[] { "rg", "rungroup", "project", "rung", "RUNG", "Rung", "RG", "run_group", "Project", "proj","run","Run","Proj","Rungroup" };
            string sNewRunGroup = spRunGroup.Substring(0, spRunGroup.LastIndexOf('>') + 1) + "RUNG~" + sNewRunGroupName;
            
            //  string sSQL = "SELECT Archived, Comment, CreationGUID, Frozen, ID, ModificationGUID, ModifiedBy, Name, WhenArchived, WhenCreated, WhenModified, hotlinks, Hidden, Archive FROM Project where (name = '" + sRG_Name + "')";
        //    string sSQL2 = "SELECT Archivf, Comment, CreationGUID, Frozen, ID, ModificationGUID, ModifiedBy, Name, WhenArchived, WhenCreated, WhenModified, hotlinks, Hidden, Archive FROM ProjecT";
    //        spRunGroup= TestRunG_Insert(sTest,sCG,sNewRunGroupName);
       //     DataSet ds = _dbContextMODEL.getDataSetfromSQL(sSQL);        //search on cg too?
       //     ds.Tables[0].Rows[0]["Name"] = sNewRunGroupName;
      //      ds.Tables[0].AcceptChanges;
       //     ds.Tables[0].Rows[0].AcceptChanges();
        //    ds.Tables[0].Rows[0].SetAdded();
      //      int nKey = _dbContextMODEL.InsertOrUpdateDBByDataset(true, ds, sSQL2, true, true);
       //     sNewRunGroup=spRunGroup.Substring(0,spRunGroup.LastIndexOf('>'))+sNewRunGroupName;

        //    string sNewRunGroup = _iw.NewObject(sCG, sNewRunGroupName, "RUNG");
      //      tryn
       //     {
        //         sNewRunGroup = _iw.NewObject(sCG, sNewRunGroupName, "rung");            //fuck everybody!   
         //   }
//            catch(Exception excel_link){
  //              sNewRunGroup = _iw.NewObject(sCG, sNewRunGroupName, "project");
    //            int n = 1;
      //      }
            return sNewRunGroup;              // sNewRunGroup;          //for now, just return the same rg sNewRunGroup;
        }

        /// <summary>
        /// terrible waste of time
        /// but i wanted a run group, and nothing else would work.
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        private string CreateRunGroupSQL(string sName, DataTable dtSource)
        {
            string sCols = "(";
            string sVals = "(";
            string sTheVal = "";
            Guid myguid = Guid.NewGuid();
            string guid = "{" + myguid.ToString().ToUpper() + "}";
            foreach (DataColumn dc in dtSource.Columns)
            {
                if(dc.ColumnName=="Catchment Group")
                    sCols += "[Catchment Group],";
                else
                    sCols += dc.ColumnName + ",";
                switch (dc.ColumnName)
                {
                    case "Name":
                        sTheVal = sName;
                        break;
                    case "CreationGUID":
                        sTheVal = guid;
                        break;
                    case "ModificationGUID":
                        sTheVal = guid;
                        break;
                    case "WhenCreated":
                        sTheVal = System.DateTime.Now.ToString();
                        break;
                    case "WhenModified":
                        sTheVal = System.DateTime.Now.ToString();
                        break;
                    default:
                        sTheVal =dtSource.Rows[0][dc].ToString();
                        break;
                }
                sVals += "'" + sTheVal + "',";
            }
            sCols = sCols.Substring(0, sCols.Length - 1) + ")";
            sVals = sVals.Substring(0, sVals.Length - 1) + ")";
            string sSQL = "insert into project " + sCols + " VALUES " + sVals;
            return sSQL;
        }

        private string TestRunG_Insert(string[] sTries, string sParent, string sNewRunGroupName)
        {
            string spRunGroup = _iw.get_Parent(_sRunSP);
            string sCG = _iw.get_Parent(spRunGroup);
            string sNewRunSP="";
            for (int i = 0; i < sTries.Length; i++)
            {
                    try
                {
                    sNewRunSP = _iw.NewObject(sCG, sNewRunGroupName, sTries[i]);            //fuck you wallingford for writing the worst fucking documentation    
                }
                catch(Exception ex){
                    Console.WriteLine("fail:" + sTries[i] + ex.Message);
                }
            }
            return sNewRunSP;
        }


        public int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINP_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1")
        {
            string sPath; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName; string sINIFile; string sSummaryFile; string sOUT;
            int nCurrentLoc = nScenStartAct; string sTS_Filename = ""; string iwTheSim = "";
            string spNewNetwork = "NOT_SET";
			string iwNewRun = "";
            int nScenarioID_IWM = GetScenarioForName(nScenarioID);      //  use this everywhere the scen id is used to generate a name.

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
                    ScenarioGetExternalData();

                    ScenarioPrepareFilenames(nScenarioID, nEvalID, out sTargetPath);
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

                        //SP 15-Jun-2016 - no longer needed
                        /*if (_dbContext._DBContext_DBTYPE == 0)      //if access
                        {
                            _dbContext.OpenCloseDBConnection();
                        }*/
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineFileSetup) && (nScenEndAct >= CommonUtilities.nScenLCBaselineFileSetup))
                    {
                        _log.AddString("IW Directory Setup Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        string spRunGroupToUse = "";
                       
                        // NOTE: This is the right idea (commented out), but can't get a new run group inserted.... 
                        //resolution: wait until we have new api with icm and resolve.
                        if (_bIsCohort)
                        {
                            if (_bIsLeadEGInCohort)
                            {
                                // create run group
                                string spNewRunGroup = CreateRunGroup(nScenarioID_IWM);
                                _cohortSpecIW._dictRunGroupSQN.Add(_nScenSQN, spNewRunGroup);
                                spRunGroupToUse = spNewRunGroup;
                  //              _nCohortSQN++;
                                if (!_bSKIP_IW_Init)                // you have to re-initialize to get get the new run group. balls.
                                {
                                    _iw = new InfoWorksLib.InfoWorks();
                                    _iw.InitForTest(0, "", "");
                                    _bIW_IsInitialized = true;

                                }
                            }
                            else
                            {           // in cohort, but not in lead. run group should already be created.
                                spRunGroupToUse = _cohortSpecIW._dictRunGroupSQN[_nScenSQN];
                       //         _nCohortSQN++;
                            }
                        }


                        string sNewRunName = _iw.get_ShortDisplayName(_sRunSP);
                        sNewRunName = CommonUtilities.RMV_FixFilename(nScenarioID_IWM.ToString() + sNewRunName.Substring(sNewRunName.IndexOf('_')));        //assume type SCEN_XXX
                        iwNewRun = _iw.NewObject(spRunGroupToUse, sNewRunName, "Run");
                        IW_CopyRunValues(_sRunSP, iwNewRun);
                        // create the sim
                        string sNewSimName = _iw.get_Value(_sSimSP, "Rainfall Event");
                        string sNewShortSimName = _iw.get_ShortDisplayName(sNewSimName);            //get the name of the rainfall event
                        if (sNewSimName == "")
                        {
                            sNewSimName = "DWF";        //handle the case where there is no rainfall event
                        }
                        iwTheSim = _iw.NewSim(iwNewRun, sNewShortSimName, sNewSimName);
                        WriteSim(iwTheSim, sTargetPath);        // push the sim
                        nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;          //
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
                        string sNewNetworkName = nScenarioID_IWM.ToString() + "_" + DateTime.Now;
                        sNewNetworkName = CommonUtilities.RMV_FixFilename(sNewNetworkName,".");         //make sure the filename meets certain standards
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
                            string sUpdateCSV = Path.Combine(sTargetPath, "update_" + nScenarioID.ToString() + ".csv");
                            if (_bScenUpdateFileUserDefined)
                            {
                                sUpdateCSV = CreateStringFromCode(nScenarioID, out sNewNetworkName, true);             //chance for user to overwrite the provided file
                            }
                            else
                                IW_WriteNetworkCSV(nScenarioID, 2, sUpdateCSV);             //only write the CSV if needed.
                            object[] files = new object[1];
                            files[0] = sUpdateCSV;                  // _sMasterDatabase;                    //iw api appears to requires this
                            spNewNetwork = _iw.Import(_spBaseNetwork, sNewNetworkName, "Network", "CSVUP", files);
                            _iw.CheckIn(spNewNetwork);
                            if (_bIsCohort)
                                _cohortSpecIW._dictNetwork.Add(_nScenSQN, spNewNetwork);      // add the sp to the dictionary
                        }
                        else
                        {   //special case: use the lead cohort EG versino of the scripting path.
                            spNewNetwork = _cohortSpecIW._dictNetwork[_nScenSQN];          // use the stored scripting path
                        }
                        _iw.set_Value(iwNewRun, "Network", spNewNetwork);
                        //step 1 write the CSV

                        //               Update_INP(sIncomingINP, nScenarioID, sTarget_INP);

                        nCurrentLoc = CommonUtilities.nScenLCBaselineModified;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        _log.AddString("Model execution begin: " + System.DateTime.Now.ToString(), Logging._nLogging_Level_2);
                        // todo: add code to run the new model
                        //todo: much of this does not have to be done each sim; so 
                        //         string spRunGroup = _iw.get_Parent(_sRunSP);       //get the parent run group
                        //         string sNewRunName = CommonUtilities.RMV_FixFilename(nScenarioID.ToString() + "_" + DateTime.Now);
                        //         string iwNewRun = _iw.NewObject(spRunGroup, sNewRunName, "Run");

                        //if (spNewNetwork == "NOT_SET")           //network exists, just want to run it (not recreate)
                        //{
                        //    // not yet supported
                        //    Console.WriteLine("not yet suppor ed; must create the network first");
                        //}
                        //else
                        //{
                        //    // don't need to do anything
                        //}

                       
                        // moved run set up to step 4
                        if (iwTheSim == "")
                            iwTheSim = GetSim(sTargetPath);         //retrieve if needed
                        _iw.Run(iwTheSim);
                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        _log.AddString("IW Results Read Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        string sPRN_OUT = Path.Combine(sTargetPath, "iw" + nScenarioID + ".prn");
                        if (!_bSKIP_IW_Init)
                            _iw.Export(iwTheSim, "PRN", sPRN_OUT);              //if this is skipped, you must already have it somehow.
                        IW_Read_PRN_File(nEvalID, sPRN_OUT, nScenarioID);


                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                        if (_dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Primary).ToString()).Count() > 0)        //5/15/14- skip HDF5 create if no results //SP 15-Feb-2017 Primary only
                        {
                            //MET 12/9/2013 - manage HDF OUTSIDE the readOUT logic... - it shouldn't be troubled with knowing about where the data will go.
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
        /// export sim id so if needed the run can be picked up later
        /// </summary>
        /// <param name="iwTheSim"></param>
        /// <param name="sTargetPath"></param>
        public void WriteSim(string iwTheSim, string sTargetPath)
        {
            string[] sWriteOut = new string[]{iwTheSim};
            string sFilename=Path.Combine(sTargetPath,"sim.txt");
            File.WriteAllLines(sFilename,sWriteOut);
        }

        /// <summary>
        /// Returns the simulation that was stored previously
        /// </summary>
        /// <param name="sTargetPath"></param>
        /// <returns></returns>
        public string GetSim(string sTargetPath)
        {
            string sFilename = Path.Combine(sTargetPath, "sim.txt");
            StreamReader stream = new StreamReader(sFilename);
            string s = stream.ReadLine();
            return s;
        }

        public void IW_CopyRunValues(string sRefRun, string sNewRun)
        {
            try
            {
                string sql = "select val as RunColumns from tlkpIW_Dictionary where (Qualifier='Run')";
                DataSet dsSP = _dbContext.getDataSetfromSQL(sql);

                foreach (DataRow dr in dsSP.Tables[0].Rows)
                {
                    string sCurrentProperty = dr["RunColumns"].ToString();
                    object oNewVal = _iw.get_Value(sRefRun, sCurrentProperty);
                    //  Debug.WriteLine("CurrentProperty: " + sCurrentProperty);
                    //Debug.WriteLine("New Val: " + oNewVal);
                    if (oNewVal.ToString().Length > 0)
                    {
                        if (sCurrentProperty == "Inflow" || sCurrentProperty == "Experimental")
                        {
                            //manually set to the db; i have a patch for this bug in IW but haven not applied yet ;believe it is fixed in later versions
                            try
                            {
                                _iw.set_Value(sNewRun, sCurrentProperty, oNewVal);          //test with new
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        else
                        {
                            _iw.set_Value(sNewRun, sCurrentProperty, oNewVal);     //set the value for the new run to the value from the reference run
                        }
                    }
                }
                // MET 8/12/2011 get rid of oledbstuff in trying to resolve memory leak
                dsSP.Dispose();


            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }


        #endregion

        #region IW API Functions

        #region NETWORK UPDates

        #region UpdateCSV

        /// <summary>
        /// version to be created which links to in memory storage...
        /// </summary>
        public void IW_WriteNetworkCSV2()
        {

        }



        public void IW_WriteNetworkCSV(int nScenarioID, int nActiveModelType, string fileBaseIWM)  //int nProjID, int nIWRunDI,
        {
            //todo change this to a request like swmm/epanet
            System.IO.StreamWriter file_INP_Out = new System.IO.StreamWriter(fileBaseIWM);
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
            catch (Exception ex)
            {
                //  return null; //cu.CreateExceptionTable(ex);
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

        /// <summary>
        /// Read a PRN file into the results summary
        /// 
        /// some weaknesses with current approach, which should be impreoved for speed and handling errors... anyway, works for now
        /// </summary>
        /// <param name="nEvalId"></param>
        /// <param name="sFileName"></param>
        /// <param name="nScenario"></param>
        public void IW_Read_PRN_File(int nEvalId, string sFileName, int nScenario)
        {
            if (File.Exists(sFileName))
            {
                StreamReader file = null;

                try
                {
                    Boolean bRESULT_ImportAll = false;  // met this doesn't work no more Convert.ToBoolean(cu.GetValueFromGeneralTable("tblEvaluationGroup", "RESULT_ImportAll", "EvaluationID=" + nEvalId, connRMG).ToString());
                    //             DataTable dtResultVar = IW_GetResultVars_PRN(nEvalId); ;
                    DataSet dsResults = new DataSet();  //  load the requested summary results into here...
                    int nCounter = 0;

                    string sbuf;
                    string sAssetSection = "empty";                  //1 nodes 2 links
                    int nExit = 0;
                    int nNumResults = _dsEG_ResultSummary_Request.Tables[0].Rows.Count;
                    //      DataTable dt = _dsEG_ResultSummary_Request.Tables[0];       // quick for compile ; needs whole rework
                    file = new StreamReader(sFileName);
                    string[] sLines = File.ReadAllLines(sFileName);
                    int nCurrentLineCounter = -1;
                    string sLastElement = "NOT_SET"; string sCurrentElement = "NOT_SET"; bool bFound;
                    int nNodeLocation = 0; int nLinkLocation = 0;
                    bool bIsSearchFromBeginning = false;
                    while (!file.EndOfStream & nExit != 1)
                    {
                        // return if the last element has been found
                        if (nCounter == nNumResults)
                            break;


                        nCurrentLineCounter++;
                        sbuf = sLines[nCurrentLineCounter];
                        if (sbuf.Length > 2) { if (sbuf.Substring(0, 2) == " +") { nExit = 1; } };
                        if (sbuf.Trim() == "********** Node data **********")
                        {         // Node section of .prn
                            sAssetSection = "Node";
                            nNodeLocation = nCurrentLineCounter + 1;
                        }
                        if (sbuf.Trim() == "********** Link data **********")
                        {
                            if (_dsEG_ResultSummary_Request.Tables[0].Rows[nCounter]["TableName"].ToString().ToUpper() == "NODE")
                            {
                                if (bIsSearchFromBeginning)
                                {   //we have come back to beginning, not found it... the reult is not there.
                                    bIsSearchFromBeginning = false;
                                    nCurrentLineCounter = nNodeLocation;
                                    _log.AddString("Result not found: " + sCurrentElement, Logging._nLogging_Level_2);
                                    nCounter++; // advance to the next result

                                }
                                else
                                {
                                    nCurrentLineCounter = nNodeLocation;
                                    bIsSearchFromBeginning = true;      //make sure we don't keep coming back here.
                                }
                            }
                            else
                            {
                                sAssetSection = "Link";
                                nLinkLocation = nCurrentLineCounter;
                            }
                        }      // Node section of .prn
                        if (HELPER_IW_PRN_IsDataLine(sbuf) & (sAssetSection != "empty") & (nCounter < nNumResults))     // removed the condition:     & (nExit != 1)
                        {
                            //do some fix on this .... quick fix to compile in update
                            do
                            {
                                DataRow dr = _dsEG_ResultSummary_Request.Tables[0].Rows[nCounter];
                                sCurrentElement = dr["Element_Label"].ToString();
                                if (nCounter == 9613)
                                    sCurrentElement = sCurrentElement;
                                bFound = IW_ReadPRNRowIntoDB(sbuf, nScenario, sAssetSection, dr, ref nCounter, false);
                            } while ((sCurrentElement == sLastElement) && bFound && (nCounter < nNumResults));
                            sLastElement = sCurrentElement;     //now set the name that we will use in the next loop
                        }
                    }


                    // now insert the records into the database. SP 21-Jul-2016 Moved this out of here to WriteResultsToDB - write back to the database at the end only - missed this previously!
                    //ResultsSummary_WriteToRepo(nScenarioID);

                }  //end try
                finally
                {
                    if (file != null)
                        file.Close();
                }  //end finally

            }  //end using  

        }
        public Boolean HELPER_IW_PRN_IsDataLine(string sbuf)
        {
            if (sbuf.Trim().Length == 0) return false;                      //skip blank line
            if (sbuf.Substring(0, 2).Trim().Length != 0) return false;      //first two char must be spaces
            if (sbuf.Trim().Substring(0, 1) == "<") return false;      //first two char must be spaces
            if (sbuf.Trim() == "********** Node data **********" || sbuf.Trim() == "********** Link data **********") return false;         //not a data row
            string sEntry = sbuf.Trim().Substring(0, 4);                    // find header lines            // more complete, but slower, shouldn't be an issue rare case sbuf.Trim().IndexOf(" "));
            if (sEntry == "Link" || sEntry == "Node" || sEntry == "Refe") return false;
            if (sbuf.Trim().Substring(0, 1) == "(") return false;
            return true;            //we have pasesed all tests
        }

        //Read the IW row into the table
        /// <summary>
        /// copied into IW automation file
        /// likely this can be improved significantly.. 
        /// 
        /// 4/13/16: changed to only pass a datarow; all else happens on simlink vars
        /// </summary>
        /// <param name="sbuf"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="sAssetSection"></param>
        /// <param name="dsRES"></param>
        /// <param name="dsFieldDict"></param>
        /// <param name="dtResultVar"></param>
        /// <param name="nCounter"></param>
        /// <param name="bRESULT_ImportAll"></param>
        public bool IW_ReadPRNRowIntoDB(string sbuf, int nScenarioID, string sAssetSection, DataRow dr, ref int nCounter, Boolean bRESULT_ImportAll = false)
        {
            //Debug.WriteLine(sbuf);
            string sbuf_orig = sbuf;
            string sElementName = sbuf.Trim().Substring(0, sbuf.Trim().IndexOf(" "));
            int nCol = 2; string sEntry; int nCurrentRow; int nPRN_Col;
            Boolean bIW_IsNonConduitLink = false; Boolean bIncrementCounter = false;
            if (sAssetSection == "Link") { bIW_IsNonConduitLink = IW_IsNonConduitLink(sbuf); }

            //
            Boolean bReadRow = bRESULT_ImportAll;
            if (!bReadRow)
            {
                if (sElementName == dr["Element_Label"].ToString().ToUpper())           //matches the list item
                {
                    bReadRow = true;
                    bIncrementCounter = true;
                }
                else
                {
                    return false;           //not found
                }
            }

            //          if (sElementName=="CO-OPCITYNORTHPS"){
            //             bReadRow = true;
            //        }

            if ((sElementName.IndexOf("_O") < 0) & bReadRow)                     //PRN contains _O stuff for drainage nodes; model values more similar to non _O, so drop these records; question to IW about what the heck.
            {
                bool bContinue = true;
                if (dr["FeatureType"].ToString() == sAssetSection)
                {         //check that record type matches the code
                    sbuf = sbuf.Trim();
                    if (sbuf.Trim().IndexOf(" ") > 0)           // get rid of column name
                    {
                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" "));
                        sbuf = sbuf.Trim();
                    }
                    //get the correct text column of the PRN file to use   (seems to be 1-indexed in tlkp)
                    if (bIW_IsNonConduitLink)
                    {
                        nPRN_Col = Convert.ToInt32(dr["AltColumnNo"].ToString());  //weir sluice etc
                    }
                    else
                    {
                        nPRN_Col = Convert.ToInt32(dr["ColumnNo"].ToString());        //conduit
                    }
                    sbuf = CommonUtilities.RemoveRepeatingChar(sbuf, ' ');
                    string[] sVals = sbuf.Split(' ');


                    // this stuff is from before...
                    //handling annoying cases- may need to address..
                    /*                if (sbuf.IndexOf(" ") > 0)
                                    {
                                        sEntry = sbuf.Substring(0, sbuf.IndexOf(" "));
                                    }
                                    else
                                    {
                                        sEntry = sbuf;
                                    }
                                    if ((sEntry.IndexOf("+") > 0) || (sEntry.IndexOf("x") > 0))
                                    {     //for fuck sake, _iw...                                    
                                        sEntry = sEntry.Substring(0, sEntry.Length - 1);             // crop it
                                    }
                                    if ((sEntry.IndexOf("%%") > 0))
                                    {     //for fuck sake, _iw...                                    
                                        sEntry = sEntry.Substring(0, sEntry.Length - 2);             // crop %%
                                    }
                */
                    double dVal = Convert.ToDouble(sVals[nPRN_Col - 2]) * 1; //  todo: support unit conversion in future * Convert.ToDouble(dr["UnitConversion"].ToString());
                    int nResultID = Convert.ToInt32(dr["Result_ID"].ToString());
                    int nElementID = Convert.ToInt32(dr["ElementID_FK"].ToString());
                    ResultSummaryHelper_AddValToDS(nScenarioID, nResultID, nElementID, dr["Element_Label"].ToString(), dVal, -1);
                    nCounter++;
                }
                else
                {           //skip a column; eat up a column in the row.
                    if (sbuf.IndexOf(" ") > 0)
                    {             //if there are any columns left... if not there is a problem
                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" "));
                        sbuf = sbuf.Trim();
                    }
                    else
                        Console.WriteLine(sbuf_orig + "ncol: " + nCol);

                }
                //            if (bIncrementCounter) nCounter++;          //increment counter because we have passed through another ID
            }
            return true;
        }

        //tests whether a data line of the .prn file is not your basic conduit (has certain missing columns; AltColumn should be used)
        public Boolean IW_IsNonConduitLink(string sbuf)
        {
            sbuf = sbuf.Trim();
            sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" "));        //eat first column
            sbuf = sbuf.Trim();
            sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" "));
            sbuf = sbuf.Trim().Substring(sbuf.Trim().IndexOf(" "), 25);        //get a 25 char string at the first space
            if (sbuf.Length - sbuf.Replace(" ", "").Length < 24) { return false; }         //mostly non spaces; not a non-conduit

            return true;            //has lots of blanks; it's a weir or something
        }

        #endregion


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