using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace SIM_API_LINKS
{
    public class Logging
    {
        #region members
        public System.IO.StreamWriter _swLogging;
        public List<string> _sList_Logging;               //list to add log notes to
        public string _sLogging_FileToWrite;              //name of file to write
        public string _sLogging_Directory;                       //directory for logging
        public bool _bLogging_InLog;                      //tracks whether we are inside of a logging element or not
        public int _nLogging_ActiveLogLevel;                     //currenct scenario log level
        public static int _nLogging_Level_Off = 0;                      //do not log anything
        public static int _nLogging_Level_1 = 1;                        //log scenario process begin / end and exceptions 
        public static int _nLogging_Level_2 = 2;                        //log begin scenario process steps, key scenario steps
        public static int _nLogging_Level_3 = 3;                        //log scenario details (todo)
        public static int _nLogging_Level_Debug = 4;                    //ad hoc debugging support (outside of IDE)

        #endregion
        #region LOGGING
        //contains functions related to logging errors/notifications etc... 

        public void Initialize(string sFileName, int nLogLevel = 1, string sDirectory = @"C:\a\")
        {
            _sList_Logging = new List<string>();
            _sList_Logging.Clear();
            _sLogging_FileToWrite = sFileName;
            _sLogging_Directory = sDirectory;
           _nLogging_ActiveLogLevel = nLogLevel;
            _bLogging_InLog = true;              //active log session is open

            Console.WriteLine(string.Format("Writing log file {0} to directory {1}", _sLogging_FileToWrite, _sLogging_Directory));
        }


        // pass string to write, and an "importance level", passed from calling location
        // for concision from call location, the conditional is contained herein
        public void AddString(string sLogString, int nLogThreshold, bool bAppendDate = true, bool bWriteToConsole=true)      //avoid appending the date in each call
        {

            if (bAppendDate)
            {
                sLogString = sLogString + " " + System.DateTime.Now;
            }
            if (_sList_Logging == null)           //met workaround TODO make sure this is initialized at the right time
            {

            }
            else
            {
                if (_nLogging_ActiveLogLevel >= nLogThreshold)
                {
                    _sList_Logging.Add(sLogString);
                    Debug.WriteLine(sLogString);

                    if (bWriteToConsole)   //bWriteToConsole)
                    {
                        Console.WriteLine(sLogString);
                    }
                }
            }




        }
        //        if (nLogging_ActiveLogLevel >= nLogThreshold){
        //           _sList_Logging.Add(sLogString);
        //      }   


        //allows the output file name to be updated to include scenario name (for ease of reference)
        // this is not possible at outset because we do not yet know scenario and it could fail before then
        public void UpdateFileOutName(string sFileName)
        {
            _sLogging_FileToWrite = sFileName;
        }

        //write the log file
        public void WriteLogFile(bool bDoNotWriteIfNoVals=true)
        {
            try
            {
                if (!System.IO.Directory.Exists(_sLogging_Directory)) { System.IO.Directory.CreateDirectory(_sLogging_Directory); }
                if (bDoNotWriteIfNoVals && _sList_Logging.Count == 0)
                {
                    //do nothing
                }
                else
                {
                    _sLogging_FileToWrite = System.IO.Path.GetFileNameWithoutExtension(_sLogging_FileToWrite) + "_" + RMV_FixFilename(DateTime.Now.ToString()) + ".log";
                    File.WriteAllLines(_sLogging_Directory + "\\" + _sLogging_FileToWrite, _sList_Logging.ToArray());
                }
                _bLogging_InLog = false;              //active log session is open}

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing log file: " + ex.Message.ToString());
            }
        }


        #endregion

        public static string RMV_FixFilename(string sIncoming)
        {
            return sIncoming.Replace(":", "_").Replace(",", "_").Replace("!", "_").Replace("*", "_").Replace(" ", "_").Replace("/", "_").Replace("#", "_").Replace(" ", "$").Replace("%", "_").Replace("&", "_").Replace("(", "_").Replace(")", "_").Replace("^", "_");
        }

    }
}
