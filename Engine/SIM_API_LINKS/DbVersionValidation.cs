using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIM_API_LINKS.DAL;
using System.Data;




namespace SIM_API_LINKS.DBVV
{
    
    public enum FieldRequirement
    {
        Present,
        Absent,
        Modified
    } 

    //SP 11-Apr-2016 Created a new class encapsulating the required tests for database version and tables required for model type
    /*-----------------------------------------------------------------------------------
     * INSTRUCTIONS FOR UPDATING THIS CLASS
     * 1) If script adds a new table - create a new case entry for script and list of tables added in function ContentsOfScriptExistsInDB SECTION 1
     * 2) If script adds or removes a field to a table - create a new case entry for script and list of fields in function ContentsOfScriptExistsInDB SECTION 2
     * 3) If script adds records to a field in a table - create a new case entry for script and list of fields in function ContentsOfScriptExistsInDB SECTION 3
     * 4) If script changes a datatype for a field in a table - TODO, haven't been able to figure out a way that is compatible with both SQLServer and MSAccess (prob need specific fuctions)
     * 5) If script is generic for all model types add the Script Number to the string sScriptVersions_String
     * 6) If script is specific for a model type add the Script Number to the appropriate case in function PerformSpecificModelRequirementTests
     * ----------------------------------------------------------------------------------*/
    public class DbValidation
    {
        private DBContext _dbContextForValidation;
        
        //using DBContext check that the DB is the latest version
        public bool ValidateForVersion(DBContext dbDatabaseConnection, ref string sPotentialIssues)
        {
            _dbContextForValidation = dbDatabaseConnection;

            //List of scripts in current version of Simlink
            string sScriptVersions_String = "00.00.10, 00.00.15, 00.00.20, 00.00.25, 00.00.30, 00.00.35, 00.00.40, 00.00.45, 00.00.50, 00.00.51, 00.00.55, " +
                "00.00.60, 00.00.61, 00.00.63, 00.00.64, 00.00.65, 00.00.67, 00.00.68, 00.00.69, 00.00.70, 00.00.71, 00.00.72, 00.00.73, 00.00.74, 00.00.75, " +
                "00.00.76, 00.00.80, 00.00.81, 00.00.85, 00.00.90, 00.00.95, 00.00.96, 00.00.97, 00.01.00, 00.01.01, 00.01.02, 00.01.04, 00.01.05, 00.01.11, 00.01.12, 00.01.13, 00.01.21, " +
                "00.01.22, 00.01.23, 00.01.24, 00.01.25, 00.01.26, 00.01.27, 00.01.29, 00.01.30, 00.01.31, 00.01.32, 00.01.34, 00.01.35";
                
            return IterateThroughScripts(sScriptVersions_String, ref sPotentialIssues);
        }


        //using DBContext check that the DB contains the required dictionary and model structure tables
        public bool ValidateForModelType(DBContext dbDatabaseConnection, SimulationType stSimType, ref string sPotentialIssues)
        {
            _dbContextForValidation = dbDatabaseConnection;
            return PerformSpecificModelRequirementTests(stSimType, ref sPotentialIssues);
        }


