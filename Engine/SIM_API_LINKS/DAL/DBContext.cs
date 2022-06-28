using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;

namespace SIM_API_LINKS.DAL
{
    public enum DataTypeSL
        {
            INTEGER,
            LONG,
            STRING,
            DOUBLE,
            DATETIME
        };

    public enum DB_Type
    {
        OLEDB = 0,
        SQL_SERVER = 1,
        EXCEL = 2,
        NONE = 3                                        //SimLink_LITE uses no backend
    }
    
    public class DBContext_Parameter
    {
        public string _sParameterName;
        public DataTypeSL _sType;               //type must be generic to work with SimLink classes
        public object _val;

#region enum
        
#endregion


        //example usage:
                //    List<DBContext_Parameter> lstParam = new List<DBContext_Parameter>();
                //    DBContext_Parameter param = new DBContext_Parameter("@EvalID", SIM_API_LINKS.DAL.DataTypeSL.INTEGER, nEvalID);
                //    lstParam.Add(param);
        public DBContext_Parameter(string sParamName, DataTypeSL sType, object val)
        {
            _sParameterName = sParamName;
            _sType = sType;
            _val = val;
        }
    }

   
    public class DBContext
    {


            public DB_Type _DBContext_DBTYPE = DB_Type.OLEDB;        //1 = MS ACCESS   2 SQL SERVER
            public string _sConnectionString = "";
            private OleDbConnection _connOleDB;
            private SqlConnection _connSS;
            public Oledb_Maintenance _oledbMaint = new Oledb_Maintenance();         // used to help manage access file size


        /// <summary>
        /// Read CSV into a datatable
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isFirstRowHeader"></param>
        /// <returns></returns>
            public static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
            {
                string header = isFirstRowHeader ? "Yes" : "No";

                string pathOnly = Path.GetDirectoryName(path);
                string fileName = Path.GetFileName(path);

                string sql = @"SELECT * FROM [" + fileName + "]";

                using (OleDbConnection connection = new OleDbConnection(
                          @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                          ";Extended Properties=\"Text;HDR=" + header + "\""))
                using (OleDbCommand command = new OleDbCommand(sql, connection))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Locale = CultureInfo.CurrentCulture;
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }


            #region OledbMaint

            public bool RectifyTestMeetsLimit()
            {
                string sFilename = GetFilenameFromConnectionString();
                long lCurrentSize = _oledbMaint.GetFileSize(sFilename);
                return lCurrentSize >= _oledbMaint._dCompactIfExceed * Oledb_Maintenance._lAccessFileLimit;
            }

        /// <summary>
        /// compacts and repairs the access file if certain conditions are met.
        /// Returns true if cr operation performed.
        /// </summary>
        /// <returns></returns>
            public bool RectifySizeLimitation(out bool bIsProblem)
            {
                bIsProblem = false;
                if (_DBContext_DBTYPE == DB_Type.OLEDB && _oledbMaint._dCompactIfExceed != -1)
                {
                    bool bReturn = false;
                    string sFilename = GetFilenameFromConnectionString();
                    long lCurrentSize = _oledbMaint.GetFileSize(sFilename);
                    string sFileDest = Path.Combine(Path.GetDirectoryName(sFilename), "1_" + Path.GetFileName(sFilename));
                    bool bIsOpen = _connOleDB.State == ConnectionState.Open;
                    if (lCurrentSize >= _oledbMaint._dCompactIfExceed * Oledb_Maintenance._lAccessFileLimit)
                    {
                        _connOleDB.Close();
                        Console.WriteLine("Compacting and repairing database: " + DateTime.Now);
                        _oledbMaint.CompactDb(sFilename, sFileDest);
                        Console.WriteLine("DB successfully compacted: " + DateTime.Now);
                        bReturn = true;
                        File.Delete(sFilename);
                        File.Move(sFileDest, sFilename);            //now move the file back.
                        if (bIsOpen)
                            _connOleDB.Open();              //if the file was open, close it.

                        // IMPORTANT- need to check that hte current file is small enough you can use it; otherwise you will just copy paste forever.
                        lCurrentSize = _oledbMaint.GetFileSize(sFilename);
                        if (lCurrentSize >= _oledbMaint._dCompactIfExceed * Oledb_Maintenance._lAccessFileLimit)
                            bIsProblem = true;
                    }

                    return bReturn;
                }
                else
                {
                    return false;
                }
            }

