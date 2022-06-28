using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using SIM_API_LINKS.DAL;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections;
//using CH2M;

namespace SIM_API_LINKS
{
    partial class simlink
    {

        #region EG

        //potentially return code indicating success?
        public void CreateProject(int nModelTypeID, string sProjLabel, string sModelFileLocation, string sProjDescription ="")
        {
            int nProjID = InsertProject(nModelTypeID, sProjLabel, sProjDescription);
            string sEG_Label = "PROJECT " + nProjID.ToString() + " (Default)";                 //add a default label  
            int nEvalID = CreateEG(nModelTypeID, sModelFileLocation, sEG_Label, nProjID, -1, "", _nActiveBaselineScenarioID);

        }

        private int InsertProject(int nModelTypeID, string sProjLabel,  string sProjDescription)
        {
            try
            {
                string strSQL = "INSERT INTO tblProj(ProjLabel, ModelType_ID, DateCreated, LastModified, ModelDescription) VALUES ('{0}', {1}, '{2}', '{3}', '{4}')";
                string strNewSQL = string.Format(strSQL, sProjLabel, nModelTypeID, DateTime.Now.ToString(), DateTime.Now.ToString(), sProjDescription);
                _dbContext.ExecuteNonQuerySQL(strNewSQL);

                strSQL = "SELECT MAX(ProjID) FROM tblProj";
                int nReturnID = (int)_dbContext.ExecuteScalarSQL(strSQL);

                //string sPK_SQL = "SKIP";
                //if (true)             
                //{
                //    string sWhere = "(ProjLabel = '" + sProjLabel + "')";
                //    sPK_SQL = SIM_API_LINKS.DAL.DBContext.GetQuerySQL_NewPK_AferInsert("tblProj", "ProjID", sWhere);          //todo: figure out a better way to get the PK- Access, can't do any better. SQL Server- can!
                //}
                //int nReturnID = _dbContext.InsertOrUpdateDBByDataset(true, ds, sSQL, true, true, sPK_SQL);
                return Convert.ToInt32(nReturnID);

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error creating project");
                return -1;
            }
        }

        public int CreateEG(int nModelTypeID, string sModelFileLocation, string sEG_Label, int nProjID_FK, 
            int intReferenceEvalID_FK, string strEvaluationDescription, int intScenarioId)
        {
            DataSet dsEG = new DataSet();           // pass by reference, so we have available for an update of baseline scenario ID
            string sSQL = "SELECT tblEvaluationGroup.EvaluationID, tblEvaluationGroup.ProjID_FK, tblEvaluationGroup.EvaluationLabel, tblEvaluationGroup.DateCreated, "
                            + "tblEvaluationGroup.ModelFileLocation, tblEvaluationGroup.ModelType_ID,ScenarioID_Baseline_FK, tblEvaluationGroup.ReferenceEvalID_FK, "
                            + "tblEvaluationGroup.EvaluationDescription FROM tblEvaluationGroup"
                            + " WHERE (0>1)";                //get empty DS
            int nEvalID = InsertEG(sSQL, nModelTypeID, sModelFileLocation, sEG_Label, strEvaluationDescription, nProjID_FK, intReferenceEvalID_FK, ref dsEG, _nActiveBaselineScenarioID);
            string sScenarioLabel= "Baseline: " + nEvalID;
            int nScenarioID = InsertScenario(nEvalID, nProjID_FK, sScenarioLabel, sScenarioLabel);
            dsEG.Tables[0].Rows[0]["ScenarioID_Baseline_FK"] = nScenarioID;
            string strSQL = "UPDATE tblEvaluationGroup SET ScenarioID_Baseline_FK = " + nScenarioID.ToString() + " WHERE EvaluationID=" + nEvalID.ToString();
            _dbContext.ExecuteNonQuerySQL(strSQL);
            //_dbContext.InsertOrUpdateDBByDataset(false, dsEG, sSQL, true, false);               //update the base scenario
            return nEvalID;
        }
        private int InsertEG(string sSQL, int nModelTypeID, string sModelFileLocation, string sEG_Label, string strEvaluationDescription, int nProjID_FK, int intReferenceEvalID_FK, ref DataSet ds, int intBaseLineScenarioId)
        {
            try
            {
                ds = _dbContext.getDataSetfromSQL(sSQL);
                DataRow row = ds.Tables[0].NewRow();
                row["ProjID_FK"] = nProjID_FK;
                row["ModelFileLocation"] = sModelFileLocation;
                row["EvaluationLabel"] = sEG_Label;
                row["ModelType_ID"] = nModelTypeID;
                row["ReferenceEvalID_FK"] = intReferenceEvalID_FK;  //not used yet
                row["EvaluationDescription"] = strEvaluationDescription;
                row["ScenarioID_Baseline_FK"] = intBaseLineScenarioId;
                row["DateCreated"] = DateTime.Now;
                ds.Tables[0].Rows.Add(row);

                string sPK_SQL = "SKIP";
                if (true)
                {
                    string sWhere = "((EvaluationLabel = '" + sEG_Label + "') and  (ProjID_FK = " + nProjID_FK + ")) ";
                    sPK_SQL = SIM_API_LINKS.DAL.DBContext.GetQuerySQL_NewPK_AferInsert("tblEvaluationGroup", "EvaluationID", sWhere);          //todo: figure out a better way to get the PK- Access, can't do any better. SQL Server- can!
                }
                int nReturnID = _dbContext.InsertOrUpdateDBByDataset(true, ds, sSQL, true, true, sPK_SQL);
                return Convert.ToInt32(nReturnID);
            }
            catch
            {
                Console.WriteLine("Error in writting data...");
                return -1;
            }
        }

        #endregion



        #region Project Handling Routines
        /// <summary>
        /// Load project
        /// </summary>
        /// <returns></returns>
        public DataTable LoadProject()
        {
            IDbConnection cn = _dbContext.CurrentDBConnection;
            IDbDataAdapter adaptor = _dbContext.CurrentDataAdaptor;
            return LoadProject(cn, adaptor); // load the project
        }
        /// <summary>
        /// Load database project
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="adaptor"></param>
        /// <returns></returns>
        public DataTable LoadProject(IDbConnection cn, IDbDataAdapter adaptor)
        {
            string strSQL = "SELECT tblProj.ProjID, tblProj.ProjLabel, tblProj.ModelFile_Location, "
                    + "tblProj.ModelType_ID, tblProj.ModelDescription, "
                    + "tblProj.DateCreated, tblProj.LastModified, tlkpUI_Dictionary.val "
                    + "FROM tblProj INNER JOIN tlkpUI_Dictionary ON tblProj.ModelType_ID = tlkpUI_Dictionary.ValInt "
                    + "WHERE tlkpUI_Dictionary.Category = 'ModelType'";
            IDbCommand cmd = cn.CreateCommand();
            cmd.CommandText = strSQL;
            adaptor.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adaptor.Fill(ds);

            cmd.Dispose();
            // data table
            return ds.Tables[0];
        }
        /// <summary>
        /// Load event result
        /// </summary>
        /// <param name="intEvaluationGroup_FK"></param>
        /// <returns></returns>
        public DataSet LoadEventResult(int intEvaluationGroup_FK)
        {
            string strSQL = "SELECT EventSummaryID, ResultTS_or_Event_ID_FK, CategoryID_FK, EventFunctionID, "
                        + "Threshold_Inst, IsOver_Threshold_Inst, Threshold_Cumulative, "
                        + "IsOver_Threshold_Cumulative, InterEvent_Threshold, tblResultTS_EventSummary.CalcValueInExcessOfThreshold FROM tblResultTS INNER JOIN tblResultTS_EventSummary ON tblResultTS.ResultTS_ID = tblResultTS_EventSummary.ResultTS_or_Event_ID_FK "
                        + "WHERE tblResultTS_EventSummary.EvaluationGroupID_FK = " + intEvaluationGroup_FK.ToString();
            return _dbContext.getDataSetfromSQL(strSQL);
        }
        /// <summary>
        /// Load performance into the table
        /// </summary>
        /// <param name="intEvalID"></param>
        /// <returns></returns>
        public DataSet LoadPerformace(int intEvalID)
        {
            string strSQL = "SELECT PerformanceID, Performance_Label, PF_Type, CategoryID_FK, "
                + "LinkTableCode, PF_FunctionType, FunctionID_FK, IsObjective, SQN, "
                + "FunctionArgs, DV_ID_FK, OptionID_FK, ResultFunctionKey, Threshold, IsOver_Threshold, "
                + "ApplyThreshold, ComponentApplyThreshold, ComponentThreshold, ComponentIsOver_Threshold "
                + "FROM tblPerformance where (EvalID_FK = " + intEvalID.ToString() + ")";
            return _dbContext.getDataSetfromSQL(strSQL);
        }

        /// <summary>
        /// Load selected project
        /// </summary>
        /// <param name="intProjectID"></param>
        /// <returns></returns>
        public Project LoadSelectedProject(int intProjectID)
        {

            IDbConnection cn = _dbContext.CurrentDBConnection;
            IDbDataAdapter adaptor = _dbContext.CurrentDataAdaptor;
            return LoadSelectedProject(intProjectID, cn, adaptor);
        }
        /// <summary>
        /// Load selected project
        /// </summary>
        /// <param name="intProjectID"></param>
        /// <param name="cn"></param>
        /// <param name="adaptor"></param>
        /// <returns></returns>
        private Project LoadSelectedProject(int intProjectID, IDbConnection cn, IDbDataAdapter adaptor)
        {
            string strSQL = "SELECT tblProj.ProjID, tblProj.ProjLabel, "
                + "tblProj.ModelType_ID, tlkpUI_Dictionary.val, tblProj.ModelDescription, "
                + "tblProj.DateCreated, tblProj.LastModified "
                + "FROM tblProj INNER JOIN tlkpUI_Dictionary ON tblProj.ModelType_ID = tlkpUI_Dictionary.ValInt "
                + "WHERE tlkpUI_Dictionary.Category = 'ModelType' and tblProj.ProjID=" + intProjectID.ToString();
            IDbCommand cmd = cn.CreateCommand();
            cmd.CommandText = strSQL;
            IDataReader myReader = cmd.ExecuteReader();
            Project pro = new Project();
            while (myReader.Read())
            {
                pro.ProjectID = myReader.GetInt32(0);
                pro.ProjectName = myReader.GetString(1);
                pro.ModelTypeID = myReader.GetInt32(2);
                pro.ModelType = myReader.GetString(3);
                pro.ProjectNote = (myReader.GetValue(4) == DBNull.Value ? "" : myReader.GetString(4));
                pro.DateCreated = (myReader.GetValue(5) == DBNull.Value ? "" : myReader.GetDateTime(5).ToString("dd/MMM/yyyy HH:mm:ss"));
                pro.LastModifiedDate = (myReader.GetValue(6) == DBNull.Value ? "" : myReader.GetDateTime(6).ToString("dd/MMM/yyyy HH:mm:ss"));

            }
            myReader.Close();

            // update active local variable
            UpdateActiveProject(pro); 
            return pro;
        }
        /// <summary>
        /// Update to local variable
        /// </summary>
        /// <param name="pro"></param>
        private void UpdateActiveProject(Project pro)
        {
            // update the project id
            _nActiveProjID = pro.ProjectID;
            _nActiveModelTypeID = pro.ModelTypeID;

        }
        /// <summary>
        /// Update Active EG
        /// </summary>
        /// <param name="row"></param>
        public void UpdateActiveEG(DataRow row)
        {
            _nActiveEvalID = int.Parse(row["EvaluationID"].ToString());
            _nActiveReferenceEG_BaseScenarioID = int.Parse(row["ScenarioID_Baseline_FK"].ToString());
            _strCurrentEGLabel = row["EvaluationLabel"].ToString();
            _nActiveModelTypeID = int.Parse(row["ModelType_Id"].ToString());

            //intialise EG link
            InitializeEG(_nActiveEvalID);

        }
        /// <summary>
        /// Get all Evalulation group to the interface
        /// </summary>
        /// <param name="intProjectID"></param>
        /// <returns></returns>
        public DataTable GetEGGroup(int intProjectID)
        {
            string strSQL = "SELECT tblEvaluationGroup.* "
                            + "FROM tblEvaluationGroup WHERE (((tblEvaluationGroup.ProjID_FK)=" + intProjectID.ToString() + "));";
            IDbDataAdapter adaptor = _dbContext.CurrentDataAdaptor;
            IDbConnection cn = _dbContext.CurrentDBConnection;
            IDbCommand cmd = cn.CreateCommand();
            cmd.CommandText = strSQL;
            adaptor.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adaptor.Fill(ds);

            // get the first one as active
            if (ds.Tables[0].Rows.Count>0)
            {
                UpdateActiveEG(ds.Tables[0].Rows[0]); //use the first row as active data
            }
            return ds.Tables[0];
        }
        /// <summary>
        /// Load UI dictrionary dataset
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="adaptor"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private DataTable LoadUIDictionary(IDbConnection cn, IDbDataAdapter adaptor, UIDictionary dic)
        {
            string strDictName = dic.ToString();
            string strSQL = "SELECT * FROM tlkpUI_Dictionary WHERE Category = '" + strDictName + "'";

            IDbCommand cmd = cn.CreateCommand();
            cmd.CommandText = strSQL;
            adaptor.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adaptor.Fill(ds);

            // data table
            return ds.Tables[0];
        }
        /// <summary>
        /// Load UI dictionary
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public DataTable LoadUIDictionary(UIDictionary dic)
        {
            IDbConnection cn = _dbContext.CurrentDBConnection;
            IDbDataAdapter adaptor = _dbContext.CurrentDataAdaptor;
            return LoadUIDictionary(cn, adaptor, dic); // load the project
        }
        #endregion Opening Project

