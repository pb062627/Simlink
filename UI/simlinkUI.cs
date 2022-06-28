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
using CH2M;

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
            int nEvalID = CreateEG(nModelTypeID, sModelFileLocation, sEG_Label, nProjID, -1, "");

        }

        private int InsertProject(int nModelTypeID, string sProjLabel,  string sProjDescription)
        {
            string sSQL = "SELECT tblProj.ProjID, tblProj.ProjLabel, tblProj.ModelType_ID, tblProj.DateCreated, tblProj.UserID_FK, tblProj.ModelDescription"
                            + " FROM tblProj"
                            +" WHERE (0>1)";                //get empty DS

            DataSet ds = _dbContext.getDataSetfromSQL(sSQL);
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[0]["ProjLabel"] = sProjLabel;
            ds.Tables[0].Rows[0]["ModelDescription"] = sProjDescription;
            ds.Tables[0].Rows[0]["ModelType_ID"] = nModelTypeID;
            ds.Tables[0].Rows[0]["UserID_FK"] = -1;                                         //not used yet
            ds.Tables[0].Rows[0]["DateCreated"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            string sPK_SQL = "SKIP";
            if (true)             
            {
                string sWhere = "(ProjLabel = '" + sProjLabel + "')";
                sPK_SQL = SIM_API_LINKS.DAL.DBContext.GetQuerySQL_NewPK_AferInsert("tblProj", "ProjID", sWhere);          //todo: figure out a better way to get the PK- Access, can't do any better. SQL Server- can!
            }
            int nReturnID = _dbContext.InsertOrUpdateDBByDataset(true, ds, sSQL, true, true, sPK_SQL);
            return Convert.ToInt32(nReturnID);
        }

        public int CreateEG(int nModelTypeID, string sModelFileLocation, string sEG_Label, int nProjID_FK, int intReferenceEvalID_FK, string strEvaluationDescription)
        {
            DataSet dsEG = new DataSet();           // pass by reference, so we have available for an update of baseline scenario ID
            string sSQL = "SELECT tblEvaluationGroup.EvaluationID, tblEvaluationGroup.ProjID_FK, tblEvaluationGroup.EvaluationLabel, tblEvaluationGroup.DateCreated, "
                            + "tblEvaluationGroup.ModelFileLocation, tblEvaluationGroup.ModelType_ID,ScenarioID_Baseline_FK, tblEvaluationGroup.ReferenceEvalID_FK, "
                            + "tblEvaluationGroup.EvaluationDescription FROM tblEvaluationGroup"
                            + " WHERE (0>1)";                //get empty DS
            int nEvalID = InsertEG(sSQL, nModelTypeID, sModelFileLocation, sEG_Label, strEvaluationDescription, nProjID_FK, intReferenceEvalID_FK, ref dsEG);
            string sScenarioLabel= "Baseline: " + nEvalID;
            int nScenarioID = InsertScenario(nEvalID, nProjID_FK, sScenarioLabel, sScenarioLabel);
            dsEG.Tables[0].Rows[0]["ScenarioID_Baseline_FK"] = nScenarioID;
            string strSQL = "UPDATE tblEvaluationGroup SET ScenarioID_Baseline_FK = " + nScenarioID.ToString() + " WHERE EvaluationID=" + nEvalID.ToString();
            _dbContext.ExecuteNonQuerySQL(strSQL);
            //_dbContext.InsertOrUpdateDBByDataset(false, dsEG, sSQL, true, false);               //update the base scenario
            return nEvalID;
        }
        private int InsertEG(string sSQL, int nModelTypeID, string sModelFileLocation, string sEG_Label, string strEvaluationDescription, int nProjID_FK, int intReferenceEvalID_FK, ref DataSet ds)
        {
            ds = _dbContext.getDataSetfromSQL(sSQL);
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[0]["ProjID_FK"] = nProjID_FK;
            ds.Tables[0].Rows[0]["ModelFileLocation"] = sModelFileLocation;
            ds.Tables[0].Rows[0]["EvaluationLabel"] = sEG_Label;
            ds.Tables[0].Rows[0]["ModelType_ID"] = nModelTypeID;
            ds.Tables[0].Rows[0]["ReferenceEvalID_FK"] = intReferenceEvalID_FK;  //not used yet
            ds.Tables[0].Rows[0]["EvaluationDescription"] = strEvaluationDescription;
            ds.Tables[0].Rows[0]["DateCreated"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                
            string sPK_SQL = "SKIP";
            if (true)
            {
                string sWhere = "((EvaluationLabel = '" + sEG_Label + "') and  (ProjID_FK = " + nProjID_FK + ")) ";
                sPK_SQL = SIM_API_LINKS.DAL.DBContext.GetQuerySQL_NewPK_AferInsert("tblEvaluationGroup", "EvaluationID", sWhere);          //todo: figure out a better way to get the PK- Access, can't do any better. SQL Server- can!
            }
            int nReturnID = _dbContext.InsertOrUpdateDBByDataset(true, ds, sSQL, true, true, sPK_SQL);
            return Convert.ToInt32(nReturnID);
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
                    + "tblProj.ModelType_ID, tblProj.ModelDescription, tblProj.ModelTargetArea, "
                    + "tblProj.DateCreated, tblProj.LastModified, tlkpUI_Dictionary.val "
                    + "FROM tblProj INNER JOIN tlkpUI_Dictionary ON tblProj.ModelType_ID = tlkpUI_Dictionary.DictID "
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
        public DataSet LoadEventResult(int intEvaluationGroup_FK)
        {
            string strSQL = "SELECT EventSummaryID, tblResultTS.Result_Label As Label, CategoryID_FK, EventFunctionID, "
                        + "Threshold_Inst, IsOver_Threshold_Inst, Threshold_Cumulative, "
                        + "IsOver_Threshold_Cumulative, InterEvent_Threshold FROM tblResultTS INNER JOIN tblResultTS_EventSummary ON tblResultTS.ResultTS_ID = tblResultTS_EventSummary.ResultTS_or_Event_ID_FK "
                        + "WHERE tblResultTS.EvaluationGroup_FK = " + intEvaluationGroup_FK.ToString();
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
                + "LinkTableCode, PF_FunctionType, EvalID_FK, FunctionID_FK, IsObjective, SQN, "
                + "FunctionArgs, DV_ID_FK, OptionID_FK FROM tblPerformance where (EvalID_FK = " + intEvalID.ToString() + ")";
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
                + "FROM tblProj INNER JOIN tlkpUI_Dictionary ON tblProj.ModelType_ID = tlkpUI_Dictionary.DictID "
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
                pro.ProjectNote = (myReader.GetValue(6) == DBNull.Value ? "" : myReader.GetString(4));
                pro.DateCreated = (myReader.GetValue(6) == DBNull.Value ? "" : myReader.GetString(5));
                pro.LastModifiedDate = (myReader.GetValue(6) == DBNull.Value ? "" : myReader.GetString(6));

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

                    strSQL = "INSERT INTO tblDV(DV_Label, EvaluationGroup_FK, VarType_FK, "
                            + "Option_FK, Option_MIN, Option_MAX, ElementID_FK, Element_Label, sqn, PrimaryDV_ID_FK, SecondaryDV_Key, XModelID_FK, PrimaryDV_ID_FK) VALUES ('{0}', {1}, {2}, {3}, "
                            + "{4}, {5}, {6}, '{7}', {8}, {9}, {10}, {11}, {12})";
                }
                else
                {
                    strSQL = "UPDATE tblDV SET DV_Label='{0}', " +
                             "EvaluationGroup_FK = {1}, " +
                             "VarType_FK = {2}, " +
                             "Option_FK = {3}, " +
                             "Option_MIN = {4}, " +
                             "Option_MAX = {5}, " +
                             "ElementID_FK={6}, " +
                             "Element_Label='{7}', " +
                             "sqn ={8}, " +
                             "SecondaryDV_Key ={9}, " +
                             "XModelID_FK={10}, " +
                             "PrimaryDV_ID_FK = {11} " +
                             "WHERE DVD_ID = {12}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string DV_ID = ParseDataRow(row, "DVD_ID");
                    string DV_Label = ParseDataRow(row, "DV_Label");
                    string EvaluationGroup_FK = intEvalationGroupID.ToString(); // ParseDataRow(row, "EvaluationGroupID");
                    string VarType_FK = ParseDataRow(row, "VarType_FK");
                    //string DV_Type = ParseDataRow(row, "DV_Type");
                    string Option_FK = ParseDataRow(row, "OptionID");
                    string Option_MIN = ParseDataRow(row, "Option_MIN");
                    string Option_MAX = ParseDataRow(row, "Option_MAX");
                    string ElementID_FK = ParseDataRow(row, "ElementID");
                    string Element_Label = ParseDataRow(row, "ElementName");
                    string PrimaryDV_ID_FK = ParseDataRow(row, "PrimaryDV_ID_FK");
                    string sqn = ParseDataRow(row, "sqn");

                    string strNewSQL = string.Format(strSQL, DV_Label, EvaluationGroup_FK, VarType_FK,
                        Option_FK, Option_MIN, Option_MAX, ElementID_FK, Element_Label, sqn, -1, -1, -1, PrimaryDV_ID_FK, DV_ID);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch(Exception ex)
            {
                Commons.ShowMessage("Error in adding/updating DV table '" + ex.Source + ": " + ex.Message +"'");
            }
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

                    strSQL = "INSERT INTO tblResultTS(Result_Label, VarResultType_FK, "
                            + "Result_Description, ElementID_FK, ElementIndex, Element_Label, EvaluationGroup_FK, SQN, FunctionID_FK ) VALUES ('{1}', {2}, '{3}', "
                            + "{4}, '{5}', '{6}', {7}, {8}, {9})";
                }
                else
                {
                    strSQL = "UPDATE tblResultTS SET Result_Label='{1}', " +
                             "VarResultType_FK = {2}, " +
                             "Result_Description = '{3}', " +
                             "ElementID_FK = {4}, " +
                             "ElementIndex = '{5}', " +
                             "Element_Label = '{6}', " +
                             "EvaluationGroup_FK = {7} " +
                             "WHERE ResultTS_ID = {0}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string Result_ID = ParseDataRow(row, "ResultTS_ID");
                    string Result_Label = ParseDataRow(row, "Result_Label");
                    string EvaluationGroup_FK = intEvalationGroupID.ToString(); // ParseDataRow(row, "EvaluationGroupID");
                    string VarResultType_FK = ParseDataRow(row, "VarResultType_FK");
                    //string DV_Type = ParseDataRow(row, "DV_Type");
                    string Result_Description = ParseDataRow(row, "Result_Description");
                    string ElementID_FK = ParseDataRow(row, "ElementID");
                    string ElementIndex = ParseDataRow(row, "ElementIndex");
                    string Element_Label = ParseDataRow(row, "Element_Label");

                    string strNewSQL = string.Format(strSQL, Result_ID, Result_Label, VarResultType_FK,
                        Result_Description, ElementID_FK, ElementIndex, Element_Label, EvaluationGroup_FK, -1, -1);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error in adding/updating timeseries table '" + ex.Source + ": " + ex.Message + "'");
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
                            + "FunctionID_FK, IsObjective, SQN, FunctionArgs, DV_ID_FK, OptionID_FK, EvalID_FK) "
                            + "VALUES ('{0}', {1}, {2}, {3}, {4}, {5}, {6}, {7}, '{8}', {9}, {10}, {11})";
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
                            "IsObjective={6}, " +
                            "SQN={7}, " +
                            "FunctionArgs='{8}', " +
                            "DV_ID_FK={9}, " +
                            "OptionID_FK={10}, " +
                            "EvalID_FK={11} " +
                            "WHERE PerformanceID = {12}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string Performance_Label = row["Performance_Label"].ToString();
                    string PF_Type = row["PF_Type"].ToString();
                    string CategoryID_FK = row["CategoryID_FK"].ToString();
                    string LinkTableCode = row["LinkTableCode"].ToString();
                    string PF_FunctionType = row["PF_FunctionType"].ToString();
                    string FunctionID_FK = row["FunctionID_FK"].ToString();
                    string IsObjective = row["IsObjective"].ToString();
                    string SQN = row["SQN"].ToString();
                    string FunctionArgs = row["FunctionArgs"].ToString();
                    string DV_ID_FK = row["DV_ID_FK"].ToString();
                    string OptionID_FK = row["OptionID_FK"].ToString();
                    string EvalID_FK = intEvalID.ToString();

                    // performance
                    int intPerformanceID = (row["PerformanceID"] == DBNull.Value ? 0 : int.Parse(row["PerformanceID"].ToString()));

                    // run the query
                    string strNewSQL = string.Format(strSQL, Performance_Label, PF_Type, CategoryID_FK, LinkTableCode,
                        PF_FunctionType, FunctionID_FK, IsObjective, SQN, FunctionArgs, DV_ID_FK, OptionID_FK, EvalID_FK, intPerformanceID);
                    _dbContext.ExecuteNonQuerySQL(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error in adding/updating performance table '" + ex.Source + ": " + ex.Message + "'");
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
            string strSQL = "DELETE * FROM tblElementListDetails WHERE ElementListDetailID = " + strElementID;
            _dbContext.RunDeleteSQL(strSQL);
        }
        /// <summary>
        /// Delete option list
        /// </summary>
        /// <param name="row"></param>
        public void DeleteOptionList(DataRow row)
        {
            string strOptionID = row["OptionID"].ToString();
            string strSQL = "DELETE * FROM tblOptionDetails WHERE OptionID = " + strOptionID;
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
                    strSQL = "INSERT INTO tblOptionDetails(OptionNo, Val, valLabelinSCEN, VarID_FK, OptionID_FK) VALUES ({0}, '{1}', '{2}', '{3}',{4})";
                }
                else
                {
                    strSQL = "UPDATE tblOptionDetails SET OptionNo={0}, " +
                            "Val='{1}', " +
                            "valLabelinSCEN='{2}', " +
                            "VarID_FK='{3}' " +
                            "WHERE OptionID={4}";
                }
                foreach (DataRow row in dt.Rows)
                {
                    int intOptionDetailKeyId = (row["OptionDetailKeyId"] == DBNull.Value ? 0 : int.Parse(row["OptionDetailKeyId"].ToString()));
                    int intOptionId = int.Parse(row["OptionID"].ToString());
                    int intOptionNo = int.Parse(row["OptionNo"].ToString());
                    string strVAL = row["val"].ToString();
                    int intOptionKey = (blnInsertNew ? intOptionId : intOptionDetailKeyId);
                    string strNewSQL = string.Format(strSQL, intOptionNo, strVAL, "", -1, intOptionKey);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error in adding/updating option list '" + ex.Source + ": " + ex.Message + "'");
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
                    strSQL = "INSERT INTO tblElementListDetails(ElementListID_FK, val, ElementID_FK, VarLabel) VALUES ({0}, '{1}', {2}, '{3}')";
                }
                else
                {
                    strSQL = "UPDATE tblElementListDetails SET ElementListID_FK={0}, " +
                            "val='{1}', " +
                            "ElementID_FK={2}, " +
                            "VarLabel='{3}' " +
                            "WHERE ElementListDetailID={4}";
                }
                foreach (DataRow row in dt.Rows)
                {
                    int intElementListDetailID = (row["ElementListDetailID"] != DBNull.Value ? int.Parse(row["ElementListDetailID"].ToString()) : -1);
                    int intElementListID_FK = int.Parse(row["ElementListID_FK"].ToString());
                    int intElementID_FK = (row["ElementID_FK"] != DBNull.Value ? int.Parse(row["ElementID_FK"].ToString()) : -1);
                    string strVarLabel = row["VarLabel"].ToString();
                    string strNewSQL = string.Format(strSQL, intElementListID_FK, "", intElementID_FK, strVarLabel, intElementListDetailID);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error in adding/updating element list '" + ex.Source + ": " + ex.Message + "'");
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
                            + "Result_Description, ElementID_FK, Element_Label) VALUES ('{0}', {1}, {2}, '{3}', "
                            + "{4}, '{5}')";
                }
                else
                {
                    strSQL = "UPDATE tblResultVar SET Result_Label='{0}', " +
                             "EvaluationGroup_FK = {1}, " +
                             "VarResultType_FK = {2}, " +
                             "Result_Description = '{3}', " +
                             "ElementID_FK = {4}, " +
                             "Element_Label = '{5}' " +
                             "WHERE Result_ID = {6}";
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string Result_ID = ParseDataRow(row, "Result_ID");
                    string Result_Label = ParseDataRow(row, "Result_Label");
                    string EvaluationGroup_FK = intEvalationGroupID.ToString(); // ParseDataRow(row, "EvaluationGroupID");
                    string VarResultType_FK = ParseDataRow(row, "VarResultType_FK");
                    //string DV_Type = ParseDataRow(row, "DV_Type");
                    string Result_Description = ParseDataRow(row, "Result_Description");
                    string ElementID_FK = ParseDataRow(row, "ElementID");
                    string Element_Label = ParseDataRow(row, "Element_Label");

                    string strNewSQL = string.Format(strSQL, Result_Label, EvaluationGroup_FK, VarResultType_FK,
                        Result_Description, ElementID_FK, Element_Label, Result_ID);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error in adding/updating summary result table '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        /// <summary>
        /// Insert/update the scenario
        /// </summary>
        /// <param name="dsUpdate"></param>
        /// <param name="blnAddNew"></param>
        internal void InsertOrUpdateScenarioTable(DataSet ds, bool blnInsertNew, string strProjectId, string strEVGroupId)
        {
            try
            {
                if (ds == null) return;
                string strSQL = "";
                if (blnInsertNew)
                {

                    strSQL = "INSERT INTO tblScenario(ProjID_FK, EvalGroupID_FK, ScenarioLabel, DateEvaluated, HasBeenRun, "
                            + "DNA, ScenStart, ScenEnd) VALUES ({0}, {1}, '{2}', '{3}', "
                            + "{4}, '{5}', {6}, {7})";
                }
                else
                {
                    strSQL = "UPDATE tblScenario SET ProjID_FK={0}, " +
                             "EvalGroupID_FK = {1}, " +
                             "ScenarioLabel = '{2}', " +
                             "DateEvaluated = '{3}', " +
                             "HasBeenRun = {4}, " +
                             "DNA = '{5}', " +
                             "ScenStart = {6}, " +
                             "ScenEnd = {7} " +
                             "WHERE ScenarioID = {8}";
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

                    string strNewSQL = string.Format(strSQL, strProjectId, strEVGroupId, ScenarioLabel,
                        DateEvaluated, HasBeenRun, DNA, ScenStart, ScenEnd, ScenarioID);
                    _dbContext.InsertOrupdatebySQLString(strNewSQL);
                }
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error in adding/updating scenario '" + ex.Source + ": " + ex.Message + "'");
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
            string strSQL = "SELECT ScenarioID, ProjID_FK, EvalGroupID_FK, ScenarioLabel, DateEvaluated, HasBeenRun, "
                + "DNA, ScenStart, ScenEnd FROM tblScenario WHERE ProjID_FK=" + strProjectID + " AND EvalGroupID_FK =" + strActiveEVID;
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
            string strShortenedFile = Commons.GetShortPathName(strCSVFile);
            string strFolder = new FileInfo(strShortenedFile).DirectoryName;
            string strCSV = new FileInfo(strShortenedFile).Name;
            string strSQL = "INSERT INTO tblScenario(ProjID_FK, EvalGroupID_FK, ScenarioLabel, DNA, HasBeenRun, ScenStart, ScenEnd, DateEvaluated) " +
                            "SELECT " + strProjectID + "," + strEvalID + ", ScenarioLabel, DNA, HasBeenRun, ScenStart, ScenEnd, DateEvaluated FROM [Text;FMT=TabDelimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
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

                string strShortenedFile = Commons.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;
                string strSQL = "INSERT INTO tblResultVar(EvaluationGroup_FK, Result_Label, VarResultType_FK, Result_Description, ElementID_FK, Element_Label) " +
                                "SELECT " + strEvalID + ", " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                return true;
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error importing csv file '" + ex.Source + ": " + ex.Message + "'");
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

                string strShortenedFile = Commons.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;
                string strSQL = "INSERT INTO tblPerformance(EvalID_FK, Performance_Label, PF_Type, LinkTableCode, Description, PF_FunctionType, FunctionID_FK, FunctionArgs, DV_ID_FK, OptionID_FK) " +
                                "SELECT " + strEvalID_FK + " AS [EvalID_FK], " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                return true;
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
            }
        }
        /// <summary>
        /// Import the event result from csv
        /// </summary>
        /// <param name="astrFieldAlias"></param>
        /// <param name="strCSVFile"></param>
        /// <returns></returns>
        public bool ImporEventResultFromCSV(string[] astrFieldAlias, string strCSVFile)
        {
            try
            {

                string strFieldList = GetCSVAliasFieldList(strCSVFile, astrFieldAlias);

                string strShortenedFile = Commons.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;
                string strSQL = "INSERT INTO tblResultTS_EventSummary(ResultTS_or_Event_ID_FK, EventFunctionID, Threshold_Inst, IsOver_Threshold_Inst, Description) " +
                                "SELECT " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                return true;
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
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

                string strShortenedFile = Commons.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;
                string strSQL = "INSERT INTO tblPerformance(Performance_Label, PF_Type, LinkTableCode, Description, PF_FunctionType, FunctionID_FK, FunctionArgs, DV_ID_FK, OptionID_FK) " +
                                "SELECT " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                return true;
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error importing csv file '" + ex.Source + ": " + ex.Message + "'");
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
                string strShortenedFile = Commons.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;

                string strSQL = "INSERT INTO tblDV(EvaluationGroup_FK, XModelID_FK, DV_Label, VarType_FK,DV_Description,DV_Type,Option_FK," +
                    "Option_MIN,Option_MAX,GetNewValMethod,FunctionID_FK,FunctionArgs,ElementID_FK,sqn,Operation_DV," +
                    "SecondaryDV_Key,PrimaryDV_ID_FK,IsSpecialCase) " +
                    "SELECT " + strEvalRefId + " AS EvaluationGroup_FK, -1 AS XModelID_FK, " + strFieldList + " FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql

                strSQL = "UPDATE tblDV SET PrimaryDV_ID_FK=-1 WHERE PrimaryDV_ID_FK IS NULL";
                _dbContext.ExecuteNonQuerySQL(strSQL); // set the default value
                return true;
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error in importing csv file '" + ex.Source + ": " + ex.Message + "'");
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
                string strShortenedFile = Commons.GetShortPathName(strCSVFile);
                string strFolder = new FileInfo(strShortenedFile).DirectoryName;
                string strCSV = new FileInfo(strShortenedFile).Name;

                string strSQL = "INSERT INTO tblResultTS(EvaluationGroup_FK,Result_Label, VarResultType_FK, Result_Description, ElementID_FK, Element_Label, SQN, FunctionID_FK) " +
                    "SELECT " + strEvalId + " As EvaluationGroup_FK, " + strFieldList + ", 1 AS SQN, -1 As FunctionID_FK FROM [Text;FMT=Delimited;HDR=YES;DATABASE=" + strFolder + "].[" + strCSV + "]";
                _dbContext.ExecuteNonQuerySQL(strSQL); // execute import from sql
                return true;
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error in importing csv file '" + ex.Source + ": " + ex.Message + "'");
                return false;
            }
        }
        /// <summary>
        /// Get result var type look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultVarTypeLookup()
        {
            string strSQL = "SELECT tlkpSWMMResults_FieldDictionary.ResultsFieldID, tlkpSWMMResults_TableDictionary.TableName, tlkpSWMMResults_FieldDictionary.FieldName " +
                        "FROM tlkpSWMMResults_FieldDictionary INNER JOIN tlkpSWMMResults_TableDictionary ON tlkpSWMMResults_FieldDictionary.TableID_FK = tlkpSWMMResults_TableDictionary.ResultTableID WHERE (((tlkpSWMMResults_FieldDictionary.IsOutFileVar)=False))";
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
            string strSQL = "SELECT tlkpSWMMResults_FieldDictionary.ResultsFieldID, "
                        + "tlkpSWMMResults_FieldDictionary.FeatureType, tlkpSWMMResults_FieldDictionary.FieldName "
                        + "FROM tlkpSWMMResults_FieldDictionary WHERE (((tlkpSWMMResults_FieldDictionary.IsOutFileVar)=True))";
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
            string strSQL = "SELECT tlkpSWMMTableDictionary.TableName, tlkpSWMMFieldDictionary.FieldName, tlkpSWMMFieldDictionary.ID AS VarType_FK "
            + "FROM tlkpSWMMFieldDictionary INNER JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID";
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
            string strSQL = "SELECT FunctionID, Label, Category, CustomFunction FROM tblFunctions WHERE ProjID_FK = " + strProjectID + " order by Label";
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
        /// Get DV lookup
        /// </summary>
        /// <returns></returns>
        public DataTable GetDVLookUp(int intEvalGroupID)
        {
            string strSQL = "SELECT DVD_ID, DV_Label FROM tblDV WHERE EvaluationGroup_FK = " + intEvalGroupID.ToString() + " order by DV_Label";
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
        /// get category look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryLookUp()
        {
            string strSQL = "SELECT CategoryID, Label, Description FROM tblCategory order by Label";
            return _dbContext.getDataSetfromSQL(strSQL).Tables[0];
        }
        public DataTable GetLinkTableCodeLookUp()
        {
            string[] astrVal = "Not Set,Model Elements,Result Summary,Timeseries Results,DV Option,Event".Split(',');
            string[] astrValID = "-1,1,2,3,4,5,6".Split(',');
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
        /// <summary>
        /// Operation lookup 
        /// </summary>
        /// <returns></returns>
        public DataTable GetOperationLookUp()
        {
            string[] astrVal = "Identity,Add,Subtract,Multiply,Divide".Split(',');
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
        /// Get EV Group look up
        /// </summary>
        /// <param name="strProjectID"></param>
        /// <returns></returns>
        public DataTable GetEVGroupLookUp(string strProjectID)
        {
            string strSQL = "SELECT EvaluationID, EvaluationLabel FROM tblEvaluationGroup WHERE ProjID_FK = " + strProjectID + " ORDER By EvaluationLabel";
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
            string sql = "SELECT tblResultTS.ResultTS_ID, tblResultTS.Result_Label, tblResultTS.VarResultType_FK, tblResultTS.Result_Description, "
               + "tblResultTS.ElementID_FK AS ElementID, tblResultTS.ElementIndex, tblResultTS.Element_Label, tblResultTS.EvaluationGroup_FK "
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
                   "tblResultVar.ElementID_FK As ElementID, tblResultVar.Element_Label FROM tblResultVar where (EvaluationGroup_FK = " + intActiveEvalID.ToString() + ")";
            DataSet ds = _dbContext.getDataSetfromSQL(sql);
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
        /// Delete DV 
        /// </summary>
        /// <param name="row"></param>
        public void DeleteDV(DataRow row)
        {
            string strDVID = row["DVD_ID"].ToString();

            string strDel = "DELETE * FROM  tblDV WHERE DVD_ID = " + strDVID;
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
            DeleteScenarioData(intEvalID, intScenStart, intScenEnd);
        }
        public void DeleteModelData(int nEvalID, bool bUseSpecialOps)
        {
            string sSQL = "delete from tblModElementVals where ";
            string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            if (bUseSpecialOps)
            {        //join to this table
                sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
            }
            sSQL = sSQL + sSubquery;
            _dbContext.RunDeleteSQL(sSQL);
        }
        public void DeleteResultSummaryData(int nEvalID, bool bUseSpecialOps)
        {
            string sSQL = "delete from tblResultVar_Details where ";                //(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            string sSubquery = "(ScenarioID_FK in (Select scenarioID from tblScenario where (EvalGroupID_FK = " + nEvalID + ")))";
            if (bUseSpecialOps)
            {        //join to this table
                sSubquery = "(ScenarioID_FK in (SELECT tblScenario.ScenarioID FROM tblScenario_SpecialOps INNER JOIN tblScenario ON tblScenario_SpecialOps.ScenarioID = tblScenario.ScenarioID"
                            + " WHERE (((tblScenario.EvalGroupID_FK)=" + nEvalID + "))))";
            }
            sSQL = sSQL + sSubquery;


            _dbContext.RunDeleteSQL(sSQL);
        }

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
                Commons.ShowMessage("Error in deleting performance data '" + ex.Source + ": " + ex.Message + "'");
            }
        }
        public void CreateNewOption(int intProjectId, string strName, string strQual1, string strQual2, string strOperation, int intVarTypeId)
        {
            string strSQL = "INSERT INTO tblOptionLists(ProjID_FK, OptionLabel, Qual1, Qual2, " +
                "Operation, VarType_FK, IsScaleValue, VarType_ScaleBy) VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7})";
            string strNewSQL = string.Format(strSQL, intProjectId, strName, strQual1, strQual2, strOperation, intVarTypeId, 0, -1);
            _dbContext.ExecuteNonQuerySQL(strNewSQL);
        }
        public void CreateNewEelement(int intProjectId, string strName, int intTableId, string strType)
        {
            string strSQL = "INSERT INTO tblElementLists(ProjID_FK, ElementListLabel, TableID_FK, Type) " +
                " VALUES ({0}, '{1}', {2}, '{3}')";
            string strNewSQL = string.Format(strSQL, intProjectId, strName, intTableId, strType);
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