            public string GetFilenameFromConnectionString()
            {
                string sReturn = "UNDEFINED";
                string[] sParts = _sConnectionString.Split(';');                    // = "";
                foreach (string sVal in sParts)
                {
                    if (sVal.Length > 11)
                    {
                        if (sVal.Substring(0, 11).ToUpper() == "DATA SOURCE")
                        {
                            string[] sDB = sVal.Split('=');
                            sReturn = sDB[1].Trim();
                        }
                    }

                }
                return sReturn;            
            }
            #endregion

        /// <summary>
        /// Return whether an integer means 'true' for a given db context
        /// </summary>
        /// <param name="nBoolAsInt"></param>
        /// <returns></returns>
            public bool IsTrue(int nBoolAsInt){
                bool bReturn = false;
                int nTrueBit = GetTrueBitByContext();
                return nBoolAsInt == nTrueBit;
            }

            public int GetDBTypeAsInt()
            {
                return (_DBContext_DBTYPE == DB_Type.SQL_SERVER ? 1 : 0);
            }
            /// <summary>
            /// Get the current db connection
            /// </summary>
            public IDbConnection CurrentDBConnection
            {
                get { return (_DBContext_DBTYPE == DB_Type.SQL_SERVER ? (IDbConnection)_connSS : (IDbConnection)_connOleDB); }
            }
            /// <summary>
            /// Current data adaptor
            /// </summary>
            public IDbDataAdapter CurrentDataAdaptor
            {
                get { return (_DBContext_DBTYPE == DB_Type.SQL_SERVER ? (IDbDataAdapter)new SqlDataAdapter() : (IDbDataAdapter)new OleDbDataAdapter()); }
            }
            /// <summary>
            /// Get the current command
            /// </summary>
            public IDbCommand CurrentCommand
            {
                get { return (_DBContext_DBTYPE == DB_Type.SQL_SERVER ? (IDbCommand)new SqlCommand() : (IDbCommand)new OleDbCommand()); }
            }
        
        #region INIT
            public bool Init(string sConn, DAL.DB_Type slDB_Type, double dSizeLimitForCompact=-1)
            {
                bool blnIsValid = true;
                try
                {
                    _sConnectionString = sConn;
                    _DBContext_DBTYPE = slDB_Type;
                    switch (_DBContext_DBTYPE)
                    {
                        case DB_Type.OLEDB:
                            _connOleDB = new OleDbConnection(sConn);
                            _connOleDB.Open();

                            if (dSizeLimitForCompact != -1)                                         // set double to check against access max file size
                            {
                                _oledbMaint._dCompactIfExceed = dSizeLimitForCompact;
                            }

                            break;
                        case DB_Type.SQL_SERVER:
                            _connSS = new SqlConnection(sConn);
                            _connSS.Open();
                            break;
                        case DB_Type.EXCEL:

                            break;
                    }
                }
                catch (Exception ex)
                {
                    blnIsValid = false;
                }
                return blnIsValid;
            }

            //close the connections
            public void Close()
            {
                switch (_DBContext_DBTYPE)
                {
                    case DB_Type.OLEDB:
                        _connOleDB.Close();
                        break;
                    case DB_Type.SQL_SERVER:
                        _connSS.Close();
                        break;
                    case DB_Type.EXCEL:

                        break;
                }
            }

            #endregion