        /// <summary>
        /// Insert or update DV table
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="blnInsertNew"></param>
        public void InsertOrUpdateDVTable(DataSet ds, bool blnInsertNew, int intEvalationGroupID)
        {
            try
            {
                if (ds == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {
                    // EvaluationGroup_FK

                    strSQL = "INSERT INTO tblDV(" +
                        "EvaluationGroup_FK, " +
                        "DV_Label," +
                        "VarType_FK," +
                        "Option_FK," +
                        "Option_MIN," +
                        "Option_MAX," +
                        "GetNewValMethod," +
                        "FunctionID_FK," +
                        "FunctionArgs," +
                        "IsListVar," +
                        "SkipMinVal," +
                        "ElementID_FK," +
                        "PrimaryDV_ID_FK," +
                        "SecondaryDV_Key," +
                        "Operation," +
                        "IsSpecialCase," +
                        "IsTS," +
                        "XModelID_FK," + 
                        "SQN) VALUES ({0}," +
                        "'{1}'," +
                        "{2}," +
                        "{3}," +
                        "{4}," +
                        "{5}," +
                        "{6}," +
                        "{7}," +
                        "'{8}'," +
                        IsBool4SQL(9) + "," +
                        IsBool4SQL(10) +  "," +
                        "{11}," +
                        "{12}," +
                        "{13}," +
                        "'{14}'," +
                        IsBool4SQL(15) + "," +
                        IsBool4SQL(16) + "," +
                        "{17}," +
                        "{18})";
                }
                else
                {
                    strSQL = "UPDATE tblDV SET " +
                        "EvaluationGroup_FK = {0}, " +
                        "DV_Label = '{1}'," +
                        "VarType_FK = {2}," +
                        "Option_FK = {3}," +
                        "Option_MIN = {4}, " +
                        "Option_MAX = {5}," +
                        "GetNewValMethod = {6}," +
                        "FunctionID_FK = {7}," +
                        "FunctionArgs= '{8}'," +
                        "IsListVar = " + IsBool4SQL(9) + "," +
                        "SkipMinVal = " + IsBool4SQL(10) + "," +
                        "ElementID_FK = {11}," +
                        "PrimaryDV_ID_FK ={12}," +
                        "SecondaryDV_Key = {13}," +
                        "Operation = '{14}'," +
                        "IsSpecialCase = " + IsBool4SQL(15) + "," +
                        "IsTS = " + IsBool4SQL(16) + "," +
                        "XModelID_FK = {17}, " +
                        "SQN = {18} " + 
                        "WHERE DVD_ID = {19}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string EvaluationGroup_FK = intEvalationGroupID.ToString(); // ParseDataRow(row, "EvaluationGroupID");
                    string DV_Label = ParseDataRow(row, "DV_Label");
                    string VarType_FK = ParseDataRow(row, "VarType_FK");
                    string Option_FK = ParseDataRow(row, "OptionID");
                    string Option_MIN = ParseDataRow(row, "Option_MIN");
                    string Option_MAX = ParseDataRow(row, "Option_MAX");
                    string GetNewValMethod = ParseDataRow(row, "GetNewValMethod");
                    string FunctionID_FK = ParseDataRow(row, "FunctionID_FK");
                    string FunctionArgs = ParseDataRow(row, "FunctionArgs");
                    string IsListVar = ParseDataRow(row, "IsListVar");
                    string SkipMinVal = ParseDataRow(row, "SkipMinVal");
                    string ElementID_FK = ParseDataRow(row, "ElementID");
                    string PrimaryDV_ID_FK = ParseDataRow(row, "PrimaryDV_ID_FK"); //-1 always
                    string SecondaryDV_Key = ParseDataRow(row, "SecondaryDV_Key");
                    string Operation = ParseDataRow(row, "Operation");
                    string IsSpecialCase = ParseDataRow(row, "IsSpecialCase");
                    string IsTS = ParseDataRow(row, "IsTS");
                    string XModelID_FK = ParseDataRow(row, "XModelID_FK");
                    if (row["SQN"] == DBNull.Value) row["SQN"] = 100;
                    string sqn = ParseDataRow(row, "SQN");
                    string DV_ID = ParseDataRow(row, "DVD_ID");


                    string strNewSQL = string.Format(strSQL, EvaluationGroup_FK, DV_Label, VarType_FK, Option_FK,
                            Option_MIN, Option_MAX, GetNewValMethod, FunctionID_FK, FunctionArgs, IsListVar,
                            SkipMinVal, ElementID_FK, PrimaryDV_ID_FK, SecondaryDV_Key, Operation,
                            IsSpecialCase, IsTS, XModelID_FK, sqn, DV_ID);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch(Exception ex)
            {
                //Commons.ShowMessage("Error in adding/updating DV table '" + ex.Source + ": " + ex.Message +"'");
            }
        }
        private string GetElementLabelName(string strLabelID)
        {
            string strSQL = "SELECT ElementListLabel FROM tblElementLists WHERE ElementListID= " + strLabelID;
            return _dbContext.ExecuteScalarSQL(strSQL).ToString();
        }
        /// <summary>
        /// Insert or update timeseries results
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="blnInsertNew"></param>
        public void InsertOrUpdateSummaryTimeseriesTable(DataSet ds, bool blnInsertNew, int intEvalationGroupID)
        {
            try
            {
                if (ds == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {

                    strSQL = "INSERT INTO tblResultTS(EvaluationGroup_FK, Result_Label, FunctionID_FK, SQN, VarResultType_FK, Result_Description, "
                            + " ElementID_FK, Element_Label, "
                            + " BeginPeriodNo, FunctionArgs, IsSecondary, RefTS_ID_FK) VALUES ({0}, '{1}', {2}, {3}, " //TODO SP 28-Feb-2017 no longer a field called IsSecondary - needs to be calculated from RetrieveCode
                            + "{4}, '{5}', {6}, '{7}', {8}, '{9}', " + IsBool4SQL(10) + ", {11})";
                }
                else
                {
                    strSQL = "UPDATE tblResultTS SET EvaluationGroup_FK = {0}, " +
							"Result_Label = '{1}', " + 
							"FunctionID_FK = {2}," + 
							"SQN = {3}," + 
							"VarResultType_FK = {4}," + 
							"Result_Description = '{5}'," + 
							"ElementID_FK = {6}," + 
							"Element_Label = '{7}'," + 
							"BeginPeriodNo = {8}," + 
							"FunctionArgs = '{9}'," + 
							"IsSecondary = " + IsBool4SQL(10) + "," +  //TODO SP 28-Feb-2017 no longer a field called IsSecondary - needs to be calculated from RetrieveCode
                            "RefTS_ID_FK= {11} " + 
                            "WHERE ResultTS_ID = {12}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string EvaluationGroup_FK = intEvalationGroupID.ToString(); // ParseDataRow(row, "EvaluationGroupID");
                    string Result_ID = ParseDataRow(row, "ResultTS_ID");
                    string Result_Label = ParseDataRow(row, "Result_Label");
                    string VarResultType_FK = ParseDataRow(row, "VarResultType_FK");
                    //string DV_Type = ParseDataRow(row, "DV_Type");
                    string Result_Description = ParseDataRow(row, "Result_Description");
                    string ElementID_FK = ParseDataRow(row, "ElementID_FK");
                    string Element_Label = ParseDataRow(row, "Element_Label");
                    //string TS_StartDate = ParseDataRow(row, "TS_StartDate");
                    //string TS_StartHour = ParseDataRow(row, "TS_StartHour");
                    //string TS_StartMin = ParseDataRow(row, "TS_StartMin");
                    //string TS_Interval = ParseDataRow(row, "TS_Interval");
                    //string TS_Interval_Unit = ParseDataRow(row, "TS_Interval_Unit");
                    string BeginPeriodNo = ParseDataRow(row, "BeginPeriodNo");
                    string FunctionID_FK = ParseDataRow(row, "FunctionID_FK");
                    string FunctionArgs = ParseDataRow(row, "FunctionArgs");
                    //string IsAux = ParseDataRow(row, "IsAux");
                    string IsSecondary = ParseDataRow(row, "IsSecondary"); //TODO SP 28-Feb-2017 no longer a field called IsSecondary - needs to be calculated from RetrieveCode
                    string RefTS_ID_FK = ParseDataRow(row, "RefTS_ID_FK");
                    string strSQN = "-999";
                    ElementID_FK = "-1"; // set -1 when the ELEMENTID_FK is invisible as it's now
                    //if (ElementID_FK == "") ElementID_FK = "-1";
                    //if (ElementID_FK != "-1")
                    //{
                    //    Element_Label= GetElementLabelName(ElementID_FK); // element label name
                    //}
                    //if (TS_Interval == "") TS_Interval = "-1";
                    //if (TS_Interval_Unit == "") TS_Interval_Unit = "-1";
                    if (FunctionID_FK == "") FunctionID_FK = "-1";
                    if (RefTS_ID_FK == "") RefTS_ID_FK = "-1";
                    if (BeginPeriodNo == "") BeginPeriodNo = "1";
                    string strNewSQL = string.Format(strSQL, EvaluationGroup_FK, Result_Label, FunctionID_FK, strSQN,
                        VarResultType_FK, Result_Description, ElementID_FK, Element_Label
                        , BeginPeriodNo, FunctionArgs, IsSecondary, RefTS_ID_FK, Result_ID); //TODO SP 28-Feb-2017 no longer a field called IsSecondary - needs to be calculated from RetrieveCode
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in adding/updating timeseries table '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        /// <summary>
        /// Insert the performance table
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="blnInsertNew"></param>
        public void InsertOrUpdatePerformanceTable(DataSet ds, int intEvalID, bool blnInsertNew)
        {
            try
            {
                if (ds == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {
                    strSQL = "INSERT INTO tblPerformance(Performance_Label, PF_Type, "
                            + "CategoryID_FK, LinkTableCode, PF_FunctionType, "
                            + "FunctionID_FK, IsObjective, SQN, FunctionArgs, DV_ID_FK, "
                            + "OptionID_FK, EvalID_FK, ResultFunctionKey,ApplyThreshold, Threshold, IsOver_Threshold) "
                            + "VALUES ('{0}', {1}, {2}, {3}, {4}, {5}, " + IsBool4SQL(6) + ", {7}, '{8}', {9}, {10}, {11}, "
                            + "{12}, " + IsBool4SQL(13) + ", {14}, " + IsBool4SQL(15) + ")";
                }
                else
                {
                    strSQL = "UPDATE tblPerformance SET " +
                            "Performance_Label='{0}', " +
                            "PF_Type={1}, " +
                            "CategoryID_FK={2}, " +
                            "LinkTableCode={3}, " +
                            "PF_FunctionType={4}, " +
                            "FunctionID_FK={5}, " +
                            "IsObjective=" + IsBool4SQL(4) + ", " +
                            "SQN={7}, " +
                            "FunctionArgs='{8}', " +
                            "DV_ID_FK={9}, " +
                            "OptionID_FK={10}, " +
                            "EvalID_FK={11}, " +
                            "ResultFunctionKey={12}, "+
                            "ApplyThreshold = " + IsBool4SQL(13) + ", " + 
                            "Threshold ={14}, " +
                            "IsOver_Threshold =" + IsBool4SQL(15) + " " +
                            "WHERE PerformanceID = {16}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string Performance_Label = row["Performance_Label"].ToString();
                    string PF_Type = row["PF_Type"].ToString();
                    string CategoryID_FK = row["CategoryID_FK"].ToString();
                    string LinkTableCode = row["LinkTableCode"].ToString();
                    string PF_FunctionType = row["PF_FunctionType"].ToString();
                    string FunctionID_FK = (row["FunctionID_FK"] == DBNull.Value ? "-1" : row["FunctionID_FK"].ToString());
                    string IsObjective = row["IsObjective"].ToString();
                    string SQN = row["SQN"].ToString();
                    string FunctionArgs = row["FunctionArgs"].ToString();
                    string DV_ID_FK = (row["DV_ID_FK"] == DBNull.Value ? "-1" : row["DV_ID_FK"].ToString());
                    string OptionID_FK = (row["OptionID_FK"] == DBNull.Value ? "-1" : row["OptionID_FK"].ToString());
                    string EvalID_FK = intEvalID.ToString();
                    string ResultFunctionKey = row["ResultFunctionKey"].ToString();
                    string ApplyThreshold = row["ApplyThreshold"].ToString();
                    string Threshold = row["Threshold"].ToString();
                    string IsOver_Threshold = row["IsOver_Threshold"].ToString();


                    // performance
                    int intPerformanceID = (row["PerformanceID"] == DBNull.Value ? 0 : int.Parse(row["PerformanceID"].ToString()));

                    if (Threshold == "") Threshold = "0";
                    if (ResultFunctionKey == "") ResultFunctionKey = "-1";
                    // run the query
                    string strNewSQL = string.Format(strSQL, Performance_Label, PF_Type, CategoryID_FK, LinkTableCode,
                        PF_FunctionType, FunctionID_FK, IsObjective, SQN, FunctionArgs, DV_ID_FK, OptionID_FK, EvalID_FK,
                        ResultFunctionKey, ApplyThreshold, Threshold, IsOver_Threshold, intPerformanceID);
                    _dbContext.ExecuteNonQuerySQL(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in adding/updating performance table '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        /// <summary>
        /// Is Bool for SQL
        /// </summary>
        /// <param name="intStringIndex"></param>
        /// <returns></returns>
        private string IsBool4SQL(int intStringIndex)
        {
            if (_dbContext._DBContext_DBTYPE == DB_Type.SQL_SERVER)
            {
                return "'{" + intStringIndex.ToString() + "}'";
            }
            else
            {
                return "{" + intStringIndex.ToString() + "}";
            }
        }
        /// <summary>
        /// Return True/false string to match db
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        private string IsBool4SQL(string strVal)
        {
            if (_dbContext._DBContext_DBTYPE == DB_Type.SQL_SERVER)
            {
                return "'" + strVal + "'";
            }
            else
            {
                return strVal;
            }
        }
        /// <summary>
        /// Insert the performance table
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="blnInsertNew"></param>
        public void InsertOrUpdateEventTable(DataSet ds, int intEvaluationGroupID_FK, bool blnInsertNew)
        {
            try
            {
                if (ds == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {
                    strSQL = "INSERT INTO tblResultTS_EventSummary(ResultTS_or_Event_ID_FK, "
                            + "EventFunctionID, IsOver_Threshold_Inst, Threshold_Cumulative, "
                            + "IsOver_Threshold_Cumulative, InterEvent_Threshold, Threshold_Inst, "
                            + "CalcValueInExcessOfThreshold, EvaluationGroupID_FK, CategoryID_FK) "
                            + "VALUES ({0}, {1}," + IsBool4SQL(2) + ", {3}," + IsBool4SQL(4) + ", {5}, {6}," + IsBool4SQL(7) + ", {8}, {9})";
                }
                else
                {
                    strSQL = "UPDATE tblResultTS_EventSummary SET " +
                            "ResultTS_or_Event_ID_FK={0}, " +
                            "EventFunctionID={1}, " +
                            "IsOver_Threshold_Inst=" + IsBool4SQL(2) + ", " +
                            "Threshold_Cumulative={3}, " +
                            "IsOver_Threshold_Cumulative=" + IsBool4SQL(4) + ", " +
                            "InterEvent_Threshold={5}, " +
                            "Threshold_Inst={6}, " +
                            "CalcValueInExcessOfThreshold = " + IsBool4SQL(7) + "," +
                            "EvaluationGroupID_FK = {8}, " +
                            "CategoryID_FK = {9} " + 
                            "WHERE EventSummaryID = {10}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string ResultTS_or_Event_ID_FK = row["ResultTS_or_Event_ID_FK"].ToString();
                    string EventFunctionID = row["EventFunctionID"].ToString();
                    string IsOver_Threshold_Inst = row["IsOver_Threshold_Inst"].ToString();
                    string Threshold_Cumulative = row["Threshold_Cumulative"].ToString();
                    string IsOver_Threshold_Cumulative = row["IsOver_Threshold_Cumulative"].ToString();
                    string InterEvent_Threshold = row["InterEvent_Threshold"].ToString();
                    string Threshold_Inst = row["Threshold_Inst"].ToString();
                    string CategoryID_FK = row["CategoryID_FK"].ToString(); ; // set the default
                    Threshold_Inst = (Threshold_Inst == "" ? "NULL" : Threshold_Inst);
                    Threshold_Cumulative = (Threshold_Cumulative == "" ? "NULL" : Threshold_Cumulative);
                    // performance
                    int EventSummaryID = (row["EventSummaryID"] == DBNull.Value ? 0 : int.Parse(row["EventSummaryID"].ToString()));
                    string CalcValueInExcessOfThreshold = row["CalcValueInExcessOfThreshold"].ToString();
                    // run the query
                    string strNewSQL = string.Format(strSQL, ResultTS_or_Event_ID_FK, EventFunctionID, IsOver_Threshold_Inst,
                        Threshold_Cumulative, IsOver_Threshold_Cumulative, InterEvent_Threshold, Threshold_Inst, 
                        CalcValueInExcessOfThreshold, intEvaluationGroupID_FK, CategoryID_FK, EventSummaryID);
                    _dbContext.ExecuteNonQuerySQL(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in adding/updating performance table '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        /// <summary>
        /// Get option list
        /// </summary>
        /// <param name="strProjectID"></param>
        /// <returns></returns>
        public DataSet GetOptionListDetails(string strProjectID)
        {
            string strSQL = "SELECT m.OptionID, d.OptionID as OptionDetailKeyId, ProjID_FK, OptionLabel, d.OptionID_FK, d.OptionNo, d.val " +
                "FROM tblOptionDetails d INNER JOIN tblOptionLists m ON d.OptionID_FK = m.OptionID " +
                "WHERE (((ProjID_FK)=" + strProjectID + ")) ORDER BY m.OptionID, d.OptionNo";
            DataSet dsDetail = _dbContext.getDataSetfromSQL(strSQL);
            dsDetail.Tables[0].TableName = "tblOptionDetails";

            return dsDetail;
        }
        /// <summary>
        /// Getelement list detail
        /// </summary>
        /// <param name="strProjectID"></param>
        /// <returns></returns>
        public DataSet GetElementListDetails()
        {
            string strSQL = "SELECT ElementListDetailID, ElementListID_FK, ElementID_FK, VarLabel FROM tblElementListDetails";
            DataSet dsDetail = _dbContext.getDataSetfromSQL(strSQL);
            dsDetail.Tables[0].TableName = "tblElementListDetails";

            return dsDetail;
        }
        /// <summary>
        /// Get option list detail
        /// </summary>
        /// <returns></returns>
        public DataSet GetOptionListDetail()
        {
            string strSQL = "SELECT OptionID, OptionID_FK, OptionNo, val FROM tblOptionDetails";
            return _dbContext.getDataSetfromSQL(strSQL);
        }
        /// <summary>
        /// Delete option list
        /// </summary>
        /// <param name="row"></param>
        public void DeleteElementList(DataRow row)
        {
            string strElementID = row["ElementListDetailID"].ToString();
            string strSQL = "DELETE FROM tblElementListDetails WHERE ElementListDetailID = " + strElementID;
            _dbContext.RunDeleteSQL(strSQL);
        }
        /// <summary>
        /// Delete option list
        /// </summary>
        /// <param name="row"></param>
        public void DeleteOptionList(DataRow row)
        {
            string strOptionID = row["OptionDetailKeyId"].ToString();
            string strSQL = "DELETE FROM tblOptionDetails WHERE OptionID = " + strOptionID;
            _dbContext.RunDeleteSQL(strSQL);
        }
        /// <summary>
        /// Delete option list
        /// </summary>
        /// <param name="row"></param>
        public void DeleteRecordLink(string strRecordId)
        {
            string strSQL = "DELETE FROM tblPerformance_ResultXREF WHERE ID = " + strRecordId;
            _dbContext.RunDeleteSQL(strSQL);
        }
        /// <summary>
        /// Insert/update option list
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="blnInsertNew"></param>
        public void InsertOrUpdateOptionListTable(DataTable dt, bool blnInsertNew)
        {
            try
            {
                if (dt == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {
                    strSQL = "INSERT INTO tblOptionDetails(OptionNo, Val, OptionID_FK) VALUES ({0}, '{1}', {2})";
                }
                else
                {
                    strSQL = "UPDATE tblOptionDetails SET OptionNo={0}, " +
                            "Val='{1}' " +
                            "WHERE OptionID={2}";
                }
                foreach (DataRow row in dt.Rows)
                {
                    int intOptionDetailKeyId = (row["OptionDetailKeyId"] == DBNull.Value ? 0 : int.Parse(row["OptionDetailKeyId"].ToString()));
                    int intOptionId = int.Parse(row["OptionID"].ToString());
                    int intOptionNo = int.Parse(row["OptionNo"].ToString());
                    string strVAL = row["val"].ToString();
                    int intOptionKey = (blnInsertNew ? intOptionId : intOptionDetailKeyId);
                    string strNewSQL = string.Format(strSQL, intOptionNo, strVAL, intOptionKey);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in adding/updating option list '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        /// <summary>
        /// insert or update element list
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="blnInsertNew"></param>
        public void InsertOrUpdateElementListTable(DataTable dt, bool blnInsertNew)
        {
            try
            {
                if (dt == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {
                    strSQL = "INSERT INTO tblElementListDetails(ElementListID_FK, ElementID_FK, VarLabel) VALUES ({0}, {1}, '{2}')";
                }
                else
                {
                    strSQL = "UPDATE tblElementListDetails SET ElementListID_FK={0}, " +
                            "ElementID_FK={1}, " +
                            "VarLabel='{2}' " +
                            "WHERE ElementListDetailID={3}";
                }
                foreach (DataRow row in dt.Rows)
                {
                    int intElementListDetailID = (row["ElementListDetailID"] != DBNull.Value ? int.Parse(row["ElementListDetailID"].ToString()) : -1);
                    int intElementListID_FK = int.Parse(row["ElementListID_FK"].ToString());
                    int intElementID_FK = (row["ElementID_FK"] != DBNull.Value ? int.Parse(row["ElementID_FK"].ToString()) : -1);
                    string strVarLabel = row["VarLabel"].ToString();
                    string strNewSQL = string.Format(strSQL, intElementListID_FK, intElementID_FK, strVarLabel, intElementListDetailID);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in adding/updating element list '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        /// <summary>
        /// Insert or update DV table
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="blnInsertNew"></param>
        public void InsertOrUpdateSummaryResultTable(DataSet ds, bool blnInsertNew, int intEvalationGroupID)
        {
            try
            {
                if (ds == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {

                    strSQL = "INSERT INTO tblResultVar(Result_Label, EvaluationGroup_FK, VarResultType_FK, "
                            + "Result_Description, ElementID_FK, Element_Label, IsListVar, ImportResultDetail) "
                            + "VALUES ('{0}', {1}, {2}, '{3}', "
                            + "{4}, '{5}', "+  IsBool4SQL(6) + ", " + IsBool4SQL(7) + ")";
                }
                else
                {
                    strSQL = "UPDATE tblResultVar SET Result_Label='{0}', " +
                             "EvaluationGroup_FK = {1}, " +
                             "VarResultType_FK = {2}, " +
                             "Result_Description = '{3}', " +
                             "ElementID_FK = {4}, " +
                             "Element_Label = '{5}', " +
                             "IsListVar = "+  IsBool4SQL(6) + ", " + 
                             "ImportResultDetail = "+ IsBool4SQL(6) + " "+
                             "WHERE Result_ID = {8}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string Result_ID = ParseDataRow(row, "Result_ID");
                    string Result_Label = ParseDataRow(row, "Result_Label");
                    string EvaluationGroup_FK = intEvalationGroupID.ToString(); // ParseDataRow(row, "EvaluationGroupID");
                    string VarResultType_FK = ParseDataRow(row, "VarResultType_FK");
                    //string DV_Type = ParseDataRow(row, "DV_Type");
                    string Result_Description = ParseDataRow(row, "Result_Description");
                    string ElementID_FK = ""; //ParseDataRow(row, "ElementID");
                    string Element_Label =  ParseDataRow(row, "Element_Label");
                    if (ElementID_FK == "") ElementID_FK = "-1";
                    string IsListVar = ParseDataRow(row, "IsListVar");
                    string ImportResultDetail = ParseDataRow(row, "ImportResultDetail");
                    string strNewSQL = string.Format(strSQL, Result_Label, EvaluationGroup_FK, VarResultType_FK,
                        Result_Description, ElementID_FK, Element_Label, IsListVar, ImportResultDetail, Result_ID);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in adding/updating summary result table '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        /// <summary>
        /// Insert/update the scenario
        /// </summary>
        /// <param name="dsUpdate"></param>
        /// <param name="blnAddNew"></param>
        public void InsertOrUpdateScenarioTable(DataSet ds, bool blnInsertNew, string strEVGroupId)
        {
            try
            {
                if (ds == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {

                    strSQL = "INSERT INTO tblScenario(EvalGroupID_FK, ScenarioLabel, DateEvaluated, HasBeenRun, "
                            + "DNA, ScenStart, ScenEnd) VALUES ({0}, '{1}', '{2}', {3}, "
                            + "'{4}', {5}, {6})";
                }
                else
                {
                    strSQL = "UPDATE tblScenario SET  " +
                             "EvalGroupID_FK = {0}, " +
                             "ScenarioLabel = '{1}', " +
                             "DateEvaluated = '{2}', " +
                             "HasBeenRun = {3}, " +
                             "DNA = '{4}', " +
                             "ScenStart = {5}, " +
                             "ScenEnd = {6} " +
                             "WHERE ScenarioID = {7}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string ScenarioLabel = ParseDataRow(row, "ScenarioLabel");
                    string DateEvaluated = ParseDataRow(row, "DateEvaluated");
                    if (DateEvaluated == "") DateEvaluated = DateTime.Today.ToString();
                    string HasBeenRun = ParseDataRow(row, "HasBeenRun");
                    string DNA = ParseDataRow(row, "DNA");
                    string ScenStart = ParseDataRow(row, "ScenStart");
                    string ScenEnd = ParseDataRow(row, "ScenEnd");
                    string ScenarioID = ParseDataRow(row, "ScenarioID");

                    ScenStart = (ScenStart == "" ? "-1" : ScenStart);
                    ScenEnd = (ScenEnd == "" ? "-1" : ScenEnd);

                    string strNewSQL = string.Format(strSQL, strEVGroupId, ScenarioLabel,
                        DateEvaluated, HasBeenRun, DNA, ScenStart, ScenEnd, ScenarioID);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in adding/updating scenario '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        /// <summary>
        /// Parse row to avoid NULL error
        /// </summary>
        /// <param name="row"></param>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        
        
        // moved to simlink.cs because this is needed by engine

        //public string ParseDataRow(DataRow row, string strColumnName)
        //{
        //    if (row.IsNull(strColumnName)) return "";
        //    else return row[strColumnName].ToString();
        //}




        /// <summary>
        /// load all scenario within the project and active Evaluation Group id into the grid
        /// </summary>
        /// <param name="strProjectID"></param>
        /// <param name="strActiveEVID"></param>
        /// <returns></returns>
        public DataSet LoadScenario(string strProjectID, string strActiveEVID)
        {
            string strSQL = "SELECT ScenarioID, EvalGroupID_FK, ScenarioLabel, DateEvaluated, HasBeenRun, "
                + "DNA, ScenStart, ScenEnd FROM tblScenario WHERE EvalGroupID_FK =" + strActiveEVID;
            return _dbContext.getDataSetfromSQL(strSQL);
        }
        /// <summary>
        /// Import scenario from csv file
        /// </summary>
        /// <param name="strCSVFile"></param>
        /// <param name="strProjectID"></param>
        /// <param name="strEvalID"></param>
        public void ImportScenarioFromCSV(string strCSVFile, string strProjectID, string strEvalID)
        {
            string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
            string strFolder = new FileInfo(strShortenedFile).DirectoryName;
            string strCSV = new FileInfo(strShortenedFile).Name;
            string strSQL = "INSERT INTO tblScenario(EvalGroupID_FK, ScenarioLabel, DNA, HasBeenRun, ScenStart, ScenEnd, DateEvaluated) " +
                            "SELECT " + strEvalID + ", ScenarioLabel, DNA, HasBeenRun, ScenStart, ScenEnd, DateEvaluated FROM [Text;FMT=TabDelimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
            _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
        }
        /// <summary>
        /// Get CSV field alias for insert and selection
        /// </summary>
        /// <param name="strCSVFile"></param>
        /// <param name="astrFieldAlias"></param>
        /// <returns></returns>
        private string GetCSVAliasFieldList(string strCSVFile, string[] astrFieldAlias)
        {
            string strFieldList = "";
            StreamReader read = new StreamReader(strCSVFile);
            string strHeader = read.ReadLine(); // first header
            string[] astrField = strHeader.Split(',');
            for (int intIndex = 0; intIndex <= astrField.GetUpperBound(0); intIndex++)
            {
                strFieldList += "[" + astrField[intIndex] + "] AS [" + astrFieldAlias[intIndex] + "],";
            }
            read.Close();
            strFieldList = strFieldList.Substring(0, strFieldList.Length - 1); // field list
            return strFieldList;
        }
        /// <summary>
        /// Import result from CSV file
        /// </summary>
        /// <param name="strCSVFile"></param>
        /// <param name="strEvalID"></param>
        public bool ImportResultFromCSV(string[] astrFieldAlias, string strCSVFile, string strEvalID)
        {
            try
            {

                string strFieldList = GetCSVAliasFieldList(strCSVFile, astrFieldAlias);

                string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;
                string strSQL = "INSERT INTO tblResultVar(EvaluationGroup_FK, Result_Label, VarResultType_FK, Result_Description, ElementID_FK, Element_Label, IsListVar, ImportResultDetail) " +
                                "VALUES ({0}, '{1}', {2}, '{3}', {4}, '{5}', '{6}', '{7}')";
                                //"SELECT " + strEvalID + ", AS EvaluationGroup_FK " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                //_dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                using (StreamReader read = new StreamReader(strShortenedFile))
                {
                    int intLineIndex = 0;
                    while (read.Peek() != -1)
                    {
                        string strLine = read.ReadLine();
                        if (strLine != "" && strLine != null && intLineIndex > 0)
                        {
                            string[] astrVal = strLine.Split(',');
                            string strNewSQL = string.Format(strSQL, strEvalID, astrVal[0].Trim(), astrVal[1].Trim(), astrVal[2].Trim(), 
                                astrVal[3].Trim(), astrVal[4].Trim(), astrVal[5].Trim(), astrVal[6].Trim());
                            IDbCommand cmd = _dbContext.CurrentCommand;
                            cmd.Connection = _dbContext.CurrentDBConnection;
                            cmd.CommandText = strNewSQL;
                            cmd.ExecuteNonQuery();
                        }
                        intLineIndex++;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
            }
        }
        /// <summary>
        /// Insert performance
        /// </summary>
        /// <param name="astrFieldAlias"></param>
        /// <param name="strCSVFile"></param>
        public bool ImportPerformanceFromCSV(string[] astrFieldAlias, string strCSVFile, string strEvalID_FK)
        {
            try
            {

                string strFieldList = GetCSVAliasFieldList(strCSVFile, astrFieldAlias);

                string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;
                string strSQL = "INSERT INTO tblPerformance(EvalID_FK, Performance_Label, PF_Type, LinkTableCode, Description, PF_FunctionType, FunctionID_FK, FunctionArgs, DV_ID_FK, OptionID_FK) " +
                                "SELECT " + strEvalID_FK + " AS [EvalID_FK], " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                return true;
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
            }
        }
        /// <summary>
        /// Import the event result from csv
        /// </summary>
        /// <param name="astrFieldAlias"></param>
        /// <param name="strCSVFile"></param>
        /// <returns></returns>
        public bool ImporEventResultFromCSV(string[] astrFieldAlias, string strCSVFile, string strEvalRefId)
        {
            try
            {

                string strFieldList = GetCSVAliasFieldList(strCSVFile, astrFieldAlias);
                string strSQL = "INSERT INTO tblResultTS_EventSummary(EvaluationGroupID_FK, " + string.Join(",", astrFieldAlias) + ") " +
                    "VALUES ({0}, {1}, {2}, {3}, {4}, " + IsBool4SQL(5) + ", {6}, " + IsBool4SQL(7) + ", " + IsBool4SQL(8) + ")";
                string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
                using(StreamReader read = new StreamReader(strShortenedFile))
                {
                    int intLineIndex = 0;
                    while(read.Peek() !=-1)
                    {
                        string strLine = read.ReadLine();
                        if (strLine != "" && strLine != null && intLineIndex>0)
                        {
                            string[] astrVal = strLine.Split(',');
                            string strNewSQL = string.Format(strSQL, strEvalRefId, astrVal[0].Trim(), astrVal[1].Trim(), astrVal[2].Trim(), astrVal[3].Trim(),
                                astrVal[4].Trim(), astrVal[5].Trim(), astrVal[6].Trim(), astrVal[7].Trim());
                            IDbCommand cmd = _dbContext.CurrentCommand;
                            cmd.Connection = _dbContext.CurrentDBConnection;
                            cmd.CommandText = strNewSQL;
                            cmd.ExecuteNonQuery();
                        }
                        intLineIndex++;
                    }
                }

                    /*
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;
                string strSQL = "INSERT INTO tblResultTS_EventSummary(EvaluationGroupID_FK, " + string.Join(",", astrFieldAlias) + ") " +
                                "SELECT " + strEvalRefId + " AS EvaluationGroup_FK, " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                 */
                return true;
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
            }
        }
        private void InsertCSV2DB(string strEvalID_FK, string[] astrFieldAlias, string strSQLInsertCmd, string strCSVFile)
        {
            IDbCommand cmd;
            if (_dbContext._DBContext_DBTYPE == DB_Type.SQL_SERVER)
            {
                cmd = new SqlCommand();
            }
            else
            {
                cmd = new OleDbCommand();
            }
            cmd.CommandText = strSQLInsertCmd; int intIndex = 2;
            cmd.Parameters.Add("@val1");

            foreach(string strVal in astrFieldAlias)
            {
                cmd.Parameters.Add("@val" + intIndex.ToString());
                intIndex++;
            }
 
            string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
            using(StreamReader read = new StreamReader(strShortenedFile))
            {
                while(read.Peek() !=-1)
                {
                    cmd.Parameters["@val1"] = strEvalID_FK;
                    string strLine = read.ReadLine();
                    int intLoop = 2;
                    if (strLine != "" && strLine !=null)
                    {
                        string[] astrVal = strLine.Split(',');
                        foreach (string strVal in astrVal)
                        {
                            cmd.Parameters["@val" + intLoop.ToString()] = strVal;
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }

        }
        /// <summary>
        /// Insert performance
        /// </summary>
        /// <param name="astrFieldAlias"></param>
        /// <param name="strCSVFile"></param>
        public bool ImportEventResultFromCSV(string[] astrFieldAlias, string strCSVFile)
        {
            try
            {

                string strFieldList = GetCSVAliasFieldList(strCSVFile, astrFieldAlias);

                string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;
                string strSQL = "INSERT INTO tblPerformance(Performance_Label, PF_Type, LinkTableCode, Description, PF_FunctionType, FunctionID_FK, FunctionArgs, DV_ID_FK, OptionID_FK) " +
                                "SELECT " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql

                return true;
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
            }
        }
        /// <summary>
        /// Import DV from CSV file
        /// </summary>
        /// <param name="strCSVFile"></param>
        public bool ImportDVFromCSV(string[] astrFieldAlias, string strCSVFile, string strEvalRefId)
        {
            try
            {
                //string[] astrFieldAlias = new string[]{"DV_Label","EvaluationGroup_FK","VarType_FK","DV_Description","DV_Type","Option_FK",
                //        "Option_MIN","Option_MAX","GetNewValMethod","FunctionID_FK","FunctionArgs","ElementID_FK","sqn","Operation_DV",
                //        "SecondaryDV_Key","PrimaryDV_ID_FK","IsSpecialCase"};
                string strFieldList = GetCSVAliasFieldList(strCSVFile, astrFieldAlias);
                string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;

                string strSQL = "INSERT INTO tblDV(EvaluationGroup_FK, XModelID_FK, DV_Label, VarType_FK,DV_Description,DV_Type,Option_FK," +
                    "Option_MIN,Option_MAX,GetNewValMethod,FunctionID_FK,FunctionArgs,ElementID_FK,sqn," +
                    "SecondaryDV_Key,PrimaryDV_ID_FK,IsSpecialCase) " +
                    "SELECT " + strEvalRefId + " AS EvaluationGroup_FK, -1 AS XModelID_FK, " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql

                strSQL = "UPDATE tblDV SET PrimaryDV_ID_FK=-1 WHERE PrimaryDV_ID_FK IS NULL";
                _dbContext.ExecuteNonQuerySQL(strSQL); // set the default value
                return true;
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
            }
        }
        public bool ImportCSVDataFile2Grid(string strTableName, string[] astrSQLField, string[] astrDataType, 
            string strCSVFileName, string strEvalIDFieldName, string strEvalID, out string strErrorMessage)
        {
            strErrorMessage = "";
            try
            {
                string strSQL = "INSERT INTO " + strTableName + "(";
                foreach (string strFld in astrSQLField)
                {
                    strSQL += strFld + ",";
                }
                if (strSQL.Length > 1) strSQL = strSQL.Substring(0, strSQL.Length - 1) + (strEvalID == "" ? "" : "," + strEvalIDFieldName) + ") "; //VALUES ("; // sql length
                if (strTableName.ToLower() == "tbldv")
                {
                    strSQL = strSQL.ToLower().Replace("elementid", "ElementID_FK");
                    strSQL = strSQL.ToLower().Replace("optionid", "Option_FK");
                    strSQL = strSQL.ToLower().Replace("evalid_fk", "EvaluationGroup_FK");
                }
                /*
                int intIndex = 0;
                // loop through data type
                foreach(string strDataType in astrDataType)
                {
                    if (CommonUtilities.IsNumericDataType(strDataType))
                    {
                        strSQL += "{" + intIndex + "},";
                    }
                    else
                    {
                        strSQL += "'{" + intIndex + "}',";
                    }
                    intIndex++;
                }
                if (strSQL.Length > 1) strSQL = strSQL.Substring(0, strSQL.Length - 1) + (strEvalID == "" ? "" : ",{" + intIndex +"}") + ")";
                */
                string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFileName);
                using (StreamReader read = new StreamReader(strShortenedFile))
                {
                    int intCounter = 0; string strValue = "";
                    while (read.Peek() != -1)
                    {
                        string strLine = read.ReadLine().Trim();
                        if (intCounter > 0)
                        {
                            string[] astrVals = strLine.Split(',');
                            for (int iintLoop = 0; iintLoop <= astrVals.GetUpperBound(0); iintLoop++)
                            {
                                if (CommonUtilities.IsNumericDataType(astrDataType[iintLoop]))
                                {
                                    strValue += astrVals[iintLoop] + ",";
                                }
                                else if (CommonUtilities.IsBoolDataType(astrDataType[iintLoop]))
                                {
                                    strValue += (_dbContext._DBContext_DBTYPE == DB_Type.SQL_SERVER ? "'" + astrVals[iintLoop] + "'" : astrVals[iintLoop]) + ",";
                                }
                                else
                                {
                                    strValue += "'" + astrVals[iintLoop] + "'" + ",";
                                }
                            }
                            if (strValue.Length > 1) strValue = strValue.Substring(0, strValue.Length - 1); // get the value length
                            if (strEvalID != "")
                            {
                                strValue += "," + strEvalID;
                            }
                            string strNewSQL = strSQL + " VALUES (" + strValue + ")";
                            _dbContext.ExecuteNonQuerySQL(strNewSQL); // execute import from sql
                            strValue = "";
                        }
                        intCounter++;
                    }
                }
                return true; 
            }
            catch (Exception ex)
            {
                strErrorMessage = "Error importing CSV file to the grid '" + ex.Source + "' and '" + ex.Message +"'";
                return false;
            }
        }
        /// <summary>
        /// Import result summary from CSV
        /// </summary>
        /// <param name="astrFieldAlias"></param>
        /// <param name="strCSVFile"></param>
        /// <returns></returns>
        public bool ImportResultTimeseriesFromCSV(string[] astrFieldAlias, string strCSVFile, string strEvalId)
        {
            try
            {
                string strFieldList = GetCSVAliasFieldList(strCSVFile, astrFieldAlias);
                string strShortenedFile = CommonUtilities.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;

                string strSQL = "INSERT INTO tblResultTS(EvaluationGroup_FK,Result_Label, VarResultType_FK, Result_Description, ElementID_FK, Element_Label, SQN, FunctionID_FK) " +
                    "SELECT " + strEvalId + " As EvaluationGroup_FK, " + strFieldList + ", 1 AS SQN, -1 As FunctionID_FK FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                return true;
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
            }
        }
        /// <summary>
        /// Get result var type look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultVarTypeLookup()
        {

            string strSQL = "";
            //SP 7-Feb-2016 - added functionality for referencing EPANET (in addition to already referenced SWMM) results dictionary
            //SQL string depending on the active model type
            switch (_nActiveModelTypeID)
            {
                case CommonUtilities._nModelTypeID_SWMM:
                    strSQL = "SELECT tlkpSWMMResults_FieldDictionary.ResultsFieldID, tlkpSWMMResults_TableDictionary.TableName, tlkpSWMMResults_FieldDictionary.FieldName " +
                        "FROM tlkpSWMMResults_FieldDictionary INNER JOIN tlkpSWMMResults_TableDictionary ON tlkpSWMMResults_FieldDictionary.TableID_FK = tlkpSWMMResults_TableDictionary.ResultTableID WHERE (((tlkpSWMMResults_FieldDictionary.IsOutFileVar)="+ IsBool4SQL("False") + "))";
                    break;

                case CommonUtilities._nModelTypeID_EPANET:
                    strSQL = "SELECT tlkpEPANET_ResultsFieldDictionary.ResultsFieldID, tlkpEPANET_ResultsFieldDictionary.FieldName " +
                        "FROM tlkpEPANET_ResultsFieldDictionary WHERE (((tlkpEPANET_ResultsFieldDictionary.IsResult)=" + IsBool4SQL("True") + "))";
                    break;
                
                default:
                    _log.AddString("Error: Model type does not have lookup values defined in UI.", Logging._nLogging_Level_1);
                    strSQL = "SELECT tlkpSWMMResults_FieldDictionary.ResultsFieldID, tlkpSWMMResults_TableDictionary.TableName, tlkpSWMMResults_FieldDictionary.FieldName " +
                        "FROM tlkpSWMMResults_FieldDictionary INNER JOIN tlkpSWMMResults_TableDictionary ON tlkpSWMMResults_FieldDictionary.TableID_FK = tlkpSWMMResults_TableDictionary.ResultTableID WHERE (((tlkpSWMMResults_FieldDictionary.IsOutFileVar)=" + IsBool4SQL("False") + "))";
                    break;
            }
            
             
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get result var type look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultTimeseriesVarTypeLookup()
        {

            string strSQL = "";
            //SP 8-Feb-2016 - added functionality for referencing EPANET (in addition to already referenced SWMM) results dictionary
            //SQL string depending on the active model type
            switch (_nActiveModelTypeID)
            {
                case CommonUtilities._nModelTypeID_SWMM:
                    strSQL = "SELECT tlkpSWMMResults_FieldDictionary.ResultsFieldID, "
                                + "tlkpSWMMResults_FieldDictionary.FeatureType, tlkpSWMMResults_FieldDictionary.FieldName "
                                + "FROM tlkpSWMMResults_FieldDictionary WHERE (((tlkpSWMMResults_FieldDictionary.IsOutFileVar)= " + IsBool4SQL("True") + "))";
                    break;

                case CommonUtilities._nModelTypeID_EPANET:
                    strSQL = "SELECT tlkpEPANET_ResultsFieldDictionary.ResultsFieldID, "
                                + "tlkpEPANET_ResultsFieldDictionary.FeatureType, tlkpEPANET_ResultsFieldDictionary.FieldName "
                                + "FROM tlkpEPANET_ResultsFieldDictionary WHERE (((tlkpEPANET_ResultsFieldDictionary.IsResult)=" + IsBool4SQL("True") + "))";
                    break;

                default:
                    _log.AddString("Error: Model type does not have lookup values defined in UI.", Logging._nLogging_Level_1);
                    strSQL = "SELECT tlkpSWMMResults_FieldDictionary.ResultsFieldID, "
                                + "tlkpSWMMResults_FieldDictionary.FeatureType, tlkpSWMMResults_FieldDictionary.FieldName "
                                + "FROM tlkpSWMMResults_FieldDictionary WHERE (((tlkpSWMMResults_FieldDictionary.IsOutFileVar)=" + IsBool4SQL("True") + "))";
                    break;
            }

            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get var type look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetVarTypeLookUp()
        {
            //string strSQL = "SELECT tlkpSWMMTableDictionary.TableName, tlkpSWMMTableDictionary.KeyColumn, tlkpSWMMFieldDictionary.FieldName, tlkpSWMMFieldDictionary.ID AS VarType_FK, tlkpSWMMTableDictionary.ID AS [TableID], FieldINP_Number, IsScenarioSpecific, RowNo, SectionNumber, SectionName "
            //            + "FROM tlkpSWMMFieldDictionary INNER JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID";
            string strSQL = "";
            switch (_nActiveModelTypeID)
            {
                case CommonUtilities._nModelTypeID_SWMM:
                    strSQL = "SELECT tlkpSWMMTableDictionary.TableName, tlkpSWMMFieldDictionary.FieldName, tlkpSWMMFieldDictionary.ID AS VarType_FK "
            + "FROM tlkpSWMMFieldDictionary INNER JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID";
                    break;

                case CommonUtilities._nModelTypeID_EPANET:
                    strSQL = "SELECT tlkpEPANET_TableDictionary.TableName, tlkpEPANETFieldDictionary.FieldName, tlkpEPANETFieldDictionary.ID AS VarType_FK" 
                                +" FROM tlkpEPANETFieldDictionary INNER JOIN tlkpEPANET_TableDictionary ON tlkpEPANETFieldDictionary.TableName_FK = tlkpEPANET_TableDictionary.ID";
                    break;
                default:
                    _log.AddString("Error: Model type does not have lookup values defined in UI.", Logging._nLogging_Level_1);
                    strSQL = "SELECT tlkpSWMMTableDictionary.TableName, tlkpSWMMFieldDictionary.FieldName, tlkpSWMMFieldDictionary.ID AS VarType_FK "
                + "FROM tlkpSWMMFieldDictionary INNER JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID";
                    break;

            }
                
                
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get option look-up
        /// </summary>
        /// <returns></returns>
        public DataTable GetOptionLookUp(string strProjectID)
        {
            string strSQL = "SELECT OptionID, OptionLabel FROM tblOptionLists WHERE ProjID_FK=" + strProjectID + " ORDER By OptionLabel";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get option look-up
        /// </summary>
        /// <returns></returns>
        public DataTable GetOptionLookUp4Performance(string strProjectID)
        {
            string strSQL = "SELECT -1 AS OptionID, '_Not Set' As OptionLabel FROM tblOptionLists UNION SELECT OptionID, OptionLabel FROM tblOptionLists WHERE ProjID_FK=" + strProjectID + " ORDER By OptionLabel";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get element look up detail
        /// </summary>
        /// <param name="strProjectID"></param>
        /// <returns></returns>
        public DataTable GetElementLookUp(string strProjectID)
        {
            string strSQL = "SELECT ElementListID, ElementListLabel, Type FROM tblElementLists WHERE ProjID_FK=" + strProjectID + " ORDER By ElementListLabel";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get option look-up
        /// </summary>
        /// <returns></returns>
        public DataTable GetOptionLookUp()
        {
            string strSQL = "SELECT OptionID, OptionLabel FROM tblOptionLists ORDER By OptionLabel";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get function look up table
        /// </summary>
        /// <param name="strProjectID"></param>
        /// <returns></returns>
        public DataTable GetFunctionTableLookup(string strProjectID)
        {
            string strSQL = "SELECT -1 AS FunctionID, '_Not Set' As Label, 'Not Set' As Category, 'Not Set' As CustomFunction FROM tblFunctions UNION SELECT FunctionID, Label, Category, CustomFunction FROM tblFunctions WHERE ProjID_FK = " + strProjectID + " order by Label";
            return _dbContext.getDataSetfromSQL(strSQL).Tables[0];
        }
        /// <summary>
        /// Get function type look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetFunctionTypeLookUp()
        {
            string[] astrVal = "Function,Lookup".Split(',');
            string[] astrValID = "1,2".Split(',');
            DataTable dt = new DataTable();
            dt.Columns.Add("FunctionTypeID", typeof(int));
            dt.Columns.Add("FunctionTypeName", typeof(string));

            for (int intIndex = 0; intIndex <= astrVal.GetUpperBound(0); intIndex++)
            {
                DataRow row = dt.NewRow();
                row["FunctionTypeID"] = int.Parse(astrValID[intIndex]);
                row["FunctionTypeName"] = astrVal[intIndex];
                dt.Rows.Add(row);
            }
            dt.AcceptChanges();
            return dt;
        }
        /// <summary>
        /// Get the secondary table
        /// </summary>
        /// <returns></returns>
        public DataTable GetSecondaryDVLookup()
        { 
            int[] aintVal = new int[]{-1, 1, 2, 3};
            string[] astrSecondary = new string[]{"NotASecondaryDV", "OptionSetFromOptionList", "OptionSetFromPrimaryDV", "MaintainExistingValue"};
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Value", typeof(string));
            for (int intLoop=0; intLoop <= aintVal.GetUpperBound(0); intLoop++)
            {
                DataRow row = dt.NewRow();
                row["ID"] = aintVal[intLoop];
                row["Value"] = astrSecondary[intLoop];
                dt.Rows.Add(row);
            }
            dt.AcceptChanges();
            return dt;

        }

        /// <summary>
        /// Get DV lookup
        /// </summary>
        /// <returns></returns>
        public DataTable GetParimaryDVLookUp(int intEvalGroupID)
        {
            string strSQL = "SELECT -1 as DVD_ID, 'Not Set' As DV_Label FROM tblDV UNION SELECT DVD_ID, DV_Label FROM tblDV WHERE EvaluationGroup_FK = " + intEvalGroupID.ToString() + " order by DV_Label";
            return _dbContext.getDataSetfromSQL(strSQL).Tables[0];
        }
        /// <summary>
        /// Get DV lookup
        /// </summary>
        /// <returns></returns>
        public DataTable GetDVLookUp4Performance(int intEvalGroupID)
        {
            string strSQL = "SELECT -1 AS DVD_ID, '_Not SET' AS DV_Label FROM tblDV UNION SELECT DVD_ID, DV_Label FROM tblDV WHERE EvaluationGroup_FK = " + intEvalGroupID.ToString() + " order by DV_Label";
            return _dbContext.getDataSetfromSQL(strSQL).Tables[0];
        }
        public DataTable GetPerf_FunctionOnLinkedData()
        {
            string strSQL = "SELECT VALINT, Subcategory FROM tlkpUI_Dictionary WHERE Category = 'Perf_FunctionOnLinkedData'";
            return _dbContext.getDataSetfromSQL(strSQL).Tables[0];
        }
        /// <summary>
        /// Get DV look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetDVLookUp()
        {
            string strSQL = "SELECT DVD_ID, DV_Label FROM tblDV order by DV_Label";
            return _dbContext.getDataSetfromSQL(strSQL).Tables[0];
        }
        /// <summary>
        /// Get primary DV lookup
        /// </summary>
        /// <returns></returns>
        public DataTable GetPrimaryDVLookUp()
        {
            string[] astrVal = "Primary DV".Split(',');
            string[] astrValID = "-1".Split(',');
            DataTable dt = new DataTable();
            dt.Columns.Add("DVD_ID", typeof(int));
            dt.Columns.Add("DV_Label", typeof(string));

            for (int intIndex = 0; intIndex <= astrVal.GetUpperBound(0); intIndex++)
            {
                DataRow row = dt.NewRow();
                row["DVD_ID"] = int.Parse(astrValID[intIndex]);
                row["DV_Label"] = astrVal[intIndex];
                dt.Rows.Add(row);
            }
            dt.AcceptChanges();
            return dt;
        }
        /// <summary>
        /// get category look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryLookUp()
        {
            string strSQL = "SELECT -1 AS CategoryID, '_Not Set' AS Label, 'Not Set' AS Description FROM tblCategory UNION SELECT CategoryID, Label, Description FROM tblCategory order by Label";
            return _dbContext.getDataSetfromSQL(strSQL).Tables[0];
        }
        /// <summary>
        /// Get all table code look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetLinkTableCodeLookUp()
        {
            string strValName = "Not Set,Result Summary,Timeseries Result,DV Option,Event,Performance";
            string strValID = "-1,2,3,4,5,6";
            string[] astrVal = strValName.Split(',');
            string[] astrValID = strValID.Split(',');
            DataTable dt = new DataTable();
            dt.Columns.Add("LinkTableCodeID", typeof(int));
            dt.Columns.Add("LinkTableCodeName", typeof(string));

            for (int intIndex = 0; intIndex <= astrVal.GetUpperBound(0); intIndex++)
            {
                DataRow row = dt.NewRow();
                row["LinkTableCodeID"] = int.Parse(astrValID[intIndex]);
                row["LinkTableCodeName"] = astrVal[intIndex];
                dt.Rows.Add(row);
            }
            dt.AcceptChanges();
            return dt;
        }
        public DataTable GetLinkTableCodeLookUp(int intFunctionType)
        {
            string strValName = "Result Summary,Timeseries Result,DV Option,Event,Performance";
            string strValID = "2,3,4,5,6";
            string[] astrVal = (intFunctionType == 1 ? "Not Set" : strValName).Split(',');
            string[] astrValID = (intFunctionType == 1 ? "-1" : strValID).Split(',');
            DataTable dt = new DataTable();
            dt.Columns.Add("LinkTableCodeID", typeof(int));
            dt.Columns.Add("LinkTableCodeName", typeof(string));

            for (int intIndex = 0; intIndex <= astrVal.GetUpperBound(0); intIndex++)
            {
                DataRow row = dt.NewRow();
                row["LinkTableCodeID"] = int.Parse(astrValID[intIndex]);
                row["LinkTableCodeName"] = astrVal[intIndex];
                dt.Rows.Add(row);
            }
            dt.AcceptChanges();
            return dt;
        }
        /// <summary>
        /// Event functon ID
        /// </summary>
        /// <returns></returns>
        public DataTable GetEventFunction()
        {
            string[] astrVal = "ThresholdAndDurationCalculations".Split(',');
            string[] astrValID = "-1".Split(',');

            DataTable dt = new DataTable();
            dt.Columns.Add("EventFunctionID", typeof(int));
            dt.Columns.Add("EventFunctionName", typeof(string));

            for (int intIndex = 0; intIndex <= astrVal.GetUpperBound(0); intIndex++)
            {
                DataRow row = dt.NewRow();
                row["EventFunctionID"] = int.Parse(astrValID[intIndex]);
                row["EventFunctionName"] = astrVal[intIndex];
                dt.Rows.Add(row);
            }
            dt.AcceptChanges();
            return dt;

        }
        public DataTable GetPFTypeLookUp()
        {
            string[] astrVal = "Cost,Performance".Split(',');
            string[] astrValID = "1,2".Split(',');
            DataTable dt = new DataTable();
            dt.Columns.Add("PF_TypeID", typeof(int));
            dt.Columns.Add("PF_TypeName", typeof(string));

            for (int intIndex = 0; intIndex <= astrVal.GetUpperBound(0); intIndex++)
            {
                DataRow row = dt.NewRow();
                row["PF_TypeID"] = int.Parse(astrValID[intIndex]);
                row["PF_TypeName"] = astrVal[intIndex];
                dt.Rows.Add(row);
            }
            dt.AcceptChanges();
            return dt;
        }
        public DataTable GetPerformanceResultXRefDefault(bool blnApplyThreshold)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ThresholdValue", typeof(string));

            DataRow row = dt.NewRow();
            row["ID"] = -2;
            if (blnApplyThreshold)
            {
                row["ThresholdValue"] = "Default- From Component Apply Threshold";
            }
            else
            {
                row["ThresholdValue"] = "Default - From Component IsOver Threshold";
            }
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["ID"] = 0;
            row["ThresholdValue"] = "False";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["ID"] = GetBoolValue4SQLOrOleDb();
            row["ThresholdValue"] = "True";
            dt.Rows.Add(row);
            return dt;
        }
        /// <summary>
        /// Get boolean value from a True / False String
        /// </summary>
        /// <param name="strBooleanString"></param>
        /// <returns></returns>
        public int GetBoolValue4SQLOrOleDb(string strBooleanString)
        {
            int intBooleanValue = 0;
            if (strBooleanString.ToLower() == "true")
            {
                intBooleanValue = (_dbContext._DBContext_DBTYPE == DB_Type.SQL_SERVER ? 1 : -1);
            }
            else
            {
                intBooleanValue = 0;
            }
            return intBooleanValue;
        }
        public int GetBoolValue4SQLOrOleDb()
        {
            return (_dbContext._DBContext_DBTYPE == DB_Type.SQL_SERVER ? 1 : -1); 
        }
        //SP 12-Feb-2016 - for the Performance Vars
        public DataTable GetLinkedRecordsLookUp(int nLinkDataCode, int nEvalID)
        {
            string strSQL = "";
            switch ((LinkedDataType)nLinkDataCode)
            {
                //tblResultVar.Result_ID is a field in tblPerformance_ResultXREF
                case LinkedDataType.ResultSummary:
                    strSQL = "SELECT tblResultVar.Result_ID AS ID, tblResultVar.Result_Label AS LinkedRecordLabel"
                                + " FROM tblResultVar WHERE ((tblResultVar.EvaluationGroup_FK)= " + nEvalID.ToString() + ")";
                    break;

                //DVD_ID is a field in tblPerformance - but if DV Option is selected should allow a selection of the DV
                /*case CommonUtilities._nLinkedDataTypeID_DVOptions:
                    strSQL = "SELECT 'DV Option' as LinkedRecordData, tblDV.DVD_ID, tblDV.DV_Label "
                                + "FROM tblDV WHERE ((tblDV.EvaluationGroup_FK)= " + nEvalID.ToString() + ")";
                    break;*/

                //tblResultTS_EventSummary.EventSummaryID is a field in tblPerformance_ResultXREF
                case LinkedDataType.ResultTS:
                    strSQL = "SELECT tblResultTS.ResultTS_ID as ID, tblResultTS.Result_Label AS LinkedRecordLabel"
                                + " FROM tblResultTS WHERE ((tblResultTS.EvaluationGroup_FK)= " + nEvalID.ToString() + ")";
                    break;

                //tblResultTS_EventSummary.EventSummaryID is a field in tblPerformance_ResultXREF
                case LinkedDataType.Event:
                    strSQL = "SELECT tblResultTS_EventSummary.EventSummaryID AS ID, tblResultTS.Result_Label AS LinkedRecordLabel"
                                + " FROM tblResultTS_EventSummary INNER JOIN tblResultTS on tblResultTS_EventSummary.ResultTS_or_Event_ID_FK = tblResultTS.ResultTS_ID"
                                + " WHERE ((tblResultTS_EventSummary.EvaluationGroupID_FK)= " + nEvalID.ToString() + ")";
                    break;

                //tblPerformance.PerformanceID is a field in tblPerformance_ResultXREF
                case LinkedDataType.Performance:
                    strSQL = "SELECT tblPerformance.PerformanceID AS ID, tblPerformance.Performance_Label AS LinkedRecordLabel"
                                + " FROM tblPerformance WHERE ((tblPerformance.EvalID_FK)= " + nEvalID.ToString() + ")";
                    break;

                default:
                    _log.AddString("LinkDataCode does not have LinkedRecord lookup values defined for Performance", Logging._nLogging_Level_1);
                    break;

            }


            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //SP 12-Feb-2016
        public DataSet GetLinkedRecordDetails(int nPerfID)
        {

            //Result Summary Linked Records
            string strSQL = "SELECT tblPerformance_ResultXREF.ID AS RecordID, tblResultVar.Result_ID as ID, tblResultVar.Result_Label as LinkedRecordLabel, tblPerformance_ResultXREF.LinkType, tblPerformance_ResultXREF.LinkTableID_FK, "
                            + " tblPerformance_ResultXREF.ApplyThreshold, tblPerformance_ResultXREF.Threshold, tblPerformance_ResultXREF.IsOver_Threshold FROM tblResultVar INNER JOIN tblPerformance_ResultXREF ON tblPerformance_ResultXREF.LinkTableID_FK = tblResultVar.Result_ID"
                            + " WHERE ((tblPerformance_ResultXREF.PerformanceID_FK)= " + nPerfID.ToString()
                            + " AND (tblPerformance_ResultXREF.LinkType)= " + ((int)LinkedDataType.ResultSummary).ToString() + ")";

            //DV Options Linked Records - part of tblPerformance - entered on the main Performance tab
            /*strSQL = strSQL + " UNION"
                        + " SELECT tblDV.DVD_ID as ID, tblDV.DV_Label as LinkedRecordLabel"
                        + " FROM tblDV INNER JOIN tblPerformance ON tblPerformance.DV_ID_FK = tblDV.DVD_ID"
                        + " WHERE ((tblPerformance.PerformanceID)= " + nPerfID.ToString() + ")";*/

            //ResultTimeSeries Linked Records
            strSQL = strSQL + " UNION"
                            + " SELECT tblPerformance_ResultXREF.ID AS RecordID, tblResultTS.ResultTS_ID as ID, tblResultTS.Result_Label as LinkedRecordLabel, tblPerformance_ResultXREF.LinkType, tblPerformance_ResultXREF.LinkTableID_FK, "
                            + " tblPerformance_ResultXREF.ApplyThreshold, tblPerformance_ResultXREF.Threshold, tblPerformance_ResultXREF.IsOver_Threshold "
                            + " FROM tblResultTS INNER JOIN tblPerformance_ResultXREF ON tblPerformance_ResultXREF.LinkTableID_FK = tblResultTS.ResultTS_ID"
                            + " WHERE ((tblPerformance_ResultXREF.PerformanceID_FK)= " + nPerfID.ToString()
                            + " AND (tblPerformance_ResultXREF.LinkType)= " + ((int)LinkedDataType.ResultTS).ToString() + ")";

            //Event Linked Records
            strSQL = strSQL + " UNION"
                    + " SELECT tblPerformance_ResultXREF.ID AS RecordID, tblResultTS_EventSummary.EventSummaryID as ID, tblResultTS.Result_Label as LinkedRecordLabel, tblPerformance_ResultXREF.LinkType, tblPerformance_ResultXREF.LinkTableID_FK, "
                    + " tblPerformance_ResultXREF.ApplyThreshold, tblPerformance_ResultXREF.Threshold, tblPerformance_ResultXREF.IsOver_Threshold "
                    + " FROM ((tblResultTS_EventSummary INNER JOIN tblPerformance_ResultXREF ON tblPerformance_ResultXREF.LinkTableID_FK = tblResultTS_EventSummary.EventSummaryID)"
                    + " INNER JOIN tblResultTS on tblResultTS_EventSummary.ResultTS_or_Event_ID_FK = tblResultTS.ResultTS_ID)"
                    + " WHERE ((tblPerformance_ResultXREF.PerformanceID_FK)= " + nPerfID.ToString()
                    + " AND (tblPerformance_ResultXREF.LinkType)= " + ((int)LinkedDataType.Event).ToString() + ")";

            //Performance Linked Records
            strSQL = strSQL + " UNION"
                    + " SELECT tblPerformance_ResultXREF.ID AS RecordID, tblPerformance.PerformanceID as ID, tblPerformance.Performance_Label as LinkedRecordLabel, tblPerformance_ResultXREF.LinkType, tblPerformance_ResultXREF.LinkTableID_FK, "
                    + " tblPerformance_ResultXREF.ApplyThreshold, tblPerformance_ResultXREF.Threshold, tblPerformance_ResultXREF.IsOver_Threshold "
                    + " FROM tblPerformance INNER JOIN tblPerformance_ResultXREF ON tblPerformance_ResultXREF.LinkTableID_FK = tblPerformance.PerformanceID"
                    + " WHERE ((tblPerformance_ResultXREF.PerformanceID_FK)= " + nPerfID.ToString()
                    + " AND (tblPerformance_ResultXREF.LinkType)= " + ((int)LinkedDataType.Performance).ToString() + ")";


            DataSet dsDetail = _dbContext.getDataSetfromSQL(strSQL);
            dsDetail.Tables[0].TableName = "tblPerformanceLinkedRecords";

            return dsDetail;
        }
        /// <summary>
        /// Performance update
        /// </summary>
        /// <param name="nLinkDataCode"></param>
        /// <param name="nLinkedTypeID"></param>
        /// <param name="intRecordId"></param>
        public void PerformanceUpdateLinkedRecord(int nLinkDataCode, int nLinkedTypeID, int intRecordId, 
            int intApplyThreshold, double dblThreshold, int intIsOverApplyThreshold)
        {
            string strSQL = "UPDATE tblPerformance_ResultXREF SET "
                           + "LinkTableID_FK = {0}, "
                           + "LinkType = {1}, "
                           + "ApplyThreshold = {2}, "
                           + "Threshold = {3}, "
                           + "IsOver_Threshold = {4} "
                           + "WHERE ID ={5}";
            string strNewSQL = string.Format(strSQL, nLinkDataCode, nLinkedTypeID, 
                intApplyThreshold, dblThreshold, intIsOverApplyThreshold, intRecordId);

            _dbContext.ExecuteNonQuerySQL(strNewSQL);
        }
        /// <summary>
        /// Update the performance linked record
        /// </summary>
        /// <param name="nLinkedTypeID"></param>
        /// <param name="intRecordId"></param>
        public void PerformanceUpdateLinkTableCode(int nLinkedTypeID, int intPerformanceId)
        {
            string strSQL = "UPDATE tblPerformance SET LinkTableCode = {0} WHERE PerformanceID ={1}";
            string strNewSQL = string.Format(strSQL, nLinkedTypeID, intPerformanceId);

            _dbContext.ExecuteNonQuerySQL(strNewSQL);
        }
        //SP 16-Feb-2016 - add a new Performance Linked Record
        public void PerformanceCreateLinkedRecord(int nPerfID, int nLinkDataCode, int nLinkedRecordID, 
            int intApplyThreshold, double dblThreshold, int intIsOverApplyThreshold)
        {
            string strSQL = "";
            string strNewSQL = "";

            //DVD_ID is a field in tblPerformance - but if DV Option is selected should allow a selection of the DV
            /*case CommonUtilities._nLinkedDataTypeID_DVOptions:
                strSQL = "Update tblPerformance set DV_ID_FK = {0} WHERE ((tblPerformance.PerformanceID)= {1})";
                strNewSQL = string.Format(strSQL, nLinkedRecordID.ToString(), nPerfID.ToString());
                break;*/

            strSQL = "INSERT INTO tblPerformance_ResultXREF(PerformanceID_FK, LinkTableID_FK, ScalingFactor, LinkType, ApplyThreshold, Threshold, IsOver_Threshold) " +
                " VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})";
            strNewSQL = string.Format(strSQL, nPerfID, nLinkedRecordID, 1, nLinkDataCode, intApplyThreshold, dblThreshold, intIsOverApplyThreshold);

            _dbContext.ExecuteNonQuerySQL(strNewSQL);
        }

        //SP 16-Feb-2016 - delete a Performance Linked Record
        public void PerformanceDeleteLinkedRecord(int nPerfID, int nLinkDataCode, int nLinkedRecordID)
        {
            string strSQL = "";
            string strDeleteSQL = "";

            strSQL = "DELETE from tblPerformance_ResultXREF WHERE PerformanceID_FK = {0} and LinkTableID_FK = {1} and LinkType = {2}";
            strDeleteSQL = string.Format(strSQL, nPerfID, nLinkedRecordID, nLinkDataCode);

            _dbContext.ExecuteNonQuerySQL(strDeleteSQL); // delete Linked Record
        }


        /// <summary>
        /// Operation lookup 
        /// </summary>
        /// <returns></returns>
        public DataTable GetOperationLookUp()
        {
            string[] astrVal = "Identity,Add,Subtract,Multiply,Divide".Split(','); //SP 4-Aug-2016 TODO use OperationType Enum and test
            DataTable dt = new DataTable();
            dt.Columns.Add("Operation");
            foreach (string strVal in astrVal)
            {
                DataRow row = dt.NewRow();
                row["Operation"] = strVal;
                dt.Rows.Add(row);
            }
            dt.AcceptChanges();
            return dt;
        }
        public DataTable GetNewValMethodLookUp()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NewMethodValID", typeof(Single));
            dt.Columns.Add("NewMethodName", typeof(string));

            DataRow row = dt.NewRow();
            row["NewMethodValID"] = 1;
            row["NewMethodName"] = "Lookup";

            dt.Rows.Add(row);

            DataRow row2 = dt.NewRow();
            row2["NewMethodValID"] = 2;
            row2["NewMethodName"] = "Function";
            dt.Rows.Add(row2);

            DataRow row3 = dt.NewRow();
            row3["NewMethodValID"] = -1;
            row3["NewMethodName"] = "None";
            dt.Rows.Add(row3);

            dt.AcceptChanges();
            return dt;
        }
        public DataTable GetElementLookUpWithNewDefine()
        {
            string strSQL = "SELECT -1 AS ElementListID, '__DEFINE NEW LABEL' AS ElementListLabel FROM tblElementLists UNION SELECT ElementListID, ElementListLabel FROM tblElementLists ORDER By ElementListLabel";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        public DataTable GetElementLookUp()
        {
            string strSQL = "SELECT ElementListID, ElementListLabel FROM tblElementLists ORDER By ElementListLabel";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get result TS look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultTSLookUp(string strReferenceId)
        {
            string strSQL = "SELECT -1 AS ResultTS_ID, 'Not Set' AS Result_Label FROM tblResultTS UNION SELECT ResultTS_ID, Result_Label FROM tblResultTS WHERE EvaluationGroup_FK = " + strReferenceId + " ORDER By Result_Label";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        public DataTable GetEVGroupLookUp()
        {
            string strSQL = "SELECT EvaluationID, EvaluationLabel FROM tblEvaluationGroup ORDER By EvaluationLabel";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }

        /// <summary>
        /// function to return the first EG in a project
        ///     enhancements: store last EG in config (make easier on user)
        /// </summary>
        /// <param name="nProj"></param>
        /// <returns></returns>
        public int GetEvalIDFromProj(int nProj)
        {
            string strSQL = "SELECT EvaluationID, EvaluationLabel FROM tblEvaluationGroup WHERE ProjID_FK = " + nProj + " ORDER By EvaluationID";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return Convert.ToInt32(dsEG.Tables[0].Rows[0]["EvaluationID"]);
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return -1;  // this would indicate a data error; there can never be no EG
            }

        }

        /// <summary>
        /// Get EV Group look up
        /// </summary>
        /// <param name="strProjectID"></param>
        /// <returns></returns>
        public DataTable GetEVGroupLookUp(string strProjectID)
        {
            string strSQL = "SELECT -1 As EvaluationID, 'New Independent Evaluation' AS EvaluationLabel FROM tblEvaluationGroup UNION SELECT EvaluationID, EvaluationLabel FROM tblEvaluationGroup WHERE ProjID_FK = " + strProjectID + " ORDER By EvaluationLabel";
            try
            {
                DataSet dsEG = _dbContext.getDataSetfromSQL(strSQL);
                return dsEG.Tables[0];
            }
            catch (Exception ex)
            {
                //sim2     cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return null;
            }
        }
        /// <summary>
        /// Get result timeseries
        /// </summary>
        /// <param name="intActiveEvalID"></param>
        /// <returns></returns>
        public DataSet GetResultTimeseries(int intActiveEvalID)
        {
            string sql = "SELECT ResultTS_ID, EvaluationGroup_FK, Result_Label, VarResultType_FK, Result_Description, "
               + "ElementIndex, ElementID_FK, Element_Label, TS_StartDate, TS_StartHour, TS_StartMin, TS_Interval, TS_Interval_Unit, "
               + "BeginPeriodNo, FunctionID_FK,FunctionArgs, IsSecondary, RefTS_ID_FK "  //TODO SP 28-Feb-2017 no longer a field called IsSecondary - needs to be calculated from RetrieveCode
               + "FROM tblResultTS WHERE (((tblResultTS.EvaluationGroup_FK)= " + intActiveEvalID.ToString() + "))";

            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }
        /// <summary>
        /// Get result summary table
        /// </summary>
        /// <param name="intActiveEvalID"></param>
        /// <returns></returns>
        public DataSet GetResultSummary(int intActiveEvalID)
        {
            string sql = "SELECT tblResultVar.Result_ID, tblResultVar.Result_Label, tblResultVar.VarResultType_FK, tblResultVar.Result_Description, " +
                   "tblResultVar.Element_Label, IsListVar, ImportResultDetail  FROM tblResultVar where (EvaluationGroup_FK = " + intActiveEvalID.ToString() + ")";
            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        /// <summary>
        /// Check if the reference is editable
        /// </summary>
        /// <returns></returns>
        public bool IsEGEditable()
        {
            int intRefEval = GetReferenceEvalID();
            return (intRefEval == _nActiveEvalID);
        }
        /// <summary>
        /// Delete Event
        /// </summary>
        /// <param name="row"></param>
        public void DeleteEventResult(DataRow row)
        {
            string strID = row["EventSummaryID"].ToString();

            string strDel = "DELETE FROM  tblResultTS_EventSummary WHERE EventSummaryID = " + strID;
            _dbContext.ExecuteNonQuerySQL(strDel); // delete scenario
        }
        /// <summary>
        /// Delete DV 
        /// </summary>
        /// <param name="row"></param>
        public void DeleteDV(DataRow row)
        {
            string strDVID = row["DVD_ID"].ToString();

            string strDel = "DELETE FROM  tblDV WHERE DVD_ID = " + strDVID;
            _dbContext.ExecuteNonQuerySQL(strDel); // delete scenario
        }
        public void DeleteTimeseries(DataRow row)
        {
            string strID = row["ResultTS_ID"].ToString();

            string strDel = "DELETE FROM  tblResultTS WHERE ResultTS_ID = " + strID;
            _dbContext.ExecuteNonQuerySQL(strDel); // delete scenario
        }
        /// <summary>
        /// Delete performance
        /// </summary>
        /// <param name="row"></param>
        public void DeletePerformace(DataRow row)
        {
            //tblPerformance
            string strID = row["PerformanceID"].ToString();

            string strDel = "DELETE FROM  tblPerformance WHERE PerformanceID = " + strID;
            _dbContext.ExecuteNonQuerySQL(strDel); // delete scenario

        }
        /// <summary>
        /// Delete result summary
        /// </summary>
        /// <param name="row"></param>
        public void DeleteResultSummary(DataRow row)
        {
            string strID = row["Result_ID"].ToString();

            string strDel = "DELETE FROM  tblResultVar WHERE Result_ID = " + strID;
            _dbContext.ExecuteNonQuerySQL(strDel); // delete scenario
        }
        /// <summary>
        /// Create evaluation group
        /// </summary>
        /// <param name="strLabel"></param>
        /// <param name="strDescription"></param>
        /// <param name="intProjectId"></param>
        public void CreateEvaluationGroup(string strLabel, string strDescription, string strReferenceEvalID_FK, int intProjectId, int intModelType_ID)
        {
            string strSQL = "INSERT INTO tblEvaluationGroup (EvaluationLabel, EvaluationDescription, DateCreated, ProjID_FK, ReferenceEvalID_FK, ModelType_ID) VALUES ('{0}', '{1}', '{2}', {3}, {4}, {5})";
            string strNewSQL = string.Format(strSQL, strLabel, strDescription, DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss"), intProjectId, strReferenceEvalID_FK, intModelType_ID);
            _dbContext.ExecuteNonQuerySQL(strNewSQL);
        }

        /// <summary>
        /// Delete the selected scenario
        /// </summary>
        /// <param name="intEvalID"></param>
        /// <param name="row"></param>
        public void DeleteScenario(int intEvalID, DataRow row)
        {
            string strScenarioID = row["ScenarioID"].ToString();
            int intScenStart = (row["ScenStart"] == DBNull.Value ? 0 : int.Parse(row["ScenStart"].ToString()));
            int intScenEnd = (row["ScenEnd"] == DBNull.Value ? 0 : int.Parse(row["ScenEnd"].ToString()));

            string strDel = "DELETE * FROM  tblScenario WHERE ScenarioID = " + strScenarioID;
            _dbContext.ExecuteNonQuerySQL(strDel); // delete scenario

            // delete data
            DeleteScenarioData(intEvalID, intScenStart, intScenEnd,"");                  //modified met 5/9/16 for increased flexibility...
        }
        //public void DeleteModelData(int nEvalID, bool bUseSpecialOps)
        //{
        //    string sSQL = "delete from tblModElementVals where ";
        //    string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
        //    if (bUseSpecialOps)
        //    {        //join to this table
        //        sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
        //                    + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
        //    }
        //    sSQL = sSQL + sSubquery;
        //    _dbContext.RunDeleteSQL(sSQL);
        //}
        //public void DeleteResultSummaryData(int nEvalID, bool bUseSpecialOps)
        //{
        //    string sSQL = "delete from tblResultVar_Details where ";                //(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
        //    string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
        //    if (bUseSpecialOps)
        //    {        //join to this table
        //        sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
        //                    + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
        //    }
        //    sSQL = sSQL + sSubquery;


        //    _dbContext.RunDeleteSQL(sSQL);
        //}

        /*SP 5-Aug-2016 Moved this back to Simlink.cs class
        public void DeleteEventData(int nEvalID, bool bUseSpecialOps)
        {
            string sSQL = "delete from tblResultTS_EventSummary_Detail where ";                //(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            if (bUseSpecialOps)
            {        //join to this table
                sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
            }
            sSQL = sSQL + sSubquery;


            _dbContext.RunDeleteSQL(sSQL);
        }


        public void DeletePerformanceData(int nEvalID, bool bUseSpecialOps)
        {
            try
            {
                string sSQL = "delete from tblPerformance_Detail where ";       //(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
                string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
                if (bUseSpecialOps)
                {        //join to this table
                    sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                                + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
                }
                sSQL = sSQL + sSubquery;

                _dbContext.RunDeleteSQL(sSQL);
            }
            catch (Exception ex)
            {
                // metSwitchCommons Commons.ShowMessage("Error in deleting performance data '" + ex.Source + ": " + ex.Message + "'");
            }
        }*/


        public void CreateNewOption(int intProjectId, string strName)
        {
            string strSQL = "INSERT INTO tblOptionLists(ProjID_FK, OptionLabel) VALUES ({0}, '{1}')";
            string strNewSQL = string.Format(strSQL, intProjectId, strName);
            _dbContext.ExecuteNonQuerySQL(strNewSQL);
        }
        public void CreateNewEelement(int intProjectId, string strName, string strType)
        {
            string strSQL = "INSERT INTO tblElementLists(ProjID_FK, ElementListLabel, Type) " +
                " VALUES ({0}, '{1}', '{2}')";
            string strNewSQL = string.Format(strSQL, intProjectId, strName, strType);
            _dbContext.ExecuteNonQuerySQL(strNewSQL);
        }
        /// <summary>
        /// Check if the name already exists
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="blnIsOption"></param>
        /// <returns></returns>
        public bool CheckOptionElementNameExists(string strName, bool blnIsOption)
        {
            string strSQL = "";
            if (blnIsOption)
            {
                strSQL = "SELECT count(*) FROM tblOptionLists WHERE OptionLabel ='" + strName + "'";
            }
            else
            {
                strSQL = "SELECT count(*) FROM tblElementLists WHERE ElementListLabel ='" + strName + "'";
            }
            int intTotal = (int)_dbContext.ExecuteScalarSQL(strSQL);
            return (intTotal > 0);

        }
    }
}