        private bool ContentsOfScriptExistsInDB(string sScriptNumber, ref string sPotentialIssues)
        {
            string sTables = "";
            bool nreturn = true;
            bool bAssessed = false;

            //----------------------------------------------------------------------------------------------------------
            //--------------------------------SECTION 1: Check for Tables existance ----------------------------
            //----------------------------------------------------------------------------------------------------------
            switch (sScriptNumber)
            {
                case "000000":
                    sTables = "tblVersion";
                    bAssessed = true;
                    break;

                case "000010":
                    sTables = "tlkpSimLinkTableDictionary, tlkpSimLinkFieldDictionary, tblSimLink_TableMaster";
                    bAssessed = true;
                    break;

                case "000015":
                    sTables = "tlkpUnitTypes, tlkpUnitSettings, tlkpUnitConversions, tlkpUI_Dictionary, tlkpDistributionDefinition, tlkpDistribStrategy, tlkpCostData";
                    bAssessed = true;
                    break;

                case "000020":
                    sTables = "tblFunctions, tblConstants";
                    bAssessed = true;
                    break;

                case "000025":
                    sTables = "tblOptionLists, tblOptionDetails, tblElementLists, tblElementListDetails, tblElementLibrary, tblElement_XREF_DomainLinks, tblElement_XREF, tblDV_GroupXREF, tblDV_Group, tblDV";
                    bAssessed = true;
                    break;

                case "000030":
                    sTables = "tblScenario, tblProj, tblEvaluationGroup_DistribDetail_OrderingList, tblEvaluationGroup_DistribDetail, tblEvaluationGroup, tblScenarioAttributes, tblScenario_SpecialOps, tblScenario_DistribDetail";
                    bAssessed = true;
                    break;
                
                case "000035":
                    sTables = "tblSplint, tblNewFeature, tblEXCEL_Range, tblEnsembleXREF, tblEnsemble, tblDistrib_ElementList_XREF, tblDistrib_ElementLibStrategy_XREF, tblDistrib_ElementLibary_XREF, tblCategoryRecord_XREF, tblCategory";
                    bAssessed = true;
                    break;
                
                case "000040":
                    sTables = "tblResultVar, tblResultTS_EventSummary, tblResultTS, tblPerformance_ResultXREF, tblPerformance";
                    bAssessed = true;
                    break;
                
                case "000045":
                    sTables = "tblResultVar_Details, tblResultTS_EventSummary_Detail, tblResultTS_Detail, tblPerformance_Detail, tblModElementVals";
                    bAssessed = true;
                    break;
                
                case "000050":
                    sTables = "tblRT_RealTime";
                    bAssessed = true;
                    break;

                case "000111":
                    sTables = "tblExternalDataRequest";
                    bAssessed = true;
                    break;

                case "000113":
                    sTables = "tblSupportingFileSpec";
                    bAssessed = true;
                    break;

                case "000115": //SP 1-Dec-2016 this is a temporary table that was added - not a necessity and will be removed
                    sTables = "tblResultTS_Aux_Detail";
                    bAssessed = true;
                    break;

                case "000005a":
                    sTables = "tlkpEPANETFieldDictionary, tlkpEPANET_TableDictionary, tlkpEPANET_ResultsFieldDictionary";
                    bAssessed = true;
                    break;

                case "000005b":
                    sTables = "tblEPANET_Coordinates, tblEPANET_Curves, tblEPANET_Emitters, tblEPANET_Junctions, tblEPANET_Mixing, tblEPANET_Patterns, tblEPANET_Pipes, " + 
                        "tblEPANET_Pumps, tblEPANET_Quality, tblEPANET_Reactions, tblEPANET_Reservoirs, tblEPANET_RunSettings, tblEPANET_Sources, tblEPANET_Status, " +
                        "tblEPANET_Tanks, tblEPANET_Valves, tblEPANET_Vertices";
                    bAssessed = true;
                    break;

                case "000005c":
                    sTables = "tlkpSWMMFieldDictionary, tlkpSWMMFieldDictionary_Key, tlkpSWMMResults_FieldDictionary, tlkpSWMMResults_TableDictionary, tlkpSWMMTableDictionary";
                    break;

                case "000005d":
                    sTables = "tblSWMM_Aquifers, tblSWMM_BUILDUP, tblSWMM_Conduits, tblSWMM_Coordinates, tblSWMM_Coverages, tblSWMM_Curves, tblSWMM_DWF, tblSWMM_Evaporation, " +
                        "tblSWMM_Groundwater, tblSWMM_Hydrographs, tblSWMM_Infiltration, tblSWMM_INFLOWS, tblSWMM_Junctions, tblSWMM_LandUses, tblSWMM_LID_Bioretention, " +
                        "tblSWMM_LID_InfiltrationTrench, tblSWMM_LID_PorousPavement, tblSWMM_LID_RainBarrel, tblSWMM_LID_Usage, tblSWMM_LID_VegetativeSwale, tblSWMM_Loadings, " +
                        "tblSWMM_Losses, tblSWMM_ORIFICES, tblSWMM_Outfalls, tblSWMM_OUTLETS, tblSWMM_PATTERNS, tblSWMM_Pollutants, tblSWMM_Polygons, tblSWMM_PROFILES, " +
                        "tblSWMM_Pumps, tblSWMM_RainGages, tblSWMM_RDII, tblSWMM_RunSettings, tblSWMM_Snowpacks, tblSWMM_Storage, tblSWMM_Subareas, tblSWMM_Subcatchments, " + 
                        "tblSWMM_Symbols, tblSWMM_Timeseries, tblSWMM_TRANSECTS, tblSWMM_Vertices, tblSWMM_Washoff, tblSWMM_Weirs, tblSWMM_XSections";
                    bAssessed = true;
                    break;

                case "000005e":
                    sTables = "tlkpISIS_2D_FieldDictionary, tlkpISIS_2D_TableDictionary, tlkpISISDictionary, tlkpISISFieldDictionary, tlkpISISTableDictionary";
                    bAssessed = true;
                    break;

                case "000005f":
                    sTables = "tlkpSIMClimTableDictionary, tlkpSIMClimFieldDictionary, tlkpSimClimDictionary, tlkpSimClim_ResultsDictionary";
                    bAssessed = true;
                    break;

                case "000005g":
                    sTables = "tblSimClimScenario";
                    bAssessed = true;
                    break;

                case "000005h":
                    sTables = "tlkpModFlow_FieldDictionary";
                    bAssessed = true;
                    break;

                case "000005i":
                    sTables = "tlkpIW_Dictionary, tlkpIWFieldDictionary, tlkpIWResults_FieldDictionary, tlkpIWTableDictionary, tlkpIWTableDictionary_Key";
                    bAssessed = true;
                    break;

                case "000005j":
                    sTables = "tblComponent, hw_conduit, hw_node, hw_pump, hw_sluice, hw_subcatchment, hw_weir";
                    bAssessed = true;
                    break;

                case "000005k":
                    sTables = "tlkpExtend_StructureDictionary";
                    bAssessed = true;
                    break;

                case "000131":
                    sTables = "tblExternalGroup";
                    bAssessed = true;
                    break;

                case "000134":
                    sTables = "tlkpModelAttribute, tlkpModelAttributeSection, tlkpModelType";
                    bAssessed = true;
                    break;

                default:
                    sTables = "";
                    break;
            }

            //the scripts listed above can check for table existance
            if (sTables != "")    
                nreturn = IterateThroughTableExistance(sScriptNumber, sTables, ref sPotentialIssues);
            //--------------------------------END SECTION 1 -------------------------------------------------------------



            //----------------------------------------------------------------------------------------------------------
            //--------------------------------SECTION 2: Check for field existance in Tables ----------------------------
            //----------------------------------------------------------------------------------------------------------
            switch (sScriptNumber)
            {
                case "000051":
                    FieldExistsWithinTable("tblScenario", "ProjID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblDV", "Operation_DV", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "ApplyThreshold", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "SkipIfBelowThreshold");
                    bAssessed = true;
                    break;
                
                case "000055":
                    FieldExistsWithinTable("tblPerformance", "ComponentApplyThreshold", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "integer", "0");
                    FieldExistsWithinTable("tblPerformance", "ComponentThreshold", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "double", "0");
                    FieldExistsWithinTable("tblPerformance", "ComponentIsOver_Threshold", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "integer", "-1");

                    FieldExistsWithinTable("tblPerformance_ResultXREF", "ApplyThreshold", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "integer", "-2");
                    FieldExistsWithinTable("tblPerformance_ResultXREF", "Threshold", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "double", "-1.234");
                    FieldExistsWithinTable("tblPerformance_ResultXREF", "IsOver_Threshold", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "integer", "-2");
                    bAssessed = true;
                    break;
                  
                case "000060":
                    FieldExistsWithinTable("tblDV", "Element_Label", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblDV", "IncludeInScenarioLabel", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblDV", "IsDistrib", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblDV", "Distrib_VarType_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000061":
                    FieldExistsWithinTable("tblResultTS", "ResultID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    // met : want this field back....FieldExistsWithinTable("tblResultTS", "IsAux", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent); //SP 28-Sep-2016 moved to script 1.02
                    bAssessed = true;
                    break;

                case "000063":
                    FieldExistsWithinTable("tblResultTS_EventSummary", "EventLevelCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblResultTS_EventSummary", "CountMeetsThreshold", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblResultTS_EventSummary", "ThresholdCalcEQ", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000064":
                    FieldExistsWithinTable("tblResultTS_EventSummary_Detail", "ResultTS_ID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblResultTS_EventSummary_Detail", "Rank_TOTAL", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblResultTS_EventSummary_Detail", "Rank_Peak", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000065":
                    //TODO find a way to check for field datatype in both MSAccess and SQLServer
                    break;

                case "000067":
                    FieldExistsWithinTable("tblPerformance", "TS_Code", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "LKP_LookupID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "LKP_ScaleBy", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "LKP_ScaleFactor", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "ScaleFactorInput", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "UseDifferenceFromBaseline", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "LKP_Qual", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "IfErrorVal", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance", "IsDistrib", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000068":
                    FieldExistsWithinTable("tblPerformance_Detail", "DVID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance_Detail", "IsLinkToGroup", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance_Detail", "ScenarioElementVal_ID", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance_Detail", "PerformanceLKP_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance_Detail", "IsInvalid", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblPerformance_Detail", "Quantity", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000069":
                    FieldExistsWithinTable("tblPerformance_ResultXREF", "SSMA_TimeStamp", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000070":
                    FieldExistsWithinTable("tblScenario", "CreatedBy_User", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblScenario", "UserID_No_DELETE", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblScenario", "ParentScenario", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblScenario", "COST_Capital", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblScenario", "COST_OM", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblScenario", "COST_Total", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblScenario", "ScenDuration", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000071":
                    FieldExistsWithinTable("tblEvaluationGroup", "EvalPrefix", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblEvaluationGroup", "RESULT_ImportAll", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblEvaluationGroup", "IsSecondary", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblEvaluationGroup", "TS_Duration", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblEvaluationGroup", "TS_ValShift", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblEvaluationGroup", "ModelScenarioFilter_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblEvaluationGroup", "IsModFileUserDefined", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "bit", "0");
                    FieldExistsWithinTable("tblEvaluationGroup", "ModFileKey", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "nvarchar(255)", "'a[replace]b'");
                    bAssessed = true;
                    break;

                case "000072":
                    FieldExistsWithinTable("tblProj", "ModelTargetArea", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblProj", "UserID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblProj", "RESULT_ImportAll", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblProj", "UnitSettings_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblProj", "DB_Model", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000073":
                    FieldExistsWithinTable("tblOptionLists", "Qual2", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblOptionLists", "VarType_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblOptionLists", "IsScaleValue", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblOptionLists", "VarType_ScaleBy", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000074":
                    FieldExistsWithinTable("tblOptionDetails", "valLabelinSCEN", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblOptionDetails", "VarID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000075":
                    FieldExistsWithinTable("tblElementLists", "TableID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblElementLists", "ElementID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblElementLists", "CostID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000076":
                    FieldExistsWithinTable("tblElementListDetails", "val", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000080":
                    FieldExistsWithinTable("tblDV", "Operation", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present);
                    FieldExistsWithinTable("tblOptionLists", "Operation", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000081":
                    //nothing in this script
                    bAssessed = true;
                    break;

                case "000085":
                    //TODO find a way to check for field datatype in both MSAccess and SQLServer
                    break;

                case "000090":
                    //TODO find a way to check for field datatype in both MSAccess and SQLServer
                    break;

                case "000095":
                    FieldExistsWithinTable("tblEvaluationGroup", "CohortID", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present);
                    FieldExistsWithinTable("tblEvaluationGroup", "CohortSQN", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present);
                    FieldExistsWithinTable("tblEvaluationGroup", "SimIDBaseline", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present);
                    FieldExistsWithinTable("tblEvaluationGroup", "IsXModel", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present);
                    bAssessed = true;
                    break;

                case "000096":
                    FieldExistsWithinTable("tblScenario", "AltScenarioID", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present);
                    bAssessed = true;
                    break;

                case "000097":
                    //TODO find a way to check for field datatype in both MSAccess and SQLServer
                    break;

                case "000100":
                    //TODO find a way to check for field datatype in both MSAccess and SQLServer
                    break;

                case "000101":
                    FieldExistsWithinTable("tblScenario", "sqn", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present);
                    bAssessed = true;
                    break;
                
                case "000102":
                    //FieldExistsWithinTable("tblResultTS", "IsAux", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present); //SP 28-Feb-2017 removed in script 1.29
                    bAssessed = true;
                    break;

                case "000104":
                    //TODO find a way to check for field datatype in both MSAccess and SQLServer
                    break;

                case "000105":
                    //TODO find a way to check for field datatype in both MSAccess and SQLServer
                    break;

                case "000112":
                    //FieldExistsWithinTable("tblResultTS", "AuxRetrieveCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present); //SP 28-Feb-2017 renamed in 00.01.29
                    FieldExistsWithinTable("tblResultTS", "AuxID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present);
                    bAssessed = true;
                    break;

                case "000121":
                    FieldExistsWithinTable("tblResultTS_EventSummary", "IsSecondary", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "bit", "0");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "RefFromBeginning", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "bit", "-1");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "RefEventID", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "-1");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "PeriodsBefore", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent); //SP 1-Mar-2017 Removed after script was committed
                    FieldExistsWithinTable("tblResultTS_EventSummary", "PeriodsAfter", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent); //SP 1-Mar-2017 Removed after script was committed
                    FieldExistsWithinTable("tblResultTS_EventSummary", "sqn", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "0");
                    bAssessed = true;
                    break;

                case "000122":
                    //FieldExistsWithinTable("tblExternalDataRequest", "SQN", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "1"); //SP 14-Feb-2017 removed in 00.01.26
                    //FieldExistsWithinTable("tblExternalDataRequest", "ReturnColumn", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "1"); //SP 14-Apr-2017 renamed in 00.01.32
                    bAssessed = true;
                    break;

                case "000123":
                    FieldExistsWithinTable("tblResultTS_EventSummary", "IsHardOrigin", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "bit", "0");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "IsHardTerminus", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "bit", "0");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "IsPointVal", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "bit", "0");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "SearchOriginForward", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "bit", "-1");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "SearchTerminusForward", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "bit", "-1");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "OriginOffset", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "-1");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "TerminusOffset", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "-1");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "Label", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "nvarchar(50)", "Label");
                    bAssessed = true;
                    break;

                case "000124":
                    //TODO find a way to check for field default value in both MSAccess and SQLServer
                    break;

                case "000125":
                    //FieldExistsWithinTable("tblSupportingFileSpec", "Destination_Code", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "3"); //SP 14-Feb-2017 renamed in 00.01.30
                    FieldExistsWithinTable("tblSupportingFileSpec", "DV_ID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "-1");
                    //FieldExistsWithinTable("tblSupportingFileSpec", "CohortID", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "1"); //SP 14-Feb-2017 removed in 00.01.26
                    //FieldExistsWithinTable("tblSupportingFileSpec", "DestColumn", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "1"); //SP 14-Apr-2017 renamed in 00.01.32
                    bAssessed = true;
                    break;

                case "000126":
                    //FieldExistsWithinTable("tblSupportingFileSpec", "GroupID", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "int", "3", "CohortID"); //SP 14-Apr-2017 renamed in 00.01.32
                    //FieldExistsWithinTable("tblExternalDataRequest", "params", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "kwargs"); //SP 14-Apr-2017 moved to tblExternalGroup in 00.01.31
                    //FieldExistsWithinTable("tblExternalDataRequest", "GroupID", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "SQN"); //SP 14-Apr-2017 renamed in 00.01.32
                    bAssessed = true;
                    break;

                case "000127":
                    FieldExistsWithinTable("tblEvaluationGroup", "CohortType", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int", "-1");
                    bAssessed = true;
                    break;

                case "000129":
                    FieldExistsWithinTable("tblResultTS", "RetrieveCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "AuxRetrieveCode");
                    FieldExistsWithinTable("tblResultTS", "IsSecondary", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblResultTS", "IsAux", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblResultTS_EventSummary", "AssignEventNoCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "RefPrimaryEvent_StartOffset", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int");
                    FieldExistsWithinTable("tblResultTS_EventSummary", "RefPrimaryEvent_EndOffset", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int");
                    FieldExistsWithinTable("tblResultTS_EventSummary_Detail", "EventNo", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "int");
                    bAssessed = true;
                    break;

                case "000130":
                    //FieldExistsWithinTable("tblSupportingFileSpec", "source_SimlinkDataTypeCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "SimlinkData_Code"); //SP 15-Aug-2017 I think this field is redundant and is no longer used. Script 1.35 removes it
                    FieldExistsWithinTable("tblSupportingFileSpec", "source_DataFormat", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "DataType_Code");
                    //FieldExistsWithinTable("tblSupportingFileSpec", "conn_string", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "Filename"); //SP 14-Apr-2017 moved to tblExternalGroup in 00.01.31
                    //FieldExistsWithinTable("tblSupportingFileSpec", "destination_ExternalDataCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "Destination_Code"); //SP 14-Apr-2017 moved to tblExternalGroup in 00.01.31
                    //FieldExistsWithinTable("tblSupportingFileSpec", "params", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "Params"); //SP 14-Apr-2017 moved to tblExternalGroup in 00.01.31
                    FieldExistsWithinTable("tblExternalDataRequest", "destination_SimlinkDataTypeCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "return_format_code");
                    //FieldExistsWithinTable("tblExternalDataRequest", "source_ExternalDataCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "source_code"); //SP 14-Apr-2017 moved to tblExternalGroup in 00.01.31
                    FieldExistsWithinTable("tblExternalDataRequest", "db_type", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000131":
                    FieldExistsWithinTable("tblSupportingFileSpec", "DestColumnName", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "NVarChar(100)", "''");
                    FieldExistsWithinTable("tblExternalDataRequest", "ReturnColumnName", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Present, "NVarChar(100)", "''");
                    bAssessed = true;
                    break;

                case "000132":
                    FieldExistsWithinTable("tblSupportingFileSpec", "DestColumnNo", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "DestColumn");
                    FieldExistsWithinTable("tblSupportingFileSpec", "GroupID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "GroupID");
                    FieldExistsWithinTable("tblSupportingFileSpec", "EvalID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblSupportingFileSpec", "description", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblSupportingFileSpec", "destination_ExternalDataCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblSupportingFileSpec", "IsInput", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblSupportingFileSpec", "params", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblSupportingFileSpec", "conn_string", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);


                    FieldExistsWithinTable("tblExternalDataRequest", "ReturnColumnNo", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "ReturnColumn");
                    FieldExistsWithinTable("tblExternalDataRequest", "GroupID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Modified, "", "", "GroupID");
                    FieldExistsWithinTable("tblExternalDataRequest", "EvalID_FK", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblExternalDataRequest", "description", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblExternalDataRequest", "source_ExternalDataCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblExternalDataRequest", "params", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    FieldExistsWithinTable("tblExternalDataRequest", "conn_string", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

                case "000135":
                    FieldExistsWithinTable("tblSupportingFileSpec", "source_SimlinkDataTypeCode", sScriptNumber, ref sPotentialIssues, ref nreturn, FieldRequirement.Absent);
                    bAssessed = true;
                    break;

            }
            //some function to determine how to deal with individual scripts
            //return XXXXXX
            //--------------------------------END SECTION 2 -------------------------------------------------------------


            //-----------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------SECTION 3: Check for record existance in a specific field in a specific Table ----------------------------
            //-----------------------------------------------------------------------------------------------------------------------------------------
            switch (sScriptNumber)
            {
                case "000103a":
                    if (!RecordExistsWithinTable("tlkpEPANETFieldDictionary", "FieldName", "RuleIDAndDetail") || !RecordExistsWithinTable("tlkpEPANETFieldDictionary", "FieldName", "ControlIDAndDetail"))
                    {
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - tlkpEPANETFieldDictionary - 'RuleIDAndDetail' OR 'ControlIDAndDetail' does not exist in field 'FieldName' in table", sScriptNumber));
                        nreturn = false;
                    }
                    break;

                case "000110a":
                    if (!RecordExistsWithinTable("tlkpEPANETFieldDictionary", "FieldName", "ReservoirHead") || !RecordExistsWithinTable("tlkpEPANETFieldDictionary", "FieldName", "ReservoirPattern"))
                    {
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - tlkpEPANETFieldDictionary - 'ReservoirHead' OR 'ReservoirPattern' does not exist in field 'FieldName' in table", sScriptNumber));
                        nreturn = false;
                    }
                    break;

                case "000120a":
                    if (!RecordExistsWithinTable("tlkpEPANETFieldDictionary", "FieldName", "Period48Multiplier"))
                    {
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - tlkpEPANETFieldDictionary - 'Period48Multiplier' and other Period Multipliers up to 48 do not exist in field 'FieldName' in table", sScriptNumber));
                        nreturn = false;
                    }
                    break;

                case "000128a":
                    if (!RecordExistsWithinTable("tlkpEPANETFieldDictionary", "FieldName", "Duration"))
                    {
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - tlkpEPANETFieldDictionary - 'Duration / HydraulicTimeStep..' and other time parameters do not exist in field 'FieldName' in table", sScriptNumber));
                        nreturn = false;
                    }
                    break;

                case "000133a":
                    if (!RecordExistsWithinTable("tlkpEPANETFieldDictionary", "FieldName", "START_DATE"))
                    {
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - tlkpEPANETFieldDictionary - 'START_DATE / START_TIME..' and other time parameters do not exist in field 'FieldName' in table", sScriptNumber));
                        nreturn = false;
                    }
                    break;


                default: //if reached this point and script is not assessed, script does not have specific checks in place to deterimine if the contents of the script exist in database
                    if (!bAssessed)
                    {
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - Warning Only! Indeterminate whether executed on this database", sScriptNumber));
                        return false;
                    }
                    break;
            }
            //some function to determine how to deal with individual scripts
            //return XXXXXX
            //------------------------------------------------End SECTION 3--------------------------------------------------------------------------------

            return nreturn;
        }

        #region HelperFunctions

        //iterate through a list of tables and report for non-existance
        private bool IterateThroughTableExistance(string sScriptNumber, string sTables_String, ref string sPotentialIssues)
        {
            bool nreturn = true;
            string[] sTables_Array = sTables_String.Split(',').Select(x => x.Trim()).ToArray();

            foreach (string sTableName in sTables_Array)
            {
                bool bTableExists = TableExists(sTableName);
                if (!bTableExists)
                {
                    if (nreturn) //first time reporting an issue with this TestNumber
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues, "(Script " + sScriptNumber + ") -", false);  
                        //sPotentialIssues = sPotentialIssues + "(Script " + sScriptNumber + ") -";

                    sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues, " missing " + sTableName + ";", false);
                    //sPotentialIssues = sPotentialIssues + " missing " + sTableName + ";";
                    nreturn = false;
                }
            }
            
            if (!nreturn) //have written a potential issue, start a new line for next script
                sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues, "");
                //sPotentialIssues = sPotentialIssues + Environment.NewLine;

            return nreturn;
        }




        private bool PerformSpecificModelRequirementTests(SimulationType stSimType, ref string sPotentialIssues)
        {
            string sScripts_String = "";

            //Check that the lookup dictionary and data structure tables for the specified model exist
            switch(stSimType)
            {
                case SimulationType.EPANET:
                    sScripts_String = "00.00.05a, 00.00.05b, 00.01.03a, 00.01.01a, 00.01.10a, 00.01.20a, 00.01.28a, 00.01.33a";
                    break;

                case SimulationType.SWMM:
                    sScripts_String = "00.00.05c, 00.00.05d";
                    break;

                case SimulationType.SIMCLIM:
                    sScripts_String = "00.00.05f, 00.00.05g";
                    break;

                case SimulationType.MODFLOW:
                    sScripts_String = "00.00.05h";
                    break;

                case SimulationType.ICM:
                    sScripts_String = "00.00.05i, 00.00.05j";
                    break;

                case SimulationType.EXTENDSIM:
                    sScripts_String = "00.00.05k";
                    break;

                //TODO SP 11-Apr-2016 more model specific requirements to be added

                default:
                    return true; //no specific scripts for model type
            }

            return IterateThroughScripts(sScripts_String, ref sPotentialIssues);

        }

        //routine takes in a list of scripts and first checks if it exists in tblVersion, if not will check the contents of the scripts are implemented in the database
        private bool IterateThroughScripts(string sScripts_String, ref string sPotentialIssues)
        {
            bool nreturn = true;
            string sTblVersion = "tblVersion";
            string sField = "VersionNumber";
            string[] sScripts_Array = sScripts_String.Split(',').Select(x => x.Trim()).ToArray();

            //check that the contents of each script have been applied to the database - first check tblVersion, if no record of the script being run, confirm the change made by the script
            foreach (string sVersionNumber in sScripts_Array)
            {
                if (!RecordExistsWithinTable(sTblVersion, sField, sVersionNumber))
                {
                    if (!ContentsOfScriptExistsInDB(CleanVersion(sVersionNumber), ref sPotentialIssues))
                        nreturn = false;
                }
            }

            return nreturn;
        }


        #region SQLDatabaseChecks 
        //--------use SQL and _dbContext to determine if a table exists----------
        private bool TableExists(string sTableName)
        {
            string _sSQL = CreateSQLForTableExistance(sTableName);

            DataSet ds = _dbContextForValidation.getDataSetfromSQL(_sSQL);
            if (ds != null)
                return true;
            else
                return false;
        }

        private string CreateSQLForTableExistance(string sTableName)
        {
            return "Select TOP 1 * from " + sTableName;
        }


        //-----use SQL and _dbContext to determine if a field exists---------
        private void FieldExistsWithinTable(string sTableName, string sFieldName, string sScriptNumber, ref string sPotentialIssues,
            ref bool bCurrentStatusOfIssue, FieldRequirement frType = FieldRequirement.Present, string sDataType = "", string sDefaultValue = "", string sOldFieldName = "")
        {
            //TODO SP 11-Apr-2016 Tested
            string _sSQL = CreateSQLForFieldExistance(sTableName, sFieldName);

            DataSet ds = _dbContextForValidation.getDataSetfromSQL(_sSQL);
            if (ds != null)
            {
                //no change to bCurrentStatusOfIssue
                switch (frType)
                {
                    case FieldRequirement.Absent:
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - {1} - Field '{2}' still exists when recent changes have it removed", sScriptNumber, sTableName, sFieldName));
                        bCurrentStatusOfIssue = false;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (frType)
                {
                    case FieldRequirement.Present:
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - {1} - requires new field '{2}' type {3} with default value {4}", sScriptNumber, sTableName, sFieldName, sDataType, sDefaultValue));
                        bCurrentStatusOfIssue = false;
                        break;
                    case FieldRequirement.Modified:
                        sPotentialIssues = AppendLineForPotentialIssues(sPotentialIssues,
                            string.Format("(Script {0}) - {1} - Field '{2}' needs to be renamed to '{3}'", sScriptNumber, sTableName, sOldFieldName, sFieldName));
                        bCurrentStatusOfIssue = false;
                        break;
                    default:
                        break;
                }
                
            }
        }


        private string CreateSQLForFieldExistance(string sTableName, string sFieldName)
        {
            return "Select " + sFieldName + " from " + sTableName;
        }


        //-----use SQL and _dbContext to determine if a record exists within a table---------
        private bool RecordExistsWithinTable(string sTableName, string sFieldName, string sValue)
        {
            //TODO SP 18-Apr-2016 test when needed
            string _sSQL = CreateSQLForRecordExistance(sTableName, sFieldName, sValue);

            DataSet ds = _dbContextForValidation.getDataSetfromSQL(_sSQL);
            if (ds != null)
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
            
            //if reached this part of the code, then false
            return false;
        }


        private string CreateSQLForRecordExistance(string sTableName, string sFieldName, string sValue)
        {
            return string.Format("Select {0} from {1} where {0} = '{2}'", sFieldName, sTableName, sValue);
        }
        #endregion

        //version number in the database may contain "." after every two numbers, remove these when checking for script numbers
        private string CleanVersion(string sRawVersionNumber)
        {
            return sRawVersionNumber.Replace(".", string.Empty);
        }

        private string AppendLineForPotentialIssues(string sCurrentString, string sAdditionalLine, bool bEndOfLine = true)
        {
            if (bEndOfLine)
                return sCurrentString + sAdditionalLine + Environment.NewLine;
            else
                return sCurrentString + sAdditionalLine;
        }

        #endregion
    }
}