            #region HELPER
            public static DB_Type GetDBContextType(int nType)
            {
                DB_Type dbReturn = DB_Type.OLEDB;
                switch (nType)
                {
                    case 0:
                        dbReturn = DB_Type.OLEDB;
                        break;
                    case 1:
                        dbReturn = DB_Type.SQL_SERVER;
                        break;
                    case 2:
                        dbReturn = DB_Type.EXCEL;
                        break;
                    case 3:
                        dbReturn = DB_Type.NONE;
                        break;
                }
                return dbReturn;
            }
            public static  int GetDBContextType2(DB_Type dbType)
            {
             //   int nReturn = 0;
                return Convert.ToInt32(dbType);
                
                /*switch (dbType)
                {
                    case DB_Type.OLEDB:
                        nReturn = 0;
                        break;
                    case DB_Type.SQL_SERVER:
                        nReturn = 1;
                        break;
                    case DB_Type.to:
                        nReturn = 2;
                        break;
                    case 3:
                        nReturn = 3;
                        break;
                }
                return dbReturn;*/
            }
            #endregion

            #region DB_AGNOSTIC
            public DataSet getDataSetfromSQL(string sql, List<DBContext_Parameter> lstParam = null)
            {
                DataSet dsReturn = new DataSet();
                switch(_DBContext_DBTYPE){                                                    //todo: get rid of _ndbContext and use enum only.
                    case DB_Type.OLEDB:
                        dsReturn = getDataSetfromSQL_OLEDB(sql, lstParam);
                        break;

                    case DB_Type.SQL_SERVER:
                        dsReturn = getDataSetfromSQL_SS(sql, lstParam);
                        break;

                }
                    return dsReturn;
            }
            /// <summary>
            /// Insert or update by sql query
            /// </summary>
            /// <param name="strSQL"></param>
            public void InsertOrupdatebySQLString(string strSQL)
            {
                IDbCommand cmd = CurrentCommand;
                cmd.CommandText = strSQL;
                cmd.Connection = CurrentDBConnection;
                cmd.ExecuteNonQuery();
            }
            /// <summary>
            /// Execute non-query sql
            /// </summary>
            /// <param name="sql"></param>
            public void ExecuteNonQuerySQL(string sql)               //, List<DBContext_Parameter> lstParam = null)
            {
                switch (_DBContext_DBTYPE)
                {                                                    //todo: get rid of _ndbContext and use enum only.
                    case DB_Type.OLEDB:
                        OleDbCommand cmd = new OleDbCommand(sql, _connOleDB);
                        cmd.ExecuteNonQuery();
                        break;

                    case DB_Type.SQL_SERVER:
                        SqlCommand cmdsql = new SqlCommand(sql, _connSS);
                        cmdsql.ExecuteNonQuery();
                        break;
                }
            }
            /// <summary>
            /// Execute non-query sql
            /// </summary>
            /// <param name="sql"></param>
            public object ExecuteScalarSQL(string sql)               //, List<DBContext_Parameter> lstParam = null)
            {
                switch (_DBContext_DBTYPE)
                {                                                    //todo: get rid of _ndbContext and use enum only.
                    case DB_Type.OLEDB:
                        OleDbCommand cmd = new OleDbCommand(sql, _connOleDB);
                        return cmd.ExecuteScalar();

                    case DB_Type.SQL_SERVER:
                        SqlCommand cmdsql = new SqlCommand(sql, _connSS);
                        return cmdsql.ExecuteScalar();
                    default:
                        return null;
                }
            }
            public void RunDeleteSQL(string sql)               //, List<DBContext_Parameter> lstParam = null)
            {
                DataSet dsReturn = new DataSet();
                switch (_DBContext_DBTYPE)
                {                                                    //todo: get rid of _ndbContext and use enum only.
                    case DB_Type.OLEDB:
                        RunDeleteSQL_OLEDB(sql);
                        break;

                    case DB_Type.SQL_SERVER:
                        RunDeleteSQL_SQL_Server(sql);
                        break;
                }
            }


            public bool GetLogicalByInt(int nVal)
            {
                bool bReturn = false;
                switch (_DBContext_DBTYPE)
                {
                    case DB_Type.OLEDB:
                        if (nVal == -1)
                            bReturn = true;
                        break;
                    case DB_Type.SQL_SERVER:
                        if (nVal == 1)
                            bReturn = true;
                        break;
                }
                return bReturn;
            }


