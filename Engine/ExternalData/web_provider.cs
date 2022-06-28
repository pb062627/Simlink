using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;


namespace SIM_API_LINKS
{
    public class web_provider : ExternalData
    {
        string _sURL = "UNDEFINED";
        List<string> _lstVals = new List<string>();         // todo: move to base, and support multiple request throughout.
        public char _sDelimeter = ',';                      //default to for webbrowser;





        // test web provider location: https://dashboard.cincywsd.com/watershed_ops/pulldata/

        #region INIT
        public web_provider(int nID, int nSourceType, int nFormat, Dictionary<string, string> dictArgs, int nSQN = 1, int nColumnNumber = 1, string sColumnName = "1", bool bIsInput = false, bool bIsColIDName = false)
            : base(nID, nSourceType, nFormat, dictArgs, nSQN, nColumnNumber, sColumnName, bIsInput, bIsColIDName)
        {
            //todo: process dictArgs to set key params
            if (dictArgs.ContainsKey("url"))
                _sURL = dictArgs["url"];
            else
            {
                Console.WriteLine("web provider data source defined without key 'http'");
            }
        }

        #endregion

        #region READ

        // early attempt which did not pan out
        public static async void RetrieveResponseFromWebProvider(Dictionary<string, string> dictRequest, string sHttp)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(dictRequest);
                var resp = await client.PostAsync(sHttp, content);
                var responseString = await resp.Content.ReadAsStringAsync();

