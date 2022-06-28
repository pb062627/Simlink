using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CIRRUS_HTC_NS
{
    public class CommonUtilities
    {
        
        public const string _sDATA_UNDEFINED = "DATA_UNDEFINED";
        // runs a batch file supplied by user
        //send optional argument requesting whether to show the window or not
        //met 3/2/2013: copied from SIM_API_LINKS to avoid having reference to that class
        public static bool cuRunBatchFile(string sBATFileName, bool bShowWindow = false)
        {
            bool bReturn = true;
            string errorMessage; string outputMessage;
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = sBATFileName;
                proc.StartInfo.RedirectStandardError = false;
                proc.StartInfo.RedirectStandardOutput = false;
                proc.StartInfo.UseShellExecute = false;
                if (bShowWindow)
                {
                    proc.StartInfo.CreateNoWindow = false;
                }
                else
                {
                    proc.StartInfo.CreateNoWindow = true;
                }


                proc.Start();

                //      outputMessage = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit
                    (

                    );


                proc.WaitForExit();

                //check to see what the exit code was
                if (proc.ExitCode != 0)
                {
                    //some error occurred
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }






        //met 8/25/2013- added overloaded mehtod that takes string array.... TODO: create single function (array) and update calls
        //todo: make static
        //assumes 7z is intalled on machine executing request
        //more robust API for zipping can be grabbed here: http://www.icsharpcode.net/opensource/sharpziplib/download.aspx
        public static bool cu7ZipFile(string[] sFilesToZip, string sZipArchive, bool bDeleteFileWhenDone = true, bool bUse7Z = true)
        {
            string s7Zexe = "7z";
            if (!bUse7Z) { s7Zexe += "a"; }
            string sTempBat = Path.GetDirectoryName(sZipArchive) + "\\" + "temp_bat_to_zip.bat";
            sZipArchive = sZipArchive;      //this should be path and dir.
            bool bReturn = true;
            try
            {
                string sFilesToZipConcat = "";
                foreach (string sFileName in sFilesToZip)
                {
                    if (File.Exists(sFileName))
                    {
                        if (Path.GetDirectoryName(sFileName) == Path.GetDirectoryName(sZipArchive))
                        {
                            sFilesToZipConcat = sFilesToZipConcat + " " + Path.GetFileName(sFileName);      //add only the filename if in same dir as the tempbat
                        }
                        else
                        {
                            sFilesToZipConcat = sFilesToZipConcat + " " + sFileName;
                        }
                    }
                }
                if (true)        //met - do we need to create archive first? && File.Exists(sZipArchive))
                {
                    string s = s7Zexe + " a -t7z " + sZipArchive + " " + sFilesToZipConcat;
                    string[] sOut = new string[] { "cd %~dp0", s };
                    File.WriteAllLines(sTempBat, sOut);
                    cuRunBatchFile(sTempBat);
                    if (bDeleteFileWhenDone)
                    {
                        File.Delete(sTempBat);
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
              //  cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), nLogging_Level_1);
            }
            return bReturn;
        }

        public static string cuGetUNCFileString(string s)
        {
            if (IsUNC(s)){
                return s;
            }
            else{
                return Path.GetFileName(s);
            }
        }

        public static string cuCreateFileString(string[] sFileNames, bool bSkipUNC = true)
        {
            string sReturn="";
            for (int i = 0; i < sFileNames.Length; i++)
            {
                bool bIsUNC = IsUNC(sFileNames[i]);
                if (!bIsUNC)               //  not quite logical- but will work and is faster    && bIsUNC))
                {
                    sReturn= sReturn + Path.GetFileName(sFileNames[i])+" "; 
                }
                else if (!bSkipUNC){
                    sReturn= sReturn +sFileNames[i]+" ";
                }
            }
            return sReturn;
        }

        public static bool IsUNC(string s){
            bool bReturn = false;
            if (s.Length>2){
                if (s.Substring(0, 2) == @"\\") { bReturn = true; }  
            }
            return bReturn;
        }

        //return true if the file is a fully qualifeid path

        public static bool cuIsFullPath(string sFile)
        {
            bool bReturn = false;
            if (IsUNC(sFile))
            {
                bReturn = true;
            }
            else
            {
                if (sFile.Length > 2)
                {
                    if (sFile.Substring(1, 1) == @":") { bReturn = true; }
                }
            }
            return bReturn;
        }

        #region ARGS            // parser of arguments
        public static Dictionary<string, string> Arguments_ToDict(string[] Args)                          //adapted from: http://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>();       // 
            //  StringDictionary Parameters;
            //  Parameters = new StringDictionary();
            Regex Spliter = new Regex(@"^-{1,2}|^/|=",                          // met replace 8/28/2013 to allow req statement (@"^-{1,2}|^/|=",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);             //orig (@"^-{1,2}|^/|=|:",

            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string Parameter = null;
            string[] Parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string Txt in Args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                Parts = Spliter.Split(Txt, 3);

                switch (Parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[0] =
                                    Remover.Replace(Parts[0], "$1");

                                Parameters.Add(Parameter, Parts[0]);
                            }
                            Parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }
                        Parameter = Parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (Parameter == "req")
                            {
                                Parameters.Add(Parameter, Txt);         //met 8/28/2013: hard-coded crutch due to not understanding regex
                            }
                            else if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }

                        if (Parameter != "req") //met 8/28/2013: hard-coded crutch due to not understanding regex
                        {


                            Parameter = Parts[1];

                            // Remove possible enclosing characters (",')
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[2] = Remover.Replace(Parts[2], "$1");
                                Parameters.Add(Parameter, Parts[2]);
                            }
                        }
                        Parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (Parameter != null)
            {
                if (!Parameters.ContainsKey(Parameter))
                    Parameters.Add(Parameter, "true");
            }

            return Parameters;
        }

        #endregion
    }
}