        //4/10/14: updated to return the true/false bit IF you pass a val (else defaults to true)
            public int GetTrueBitByContext(bool bVal = true)
            {
                int nReturnBit = -1;
                switch (_DBContext_DBTYPE)
                {
                    case DB_Type.OLEDB:
                        nReturnBit = -1;
                        break;
                    case DB_Type.SQL_SERVER:
                        nReturnBit = 1;
                        break;
                }
                if (!bVal)
                    nReturnBit = 0;     //set to false if they pass false (defaults to true if not set)
                return nReturnBit;
              }

        //met todo
            // should dsToWrite be passed by REF (so callling function knows changes are accepted?
            // better way to get PK back for SQL SERVER
             public int InsertOrUpdateDBByDataset(bool bIsInsert, DataSet dsToWrite, string sql, bool bHasPK = true, bool bReturnPK = true, string sOLEDB_GetIDSQL ="SKIP")
            {
                int nReturnID = -1;
                DataSet dsReturn = new DataSet();
                switch (_DBContext_DBTYPE)
                {
                    case DB_Type.OLEDB:
                        //SP 15-Jun-2016 Tried retrievinng the PK using @@Identity but did not work with multithreading, too many inserts together 
                        // met changed to return this ID so we have available to calling function.
                        nReturnID = InsertOrUpdateDBByDataset_OLEDB(bIsInsert, dsToWrite, sql, bHasPK); 
                        if (bReturnPK && sOLEDB_GetIDSQL!="SKIP")
                        {
                            nReturnID = gh_GetPrimaryKeyAfterInsert_OLEDB(sOLEDB_GetIDSQL);
                        }
                        
                        break;
                    case DB_Type.SQL_SERVER:
                        nReturnID = InsertOrUpdateDBByDataset_SS(bIsInsert, dsToWrite, sql, bHasPK);
                        if (bReturnPK && sOLEDB_GetIDSQL != "SKIP")
                        {
                            nReturnID = gh_GetPrimaryKeyAfterInsert_SS(sOLEDB_GetIDSQL);
                        }
                        
                        break;
                }
                return nReturnID;
              }
            
        //not used
        /*
        public void UpdateByDataSet(DataSet dsToWrite, string sql, bool bHasPK = true)
             {
                 switch (_DBContext_DBTYPE)
                 {
                     case DB_Type.OLEDB:
                         UpdateDBByDataset_OLEDB(dsToWrite, sql);
                         break;
                     case DB_Type.SQL_SERVER:
                        // UpdateDBByDataset_SS(dsToWrite, sql);
                         break;
                 }

             }
        */


             public void OpenCloseDBConnection(bool bOnlyCloseIfOLEDB = true)
             {
                 if (( _DBContext_DBTYPE == DB_Type.OLEDB))           //always close oledb
                 {
                     OledDBCloseOpenDBConnection();
                 }
                 else if (!bOnlyCloseIfOLEDB)
                 {

                     //todo: close/open other types of database.
                 }


             }

            #endregion

             #region SQL_SERVER
             private DataSet getDataSetfromSQL_SS(string sql, List<DBContext_Parameter> lstParam = null)
             {
                 SqlDataReader dr = null;
                 try
                 {
                     SqlCommand cmd = new SqlCommand(sql, _connSS);
                     if (lstParam != null)
                        AddParametersSS(ref cmd, ref lstParam);
                     DataSet ds = new DataSet();
                     DataTable oTable = new DataTable();
                     dr = cmd.ExecuteReader();
                     ds.Tables.Add(oTable);
                     ds.Tables[0].Load(dr);
                     return ds;
                 }
                 catch (Exception ex)
                 {
                     return null; //cu.CreateExceptionTable(ex);
                 }
                 finally
                 {
                     if (dr != null) dr.Close();
                 }
             }

            private void AddParametersSS(ref SqlCommand cmd, ref List<DBContext_Parameter> lstParam)
            {
                foreach (DBContext_Parameter dbParam in lstParam)
                {
                    SqlDbType dbType = getDataTypeSS(dbParam._sType);
                    cmd.Parameters.Add(dbParam._sParameterName, dbType).Value = dbParam._val;
                }
            }

