/*
 * 
 * 
 * Adaptor4HECDSS.exe - This is a common class and used to share with the application
 * 
 * Created by: Leng Dieb 08/Apr/2010
 * 
 * Copyright        2010 by Halcrow
 *                  All rights reserved.
 * 
 * This code is not to be distributed without the written 
 * permission of Halcrow Group Ltd
 * 
 * 
 */

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
using DTTCommons;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Adaptor4HecDSS
{
    public class Commons
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(string LongPath, StringBuilder ShortPath, int BufferSize);

        public const string THIS_ADAPTOR_NAME = "HEC_DSS"; // this is the name of the data adaptor
        public static string TEMP_FOLDER = "";
        public static bool _blnAborted = false; // is abort to false by default
        public static string ADAPTOR_RETURN_XML_FILE = "";
        public static string DSS_UTILITY_EXE = "DSSUTL.EXE"; // dss utility executable file

        /// <summary>
        /// Abort the the running process
        /// </summary>
        /// <returns></returns>
        public static bool ReallyAbortProcess(frmProgress frm)
        {
            if (_blnAborted)
            {
                DialogResult dlg = MessageBox.Show("Do you wish to abort the adaptor run?", "HEC-DSSVue Adaptor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg == DialogResult.Yes)
                {
                    // write abort content to adaptor
                    XElement elementRoot = new XElement("root");
                    XElement elementAbort = new XElement("abort", "true");
                    elementRoot.Add(elementAbort);
                    elementRoot.Save(ADAPTOR_RETURN_XML_FILE); //save back to file on aborting
                    frm.Close(); // close the dialog
                }
                else
                {
                    _blnAborted = false;
                    Application.DoEvents(); // do events
                }
            }
            return _blnAborted;
        }
        /// <summary>
        /// display standard type of window message
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="icon"></param>
        public static void ShowMessage(string strMessage, MessageBoxIcon icon)
        {
            MessageBox.Show(strMessage, "HEC-DSS Adaptor", MessageBoxButtons.OK, icon);
        }
        /// <summary>
        /// display standard type of window message [always exclaimation]
        /// </summary>
        /// <param name="strMessage"></param>
        public static void ShowMessage(string strMessage)
        {
            MessageBox.Show(strMessage, "HEC-DSS Adaptor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        /// <summary>
        /// Is numerical value
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsNumeric(string strValue)
        {
            try
            {
                float fltValue = float.Parse(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Format date to HEC format
        /// </summary>
        /// <param name="datValue"></param>
        /// <returns></returns>
        public static string FormatDATE2HEC(DateTime datValue)
        {
            int intHH = datValue.Hour;
            if (intHH == 0)
            {
                string strMin = datValue.Minute.ToString("00");
                return datValue.ToString("ddMMMyyyy") + " 24" + strMin;
            }
            else
            {
                return datValue.ToString("ddMMMyyyy HHmm");
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
        public static bool ShellDSSUTLExe(string strDSSFile, string strMacroFile)
        {
            string strDSSUTLExe = Application.StartupPath + "\\" + DSS_UTILITY_EXE;
            System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
            bool blnValid = false;
            if (System.IO.File.Exists(strDSSUTLExe) == false)
            {
                strDSSUTLExe = Application.StartupPath + "\\" + new System.IO.FileInfo(strDSSUTLExe).Name;
            }
            if (System.IO.File.Exists(strDSSUTLExe))
            {
                string strCmd = "\"dssfile=" + ToShortPathName(strDSSFile) + "\" \"input=" + ToShortPathName(strMacroFile) + "\"";
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
                Commons.ShowMessage("Cannot find DSS Utility Application '" + strDSSUTLExe + "'", MessageBoxIcon.Exclamation);
            }
            return blnValid; // valid shelling adaptor process
        }
        /// <summary>
        /// Shelling adaptor application (this is used when INPUT="xxx") command
        /// </summary>
        /// <param name="strInputFile">Input file command</param>
        public static bool ShellDSSUTLExe(string strInputFile)
        {
            string strDSSUTLExe = Application.StartupPath + "\\" + DSS_UTILITY_EXE;
            System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
            bool blnValid = false;
            if (System.IO.File.Exists(strDSSUTLExe) == false)
            {
                strDSSUTLExe = Application.StartupPath + "\\" + new System.IO.FileInfo(strDSSUTLExe).Name;
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
                Commons.ShowMessage("Cannot find DSS Utility Application '" + strDSSUTLExe + "'", MessageBoxIcon.Exclamation);
            }
            return blnValid; // valid shelling adaptor process
        }
        /// <summary>
        /// The ToLongPathNameToShortPathName function retrieves the short path form of a specified long input path
        /// </summary>
        /// <param name="longName">The long name path</param>
        /// <returns>A short name path string</returns>
        public static string ToShortPathName(string longName)
        {
            int bufferSize = 256;

            /* don´t allocate stringbuilder here but outside of the function for fast access , believe me, it will speed up your code */

            StringBuilder shortNameBuffer = new StringBuilder(bufferSize);

            int result = GetShortPathName(longName, shortNameBuffer, bufferSize);

            return shortNameBuffer.ToString();
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
    }
}
