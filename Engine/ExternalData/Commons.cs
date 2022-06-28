using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Nini.Config;

namespace SIM_API_LINKS
{
    public static class Commons
    {
        public static string DSS_UTILITY_EXE = "DSSUTL.EXE"; // dss utility executable file
        public static string TEMP_FOLDER = @"c:\temp";



        /// <summary>
        /// Means of testing external web provider
        /// </summary>
        /// <param name="sVal"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static ExternalData GetExternalData(string sVal, Dictionary<string,string> dict)
        {
            switch (sVal)
            {
                case "web_provider":
                    web_provider webby = new web_provider(1, 1, 1, dict);
                    return webby;
                    break;
                case "csv":
                    return null;
                    break;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Convert a nini config into a string
        /// </summary>
        /// <param name="config"></param>
        /// <param name="dict"></param>
        public static void GetDictValsFromConfig(IConfig config, Dictionary<string, string> dict)
        {
            foreach (string s in config.GetKeys()){
                string sVal = config.GetString(s);
                if (!dict.ContainsKey(s))
                {
                    dict.Add(s, sVal);
                }
            }
        }

        /// <summary>
        /// Format date to HEC format
        /// </summary>
        /// <param name="datValue"></param>
        /// <returns></returns>
        public static string FormatDATE2HEC(DateTime datValue, bool bDateOnly = false)
        {
            if (bDateOnly)
            {
                return datValue.ToString("ddMMMyyyy");
            }
            else
            {

                int intHH = datValue.Hour;
                if (intHH == 0)
                {
                    //SP 20-Apr-2017 need to subtract a day. i.e Apr-20-2017 00:00 should become Apr-19-2017 24:00
                    string strMin = datValue.Minute.ToString("00");
                    return datValue.AddDays(-1).ToString("ddMMMyyyy") + " 24" + strMin;
                }
                else
                {
                    return datValue.ToString("ddMMMyyyy HHmm");
                }
            }
        }

        public static string GetDateRangeFromDates(DateTime dtStart, DateTime dtEnd)
        {
            string sDateReturn = Commons.FormatDATE2HEC(dtStart) + " " + Commons.FormatDATE2HEC(dtEnd);
            return sDateReturn;
        }

        /// <summary>
        /// Check and create a dss file when it does not exist
        /// </summary>
        /// <param name="strDSSFile"></param>
        public static void CreateNewDSSFile(string strDSSFile, string sActiveDir)
        {
            if (File.Exists(strDSSFile) == false) // create a dss file
            {
                // create this just to make sure that ToShortPathName working
                using (StreamWriter write = new StreamWriter(strDSSFile))
                {
                    write.WriteLine("testing...");
                }
                string strDOSDSSPath = Commons.ToShortPathName(strDSSFile);
                // delete the temp
                File.Delete(strDSSFile); // delete file

                string strCreateNewDSSFileInput = Commons.TEMP_FOLDER + "\\newdss.txt";
                using (StreamWriter writeMacro = new StreamWriter(strCreateNewDSSFileInput))
                {
                    writeMacro.WriteLine(strDOSDSSPath);
                }
                Commons.ShellDSSUTLExe(strCreateNewDSSFileInput, sActiveDir); // create dss file using DSSUTL
            }
        }



        /// <summary>
        /// HEC to DSS format
        /// </summary>
        /// <param name="strDateTimeValue"></param>
        /// <returns></returns>
        public static DateTime FormatHEC2Date(string strDateTimeValue)
        {
            string[] astrDate = strDateTimeValue.Split(' ');
            int intHour = int.Parse(astrDate[1].Substring(0, 2));
            string strHour = (intHour == 24 ? "00" : intHour.ToString("00"));
            string strMin = astrDate[1].Substring(3);

            string strDate = astrDate[0] + " " + strHour + ":" + strMin;
            DateTime datValue = DateTime.ParseExact(strDate, "ddMMMyyyy HH:mm", CultureInfo.InvariantCulture);
            return datValue;
        }

        #region Shell Command
        /// <summary>
        /// Shelling adaptor application 
        /// </summary>
        /// <param name="strMacroFile">Macro file</param>
        /// 
        //met: pass the active dir
        public static bool ShellDSSUTLExe(string strDSSFile, string strMacroFile)
        {
            //SP 11-Sept-2017 IMPORTANT - this change requires the DSSUTL.exe file to be in a path specified in the System Environment variables----
            //string strDSSUTLExe = @"C:\Program Files (x86)\HEC\HEC-DSSVue\DSSUTL.EXE";    //DSS_UTILITY_EXE;             //mod to assume in path var Application.StartupPath + "\\" + DSS_UTILITY_EXE;
            string strDSSUTLExe = "DSSUTL.EXE";
            string strDSSUTLExePath = strDSSUTLExe;
            //--------

            System.IO.Directory.SetCurrentDirectory(Commons.TEMP_FOLDER);
            bool blnValid = false;
            if (System.IO.File.Exists(strDSSUTLExePath) == false)
            {
                strDSSUTLExePath = Directory.GetCurrentDirectory() + "\\" + new System.IO.FileInfo(strDSSUTLExePath).Name;      //look in startup path
            }
            if (System.IO.File.Exists(strDSSUTLExePath) == false)
            {
                strDSSUTLExePath = @"C:\a\dssutl.exe";   //Application.StartupPath + "\\" + new System.IO.FileInfo(strDSSUTLExe).Name;
            }
            if (System.IO.File.Exists(strDSSUTLExePath) == false)
            {
                //check the path environment variables
                string tmp_strDSSUTLExePath = GetFullPath(strDSSUTLExe);
                if (tmp_strDSSUTLExePath != "")
                    strDSSUTLExePath = tmp_strDSSUTLExePath;
            }
            if (System.IO.File.Exists(strDSSUTLExePath) == false)
            {
                //hardcode a known default path for the DSSUTL exe
                strDSSUTLExePath = @"C:\Program Files (x86)\HEC\HEC-DSSVue\DSSUTL.EXE";
            }
            if (System.IO.File.Exists(strDSSUTLExePath))
            {
                string strCmd = "\"dssfile=" + strDSSFile + "\" \"input=" + strMacroFile + "\"";
                Process p = new Process();
                p.StartInfo.Arguments = strCmd;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
           ///     p.StartInfo.WorkingDirectory = new System.IO.FileInfo(Commons.TEMP_FOLDER);   // (strDSSUTLExe).DirectoryName;
                p.StartInfo.FileName = strDSSUTLExePath; // adaptor application
                blnValid = p.Start();  // start running the application
                if (blnValid)
                    p.WaitForExit(); // wait till finish to exit
            }
            else
            {
                //todo: log issue
                Console.WriteLine("cannot find the dssutl. Please ensure the DSSUTL.EXE file exists in a location specified in the path environament variables");
         //       Commons.ShowMessage("Cannot find DSS Utility Application '" + strDSSUTLExe + "'", MessageBoxIcon.Exclamation);
            }
            return blnValid; // valid shelling adaptor process
        }

        //given an incoming file name, create a filename that won't screw up windows.
        public static string RMV_FixFilename(string sIncoming, string sReplaceChar = "_")
        {
            return sIncoming.Replace(":", sReplaceChar).Replace(",", sReplaceChar).Replace("!", sReplaceChar).Replace("*", sReplaceChar).Replace(" ", sReplaceChar).Replace("/", sReplaceChar).Replace("#", sReplaceChar).Replace(" ", "$").Replace("%", sReplaceChar).Replace("&", sReplaceChar).Replace("(", sReplaceChar).Replace(")", sReplaceChar).Replace("^", sReplaceChar);
        }

        /// <summary>
        /// Shelling adaptor application (this is used when INPUT="xxx") command
        /// </summary>
        /// <param name="strInputFile">Input file command</param>
        public static bool ShellDSSUTLExe(string strInputFile)
        {
            string strDSSUTLExe = @"C:\Program Files (x86)\HEC\HEC-DSSVue 2.0.1\DSSUTL.EXE";        // DSS_UTILITY_EXE;  // Application.StartupPath + "\\" + DSS_UTILITY_EXE;
            System.IO.Directory.SetCurrentDirectory(Commons.TEMP_FOLDER);           //Application.StartupPath);
            bool blnValid = false;
            if (System.IO.File.Exists(strDSSUTLExe) == false)
            {
                strDSSUTLExe = new System.IO.FileInfo(strDSSUTLExe).Name;   // Application.StartupPath + "\\" + new System.IO.FileInfo(strDSSUTLExe).Name;
            }
            if (System.IO.File.Exists(strDSSUTLExe))
            {
                string strCmd = "\"INPUT=" + ToShortPathName(strInputFile) + "\"";
                Process p = new Process();
                p.StartInfo.Arguments = strCmd;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = new System.IO.FileInfo(strDSSUTLExe).DirectoryName;
                p.StartInfo.FileName = strDSSUTLExe; // adaptor application
                blnValid = p.Start();  // start running the application
                p.WaitForExit(); // wait till finish to exit
            }
            else
            {
                //todo: log issue
                Console.WriteLine("cannot find the dssutl");
           //     Commons.ShowMessage("Cannot find DSS Utility Application '" + strDSSUTLExe + "'", MessageBoxIcon.Exclamation);
            }
            return blnValid; // valid shelling adaptor process
        }
        /// <summary>
        /// The ToLongPathNameToShortPathName function retrieves the short path form of a specified long input path
        /// </summary>
        /// <param name="longName">The long name path</param>
        /// <returns>A short name path string</returns>
        /// 
        //met do not understand what this is doing; return jusrt filename with ext?
        public static string ToShortPathName(string longName)
        {
            int bufferSize = 256;

            /* don´t allocate stringbuilder here but outside of the function for fast access , believe me, it will speed up your code */

   //         StringBuilder shortNameBuffer = new StringBuilder(bufferSize);

   //         int result = GetShortPathName(longName, shortNameBuffer, bufferSize);

      //      return shortNameBuffer.ToString();
            return Path.GetFileName(longName);
        }
        #endregion Shell Command

        #region Accessing DSS Catalogue
        /// <summary>
        /// Refresh DSS catalogue to make sure it's last updated
        /// </summary>
        /// <param name="strDSSFile">DSS File</param>
        public static void RefreshDSSCatalogue(string strDSSFile)
        {
            string strDSSRefMacro = Commons.TEMP_FOLDER + "\\refdsscat.txt"; // refresh dsc catalogue
            using (StreamWriter writeRef = new StreamWriter(strDSSRefMacro))
            {
                writeRef.WriteLine("CA.N"); // refresh catalog command
                writeRef.WriteLine("FI");  // finish command
            }
            // write refresh
            Commons.ShellDSSUTLExe(strDSSFile, strDSSRefMacro); // executing the refresh catalogue file
        }
        #endregion Accessing DSS Catalogue

        /// <summary>
        /// Remove white spaces and replaced by ';'
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        public static string RemoveWhiteSpaces(string strLine)
        {
            Regex reg = new Regex(@"\s+");
            string out_string = reg.Replace(strLine, " ");
            return out_string;
        }

        //SP 12-Aug-2016 Standardizing Interval Type for HEC-DSS
        public static string FormatInterval2HEC(IntervalType itTimeSeriesIntervalType, int nInterval)
        {
            string sReturn = "";

            int n1Hour_Seconds = Convert.ToInt32(TimeSpan.FromHours(1).TotalSeconds);

            //from trial and error can do any denomination < 60 mins for minutes, but only increments of 1 for lower resolutions
            switch(itTimeSeriesIntervalType)
            {
                case SIM_API_LINKS.IntervalType.Day:
                    sReturn = "1DAY";
                    break;
                case SIM_API_LINKS.IntervalType.Hour:
                    sReturn = "1HOUR";
                    break;
                case SIM_API_LINKS.IntervalType.Minute:
                    sReturn = nInterval + "MIN";
                    break;
                case SIM_API_LINKS.IntervalType.Month:
                    sReturn = "1MON";
                    break;
                case SIM_API_LINKS.IntervalType.Year:
                    sReturn = "1YEAR";
                    break;
                case SIM_API_LINKS.IntervalType.Second:
                    if (nInterval >= Convert.ToInt32(TimeSpan.FromHours(1).TotalSeconds))
                    {
                        if (nInterval == Convert.ToInt32(TimeSpan.FromHours(1).TotalSeconds))
                            sReturn = "1HOUR";
                        else if (nInterval == Convert.ToInt32(TimeSpan.FromDays(1).TotalSeconds))
                            sReturn = "1DAY";
                        else
                            sReturn = "1MIN";
                    }
                    else
                        sReturn = TimeSpan.FromSeconds(nInterval).TotalMinutes + "MIN";
 
                    break;
            }

            return sReturn;
        }


        //SP 11-Sept-2017 Get full path from environment variables
        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return "";
        }

    }
}
