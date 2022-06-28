using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Access.Dao;
using System.IO;
using System.Data;

namespace SIM_API_LINKS.DAL
{
    /// <summary>
    /// class used to help manage size of an access database. This can be vital for both simlink and model files.
    
        //NOTE!!
        //11/2/16: added the same source from sim_api_links into new project ExternalData
            // be advised that any changes may affect VERY CORE functionality in sim_api_links
            // aka, try to avoid changing!
    /// </summary>
    public class Oledb_Maintenance
    {
        public const long _lAccessFileLimit = 2000000000;       // approx of 2gb upper limit
        public double _dCompactIfExceed = -1;           //set to percentage of file size


        /// <summary>
        /// Comp;act and repair a database to a given location
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destFilePath"></param>
        /// <param name="password"></param>
        public void CompactDb(string sourceFilePath, string destFilePath, string password = "NOT_YET_SUPPORTED")
        {
            var dbEngine = new Microsoft.Office.Interop.Access.Dao.DBEngine();
            dbEngine.CompactDatabase(sourceFilePath, destFilePath);
        }

        /// <summary>
        /// Get size of filename
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public long GetFileSize(string sFileName)
        {
            FileInfo f = new FileInfo(sFileName);
            long s1 = f.Length;
            return s1;
        }

        //SP 28-Jul-2016 transfer default values from SQL Server to MSAccess - currently only primary keys are pulled across when linked to SQL Server and local copies of the tables made
        public void TransferDefaultValues(string sConn_Source, int nSourceDBType, string sConn_Dest, int nDestDBType)
        {
            DBContext _dbContextSource = new DBContext();
            bool IsValidConnection = _dbContextSource.Init(sConn_Source, DBContext.GetDBContextType(nSourceDBType));

            DBContext _dbContextDest = new DBContext();
            IsValidConnection = _dbContextDest.Init(sConn_Dest, DBContext.GetDBContextType(nDestDBType));

            //get all tables from SQL server database
            string sqlGetAllTables = "Select * from information_schema.tables where TABLE_TYPE = 'BASE TABLE'";
            DataSet dsTableNames = _dbContextSource.getDataSetfromSQL(sqlGetAllTables);

            //for each table
            foreach (DataRow dr_table in dsTableNames.Tables[0].Rows)
            {
                string sTableName = dr_table["TABLE_NAME"].ToString();
                //get column information for the table
                string sqlGetAllColumns = string.Format("Select c.name from sys.columns c inner join sys.tables t on t.object_id = c.object_id and t.name = '{0}' and t.type = 'U'", sTableName);
                DataSet dsColumnNames = _dbContextSource.getDataSetfromSQL(sqlGetAllColumns);

                //for each column check if there is a default value and update it in source database
                foreach (DataRow dr_columns in dsColumnNames.Tables[0].Rows)
                {
                    string sColumnName = dr_columns["name"].ToString();
                    //get the schema for each column

                    //column_default, data_type
                    string sqlGetColumnSchema = string.Format("Select * from information_schema.columns where table_name = '{0}' and column_name = '{1}'", sTableName, sColumnName);
                    DataSet dsColumnSchema = _dbContextSource.getDataSetfromSQL(sqlGetColumnSchema);

                    //only 1 value should be returned - need to format this when returned
                    string sDefaultValue = dsColumnSchema.Tables[0].Rows[0]["column_default"].ToString();
                    string sType = dsColumnSchema.Tables[0].Rows[0]["data_type"].ToString();
                    
                    int nTypeLength = 0;
                    if (sType.Contains("nvarchar"))
                        nTypeLength = Convert.ToInt32(dsColumnSchema.Tables[0].Rows[0]["character_maximum_length"]);

                    if (sDefaultValue != "")
                    {
                        //change datatype from SQL Server compatible to MS Access compatible
                        string sDefaultValue_OLEDB = GetOleDBDefaultValue(sDefaultValue);
                        string sType_OLEDB = GetOleDBDataType(sType, nTypeLength);

                        //create the string
                        string sqlSetDefaultValue = string.Format("Alter table {0} alter column {1} {2} DEFAULT {3}", sTableName, sColumnName, sType_OLEDB, sDefaultValue_OLEDB);
                        _dbContextDest.ExecuteScalarSQL(sqlSetDefaultValue);
                    }

                }
            }
        }

        private string GetOleDBDataType(string sSQLServerDataType, int nTypeLength)
        {
            switch (sSQLServerDataType)
            {
                case "int":
                    return "integer";
                case "float":
                    return "double";
                case "nvarchar":
                    return string.Format("nvarchar({0})", nTypeLength);
                case "datetime":
                    return "datetime";
                case "bit":
                    return "bit";
                default:
                    return "";
            }
        }

        private string GetOleDBDefaultValue(string sSQLServerDefaultValue)
        {      
            switch (sSQLServerDefaultValue)
            {
                case "(getdate())":
                    return "Now()";
                default:
                    return sSQLServerDefaultValue.Replace("(", "").Replace(")", "").Replace("'",@"""");
            }
        }

    }
}
