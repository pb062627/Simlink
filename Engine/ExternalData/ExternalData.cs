using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace SIM_API_LINKS
{
    /// <summary>
    /// an external data object knows how to read / write to different data formats
    /// the initial implementation is to read from external sources.
    /// when invoked, it retrieves a timeseries list, which simlink can then work with.
    /// 
    /// TODO
    ///     - HDF5 class could be re-written as a derived class
    ///     - 
    /// </summary>
    /// 

    public enum ExternalDataType
    {
        MS_ACCESS = 0,
        SQL_SERVER, 
        XML, 
        CSV,
        WEB_SERVICE,  //4
        HDF5,
        DSS,
        DAT,                        // probably want to override the CSV format for this... want to keep reduced set of distincly different types
        TBLMODELEMENTVALS,
        WEB_SERVICE_NOAA_FORECAST     //  9
    }
    public enum DataFormat
    {
        Dataset = 0,
        Timeseries,
        Val
    }

    public class ExternalData
    {
        public ExternalDataType _externaldatatype = ExternalDataType.MS_ACCESS;
        public DataFormat _return_format = DataFormat.Timeseries;
        public int _nUID = -1;
        public int _nGroupID = 1;
        public int _nColumnNumber = -1;
        public string _sColumnName = "";
        public bool _bIsInput = false;
        public int _nTSRecordID = -1;
        public string _sDescription = "";
        public int _nDVID = -1;
        public TimeSpan _tsShiftTimeFromPresent = TimeSpan.FromSeconds(0);        //ofset from present time  - taken from realtime 
        public Dictionary<string, string> _dictRequest;                         // used to house items needed by the external data request
        public Dictionary<string, string> _dictPush;                         // SP 14-Mar-2017 used to house items needed by the external data push - TODO not sure if we need a separate one for pushing
        public bool _bGetColumnsFromHeader = true;
        public string _sHeaderColumnInDict = "column_names";                                    // which dict key to grab to populate correct array indices
        public Logging _logEXTERNAL = new Logging();
        public bool _bCheckTimestamp = false;
        private string _sTimestampFormat = "yyyy-MM-dd HH:mm"; //default timestamp if not specified
        private bool _bDestColumnIsSequenceOnly = true; //default if not specified
        private double _dScalar = 1.0;

        public string _sFileOut = "UNDEFINED";      // location to write the CSV
        public bool _bWriteIntermediate = false;    // whether to write out the data after it is received

        //   public Dictionary<string, int> _dictColumns = new Dictionary<string, int>();

        #region INIT
        public ExternalData(int nID, int nExternalDataType, int nFormat, Dictionary<string,string> dictArgs, int nGroupID = 1, int nColumnNumber = 1, string sColumnName = "1", 
            bool bIsInput = false, bool bIsColIDName = false)
        {
         //   SetBase(nID,nSourceType,nFormat);
            _nUID = nID;
            _return_format = (DataFormat)nFormat;
            _externaldatatype = (ExternalDataType)nExternalDataType;
            _nGroupID = nGroupID; //SP 13-Dec-2016
            
            //SP 14-Apr-2017 ColumnID can be an integer or a name of the column
            _sColumnName = sColumnName;
            _nColumnNumber = nColumnNumber; //SP 13-Dec-2016

            _bIsInput = bIsInput;
            _dictRequest = dictArgs;
            _dictPush = new Dictionary<string, string>();

            
            //SP 14-Apr-2017 Now using bIsColIDName field to determine if column is a number or name identifier
            /*
            _bGetColumnsFromHeader = false;
            if (dictArgs.ContainsKey("use_header"))
            {
                if (dictArgs["use_header"].ToLower() == "y" || dictArgs["use_header"].ToLower() == "yes")
                {
                    _bGetColumnsFromHeader = true;
                }
            }*/
            _bGetColumnsFromHeader = bIsColIDName;

            // todo: consider if these dict kvp are important enough to get their own columns!
            if (dictArgs.ContainsKey("header_col"))
            {
                _sHeaderColumnInDict = dictArgs["header_col"];
            }
            if (dictArgs.ContainsKey("check_timestamp"))
            {
                _bCheckTimestamp = dictArgs["check_timestamp"].ToLower() == "y" || dictArgs["check_timestamp"].ToLower() == "yes";
            }
            //SP 29-Mar-2017 Use default timestamp if not specified
            if (dictArgs.ContainsKey("timestamp_format"))
            {
                _sTimestampFormat = dictArgs["timestamp_format"];
            }

            //SP 29-Mar-2017 Use default if not specified
            if (dictArgs.ContainsKey("Dest_Column_Is_Sequence"))
            {
                _bDestColumnIsSequenceOnly = dictArgs["Dest_Column_Is_Sequence"].ToLower() == "y" || dictArgs["Dest_Column_Is_Sequence"].ToLower() == "yes";
            }

            //SP 17-Apr-2017 Use default if not specified for Scalar for the group
            if (dictArgs.ContainsKey("group_scalar"))
            {
                _dScalar = Convert.ToDouble(dictArgs["group_scalar"].ToString());
            }
        }


        // need to call this first to get the dict based upon kwargs
        public static Dictionary<string, string> GetExternalDataDictionary(ExternalDataType source, string sParams, string sConn)
        {
            Dictionary<string,string> dict = new Dictionary<string,string>();
            if (source == ExternalDataType.MS_ACCESS || source == ExternalDataType.SQL_SERVER)
            {
                dict = new Dictionary<string, string>()
                {
                    {"sql",sParams},
                    {"connection",sConn},
                    {"kwargs",sParams},
                };
                if (source == ExternalDataType.MS_ACCESS)      // better to not hard code these db types in here, but oh well.
                    dict.Add("db_type", "0");
                else
                    dict.Add("db_type", "1"); //SP 13-Dec-2016 Should this be a 1 for DBType SQLServer?
            }
            else
            {
                dict = GetDict(sParams);

                // add provider specific vals to the dict here
                switch (source)
                {
                    case ExternalDataType.WEB_SERVICE:
                        dict.Add("url", sConn);
                        break;

                    //SP 13-Dec-2016 Still needs work but setup for testing. sConn will be filename for CSV. not sure if any kwargs are needed
                    case ExternalDataType.CSV:
                        dict.Add("file", sConn);
                        break;
                }
            }
            return dict;
         }


        /// <summary>
        /// Get an external data connection
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="nSourceType"></param>
        /// <param name="nFormat"></param>
        /// <param name="dictArgs"></param>
        /// <returns></returns>
        public static ExternalData GetExternalData(int nID, int nSourceType, int nFormat, string sParams, string sConn, 
            int nGroupID = 1, int nColumnNumber = 1, string sColumnName = "1", bool bIsInput = false, bool bIsColIDName = false)
        {
            ExternalDataType source = (ExternalDataType)nSourceType;
            Dictionary<string, string> dictArgs = GetExternalDataDictionary(source, sParams, sConn);

            switch (source)
            {
                case ExternalDataType.MS_ACCESS:
                    external_db db = new external_db(nID, nSourceType, nFormat, dictArgs, nGroupID, nColumnNumber, sColumnName, bIsInput, bIsColIDName);
                    return db;
                case ExternalDataType.SQL_SERVER:
                    external_db db2 = new external_db(nID, nSourceType, nFormat, dictArgs, nGroupID, nColumnNumber, sColumnName, bIsInput, bIsColIDName);
                    return db2;
                case ExternalDataType.XML:
                    Console.WriteLine("not implemented yet");
                    ExternalData x = new external_db(nID, nSourceType, nFormat, dictArgs, nGroupID, nColumnNumber, sColumnName, bIsInput, bIsColIDName);  //override w xml
                    return x;
                case ExternalDataType.CSV:
                    external_csv csv = new external_csv(nID, nSourceType, nFormat, dictArgs, nGroupID, nColumnNumber, sColumnName, bIsInput, bIsColIDName);  //override w xml
                    return csv;
                case ExternalDataType.WEB_SERVICE:
                    web_provider web = new web_provider(nID, nSourceType, nFormat, dictArgs, nGroupID, nColumnNumber, sColumnName, bIsInput, bIsColIDName);  //override w xml
                    return web;
                case ExternalDataType.WEB_SERVICE_NOAA_FORECAST:
                    noaa_webservice noaa = new noaa_webservice(nID, nSourceType, nFormat, dictArgs, nGroupID, nColumnNumber, sColumnName, bIsInput, bIsColIDName);
                    return noaa;
                default:
                    return new ExternalData(nID, nSourceType, nFormat, dictArgs, nGroupID, nColumnNumber, sColumnName, bIsInput, bIsColIDName);
            }
        }


        public static Dictionary<string, string> GetDict(string sParams, List<KeyValuePair<string,string>> lstKVP = null)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            // now iterate over kvps
            string[] sVals = sParams.Split(',');        // CRITICAL : right now can be no commas in the params. figure out better way.
            if (sVals[0].Trim().Length > 1)
            {
                foreach (string sKVP in sVals)
                {
                    string[] s = sKVP.Split('?');          // ad-hoc delimiter  bojangles must do better.
                    if (s.Length == 2)
                    {
                        dict.Add(s[0], s[1]);
                    }
                    else
                    {
                        if(sKVP!="-1")
                            Console.WriteLine("prob with kvp: {0}", sKVP);
                    }
                }
            }
            return dict;
        }
      
        //SP 13-Dec-2016 not used
        /*public void SetBase(int nID, int nSourceType, int nFormat)
        {
            _nUID = nID;
            _return_format = (ReturnFormat)nFormat;
            _source = (SourceType)nSourceType;
        }*/

        #endregion

        #region OPENCLOSE

        #endregion

        #region READ

        // NOTE: I would prefer to pass as arg the lstParams as follows: List<DAL.DBContext_Parameter> lstParams = null
            // HOWEVER - this will require using a single DAL reference for both sim_api and external
            // that should be implemente as soon as we have time- but don't want to mess up core functionality
        public virtual double[,] RetrieveData(DateTime dtStart, DateTime dtEnd /*DateStart and DateEnd need to be added to dictionary*/)
        {
            List<DAL.DBContext_Parameter> lstParams = GetStartEndTimes();
            return null;
        }

        //SP 13-Dec-2016 changed to return an array of double[,]
        public virtual double[][,] RetrieveData(Dictionary<string, string> dictRequestToAdd = null, int[] arrReturnColumns = null, Logging log = null)
        {
            return null;
        }

        /// <summary>
        /// Sets any key values that were not available at the time of object creation (eg when testing from cli)
        /// </summary>
        /// <param name="dict"></param>
        public virtual void SetKeyDataValsByDict_AfterCreate(Dictionary<string, string> dict)
        {

        }


        //SP 21-Dec-2016
        public virtual void WriteData(double[][,] arr2DTimeSeriesToWrite, string sBasePath = null,
            int[] arrWriteColumns = null, string[] arrsColumnHeader = null, List<string> lstLeadColumn = null)
        {
            // copy the csv stuff...  trying to write out data?    - MET testing 10/21/17
            char sDelimeter = ',';
         
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
                sHeader = GetHeader(arrsColumnHeader, bIncludeLeadColumn, sLeadColumnName, sDelimeter.ToString(), arrWriteColumns);

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
                Console.WriteLine("error writing tblSupportingFileSpec ID " + _nUID.ToString());
            }




        }



        protected string GetOutputFilename(string sTargetDir)
        {
            return Path.Combine(sTargetDir, _dictRequest["file"] + ".csv");
        }

        /// <summary>
        /// todo: add dateend param
        /// </summary>
        /// <param name="dtStart"></param>
        /// <returns></returns>
        public List<DAL.DBContext_Parameter> GetStartEndTimes(/*Dictionary<string, string> dictArgs DateTime dtStart, DateTime dtEnd*/)
        {
            List<DAL.DBContext_Parameter> lstParams = new List<DAL.DBContext_Parameter>();
            //SP 13-Dec-2016 check if start_date and end_date exist first.... part of generic dictionary change
            if (_dictRequest.ContainsKey("start_date"))
            {
                DAL.DBContext_Parameter p1 = new DAL.DBContext_Parameter("p1", DAL.DataTypeSL.DATETIME, _dictRequest["start_date"]);
                lstParams.Add(p1);
            }
            if (_dictRequest.ContainsKey("end_date"))
            {
                DAL.DBContext_Parameter p2 = new DAL.DBContext_Parameter("p2", DAL.DataTypeSL.DATETIME, _dictRequest["end_date"]);
                lstParams.Add(p2);
            }
            
            return lstParams;
        }

        /// <summary>
        /// Review header and assign column index
        /// Added 12/21/16 based on web provider responding with sorted column info
        /// </summary>
        /// <param name="sHeaders"></param>
        /// <param name="dictRequestToAdd"></param>
        /// <param name="sKeyForHeader"></param>
        /// <returns></returns>
        public void SetColumnDictionary(ref int[] arrReturnColumns, string sHeaders, Dictionary<string, string> dictRequestToAdd, Logging log = null)           //, string sKeyForHeader)
        {
            string[] sColHeadings = sHeaders.Split(',');        // todo: reference delimiter on base class
            string sVal = "";
            if (dictRequestToAdd.ContainsKey(_sHeaderColumnInDict)) //SP 14-Apr-2017 Now a column DestColumn
            {
                string[] sRequestInSequence = dictRequestToAdd[_sHeaderColumnInDict].Split(',');
                for (int i = 0; i < sRequestInSequence.Length; i++)
                {
                    int nIndex = Array.FindIndex(sColHeadings, item => item == sRequestInSequence[i]);
                    if (nIndex >= 0)
                    {
                        arrReturnColumns[i] = nIndex + 1; //SP 9-Mar-2017 added  + 1. arrReturnColumns should be a 1 based index - similar to database entries. TODO maybe make all of these 0 based indexes for simplicity
                    }
                    else
                    {
                        //Console.WriteLine("{0} not found in header", sRequestInSequence[i]);                         // not found - let user know
                        log.AddString(string.Format("{0} not found in header", sRequestInSequence[i]), Logging._nLogging_Level_1, false, true);
                    }
                }
            }
            else{
                // problem
                // inform user and hope for best with original requsest....
                //Console.WriteLine("Key {0} not found in external data request", _sHeaderColumnInDict);
                log.AddString(string.Format("Key {0} not found in external data request", _sHeaderColumnInDict), Logging._nLogging_Level_1, false, true);
                // do nothing
                return;
            }
        }

        #endregion

        #region WRITE
        public void Write()
        {
            //implement
        }
        /// <summary>
        /// Create header for export file
        /// In base class to allow use for multiple cases
        /// </summary>
        /// <param name="arrsColumnHeader"></param>
        /// <param name="bIncludeLeadColumn"></param>
        /// <param name="sLeadColumnName"></param>
        /// <returns></returns>
        protected string GetHeader(string[] arrsColumnHeader, bool bIncludeLeadColumn, string sLeadColumnName, string sDelimiter, int[] arrWriteColumns)
        {
            //SP 28-Dec-2016 Column headers may not be in order
            int nSize = arrWriteColumns.Max();
            if (!bIncludeLeadColumn)
                nSize--;

            string[] sOrderedHeader = new string[nSize];
            if (bIncludeLeadColumn)
                sOrderedHeader[0] = sLeadColumnName;

            for (int j = 0; j < arrWriteColumns.Count(); j++)
            {
                try
                {
                    sOrderedHeader[arrWriteColumns[j] - 1] = arrsColumnHeader[j];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("No header value for column {1}. Error message: {2}", arrWriteColumns[j], ex.Message));
                    sOrderedHeader[arrWriteColumns[j] - 1] = "";
                }
            }

            string s = string.Join(sDelimiter.ToString(), sOrderedHeader);
            /*if (bIncludeLeadColumn)
            {
                s = sLeadColumnName + sDelimiter + s;
            }*/
            return s;
        }
        #endregion

        #region STATIC
        public static void Write(string sInfo, TimeSeries.TimeSeriesDetail tsd, double[,] dVals,Dictionary<string,string> dictParams){


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictRequestToAdd"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public bool GetTimestampFromDict(Dictionary<string, string> dictRequestToAdd,out DateTime dtStart, out DateTime dtEnd)
        {
            bool bCheckTime = false;
            dtStart = System.DateTime.Now;
            dtEnd = System.DateTime.Now;

            if (_bCheckTimestamp)
            {
                dtStart = Convert.ToDateTime(dictRequestToAdd["start_date"]);       // this dict is provided by the calling app (like RT)
                dtEnd = Convert.ToDateTime(dictRequestToAdd["end_date"]);
            }      

            return bCheckTime;
        }

        /// <summary>
        /// This is a stub function to generate an argument that is subsequently used by the External Data Request.
        /// Developed initially for a web provider where tag names require concatentation.
        /// Ultimately this will neeed to be generalized significantly, and may be called within ExternalData only...
        /// 
        /// //todo: Figure out WHOSE RESPONSIBILITY it is to filter a dataset for multiple data sources
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="sObj"></param>
        /// <param name="nFilterCode"></param>
        /// <param name="cDelimKwarg"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        //SP 14-Apr-2017 - no longer used - kept in case this function is useful for other reasons.
        /*public static string CreateMultiElementSearchString(List<ExternalData> lst, bool bUseColumnName = true, string sDictObj = "", int nFilterCode = 1, char cDelimPrimary = ',', char cDelimKwarg = '?', char delim = ',')
        {
            string sJoin = "";
            foreach (ExternalData ex in lst)                    // in ds.Tables[0].Rows)
            {
                string sVAL = "UNDEFINED";
                //SP 14-Apr-2017 Check if creating multielement string from a dictionary item or ColumnName
                if (bUseColumnName)
                {
                    sVAL = ex._sColumnName;
                }
                else
                {
                    //SP 14-Apr-2017 May not be necessary anymore - kept for now in case this is something we need later
                    if (ex._dictRequest.ContainsKey(sDictObj))
                        sVAL = ex._dictRequest[sDictObj];
                }   

                //SP 9-Mar-2017 Test for duplicates
                string[] sVALarr = sVAL.Split(delim).ToArray();
                foreach (string sVALItem in sVALarr)
                {
                    if (!sJoin.Split(delim).Contains(sVALItem))
                        sJoin += sVAL + delim;
                }
            }
            if (sJoin!="")
                sJoin=sJoin.Substring(0,sJoin.Length-1);    // rm the last delimiter
            else
                sJoin="NO_ROWS_IN_EXTERNAL_DATA_REQUEST";

            return sJoin;
        }*/

        //SP 14-Apr-2017 this is no longer used as we can utilize the _sColumnName field of the ExternalData Object
        //SP 13-Mar-2017 Create header row from dictionary elements in the same group
        /*public static string CreateMultiElementHeaderString(List<ExternalData> lst, string sObj, int nFilterCode = 1, char cDelimPrimary = ',', char cDelimKwarg = '?', char delim = ',')
        {
            string[] sarrHeaderValues = new string[lst.Max(x => x._nColumnNumber)];
            //string sJoin = "";
            foreach (ExternalData ex in lst)                    // in ds.Tables[0].Rows)
            {
                string sVAL = "UNDEFINED";
                if (ex._dictRequest.ContainsKey(sObj))
                    sarrHeaderValues[ex._nColumnNumber] = ex._dictRequest[sObj];
            }

            //convert to a string
            return string.Join(cDelimPrimary.ToString(), sarrHeaderValues);
        }*/


        /// <summary>
        /// SP 9-Mar-2017
        /// SP 14-Apr-2017 this is no longer used as we can utilize the _sColumnName field of the ExternalData Object
        /// populate fake "return columns" - i.e. what will be returned from RetrieveData
        /// </summary>
        /// <returns></returns>
        /*public static void PopulateReturnColumns(List<ExternalData> lst, string sHeaderRowDictKey, string sMultiElementSearchString, char delim = ',')
        {
            //decouple the multi element search string
            string[] sElementarr = sMultiElementSearchString.Split(delim).ToArray();

            //for each ex in the list, find the position in the array of the first element in sHeaderRowDictKey which will correspond to the order of the dataset returned from RetrieveData
            foreach (ExternalData ex in lst)                    
            {
                int nReturnColumn = Array.IndexOf(sElementarr, ex._sColumnName); //_dictRequest[sHeaderRowDictKey].Split(delim).ToArray()[0]);
                if (nReturnColumn >= 0)
                    ex._nColumnNumber = nReturnColumn;
            }
        }*/

        //SP 13-Dec-2016 TODO a tidier way of passing through processed list of lstExternalDataSources. Still requires a call to EGDS_GetExternalDataSources in Simlink to create input list
        public static double[][,] ExecuteGetExternalDataSources(List<ExternalData> lstExternalDataSources, Dictionary<string, string> dictAdditionalRequests = null, 
            /*string sNewKey="NONE",*/ Logging log = null)
        {
            //SP 13-Dec-2016 Instead of looping through _dsEG_TS_Combined_Request, loop through arrGroupID in ExternalDataRequest list
            //maintain list of completed ExternalDataRequests to avoid duplicating
            List<int> ExternalDataRequestsToComplete = lstExternalDataSources.Select(x => x._nUID).ToList();

            double[][,] dValsExternalData = new double[lstExternalDataSources.Count()][,];

            //Get list of distinct GroupIDs   Note: Can we allow -1 to mean "no group", or do we need to enforce distinct?
            int[] arrGroupID = lstExternalDataSources.Select(x => x._nGroupID).ToArray().Distinct().ToArray();
            int  nLocalLogLevel = Logging._nLogging_Level_2;
            foreach (int sGroupID in arrGroupID)
            {
                //get all the ExternalDataRequests with the GroupID and same sourcecode, return_format_code, db_type, kwargs and conn_string
                List<ExternalData> lstExternalDataSources_Group = lstExternalDataSources.FindAll(x => x._nGroupID == sGroupID);
                bool bIsMultiple = lstExternalDataSources_Group.Count() > 1;
                if (bIsMultiple)
                    nLocalLogLevel = Logging._nLogging_Level_3;
                else
                    nLocalLogLevel = Logging._nLogging_Level_2;


                // MET
                // here we need to potentially add additional elements to the dictionary.
                // SP 9-Mar-2017 moved this to be external data object specific 
                /*if (sNewKey != "NONE")
                {
                    string sVal = CreateMultiElementSearchString(lstExternalDataSources.Where(x => x._nGroupID==sGroupID).ToList(), sNewKey);
                    if(dictAdditionalRequests.ContainsKey(sNewKey))
                        dictAdditionalRequests[sNewKey]=sVal;
                    else
                        dictAdditionalRequests.Add(sNewKey,sVal);
                }*/



                //obtain all Requests with same arguments - accounts for error checking if user has accidentally indicated to retrieve in the same call when they are in different destinations
                foreach (ExternalData ex_Request in lstExternalDataSources_Group)
                {
                    if (ExternalDataRequestsToComplete.Contains(ex_Request._nUID))
                    {
                        if (ex_Request._nUID==24)
                        {
                            int n = 1;
                        }

                        //SP 13-Dec-2016 This is a more thorough check through _dictRequests
                        List<ExternalData> lstExternalDataSources_Grouped = ex_Request.GetGroupedExternalData(lstExternalDataSources_Group);

                        //SP 9-Mar-2017 moved from above - check if we should be using header
                        int[] arrReturnColumns = null;
                        string[] arrReturnColumns_Names = null;
                        bool bGetColsFromHeader = ex_Request._bGetColumnsFromHeader;

                        if (bGetColsFromHeader)
                        {
                            //string sVal = CreateMultiElementSearchString(lstExternalDataSources_Grouped); //SP 14-Apr-2017 modified function to get column names by default 
                            //SP 14-Apr-2017 - can now build string based on distinct column names - get the list of distinct columns to return
                            arrReturnColumns_Names = lstExternalDataSources_Grouped.Select(x => x._sColumnName).ToArray().Distinct().ToArray();
                            string sVal = string.Join(",", arrReturnColumns_Names);
                            if (dictAdditionalRequests.ContainsKey(ex_Request._sHeaderColumnInDict))
                                dictAdditionalRequests[ex_Request._sHeaderColumnInDict] = sVal; // sVal;
                            else
                                dictAdditionalRequests.Add(ex_Request._sHeaderColumnInDict, sVal); // sVal);

                            //determine the returned column position based on the order of the _sHeaderColumnInDict in dictionary string -  returned columns from RetrieveData will be ordered based on _sHeaderColumnInDict
                            //SP 14-Apr-2017 - can now perform PopulateReturnColumns with a simple linq function
                            //PopulateReturnColumns(lstExternalDataSources_Grouped, ex_Request._sHeaderColumnInDict, sVal);
                            lstExternalDataSources_Grouped.ForEach(x => x._nColumnNumber = Array.IndexOf(arrReturnColumns_Names, x._sColumnName));
                            arrReturnColumns = new int[dictAdditionalRequests[ex_Request._sHeaderColumnInDict].Split(',').Count()];         // initialize array with correct # of vars
                        }
                        else
                        {
                            //get the list of distinct columns to return - this is a 1 based index as per database
                            arrReturnColumns = lstExternalDataSources_Grouped.Select(x => x._nColumnNumber).ToArray().Distinct().ToArray();
                        }
                        
                        try
                        {
                            //retrieve the Data from single external data request into a temporary double[][,] storing each of the columns returned
                            double[][,] tempdVals = ex_Request.RetrieveData(dictAdditionalRequests, arrReturnColumns, log);
                            //store each external data request in array of dVals

                            int nCounter = 0;
                            foreach (ExternalData exGrouped in lstExternalDataSources_Grouped)
                            {
                                if (bGetColsFromHeader)
                                {
                                    // values should be in order
                                    //dValsExternalData[nCounter] = tempdVals[nCounter];          //arrReturnColumns[nCounter]];
                                    //SP 9-Mar-2017 _nReturnColumn is set based on the position of _sHeaderColumnInDict
                                    dValsExternalData[lstExternalDataSources.IndexOf(exGrouped)] = tempdVals[exGrouped._nColumnNumber];     //SP 9-Mar-2017 I think this needs to be the index of the Ext data in the External Data List. May have Web requests and CSV requests
                                }
                                else
                                {       // original case, where we use the column number
                                    dValsExternalData[lstExternalDataSources.IndexOf(exGrouped)] = tempdVals[Array.IndexOf(arrReturnColumns, exGrouped._nColumnNumber)];
                                }
                                nCounter++;
                            }

                            // note: Log level set to be more VERBOSE for "multiple" data requests
                            log.AddString(string.Format("Retrieved external data for ExternalDataRequestID {0} and others in the group. # external data sets attempted: {1}", ex_Request._nUID.ToString(), nCounter), nLocalLogLevel, false, true);
                            ExternalDataRequestsToComplete.RemoveAll(y => lstExternalDataSources_Grouped.Select(x => x._nUID).Contains(y));
                        }
                        catch (Exception ex)
                        {
                            log.AddString(string.Format("Failed trying to retrieve external data request for ID" + ex_Request._nUID.ToString() + ". msg: " + ex.Message), nLocalLogLevel, false, true);
                        }
                    }
                }
            }

            return dValsExternalData;
        }


        //SP 20-Dec-2016
        // met: changed to non-static function.  I think that over time, having more data available on the ex - especially LEAD ex- will be useful.
        //SP 13-Mar-2017 NOTE: sHeader was not being used here - commented out to avoid confusion for now
        public void ExecuteWritingSupportingFileSpec(double[][,] dValsSupportingFileSpec, /*string sHeader,*/
             List<string> lstDate, List<ExternalData> lstExternalDataDestinations, string sBasePath = null)
        {
            //maintain list of completed ExternalDataRequests to avoid duplicating
            List<int> lstExternalDataDestinationsToComplete = lstExternalDataDestinations.Select(x => x._nUID).ToList(); 

            //Get list of distinct GroupIDs
            int[] arrGroup = lstExternalDataDestinations.Select(x => x._nGroupID).ToArray().Distinct().ToArray();

            foreach (int nGroupID in arrGroup)
            {
                //get all the SupportingFileSpecExports with the GroupID and same outputformat_code, return_format_code and filename
                List<ExternalData> lstExternalDataDestinations_Group = lstExternalDataDestinations.FindAll(x => x._nGroupID == nGroupID);

                //obtain all Exports with same arguments - accounts for error checking if user has accidentally indicated to retrieve in the same call when they are in different destinations
                foreach (ExternalData ex_Output in lstExternalDataDestinations_Group)
                {
                    if (lstExternalDataDestinationsToComplete.Contains(ex_Output._nUID))
                    {
                        //This is a more thorough check through _dictRequests - TODO
                        List<ExternalData> lstExternalDataDestinations_Filtered = ex_Output.GetGroupedExternalData(lstExternalDataDestinations_Group);

                        //SP 13-Mar-2017 added a bool to track whether we want to get the header row from dictionary
                        bool bGetHeaderFromDictionary = ex_Output._bGetColumnsFromHeader;

                        //SP 29-Mar-2017 renumber the nReturnColumn to be sequential
                        if (_bDestColumnIsSequenceOnly)
                        {
                            //order the list by _nReturnColumn and then reindex
                            lstExternalDataDestinations_Filtered = lstExternalDataDestinations_Filtered.OrderBy(x => x._nColumnNumber).ToList();

                            lstExternalDataDestinations_Filtered = lstExternalDataDestinations_Filtered.Select
                                (x => { x._nColumnNumber = lstExternalDataDestinations_Filtered.IndexOf(x) + 2; return x; }).ToList(); //+2 as first column is 2 to allow for column 1 to be leading date column 
                        }

                        //get the list of distinct columns to write to - use 1 based index
                        int[] arrReturnColumns = lstExternalDataDestinations_Filtered.Select(x => x._nColumnNumber).ToArray();

                        try
                        {
                            //find the positions of the dVals arrays for group of ExternalData and create list of dVals[,]
                            List<double[,]> lstdValsToWrite = new List<double[,]>();
                            List<string> lstsTSHeaders = new List<string>();

                            //SP 29-Mar-2017 Temp hardcoding moved from Simlink.cs - 14-Apr-2017 now specified in params 'group_scalar'
                            //double dScalar = 1.0 / (24.0 * 60.0 * 60.0) * 300.0;         // convert MGD to MGAL assuming a 5 min timestep   TODO provide as a parameter

                            foreach (ExternalData ex_position in  lstExternalDataDestinations_Filtered)
                            {
                                int nPos = lstExternalDataDestinations.FindIndex(x => x == ex_position);
                                lstdValsToWrite.Add((double[,])dValsSupportingFileSpec[nPos].Clone()); //SP 29-Mar-2017 Clone required to avoid applying scalar more than once

                                //SP 14-Apr-2017 scalar now specified in params for group in dictionary
                                for (int i = 0; i < lstdValsToWrite[lstdValsToWrite.Count() - 1].Length; i++)
                                {
                                    lstdValsToWrite[lstdValsToWrite.Count() - 1][i, 0] = lstdValsToWrite[lstdValsToWrite.Count() - 1][i, 0] * ex_position._dScalar;
                                }

                                if (!bGetHeaderFromDictionary)           // todo: pass arg if you want header
                                {
                                    string sHeaderVal = GetHeaderVal(ex_position); //,lstExternalDataDestinations[nPos]._sDescription);
                                    lstsTSHeaders.Add(sHeaderVal);
                                }
                            }

                            if (bGetHeaderFromDictionary)
                            {
                                //SP 13-Mar-2017 added option to get the header from the dictionary arguments
                                //string sVal = CreateMultiElementHeaderString(lstExternalDataDestinations_Filtered, ex_Output._sHeaderColumnInDict);
                                //lstsTSHeaders = sVal.OfType<string>().ToList();
                                //SP 14-Apr-2017 can now utilize the sColumnName field to get the list of headers
                                lstsTSHeaders = lstExternalDataDestinations_Filtered.Select(x => x._sColumnName).ToList();
                            }

                            //SP 29-Mar-2017 convert the date format
                            lstDate = ex_Output.ConvertDateFormat(lstDate);

                            //write the Data by passing in the required array of TS and the columns
                            ex_Output.WriteData(lstdValsToWrite.ToArray(), sBasePath, arrReturnColumns, lstsTSHeaders.ToArray(), lstDate);
                            
                            //if successfully retrieved the data, indicate that External Data Source has been exhausted through identical GroupIDs
                            Console.WriteLine("Sucessfully completed write external data destinations for ID " + ex_Output._nUID.ToString() + " and others with same GroupID.");
                            lstExternalDataDestinationsToComplete.RemoveAll(y => lstExternalDataDestinations_Filtered.Select(x => x._nUID).Contains(y));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed trying to write external data destinations for ID " + ex_Output._nUID.ToString() + ". msg: " + ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return a column header  
        /// </summary>
        /// <param name="nUID"></param>
        /// <returns></returns>
        /// SP 14-Mar-2017 Made static as need to pass in an external data object to get each individual key
        private static string GetHeaderVal(ExternalData exD)
        {
            string sReturn = exD._nUID.ToString();
            if(exD._dictRequest.ContainsKey(exD._sHeaderColumnInDict))
            {
                sReturn = exD._dictRequest[exD._sHeaderColumnInDict];
            }
            return sReturn;
        }

        #endregion
        #region UTIL
        /// <summary>
        /// Takes params associated with a specific paramater and either add or overwrites to the external data objects base _dictRequest object
        ///     - first implementd on web_provider
        /// </summary>
        /// <param name="dictRequestToAdd"></param>
        public void AddRequestParams(Dictionary<string, string> dictRequestToAdd)
        {
            if (dictRequestToAdd != null) //SP 13-Dec-2016 allow for default of null dictionary
            {
                foreach (KeyValuePair<string, string> kvp in dictRequestToAdd)
                {
                    if (_dictRequest.ContainsKey(kvp.Key))
                        _dictRequest[kvp.Key] = kvp.Value;           //update the item
                    else
                        _dictRequest.Add(kvp.Key, kvp.Value);
                }
            }
        }

        //SP 13-Dec-2016 Need to do a more thorough check through _dictRequests in individual classes - different source types contain different dictArgs
        //Assume user may not pay close attention to GroupID
        private List<ExternalData> GetGroupedExternalData(List<ExternalData> lstEDS)
        {
            List<ExternalData> lstreturn = lstEDS.FindAll(x => x._externaldatatype == _externaldatatype && x._return_format == _return_format);

            switch (_externaldatatype)
            {
                case ExternalDataType.MS_ACCESS:
                    return lstreturn.FindAll(x => x._dictRequest["sql"] == _dictRequest["sql"] && x._dictRequest["connection"] == _dictRequest["connection"]);
                case ExternalDataType.SQL_SERVER:
                    return lstreturn.FindAll(x => x._dictRequest["sql"] == _dictRequest["sql"] && x._dictRequest["connection"] == _dictRequest["connection"]);
                case ExternalDataType.CSV:
                    return lstreturn.FindAll(x => x._dictRequest["file"] == _dictRequest["file"]);
                case ExternalDataType.WEB_SERVICE:
                    return lstreturn.FindAll(x => x._dictRequest["url"] == _dictRequest["url"]);
                default:
                    return lstreturn;
            }

        }

        //SP 29-Mar-2017 - function to modify the date format that will be written out
        public List<string> ConvertDateFormat(List<string> lstLeadColumn)
        {
            List<string> lstNewLeadColumn = new List<string>();

            for (int i = 0; i < lstLeadColumn.Count(); i++)
            {
                string sNewLeadColumnVal = lstLeadColumn[i];
                DateTime dtNewLeadColumnVal;

                //confirm that the lead column value is date
                if (DateTime.TryParse(sNewLeadColumnVal, out dtNewLeadColumnVal)) //SP 29-Mar-2017 TODO may need to consider different culture formats, i.e US, SI
                {
                    lstNewLeadColumn.Add(dtNewLeadColumnVal.ToString(_sTimestampFormat));
                }
                else
                    lstNewLeadColumn.Add(lstLeadColumn[i]);
            }

            return lstNewLeadColumn;
        } 

        #endregion
    }
}