            private SqlDbType getDataTypeSS(DataTypeSL sType)
            {
                SqlDbType dbType = new SqlDbType();
                switch (sType)
                {
                    case DataTypeSL.INTEGER:
                        dbType = SqlDbType.Int;
                        break;
                    case DataTypeSL.STRING:
                        dbType = SqlDbType.Char;
                        break;
                    case DataTypeSL.DOUBLE:
                        dbType = SqlDbType.Float;           //untested
                        break;
                    case DataTypeSL.DATETIME:
                        dbType = SqlDbType.DateTime;        //untested
                        break;
                }
                return dbType;
            }

        //met 12/11/13- add ref to first arg- not in OLEDB but maybe should be?
            private int InsertOrUpdateDBByDataset_SS(bool bIsInsert, DataSet dsToWrite, string sql, bool bHasPK = true)
            {
                DataSet ds2 = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(sql, _connSS);
                da.SelectCommand = cmd;
                //  cmd.Connection = _connOleDB;                               //added due to error when tables are linked from SQL SERVER yielding error: The DataAdapter.SelectCommand.Connection property needs to be initialized
                if (bIsInsert)
                    da.InsertCommand = new SqlCommandBuilder(da).GetInsertCommand();
                else
                    da.UpdateCommand = new SqlCommandBuilder(da).GetUpdateCommand();
               
                da.Fill(ds2);

                DupDataset(ref ds2, ref dsToWrite, bHasPK);
                //ds2 = dsToWrite;        //test whether you can just overwrite (no, you cannot)
                //todo: i'm sure one could loop through the rows in the single table of dsToWrite and transfer into ds2

                da.Update(ds2);
                da.Dispose();

                int nID = -2;
                dsToWrite.Tables[0].AcceptChanges();            // indicate to DS that the changes have been accepted

                if (bHasPK && bIsInsert)             //only grab new pk on insert (not following update)
                {
                    string query2 = "Select @@Identity";
                    cmd.CommandText = query2;
                    nID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return nID;
            }

             #endregion


             #region OLEDB



        // met 1/3/2013: updated to work with both OLEDB and SS 
             public int InsertSingleRecordByDict(ref Dictionary<string, string> dictInsert, string sTableName, string sKeyFieldName, bool bGetPK = false, string sWHEREforPK = "")
            {
                string sSQL = "select * from " + sTableName + " where (FALSE)";
                int nReturn = -1;

                DataSet ds = new DataSet();
                foreach (KeyValuePair<string, string> pair in dictInsert)
                {
                    ds.Tables[0].Rows[0][pair.Key] = pair.Value;
                }
                string sPK_SQL="SKIP";
                if (bGetPK)
                {
                    sPK_SQL = GetQuerySQL_NewPK_AferInsert(sTableName, sKeyFieldName, sWHEREforPK);
                }

                nReturn = InsertOrUpdateDBByDataset(true, ds, sSQL, true, bGetPK, sPK_SQL);           //todo: may need to pass third arg ... for now pass true
                return nReturn;
            }


            public void InsertMultipleRecords(double[,] dVals, string sTableName, int nForeignKey, string[] sFieldNames)
            {
                string sSQL = "select * from " + sTableName + " where (FALSE)";
                int nReturn = -1;

                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand(sSQL, _connOleDB);
                da.SelectCommand = cmd;
                da.Fill(ds);

                for (int i = 0; i < dVals.GetLength(0); i++)
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    ds.Tables[0].Rows[i][sFieldNames[0]] = nForeignKey;
                    ds.Tables[0].Rows[i][sFieldNames[1]] = dVals[i, 0];
                    ds.Tables[0].Rows[i][sFieldNames[2]] = dVals[i, 1];
                }
                da.InsertCommand = new OleDbCommandBuilder(da).GetInsertCommand();
                da.Update(ds);
            }