                // deserialize the resposne string and figure out how to return this
            }
        }

        //works
        public string Post(Dictionary<string, string> values)
        {
            string result = string.Empty;
            try
            {

                var request = (HttpWebRequest)WebRequest.Create(_sURL);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                var postData = string.Empty;
                foreach (KeyValuePair<string, string> entry in values)
                {
                    postData += string.Format("{0}={1}", entry.Key, entry.Value);
                    postData += "&";
                }

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Sets any key values that were not available at the time of object creation (eg when testing from cli)
        /// </summary>
        /// <param name="dict"></param>
        public override void SetKeyDataValsByDict_AfterCreate(Dictionary<string, string> dict)
        {
            if(dict.ContainsKey("url"))
                _sURL=dict["url"];


           //todo: feedback, eh
        }
        public string GetFileName()
        {
            string sVal = "data_web_" + System.DateTime.Now.ToString();
            sVal = Commons.RMV_FixFilename(sVal);
            return sVal;
        }


        //SP 13-Dec-2016 Make compatible with multiple column returns
        /// <summary>
        /// Dictionary of arguments that the web request needs
        /// The dictionary should be "scenario specific" elements- beyond what is stored on _dictRequest
        /// </summary>
        /// <param name="dictRequest"></param>
        /// <returns></returns>
        public override double[][,] RetrieveData(Dictionary<string, string> dictRequestToAdd = null, int[] arrReturnColumns = null, Logging log = null)
        {
            AddRequestParams(dictRequestToAdd);

            //SP 13-Dec-2016 by default, retrieve the single return column of the webrequest if no other return columns are provided
            if (arrReturnColumns == null)
                arrReturnColumns = new int[] { _nColumnNumber };


            //  _dictRequest["tag_names"] = "M2_28308017_930002_LI_EL_CALC,M2_28308017_930003_LI_EL_CALC ,M2_28308017_930003_LI_EL_CALC ,M2_29509042_930002_LI_EL_CALC,M2_29509042_930003_LI_EL_CALC,M2_33506005_930002_LI_EL_CALC,M2_33506005_930003_LI_EL_CALC,M2_34115035_930002_LI_EL_CALC,M2_34115035_930003_LI_EL_CALC,M2_37501035_930002_LI_EL_CALC";

            string sResult = Post(_dictRequest);

            // write the data out for quick viewing if you want to test...
            if(_bWriteIntermediate)
            {
                if (_sFileOut == "UNDEFINED")
                    _sFileOut = GetFileName();
                File.WriteAllLines(_sFileOut, sResult.Split(new string[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries));         ///"/r/n");
            }

            List<string> lstLines = new List<string>();
            if (!string.IsNullOrEmpty(sResult))
            {
                lstLines = sResult.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
            }

            if (_bGetColumnsFromHeader)                     // get columns based upon dictionary
                SetColumnDictionary(ref arrReturnColumns, lstLines[0], dictRequestToAdd, log);

            //SP 13-Dec-2016 change to now initialise per array value for each column that is being returned
            //double[,] dReturn = new double[lstLines.Count()-3, 1];      // offset by 2 vals; todo: better way of generating list of "real values"; trailing carriage return leaves blank val

            //initialise the number of columns to return
            double[][,] arrreturn = new double[arrReturnColumns.Count()][,];
            for (int i = 0; i < arrReturnColumns.Length; i++)
                arrreturn[i] = new double[lstLines.Count() - 3, 1]; // offset by 2 vals; todo: better way of generating list of "real values"; trailing carriage return leaves blank val

            for (int nCounter = 1; nCounter < lstLines.Count(); nCounter++)
            {
                string[] sVals = lstLines[nCounter].Split(',');         //skip the first header
                if (lstLines.Count() != 292)
                {
                    int n = 1;
                }
                
                if (sVals.Length > 1)   // some values returned are -1
                {
                    for (int i = 0; i < arrReturnColumns.Length; i++)
                    {
                        double dVal = -666.6;
                        int nArrayIndex = arrReturnColumns[i] - 1;
                        //SP 13-Dec-2016 retrieve the specified column value - 1 based index

                        //SP 7-Sept-2017 added a check in here to carry on with default values if one value cannot be retrieved - arrReturnColumns has a value null or 0 which is not a valid index for sVals
                        bool bValid = true;
                        try
                        {
                            bValid = Double.TryParse(sVals[arrReturnColumns[i] - 1], out dVal);       //handle bad data
                        }
                        catch (Exception Ex)
                        {
                            bValid = false;
                        }

                        if (!bValid)
                        {
                            //do something.
                            int n = 1;
                        }
                        else
                            //arrreturn[nArrayIndex-1][nCounter - 1, 0] = dVal;               // metRT2  : had to decrement array index 
                            arrreturn[i][nCounter - 1, 0] = dVal;//SP 9-Mar-2017 I think this needs to be 'i' instead of nArrayIndex
                        }

                }
            }
            return arrreturn; //SP 13-Dec-2016 now return an array of 2D arrays corresponding to each column
        }



        #endregion

        #region Write

        //SP 13-Mar-2017 - Updated with instructions from Raja Kadiyala
        // NEED MET Assistance to get this working - is currently returning 'Bad Gateway'
        public /*async Task*/ string Push(Dictionary<string, string> values)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_sURL);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                var pushData = string.Empty;
                foreach (KeyValuePair<string, string> entry in values)
                {
                    pushData += string.Format("{0}={1}", entry.Key, entry.Value);
                    pushData += "&";
                }

                var data = Encoding.ASCII.GetBytes(pushData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;

                //TODO - Needs updating
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new StreamReader(response.GetResponseStream()).ReadToEnd();

            }
            catch (Exception ex)
            {
                throw;
            }
        }


//SP 13-Mar-2017 Mostly copied from external_csv for now
public override void WriteData(double[][,] arr2DTimeSeriesToWrite, string sBasePath = null,
            int[] arrWriteColumns = null, string[] arrsColumnHeader = null, List<string> lstLeadColumn = null)
        {

            //SP 13-Dec-2016 by default, retrieve the return the single return column of CSV if no other return columns are provided
            if (arrWriteColumns == null)
                arrWriteColumns = new int[] { _nColumnNumber };

            //   AddRequestParams(dictRequestToAdd);
            bool bIncludeLeadColumn = lstLeadColumn != null;

            //get include header from dictionary
            bool bIncludeHeader = arrsColumnHeader != null;
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

            try
            {
                //TODO 21-Dec-2016 still need a way to pass in the actual date and times
                /*DateTime dtWrite = tsd._dtStartTimestamp;
                TimeSpan tIncrement = new TimeSpan(0, 0, (int)tsd._nTSIntervalInSeconds);*/

                int nRecords = arr2DTimeSeriesToWrite[0].GetLength(0); //TODO not all array will be the same length - account for this. Currently retrieving the first TS in the list

                //create the string to be passed to webbrowser
                string sDataToPOST = "";

                //write column header line
                //initialise the row
                if (bIncludeHeader)
                    sDataToPOST = sDataToPOST + string.Join(",", sHeader) + '\n'; // +Environment.NewLine;

                for (int i = 0; i < nRecords; i++)
                {
                    //initialise the row
                    int nLenRow = arrWriteColumns.Max();
                    if (!bIncludeLeadColumn)
                        nLenRow--;
                    string[] arrsRow = new string[nLenRow];

                    //populate each element in the row
                    if (bIncludeLeadColumn)
                        arrsRow[0] = lstLeadColumn[i]+":00";        //.ToString("yyyy-MM-dd HH:mm:ss");      // (i + 1).ToString(); //Period Number

                    for (int j = 0; j < arrWriteColumns.Count(); j++)
                        try
                        {
                            arrsRow[arrWriteColumns[j] - 1] = arr2DTimeSeriesToWrite[j][i, 0].ToString();
                        }
                        catch (Exception ex)
                        {
                            _logEXTERNAL.AddString(string.Format("No TS value for period number {0} for TSRecordHeader {1}. Error message: {2}", i, arrsColumnHeader[j], ex.Message), Logging._nLogging_Level_1, false, true);
                            arrsRow[arrWriteColumns[j] - 1] = "";
                        }

                    //SP 14-Mar-2017 add string to dictionary element
                    sDataToPOST = sDataToPOST + string.Join(",", arrsRow) + '\n';
                }   //moved 12/6/17- was pushing each time


                    if (true)
                    {
                        try
                        {
                            //write the data locally so that we have a copy
                            File.WriteAllText(Path.Combine(sBasePath, "output.csv"), sDataToPOST);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    _dictPush.Add("csv_data", sDataToPOST);
                    var responseString = Push(_dictPush);

                    _logEXTERNAL.AddString(string.Format("tblSupportingFileSpec ID {0}", _nUID.ToString()), Logging._nLogging_Level_1, false, true);
                    _logEXTERNAL.AddString(string.Format("{0}", responseString.ToString()), Logging._nLogging_Level_1, false, true);

            }
            catch (Exception ex)
            {
                _logEXTERNAL.AddString(string.Format("error writing tblSupportingFileSpec ID {0}", _nUID.ToString()), Logging._nLogging_Level_1, false, true);
                throw ex;
            }

        }

        #endregion

    }
}
