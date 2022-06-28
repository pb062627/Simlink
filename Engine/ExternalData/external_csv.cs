using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace SIM_API_LINKS
{
    public class external_csv : ExternalData
    {
        //

        public bool _bReadMethodDataTable = true;            // set to false to read from csv using SQL (not implemented
        public string _sField_Key = "tagname";
        public string _sField_Timestamp = "timestamp";
        public string _sField_Value = "value";
        public string _sFilename = "";
        public char _sDelimeter = ',';                    //default to csv;

        #region INIT
        public external_csv(int nID, int nSourceType, int nFormat, Dictionary<string, string> dictArgs, int nSQN = 1, int nColumnNumber = 1, string sColumnName = "1", bool bIsInput = false, bool bIsColIDName = false)
            : base(nID, nSourceType, nFormat, dictArgs, nSQN, nColumnNumber, sColumnName, bIsInput, bIsColIDName)
        {
            //todo: process dictArgs to set key params
            if (dictArgs.ContainsKey("file"))
                _sFilename = dictArgs["file"];
            else
            {
                _sFilename = "UNDEFINED-MUST SET";
            }
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
        public override double[,] RetrieveData(DateTime dtStart, DateTime dtEnd) //SP 13-Dec-2016 TODO - I believe this function (with args for starttime and endtime) can be removed. Encompassed in _dictRequests
        {
            List<double> lstVals = new List<double>();
            if (_bReadMethodDataTable)
            {
                using (StreamReader stream = new StreamReader(_sFilename))
                {
                    string sBuf = stream.ReadLine();    //header required for version 1.0
                    Dictionary<string, int> dictCols = GetColDict(sBuf);
                    while (!stream.EndOfStream)
                    {
                        string[] sVals = stream.ReadLine().Split(_sDelimeter);
                        if (MeetsCriteria(sVals, dtStart, dtEnd, true))
                        {
                            double dVal = Convert.ToDouble(sVals[dictCols["value"]]);
                            lstVals.Add(dVal);
                        }

                    }

                }
                double[,] dVals = TimeSeries.ConvertListToDouble2D(lstVals);
                return dVals;
            }
            else
            {
                return null;
            }

            //implement

        }

        //SP 13-Dec-2016 Retrieve data to return multiple columns from a CSV avoiding multiple calls 
        /// <summary>
        /// Return an array of 2D arrays (though multiple cols) of data
        /// 
        /// </summary>
        /// <param name="lstParams"></param>
        /// <returns></returns>
        public override double[][,] RetrieveData(Dictionary<string, string> dictRequestToAdd = null, int[] arrReturnColumns = null, Logging log = null)
        {
            AddRequestParams(dictRequestToAdd);

            //SP 13-Dec-2016 by default, retrieve the return the single return column of CSV if no other return columns are provided
            if (arrReturnColumns == null)
                arrReturnColumns = new int[] { _nColumnNumber };

            //initialise the number of columns to return
            List<double>[] lstVals = new List<double>[arrReturnColumns.Count()];
            for (int i = 0; i < arrReturnColumns.Length; i++)
                lstVals[i] = new List<double>();

            DateTime dtStart = new DateTime(); DateTime dtEnd = new DateTime();
            bool bCheckTime = GetTimestampFromDict(dictRequestToAdd, out dtStart, out dtEnd);
            if (_bReadMethodDataTable)
            {
                using (StreamReader stream = new StreamReader(_sFilename))
                {
                    string sBuf = stream.ReadLine();    //header required for version 1.0

                    Dictionary<string, int> dictCols = GetColDict(sBuf); //SP 13-Dec-2016 Don't use column names, instead use arrReturnColumns. maintained to allow for filtering
                    while (!stream.EndOfStream)
                    {
                        string[] sVals = stream.ReadLine().Split(_sDelimeter);
                        if (MeetsCriteria(sVals, dtStart, dtEnd, _bCheckTimestamp))
                        {
                            try
                            {
                                for (int i = 0; i < arrReturnColumns.Length; i++)
                                {
                                    double dVal = Convert.ToDouble(sVals[arrReturnColumns[i] - 1]);
                                    lstVals[i].Add(dVal);
                                }
                            }
                            catch (Exception ex2)
                            {
                                int n = 1;
                            }
                        }

                    }

                }

                double[][,] arrreturn = new double[arrReturnColumns.Count()][,];
                for (int i = 0; i < arrReturnColumns.Length; i++)
                {
                    double[,] dVals = TimeSeries.ConvertListToDouble2D(lstVals[i]);
                    arrreturn[i] = dVals;
                }
                return arrreturn;
            }
            else
            {
                return null;
            }

            //implement

        }





        /// <summary>
        /// return true if row meets  a condition
        /// todo: add conditionality based on row number or timestamp (from _dictRequest
        /// </summary>
        /// <returns></returns>
        private bool MeetsCriteria(string[] sVals, DateTime dtStart, DateTime dtEnd, bool bCheckTime)
        {
            if (sVals.Length < 1 || sVals[0].Trim().Length == 0)
                return false;
            else
            {
                if (bCheckTime == true)
                {
                    DateTime dt = Convert.ToDateTime(sVals[0]);
                    bool bValid = (dt >= dtStart && dt <= dtEnd);
                    return bValid;
                }
                return true;
            }
        }

        private Dictionary<string, int> GetColDict(string sBuf)
        {
            string[] sVals = sBuf.Split(_sDelimeter);
            Dictionary<string, int> dict = new Dictionary<string, int>();
            int nCounter = 0;
            foreach (string s in sVals)
            {
                if (s == _sField_Key)
                {
                    dict.Add("key", nCounter);
                }
                if (s == _sField_Value)
                {
                    dict.Add("value", nCounter);
                }
                if (s == _sField_Timestamp)
                {
                    dict.Add("timestamp", nCounter);
                }
                nCounter++;
            }
            return dict;

        }

        #endregion

        #region WRITE

        public override void WriteData(double[][,] arr2DTimeSeriesToWrite, string sBasePath = null,
            int[] arrWriteColumns = null, string[] arrsColumnHeader = null, List<string> lstLeadColumn = null)
        {

            //SP 13-Dec-2016 by default, retrieve the return the single return column of CSV if no other return columns are provided
            if (arrWriteColumns == null)
                arrWriteColumns = new int[] { _nColumnNumber };

            //   AddRequestParams(dictRequestToAdd);
            bool bIncludeLeadColumn = lstLeadColumn != null;
            bool bIncludeHeader = arrsColumnHeader != null;             // override in dict as well? 
            string sHeader = "";
            //default to ID if header_col is not provided
            string sLeadColumnName = "ID";
            if (_dictRequest.ContainsKey("header_col"))
                sLeadColumnName = _dictRequest["header_col"];

            if (bIncludeLeadColumn)
            {
                // todo: get colID from dict, or infer based on data type.
            }

            if (bIncludeHeader)
                sHeader = GetHeader(arrsColumnHeader, bIncludeLeadColumn, sLeadColumnName, _sDelimeter.ToString(), arrWriteColumns);

            //initialise the number of columns to return
            List<double>[] lstVals = new List<double>[arrWriteColumns.Max()];
            for (int i = 0; i < arrWriteColumns.Max(); i++)
                lstVals[i] = new List<double>();

            try
            {
                //TODO 21-Dec-2016 still need a way to pass in the actual date and times
                /*DateTime dtWrite = tsd._dtStartTimestamp;
                TimeSpan tIncrement = new TimeSpan(0, 0, (int)tsd._nTSIntervalInSeconds);*/

                int nRecords = arr2DTimeSeriesToWrite[0].GetLength(0); //TODO not all array will be the same length - account for this. Currently retrieving the first TS in the list
                using (StreamWriter writer = new StreamWriter(GetOutputFilename(sBasePath)))
                {
                    //write column header line
                    //initialise the row

                    if (bIncludeHeader)
                        writer.WriteLine(string.Join(",", sHeader));

                    /*       string[] arrsHeaderRow = new string[arrWriteColumns.Max() + 1];
                           arrsHeaderRow[0] = "Period";
                    
                           for (int j = 0; j < arrWriteColumns.Count(); j++)
                               if (arrsColumnHeader != null)
                                   arrsHeaderRow[arrWriteColumns[j]] = arrsColumnHeader[j];
                               else
                                   arrsHeaderRow[arrWriteColumns[j]] = arrWriteColumns[j].ToString();


                           writer.WriteLine(string.Join(",", arrsHeaderRow));*/


                    for (int i = 0; i < nRecords; i++)
                    {
                        //initialise the row
                        int nLenRow = arrWriteColumns.Max();
                        if (!bIncludeLeadColumn)
                            nLenRow--;
                        string[] arrsRow = new string[nLenRow];

                        //populate each element in the row
                        if (bIncludeLeadColumn)
                            arrsRow[0] = lstLeadColumn[i];      // (i + 1).ToString(); //Period Number
                        for (int j = 0; j < arrWriteColumns.Count(); j++)
                            try
                            {
                                if (i < arr2DTimeSeriesToWrite[j].GetLength(0))
                                    arrsRow[arrWriteColumns[j] - 1] = arr2DTimeSeriesToWrite[j][i, 0].ToString();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(string.Format("No TS value for period number {0} for TSRecordHeader {1}. Error message: {2}", i, arrsColumnHeader[j], ex.Message));
                                arrsRow[arrWriteColumns[j] - 1] = "";
                            }

                        writer.WriteLine(string.Join(",", arrsRow));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error writing supporting file  ID " + _nUID.ToString());
            }


        }



        #endregion

        #region STATIC
        public static void Write_CSV(string sInfo, TimeSeries.TimeSeriesDetail tsd, double[,] dVals, Dictionary<string, string> dictParams)
        {
            try
            {
                string sFilename = dictParams["filename"];
                DateTime dtWrite = tsd._dtStartTimestamp;
                TimeSpan tIncrement = new TimeSpan(0, 0, (int)tsd._nTSIntervalInSeconds);
                int nRecords = dVals.GetLength(0);
                using (StreamWriter writer = new StreamWriter(sFilename))
                {
                    writer.WriteLine("Timestamp,val");
                    for (int i = 0; i < nRecords; i++)
                    {
                        writer.WriteLine(dtWrite.ToString() + "," + dVals[i, 0].ToString());
                        dtWrite += tIncrement;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error writing ts val" + sInfo);
            }
        }

        #endregion

    }
}