            private DataSet getDataSetfromSQL_OLEDB(string sql, List<DBContext_Parameter> lstParam = null)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql, _connOleDB);
                    if (lstParam!=null)
                        AddParametersOLEDB(ref cmd, ref lstParam);
                    DataSet ds = new DataSet();
                    DataTable oTable = new DataTable();
                    OleDbDataReader dr = cmd.ExecuteReader();
                    ds.Tables.Add(oTable);
                    ds.Tables[0].Load(dr);
                    return ds;
                }
                catch (Exception ex)
                {
                    return null; //cu.CreateExceptionTable(ex);
                }
            }
        //todo: implement param list
            private void RunDeleteSQL_OLEDB(string sql)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql, _connOleDB);
               //     if (lstParam != null)
                 //       AddParametersOLEDB(ref cmd, ref lstParam);
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.DeleteCommand = cmd;
                    da.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                 //   return null; //cu.CreateExceptionTable(ex);
                }
            }

            private void RunDeleteSQL_SQL_Server(string sql)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, _connSS);
                    //     if (lstParam != null)
                    //       AddParametersOLEDB(ref cmd, ref lstParam);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.DeleteCommand = cmd;
                    da.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
               //     return null; //cu.CreateExceptionTable(ex);
                }
            }



        //add any number of parametrs to an OLEDB commands
            private void AddParametersOLEDB(ref OleDbCommand cmd, ref List<DBContext_Parameter> lstParam)
            {
                foreach (DBContext_Parameter dbParam in lstParam)
                {
                    OleDbType dbType = getDataTypeOLEDB(dbParam._sType);
                    cmd.Parameters.Add(dbParam._sParameterName, dbType).Value = dbParam._val;
                }
            }

            private void OledDBCloseOpenDBConnection()
            {
                _connOleDB.Close();
                _connOleDB.Open();
            }


        private OleDbType getDataTypeOLEDB(DataTypeSL sType){
            OleDbType dbType = new OleDbType();
            switch (sType){
                case DataTypeSL.INTEGER:
                    dbType = OleDbType.Integer;
                    break;
                case DataTypeSL.STRING:
                    dbType = OleDbType.Char;
                    break;
                case DataTypeSL.DOUBLE:
                    dbType = OleDbType.Double;
                    break;
                case DataTypeSL.DATETIME:
                    dbType = OleDbType.DBTimeStamp;
                    break;
            }
            return dbType;
        }
            
        
        //oledb version of insert.... testing out approach for segreating program logic for Data i/o
        private int InsertOrUpdateDBByDataset_OLEDB(bool bIsInsert, DataSet dsToWrite, string sql, bool bHasPK = true)
            {
                int nID = -1;
                DataSet ds2 = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter();
                using (OleDbCommand cmd = new OleDbCommand(sql, _connOleDB))
                {
                    da.SelectCommand = cmd;
                    //  cmd.Connection = _connOleDB;                               //added due to error when tables are linked from SQL SERVER yielding error: The DataAdapter.SelectCommand.Connection property needs to be initialized
                    if (bIsInsert)
                        da.InsertCommand = new OleDbCommandBuilder(da).GetInsertCommand();
                    else
                        da.UpdateCommand = new OleDbCommandBuilder(da).GetUpdateCommand();

                    da.Fill(ds2);

                    DupDataset(ref ds2, ref dsToWrite, bHasPK);
                    //ds2 = dsToWrite;        //test whether you can just overwrite (no, you cannot)
                    //todo: i'm sure one could loop through the rows in the single table of dsToWrite and transfer into ds2

                    da.Update(ds2);
                    da.Dispose();

                    dsToWrite.Tables[0].AcceptChanges();            // indicate to DS that the changes have been accepted

                    //SP 15-Jun-2016 tried retrieving PK through @@Identity but not possible with multithreading
                    //TODO kept in here for now in case we can figure out a better way to use this
                    // met question: is bHasPk what we want here? don't we want to bReturnPK ? see ss too. added for insert only, since we already have pk for update

                    if (bHasPK && bIsInsert)
                    {
                        string query2 = "Select @@Identity";
                        cmd.CommandText = query2;
                        nID = (int)cmd.ExecuteScalar();
                    }
                }

                return nID; //SP 7-Jun-2016 return the primary key after insert
            }
        

            #endregion


            #region HELPERS

        public static string GetQuerySQL_NewPK_AferInsert(string sTableName, string sKeyFieldName, string sWhereClause)
        {
            string sql = "select " + sKeyFieldName + " from " + sTableName + " where " + sWhereClause + "ORDER BY " + sKeyFieldName + " DESC";      // can return more than one - get the most recent
            return sql;
        }

        public int gh_GetPrimaryKeyAfterInsert_SS(string sSQL)                    //string sTableName, string sKeyFieldName, string sWhereClause)
        {
            //  string sql = "select " + sKeyFieldName + " from " + sTableName + " where " + sWhereClause + "ORDER BY " + sKeyFieldName + " DESC";      // can return more than one - get the most recent
            SqlCommand cmd = new SqlCommand(sSQL, _connSS);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable oTable = new DataTable();
            DataSet dsMyDs = new DataSet();
            dsMyDs.Tables.Add(oTable);
            dsMyDs.Tables[0].Load(dr);
            if (dsMyDs.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(dsMyDs.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -667;
            }
        }


        //met 10/18/2013 updated first version of this 
        public int gh_GetPrimaryKeyAfterInsert_OLEDB(string sSQL)                    //string sTableName, string sKeyFieldName, string sWhereClause)
            {
              //  string sql = "select " + sKeyFieldName + " from " + sTableName + " where " + sWhereClause + "ORDER BY " + sKeyFieldName + " DESC";      // can return more than one - get the most recent
                OleDbCommand cmd = new OleDbCommand(sSQL, _connOleDB);
                OleDbDataReader dr = cmd.ExecuteReader();
                DataTable oTable = new DataTable();
                DataSet dsMyDs = new DataSet();
                dsMyDs.Tables.Add(oTable);
                dsMyDs.Tables[0].Load(dr);
                if (dsMyDs.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(dsMyDs.Tables[0].Rows[0][0].ToString());
                }
                else
                {
                    return -667;
                }
            }

        //pass single arg to get back empty table
        //4/3/13 changed sWhere = (FALSE) to (0=1) which works with SQL SERVER
            public static string GetSimpleSQLfromTableName(string sTableName, string sWhere = "(0=1)")
            {
                string sql = "select * from " + sTableName + " WHERE " + sWhere;
                return sql;
            }

        //utility function to take a dataset and copy vals into dataset associated with dataadapter
            public void DupDataset(ref DataSet dsTarget, ref DataSet dsSource, bool bHasPK = true)
            {
                int nRows = dsSource.Tables[0].Rows.Count;
                int nCol = dsSource.Tables[0].Columns.Count; int nStartCol = 0;
                if (bHasPK)
                    nStartCol++;

                int nInsertRow = 0;
                for (int i = 0; i < nRows; i++)
                {
                    if (dsSource.Tables[0].Rows[i].RowState == DataRowState.Added){         //for insert
                        dsTarget.Tables[0].Rows.Add(dsTarget.Tables[0].NewRow());
                        for (int j = nStartCol; j < nCol; j++)
                        {
                            if (j < dsTarget.Tables[0].Columns.Count)
                                dsTarget.Tables[0].Rows[nInsertRow][j] = dsSource.Tables[0].Rows[i][j];
                            else
                            {
                                int n = 1; // skip col... must be at end. a bit dangerous. added 12/27/17 (pauline 95) because of addition to result_var
                            }
                        }
                        nInsertRow++;
                    }
                    else if (dsSource.Tables[0].Rows[i].RowState == DataRowState.Modified)      //for update
                    {
                        for (int j = nStartCol; j < nCol; j++)
                        {
                            dsTarget.Tables[0].Rows[i][j] = dsSource.Tables[0].Rows[i][j];
                        }

                    }
                }

            }
            #endregion
        
    }
}
