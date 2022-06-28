using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SIM_API_LINKS
{
    public  class template_link
    {
        #region MEMBERS
        private  string _sInjection = "inj!";
        private char _charPadding = ' ';
        private string _sExtTemplate = "tpl";                //todo: verify that user doesn't set these equal to one another
        private string _sExtInstructionFile = "tpli";
        
        private bool _bTemplateHasErrors = false;           //not yet implemented
        private bool _bInstructionHasErrors = false;        //not yet implemented
        #endregion


        #region INIT and SET
        public template_link(string sTempl, string sInstr)
        {
            _sExtTemplate = sTempl;
            _sExtInstructionFile = sInstr;
        }
        public template_link()
        {
        }

        //dangerous- need to perform some validation of the delimiter
        public void set_injection_delimiter(string sInjectionDelimiter){
            _sInjection = sInjectionDelimiter;
        }
        public void set_padding_char(char charPadding)
        {
            _charPadding = charPadding;
        }

        #endregion

        #region TEMPLATE

        //overloaded function allows user to call from COM (eg Excel)

        /*

        public  bool TEMPLATE_ProcessFile(string sTemplateFile, string[,] sNewVals, string sArchive = "SKIP")
        {
            Dictionary<string, string> dictNewVals = new Dictionary<string, string>();
            dictNewVals = ArrayToDictionary(sNewVals);
            bool bReturn = TEMPLATE_ProcessFile(sTemplateFile, dictNewVals);
            return bReturn;
        }




         * 
         * public  void TEMPLATE_ProcessTemplateFiles(string sTemplateDirectory, string[,] sNewVals, string sArchive = "SKIP")
        {
            Dictionary<string, string> dictNewVals = new Dictionary<string, string>();
            dictNewVals = ArrayToDictionary(sNewVals);
            TEMPLATE_ProcessTemplateFiles(sTemplateDirectory, dictNewVals);


        }*/

        //looks for files ending in .tpl 
        //dict new vals is passed and should include all DV that need written to text file
        public  void TEMPLATE_ProcessTemplateFiles(string sTemplateDirectory, Dictionary<string, string> dictNewVals, ref Logging log, string sArchive = "SKIP")
        {
            //sim2.1 CommonUtilitiesLog..cuLOGGING_Init("TMPL", _nLogLevelOnCU, sTemplateDirectory + "\\LOGS");
            string[] fileEntries = Directory.GetFiles(sTemplateDirectory);
            foreach (string fileName in fileEntries)
            {
                log.AddString("Template process file : " + fileName, Logging._nLogging_Level_Debug);
                if (IsTemplateFile(Path.GetExtension(fileName)))       //template file
                {
                    log.AddString("Template process file : " + fileName, Logging._nLogging_Level_3);
                    bool bWorked = TEMPLATE_ProcessFile(fileName, dictNewVals, ref log, sArchive);
                }
            }
            //sim2.1 CommonUtilitiesLog..cuLogging_AddString("End template file processing of folder: " + sTemplateDirectory, Logging._nLogging_Level_2);
            //sim2.1 CommonUtilitiesLog..cuLOGGING_WriteLogFile();
        }

        //function attempts to return most probable case(s) first to avoid string processing on non-template files
        //this is callled for all files in a directory. not a huge hit... but avoid time overhead
        private bool IsTemplateFile(string sExtension)
        {
            if (sExtension == _sExtTemplate)
            {
                return true;
            }
            else if (sExtension == _sExtInstructionFile){       //temp: tpl  inst tpli,, so you can't just check for contains tpl. rule: you can't reverse these
                return false;
            }
            else if(sExtension.Contains(_sExtInstructionFile)){
                return false;
            }
            else if(sExtension.Contains(_sExtTemplate)){
                return true;
            }
            else{
                return false;
            }

        }

        public  Dictionary<string, string> ArrayToDictionary(string[,] sArrayVals)
        {
            Dictionary<string, string> dictReturn = new Dictionary<string, string>();
            for (int i = 0; i < sArrayVals.GetLength(0); i++)
            {
                if (!dictReturn.ContainsKey(sArrayVals[i, 0]))
                {
                    dictReturn.Add(sArrayVals[i, 0], sArrayVals[i, 1]);
                }
            }
            return dictReturn;
        }

        private  string TEMPLATE_ReadFirstLine(string sFirstLine, out string sExtension, out bool bIsValid, out string sErrorMsg)
        {
            sErrorMsg = ""; bIsValid = true; string sDelimiter = ""; sExtension = "";
            string[] s = sFirstLine.Split(',');
            if (s.Length == 2)
            {
                sDelimiter = s[0].Trim().Substring(0, 1);
                sExtension = s[1].Trim();
                //sim2.1 CommonUtilitiesLog..cuLogging_AddString("Delimiter/Extension: " + sDelimiter + ", " + sExtension, Logging._nLogging_Level_3);
            }
            else
            {
                bIsValid = false;
                sErrorMsg = "Error processing first line of template file. TPL file must be of format <delimiter>,<extension>, e.g. ~,txt";
                //sim2.1 CommonUtilitiesLog..cuLogging_AddString(sErrorMsg, Logging._nLogging_Level_1);
            }
            return sDelimiter;
        }


        // called to process a template file
        //return true if everything works.
        public  bool TEMPLATE_ProcessFile(string sTemplateFile, Dictionary<string, string> dictNewVals, ref Logging log, string sArchive = "SKIP")
        {
            string sFileName = Path.GetFileName(sTemplateFile);
            string sFileModify = Path.GetDirectoryName(sTemplateFile) + "\\" + Path.GetFileNameWithoutExtension(sFileName);
            bool bReturn = false; bool bIsValid = true;
            bool bFirstLine = true;
            string sDelimiter = "~"; string sbuf = ""; string sbufEdit = ""; string sItemKey = ""; string sVal = ""; string sErrorMessage = ""; string sExtension = "";

            //sim2.1 CommonUtilitiesLog..cuLogging_AddString("begin process template file: " + sTemplateFile, Logging._nLogging_Level_3);

            if (true)                   //  this condition was only relevant for the initial version of the reader.     File.Exists(sTemplateFile) && File.Exists(sFileModify))
            {
                StreamReader fileTemplate = null; StreamReader fileToWrite = null;
                try
                {
                    fileTemplate = new StreamReader(sTemplateFile);
                    string[] sFileWriting = File.ReadAllLines(sTemplateFile);
                    int i = 1; int nCurrentBufPos = 0; int nPrecFromTemplate = 0;                   //met 4/17/2013: we do an sbuf read outside the loop.
                    sbuf = fileTemplate.ReadLine();
                    sDelimiter = TEMPLATE_ReadFirstLine(sbuf, out sExtension, out bIsValid, out sErrorMessage);
                    sFileModify = sFileModify + "." + sExtension;
                    sbuf.Trim().Substring(sbuf.Trim().Length - 1, 1); // get the delimiter (single char)
                    while (!fileTemplate.EndOfStream && bIsValid)                       //check only TEMPLATE? these should be identical. catch error?
                    {
                        sbuf = fileTemplate.ReadLine();
                        nCurrentBufPos = 0;
                        if (sbuf.IndexOf(sDelimiter) >= 0)       //there is AT LEAST one
                        {
                            int nReplace = (sbuf.Split(Convert.ToChar(sDelimiter)).Count() - 1) / 2;      //get a fucking count function, C#!!   EACH template replace has two instances of the char.

                            for (int j = 0; j < nReplace; j++)
                            {
                                int nBeginDelim = sbuf.IndexOf(sDelimiter, nCurrentBufPos + j * 2);
                                int nEndDelim = sbuf.IndexOf(sDelimiter, nBeginDelim + 1);
                                nPrecFromTemplate = nEndDelim - nBeginDelim + 1;                            //number of characters between the delimiters + delimeters (enabling complete replace)

                                nCurrentBufPos = sbuf.IndexOf(sDelimiter, nCurrentBufPos + j * 2) - j * 2;      // this is the location we will write to in template file
                                sItemKey = sbuf.Substring(sbuf.IndexOf(sDelimiter, nCurrentBufPos + j * 2) + 1, nPrecFromTemplate - 2).Trim();

                                //        cuLogging_WriteString("ItemKey: " + sItemKey);
                                if (dictNewVals.ContainsKey(sItemKey)) // test whether the template contains a value
                                {
                                    sVal = dictNewVals[sItemKey];
                                }
                                else
                                {
                                    sVal = "-666";          //value was not found
                                }

                                if (TEMPLATE_ContainsInjectString(sItemKey))                  //test whether the dictionary key contains the special characters indicating do a string injection
                                {
                                    //sim2.1 CommonUtilitiesLog..cuLogging_AddString("Processing template replacement string: " + sItemKey, Logging._nLogging_Level_Debug);
                                    sFileWriting[i] = TEMPLATE_InjectString(sFileWriting[i], sVal, nCurrentBufPos, sItemKey);
                                }
                                else
                                {

                                    sVal = TEMPLATE_ConvertValPrecision(sVal, nPrecFromTemplate);
                                    sFileWriting[i] = TEMPLATE_ReplaceValInString(sFileWriting[i], sVal, nCurrentBufPos, nPrecFromTemplate);     //write the new template val to the string
                                    //         cuLogging_WriteString("RevisedLine: " + sFileWriting[i]);
                                }


                                nCurrentBufPos += nPrecFromTemplate;      //advance through the file
                            }
                        }
                        i = i + 1;
                    }

                    List<string> l = new List<string>();
                    l = sFileWriting.ToList();
                    l.RemoveAt(0);      //rmv delimiter
                    sFileWriting = l.ToArray();

                    File.WriteAllLines(sFileModify, sFileWriting);


                    bReturn = true;
                    if (sArchive != "SKIP")      //update the archive
                    {
                  //sim2   figure this out      CommonUtilities.cu7ZipFile(sFileModify, sArchive, true);
                    }

                }
                catch (Exception ex)
                {
                   log.AddString("Exception processing template file  : " + sTemplateFile + " ex:" + ex.Message,Logging._nLogging_Level_3);
                    //sim2.1 CommonUtilitiesLog..cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                }
                finally
                {
                    if (fileTemplate != null)
                        fileTemplate.Close();

                    //     File.Delete(sTemplateFile);           //delete orig file
                }

            }
            return bReturn;
        }

        private  bool TEMPLATE_ContainsInjectString(string sKeyItem)
        {
            return (sKeyItem.IndexOf(_sInjection) >= 0);
        }


        //met 4/16/2013: updated to check for .tpl instead of doing all files
        public  string TEMPLATE_CheckTemplate(string sTemplateDirectory)
        {
            string sReturnSummary = "****************    Template File Check    ****************** " + System.Environment.NewLine;
            string[] fileEntries = Directory.GetFiles(sTemplateDirectory);
            foreach (string fileName in fileEntries)
            {
                if (fileName.Substring(fileName.Length - 4, 4) == ".tpl")       //template file
                {

                    //todo: consider breaking out into subfunction
                    int nTemplateStringCounter = 0;
                    int nInjectionCounter = 0; int nLoops = 0;
                    string[] sLines = File.ReadAllLines(fileName);
                    string sDelimiter = sLines[0].Trim().Substring(sLines[0].Trim().Length - 1, 1);

                    foreach (string sBuf in sLines)
                    {
                        if (nLoops > 0)
                        {
                            nTemplateStringCounter += CommonUtilities.CountStringOccurrences(sBuf, sDelimiter);
                            nInjectionCounter += CommonUtilities.CountStringOccurrences(sBuf, _sInjection);
                        }
                        nLoops++;
                    }

                    sReturnSummary += System.IO.Path.GetFileName(fileName) + ": Template Vars:" + (nTemplateStringCounter) / 2 + ",including Injection Strings: " + nInjectionCounter + System.Environment.NewLine;
                }
            }
            if (fileEntries.Length == 0) { sReturnSummary += "No files in template folder; this could be because .bat file is in wrong location:"; }
            return sReturnSummary;
        }

        private  string TEMPLATE_ConvertValPrecision(string sVal, int nPrecision)
        {
            string sValReturn = ""; int nIndexE = 0;
            bool bIsNumeric = CommonUtilities.IsDouble(sVal);               //sim2 changed from isnumber

            nIndexE = sVal.ToUpper().IndexOf('E');
            if ((nIndexE > 0) && bIsNumeric)
            {     //test for scientific notation or size issue that requires scientific notation;  TODO: test/handle if sVal is not numeric
                sValReturn = sVal.Substring(0, nIndexE).PadRight(nPrecision - nIndexE, '0');         //Pad the portion left of the "E"   TODO: handle case where this is too big and needs truncated?
                sValReturn = sValReturn + sVal.Substring(nIndexE + 1, sVal.Length - nIndexE);         //Add the E portion

            }
            else
            {                  //non scientific, IsNumber type of sVal
                if (sVal.Length > nPrecision)
                {
                    sValReturn = sVal.Substring(0, nPrecision);
                }
                else
                {
                    if (_charPadding == ' ')
                    {
                        sValReturn = sVal.PadRight(nPrecision, _charPadding);                //met 9/24/2014 simply pad the spaces to the right.
                    }
                    else
                    {
                        if (sVal.IndexOf(",")<0){
                            sVal = sVal + ".";          //add a decimal point to void multiplying
                        }
                    }
                    
                    /*    if (bIsNumeric)
                        {
                       
                            sValReturn = sVal.PadRight(nPrecision, '0');         //add the necessary zeroes
                        }
                        else
                        {
                            sValReturn = sVal.PadRight(nPrecision, ' ');         //add the spaces
                        }   */

                }
            }


            return sValReturn;
        }

        //added functionality such that
        //bInject : if this is true,

        private  string TEMPLATE_ReplaceValInString(string sbuf, string sVal, int nCurrentBufPos, int nPrecision, bool bInject = false, string sEnd = "-1")      //precision is a misnomer; its really just the length of the new val
        {
            string sValReturn = ""; int nIndexE = 0;

            sValReturn = sbuf.Substring(0, nCurrentBufPos) + sVal;      //first part of inject (broken into two lines for code clarity)
            sValReturn = sValReturn + sbuf.Substring(nCurrentBufPos + nPrecision, sbuf.Length - (nCurrentBufPos + nPrecision));     // this approach drops the two delimiters
            return sValReturn;
        }

        // TODO: if we have to loop through this a lot, this should perhaps be done a single time at the beginning of each iteration. for now, don't think this is too bad
        // TODO: if there is a regular template replace FOLLOWING an injection string, you have to manage/update the buffer position- it can' tbe calculated from template string alone   

        private  string TEMPLATE_InjectString(string sbuf, string sVal, int nCurrentBufPos, string sItemKey)
        {
            int nReplaceUpTo = -1;

            string sValReturn = sbuf.Substring(0, nCurrentBufPos) + sVal;           //prepare the string
            if (sItemKey.LastIndexOf("!") == (sItemKey.Length - 1))
            {        //there is no char after the "!"--> this means replace to end of string
                // no need to do anything if there is no character replace                                  //nReplaceUpTo = sItemKey.Length;
            }
            else
            {                                                   //replace until this character is found
                string sEnd = "";
                if (sItemKey.IndexOf("zz", sItemKey.LastIndexOf("!")) > 0)
                {          //test for "zz" which is used in place of a space
                    sEnd = " ";
                }
                else
                {
                    sEnd = sItemKey.Substring(sItemKey.IndexOf("!") + 1, sItemKey.Length - sItemKey.IndexOf("!") - 1);     //get the string that identifies where to stop the inject
                }
                nReplaceUpTo = sbuf.IndexOf(sEnd, nCurrentBufPos);
                if (nReplaceUpTo == -1) { nReplaceUpTo = sItemKey.Length; }      //the ending delimiter was not found; replace to the end of the string
                //    else
                //   {
                //       nReplaceUpTo += nCurrentBufPos;                             //set ending postition to overall string position
                //   }
                sValReturn = sValReturn + sbuf.Substring(nReplaceUpTo, sbuf.Length - (nReplaceUpTo));
            }


            return sValReturn;
        }

        #region InstructionFIle

        public  List<string[,]> InstructionFile_ProcessFolder(string sTemplateDirectory, string sArchive = "SKIP")
        {
            //sim2.1 CommonUtilitiesLog..cuLOGGING_Init("TMPL", _nLogLevelOnCU, sTemplateDirectory + "\\LOGS");
            string[] fileEntries = Directory.GetFiles(sTemplateDirectory);
            List<string[,]> lstResults = new List<string[,]>();
            foreach (string fileName in fileEntries)
            {
                if (fileName.Substring(fileName.Length - 5, 5) == ".tpli")       //template file
                {
                    string[,] sResults = Instruction_ProcessFile(fileName, sArchive);
                    lstResults.Add(sResults);
                }
            }
            //sim2.1 CommonUtilitiesLog..cuLogging_AddString("End template file processing of folder: " + sTemplateDirectory, Logging._nLogging_Level_2);
            //sim2.1 CommonUtilitiesLog..cuLOGGING_WriteLogFile();
            return lstResults;
        }

        //1: First line holds the delimiter, and the filename and/or extension (extension can be used if filename not easily known and ONLY one file of that type in the folder)
        //2:


        private  string[,] Instruction_ProcessFile(string sFileName, string sArchive)
        {
            Dictionary<string, string> dctKeyVals = new Dictionary<string, string>();
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            string sTargetFile; string sBUF_INS = ""; string sBUF_Target = ""; bool bIsValid = true; bool bIsTargetEOF = false;
            string sTargetBuffer = ""; int nTargetBufferIndex = 0;

            using (StreamReader fileINS = new StreamReader(sFileName))
            {
                sBUF_INS = fileINS.ReadLine();
                string sDelimiter = Instruction_ProcessFirstLine(sBUF_INS, sFileName, out sTargetFile, out bIsValid);
                StreamReader fileTarget = new StreamReader(sTargetFile);

                while (!fileINS.EndOfStream)
                {
                    sBUF_INS = fileINS.ReadLine();
                    Instruction_ProcessLine(sBUF_INS, ref fileTarget, ref dctKeyVals, sDelimiter, ref sTargetBuffer, ref nTargetBufferIndex, out  bIsTargetEOF);
                }

                fileTarget.Close();
                fileTarget.Dispose();
            }
            string[,] s = DictToArray(dctKeyVals);
            return s;


        }

        private  string[,] DictToArray(Dictionary<string, string> dict)
        {
            string[,] s = new string[dict.Count, 2];
            int i = 0;
            foreach (KeyValuePair<string, string> pair in dict)
            {
                s[i, 0] = pair.Key;
                s[i, 1] = pair.Value;
                i++;
            }
            return s;

        }

        private  void Instruction_ProcessLine(string sBUF_INS, ref StreamReader fileTarget, ref Dictionary<string, string> dictVals, string sDelimiter, ref string sTargetBuffer, ref int nTargetBufferIndex, out bool bIsTargetEOF)
        {
            bool bIsEOF = false;
            while (sBUF_INS.Trim().Length > 0)
            {
                sBUF_INS = sBUF_INS.Trim();
                string sInstruction = Instruction_GetNextInstruction(ref sBUF_INS, sDelimiter);
                Instruction_Process_Instruction(sInstruction, ref fileTarget, ref dictVals, ref sTargetBuffer, ref nTargetBufferIndex, sDelimiter);
            }

            bIsTargetEOF = true;    //not used yet.
        }


        // takes the instruction and processes it based on one of 3 key options: seek, advance, set val
        private  void Instruction_Process_Instruction(string sInstruction, ref StreamReader fileTarget, ref Dictionary<string, string> dictVals, ref string sTargetBuffer, ref int nTargetBufferIndex, string sDelimiter)
        {
            if (sInstruction[0] == sDelimiter[0])
            {
                sInstruction = sInstruction.Replace(sDelimiter, "");
                Instruction_SeekTargetFileLocation(ref fileTarget, sInstruction, ref sTargetBuffer, ref nTargetBufferIndex);
            }
            else if (sInstruction.Trim().ToLower() == "l1")
            {
                sTargetBuffer = fileTarget.ReadLine();
                nTargetBufferIndex = 0;
            }
            else
            {
                string sKey = sInstruction;
                string sVal = Instruction_ReturnVal(ref sKey, ref sTargetBuffer, ref nTargetBufferIndex);
                if (dictVals.ContainsKey(sKey))
                {
                    Console.WriteLine("Instruction file already contains variable: " + sKey);
                }
                else
                {
                    dictVals.Add(sKey, sVal);
                }
            }
        }

        //moves target file cursor to the correct line and column
        private  void Instruction_SeekTargetFileLocation(ref StreamReader fileTarget, string sSeek, ref string sTargetBuffer, ref int nTargetBufferIndex)
        {
            int nIndex = -1;
            while ((!fileTarget.EndOfStream) && (nIndex < 0))
            {
                sTargetBuffer = fileTarget.ReadLine();
                nTargetBufferIndex = 0;
                nIndex = sTargetBuffer.IndexOf(sSeek, nTargetBufferIndex);
                if (nIndex >= 0)
                {
                    if (nIndex + sSeek.Length >= sTargetBuffer.Length)
                    {
                        sTargetBuffer = "";
                        nTargetBufferIndex = 0;
                    }
                    else
                    {
                        //    sTargetBuffer = sTargetBuffer.Substring(nIndex + sSeek.Length).Trim();
                        nTargetBufferIndex = nTargetBufferIndex + sSeek.Length - 1;
                    }
                }
            }
        }

        //the correct location in file is already found- task is to grab the value that we want.
        //several options for how to do that.
        private  string Instruction_ReturnVal(ref string sKey, ref string sTargetBuffer, ref int nTargetBufferIndex)
        {
            string sValReturn = "UNDEFINED";
            switch (sKey[0])
            {
                case '[':           // type: [var]nStartCol:nEndCol
                    string s = sKey.Substring(sKey.IndexOf("]") + 1);
                    string[] sIndex = s.Split(':');
                    int nStartCol = Convert.ToInt32(sIndex[0]);
                    int nEndCol = Convert.ToInt32(sIndex[1]);           //get start and end columns
                    sValReturn = sTargetBuffer.Substring(nStartCol - 1, nEndCol - nStartCol + 1);
                    sKey = sKey.Substring(1, sKey.IndexOf(']') - 1);                 //   sKey= sKey.Replace("[","").Replace("]","");

                    break;
                case '{':              //type: [sVarName]nColumn:<optional>sColDelimiter        //added by MET to grab the proper from a separated list if you are unaware of the particular column
                    //if sColDelimiter is not provided, it is assumed to be a Space
                    //this does not change the nTargetBufferIndex

                    string s2 = sKey.Substring(sKey.IndexOf("}") + 1);
                    string[] sIndex2 = s2.Split(':');
                    string sDataDelimiter = " ";
                    if (sIndex2.Length > 1)
                        sDataDelimiter = sIndex2[1];
                    string[] sBufferSplit = sTargetBuffer.Split(sDataDelimiter[0]);
                    sValReturn = sBufferSplit[Convert.ToInt32(sIndex2[0])];
                    sKey = sKey.Substring(1, sKey.IndexOf('}') - 1);                            //sKey= sKey.Replace("{","").Replace("}","");
                    break;
                case '!':           // type: [var]nStartCol:nEndCol                      //!VAR!- space separated !!VAR!! comma separated
                    string sDataDelimiterType3 = " ";
                    if (sKey[1] == '!')
                    {
                        sDataDelimiterType3 = ",";
                    }
                    sValReturn = Instruction_GetValFromString(ref sTargetBuffer, ref nTargetBufferIndex, sDataDelimiterType3[0]);
                    sKey = sKey.Replace("!", "");
                    break;
            }

            return sValReturn;
        }


        // take either the next version of the string, or to end of line
        //actually, end of line should already be taken  care of.
        private  string Instruction_GetValFromString(ref string sTargetBuffer, ref int nTargetBufferIndex, char charLineDelimter)
        {
            string sValReturn = "";
            int nIndex = sTargetBuffer.IndexOf(charLineDelimter);
            if (nIndex < 1)           // we are at end of the line
            {
                sValReturn = sTargetBuffer.Trim();
                sTargetBuffer = "";
                nTargetBufferIndex = 0;
            }
            else
            {
                sValReturn = sTargetBuffer.Substring(0, nIndex);
                sTargetBuffer = sTargetBuffer.Substring(nIndex + 1);
                nTargetBufferIndex = 0;
            }

            return sValReturn;
        }



        //gets next instruction and removes this from the current buffer
        private  string Instruction_GetNextInstruction(ref string sBUF_INS, string sDelimiter)
        {
            string s = "";
            if (sBUF_INS[0] == sDelimiter[0])
            {
                int nClose = sBUF_INS.IndexOf(sDelimiter, 1);
                s = sBUF_INS.Substring(0, nClose + 1);
                if (s.Length == sBUF_INS.Length)
                {
                    sBUF_INS = "";
                }
                else
                {
                    sBUF_INS = sBUF_INS.Substring(s.Length + 1, sBUF_INS.Length - s.Length - 1);
                }
            }
            else
            {
                int nIndex = sBUF_INS.IndexOf(' ');
                if (nIndex > 0)
                {
                    s = sBUF_INS.Substring(0, sBUF_INS.IndexOf(' '));
                    sBUF_INS = sBUF_INS.Substring(s.Length + 1, sBUF_INS.Length - s.Length - 1);
                }
                else
                {
                    s = sBUF_INS;       //last instructin on the line
                    sBUF_INS = "";
                }



            }
            return s;
        }

        //sets the delimiter, and also the filename of the target.
        private  string Instruction_ProcessFirstLine(string sBUF_INS, string sInstructionFileName, out string sTargetFile, out bool bIsValid)
        {
            string sDelimiter = ""; string sTargetFileName = ""; bool ReturnIsValid = true;
            string[] sVals = sBUF_INS.Split(' ');
            if (sVals.Length == 3)
            {
                sDelimiter = sVals[1];
                sTargetFileName = sVals[2];
                if (sTargetFileName[0] == '.')
                {
                    sTargetFileName = Path.GetFileNameWithoutExtension(sInstructionFileName) + sTargetFileName;   //only extension was provided; get filename from .tpli
                }
                bIsValid = true;
            }
            else
            {
                //not of correct file type
                bIsValid = false;
            }
            sTargetFile = Path.Combine(Path.GetDirectoryName(sInstructionFileName), sTargetFileName);
            return sDelimiter;
        }


        #endregion


        #endregion         // end template


    }
}
