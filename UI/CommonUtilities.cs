    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;
 //   using muMathParser;
    using System.Data;
    using System.Runtime.InteropServices;
    using SIM_API_LINKS.DAL;
    using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

    namespace SIM_API_LINKS
    {
       // defined in TimeSeries, which is added as ref here
        /* public enum SimLinkDataType_Major       //met 9/6/2013: added vals that correspond to db stored vals    //sim2: was in TimeSeries
        {
            MEV = 1,
            ResultSummary = 2,
            ResultTS = 3,
            Event = 4,
            Performance = 5,
            Network = 6
        }*/
    
    
        public static class CommonUtilities
        {
            #region SimLink Constants
            //declare Scenario Life Cycle elements here (not in db- RMG would make sense but makes harder to access in "child" classses
            public const int nScenLCExist = 1;
            public const int nScenLCModElementExist = 2;
            public const int nScenLCBaselineFileSetup = 3;
            public const int nScenLCBaselineModified = 4;
            public const int nScenLCModelExecuted = 5;
            public const int nScenLCModelResultsRead = 6;
            public const int nScenLModelResultsTS_Read = 10;      //have option to process TS read
            public const int nScenResultTS_Operations = 12;
            public const int nScenDefineEvents = 15;              //have option for event definion
            public const int nScenEventAggregation = 15;              //have option for event definion
            public const int nScenLCSecondaryProcessing = 20;
            public const int nScenLCTertiaryProcessing = 30;  //groupings of performance variables (cost---> total cost)
            public const int nScenLCQuaternaryProcessing = 40;  //objective function
      //sim2      private muMathParser.Parser m_parser;

            public const int _nBAD_DATA = -99999;
            public const double _dBAD_DATA = -99999.99;
            public const double _dNaN = 0.000006666666;
            public const double _dInfinity = -99999999.876;
            public const string _sBAD_DATA = "-99999.99";
            public const string _sDATA_UNDEFINED = "NOTHING";
            public const int _nDV_ID_Code_LinkedData = -234;            //insert into tbMEV to indicate

            public const int _nEvalID_SL_LITE = 99999;           //placeholder val if no backend db used
            //  vals set in tlkpUIDictionary in SimLink (move to webconfig?)
            public const int _nModelTypeID_SWMM = 1;
            public const int _nModelTypeID_IW = 2;
            public const int _nModelTypeID_MODFLOW = 4;
            public const int _nModelTypeID_SimClim = 8;
            public const int _nModelTypeID_ISIS1D = 7;
            public const int _nModelTypeID_ISIS2D = 9;
            public const int _nModelTypeID_ISIS_FAST = 10;
            public const int _nModelTypeID_ExtendSim = 11;
            public const int _nModelTypeID_EPANET = 3;
            public const int _nModelTypeID_Excel = 12;

            public const int _nSimClim_ResultsCodePrecip = 2;
            public const int _nSimClim_ResultsCodePrecipPerturb = 13;
            #endregion

            #region TEXT_HELPERS
            //given an incoming file name, create a filename that won't screw up windows.
            public static string RMV_FixFilename(string sIncoming)
            {
                return sIncoming.Replace(":", "_").Replace(",", "_").Replace("!", "_").Replace("*", "_").Replace(" ", "_").Replace("/", "_").Replace("#", "_").Replace(" ", "$").Replace("%", "_").Replace("&", "_").Replace("(", "_").Replace(")", "_").Replace("^", "_");
            }

            public static string GetFirstChar(string sbuf, bool bReturnSignChar = true)
            {
                if (sbuf.Length > 0)
                {
                    return sbuf.Trim().Substring(0, 1);
                }
                else
                {
                    return "@";
                }
            }

            //formerly cuRemoveRepeating
            public static string RemoveRepeatingChar(string sbuf, char sCleanChar = ' ')
            {
                bool bPrevIsTargetChar = false;
                string sOut = "";
                foreach (char c in sbuf)
                {
                    if (bPrevIsTargetChar)
                    {
                        if (c == sCleanChar)
                        {
                            // do nothing
                        }
                        else
                        {
                            sOut += c;
                            bPrevIsTargetChar = false;
                        }
                    }
                    else
                    {
                        sOut += c;
                        if (c == sCleanChar)
                        {
                            bPrevIsTargetChar = true;
                        }
                    }
                }
                return sOut;
            }

            public static int CountStringOccurrences(string text, string pattern)
            {
                // Loop through all instances of the string 'text'.
                int count = 0;
                int i = 0;
                while ((i = text.IndexOf(pattern, i)) != -1)
                {
                    i += pattern.Length;
                    count++;
                }
                return count;
            }
            public static bool GetKVP(string sLine, out KeyValuePair<string, string> kvp, string sDelim)
            {
                bool bValid = true;
                if (sLine.IndexOf(sDelim) > 0)
                {
                    string sKey = sLine.Substring(0, sLine.IndexOf("=") - 1).Trim();
                    string sVal = sLine.Substring(sLine.IndexOf("=") + 1).Trim();
                    kvp = new KeyValuePair<string, string>(sKey, sVal);
                }
                else
                {
                    bValid = false;
                    kvp = new KeyValuePair<string, string>("NOTFOUND", "NOTFOUND");
                }
                return bValid;
            }


            #endregion

            #region TEMPLATE_VARS
        

        
        
            #endregion

            //return the default location where we can retrieve data from
            //met added to CU instead of timeseries
            public static TSRepository GetDefaultSimLinkTSFormatByModelType(int nModelTypeID)
            {
                TSRepository tsrepoReturn = TSRepository.NetworkTable;
                if (nModelTypeID == CommonUtilities._nModelTypeID_SimClim)
                {
                    tsrepoReturn = TSRepository.HDF5;
                }
                else if (nModelTypeID == CommonUtilities._nModelTypeID_ISIS2D)
                {
                    tsrepoReturn = TSRepository.XML;
                }

                return tsrepoReturn;

            }

            #region DATATYPES

            //5/15/14- test modified IsDouble that also returns a double val as well
            public static bool IsDouble(string value, out double dVal)
            {
                bool bReturn;
                switch (value)
                {
                    case "NaN":
                        bReturn = false;
                        dVal = CommonUtilities._dNaN;
                        break;
                    case "-Infinity": 
                        bReturn = false;
                        dVal = -1*CommonUtilities._dInfinity;
                        break;
                    case "Infinity":
                        bReturn = false;
                        dVal = CommonUtilities._dInfinity;
                        break;
                    default:
                        bReturn = double.TryParse(value, out dVal);
                        break;
                }

                return bReturn;

            }


            public static bool IsDouble(string value)
            {
                // Return true if this is a number.
                double number1;
                return double.TryParse(value, out number1);
            }

            //public helper for testing for true
            // 0  = false, anything else = treu (Actually muparser returns 1 for true
            public static bool IsTrue_MUParser(string value)
            {
                // Return true if this is a number.
                if (value == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            //public helper for testing for a double
            public static bool IsInteger(string value)
            {
                // Return true if this is a number.
                int number1;
                return int.TryParse(value, out number1);
            }
            public static bool GetInteger(string value, out int nValInt)
            {
                // Return true if this is a number.
                int number1;
                return int.TryParse(value, out nValInt);
            }

            #endregion
            #region FILEMGMT


            //return directory of file, if provided, or else return current directory
            public static string  GetWorkingDirectoryFromFile(string sFile){
                string sCurrentDirectory = Directory.GetCurrentDirectory();
                if (true)
                {
                    sCurrentDirectory = Path.GetDirectoryName(sFile);
                }
                return sCurrentDirectory;
            }

            public static void CopyEntireDirectory(string sourcePath, string targetPath)
            {
                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);
                    string fileName; string destFile;
                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }

            }

            //MET 9/6/2012: Modified to have an option to only use sub-folders with same name
            //pass file name to be copied if true
            //only implemented for filenames at this time; directories would be a major pain, i think, for fsw.
            public static bool CopyDirectoryAndSubfolders(string SourcePath, string DestinationPath, bool overwriteexisting, string sOnlyCopySameName = "FALSE")
            {
                bool ret = false; bool bCopyFiles = true; //default to copying the files regardless of name

                if (sOnlyCopySameName != "FALSE") { bCopyFiles = false; }

                try
                {
                    SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                    DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                    if (Directory.Exists(SourcePath))
                    {
                        if (Directory.Exists(DestinationPath) == false)
                            Directory.CreateDirectory(DestinationPath);

                        foreach (string fls in Directory.GetFiles(SourcePath))
                        {
                            if (bCopyFiles || (Path.GetFileNameWithoutExtension(fls) == sOnlyCopySameName))
                            {
                                FileInfo flinfo = new FileInfo(fls);
                                flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                            }
                        }
                        foreach (string drs in Directory.GetDirectories(SourcePath))
                        {
                            DirectoryInfo drinfo = new DirectoryInfo(drs);

                            if (bCopyFiles || (Path.GetFileNameWithoutExtension(drs) == sOnlyCopySameName))     //skip folder unless same name if requested.
                            {
                                if (CopyDirectoryAndSubfolders(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                                    ret = false;
                            }
                        }
                    }
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                }
                return ret;
            }

            //added 1/29/13 to support CLI
            public static int GetModelTypeIDFromString(string sPlatform)
            {
                int nReturn = -1;
                switch (sPlatform.ToLower())
                {
                    case "swmm":
                        nReturn = 1;
                        break;
                    case "iw_cs":
                        nReturn = 2;
                        break;
                    case "epanet":
                        nReturn = 3;
                        break;
                    case "modflow":
                        nReturn = 4;
                        break;
                    case "isis1d":
                        nReturn = 7;
                        break;
                    case "simclim":
                        nReturn = 8;
                        break;
                    case "isis2d":
                        nReturn = 9;
                        break;
                    case "isisfast":
                        nReturn = 10;
                        break;
                    case "extendsim":
                        nReturn = 11;
                        break;

                }
                return nReturn;
            }

            #region SimLinkNaming
            public static string GetSimLinkDirectory(string sModelLocation, int nScenarioID, int nEvalID, bool bIsScenarioFolder = false)
            {
                string sPathReturn;
                sPathReturn = Path.GetDirectoryName(sModelLocation);           // take off the filename
                sPathReturn = sPathReturn.Substring(0, sPathReturn.LastIndexOf("\\")) + "\\" + nEvalID.ToString();
                if (bIsScenarioFolder)
                {
                    sPathReturn = sPathReturn + "\\" + nScenarioID.ToString();      //add scenario folder if instructed
                }
                return sPathReturn;
            }

            //gets the directory of for a baseline model.
            //for now puts it at same 'level' as uploaded file
            //todo: in server environment, pass a config value 
            public static string GetSimLink_BaseEvalDirectory(string sModelLocation, int nEvalID)
            {
                string sPathReturn;
                sPathReturn = Path.GetDirectoryName(sModelLocation);           // take off the filename
                sPathReturn = sPathReturn.Substring(0, sPathReturn.LastIndexOf("\\")) + "\\Base_" + nEvalID.ToString();
                return sPathReturn;
            }


            //directory for placing log files.
            public static string GetSimLink_LogDirectory(string sModelLocation)
            {
                string sPathReturn;
                sPathReturn = Path.GetDirectoryName(sModelLocation);           // take off the filename
                sPathReturn = sPathReturn.Substring(0, sPathReturn.LastIndexOf("\\")) + "\\LOGS";
                return sPathReturn;
            }


            //met 8/13/2013: rename arg to make clear that this could be named based on EG or SCEN
            public static string GetSimLinkFileName(string sBaseFileName, int nScenarioOrEvalID)
            {
                string sFileNameReturn;
                sFileNameReturn = Path.GetFileNameWithoutExtension(sBaseFileName) + "_" + nScenarioOrEvalID.ToString() + Path.GetExtension(sBaseFileName);
                return sFileNameReturn;
            }


            // get the path of the TS container
            public static string GetSimLinkFull_TS_FilePath(string sBasePath, int nModelTypeID, int nEvalID, int nScenarioID, bool bIsScenarioPath = false)
            {
                string sFileName = GetSimLinkTS_BaseFileName(nModelTypeID);

                if (bIsScenarioPath)                                                                    // allow user to pass \\EVAL\\SCEN\\MODEL_SCEN.INP  type file
                {
                    sFileName = GetSimLinkFileName(sFileName, nScenarioID);
                }
                else
                {
                    sFileName = GetSimLinkFileName(sFileName, nEvalID);
                }

                if (bIsScenarioPath)                                                                    // allow user to pass \\EVAL\\SCEN\\MODEL_SCEN.INP  type file
                {
                 //met 6/1/14: trying out for ts generation; see if works.  was getting wrong dir
                     
                     //   sBasePath = sBasePath.Substring(0, sBasePath.LastIndexOf("\\"));
                  //  sBasePath = sBasePath.Substring(0, sBasePath.LastIndexOf("\\")) + "\\xxx.txt";   
                   // sBasePath = _sa
                    //create a fake "Base_EVAL\\xxx.txt"   (eg base model location)
                }

                sFileName = GetSimLinkDirectory(sBasePath, nScenarioID, nEvalID, bIsScenarioPath) + "\\" + sFileName;
                return sFileName;
            }



            public static string GetSimLink_TS_GroupName(SimLinkDataType_Major slType, string sVarTypeID, string sID = "SKIP", string sScenarioID = "SKIP")
            {
                string sCode = "";
                switch (slType)
                {
                    case SimLinkDataType_Major.Event:
                        sCode = "EV";
                        break;
                    case SimLinkDataType_Major.MEV:
                        sCode = "MV";
                        break;
                    case SimLinkDataType_Major.ResultSummary:
                        sCode = "RS";
                        break;
                    case SimLinkDataType_Major.ResultTS:
                        sCode = "TS";
                        break;
                    case SimLinkDataType_Major.XMODEL:          //met 1/10/13: not sure we need this.
                        sCode = "XMOD";
                        break;
                }

                sCode = sCode + "_" + sVarTypeID;
                if (sID != "SKIP")
                {
                    sCode = sCode + "_" + sID;
                }
                if (sScenarioID != "SKIP")
                {
                    sCode = sScenarioID + "_" + sCode;
                }
                return sCode;
            }


            /// <summary>
            /// Get the base file name used for TS storage for specific model type id
            /// </summary>
            /// <param name="nModelTypeID"></param>
            /// <returns> String that forms the (scenario independent) base file name for the run. </TS></returns>
            ///  <date> 7/2/2013</date>
            private static string GetSimLinkTS_BaseFileName(int nModelTypeID)
            {
                string sReturn = "NOTHING";
                switch (nModelTypeID)
                {
                    case CommonUtilities._nModelTypeID_SWMM:
                        sReturn = "SWMM_TS.H5";
                        break;
                    case CommonUtilities._nModelTypeID_SimClim:
                        sReturn = "SimClimTS.H5";
                        break;
                    case CommonUtilities._nModelTypeID_ISIS1D:
                        sReturn = "ISIS1D_TS.H5";
                        break;
                    case CommonUtilities._nModelTypeID_ISIS2D:
                        sReturn = "ISIS2D_TS.H5";
                        break;
                }
                return sReturn;
            }
            #endregion

            #endregion
            #region FileManipulation
            //function purpose: for use reading text files, which are generally exported in sections. user passes the line, and current section, along with indicator string for section change
            //function returns whether text file has passed into new section
            //built off of original MOUSE_CheckSectionName.... more general function 
            //TODO - add flexibility so not necessraily check first char
            public static string cuFile_CheckCurrentFileSection(string sCurrentLine, string sCurrentSectionName, string sSectionIndicator)
            {
                if (sCurrentLine.Trim() == "")
                {
                    return sCurrentSectionName;
                }
                else if (sCurrentLine.Trim().Substring(0, 1) == sSectionIndicator)
                {
                    return cuFile_Line_CleanChars(sCurrentLine);
                }
                else
                {
                    return sCurrentSectionName;
                }
            }
            //built off of original muCleanString.... more general function 
            //TODO - add flexibility for which sets to pull from
            private static string cuFile_Line_CleanChars(string sbuf)
            {
                char[] nogood = { '[', ']', ';', 'Z', '\t', ' ' };
                return sbuf.TrimStart(nogood).TrimEnd(nogood);
            }

            public static int cuFilePosition_FindString(List<string> sListToSearch, string sCurrentTableName, int nCurrentFilePosition)
            {
                int nStartingPosition = nCurrentFilePosition;
                int nReturnVal = -1;
                while ((nCurrentFilePosition < sListToSearch.Count) && (nReturnVal < 0))
                {
                    if (sListToSearch[nCurrentFilePosition].ToString().IndexOf(sCurrentTableName) >= 0)
                    {      //please don't name your nodes Link Discharge Table
                        nReturnVal = nCurrentFilePosition;
                    }
                    else
                    {
                        nCurrentFilePosition++;
                    }
                }
                if (nReturnVal < 0)
                {
                    return nStartingPosition;
                }
                else
                {
                    return nCurrentFilePosition;
                }
            }
            #endregion
            #region BATCH_PROCESSING
            // runs a batch file supplied by user
            //send optional argument requesting whether to show the window or not
            public static bool RunBatchFile(string sBATFileName, bool bShowWindow = false)
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
            #endregion

    #region ZIPPING
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
                        string s = bUse7Z + " a -t7z " + sZipArchive + " " + sFilesToZipConcat;
                        string[] sOut = new string[] { "cd %~dp0", s };
                        File.WriteAllLines(sTempBat, sOut);
                        RunBatchFile(sTempBat);
                        if (bDeleteFileWhenDone)
                        {
                            File.Delete(sTempBat);
                        }
                    }
                }
                catch (Exception ex)
                {
                    bReturn = false;
               //sim2     cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(),_nLogging_Level_1);
                }
                return bReturn;
            }
    #endregion
            // in process of migrating to  simlink.cs
            #region MathParser
            #region QuickParse

            public static double QuickParse(string sExpression)
            {
                sExpression = Quickparse_ProcessParentheses(sExpression);
                bool bErr = false;
                double dTheVal = Convert.ToDouble(QuickParse_Clean(sExpression, out bErr));                     // no parentheses
                if (bErr)
                {
                    //todo           - log it
                }
                return dTheVal;
            }

            //v1- nested parentheses not supported!!!
            //idea - create an object where you can track info done preivously
            private static string Quickparse_ProcessParentheses(string sExpression)
            {
                bool bERR = false;
                int nPosCounter = 0;
                while (sExpression.IndexOf('(') >= 0)
                {
                    int nLoc1 = sExpression.IndexOf('(', nPosCounter);
                    nPosCounter = sExpression.IndexOf(')', nPosCounter);
                    string sVal = QuickParse_Clean(sExpression.Substring(nLoc1 + 1, nPosCounter - nLoc1 - 1), out bERR);
                    if (bERR)
                    {
                        //todo: log the issue
                        return "-666.66666";
                    }
                    else
                    {
                        string sReplace = sExpression.Substring(nLoc1, nPosCounter - nLoc1 + 1);
                        sExpression = sExpression.Replace(sReplace, sVal);
                    }
                }
                return sExpression;
            }

            private static string SubstituteScientific(string sExpression, out bool bHasScientific)
            {
                bHasScientific = false;
                if (sExpression.IndexOf("E-") > 0)
                {
                    sExpression = sExpression.Replace("E-", "XX");
                    bHasScientific = true;
                }
                return sExpression;
            }

            private static void SubstituteBackScientific(ref string[] sVals)
            {
                for (int i = 0; i < sVals.Length; i++)
                {
                    sVals[i] = sVals[i].Replace("XX", "E-");
                }
            }

            //no parentehss in this expression
            //naive evaluation from left to right
            private static string QuickParse_Clean(string sExpression, out bool bErr)
            {
                char[] sDelimiter = new char[] { '+', '?', '*', '/', '^' };
                try
                {
                    bErr = false; bool bHasScientific = false;
                    sExpression = SubstituteScientific(sExpression, out bHasScientific);                                //need to trick this silly parser.
                    string[] sVals = sExpression.Split(sDelimiter);
                    if (bHasScientific)
                    {
                        SubstituteBackScientific(ref sVals);
                    }


                    int nNumberOfTerms = sVals.Length;
                    if (nNumberOfTerms == 0)
                    {
                        return sExpression;         //no terms: easy
                    }

                    int nCurrentPos = 0;
                    double dTheVal = 0;
                    for (int i = 0; i < nNumberOfTerms - 1; i++)
                    {
                        nCurrentPos = sExpression.IndexOfAny(sDelimiter, nCurrentPos);
                        string sDelimiter1 = sExpression.Substring(nCurrentPos, 1);
                        nCurrentPos += 1;

                        switch (sDelimiter1)
                        {
                            case "+":
                                dTheVal = Convert.ToDouble(sVals[i]) + Convert.ToDouble(sVals[i + 1]);
                                break;
                            case "?":
                                dTheVal = Convert.ToDouble(sVals[i]) - Convert.ToDouble(sVals[i + 1]);
                                break;
                            case "*":
                                dTheVal = Convert.ToDouble(sVals[i]) * Convert.ToDouble(sVals[i + 1]);
                                break;
                            case "/":
                                dTheVal = Convert.ToDouble(sVals[i]) / Convert.ToDouble(sVals[i + 1]);
                                break;
                        }
                        sVals[i + 1] = dTheVal.ToString();     // this will become the next first argument
                    }
                    return sVals[nNumberOfTerms - 1];
                }
                catch (Exception EX)
                {
                    bErr = true;
                    return "-666";
                }
            }


            #endregion

            //todo: error handling, including pass back exception
            public static double cuPARSE_Expression(string sVal)
            {
                //muMathParser.Parser m_parser = new muMathParser.Parser(muMathParser.Parser.EBaseType.tpDOUBLE);
                //muMathParser.ParserVariable m_ans = new ParserVariable(0);
                ////    m_parser = new muMathParser.Parser(muMathParser.Parser.EBaseType.tpDOUBLE);
                //m_parser.SetExpr(sVal);
                //m_ans.Value = m_parser.Eval();

                //return Convert.ToDouble(m_ans.Value);

                return -666;
            }

            //  the following are the delimiters for identifying variables to look up
            //  _tblModelElementVals_                       //what if there are more than one?
            //  !tblResultVar_Details!
            //  @tblResultTS_EventSummary_Detail@
            //  %tblPerformance_Detail%
            //  &Model Database with 'underscore delimited VarType_FK_ElementID
            //  #- use  this following one of the above characters to indicate: compare to baseline ..... not yet supported
            //  ~ - pull from tblConstants (which is loaded into dictionary on rmg.init
            //  $tblResultTS$       performs operation for each value in the TS


            public static string cuPARSE_PrepareExpressionValues(int nScenarioID, int nEvalID, string sExpression,   simlink slREF, string sArgs = "NONE")
            {
                string sEXP = ""; string sCurrentChunk = "";
                int nCurrentIndex = 0;         //index of the string we are looking at
                int nMatch = 0;
                int nReplaceChars = -1; //amount of characters to replaceF
                bool bIsCompareToBaseline = false;
                bool bExit = false;
                string sReturnVal = "";

                Dictionary<string, string> dictDelimiter = new Dictionary<string, string>();
                dictDelimiter.Add("_", "tblModelElementVals"); dictDelimiter.Add("!", "tblResultVar_Details");
                dictDelimiter.Add("@", "tblResultTS_EventSummary_Detail"); dictDelimiter.Add("%", "tblPerformance_Detail");

                char[] sDelimiter = new char[] { '_', '!', '@', '%', '~', '&' };
                char sCurrentDel = '1';

                sExpression = cuParse_ProcessArgumentTuple(sExpression, sArgs);         //switch in the args.

                //TODO: improved error handling
                while (!bExit)
                {
                    nMatch = sExpression.IndexOfAny(sDelimiter, nCurrentIndex);            //get open var ref
                    if (nMatch >= 0)
                    {
                        sCurrentDel = sExpression[nMatch];
                        nCurrentIndex = sExpression.IndexOf(sCurrentDel, nMatch + 1);       //get close var ref    (must be same type
                        sEXP = sExpression.Substring(nMatch + 1, nCurrentIndex - nMatch - 1);
                        sReturnVal = cuPARSE_GetVariableReferenceValue(sEXP, sCurrentDel.ToString(), nScenarioID, ref slREF);
                        int nAddtlChar = sReturnVal.Length - sEXP.Length;
                        sExpression = sExpression.Substring(0, nMatch) + sReturnVal + sExpression.Substring(nCurrentIndex + 1);
                        nCurrentIndex = nCurrentIndex + nAddtlChar - 1;     //adjust index
                    }
                    else
                    {
                        bExit = true;
                    }
                }
                return sExpression;
            }


            // Pass back single value
            public static string cuParse_EvaluateExpression(int nScenarioID, int nEvalID, string sExpression, ref simlink slREF, string sArgs = "NONE", bool bCheckForValidDouble = true, bool bSkipPrepare = false)
            {
                if (!bSkipPrepare)
                {
                    sExpression = cuPARSE_PrepareExpressionValues(nScenarioID, nEvalID, sExpression, slREF, sArgs);     //todo: check if this duplicating the slREF object!!! cannot pass by ref
                }
                string sReturn = Convert.ToString(cuPARSE_Expression(sExpression));
                if (bCheckForValidDouble)
                {
                    if (!IsDouble(sReturn))
                    {
                        //log the issue (once?).
                        sReturn = "-668";
                    }
                }
                return sReturn;
            }



            //creates and stores a TS that 
            //assumption: resultTS,

            //not yet implemented for baseline... 
            public static double[,] cuParseTimeSeriesExpression(int nScenarioID, int nEvalID, string sExpression, ref  simlink slREF)       // , ref hdf5_wrap hdf, ref hdf5_wrap hdfBaseline, string sArgs = "NONE", bool bUseQuickParse = false)
            {
                //sExpression = cuParse_ProcessArgumentTuple(sExpression, sArgs);         //switch in the args.
                //int nTS_Count = Convert.ToInt32(sExpression.Length - sExpression.Replace("$", "").Length) / 2;               //number of indices
                //bool bIsBaseline = false;
                //double[][,] dValsJagged = new double[nTS_Count][,];
                //string[] sItem = new string[nTS_Count];
                //// step 1, get the TS in the order they appear in the expression.
                //int nMatch = 0; int nCurrentIndex = 0; string sGroupName = ""; int nTS_Records;

                //for (int i = 0; i < nTS_Count; i++)
                //{
                //    nMatch = sExpression.IndexOf("$", nCurrentIndex);          //get open var ref
                //    nCurrentIndex = sExpression.IndexOf("$", nMatch + 1);
                //    sItem[i] = sExpression.Substring(nMatch + 1, nCurrentIndex - nMatch - 1);
                //    nCurrentIndex = nCurrentIndex + 1;              // sItem[i].Length - 1;
                //    bIsBaseline = (sItem[i].Substring(0, 1) == "#");                                                  //check for baseine indicator
                //    sGroupName = GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, sItem[i].Replace("$", "").Replace("#", ""));
                //    //now get the array
                //    double[,] dResultVals;
                //    if (bIsBaseline)
                //    {
                //        dResultVals = hdfBaseline.hdfGetDataSeries(sGroupName, "1");        //look up in baseline TS holder
                //    }
                //    else
                //    {
                //        dResultVals = hdf.hdfGetDataSeries(sGroupName, "1");
                //    }
                //    dValsJagged[i] = dResultVals;
                //}
                //int nMinTSRecords;  //
                //cuParseTimeSeriesCheckForMissing(ref dValsJagged, out nTS_Records, nTS_Count, out nMinTSRecords);          //nTS_Records: this is the # of records in the TS

                ////parse the expression ONCE, and then we will fill in the TS info.
                //sExpression = cuPARSE_PrepareExpressionValues(nScenarioID, nEvalID, sExpression, slREF, sArgs);
                //string sExpressionOrig = sExpression;
                //double[,] dReturn = new double[nTS_Records, 1];              //make this this size of Max records- as we SHOULD have data for that size
                //for (int i = 0; i < nMinTSRecords; i++)
                //{                         //only index up to MIN records to avoid error.      WAS : nTS_Records
                //    sExpression = sExpressionOrig;
                //    for (int j = 0; j < nTS_Count; j++)
                //    {
                //        double s = dValsJagged[j][i, 0];
                //        string sReplace = "$" + sItem[j] + "$";                             // was  _sExpression_TS_Delimiter;
                //        sExpression = sExpression.Replace(sReplace, s.ToString());       //add the double val
                //    }
                //    if (!bUseQuickParse)
                //    {
                //        dReturn[i, 0] = Convert.ToDouble(cuParse_EvaluateExpression(nScenarioID, nEvalID, sExpression, ref slREF, "NONE", true, true));         //skip the "prepare vals step, which is lengthy
                //    }
                //    else
                //    {
                //        dReturn[i, 0] = QuickParse(sExpression);
                //    }


                //    //        cuPARSE_Expression(sExpression);;
                //}
                return null;
            }

            //todo: add a check for different sized arrays
            private static void cuParseTimeSeriesCheckForMissing(ref double[][,] dValsJagged, out int nRecords, int nTS_SeriesCount, out int nMinRecords)
            {
                nRecords = 0; int nMaxRecords = 0; int nActiveArrayRecords = 0; nMinRecords = 2000000000;
                int[] nRecordCount = new int[nTS_SeriesCount];
                for (int i = 0; i < nTS_SeriesCount; i++)
                {
                    nRecordCount[i] = dValsJagged[i].GetLength(0);
                    if (nRecordCount[i] > nMaxRecords) { nMaxRecords = nRecordCount[i]; }
                    if (nRecordCount[i] < nMinRecords) { nMinRecords = nRecordCount[i]; }
                }
                for (int i = 0; i < nTS_SeriesCount; i++)
                {
                    if (nRecordCount[i] == 0)
                    {
                        dValsJagged[i] = new double[nMaxRecords, 1];        //create an empty array of zeroes.
                    }
                }
                nRecords = nMaxRecords;
            }

            private const char _sTupleExpressionDelimiter = '?';

            //{arg1,arg2},{$ID1$,$ID2$}  args tuple which allows you to substitute into the function. This enables one to re-use functions
            private static string cuParse_ProcessArgumentTuple(string sExpression, string sArgs)
            {
                if ((sArgs == "NONE") || (sArgs == "0"))
                {
                    return sExpression;
                }
                else
                {
                    sArgs = sArgs.Replace("{", "").Replace("}", "");  //not needed- just for easier visual input.            
                    string[] sTuples = sArgs.Split(_sTupleExpressionDelimiter);
                    string[] sLabels = sTuples[0].Split(',');
                    string[] sVals = sTuples[1].Split(',');
                    for (int i = 0; i < sLabels.Length; i++)
                    {
                        if (sExpression.IndexOf(sLabels[i].Trim()) < 0)
                        {                           //trim the string (spaces not supported)
                            //todo: log that this was not found.
                            Console.WriteLine("tuple label not found: " + sLabels[i]);
                        }
                        else
                        {
                            sExpression = sExpression.Replace(sLabels[i].Trim(), sVals[i]);
                        }
                    }
                    return sExpression;
                }
            }


            //met 11/5/2013
            //very quickly thrown togheter function to get specific value back based upon a value pair ValueCode_ElementID
            private static string GetModelInventoryFieldValue(string sEXP, int nActiveModelTypeID, ref simlink slREF)
            {
                string sSQL_RETURN = "ERROR";
                char sDelimiter = '_';
                string[] sID_ValPair = sEXP.Split(sDelimiter);
                int nReturnFieldID = Convert.ToInt32(sID_ValPair[0]);

                //todo: embed this per supported model platform
                string sSQL = "SELECT tlkpSWMMTableDictionary.TableName, tlkpSWMMFieldDictionary.FieldName,tlkpSWMMTableDictionary.KeyColumn FROM tlkpSWMMFieldDictionary INNER JOIN tlkpSWMMTableDictionary ON tlkpSWMMFieldDictionary.TableName_FK = tlkpSWMMTableDictionary.ID"
                            + " WHERE (((tlkpSWMMFieldDictionary.ID)=" + nReturnFieldID + "))";

                DataSet ds = slREF._dbContext.getDataSetfromSQL(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sSQL_RETURN = "SELECT " + ds.Tables[0].Rows[0]["FieldName"] + " as val FROM " + ds.Tables[0].Rows[0]["TableName"] + " WHERE (( " + ds.Tables[0].Rows[0]["KeyColumn"] + "=" + sID_ValPair[1] + ") AND (ModelVersion = @Scenario))";
                }
                return sSQL_RETURN;
            }

            //takes substring, and goes and gets the necessary values
            private static string cuPARSE_GetVariableReferenceValue(string sEXP, string sCurrentDel, int nScenarioID, ref  simlink slREF)
            {
                string sql = "";
                sEXP = sEXP.Replace(sCurrentDel, "");        //get rid of the delimiter
                bool bIsBaseline = sEXP[0] == '#';            //check for vs baseline

                if (bIsBaseline)
                {
                    sEXP = sEXP.Substring(1);               //remove the identifier
                    nScenarioID = slREF._nActiveBaselineScenarioID;
                }


                int nVarType_FK = -1;
                if (sCurrentDel != "&")
                {                                 //not used in this case
                    nVarType_FK = Convert.ToInt32(sEXP);
                }
                //TODO - this will get more complicated as more instructions are injected. met 4/13/2013 this is simplest case
                //todo: we rmg should store baseline     if (bIsBaseline ) {nScenarioID = rmg.BaselineScenarioID; //also remove the # after this is done

                switch (sCurrentDel)
                {
                    case "_":                       //tblModelElementVals
                        //sql = "select val from tblModelElementVals where ((ScenarioID_FK=@Scenario) AND (TableFieldKey_FK=@VarType))";
                        sql = "select val from tblModElementVals where ((ScenarioID_FK=@Scenario) AND (DV_ID_FK=@VarType) and (TableFieldKey_FK=345))";    //hardcoded bad         //met todo : this is not fully sorted out. 11/7/2013 for Boston
                        break;
                    case "!":                       //tblModelElementVals
                        sql = "select val from tblResultVar_Details where ((ScenarioID_FK=@Scenario) AND (Result_ID_FK=@VarType))";
                        break;
                    case "@":                       //tblModelElementVals
                        sql = "select TotalVal as val from tblResultTS_EventSummary_Detail where ((ScenarioID_FK=@Scenario) AND (ResultTS_ID_FK=@VarType))";
                        break;
                    case "%":                       //tblModelElementVals
                        sql = "select val from tblPerformance_Detail where ((ScenarioID_FK=@Scenario) AND (PerformanceID_FK=@VarType))";
                        break;
                    case "&":                       //tblModelElementVals
                        sql = GetModelInventoryFieldValue(sEXP, 1, ref slREF);         //only works for SWMM rightt now- needs improved.
                        nScenarioID = 7586;  // rmg.nActiveBaselineScenario;          //you want the baseline scenario for this, not the actual val
                        break;
                    case "~":                       //constant
                        sql = "SELECT val, tblConstants.ConstantID FROM tblConstants WHERE (((tblConstants.ConstantID)=@VarType))";
                        break;
                }

                List<DBContext_Parameter> lstParam = new List<DBContext_Parameter>();
            
                lstParam.Add(new DBContext_Parameter("@Scenario", SIM_API_LINKS.DAL.DataTypeSL.INTEGER, nScenarioID));    

                if (sCurrentDel != "~")
                {
                    lstParam.Add(new DBContext_Parameter("@Scenario", SIM_API_LINKS.DAL.DataTypeSL.INTEGER, nScenarioID));
                }
                if (sCurrentDel != "&")
                {
                    lstParam.Add(new DBContext_Parameter("@VarType", SIM_API_LINKS.DAL.DataTypeSL.INTEGER, Convert.ToInt32(sEXP)));
                }
                DataSet ds = null;      // MET simUiintegrate- this is not used in UI class- don't know why was causing error. slREF._dbContext.getDataSetfromSQL(sql, lstParam);
                int nCounter = 0;

                string sReturn = "666";       //default return value
                if (sCurrentDel == "_")
                {
                    sReturn = "0";
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    sReturn = ds.Tables[0].Rows[0]["val"].ToString();
                }

                return sReturn;
            }

            //todo: actually check for well-formed expression and update IsValid field in db to avoid confusion
            //met 4/14/2013 quick placeholder
            public static bool cuParserIsValidExpression(string sFunction)
            {
                return (sFunction.Length >= 4);
            }


            #endregion              
            #region ARGUMENTS
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

            /// <summary>
            /// Get/set clipboard data
            /// </summary>
            public static string ClipboardData
            {
                get
                {
                    IDataObject iData = Clipboard.GetDataObject();
                    if (iData == null) return "";

                    if (iData.GetDataPresent(DataFormats.Text))
                        return (string)iData.GetData(DataFormats.Text);
                    return "";
                }
                set { Clipboard.SetDataObject(value); }
            }
        }
    }
