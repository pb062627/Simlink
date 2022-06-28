using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data.DataSetExtensions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIM_API_LINKS.DAL;
using muMathParser;                                     //parse path expressions
using CIRRUS_HTC_NS;
using System.IO;
using dss_wrap;
using OptimizationWrapper;
using Optimization;                     // only used to reference the specif type of algo... move that elsehwer?
using Nini.Config;
using SIM_API_LINKS.DBVV;
using System.Threading;
using SimLink_HPC_NS;                   // link to jk hpc class.

namespace SIM_API_LINKS
{
    //SP 18-Feb-2016
    public enum LinkedDataType
    {
        NotSet = -1,
        ModelElements = 1,
        ResultSummary = 2,
        ResultTS = 3,
        DVOptions = 4,
        Event = 5,
        Performance = 6
    }

    //SP 4-Aug-2016
    public enum SecondaryDVType
    {
        NotASecondaryDV = -1,
        OptionSetFromOptionList = 1,
        OptionSetFromPrimaryDV = 2,
        MaintainExistingValue = 3 //Not sure how this works exactly
    }

    /// <summary>
    /// Add code for when the scen details are deleted.
    /// Added during rt setup - need greater control of this
    /// </summary>
    public enum DeleteScenDetails
    {
        BeforeRun =0,
        AfterRun,
        Manual

    }


    //SP 4-Aug-2016
    public enum OperationType
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Mult_Inv,
        Identity,
        Complement
    }

    public enum ComputeEnvironment
    {
        LocalMachine,
        AWS,
        Condor,
        LocalViaHPC
    }


    public enum UIDictionary
    {
        ModelType,
        FieldDictionaryTable,
        UnitSettings,
        SWMM_Out,
        ElementLibrary,
        DistribStrategyCat,
        SystemType
    }

    /// <summary>
    /// Define what type of analysis Simlink will run.
    /// </summary>
    public enum SimlinkRunType
    {
        Optimization,
        UserDefinedRuns,
        realtime
    }

    /// <summary>
    /// Note: This varies slightly from the common utilities and tlkpModelType numbers.
    /// Definite need to standardize.
    /// Thought: The number here may, or may not need to correspond to ModelTypeID?
    /// 
    /// </summary>
    public enum SimulationType
    {
        Undefined,
        SWMM,
        ICM,
        EPANET,
        MODFLOW,
        MIKE_URBAN,
        SIMCLIM,
        FLOODMODELLER,
        FLOODMODELLER2D,
        FLOODMODELLER_FAST,
        EXTENDSIM,
        VISSIM
    }


    public enum RetrieveCode
    {
        Primary = 1,
        Secondary = 2,
        Aux = 3,
        AuxEG = 4
    }

    public enum AssociatedFileSupport
    {
        SingleFile = 0,
        FilesWithSameFileName = 1,
        AllFilesInFolder = 2
    }

    public partial class simlink
    //simlink is the v2.0 replacement for rmg_db
    //model linkage classes inherit from this class
    {

        #region HelperClasses
        public class IntermediateStorageSpecification
        {
            public bool _bScenario { get; set; }
            public bool _bMEV { get; set; }
            public bool _bResultSum { get; set; }
            public bool _bResultTS { get; set; }
            public bool _bResultEventSummary { get; set; }
            public int _nPerformanceCode { get; set; }

            /// <summary>
            /// code for storing in DB (on EG 'IntermediateStorageCode')
            /// </summary>
            public enum IntermediateStorageSpecENUM
            {
                ALL = 1,
                SkipTimeSeries = 20,
                MEVResultSummaryAndPerformance = 30,
                PerformanceALL = 70,
                PerformanceSEQ_GT_10 = 80,
                PerformanceOptOnly = 90,
                None = 100
            }


            /// <summary>
            /// set scenario life cycle for intermediate storage
            /// </summary>
            /// <param name="nStorageCode"></param>
            public void SetIntermediateStorage(int nStorageCode)
            {
                // scenario
                if (nStorageCode != 100)
                {          //Convert.ToInt32(IntermediateStorageSpecENUM.None.ToString())){
                    _bScenario = true;
                }
                else
                {
                    _bScenario = false;
                }
                // MEV, result summary, and events
                if (nStorageCode <= 30)          //Convert.ToInt32(IntermediateStorageSpecENUM.MEVResultSummaryAndPerformance.ToString()))
                {
                    _bMEV = true;
                    _bResultSum = true;
                    _bResultEventSummary = true;
                }
                else
                {
                    _bMEV = false;
                    _bResultSum = false;
                    _bResultEventSummary = false;
                }
                //ts
                if (nStorageCode < 40)          //Convert.ToInt32(IntermediateStorageSpecENUM.ALL.ToString())) //SP 16-Oct-2016 Need a way to save resultTS without other metrics - needed for performance
                {
                    _bResultTS = true;
                }
                else
                {
                    _bResultTS = false;
                }
                // performance
                if (nStorageCode <= 70)          //Convert.ToInt32(IntermediateStorageSpecENUM.PerformanceALL.ToString()))
                {
                    _nPerformanceCode = 1;              // Convert.ToInt32(IntermediateStorageSpecENUM.ALL.ToString());
                }
                else
                {
                    _nPerformanceCode = nStorageCode;   // set to storage code; this is used to persist perf
                }
            }
        }

        public class simlinkDetail
        {
            public SimLinkDataType_Major _slDataType { get; set; }  //type of SimLink data
            public int _nRecordID { get; set; }                     //table field PK
            public int _nElementID { get; set; }
            public string _sElementName { get; set; }      //generally not used
            public int _nVarType_FK { get; set; }              //field type or rsult fk
            public string _sVal { get; set; }      //not sure about this: double _dVal = -1;
            public int _nScenarioID { get; set; }
            public bool _bIsInsert { get; set; }            // 4/10/14  need to support specific type of MEV
            public int _nDV_Option { get; set; }            // MEV only

            //constructor that sets all vals
            public simlinkDetail(SimLinkDataType_Major slDT, string sVAL, int nRecordID, int nVarType_FK, int nScenarioID, int nElementID = -1, string sElementName = "", bool bIsInsert = false, int nDV_Option = -1)
            {
                _slDataType = slDT;
                _nElementID = nElementID;
                _sElementName = sElementName;
                _nRecordID = nRecordID;
                _nVarType_FK = nVarType_FK;
                _sVal = sVAL;
                _nScenarioID = nScenarioID;
                _bIsInsert = bIsInsert;
                _nDV_Option = nDV_Option;
            }
            public simlinkDetail()
            {
            }
        }

        // used to house dictionary of table details- loaded once upon model init
        public partial class simlinkTableHelper
        {
            public int _nVarType_FK { get; set; }       //field identifier and dictionary key
            public string _sKeyFieldName { get; set; }
            public int _nTableID { get; set; }
            public string _sFieldName { get; set; }
            public string _sTableName { get; set; }
            public string _sStructureName { get; set; }
            public bool _bIsResult { get; set; }

            // these were added for SWMM purpose but may be useful in other instances
            public int _nModelType_FK { get; set; }
            public int _nFieldNumber { get; set; }
            public bool _bIsScenarioSpecific { get; set; }
            public int _nRowNo { get; set; }
            public int _nColumnNo { get; set; }
            public int _nFieldAPICode { get; set; }
            public int _nSectionNumber { get; set; }
            public string _sSectionName { get; set; }
            public string _sQualifier1 { get; set; }          //met 1/17/14: would like to get rid of these
            public int _nQual1Pos { get; set; }               //met 1/17/14: would like to get rid of these


            /// <summary>
            /// Create a helper object for interacting with the model.
            /// 
            /// This was originally created with SWMM in mind....
            /// 1/11/2019- made the base function (this one) virtual, and override in ICM
            /// TODO - move this actually to sWMM
            /// </summary>
            /// <param name="dr"></param>
            /// <param name="nTrueBit"></param>
            /// <param name="bFakeVar"></param>
            //for now this is intended to be called from SWMM, as field names may not be totally valid across others..
            public simlinkTableHelper(DataRow dr, int nTrueBit, bool bFakeVar)
            {
                _nFieldNumber = Convert.ToInt32(dr["FieldINP_Number"].ToString());
                if (Convert.ToInt32(dr["IsScenarioSpecific"].ToString()) == nTrueBit)
                    _bIsScenarioSpecific = true;
                else
                    _bIsScenarioSpecific = false;
                _nRowNo = Convert.ToInt32(dr["RowNo"].ToString());
                //    _sQualifier1 = dr["Qualifier1"].ToString();
                //    _sQual1Pos = dr["Qual1Pos"].ToString();
                _nVarType_FK = Convert.ToInt32(dr["VarType_FK"].ToString());
                _sKeyFieldName = dr["KeyColumn"].ToString();
                _nTableID = Convert.ToInt32(dr["TableID"].ToString());
                _sFieldName = dr["FieldName"].ToString();
                _sTableName = dr["TableName"].ToString();
                _nSectionNumber = Convert.ToInt32(dr["SectionNumber"].ToString());
                _sSectionName = dr["SectionName"].ToString();

                _sQualifier1 = "NOT YET IMPLEMENTED- figure out a better way";
                _nQual1Pos = -1;
            }

            public simlinkTableHelper(int nVarType_FK, string sKeyFieldName, int nTableID, string sFieldName, string sTableName)
            {
                _nVarType_FK = nVarType_FK;
                _sKeyFieldName = sKeyFieldName;
                _nTableID = nTableID;
                _sFieldName = sFieldName;
                _sTableName = sTableName;
            }

            //SP 22-Jul-2016 For ExtendSim 
            public simlinkTableHelper(int nVarType_FK, string sStructurename)
            {
                _nVarType_FK = nVarType_FK;
                _sStructureName = sStructurename;
            }

            // Created 21 Jun-2016 for Vissim and could be standard for all others moving forward
            public simlinkTableHelper(DataRow dr, int nTrueBit)
            {
                //_nModelType_FK = Convert.ToInt32(dr["ModelType_FK"].ToString());
                _nVarType_FK = Convert.ToInt32(dr["VarType_FK"].ToString());
                _sSectionName = dr["SectionName"].ToString();
                _sFieldName = dr["FieldName"].ToString();
                _nColumnNo = Convert.ToInt32(dr["FieldINP_ColNo"].ToString());
                _nRowNo = Convert.ToInt32(dr["FieldINP_RowNo"].ToString());
                _nFieldAPICode = Convert.ToInt32(dr["FieldAPI_Code"].ToString());
                //do we need field class?
                //if (Convert.ToInt32(dr["IsResult"].ToString()) == nTrueBit)
                //    _bIsResult = true;
                //else
                //    _bIsResult = false;
            }

            public simlinkTableHelper()
            {
            }
        }

        public class simLinkModelChangeHelper : simlinkTableHelper
        {
            public string _sVal { get; set; }
            public int _nElementID { get; set; }       //field identifier and dictionary key
            public string _sElementName { get; set; }
            public int _nRecordID { get; set; }
            public bool _bIsInsert { get; set; }        //this is not (presently 1/17/2014) set until the records are processed...

            //generally, the DV          
            public simLinkModelChangeHelper() { }
        }

        //SP 23-Feb-2016
        public class PerformanceValues
        {
            public double dval { get; set; }
            public double dscalar { get; set; }
            public bool bapplythreshold { get; set; }
            public double dthreshold { get; set; }
            public bool bisabove_threshold { get; set; }

            public PerformanceValues(double val, double scalar, bool applythreshold, double threshold, bool isabove_threshold)
            {
                dval = val;
                dscalar = scalar;
                bapplythreshold = applythreshold;
                dthreshold = threshold;
                bisabove_threshold = isabove_threshold;
            }
        }

        //SP 15-Jun-2016
        public class ResultTimeSeriesValues
        {
            public string sGroupID { get; set; }
            public double dAgg { get; set; }
            public double dScalar { get; set; }
            public bool bapplythreshold { get; set; }
            public double dthreshold { get; set; }
            public bool bisabove_threshold { get; set; }

            public ResultTimeSeriesValues(string ID, double val, double scalar, bool applythreshold, double threshold, bool isabove_threshold)
            {
                sGroupID = ID;
                dAgg = val;
                dScalar = scalar;
                bapplythreshold = applythreshold;
                dthreshold = threshold;
                bisabove_threshold = isabove_threshold;
            }
        }

        #endregion

        #region MEMBERS
        #region SimLinkVars
        public SIM_API_LINKS.DAL.DBContext _dbContext;                 //db access          must be public for CU parser class to ref
        public TSRepository _tsRepo = TSRepository.HDF5;                    //default storage for TS
        public int _nDB_WriteLevel = -1;                                // how/what to write to db   
        //      private enum DataTypesSL = SIM_API_LINKS.DAL.DataTypeSL;

        protected Dictionary<string, string> _dictTemplate = new Dictionary<string, string>();    //TODO- change dict to work with MEV list, not dictionary (actually, still needs to be dictionary for COM??>>
        protected List<simlinkDetail> _lstSimLinkDetail = new List<simlinkDetail>();
        public int _nActiveModelTypeID;
        public SimulationType _simType = SimulationType.Undefined;      //todo: in time use this instead of model type ID- 
        public SimlinkRunType _runType = SimlinkRunType.UserDefinedRuns;
        public int _nActiveScenarioID;
        public int _nActiveBaselineScenarioID;
        public int _nActiveReferenceEG_BaseScenarioID;             // baseline scenario id of reference ID. not fully implemented
        public int _nActiveEvalID = -1;
        public int _nActiveReferenceEvalID = -1;
        public string _strCurrentEGLabel = "";                      //UI_ADD;
        public int _nSimID;                                 //especially used in iw; added met 8/x/16
        public int _nActiveProjID = -1;
        public string _sActiveModelLocation;
        public string _sDNA_Delimiter = ".";
        public bool _bIsSimCondor;                      // to depecrecate in favor of _compute_env 
        bool _bFirstPass = true;
        int _nNumDV = -1;               // 
        protected int _nLoopNumber;                                   //used to store which loop we of a process we are in
        protected int _nLoopsOpenCloseDB=30;     //met test - todo support in xml
        private bool _bRemoveScenarioDataInMem = true;              //if true, vals from _lstSimLinkDetail will be removed for each scenario UNLESS scenario =_nActiveBaselineScenarioID
        public bool _bIsSimLinkLite = false;                       //true if no DB backend. Means can't pick up scenario mid-stream.
        protected template_link _template = new template_link();
        public Logging _log = new Logging();                        //met: test reference log from cli
        protected Logging _logInitEG = new Logging();               // MET: there should only be one log object.... 
        protected Logging _logInitModel = new Logging();
        public CIRRUS_HTC _htc = new CIRRUS_HTC();              // to depecrecate in favor of _hpc  //cirrus object for launching COND
        public SimLink_HPC _hpc;                                // link to an hpc object (could wrap aws/condor/other)
        public Dictionary<string, string> _dictHPC = new Dictionary<string, string>();      //dict defining platform specific controls/preferences- set during init: todo - read from base xml? 
        public ComputeEnvironment _compute_env = ComputeEnvironment.LocalMachine;       // set the default system
        protected IntermediateStorageSpecification _IntermediateStorageSpecification = new IntermediateStorageSpecification();
        public string _sUserFileUpdateKey = "";
        public bool _bSpecialSimlinkNaming = false;                         // key to use the special name.
        public bool _bUseHotStart_CrossScenario = false;                    // whether to use try to use scen output from one scenario for the subsequent (mostly applicable in RT)
        public int _nScenarioHotStart = -1;                                 // track the last scenario used, so you can grab it's hot start if needed
        public string _sHotStart_ToUse = "UNDEFINED";                       // hotstart file to use (set from RT)
        public string _sHotStart_ToCopy = "UNDEFINED";  

        public bool _bScenUpdateFileUserDefined = false;
        public string _sLogLocation_Simlink = @"C:\temp\";
        public bool _bDBWriteOnFail = true;
        public string _sWriteEGToDSSLocation = "";

        public bool _bUseAltFrameworkScen = false;              // if true, use the alt scen name for naming instead of the scenario id (scenID always used for simlink internals)
        public int _nScenarioID_AltFramework = -1;
        public int _nScenSQN = -1;
        public bool _bClearScenarioDetails = true;
        public cohortSpec _cohortSpec;
        public bool _bIsCohort = false; // set to true in InitCohort so later functions know if they are run as part of cohort
        public bool _bIsLeadEGInCohort = true;      //set to false after first EG executes (also for subsequent functions)
        protected bool _bSaveSecondaryAndAuxTS = true;              // set to false if you do NOT want to save these.
        public bool _bUseCostingModule = false;
        public cost_master_wrap.cost_master_simlink _cost_wrap = new cost_master_wrap.cost_master_simlink();  // wrapper to the cost_master

        public int _nTS_WriteMessageToConsoleFreq = 1000;       // write out every n records to let them know you're still alive. can set directly (public)

        public bool _bTSStartOfInterval = true;                 //SP 14-Mar-2017 Model specific indicating whether the resulting TS data corresponds to the start or end of the timeseries interval 
        #endregion

        //  pubic TimeSeries.TimesSeriesDetail _tsdBaseline;    //timeseries detail associated with the sim

        //    public static SIM_API_Links.swmm_link5022 swmm = new SIM_API_Links.swmm_link5022();

        //new functionality in SimLink2 to better handle when something goes wrong
        #region VALIDITY
        bool _bEvalIsValid = true;
        public bool _bScenarioIsValid = true;
        bool _bContinueOnInvalid = true;
        public bool _bPerformVersionValidation = true;     // todo: integrate into standard workflow...

        #endregion

        #region TS_Repository
        //   private TS_ReposiryType = TS_HDF5;
        // private SIM_API_LINKS.hd

        protected hdf5_wrap _hdf5 = new hdf5_wrap();
        public List<ExternalData> _lstExternalDataSources = new List<ExternalData>();
        public List<ExternalData> _lstExternalDataDestinations = new List<ExternalData>();
        #endregion

        #region MEMBER_DATASETS
        public DataSet _dsEG_Cohort;                            // added 12/26/16: Added for realtime support
        public DataSet _dsEG_ScenarioList;
        public DataSet _dsEG_DecisionVariables;
        public DataSet _dsEG_DV_Consequent;


        public DataSet _dsEG_Splint;                                   // added 7/9/14    
        public DataSet _dsElementLibrary;                           //used only when NEW elements are needed (not the general case)
        private DataSet _dsEG_ElementList;                  //stores all element lists- then use LINQ to grab DV specific 
        private DataSet _dsEG_OptionVals;                      //holds optionIDs and Vals for given EG
        protected DataSet _dsEG_ResultTS_Request = new DataSet();             //hold which TS to retrieve
        //protected DataSet _dsEG_SecondaryTS_Request;             // TS opeationsto perform //SP 15-Feb-2017 Removing necessity for 3 different datasets, can filter based on RetrieveCode
        //protected DataSet _dsEG_TS_AUX_Request;             //auxiliary TS for different uses //SP 15-Feb-2017 Removing necessity for 3 different datasets, can filter based on RetrieveCode
        //protected DataSet _dsEG_TS_Combined_Request = new DataSet();             //auxiliary TS for different uses  WHY 2? //SP 15-Feb-2017 Removing necessity for 3 different datasets, can filter based on RetrieveCode
        protected DataSet _dsEG_ExternalDataSources = new DataSet();                /// use the list- but this is stored for easy write to xml

        protected DataSet _dsEG_XMODEL_LINKS;                         //XMODEL linkages
        protected DataSet _dsEG_ResultSummary_Request;          //holds summary results
        protected DataSet _dsEG_Event_Request;                  //event definition based on TS results
        protected DataSet _dsEG_Event_RequestSecondary; 
        protected DataSet _dsSCEN_EventDetails;
        protected DataSet _dsSCEN_EventDetails_Empty= new DataSet(); //SP 15-Jun-2016 - to avoid frequent calls to database    : MET- think this should not be needed?
        protected DataSet _dsSCEN_SecondaryEventDetails;
        protected string _sSQL_InsertEventDetailVals = "NOTHING";
        protected DataSet _dsSCEN_ResultSummary;
        protected DataSet _dsSCEN_ResultSummary_Empty = new DataSet(); //SP 15-Jun-2016 - to avoid frequent calls to database
        private string _sSQL_InsertResultSummary = "NOTHING";
        protected DataSet _dsEG_Function_Request;
        protected DataSet _dsEG_SupportingFileSpec = new DataSet();             // info for writing to external files not associated with DV. TODO 20-Dec-2016 - make into a list of ExternalData 
        //  rm in favor of _scenDeleteSpec  protected bool _bClearScenarioDS_AtBeginning = true;    // default per SP recent update; to support RT made this optional to happen at end
        protected string _sOutputDir = "OUT";                   // add this to work dir to define location for writing output
        protected DeleteScenDetails _scenDeleteSpec = DeleteScenDetails.BeforeRun;
        protected string _sSimlinkLiteDir;                      // location of simlink_lite files for an eg  (similar to db conn string)

        DataSet _dsEG_PerformanceResultXREF;                    // link results records to performance (TODO: hold all in here and use linq to filter dynamically

        // not use; value placed on EG results request because 1:1.......  will see how we like this.        protected DataSet _dsSCEN_ResultSummary;
        protected DataSet _dsEG_Performance_Request;            //performance summary (including objective)

        private bool _dsEG_DecisionVariables_Loaded = false;
        private DataSet _dsSCEN_ModVals;
        private DataSet _dsSCEN_ModVals_Empty = new DataSet(); //SP 15-Jun-2016 - to avoid frequent calls to database
        private string _sSQL_InsertModVals = "NOTHING";
        private DataSet _dsSCEN_PerformanceDetails;
        private DataSet _dsSCEN_PerformanceDetails_Empty= new DataSet(); //SP 15-Jun-2016 - to avoid frequent calls to database
        private string _sSQL_InsertPerformanceVals = "NOTHING";
        private DataSet _dsSCEN_ScenarioDetails; //SP 10-Jun-2016
        private DataSet _dsSCEN_ScenarioDetails_Empty; //SP 15-Jun-2016 - to avoid frequent calls to database
        //private DataSet _dsInsertScenario; //SP 10-Jun-2016 seems to be unused?
        private string _sSQL_InsertScenarioVals = "NOTHING";                    //this is needed to send back with the ds to load da
        private string _sSQL_ScenarioVals = "NOTHING";
        private string _sSQL_UpdateScenarioVals = "NOTHING";
        public double[][,] _dResultTS_Vals;                     //jagged array of 2D arrays   order: 1) all result TS  2) secondary TS
        public string[] _sTS_GroupID;                           // stores the groupID of the _dResultTS_Vals    //same order as above
        protected double[,][,] _dMEV_Vals;                    //2d jagged array (!)- 0: reference TS  1: modified TS
        public string[] _sMEV_GroupID;
        protected TimeSeries.TimeSeriesDetail _tsdResultTS_SIM;       //default TS that defines start date and TS interval
        protected TimeSeries.TimeSeriesDetail _tsdSimDetails;
        protected simlink _slXMODEL = null;                     //linked simlink for XMODEL
        protected bool _bIsLinkedModel = false;                 //set to true if this is in 'linked' model...
        public Dictionary<int, int> _dictResultTS_Indices;       //store ts indices of the jagged double array- for storage / retrieval  met 11/8/16 to support realtime functionality

        public string _sTS_FileNameRoot = CommonUtilities._sDATA_UNDEFINED;        // set on init in derived class.

        //SP 24-May-2017 Default value TODO Support readin in from Config file to modify this - currently only in parent class, derived classes use their own but should be migrated to this
        protected AssociatedFileSupport _nAssociatedFileSupport = AssociatedFileSupport.FilesWithSameFileName; 

        #endregion
        #region OPTIMIZAION
        public bool _bIsOptimization;                       //whether simlink is running an opt algorithm
        public OptimizationWrapper.OptWrap _optWrapper;
        public double _dOpt_TempObjective;              //temp val - real data held on optimization object
        public int _nPerformanceID_Objective = -1;      //set to PerfID of objective (can only be one (exceept multi-objecive?)

        #endregion

        #region SpecialCaseVarTypes

        protected const int _nEPANET_FieldDict_CONTROLS = 360;
        protected const int _nEPANET_FieldDict_RULES = 359;

        #endregion

        #region NetworkNavigation
        protected Dictionary<int, simlinkTableHelper> _dictSL_TableNavigation = new Dictionary<int, simlinkTableHelper>();
        protected Dictionary<int, string> _dictConstants = new Dictionary<int, string>();

        #endregion

        public simlinkHash _cloneHash = new simlinkHash();  // used to facilitate cloning

        #region POSSIBLE_THINGS_TO_GET_RID_OF
        private bool _bdictBaselineValSummation_Loaded = false;
		#endregion
		#endregion

		public string _sConfigFileLocation = "undefined";				//4/9/19, dumb workaround to be able to load the config independently in the opt lib


		public bool _blnIsValidConnection = true;
        public bool IsValidConnection
        {
            get;
            set;
        }
        #region INIT/CLOSE

        /// <summary>
        ///  public function to set private var (defaults to true and doesn't need to be called in that case due to declaration)
        /// </summary>
        /// <param name="bBegin"></param>
        public void SetScenarioDS_DeleteAtBeginning(DeleteScenDetails spec){
            _scenDeleteSpec = spec;
        }

        public virtual void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = ".", int nLogLevel = 1)
        {
            //sConnRMG = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\DEBUG_SIMLINK\SimLink2.0_NewHaven_LOCAL.mdb";
            _bIsSimCondor = bSimCondor;

            if (_dbContext == null)
            {
                _dbContext = new DBContext();
                IsValidConnection = _dbContext.Init(sConnRMG, DBContext.GetDBContextType(nDB_Type));
            }
            _bdictBaselineValSummation_Loaded = false;

            //SP 03-Oct-2016 initiatlization for scenario logging - generic for all model types
            _log = new Logging();
            _log._nLogging_ActiveLogLevel = nLogLevel;

            //SP 03-Oct-2016 new logging for model initiatlization
            _logInitModel = new Logging();
            _logInitModel._nLogging_ActiveLogLevel = nLogLevel;
            _logInitModel.Initialize("logModelTypeInit_" + ((SimulationType)_nActiveModelTypeID).ToString(),
                _logInitModel._nLogging_ActiveLogLevel, System.IO.Directory.GetCurrentDirectory());   //begin a logging session to current directory

            try
            {
                if (_dbContext._DBContext_DBTYPE == DB_Type.EXCEL || _dbContext._DBContext_DBTYPE == DB_Type.NONE)                //may be other LITE wrappers in the future.
                {
                    _bIsSimLinkLite = true;
                    Helper_SetSimlinkLiteDir(sConnRMG);     // set / test that simlink_lite dir is valid
                }
                else
                {
                    //TODO SP 25-Jul-2016 should this validation be shifted to a tidier location? - Maybe InitializeEG or a separate validation procedure? 
                    //Not urgent but could be easier to follow - but would need to explicitly be called
                    if (_bPerformVersionValidation)
                    {
                        //SP 11-Apr-2016 check the database is compatible with version of Simlink
                        string sPotentialIssues = Environment.NewLine;
                        SIM_API_LINKS.DBVV.DbValidation _dbValidation = new DbValidation();
                        bool bDBVersionValid = _dbValidation.ValidateForVersion(_dbContext, ref sPotentialIssues);
                        _logInitModel.AddString("", Logging._nLogging_Level_1, false);
                        _logInitModel.AddString("", Logging._nLogging_Level_1, false);
                        _logInitModel.AddString("****************************************************************", Logging._nLogging_Level_1, false);
                        if (!bDBVersionValid)
                        {
                            //SP 22-Apr-2016 writing to console until we determine where to provide a warning to user
                            _logInitModel.AddString("<------      DB Version validation failed       ------>" + sPotentialIssues, 
                                Logging._nLogging_Level_1, false);
                        }
                        else
                        {
                            _logInitModel.AddString(string.Format("<-----           Simlink initialized version {0}       ----->    ", "1.28a"), 
                                Logging._nLogging_Level_1, false); //todo: add to .appconfig
                        }
                        _logInitModel.AddString("****************************************************************", Logging._nLogging_Level_1, false);

                        //SP 11-Apr-2016 check that the simluation type contains the required DB tables
                        string sPotentialIssues_ModelType = Environment.NewLine;
                        bool bDBVersionValid_ModelType = true;

                        _logInitModel.AddString("", Logging._nLogging_Level_1, false);
                        _logInitModel.AddString("", Logging._nLogging_Level_1, false);
                        bDBVersionValid_ModelType = _dbValidation.ValidateForModelType(_dbContext, (SimulationType)_nActiveModelTypeID, ref sPotentialIssues_ModelType);
                        _logInitModel.AddString("****************************************************************", Logging._nLogging_Level_1, false);
                        if (!bDBVersionValid_ModelType)
                        {
                            _logInitModel.AddString(string.Format("<------     DB Version validation failed for required model type {0}    ------->", 
                                (string)((SimulationType)_nActiveModelTypeID).ToString()) + sPotentialIssues_ModelType, Logging._nLogging_Level_1, false);
                        }
                        else
                        {
                            _logInitModel.AddString(string.Format("<-----           Simlink initialized model type {0}        ----->", 
                                (string)((SimulationType)_nActiveModelTypeID).ToString()), Logging._nLogging_Level_1, false); //todo: add to .appconfig
                        }
                        _logInitModel.AddString("****************************************************************", Logging._nLogging_Level_1, false);


                    }
                }

                //SP 14-Mar-2017 Set time series offset 
                SetTimeSeriesDateOffset();

                SetSQL_ForSimLink();          //set the sql strings that pull back an empty dataset

                //retrieve empty Scenario DataSets if there is a DB backend   (*placed in if statement 11/10/16 for no backend testing
                if(LoadSetupData())
                    SetEmptyScenarioDataSets_ForSimlink();
            }
            catch (Exception ex)
            {
                _logInitModel.AddString("Error in model linkage initialization", Logging._nLogging_Level_1);
            }
            finally
            {
                _logInitModel.WriteLogFile();
            }
        }


        /// <summary>
        /// check that a valid base model is setup.
        /// </summary>
        private void HelperBaseModelFileLocation_CheckConfirm()
        {
            bool bCreateDir =false; 
            if (_sActiveModelLocation == null) //SP 24-May-2017 TODO -= this is where the PUSH function seems to fail as the ActiveModelLocation is empty but is not handled correctly here
            {
                bCreateDir = true;
            }
            else
            {
                if(_sActiveModelLocation=="")
                    bCreateDir = true;
                if (!Directory.Exists(Path.GetDirectoryName(_sActiveModelLocation)))
                    bCreateDir = true;
            }
            if (bCreateDir)
            {
                // this is bad: shouldn't not have a base model.
                // todo: consider passing a bool bFail instead of crating on (which is unlikely to work.
                string sDir = @"c:\a\";
                if (!Directory.Exists(sDir))
                    Directory.CreateDirectory(sDir);
                Console.WriteLine("no base model location set"); 
                // no log yet_log.AddString("Valid modl file location did not exist. Fake one creatd at c.a", Logging._nLogging_Level_1, false, true);
            }
            
        }

        //model linkage must be initialized before this call.
        public virtual void InitializeEG(int nEvalID)
        {
            if (LoadSetupData())
                LoadSimLinkVarsForEG(nEvalID);

            //met 11/13/16: one must have a valid base model path
            HelperBaseModelFileLocation_CheckConfirm();             // fix no model location  (11/13/16: consider alternative of throw fail exception
                //after know the evaluation group - set the location of the log files from config or a default locations
                
            string sDefaultLogPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, -1, _nActiveEvalID) + @"\\!LOGS";
            _sLogLocation_Simlink = sDefaultLogPath;
            _logInitEG = new Logging();             //SP 03-Oct-2016 new logging for EG initialization          MET: need to consolidate around 1 log object
            _logInitEG._nLogging_ActiveLogLevel = _log._nLogging_ActiveLogLevel;
            _logInitEG.Initialize("logInitEG_" + nEvalID.ToString(),3, _sLogLocation_Simlink);   //begin a logging session to current directory
            _logInitEG.AddString("Begin reporting that is scenario agnostic -  BETA testing SP 3-Oct-2016", Logging._nLogging_Level_1,false,false);

            if (_nActiveModelTypeID == CommonUtilities._nModelTypeID_Simlink)
            {
                if (LoadSetupData())            // decide if _bIsSimlinkLite is a btter approach?
                {
                    // if eg is of type simlink, then load data usually called by dervied class             
                    string sSQL = "SELECT ResultTS_ID, Result_Label, EvaluationGroup_FK, ResultID_FK, VarResultType_FK, ElementIndex, Element_Label, SQN, RetrieveCode, BeginPeriodNo" //SP 28-Feb-2017 changed IsAux to RetrieveCode, removed IsSecondary
                                    + " FROM tblResultTS"
                                    + " WHERE (((EvaluationGroup_FK)=" + nEvalID + "))";

                    _dsEG_ResultTS_Request = _dbContext.getDataSetfromSQL(sSQL);

                    LoadReference_EG_Datasets();
                }
                else
                {
                    // bojangles- get the dir from the config... stored in the config but that's not good (enough)
                   
                }
            }
            if (_bIsSimLinkLite)
            {
                LoadReference_EG_DatasetsFromXML(_sSimlinkLiteDir);             //either db or xml is supported (partially) right now as backend; not both
                _dictResultTS_Indices = GetResultTS_IndexDict();        // set ts index dict (since results set in preceding step)
            }           
        }

        /// <summary>
        /// Return true if a dataset should be loaded
        /// For access//ss return true
        /// For others, one could anticipate having a more refined way of deteriming whethe it should be loaded
        /// Right now, very simple.
        /// </summary>
        /// <param name="sType"></param>
        /// <returns></returns>
        protected bool LoadSetupData(string sType="UNDEFINED"){
            if (_dbContext._DBContext_DBTYPE == DB_Type.OLEDB || _dbContext._DBContext_DBTYPE == DB_Type.SQL_SERVER)
                return true;
            else
            {
                return false;
            }


        }




        #region optimization
        /// <summary>
        /// return 2d array of min/max val for each possible DV
        /// </summary>
        /// <returns></returns>
        public double[,] GetMinMaxBoundsFromDV(double dInterval = 1)
        {
            int nNumDV = _dsEG_DecisionVariables.Tables[0].Select("PrimaryDV_ID_FK = -1").Count();

            double[,] dReturn = new Double[nNumDV, 3];
            for (int i = 0; i < nNumDV; i++)
            {
                dReturn[i, 0] = Convert.ToDouble(_dsEG_DecisionVariables.Tables[0].Rows[i]["Option_MIN"]);
                dReturn[i, 1] = Convert.ToDouble(_dsEG_DecisionVariables.Tables[0].Rows[i]["Option_MAX"]);
                dReturn[i, 2] = dInterval;
            }
            return dReturn;
        }

        /// <summary>
        /// Create and initialize the optwrapper 
        /// met 11/26/15: think this probably needs to be removed. 
        /// keep for now so we can understand how this vs OptWrapper init works... 
        /// 
        /// </summary>
        /// <param name="OptType"></param>
        /// <param name="dictArgs"></param>
        /// <param name="dBounds"></param>
        /// <param name="sXML_Config"></param>
        public void InitializeOptimization(OptWrap.OptAlgo OptType, Dictionary<string, string> dictArgs, double[,] dBounds = null, string sXML_Config = CommonUtilities._sBAD_DATA)
        {

            //met 11/28/15: commented out as testing whether optimization wrapper class is causing bad image exception... 


            //if (dBounds == null && (OptType != OptWrap.OptAlgo.DecViz_BORG))
            //    dBounds = GetMinMaxBoundsFromDV();

            //switch (OptType)
            //{
            //    case OptWrap.OptAlgo.CH2M_GA:
            //        _optWrapper = new OptimizationWrapper.OptCH2M();
            //        break;
            //    case OptWrap.OptAlgo.CH2M_SFLA:
            //        _optWrapper = new OptimizationWrapper.OptCH2M();
            //        break;
            //    case OptimizationWrapper.OptWrap.OptAlgo.DecViz_BORG:
            //        string sParams = "2,3,1";     //har code
            //        //wrap w = new wrap(true); // //YW comment out just to make it compile 
            //        //_optWrapper = w.GetNewBorg(sParams);
            //        //_optWrapper._delProcessScenario = new Optimization.Optimization.delProcessScenario(ProcessScenario_BORG);

            //        break;
            //}

            ////TODO: use attribute based way of knowing whether to do this (not ad-hoc)
            //if (OptType != OptimizationWrapper.OptWrap.OptAlgo.DecViz_BORG)
            //{
            //    //YW comment out just to make it compile 
            //    //_optWrapper.InitStartingPopulation(dictArgs, dBounds);
            //   // _optWrapper.InitOptimization(dictArgs);
            //}
        }

        /// <summary>
        /// Call the execut routine on the base optimization
        /// </summary>
        public void ExecuteOptimization()
        {
            //bojangles            _optWrapper.Execute();
            return;
        }

        //quick dirty debug

        /// <summary>
        /// the code below was in the previous execute optimiation function.
        /// it seems totally inimical to the idea of having a wrapper, but i am preserving here in case there is some code worth salvaging.
        /// 
        /// raises question: how data/info flow from optwrapper back to simlink WHEN running in parallel in the future?
        /// </summary>

        //    bool bFirstPass = true;
        //    if (_optWrapper._bIsValidOpt)
        //    {
        //        _optWrapper._populatation.Fill();               //create initial population (for those not initialized)
        //        while (!_optWrapper.StoppingCriteriaMet() && !_optWrapper._bAbortOptimization)
        //        {                    //only check
        //            // 1: Any pre-processing  (non-init)
        //            // 2: Execute Model   -excel, SimLink etc.  Will need to wait 
        //            if (_optWrapper._bOptDriveModel && _optWrapper._nLoopNo == 0)
        //            {
        //                //Evaluate entire Population
        //                //         _optWrapper.ExecuteSimulationsAll();ExecuteSimulationsAll
        //                ExecuteSimulationsAll();
        //            }
        //            else
        //            {


        //            }
        //            _optWrapper._nLoopNo++;
        //            //can put in dummy function for now ; in near future,  link this to Simlink

        //            //2.5 track optimization between execute and evolve
        //            _optWrapper.LogOptimizationData();          //capture info for easy comparison of behavior (Set to none for increased speed)
        //            // 3: Evolve The Population                                                                           
        //            _optWrapper.EvolvePopulation();

        //            if (_optWrapper._bDEBUG_SetDNA_Copy)                    // skip if not needed (default)
        //                _optWrapper._populatation.UpdateDNAStrings();       //note this skips the first round which is fine

        //            //4: (optional) Visual ouput
        //            //5: (optional) logging

        //            bFirstPass = false;
        //        }
        //        _optWrapper.WriteLogFile();

        //    }
        //    else
        //    {           //notify the user, etc etc

        //    }

        //}

        /// <summary>
        /// not sure what this was supposed to do
        /// commented out for now  met 10/27/15
        /// </summary>
        //public void ExecuteSimulationsAll()
        //{
        //    int nPopulationSize = _optWrapper._populatation._lstDNA.Count();
        //    for (int i = 0; i < nPopulationSize; i++)
        //    {
        //        double[] dDNA = _optWrapper._populatation._lstDNA[i].GetBitsAsArray();
        //        string sDNA = _optWrapper._populatation._lstDNA[i].GetDNABitsAsString(_sDNA_Delimiter);
        //        int nVal = ProcessScenarioWRAP(sDNA, -1, -1, 100);
        //        //      int nVal = TestFuck(sDNA, 1, 2, 3);
        //        //           int nVal = TestFuck(_nActiveProjID, _nActiveEvalID, _nActiveReferenceEvalID, _sActiveModelLocation, -1, -1,100);    //, sDNA);
        //        _optWrapper._populatation._lstDNA[i]._dObjective = SetObjectiveVal(1);
        //        _optWrapper._nEvaluations++;
        //        //    Console.WriteLine("Objective: " + _populatation._lstDNA[i]._dObjective + ", DNA: " + _populatation._lstDNA[i].GetDNABitsAsString() + ", Evaluation No: " + _nEvaluations + ", Loop No: " + _nLoopNo);
        //    }
        //}


        /// <summary>
        /// must override on derived class
        /// </summary>
        /// <param name="sDNA"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="nScenStartAct"></param>
        /// <param name="nScenEndAct"></param>
        /// <returns></returns>
        public virtual int ProcessScenarioWRAP(string sDNA, int nScenarioID, int nScenStartAct, int nScenEndAct, bool bCreateIntDNA = true)
        {
            return -4;
        }

        /// <summary>
        /// Takes a DNA string that may or may not have float vals to int (depending on opt algo)
        /// Note: Assumes that delimiter has been set to comma or some non-period something
        /// </summary>
        /// <param name="sDNA"></param>
        /// <returns></returns>
        public string ConvertDNAtoInt(string sDNA)
        {
            if (_sDNA_Delimiter == ".")
            {
                _log.AddString("DNA assumed to be of type int;", Logging._nLogging_Level_2, true);
                return sDNA;
            }
            if (sDNA.IndexOf('.') > 0)
            {
                string[] sVals = sDNA.Split(_sDNA_Delimiter[0]);
                string[] sRounded = new string[sVals.Length];
                int nCounter =0;
                foreach (string s in sVals)
                {
                    sRounded[nCounter] = Math.Round(Convert.ToDouble(s), 0).ToString();
                    nCounter++;
                }
                sDNA = string.Join(_sDNA_Delimiter, sRounded);
                return sDNA;
            }
            else
            {
                return sDNA;
            }

        }

        #endregion

        //use when calling from cli...
        public void InitializeHTC_ByPlatform(int nActiveModelType)
        {
            _htc = new CIRRUS_HTC_NS.CIRRUS_HTC();              // update for xml
            /*switch (nActiveModelType)
            {
                case 1:
                    _htc = new CIRRUS_HTC_NS.HTC_SWMM();
                    break;
                case 3:
                    _htc = new CIRRUS_HTC_NS.HTC_EPANET();
                    break;
                case 4:
                    _htc = new CIRRUS_HTC_NS.HTC_MODFLOW();
                    break;
                case 11:
                    _htc = new CIRRUS_HTC_NS.HTC_ExtendSim();
                    break;
                default:
                    _htc = new CIRRUS_HTC_NS.CIRRUS_HTC();      //non specialized
                    break;
            }*/
        }
        #region SPLINT
        private void ProcessSplint()
        {
            foreach (DataRow dr in _dsEG_Splint.Tables[0].Rows)
            {
                ProcessSplintDV(dr);               
              
                /*
                switch (sDataTable)
                {
                    case "tblDV":
                        ProcessSplintDV(dr);
                        break;
                    default:
                        Console.WriteLine("splint not yet developed for table: " + sDataTable);
                        break;
                }
                 */
            }
        }
        private void ProcessSplintDV(DataRow dr)
        {
            int nAction = Convert.ToInt32(dr["ActionCode"].ToString());
            string sDataTable = dr["TableName"].ToString();
            string sPK= dr["KeyColumn"].ToString();          // add P
            string sID = dr["VarType_FK"].ToString();
            string sRecordID = dr["RecordID"].ToString();
            string sVal = dr["val"].ToString();
            string sFieldName = dr["FieldAlias"].ToString();
            bool bProcessSecondary = (Convert.ToInt32(dr["ApplyToDependent"]) == 1);     //code for applying a dv change to secondary as well (eg element lists)
            
            // met 2/1/17: revise code to filter on match
            foreach (DataRow drMatch in _dsEG_DecisionVariables.Tables[0].Select(sPK + " = " + sRecordID)){
                switch (nAction){
                    case 2: 
                        _dsEG_DecisionVariables.Tables[0].Rows.Remove(drMatch);     // remove the dr!
                        break;

                    case 1:    // do an update!
                        drMatch[sFieldName] = sVal;
                        Splint_ProcessRequiredDependents(sID, sRecordID, sVal, -1);         // bojangles- set last arg to -1 since not looping properly.
                        if (bProcessSecondary)
                        {
                            switch (sDataTable){
                                case "tblDV":
                                    string sSecondaryKey = "PrimaryDV_ID_FK";   
                                    foreach (DataRow drSec in _dsEG_DV_Consequent.Tables[0].Select(sPK + " = " + sRecordID)){
                                        _dsEG_DecisionVariables.Tables[0].Rows.Remove(drSec);     // remove the dr!
                                    }
                                    break;

                            }
                        }
                        break;
                    }
                }
            }
            
            
            /*
            switch (nAction)
            {
                case 1:     //update
                    for (int i = 0; i < _dsEG_DecisionVariables.Tables[0].Rows.Count; i++)
                    {
                        if (_dsEG_DecisionVariables.Tables[0].Rows[i][nPK].ToString() == sRecordID)
                        {
                           sw
                            
                            _dsEG_DecisionVariables.Tables[0].Rows[i][sFieldName] = sVal;      //over-write the value
                            Splint_ProcessRequiredDependents(sID, sRecordID, sVal, i);    //in some cases, it may be required to perform an updtae of another field based on new field vals.
                            break;  //leave loop
                        }
                    }
                    //now process secondary DV    (some types of updates should be propagated some should/need not be
                        //there should only be a couple of these
                    if (bProcessSecondary)
                    {
                        string sSecondaryKey = Helper_GetSecondaryKey(dr["TableName"].ToString());
                        for (int j = 0; j < _dsEG_DV_Consequent.Tables[0].Rows.Count; j++)
                        {
                            if (_dsEG_DV_Consequent.Tables[0].Rows[j]["PrimaryDV_ID_FK"].ToString() == sRecordID)
                            {
                                _dsEG_DV_Consequent.Tables[0].Rows[j][sFieldName] = sVal;      //over-write the value
                                //may be multiple ; stay in loop 
                            }
                        }
                    }
                    break;
            }*/
      //  }

        //
        /// <summary>
        /// bush league function to take care of necessary business if splint value causes OTHER info to REQUIRE update
        /// this is distinct from depenent var updates, which need to be requested sparingly
        //
        /// </summary>
        /// <param name="sVarTypeID"></param>
        /// <param name="sRecordID"></param>
        /// <param name="j"></param>
        private void Splint_ProcessRequiredDependents(string sVarTypeID, string sRecordID, string sParam, int j)
        {
            string sSQLFilter = "";

            try
            {
                switch (sVarTypeID)
                {
                    case "12":     //CustomFunction was returned by query on linked table, and so must be updated
                        //SP 9-Jun-2016 Loaded into _dsEG_CustomFunction_Request at the start therefore read from memory
                        //sSQL = "select CustomFunction from tblFunctions where (FunctionID = " + Convert.ToInt32(sParam) + ")";
                        //DataSet ds = _dbContext.getDataSetfromSQL(sSQL);


                        sSQLFilter = "FunctionID = " + Convert.ToInt32(sParam);
                        DataRow[] dsSpecificFunction = _dsEG_Function_Request.Tables[0].Select(sSQLFilter); //gets the required row - there will only be one as filtering by FunctionID

                        _dsEG_DecisionVariables.Tables[0].Rows[j]["CustomFunction"] = dsSpecificFunction[0]["CustomFunction"]; //ds.Tables[0].Rows[0]["CustomFunction"];
                        break;
                    default:
                        // in most cases nothing is required
                        break;

                }
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error in retrieving FunctionID {0} from val field in tblSplint: Exception {1}", sParam, ex.Message), Logging._nLogging_Level_1);
                throw; //SP 5-Aug-2016 throw error so that it is handled by ProcessScenario rather than missing it
            }

        }

        #endregion

        public virtual bool InitializeConn_ByFileCon(string sFile, string sDelim = "=")
        {
            return false;
        }


        /// <summary>
        /// Added to perform any wrapper specific init
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual bool InitByConfig_Model(IConfigSource config)
        {


            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <param name="sKEY"></param>
        /// <param name="_dtSIM_StartSim"></param>
        /// <param name="_tsSim_Duration"></param>
    //    public void ManageHotStart(int nScenarioID, string sKEY, DateTime _dtSIM_StartSim = null, TimeSpan tsSim_Duration=null){
    //        DateTime dt_seek = _dtSIM_StartSim + ts
    //        int nScenarioLast= 

    //}



        /// <summary>
        /// Scenario Label function. Added to support RT.
        /// Does not guarantee unique label.
        /// Needs dictionary for more refined naming
        /// </summary>
        /// <returns></returns>
        protected string GetScenarioLabel(string sLabel, bool bAppendDate=false)
        {
            if (sLabel == "DEFAULT")
            {
                sLabel = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");  //rm .fff
            }
            else
            {
                if(bAppendDate)
                    sLabel += System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");   //rm.fff
            }
            return sLabel;
        }

        /// <summary>
        /// Read the configuration and peform necessary setup
		/// 
		/// # 4/9/18: Add config file location...
        /// </summary>
        /// <param name="config"></param>
        public virtual bool InitializeByConfig(IConfigSource config)
        {
            try
            {
                string sConn = ""; DB_Type dbType; double[] dStartingPop = null; Optimization.Optimization.delProcessScenario del = null;
                bool bValid = HelperGetConn(config, out sConn, out dbType);         //currently does not return valid
                // note: if simlink_lite - dir gets set in model linkage init.
                int nEvalID = Convert.ToInt32(config.Configs["simlink"].GetString("evaluationgroup", "-1"));       // user must know ID for now. any cli user should be ok with that.
                _nActiveEvalID = nEvalID;

                string sDelimiter = config.Configs["simlink"].GetString("delimiter", ".");
                _runType = HelperGetRunType(config.Configs["simlink"].GetString("run_type"));
                int nLogLevel = HelperGetLogLevel(config.Configs["simlink"].GetString("log", Logging._nLogging_Level_3.ToString()));
                bool bIsCondor = false; // todo: remove and use compute_env HelperGetCondor(config.Configs["simlink"].GetString("condor", "N"));       //
                _nActiveModelTypeID = CommonUtilities.GetModelTypeIDFromString(config.Configs["simlink"].GetString("type", "NOT_DEFINED"));
                _sActiveModelLocation = config.Configs["simlink"].GetString("base_model_location", "");     // "" empty val cleaned up later if not set....     met 11/13/16
                _sWriteEGToDSSLocation = config.Configs["simlink"].GetString("WriteEGToDSSLocation", "");

                //if bools don't exist, set to initialized value - true
                _bDBWriteOnFail = config.Configs["simlink"].GetBoolean("db_writeonfail", _bDBWriteOnFail);
                _bClearScenarioDetails = config.Configs["simlink"].GetBoolean("db_clearscenariodetails", _bClearScenarioDetails);
                

                // now perform any wrapper specific initialization
                //        InitByConfig_Model(config);

                InitHPCbyConfig(config.Configs["Environment"]);                //initialize the hpc object (if applicable)

                // init the EG

                //init the opt before initializing the EG - SP 9-Mar-2016
                if (_runType == SimlinkRunType.Optimization)
                    _bIsOptimization = true;

                //SP 22-Jul-2016 - now called after setting _bIsOptimization parameter - needed as an input to this function
                InitializeModelLinkage(sConn, (int)dbType, bIsCondor, sDelimiter, nLogLevel);

                InitializeEG(nEvalID);

                //after know the evaluation group - set the location of the log files from config or a default location
                //_sLogLocation_Simlink default location set in InitializeEG
                _sLogLocation_Simlink = config.Configs["simlink"].GetString("simlink_log_file_location", _sLogLocation_Simlink);

                //init the opt
                if (_runType == SimlinkRunType.Optimization)
                {
                    //_bIsOptimization = true; //SP 9-Mar-2016 Set earlier as required before Initilizing the EG
                    //set the delegate
                    Optimization.Optimization.delProcessScenario slDelegate = ProcessScenario;      //make sure we are getting to the3 
                    OptimizationWrapper.OptWrap.OptAlgo myAlgo = OptimizationWrapper.CommonUtilities.GetOptObject(config.Configs["simlink"].GetString("algo", "CH2M_SFLA"));
                    _optWrapper = OptimizationWrapper.CommonUtilities.GetOptObject(myAlgo);
                    double[,] dLimits = GetMinMaxBoundsFromDV();            // use simlink limits to get the dv vals...  TODO:  add option for if no db bacend...n
																			//			dStartingPop = OptimizationWrapper.OptWrap.GetStartingPopulationFromConfig(config.Configs["simlink"].GetString("startingpopulation", ""));

					//4/9/19: will not init, due to stupid nini bullshit	
	//				_optWrapper.InitializeAlgorithm(myAlgo, config, dLimits, slDelegate, dStartingPop);  //dlimits, del will be overwritten
					// so do a dumb workaround.
					_optWrapper.InitializeAlgorithmFULL(myAlgo, _sConfigFileLocation, slDelegate, dLimits);

				}
                return true;
            }
            catch (Exception ex)
            {
				throw new Exception(string.Format("Error initializing optimization: {0}", ex.Message));
            }
        }
        #region InitByXML Helpers
        /// <summary>
        /// Return a connection string from key db_location
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sConn"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static bool HelperGetConn(IConfigSource config, out string sConn, out DB_Type dbType)
        {
            try
            {
                IConfig myConfig = config.Configs["simlink"];
                sConn = myConfig.GetString("db_location", "DB Source UNDEFINED in XML");
                int nDB_Type = myConfig.GetInt("db_type", -1);
                if(nDB_Type==-1)
                    sConn = simlink.HelperConnGetDBType(sConn, out dbType);         // get db_type IF not included in the config       // made static call met 1/26/17         
                else
                    dbType =(DB_Type)nDB_Type;

                if (dbType == DB_Type.OLEDB)                 // add conn string details if only file path provided.
                {
                    bool bIsPathOnly = sConn.IndexOf("Data Source=") == -1;
                    if (bIsPathOnly)
                    {
                        sConn = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + sConn;
                    }
                }
               
                // now we have the connection string and the type
                // TODO: quick test that conn is valid?
                return true;
            }
            catch (Exception ex)
            {
                // TODO get log message back (handle on receiver  _log.AddString("XML file must contain a valid db_location", Logging._nLogging_Level_1);
                sConn = CommonUtilities._sBAD_DATA;
                dbType = DB_Type.OLEDB;
                return false;
            }
        }

        /// <summary>
        //  ensure that param is numeric
        /// </summary>
        /// <param name="sLogLevel"></param>
        /// <returns></returns>
        private int HelperGetLogLevel(string sLogLevel)
        {
            int nReturn;
            bool bIsInt = Int32.TryParse(sLogLevel, out nReturn);
            if (bIsInt)
                return nReturn;
            else
                return Logging._nLogging_Level_3;           //log it as well?
        }

        /// <summary>
        /// Returns whether SimLink should run over Condor
        /// </summary>
        /// <param name="sCondorYN"></param>
        /// <returns></returns>
        private bool HelperGetCondor(string sCondorYN)
        {
            if (sCondorYN.ToLower() == "n")
                return false;
            else
                return true;
        }
        /// <param name="sRunType"></param>
        /// <returns></returns>
        private SimlinkRunType HelperGetRunType(string sRunType)
        {
            // OptimizationWrapper.OptWrap.OptAlgo optReturn = (OptimizationWrapper.OptWrap.OptAlgo)System.Enum.Parse(typeof(OptimizationWrapper.OptWrap.OptAlgo), sRunType, true);
            try
            {
                SimlinkRunType slReturn = (SimlinkRunType)System.Enum.Parse(typeof(SimlinkRunType), sRunType, true);
                return slReturn;
            }
            catch (Exception ex)
            {
                _log.AddString("No key 'run_type'. Setting to type 'User Defined Runs'", Logging._nLogging_Level_2);
                return SimlinkRunType.UserDefinedRuns;
            }
        }

        /// <summary>
        /// Sets the working dir for simlink lite.
        /// Data files go in here, as well as the scenario tracker.
        /// </summary>
        /// <param name="sConn"></param>
        private void Helper_SetSimlinkLiteDir(string sConn){
            if(Directory.Exists(sConn)){
                _sSimlinkLiteDir = sConn;
            }
            else
            {
                _sSimlinkLiteDir=@"C:\a\";
                if(!Directory.Exists(_sSimlinkLiteDir))
                    Directory.CreateDirectory(_sSimlinkLiteDir);        // create dir if needed
                _log.AddString(@"Simlink lite request without data dir; added to c:\a\", Logging._nLogging_Level_2,false,true ); 
            }

        }



        /// <summary>
        /// Gets the type of connection from the string (so user needn't supply).
        /// If Access, create a proper connection string if needed.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sConn"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private static string HelperConnGetDBType(string sConn, out DB_Type dbType)
        {
            // probably more elegant ways of doing this... brute force
            bool bIsPathOnly = sConn.IndexOf("Data Source=") == -1;          //assume anything with an = is a full connection string
            bool bIsAccess = (sConn.IndexOf(".mdb") > 0) || (sConn.IndexOf(".accdb") > 0);
            bool bIsExcel = sConn.IndexOf(".xlsm") > 0;
            if (bIsAccess)
            {
                dbType = DB_Type.OLEDB;
                if (bIsPathOnly)
                {
                    sConn = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + sConn;
                }
            }
            else if (bIsExcel)
                dbType = DB_Type.EXCEL;
            else
                dbType = DB_Type.SQL_SERVER;

            return sConn;
        }

        #endregion


        /// <summary>
        /// Create dict to store index of ts data
        /// 3/10/17: updated to ensure retrieve coede order is respected...
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, int> GetResultTS_IndexDict()
        {
            Dictionary<int, int> dictReturn = new Dictionary<int,int>();
            int nCounter =0;
            int[] nRetrieveCodes=new int[]{1,2,3,4};
            foreach (int n in nRetrieveCodes){
                //todo: force an 'orderby' to make explicit!!
                foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode=" + n)) //met 3-10-17: update to enforce ts order SP 15-Feb-2017 Changed from  _dsEG_CombinedTS_Request
                { 
                    try
                    {
                        dictReturn.Add(Convert.ToInt32(dr["ResultTS_ID"].ToString()), nCounter);
                    }
                    catch (Exception ex)
                    {
                        _log.AddString(string.Format("Error adding TS result {0}", dr["ResultTS_ID"].ToString()), Logging._nLogging_Level_1, false, true);
                    }
                    nCounter++;
                   }
            }
            return dictReturn;
        }

        /// <summary>
        /// This function imports ts data based on a simple file which identifies CSVs of type Timestamp,Val (and provides some metadata)
        /// in this first round, the code will be based on ONE ts per file. This could be improved.
        /// function requires an initialized simlink object
        /// currently requires user to know the field id.
        ///   this code "follows" SimClimTest1
        ///
        /// </summary>
        /// <param name="sFileSpec"></param>
        public int[] ImportAuxiliaryTimeSeriesByFileSpec(string sFileSpec, bool bIsAux = true, int[] narrResultTS = null, int nStationID = -1)
        {
            StreamReader file = new StreamReader(sFileSpec);
            List<int> lstResultTS = new List<int>();    // return a list of the ids entered (or just -1 if not)
            SetTS_FileName(_nActiveBaselineScenarioID);     // set the hdf5 file to be used for this
            using (file)
            {       // close the file      
                string sBuf = file.ReadLine();  //read the header
                int counter = 0;

                while (!file.EndOfStream)
                {
                    int nResultTS = -1;     //default value if a null array is passed (means perform an insert)
                    sBuf = file.ReadLine();  //read the header
                    string[] sParts = sBuf.Split(',');
                    DateTime dtStart = DateTime.Parse(sParts[3]);
                    // todo: some error handling would be nice here...
                    IntervalType intType = new IntervalType();
                    bool bSuccess = Enum.TryParse(sParts[4], out intType);
                    int nInterval = Convert.ToInt32(sParts[5]);
                    string sFilename = sParts[6];
                    string sLabel = sParts[1];
                    int nVarType_FK = Convert.ToInt32(sParts[2]);
                    string sResultLabel = sParts[0];
                    TimeSeries.TimeSeriesDetail tsDtl = new TimeSeries.TimeSeriesDetail(dtStart, intType, nInterval);
                    List<TimeSeries> lstTS = TimeSeries.tsImportTimeSeries(sFilename, tsDtl);
                    if (narrResultTS != null)
                        nResultTS = narrResultTS[counter];      // grab the right ts id to insert
                    nResultTS = ImportResultsTimeSeries(nResultTS, nVarType_FK, nStationID, lstTS, tsDtl, sResultLabel, sLabel, bIsAux);
                    lstResultTS.Add(nResultTS);     // add the result ts
                    counter++;
                }
            }
            return lstResultTS.ToArray();
        }

        /// <summary>
        /// Import a TS or reference (eg observed vs predicted)
        /// Note: hdf5 open/close around each import... not bad becaues not done freq
        /// 
        /// adapted from simclim functi\on
        /// 
        /// </summary>
        /// <param name="nEvalID"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="nVarType"></param>
        /// <param name="nStationID"></param>
        /// <param name="lstTS"></param>
        /// <param name="sLabel"></param>
        /// <param name="bIsAux"></param>
        public int ImportResultsTimeSeries(int nResultID, int nVarType, int nStationID, List<TimeSeries> lstTS, TimeSeries.TimeSeriesDetail tsDtl, string sResultLabel, string sLabel, bool bIsAux = true)
        {
            // 1: Store a result record
            if (nResultID == -1)      //val of -1 indicates that you wnat to creat the record
                nResultID = InsertTS(_nActiveEvalID, nVarType, nStationID, tsDtl, sResultLabel, sLabel, bIsAux);
            // 2: get name of the TS Container
            string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, nResultID.ToString(), "SKIP", "SKIP");        //_nActiveBaselineScenarioID.ToString());
            // 3: Store the TS data

            _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
            _hdf5.hdfWriteDataSeries(TimeSeries.tsTimeSeriesTo2DArray(lstTS), sGroupID, "1");
            _hdf5.hdfClose();
            return nResultID;
        }

        /// <summary>
        /// Create a cohort object
        ///     QUESTION: should this automaticaslly be called on EG load (happening currently)
        /// 
        /// Update 12/26/16: reference member dataset
        /// </summary>
        public void InitCohort()
        {
            int nCountEGinCohort = _dsEG_Cohort.Tables[0].Rows.Count;
            if (nCountEGinCohort > 1)               // only init if GT 1?
            {
                _cohortSpec = new cohortSpec();
                _cohortSpec._cohortType = (CohortType)Convert.ToInt32(_dsEG_Cohort.Tables[0].Rows[0]["CohortType"].ToString());
                _bIsCohort = true;
                List<int> lstLeadScenarios = new List<int>();
                int nCounter = 0;
                foreach (DataRow drEG in _dsEG_Cohort.Tables[0].Rows)
                {
                    int nEvalID = Convert.ToInt32(drEG["EvaluationID"].ToString());
                    _cohortSpec._dictEG_Order.Add(nEvalID, nCounter);

                    //  todo: consider building this in the process EGS
                    DataSet dsScen = ProcessEG_GetGS_Initialize(nEvalID, new string[] { }, false);  // get all scen
                    int nScenarioCounter = -1;
                    foreach (DataRow drScen in dsScen.Tables[0].Rows)
                    {
                        int nScenarioID = Convert.ToInt32(drScen["ScenarioID"].ToString());
                        bool bIsSummary = Convert.ToBoolean(drEG["IsXModel"].ToString());
                        nScenarioCounter++;
                        if (nScenarioCounter == 0)                              // first time through, add a new dict item and a record to the list of scenarios
                        {
                            _cohortSpec._dictScenXREF.Add(nCounter, new List<int>());
                            lstLeadScenarios.Add(nScenarioID);
                        }
                        _cohortSpec._dictScenXREF[nCounter].Add(nScenarioID);    // add the scenario ID
                        if (bIsSummary)     //you'll need this later.
                            _cohortSpec._dictScenSummary_Order.Add(nScenarioID, nScenarioCounter);
                    }
                    nCounter++;
                }
            }
        }

        public Dictionary<string, string> InitializeConnDict_ByFileCon(string sFile, out bool bValidReturn, string sDelim = "=")
        {
            bool bSuccess = true;                               //todo catch bad conn and return false
            StreamReader file = new StreamReader(sFile);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            using (file)
            {
                while (!file.EndOfStream)
                {
                    string sBuf = file.ReadLine();
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>();
                    bool bValid = CommonUtilities.GetKVP(sBuf, out kvp, sDelim);
                    if (bValid)
                        dict.Add(kvp.Key, kvp.Value);
                }
            }
            //   DB_Type dbtype = DBContext.GetDBContextType(Convert.ToInt32(dict["dbtype"]));
            bValidReturn = false;
            if (dict.ContainsKey("conn") && dict.ContainsKey("dbtype"))
                bValidReturn = true;
            return dict;
        }

        //perform on derived
        public virtual void SetTSDetails()
        {
        }

        // met 12/30: split off from ProcessEG_GetGS_Initialize- this does just the load, and the dataset retrieval is separate.
        //todo: consider removing the nModelTypeID...
        protected void LoadSimLinkVarsForEG(int nEvalID, int nModelTypeID = -1, int nRefScenarioID = -1)
        {
            try
            {
                string sSQL = "SELECT ProjID_FK, EvaluationID, ModelFileLocation, ReferenceEvalID_FK, ScenarioID_Baseline_FK, IntermediateResultCode, IsModFileUserDefined, ModFileKey, SimIDBaseline, CohortID, IsXmodel"
                            + " FROM tblEvaluationGroup WHERE (EvaluationID = " + nEvalID + ")";
                //string sSQL = "SELECT ProjID_FK, EvaluationID, ModelFileLocation, ReferenceEvalID_FK, ScenarioID_Baseline_FK, IntermediateResultCode, IsModFileUserDefined, ModFileKey "
                DataSet dsEvals = _dbContext.getDataSetfromSQL(sSQL);
                if (dsEvals == null)
                {
                    string sMessage = string.Format("Error retrieving evaluation group. Chec db version");
                    throw new Exception(sMessage);
                }
                if (dsEvals.Tables[0].Rows.Count > 0)
                {
                    _nActiveProjID = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["ProjID_FK"].ToString());
                    _nActiveBaselineScenarioID = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["ScenarioID_Baseline_FK"].ToString());      //met 1/8/2013
                    _nActiveEvalID = nEvalID;
                    _sActiveModelLocation = dsEvals.Tables[0].Rows[0]["ModelFileLocation"].ToString();
                    _nSimID = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["SimIDBaseline"].ToString());
                    //met 6/29/16: add ability to use update files prepared by user - connect to Gill app

                    //met i couldnt' get this to wokr as a bit. don't know why, has worked before. //SP 15-Jul-2016 - can't seem to reproduce the problem, works ok with bit on my machine. 
                    //TODO Kept as int for now but should be changed to bit - see script 00.01.05
                    _bScenUpdateFileUserDefined = Convert.ToBoolean(dsEvals.Tables[0].Rows[0]["IsModFileUserDefined"]);

                    // special naming?
                    _sUserFileUpdateKey = dsEvals.Tables[0].Rows[0]["ModFileKey"].ToString();
                    if (File.Exists(_sUserFileUpdateKey))
                    {
                        if (false)      //todo: check if it is the right type of file. 8/7/17 hacked in the a way of handling the SWMM cohort file.
                            _bSpecialSimlinkNaming = true;
                        else
                            _bSpecialSimlinkNaming = false;     //
                                                                // met 11/22/16: other checks to set "Special Case"
                    }

                    //already done      _nActiveModelTypeID = nModelTypeID;
                    _nActiveReferenceEvalID = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["ReferenceEvalID_FK"].ToString());
                    _nActiveReferenceEvalID = GetReferenceEvalID();     // this is now set to refID if exists else current id
                    if (_nActiveReferenceEvalID == _nActiveEvalID)
                        _nActiveReferenceEG_BaseScenarioID = _nActiveBaselineScenarioID;
                    else
                        _nActiveReferenceEG_BaseScenarioID = HELPER_LoadEG_GetRefBaselineID(_nActiveReferenceEvalID);
                    //1/5/14: add code to set variable storage
                    int nCode = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["IntermediateResultCode"].ToString());
                    _IntermediateStorageSpecification.SetIntermediateStorage(nCode);
                }
                else
                {
                    //log the issue; eval not found
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int HELPER_LoadEG_GetRefBaselineID(int nRefEvalID)
        {
            int nReturn = CommonUtilities._nBAD_DATA;
            string sSQL = "SELECT ScenarioID_Baseline_FK"
                        + " FROM tblEvaluationGroup WHERE (EvaluationID = " + nRefEvalID + ")";
            DataSet dsEvals = _dbContext.getDataSetfromSQL(sSQL);

            if (dsEvals.Tables[0].Rows.Count > 0)
            {
                nReturn = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["ScenarioID_Baseline_FK"].ToString());
            }
            return nReturn;
        }

        public DataSet ProcessEG_GetElementLibVal(int nEvalGroupID, int nRefEvalID, int nVarType_FK)
        {
            DataSet ds = new DataSet();
            int nEvalParam = nEvalGroupID;
            if (nRefEvalID != -1) { nEvalParam = nRefEvalID; }

            string sql = "SELECT ElementLibID, ElementLibVal, SubTuple FROM tblElementLibrary"
                        + " WHERE (((EvalID_FK)=" + nEvalParam + ") AND ((VarTypeID_FK)=" + nVarType_FK + "))";

            ds = _dbContext.getDataSetfromSQL(sql);
            return ds;

        }


        /// <summary>
        /// met added to support dynamic generation of names related to CSVs.... 
        /// future:  have access to this without going back to the db each time...
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <returns></returns>
        public DataSet GetScenarioDataset(int nScenarioID)
        {
            string sSQL_GetScenarios = "SELECT ScenarioID, ScenarioLabel, ScenarioDescription, DNA, ScenStart, ScenEnd, AltScenarioID FROM tblScenario WHERE (((ScenarioID) = " + nScenarioID + "))";
            DataSet dsEvals = _dbContext.getDataSetfromSQL(sSQL_GetScenarios);
            return dsEvals;
        }

        public DataSet ProcessEG_GetGS_Initialize(int nEvalID, string[] astrScenarioId2Run, bool bGetNotRun = true)        //, int nRefScenarioID = -1)
        {
            string strScenarioId = string.Join(",", astrScenarioId2Run);
            string sSQL_GetScenarios = "SELECT tblProj.ProjID, tblProj.ProjLabel, tblScenario.ScenarioID, tblScenario.ScenarioLabel, tblEvaluationGroup.EvaluationID, " +
                                            "IIf([tblEvaluationGroup].[ModelFileLocation] = '',[tblProj].[ModelFile_Location]," +
                                            "[tblEvaluationGroup].[ModelFileLocation]) AS ModelFile_Location, " +
                                            "tblScenario.DateEvaluated, tblScenario.HasBeenRun, tblScenario.ScenStart, tblScenario.ScenEnd, tblScenario.SQN, " +
                                            "tblScenario.DNA, AltScenarioID, IIf([tblEvaluationGroup].[ReferenceEvalID_FK]=-1,[tblEvaluationGroup].[EvaluationID]," +
                                            "[tblEvaluationGroup].[ReferenceEvalID_FK]) AS ReferenceEvalID_FK, tblEvaluationGroup.ScenarioID_Baseline_FK " +
                                            "FROM (tblProj INNER JOIN tblEvaluationGroup ON tblProj.ProjID = tblEvaluationGroup.ProjID_FK) " +
                                            "INNER JOIN tblScenario ON tblEvaluationGroup.EvaluationID = tblScenario.EvalGroupID_FK " +
                                            "WHERE (((EvaluationID)=" + nEvalID + ")";

            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server
            /*"SELECT ProjID, ProjLabel, ScenarioID, EvaluationID, ModelFile_Location, EvalPrefix, DateEvaluated, ScenStart, ScenEnd, DNA, ReferenceEvalID_FK, ScenarioID_Baseline_FK"
            + " FROM qrySimLink_ScenarioEvalProj"
            + " WHERE (((EvaluationID)=" + nEvalID + ")";*/
            // and (HasBeenRun=0)";

            if (astrScenarioId2Run.Length > 0)
            {
                sSQL_GetScenarios += " AND ScenarioID IN (" + strScenarioId + ")";
            }
            if (bGetNotRun)
            {
                sSQL_GetScenarios += " and (HasBeenRun=0)";         //typical case; get only those which have not 
            }
            string sql2 = ") ORDER BY sqn, ScenarioID;";                    // met 8/13/16: add sqn to support cohort; otherwise scen id overrides def -1 val
            //   string sWhere = " AND (true)";                                                                    //met 3/8/13 update to allow user to define single run
            //sim2_OPT  if (nSingleScenario > 0) { sWhere = " AND (ScenarioID = " + nSingleScenario + ")"; }                 //run single scenario if scenarioID is pased
            sSQL_GetScenarios = sSQL_GetScenarios + sql2;               // sWhere + sql2;
            DataSet dsEvals = _dbContext.getDataSetfromSQL(sSQL_GetScenarios);

            return dsEvals;
        }

        public virtual void CloseModelLinkage()
        {
            _dbContext.Close();
            if (_bUseCostingModule)
                _cost_wrap.Close();
        }
        public virtual double[,] GetNetworkTS_Data(int nElementID, int nVarType_FK, string sElement = "ISIS2D only", string sFileLocation = "NOTHING")
        {
            return null;
        }

        //moved from swmm5022.cs on 1/20

        public void LoadAndInitDV_TS()
        {
            DataRow[] drTS = _dsEG_DecisionVariables.Tables[0].Select("IsTS = " + _dbContext.GetTrueBitByContext());
            DataRow[] drXREF = _dsEG_XMODEL_LINKS.Tables[0].Select("IsDV_Link = 0");
            int nDV_TS_Count = drTS.Count() + drXREF.Count();
            _dMEV_Vals = new double[nDV_TS_Count, 2][,];
            _sMEV_GroupID = new string[nDV_TS_Count];
            int nCounter = 0;
            foreach (DataRow dr in drTS)
            {
                _dMEV_Vals[nCounter, 0] = GetNetworkTS_Data(Convert.ToInt32(dr["ElementID"]), Convert.ToInt32(dr["VarType_FK"]));
                nCounter++;
            }
            foreach (DataRow dr in drXREF)
            {
                if (_nActiveModelTypeID != CommonUtilities._nModelTypeID_ISIS2D && _nActiveModelTypeID != CommonUtilities._nModelTypeID_ISIS_FAST)
                {
                    _dMEV_Vals[nCounter, 0] = GetNetworkTS_Data(Convert.ToInt32(dr["RefID"]), Convert.ToInt32(dr["RefTypeID"]));
                }
                else
                {
                    _dMEV_Vals[nCounter, 0] = GetNetworkTS_Data(Convert.ToInt32(dr["RefID"]), Convert.ToInt32(dr["RefTypeID"]), dr["RefID_Label"].ToString());
                }

                string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.MEV, dr["RefTypeID"].ToString(), dr["RefID"].ToString(), _nActiveBaselineScenarioID.ToString());
                _sMEV_GroupID[nCounter] = sGroupID;         //track it.
                nCounter++;
            }
        }


        #endregion
        /// <summary>
        /// Class description
        /// </summary>
        public string ClassDescription
        {
            get { return "main simlink"; }
        }


        public void SetTSOutToArrayLike(){
            _hdf5._bOutputArrayFormat = true;
        }

        /// <summary>
        /// function to dynamically generate a string (potentially a path\filename etc) based on simlink vars
        ///                 // this could be built out much further to reference any simlink variable name... see MU parser as example for vals? simple for now.
        ///                 bCreateSecondaryOutput: temporary hack, needed to create a new scenarion name
        /// </summary>
        /// <param name="nSCenarioID"></param>
        /// <returns></returns>
        public string CreateStringFromCode(int nSCenarioID, out string sSecondaryOutput, bool bCreateSecondaryOutput = false)
        {
            sSecondaryOutput = "UNDEFINED";
            try
            {
                string sReturn = _sUserFileUpdateKey;

                DataSet dsScen = GetScenarioDataset(nSCenarioID);
                while (sReturn.IndexOf('[') > 0)
                {
                    string sLeftPart = sReturn.Substring(0, sReturn.IndexOf('['));
                    int nRightIndex = sReturn.IndexOf(']');
                    if (nRightIndex < sReturn.Length)
                        nRightIndex++;
                    string sRightPart = sReturn.Substring(nRightIndex);
                    int nLenKey = nRightIndex - sLeftPart.Length;
                    string sKey = sReturn.Substring(sReturn.IndexOf('[') + 1, nLenKey - 2);
                    string sVal = "";
                    switch (sKey.ToLower())
                    {
                        case "eg":
                            sVal = _nActiveEvalID.ToString();
                            break;
                        case "scen_label":
                            sVal = dsScen.Tables[0].Rows[0]["ScenarioLabel"].ToString();
                            break;
                        case "scen":
                            sVal = nSCenarioID.ToString();
                            break;
                        case "scen_description":
                            sVal = dsScen.Tables[0].Rows[0]["ScenarioDescription"].ToString();
                            break;
                        case "altscenid":
                            sVal = dsScen.Tables[0].Rows[0]["AltScenarioID"].ToString();
                            break;
                        case "dna":
                            sVal = dsScen.Tables[0].Rows[0]["dna"].ToString();
                            break;
                    }
                    sReturn = sLeftPart + sVal + sRightPart;
                }
                if (bCreateSecondaryOutput)
                    sSecondaryOutput = CommonUtilities.RMV_FixFilename(dsScen.Tables[0].Rows[0]["ScenarioDescription"].ToString() + "_" + dsScen.Tables[0].Rows[0]["ScenarioLabel"].ToString() + System.DateTime.Now.ToString());   //todo: this should be performed with similar grace to the csv file location

                return sReturn;
            }
            catch (Exception ex)
            {
                return "UNKNOWN";
                //todo : log the issue
            }
        }

        /// <summary>
        /// Public void
        /// </summary>
        /// <param name="bForceUseSameTSDict"></param>
        public void SynthTimeSeries_Head(int nStartScenario, int nEndScenario, int nEvalID_Simlink, bool bForceUseSameTSDict = false)  //, Dictionary<string, string> dictArgs)
        {
            string sLabel = "SYNTH_" + nStartScenario + "_" + nEndScenario + "_eg_" + _nActiveEvalID.ToString();
            bForceUseSameTSDict = true;     // coming up one short otherwise- need to figure out why the "simlink" var does not initialize the same way as the swmm var  (NOT SURE WHY- need to note)

            // get the ts recrods on a new simlink object.
            simlink simRetrieveRecords = CommonUtilities.GetSimLinkObject(_nActiveModelTypeID);     // get simlink object of the same time
            simRetrieveRecords.InitializeModelLinkage(_dbContext._sConnectionString, (int)_dbContext._DBContext_DBTYPE);        
            simRetrieveRecords.InitializeEG(nEvalID_Simlink);
            simRetrieveRecords._dResultTS_Vals = simRetrieveRecords.SynthTimeSeries(nStartScenario, nEndScenario, false);

            if (bForceUseSameTSDict)
            {
                _dictResultTS_Indices = simRetrieveRecords._dictResultTS_Indices;     // set the dict indices directly  (MET: copied from testing function- not sure what this does exactly
            }
            //    if (bForceUseSameTSDict)

            int nNewScenarioID = InsertScenario(sLabel, simRetrieveRecords._dResultTS_Vals, _sTS_GroupID, true);
            //todo: reference testing function SynthTimeSeries_ToScenario_ToDSS to expand to push to DSS

        }


        protected virtual void InitNavigationDict()
        {

        }
        /// <summary>
        /// Override class
        /// </summary>
        /// <param name="nEvalID"></param>
        /// <param name="nRunsInBatch"></param>
        /// <param name="sSWMM_Path"></param>
        /// <param name="sOutputPath"></param>
        public virtual void CreateBatchFile_ByEval(int nEvalID, int nRunsInBatch = -1, int nCohortID =-1,string sEXE_NAME = "swmm5.exe", string sOutputPath = "DEFAULT")
        {
            
        }

        #region DatasetLoad


        /// <summary>
        /// read  subset of datasets from available xml data
        /// </summary>
        /// <param name="sDir"></param>
        public void LoadReference_EG_DatasetsFromXML(string sDir)
        {
            string[] sDS = new string[] { "supporting_file", "result_ts", "external_data_request" , "model_changes" };                // todo: add full support for simlink tables? init step, doesn't take long...
            string sFilename = Path.Combine(sDir,"slite_REPLACE.xml");
            foreach (string sName in sDS)
            {
                string sFileRead = sFilename.Replace("REPLACE", sName);
                switch (sName)
                {
                    case "supporting_file":                 // ds must be initialized ahead of time....
                        _dsEG_SupportingFileSpec.ReadXml(sFileRead);
                        break;
                    case "result_ts":
                        _dsEG_ResultTS_Request.ReadXml(sFileRead); //SP 15-Feb-2017 Changed from _dsEG_TS_Combined_Request
                        break;
                    case "external_data_request":
                        _dsEG_ExternalDataSources.ReadXml(sFileRead);
                        _lstExternalDataSources = CreateListExternalDataSources();
                        break;

                        // model changes are really scen specific but load em.
                    case "model_changes":        
                    /* bOJANGLES.... FIGURE OUT HOW THIS IS HAPPENING...
                        // 4/13/17: run code without 
                        _dsSCEN_ModVals_Empty.ReadXml(sFileRead);
                        _dsSCEN_ModVals_Empty.Tables[0].Rows[0].Delete();   // remove the one record, just get schema
                     //   _lstExternalDataSources = CreateListExternalDataSources();
                     * 
                     */
                        break;
                }             
            }
        }

        //met 1/3/2014: add optional initialization of timeseries
        // met 11/13/16: removed the ts var definition from this call, and place in initEG, to support no backend workflows
        protected void LoadReference_EG_Datasets()
        {
            string sErr = "Phase1";
            int nEvalID = -1;
            try
            {
                nEvalID = GetReferenceEvalID();
                string sql;
                _dsEG_Cohort = EG_GetCohortDS(_nActiveEvalID, _nActiveProjID);                                     //12/26/16
                InitCohort();                              // load the cohort spec. True?           //12/26/16
                _dsEG_DecisionVariables = DNA_GetDVInfo(nEvalID, _nActiveModelTypeID, out sql, "");
                _dsElementLibrary = EGDS_GetElementLibVal(nEvalID);
                _dsEG_ElementList = EGDS_GetElementListVars(nEvalID);
                _dsEG_OptionVals = EGDS_GetOptionVals(_nActiveProjID);
                _dsEG_DV_Consequent = EGDS_GetDV_Consequent(nEvalID, _nActiveModelTypeID);

                _dsEG_PerformanceResultXREF = EGDS_GetPerformanceResultXREF(nEvalID);
                _dsEG_Performance_Request = EGDS_GetPerformanceRequest(nEvalID);
                SetObjectivePerfID();           //Have ObjectiveID of PerfID handy

                sErr = "TS Setup";

                _dsEG_ResultTS_Request = EGDS_GetResultTS(nEvalID); //SP 15-Feb-2017 Moved in here from derived class routines - all derived classes will have different requests
                                                                    //_dsEG_SecondaryTS_Request = EGDS_GetTS_SecondaryRequest(nEvalID); //SP 15-Feb-2017 Call together and remove individual datasets
                                                                    //_dsEG_TS_AUX_Request = EGDS_GetTS_AuxRequest(nEvalID); //SP 15-Feb-2017 Call together and remove individual datasets
                                                                    // now update these into single TS ds  met 11/8/17
                                                                    // todo: these could all go into single TS extract  (discussion with Sanjay)
                                                                    //_dsEG_TS_Combined_Request = _dsEG_ResultTS_Request.Clone();
                                                                    //_dsEG_TS_Combined_Request.Merge(EGDS_GetTS_SecondaryAndAUXRequest(nEvalID));
                                                                    //SP 15-Feb-2017 - Merge and append onto the end the Secondary and AUX Requests which should be more standard among model types

                sErr = "Tertiary phase";

                if (_nActiveModelTypeID != CommonUtilities._nModelTypeID_Simlink)      //bojangles : was getting an error and so threw in switch; should figure out proper merge
                {
                    DataSet dsSecondaryAux = EGDS_GetTS_SecondaryAndAUXRequest(nEvalID);
                    try
                    {
                        // met 11/15/18: Catch this issue.. failing for new ICM build. Make sure use aware if this didn't happen.
                        _dsEG_ResultTS_Request.Merge(dsSecondaryAux);
                    }
                    catch (Exception ex)
                    {
                        string sMessage = string.Format("Error merging secondary timeseries request with primary. Secondary / aux ts may not be included! This is important! : {0}", ex.Message);
                        Console.WriteLine(sMessage);
                        _log.AddString(sMessage, Logging._nLogging_Level_Debug, true, true);
                    }
                }


                // bojangles-pending: believe Sanjay corrected this on the data pull - SP 15-Feb-2017 YES!
                //_dsEG_TS_Combined_Request.Merge(_dsEG_ResultTS_Request);
                //_dsEG_TS_Combined_Request.Merge(_dsEG_SecondaryTS_Request);
                //_dsEG_TS_Combined_Request.Merge(_dsEG_TS_AUX_Request);
                SetAuxAndSecondaryFieldLabel(); //TODO tidy up

                _dictResultTS_Indices = GetResultTS_IndexDict();            //hash for the index to store a given TS in

                _dsEG_SupportingFileSpec = EGDS_GetSupportingFileSpec(nEvalID); // 11/8/16: added to support rtc

                _lstExternalDataSources = EGDS_GetExternalDataSources(nEvalID);
                _dsEG_Event_Request = EGDS_GetEventRequest(nEvalID);
                _dsEG_Event_RequestSecondary = EGDS_GetSecondaryEventRequest(nEvalID);

                _dsEG_XMODEL_LINKS = XMODEL_LoadDS_LinkedRecords(_nActiveBaselineScenarioID);
                InitializeLinkedSimLink();

                _dsEG_Splint = EGDS_GetSplint(_nActiveEvalID);                        //7/9/14
                _dsEG_Function_Request = EGDS_GetFunctionRequest(_nActiveProjID); //SP 9-Jun-2016

                //scenario level- pull on the EG to avoid pulling for each scenario if not scenario specific.
                //if midpoint start, these must be pulled each scenario 
                /* met 4/29/14: 
                 * this was not pulling anything back; make sure baseline scenario is pulled back
                 _dsSCEN_ModVals = EGDS_GetModElementDS(out _sSQL_InsertModVals, _nActiveBaselineScenarioID);
                 _dsSCEN_ResultSummary = EGDS_GetResultSummaryDetail(out _sSQL_InsertResultSummary, _nActiveBaselineScenarioID);
                 //todo: grab events
                 _dsSCEN_Performance = EGDS_GetPerformanceDetail(out _sSQL_InsertPerformanceVals, _nActiveBaselineScenarioID);

                 */
                if (_dictConstants.Count == 0)                              //met 7/29/16 somehow this was set to != ??
                    LoadConstantsDictionary();                  // only load these if unloaded already
                ProcessSplint();
            }
            catch (Exception ex)
            {
                string sMessage = string.Format("Error loading referenced datasets  for reference eval {0} - {1}: {2}", nEvalID, sErr, ex.Message);
                _log.AddString(sMessage, Logging._nLogging_Level_2);
                throw new Exception(sMessage);
            }
        }

        //SP 9-Jun-2016 Load all functions at the start of the project to avoid constantly making database queries
        private DataSet EGDS_GetFunctionRequest(int nProjID)
        {
            string sSQL = "select FunctionID, Label, Category, CustomFunction from tblFunctions where (ProjID_FK = " + nProjID + ")";
            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
            return ds;
        }

        //used for initialization, and also to clear vals following each scenario
        // met 11/13/16: moved call from LoadReference_EG_Datasets to support simlink_lite
            // must be called directly from derived class for now 
        protected void InitTS_Vars()
        {
            if (_dsEG_ResultTS_Request != null)             //ISIS 2d was tripping this: bettedr solution is to have a helper key say not to call this....
            {
                //SP 15-Feb-2017 Now initialise secondary and Aux in one go
                int nTS_Records = _dsEG_ResultTS_Request.Tables[0].Rows.Count;  //+ _dsEG_SecondaryTS_Request.Tables[0].Rows.Count + _dsEG_TS_AUX_Request.Tables[0].Rows.Count;
                _dResultTS_Vals = new double[nTS_Records][,];
                _sTS_GroupID = new string[nTS_Records];
            }
            else
            {
                if (_dsEG_ResultTS_Request != null)          //met 11/13/16 //SP 15-Feb-2017 Changed from _dsEG_TS_Combined_Request
                {
                    int nTS_Records = _dsEG_ResultTS_Request.Tables[0].Rows.Count; //SP 15-Feb-2017 Changed from _dsEG_TS_Combined_Request
                    _dResultTS_Vals = new double[nTS_Records][,];
                    _sTS_GroupID = new string[nTS_Records];
                }
            }
        }


        /// <summary>
        /// met 12/14/16: Not 100% sure what this is doing or that it is correct.
        ///     this was implemented early on and should probably be revisited.
        /// </summary>
        /// <param name="t"></param>
        public void SetRealTimeOffset(TimeSpan t){
            foreach (ExternalData ex in _lstExternalDataSources)
                ex._tsShiftTimeFromPresent=t;
        }

        private DataSet EGDS_GetSupportingFileSpec(int nEval_ID)
        {
            //SP 13-Apr-2017 Added tblExternalGroup
            //string sSQL = "SELECT ID, EvalID_FK, source_SimlinkDataTypeCode, RecordID_FK, conn_string, source_DataFormat, Description, params, IsInput, destination_ExternalDataCode, DV_ID_FK, GroupID, DestColumn" //SP 1-Mar-2017 renamed SimlinkData_Code, DataType_Code, Filename, Destination_Code, Params columns
            //    + " FROM tblSupportingFileSpec where (EvalID_Fk= " + nEval_ID +")";
            string sSQL = "SELECT SFS.ID, EG.EvalID_FK, SFS.RecordID_FK, EG.conn_string, SFS.source_DataFormat, " //SP 15-Aug-2017 removed SFS.source_SimlinkDataTypeCode,
                + " EG.Description, EG.params, EG.IsInput, EG.ExternalDataCode as destination_ExternalDataCode, SFS.DV_ID_FK, EG.GroupID, EG.IsColIDName,"
                + " SFS.DestColumnName, SFS.DestColumnNo as DestColumnNumber"
                + " FROM tblSupportingFileSpec SFS inner join tblExternalGroup EG ON (SFS.GroupID_FK = EG.GroupID) where (EG.EvalID_Fk= " + nEval_ID +")";

            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
            
            //TODO 21-Dec-2016 Make into a list of ExternalData for easier passing
            try
            {
                _lstExternalDataDestinations = CreateListExternalDataDestinations(ds);
                return ds;
            }
            catch (Exception ex)
            {
                _logInitEG.AddString(string.Format("Error retrieving data from tblSupportingFileSpec. Error: {0}", ex.Message), Logging._nLogging_Level_1);
                return null;
            }
        }

        //SP 21-Dec-2016
        //TODO after creating list, need to migrate this into existing code rather than dataset for tblSupportingFileSpec. Kept legacy Dataset for now. List currently only used by WriteOutputData 
        private List<ExternalData> CreateListExternalDataDestinations(DataSet dsSupportingFileSpec)
        {
            List<ExternalData> lstExternalData = new List<ExternalData>();
            foreach (DataRow dr in dsSupportingFileSpec.Tables[0].Rows)
            {
                // todo: create constructor in base class that handles this?
                int nID = Convert.ToInt32(dr["ID"].ToString());
                int nSource = Convert.ToInt32(dr["destination_ExternalDataCode"].ToString()); //SP 1-Mar-2017 renamed Destination_Code column
                int nFormat = Convert.ToInt32(dr["source_DataFormat"].ToString()); //SP 1-Mar-2017 renamed DataType_Code column
                string sParams = dr["params"].ToString();
                string sConn = dr["conn_string"].ToString(); //SP 1-Mar-2017 renamed Filename column
                //SP 1-Mar-2017 dictRequest is already loaded with "file" when GetExternalData is called obtained from conn_string - additional KVP should be handled by sKwargs
                //KeyValuePair<string,string> kvp = new KeyValuePair<string,string>("file",sConn);        // bojangles: this must be handled by derived class. file is CSV specific
                //List<KeyValuePair<string,string>> lstKVP = new List<KeyValuePair<string,string>>();
                //lstKVP.Add(kvp);
                int nGroupID = Convert.ToInt32(dr["GroupID"].ToString());
                string sDestColumn = dr["DestColumnName"].ToString(); //SP 14-Apr-2017 changed to type string to account for a number or a string from the database
                int nDestColumn = Convert.ToInt32(dr["DestColumnNumber"].ToString());
                bool bIsInput = Convert.ToBoolean(dr["IsInput"].ToString());
                bool bIsColIDName = Convert.ToBoolean(dr["IsColIDName"].ToString());
                ExternalData ex = ExternalData.GetExternalData(nID, nSource, nFormat, sParams, sConn, nGroupID, nDestColumn, sDestColumn, bIsInput, bIsColIDName);
                ex._nTSRecordID = Convert.ToInt32(dr["RecordID_FK"].ToString()); //TODO Find a tidier way to integrate this into dictionary or property of Ex
                ex._nDVID = Convert.ToInt32(dr["DV_ID_FK"].ToString()); //TODO Find a tidier way to integrate this into dictionary or property of Ex
                ex._sDescription = dr["description"].ToString();
                //ex._dictRequest=ExternalData.GetDict(sKwargs,lstKVP); //SP 1-Mar-2017 dictRequest is already loaded with "file" when GetExternalData is called
                if(ex._dictRequest.ContainsKey("header_col"))
                {
                    ex._sHeaderColumnInDict= ex._dictRequest["header_col"];         // if not present, user default value for this
                }

                lstExternalData.Add(ex);
            }
            return lstExternalData;
        }

        /// <summary>
        /// Create a list of external data sources and return it
        /// Added to a subfunction so could be called when loaded from xml
        /// issue: ds and lst are duplicative. choose one.
        /// 
        /// met 12/10/16: add ability to handle diff source code
        /// </summary>
        /// <returns></returns>
        private List<ExternalData> CreateListExternalDataSources()
        {
            List<ExternalData> lstExternalData = new List<ExternalData>();
            foreach (DataRow dr in _dsEG_ExternalDataSources.Tables[0].Rows)
            {
                int nID = Convert.ToInt32(dr["ID"].ToString());
                int nSource = Convert.ToInt32(dr["source_ExternalDataCode"].ToString()); //SP 1-Mar-2017 renamed from source_code
                int nFormat = Convert.ToInt32(dr["destination_SimlinkDataTypeCode"].ToString()); //SP 1-Mar-2017 renamed from return_format_code
                string sParams = dr["params"].ToString();
                string sConn = dr["conn_string"].ToString();
                bool bIsColIDName = Convert.ToBoolean(dr["IsColIDName"].ToString());
                //         bool bIsInput = Convert.ToBoolean(dr["IsInput"].ToString());
                int nGroupID = Convert.ToInt32(dr["GroupID"].ToString()); //SP 13-Dec-2016 Added field for sequence of multi-retrieve
                string sReturnColumn = dr["ReturnColumnName"].ToString(); //SP 13-Dec-2016 Added field for column of multi-retrieve //SP 14-Apr-2017 changed to type string to account for a number or a string from the database
                int nReturnColumn = Convert.ToInt32(dr["ReturnColumnNumber"].ToString());
                ExternalData ex = ExternalData.GetExternalData(nID, nSource, nFormat, sParams, sConn, nGroupID, nReturnColumn, sReturnColumn, false, bIsColIDName);
                lstExternalData.Add(ex);
            }
            return lstExternalData;
        }

        private List<ExternalData> EGDS_GetExternalDataSources(int nEvalID)
        {
            //string sSQL = "SELECT ID, label, source_ExternalDataCode, destination_SimlinkDataTypeCode, EvalID_FK, conn_string, params, GroupID, ReturnColumn" //SP 1-Mar-2017 renamed source_code and return_format_code columns, removed db_type
            //                + " FROM tblExternalDataRequest"
            //                + " WHERE (EvalID_FK=" + nEvalID.ToString() + ")";
            //SP 13-Apr-2017 Added tblExternalGroup
            string sSQL = "SELECT tblExternalDataRequest.ID, tblExternalDataRequest.label, tblExternalGroup.ExternalDataCode as source_ExternalDataCode, tblExternalDataRequest.destination_SimlinkDataTypeCode, "
                + " tblExternalGroup.EvalID_FK, tblExternalGroup.conn_string, tblExternalGroup.params, tblExternalGroup.GroupID, tblExternalGroup.IsColIDName, "
                + " iif(tblExternalGroup.IsColIDName = true, -1, tblExternalDataRequest.ReturnColumnNo) As ReturnColumnNumber, " //SP 14-Apr-2017
                + " iif(tblExternalGroup.IsColIDName = true, tblExternalDataRequest.ReturnColumnName, '') As ReturnColumnName" //SP 14-Apr-2017
                + " FROM tblExternalDataRequest inner join tblExternalGroup ON (tblExternalDataRequest.GroupID_FK = tblExternalGroup.GroupID)"
                + " WHERE (tblExternalGroup.EvalID_FK=" + nEvalID.ToString() + ")";
            _dsEG_ExternalDataSources = _dbContext.getDataSetfromSQL(sSQL);     //store this- might as well!
            try
            {
                List<ExternalData> lstExternalData = CreateListExternalDataSources();
                return lstExternalData;
            }
            catch (Exception ex)
            {
                _logInitEG.AddString(string.Format("Error retrieving data from tblExternalDataRequest. Error: {0}", ex.Message), Logging._nLogging_Level_1);
                return null;
            }
        }


        /// <summary>
        ///         //SP 15-Feb-2017 - Scenario extract external data as part of ProcessScenario routine
        ///         
        /// Updated MET 1/11/19- add to try catch block so we don't disrupt whole workflow if this is not found
        /// Better- do a quick check on whether you actually need to get data or not
        /// </summary>
        protected void ScenarioGetExternalData()
        {
            try
            {

                Dictionary<string, string> dictRequest = new Dictionary<string, string>()
            {
                { "start_date", _tsdSimDetails._dtStartTimestamp.ToString("yyyy-MM-dd HH:mm:ss")},
                { "end_date", _tsdSimDetails._dtEndTimestamp.ToString("yyyy-MM-dd HH:mm:ss")},
            };
                ExtractExternalData(dictRequest, RetrieveCode.Aux);
            }

            catch (Exception ex)
            {
                _logInitEG.AddString(string.Format("Exception retrieving External Data.... NO external data will be retrieved: {0}", ex.Message), Logging._nLogging_Level_1);
            }
        }

        //SP 15-Feb-2017 - Scenario extract external data as part of ProcessScenario routine
        protected void EGGetExternalData()
        {
            //12/4/17: note that the tsd is not set... 

            try
            {
                Dictionary<string, string> dictRequest = new Dictionary<string, string>()
            {
                { "start_date", _tsdResultTS_SIM._dtStartTimestamp.ToString("yyyy-MM-dd HH:mm:ss")},
                { "end_date", _tsdResultTS_SIM._dtEndTimestamp.ToString("yyyy-MM-dd HH:mm:ss")},
            };
                ExtractExternalData(dictRequest, RetrieveCode.AuxEG);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Issue setting external data request timestep; mostlikely timestamp not defined.");                   
            }
        }

        //get the right type of sl object based on ModeTypeID.. 
        public static simlink GetSimLinkObjByModelType(int nActiveModelTypeID)
        {
            simlink slReturn;
            switch (nActiveModelTypeID)
            {
                case 1:
                    slReturn = new SIM_API_LINKS.swmm5022_link();
                    break;
                case 3:
                    slReturn = new SIM_API_LINKS.EPANET_link();
                    break;
                case 4:
                    slReturn = new SIM_API_LINKS.modflow_link();
                    break;
                case 8:
                    //simclim moved to simclim_wrap...      slReturn = new SIM_API_LINKS.simclim_link();
                    slReturn = new SIM_API_LINKS.simlink();
                    break;
                case 9:
                    slReturn = new SIM_API_LINKS.isis_2DLink();
                    break;
                case 10:
                    slReturn = new SIM_API_LINKS.isis_2DLink();
                    break;
                default:
                    slReturn = new SIM_API_LINKS.simlink();
                    Console.WriteLine("no override exists for requested sim");
                    break;
            }
            return slReturn;
        }




        //MUST be called by derived class before processing a sc
        //met 5/20/14: added arg bIsBaselineScenarioLoad- if true, delete from DS
        //--> Todo:  implement on other tables (just result var for now)
        public void LoadScenarioDatasets(int nScenarioID = -1, int nScenStartAct = -1, bool bIsBaselineScenarioLoad = false)
        {                  //, string sTS_Filename = "NOTHING"){

            //SP 10-Jun-2016 Load the ScDS Scenario Data - ScenarioID must be known - QUERY to DB
            _dsSCEN_ScenarioDetails = EGDS_GetScenarioDetailsDS(nScenarioID);

            if (nScenStartAct <= CommonUtilities.nScenLCExist)        //starting from step 1 (e.g. optimization)
            {
                //SP 15-Jun-2016 create empty scenario data sets - does not query database, uses preloaded empty sets
                _dsSCEN_ModVals = _dsSCEN_ModVals_Empty.Copy();
                _dsSCEN_ResultSummary = _dsSCEN_ResultSummary_Empty.Copy();
                _dsSCEN_EventDetails = _dsSCEN_EventDetails_Empty.Copy();
                _dsSCEN_PerformanceDetails = _dsSCEN_PerformanceDetails_Empty.Copy();
            }
            else
            {   //in this case, processing starts at later step- predecessors must already be complete; go grab 
                if (nScenStartAct > CommonUtilities.nScenLCModElementExist)
                {
                    // - QUERY to DB
                    _dsSCEN_ModVals = EGDS_GetModElementDS(nScenarioID);
                }
                else //SP 15-Jun-2016 load an empty dataset as this will be evaluated
                    _dsSCEN_ModVals = _dsSCEN_ModVals_Empty.Copy();

                if (nScenStartAct > CommonUtilities.nScenLCModelResultsRead)
                {
                    // - QUERY to DB
                    _dsSCEN_ResultSummary = EGDS_GetResultSummaryDetail(nScenarioID, bIsBaselineScenarioLoad);
                }
                else //SP 15-Jun-2016 load an empty dataset as this will be evaluated
                    _dsSCEN_ResultSummary = _dsSCEN_ResultSummary_Empty.Copy();

                if (nScenStartAct > CommonUtilities.nScenLModelResultsTS_Read)
                {
                    EGDS_GetTS_Details(nScenarioID);        //, sTS_Filename);    
                }

                if (nScenStartAct > CommonUtilities.nScenResultTS_Operations)
                {
                    //EGDS_GetTS_SecondaryDetails(nScenarioID);                   //, sTS_Filename); //SP 15-FEb-2017 now performed in GetTS_Details
                }

                if (nScenStartAct > CommonUtilities.nScenDefineEvents)
                {
                    // - QUERY to DB
                    _dsSCEN_EventDetails = EGDS_GetEventDetail(nScenarioID);                   //, sTS_Filename); 
                }
                else //SP 15-Jun-2016 load an empty dataset as this will be evaluated
                    _dsSCEN_EventDetails = _dsSCEN_EventDetails_Empty.Copy();

                if (nScenStartAct > CommonUtilities.nScenDefineSecondaryEvents)
                {
                    // - QUERY to DB
     // met 11/9/17 I don't see how this is filtering on secondary events
                    _log.AddString("Secondary events not yet supported", Logging._nLogging_Level_Debug, true, false);
                    //_dsSCEN_SecondaryEventDetails = EGDS_GetEventDetail(nScenarioID);                   // event details same for secondary events
                }

                if (nScenStartAct > CommonUtilities.nScenLCSecondaryProcessing)
                {
                    // - QUERY to DB
                    _dsSCEN_PerformanceDetails = EGDS_GetPerformanceDetail(nScenarioID);
                }
                else //SP 15-Jun-2016 load an empty dataset as this will be evaluated
                    _dsSCEN_PerformanceDetails = _dsSCEN_PerformanceDetails_Empty.Copy();
            }
        }

        public void ScenDS_ClearAfterScenario(int nScenarioID)
        {
            //SP 15-Jun-2016 - TODO I believe these are now redundant as LoadScenarioDataset now repopulates all the _dsSCEN_ datasets
            if (_dsSCEN_ModVals != null)
                _dsSCEN_ModVals.Tables[0].Clear();
            if (_dsSCEN_ResultSummary != null)
                _dsSCEN_ResultSummary.Tables[0].Clear();
            if (_dsSCEN_PerformanceDetails != null)
                _dsSCEN_PerformanceDetails.Tables[0].Clear();
            //SP 10-Jun-2016 - clear scenario details after scenario is completed
            if (_dsSCEN_ScenarioDetails != null)
                _dsSCEN_ScenarioDetails.Tables[0].Clear();

            //SP 104SCEN_EventDetails != null)
            if (_dsSCEN_EventDetails != null)            //met added 8/2/16- discuss with sanjay  (can't delete if null, though shouldn't be null) -SP 24-Oct-2016 maybe this was a typo - should be eventdetails
                _dsSCEN_EventDetails.Tables[0].Clear();

            //todo for event          if (_dsSCEN_ResultSummary != null)
            //     _dsSCEN_ResultSummary.Tables[0].Clear();

            if (_bRemoveScenarioDataInMem && (nScenarioID != _nActiveBaselineScenarioID) && (nScenarioID != _nActiveReferenceEG_BaseScenarioID))      //keep baseline data handy
            {
                _lstSimLinkDetail.RemoveAll(x => x._nScenarioID == nScenarioID);
            }

            if (_dResultTS_Vals != null)
                ResetTS_TimeSeries(); //SP 9-Mar-2016 - Created a new procedure which only clears the values, not the references

            if (_dMEV_Vals != null)
            {
                ResetMEV_TimeSeries();
            }
        }


        //_dMEV_Vals[i, 0] holds the reference TS, and so should not be discarded
        //likewise, _sMEV holds a linke to the reference TS and should not be discarded

        private void ResetMEV_TimeSeries()
        {
            int nTS_Number = _dMEV_Vals.GetLength(0);
            for (int i = 0; i < nTS_Number; i++)
            {
                _dMEV_Vals[i, 1] = null;
            }
        }


        //_sTS_GroupID[i] holds the reference TS, and so should not be discarded
        //SP 15-Feb-2017 Proceed with only one _dsEG_ResultTS_Request dataset
        private void ResetTS_TimeSeries()
        {
            //SP 9-Nov-2016 clear only the primary, secondary and AUX (not AUXEG)
            foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode <>" + ((int)RetrieveCode.AuxEG).ToString()))
            {
                int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())];
                _dResultTS_Vals[nIndex] = null;
            }

            /* Existing code prior to 15-Feb-2017
            int nTS_Number = _dsEG_ResultTS_Request.Tables[0].Rows.Count + _dsEG_SecondaryTS_Request.Tables[0].Rows.Count;
            for (int i = 0; i < nTS_Number; i++)
            {
                _dResultTS_Vals[i] = null;
            }*/
        }

        //in general (definitely in SWMM) this should be overridden in derived class
        // met 3/30/17: add ability to load a general results EG
            // bojangles: add linked function capability
            //met 4/28/17: set bIncludeAux to true... for exporting aux... might not work in all cases.
        public virtual DataSet EGDS_GetResultTS(int nEvalID, bool bIncludeAux = true)
        {
            string sqlFD = "SELECT ResultTS_ID, Result_Label, ElementIndex, -1 as ResultIndex,VarResultType_FK as FeatureType, ResultTS_ID as FieldLabel, VarResultType_FK, Element_Label, RetrieveCode, AuxID_FK," //, 1 as ts_code"  // 11/11/13 - add ts_code for quick filter, SP 15-Feb-2017 should not be able to use RetrieveCode
                           + " SQN, -1 as CustomFunction, -1 as FunctionArgs, RefTS_ID_FK, FunctionID_FK, 0 as UseQuickParse"
                            + " FROM tblResultTS"
                           + " WHERE (((EvaluationGroup_FK)=" + nEvalID + ") and (RetrieveCode <> " + ((int)RetrieveCode.Secondary).ToString() + ")"; //+ " WHERE (((EvaluationGroup_FK)=" + nEvalId + ") and (IsSecondary=0)"; //SP 28-Feb-2017 remove reference to IsSecondary
            if (bIncludeAux)                //
                sqlFD += ")";
            else
                sqlFD += "and (RetrieveCode not in (" + ((int)RetrieveCode.Aux).ToString() + ", " + ((int)RetrieveCode.AuxEG).ToString() + ")))"; //sqlFD += "and (IsAux=0))"; //SP 28-Feb-2017 removed reference to IsAux
            /* met 5/31/14: redone to 1) use virtual/override  2) not depend on query    string sqlFD = "SELECT ResultTS_ID, Result_Label, ResultIndex,ElementIndex, FeatureType"
                            + " FROM qryResultTS001_SWMM_OUT WHERE (((EvaluationGroup_FK)=" + nEvalId + "))";  */
            DataSet dsFD = _dbContext.getDataSetfromSQL(sqlFD);
            return dsFD;
        }

        /// <summary>
        /// override that allows a direct call to read output if needed.
        /// allows user to override the name of the output file...
        /// 
        /// must override on derived class
        /// </summary>
        /// <param name="nEvalId"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="sOutFile"></param>
        /// <returns></returns>
        public virtual long ReadTimeSeriesOutput(int nEvalId, int nScenarioID, string sOutFile = "UNDEFINED",long nStartIndex=0,long nEndIndex=-1,string sDatasetLabel="1", string sHDF_Name="UNDEFINED")
        {
            return -1;
        }


        //first attempt at polymorphism- initialize the linked object.
        // significant limitations: 
        //1. assume just one linkage
        private void InitializeLinkedSimLink()
        {
            if (_dsEG_XMODEL_LINKS.Tables[0].Rows.Count > 0)
            {
                int nLinkedBaselineID = Convert.ToInt32(_dsEG_XMODEL_LINKS.Tables[0].Rows[0]["LinkScenarioID"]); //todo: support more than just one row!!
                int nLinkedModelTypeID = Convert.ToInt32(_dsEG_XMODEL_LINKS.Tables[0].Rows[0]["LinkSimCode"]);
                DataRow drLinked = HELPER_GetLinkedSimLinkInitRow(nLinkedBaselineID);
                string sModelLocation = drLinked["ModelFile_Location"].ToString();
                int nEvalID_LINK = Convert.ToInt32(drLinked["EvaluationID"].ToString());
                int nDB_Type = Convert.ToInt32(_dbContext._DBContext_DBTYPE);

                switch (nLinkedModelTypeID)
                {
                    case CommonUtilities._nModelTypeID_SWMM:
                        _slXMODEL = new swmm5022_link();
                        break;
                    case CommonUtilities._nModelTypeID_SimClim:
                        _slXMODEL = new simlink();          // met 10/27/15- simclim_link moved to other class     simclim_link();
                        break;
                    /*case CommonUtilities._nModelTypeID_ISIS2D:
                        _slXMODEL = new 
                        break;*/

                    default:
                        _slXMODEL = new simlink();
                        break;
                }

                _slXMODEL.InitializeModelLinkage(_dbContext._sConnectionString, nDB_Type, false);
                _slXMODEL.InitializeEG(nEvalID_LINK);
                _slXMODEL._bIsLinkedModel = true;
            }

        }

        //point of this is to get the modelfile_location and eval id from the base scneario id.
        private DataRow HELPER_GetLinkedSimLinkInitRow(int nLinkedBaselineID)
        {
            string sql = "SELECT tblProj.ProjID, tblScenario.ScenarioID, tblEvaluationGroup.EvaluationID, " +
                                            "IIf([tblEvaluationGroup].[ModelFileLocation] = '',[tblProj].[ModelFile_Location]," +
                                            "[tblEvaluationGroup].[ModelFileLocation]) AS ModelFile_Location, " +
                                            "tblScenario.DateEvaluated, tblScenario.HasBeenRun, tblScenario.ScenStart, tblScenario.ScenEnd, " +
                                            "tblScenario.DNA, IIf([tblEvaluationGroup].[ReferenceEvalID_FK]=-1,[tblEvaluationGroup].[EvaluationID]," +
                                            "[tblEvaluationGroup].[ReferenceEvalID_FK]) AS ReferenceEvalID_FK, tblEvaluationGroup.ScenarioID_Baseline_FK " +
                                            "FROM (tblProj INNER JOIN tblEvaluationGroup ON tblProj.ProjID = tblEvaluationGroup.ProjID_FK) " +
                                            "INNER JOIN tblScenario ON tblEvaluationGroup.EvaluationID = tblScenario.EvalGroupID_FK " +
                                            "WHERE ((ScenarioID)=" + nLinkedBaselineID + ") ";


            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server
            /*"SELECT ProjID, ScenarioID, EvaluationID, ModelFile_Location, EvalPrefix,"
                        + " DateEvaluated, ScenStart, ScenEnd, DNA, ReferenceEvalID_FK, ScenarioID_Baseline_FK"
                        + " FROM qrySimLink_ScenarioEvalProj"
                        + " WHERE ((ScenarioID)=" + nLinkedBaselineID + ") ";*/

            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds.Tables[0].Rows[0];
        }

        #region DatasetHELPERS
        #region EG Level

        //6/2014: pull back IsObjective
        public DataSet EGDS_GetPerformanceRequest(int nEvalID)
        {
            string sql = " SELECT tblPerformance.PerformanceID, tblPerformance.Performance_Label, tblPerformance.PF_Type,"
                            + " tblPerformance.LinkTableCode, tblPerformance.PF_FunctionType, tblPerformance.EvalID_FK, tblPerformance.CategoryID_FK, tblPerformance.SQN,"
                            + " tblPerformance.IsObjective, tblPerformance.ResultFunctionKey, tblFunctions.CustomFunction, tblFunctions.UseQuickParse, tblPerformance.FunctionArgs, DV_ID_FK, OptionID_FK, "
                            + "Threshold, tblPerformance.IsOver_Threshold, ApplyThreshold"
                              + " FROM tblPerformance LEFT JOIN tblFunctions ON tblPerformance.FunctionID_FK = tblFunctions.FunctionID"
                              + " WHERE (((tblPerformance.[EvalID_FK])=" + nEvalID + "))"
                              + " ORDER BY tblPerformance.SQN, tblPerformance.PerformanceID";
            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        //
        private void SetObjectivePerfID()
        {

            if (true)           //   _bIsOptimization)
            {
                var ObjectivePerformance = from Performance in _dsEG_Performance_Request.Tables[0].AsEnumerable()                //which performance to characterize
                                           where Performance.Field<Boolean>("IsObjective") == true
                                           select new
                                           {
                                               PerformanceID = Performance.Field<int>("PerformanceID"),
                                           };
                int nCount = ObjectivePerformance.Distinct().Count();
                if (nCount == 0)
                {
                    if(_runType==SimlinkRunType.Optimization)       // met 1/26/17
                        Console.WriteLine("Running in optimization mode, however no objective defined");
                    //todo: log
                }
                else if (nCount > 1)
                {
                    Console.WriteLine("More than one objective defined");
                    _nPerformanceID_Objective = ObjectivePerformance.Last().PerformanceID;  // use last as default? placed there for now (met 10/28/16)
                    //todo: should be able to support this, but need a primary objectvie too
                    //todo: log
                }
                else
                {
                    _nPerformanceID_Objective = ObjectivePerformance.First().PerformanceID;
                }
            }
        }

        public DataSet EGDS_GetSplint(int nEvalID)
        {
            string sql = "SELECT TableName, KeyColumn, VarType_FK, RecordID, FieldName, FieldAlias, val, tblSplint.EvalID_FK, ActionCode,ApplyToDependent"
                + " FROM (tblSplint INNER JOIN tlkpSimLinkFieldDictionary ON tblSplint.VarType_FK = tlkpSimLinkFieldDictionary.FieldID) INNER JOIN tlkpSimLinkTableDictionary ON tlkpSimLinkFieldDictionary.TableID_FK = tlkpSimLinkTableDictionary.ID"
                + " WHERE (EvalID_FK = " + nEvalID + ")";

            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        //5/28/14: revised to pull back all linked results (must be filtered when linking to tblPer, tblResults etc etc
        public DataSet EGDS_GetPerformanceResultXREF(int nEvalID)
        {
            string sql = "SELECT tblPerformance_ResultXREF.PerformanceID_FK, tblPerformance_ResultXREF.LinkTableID_FK, tblPerformance_ResultXREF.ScalingFactor, tblPerformance.EvalID_FK, tblPerformance_ResultXREF.LinkType,"
                        + " IIf([tblPerformance_ResultXREF].[ApplyThreshold] = -2, [tblPerformance].[ComponentApplyThreshold], [tblPerformance_ResultXREF].[ApplyThreshold]) AS ApplyThreshold,"
                        + " IIf([tblPerformance_ResultXREF].[Threshold] = -1.234, [tblPerformance].[ComponentThreshold], [tblPerformance_ResultXREF].[Threshold]) AS Threshold,"
                        + " IIf([tblPerformance_ResultXREF].[IsOver_Threshold] = -2, [tblPerformance].[ComponentIsOver_Threshold],[tblPerformance_ResultXREF].[IsOver_Threshold]) AS IsOver_Threshold"
                        + " FROM tblPerformance_ResultXREF INNER JOIN tblPerformance ON tblPerformance_ResultXREF.PerformanceID_FK = tblPerformance.PerformanceID"
                        + " WHERE (((tblPerformance.EvalID_FK)= " + nEvalID + "))";          // AND ((tblPerformance_ResultXREF.LinkType)=1))";
            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        //MET 8/15/14: CHANGED TO left join to support some standard TS operations.
        //todo: consider additional changes to enable multiple steps without saving each one?
        //SP 15-Feb-2017 Not retrieved together with AUX in EGDS_GetTS_SecondaryAndAUXRequest
        /*private DataSet EGDS_GetTS_SecondaryRequest(int nEvalId)
        {
            string sqlResultTS_Process = "SELECT tblResultTS.ResultTS_ID, tblResultTS.Result_Label,VarResultType_FK, tblResultTS.SQN, tblFunctions.CustomFunction, tblResultTS.FunctionArgs, RefTS_ID_FK, FunctionID_FK, tblFunctions.UseQuickParse, tblResultTS.BeginPeriodNo, RetrieveCode, AuxID_FK, 2 as ts_code"
                                        + " FROM tblResultTS LEFT JOIN tblFunctions ON tblResultTS.FunctionID_FK = tblFunctions.FunctionID"
                                        + " WHERE (((tblResultTS.EvaluationGroup_FK)=" + nEvalId + ") AND ((tblResultTS.[IsSecondary])=" + _dbContext.GetTrueBitByContext() + "))"
                                        + " ORDER BY tblResultTS.SQN,tblResultTS.ResultTS_ID;";

            DataSet ds = _dbContext.getDataSetfromSQL(sqlResultTS_Process);
            return ds;
        }*/

        // updated to add the RetrieveCode to support realtime
        //SP 15-Feb-2017 Not retrieved together with Secondary in EGDS_GetTS_SecondaryAndAUXRequest
        /*private DataSet EGDS_GetTS_AuxRequest(int nEvalId)
        {
            string sqlResultTS_Process = "SELECT tblResultTS.ResultTS_ID, tblResultTS.Result_Label, Element_Label,VarResultType_FK, tblResultTS.SQN, tblResultTS.FunctionArgs, RefTS_ID_FK, FunctionID_FK, tblResultTS.BeginPeriodNo, RetrieveCode, AuxID_FK, 3 as ts_code"
                                        + " FROM tblResultTS"
                                        + " WHERE (((tblResultTS.EvaluationGroup_FK)=" + nEvalId + ") AND ((tblResultTS.[IsAux])=" + _dbContext.GetTrueBitByContext() + "))"
                                        + " ORDER BY tblResultTS.SQN,tblResultTS.ResultTS_ID;";

            DataSet ds = _dbContext.getDataSetfromSQL(sqlResultTS_Process);
            return ds;
        }*/

        //SP 28-Feb-2017 Dropped AUX and IsSecondary from tblResultTS - can be handled with RetrieveCode
        private DataSet EGDS_GetTS_SecondaryAndAUXRequest(int nEvalId)
        {
            string sqlResultTS_Process = "SELECT tblResultTS.ResultTS_ID, tblResultTS.Result_Label, Element_Label,VarResultType_FK, tblResultTS.SQN, tblFunctions.CustomFunction, tblResultTS.FunctionArgs, RefTS_ID_FK, FunctionID_FK, tblFunctions.UseQuickParse, tblResultTS.BeginPeriodNo, RetrieveCode, AuxID_FK"
                                        + " FROM tblResultTS LEFT JOIN tblFunctions ON tblResultTS.FunctionID_FK = tblFunctions.FunctionID"
                                        + " WHERE (tblResultTS.EvaluationGroup_FK=" + nEvalId + ") AND (tblResultTS.[RetrieveCode] <> " + ((int)RetrieveCode.Primary).ToString() + ")"
                                        + " ORDER BY tblResultTS.[RetrieveCode], tblResultTS.SQN,tblResultTS.ResultTS_ID;"; //SP 15-Feb-2017 ordering by RetrieveCode will maintain the memory array ordering

            DataSet ds = _dbContext.getDataSetfromSQL(sqlResultTS_Process);
            return ds;
        }


        // secondary and aux do not have strong link to vartype_fk from derived class.
        // this is a cheap function to set this
        // usrful for DSS export.
        private void SetAuxAndSecondaryFieldLabel(){
            Dictionary<int, string> dictKeys = new Dictionary<int, string>();
            try{
                foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Rows) //SP 15-Feb-2017 Now retrieving all AUX and Secondary TS from single dataset
                {               // noteL: this should loop through ResultTS first- where the keys are. could be made more explicit
                    int nVarType_FK = Convert.ToInt32(dr["VarResultType_FK"].ToString());             
                    if(dr["FieldLabel"].ToString().Length>0){
                        if(!dictKeys.ContainsKey(nVarType_FK)){
                            dictKeys.Add(nVarType_FK,dr["FieldLabel"].ToString());
                        }
                    }
                    else{
                        // empty FieldLabel
                        if (dictKeys.ContainsKey(nVarType_FK))
                        {
                            dr["FieldLabel"] = dictKeys[nVarType_FK];
                        }
                    }

                }
            }
            catch (Exception ex){
                // log it.

                }
        }

        /// <summary>
        /// Get the event request
        /// 
        /// UPDATE HISTORY
        /// 
        // met 11/12/16: this can be simplified significantly i think
        // SP 1-Dec-2016 had to explicity specify tblResultTS.IsSecondary now that there is a field IsSecondary in tblResultTS_EventSummary
        // SP 1-Dec-2016 TimeSeries changes to reference tblResultTS_EventSummary.ResultTS_or_Event_ID_FK prob due to secondary events therefore added retrieved field to SQL request
        /// 
        /// 12/17/16:  added additional request to SECONDARY event (not this one)
        ///     TODO: Consider handling new fields in the primary as well   //IsHardOrigin,OriginOffset, IsHardTerminus, TerminusOffset, SearchOriginForward, SearchTerminusForward, IsPointVal
        ///     NOTE: sql seems too loquacious... can be cleaned in the join?
        ///     2/2/17: added 
        /// </summary>
        /// <param name="nEvalId"></param>
        /// <returns></returns>
        private DataSet EGDS_GetEventRequest(int nEvalId)
        {
            string sql_select = "SELECT tblResultTS.EvaluationGroup_FK, tblResultTS.ResultTS_ID, tblResultTS_EventSummary.EventSummaryID, tblResultTS_EventSummary.ResultTS_or_Event_ID_FK," + 
                                "tblResultTS.Result_Label, tblResultTS.Element_Label, tblResultTS.Element_Label AS ElementLabel_NameInTS, " +
                                "tblResultTS.ElementIndex, tblEvaluationGroup.TS_Interval, tblResultTS.BeginPeriodNo, " +
                                "tblResultTS_EventSummary.EventFunctionID, tblResultTS_EventSummary.Threshold_Inst, " +
                                "tblResultTS_EventSummary.IsOver_Threshold_Inst, tblResultTS_EventSummary.Threshold_Cumulative, " +
                                "tblResultTS_EventSummary.IsOver_Threshold_Cumulative, tblResultTS_EventSummary.InterEvent_Threshold, " +
                //"tblResultTS_EventSummary.EventLevelCode, tblResultTS_EventSummary.CalcValueInExcessOfThreshold " +
                                "tblResultTS_EventSummary.CalcValueInExcessOfThreshold, AssignEventNoCode, RefEventID, RefPrimaryEvent_StartOffset, RefPrimaryEvent_EndOffset " +
                                "FROM tblResultTS_EventSummary INNER JOIN (tblResultTS INNER JOIN tblEvaluationGroup ON " +
                                    "tblResultTS.EvaluationGroup_FK = tblEvaluationGroup.EvaluationID) ON " +
                                    "(tblResultTS_EventSummary.ResultTS_or_Event_ID_FK = tblResultTS.ResultTS_ID) AND " +
                                    "(tblResultTS_EventSummary.EvaluationGroupID_FK = tblResultTS.EvaluationGroup_FK) " +
                //"WHERE (((tblResultTS_EventSummary.EventLevelCode)=1)) " + //SP 4-Mar-2016 TODO Unsure why EventLevelCode is in here - to assess
                                    "WHERE ((EvaluationGroup_FK=" + nEvalId + ") and tblResultTS_EventSummary.IsSecondary = 0)"; //SP 22-Jul-2016 Changed 'AND' to 'WHERE' can't see how this would have been working after 14-Jul-16 commits //SP 1-Mar-2017 - changed 'IsSecondary = false' to 'IsSecondary = 0' for compatibility with SQL Server


            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server
            /*"SELECT EvaluationGroup_FK, ResultTS_ID, EventSummaryID, EventFunctionID, Element_Label, ElementIndex, TS_Interval, InterEvent_Threshold,"
                            + " Threshold_Inst, IsOver_Threshold_Inst, Threshold_Cumulative, IsOver_Threshold_Cumulative, BeginPeriodNo, CalcValueInExcessOfThreshold"
                            + " FROM qryEvent001_DefineRequest "
                            + "WHERE (EvaluationGroup_FK=" + nEvalId + ")";*/

            DataSet ds = _dbContext.getDataSetfromSQL(sql_select);
            return ds;
        }


        /// <summary>
        /// Get seconary update request IsHardOrigin,OriginOffset, IsHardTerminus, TerminusOffset, SearchOriginForward, SearchTerminusForward, IsPointVal
        /// 
        /// Update History
        /// MET 12/17/16:  added more detailed fields: 
        /// 1/9/17: add sqn
        /// </summary>
        /// <param name="nEvalId"></param>
        /// <returns></returns>
        private DataSet EGDS_GetSecondaryEventRequest(int nEvalId)
        {
            string sql_select = "SELECT EventSummaryID, ResultTS_or_Event_ID_FK, EventFunctionID, Threshold_Inst, IsOver_Threshold_Inst, Threshold_Cumulative, IsOver_Threshold_Cumulative, InterEvent_Threshold, CalcValueInExcessOfThreshold, "
                                + " IsSecondary, RefEventID, RefFromBeginning, IsHardOrigin,OriginOffset, IsHardTerminus, TerminusOffset, SearchOriginForward, SearchTerminusForward, IsPointVal, sqn, AssignEventNoCode"
                                + " FROM tblResultTS_EventSummary"
                                + " WHERE ((EvaluationGroupID_FK=" + nEvalId + ") and (IsSecondary <> 0))"; //SP 1-Mar-2017 Changed 'IsSecondary = True' to 'IsSecondary <> 0' for compatibility with SQL Server
            DataSet ds = _dbContext.getDataSetfromSQL(sql_select);
            return ds;
        }

        public DataSet EGDS_GetElementLibVal(int nEvalID)
        {
            string sql = "SELECT ElementLibID, VarTypeID_FK, ElementLibVal, SubTuple FROM tblElementLibrary"
                         + " WHERE (((EvalID_FK)=" + nEvalID + "))";
            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        public DataSet EGDS_GetElementListVars(int nEvalID)
        {

            string sqlElementList = "SELECT tblElementLists.ElementListLabel, tblDV.DVD_ID, tblDV.VarType_FK, tblElementListDetails.ElementID_FK, " +
                                        "tblElementListDetails.VarLabel, tblDV.EvaluationGroup_FK, tblElementLists.ElementListID, '-1.234' as ElementVal " +
                                        "FROM tblDV INNER JOIN (tblElementListDetails INNER JOIN tblElementLists ON tblElementListDetails.ElementListID_FK = " +
                                        "tblElementLists.ElementListID) ON tblDV.ElementID_FK = tblElementLists.ElementListID" +
                                        " where ((EvaluationGroup_FK= " + nEvalID + "));";

            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server
            /*"SELECT ElementListID, ElementListLabel, DVD_ID, VarType_FK, ElementID_FK, VarLabel, '-1.234' as ElementVal"
                + " FROM qryElementList001_ListVars where ((EvaluationGroup_FK= " + nEvalID + "));";*/

            DataSet ds = _dbContext.getDataSetfromSQL(sqlElementList);
            return ds;
        }

        //returns referenceEvalID if not -1, else EvalID
        public int GetReferenceEvalID()
        {
            if (_nActiveReferenceEvalID != -1)
                return _nActiveReferenceEvalID;
            else
                return _nActiveEvalID;
        }

        private DataSet EGDS_GetOptionVals(int nProjID)
        {

            string sSQL_OptionList = "SELECT tblOptionLists.ProjID_FK, tblOptionDetails.OptionID_FK, tblOptionLists.OptionLabel, tblOptionLists.OptionID, " +
                //"tblOptionDetails.OptionNo, tblOptionDetails.Val, tblOptionDetails.valLabelinSCEN, tblOptionDetails.VarID_FK " + //SP 13-Jul-2016 removed valLabelinSCEN and VarID_FK from tblOptionDetails
                "tblOptionDetails.OptionNo, tblOptionDetails.Val " +
                "FROM tblOptionLists INNER JOIN tblOptionDetails ON tblOptionLists.OptionID = tblOptionDetails.OptionID_FK " +
                "where (ProjID_FK = " + nProjID + ")";

            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server 
            /*"SELECT val,OptionID, OptionNo from qryOption001_Link"
                + " where (ProjID_FK = " + nProjID + ")";*/

            DataSet ds = _dbContext.getDataSetfromSQL(sSQL_OptionList);
            return ds;
        }
        #endregion

        #region ScenMGMT


        /// <summary>
        /// added to make easier to del scen data 
        /// 
        /// </summary>
        /// <param name="bFromConsoleUI"></param>
        /// <param name="nEvalID"></param>
        /// <param name="nScenStartAct"></param>
        /// <param name="nEndStage"></param>
        /// <param name="sConcatScenarios"></param>
        /// <param name="bUseSpecialOps"></param>
        public void DeleteScenariosWrapper(bool bFromConsoleUI, int nEvalID, int nScenStartAct, int nEndStage, string sConcatScenarios, bool bUseSpecialOps = false, bool bDeleteAllinEG = false)
        {
            string sFeedback = "Error deleting scenario data.";
            string sMessage = "You are about to delete scenario data for steps " + nScenStartAct + " to " + nEndStage + " for {0} scenarios. Hit any key to proceed..";
            int nScenarios = -1;
            try
            {
                bool bValid = TestScenarioRequest(nEvalID, sConcatScenarios, bUseSpecialOps, ref sFeedback, ref nScenarios, bDeleteAllinEG);
                if (bValid)
                {
                    bool bContinue = true;
                    if (bFromConsoleUI)
                    {                                             // add little hook for feedback if running from console
                        _log.AddString(string.Format(sMessage, nScenarios), Logging._nLogging_Level_1);
                        string sKey = Console.ReadKey().ToString().ToLower();
                        if (false)                //todo: figure this out : sKey!="y")
                        {
                            bContinue = false;
                            _log.AddString("Exiting without deleting data", Logging._nLogging_Level_1);
                        }
                    }
                    if (bContinue)
                    {
                        DeleteScenarioData(nEvalID, nScenStartAct, nEndStage, sConcatScenarios, bUseSpecialOps);
                    }
                }
                else
                {
                    Console.WriteLine(sFeedback);
                }
            }
            catch (Exception ex)
            {
                _log.AddString("Error!! " + sFeedback + " " + ex.Message, Logging._nLogging_Level_1);
            }
        }


        /// <summary>
        /// Helper function to test whether the delete args are passed properly.
        /// </summary>
        /// <param name="nEvalID"></param>
        /// <param name="sConcatScenarios"></param>
        /// <param name="bUseSpecialOps"></param>
        /// <param name="sFeedback"></param>
        /// <param name="nScenarios"></param>
        /// <param name="bDeleteAll"></param>
        /// <returns></returns>
        private bool TestScenarioRequest(int nEvalID, string sConcatScenarios, bool bUseSpecialOps, ref string sFeedback, ref int nScenarios, bool bDeleteAll)
        {
            bool bValid = true;
            if (bUseSpecialOps)
            {
                string sSQL = "SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))";
                DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
                nScenarios = ds.Tables[0].Rows.Count;
            }
            if (sConcatScenarios != "")
            {
                try
                {
                    string sSQL = "SELECT tblScenario.ScenarioID FROM  tblScenario WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + ") and (ScenarioID in (" + sConcatScenarios + ")))";
                    DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
                    nScenarios = ds.Tables[0].Rows.Count;
                }
                catch (Exception ex)
                {
                    sFeedback += "; Error with scenario list. Provide comma separated scenario list.";
                    bValid = false;
                }
            }
            else
            {
                if (bDeleteAll)
                {
                    string sSQL = "SELECT tblScenario.ScenarioID FROM  tblScenario WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))";
                    DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
                    nScenarios = ds.Tables[0].Rows.Count;
                }
                else
                {
                    sFeedback += "; Must pass flag -delete_all if specialops flag or scen list not passed... Aborting";
                    bValid = false;
                }
            }
            return bValid;
        }

        /// <summary>
        /// function to delete all scenario, eg, and proj data from a db. 
        /// if called from console, prompts user.
        /// </summary>
        /// <param name="bFromConsoleUI"></param>
        /// <param name="nProjID"></param>
        public void DeleteProj(bool bFromConsoleUI, int nProjID)
        {
            string sFeedback = "Error deleting project data.";
            string sMessage = "You are about to delete project data for proj: {0}. Hit 'n' to abort";
            string sSQL_EG = "";
            bool bContinue = true;
            try
            {
                if (bFromConsoleUI)
                {                                             // add little hook for feedback if running from console
                    _log.AddString(string.Format(sMessage, nProjID), Logging._nLogging_Level_1);
                    string sKey = Console.ReadKey().ToString().ToLower();
                    if (sKey.ToLower() == "n")                //todo: figure this out : sKey!="y")
                    {
                        bContinue = false;
                        _log.AddString("Exiting without deleting data", Logging._nLogging_Level_1);
                    }
                }
                if (bContinue)
                {
                    DataSet dsEG = CommonUtilities.GetDSbySQL(_dbContext, "eg", nProjID, out sSQL_EG);
                    foreach (DataRow drEG in dsEG.Tables[0].Rows)
                    {
                        int nEvalID = Convert.ToInt32(drEG["EvaluationID"]);
                        DeleteScenarioData(nEvalID, -1, 100, "", false);          // delete all scenario data associated with EG   
                        DeleteEGFormulation(nEvalID);
                    }
                    DeleteProjFormulation(nProjID);         // del proj
                }
            }
            catch (Exception ex)
            {
                _log.AddString("Error!! " + sFeedback + " " + ex.Message, Logging._nLogging_Level_1);
            }
        }



        /// <summary>
        /// Remove all formulation for an EG
        /// note: deletes scenario just as if they were formulation . this is fine for any forseeable use i can see... conceptually they are diff
        /// </summary>
        /// <param name="nProjID"></param>
        /// <param name="nEGID"></param>
        public void DeleteEGFormulation(int nEGID)
        {
            //note: order matters here as all EG done first then all PROJ tables
            string[] sTables = new string[] { "tblResultVar", "tblResultTS", "tblResultTS_EventSummary", "tblPerformance", "tblDV", "tblSCenario" };
            string[] sKeys = new string[] { "EvaluationGroup_FK", "EvaluationGroup_FK", "EvaluationGroupID_FK", "EvalID_FK", "EvaluationGroup_FK", "EvalGroupID_FK" };
            int nCount = sTables.Length;
            for (int i = 0; i < nCount; i++)
            {
                if (sTables[i] == "tblPerformance")
                {
                    //if (sTables[i] == "tblOptionLists")
                    //also delete the ResultsXREF table
                    DeleteDetailsTable("tblPerformance_ResultXREF", nEGID);
                }
                DeleteRowsBySimpleFilter(sTables[i], sKeys[i], nEGID);
            }
        }

        /// <summary>
        /// Delete data linked to specific proj
        /// </summary>
        /// <param name="nProjID"></param>
        public void DeleteProjFormulation(int nProjID)
        {
            //note: order matters here as all EG done first then all PROJ tables
            string[] sTables = new string[] { "tblOptionLists", "tblElementLists", "tblFunctions", "tblConstants", "tblEvaluationGroup", "tblProj" };
            string[] sKeys = new string[] { "ProjID_FK", "ProjID_FK", "ProjID_FK", "ProjID_FK", "ProjID_FK", "ProjID" };
            int nCount = sTables.Length;
            for (int i = 0; i < nCount; i++)
            {
                if (sTables[i] == "tblOptionLists" || sTables[i] == "tblElementLists")
                {
                    DeleteDetailsTable(sTables[i], nProjID);
                }
                DeleteRowsBySimpleFilter(sTables[i], sKeys[i], nProjID);
            }
        }


        /// <summary>
        /// Deletes linked tables that must be accessed via a subquery.
        /// </summary>
        /// <param name="sTable"></param>
        /// <param name="nProjID"></param>
        public void DeleteDetailsTable(string sTable, int nProjID)
        {
            string sSQL = "";
            switch (sTable)
            {
                case "tblOptionLists":
                    sSQL = "delete from tblOptionDetails WHERE (OptionID_FK in (select OptionID from tblOptionLists WHERE (ProjID_FK=" + nProjID + ")))";
                    break;
                case "tblElementLists":
                    sSQL = "delete from tblElementListDetails WHERE (ElementListID_FK in (select ElementListID from tblElementLists WHERE (ProjID_FK=" + nProjID + ")))";
                    break;
                case "tblPerformance_ResultXREF":
                    sSQL = "delete from tblPerformance_ResultXREF WHERE PerformanceID_FK in (select PerformanceID from tblPerformance WHERE (EvalID_FK=" + nProjID + "))";    //nproj is actualy EG here
                    break;
            }
            _dbContext.RunDeleteSQL(sSQL);
        }

        public void DeleteRowsBySimpleFilter(string sTable, string sKey, int nFilterVal)
        {
            string sSQL = "delete from " + sTable + " where (" + sKey + " = " + nFilterVal + ")";
            _dbContext.RunDeleteSQL(sSQL);
        }

        //todo: implement special ops to limit what scenarios info is removed
        public void DeleteScenarioData(int nEvalID, int nScenStartAct, int nEndStage, string sConcatScenarios, bool bUseSpecialOps = false)
        {

            if ((nScenStartAct <= CommonUtilities.nScenLCModElementExist) && (nEndStage >= CommonUtilities.nScenLCModElementExist))
            {
                DeleteDataFromDB("tblModElementVals", nEvalID, sConcatScenarios, bUseSpecialOps);
            }
            if ((nScenStartAct <= CommonUtilities.nScenLCModelResultsRead) && (nEndStage >= CommonUtilities.nScenLCModelResultsRead))
            {
                DeleteDataFromDB("tblResultVar_Details", nEvalID, sConcatScenarios, bUseSpecialOps);
            }

            if ((nScenStartAct <= CommonUtilities.nScenLModelResultsTS_Read) && (nEndStage >= CommonUtilities.nScenLModelResultsTS_Read))
            {

            }

            if ((nScenStartAct <= CommonUtilities.nScenResultTS_Operations) && (nEndStage >= CommonUtilities.nScenResultTS_Operations))
            {
                //todo: clean out HDF5 store?
            }

            if ((nScenStartAct <= CommonUtilities.nScenDefineEvents) && (nEndStage >= CommonUtilities.nScenDefineEvents))
            {
               // DeleteDataFromDB("tblResultTS_EventSummary_Detail", nEvalID, sConcatScenarios, bUseSpecialOps);
                DeleteEventData(_nActiveReferenceEvalID,sConcatScenarios, false);
            }

            if ((nScenStartAct <= CommonUtilities.nScenDefineSecondaryEvents) && (nEndStage >= CommonUtilities.nScenDefineSecondaryEvents))
            {
                // DeleteDataFromDB("tblResultTS_EventSummary_Detail", nEvalID, sConcatScenarios, bUseSpecialOps);
                DeleteEventData(_nActiveReferenceEvalID,sConcatScenarios, true);
            }

            if ((nScenStartAct <= CommonUtilities.nScenLCSecondaryProcessing) && (nEndStage >= CommonUtilities.nScenLCSecondaryProcessing))
            {
                DeleteDataFromDB("tblPerformance_Detail", nEvalID, sConcatScenarios, bUseSpecialOps);
            }

        }

        //SP 5-Aug-2016 Consolidated DB deletion into one routine - all very similar except for the table being deleted from
        public void DeleteDataFromDB(string sTableName, int nEvalID, string sConcatScenarios = "", bool bUseSpecialOps = false)
        {
            string sSQL = "delete from " + sTableName + " where ";
            string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            if (sConcatScenarios != "")
            {
                sSubquery = "(ScenarioID_FK in (" + sConcatScenarios + "))";
            }

            if (bUseSpecialOps)
            {        //join to this table
                sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
            }
            sSQL = sSQL + sSubquery;
            _dbContext.RunDeleteSQL(sSQL);
        }


        public void DeleteEventData(int nEvalID, string sScenarioID,  bool bIsSecondary =false)
        {
            string sSQL = "delete from tblResultTS_EventSummary_Detail where (EventSummary_ID in ";
            string sSQL2 = "(select EventSummaryID from tblResultTS_EventSummary where ((tblResultTS_EventSummary.EvaluationGroupID_FK=" + nEvalID + ") and (tblResultTS_EventSummary.IsSecondary = " + _dbContext.GetTrueBitByContext(bIsSecondary) + "))))";
            sSQL+=sSQL2;
            if(sScenarioID.IndexOf(',')>0)                              // Delete can be passed concat string of scenario; handle this (not expected)
                sSQL += " and (scenarioid_fk in [" + sScenarioID + "])";
            else
                sSQL += " and (scenarioid_fk =" +sScenarioID + ")";
            _dbContext.RunDeleteSQL(sSQL);
        }


        /*public void DeleteModelData(int nEvalID, string sConcatScenarios = "", bool bUseSpecialOps = false)
        {
            string sSQL = "delete from tblModElementVals where ";
            string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            if (sConcatScenarios != "")
            {
                sSubquery = "(ScenarioID_FK in (" + sConcatScenarios + "))";
            }

            if (bUseSpecialOps)
            {        //join to this table
                sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
            }
            sSQL = sSQL + sSubquery;
            _dbContext.RunDeleteSQL(sSQL);
        }


        public void DeleteResultSummaryData(int nEvalID, string sConcatScenarios = "", bool bUseSpecialOps = false) //SP 5-Aug-2016 Added option for scenario specific rather than entire evaluation group
        {
            string sSQL = "delete from tblResultVar_Details where ";                //(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";

            //SP 5-Aug-2016 Added option for scenario specific rather than entire evaluation group
            if (sConcatScenarios != "")
            {
                sSubquery = "(ScenarioID_FK in (" + sConcatScenarios + "))";
            }

            if (bUseSpecialOps)
            {        //join to this table
                sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
            }
            sSQL = sSQL + sSubquery;


            _dbContext.RunDeleteSQL(sSQL);
        }


        public void DeleteEventData(int nEvalID, string sConcatScenarios = "", bool bUseSpecialOps = false) //SP 5-Aug-2016 Added option for scenario specific rather than entire evaluation group
        {
            string sSQL = "delete from tblResultTS_EventSummary_Detail where ";                //(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";

            //SP 5-Aug-2016 Added option for scenario specific rather than entire evaluation group
            if (sConcatScenarios != "")
            {
                sSubquery = "(ScenarioID_FK in (" + sConcatScenarios + "))";
            }

            if (bUseSpecialOps)
            {        //join to this table
                sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
            }
            sSQL = sSQL + sSubquery;


            _dbContext.RunDeleteSQL(sSQL);
        }


        public void DeletePerformanceData(int nEvalID, string sConcatScenarios = "", bool bUseSpecialOps = false) //SP 5-Aug-2016 Added option for scenario specific rather than entire evaluation group
        {
            string sSQL = "delete from tblPerformance_Detail where ";       //(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";

            //SP 5-Aug-2016 Added option for scenario specific rather than entire evaluation group
            if (sConcatScenarios != "")
            {
                sSubquery = "(ScenarioID_FK in (" + sConcatScenarios + "))";
            }

            if (bUseSpecialOps)
            {        //join to this table
                sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
            }
            sSQL = sSQL + sSubquery;

            _dbContext.RunDeleteSQL(sSQL);
        }*/


        #endregion



        #region ScenarioLevel

        //set the sql strings to an empty record (for insert)
        private void SetSQL_ForSimLink()
        {
            string sEmptyWhere = " WHERE (0 > 1)";

            _sSQL_InsertModVals = "select Model_ID, DV_ID_FK,TableFieldKey_FK,ScenarioID_FK,val,element_note,ElementName,ElementID,DV_Option, IsInsert"
                                + " from tblModElementVals WHERE " + "(Model_ID<0)";

            _sSQL_InsertResultSummary = "select ResultDetail_ID, Result_ID_FK, ScenarioID_FK, val, ElementName, ElementID"
                            + " from tblResultVar_Details WHERE " + "(ResultDetail_ID<0)";

            //_sSQL_InsertPerformanceVals = "select PF_DetailID, PerformanceID_FK, DVID_FK, VAL,ScenarioID_FK,IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK" //SP 13-Jul-2016 DVID_FK, IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK removed from DB Schema
            _sSQL_InsertPerformanceVals = "select PF_DetailID, PerformanceID_FK, VAL,ScenarioID_FK"
                            + " from tblPerformance_Detail WHERE " + "(PF_DetailID<0)";

            _sSQL_InsertEventDetailVals = "SELECT TS_EventSummaryDetailID,  ScenarioID_FK, EventSummary_ID, EventDuration, EventBeginPeriod, MaxVal, TotalVal, SubEventThresholdPeriods, EventNo " +
                "FROM tblResultTS_EventSummary_Detail where (0>1)";

            //SP 10-Jun-2016 - Insert Scenario SQL
            //met 7/17/16: if scens already set, do as minor update as possible (this was throwing error with some null data or something)
            if (_bIsOptimization)
                _sSQL_ScenarioVals = "select ScenarioID, EvalGroupID_FK, ScenarioLabel, ScenarioDescription, DateCreated, DNA, HasBeenRun, ScenLC_LastStage, DateEvaluated " +
               "from tblScenario";
            else
                _sSQL_ScenarioVals = "select ScenarioID, HasBeenRun, ScenLC_LastStage, DateEvaluated from tblScenario";

            _sSQL_InsertScenarioVals = _sSQL_ScenarioVals + sEmptyWhere;
        }

        //SP 15-Jun-2015 populate the empty datasets once to be used for every scenario
        private void SetEmptyScenarioDataSets_ForSimlink()
        {
            _dsSCEN_ModVals_Empty = EGDS_GetModElementDS();
            _dsSCEN_ResultSummary_Empty = EGDS_GetResultSummaryDetail();
            _dsSCEN_EventDetails_Empty = EGDS_GetEventDetail();
            _dsSCEN_PerformanceDetails_Empty = EGDS_GetPerformanceDetail();
        }

        /// <summary>
        /// Set the active scenario ; set ts filenam and load datasets
        /// </summary>
        /// <param name="nNewScenarioID"></param>
        /// <param name="nScenStart"></param>
        /// <param name="sTargetPath"></param>
        public void SetActiveScenarioID(int nNewScenarioID, int nScenStart = 100, string sTargetPath = "NOTHING")
        {
            _nActiveScenarioID = nNewScenarioID;
            SetTS_FileName(_nActiveScenarioID, sTargetPath, true);
            LoadScenarioDatasets(_nActiveScenarioID, nScenStart);
        }


        public enum EvalActivationCode{
            Manual,
            Cohort,
            DataRow
        }

        /// <summary>
        /// Utility function to facilitate consistent updating of simlink EG related variables when switching between EG in code
        /// TODO: Compare to UI code and make sure everything is happening correctly
        /// Motivation: Primarily driven by cohorts, when we are swithching between EG, want to make that happen accurately. may be other uses
            // it is possible this should only be used in a cohort context and the other uses can be removed.
        /// </summary>
        /// <param name="nEvalID"></param>
        /// <param name="evalActivationCode"></param>
        /// <param name="sModelLocation"></param>
        /// <param name="dr"></param>
        public void SetActiveEvalID(int nEvalID, EvalActivationCode evalActivationCode = EvalActivationCode.Manual, int nCohortRow = 0, string sModelLocation = "Nothing", DataRow dr = null)
        {
            _nActiveEvalID = nEvalID;
            switch (evalActivationCode){
                case EvalActivationCode.Manual:
                    if (sModelLocation != "Nothing")
                        _sActiveModelLocation=sModelLocation;
                    //   todo: update the base scenario and ref id  somehow?
                    break;
                case EvalActivationCode.Cohort:    // requires a valid _dsEG_Cohort
                    _sActiveModelLocation = _dsEG_Cohort.Tables[0].Rows[1]["ModelFileLocation"].ToString();
                    // nCohortRow: the row in the cohort DT to be used for this
                    _nActiveBaselineScenarioID = Convert.ToInt32(_dsEG_Cohort.Tables[0].Rows[nCohortRow]["ScenarioID_Baseline_FK"].ToString());
                    _nActiveReferenceEvalID = Convert.ToInt32(_dsEG_Cohort.Tables[0].Rows[nCohortRow]["ReferenceEvalID_FK"].ToString());
                    break;
                case EvalActivationCode.DataRow:
                    // not supported.
                    break;
            }

        }

        private DataSet EGDS_GetModElementDS(int nScenarioID = -1)
        {
            string sWhere = "(Model_ID<0)";
            if (nScenarioID != -1)
                sWhere = "(ScenarioID_FK = " + nScenarioID.ToString() + ")";

            string sqlInsertModVals = "select Model_ID, DV_ID_FK,TableFieldKey_FK,ScenarioID_FK,val,element_note,ElementName,ElementID,DV_Option, IsInsert"
                                + " from tblModElementVals WHERE " + sWhere;


            DataSet dsInsert = _dbContext.getDataSetfromSQL(sqlInsertModVals);
            //SP 5-Aug-2016 More explicit warning message if SQL returns a null dataset when there should be at least an empty dataset retrieved
            if (dsInsert == null)
                throw new Exception("SQL returned null from DB when trying to retrieve information from tblModElementVals");

            if (true)       //add to SL list. easier to dup the code and customize the field names ( though not ideal)
            {
                foreach (DataRow dr in dsInsert.Tables[0].Rows)
                {
                    int nRecordID = Convert.ToInt32(dr["DV_ID_FK"].ToString());
                    string sVal = dr["val"].ToString();
                    int nVarType_FK = Convert.ToInt32(dr["TableFieldKey_FK"].ToString());
                    int nElementID = Convert.ToInt32(dr["ElementID"].ToString());
                    string sElementName = dr["ElementName"].ToString();
                    bool bIsInsert = Convert.ToBoolean(dr["IsInsert"].ToString());              // _dbContext.GetLogicalByInt(Convert.ToInt32(dr["IsInsert"].ToString()));
                    int nDV_Option = Convert.ToInt32(dr["DV_Option"].ToString());

                    try
                    {
                        _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.MEV, sVal, nRecordID, nVarType_FK, nScenarioID, nElementID, sElementName, bIsInsert, nDV_Option));
                    }
                    catch (Exception ex)
                    {
                        int n = 1;
                        _log.AddString(string.Format("Error adding tblModElementVals to Simlink memory for DV_ID_FK = {0}", nRecordID), Logging._nLogging_Level_1);
                        throw; //SP 5-Aug-2016 if trying to access values from tblModElementVals but can't, need do pass this message back to processscenario routine rather than carrying on 
                    }

                }
            }

            return dsInsert;
        }


        //SP 10-Jun-2016 Get empty scenario details dataset or the specific scenario dataset for the ScenID
        private DataSet EGDS_GetScenarioDetailsDS(int nScenarioID = -1)
        {
            string sWhere = " where ScenarioID = -1";
            if (nScenarioID != -1)
                sWhere = " where ScenarioID = " + nScenarioID;
            _sSQL_UpdateScenarioVals = _sSQL_ScenarioVals + sWhere;

            DataSet dsInsert = _dbContext.getDataSetfromSQL(_sSQL_UpdateScenarioVals);
            return dsInsert;
        }


        private DataSet EGDS_GetResultSummaryDetail(int nScenarioID = -1, bool bIsBaselineScenarioLoad = false)
        {
            string sWhere = "(ResultDetail_ID<0)";
            if (nScenarioID != -1)
                sWhere = "(ScenarioID_FK = " + nScenarioID.ToString() + ")";
            string sqlInsertResultSummaryDetail = "select ResultDetail_ID, Result_ID_FK, ScenarioID_FK, val, ElementName, ElementID,VarResultType_FK"
                            + " FROM tblResultVar INNER JOIN tblResultVar_Details ON tblResultVar.Result_ID = tblResultVar_Details.Result_ID_FK"
                            + " WHERE " + sWhere;
            DataSet dsInsert = _dbContext.getDataSetfromSQL(sqlInsertResultSummaryDetail);

            if (true)       //add to SL list. easier to dup the code and customize the field names ( though not ideal)
            {
                foreach (DataRow dr in dsInsert.Tables[0].Rows)
                {
                    int nRecordID = Convert.ToInt32(dr["Result_ID_FK"].ToString());
                    string sVal = dr["val"].ToString();
                    int nVarType_FK = Convert.ToInt32(dr["VarResultType_FK"].ToString());
                    int nElementID = Convert.ToInt32(dr["ElementID"].ToString());
                    string sElementName = dr["ElementName"].ToString();

                    _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.ResultSummary, sVal, nRecordID, nVarType_FK, nScenarioID, nElementID, sElementName));
                }
            }
            //if true, we are just grabbing ref base vars, but do not want the result DS to include.
            if (bIsBaselineScenarioLoad)            //=  && (nScenarioID != _nActiveBaselineScenarioID))     //
                dsInsert.Clear();

            return dsInsert;
        }

        //met 9/1/14
        // this was overlooked
        //event does not have a dsScen; everything in the list.
        private DataSet EGDS_GetEventDetail(int nScenarioID = -1, bool bIsBaselineScenarioLoad = false)
        {
            string sWhere = "(TS_EventSummaryDetailID<0)";
            if (nScenarioID != -1)
                sWhere = "(ScenarioID_FK = " + nScenarioID.ToString() + ")";
            string sql = "SELECT TS_EventSummaryDetailID,  ScenarioID_FK, EventSummary_ID, EventDuration, EventBeginPeriod, MaxVal, TotalVal, SubEventThresholdPeriods, EventNo"
                        + " FROM tblResultTS_EventSummary_Detail WHERE " + sWhere;

            DataSet dsInsert = _dbContext.getDataSetfromSQL(sql);

            if (true)       //add to SL list. easier to dup the code and customize the field names ( though not ideal)
            {
                AddEventsToListDetail(dsInsert, nScenarioID);
            }
            //if true, we are just grabbing ref base vars, but do not want the result DS to include.
            if (bIsBaselineScenarioLoad)            //=  && (nScenarioID != _nActiveBaselineScenarioID))     //
                dsInsert.Clear();

            return dsInsert;
        }



        //loads saved TS 
        //in fuutre, consider only need to use those needed for secondary processing performance calcs. Don't load what isn't needed.
        //  thuis could be future enhancement; for now add all
        // update met 12/29/16: set default to pull in the aux ts as well (can override). Copy ds to loop through.
        //SP 15-Feb-2017 TODO maybe implement filter simlar to WriteTimeSeriesToRepo to allow certain RetrieveCodes?
        public void EGDS_GetTS_Details(int nScenarioID, bool bReadAllTS=true)            //, string sTS_Filename)
        {
            try
            {
                int nCounter = 0;
                DataTable dtResults;
                if (bReadAllTS)
                    dtResults = _dsEG_ResultTS_Request.Tables[0];  //SP 15-Feb-2017 Get all TS
                else
                    dtResults = _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode = " + ((int)RetrieveCode.Primary).ToString()).CopyToDataTable(); //SP 15-Feb-2017 only primary

                if (dtResults.Rows.Count > 0)            //  && sTS_Filename != "NOTHING")
                {
                    // met 10/11/18: should probably check that the dictionary has the exact count as datatable...
                    if (_dictResultTS_Indices.Count == 0)
                    {
                        throw new Exception(string.Format("{0} timeseries requested, but no indices stored in the results dictionary.", dtResults.Rows.Count));
                    }

                    if (File.Exists(_hdf5._sHDF_FileName))
                    {

                        _hdf5.hdfOpen(_hdf5._sHDF_FileName);
                        foreach (DataRow dr in dtResults.Rows)
                        {
                            string sScenarioID = "SKIP";        //typically we don't want the scenarioID included
                            if (_nActiveModelTypeID == CommonUtilities._nModelTypeID_SimClim)
                                sScenarioID = dr["ScenarioID"].ToString();

                            string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS,
                                                                        dr["ResultTS_ID"].ToString(), "SKIP", sScenarioID);
                            double[,] dvals = _hdf5.hdfGetDataSeries(sGroupID, "1");
                            if (dvals == null)
                            {
                                _log.AddString(string.Format("Null timeseries retrieved for scenario {0}, record {1}", _nActiveScenarioID.ToString(), sGroupID), Logging._nLogging_Level_2, false, true);

                            }
                            int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())];          //met 5/13/17: get the correct index for the results...
                            _sTS_GroupID[nCounter] = sGroupID;
                            if (dvals != null)
                                _dResultTS_Vals[nIndex] = dvals;                        //nCounter - used previously, updeated to nCounter met 5/13/17
                            else
                            {
                                //log the issue
                            }
                            nCounter++;
                        }
                        _hdf5.hdfClose();
                    }
                    else
                    {
                        //SP 21-Dec-2016 If HDF5 doesn't exist at least populate _sTS_GroupID 
                        //TODO is there anther place where this shoudl be getting done? Giving errors with project (CityOfTracy Nov 2016) with 1 TS and no events
                        foreach (DataRow dr in dtResults.Rows) //SP 15-Feb-2017 modified TS dataset to be consistent with the true condition
                        {
                            string sScenarioID = "SKIP";        //typically we don't want the scenarioID included
                            if (_nActiveModelTypeID == CommonUtilities._nModelTypeID_SimClim)
                                sScenarioID = dr["ScenarioID"].ToString();

                            string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS,
                                dr["ResultTS_ID"].ToString(), "SKIP", sScenarioID);

                            _sTS_GroupID[nCounter] = sGroupID;

                            nCounter++;
                        }
                        // this is the case if baseline data is not around yet.
                        _log.AddString("Load TS failed as file not found: " + _hdf5._sHDF_FileName, Logging._nLogging_Level_1, false, true);
                        Console.WriteLine("Load TS failed as file not found: " + _hdf5._sHDF_FileName);     // log is not getting written to screen?
                    }
                }
            }
            catch (Exception ex){
                string sMessage = string.Format("Error loading scenario timeseries details: {0}", ex.Message);
                throw new Exception(sMessage);
            }
        }

        //SP 15-Feb-2017 - this should now be retrieved through EGDS_GetTS_Details - this procedure shoudn't be needed
        // met 8/15/14: updated to start counter AFTER originalTS
        //question: why is the TS array being loaded at this very time?
        /*private void EGDS_GetTS_SecondaryDetails(int nScenarioID)  //, string sTS_Filename)
        {
            int nCounter = _dsEG_ResultTS_Request.Tables[0].Rows.Count;

            if (_dsEG_SecondaryTS_Request.Tables[0].Rows.Count > 0)     // && sTS_Filename != "NOTHING")
            {
                _hdf5.hdfOpen(_hdf5._sHDF_FileName);
                foreach (DataRow dr in _dsEG_SecondaryTS_Request.Tables[0].Rows)
                {
                    string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_ID"].ToString(), "SKIP", "SKIP");
                    double[,] dvals = _hdf5.hdfGetDataSeries(sGroupID, "1");
                    _sTS_GroupID[nCounter] = sGroupID;
                    if (dvals != null)
                        _dResultTS_Vals[nCounter] = dvals;
                    else
                    {
                        //log the issue
                    }
                    nCounter++;
                }
                _hdf5.hdfClose();
            }
        }*/


        /// <summary>
        /// Load the aux results for the EG
        ///     1st attempt: look in the EG itself.
        ///     if not found, then look in the ref
        ///     
        ///     NOTE: HDF file may differ from active scenario
        /// 
        ///         TODO TODO: implement the second loop looking on the base scenario of ref eval
        /// </summary>
        /// <param name="nScenarioID"></param>
        //SP 15-Feb-2017 - this should now be retrieved through EGDS_GetTS_Details - this procedure shoudn't be needed
        /*protected void EGDS_GetTS_AuxDetails(int nScenarioID)  //, string sTS_Filename)
        {
            try
            {
                bool bLoadedFromHDF = true;
                int nCounter = _dsEG_ResultTS_Request.Tables[0].Rows.Count + _dsEG_SecondaryTS_Request.Tables[0].Rows.Count;

                //todo: you'll need to be more sophisticatedd about getting this aux TS data
                string sHDF_FileName = CommonUtilities.GetSimLinkFull_TS_FilePath(_sActiveModelLocation, _nActiveModelTypeID, _nActiveEvalID, _nActiveBaselineScenarioID, true);
                if (_dsEG_TS_AUX_Request.Tables[0].Rows.Count > 0)     // && sTS_Filename != "NOTHING")
                {
                    _hdf5.hdfOpen(sHDF_FileName);
                    foreach (DataRow dr in _dsEG_TS_AUX_Request.Tables[0].Rows)
                    {
                        string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_ID"].ToString(), "SKIP", "SKIP");
                        double[,] dvals = _hdf5.hdfGetDataSeries(sGroupID, "1");
                        _sTS_GroupID[nCounter] = sGroupID;
                        if (dvals != null)
                            _dResultTS_Vals[nCounter] = dvals;
                        else
                        {
                            bLoadedFromHDF = false;
                            //log the issue
                        }
                        nCounter++;
                    }

                    _hdf5.hdfClose();

                    //SP 1-Dec-2016 replaced existing code with ExternalDataRequest - 
                    //TODO determine where ExtractExternalData is best called from????
              // met: don't believe this should be called here.  discuss with Sanjay      ExtractExternalData();
                }
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error getting auxilliary time series data. Error: {0}", ex.Message), Logging._nLogging_Level_1);
                throw;
            }
        }*/


        private DataSet EGDS_GetPerformanceDetail(int nScenarioID = -1)
        {
            string sWhere = "(PF_DetailID<0)";
            if (nScenarioID != -1)
                sWhere = "(ScenarioID_FK = " + nScenarioID.ToString() + ")";
            //string sqlInsertPerfDetail = "select PF_DetailID, PerformanceID_FK, DVID_FK, VAL,ScenarioID_FK,IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK" //SP 13-Jul-2016 removed DVID_FK, IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK from DB Schema
            string sqlInsertPerfDetail = "select PF_DetailID, PerformanceID_FK, VAL,ScenarioID_FK"
                    + " from tblPerformance_Detail WHERE " + sWhere;
            DataSet dsInsert = _dbContext.getDataSetfromSQL(sqlInsertPerfDetail);

            if (true)       //add to SL list. easier to dup the code and customize the field names ( though not ideal)
            {
                foreach (DataRow dr in dsInsert.Tables[0].Rows)
                {
                    int nRecordID = Convert.ToInt32(dr["PerformanceID_FK"].ToString());
                    string sVal = dr["VAL"].ToString();
                    int nVarType_FK = -1;
                    int nElementID = -1;
                    string sElementName = "";

                    _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.Performance, sVal, nRecordID, nVarType_FK, nScenarioID, nElementID, sElementName));
                }
            }


            return dsInsert;
        }

        #endregion


        public void UpdateScenarioStamp(int nScenarioID, int nScenarioLC_Stage)
        {
            if (!_bIsSimLinkLite)
            {
                //SP 10-Jun-2016 Instead of writing straight back to the DB, save to memory and write back in one procedure at the end.
                //get the ScenID from the dataset in memory
                DataRow[] ScenRow = _dsSCEN_ScenarioDetails.Tables[0].Select("ScenarioID = " + nScenarioID); //only one will be returned due to ScenarioID being a primary key
                int RowNumber = _dsSCEN_ScenarioDetails.Tables[0].Rows.IndexOf(ScenRow[0]);

                //load the details into the scenario dataset in memory
                _dsSCEN_ScenarioDetails.Tables[0].Rows[RowNumber]["HasBeenRun"] = true;
                _dsSCEN_ScenarioDetails.Tables[0].Rows[RowNumber]["ScenLC_LastStage"] = nScenarioLC_Stage;
                _dsSCEN_ScenarioDetails.Tables[0].Rows[RowNumber]["DateEvaluated"] = System.DateTime.Now;
            }
        }

        //write all the datasets back to the DB
        protected void WriteResultsToDB(int nScenarioID)
        {
           
            //SP 9-Jun-2016 moved inserting back to DB until the very end - write all model element changes
            //met 7/17/16: only send if there are records (can do more excplicit testing too based on scenstart/end
            if (!_bIsSimLinkLite)
            {
                if (_IntermediateStorageSpecification._bResultSum && _dsSCEN_ResultSummary.Tables[0].Rows.Count > 0)
                {
                    _dbContext.InsertOrUpdateDBByDataset(true, _dsSCEN_ResultSummary, _sSQL_InsertResultSummary, true, false);
                }

                if (_IntermediateStorageSpecification._bMEV && _dsSCEN_ModVals.Tables[0].Rows.Count > 0)
                {
                    _dbContext.InsertOrUpdateDBByDataset(true, _dsSCEN_ModVals, _sSQL_InsertModVals);
                }

                //SP 10-Jun-2016 moved updating DB until the very end - write all tblscenario updates
                if (_IntermediateStorageSpecification._bScenario && _dsSCEN_ScenarioDetails.Tables[0].Rows.Count > 0)
                {
                    _dbContext.InsertOrUpdateDBByDataset(false, _dsSCEN_ScenarioDetails, _sSQL_UpdateScenarioVals, true, false);
                }

                //SP 14-Jun-2016 write all Event Details back to DB
                if (_IntermediateStorageSpecification._bResultEventSummary && _dsSCEN_EventDetails.Tables[0].Rows.Count > 0)
                {
                    _dbContext.InsertOrUpdateDBByDataset(true, _dsSCEN_EventDetails, _sSQL_InsertEventDetailVals);
                }

                //SP 14-Jun-2016 write all Performance Details back to DB
                if (_IntermediateStorageSpecification._nPerformanceCode != Convert.ToInt32(IntermediateStorageSpecification.IntermediateStorageSpecENUM.PerformanceOptOnly)
                    && _dsSCEN_PerformanceDetails.Tables[0].Rows.Count > 0)
                {
                    _dbContext.InsertOrUpdateDBByDataset(true, _dsSCEN_PerformanceDetails, _sSQL_InsertPerformanceVals);
                }
            }
            if (_bUseCostingModule)
            {
                _cost_wrap.WriteCostingResultsForScenario();
            }
        }


        private void LoadConstantsDictionary()
        {
            string sSQL = "SELECT val, tblConstants.ConstantID FROM tblConstants WHERE (((tblConstants.ProjID_FK)=" + _nActiveProjID + ") OR ((tblConstants.ProjID_FK)=-1))";
            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _dictConstants.Add(Convert.ToInt32(dr["ConstantID"].ToString()), dr["val"].ToString());
            }

        }


        //met 3/23/2012: updated this code to be consistent with housing primary and secondary dv in a single tab (an outcome of refining / streamlining costing.)
        //TODO: Update and test for MU / IW
        //sim2_note: this could also be overidden in the derived class- for quick start left as is
        // met 8/17/14: definitely think this should go in derived class; posibly get rid of "query" dependency.
        public DataSet DNA_GetDVInfo(int nEvalID, int nActiveModelID, out string sql_select, string strFilterString)  //int nModelTypeID)         //was GetDV_EvalList
        {
            //    string sql_select = "";
            sql_select = "unassigned";
            switch (nActiveModelID)
            {
                case (CommonUtilities._nModelTypeID_SWMM):       // met 4/24/2012: add option Scale by
                    //12/30/2013: added IsTS and XMODEL  as part of v2.0 conv
                    //SP 4-Mar-2016 - Requires Testing after changing from using the Query in access
                    sql_select = "SELECT tblDV.DVD_ID, tblDV.DV_Label, tlkpSWMMTableDictionary.TableName, tlkpSWMMFieldDictionary.FieldAlias, " +
                        "tblOptionLists.OptionLabel, tblDV.Option_MIN, tblDV.Option_MAX, tblDV.ElementID_FK as ElementID, " +
                        //"IIf([tblDV].[Element_Label] = '','n/a',[tblDV].[Element_Label]) AS ElementName, 2 AS codeArrColl, " + //SP 13-Jul-2016 removed Element_Label in DB restructure, required if non-list vars
                        "2 AS codeArrColl, " +
                        "tblDV.EvaluationGroup_FK AS EvaluationGroupID, tlkpSWMMFieldDictionary.FieldClass, " +
                        "tblDV.VarType_FK, tblDV.Option_FK AS OptionID, tlkpSWMMTableDictionary.SectionName_Alias AS ModelComponent, " +
                        "tlkpSWMMTableDictionary.IsOwnTable, tlkpSWMMFieldDictionary.FieldName, tblDV.IsListVar, -1 AS HasConsDV, 1 AS UnitConversion, " +
                        "tblDV.SkipMinVal, tblDV.sqn, tblDV.Operation, tblDV.PrimaryDV_ID_FK, tblDV.SecondaryDV_Key, " +
                        //"tblDV.SkipMinVal, tblDV.sqn, tblOptionLists.Operation, tblDV.PrimaryDV_ID_FK, tblDV.SecondaryDV_Key, " + //SP 18-Jul-2016 Operation moved from tblOptionLists to tblDV
                        //"tblOptionLists.IsScaleValue AS IsScaleOptionValue, tblOptionLists.VarType_ScaleBy AS VarType_OptionScaleBy, " + //SP 13-Jul-2016 removed IsScaleValue, vartype_scaleby  from db schema
                        "tblDV.IsSpecialCase, tblFunctions.CustomFunction, tblDV.FunctionArgs, tblDV.GetNewValMethod, tblDV.IsTS, " +
                        "tblDV.XModelID_FK, tblDV.FunctionID_FK, '-1.234' as ElementVal " +
                        "FROM (((tblDV LEFT JOIN tblOptionLists ON tblDV.Option_FK = tblOptionLists.OptionID) " +
                        "LEFT JOIN tlkpSWMMFieldDictionary ON tblDV.VarType_FK = tlkpSWMMFieldDictionary.ID) " +
                        "LEFT JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID) " +
                        "LEFT JOIN tblFunctions ON tblDV.FunctionID_FK = tblFunctions.FunctionID " +
                        "WHERE ((EvaluationGroup_FK)=" + nEvalID + ")";

                    //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server   
                    /*"SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, TableName, FieldAlias, OptionLabel, OptionID, Option_MIN, Option_MAX, Operation, "
                    + "ElementID_FK as ElementID, ElementName, codeArrColl, IncludeInScenarioLabel, FieldClass, VarType_FK, ModelComponent, EvaluationGroupID, '-1.234' as ElementVal, FieldName, IsListVar, HasConsDV, IsTS, XModelID_FK"
                    + ", UnitConversion, SkipMinVal, IsScaleOptionValue,  VarType_OptionScaleBy, FunctionID_FK, CustomFunction, FunctionArgs, GetNewValMethod, SQN  FROM qryDV001_LinkInfo_SWMM WHERE ((EvaluationGroupID)=" + nEvalID + ")";*/

                    //todo: change query names
                    break;
                case (CommonUtilities._nModelTypeID_IW):
                    //SP 4-Mar-2016 TODO remove reliance on Query 'qryRMG_DV001_IW_LinkInfo' in access to ensure compatible with SQL Server 
                    sql_select = "SELECT tblDV.DVD_ID, tblDV.DV_Label, tlkpIWTableDictionary.TableName, tlkpIWFieldDictionary.FieldAlias, " +
                        "tblOptionLists.OptionLabel, tblDV.Option_MIN, tblDV.Option_MAX, tblDV.ElementID_FK as ElementID, " +
                        //"IIf([tblDV].[Element_Label] = '','n/a',[tblDV].[Element_Label]) AS ElementName, 2 AS codeArrColl, " + //SP 13-Jul-2016 removed Element_Label in DB restructure, required if non-list vars
                        "2 AS codeArrColl, " +
                        "tblDV.EvaluationGroup_FK AS EvaluationGroupID, tlkpIWFieldDictionary.FieldClass, " +
                        "tblDV.VarType_FK, tblDV.Option_FK AS OptionID, tlkpIWTableDictionary.ComponentName_Alias AS ModelComponent, " +
                        "tlkpIWTableDictionary.IsOwnTable, tlkpIWFieldDictionary.FieldName, tblDV.IsListVar, -1 AS HasConsDV, 1 AS UnitConversion, " +
                        "tblDV.SkipMinVal, tblDV.sqn, tblDV.Operation, tblDV.PrimaryDV_ID_FK, tblDV.SecondaryDV_Key, " +
                        //"tblDV.SkipMinVal, tblDV.sqn, tblOptionLists.Operation, tblDV.PrimaryDV_ID_FK, tblDV.SecondaryDV_Key, " + //SP 18-Jul-2016 Operation moved from tblOptionLists to tblDV
                        //"tblOptionLists.IsScaleValue AS IsScaleOptionValue, tblOptionLists.VarType_ScaleBy AS VarType_OptionScaleBy, " +  //SP 13-Jul-2016 removed IsScaleValue, vartype_scaleby from db schema
                        "tblDV.IsSpecialCase, tblFunctions.CustomFunction, tblDV.FunctionArgs, tblDV.GetNewValMethod, tblDV.IsTS, " +
                        "tblDV.XModelID_FK, tblDV.FunctionID_FK, '-1.234' as ElementVal " +
                        "FROM (((tblDV LEFT JOIN tblOptionLists ON tblDV.Option_FK = tblOptionLists.OptionID) " +
                        "LEFT JOIN tlkpIWFieldDictionary ON tblDV.VarType_FK = tlkpIWFieldDictionary.ID) " +
                        "LEFT JOIN tlkpIWTableDictionary ON tlkpIWFieldDictionary.TableName_FK = tlkpIWTableDictionary.ID) " +
                        "LEFT JOIN tblFunctions ON tblDV.FunctionID_FK = tblFunctions.FunctionID " +
                        "WHERE ((EvaluationGroup_FK)=" + nEvalID + ")";
                    break;
                case (CommonUtilities._nModelTypeID_EPANET):       // met 3/21/14: add EPANET functionality 
                    //SP 4-Mar-2016 Small modification to remove duplicate 'VarType_OptionScaleBy' for compatibility with SQL Server
                    //met test no use  query; easier to maintain or?
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, TableName, FieldAlias, OptionLabel, tblDV.Option_FK AS OptionID, " +
                        "Option_MIN, Option_MAX,  tblDV.Operation, ElementID_FK as ElementID, 'not used' as ElementName, 2 as codeArrColl, " +
                        //"Option_MIN, Option_MAX,  tblOptionLists.Operation, ElementID_FK as ElementID, 'not used' as ElementName, 2 as codeArrColl, " +//SP 18-Jul-2016 Operation moved from tblOptionLists to tblDV
                        "FieldClass, tblDV.VarType_FK, SectionName_Alias as ModelComponent, tblDV.EvaluationGroup_FK as EvaluationGroupID, '-1.234' as ElementVal," +
                        //" FieldName, IsListVar, -1 as HasConsDV, 1.0 as UnitConversion, SkipMinVal, IsScaleValue as IsScaleOptionValue,  " + //SP 13-Jul-2016 removed IsScaleValue from db schema
                        " FieldName, IsListVar, -1 as HasConsDV, 1.0 as UnitConversion, SkipMinVal, " +
                        //"VarType_ScaleBy as VarType_OptionScaleBy, IsTS, FunctionID_FK, CustomFunction, FunctionArgs, " + //SP 13-Jul-2016 removed VarType_ScaleBy from db schema
                        "IsTS, FunctionID_FK, CustomFunction, FunctionArgs, tblDV.IsSpecialCase, tblDV.XModelID_FK, " +
                        "GetNewValMethod, SQN FROM (((tblDV LEFT JOIN tblOptionLists ON tblDV.Option_FK = tblOptionLists.OptionID) " +
                        "LEFT JOIN tlkpEPANETFieldDictionary ON tblDV.VarType_FK = tlkpEPANETFieldDictionary.ID) " +
                        "LEFT JOIN tlkpEPANET_TableDictionary ON tlkpEPANETFieldDictionary.TableName_FK = tlkpEPANET_TableDictionary.ID) " +
                        "LEFT JOIN tblFunctions ON tblDV.FunctionID_FK = tblFunctions.FunctionID " +
                        "WHERE ((EvaluationGroup_FK)=" + nEvalID + ") ";
                    break;
                case (CommonUtilities._nModelTypeID_MODFLOW):       // USING qryRMG_DV001_SWMM_LinkInfo WORKS EVEN WITH TEMPLATE FILE (SAVE CREATING ADDTL QUERIES)
                    //SP 4-Mar-2016 TODO remove reliance on Query 'qryRMG_DV001_SWMM_LinkInfo' in access to ensure compatible with SQL Server
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, 'N/A' as TableName, 'N/A' as FieldName, DV_Label, OptionLabel, tblDV.Option_FK AS OptionID, Option_MIN, Option_MAX,  tblDV.Operation,"
                        //sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, 'N/A' as TableName, 'N/A' as FieldName, DV_Label, OptionLabel, OptionID, Option_MIN, Option_MAX,  tblOptionLists.Operation," //SP 6-Jun-2016 TODO This operation is not correct, will need to change query //SP 18-Jul-2016 Operation moved from tblOptionLists to tblDV
                                    + "ElementID_FK as ElementID, ElementName, codeArrColl, FieldClass, VarType_FK,  EvaluationGroupID, '-1.234' as ElementVal,  IsListVar, HasConsDV"
                                    + ", UnitConversion, SkipMinVal, IsScaleOptionValue, 4 as FieldClass, VarType_OptionScaleBy, SQN FROM qryRMG_DV001_SWMM_LinkInfo WHERE ((EvaluationGroupID)=" + nEvalID + ") ";
                    break;
                case (5): // SP 23-Jun-2016 this doesn't have a corresponding model type in CommonUtilities._nModelTypeID_MODFLOW
                    //SP 4-Mar-2016 TODO remove reliance on Query 'qryRMG_DV001_Mouse_LinkInfo' in access to ensure compatible with SQL Server
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, TableName, FieldAlias, OptionLabel, tblDV.Option_FK AS OptionID, Option_MIN, Option_MAX,  tblDV.Operation,"
                        //sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, TableName, FieldAlias, OptionLabel, OptionID, Option_MIN, Option_MAX,  tblOptionLists.Operation," //SP 6-Jun-2016 TODO This operation is not correct, will need to change query //SP 18-Jul-2016 Operation moved from tblOptionLists to tblDV
                        + "ElementID_FK as ElementID, ElementName, codeArrColl, FieldClass, VarType_FK, ModelComponent, EvaluationGroupID, '-1.234' as ElementVal, FieldName, IsListVar, HasConsDV"
                        + ", UnitConversion, SkipMinVal, SQN FROM qryRMG_DV001_Mouse_LinkInfo WHERE (((EvaluationGroupID)=" + nEvalID + ")) ";

                    break;
                case (CommonUtilities._nModelTypeID_ISIS1D):
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, IsTS FROM tblDV WHERE (((EvaluationGroup_FK)=" + -1 + "))";     //DNA not yet implemented - do this to avoid return null dataset
                    break;
                case (CommonUtilities._nModelTypeID_ISIS2D):
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, IsTS FROM tblDV WHERE (((EvaluationGroup_FK)=" + -1 + "))";     //DNA not yet implemented - do this to avoid return null dataset
                    break;

                case (CommonUtilities._nModelTypeID_ISIS_FAST):
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, IsTS FROM tblDV WHERE (((EvaluationGroup_FK)=" + -1 + "))";     //DNA not yet implemented
                    break;

                    // met 3/30/17:  load an empty dataset for base simlink...
                case (CommonUtilities._nModelTypeID_Simlink):
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, IsTS FROM tblDV WHERE ((EvaluationGroup_FK)=" + nEvalID + ")";     //DNA not yet implemented
                    break;

                case (CommonUtilities._nModelTypeID_ExtendSim):
                case (CommonUtilities._nModelTypeID_Vissim):
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, OptionLabel, tblDV.Option_FK AS OptionID, " +
                        "Option_MIN, Option_MAX,  tblDV.Operation, ElementID_FK as ElementID, 'not used' as ElementName, 2 as codeArrColl, tblDV.IsSpecialCase," +
                        //"Option_MIN, Option_MAX,  tblOptionLists.Operation, ElementID_FK as ElementID, 'not used' as ElementName, 2 as codeArrColl, " + //SP 18-Jul-2016 Operation moved from tblOptionLists to tblDV
                        "tblDV.VarType_FK, EvaluationGroup_FK as EvaluationGroupID, '-1.234' as ElementVal, " +
                        //"IsListVar, -1 as HasConsDV, 1.0 as UnitConversion, SkipMinVal, IsScaleValue as IsScaleOptionValue,  " + //SP 13-Jul-2016 removed IsScaleValue from db schema
                        "IsListVar, -1 as HasConsDV, 1.0 as UnitConversion, SkipMinVal, " +
                        //"VarType_ScaleBy as VarType_OptionScaleBy, IsTS, FunctionID_FK, CustomFunction, FunctionArgs, " + //SP 13-Jul-2016 removed VarType_ScaleBy from db schema
                        "IsTS, FunctionID_FK, CustomFunction, FunctionArgs, " +
                        "GetNewValMethod, SQN, '-1' as FieldClass  FROM ((tblDV LEFT JOIN tblOptionLists ON tblDV.Option_FK = tblOptionLists.OptionID)" +
                        "LEFT JOIN tblFunctions ON tblDV.FunctionID_FK = tblFunctions.FunctionID) " +
                        "WHERE ((EvaluationGroup_FK)=" + nEvalID + ") ";
                    break;

            }
            if (strFilterString != "") // filter string
            {
                sql_select += " AND " + strFilterString;
            }
            sql_select += " order by sqn, DVD_ID";
            try
            {
                //SP 5-Aug-2016 tidied this up to ensure an exception is thrown if a null dataset is returned
                DataSet dsDVInfo = _dbContext.getDataSetfromSQL(sql_select);
                if (dsDVInfo != null)
                    return dsDVInfo;
                else
                    throw new Exception("SQL returned null from DB when trying to retrieve information from tblDV");
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                _log.AddString("Error retrieving DV info from DB. Exception: " + ex.Message, Logging._nLogging_Level_1);
                throw;
                return null;
            }
        }

        public DataSet EGDS_GetDV_Consequent(int nEvalID, int nActiveModelID)  //int nModelTypeID)
        {
            string sql_select = "";
            switch (nActiveModelID)
            {
                case (CommonUtilities._nModelTypeID_SWMM):               //MET 3/16/2012: Added queries to RMG db and updated SQL below
                    //MET 3/23/2012: Refined the query to support a LIST unspooling in the DV_ Consequent. Previoulsy this was done in
                    //complex query train. This will make it easier.
                    //met 12/11/2013: added PrimaryDV_ID_FK for subsequent _ds filter
                    //SP 4-Mar-2016 - Requires Testing after changing from using the Query in access
                    sql_select = "SELECT tblDV.DVD_ID, tblDV.DV_Label, tlkpSWMMTableDictionary.TableName, tlkpSWMMFieldDictionary.FieldAlias, " +
                        "tblOptionLists.OptionLabel, tblDV.Option_MIN, tblDV.Option_MAX, tblDV.ElementID_FK as ElementID, " +
                        //"IIf([tblDV].[Element_Label] = '','n/a',[tblDV].[Element_Label]) AS ElementName, 2 AS codeArrColl, " + //SP 13-Jul-2016 Element_Label removed from db Schema. Needed only if non-list vars
                        "2 AS codeArrColl, " +
                        "tblDV.EvaluationGroup_FK AS EvaluationGroupID, tlkpSWMMFieldDictionary.FieldClass, " +
                        "tblDV.VarType_FK, tblOptionLists.OptionID, tlkpSWMMTableDictionary.SectionName_Alias AS ModelComponent, " +
                        "tlkpSWMMFieldDictionary.FieldName, tblDV.IsListVar, -1 AS HasConsDV, 1 AS UnitConversion, " +
                        "tblDV.SkipMinVal, tblDV.Operation, tblDV.PrimaryDV_ID_FK, tblDV.SecondaryDV_Key, " +
                        //"tblDV.SkipMinVal, tblOptionLists.Operation, tblDV.PrimaryDV_ID_FK, tblDV.SecondaryDV_Key, " + //SP 18-Jul-2016 Operation moved from tblOptionLists to tblDV
                        "tblDV.IsSpecialCase, tblFunctions.CustomFunction, tblDV.FunctionArgs, tblDV.GetNewValMethod, " +
                        "'-1.234' as ElementVal " +
                        "FROM (((tblDV LEFT JOIN tblOptionLists ON tblDV.Option_FK = tblOptionLists.OptionID) " +
                        "LEFT JOIN tlkpSWMMFieldDictionary ON tblDV.VarType_FK = tlkpSWMMFieldDictionary.ID) " +
                        "LEFT JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID) " +
                        "LEFT JOIN tblFunctions ON tblDV.FunctionID_FK = tblFunctions.FunctionID " +
                        "WHERE ((EvaluationGroup_FK)=" + nEvalID + ")";

                    //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server    
                    /*"SELECT DVD_ID, DV_Label, TableName, FieldAlias, OptionLabel, OptionID, Option_MIN, Option_MAX, Operation, "
                    + "ElementID_FK as ElementID, ElementName, codeArrColl, IncludeInScenarioLabel, FieldClass, VarType_FK, ModelComponent, EvaluationGroupID, '-1.234' as ElementVal, FieldName, IsListVar, HasConsDV"
                    + ", UnitConversion, SkipMinVal, SecondaryDV_Key, IsSpecialCase, PrimaryDV_ID_FK, CustomFunction, FunctionArgs, GetNewValMethod FROM qryDV001_LinkInfo_SWMM WHERE (((EvaluationGroupID)= " + nEvalID + "))";*/
                    //rm  WHERE (((PrimaryDV_ID_FK)=@nDV AND (EvaluationGroupID)=@Eval))";  just get once
                    break;
                case (CommonUtilities._nModelTypeID_IW):
                    //SP 4-Mar-2016 TODO remove reliance on Query 'qryRMG_DV_CONS_001_IW_LinkInfo' in access to ensure compatible with SQL Server
                    sql_select = "SELECT tblDV.DVD_ID, tblDV.DV_Label, tlkpIWTableDictionary.TableName, tlkpIWFieldDictionary.FieldAlias, tblOptionLists.OptionLabel, "
                        //+ "tblDV.Option_MIN, tblDV.Option_MAX, tblDV.ElementID_FK AS ElementID, IIf([tblDV].[Element_Label] = '','n/a',[tblDV].[Element_Label]) AS ElementName, 2 AS codeArrColl, " //SP 13-Jul-2016 removed Element_Label from db Schema. Needed only if non-list vars 
                                + "tblDV.Option_MIN, tblDV.Option_MAX, tblDV.ElementID_FK AS ElementID, 2 AS codeArrColl, "
                                + "tblDV.EvaluationGroup_FK AS EvaluationGroupID, tlkpIWFieldDictionary.FieldClass, tblDV.VarType_FK, tblOptionLists.OptionID, "
                                + "tlkpIWTableDictionary.ComponentName AS ModelComponent, tlkpIWFieldDictionary.FieldName, tblDV.IsListVar, -1 AS HasConsDV, 1 AS UnitConversion, tblDV.SkipMinVal, tblDV.Operation, "
                        //+ "tlkpIWTableDictionary.ComponentName AS ModelComponent, tlkpIWFieldDictionary.FieldName, tblDV.IsListVar, -1 AS HasConsDV, 1 AS UnitConversion, tblDV.SkipMinVal, tblOptionLists.Operation, " //SP 18-Jul-2016 Operation moved from tblOptionLists to tblDV
                                + "tblDV.PrimaryDV_ID_FK, tblDV.SecondaryDV_Key, tblDV.IsSpecialCase, tblFunctions.CustomFunction, tblDV.FunctionArgs, tblDV.GetNewValMethod, '-1.234' AS ElementVal "
                                + "FROM (((tblDV LEFT JOIN tblOptionLists ON tblDV.Option_FK = tblOptionLists.OptionID) LEFT JOIN tlkpIWFieldDictionary ON tblDV.VarType_FK = tlkpIWFieldDictionary.ID) LEFT JOIN tlkpIWTableDictionary ON tlkpIWFieldDictionary.TableName_FK = tlkpIWTableDictionary.ID) LEFT JOIN tblFunctions ON tblDV.FunctionID_FK = tblFunctions.FunctionID "
                                + "WHERE (((tblDV.EvaluationGroup_FK)=" + nEvalID + "))";


                    //         "SELECT DV_c_ID, DV_c_Label, TableName, FieldName, ElementID_FK, ElementName, '-1.234' as ElementVal,  "
                    //    + "EvaluationGroupID, VarType_FK, IsListVar, REF_UseThisVal, REF_Operation, REF_AdditionalData, UnitConversion, OptionID_FK, REF_UseConOptionVal "
                    //   + "FROM qryRMG_DV_CONS_001_IW_LinkInfo WHERE (((DV_ID_FK)=@nDV AND (EvaluationGroupID)=@Eval))";
                    break;

                case (CommonUtilities._nModelTypeID_EPANET):
                    sql_select = "SELECT DVD_ID, DV_Label, TableName, FieldAlias, OptionLabel, OptionID, Option_MIN, Option_MAX, Operation,"
                                + " ElementID_FK as ElementID, 'not used' as ElementName, 2 as codeArrColl, FieldClass, tblDV.VarType_FK, SectionName_Alias as ModelComponent,"
                                + "EvaluationGroup_FK as EvaluationGroupID, '-1.234' as ElementVal, FieldName, IsListVar, -1 as HasConsDV,"
                                + " 1.0 as UnitConversion, SkipMinVal, SecondaryDV_Key, IsSpecialCase, PrimaryDV_ID_FK, CustomFunction, FunctionArgs, GetNewValMethod"
                                + " FROM (((tblDV LEFT JOIN tblOptionLists ON tblDV.Option_FK = tblOptionLists.OptionID) LEFT JOIN tlkpEPANETFieldDictionary ON tblDV.VarType_FK = tlkpEPANETFieldDictionary.ID) LEFT JOIN tlkpEPANET_TableDictionary ON tlkpEPANETFieldDictionary.TableName_FK = tlkpEPANET_TableDictionary.ID) LEFT JOIN tblFunctions ON tblDV.FunctionID_FK = tblFunctions.FunctionID"
                                + " WHERE (((EvaluationGroup_FK)= " + nEvalID + "))";
                    break;


                case (CommonUtilities._nModelTypeID_MODFLOW):       // USING qryRMG_DV001_SWMM_LinkInfo WORKS EVEN WITH TEMPLATE FILE (SAVE CREATING ADDTL QUERIES)
                    //SP 4-Mar-2016 TODO remove reliance on Query 'qryRMG_DV001_SWMM_LinkInfo' in access to ensure compatible with SQL Server
                    sql_select = "SELECT DVD_ID, DV_Label, 'N/A' as TableName, 'N/A' as FieldName,OptionLabel, OptionID, Option_MIN, Option_MAX, Operation, "
                                    + "ElementID_FK as ElementID, ElementName, codeArrColl, FieldClass, VarType_FK,  EvaluationGroupID, '-1.234' as ElementVal,  IsListVar, HasConsDV"
                                    + ", UnitConversion, SkipMinVal, IsScaleOptionValue, 4 as FieldClass, VarType_OptionScaleBy FROM qryRMG_DV001_SWMM_LinkInfo WHERE (((PrimaryDV_ID_FK)=@nDV AND (EvaluationGroupID)=@Eval))";
                    break;


                case (5):       //added 12/20/2011 met - //SP 5-Aug-2016 no modelID exists for #5
                    //SP 4-Mar-2016 TODO remove reliance on Query 'qryRMG_DV_CONS_002_Mouse_UNION_List' in access to ensure compatible with SQL Server
                    sql_select = "SELECT DV_c_ID, DV_c_Label, TableName, FieldName, ElementID_FK, ElementName, '-1.234' as ElementVal,  "
                    + "EvaluationGroupID, VarType_FK, IsListVar, REF_UseThisVal, REF_Operation, REF_AdditionalData, UnitConversion, OptionID_FK, REF_UseConOptionVal "
                    + "FROM qryRMG_DV_CONS_002_Mouse_UNION_List WHERE (((DV_ID_FK)=@nDV AND (EvaluationGroupID)=@Eval))";
                    break;

                case (CommonUtilities._nModelTypeID_ExtendSim):
                case (CommonUtilities._nModelTypeID_Vissim):
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, OptionLabel, tblDV.Option_FK AS OptionID, " +
                        "Option_MIN, Option_MAX,  tblDV.Operation, ElementID_FK as ElementID, 'not used' as ElementName, 2 as codeArrColl, tblDV.IsSpecialCase," +
                        "tblDV.VarType_FK, EvaluationGroup_FK as EvaluationGroupID, '-1.234' as ElementVal, " +
                        "IsListVar, -1 as HasConsDV, 1.0 as UnitConversion, SkipMinVal, " +
                        "IsTS, FunctionID_FK, CustomFunction, FunctionArgs, " +
                        "GetNewValMethod, SQN, '-1' as FieldClass  FROM ((tblDV LEFT JOIN tblOptionLists ON tblDV.Option_FK = tblOptionLists.OptionID)" +
                        "LEFT JOIN tblFunctions ON tblDV.FunctionID_FK = tblFunctions.FunctionID) " +
                        "WHERE ((EvaluationGroup_FK)=" + nEvalID + ") ";
                    break;

                // met 3/30/17:  load an empty dataset for base simlink... (must build out options/functions etc if you need them)
                case (CommonUtilities._nModelTypeID_Simlink):
                    sql_select = "SELECT PrimaryDV_ID_FK, SecondaryDV_Key, DVD_ID, DV_Label, IsTS FROM tblDV WHERE ((EvaluationGroup_FK)=" + nEvalID + ")";     //DNA not yet implemented
                    break;

            }
            //    cu.cuLogging_WriteString(sql_select);
            //       cu.cuLogging_WriteString("nDV_ID, neval: " + nDV_ID + ", " + nEvalID);


            try
            {
                DataSet dsEG_cons = _dbContext.getDataSetfromSQL(sql_select);
                if (dsEG_cons != null)
                    return dsEG_cons;
                else
                    throw new Exception("SQL returned null from DB when trying to retrieve information from tblDV");
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                _log.AddString("Error retrieving DV info from DB. Exception: " + ex.Message, Logging._nLogging_Level_1);
                throw;
                return null;
            }
        }



        #endregion



        #endregion

        #region COM_TESTING
        public double DoubleVal(double dVal)
        {
            return 2 * dVal;
        }

        public string[] DoubleArray(string[] sVals)
        {
            int nItems = sVals.Length;
            string[] sReturn = new string[nItems];

            for (int i = 0; i < nItems; i++)
            {
                double dVal = 2 * Convert.ToDouble(sVals[i]);
                sReturn[i] = dVal.ToString();
            }
            return sReturn;
        }
        #endregion

        #region DataIO

        /// <summary>
        /// MET 7/24/17: Created a push function on the simlink object.
        /// TODO: Decide if the static function in cli should be replaced
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="?"></param>
        /// <param name="nStart"></param>
        /// <param name="nEnd"></param>
        public void Push(string sFile, string sLabel, int nStart = 5, int nEnd = 20, bool bSetupOnly=false){
            if(File.Exists(sFile)){
            
                // insert your scenario
                int nScenarioID = InsertScenario(sLabel);
                // copy file to target location  - todo manage all directory files...
                SetupScenarioFiles(sFile, nScenarioID);

                if(!bSetupOnly)
                    ProcessScenario(_nActiveProjID, _nActiveEvalID, _nActiveReferenceEvalID, _sActiveModelLocation, nScenarioID, nStart, nEnd);                  // RUN THE SCENARIO

                }
                else{
                    _log.AddString(string.Format("File {0} does not exist. Please pass a valid filename.", sFile), SIM_API_LINKS.Logging._nLogging_Level_3, true, true);
                }

        }

        //SP 10-Oct-2017 - Additional default elements to allow for inserting with specified start and end LC steps
        //todo : dont' get the DS each time; have available and update it (Met 7/3/14
        public int InsertScenario(int nEvalGroupID, int nProjID, string sScenarioLabel, string sScenarioDescrip, string sDNA = "-1", int nStart = -1, int nEnd = -1)
        {
            try
            {
                string _sUpdatedScenLabel = sScenarioLabel + " Thread: " + Thread.CurrentThread.ManagedThreadId; //SP 15-Jun-2016 temporary to ensure these scenario labels are unique for multithreading
                string strSQL = ""; string strNewSQL;

                if (nStart == -1 && nEnd == -1) //if default values then use the DB default values for inserting
                {
                    strSQL = "INSERT INTO tblScenario(EvalGroupID_FK, ScenarioLabel, ScenarioDescription, DateCreated, DNA, sqn) VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5})";
                    strNewSQL = string.Format(strSQL, nEvalGroupID, _sUpdatedScenLabel, sScenarioDescrip, DateTime.Now.ToString(), sDNA, -1);
                }
                else //use the function inputs
                {
                    strSQL = "INSERT INTO tblScenario(EvalGroupID_FK, ScenarioLabel, ScenarioDescription, DateCreated, DNA, sqn, ScenStart, ScenEnd) VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7})";
                    strNewSQL = string.Format(strSQL, nEvalGroupID, _sUpdatedScenLabel, sScenarioDescrip, DateTime.Now.ToString(), sDNA, -1, nStart, nEnd);
                }

                _dbContext.ExecuteNonQuerySQL(strNewSQL);

                strSQL = "SELECT MAX(ScenarioID) FROM tblScenario";
                int nReturnID = (int)_dbContext.ExecuteScalarSQL(strSQL);

                return nReturnID;
                //string sqlInsertScenario = _sSQL_InsertScenarioVals;// "select EvalGroupID_FK, ScenarioLabel, ScenarioDescription, DateCreated, DNA from tblScenario where (ScenarioID=-1)";
                //DataSet dsInsert = _dbContext.getDataSetfromSQL(sqlInsertScenario);
                //dsInsert.Tables[0].Rows.Add(dsInsert.Tables[0].NewRow());


                ////now update the values
                ////dsInsert.Tables[0].Rows[0]["ProjID_FK"] = nProjID; //SP 13-Jun-2016 ProjID_FK no longer exists in tblScenario
                //dsInsert.Tables[0].Rows[0]["EvalGroupID_FK"] = nEvalGroupID;
                //dsInsert.Tables[0].Rows[0]["ScenarioLabel"] = _sUpdatedScenLabel;
                //dsInsert.Tables[0].Rows[0]["ScenarioDescription"] = sScenarioDescrip;
                //dsInsert.Tables[0].Rows[0]["DateCreated"] = DateTime.Now;
                //dsInsert.Tables[0].Rows[0]["DNA"] = sDNA;

                //string sPK_SQL = "SKIP";
                //if (true)               //_dbContext._DBContext_DBTYPE==1)          //if access?
                //{
                //    string sWhere = "((ScenarioLabel = '" + _sUpdatedScenLabel + "') and  (EvalGroupID_FK = " + nEvalGroupID + ")) ";
                //    sPK_SQL = DBContext.GetQuerySQL_NewPK_AferInsert("tblScenario", "ScenarioID", sWhere);
                //}
                //int nReturnID = _dbContext.InsertOrUpdateDBByDataset(true, dsInsert, sqlInsertScenario, true, true, sPK_SQL);
                //return nReturnID;//insertStatus + " row(s) updated";
            }
            catch (Exception ex)
            {
                //sim2 cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                _log.AddString("Error trying to insert scenario data to database. Exception: " + ex.Message, Logging._nLogging_Level_1);
                throw;
                //return -666;//"Error " + ex.Message.ToString();
            }
        }

        protected void InsertModelValList(int DV_ID_FK, int TableFieldKey, int ScenID, string val, string note, string ElName, int ElId, int nDV_Option, string sDOMAIN = CommonUtilities._sDATA_UNDEFINED, bool bIsInsert = false)
        {


            int nCurrentRow = -1;
            if (!_bIsSimLinkLite)                           // typical case
                nCurrentRow = _dsSCEN_ModVals.Tables[0].Rows.Count;         //don't decrement because we will add row.
            //SP 15-Jun-2016 - TODO should always be saved to memory dataset and added to simlink detail list
            // if (_IntermediateStorageSpecification._bMEV)
            //{
            try
            {
                //SP 28-Sep-2016 For rules and controls, the ElementName needs to be the entire row for identification and Val indicates whether it is active
             //met2   if (TableFieldKey == _nEPANET_FieldDict_CONTROLS || TableFieldKey == _nEPANET_FieldDict_RULES)
             //met2       ElName = val.Replace(";", "");

                if (!_bIsSimLinkLite)                       // todo: determine if this is needed
                {
                    _dsSCEN_ModVals.Tables[0].Rows.Add(_dsSCEN_ModVals.Tables[0].NewRow());
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["DV_ID_FK"] = DV_ID_FK;
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["TableFieldKey_FK"] = TableFieldKey;
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["ScenarioID_FK"] = ScenID;
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["val"] = val;
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["element_note"] = note;
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["ElementName"] = ElName;
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["ElementID"] = ElId;
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["DV_Option"] = nDV_Option;
                    _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["IsInsert"] = _dbContext.GetTrueBitByContext(bIsInsert);
                    if (sDOMAIN != CommonUtilities._sDATA_UNDEFINED)
                        _dsSCEN_ModVals.Tables[0].Rows[nCurrentRow]["DomainQual"] = sDOMAIN;     //used for ISIS2D: untested
                }
                _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.MEV, val, DV_ID_FK, TableFieldKey, ScenID, ElId, ElName, bIsInsert, nDV_Option));  //add to single in-memory storage repo
                //SP 9-Jun-2016 move inserting back to DB until the very end
                //_dbContext.InsertOrUpdateDBByDataset(true, _dsSCEN_ModVals, _sSQL_InsertModVals);            //todo: consider how to just do this at the end?
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error adding ModelVal items to SimlinkDetail list for DV_ID_FK {0}: Exception", DV_ID_FK, ex.Message), Logging._nLogging_Level_1);
                throw; //SP 5-Aug-2016 don't want to diregard these I don't think - let process scenario handle it so it can be fixed
                //cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                //return -666;
            }
            //}
            //else
            //{
            //log that no
            //}
        }

        //SP 10-Jun-2016 - avoid regular calls back to the database
        protected void InsertScenarioDetails(int nScenarioID, int nProjID_FK, int nEvalGroupID,
            string sScenLabel, string sScenDesc, string sDNA, string dtDateCreated)
        {
            int nCurrentRow = _dsSCEN_ScenarioDetails.Tables[0].Rows.Count;         //don't decrement because we will add row.
            //SP 15-Jun-2016 - TODO should always be saved to memory dataset
            //if (_IntermediateStorageSpecification._bScenario)
            //{
            try
            {
                _dsSCEN_ScenarioDetails.Tables[0].Rows.Add(_dsSCEN_ScenarioDetails.Tables[0].NewRow());
                _dsSCEN_ScenarioDetails.Tables[0].Rows[nCurrentRow]["ScenarioID"] = nScenarioID;
                _dsSCEN_ScenarioDetails.Tables[0].Rows[nCurrentRow]["ProjID_FK"] = nProjID_FK;
                _dsSCEN_ScenarioDetails.Tables[0].Rows[nCurrentRow]["EvalGroupID_FK"] = nEvalGroupID;
                _dsSCEN_ScenarioDetails.Tables[0].Rows[nCurrentRow]["ScenarioLabel"] = sScenLabel;
                _dsSCEN_ScenarioDetails.Tables[0].Rows[nCurrentRow]["ScenarioDescription"] = sScenDesc;
                _dsSCEN_ScenarioDetails.Tables[0].Rows[nCurrentRow]["DateCreated"] = dtDateCreated;
                _dsSCEN_ScenarioDetails.Tables[0].Rows[nCurrentRow]["DNA"] = sDNA;
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error loading scenario details to dataset. Exception: {0}", ex.Message), Logging._nLogging_Level_1);
                throw; //SP 5-Aug-2016 I think we want to throw this exception - but don't actually think this function is used anywhere
            }
        }



        //SP 8-Mar-2016 - Additional model required changes based on a DV value - specific per model type
        protected virtual void AdditionalRequiredModelChanges_SpecialCase
            (int DV_ID_FK, int TableFieldKey, int ScenID, string val, string note, string ElName, int ElId, int nDV_Option)
        {
            //do nothing in generic case
        }

        //SP 8-Mar-2016 - Modify required changes based on a DV value - specific per model type
        protected virtual void ModifyModelChanges_SpecialCase
            (int DV_ID_FK, int TableFieldKey, int ScenID, ref string val, string note, string ElName, int ElId, int nDV_Option)
        {
            //do nothing in generic case
        }


        //met 9/3/2013: static function to insert a result val (UI)
        //met 12/12/2013: changed to use dataset; only called by clim for now (though UI function presumably would call)
        public int InsertTS(int nEvalID, int nVarType, int nElementID, TimeSeries.TimeSeriesDetail tsDtl, string sResultLabel, string sLabel, bool bIsAux = false)
        {

            string sql = "select ResultTS_ID, EvaluationGroup_FK, Result_Label, VarResultType_FK, TS_StartDate, TS_StartHour, TS_StartMin, TS_Interval, TS_Interval_Unit, ElementID_FK, Element_Label, RetrieveCode" //SP 28-Feb-2017 Removed IsAux field in DB
                        + " FROM tblResultTS WHERE (ResultTS_ID<0)";
            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            DataRow dr = ds.Tables[0].NewRow();             // modified new row approach to create the dr and then add
            dr["EvaluationGroup_FK"] = nEvalID;
            dr["VarResultType_FK"] = nVarType;
            dr["TS_StartDate"] = tsDtl._dtStartTimestamp.Date.ToString("d");
            dr["TS_StartHour"] = tsDtl._dtStartTimestamp.Hour;
            dr["TS_StartMin"] = tsDtl._dtStartTimestamp.Minute;
            dr["TS_Interval_Unit"] = TimeSeries.TimeSeriesDetail.GetTSIntervalType(tsDtl._tsIntervalType);
            dr["TS_Interval"] = tsDtl._nTSInterval;
            if (bIsAux)
                dr["RetrieveCode"] =  ((int)RetrieveCode.Aux).ToString();
            else
                dr["RetrieveCode"] = ((int)RetrieveCode.Primary).ToString(); ;
            dr["ElementID_FK"] = nElementID;
            dr["Element_Label"] = sLabel;
            dr["Result_Label"] = sResultLabel;
            ds.Tables[0].Rows.Add(dr);
            string sWhere = "((EvaluationGroup_FK=" + nEvalID + ") and (VarResultType_FK = " + nVarType + ") AND (ElementID_FK = " + nElementID + "))";

            string sGetPK = DBContext.GetQuerySQL_NewPK_AferInsert("tblResultTS", "ResultTS_ID", "((EvaluationGroup_FK = " + nEvalID + ") AND (VarResultType_FK = " + nVarType + "))");
            int nPK = _dbContext.InsertOrUpdateDBByDataset(true, ds, sql, true, true, sGetPK);
            return nPK;
        }

        /// <summary>
        /// Parse row to avoid NULL error
        /// </summary>
        /// <param name="row"></param>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        public string ParseDataRow(DataRow row, string strColumnName)
        {
            if (row.IsNull(strColumnName)) return "";
            else return row[strColumnName].ToString();
        }

        // met commented this out with simlinkUI integration; assume most valid versions are in that 
        //#region UI Leng

        ///// <summary>
        ///// Get option list
        ///// </summary>
        ///// <param name="strProjectID"></param>
        ///// <returns></returns>
        //public DataSet GetOptionListDetails(string strProjectID)
        //{
        //    string strSQL = "SELECT m.OptionID, d.OptionID as OptionDetailKeyId, ProjID_FK, OptionLabel, d.OptionID_FK, d.OptionNo, d.val " +
        //        "FROM tblOptionDetails d INNER JOIN tblOptionLists m ON d.OptionID_FK = m.OptionID " +
        //        "WHERE (((ProjID_FK)=" + strProjectID + ")) ORDER BY m.OptionID, d.OptionNo";
        //    DataSet dsDetail = _dbContext.getDataSetfromSQL(strSQL);
        //    dsDetail.Tables[0].TableName = "tblOptionDetails";

        //    return dsDetail;
        //}
        ///// <summary>
        ///// Getelement list detail
        ///// </summary>
        ///// <param name="strProjectID"></param>
        ///// <returns></returns>
        //public DataSet GetElementListDetails(string strProjectID)
        //{
        //    string strSQL = "SELECT d.ElementListDetailID, d.ElementListID_FK, d.ElementID_FK, d.VarLabel, e.Type " +
        //        "FROM tblElementListDetails d INNER JOIN tblElementLists e ON d.ElementListID_FK = e.ElementListID " +
        //        "WHERE (((ProjID_FK)=" + strProjectID + "))";
        //    DataSet dsDetail = _dbContext.getDataSetfromSQL(strSQL);
        //    dsDetail.Tables[0].TableName = "tblElementListDetails";

        //    return dsDetail;
        //}

        ///// <summary>
        ///// Getelement list detail
        ///// </summary>
        ///// <param name="strProjectID"></param>
        ///// <returns></returns>
        //public DataSet GetElementListDetails()
        //{
        //    string strSQL = "SELECT ElementListDetailID, ElementListID_FK, ElementID_FK, VarLabel FROM tblElementListDetails";
        //    DataSet dsDetail = _dbContext.getDataSetfromSQL(strSQL);
        //    dsDetail.Tables[0].TableName = "tblElementListDetails";

        //    return dsDetail;
        //}

        ///// <summary>
        ///// Get option list detail
        ///// </summary>
        ///// <returns></returns>
        //public DataSet GetOptionListDetail()
        //{
        //    string strSQL = "SELECT OptionID, OptionID_FK, OptionNo, val FROM tblOptionDetails";
        //    return _dbContext.getDataSetfromSQL(strSQL);
        //}
        ///// <summary>
        ///// Delete option list
        ///// </summary>
        ///// <param name="row"></param>
        //public void DeleteElementList(DataRow row)
        //{
        //    string strElementID = row["ElementListDetailID"].ToString();
        //    string strSQL = "DELETE * FROM tblElementListDetails WHERE ElementListDetailID = " + strElementID;
        //    _dbContext.RunDeleteSQL(strSQL);
        //}
        ///// <summary>
        ///// Delete option list
        ///// </summary>
        ///// <param name="row"></param>
        //public void DeleteOptionList(DataRow row)
        //{
        //    string strOptionID = row["OptionID"].ToString();
        //    string strSQL = "DELETE * FROM tblOptionDetails WHERE OptionID = " + strOptionID;
        //    _dbContext.RunDeleteSQL(strSQL);
        //}

        ///// <summary>
        ///// insert or update element list
        ///// </summary>
        ///// <param name="ds"></param>
        ///// <param name="blnInsertNew"></param>
        //public void InsertOrUpdateElementListTable(DataSet ds, bool blnInsertNew)
        //{
        //    if (ds == null) return;
        //    string strSQL = "";
        //    if (blnInsertNew)
        //    {
        //        strSQL = "INSERT INTO tblElementListDetails(ElementListID_FK, val, ElementID_FK, VarLabel) VALUES ({0}, '{1}', {2}, '{3}')";
        //    }
        //    else
        //    {
        //        strSQL = "UPDATE tblElementListDetails SET ElementListID_FK={0}, " +
        //                "val='{1}', " +
        //                "ElementID_FK={2}, " +
        //                "VarLabel='{3}' " +
        //                "WHERE ElementListDetailID={4}";
        //    }
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        int intElementListDetailID = int.Parse(row["ElementListDetailID"].ToString());
        //        int intElementListID_FK = int.Parse(row["ElementListID_FK"].ToString());
        //        int intElementID_FK = int.Parse(row["ElementID_FK"].ToString());
        //        string strVarLabel = row["VarLabel"].ToString();
        //        string strNewSQL = string.Format(strSQL, intElementListID_FK, "", intElementID_FK, strVarLabel, intElementListDetailID);
        //        _dbContext.InsertOrupdatebySQLString(strNewSQL);
        //    }
        //}
        ///// <summary>
        ///// Insert or update DV table
        ///// </summary>
        ///// <param name="ds"></param>
        ///// <param name="blnInsertNew"></param>
        //public void InsertOrUpdateSummaryResultTable(DataSet ds, bool blnInsertNew, int intEvalationGroupID)
        //{
        //    if (ds == null) return;
        //    string strSQL = "";
        //    if (blnInsertNew)
        //    {

        //        strSQL = "INSERT INTO tblResultVar(Result_Label, EvaluationGroup_FK, VarResultType_FK, "
        //                + "Result_Description, ElementID_FK, Element_Label) VALUES ('{0}', {1}, {2}, '{3}', "
        //                + "{4}, '{5}')";
        //    }
        //    else
        //    {
        //        strSQL = "UPDATE tblResultVar SET Result_Label='{0}', " +
        //                 "EvaluationGroup_FK = {1}, " +
        //                 "VarResultType_FK = {2}, " +
        //                 "Result_Description = '{3}', " +
        //                 "ElementID_FK = {4}, " +
        //                 "Element_Label = '{5}' " +
        //                 "WHERE Result_ID = {6}";
        //    }
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        string Result_ID = ParseDataRow(row, "Result_ID");
        //        string Result_Label = ParseDataRow(row, "Result_Label");
        //        string EvaluationGroup_FK = intEvalationGroupID.ToString(); // ParseDataRow(row, "EvaluationGroupID");
        //        string VarResultType_FK = ParseDataRow(row, "VarResultType_FK");
        //        //string DV_Type = ParseDataRow(row, "DV_Type");
        //        string Result_Description = ParseDataRow(row, "Result_Description");
        //        string ElementID_FK = ParseDataRow(row, "ElementID");
        //        string Element_Label = ParseDataRow(row, "Element_Label");

        //        string strNewSQL = string.Format(strSQL, Result_Label, EvaluationGroup_FK, VarResultType_FK,
        //            Result_Description, ElementID_FK, Element_Label, Result_ID);
        //        _dbContext.InsertOrupdatebySQLString(strNewSQL);
        //    }

        //}


        ///// <summary>
        ///// Parse row to avoid NULL error
        ///// </summary>
        ///// <param name="row"></param>
        ///// <param name="strColumnName"></param>
        ///// <returns></returns>
        //public string ParseDataRow(DataRow row, string strColumnName)
        //{
        //    if (row.IsNull(strColumnName)) return "";
        //    else return row[strColumnName].ToString();
        //}

        ///// <summary>
        ///// load all scenario within the project and active Evaluation Group id into the grid
        ///// </summary>
        ///// <param name="strProjectID"></param>
        ///// <param name="strActiveEVID"></param>
        ///// <returns></returns>
        //public DataSet LoadScenario(string strProjectID, string strActiveEVID)
        //{
        //    string strSQL = "SELECT ScenarioID, ProjID_FK, EvalGroupID_FK, ScenarioLabel, DateEvaluated, HasBeenRun, "
        //        + "DNA, ScenStart, ScenEnd FROM tblScenario WHERE ProjID_FK=" + strProjectID + " AND EvalGroupID_FK =" + strActiveEVID;
        //    return _dbContext.getDataSetfromSQL(strSQL);
        //}
        ///// <summary>
        ///// Import scenario from csv file
        ///// </summary>
        ///// <param name="strCSVFile"></param>
        ///// <param name="strProjectID"></param>
        ///// <param name="strEvalID"></param>
        //public void ImportScenarioFromCSV(string strCSVFile, string strProjectID, string strEvalID)
        //{
        //    string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
        //    string strFolder = new FileInfo(strShortenedFile).DirectoryName;
        //    string strCSV = new FileInfo(strShortenedFile).Name;
        //    string strSQL = "INSERT INTO tblScenario(ProjID_FK, EvalGroupID_FK, ScenarioLabel, DNA, HasBeenRun, ScenStart, ScenEnd) " +
        //                    "SELECT " + strProjectID + "," + strEvalID + ", * FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
        //    _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
        //}
        ///// <summary>
        ///// Import result from CSV file
        ///// </summary>
        ///// <param name="strCSVFile"></param>
        ///// <param name="strEvalID"></param>
        //public void ImportResultFromCSV(string strCSVFile, string strEvalID)
        //{
        //    string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
        //    string strFolder = new FileInfo(strShortenedFile).DirectoryName;
        //    string strCSV = new FileInfo(strShortenedFile).Name;
        //    string strSQL = "INSERT INTO tblResultTS(EvaluationGroup_FK, Result_Label, Result_Description, ElementID_FK, Element_Label) " +
        //                    "SELECT " + strEvalID + ", * FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
        //    _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
        //}
        ///// <summary>
        ///// Import DV from CSV file
        ///// </summary>
        ///// <param name="strCSVFile"></param>
        //public void ImportDVFromCSV(string strCSVFile)
        //{
        //    string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
        //    string strFolder = new FileInfo(strShortenedFile).DirectoryName;
        //    string strCSV = new FileInfo(strShortenedFile).Name;

        //    string strSQL = "INSERT INTO tblDV(DV_Label,EvaluationGroup_FK,VarType_FK,DV_Description,DV_Type,Option_FK," +
        //        "Option_MIN,Option_MAX,GetNewValMethod,FunctionID_FK,FunctionArgs,ElementID_FK,sqn,Operation_DV," +
        //        "SecondaryDV_Key,PrimaryDV_ID_FK,IsSpecialCase) " +
        //        "SELECT * FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
        //    _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
        //}

        //#endregion


        #endregion


        #region ScenarioValues


        /// <summary>
        /// Retrieve the scenario name to use in naming of external files
        /// Implemented only in iw class as of 9/6/16
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <param name="nAltFrameworkScenarioID"></param>
        /// <returns></returns>
        public int GetScenarioForName(int nScenarioID)
        {
            if (!_bUseAltFrameworkScen)
                return nScenarioID;
            else
            {
                if (_nScenarioID_AltFramework == -1)
                {
                    _log.AddString("Alt scenario ID requested, but set to -1. Override.", Logging._nLogging_Level_2);
                    return nScenarioID;
                }
                else
                {
                    return _nScenarioID_AltFramework;
                }
            }
        }

        /*
         * codebase.Methods.Where(x => (x.Body.Scopes.Count > 5) && (x.Foo == "test"));

Or you can use a separate Where call for each condition:

codebase.Methods.Where(x => x.Body.Scopes.Count > 5)
                .Where(x => x.Foo == "test");       */


        public string GetSimLinkDetail(SimLinkDataType_Major slDataType, int nRecordID, int nVarType_FK, int nScenarioID, int nElementID)
        {
            string sVal = CommonUtilities._sBAD_DATA;

            //first, see if it is stored in the list.
            sVal = GetSimLinkDetail_FromMem(slDataType, nRecordID, nVarType_FK, nScenarioID, nElementID);

            if (sVal == CommonUtilities._sBAD_DATA)
            {
                sVal = GetSimLinkDetail_NotInMem(slDataType, nRecordID, nVarType_FK, nScenarioID, nElementID);
                //now add the val to memory
                SimLinkDetail_InsertIntoMem(slDataType, nRecordID, nVarType_FK, nScenarioID, nElementID, sVal);
            }
            return sVal;
        }

        /// <summary>
        /// Return a simlink detai thatmeets a criteria
        /// Deveoped to support sending data to a python function
        /// 
        /// </summary>
        /// <param name="slDataType"></param>
        /// <param name="nRecordID"></param>
        /// <param name="nVarType_FK"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="nElementID"></param>
        /// <returns></returns>
        private simlinkDetail GetSimlinkDetail(SimLinkDataType_Major slDataType, int nRecordID, int nVarType_FK, int nScenarioID, int nElementID)
        {
            if (slDataType == SimLinkDataType_Major.MEV)
            {
                simlinkDetail det = _lstSimLinkDetail
                                 .Where(x => x._nScenarioID == nScenarioID)
                                 .Where(x => x._slDataType == slDataType)
                                 .Where(x => x._nRecordID == nRecordID)
                                 .Where(x => x._nElementID == nElementID).SingleOrDefault();
                if (det != null)
                    return det;
                else
                    return null;
            }
            else
            {
                IEnumerable<simlinkDetail> lstSimDetail = _lstSimLinkDetail.Where(x => x._nScenarioID == nScenarioID)          //.Where(x => x._nElementID == nElementID)  MET 6/2/17
                 .Where(x => x._nVarType_FK == nVarType_FK)        //met 12/30- added to work with NETWORK retrieval.. all still good? 
                 .Where(x => x._slDataType == slDataType)
                 .Where(x => x._nRecordID == nRecordID);

                if (lstSimDetail.Count() > 0)
                {
                    return lstSimDetail.First(); // return the first scen (Should be only one
                    // log the
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Retrieve a LIST of simlink details
        ///     VERSION 1: This filters on VarType_FK (basically for results...
        ///     VERSION 2: Support category search and more?
        /// </summary>
        /// <param name="slDataType"></param>
        /// <param name="nRecordID"></param>
        /// <param name="nVarType_FK"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="nElementID"></param>
        /// <returns></returns>
        private List<simlinkDetail> GetSimlinkDetail(SimLinkDataType_Major slDataType, int nRecordID, int nVarType_FK, int nScenarioID, int nElementID, bool bIsList)
        {
            if (slDataType == SimLinkDataType_Major.MEV)
            {
                IEnumerable<simlinkDetail> mev = _lstSimLinkDetail
                                 .Where(x => x._nScenarioID == nScenarioID)
                                 .Where(x => x._slDataType == slDataType)
                    //   .Where(x => x._nRecordID == nRecordID)   REM the filter on record id
                                 .Where(x => x._nElementID == nElementID);
                if (mev != null)
                    return mev.ToList();
                else
                    return null;
            }
            else
            {
                IEnumerable<simlinkDetail> lstSimDetail = _lstSimLinkDetail.Where(x => x._nScenarioID == nScenarioID)          //.Where(x => x._nElementID == nElementID)  MET 6/2/17
                 .Where(x => x._nVarType_FK == nVarType_FK)        //met 12/30- added to work with NETWORK retrieval.. all still good? 
                 .Where(x => x._slDataType == slDataType);
              //   .Where(x => x._nRecordID == nRecordID);  REM the filter on record id

                if (lstSimDetail.Count() > 0)
                {
                    return lstSimDetail.ToList();
                    // log the
                }
                else
                {
                    return null;
                }
            }
        }

        private string GetSimLinkDetail_FromMem(SimLinkDataType_Major slDataType, int nRecordID, int nVarType_FK, int nScenarioID, int nElementID)
        {
            string sVal = CommonUtilities._sBAD_DATA;

            if (nRecordID == 2830)
            {
                int n = 1;
            }

            if (slDataType == SimLinkDataType_Major.MEV)
            {
                var filteredProjects = _lstSimLinkDetail
                                 .Where(x => x._nScenarioID == nScenarioID)
                                 .Where(x => x._slDataType == slDataType)
                                 .Where(x => x._nRecordID == nRecordID)
                                 .Where(x => x._nElementID == nElementID).AsEnumerable();
                if (filteredProjects != null)
                    if (true)
                    {
                        if (filteredProjects.Count() > 0)
                        {
                            sVal = filteredProjects.First()._sVal;
                            _log.AddString(string.Format("{0} MEV instances retured for DV {1}; returning first.", filteredProjects.Count(), nRecordID), Logging._nLogging_Level_3, false, true);
                        }
                        else
                        {
                            sVal = "0";  
                        }
                    }
                    else
                        sVal = "CONCATENATE ME";
                else
                {
                    if (true)                   //may want to add a check
                        sVal = "0";              //MEV is special case- may not be found, meaning zero of that type was added.
                }
            }
            else
            {       //do not filter on Element ID for these

                if (nVarType_FK == -1)   // this is if you are retriveing from an equation- var type fk not known.
                {
                    var filteredProjects = _lstSimLinkDetail.Where(x => x._nScenarioID == nScenarioID)          //.Where(x => x._nElementID == nElementID)  MET 6/2/17                    //   .Where(x => x._nVarType_FK == nVarType_FK)        //met 12/30- added to work with NETWORK retrieval.. all still good? 
                     .Where(x => x._slDataType == slDataType)
                     .Where(x => x._nRecordID == nRecordID);    //.SingleOrDefault();                          //the record includes the element ID as well as vartype

                    if (filteredProjects.Count() > 0) //!= null) //SP 28-Jun-2017 when no matches with linq, returns an empty set that is NOT null. First() was causing exception //Uncertain how this was working before
                    {
                        sVal = filteredProjects.First()._sVal;
                        if (filteredProjects.Count() > 0)       //met 6/4/17: getting error previously when multiple vals returned
                        {
                            _log.AddString(string.Format("Warning: {0} vals found for record {1} on active scenario {2}. This typically occurs if an EG baseline is not run all the way through", filteredProjects.Count().ToString(), nRecordID.ToString(), nScenarioID.ToString()), Logging._nLogging_Level_2, false, true);
                        }
                    }
                }
                else
                {
                    var filteredProjects = _lstSimLinkDetail.Where(x => x._nScenarioID == nScenarioID)          //.Where(x => x._nElementID == nElementID)  MET 6/2/17
                     .Where(x => x._nVarType_FK == nVarType_FK)        //met 12/30- added to work with NETWORK retrieval.. all still good? 
                     .Where(x => x._slDataType == slDataType)
                     .Where(x => x._nRecordID == nRecordID);    //.SingleOrDefault();                          //the record includes the element ID as well as vartype

                    if (filteredProjects.Count() > 0) //!= null) //SP 28-Jun-2017 when no matches with linq, returns an empty set that is NOT null. First() was causing exception //Uncertain how this was working before
                    {
                        sVal = filteredProjects.First()._sVal;
                        if (filteredProjects.Count() > 0)       //met 6/4/17: getting error previously when multiple vals returned
                        {
                            _log.AddString(string.Format("Warning: {0} vals found for record {1} on active scenario {2}. This typically occurs if an EG baseline is not run all the way through", filteredProjects.Count().ToString(), nRecordID.ToString(), nScenarioID.ToString()), Logging._nLogging_Level_2, false, true);
                        }
                    }
                }
            }
            return sVal;
        }


        private void SimLinkDetail_InsertIntoMem(SimLinkDataType_Major slDataType, int nRecordID, int nVarType_FK, int nScenarioID, int nElementID, string sVAL)
        {
            simlinkDetail slDT = new simlinkDetail(slDataType, sVAL, nRecordID, nVarType_FK, nScenarioID, nElementID);
            _lstSimLinkDetail.Add(slDT);
        }


        //this functions goes and gets the needed value if not found in memory

        private string GetSimLinkDetail_NotInMem(SimLinkDataType_Major slDataType, int nRecordID, int nVarType_FK, int nScenarioID, int nElementID)
        {
            string sReturn = CommonUtilities._sBAD_DATA;
            DataSet ds; string sql = "";

            switch (slDataType)
            {
                case SimLinkDataType_Major.Network:
                    ds = null;
                    sReturn = GetNetworkAttribute(nVarType_FK, nScenarioID, nElementID);
                    break;
                case SimLinkDataType_Major.MEV:
                    sql = "select val from tblModelElementVals where ((ScenarioID_FK=" + nScenarioID + ") AND (TableFieldKey_FK=" + nVarType_FK + "))";
                    ds = _dbContext.getDataSetfromSQL(sql);
                    break;
                default:
                    ds = null;
                    break;

            }
            if (ds != null)
                sReturn = ds.Tables[0].Rows[0][0].ToString();

            return sReturn;
        }
        protected string GetNetworkAttribute(int nVarType_FK, int nScenarioID, int nElementID)
        {
            string sReturn = "-1";
            if (_dictSL_TableNavigation.ContainsKey(nVarType_FK))
            {
                simlinkTableHelper slTH = _dictSL_TableNavigation[nVarType_FK];
                string sSQL = GetNetworkAttribute_SQL_GEN(ref slTH, nElementID);
                DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sReturn = ds.Tables[0].Rows[0][0].ToString();
                }
                else
                { } //log that element ID was not found.
            }
            return sReturn;
        }
        private string GetNetworkAttribute_SQL_GEN(ref simlinkTableHelper slTH, int nElementID)
        {
            string sSQL = "select " + slTH._sFieldName + " FROM " + slTH._sTableName
                        + " WHERE (" + slTH._sKeyFieldName + " = " + nElementID.ToString() + ")";
            return sSQL;
        }


        #endregion
        #region DNAProcessing
        public int DNA_GetBit(ref string sDNA)
        {
            int nOptionVal;

            try
            {
                if (sDNA.IndexOf(_sDNA_Delimiter) > 0)              //typical case, delimiter found
                {
                    int nDelPos = sDNA.IndexOf(_sDNA_Delimiter);
                    nOptionVal = Convert.ToInt32(sDNA.Substring(0, nDelPos));
                    sDNA = sDNA.Substring(nOptionVal.ToString().Length + 1, sDNA.Length - nDelPos - 1);         //requires 1 char delimiter (easy to allow more, but why?)
                }
                else
                {
                    if (_sDNA_Delimiter == "")                 //support (stupid) case where user not using delimiter (??)
                    {
                        nOptionVal = Convert.ToInt32(sDNA.Substring(0, 1));
                        sDNA = sDNA.Substring(1, sDNA.Length - 1);
                    }
                    else
                    {
                        nOptionVal = Convert.ToInt32(sDNA);         //return the whole sint val
                    }
                }
            }
            catch (Exception ex)       //string error probably
            {
                _log.AddString(string.Format("Error trying to find bit value for DNA string {0}. Exception {1}", ex.Message), Logging._nLogging_Level_1); //SP 5-Aug-2016
                throw; //SP 5-Aug-2016 think we want to throw an error here TODO test!
                nOptionVal = -666;
            }
            return nOptionVal;
        }
        public int DNA_CountDV(string sDNA)
        {
            if (_sDNA_Delimiter == "")
            {
                return sDNA.Length;
            }
            else
            {
                int nLength = sDNA.Length - sDNA.Replace(_sDNA_Delimiter, "").Length;
                if (sDNA.Substring(sDNA.Length - 1) != _sDNA_Delimiter)                      //subtract one if last char is not the delimiter.
                    nLength++;
                return nLength;        // this assumes that there is a 'hanging delimiter on end'.  could test for if needed
            }

        }


        /// <summary>
        /// Processes tblSupportingFileSpec records
        /// only DataType_Code < 0 should be handled in the base class
        /// </summary>
        protected virtual void ProcessNonDV_FileDependency(string sFilePath, int nScenID = -1, int nRefEvalID = -1)
        {
            //foreach (DataRow dr in _dsEG_SupportingFileSpec.Tables[0].Select("source_DataFormat in (" +
            //    string.Join(",", (int[])Enum.GetValues(typeof(DataFormat))) 
            //    + ") and IsInput"))
            foreach (ExternalData ex in _lstExternalDataDestinations.Where(x => x._bIsInput))
            {
                          // todo: default val of -1 uses Result ElementName, val of -2 uses ResultLabel, Filename overwrites
                //int nVarType_FK = Convert.ToInt32(dr["VarType_FK"].ToString());
                int nDV_ID_FK = ex._nDVID; //Convert.ToInt32(dr["DV_ID_FK"].ToString());
                DataFormat nOutputFormat = (DataFormat)ex._return_format; //(DataFormat)Convert.ToInt32(dr["source_DataFormat"].ToString());
                ExternalDataType edtExternalDataType = (ExternalDataType)ex._externaldatatype; //(ExternalDataType)Convert.ToInt32(dr["Destination_Code"].ToString());
                int nRecordID = ex._nTSRecordID; // Convert.ToInt32(dr["RecordID_FK"].ToString());

                switch (edtExternalDataType)
                {
                    case ExternalDataType.TBLMODELEMENTVALS:
                        //SP 15-Dec-2016 If importing the whole timeseries then retrieve the required value
                        //int nStartingPeriodNumber = _tsdResultTS_SIM.GetTSPeriodNumberFromDateTime(_tsdResultTS_SIM._dtStartTimestamp) - 1;

                        //Get value
                        int nIndex = _dictResultTS_Indices[nRecordID];

                        //if (nStartingPeriodNumber < 0)
                        //    nStartingPeriodNumber = 0;

                        double dModElementVal = _dResultTS_Vals[nIndex][0, 0];

                        //get DV row equating to this change and process the elements that require changing in the model
                        DataRow drDV = _dsEG_DecisionVariables.Tables[0].Select("DVD_ID = " + nDV_ID_FK).First();
                        DecipherElementValueChanges(drDV, nScenID, nRefEvalID, _nActiveModelTypeID, -1, dModElementVal.ToString());
                        break;

                    case ExternalDataType.DAT:
                        string sFileName = (ex as external_csv)._sFilename;
                        WriteDatFile(nRecordID, sFilePath, sFileName);
                        break;
                }
            }
        }

        protected virtual void WriteDatFile(int nRecordID, string sBasePath, string sLabel, bool bIncludeScenarioInFilename = false)
        {
        }

        ///need to implement this

        public int DistribDNAToScenario(string sDNA, int nEvalGroupID, int nRefEvalID, int nActiveProjID, int nActiveModelTypeID, int nLoopNo, int nNewScenarioID = -1)
        {
            string sScenLabel; string sScenDescrip; string sElementLabel; int nDV_ID; int nElementID; int nVarType_FK; int nOptionVal; int nFieldClass;
            string sOptionList_Val; string sReturn; int nOptionID; string sExistVal; string sRootFolder; bool bSkipMinVal; int nOptionMin; /*bool bOptionScaleVal; int nVarType_OptionScaleBy;*/
            bool bIsInsert = false; string sCustomFunction; int nGetNewValMethod; string sFunctionArgs;
            //  DataSet dsElementLibrary = new DataSet();   //holds any element library value associated with the DNA val


            //sim2      cu.cuLogging_AddString("Begin DNA Distrib: " + System.DateTime.Now.ToString(), Logging._nLogging_Level_2);

            if (_bFirstPass)
            {
                _nNumDV = DNA_CountDV(sDNA);  //  count the dna (based on delimiter                                                    cu.getDNA_Length(sDNA);
                //sim2  should already be loaded, right?  LoadReferenceDatasets();
                _nLoopNumber = 1;
                _bFirstPass = false;                //may belong further down; 
            }


            //sim2            cu.cuLogging_AddString("Num DV: " + nNumDV, Logging._nLogging_Level_2);
            //sim2            cu.cuLogging_AddString("Data Rows: " +_dsEG_DecisionVariables.Tables[0].Rows.Count, Logging._nLogging_Level_2);

            //updated code below to check only primary DV
            DataRow[] drDNA_Primary = _dsEG_DecisionVariables.Tables[0].Select("PrimaryDV_ID_FK = -1");
            if (_nNumDV == drDNA_Primary.Count())           //_dsEG_DecisionVariables.Tables[0].Select("PrimaryDV_ID_FK = -1").Count())     //check that DV string has the same number of 'bits' as what is returned from dv eval
            {
                if (nNewScenarioID == -1)
                {
                    sScenLabel = "LOOP_" + _nLoopNumber.ToString();
                    nNewScenarioID = InsertScenario(nEvalGroupID, nActiveProjID, sScenLabel, sScenLabel, sDNA);
                    _nLoopNumber++;
                }

                Console.WriteLine("Processing {0} DVs", _nNumDV);
                for (int i = 0; i < _nNumDV; i++)            //unravel the DNA
                {
                    nOptionVal = DNA_GetBit(ref sDNA);

                    //     Convert.ToInt32(sDNA.Substring(0, 1));                     //todo: make more general; support delimiters.//    sDNA = sDNA.Substring(1, sDNA.Length - 1);
                    nDV_ID = Convert.ToInt32(drDNA_Primary[i]["DVD_ID"].ToString());
                    nOptionID = Convert.ToInt32(drDNA_Primary[i]["OptionID"].ToString());
                    nOptionMin = Convert.ToInt32(drDNA_Primary[i]["Option_MIN"].ToString());
                    sOptionList_Val = GetOptionVal(nOptionID, nOptionVal);                //todo: somehow hold in memory instead of searching every time?
                    nVarType_FK = Convert.ToInt32(drDNA_Primary[i]["VarType_FK"].ToString());         //VarType_FK;
                    bSkipMinVal = Convert.ToBoolean(drDNA_Primary[i]["SkipMinVal"].ToString());
                    //bOptionScaleVal = Convert.ToBoolean(drDNA_Primary[i]["IsScaleOptionValue"].ToString());
                    //nVarType_OptionScaleBy = Convert.ToInt32(drDNA_Primary[i]["VarType_OptionScaleBy"].ToString());
                    nFieldClass = Convert.ToInt32(drDNA_Primary[i]["FieldClass"].ToString());
                    nGetNewValMethod = Convert.ToInt32(drDNA_Primary[i]["GetNewValMethod"].ToString());         //5/21/14- add for function processing
                    sFunctionArgs = drDNA_Primary[i]["FunctionArgs"].ToString();
                    sCustomFunction = drDNA_Primary[i]["CustomFunction"].ToString();
                    bool bIsSpecialCase = Convert.ToBoolean(drDNA_Primary[i]["IsSpecialCase"].ToString());          //met 5/14/17


                    if (nDV_ID == 2830)
                        nDV_ID = nDV_ID;    //debug

                    if (i == 201)
                        nDV_ID = nDV_ID;    //debug


                    //SP 8-Mar-2016 Need to modify this to Option Value rather than Option Number
                    if ((bSkipMinVal && (nOptionVal > nOptionMin)) || !bSkipMinVal)        // met 5/2/2012 enforce equality         determine whether to skip this dv, if user has requested the dv to skip the min val.
                    {
                        //string sFieldScalar = ""; string sTableScalar = "";
                        //      if (nVarType_OptionScaleBy > 0)       //special case- get scalar field tablekey / field once per dv pass
                        //     {
                        //        cu.SetFieldTableName(nActiveModelType, nVarType_OptionScaleBy, ref sTableScalar, ref sFieldScalar, ref connRMG);
                        //   }

                        if (Convert.ToBoolean(drDNA_Primary[i]["IsListVar"]))
                        {   //in this case, the DV represents a number of actual model elements
                            DataRow[] drARRListVars = _dsEG_ElementList.Tables[0].Select("DVD_ID = " + nDV_ID);           //todo get with LINQ
                            //sim2                dtListVars.Columns["ElementVal"].ReadOnly = false;
                            //sim2                 DataRow drDVEval =drDNA_Primary[i];

                            //sim2                if (drDNA_Primary[i]["Operation"].ToString() != "Identity")    //met 3/19/2012: only get the existing conditions val when the operation calls for it. not needed for identity
                            //sim2                 {
                            //sim2                   rmgHELPER_ExistListVarModelVals_Update(ref dtListVars, drDVEval, nActiveModelTypeID, connMod);
                            //sim2               }


                            foreach (DataRow dr in drARRListVars)
                            {
                                string sNewVal = CommonUtilities._sDATA_UNDEFINED;
                                double dFieldScalar = 1.0;
                                //sim2                    if (nVarType_OptionScaleBy > 0)       //special case- scale by the value that is stored in a given database field by. Generally this will be skipped, and is only needed in rare circumstances
                                //sim2                    {
                                //sim2 //sim2                      dFieldScalar = Convert.ToDouble(rmgHELPER_getModelElementVal(sTableScalar, sFieldScalar, Convert.ToInt32(dr["ElementID_FK"].ToString()), nActiveModelTypeID, 1.0));
                                //sim2                  }
                                int nElementID_FK = Convert.ToInt32(dr["ElementID_FK"].ToString());


                                if (nFieldClass != 1)           //not a UID
                                {
                                    bIsInsert = false;          //typical case- updated performed (LNC)
                                    string sElementVal = dr["ElementVal"].ToString();
                                    if (drDNA_Primary[i]["Operation"].ToString().ToLower() != "identity") //SP 4-Aug-2016 TODO use OperationType Enum and test
                                    {
                                        sElementVal = GetSimLinkDetail(SimLinkDataType_Major.Network, nElementID_FK, nVarType_FK, _nActiveBaselineScenarioID, nElementID_FK);
                                    }

                                    sNewVal = GetNewModelVal(sElementVal, sOptionList_Val, drDNA_Primary[i]["Operation"].ToString(), "-1", true, dFieldScalar); //SP 4-Aug-2016 TODO use OperationType Enum and test
                                }
                                else
                                {                           // insert new val from tblElementLibrary
                                    bIsInsert = true;

                                    string sFilter = "VarTypeID_FK = " + nVarType_FK.ToString();

                                    //     cu.cuLogging_WriteString("outside for");
                                    DataRow[] drElement = _dsElementLibrary.Tables[0].Select(sFilter);           //only 1 row should be returned.
                                    if (drElement.Count() == 1)
                                    {

                                        sNewVal = drElement[0]["ElementLibVal"].ToString();                         //todo: neeed support for more than one?
                                        //SP 15-Jun-2016 TODO 'SubstituteTuple' function has a call to the DB, need to factor this out to reference memory
                                        sNewVal = SubstituteTuple(sNewVal, drElement[0]["SubTuple"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nActiveModelTypeID);      //process the tuple on element lib to do semi-fancy replacements


                                    }
                                    else
                                    {
                                        //todo: log the issue (either too many or too few
                                    }


                                }
                                //now insert any new <legitimate> values into the db.
                                //met 11/15/2013: add some processing to calc function if necessary
                                //proceed through above code to get val, which is generally an input to the function.
                                //met 5/14/2014: added to main function
                                if (nGetNewValMethod == 2)        //if we need a function to get the actual val
                                {
                                    sFunctionArgs = sFunctionArgs.Replace("_DV_", sNewVal);
                                    //todo:    get the element ID          sFunctionArgs = sFunctionArgs.Replace("_ELEMENTID_", nElementID.ToString());
                                    sNewVal = Parse_EvaluateExpression(nNewScenarioID, _nActiveEvalID, sCustomFunction, sFunctionArgs);
                                }

                                if ((sNewVal != "error%") && (sNewVal != "val_no_change%"))
                                {
                                    //met 1/3/2012: add option to perform several inserts
                                    //may want to add a switch to amke this occur, or pass arg, but for now \r\n is the key

                                    if (nFieldClass == 1)
                                    {
                                        while ((sNewVal.IndexOf("\\r\\n") >= 0) && (sNewVal.Length >= 4))
                                        {
                                            string sNew_wNewline = sNewVal.Substring(0, sNewVal.IndexOf("\\r\\n"));

                                            InsertModelValList(nDV_ID, nVarType_FK, nNewScenarioID, sNew_wNewline, "", dr["VarLabel"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nOptionVal, CommonUtilities._sDATA_UNDEFINED, bIsInsert);
                                            sNewVal = sNewVal.Substring(sNewVal.IndexOf("\\r\\n") + 4);
                                        }
                                    }
                                    else
                                    {
                                        //SP 8-Mar-2014 Additional special case code per model which can be called to modify required changes

                                        if (bIsSpecialCase)
                                        {
                                            ModifyModelChanges_SpecialCase (nDV_ID, nVarType_FK, nNewScenarioID, ref sNewVal, "", dr["VarLabel"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nOptionVal);

                                        }

                                        InsertModelValList(nDV_ID, nVarType_FK, nNewScenarioID, sNewVal, "", dr["VarLabel"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nOptionVal, CommonUtilities._sDATA_UNDEFINED, bIsInsert);
                                    }//todo: log this error

                                    //SP 8-Mar-2014 Additional special case code per model which can be called to add required changes to the model
                                    AdditionalRequiredModelChanges_SpecialCase
                                        (nDV_ID, nVarType_FK, nNewScenarioID, sNewVal, "", dr["VarLabel"].ToString(),
                                        Convert.ToInt32(dr["ElementID_FK"].ToString()), nOptionVal);

                                }
                                if (Convert.ToInt32(drDNA_Primary[i]["HasConsDV"].ToString()) != 0)
                                {
                                    //met TODO: if you call from within the loop, must pass the ID of the element list item and change d/s query to handle                           rmgHELPER_ProcessDV_Consequent(nNewScenarioID, nDV_ID, sNewVal, sOptionList_Val, nActiveModelTypeID, connMod);
                                }

                            }

                            //       cu.cuLogging_WriteString("pre DV Cons scen / eval / nDV_ID / nOption / sOption / nActiveModel:)" + nNewScenarioID.ToString() + ", " + nRefEvalID.ToString() + ", " + nDV_ID.ToString() + ", " + sOptionList_Val + ", " + nActiveModelTypeID.ToString());
                            ProcessDV_Consequent(nRefEvalID, nNewScenarioID, nDV_ID, "-666", nOptionVal, sOptionList_Val, nActiveModelTypeID);       //met 5/9 change to reference
                            //process consequent DV? need to potentially add this

                        }
                        else
                        {
                            //non-lists not supported at this time.
                            //stub code removed in SimLink 2.0
                            //todo: log issue
                            Console.WriteLine("non list vars not supported; check DV setup");
                        }
                    }
                }
                //    cu.cuLoggingClose();                
                return nNewScenarioID;
            }
            else
            {               //else: number of DV does not match the number of DVs
                //sim2       cu.cuLogging_AddString("DV Setup Error: DV Count does not equal DNA STRING", Logging._nLogging_Level_2);
                _bEvalIsValid = false;
                _log.AddString("DNA count does not match string val passed; Check scenario/formulation", Logging._nLogging_Level_1);
                Console.WriteLine("DNA count does not match string val passed; Check scenario/formulation");
                throw new Exception("DNA count does not match string val passed; Check scenario/formulation");
                //return -1;      //DV count did not match the string that was passed. this is an error and shouldn't be processed.
            }
        }


        //SP 19-Dec-2016 TODO This was configured to factor out a part of DistribDNAToScenario and should be migrated to be used by this function eventually.
        private void DecipherElementValueChanges(DataRow drPrimaryDV, int nScenarioID, int nRefEvalID, int nActiveModelTypeID, int nOptionVal, string sRealOptionVal = null)
            /*DataRow[] drARRListVars, int nFieldClass, int nVarType_FK, bool bIsInsert, int nActiveModelTypeID, string sFunctionArgs, 
            string sCustomFunction, int nGetNewValMethod, int nScenarioID, string sHasConsDV, string sOperation = "Identity", int nParentID = -1, int nOptionVal = -1, string sOptionList_Val = null)*/
        {
            int nDV_ID; int nVarType_FK; int nFieldClass;
            int nOptionID; bool bSkipMinVal; int nOptionMin;
            bool bIsInsert = false; string sCustomFunction; int nGetNewValMethod; string sFunctionArgs;

            nDV_ID = Convert.ToInt32(drPrimaryDV["DVD_ID"].ToString());
            nOptionID = Convert.ToInt32(drPrimaryDV["OptionID"].ToString());
            nOptionMin = Convert.ToInt32(drPrimaryDV["Option_MIN"].ToString());
            //SP 19-Dec-2016 if nOptionVal = -1, then imply passing a RealOptionVal and should not be obtained from OptionList
            if (nOptionVal != -1)
                sRealOptionVal = GetOptionVal(nOptionID, nOptionVal);                //todo: somehow hold option vals in memory instead of searching every time?
            nVarType_FK = Convert.ToInt32(drPrimaryDV["VarType_FK"].ToString());         //VarType_FK;
            bSkipMinVal = Convert.ToBoolean(drPrimaryDV["SkipMinVal"].ToString());
            nFieldClass = Convert.ToInt32(drPrimaryDV["FieldClass"].ToString());
            nGetNewValMethod = Convert.ToInt32(drPrimaryDV["GetNewValMethod"].ToString());         //5/21/14- add for function processing
            sFunctionArgs = drPrimaryDV["FunctionArgs"].ToString();
            sCustomFunction = drPrimaryDV["CustomFunction"].ToString();


            //SP 19-Dec-2016 Changed this to not assume Min Real Value coincides with Min Option Number
            if ((bSkipMinVal && (nOptionVal != OptionValWithMinRealOptionVal(nOptionID))) || !bSkipMinVal)
            {

                if (Convert.ToBoolean(drPrimaryDV["IsListVar"]))
                {   //in this case, the DV represents a number of actual model elements
                    DataRow[] drARRListVars = _dsEG_ElementList.Tables[0].Select("DVD_ID = " + nDV_ID);           //todo get with LINQ

                    foreach (DataRow dr in drARRListVars)
                    {
                        string sNewVal = CommonUtilities._sDATA_UNDEFINED;
                        double dFieldScalar = 1.0;
                        //sim2                    if (nVarType_OptionScaleBy > 0)       //special case- scale by the value that is stored in a given database field by. Generally this will be skipped, and is only needed in rare circumstances
                        //sim2                    {
                        //sim2 //sim2                      dFieldScalar = Convert.ToDouble(rmgHELPER_getModelElementVal(sTableScalar, sFieldScalar, Convert.ToInt32(dr["ElementID_FK"].ToString()), nActiveModelTypeID, 1.0));
                        //sim2                  }
                        int nElementID_FK = Convert.ToInt32(dr["ElementID_FK"].ToString());


                        if (nFieldClass != 1)           //not a UID
                        {
                            bIsInsert = false;          //typical case- updated performed (LNC)
                            string sElementVal = dr["ElementVal"].ToString();
                            if (drPrimaryDV["Operation"].ToString().ToLower() != "identity") //SP 4-Aug-2016 TODO use OperationType Enum and test
                            {
                                sElementVal = GetSimLinkDetail(SimLinkDataType_Major.Network, nElementID_FK, nVarType_FK, _nActiveBaselineScenarioID, nElementID_FK);
                            }

                            sNewVal = GetNewModelVal(sElementVal, sRealOptionVal, drPrimaryDV["Operation"].ToString(), "-1", true, dFieldScalar); //SP 4-Aug-2016 TODO use OperationType Enum and test
                        }
                        else
                        {                           // insert new val from tblElementLibrary
                            bIsInsert = true;

                            string sFilter = "VarTypeID_FK = " + nVarType_FK.ToString();

                            //     cu.cuLogging_WriteString("outside for");
                            DataRow[] drElement = _dsElementLibrary.Tables[0].Select(sFilter);           //only 1 row should be returned.
                            if (drElement.Count() == 1)
                            {

                                sNewVal = drElement[0]["ElementLibVal"].ToString();                         //todo: neeed support for more than one?
                                //SP 15-Jun-2016 TODO 'SubstituteTuple' function has a call to the DB, need to factor this out to reference memory
                                sNewVal = SubstituteTuple(sNewVal, drElement[0]["SubTuple"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nActiveModelTypeID);      //process the tuple on element lib to do semi-fancy replacements


                            }
                            else
                            {
                                //todo: log the issue (either too many or too few
                            }


                        }
                        //now insert any new <legitimate> values into the db.
                        //met 11/15/2013: add some processing to calc function if necessary
                        //proceed through above code to get val, which is generally an input to the function.
                        //met 5/14/2014: added to main function
                        if (nGetNewValMethod == 2)        //if we need a function to get the actual val
                        {
                            sFunctionArgs = sFunctionArgs.Replace("_DV_", sNewVal);
                            //todo:    get the element ID          sFunctionArgs = sFunctionArgs.Replace("_ELEMENTID_", nElementID.ToString());
                            sNewVal = Parse_EvaluateExpression(nScenarioID, _nActiveEvalID, sCustomFunction, sFunctionArgs);
                        }

                        if ((sNewVal != "error%") && (sNewVal != "val_no_change%"))
                        {
                            //met 1/3/2012: add option to perform several inserts
                            //may want to add a switch to amke this occur, or pass arg, but for now \r\n is the key

                            if (nFieldClass == 1)
                            {
                                while ((sNewVal.IndexOf("\\r\\n") >= 0) && (sNewVal.Length >= 4))
                                {
                                    string sNew_wNewline = sNewVal.Substring(0, sNewVal.IndexOf("\\r\\n"));

                                    InsertModelValList(nDV_ID, nVarType_FK, nScenarioID, sNew_wNewline, "", dr["VarLabel"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nOptionVal, CommonUtilities._sDATA_UNDEFINED, bIsInsert);
                                    sNewVal = sNewVal.Substring(sNewVal.IndexOf("\\r\\n") + 4);
                                }
                            }
                            else
                            {
                                //SP 8-Mar-2014 Additional special case code per model which can be called to modify required changes
                                ModifyModelChanges_SpecialCase
                                    (nDV_ID, nVarType_FK, nScenarioID, ref sNewVal, "", dr["VarLabel"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nOptionVal);

                                InsertModelValList(nDV_ID, nVarType_FK, nScenarioID, sNewVal, "", dr["VarLabel"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nOptionVal, CommonUtilities._sDATA_UNDEFINED, bIsInsert);
                            }//todo: log this error

                            //SP 8-Mar-2014 Additional special case code per model which can be called to add required changes to the model
                            AdditionalRequiredModelChanges_SpecialCase
                                (nDV_ID, nVarType_FK, nScenarioID, sNewVal, "", dr["VarLabel"].ToString(),
                                Convert.ToInt32(dr["ElementID_FK"].ToString()), nOptionVal);

                        }
                        if (Convert.ToInt32(drPrimaryDV["HasConsDV"].ToString()) != 0)
                        {
                            //met TODO: if you call from within the loop, must pass the ID of the element list item and change d/s query to handle                           rmgHELPER_ProcessDV_Consequent(nNewScenarioID, nDV_ID, sNewVal, sOptionList_Val, nActiveModelTypeID, connMod);
                        }
                    }

                    //       cu.cuLogging_WriteString("pre DV Cons scen / eval / nDV_ID / nOption / sOption / nActiveModel:)" + nNewScenarioID.ToString() + ", " + nRefEvalID.ToString() + ", " + nDV_ID.ToString() + ", " + sOptionList_Val + ", " + nActiveModelTypeID.ToString());
                    ProcessDV_Consequent(nRefEvalID, nScenarioID, nDV_ID, "-666", nOptionVal, sRealOptionVal, nActiveModelTypeID);       //met 5/9 change to reference
                    //process consequent DV? need to potentially add this

                }
                else
                {
                    //non-lists not supported at this time.
                    //stub code removed in SimLink 2.0
                    //todo: log issue
                    Console.WriteLine("non list vars not supported; check DV setup");
                }
            }
        }

        /// <summary>
        /// Load one or multiple objectives from performance objective into 
        /// this assumes that objectives has the right length already
        /// </summary>
        /// <param name="objectives"></param>
        public void LoadObjectives(double[] objectives)
        {

            //note: _dsSCen should have vals only for this scenario
            // so you don't need to filter on the scnario.
            var slObjectives = from ObjectiveEQ in _dsEG_Performance_Request.Tables[0].AsEnumerable()
                               join ScenarioValues in _dsSCEN_PerformanceDetails.Tables[0].AsEnumerable()
                               on ObjectiveEQ.Field<int>("PerformanceID") equals
                               ScenarioValues.Field<int>("PerformanceID_FK")
                               where ObjectiveEQ.Field<Boolean>("IsObjective") == true
                               select new
                               {
                                   val = ScenarioValues.Field<double>("val")
                               };
            int nCounter = 0;
            foreach (var dVal in slObjectives)
            {
                objectives[nCounter] = Convert.ToDouble(dVal.val);            // assign the value to the array (passed by ref) //SP 9-Mar-2016 added a .val
            }

        }


        //todo: shouuld be able to reference entirely different option list. (on dvConsequent table, not the parent dv sent as argument)
        //met 1/1/2013: added insert capability similar to DV based on UID

        public void ProcessDV_Consequent(int nEvalID, int nScenarioID, int nDV_ID, string sExistVal, int nOptionVal, string sOptionList_Val, int nActiveModelTypeID)
        {
            string sOptionList_c_Val = ""; //int nOption_ID; int nOptionMin;
            string sElementLabel = ""; int nElementID;
            bool bIsInsert = false; //added to track whether the MEV must be inserted.  4/11/14 LNC

            try
            {

                string sExistVal_Cons; string sNewVal = ""; int nVarType_FK; string sScenDescrip; string sReturn; bool bIsSpecialCase;
                string sFilter = "PrimaryDV_ID_FK = " + nDV_ID.ToString();

                //     cu.cuLogging_WriteString("outside for");
                DataRow[] drDV_Consequent = _dsEG_DV_Consequent.Tables[0].Select(sFilter);
                if (nDV_ID == 2812)
                {
                    nDV_ID = nDV_ID;        //debug
                }

                for (int j = 0; j < drDV_Consequent.Length; j++)      //could be several consequent changes associated with single dv change
                {
                    int nDV_ID_Secondary = Convert.ToInt32(drDV_Consequent[j]["DVD_ID"]);
                    int nFieldClass = Convert.ToInt32(drDV_Consequent[j]["FieldClass"]);
                    int nElementListID = Convert.ToInt32(drDV_Consequent[j]["ElementID"]);           //classified as ElementID on tblDV- must correspond to a list var.
                    int nGetNewValMethod = Convert.ToInt32(drDV_Consequent[j]["GetNewValMethod"]);
                    string sCustomFunction = drDV_Consequent[j]["CustomFunction"].ToString();
                    string sFunctionArgs = drDV_Consequent[j]["FunctionArgs"].ToString();

                    nVarType_FK = Convert.ToInt32(drDV_Consequent[j]["VarType_FK"].ToString());
                    bIsSpecialCase = Convert.ToBoolean(drDV_Consequent[j]["IsSpecialCase"]);            //added 1/4/2013

                    if (nDV_ID_Secondary == 2812)
                    {
                        nDV_ID_Secondary = nDV_ID_Secondary;        //debug
                    }


                    if (Convert.ToBoolean(drDV_Consequent[j]["IsListVar"]))
                    {   //in this case, the DV represents a number of actual model elements
                        DataRow[] drARRListVarsCONS = _dsEG_ElementList.Tables[0].Select("DVD_ID = " + nDV_ID_Secondary);

                        //    cu.cuLogging_WriteString("inside list var");
                        string sElementVal = drDV_Consequent[j]["ElementVal"].ToString();
                        //IMPORTANT: spe present, the DvConsequent processing uses the OPERATION that is defined on tblDV_Consequent.   (MET3/16/2012)
                        // This differs from how operation is pulled from DV (from the tblOptionList itself).
                        //This is a "trial:" to see which was is more sensible /flexible
                        //TODO: Apply consistent approach in both instances.


                        foreach (DataRow dr in drARRListVarsCONS)
                        {
                            nElementID = Convert.ToInt32(dr["ElementID_FK"].ToString());                    //
                            sElementLabel = dr["VarLabel"].ToString();
                            nVarType_FK = Convert.ToInt32(drDV_Consequent[j]["VarType_FK"].ToString());         //VarType_FK;

                            if (drDV_Consequent[j]["Operation"].ToString() != "Identity") //SP 4-Aug-2016 TODO use OperationType Enum and test
                            {
                                //if "identity", we do not need the baseline values (as we are setting to a value). Avoid the wasted time of an update.  TODOL carry this change into rmgDNA_DistribToScenario
                                //met 3/16/2012 UNTESTED BELOW. TODO: Test for non-identity; modifications may be required for the update function to work.
                                sElementVal = GetSimLinkDetail(SimLinkDataType_Major.Network, nElementID, nVarType_FK, _nActiveBaselineScenarioID, nElementID);

                                //old       _dsEG_DV_Consequent.Tables[0].Columns["ElementVal"].ReadOnly = false;
                                //old       rmgHELPER_ExistModelVals_Update(ref _dsEG_DV_Consequent.Tables[0], nActiveModelTypeID, connMod);
                            }


                            if (nFieldClass != 1)           // typical case where its not the UID
                            {
                                bIsInsert = false;          //typical case- update
                                //SP 4-Aug-2016 added enum for SecondaryDVType (preivously 1,2,3) to link to UI
                                switch (Convert.ToInt32(drDV_Consequent[j]["SecondaryDV_Key"]))       //to do: handle null value- this causes unnecessary bomb  out
                                {
                                    case ((int)SecondaryDVType.OptionSetFromOptionList):       //treat like regular DV.
                                        sOptionList_c_Val = GetOptionVal(Convert.ToInt32(drDV_Consequent[j]["OptionID"].ToString()), nOptionVal);
                                        //   sExistVal_Cons = drDV_Consequent[j]["ElementVal"].ToString();     //met 3/16/2012 I think the ExistVal will already be updated? So no need to mke the call that has now been commented out.           rmgHELPER_getModelElementVal(drDV_Consequent[j]["TableName"].ToString(), drDV_Consequent[j]["FieldName"].ToString(), nElementID, nActiveModelTypeID, -1, connMod);
                                        sNewVal = GetNewModelVal(sElementVal, sOptionList_c_Val, drDV_Consequent[j]["Operation"].ToString(), "nothing");   // met 3/23/2012 additional data not supported       drDV_Consequent[j]["REF_AdditionalData"].ToString());
                                        break;
                                    case ((int)SecondaryDVType.OptionSetFromPrimaryDV):
                                        {     //CASE: use the value that is beinsg set for the main DV
                                            //MET 3/16/2012: UNTESTED in new code setup. Need to verify. Seems like we may be making more calls than are necessary.
                                            sNewVal = sOptionList_Val;


                                            // met 8/15/14: dont think this is needed?
                                            // in general, should ONLY get exist val when needed by the DV operation.
                                            /*nOptionMin = Convert.ToInt32(drDV_Consequent[j]["Option_MIN"].ToString());
                                            sExistVal_Cons = GetModelElementVal(drDV_Consequent[j]["TableName"].ToString(), drDV_Consequent[j]["FieldName"].ToString(), nElementID, nActiveModelTypeID, 1);
                                            sNewVal = GetNewModelVal(sExistVal_Cons, sOptionList_Val, drDV_Consequent[j]["Operation"].ToString(), "nothing");   // met 3/23/2012 additional data not supported      , drDV_Consequent[j]["REF_AdditionalData"].ToString());
                                            
                                             * */
                                            break;
                                        }
                                    case ((int)SecondaryDVType.MaintainExistingValue):
                                        {   //CASE: UNTESTED
                                            sExistVal_Cons = sExistVal;     //met3/16212: unclear what sExistVal is.
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                bIsInsert = true;          // LNC update - track that this is an insert
                                sFilter = "VarTypeID_FK = " + nVarType_FK.ToString();           //todo: should be able to have multiple element lib object per sim

                                //     cu.cuLogging_WriteString("outside for");
                                DataRow[] drElement = _dsElementLibrary.Tables[0].Select(sFilter);           //only 1 row should be returned.
                                if (drElement.Count() == 1)
                                {
                                    sNewVal = drElement[0]["ElementLibVal"].ToString();                         //todo: neeed support for more than one?
                                    sNewVal = SubstituteTuple(sNewVal, drElement[0]["SubTuple"].ToString(), Convert.ToInt32(dr["ElementID_FK"].ToString()), nActiveModelTypeID);      //process the tuple on element lib to do semi-fancy replacements
                                }
                                else
                                {
                                    sNewVal = CommonUtilities._sDATA_UNDEFINED;
                                    //todo: log the issue (either too many or too few
                                }
                            }
                            //now insert any new <legitimate> values into the db.
                            //met 11/15/2013: add some processing to calc function if necessary
                            //proceed through above code to get val, which is generally an input to the function.

                            if (nGetNewValMethod == 2)        //if we need a function to get the actual val
                            {
                                sFunctionArgs = sFunctionArgs.Replace("_DV_", sNewVal);
                                sFunctionArgs = sFunctionArgs.Replace("_ELEMENTID_", nElementID.ToString());
                                sNewVal = Parse_EvaluateExpression(nScenarioID, nEvalID, sCustomFunction, sFunctionArgs);
                            }

                            //begin special case processes
                            //overwrites the sNewVal from above
                            //todo: integrate with the primary distributor

                            Dictionary<int, string> dictResultVals = new Dictionary<int, string>();             //functions may have several return vals
                            int nElementID_Special = 0;                             //ElementID to be used for special case processing
                            int nVarType_FK_SpecialCase = 187;                    //TODO : pull from SimLink. This cannot 
                            string sElementLabel_Special = "";
                            string sPrefix_AGG = nEvalID + "_1_" + nVarType_FK_SpecialCase;     //basic part of the lookup key (specific to allow a large dictionary). "_1_" references a function (could be max/total/min etc) with 1 indicating total 
                            //todo : support multiple functions
                            string sAggregateDict_Lookup;


                            if (bIsSpecialCase)            //special case processes.   TODO: may be better way to identify them
                            {
                                int nSpecialCaseKey = 1;//  was 3// 3;        //TODO: pull from SimLink met 1/8/2013
                                double dAggregateVal = -1;


                                switch (nActiveModelTypeID)
                                {

                                    case 1:     //SWMM
                                        //only do following step if you need to have the aggregate available.
                                        //sim2 4/7/14: aggregation better done inside SWMM clase?
                                        // consider...  dAggregateVal = GetBaselineElementList_Aggregate(nVarType_FK_SpecialCase, nElementListID, sPrefix_AGG);  //get the total for the ElementList of interest    

                                        switch (nVarType_FK)
                                        {
                                            case 402:     //LID_USAGE Special Case
                                                //    int n = ReturnNum(nSpecialCaseKey, dLoadingRatio, nElementID, sElementLabel, sOptionList_c_Val, -1); 
                                                double dLoadingRatio = 7;   //todo: pass as param
                                                nSpecialCaseKey = 2;    //LNC- todo pass as param
                                                int n = ReturnNum();            //nSpecialCaseKey, dLoadingRatio);  //, nElementID, sElementLabel, sOptionList_c_Val, -1); 
                                                //   dictResultVals 

                                                //todo: parameterize function call so these items can be calculated in code. lot of thinking required
                                                nElementID_Special = -1;      //do
                                                sElementLabel_Special = sElementLabel;    //same in this case
                                                /*                if (sElementLabel_Special == "3756-L");
                                                                    sElementLabel_Special = sElementLabel_Special;*/
                                                double dVal = Convert.ToDouble(sOptionList_c_Val);
                                                int[] nVals = new int[2];
                                                nVals[0] = nElementID;
                                                nVals[1] = nSpecialCaseKey;

                                                //met 4/9/14: getting this call below to work was VERY challenging
                                                //many configurations of attempts let to ACCESS VIOLATION errors.
                                                //oddly, it seemed to depend on the number of vars sent (on basis of MUCH testing).
                                                //thus, the stupid pack into nvals.

                                                dictResultVals = GetNewModVals_SPECIALCASE(sElementLabel_Special, dVal, nVals, dLoadingRatio);       //, nElementID);    //, -1);                  //todo: Param1 should come from SimLink. Hard codeed for now.


                                                break;
                                        }

                                        break;

                                    case 2:

                                        break;

                                }
                            }

                            if ((sNewVal != "error%") && (sNewVal != "val_no_change%"))
                            {
                                sScenDescrip = "Cons DV :" + nDV_ID;
                                if (bIsSpecialCase)
                                {        //enumerate over dictionary
                                    foreach (KeyValuePair<int, string> pair in dictResultVals)
                                    {
                                        //kvp used for vartype//NewVal.  special case elementID/Label used   met 1/4/2013    
                                        InsertModelValList(nDV_ID_Secondary, pair.Key, nScenarioID, pair.Value, sScenDescrip, sElementLabel_Special, nElementID_Special, nOptionVal, CommonUtilities._sDATA_UNDEFINED, bIsInsert);
                                    }
                                }
                                else
                                {
                                    //SP 27-Sep-2016 - Modified to incorporate similar logic to primary variables where process an special requirements with each variable type
                                    if (nFieldClass == 1)
                                    {
                                        //typical case
                                        InsertModelValList(nDV_ID_Secondary, nVarType_FK, nScenarioID, sNewVal, sScenDescrip, sElementLabel, nElementID, nOptionVal, CommonUtilities._sDATA_UNDEFINED, bIsInsert);
                                    }
                                    else
                                    {
                                        //SP 8-Mar-2014 Additional special case code per model which can be called to modify required changes
                                        ModifyModelChanges_SpecialCase
                                            (nDV_ID_Secondary, nVarType_FK, nScenarioID, ref sNewVal, sScenDescrip, sElementLabel, nElementID, nOptionVal);

                                        InsertModelValList(nDV_ID_Secondary, nVarType_FK, nScenarioID, sNewVal, sScenDescrip, sElementLabel, nElementID, nOptionVal, CommonUtilities._sDATA_UNDEFINED, bIsInsert);
                                    }

                                    //SP 8-Mar-2014 Additional special case code per model which can be called to add required changes to the model
                                    AdditionalRequiredModelChanges_SpecialCase
                                        (nDV_ID, nVarType_FK, nScenarioID, sNewVal, sScenDescrip, sElementLabel, nElementID, nOptionVal);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //      cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                _log.AddString("Error in ProcessDV_Consequent. Exception: " + ex.Message, Logging._nLogging_Level_1);
                // tell it on the mountain
            }
        }

        // test and destroy
        public virtual int ReturnNum()          //int nCode, double dParam1) //, int nElementID, string sElementLabel, string sOptionList_c_Val, double dAggregateVal = -1)
        {
            return -1;
        }

        //met 4/7/14: override in base class
        //public virtual Dictionary<int, string> 
        public virtual Dictionary<int, string> GetNewModVals_SPECIALCASE(string sElementLabel, double sOptionList_c_Val_HORSESHIT, int[] nCode, double dParam1)   //, int nElementID)         //, double dAggregateVal = -1)
        {
            return null;
        }

        // used for performance parse ; given a dv id, go find what the option was, and then get the option val for the given optionlist
        // Updated 10/3/17 to send back the DV flag by default... Maybe one can have the option to pull either the value or flag.
        // met - set to false so that this is returned like it was before
        private string GetOptionVal_Parse(int nScenarioID, int nDVID_FK, int nOptionID, bool bReturnOptionval=false)
        {
            string sReturn = CommonUtilities._dBAD_DATA.ToString();
            int nOptionVal = GetOptionNumFromDV(nScenarioID, nDVID_FK);
            if (bReturnOptionval)
            {
                return nOptionVal.ToString();
            }
            else
            {
                sReturn = GetOptionVal(nOptionID, nOptionVal);
                return sReturn;
            }

        }

        private int GetOptionNumFromDV(int nScenarioID, int nDVID_FK)
        {
            int nReturn;
            var OptionNo = from ModelChanges in _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.MEV)
                            .Where(x => x._nScenarioID == nScenarioID)
                            .Where(x => x._nRecordID == nDVID_FK)
                           select new
                           {
                               val = ModelChanges._nDV_Option
                           };

            if (OptionNo.Count() == 0)
                return 1;           //assumed default optionno; todo: get the actual default for that DV
            else
            {
                var theVal = OptionNo.FirstOrDefault().val;
                nReturn = Convert.ToInt32(theVal);
                return nReturn;
            }
        }

        //todo : figure out how to do this in LINQ
        private string GetOptionVal(int nOptionID, int nOptionVal)
        {
            string sOptionReturn = "-1; not found";      // returned if for some reason we don't get a good value
            var query =
                from order in _dsEG_OptionVals.Tables[0].AsEnumerable()
                where ((order.Field<int>("OptionID") == nOptionID) && (order.Field<int>("OptionNo") == nOptionVal))
                select new
                {
                    OptionVal = order.Field<string>("val"),
                };
            //  todo : figure out how to return a single value.
            foreach (var Option in query)
            {
                sOptionReturn = Option.OptionVal;
                break;
            }
            return sOptionReturn;
        }


        //SP 19-Dec-2016 - find the option number corresponding to the minimum value
        private int OptionValWithMinRealOptionVal(int nOptionID)
        {
            int sOptionReturn = -1;      // returned if for some reason we don't get a good value
            try
            {
                double dMinVal = _dsEG_OptionVals.Tables[0].AsEnumerable().Where(x => x.Field<int>("OptionID") == nOptionID).Min(r => r.Field<double>("val"));

                //find the option relating to min value
                int nMinOptionNumber = _dsEG_OptionVals.Tables[0].AsEnumerable().Where(x => x.Field<int>("OptionID") == nOptionID && x.Field<double>("val") == dMinVal).Select(x => x.Field<int>("OptionNo")).First();
                return nMinOptionNumber;
            }
            catch (Exception ex)
            {
                return sOptionReturn;
            }
        }


        //returns a previously calculated aggregate based upon a model list
        //sim2: added code from previous, however think this should go to derived class ; non-functional
        public double GetBaselineElementList_Aggregate(int nVarType_FK, int nElementListID, string sKeyPrefix)
        {

            Dictionary<string, double> dictBaselineValSummation = new Dictionary<string, double>();   //sim2- just added so this compiles
            //todo:
            double dReturn; double dVal;
            string sKey = sKeyPrefix + "_" + nElementListID;
            if (dictBaselineValSummation.ContainsKey(sKey))
            {
                dReturn = dictBaselineValSummation[sKey];
            }
            else
            {
                dVal = CommonUtilities._dBAD_DATA;  // Convert.ToDouble(GetSimLinkDetail_FromMem(SimLinkDataType_Major.Network,nElementID,nVarType_FK,_nActiveBaselineScenarioID,nElementID);

                // rmgDIST_GetScenarioTotal_BaselineModelVal(nElementListID, nVarType_FK, 1);
                dictBaselineValSummation.Add(sKey, dVal);
                dReturn = dVal;
            }
            return dReturn;
        }

        //met sim2 lnc

        /*sim2
         * public double DIST_GetScenarioTotal_BaselineModelVal(string sID_Field, string sFieldName, string sTableName, int nProjID, string sQual, int nFieldScalar = -1)
          {
              string sql = "SELECT ElementID_FK FROM qryElementList001_DistribXREF"
                          + " WHERE (((Qualifier)= " + sQual + ") AND ((ProjID_FK)" + nProjID.ToString() + "))";                // WHERE (((ProjID_FK)=@Proj) AND ((Qualifier)=@QUAL));";

              DataSet dsElementList = _dbContext.getDataSetfromSQL(sql);

              string sWhereClause;
              double dTotal = 0; double dVal;

              foreach (DataRow dr in dsElementList.Tables[0].Rows)
              {
                  sWhereClause = "(" + sID_Field + "= " + dr["ElementID_FK"].ToString() + ")";
                  dVal = CommonUtilities._dBAD_DATA;          // Convert.ToDouble(GetNetworkAttribute(sTableName, sFieldName, sWhereClause,));
                  if (nFieldScalar > 0)         //only pass the VarType_FK of the Field SCALAR IFF you mean to use it!!!
                  {
                      dVal = dVal * -1;   // sim2- corrupted this... todo dictFieldScalar[dr["ElementID_FK"].ToString() + "_" + nFieldScalar.ToString()];        //go to the dictionary (already populated) for the scalar field
                  }

                  dTotal = dTotal + dVal;
              }
              return dTotal;
          }

          */

        private string SubstituteTuple(string sVal, string sTuple, int nElementID, int nActiveModelID)
        {
            var dictTuple = new Dictionary<string, string>(); string sReturnVal = "";
            dictTuple = TUPLE_ProcessTuple(sTuple, nElementID, nActiveModelID);
            foreach (KeyValuePair<string, string> pair in dictTuple)
            {
                sVal = sVal.Replace(pair.Key, pair.Value);
            }
            return sVal;
        }

        //     returns a dictionary
        private Dictionary<string, string> TUPLE_ProcessTuple(string sTuple, int nElementID, int nModelTypeID)
        {
            string[] sTuples = sTuple.Split('#');
            var dictTuple = new Dictionary<string, string>();

            for (int i = 0; i < sTuples.Length; i++)
            {
                string[] sVals = sTuples[i].Split(',');
                string sVal = rmgHelper_GetModelElement_ByID(nElementID, Convert.ToInt32(sVals[1]), nModelTypeID);       //UID,modeltype

                dictTuple.Add(sVals[0], sVal);
            }

            return dictTuple;
        }

        // met 4/24/2012: add dFieldScalar. This allows a more complicated 
        // general case is that this is not used.
        //furthermore, not clear what the meaning of this would be for some functions. I only added to Mult/Mult_Inv

        public string GetNewModelVal(string sExistVal, string sOptionVal, string sOperation, string sAdditionalData, bool bSkipIfSame = true, double dFieldScalar = 1.0)
        {
            double d1; double d2; double dOut = -1;
            if (Double.TryParse(sExistVal, out d1) & Double.TryParse(sOptionVal, out d2))   //existing value AND option can be converted to numbers
            {
                double dExistVal = Convert.ToDouble(sExistVal); double dOptionVal = Convert.ToDouble(sOptionVal);

                //SP 4-Aug-2016 TODO use OperationType Enum and test
                switch (sOperation)
                {
                    case ("Add"):
                        dOut = dExistVal + dOptionVal;
                        break;
                    case ("Subtract"):
                        dOut = dExistVal - dOptionVal;
                        break;
                    case ("Multiply"):
                        dOut = dExistVal * dOptionVal * dFieldScalar;
                        break;
                    case ("Mult_Inv"):
                        dOut = dExistVal * (1 - dOptionVal) * dFieldScalar;
                        break;
                    case ("Identity"):
                        dOut = dOptionVal * dFieldScalar;
                        break;
                    case ("Complement"):    //the term, and the procedures will be refined
                        dOut = Convert.ToDouble(sAdditionalData) - Convert.ToDouble(sExistVal);   //defined 5/3/2011 for getting changed area for subareas
                        break;
                }
                if (bSkipIfSame && (dOut.ToString() == sExistVal))
                {
                    return "val_no_change%";        // no real value change, don't insert (As if error) todo: can refine the value sent back if there's reason
                }
                else
                {
                    return dOut.ToString();
                }
            }
            else
            {
                return sOptionVal;          //met 2/28/2012: todo perform better check for whether value has changed. replaced:                return "error%";
            }
        }

        //met 12/31/2012: todo: load a dictionary for this type of thing?
        // add optional keycolumn name to avoid extra lookup on key column  
        public string GetModelElementVal(string sTable, string sFieldName, int nElementID, int nActiveModelTypeID, double dUnitConversion, string sKeyColumnName = "SKIP")
        {
            string sql = ""; //string sKeyColumnName = "";
            if (sKeyColumnName != "SKIP")           //code passes a "valid" KeyColumnID; avoid doing another lookup.
            {

            }
            else
            {
                switch (nActiveModelTypeID)
                {
                    case (1):
                        sql = "select KeyColumn from tlkpSWMMTableDictionary where (TableName=@TableName)";
                        break;
                    case (2):
                        sql = "select KeyColumn from tlkpIWTableDictionary where (TableName=@TableName)";
                        break;
                    case (5):
                        sql = "select KeyColumn from tlkpMouseTableDictionary where (TableName=@TableName)";
                        break;
                    default:
                        sql = "";
                        break;
                }


                if (nActiveModelTypeID == 5)
                {
                    sKeyColumnName = "OBJECTID";
                }
                else
                {

                    List<DBContext_Parameter> lstParam = new List<DBContext_Parameter>();
                    DBContext_Parameter param = new DBContext_Parameter("@TableName", SIM_API_LINKS.DAL.DataTypeSL.STRING, sTable);
                    lstParam.Add(param);
                    //SP 9-Jun-2016 TODO - avoid this call to database and instead keep these values in memory
                    DataSet dsKeyColumn = _dbContext.getDataSetfromSQL(sql, lstParam);
                    sKeyColumnName = dsKeyColumn.Tables[0].Rows[0]["KeyColumn"].ToString();
                }
            }

            //SP 9-Jun-2016 TODO - avoid this call to database and instead keep these values in memory
            string sql_select = "SELECT " + sFieldName + " as ElementVal from " + sTable + " where (" + sKeyColumnName + " = " + nElementID + ")";
            DataSet dsMyDs = _dbContext.getDataSetfromSQL(sql_select);

            // superceded 2/28/2012   if (dUnitConversion != -1)      //don't do anything for -1 (this should capture all strings
            if (CommonUtilities.IsDouble(dsMyDs.Tables[0].Rows[0][0].ToString()))      //perform the unit conversion if we actually have a double
            {
                dsMyDs.Tables[0].Columns["ElementVal"].ReadOnly = false;
                dsMyDs.Tables[0].Rows[0][0] = (Convert.ToDouble(dsMyDs.Tables[0].Rows[0][0].ToString()) * dUnitConversion).ToString();
            }
            return dsMyDs.Tables[0].Rows[0][0].ToString();
            //

        }



        //wraps around "rmgHELPER_getModelElementVal" if you only know the UID
        public string rmgHelper_GetModelElement_ByID(int nElementID, int nFieldID, int nActiveModelTypeID)
        {
            string sql = "";
            switch (nActiveModelTypeID)
            {
                case (1):
                    sql = "SELECT tlkpSWMMFieldDictionary.ID AS FieldID, tlkpSWMMTableDictionary.TableName, tlkpSWMMTableDictionary.KeyColumn, tlkpSWMMFieldDictionary.FieldName, "
                    + "tlkpSWMMFieldDictionary.FieldClass FROM tlkpSWMMFieldDictionary INNER JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID "
                    + "WHERE (((tlkpSWMMFieldDictionary.ID)=" + nFieldID + "));";
                    break;
                case (2):
                    sql = "TODO UPDATE";
                    break;
                case (5):
                    sql = "TODO UPDATE";
                    break;
                default:
                    sql = "";
                    break;
            }

            //SP 9-Jun-2016 TODO - avoid this call to database and instead keep these values in memory
            DataSet ds = _dbContext.getDataSetfromSQL(sql);

            string sVal = GetModelElementVal(ds.Tables[0].Rows[0]["TableName"].ToString(), ds.Tables[0].Rows[0]["FieldName"].ToString(), nElementID, nActiveModelTypeID, 1, ds.Tables[0].Rows[0]["KeyColumn"].ToString());
            return sVal;
        }

        #endregion

        #region ProcessScenario         // process secondary part of alternative- called from derived class

        //generally, following results read the processing steps should be identical
        protected void ProcessScenario_COMMON(int nEvalID, int nScenarioID = -1, int nScenStart = 1, int nScenEnd = 100, string sHDF_TS_Location = "NOTHING")
        {
            int nCurrentLoc = nScenStart;
            bool bDebug = true;
            if ((nCurrentLoc <= CommonUtilities.nScenResultTS_Operations) && (nScenEnd >= CommonUtilities.nScenResultTS_Operations))
            {
                ResultTS_ProcessSecondary(nEvalID, nScenarioID/*, sHDF_TS_Location SP 2-Mar-2017 TS should already be in memory, no need to obtain from HDF5*/);
                nCurrentLoc = CommonUtilities.nScenResultTS_Operations;
            }
            if ((nCurrentLoc <= CommonUtilities.nScenDefineEvents) && (nScenEnd >= CommonUtilities.nScenDefineEvents))
            {
                //pull from rmg db link and fix this

                if (bDebug)
                {
                    int nEvents = _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.Event).Where(x => x._nScenarioID == nScenarioID).Count();
                    _log.AddString(string.Format("{0} events for scenario {1}- before process events", nEvents, nScenarioID), Logging._nLogging_Level_Debug, false, true);
                }
                //string sql = "SELECT TS_EventSummaryDetailID,  ScenarioID_FK, EventSummary_ID, EventDuration, EventBeginPeriod, MaxVal, TotalVal, SubEventThresholdPeriods FROM tblResultTS_EventSummary_Detail where (0>1)";
                ProcessEvents(nScenarioID);
                nCurrentLoc = CommonUtilities.nScenDefineEvents;

                if (bDebug)
                {
                    int nEvents = _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.Event).Where(x => x._nScenarioID == nScenarioID).Count();
                    _log.AddString(string.Format("{0} events for scenario {1}- after process events", nEvents, nScenarioID), Logging._nLogging_Level_Debug, false, true);
                }
            }
            if ((nCurrentLoc <= CommonUtilities.nScenDefineSecondaryEvents) && (nScenEnd >= CommonUtilities.nScenDefineSecondaryEvents))
            {
                ProcessEventsSecondary(nScenarioID);
                nCurrentLoc = CommonUtilities.nScenDefineSecondaryEvents;
            }


            if ((nCurrentLoc <= CommonUtilities.nScenLCSecondaryProcessing) && (nScenEnd >= CommonUtilities.nScenLCSecondaryProcessing))
            {
                if (bDebug)
                {
                    int nEvents = _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.Event).Where(x => x._nScenarioID == nScenarioID).Count();
                    _log.AddString(string.Format("{0} events for scenario {1}- before process performance", nEvents, nScenarioID), Logging._nLogging_Level_Debug, false, true);
                }

                if (_bUseCostingModule)
                {
                    // only update the scenario of costing module and delete costs if processing step 20.
                    // "just in time"....
                    _cost_wrap.UpdateCostingForScenario(nScenarioID);
                }

                Performance_CalcResults(nEvalID, nScenarioID);
                nCurrentLoc = CommonUtilities.nScenLCSecondaryProcessing;
                if (_bIsOptimization)                           //set a value which is pased back to pop member
                    _dOpt_TempObjective = GetObjectiveVal(nScenarioID);
            }

            //SP 15-Feb-2017 Added feature to write Secondary and AUX back to Repo
            //MET moved so doesn't get passed over due to nCurrentLoc
            //SP 13-Oct-2017 CAUTION - added || _bSaveSecondaryAndAuxTS to the conditional statement. May be undesirable
            //SP 13-Oct-2017 Needed to get external data requests and create the .DAT files but also save the secondary and AUX to repo for synthing later - See SWMM Conditional to set _bSaveSecondaryAndAuxTS
            if (((nCurrentLoc <= CommonUtilities.nScenSecondaryAUXResultsTS_Write) && (nScenEnd >= CommonUtilities.nScenSecondaryAUXResultsTS_Write)) || _bSaveSecondaryAndAuxTS)
            {
                if (_bSaveSecondaryAndAuxTS)
                {
                    WriteSecondaryAndAUXTimeSeriesToRepo("1");      //hard coded data id for now
                }
                nCurrentLoc = CommonUtilities.nScenSecondaryAUXResultsTS_Write;
            }


            if ((nCurrentLoc <= CommonUtilities.nScenCommon_WriteOutData) && (nScenEnd >= CommonUtilities.nScenCommon_WriteOutData))
            {
                //SP 1-Mar-2017 Write output data from tblSupportingFileSpec as part of the standard Simlink Process
                WriteOutputData_Grouped(); //TODO Want to move HDF to ExternalData class - need to reassess where this line is called from and how
            }

        }


        /// <summary>
        /// Retrieve the (single) objective value for the requested scenario
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <returns></returns>
        public double GetObjectiveVal(int nScenarioID=-1)
        {
            if (nScenarioID == -1)
                nScenarioID = _nActiveScenarioID;
            string sObjectiveVal = GetSimLinkDetail_FromMem(SimLinkDataType_Major.Performance, _nPerformanceID_Objective, -1, nScenarioID, -1);
            return Convert.ToDouble(sObjectiveVal);

        }

        /// <summary>
        /// Retrieve the first index to begin searching based upon reference event and secondary request information
        /// 
        ///     TODO: consider more complex cases, like when you request 12 periods in front of an event but the event itself goees for 30.. still pad 12?
        ///         for now most literal request is interpreted.
        ///     MET 11/17/16: Updated to more generalize the language.  
        ///     behavior for origin and terminus must BOTH check for start/end array becauase we could be searching a TS backwards or forwards
        /// 
        /// </summary>
        /// <param name="nReferencePeriodAnchor"></param>

        /// <returns></returns>
        /// ApplyOriginOffset(nEventOrigin, nOriginOffset, nReferencePeriodAnchor);   ApplyOriginOffset(nEventOrigin, nOriginOffset, nReferencePeriodAnchor);   
        private int ApplyEventPeriodOffset(int nEventOrigin, int nOffset, int nReferencePeriodAnchor, int nMaxPeriods, bool bIsOrigin = true)
        {
            int nReturn = nEventOrigin + nOffset;
            if (nReturn < 1)
            {
                _log.AddString(string.Format("TS padding request ended before first index for event: {0}. Set to first index; ", nReferencePeriodAnchor), Logging._nLogging_Level_3, false, false);
                nReturn = 1;        // confirm indexed from 1? 
            }
            if (nReturn >= nMaxPeriods)
            {
                _log.AddString(string.Format("TS padding request ended before second index for event: {0}. Set to last index;", nReferencePeriodAnchor), Logging._nLogging_Level_3, false, false);
                nReturn = 1;        // confirm indexed from 1? 
            }
            return nReturn;
        }

        /// <summary>
        ///  Process secondary events
        ///  
        ///  1/15/17: add ability to process only events with totalval = 0. This performs pickups of "missing events" addedd thrrough CreateMissingEvents".
        ///  
        /// </summary>
        /// <param name="nSCenarioID"></param>
        private void ProcessEventsSecondary(int nScenarioID, bool bProcessZeroEventsOnly = false)
        {
            if (_dsSCEN_SecondaryEventDetails != null)
            {
                if(_dsSCEN_SecondaryEventDetails.Tables[0].Rows.Count>0){
                    DataSet dsCurrentEventDetails = _dsSCEN_EventDetails_Empty.Copy();
                    Console.WriteLine("NOTE: Check/verify that events are being deleted correctly. This is very new functionality");
                 //   ClearEventsBeforeInsert(nScenarioID, SimLinkDataType_Major.Event);      // MET 11/6/17: Somehow extra elements are being inserted
                    // need to track down where these excess push come from
                    // in the meantime, this does seem to work.
                    try
                    {
                        foreach (DataRow dr in _dsEG_Event_RequestSecondary.Tables[0].Rows)
                        {
                            int nResultOrEventID_FK = Convert.ToInt32(dr["ResultTS_or_Event_ID_FK"].ToString());
                            int nEventID = Convert.ToInt32(dr["EventSummaryID"].ToString());
                            int nSQN = Convert.ToInt32(dr["sqn"].ToString());
                            bool bRefFromBeginning = Convert.ToBoolean(dr["RefFromBeginning"].ToString());
                            bool bIsHardOrigin = Convert.ToBoolean(dr["IsHardOrigin"].ToString());
                            bool bIsPointVal = Convert.ToBoolean(dr["IsPointVal"].ToString());
                            bool bSearchOriginForward = Convert.ToBoolean(dr["SearchOriginForward"].ToString());
                            bool bSearchTerminusForward = Convert.ToBoolean(dr["SearchTerminusForward"].ToString());
                            int nOriginOffset = Convert.ToInt32(dr["OriginOffset"].ToString());
                            int nTerminusOffset = Convert.ToInt32(dr["TerminusOffset"].ToString());
                            // set the offsets depending on search approach.
                            if (!bSearchOriginForward)
                                nOriginOffset *= -1;                  // 
                            if (!bSearchTerminusForward)
                                nTerminusOffset *= -1;

                            //         int nPadStart = Convert.ToInt32(dr["PeriodsBefore"].ToString());
                            //        int nPadEnd = Convert.ToInt32(dr["PeriodsAfter"].ToString());
                            int nReferenceEventID = Convert.ToInt32(dr["RefEventID"].ToString());
                            double dThresholdINST = Convert.ToDouble(dr["Threshold_Inst"].ToString());     // todo : use the threshold properly (overunder)
                            int nInterEventDuration_SEC = Convert.ToInt32(dr["InterEvent_Threshold"].ToString());  // retrieve in seconds
                            int nInterEventPeriods = Convert.ToInt32(Math.Round(nInterEventDuration_SEC / _tsdResultTS_SIM._nTSIntervalInSeconds, 0));  // todo: better manage non-standard intervals
                            EventNumberCode eventNumCode = (EventNumberCode)(Convert.ToInt32(dr["AssignEventNoCode"].ToString()));
                            int nEventCounter = -1; // default val if OFF
                            if( eventNumCode==EventNumberCode.PRIMARY){
                                nEventCounter = 0;      // even though we are processing secondary event, no reason we couldn't count events here... (though it seems like there would be no reason to do so- mostly take from ref event)  (expect increment to 1 in for loop- 1 indexed for event count)
                            }



                            string sdebug = "";
                            try
                            {
                                string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_or_Event_ID_FK"].ToString(), "SKIP", "SKIP");
                                sdebug = sGroupID;
                                double[,] dVals = GetTS_FromMemory(sGroupID);
                                int nTS_Duration = dVals.GetLength(0);
                                // retrieve all the event details 
                                DataRow[] drEvents = _dsSCEN_EventDetails.Tables[0].Select("EventSummary_ID = " + nReferenceEventID.ToString());
                          
                                // bojangles- hard-coded for now to sip 4
                               /* if (bProcessZeroEventsOnly && nSQN!=4)     
                                {
                                    drEvents = _dsSCEN_EventDetails.Tables[0].Select("EventSummary_ID = " + nReferenceEventID.ToString() + " and MaxVal = " + CommonUtilities._dAlmostZero.ToString());
                                }*/
                                foreach (DataRow drEvent in drEvents)
                                {
                                    int nReferencePeriodAnchor = Convert.ToInt32(drEvent["EventBeginPeriod"].ToString());
                                    int nRefPeriodCount = Convert.ToInt32(drEvent["EventDuration"].ToString());
                                    int nRefEventNumber = Convert.ToInt32(drEvent["EventNo"].ToString());       // event code of reference event..

                                    switch (eventNumCode){
                                        case EventNumberCode.PRIMARY:
                                            nEventCounter++;
                                            break;
                                        case EventNumberCode.USE_REF:
                                            nEventCounter = nRefEventNumber;
                                            break;
                                        // skip OFF
                                       
                                    }

                                    if (!bRefFromBeginning)
                                        nReferencePeriodAnchor += nRefPeriodCount;      // refrence from "end" of event

                                    // search backwards against the timeseries to find start of the event
                                    int nEventOrigin = nReferencePeriodAnchor;
                                    if (bIsHardOrigin)       // origin is based solely on the reference point 
                                    {

                                    }
                                    else
                                    {
                                        //question: include option to apply origin offset BEFORE we find the event origin in this manner?
                                        // do if needed in future, not proactively. 
                                        nEventOrigin = TimeSeries.ScanForEventTerminus(ref dVals, nReferencePeriodAnchor, dThresholdINST, nInterEventPeriods, bSearchOriginForward);
                                    }

                                    // TODO (12/18/16): how to handle IsHardTerminus? Not yet handled.?

                                    // adjust for padding requests
                                    nEventOrigin = ApplyEventPeriodOffset(nEventOrigin, nOriginOffset, nReferencePeriodAnchor, nTS_Duration, true);                              // nEventOrigin = GetStartRow(nEventOrigin, nReferencePeriodAnchor, nRefPeriodCount, bRefFromBeginning,nPadStart, nPadEnd);

                                    int nEventCode = Convert.ToInt32(dr["EventFunctionID"]);            //met 12/23/14
                                    Event_FunctionOnTimeSeries ts = (Event_FunctionOnTimeSeries)Enum.Parse(typeof(Event_FunctionOnTimeSeries), nEventCode.ToString());
                                    switch (ts)         //todo: enum
                                    {
                                        case Event_FunctionOnTimeSeries.ThresholdAndDurationCalculations:  // threshold based definition - store all event details for the scenario in _dsSCEN_EventDetails
                                            //SP 14-Jun-2016 - store all events and the the current event details
                                            // todo: does this need to suport a event terminus padding? neglected for now.
                                            TimeSeries.EventDefine(nScenarioID, dVals, dr, _tsdResultTS_SIM, ref _dsSCEN_EventDetails, ref dsCurrentEventDetails, nEventOrigin, true,false,nEventCounter);
                                            break;
                                        case Event_FunctionOnTimeSeries.DefinedDuration:
                                            int nEventTerminus = ApplyEventPeriodOffset(nEventOrigin, nTerminusOffset, nReferencePeriodAnchor, nTS_Duration, false);
                                            if (nEventOrigin > nEventTerminus)          //  at this point, swap, so an event starts before it ends
                                            {                                           // not 100% sure this is what we want, but seems right because time only flows one direction (for our purposes)
                                                int nTemp = nEventTerminus;
                                                nEventTerminus = nEventOrigin;
                                                nEventOrigin = nTemp;
                                            }
                                            int nDurationPeriods = nEventTerminus - nEventOrigin;
                                            // call overloaded function
                                            TimeSeries.EventDefine(nScenarioID, dVals, nEventOrigin, nDurationPeriods, nResultOrEventID_FK, nEventID, ref _dsSCEN_EventDetails, ref dsCurrentEventDetails);
                                            break;
                                        case Event_FunctionOnTimeSeries.AverageValAndPercentile:        // Todo! define range of event requests
                                            // todo: DO NOT repeat code from previous event definition. Consolidate before switch
                                            nEventTerminus = ApplyEventPeriodOffset(nEventOrigin, nTerminusOffset, nReferencePeriodAnchor, nTS_Duration, false);
                                            if (nEventOrigin > nEventTerminus)          //  at this point, swap, so an event starts before it ends
                                            {                                           // not 100% sure this is what we want, but seems right because time only flows one direction (for our purposes)
                                                int nTemp = nEventTerminus;
                                                nEventTerminus = nEventOrigin;
                                                nEventOrigin = nTemp;
                                            }
                                            nDurationPeriods = nEventTerminus - nEventOrigin;
                                            //  2/2/17: added ability to set event counter. not 100% sure which EventDefine was being called before!!
                                            TimeSeries.EventDefine(nScenarioID, dVals, nEventOrigin, nDurationPeriods, nResultOrEventID_FK, nEventID, ref _dsSCEN_EventDetails, ref dsCurrentEventDetails, true,true,nEventCounter);
                                            break;
                                        default:
                                            _log.AddString(string.Format("Not a valid option for EventFunctionID in tblResultTS_EventSummary for Event {0}",
                                                dr["EventSummaryID"].ToString()), Logging._nLogging_Level_1);
                                            break;
                                    }
                                    AddEventsToListDetail(dsCurrentEventDetails, nScenarioID);            //have event available in list
                                    dsCurrentEventDetails.Tables[0].Clear();
                                }
                                }
                            catch (Exception ex)
                            {
                                _log.AddString("Result TS not found; error processing Event: " + sdebug, Logging._nLogging_Level_1);
                                throw; //SP 5-Aug-2016 throw error to processscenario to ensure this is not overlooked
                                //todo: log the issue
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.AddString("tblEventSummary secondary events is null. Check tblResultTS_EventSummary columns. Error message:" + ex.Message, Logging._nLogging_Level_1);
                        throw; //SP 5-Aug-2016 throw error to processscenario to ensure this is not overlooked                    
                    }
                }
            }
            else
            {
                // log the issue
            }
        }

        // enum added for defining how to track numbered events which one would like to reassociate in the future.
        public enum EventNumberCode
        {
            OFF = -1,
            USE_REF,
            PRIMARY
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nSCenarioID"></param>
        /// 
        //todo: event request should be processed ONCE at beginning! not on every iteration.
        private void ProcessEvents(int nSCenarioID)
        {
            if (_dsSCEN_EventDetails_Empty != null)
            {
                //SP 9-Jun-2016
                //string sql = "SELECT TS_EventSummaryDetailID,  ScenarioID_FK, EventSummary_ID, EventDuration, EventBeginPeriod, MaxVal, TotalVal, SubEventThresholdPeriods FROM tblResultTS_EventSummary_Detail where (0>1)";
                //DataSet dsEvents = _dbContext.getDataSetfromSQL(sql);
                DataSet dsCurrentEventDetails = _dsSCEN_EventDetails_Empty.Copy();
                ClearEventsBeforeInsert(nSCenarioID, SimLinkDataType_Major.Event);      // MET 11/6/17: Somehow extra elements are being inserted
                                                                                        // need to track down where these excess push come from
                                                                                          // in the meantime, this does seem to work.

                //todo: it would be more efficient to perform all the events on a single TS
                //this may not be a typical case; for coding ease, performed row by row at present

                //bojangles: dtEvalSim could differ by result; so far it hasn't; todo, support this

                foreach (DataRow dr in _dsEG_Event_Request.Tables[0].Rows)
                {
                    string sdebug = "";
                    try
                    {
                        string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_ID"].ToString(), "SKIP", "SKIP");
                        sdebug = sGroupID;
                        double[,] dVals = GetTS_FromMemory(sGroupID);
                        int nEventCode = Convert.ToInt32(dr["EventFunctionID"]);            //met 12/23/14
                        EventNumberCode eventNumberCode = (EventNumberCode)Convert.ToInt32(dr["AssignEventNoCode"]);
                        bool bCountEvents = (eventNumberCode == EventNumberCode.PRIMARY);       //

                        Event_FunctionOnTimeSeries ts = (Event_FunctionOnTimeSeries)Enum.Parse(typeof(Event_FunctionOnTimeSeries), nEventCode.ToString());
                        //SP 18-Feb-2016 Converted to enum type Event_FunctionOnTimeSeries
                        switch (ts)         //todo: enum
                        {
                            case Event_FunctionOnTimeSeries.ThresholdAndDurationCalculations:  // threshold based definition - store all event details for the scenario in _dsSCEN_EventDetails
                                //SP 14-Jun-2016 - store all events and the the current event details
                                TimeSeries.EventDefine(nSCenarioID, dVals, dr, _tsdResultTS_SIM, ref _dsSCEN_EventDetails, ref dsCurrentEventDetails, -1, false, bCountEvents);
                                break;


                            default:
                                _log.AddString(string.Format("Not a valid option for EventFunctionID in tblResultTS_EventSummary for Event {0}",
                                    dr["EventSummary_ID"].ToString()), Logging._nLogging_Level_1);
                                break;
                            //SP 22-Feb-2016 - this is now only an option when a performance metric has a TS as a linked record 
                            /* double dVal = TimeSeries.SummaryStatForTS(dVals, nEventCode);
                             int nEventID = Convert.ToInt32(dr["EventSummaryID"].ToString());

                             TimeSeries.tsInsertTS_EventDetail(ref dsEvents, nSCenarioID, nEventID, dVal);    //add new record to ds
                             break;*/
                        }

                        //SP 14-Jun-2016 save back to DB at the very end of processing scenar
                        /*if (_IntermediateStorageSpecification._bResultEventSummary)
                        {
                            _dbContext.InsertOrUpdateDBByDataset(true, dsEvents, _sSQL_InsertEventDetailVals, true, false);
                        }*/
                        if (eventNumberCode == EventNumberCode.USE_REF)
                        {
                            int nRefID = Convert.ToInt32(dr["RefEventID"]);
                            int nStartOffset = Convert.ToInt32(dr["RefPrimaryEvent_StartOffset"]); ;
                            int nEndOffset = Convert.ToInt32(dr["RefPrimaryEvent_EndOffset"]); ;
                            SetEventNumberFromPrimary(Convert.ToInt32(dr["EventSummaryID"].ToString()), nRefID, nStartOffset, nEndOffset);
                        }

                        AddEventsToListDetail(dsCurrentEventDetails, nSCenarioID);            //have event available in list, 
                        dsCurrentEventDetails.Tables[0].Clear();
                    }
                    catch (Exception ex)
                    {
                        _log.AddString("Result TS not found; error processing Event: " + sdebug, Logging._nLogging_Level_1);
                        throw; //SP 5-Aug-2016 throw error to processscenario to ensure this is not overlooked
                        //todo: log the issue
                    }
                }
            }
            else
            {
                // log the issue
            }
        }

        /// <summary>
        /// Associate primary event with another primary event....
        /// </summary>
        /// <param name="dsCurrentEventDetails"></param>
        /// <param name="nRefID"></param>
        private void SetEventNumberFromPrimary(int nTargetID, int nRefID, int nStartPeriodOffset = 0, int nMaxStartPeriodDiffAllowed=-1)
        {
            int nCurrentRefIndex = 0;
     //       int nStartPeriodOffset = 0;      // 0 indicates ref event with same start index as main event- contained.  -1 would only start at the 
     //       int nMaxStartPeriodDiffAllowed = -6;     //parameterize?
            DataRow[] drRef = _dsSCEN_EventDetails.Tables[0].Select("EventSummary_ID = " + nRefID);
            int nMaxSearchIndex = drRef.Count() - 1;
            foreach (DataRow dr in _dsSCEN_EventDetails.Tables[0].Select("EventSummary_ID = " + nTargetID))
            {
                int nTargetStartPeriod = Convert.ToInt32(dr["EventBeginPeriod"]);
                bool bFound = false;
                while (!bFound && nCurrentRefIndex<=nMaxSearchIndex)
                {
                    int nRefStartPeriod = Convert.ToInt32(drRef[nCurrentRefIndex]["EventBeginPeriod"].ToString());
                    if ((nRefStartPeriod >= nTargetStartPeriod + nMaxStartPeriodDiffAllowed) && (nRefStartPeriod <= nTargetStartPeriod + nStartPeriodOffset))
                    {
                        // potentially valid event
                        // but possible that there is another closer event...
                        if (nCurrentRefIndex + 1 < nMaxSearchIndex)     // check
                        {        
                            int nAdvance = 1;
                            nRefStartPeriod = Convert.ToInt32(drRef[nCurrentRefIndex + nAdvance]["EventBeginPeriod"].ToString());
                            while (nRefStartPeriod <= nTargetStartPeriod + nStartPeriodOffset && (nCurrentRefIndex + nAdvance < nMaxSearchIndex))
                            {
                                nAdvance++;
                                nRefStartPeriod = Convert.ToInt32(drRef[nCurrentRefIndex + nAdvance]["EventBeginPeriod"].ToString());
                            }    
                            // decrement one and assign refstartperiod
                            nCurrentRefIndex = nCurrentRefIndex + nAdvance - 1;
                            nRefStartPeriod = Convert.ToInt32(drRef[nCurrentRefIndex]["EventBeginPeriod"].ToString());
                        }


                        dr["EventNo"] = drRef[nCurrentRefIndex]["EventNo"].ToString();    // set the event no as match
                        bFound = true;
                        nCurrentRefIndex++;
                    }
                    else if (nRefStartPeriod > nTargetStartPeriod)
                    {   
                        dr["EventNo"] = -2;     // set symbol that not found (above index so will
                        bFound = true;
                    }
                    else
                    {
                        nCurrentRefIndex++;
                    }
                }
                if(nMaxSearchIndex==nCurrentRefIndex)  //no more places to search
                    break;
            }
        }


        // Create the header needed for the concatenated string
        private int HELPER_CreateEventHeader(out string sHeader, int nCountSecondary)
        {
            int nOutputFields = 5;
            int nEventPrimarFields = nOutputFields + 2;   //element label and scenario   TODO: Result label?

            string[] sVals = new string[] { "EventSummarID","BeginPeriod", "Periods", "MaxVal", "TotalVal" };
            sHeader = "ScenarioID,";        // removed element_label
            sHeader = sHeader + string.Join(",", sVals) + ",";
            // create the headers for the core events (always  a core event)
            int nAppend = 1;
            for (int i = 0; i < nCountSecondary; i++)
            {
                string[] sValsMOD = new List<string>(sVals).ToArray();  // create shallow copy
                for (int j = 0; j < sValsMOD.Count(); j++)
                {
                    sValsMOD[j] = sValsMOD[j] + "_" + nAppend.ToString();
                }
                sHeader += string.Join(",", sValsMOD) + ",";
                nAppend++;
            }
            return nCountSecondary * nOutputFields + nEventPrimarFields;            //total number of columns
        }

        /// <summary>
        /// Returns the secondary events associated with a given primary event.
        /// When truly a secondary event, this is no problem.
        /// However, events may be tertiary or beyond, and so the primary event ID is not useful for linking
        /// In this case, it loops through the sqn in descending order and accepts the first hit.
        ///     
        /// may only work for up to tertiary at this point.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        private DataRow[] GetSecondaryEvents(int[,] nEventSummary,  int nEventPrimaryID, int nRowCounter,  int nSQN_Active, int nCounter)
        {
            DataRow[] drReturn = _dsEG_Event_RequestSecondary.Tables[0].Select("RefEventID = " + nEventPrimaryID.ToString() + " and sqn = " + nSQN_Active.ToString());
            if (drReturn.Count() > 0)
                return drReturn;
            else
            {
                nCounter--;
                while (nCounter >= 0)
                {

                    int nEventTargetID_Seconary = nEventSummary[nRowCounter-1, nCounter];     // this is a "secondary" (at least, non primary) event
                    drReturn = _dsEG_Event_RequestSecondary.Tables[0].Select("RefEventID = " + nEventTargetID_Seconary.ToString() + " and sqn = " + nSQN_Active.ToString());
                    if (drReturn.Count() > 0)
                        return drReturn;

                    nCounter--;
                }
            }
            _log.AddString(string.Format("No secondary event was foung for sqn {0} and EventID {1}",nSQN_Active,nEventPrimaryID),Logging._nLogging_Level_2, false, true);
            // return the empty search that was alraeady performed; this will at least allow code progression
            return _dsEG_Event_RequestSecondary.Tables[0].Select("RefEventID = " + nEventPrimaryID.ToString() + " and sqn = " + nSQN_Active.ToString());
        }

        /// <summary>
        /// Create a string from an event history
        /// </summary>
        /// <param name="drEvent"></param>
        /// <param name="bIsPrimary"></param>
        /// <returns></returns>
        private string HELPER_GetVals(DataRow drEvent, bool bIsPrimary)
        {
            string[] sVals = new string[] { "ScenarioID_FK", "EventSummary_ID", "EventBeginPeriod", "EventDuration", "MaxVal", "TotalVal" };        // rm elemen_label because wasn't avialable
            int nIndexStart = 0;
            if (!bIsPrimary)
            {
                nIndexStart = 1;
            }
            string sReturn = "";
            for (int i = nIndexStart; i < sVals.Length; i++)
            {
                sReturn = sReturn + drEvent[sVals[i]].ToString() + ",";
            }
            return sReturn;
        }

        /// <summary>
        /// Take primary event, and then add additional row-sets for associated secondary events
        /// PHASE 1: Require same number of primary secondary events
        /// PHASE 2: use matching to make sure correct events are connected   (NOT DONE YET
        /// </summary>
        /// <param name="nScenarioList"></param>
        /// <param name="nSQN_Exclude"></param>
        /// <returns></returns>
        public List<string> ConcatenateEvents(int[] nScenarioList, int[] nSQN_Exclude=null){

            string sHeader ="";
            Dictionary<int, int> dictSQNtoIndex = new Dictionary<int, int>(){       // todo : create function to iterate over distinct sqn and create
                {2,1},
                {3,2},
                {4,3}
            };
            int nSecondaryCount = dictSQNtoIndex.Count;
            int nRecordCount = _dsEG_Event_Request.Tables[0].Rows.Count;
            int nPrimaryEvents = _dsSCEN_EventDetails.Tables[0].Select("ScenarioID_FK = " + nScenarioList[0].ToString()).Count();    // number of PRIMARY events for the first scenario

            int[,] arrEventSummaryIDs = new int[nPrimaryEvents, nSecondaryCount + 1];               // track the 

            List<string> lstReturn = new List<string>();
            int nCol = HELPER_CreateEventHeader(out sHeader, dictSQNtoIndex.Count);      //create header and get count of cols
            lstReturn.Add(sHeader);
            int nPrimaryEventCounter = 1;               
    //        int nPrimaryEventSummaryCounter = 0;        // indexed once per primary event
            foreach (int nScenario in nScenarioList){
                foreach (DataRow drEventRequest in _dsEG_Event_Request.Tables[0].Rows)
                {
                    int nEventPrimaryID = Convert.ToInt32(drEventRequest["EventSummaryID"].ToString());     // note the field is named differently in the master and child table: todo: standardie?
                    DataRow[] drPrimary = _dsSCEN_EventDetails.Tables[0].Select("ScenarioID_FK = " + nScenario.ToString() + " and EventSummary_ID = " + nEventPrimaryID,"EventBeginPeriod ASC"); //      .OrderBy("EventBeginPeriod");
                    int nLastPrimaryCounter = nPrimaryEventCounter;
                    
                    foreach (DataRow drEventPrimary in drPrimary)
                    {      // iterate over the specific events associated with the event request
                        string sVal = HELPER_GetVals(drEventPrimary, true);
                        lstReturn.Add(sVal);
                        arrEventSummaryIDs[nPrimaryEventCounter-1, 0] = nEventPrimaryID;              // store the primary ID     (MET 1/16/17: decrement by 1
                        nPrimaryEventCounter++;
                    }

                    // now handle secondary events   - for now assuming in same order and correct count
                    int nSecondaryCounter = 1;
                    foreach (KeyValuePair<int,int> kvp in dictSQNtoIndex)
                    {
                        int nCounter = nLastPrimaryCounter;         //set to the start location for that event
                        
                        int nSQN_Active = kvp.Key;
                        // step 1: get the event id 

                        DataRow[] drSecondaryEventRequest = GetSecondaryEvents(arrEventSummaryIDs, nEventPrimaryID, nCounter, nSQN_Active, nSecondaryCounter);              
                        if (drSecondaryEventRequest.Count() > 0)
                        {
                            int nEventSummary_RefID = Convert.ToInt32(drSecondaryEventRequest[0]["EventSummaryID"].ToString());       // should be only one row returned  TODO: harden
                            arrEventSummaryIDs[nCounter-1, nSecondaryCounter] = nEventSummary_RefID;      // add the secondary ID


                            DataRow[] drSecondary = _dsSCEN_SecondaryEventDetails.Tables[0].Select("ScenarioID_FK = " + nScenario.ToString() + " and EventSummary_ID = " + nEventSummary_RefID.ToString(), "EventBeginPeriod ASC");    //.OrderBy("EventBeginPeriod");
                            foreach (DataRow drEventSecondary in drSecondary)
                            {      //
                                string sValSecondary = HELPER_GetVals(drEventSecondary, false);
                                lstReturn[nCounter] = lstReturn[nCounter] + sValSecondary;  // add the secondary event string to what is already there 
                                nCounter ++;
                            }
                        }
                        else
                        {
                            // no associated events were found
                            arrEventSummaryIDs[nCounter-1, nSecondaryCounter] = -1;       //placeholder 
                        }
                        nSecondaryCounter++;        
                    }
                }
             }
            return lstReturn;
        }

        private int HelperGetSearchVal(DataRow[] drCompare, int nEventCounter, int nLargestIndex)
        {
            int nReturn = nLargestIndex + 1;
            if (nEventCounter < drCompare.Count())
            {
                nReturn = Convert.ToInt32(drCompare[nEventCounter]["EventBeginPeriod"]);
            }
            return nReturn;
        }

        /// <summary>
        /// Iterates over primary events and looks for "missing" events....
        /// This is important for statistical analysis, because we don't want to only see when events did occur, but when they did not.
        /// Motivated by using rainfall as ref- and then if we do not have a match, inserting such a "zero" event.
        /// 
        /// 
        /// </summary>
        /// <param name="nID_EventReference"></param>
        /// <param name="nOffsetBegin"></param>
        public void CreateMissingEvents(int nScenarioID, int nID_EventReference, int nSQN, int nOffsetBegin = -1,  double dTotalVal = 0, bool bCreateSecondary = true)
        {
            DataRow[] drPrimaryReference = _dsSCEN_EventDetails.Tables[0].Select("ScenarioID_FK = " + nScenarioID.ToString() + " and EventSummary_ID = " + nID_EventReference); // 
            DataSet dsNothing = new DataSet();
            int nCounter = 1;
            double dMaxVal = CommonUtilities._dAlmostZero; // this value is used later to know that this was a s
            int nLargestIndex = Convert.ToInt32(drPrimaryReference[drPrimaryReference.Count()-1]["EventBeginPeriod"]);        // this is the last recorded start date of the biggest event.

            foreach (DataRow drEventToProcess in _dsEG_Event_RequestSecondary.Tables[0].Select("sqn = " + nSQN))
            {
                
                int nEventCounter = 0;
                int nEventID = Convert.ToInt32(drEventToProcess["EventSummaryID"]);         // get the all the events
                int nPrimaryEventIDforActiveEvent = Convert.ToInt32(drEventToProcess["RefEventID"]); 
                _logInitEG.AddString(string.Format("Begin processing event {0} of {1}. Event {2}", nCounter, drPrimaryReference.Count(), nEventID), Logging._nLogging_Level_3, true, true);
                DataRow[] drSpecificEvent = _dsSCEN_EventDetails.Tables[0].Select("ScenarioID_FK = " + nScenarioID.ToString() + " and EventSummary_ID = " + nEventID); // 
                int nTargetStart = HelperGetSearchVal(drSpecificEvent, nEventCounter, nLargestIndex);
                
                foreach (DataRow drRefEvent in drPrimaryReference)      // iterate over the specific event you are comparing to (the rainfall)
                {

                    int nBeginPeriod = Convert.ToInt32(drRefEvent["EventBeginPeriod"]);
                    nBeginPeriod += nOffsetBegin;
                    double dTotalVal_IND = Convert.ToDouble(drRefEvent["TotalVal"]);                        // these values are used for inserting the relevant "matched" event
                    double dMaxVal_IND = Convert.ToDouble(drRefEvent["MaxVal"]);
                    int nDuration_IND = Convert.ToInt32(drRefEvent["EventDuration"]);
                    int nSubEvent_IND = Convert.ToInt32(drRefEvent["SubEventThresholdPeriods"]);
                    if (nTargetStart>nBeginPeriod){
                        // enter the so-called PRIMARY evnt (The one that was NOT picked up by the original event processing and should have value of zero.   
                        TimeSeries.tsInsertTS_EventDetail(ref _dsSCEN_EventDetails, ref dsNothing, nScenarioID, nPrimaryEventIDforActiveEvent, nBeginPeriod,
                        0, 0, 0, dMaxVal, dTotalVal, false);
//
//                        TimeSeries.tsInsertTS_EventDetail(ref _dsSCEN_SecondaryEventDetails, ref dsNothing, nScenarioID, nEventID, nBeginPeriod,
 //                              nDuration_IND, 0, nSubEvent_IND, dMaxVal_IND, dTotalVal_IND, false);
                    }
                    else{   // the event start should match if not meeting above criteria
                        nEventCounter++;
                        nTargetStart = HelperGetSearchVal(drSpecificEvent, nEventCounter, nLargestIndex);
                    }
                }
                nCounter++;
            }
            if (true)      //bCreateSecondary)
            {
                ProcessEventsSecondary(nScenarioID, true);          // create associated seconary events
            }
            _logInitEG.AddString("Begin write missing events to db", Logging._nLogging_Level_3, true, true);
            WriteResultsToDB(nScenarioID);
            _logInitEG.AddString("End write missing events to db", Logging._nLogging_Level_3, true, true);

        }



        //todo: parameterize the event type (totalval, maxval etc... for now assumes totalval
        private void AddEventsToListDetail(DataSet ds, int nSCenarioID)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string sVal = dr["TotalVal"].ToString();
                int nRecordID = Convert.ToInt32(dr["EventSummary_ID"].ToString());
                _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.Event, sVal, nRecordID, -1, nSCenarioID));  //add to single in-memory storage repo
            }
        }

        private void ClearEventsBeforeInsert(int nScenarioID, SimLinkDataType_Major slType)
        {
            _lstSimLinkDetail.RemoveAll(x => x._nScenarioID == nScenarioID && x._slDataType == slType);

        }

        #endregion
        #region Results

        protected void ResultsSummary_WriteToRepo(int nScenarioID)
        {
            _dbContext.InsertOrUpdateDBByDataset(true, _dsSCEN_ResultSummary, _sSQL_InsertResultSummary, true, false);
        }


        #endregion

        /// <summary>
        /// Function sets the sim start and end times, along with ability to set for reporting.
        /// The derived class has option to create the model changes at this time, but sould call base class to execute the model
        /// </summary>
        /// <param name="bCreateModelChanges"></param>
        /// <param name="dtSimStart"></param>
        /// <param name="dtSimEnd"></param>
        /// <param name="nTS_Interval_Sec"></param>
        /// <param name="dtRptStart"></param>
        /// <param name="dtRptEnd"></param>
        /// <param name="nInterval_Sec_rpt"></param>
        public virtual void SetSimTimeSeries(bool bCreateModelChanges, DateTime dtSimStart, DateTime dtSimEnd, int nTS_Interval_Sec, DateTime dtRptStart = default(DateTime), DateTime dtRptEnd = default(DateTime), int nInterval_Sec_rpt=-1)
        {
            _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtSimStart, IntervalType.Second, nTS_Interval_Sec, dtSimEnd);
            if (dtRptStart == default(DateTime))
                dtRptStart = dtSimStart;
            if (dtRptEnd == default(DateTime))
                dtRptEnd = dtSimEnd;
            _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtRptStart, IntervalType.Second, nTS_Interval_Sec, dtRptEnd);
        }

        //Function to allow processing time series data
        public virtual void PreProcessTimeseriesData(TimeSeries.TimeSeriesDetail dtFirstSim, DateTime dtSimStart)
        {

 // not sure why this is called here Bojangles MET. Was leading to infinite loop for proper secondary TS           ResultTS_ProcessSecondary(_nActiveEvalID, _nActiveScenarioID, /*_hdf5._sHDF_FileName, SP 2-Mar-2017 TS should be in memory - not using HDF5*/ true);  // perform any secondary operations on ResultTS

            //ResultTS_ProcessSecondary(_nActiveEvalID, _nActiveScenarioID, /*_hdf5._sHDF_FileName, SP 2-Mar-2017 TS should be in memory - not using HDF5*/ true);  // perform any secondary operations on ResultTS


        }
        /// <summary>
        /// Write timeseries data to HDF5
        /// 11/23/16: made virtual/override
        ///     TODO
        ///         -implement with EPANET
        ///         - consistency on hdf5 opening
        ///         - better specificaion of which TS to write  (all, primary, aux... ?
        ///         
        ///   Note: file input/output stored on this member      
        /// </summary>
        /// <param name="sDatasetLabel"></param>
        public virtual void WriteTimeSeriesToRepo(RetrieveCode[] rcSaveTypes, string sDatasetLabel = "1")
        {
            //SP 15-feb-2017 this now contains all the TS (primary, secondary and AUX)
            //int nTSCount = _dsEG_ResultTS_Request.Tables[0].Rows.Count;    // dr.Count();          //only write out "primary" TS... secondary 

            if (!_hdf5._bOutputArrayFormat)
            {
                //met10/10/16:   this is the standard case..
                foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Rows) //SP 15-Feb-2017
                {
                    //Check the TS type
                    if (rcSaveTypes.Contains((RetrieveCode)(Convert.ToInt32(dr["RetrieveCode"].ToString()))))
                    {
                        //for (int i = 0; i < nTSCount; i++)
                        //{
                        if (true)  //dr["ElementIndex"].ToString() != "-1") /*_dsEG_ResultTS_Request.Tables[0].Rows[i]["ElementIndex"] != "-1"*/ //SP 15-Feb-2017 TODO Use a different filter, maybe Element Label = -1
                        {
                            try
                            {
                                //SP 15-Feb-2017 Get index from the datarow
                                int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())];
                                string sGroup = "TS_" + dr["ResultTS_ID"].ToString();       //note:  _sTS_GroupID[nIndex] not ordered the same as _dict
                                _hdf5.hdfWriteDataSeries(_dResultTS_Vals[nIndex], sGroup, sDatasetLabel);   //  met 5/4/17   _sTS_GroupID[nIndex], sDatasetLabel);
                            }
                            catch (Exception ex)
                            {
                                _log.AddString("Error with ts record: " + dr["resultts_id"].ToString(), Logging._nLogging_Level_1, false, true);
                            }
                        }
                    }
                }
            }
            else
            {
                double[,] dArr = TimeSeries.Get2DArrayFromJagged(_dResultTS_Vals);
                _hdf5.hdfWriteDataSeries(dArr, hdf5_wrap._sGroupName_ArrayFormat, sDatasetLabel);          //met 10/11/16: allow to send the dataset
            }
        }

        //Overloaded function for WriteTimeSeriesToRepo to allow a default primary RetrieveCode array type 
        public virtual void WriteTimeSeriesToRepo(string sDatasetLabel = "1")
        {
            WriteTimeSeriesToRepo(new[] { RetrieveCode.Primary }, sDatasetLabel);
        }


        //SP 15-Feb-2017 - create a virtual function that can be overriden
        //MET 5/25/17: believe there is no need for override; removed SWMM version of function
        protected virtual void WriteSecondaryAndAUXTimeSeriesToRepo(string sDatasetLabel = "1")
        {
            _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
            WriteTimeSeriesToRepo(new[] { RetrieveCode.Secondary, RetrieveCode.Aux, RetrieveCode.AuxEG }, sDatasetLabel);
            _hdf5.hdfClose();
        }

        /// <summary>
        /// Writes secondary TS to HDF5 file.
        /// This is useful because it saves the data imported from external data sources.
        /// </summary>
        /// <param name="sDatasetLabel"></param>
        //SP 15-Feb-2017 - Should now all be writtten by default as the entire _dsEG_ResultTS_Request (containing primary, secondary and AUX) is written to HDF
        //This routine is now no longer required 
        /*public void WriteSecondaryAndAuxTS_ToRepo(string sDatasetLabel = "1")
        {
            //int nPrimaryTSCount = _dsEG_ResultTS_Request.Tables[0].Rows.Count;    // dr.Count();          //only write out "primary" TS... secondary 
            int nTotalTSCount = _dsEG_ResultTS_Request.Tables[0].Rows.Count; //SP 15-Feb-2017 Now retrieving all Secondary TS from single dataset

            _hdf5.hdfCheckOrCreateH5(_hdf5._sHDF_FileName);
            _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);

            if (!_hdf5._bOutputArrayFormat)
            {                       //met10/10/16:   this is the standard case..
                for (int i = nPrimaryTSCount; i < nTotalTSCount; i++)                   
                {
                    if (_dsEG_TS_Combined_Request.Tables[0].Rows[i]["ElementIndex"] != "-1")
                    {
                        try
                        {
                            _hdf5.hdfWriteDataSeries(_dResultTS_Vals[i], _sTS_GroupID[i], sDatasetLabel);
                        }
                        catch (Exception ex)
                        {
                            _log.AddString("Error with ts record: " + _dsEG_ResultTS_Request.Tables[0].Rows[i]["resultts_id"].ToString(), Logging._nLogging_Level_1, false, true);
                        }
                 
                    }
                }
            }


            else
            {
                double[,] dArr = TimeSeries.Get2DArrayFromJagged(_dResultTS_Vals);
                _hdf5.hdfWriteDataSeries(dArr, hdf5_wrap._sGroupName_ArrayFormat, sDatasetLabel);          //met 10/11/16: allow to send the dataset
            }

            _hdf5.hdfClose();

        }*/


        /// <summary>
        /// Insert a result detail
        /// Moved from swmm to simlink to support numerous wrappers  4/13/16
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <param name="nResultID_FK"></param>
        /// <param name="nElementID"></param>
        /// <param name="sCurrentElementName"></param>
        /// <param name="dVal"></param>
        protected void ResultSummaryHelper_AddValToDS(int nScenarioID, int nResultID_FK, int nElementID, string sCurrentElementName, double dVal, int nVarType_FK)
        {
            _dsSCEN_ResultSummary.Tables[0].Rows.Add();
            int nCurrentIndex = _dsSCEN_ResultSummary.Tables[0].Rows.Count - 1;
            _dsSCEN_ResultSummary.Tables[0].Rows[nCurrentIndex]["Result_ID_FK"] = nResultID_FK;
            _dsSCEN_ResultSummary.Tables[0].Rows[nCurrentIndex]["ScenarioID_FK"] = nScenarioID;
            _dsSCEN_ResultSummary.Tables[0].Rows[nCurrentIndex]["val"] = dVal;
            _dsSCEN_ResultSummary.Tables[0].Rows[nCurrentIndex]["ElementName"] = sCurrentElementName;
            _dsSCEN_ResultSummary.Tables[0].Rows[nCurrentIndex]["ElementID"] = nElementID;

            //var type is related to the resultid but not immediately available here. don't think it is needed
            _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.ResultSummary, dVal.ToString(), nResultID_FK, nVarType_FK, nScenarioID, nElementID, sCurrentElementName));

        }

        #region TS Processingg
        #region ToDSS

        //SP 14-Mar-2017 TODO consider making this a time value offset number
        //Depending on the model, this 
        public void SetTimeSeriesDateOffset()
        {
            switch ((SimLinkModelTypeID)_nActiveModelTypeID)
            {
                case SimLinkModelTypeID.EPA_SWMM5:
                    _bTSStartOfInterval = false;
                    break;
                default:
                    break;
                    //do nothing, already set to true by default
            }
        }

        //default is to export all vals
        //9/21/16: added the ability to request that aux vals be included too. Note- these will have to link up to a valid vartype_fk at this point.


        public void ExportScenarioToDSS(string sDSS_File, bool bScaleToOne = false)     // TODO: , Dictionary<string,string> dictPartsCustomize)
        {
            DateTime dtStartActual = GetSimStartEnd(true);          // check against MEV to verify _tsd start time\
            string sStartTimeHEC = Commons.FormatDATE2HEC(dtStartActual);
            string sTS_HECInterval = Commons.FormatInterval2HEC(_tsdSimDetails._tsIntervalType, _tsdSimDetails._nTSInterval);
            DateTime dtEndActual = GetSimStartEnd(false);
            string sDateRange = externalDSS.GetDateRangeFromDates(dtStartActual, dtEndActual);
            string sScenarioLabel = _nActiveScenarioID.ToString();  //todo: give fleibility in scen naming
            List<double[,]> lstVals = new List<double[,]>();
            List<string> lstDSS_Parts = new List<string>();
            foreach(DataRow drResultTS in _dsEG_ResultTS_Request.Tables[0].Rows) //SP 15-Feb-2017 _dsEG_ResultTS_Request is now the combined ResultTS dataset
            { 
                string sGroupID = "TS_" + Convert.ToInt32(drResultTS["ResultTS_ID"].ToString());
                double[,] dVals = GetTS_FromMemory(sGroupID);
                if (bScaleToOne)
                {
                    TimeSeries.ScaleArrayByMaxVal(dVals);
                }

                if (dVals != null)
                {
                    string[] sParts = DSS_GetParts(drResultTS, _nActiveScenarioID, sDateRange, sTS_HECInterval, dtStartActual, dtEndActual, sScenarioLabel); //changed to sTS_HECInterval
                    string sJoined = string.Join(",", sParts);
                    lstVals.Add(dVals);
                    lstDSS_Parts.Add(sJoined);      //merged together because works better with overridden features
                }
            }
            double[][,] dOut = lstVals.ToArray();
            externalDSS dssOut = new externalDSS(1, 1, 1, new Dictionary<string, string> { }, 1, 1);
            dssOut.WriteData(dOut, sDSS_File, null, null, lstDSS_Parts);
        }

        // Return the start or tend time of a sim
        // check with base class to make sure not over-written
        private DateTime GetSimStartEnd(bool bIsStartTime){
            DateTime dtReturn;
            if(_tsdSimDetails==null) // not set for some reason; this is generally bad, and user should be alerted.            
            {
                _log.AddString(string.Format("Warning: simulation start/end duration not set for scenario {0}. Using default value as placeholder",_nActiveScenarioID), Logging._nLogging_Level_1, false, true);
                _tsdSimDetails = new TimeSeries.TimeSeriesDetail(new DateTime(2000, 1, 1), IntervalType.Hour, 1);
            }



            dtReturn = _tsdSimDetails._dtStartTimestamp;
            if(!bIsStartTime)
                dtReturn = _tsdSimDetails._dtEndTimestamp;

            dtReturn=GetSimStartEndMEV(dtReturn,bIsStartTime);
            return dtReturn;
        }

        /// <summary>
        /// Check whether base class has overridden sim start-end time
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bIsStartTime"></param>
        protected virtual DateTime GetSimStartEndMEV(DateTime dt, bool bIsStartTime)
        {
            // should happen on derived class
            return dt;
        }


        /// <summary>
        /// Function wraps around eval calls to export data from a cohort to a DSS
        /// </summary>
        /// <param name="nCohortID"></param>
        /// <param name="sDSS_FullPath"></param>
        /// <param name="bGetNotRun"></param>
        /// <param name="astrScenarioId"></param>
        /// <param name="bIncludeAux"></param>
        /// <param name="bSkipBaseline"></param>
        /// <param name="bAppendTS_ToScenarioLabel"></param>
        public void ExportTimeseriestoDSSByCohort(int nCohortID, string sDSS_FullPath, bool bGetNotRun = true, bool bScaleValsToOne = false, string[] astrScenarioId = null, Dictionary<string, DSS_CustomPart> dictCustom = null, bool bIncludeAux = true, bool bSkipBaseline = false, bool bAppendTS_ToScenarioLabel = false)
        {
            InitCohort();       // not sure if necessary?

            foreach (DataRow drEG in _dsEG_Cohort.Tables[0].Rows)
            {
                bool bIsXModel = Convert.ToBoolean(drEG["IsXModel"].ToString());
                int nEvalID = Convert.ToInt32(drEG["EvaluationID"].ToString());
                SetActiveEvalID(nEvalID, EvalActivationCode.Cohort);                                           // update the current eval
                ExportTimeseriestoDSSByEval(nEvalID, sDSS_FullPath, false, bScaleValsToOne, astrScenarioId, dictCustom);
            }
        }

        /// <summary>
        /// Use dictionary arguments to override dtstart/end stuff when not generated clearly from associated simulations...
        /// Developed for when multiple scenarios are joined into an associated simlink scenario
        /// </summary>
        /// <param name="dictOverrides"></param>
        /// <param name="dtStartActual"></param>
        /// <param name="dtEndActual"></param>
        /// <param name="sTS_HECInterval"></param>
        private void SyncOverrides(Dictionary<string, string> dictOverrides, ref DateTime dtStartActual, ref DateTime dtEndActual, ref string sTS_HECInterval)
        {
            if (dictOverrides != null)
            {
                int nSeconds= -1;
                bool bChange = false;
                if (dictOverrides.ContainsKey("dss_start_time")){
                    dtStartActual = Convert.ToDateTime(dictOverrides["dss_start_time"]);
                    // do not support time interval definition here... this could be accomplished more easily 
                    bChange = true;
                    _log.AddString(string.Format("Explicitly setting start time for dss export to {0}", dtStartActual.ToString()), Logging._nLogging_Level_2, false, true);
                }
                if (dictOverrides.ContainsKey("ts_interval"))
                {
                    bChange = true;
                    int nTS_Inverval = Convert.ToInt32(dictOverrides["ts_interval"]);       //todo: support different untis for the ts_interval
                    nSeconds = Convert.ToInt32(nTS_Inverval * _dResultTS_Vals[0].GetLength(0));
                    sTS_HECInterval = Commons.FormatInterval2HEC(IntervalType.Second, nTS_Inverval);
                }
                if(bChange){
                    TimeSpan t = new TimeSpan(0, 0, nSeconds);
                    dtEndActual = dtStartActual + t; 
                }
            }
        }

        /// <summary>
        /// Updated DSS export using new externalDSS class
        /// (todo: update similarly for the scenario)
        /// </summary>
        /// <param name="nEvalID"></param>
        /// <param name="sDSS_FullPath"></param>
        /// <param name="bGetNotRun"></param>
        /// <param name="astrScenarioId"></param>
        /// <param name="dictCustom"></param>
        /// <param name="bIncludeAux"></param>
        /// <param name="bSkipBaseline"></param>
        /// <param name="bAppendTS_ToScenarioLabel"></param>
        public void ExportTimeseriestoDSSByEval(int nEvalID, string sDSS_FullPath, bool bGetNotRun = true, bool bScaleValsToOne = false, string[] astrScenarioId = null, Dictionary<string, DSS_CustomPart> dictCustom = null, Dictionary<string, string> dictOverrides = null, bool bIncludeAux = true, bool bSkipBaseline = false, bool bAppendTS_ToScenarioLabel = false)
        {
            if (astrScenarioId ==null)
                astrScenarioId = new string[]{};
            DataSet dsScen = ProcessEG_GetGS_Initialize(_nActiveEvalID, astrScenarioId, bGetNotRun);
            foreach (DataRow dr in dsScen.Tables[0].Rows)
            {
                int nScenarioID = Convert.ToInt32(dr["ScenarioID"]);
                if (!bSkipBaseline || nScenarioID != _nActiveBaselineScenarioID)        // may wish to skip baseline (especially in RT setting)
                {
                    SetActiveScenarioID(nScenarioID);
                    string sScenarioLabel = dr["ScenarioLabel"].ToString();

                    DateTime dtStartActual = GetSimStartEnd(true);          // check against MEV to verify _tsd start time\
                    string sStartTimeHEC = Commons.FormatDATE2HEC(dtStartActual);
                    string sTS_HECInterval = Commons.FormatInterval2HEC(_tsdSimDetails._tsIntervalType, _tsdSimDetails._nTSInterval);
                    DateTime dtEndActual = GetSimStartEnd(false);

                    SyncOverrides(dictOverrides, ref dtStartActual, ref dtEndActual, ref sTS_HECInterval);
                    string sDateRange = externalDSS.GetDateRangeFromDates(dtStartActual, dtEndActual);
                    //       string sScenarioLabel = _nActiveScenarioID.ToString();  //todo: give fleibility in scen naming
                    List<double[,]> lstVals = new List<double[,]>();
                    List<string> lstDSS_Parts = new List<string>();
                    foreach (DataRow drResultTS in _dsEG_ResultTS_Request.Tables[0].Rows) //SP 15-Feb-2017 _dsEG_ResultTS_Request is now the combined ResultTS dataset
                    {
                        string sGroupID = "TS_" + Convert.ToInt32(drResultTS["ResultTS_ID"].ToString());
                        double[,] dVals = GetTS_FromMemory(sGroupID);
                        if (bScaleValsToOne)
                        {
                            TimeSeries.ScaleArrayByMaxVal(dVals);
                        }

                        if (dVals != null)
                        {
                            string[] sParts = externalDSS.DSS_GetParts(drResultTS, _nActiveScenarioID, sDateRange, sTS_HECInterval, dtStartActual, dtEndActual, sScenarioLabel, dictCustom); //changed to sTS_HECInterval
                            string sJoined = string.Join(",", sParts);
                            lstVals.Add(dVals);
                            lstDSS_Parts.Add(sJoined);      //merged together because works better with overridden features
                        }
                    }
                    double[][,] dOut = lstVals.ToArray();
                    externalDSS dssOut = new externalDSS(1, 1, 1, new Dictionary<string, string> { }, 1, 1);
                    dssOut.WriteData(dOut, sDSS_FullPath, null, null, lstDSS_Parts);
                }
            }
        }


        // met 1/20/17: this is the older version and needs to be gotten rid of
        //SP 15-Feb-2017 As per METs comment about it being old - commented out whole routine
        // met 3/30/17: not seeing this commented yout yet, can we just remove??
        
        /*public void ExportTimeseriesToDSSByEval(int nEvalID, string sDSS_FullPath, bool bGetNotRun = true, string[] astrScenarioId = null, bool bIncludeAux = true)
        {
            if (_nActiveEvalID == -1)
            {
                InitializeEG(nEvalID);
                SetTSDetails();
            }

            // TODO SP 12-Aug-2016 _tsdSimDetails needs to be defined for all classes - needed to be added for EPANET expect the same for others

            string sDefaultLogPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, -99999, _nActiveEvalID) + @"\\!LOGS";
            _sLogLocation_Simlink = sDefaultLogPath;
            _log.Initialize("logConvertTimeSeriesToDSS_" + _nActiveEvalID.ToString(), _log._nLogging_ActiveLogLevel, _sLogLocation_Simlink);   //begin a logging session

            //SP 12-Aug-2016 if astrScenarioId is not specified, pass an empty string
            if (astrScenarioId == null)
                astrScenarioId = new string[0]; // when there is no scenario defined then run all scenarios
            DataSet dsScen = ProcessEG_GetGS_Initialize(_nActiveEvalID, astrScenarioId, bGetNotRun);

            //DataSet dsResultTS = EGDS_GetResultTS(_nActiveReferenceEvalID, bIncludeAux); //SP 15-Feb-2017 Think this should be called in parent routine InitialiseEG

            string sBaseFolder = @"c:\a";
            //string sDSS = Path.Combine(sBaseFolder,"DSS_Output.dss");
            bool bIsDSS_ByScen = true;          //todo: store on simlink instance
            string sStartTimeHEC = dss_wrap.Commons.FormatDATE2HEC(_tsdSimDetails._dtStartTimestamp);

            //SP 12-Aug-2016 MET advised valid HECDSS date range is required to be passed to WriteTS_ToDSS
            string sDateRange = dss_wrap.dssutl_wrap.GetDateRangeFromDates(_tsdSimDetails._dtStartTimestamp, _tsdSimDetails._dtEndTimestamp);

            //string sTS_Interval = "30MIN";   //dssutl_wrap.Common.FormatDATE2HEC()
            //SP 11-Aug-2016 this HECInterval specifies how to increment the start time so needs a specific format
            string sTS_HECInterval = dss_wrap.Commons.FormatInterval2HEC(_tsdSimDetails._tsIntervalType, _tsdSimDetails._nTSInterval);

            foreach (DataRow dr in dsScen.Tables[0].Rows)
            {
                int nScenarioID = Convert.ToInt32(dr["ScenarioID"]);
                string sScenarioLabel = Convert.ToString(dr["ScenarioLabel"]);
                _log.AddString(string.Format("----------------Processing ScenarioID = {0} ------------------", nScenarioID), Logging._nLogging_Level_1);
                string sTS_File = CommonUtilities.GetSimLinkFull_TS_FilePath(_sActiveModelLocation, _nActiveModelTypeID, _nActiveEvalID, nScenarioID, bIsDSS_ByScen);
                _hdf5._sHDF_FileName = sTS_File;
                if (_hdf5.hdfIsValidHDF(_hdf5._sHDF_FileName))
                {
                    _hdf5.hdfOpen(sTS_File, true, false);

                    foreach (DataRow drResultTS in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode = " + ((int)RetrieveCode.Primary).ToString())) //SP 15-Feb-2017 dataset for TSREsults is now obtained in InitializeEG, get just the primary
                    {

                        if (true)           //    todo: potentially implement this check   CheckTS_RecordHasData(drResultTS))
                        {
                            int nRecordID = Convert.ToInt32(drResultTS["ResultTS_ID"].ToString());
                            _log.AddString("Processing ResultTS recordID = " + nRecordID, Logging._nLogging_Level_1, false);
                            string[] sDSS_Parts = DSS_GetParts(drResultTS, nScenarioID, sStartTimeHEC, sTS_HECInterval,_tsdSimDetails._dtStartTimestamp,_tsdSimDetails._dtEndTimestamp, sScenarioLabel); //changed to sTS_HECInterval
                            string sGroupName = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, nRecordID.ToString());
                            double[,] dVals = _hdf5.hdfGetDataSeries(sGroupName, "1", true);
                            List<TimeSeries> lstTS = TimeSeries.Array2DToTSList(_tsdSimDetails._dtStartTimestamp, _tsdSimDetails._nTSInterval, _tsdSimDetails._tsIntervalType, dVals);

                            if (lstTS != null)
                                dss_wrap.dssutl_wrap.WriteTS_ToDSS(sDSS_FullPath, lstTS, sDSS_Parts, sDateRange);
                        }
                    }
                    _hdf5.hdfClose();
                }
                else
                {
                    //todo log the issue
                }
            }

            //write log file
            _log.WriteLogFile();
        }*/

        //9/11/14: added to check if TS data exist for that record
        //this is SWMM Specific at this point.
        //todo: override?
        private bool CheckTS_RecordHasData(DataRow dr)
        {
            if (_nActiveModelTypeID != CommonUtilities._nModelTypeID_SWMM)
            {
                return true;
            }
            else
            {
                if (Convert.ToInt32(dr["ElementIndex"]) == -1)
                {      //signal for TS not output
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //tested for SWMM

        // added start/end dates which you must know so might as well make available
        // MET 1/20/17: moved this to a static function in ExternalDSS
        public virtual string[] DSS_GetParts(DataRow dr, int nScenarioID, string sDateDSS_Format, string sIntervalDSS_Format, DateTime dtStart, DateTime dtEnd,  string sScenarioLabel = "")
        {
            string[] sParts = new string[8];
            sParts[0] = dr["Result_Label"].ToString();
            sParts[1] = dr["Element_Label"].ToString();     //b part
            sParts[2] = dr["FieldLabel"].ToString();
            sParts[3] = sDateDSS_Format;                    //HEC interval
            sParts[4] = sIntervalDSS_Format;
            sParts[5] = nScenarioID.ToString() + " " + sScenarioLabel;
            sParts[6] = dtStart.ToString();
            sParts[7] = dtEnd.ToString();
            return sParts;
        }

        #endregion

        public void SetTS_Filename2(string sFilename)
        {
            _hdf5._sHDF_FileName = sFilename;
        }
        //
        public virtual void SetTS_FileName(int nScenarioID, string sPath = "NOTHING", bool bSetHDF=true)
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


        /// <summary>
        /// Check whether a specific ResultTS is auxiliary
        /// </summary>
        /// <param name="nRefID"></param>
        /// <returns></returns>
        private bool HelperResultTSisAux(int nRefID){
            bool bReturn = false;
            DataRow[] dr = _dsEG_ResultTS_Request.Tables[0].Select("ResultTS_ID = " + nRefID); //SP 15-Feb-2017 _dsEG_ResultTS_Request is now the combined ResultTS dataset
            if (dr.Count() > 0)
            {
                bReturn = (Convert.ToInt32((dr[0]["RetrieveCode"].ToString())) == (int)RetrieveCode.Aux);      // 3 is code for aux //SP 15-Feb-2017 changed from ts_code to RetrieveCode
            }

            return bReturn;
        }


/// <summary>
/// Process timeseries data
///     - primary use case: take results output and do something cool with each ts data point in the ts
///     - secondary use case: do something cool with aux in data. As aux, it can (and sometimes must) be performed prior to the sim run
///
///   1/25/17: updated to take a param for whether we want to run auxiliary ts or not.
///         PHASE 1: added a check on whether the refeferenced TS is AUX, but nothing else.
///         PHASE 2: consider avoiding loop over all TS (sometimes many thousand)- for AUX you may just care about 1 or 2.
/// 
/// </summary>
/// <param name="nEvalId"></param>
/// <param name="nScenarioID"></param>
/// <param name="sHDF_TS_Location"></param>
        private void ResultTS_ProcessSecondary(int nEvalId, int nScenarioID, /*string sHDF_TS_Location, SP 2-Mar-2017 Should already be in memory so no need to access HDF5*/ 
            bool bRunAux = false)
        {
            hdf5_wrap hdfFAKE_DELETE = new hdf5_wrap();     //probably need a baseline hdf on the simlink object; placeholder
            if (_dsEG_ResultTS_Request != null)          // you can only use this if it exists. todo: set earlier on
            {
                int nSecondaryTSCount = _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Secondary).ToString()).Count(); //SP 15-Feb-2017 Now retrieving all Secondary TS from single dataset

                if (nSecondaryTSCount > 0) //SP 15-Feb-2017 Now retrieving all Secondary TS from single dataset
                {
                    //int nPrimaryTSCount = _dsEG_ResultTS_Request.Tables[0].Rows.Count;
                    //if (sHDF_TS_Location == "NOTHING")            // believe hdf5 mgmt should be happening elsewhere
                    //{
                    //    SetTS_FileName(nScenarioID, "NOTHING");
                    //    sHDF_TS_Location = _hdf5._sHDF_FileName;
                    //}

                    //SP 5-Oct-2016 Incorporate sequences - open and close the HDF5
                    //get the minimum in the sequence for secondary variables
                    int nSQNProcessing = int.MaxValue;
                    foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Secondary).ToString())) //SP 15-Feb-2017 Now retrieving all Secondary TS from single dataset
                    {
                        int nSQN = dr.Field<int>("SQN");
                        nSQNProcessing = Math.Min(nSQN, nSQNProcessing);
                    }

                    int nCounter = 0;

                    //loop while nCounter < _dsEG_SecondaryTS_Request.Tables[0].Rows
                    while (nCounter < nSecondaryTSCount)
                    {
                        //SP 27-Oct-2016 - only open HDF5 if intermediate storage
                        // met 1/26/17: believe hdf5 open/close should not be managed at the level of this function //SP 15-Feb-2017 Agree - will change
                        // you should already have in memory at this time, either through creating the ts or having initialized to pre-created 
                //        if (_IntermediateStorageSpecification._bResultTS)
                //            _hdf5.hdfOpen(sHDF_TS_Location, false, true);

                        foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Secondary).ToString())) //SP 15-Feb-2017 Now retrieving all Secondary TS from single dataset
                        {
                            int nRefID = Convert.ToInt32(dr["RefTS_ID_FK"].ToString());
                          //  int nRefID = Convert.ToInt32(dr["ResultTS_ID"].ToString());
                            bool bIsAux = HelperResultTSisAux(nRefID);
                            bool bMatch = (bIsAux == bRunAux);           // whether we havea match on the ref TS
                                                                         
                            //only process the rows equal to SQNProcessing
                            if ((Convert.ToInt32(dr["SQN"].ToString()) == nSQNProcessing) && bMatch)
                            {
                                //   _hdf5_SWMM.hdfOpen(sTS_H5, false);
                                int nFunctionCode = Convert.ToInt32(dr["FunctionID_FK"].ToString());
                                if (nFunctionCode > 0)
                                {
                                    string sFunction = dr["CustomFunction"].ToString();
                                    string sArgs = dr["FunctionArgs"].ToString();

                                    //SP 16-Jun-2016 - for backwards compatibility
                                    bool bUseQuickParse;
                                    if (dr["UseQuickParse"].ToString() != string.Empty)
                                    {
                                        bUseQuickParse = Convert.ToBoolean(dr["UseQuickParse"].ToString());            //defined on the function 
                                    }
                                    else
                                        bUseQuickParse = false;

                                    double[,] dVals = ParseTimeSeriesExpression(nScenarioID, nEvalId, sFunction, ref hdfFAKE_DELETE, sArgs, bUseQuickParse);
                                    string sGroupName_Out = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_ID"].ToString());
                                    
                                    //SP 27-Oct-2016 - only write back to HDF5 if intermediate storage
                                    //SP 15-Feb-2017 Shouldn't need to write to HDF5 in this routine anymore - done once at the end of common
                                    /*if (_IntermediateStorageSpecification._bResultTS)
                                        _hdf5.hdfWriteDataSeries(dVals, sGroupName_Out, "1");*/
                                    
                                    //get the index for the group name
                                    //int nIndex = Array.IndexOf(_sTS_GroupID, sGroupName_Out);
                                    int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())];
                                    _dResultTS_Vals[nIndex] = dVals;
                                }
                                else
                                {
                                    string sArgs = dr["FunctionArgs"].ToString();
                                    if (nRefID == 1303)
                                        nRefID = nRefID;

                                    double[,] dVals = TS_StandardSecondaryOperation(nFunctionCode, sArgs, nRefID);
                                    string sGroupName_Out = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, dr["ResultTS_ID"].ToString());
                                    
                                    //SP 27-Oct-2016 - only write back to HDF5 if intermediate storage
                                    //SP 15-Feb-2017 Shouldn't need to write to HDF5 in this routine anymore - done once at the end of common
                                    /*if (_IntermediateStorageSpecification._bResultTS)
                                        _hdf5.hdfWriteDataSeries(dVals, sGroupName_Out, "1");*/

                                    Console.WriteLine("Got REF ID: " + nRefID);

                                    //get the index for the group name
                                    //int nIndex = Array.IndexOf(_sTS_GroupID, sGroupName_Out);
                                    int nIndex = _dictResultTS_Indices[Convert.ToInt32(dr["ResultTS_ID"].ToString())];
                                    _dResultTS_Vals[nIndex] = dVals;
                                }
                                nCounter++;
                            }
                            else
                            {
                                //SP 15-Feb-2017 don't think we want to increment counter. Using counter here to keep track of how many secondary TS arrays have been processed
                                //nCounter++;         // pass over- increment counter 
                            }

                        }

                        //SP 27-Oct-2016 - only close HDF5 if intermediate storage
                        // met 1/26/17: believe hdf5 open/close should not be managed at the level of this function
                        // you should already have in memory at this time, either through creating the ts or having initialized to pre-created 
                    //    if (_IntermediateStorageSpecification._bResultTS)
                       //     _hdf5.hdfClose();

                        //SP 5-Oct-2016 Incorporate sequences - open and close the HDF5
                        //get the new minimum in the sequence for secondary variables
                        int nSQNPrcoessing_Prev = nSQNProcessing;
                        nSQNProcessing = int.MaxValue;
                        foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)RetrieveCode.Secondary).ToString())) //SP 15-Feb-2017 Now retrieving all Secondary TS from single dataset
                        {
                            int nSQN = dr.Field<int>("SQN");
                            if (nSQN > nSQNPrcoessing_Prev)
                                nSQNProcessing = Math.Min(nSQN, nSQNProcessing);
                        }
                    }
                }
            }
        }




        public double[,] TS_StandardSecondaryOperation(int nFunctionCode, string sArgs, int nRefID)
        {
            double[,] dVals;
            double[,] dTS_Ref; double dDefVal; List<TimeSeries> lstInput; List<TimeSeries> lstOutput;
            string sGroupID_REF = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, nRefID.ToString());
            switch (nFunctionCode)
            {
                case -1:                    //TimeSeries.filter_by_hour_of_day.

                    int nLowVal; int nHighVal;
                    dTS_Ref = GetTS_FromMemory(sGroupID_REF);
                    dDefVal = TS_HELPER_GetArgs_FilterByTime(nRefID, sArgs, out  nLowVal, out  nHighVal);
                    lstInput = TimeSeries.Array2DToTSList(_tsdResultTS_SIM._dtStartTimestamp, _tsdResultTS_SIM._nTSInterval, _tsdResultTS_SIM._tsIntervalType, dTS_Ref, 0);
                    lstOutput = TimeSeries.FilterTimeSeriesByTime(lstInput, nLowVal, nHighVal, dDefVal);
                    dVals = TimeSeries.tsTimeSeriesTo2DArray(lstOutput);
                    break;
                case -2:                    //TimeSeries.filter_by_hour_of_day.
                    dVals = null;
                    dTS_Ref = GetTS_FromMemory(sGroupID_REF);
                    double dThresholdVal; bool bIsOverThreshold;
                    lstInput = TimeSeries.Array2DToTSList(_tsdResultTS_SIM._dtStartTimestamp, _tsdResultTS_SIM._nTSInterval, _tsdResultTS_SIM._tsIntervalType, dTS_Ref, 0);
                    int nTS_Compare;    //ID of the TS to compare against is packed into args

                    dDefVal = TS_HELPER_GetArgs_FilterByThreshold(nRefID, sArgs, out nTS_Compare, out  dThresholdVal, out  bIsOverThreshold);

                    string sGroupID_CMP = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, nTS_Compare.ToString());
                    double[,] dTS_CMP = GetTS_FromMemory(sGroupID_CMP);

                    List<TimeSeries> lstCompare = TimeSeries.Array2DToTSList(_tsdResultTS_SIM._dtStartTimestamp, _tsdResultTS_SIM._nTSInterval, _tsdResultTS_SIM._tsIntervalType, dTS_CMP, 0);
                    lstOutput = TimeSeries.FilterTimeSeriesByPeakValue(lstInput, lstCompare, dThresholdVal, bIsOverThreshold, dDefVal);
                    break;
                default:
                    dVals = null;
                    break;
            }

            return dVals;
        }

        private double TS_HELPER_GetArgs_FilterByTime(int nRefID, string sIN, out int nLowVal, out int nHighVal)
        {
            try
            {
                string[] sVals = sIN.Split(',');
                nLowVal = Convert.ToInt32(sVals[0]);
                nHighVal = Convert.ToInt32(sVals[1]);
                double dReturn = Convert.ToDouble(sVals[2]);
                return dReturn;
            }
            catch (Exception ex)
            {
                _log.AddString("Error processing args; should be 3 comma sep vals for ref tS :" + nRefID, Logging._nLogging_Level_1);
                //log the issue

                nLowVal = 0;
                nHighVal = 0;
                double dReturn = -666;
                return dReturn;
            }
        }

        private double TS_HELPER_GetArgs_FilterByThreshold(int nRefID, string sIN, out int nCompareTS_ID, out double dThreshold, out bool bIsOver)
        {
            try
            {
                bIsOver = false;
                string[] sVals = sIN.Split(',');
                nCompareTS_ID = Convert.ToInt32(sVals[0]);
                dThreshold = Convert.ToDouble(sVals[1]);
                if (Convert.ToInt32(sVals[2]) == 1)
                {
                    bIsOver = true;
                }
                double dReturn = Convert.ToDouble(sVals[3]);
                return dReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing args; should be 3 comma sep vals for ref tS :" + nRefID);
                //log the issue
                nCompareTS_ID = -1;
                dThreshold = 0;
                bIsOver = true;
                double dReturn = -666;
                return dReturn;
            }
        }

        #endregion



        #region Performance

        private double Performance_CalcLinkedTables(int nScenarioID, int nPerfID, int nLinkTableCode, int nLookupFunction, DataRow drPerf)
        {
            double dReturn = -1;

            //SP 18-Feb-2016 - updated for enum type LinkedDataType
            switch ((LinkedDataType)nLinkTableCode)                      //from performance table met 5/13/2013      
            //Code indicating reference table:  (0 = function, 1=tblModelElementVals- Val, 2: tblModelElementVals- DV Option, 3 tblResultVar_Details, 4 Event Summary 5: Distrib  (NOT USED!) 6: tblPerformance  )
            {

                case LinkedDataType.ModelElements:
                    //do nothing
                    break;
                case LinkedDataType.DVOptions:         // Based on DV Option
                    int nDV_ID = Convert.ToInt32(drPerf["DV_ID_FK"]);                   //get this
                    dReturn = Performance_CalcFromDV_Option(nScenarioID, nPerfID, nDV_ID);
                    // todo
                    break;
                case LinkedDataType.ResultSummary:                 //Result VARS (per LNC!)
                    dReturn = PerformanceCalcLinkedTables_RESULTSUMMARY(nScenarioID, nPerfID, nLookupFunction);
                    break;
                case LinkedDataType.ResultTS:
                    dReturn = PerformanceCalcLinkedTables_RESULTTIMESERIES(nScenarioID, nPerfID, nLookupFunction);
                    break;
                case LinkedDataType.Event:                 //EVENT
                    dReturn = PerformanceCalcLinkedTables_EVENT_SUMMARY(nScenarioID, nPerfID, nLookupFunction);
                    break;
                /*sqlPerf = "SELECT PerformanceID, Performance_Label, val, ScenarioID_FK FROM qryPerformance002_EventLink "
                 + "WHERE (((PerformanceID)=@PerfID) AND ((ScenarioID_FK)=@Scenario))";
                break;*/
                case LinkedDataType.Performance:            // PERFORMANCE
                    dReturn = PerformanceCalcLinkedTables_PERFORMANCESUMMARY(nScenarioID, nPerfID, nLookupFunction);
                    break;
                // this case is fundamentally different: it does not link to previous results...
            }

            return dReturn;

        }

        // return -1 if no DV option found
        private int GetDVOptionFromMEV(int nScenarioID, int nDV_ID)
        {

            IEnumerable<simlinkDetail> ModelChangesList = from ModelChanges in _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.MEV)
                                                                       .Where(x => x._nScenarioID == nScenarioID)
                                                                       .Where(x => x._nRecordID == nDV_ID).AsEnumerable()
                                                          select new simlinkDetail
                                                          {
                                                              _nDV_Option = ModelChanges._nDV_Option,
                                                              _sElementName = ModelChanges._sElementName,
                                                              _sVal = ModelChanges._sVal,
                                                              _nRecordID = ModelChanges._nRecordID
                                                          };
            if (ModelChangesList.FirstOrDefault() == null)
                return -1;
            else
                return ModelChangesList.FirstOrDefault()._nDV_Option;
        }

        //link return an option list based on DV option
        private double Performance_CalcFromDV_Option(int nScenarioID, int nPerfID, int nDV_ID, double dDefaultReturnVal = 0)
        {
            int nDV_Option = GetDVOptionFromMEV(nScenarioID, nDV_ID);
            if (nDV_Option == -1)
            {
                return dDefaultReturnVal;
            }
            else
            {
                var PerformanceOptionVals = from Performance in _dsEG_Performance_Request.Tables[0].AsEnumerable()                //which performance to characterize
                                            join OptionList in _dsEG_OptionVals.Tables[0].AsEnumerable()               //which results it links to
                                            on Performance.Field<int>("OptionID_FK") equals
                                            OptionList.Field<int>("OptionID")
                                            where OptionList.Field<int>("OptionNo") == nDV_Option
                                            where Performance.Field<int>("PerformanceID") == nPerfID
                                            select new
                                            {
                                                val = OptionList.Field<string>("val"),
                                                OptionNo = OptionList.Field<int>("OptionNo"),
                                                OptionID = OptionList.Field<int>("OptionID"),
                                                OptionID2 = Performance.Field<int>("OptionID_FK"),
                                            };
                try
                {
                    string sVal = PerformanceOptionVals.FirstOrDefault().val;
                    return Convert.ToDouble(sVal);              //todo: ensure that val is actualy numeric
                }
                catch (Exception ex)
                {
                    _log.AddString(string.Format("Error processing CalcFromDV_Option values for ScenarioID {0}, PerfID {1}. Exception: {2}", nScenarioID, nPerfID, ex.Message), Logging._nLogging_Level_1);
                    throw; //SP 5-Aug-2016 throw error to ProcessScenario I don't think we want these to just fall through without being caught - open to other suggestions though
                }
            }
        }

        //todo: currently linking to dsScenResults... --> Transition to SimLinkDetails as we move away from datasets
        private double PerformanceCalcLinkedTables_RESULTSUMMARY(int nScenarioID, int nPerfID, int nFunctionType/*, bool bDefault_ApplyThreshold,
            double dDefault_Threshold, bool bDefault_IsAboveThreshold*/) //SP 23-Feb-2016 Added in threshold parameters for consistency
        {
            double dReturn = 0;

            var ResultsVals = from Performance in _dsEG_Performance_Request.Tables[0].AsEnumerable()                //which performance to characterize
                              join ResultXREF in _dsEG_PerformanceResultXREF.Tables[0].AsEnumerable()               //which results it links to
                              on Performance.Field<int>("PerformanceID") equals
                              ResultXREF.Field<int>("PerformanceID_FK")
                              join ResultsSummary in _dsSCEN_ResultSummary.Tables[0].AsEnumerable()                              //current scenario results
                          on ResultXREF.Field<int>("LinkTableID_FK") equals
                          ResultsSummary.Field<int>("Result_ID_FK")
                              where ResultXREF.Field<int>("LinkType") == (int)LinkedDataType.ResultSummary              //code for linked results table //SP 18-Feb-2016 Changed enum type LinkedDataType
                              where ResultXREF.Field<int>("PerformanceID_FK") == nPerfID
                              select new
                              {
                                  val = ResultsSummary.Field<double>("val"),
                                  scalar = ResultXREF.Field<double>("ScalingFactor"),
                                  applythreshold = Convert.ToBoolean(ResultXREF.Field<Int32>("ApplyThreshold")), //SP 3-Aug-2016 Confirm this is working with change datatype in tblPerformance
                                  threshold = Convert.ToDouble(ResultXREF.Field<double>("Threshold")),
                                  isabove_threshold = Convert.ToBoolean(ResultXREF.Field<Int32>("IsOver_Threshold"))
                              };

            try
            {
                //SP 23-Feb-2016 Refactored to a function to process performance variables
                IEnumerable<PerformanceValues> PerformanceVals =
                    from item in ResultsVals.AsEnumerable() select new PerformanceValues(item.val, item.scalar, item.applythreshold, item.threshold, item.isabove_threshold);
                dReturn = ProcessPerformanceVals(nPerfID, (Perf_FunctionOnLinkedData)nFunctionType, PerformanceVals/*,
                bDefault_ApplyThreshold, dDefault_Threshold, bDefault_IsAboveThreshold*/);
                return dReturn;
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error processing Result Summary values for ScenarioID {0}, PerfID {1}. Exception: {2}", nScenarioID, nPerfID, ex.Message), Logging._nLogging_Level_1);
                throw; //SP 5-Aug-2016 throw error to ProcessScenario I don't think we want these to just fall through without being caught - open to other suggestions though
            }

            //SP 18-Feb-2016 Changed switch and case options to enum
            /*switch ((Perf_FunctionOnLinkedData)nFunctionType)      //for some reason, must treat val as decimal.... don't know why.
            {
                case Perf_FunctionOnLinkedData.Sum:
                    if (bExceedsThreshold)
                        dReturn = Convert.ToDouble(ResultsVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Sum());
                    else
                        dReturn = Convert.ToDouble(ResultsVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Sum());
                    break;

                case Perf_FunctionOnLinkedData.Min:
                    if (bExceedsThreshold)
                        dReturn = Convert.ToDouble(ResultsVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Min());
                    else
                        dReturn = Convert.ToDouble(ResultsVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Min());
                    break;
                
                case Perf_FunctionOnLinkedData.Max:
                    if (bExceedsThreshold)
                        dReturn = Convert.ToDouble(ResultsVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Max());
                    else
                        dReturn = Convert.ToDouble(ResultsVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Max());
                    break;

                case Perf_FunctionOnLinkedData.Count:
                    if (bExceedsThreshold)
                        dReturn = Convert.ToDouble(ResultsVals.Where(c => c.val >= dThreshold).Count());
                    else
                        dReturn = Convert.ToDouble(ResultsVals.Where(c => c.val <= dThreshold).Count());
                    break;
            }*/


        }

        private double PerformanceCalcLinkedTables_PERFORMANCESUMMARY(int nScenarioID, int nPerfID, int nFunctionType)
        {
            double dReturn = 0;
            var PerfVals = from Performance in _dsEG_Performance_Request.Tables[0].AsEnumerable()                //which performance to characterize
                           join ResultXREF in _dsEG_PerformanceResultXREF.Tables[0].AsEnumerable()               //which results it links to
                           on Performance.Field<int>("PerformanceID") equals
                           ResultXREF.Field<int>("PerformanceID_FK")
                           join PerformanceSummary in _dsSCEN_PerformanceDetails.Tables[0].AsEnumerable()                              //current scenario results
                         on ResultXREF.Field<int>("LinkTableID_FK") equals
                         PerformanceSummary.Field<int>("PerformanceID_FK")
                           where (ResultXREF.Field<int>("LinkType") == (int)LinkedDataType.Performance)  //SP 18-Feb-2016 Changed enum type LinkedDataType
                           where (Performance.Field<int>("PerformanceID") == nPerfID)          //code for linked results table           
                           where PerformanceSummary.Field<int>("ScenarioID_FK") == nScenarioID
                           select new
                           {
                               val = PerformanceSummary.Field<double>("val"),
                               scalar = ResultXREF.Field<double>("ScalingFactor"),
                               applythreshold = Convert.ToBoolean(ResultXREF.Field<Int32>("ApplyThreshold")), //SP 3-Aug-2016 Confirm this is working with change datatype in tblPerformance
                               threshold = Convert.ToDouble(ResultXREF.Field<double>("Threshold")),
                               isabove_threshold = Convert.ToBoolean(ResultXREF.Field<Int32>("IsOver_Threshold"))
                           };

            try
            {
                //SP 23-Feb-2016 Refactored to a function to process performance variables
                IEnumerable<PerformanceValues> PerformanceVals =
                    from item in PerfVals.AsEnumerable() select new PerformanceValues(item.val, item.scalar, item.applythreshold, item.threshold, item.isabove_threshold);
                dReturn = ProcessPerformanceVals(nPerfID, (Perf_FunctionOnLinkedData)nFunctionType, PerformanceVals/*,
                bDefault_ApplyThreshold, dDefault_Threshold, bDefault_IsAboveThreshold*/);
                return dReturn;
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error processing Performance Summary values for ScenarioID {0}, PerfID {1}. Exception: {2}", nScenarioID, nPerfID, ex.Message), Logging._nLogging_Level_1);
                throw; //SP 5-Aug-2016 throw error to ProcessScenario I don't think we want these to just fall through without being caught - open to other suggestions though
            }

            //todo: implement threshold on this/ 
            //SP 18-Feb-2016 Changed switch and case options to enum
            /*switch ((Perf_FunctionOnLinkedData)nFunctionType)                                                                  //for some reason, must treat val as decimal.... don't know why.
            {
                case Perf_FunctionOnLinkedData.Sum:                 //       todo figure this out   SIM_API_LINKS.ResultsAggregation.SUM.ToString():
                    //SP 23-Feb-2016 - Incorporate threshold and indication of direction for exceeds tolerance 
                    if (bExceedsThreshold)
                        dReturn = Convert.ToDouble(PerfVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Sum());
                    else
                        dReturn = Convert.ToDouble(PerfVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Sum());
                    //var sum = PerfVals.Select(c => c.val * c.scalar).Sum();                  //todo figure out how t
                    //dReturn = Convert.ToDouble(sum);
                    break;
                case Perf_FunctionOnLinkedData.Min:
                    //SP 23-Feb-2016 - Incorporate threshold and indication of direction for exceeds tolerance
                    if (bExceedsThreshold)
                        dReturn = Convert.ToDouble(PerfVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Min());
                    else
                        dReturn = Convert.ToDouble(PerfVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Min());
                    //var min = PerfVals.Select(c => c.val * c.scalar).Min();                  //min
                    //dReturn = Convert.ToDouble(min);
                    break;
                case Perf_FunctionOnLinkedData.Max:
                    //SP 23-Feb-2016 - Incorporate threshold and indication of direction for exceeds tolerance
                    if (bExceedsThreshold)
                        dReturn = Convert.ToDouble(PerfVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Max());
                    else
                        dReturn = Convert.ToDouble(PerfVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Max());
                    //var max = PerfVals.Select(c => c.val * c.scalar).Max();                  //max
                    //dReturn = Convert.ToDouble(max);
                    break;
                case Perf_FunctionOnLinkedData.Count:
                    //SP 23-Feb-2016 - Incorporate threshold and indication of direction for exceeds tolerance
                    if (bExceedsThreshold)
                        dReturn = Convert.ToDouble(PerfVals.Where(c => c.val >= dThreshold).Count());
                    else
                        dReturn = Convert.ToDouble(PerfVals.Where(c => c.val <= dThreshold).Count());
                    //var count = PerfVals.Select(c => c.val * c.scalar).Count();                  //max
                    //dReturn = Convert.ToDouble(count);
                    break;
            }*/

        }



        private double PerformanceCalcLinkedTables_EVENT_SUMMARY(int nScenarioID, int nPerfID, int nFunctionType)
        {
            double dReturn = 0;

            if (nPerfID == 7721 || nPerfID == 7748)
            {
                int n = 1;
            }


            var EventVals = from Performance in _dsEG_Performance_Request.Tables[0].AsEnumerable()                //which performance to characterize
                            join ResultXREF in _dsEG_PerformanceResultXREF.Tables[0].AsEnumerable()               //which results it links to
                            on Performance.Field<int>("PerformanceID") equals
                            ResultXREF.Field<int>("PerformanceID_FK")
                            join EventSummary in _lstSimLinkDetail.Where(x => x._slDataType == SimLinkDataType_Major.Event)
                                                            .Where(x => x._nScenarioID == nScenarioID).AsEnumerable()                           //current scenario results
                            on ResultXREF.Field<int>("LinkTableID_FK") equals
                            EventSummary._nRecordID
                            where ((ResultXREF.Field<int>("LinkType") == (int)LinkedDataType.Event) &&          //code for linked results table //SP 18-Feb-2016 Changed enum type LinkedDataType
                            (Performance.Field<int>("PerformanceID") == nPerfID))
                            select new
                            {
                                val = Convert.ToDouble(EventSummary._sVal),                                   //PerformanceSummary.Field<double>("val"),
                                scalar = ResultXREF.Field<double>("ScalingFactor"),
                                applythreshold = Convert.ToBoolean(ResultXREF.Field<Int32>("ApplyThreshold")), //SP 3-Aug-2016 Confirm this is working with change datatype in tblPerformance
                                threshold = Convert.ToDouble(ResultXREF.Field<double>("Threshold")),
                                isabove_threshold = Convert.ToBoolean(ResultXREF.Field<Int32>("IsOver_Threshold"))
                            };

            try
            {
                //SP 23-Feb-2016 Refactored to a function to process performance variables
                IEnumerable<PerformanceValues> PerformanceVals =
                    from item in EventVals.AsEnumerable() select new PerformanceValues(item.val, item.scalar, item.applythreshold, item.threshold, item.isabove_threshold);
                dReturn = ProcessPerformanceVals(nPerfID, (Perf_FunctionOnLinkedData)nFunctionType, PerformanceVals);
                return dReturn;
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error processing Event Summary values for ScenarioID {0}, PerfID {1}. Exception: {2}", nScenarioID, nPerfID, ex.Message), Logging._nLogging_Level_1);
                throw; //SP 5-Aug-2016 throw error to ProcessScenario I don't think we want these to just fall through without being caught - open to other suggestions though
            }

            //SP 18-Feb-2016 Changed switch and case options to enum
            /*switch ((Perf_FunctionOnLinkedData)nFunctionType)
            {
                case Perf_FunctionOnLinkedData.Sum:     //Sum of event values outside threshold
                    if (bExceedsThreshold)
                        dAggregateVal = EventVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Sum();
                    else
                        dAggregateVal = EventVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Sum();
                    break;
                case Perf_FunctionOnLinkedData.Min: 
                    //SP 22-Feb-2016 - Min of Event values outside threshold
                    if (bExceedsThreshold)
                        dAggregateVal = EventVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Min();
                    else
                        dAggregateVal = EventVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Min();
                    break;

                case Perf_FunctionOnLinkedData.Max: //SP 22-Feb-2016 - Max of Event values outside threshold
                    if (bExceedsThreshold)
                        dAggregateVal = EventVals.Where(c => c.val >= dThreshold).Select(c => c.val * c.scalar).Max();
                    else
                        dAggregateVal = EventVals.Where(c => c.val <= dThreshold).Select(c => c.val * c.scalar).Max();
                    break;
                case Perf_FunctionOnLinkedData.Count: //count
                    if (bExceedsThreshold)
                        dAggregateVal = EventVals.Where(c => c.val >= dThreshold).Count();
                    else
                        dAggregateVal = EventVals.Where(c => c.val <= dThreshold).Count();
                    break;
            }*/


        }

        //calculate performance metrics on Result TimeSeries - SP 22-Feb-2016
        private double PerformanceCalcLinkedTables_RESULTTIMESERIES(int nScenarioID, int nPerfID, int nFunctionType)
        {
            double dReturn = 0;

            var ResultTSRecords = from Performance in _dsEG_Performance_Request.Tables[0].AsEnumerable()                //which performance to characterize
                                  join ResultXREF in _dsEG_PerformanceResultXREF.Tables[0].AsEnumerable()               //which results it links to
                                  on Performance.Field<int>("PerformanceID") equals
                                  ResultXREF.Field<int>("PerformanceID_FK")
                                  //join ResultTimeSeries in _dsEG_ResultTS_Request.Tables[0].AsEnumerable() //current scenario results
                                  //on ResultXREF.Field<int>("LinkTableID_FK") equals
                                  //ResultTimeSeries.Field<int>("ResultTS_ID")
                                  join CombinedResultTimeSeries in _dsEG_ResultTS_Request.Tables[0].AsEnumerable() //current scenario results //SP 15-Feb-2017 Now retrieving all TS from single dataset
                                  on ResultXREF.Field<int>("LinkTableID_FK") equals
                                  CombinedResultTimeSeries.Field<int>("ResultTS_ID")
                                  where ((ResultXREF.Field<int>("LinkType") == (int)LinkedDataType.ResultTS) &&
                                  (Performance.Field<int>("PerformanceID") == nPerfID))
                                  select new
                                  {
                                      GroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, CombinedResultTimeSeries.Field<int>("ResultTS_ID").ToString() /*ResultTimeSeries.Field<int>("ResultTS_ID").ToString()*/),
                                      scalar = ResultXREF.Field<double>("ScalingFactor"),
                                      applythreshold = Convert.ToBoolean(ResultXREF.Field<Int32>("ApplyThreshold")), //SP 3-Aug-2016 Confirm this is working with change datatype in tblPerformance
                                      threshold = Convert.ToDouble(ResultXREF.Field<double>("Threshold")),
                                      isabove_threshold = Convert.ToBoolean(ResultXREF.Field<Int32>("IsOver_Threshold"))
                                      /*,startatperiod = CombinedResultTimeSeries.Field<Int32>("BeginPeriodNo")*/ //SP 1-Dec-2016 TS BeginPeriodNo now only returns values in timeseries after start period  
                                  };


            var dAggregateVal = CommonUtilities._dBAD_DATA;

            try
            {
                //SP 15-Jun-2016 process the time series data by aggregating through SummaryStatForTS
                IEnumerable<ResultTimeSeriesValues> ResultTimeSeriesVals = from item in ResultTSRecords.AsEnumerable()
                                                                           select new ResultTimeSeriesValues
                                                                               (item.GroupID,
                                                                               TimeSeries.SummaryStatForTS(GetTS_FromMemory(item.GroupID), (int)nFunctionType/*SP 1-Dec-2016 remove start at period, item.startatperiod*/),
                                                                               item.scalar, item.applythreshold, item.threshold, item.isabove_threshold);
                //SP 15-Jun-2016 convert the time series data into standard format for processsing performance vals
                IEnumerable<PerformanceValues> PerformanceVals =
                    from item in ResultTimeSeriesVals.AsEnumerable() select new PerformanceValues(item.dAgg, item.dScalar, item.bapplythreshold, item.dthreshold, item.bisabove_threshold);

                //SP 15-Jun-2016 Process the aggregated time series data using required thresholds
                dReturn = ProcessPerformanceVals(nPerfID, (Perf_FunctionOnLinkedData)nFunctionType, PerformanceVals);
                return dReturn;
            }
            catch (Exception ex)
            {
                _log.AddString(string.Format("Error processing result time series values for ScenarioID {0}, PerfID {1}. Exception: {2}", nScenarioID, nPerfID, ex.Message), Logging._nLogging_Level_1);
                throw; //SP 5-Aug-2016 throw error to ProcessScenario I don't think we want these to just fall through without being caught - open to other suggestions though
                return dAggregateVal;
            }

            //SP 18-Feb-2016 Changed switch and case options to enum
            /*switch ((Perf_FunctionOnLinkedData)nFunctionType)
            {
                case Perf_FunctionOnLinkedData.Sum:     //summate all TS values that fall outside threshold
                    /*if (bExceedsThreshold)
                    {*/
            //dAggregateVal = ResultTSRecords.Select
            //    (c => TimeSeries.SummaryStatForTS(GetTS_FromMemory(c.GroupID), (int)nFunctionType) * c.scalar)
            //    .Sum();
            /*}
            else
            {
                dAggregateVal = PerfRecords.Select
                    (c => TimeSeries.SummaryStatForTS(GetTS_FromMemory(c.GroupID), (int)nFunctionType) * c.scalar)
                    .Where(x => x < dThreshold)
                    .Sum();
            }*/
            //break;
            //case Perf_FunctionOnLinkedData.Min:     //SP 22-Feb-2016 - minimum of all TS values outside threshold
            /*if (bExceedsThreshold)
            {*/
            //dAggregateVal = ResultTSRecords.Select
            //    (c => TimeSeries.SummaryStatForTS(GetTS_FromMemory(c.GroupID), (int)nFunctionType, bApplyThreshold, dThreshold, bExceedsThreshold) * c.scalar)
            //    .Min();
            /*}
            else
            {
                dAggregateVal = PerfRecords.Select
                    (c => TimeSeries.SummaryStatForTS(GetTS_FromMemory(c.GroupID), (int)nFunctionType, dThreshold, bExceedsThreshold) * c.scalar)
                    .Where(x => x < dThreshold)
                    .Min();
            }*/
            //break;

            //case Perf_FunctionOnLinkedData.Max:     //SP 22-Feb-2016 - Maximum of all TS values outside threshold
            /*if (bExceedsThreshold)
            {*/
            //dAggregateVal = ResultTSRecords.Select
            //    (c => TimeSeries.SummaryStatForTS(GetTS_FromMemory(c.GroupID), (int)nFunctionType, bApplyThreshold, dThreshold, bExceedsThreshold) * c.scalar)
            //    .Max();
            /*}
            else
            {
                dAggregateVal = PerfRecords.Select
                    (c => TimeSeries.SummaryStatForTS(GetTS_FromMemory(c.GroupID), (int)nFunctionType) * c.scalar)
                    .Where(x => x < dThreshold)
                    .Max();
            }*/
            // break;
            //case Perf_FunctionOnLinkedData.Count:   //SP 22-Feb-2016 - Count all TS values outside threshold
            /*if (bExceedsThreshold)
            {*/
            //dAggregateVal = ResultTSRecords.Select
            //    (c => TimeSeries.SummaryStatForTS(GetTS_FromMemory(c.GroupID), (int)nFunctionType, bApplyThreshold, dThreshold, bExceedsThreshold))
            //    .Sum();
            /*}
            else
            {
                dAggregateVal = PerfRecords.Select
                    (c => TimeSeries.SummaryStatForTS(GetTS_FromMemory(c.GroupID), (int)nFunctionType))
                    .Where(x => x < dThreshold)
                    .Sum();
            }*/
            //break;
            //}

            //dReturn = Convert.ToDouble(dAggregateVal);
            //return dReturn;*/
        }

        private void Performance_CalcResults(int nEvalID, int nScenarioID)
        {

            int nLastSQN = -1;                              //only need to close db if using ACCESS and last sqn is new
            if (_dsEG_Performance_Request != null)
            {
                foreach (DataRow dr in _dsEG_Performance_Request.Tables[0].Rows)
                {


                    int nPerfCalcType = Convert.ToInt32(dr["PF_FunctionType"]);
                    int nLookupFunction = Convert.ToInt32(dr["ResultFunctionKey"]);             //sum, count etc... applies to lookup only  MET 5/23/16: could be applied using functinon code instead?
                    int nPerfID = Convert.ToInt32(dr["PerformanceID"].ToString());
                    int nLinkTableCode = Convert.ToInt32(dr["LinkTableCode"].ToString());
                    double dThreshold = Convert.ToDouble(dr["Threshold"]);               //SP 16-Jun-2016 - Applies to aggregate of linked records
                    bool bApplyThreshold = Convert.ToBoolean(dr["ApplyThreshold"]);      //SP 16-Jun-2016 - Applies to aggregate of linked records - reconciled   //met 5/23/16 presently the threshold is applied in two different locations for lkp/function... not good.  
                    bool bIsOverThreshold = Convert.ToBoolean(dr["IsOver_Threshold"]);              //SP 16-Jun-2016 - Applies to aggregate of linked records
                    string sArgs = dr["FunctionArgs"].ToString();
                    int nDVID_FK = Convert.ToInt32(dr["DV_ID_FK"].ToString());
                    int nOptionID = Convert.ToInt32(dr["OptionID_FK"].ToString());
                    if (nPerfID == 8916)
                        nPerfID = nPerfID;

                    //SP 16-Jun-2016 - for backwards compatibility
                    bool bUseQuickParse;
                    if (dr["UseQuickParse"].ToString() != string.Empty)
                    {
                        bUseQuickParse = Convert.ToBoolean(dr["UseQuickParse"].ToString());            //defined on the function 
                    }
                    else
                        bUseQuickParse = false;

                    int nSQN = Convert.ToInt32(dr["SQN"].ToString());

                    //SP 16-Jun-2016 Applied default and override values for component threshold parameters when querying from DB
                    //double dThreshold_Component = Convert.ToDouble(dr["ComponentThreshold"].ToString());
                    //bool bApplyThreshold_Component = Convert.ToBoolean(dr["ComponentApplyThreshold"].ToString());       
                    //bool bIsOverThreshold_Component = Convert.ToBoolean(dr["ComponentIsOver_Threshold"]);

                    //SP 15-Jun-2016 TODO is this still ok with not closing and reopening database - should be fine as all references lookup _dsSCEN_PerformanceDetails rather than query the DB
                    //SP 7-Jun-2016 no longer needed as all information comes from memory and not the DB
                    /*if (nSQN != nLastSQN) 
                        _dbContext.OpenCloseDBConnection();*/

                    //string sqlPerf = "";

                    double dTheVal = 0;

                    switch (nPerfCalcType)
                    {
                        case 3:         // cohort? let's try it
                            //todo: consider 
                            //currently does not replace args... that will be one step towards integrating with type 1 function eval
                            int nSequence = _cohortSpec._dictScenSummary_Order[nScenarioID];
                            dTheVal = ParseCohortFunctions(dr["CustomFunction"].ToString(), nSequence);
                            break;
                        case 2:             // operation on linked table
                            // test and remove
                            try
                            {
                                _log.AddString("Calculating PerformanceID " + nPerfID, Logging._nLogging_Level_2, false);
                                dTheVal = Performance_CalcLinkedTables(nScenarioID, nPerfID, nLinkTableCode, nLookupFunction, dr);
                            }
                            catch (Exception ex)
                            {
                                _log.AddString(string.Format("Error calculating performance variable {0}: {1}", nPerfID, ex.Message), Logging._nLogging_Level_1);
                                throw; //SP 5-Aug-2016 rethrow the error so that is handled in the Process Evaluation Group routine
                            }
                            break;
                        case 1:             //execute function
  //met 2/3/18                          string sArgs = dr["FunctionArgs"].ToString();
                            string sFunction = dr["CustomFunction"].ToString();                       //PerfFunction
                            if (ParserIsValidExpression(sFunction))
                            {
                                if (bUseQuickParse)
                                {
                                    string sExpression = Parse_PrepareExpressionValues(nScenarioID, nEvalID, sFunction, sArgs);
                                    dTheVal = QuickParse(sExpression);
                                }
                                else
                                {
                                    try
                                    {
                                        string sTheVal = Parse_EvaluateExpression(nScenarioID, nEvalID, sFunction, sArgs, nDVID_FK, nOptionID);                 //met 4/14/2013 current
                                        if (sTheVal == "NaN") { dTheVal = CommonUtilities._dNaN; }      //this is not caught by subsequent check
                                        else if (CommonUtilities.IsDouble(sTheVal)) { dTheVal = Convert.ToDouble(sTheVal); }
                                        else { dTheVal = CommonUtilities._dBAD_DATA; }                                       //convert to touble if needed
                                    }
                                    catch (Exception ex)
                                    {
                                        _log.AddString(string.Format("Error calculating PerformanceID {0} with function {1}", nPerfID, sFunction), Logging._nLogging_Level_1, false);
                                    }
                                }
                            }
                            else { dTheVal = CommonUtilities._dBAD_DATA; }
                            break;
                        //SP 22-Feb-2016 - added default option

                        case 4:   // execute a batch function (primarily intended to be a function arg...)
                       //     string sArgs4 = dr["FunctionArgs"].ToString();
                            string[] sArgVals = sArgs.Split('?');  // good char to split on
                            string sBat = sArgVals[0]; //  first arg has to be be filename... HELPER_GetBat(sArgs);
                            List<simlinkDetail> lstDetails = HELPER_GetSimLinkDetails(sArgVals, nScenarioID);
                            string[] sArgList = CreateArgList(lstDetails); 
                          // string sResult = "";
                            File.WriteAllLines(@"c:\temp\python_args.csv", sArgList);
                            bool bReturn = CommonUtilities.RunBatchFile(sBat, true);
                            string[] sResult = File.ReadAllLines(@"c:\temp\python_out.csv");
                            
                            dTheVal = GetValFromStream(sResult);    // todo: handle multitudinous output back.
                            break;

                        case 5:     // cost someting
                            double d1=0; double d2=0; Dictionary<string,double> dictParams=new Dictionary<string,double>(); string sUID="";
                            if (nPerfID == 108 || nPerfID == 101)
                            {
                                int n = 1;
                            }

                            try
                            {
                                sUID = GetCostParamsFromArgs(nPerfID, sArgs, ref d1, ref d2, ref dictParams, nDVID_FK, nOptionID);
                              //  dTheVal = _cost_wrap.CostAsset(sUID, d1, d2, dictParams);
                            }
                            catch (Exception ex)
                            {
                                _log.AddString(string.Format("Error getting cost params for perf_id {0}", nPerfID), Logging._nLogging_Level_1, false, true);
                            }
                            if (d1!=0)  // ASSUME taht d1 =0 means that alt is off... limitation for now. more explicit testing could be added.
                                try
                                {
                                    dTheVal = _cost_wrap.CostAsset(sUID, d1, d2, dictParams);
                                }
                                catch (Exception ex)
                                {
                                    _log.AddString(string.Format("Error calculating cost for perf_id {0} d1 {1} d2 {2}", nPerfID, d1, d2), Logging._nLogging_Level_1, false, true);
                                }
                            else
                            {
                                dTheVal = 0;        // insert a value of zero (up to perf val whether recorded.
                            }
                            break;
                        default:
                            _log.AddString(string.Format("Not a valid option for PF_FunctionType in tblPerformance for PerformanceID {0}",
                            dr["PerformanceID"].ToString()), Logging._nLogging_Level_1);
                            break;

                        //SP 22-Feb-2016 - no longer an option - can be performed on LinkedRecords
                        /*case 3:

                            string sSQL_Aggregation = "SELECT tblPerformance.PerformanceID, tblPerformance.Performance_Label, tblPerformance.PF_Type, tblPerformance.LinkTableCode, tblPerformance.PF_FunctionType, tblPerformance.EvalID_FK, tblPerformance.CategoryID_FK, tblPerformance.SQN, tblPerformance.ResultFunctionKey, tblFunctions.CustomFunction, tblPerformance_ResultXREF.LinkTableID_FK"
                                                        + " FROM (tblPerformance LEFT JOIN tblFunctions ON tblPerformance.FunctionID_FK = tblFunctions.FunctionID) INNER JOIN tblPerformance_ResultXREF ON tblPerformance.PerformanceID = tblPerformance_ResultXREF.PerformanceID_FK"
                                                        + " WHERE (((tblPerformance.PerformanceID)=" + nPerfID + ") AND ((tblPerformance.PF_FunctionType)=3))"
                                                        + " ORDER BY tblPerformance.PerformanceID";
                            DataSet dsAggregation = _dbContext.getDataSetfromSQL(sSQL_Aggregation);

                            sFunction = dr["CustomFunction"].ToString();

                            dTheVal = PerformanceProcessAggregate(dsAggregation, nScenarioID, nLinkTableCode, sFunction);

                            break;*/
                    }

                    //SP 13-Jul-2016 removed DVID_FK, IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK from DB Schema
                    //int nResultID = -1;     //because there are many, we cannot list for each 

                    //  TODO:  optionally insert values- probably all at once at end of loop

                    //SP 15-Jun-2016 - should always be saved to memory dataset and added to simlink detail list
                    //if (_IntermediateStorageSpecification._nPerformanceCode != Convert.ToInt32(IntermediateStorageSpecification.IntermediateStorageSpecENUM.None))           //sim2: consider do this at end? for now this works well
                    //{
                    // met added 5/16/16: not sure the logic is properly thought out on this
                    // goal today: do not store 8,000 uninteresting values to store 5 interesting values. option to assume lack of stored val is 'not interesting'
                    // goal soon: ability to unambiguously define thresholds for being under or above

                    //update met 5/23/16: the question here is whether the record gets inserted.
                    //in the lookup logic (sp recent upgrade), it seems more a question orf calculation. these must be reconciled/clear
                    //SP 15-Jun-2016 - reconciled this logic to use the overall 'applythreshold' to determine whether the aggregate meets a certain criteria before reporting the performance 
                    if (bApplyThreshold)
                    {
                        if (bIsOverThreshold)
                        {
                            if (dTheVal > dThreshold)
                                InsertPerformanceDetail(dTheVal, nPerfID, /*-1,*/ nScenarioID/*, nResultID, -1, false*/); //SP 13-Jul-2016 removed DVID_FK, IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK from DB Schema 
                            else
                                _log.AddString("Performance var does not exceed threshold; not stored", Logging._nLogging_Level_3);
                        }
                        else
                        {
                            if (dTheVal > dThreshold)
                                InsertPerformanceDetail(dTheVal, nPerfID, /*-1,*/ nScenarioID/*, nResultID, -1, false*/); //SP 13-Jul-2016 removed DVID_FK, IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK from DB Schema
                            else
                                _log.AddString("Performance var does not exceed threshold; not stored", Logging._nLogging_Level_3, true, false);
                        }
                    }
                    else
                    {
                        InsertPerformanceDetail(dTheVal, nPerfID, /*-1,*/ nScenarioID/*, nResultID, -1, false*/); //SP 13-Jul-2016 removed DVID_FK, IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK from DB Schema
                    }
                    //}
                    nLastSQN = nSQN;
                }
            }
            else
            {
                // log the issue that it is not set (not necessarily an error if no db backend
            }
        }

        #region Cost_ARGS
        private string GetCostParamsFromArgs(int nPerformanceID, string sArgs, ref double d1, ref double d2, ref Dictionary<string, double> dictParams, int nDVD_FK, int nOptionID)
        {
            sArgs = sArgs.Trim();   //remove any incidental whitespace
            string sUID_return = costarghelp_GetUID(ref sArgs, nPerformanceID);
            int nLoops = sArgs.Count(f => f == '[');
            int nCurrentIndex = 0;
            for (int i = 0; i < nLoops; i++)
            {
                int nIndexStart = sArgs.Substring(nCurrentIndex).IndexOf('[');
                int nIndexEnd = sArgs.Substring(nCurrentIndex).IndexOf(']');
                int nNumberChar = nIndexEnd - nIndexStart-1;
                string sCurrentArg = sArgs.Substring(nCurrentIndex + nIndexStart + 1, nNumberChar);  // strip out the  [ ] symbols
                nCurrentIndex = nIndexEnd + 1;
                if (i >=2)
                {
                    costarghelp_DictParamAdd(ref dictParams, sCurrentArg);  // not yet supported
                }
                else
                {
                    double dVal = costarghelp_ProcessArg(sCurrentArg, nDVD_FK, nOptionID);
                    if (i == 0)
                        d1 = dVal;
                    else
                        d2 = dVal;
                }
            }
            return sUID_return;
        }
        private string costarghelp_GetUID(ref string sArgs, int nPerformanceID)
        {
            if (sArgs.Substring(0, 1) == "[")
            {
                return nPerformanceID.ToString();
            }
            else
            {
                int nEndIndex = sArgs.IndexOf("[");
                if (nEndIndex<=0){
                    return nPerformanceID.ToString();
                }
                else
                {
                    return sArgs.Substring(0, nEndIndex);
                }
            }
        }

        private double costarghelp_ProcessArg(string sCurrentArg, int nDVD_FK, int nOptionID)
        {
            sCurrentArg = sCurrentArg.Trim();
            double dValReturn = CommonUtilities._dBAD_DATA;
            if (true)
            {
                try
                {
                    sCurrentArg = Parse_PrepareExpressionValues(_nActiveScenarioID, _nActiveEvalID, sCurrentArg, "NONE", nDVD_FK, nOptionID);
                    dValReturn = Parse_Expression(sCurrentArg);
                }
                catch (Exception ex)
                {
                    _log.AddString(string.Format("Error processing cost perf arg {0} exception: {1}", sCurrentArg, ex.Message), Logging._nLogging_Level_2, true, true);
                }
            }
            return dValReturn;
          //  if (sCurrentArg.Substring(sCurrentArg, 1) == "@")  // we have an equation to be parsed by the parse
        //    { }

        }

        /// <summary>
        /// process a function that adds additional detail regarding the alt
        /// </summary>
        /// <param name="dictParams"></param>
        /// <param name="sCurrentArg"></param>
        private void costarghelp_DictParamAdd(ref Dictionary<string, double> dictParams, string sCurrentArg)
        {
            dictParams.Add(sCurrentArg, CommonUtilities._dBAD_DATA);    //    "not supported yet!");
        }


        #endregion

        /// <summary>
        /// Extract result from string returned from external task (Eg python code)
        /// </summary>
        /// <param name="sReturn"></param>
        /// <returns></returns>
        private double GetValFromStream(string[] sReturn)
        {
            //string[] sVals = sReturn.Split(';');
            try
            {
                double dVal = Convert.ToDouble(sReturn[sReturn.Length - 1]);
                return dVal;
            }
            catch (Exception ex)
            {
                _log.AddString("Error getting value(s) backk from external task",Logging._nLogging_Level_1,true,true);
                _log.AddString(string.Format("external task not converted to double: {0}",sReturn),Logging._nLogging_Level_3,true,false);
                return CommonUtilities._dBAD_DATA;
            }
        }

        /// <summary>
        ///  12/5/16:  made public for testing of new math.net stuff
        /// </summary>
        /// <param name="sGroupID"></param>
        /// <returns></returns>
        public double[,] GetTS_FromMemory(string sGroupID)
        {
            int nIndex = Array.IndexOf(_sTS_GroupID, sGroupID);
            if (nIndex >= 0)
                return _dResultTS_Vals[nIndex];
            else
                return null;
        }

        /// <summary>
        /// Retrieve values 
        /// </summary>
        /// <param name="sArgVals"></param>
        /// <param name="nScenarioID"></param>
        /// <returns></returns>
        private List<simlinkDetail> HELPER_GetSimLinkDetails(string[] sArgVals, int nScenarioID)
        {
            List<simlinkDetail> lstDetail = new List<simlinkDetail>();
            for (int i = 1; i < sArgVals.Length; i++)
            {
                string sEXP = sArgVals[i];
                bool bIsBaseline = (sEXP.IndexOf("#") > 0);  //
                bool bIsArray = (sEXP.IndexOf("|") > 0);  //
                int nActiveScenario = nScenarioID;
                sEXP = sEXP.Replace("#", "").Replace("|", "").Trim();
                SimLinkDataType_Major datatype = GetSimlinkDataTypeFromChar(sEXP[0].ToString());
                if(bIsBaseline)
                    nActiveScenario=_nActiveBaselineScenarioID; // set to baseline scenario id.
                sEXP = sEXP.Replace(GetCharDelimiterFromDataType(datatype), "");
                int nVarType_or_RecordID = Convert.ToInt32(sEXP);

                if(!bIsArray){
                    simlinkDetail det = GetSimlinkDetail(datatype, nVarType_or_RecordID, -1, nActiveScenario, -1);
                    if(det != null)
                        lstDetail.Add(det);
                }
                else{
                    List<simlinkDetail> lstDet = GetSimlinkDetail(datatype,-1, nVarType_or_RecordID, nActiveScenario, -1,true);   // return the range
                    if(lstDet!=null)
                        lstDetail.AddRange(lstDet);
                    else
                    {
                        _log.AddString(string.Format("Zero values returned for key {0}", nVarType_or_RecordID.ToString()), Logging._nLogging_Level_3, false, true);
                    }
                }
            }
            return lstDetail;
        }


        /// <summary>
        /// Convert a list of sld into a string of arguments
        /// 
        /// returns list of strings because needed to write out...
        /// </summary>
        /// <param name="lstDetail"></param>
        /// <returns></returns>
        public string[] CreateArgList(List<simlinkDetail> lstDetail)
        {
            string sReturn = "";
            foreach (simlinkDetail det in lstDetail)
            {
                sReturn = sReturn + string.Format("{0},{1},{2},{3},{4};", det._slDataType, det._nScenarioID.ToString(), det._sElementName, det._sVal, det._nVarType_FK.ToString());
            }
            string[] sOut = new string[1];
            sOut[0] = sReturn;
            return sOut;
        }

        /// <summary>
        /// utility functon to export data as xml.
        /// This can be used to deliver a version of simlin which does not use a db backend.
        /// The data is still exposed, but the schema is less clear...
        /// 
        /// // initial tests with multi ds in same xml didn't work .. must be possible, should be figured out...
        /// // version 1 of this is just the info  I need for the MSD project
        /// 
        /// </summary>
        /// <param name="sFilename"></param>
        /// <param name="sCode"></param>
        public void WriteXML(string sDir, string sCode)
        {
            string sFilename = Path.Combine(sDir,"slite_REPLACE.xml");
            string sFileOUT = sFilename.Replace("REPLACE","supporting_file");
            using (StreamWriter writer = new StreamWriter(sFileOUT)){         
                _dsEG_SupportingFileSpec.Tables[0].TableName = "supporting_file";
                _dsEG_SupportingFileSpec.WriteXml(writer);
            }
            sFileOUT = sFilename.Replace("REPLACE","result_ts");
            using (StreamWriter writer = new StreamWriter(sFileOUT)){          
                _dsEG_ResultTS_Request.Tables[0].TableName = "result_ts"; //SP 15-Feb-2017 _dsEG_ResultTS_Request is now the combined ResultTS dataset
                _dsEG_ResultTS_Request.WriteXml(writer);
            }
            sFileOUT = sFilename.Replace("REPLACE","external_data_request");
            using (StreamWriter writer = new StreamWriter(sFileOUT)){ 
                _dsEG_ExternalDataSources.Tables[0].TableName = "external_data_request";
                _dsEG_ExternalDataSources.WriteXml(writer);
            }
            // added 4/13/17 to support realtime work
            sFileOUT = sFilename.Replace("REPLACE", "model_changes");
            using (StreamWriter writer = new StreamWriter(sFileOUT))
            {
                //todo: CHECK THAT AT LEAST ONE IS inserted.. if not this will not work.
                _dsSCEN_ModVals.Tables[0].TableName = "model_changes";
                _dsSCEN_ModVals.WriteXml(writer);
            }

            /*  not yet implemented.
            sFileOUT = sFilename.Replace("REPLACE", "result_vars");
            using (StreamWriter writer = new StreamWriter(sFileOUT))
            {
                _dsSCEN_ResultSummary.Tables[0].TableName = "model_changes";
                _dsSCEN_ResultSummary.WriteXml(writer);
            }
            sFileOUT = sFilename.Replace("REPLACE", "events");
            using (StreamWriter writer = new StreamWriter(sFileOUT))
            {
                _dsSCEN_EventDetails.Tables[0].TableName = "events";
                _dsSCEN_EventDetails.WriteXml(writer);
            }
            sFileOUT = sFilename.Replace("REPLACE", "performance");
            using (StreamWriter writer = new StreamWriter(sFileOUT))
            {
                _dsSCEN_PerformanceDetails.Tables[0].TableName = "performance";
                _dsSCEN_PerformanceDetails.WriteXml(writer);
            }*/
        }

        //met new 11/2/2013: add to aggregate results of functions based upon tblFunctions
        //general case- likely just pull back one TS and process, but this function will perform necessary operation on EACH ts....

        //not sure how this is used; added aggregation ENUM which should be usable with a variety of 
        private double PerformanceProcessAggregate(DataSet dsAggregation, int nScenarioID, int nLinkTableCode, string sFunction)           //DataSet dsAggregation,
        {
            int nCountTS = dsAggregation.Tables[0].Rows.Count;
            double[][,] dTS_Arrays = new double[nCountTS][,];
            SimLinkDataType_Major slDataType = SimLinkDataType_Major.ResultTS;                  //general case
            if (nLinkTableCode == 1) { slDataType = SimLinkDataType_Major.MEV; }

            double dReturn = 0;
            //met : not sure yet whether this will be looop or not... 
            for (int i = 0; i < 1; i++)
            {
                string sResultTS_ID = dsAggregation.Tables[0].Rows[i]["LinkTableID_FK"].ToString();
                string sGroupName = CommonUtilities.GetSimLink_TS_GroupName(slDataType, sResultTS_ID);

                dTS_Arrays[i] = GetTS_FromMemory(sGroupName);

                // met - test out filling wiht a dummy array if needed. nothing will be pulled back if not written out for SWMM for that iteration.
                if (dTS_Arrays[i] == null)
                {
                    dTS_Arrays[i] = new double[1, 1];       //single element dummmy array.
                }


                // met : todo fill in with linq

                switch (sFunction)
                {
                    case "TOTAL":

                        break;
                    case "TOTAL_TIMESCALED":
                        double dTS_Interval = 0.056100368;      //=7.48*3.78*60*15/1000*0.0022046
                        //TODO_CRITICAL: get this value through code
                        for (int j = 0; j < dTS_Arrays[i].GetLength(0); j++)
                        {
                            dReturn = dReturn + dTS_Arrays[i][j, 0];
                        }
                        dReturn = dReturn * dTS_Interval;
                        Console.WriteLine("warning, ts_interval hard coded to 900 sec)");
                        break;
                    case "AVERAGE":

                        break;

                    case "MAXIMUM":

                        break;

                    case "MINIMUM":

                        break;
                }
            }
            return dReturn;
        }


        //sim2: added this; consider adding original function which works with quantity as well
        public void InsertPerformanceDetail(double dPerfVAL, int nPerfID_FK, /*int nDVID_FK,*/ int nScenarioID_FK/*, int nScenarioElementVal_ID, int nLKP_FK, bool bIsLinkToGroup*/)
        {
            if (_dsSCEN_PerformanceDetails == null)              //bojangles: met 12/14/14: triggering for epanet- need to understand why and reconcile vs swmm
            {
                _dsSCEN_PerformanceDetails = EGDS_GetPerformanceDetail();    //init the DS
            }

            _dsSCEN_PerformanceDetails.Tables[0].Rows.Add();
            int nCurrentIndex = _dsSCEN_PerformanceDetails.Tables[0].Rows.Count - 1;
            //SP 13-Jul-2016 removed DVID_FK, IsLinkToGroup, ScenarioElementVal_ID, PerformanceLKP_FK from DB Schema
            _dsSCEN_PerformanceDetails.Tables[0].Rows[nCurrentIndex]["PerformanceID_FK"] = nPerfID_FK;
            //_dsSCEN_PerformanceDetails.Tables[0].Rows[nCurrentIndex]["DVID_FK"] = nDVID_FK;
            _dsSCEN_PerformanceDetails.Tables[0].Rows[nCurrentIndex]["VAL"] = dPerfVAL;
            _dsSCEN_PerformanceDetails.Tables[0].Rows[nCurrentIndex]["ScenarioID_FK"] = nScenarioID_FK;
            //_dsSCEN_PerformanceDetails.Tables[0].Rows[nCurrentIndex]["IsLinkToGroup"] = bIsLinkToGroup;
            //_dsSCEN_PerformanceDetails.Tables[0].Rows[nCurrentIndex]["ScenarioElementVal_ID"] = nScenarioElementVal_ID;
            //_dsSCEN_PerformanceDetails.Tables[0].Rows[nCurrentIndex]["PerformanceLKP_FK"] = nLKP_FK;

            _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.Performance, dPerfVAL.ToString(), nPerfID_FK, -1 /*nScenarioElementVal_ID*/, nScenarioID_FK));

            //bojangles: this is not implemented yet.
            //SP 14-Jun-2016 - save back to the DB at the end
            /*if (_IntermediateStorageSpecification._nPerformanceCode != Convert.ToInt32(IntermediateStorageSpecification.IntermediateStorageSpecENUM.PerformanceOptOnly))
            {
                _dbContext.InsertOrUpdateDBByDataset(true, _dsSCEN_Performance, _sSQL_InsertPerformanceVals, false);
            }*/
        }

        public double ProcessPerformanceVals(int nPerfID, Perf_FunctionOnLinkedData nFunctionType, IEnumerable<PerformanceValues> _lstPerformanceVals/*, bool bApplyThreshold,
            double dThreshold, bool bExceedsThreshold*/)
        {
            //SP 18-Feb-2016 Changed switch and case options to enum
            switch ((Perf_FunctionOnLinkedData)nFunctionType)      //for some reason, must treat val as decimal.... don't know why.
            {
                case Perf_FunctionOnLinkedData.Sum:
                    //added option for each switch to apply threshold
                    /*if (bApplyThreshold)
                    {
                        if (bExceedsThreshold)*/
                    return Convert.ToDouble(_lstPerformanceVals.Where(c => c.bapplythreshold ? (c.bisabove_threshold ? c.dval > c.dthreshold : c.dval < c.dthreshold) : true)
                        .Select(c => c.dval * c.dscalar).Sum());
                /*else
                    return Convert.ToDouble(_lstPerformanceVals.Where(c => c.dval <= dThreshold).Select(c => c.dval * c.dscalar).Sum());
            }
            else
                return Convert.ToDouble(_lstPerformanceVals.Select(c => c.dval * c.dscalar).Sum());*/

                case Perf_FunctionOnLinkedData.Min:
                    return Convert.ToDouble(_lstPerformanceVals.Where(c => c.bapplythreshold ? (c.bisabove_threshold ? c.dval > c.dthreshold : c.dval < c.dthreshold) : true)
                        .Select(c => c.dval * c.dscalar).Min());
                /*if (bApplyThreshold)
                    {
                        if (bExceedsThreshold)
                            return Convert.ToDouble(_lstPerformanceVals.Where(c => c.dval >= dThreshold).Select(c => c.dval * c.dscalar).Min());
                        else
                            return Convert.ToDouble(_lstPerformanceVals.Where(c => c.dval <= dThreshold).Select(c => c.dval * c.dscalar).Min());
                    }
                    else
                        return Convert.ToDouble(_lstPerformanceVals.Select(c => c.dval * c.dscalar).Min());*/

                case Perf_FunctionOnLinkedData.Max:
                    return Convert.ToDouble(_lstPerformanceVals.Where(c => c.bapplythreshold ? (c.bisabove_threshold ? c.dval > c.dthreshold : c.dval < c.dthreshold) : true)
                        .Select(c => c.dval * c.dscalar).Max());
                /*if (bApplyThreshold)
                {
                    if (bExceedsThreshold)
                        return Convert.ToDouble(_lstPerformanceVals.Where(c => c.dval >= dThreshold).Select(c => c.dval * c.dscalar).Max());
                    else
                        return Convert.ToDouble(_lstPerformanceVals.Where(c => c.dval <= dThreshold).Select(c => c.dval * c.dscalar).Max());
                }
                else
                    return Convert.ToDouble(_lstPerformanceVals.Select(c => c.dval * c.dscalar).Max());*/

                case Perf_FunctionOnLinkedData.Count:
                    return Convert.ToDouble(_lstPerformanceVals.Where(c => c.bapplythreshold ? (c.bisabove_threshold ? c.dval > c.dthreshold : c.dval < c.dthreshold) : true)
                        .Select(c => c.dval * c.dscalar).Count());      //corrected 11/6/17 met
                /*if (bApplyThreshold)
                {
                    if (bExceedsThreshold)
                        return Convert.ToDouble(_lstPerformanceVals.Where(c => c.dval >= dThreshold).Count());
                    else
                        return Convert.ToDouble(_lstPerformanceVals.Where(c => c.dval <= dThreshold).Count());
                }
                else
                    return Convert.ToDouble(_lstPerformanceVals.Count());*/

                default:
                    _log.AddString(string.Format("Not a valid option for PF_FunctionType in tblPerformance for PerformanceID {0}",
                        nPerfID), Logging._nLogging_Level_1);
                    return CommonUtilities._dBAD_DATA; ;
            }
        }

        #endregion

        /// <summary>
        /// Generalized Simlink function for setting up a simlink dir and doing some naming.  MET 5/17/17
        /// Currently handled by derived classes- in future they may call this.
        /// 
        /// </summary>
        /// <param name="sTargetFile"></param>
        public void SetupScenarioFiles(string sIncomingTargetFile, int nScenarioID, out string sTargetPath, out string sTargetFilename, bool bOverwriteexisting = true)
        {
            sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, _nActiveEvalID, true);   //met 4/29/14- was being done manually. confirm and delete prev line
            if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
            string sModelBase = Path.GetFileNameWithoutExtension(_sActiveModelLocation);
            string sExtension = Path.GetExtension(_sActiveModelLocation);
            sTargetFilename = Path.Combine(sTargetPath, CommonUtilities.GetSimLinkFileName(sModelBase + sExtension, nScenarioID));
            // sTargetFilename = CommonUtilities.GetSimLinkFileName(sTargetFilename, nScenarioID);
            _log.AddString(string.Format("Copying user-request file {0} to location {1}", sIncomingTargetFile, sTargetFilename), Logging._nLogging_Level_2, true, true);
            string sTempTargetFilename = "";
            //SP 24-May-2017 Copy files within target folder - TODO!!! There is a disconnect between sIncomingTargetFile and _sActiveModelLocation if the EG already exists Needs addressing
            string[] files = null;
            switch ((AssociatedFileSupport)_nAssociatedFileSupport)
            {
                case AssociatedFileSupport.SingleFile:
                    FileInfo flinfo = new FileInfo(sIncomingTargetFile);
                    flinfo.CopyTo(sTargetFilename, bOverwriteexisting);
                    break;
                case AssociatedFileSupport.FilesWithSameFileName:    // met 7/24/17: not quite sure how this is getting set.... //SP 3-Aug-2017 Is a Simlink class property. Can change to input to this function if needed
                    files = Directory.GetFiles(Path.GetDirectoryName(sIncomingTargetFile), sModelBase + ".*", SearchOption.TopDirectoryOnly);
                    foreach (string sFile in files)
                    {
                        sExtension = Path.GetExtension(sFile);
                        sTempTargetFilename = Path.Combine(sTargetPath, CommonUtilities.GetSimLinkFileName(Path.GetFileNameWithoutExtension(sFile) + sExtension, nScenarioID));
                        FileInfo flinfo2 = new FileInfo(sFile);
                        flinfo2.CopyTo(sTempTargetFilename, bOverwriteexisting);
                    }
                    break;
                case AssociatedFileSupport.AllFilesInFolder:
                    files = Directory.GetFiles(Path.GetDirectoryName(sIncomingTargetFile), "*", SearchOption.TopDirectoryOnly);
                    foreach (string sFile in files)
                    {
                        sExtension = Path.GetExtension(sFile);
                        sTempTargetFilename = Path.Combine(sTargetPath, CommonUtilities.GetSimLinkFileName(Path.GetFileNameWithoutExtension(sFile) + sExtension, nScenarioID));
                        FileInfo flinfo3 = new FileInfo(sFile);
                        flinfo3.CopyTo(sTempTargetFilename, bOverwriteexisting);
                    }
                    break;
            }
        }

        //SP 8-Jul-2017 overload this function to allow optional return target path and target file name parameters
        public void SetupScenarioFiles(string sIncomingTargetFile, int nScenarioID, bool bOverwriteexisting = true)
        {
            string sTemp = "";
            string sTemp2 = "";
            SetupScenarioFiles(sIncomingTargetFile, nScenarioID, out sTemp, out sTemp2, bOverwriteexisting);
        }

        /// <summary>
        /// Function to insert/retrieve the next scenario and return this value.
        /// NOTE: In most cases, this should not be called- process scenario handles.
        /// This was added especially in the case where a means of persisting scen with no db backend was needed. (bIsSimlinkLite
        /// </summary>
        /// <returns></returns>
        public int InsertScenario(string sLabel="DEFAULT", int nScenStartAct = -1, int nScenEndAct = -1)
        {
            if (_nActiveEvalID == -1)
            {
                Console.WriteLine("EG must be defined to insert a scenario");
                return -1;
            }

            if (_bIsSimLinkLite)
            {
                //bojangles.   FIX the hardcoding.
                InsertScenario_SimlinkLite(_sSimlinkLiteDir);
            }
            else
            {
                //SP 10-Oct-2017 - added option to insert scenario with specified start and end LC steps
                //TODO the Virtual and Override ProcessScenario routines assume -1 and 100 when inserting. CAN BE TIDIED but a lot of changes
                if (nScenStartAct != -1 || nScenEndAct != -1) //check for non-defaults for function
                {
                    int nScenarioID = InsertScenario(_nActiveEvalID, _nActiveProjID, sLabel, "", "-1", nScenStartAct, nScenEndAct);
                    _nActiveScenarioID = nScenarioID;
                }
                else
                    ProcessScenario(_nActiveProjID, _nActiveEvalID, _nActiveReferenceEvalID, _sActiveModelLocation, -1, 1, 1, "-1", sLabel);    // create a new scenario
            }
            return _nActiveScenarioID;
        }

        /// <summary>
        /// Insert a scenario and apply a set of timeseries results synthesized from another set of runs.
        /// This is to be used when the results for this scenario come from runs in another EG, though cannot be fabricated directly from cohorts
        ///     //cohort idea for realtime- set cohorts to a given duration so runs are automatically grouped as cohorts
        ///     // facilitating export and mgmt of "blocks of runs" ?
        /// </summary>
        /// <param name="sLabel"></param>
        /// <param name="dVal"></param>
        /// <returns></returns>
        public int InsertScenario(string sLabel, double[][,] dVals, string[] sGroupNames, bool bWriteToRepo=true)
        {
            int nScenarioID = InsertScenario(sLabel);
            _dResultTS_Vals = dVals;
            _sTS_GroupID = sGroupNames;
            if (bWriteToRepo)
            {
                string sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, _nActiveEvalID, true);
                if (!Directory.Exists(sTargetPath))
                    Directory.CreateDirectory(sTargetPath);
                SetTS_FileName(nScenarioID, sTargetPath, true);
                _hdf5.hdfCheckOrCreateH5(_hdf5._sHDF_FileName);
                _hdf5.hdfOpen(_hdf5._sHDF_FileName, false, true);
                ProcessNonDV_FileDependency(sTargetPath, nScenarioID); //SP 13-Oct-2017
                WriteTimeSeriesToRepo();
                if (true)
                    WriteSecondaryAndAUXTimeSeriesToRepo();
                _hdf5.hdfClose();
            }
            return nScenarioID;
        }

        public void WriteTS_to_CSV(int nEvalID, string sFilename)
        {
            string[] a= new string[] { };
            DataSet dsEvals = ProcessEG_GetGS_Initialize(_nActiveEvalID, a,false);       //, nRefScenarioID);
            int nRecordCount =dsEvals.Tables[0].Rows.Count;
            double[][,] dRetrieve = new double[nRecordCount][,];       // create one ds for each scenario  (update if you want to support multiple ts
                                                                        // created as jagged array of 2d to reflect simlink data structure
            int nCounter = 0;
            string sHeader = "";
            foreach (DataRow dr in dsEvals.Tables[0].Rows)
            {
                int nScenario = Convert.ToInt32(dr["ScenarioID"].ToString());
                SetTS_FileName(nScenario);
                LoadScenarioDatasets(nScenario, 20);                        // get the ts in memory
                int nIndex = 0;     //todo: get index properly
                dRetrieve[nCounter] = _dResultTS_Vals[nIndex];
                sHeader=sHeader+nScenario.ToString()+",";
                nCounter++;
            }
            sHeader = sHeader.Substring(0, sHeader.Length - 1);
            using (StreamWriter writer = new StreamWriter(sFilename))
            {
                int nRows = dRetrieve[0].GetLength(0);
                writer.WriteLine(sHeader);
                for (int i = 0; i < nRows; i++)
                {
                    string sBuf = "";
                    for (int j = 0; j < nRecordCount; j++){
                        sBuf = sBuf + dRetrieve[j][i,0].ToString() + ",";
                    }
                    sBuf = sBuf.Substring(0,sBuf.Length - 1);       // drop last comma
                    writer.WriteLine(sBuf);
                }
            }
        }

        /// <summary>
        /// Export anything defined in tblSupportingFileSpec
        /// Added for realtime support
        /// </summary>
        //SP 20-Dec-2016 bulk write out the TS Results to the various export specifications
        //MET 2/12/2018: Update the code to avoid error when nothing happening here... (ICM integration). Untested following update, should be no issue.

        public void WriteOutputData_Grouped()
        {
            try
            {
                if (_lstExternalDataDestinations.Count() > 0)
                {
                    string sTargetDir = Path.Combine(GetWorkingDir(), _sOutputDir);
                    if (!Directory.Exists(sTargetDir))
                        Directory.CreateDirectory(sTargetDir);

                    Dictionary<string, string> dictParams = new Dictionary<string, string>();

                    //SP 20-Dec-2016 get a complete list of TS Results in an array that will be passed out - now changed to using a list of ExternalData
                    List<ExternalData> lstSupportingFileSpec_Output = _lstExternalDataDestinations.Where(x => !x._bIsInput &&
                        Array.IndexOf((int[])Enum.GetValues(typeof(ExternalDataType)), x._externaldatatype) > -1).ToList();

                    double[][,] dValsSupportingFileSpec = new double[lstSupportingFileSpec_Output.Count()][,];

                    int nCounter = 0;
                    string sHeader = "Timestamp";           // todo: support a header based on dict_request item...
                    int nRecords = 0;
                    //double dScalar = 7.48052 * 300 *Math.Pow(10,-6);         // convert CFS to MGAL assuming a 5 min timestep   // SP 14-Apr-2017 now param argument group_scalar
                    //double dScalar = 1.0 / (24.0 * 60.0 * 60.0) * 300.0;         // convert MGD to MGAL assuming a 5 min timestep   // SP 14-Apr-2017 now param argument group_scalar

                    foreach (ExternalData ex in lstSupportingFileSpec_Output)
                    {
                        int nIndex = _dictResultTS_Indices[ex._nTSRecordID];
                        dValsSupportingFileSpec[nCounter] = _dResultTS_Vals[nIndex];

                        //SP 29-Mar-2017 Loop through each dValsSupportingFileSpec once - currently its applying the scalar more than once if the TS appears more than once // SP 14-Apr-2017 now param argument group_scala
                        //SP - looking to refactor anyway but in meantime this should be done within ExecuteWritingSupportingFileSpec - Moved to avoid applying scalar more than once
                        /*for (int i = 0; i < dValsSupportingFileSpec[nCounter].Length; i++)
                        {
                            dValsSupportingFileSpec[nCounter][i, 0] = dValsSupportingFileSpec[nCounter][i, 0] * dScalar;
                        }*/

                        nCounter++;
                        sHeader += "," + ex._nTSRecordID.ToString();
                        nRecords = _dResultTS_Vals[nIndex].GetLength(0);
                    }
                    List<string> lstDT = GetDateTimeList(nRecords);
                    if (lstSupportingFileSpec_Output.Count() > 0)
                        lstSupportingFileSpec_Output[0].ExecuteWritingSupportingFileSpec(dValsSupportingFileSpec, /*sHeader,*/lstDT, lstSupportingFileSpec_Output, sTargetDir);

                }
            }
                catch(Exception ex)
            {
                //todo- log
            }
        }

        /// <summary>
        /// Creeate a list of timstamps for first col of output (if needed)
        /// Return as string for flexibility w diferent "label columns"
        /// </summary>
        /// <param name="nPeriods"></param>
        /// <returns></returns>
        private List<string> GetDateTimeList(int nPeriods)
        {
            List<string> lstDT = new List<string>();
            DateTime dtToWrite = _tsdSimDetails._dtStartTimestamp;        // ASSUME start at sim start.
            TimeSpan tStep = new TimeSpan(0, 0, (int)_tsdSimDetails._nTSIntervalInSeconds);

            for (int i = 0; i < nPeriods; i++)
            {
                //SP 14-Mar-2017 added option to write date stamps based on the value corresponding to end of the interval
                if (_bTSStartOfInterval)
                {
                    lstDT.Add(dtToWrite.ToString());
                }
                else
                {
                    lstDT.Add((dtToWrite + tStep).ToString());
                }
                dtToWrite += tStep;
            }
            return lstDT;
        }

        //SP 1-Mar-2017 No longer used. Instead using WriteOutputData_Grouped
        /// <summary>
        /// Export anything defined in tblSupportingFileSpec
        /// Added for realtime support
        /// </summary>
        ///
        /*public void WriteOutputData()
        {
            string sTargetDir = Path.Combine(GetWorkingDir(), _sOutputDir);
            if (!Directory.Exists(sTargetDir))
                Directory.CreateDirectory(sTargetDir);
            Dictionary<string, string> dictParams = new Dictionary<string, string>();
            foreach (DataRow dr in _dsEG_SupportingFileSpec.Tables[0].Select("DataType_Code > 0 and IsInput=False"))
            {
                ExternalDataType outType = ExternalDataType.CSV;
                DataFormat dataType = DataFormat.Timeseries;
                // not yet supported...   int nAuxID = Convert.ToInt32(dr["AuxID_FK"].ToString());
                int nRecordID = Convert.ToInt32(dr["RecordID_FK"].ToString());      //update for different data sources
                ExternalData ex = _lstExternalDataSources.Find(x => x._nUID == -1); //nAuxID);
                int nIndex = _dictResultTS_Indices[nRecordID];
                if (ex != null)
                {
                    // todo: update vars outType and dataType
                }
                switch (outType)
                {
                    case ExternalDataType.CSV:
                        dictParams["filename"] = GetOutputFilename(sTargetDir, ex, dr);

                        if (ex != null)
                            ex.Write();
                        else
                            external_csv.Write_CSV(nRecordID.ToString(), _tsdResultTS_SIM, _dResultTS_Vals[nIndex], dictParams);
                        break;
                }
            }
        }*/


        /// <summary>
        /// function for getting the current work dir
        /// can be overridden if needed  / specialized etc
        /// </summary>
        /// <returns></returns>
        protected virtual string GetWorkingDir(bool bUseScen = true)
        {
            string sPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, _nActiveScenarioID, _nActiveEvalID, bUseScen);
            return sPath;
        }

        // todo: refine, possibly push to externaldata....
        private string GetOutputFilename(string sTargetDir,ExternalData ex,DataRow dr){
            string sReturn = "";
            if (ex != null)
            {

            }
            else
            {
                sReturn = Path.Combine(sTargetDir, dr["filename"].ToString()) + ".csv";
            }
            return sReturn;
        }


        //majority of functionality in CU class; this complements that with SL-based info...
        #region TEMPLATE

        //formerly 
        //bad- we have two data structures here- have not yet decided which one meets all needs.
        //todo: resolve.
        protected void TEMPLATE_InsertMEV(string[] sTemplateNames, string[] sTemplatVals, int nScenarioID)          //int[]
        {
            for (int i = 0; i <= sTemplateNames.GetUpperBound(0); i++)
            {
                _dictTemplate.Add(sTemplateNames[i], sTemplatVals[i]);
                _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.MEV, sTemplatVals[i], -2, -2, nScenarioID, -2, sTemplateNames[i]));
            }

        }

        #endregion


    }
}
