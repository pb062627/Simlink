using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SIM_API_LINKS
{
    public class external_db:ExternalData 
    {
        public SIM_API_LINKS.DAL.DBContext _dbContext = new SIM_API_LINKS.DAL.DBContext();
        public string _sSQL = "UNDEFINED";
        public string _sCONN = "UNDEFINED";
        public SIM_API_LINKS.DAL.DB_Type _dbType = SIM_API_LINKS.DAL.DB_Type.OLEDB;

        #region INIT
        public external_db(int nID, int nSourceType, int nFormat, Dictionary<string, string> dictArgs, int nSQN = 1, int nColumnNumber = 1, string sColumnName = "1", bool bIsInput = false, bool bIsColIDName = false)
            : base(nID, nSourceType, nFormat, dictArgs, nSQN, nColumnNumber, sColumnName, bIsInput, bIsColIDName)
        {
          //  SetBase(nID, nSourceType, nFormat);

            // met 12/10/16: UNTESTED and will not work after this code was remoed from simlink.cs
            // minor changes needed to get working
            /*
             *                 Dictionary<string, string> dictArgs = new Dictionary<string, string>()
                {
                    {"sql",dr["kwargs"].ToString()},
                    {"connection",dr["conn_string"].ToString()},
                    {"db_type",dr["db_type"].ToString()}
                };
             */ 

            _sSQL = dictArgs["sql"];
            _sCONN = dictArgs["connection"];
            _dbType = (DAL.DB_Type)(Convert.ToInt32(dictArgs["db_type"]));

        }

        #endregion

        #region OPENCLOSE

        #endregion

        #region READ


        /// <summary>
        /// Return a 2D array (though just 1 col) of data
        /// Requires that sql have just 1 col (or that the val be first col returned)
        /// 
        /// </summary>
        /// <param name="lstParams"></param>
        /// <returns></returns>
        public override double[,] RetrieveData(DateTime dtStart, DateTime dtEnd)
        {
            _dbContext.Init(_sCONN, _dbType);

            List<DAL.DBContext_Parameter> lstParams = GetStartEndTimes();           //dtStart, dtEnd);
            DataSet ds = _dbContext.getDataSetfromSQL(_sSQL, lstParams);
            double[,] dReturn = new double[ds.Tables[0].Rows.Count, 1];
            int nCounter = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dReturn[nCounter, 0] = Convert.ToDouble(dr[0].ToString());   // assumes value in first row!
                nCounter++;
            }
            // todo: manage open/close outside of the individual EX, so you can minmize conns
            _dbContext.Close();

            return dReturn;
        }

        //SP 13-Dec-2016 Make compatible with multiple column returns
        /// <summary>
        /// Return a array of 2D arrays through multiple columns of data
        /// Sql can have more than 1 col returned
        /// 
        /// </summary>
        /// <param name="lstParams"></param>
        /// <returns></returns>
        public override double[][,] RetrieveData(Dictionary<string, string> dictRequestToAdd = null, int[] arrReturnColumns = null, Logging log = null)
        {
            AddRequestParams(dictRequestToAdd);

            //SP 13-Dec-2016 by default, retrieve the single return column of the DB request if no other return columns are provided
            if (arrReturnColumns == null)
                arrReturnColumns = new int[] { _nColumnNumber };

            _dbContext.Init(_sCONN, _dbType);

            List<DAL.DBContext_Parameter> lstParams = GetStartEndTimes(); //SP 13-Dec-2016 Does this need to made more generic? What actual parameters do we want to pass?
            DataSet ds = _dbContext.getDataSetfromSQL(_sSQL, lstParams);

            //initialise the number of columns to return to support returning multiple columns
            double[][,] arrreturn = new double[arrReturnColumns.Count()][,];
            for (int i = 0; i < arrReturnColumns.Length; i++)
                arrreturn[i] = new double[ds.Tables[0].Rows.Count, 1];
            //double[,] dReturn = new double[ds.Tables[0].Rows.Count, 1];


            int nCounter = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < arrReturnColumns.Length; i++)
                {
                    arrreturn[i][nCounter, 0] = Convert.ToDouble(dr[arrReturnColumns[i] - 1].ToString());   // assumes value in first row!
                    //dReturn[nCounter, 0] = Convert.ToDouble(dr[0].ToString());   // assumes value in first row
                }
                nCounter++;
            }
            // todo: manage open/close outside of the individual EX, so you can minmize conns
            _dbContext.Close();

            return arrreturn;
        }

        #endregion

            #region WRITE

            #endregion

            #region STATIC


            #endregion

    }
}
