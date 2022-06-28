using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using SIM_API_LINKS;


namespace dss_wrap
{

    /// <summary>
    /// Utility class that  defines a requested value to push/pull from DSS
    /// built to support USACE level file generation 1/27/16
    /// </summary>
    public class dss_specification
    {
        public string _sLabel;
        public string _sVar;
        public DateTime _dtStartTime;
        public DateTime _dtEndTime;
        public string _sCode;
        public string _sTS_Interval;
        public string _sOperation;          //very simple operational editor for performing conversions.


        public dss_specification(string sSpecifyRecordByDSS_Record)
        {
            try
            {
                string[] sVals = sSpecifyRecordByDSS_Record.Split(',');
                _sLabel = sVals[0];
                _sVar = sVals[1];
                _dtStartTime = Convert.ToDateTime(sVals[2]);
                _dtEndTime = Convert.ToDateTime(sVals[3]);
                _sTS_Interval = sVals[4];
                _sCode = sVals[5];
                if(sVals.Length>5)
                    _sOperation = sVals[6];
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
                while (!reader.EndOfStream) {
                    string sBuf = reader.ReadLine();
                    dss_specification spec = new dss_specification(sBuf);
                    lstReturn.Add(spec);
                }
            }
            return lstReturn;
        }
    }


    public static class dssutl_wrap
    {
      
        /// <summary>
        /// Convert the common format to DSS using the DSSUTL
        /// We need to initially transform to CSV file first and then using DSSUTL to write to dss
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        /// 
        //met 6/2/14: TransformCFXML2DSS adjusted for SimLink DG

        //SP 12-Aug-2016 changed sType = "INST-VAl" instead of "???" - TODO confirm what this change means
        public static bool WriteTS_ToDSS(string strDSSFile, List<SIM_API_LINKS.TimeSeries> lstTS, string[] sParts, string sDateRange, string sUnits="CFS", string sType = "INST-VAL")  //DTTCommandTSInputOuput output)
        {
           string strCSVFile = Commons.TEMP_FOLDER + "\\indss.csv";
            using (StreamWriter writer = new StreamWriter(strCSVFile))
            {
                // write each row data
                foreach (SIM_API_LINKS.TimeSeries ts in lstTS)
                {
                    writer.WriteLine(ts._dblValue.ToString());
                }
            }
            // now prepare macro file to import the CSV file
            string strMacroIn = Commons.TEMP_FOLDER + "\\inmacro.txt";
            using (StreamWriter writer = new StreamWriter(strMacroIn))
            {
                //SP 12-Aug-2016 previously this was sDateRange.Substring(0,9) i.e. just taking the date component. Documentation allows format (startdate, starttime, enddate, endtime)
                writer.WriteLine("TI " + sDateRange.Replace(" ", ","));     //todo: figure get the date piece working better. 
                string strPathname = "/" + sParts[0] + "/" + sParts[1] + "/" + sParts[2] 
                                    + "/" + sParts[3].Substring(0,9)  + "/" + sParts[4]  + "/" + sParts[5]  + "/";      //todo: same date fix
      //          writer.WriteLine("TI " + Commons.FormatDATE2HEC(output.StartDate) + " " + Commons.FormatDATE2HEC(output.EndDate));
                writer.WriteLine("EV A=" + strPathname + " UNITS=" + sUnits + " TYPE=" + sType);
                writer.WriteLine("EF.M -9999"); // defining a missing value
      //no skip          writer.WriteLine("EF.H SKIP 1");
                writer.WriteLine("EF [A]");     //rread in first line
                writer.WriteLine("IMP " + Commons.ToShortPathName(strCSVFile));
                writer.WriteLine("CL");
                writer.WriteLine("FI");
            }
          // return false;
           return Commons.ShellDSSUTLExe(strDSSFile, strMacroIn); // executing the refresh catalogue file
        }
        public static string GetDateRangeFromDates(DateTime dtStart, DateTime dtEnd)
        {
            string sDateReturn = Commons.FormatDATE2HEC(dtStart) + " " + Commons.FormatDATE2HEC(dtEnd);
            return sDateReturn;
        }

        /// <summary>
        /// todo: move this to dssutl in 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string[] GetSPartsFromString(string sAPart, string sBPart, string sCPart, DateTime dtStart, string sEPart, string sFPart)
        {
            string sDPart = Commons.FormatDATE2HEC(dtStart);
            
            string[] sReturn = new string[] { sAPart, sBPart, sCPart, sDPart, sEPart, sFPart };
            return sReturn;
        }


        /// <summary>
        /// Utility function to extract a timeseries from the DSS to a text file
        /// </summary>
        /// <param name="sDSSFile"></param>
        /// <param name="sRecordID"></param>
        /// <param name="sFileOut"></param>
        /// <param name="dtStartTime"></param>
        /// <param name="dtEndtime"></param>
        /// <returns></returns>
        public static bool ExtractFromDSS(string sDSSFile, string sRecordID, string sFileOut, DateTime dtStartTime, DateTime dtEndtime, string sWorkingDir="SKIP")
        {
            if (sWorkingDir == "SKIP")
            {
                sWorkingDir = Commons.TEMP_FOLDER;
            }
            string strMacroIn = sWorkingDir + "\\inmacro.txt";
            using (StreamWriter writer = new StreamWriter(strMacroIn))
            {
                writer.WriteLine("EF.H Timestamp, val");
                writer.WriteLine("EV DATA=" + sRecordID);
                string sTime = "TIME " + Commons.FormatDATE2HEC(dtStartTime, true) + " " + dtStartTime.ToShortTimeString().Substring(0, 5);
                sTime += " " + Commons.FormatDATE2HEC(dtEndtime, true) + " " + dtEndtime.ToShortTimeString().Substring(0, 5);
                writer.WriteLine(sTime);            
                writer.WriteLine("EF [DATE:DDMMMYYYY] [TIME:HH:MM],[data]");
                writer.WriteLine("EXP " + sFileOut);
            }
            // return false;
             return Commons.ShellDSSUTLExe(sDSSFile, strMacroIn);       //execute the macro
        }
    }
}
