using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace SIM_API_LINKS
{
    // way of specifying / overriding the DSS parts
    public class DSS_CustomPart
    {
        public string _sVAl = "UNDEFINED";
        public bool _bReplace = false;

        public DSS_CustomPart(string sVal, bool bReplace)
        {
            _sVAl = sVal;
            _bReplace = bReplace;
        }
    }

    public class externalDSS : ExternalData
    {
        public string _sFilename = "UNDEFINED";
        public dss_specification _dssSpecActive;
        public string _sUnits="CFS";
        public string _sType = "INST-VAL";


        #region INIT
        public externalDSS(int nID, int nSourceType, int nFormat, Dictionary<string, string> dictArgs, int nSQN = 1, int nColumnNumber = 1, string sColumnName = "1", 
            bool bIsInput = false, bool bIsColIDName = false)
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



        #region WRITE

        // added start/end dates which you must know so might as well make available
        // 5/15/17: added bAppendScenLabel to give option of whether this gets put into the fpart.
        public static string[] DSS_GetParts(DataRow dr, int nScenarioID, string sDateDSS_Format, string sIntervalDSS_Format, DateTime dtStart, DateTime dtEnd, string sScenarioLabel = "", Dictionary<string, DSS_CustomPart> dictCustom = null, bool bAppendScenLabel = false)
        {
            string[] sParts = new string[8];
            sParts[0] = SyncToCustomDict(dr["Result_Label"].ToString(), "a", dictCustom);
            sParts[1] = SyncToCustomDict(dr["Element_Label"].ToString(), "b", dictCustom);   //b part
            sParts[2] = SyncToCustomDict(dr["FieldLabel"].ToString(), "c", dictCustom);
            sParts[3] = sDateDSS_Format;                    //HEC interval
            sParts[4] = sIntervalDSS_Format;
            string sLabel = nScenarioID.ToString();
            if (bAppendScenLabel)
                sLabel += " " + sScenarioLabel;
            sParts[5] = SyncToCustomDict(sLabel, "f", dictCustom);
            sParts[6] = dtStart.ToString();
            sParts[7] = dtEnd.ToString();
            return sParts;
        }

        /// <summary>
        /// Checks whether a DSS part is overridden in the dictionary, and then replaces/appends depending....
        /// </summary>
        /// <param name="sPart"></param>
        /// <param name="sDictKey"></param>
        /// <param name="dictCustom"></param>
        /// <returns></returns>
        public static string SyncToCustomDict(string sPart, string sDictKey, Dictionary<string, DSS_CustomPart> dictCustom = null)
        {
            if (dictCustom == null)
            {
                return sPart;
            }
            else if (dictCustom.ContainsKey(sDictKey))
            {
                DSS_CustomPart customPart = dictCustom[sDictKey];
                if (customPart._bReplace)
                {
                    sPart = customPart._sVAl;
                }
                else
                {
                    sPart = sPart + "_" + customPart._sVAl;
                }
                return sPart;
            }
            else
            {
                return sPart;
            }
        }


        public override void WriteData(double[][,] arr2DTimeSeriesToWrite, string sDSS_File = null,
            int[] arrWriteColumns = null, string[] arrsColumnHeader = null, List<string> lstDSS_Spec = null)
        {
            //ensure there is a C:\Temp folder for storage of these files
            System.IO.Directory.CreateDirectory(Commons.TEMP_FOLDER);

            int nRecords = arr2DTimeSeriesToWrite.GetLength(0);
            for (int i = 0; i < nRecords; i++)
            {
                double[,] dVals = arr2DTimeSeriesToWrite[i];
                HelperWriteTextFile(dVals,Commons.TEMP_FOLDER + "\\indss.csv");
                _dssSpecActive = new dss_specification(lstDSS_Spec[i]);     // specification for write
        //        _sDateRange =  Commons.FormatDATE2HEC(_dssSpecActive._dtStartTime) + " " + Commons.FormatDATE2HEC(_dssSpecActive._dtEndTime);
                string sMacroIN = HelperWriteMacro();
                bool b =  Commons.ShellDSSUTLExe(sDSS_File, sMacroIN); // executing the refresh catalogue file
            }
        }

        public static string GetDateRangeFromDates(DateTime dtStart, DateTime dtEnd)
        {
            string sDateReturn = Commons.FormatDATE2HEC(dtStart) + " " + Commons.FormatDATE2HEC(dtEnd);
            return sDateReturn;
        }
        private string HelperWriteMacro(){
            string strMacroIn = Commons.TEMP_FOLDER + "\\inmacro.txt";
            string strCSVFile = Commons.TEMP_FOLDER + "\\indss.csv";
             using (StreamWriter writer = new StreamWriter(strMacroIn))
            {
                //SP 12-Aug-2016 previously this was sDateRange.Substring(0,9) i.e. just taking the date component. Documentation allows format (startdate, starttime, enddate, endtime)
                writer.WriteLine("TI " + _dssSpecActive._sDateTimeInterval.Replace(" ", ","));     //todo: figure get the date piece working better. 
                string strPathname = "/" + _dssSpecActive._sLabel + "/" + _dssSpecActive._sLabelDetail + "/" + _dssSpecActive._sVar + "/" + Commons.FormatDATE2HEC(_dssSpecActive._dtStartTime)            //
                        + "/" +_dssSpecActive._sTS_Interval  + "/" + _dssSpecActive._sCode  + "/";      //todo: same date fix
      //          writer.WriteLine("TI " + Commons.FormatDATE2HEC(output.StartDate) + " " + Commons.FormatDATE2HEC(output.EndDate));
                writer.WriteLine("EV A=" + strPathname + " UNITS=" + _sUnits + " TYPE=" + _sType);
                writer.WriteLine("EF.M -9999"); // defining a missing value
      //no skip          writer.WriteLine("EF.H SKIP 1");
                writer.WriteLine("EF [A]");     //rread in first line
                writer.WriteLine("IMP " + Commons.ToShortPathName(strCSVFile));
                writer.WriteLine("CL");
                writer.WriteLine("FI");
            }
           return strMacroIn;
        }

        // writes the test file that DSSUTL will use
        private void HelperWriteTextFile(double[,] dVals, string sFile)
        {
            using (StreamWriter writer = new StreamWriter(sFile))
            {
                // write each row data
                for (int i=0;i<dVals.Length;i++)
                    writer.WriteLine(dVals[i,0]);
                }
            }
        }
    }
    /// <summary>
    /// Utility class that  defines a requested value to push/pull from DSS
    /// built to support USACE level file generation 1/27/16
    /// </summary>
    public class dss_specification
    {
        public string _sLabel;
        public string _sLabelDetail;
        public string _sVar;
        public DateTime _dtStartTime;
        public DateTime _dtEndTime;
        public string _sCode;
        public string _sTS_Interval;
        public string _sOperation;          //very simple operational editor for performing conversions.
        public string _sDateTimeInterval;

        public dss_specification(string sSpecifyRecordByDSS_Record)
        {
            try
            {
                string[] sVals = sSpecifyRecordByDSS_Record.Split(',');
                _sLabel = sVals[0];
                _sLabelDetail = sVals[1];
                _sVar = sVals[2];
                _dtStartTime = Convert.ToDateTime(sVals[6]);
                _dtEndTime = Convert.ToDateTime(sVals[7]);
                _sDateTimeInterval = sVals[3];
                _sTS_Interval = sVals[4];
                _sCode = sVals[5];


     //           if (sVals.Length > 7)
     //               _sOperation = sVals[8];     //untested
            }
            catch (Exception ex)
            {
                _sCode = "Error reading in spec from file: " + ex.Message;
            }
        }

        /// <summary>
        /// Reads a text file and creates dss specification objects from it
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public static List<dss_specification> ReadDSS_Specification(string sFileName)
        {
            List<dss_specification> lstReturn = new List<dss_specification>();
            using (StreamReader reader = new StreamReader(sFileName))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string sBuf = reader.ReadLine();
                    dss_specification spec = new dss_specification(sBuf);
                    lstReturn.Add(spec);
                }
            }
            return lstReturn;
        }
    }

        #endregion