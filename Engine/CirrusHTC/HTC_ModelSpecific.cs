using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CIRRUS_HTC_NS
{

    #region HTC_ExtendSim
    public class HTC_ExtendSim : CIRRUS_HTC
    {
        #region Constructors

        public HTC_ExtendSim()
        {
            _bOutputIsInput = true;
        }
        #endregion Constructors
        
        //todo: this is overly specific; replace with HTC_SimType
        public enum HTC_ExtendSimType
        {
            MoxFile,
            ZipFile
        }

        HTC_ExtendSimType _HTC_ExtendSimTypeFlag = HTC_ExtendSimType.ZipFile;
        int _nExtendSimConfig;
        const string _sExtendBat = "runExtend.bat";
        public Dictionary<string, string> _dictCHTC_ModelSpec =new Dictionary<string, string>();       //specific model updates that will then update the overall 

        public void PreProcessCondorJob(string[] sFileNames, string[] args)
        {
            string sTargetMOX = GetMOX(sFileNames);
            updateWorkingDir(sTargetMOX, false);                //set the activeDir (if already set, it will not be modified)
            if (sTargetMOX == "UNDEFINED")                 
            {
                _HTC_ExtendSimTypeFlag = HTC_ExtendSimType.ZipFile;
                //for now, assume that the .bat file is loaded
                //future: create the batch file
            }
            else
            {
                _HTC_ExtendSimTypeFlag = HTC_ExtendSimType.MoxFile;                                                                //transfer back the ACTUAL.mox (not ORIG_HTC.mox in disugise)
                if (_bOutputIsInput)
                {
                    sTargetMOX = HTC_DisguiseInputFile(sTargetMOX, ref sFileNames);
                }
                CreateBat(_sActiveHTCDir, sTargetMOX, _HTC_ExtendSimTypeFlag);

                CommonUtilities.cu7ZipFile(sFileNames, _sActiveHTCDir + "\\" + _sZipFileName, true, true);
                if (true)
                {
                    File.Delete(sTargetMOX);                //potentially parametize this- delete the file so we do not confuse the user.
                }

                
            }
            SetExecutable(sTargetMOX);          //add to the dictionary
            SetRequirements();
            SetFilesToTransfer(sTargetMOX);
            UpdateHTC_DictionaryByDictionary(_dictCHTC_ModelSpec);
        }

        private void SetFilesToTransfer(string sTargetInputFile)
        {
            _dictCHTC_ModelSpec.Add("should_transfer_files", "YES");
            _dictCHTC_ModelSpec.Add("transfer_input_files", _sZipFileName);
            _dictCHTC_ModelSpec.Add("transfer_output_files", Path.GetFileName(sTargetInputFile.Replace("_HTC.mox", ".mox")));  
        }

        private string HTC_DisguiseInputFile(string sTargetInputFile, ref string[] sFileNames)
        {
            string sTargetDisguise = Path.GetDirectoryName(sTargetInputFile) + "\\" + Path.GetFileNameWithoutExtension(sTargetInputFile) + "_HTC" + ".mox"; //move the file
            File.Move(sTargetInputFile, sTargetDisguise);
            
            //now change in the list of files that gets zipped
            for (int i = 0; i < sFileNames.Length; i++)
            {
                if (Path.GetExtension(sFileNames[i]).ToLower() == ".mox")
                {
                    sFileNames[i] = sTargetDisguise;
                }
            }
            
            return sTargetDisguise;
        }

        private void SetRequirements()
        {
            _dictCHTC_ModelSpec.Add("requirements", HTC_REQ_SOFTWARE.HasEXTENDSIM.ToString());         //add the exe
            argREQUIREMENT_Update();
        }

        private void SetExecutable(string sTargetMOX)
        {
            bool bIsUNC = false; string sBAT_Call = "";
            if (bIsUNC)
            {
                sBAT_Call = System.IO.Path.GetDirectoryName(sTargetMOX) + "\\" + _sExtendBat;
            }
            else
            {
                sBAT_Call = _sExtendBat;
            }
            _dictCHTC_ModelSpec.Add("executable", sBAT_Call);         //add the exe
        }

        public static void CreateBat(string sTargetPath, string sTargetMOX, HTC_ExtendSimType htcType)
        {
            bool bIsUNC = false; string sBAT_Call = ""; string sEXE = "";
            string sBat = System.IO.Path.Combine(sTargetPath, _sExtendBat);
            if (sTargetMOX.Substring(0, 2) == @"\\") { bIsUNC = true; }         //explicit paths required if UNC
            if (bIsUNC)
            {
                sEXE = "ExtendSim  " + sTargetMOX ;
                sBAT_Call = System.IO.Path.GetDirectoryName(sTargetMOX) + "\\" + _sExtendBat;
            }
            else
            {
                sEXE = "ExtendSim  " + System.IO.Path.GetFileName(sTargetMOX);
                sBAT_Call = _sExtendBat;
            }

            string[] s = new string[1];
            if (htcType == HTC_ExtendSimType.ZipFile)
            {
                s[0] = "7z x " + sTargetMOX + "\r\n" + sEXE;
                //do nothing for now (Assuming .bat is already done- in the future we will create this as well, but requires reading the archive to identify mox (or taking arg)
            }
            else
            {
                s[0] = "7z x " + _sZipFileName + "\r\n" + sEXE;
                s[0] = s[0] +  "\r\n" + "REN " + Path.GetFileName(sTargetMOX) + " " + Path.GetFileName(sTargetMOX.Replace("_HTC.mox", ".mox"));
                File.WriteAllLines(sBat, s);
            }
        }

        //passed list of loaded files, determine the .mox (or return undefined)
        private static string GetMOX(string[] sFileNames)
        {
            string sReturn = "UNDEFINED";
            for (int i = 0; i < sFileNames.Length; i++)
            {
                if (Path.GetExtension(sFileNames[i]).ToLower() == ".mox")
                {
                    sReturn = sFileNames[i];
                }
            }

            return sReturn;
        }

        /*
         * 7z x files.7z
echo %userdomain%\%username%
echo path
ExtendSim Test.mox
         * 
         */

    }
    #endregion      //extendsim


    #region HTC_SWMM

    public class HTC_SWMM : CIRRUS_HTC
    {
        #region Constructors

        public HTC_SWMM()
        {
            _bOutputIsInput = false;
        }
        #endregion Constructors

        int _nExtendSimConfig;
        const string _sSimBat = "run_swmm5.bat";
        public Dictionary<string, string> _dictCHTC_ModelSpec = new Dictionary<string, string>();       //specific model updates that will then update the overall 

        public override void PreProcessCondorJob(string[] sFileNames, string[] args)
        {
            string sTargetInputFile = GetINP(sFileNames);
            updateWorkingDir(sTargetInputFile, false);                //set the activeDir (if already set, it will not be modified)
            if (sTargetInputFile == "UNDEFINED")
            {
                _SimType = CIRRUS_HTC.HTC_SimType.ZipFile;
                //for now, assume that the .bat file is loaded
                //future: create the batch file
            }
            else
            {
                _SimType = CIRRUS_HTC.HTC_SimType.InputFile;                                                         //transfer back the ACTUAL.mox (not ORIG_HTC.mox in disugise)
                CreateBat(_sActiveHTCDir, sTargetInputFile, _SimType);
            }
            SetExecutable(sTargetInputFile);          //add to the dictionary
            SetRequirements();
            SetFilesToTransfer(ref sFileNames);
            UpdateHTC_DictionaryByDictionary(_dictCHTC_ModelSpec);
        }

        private void SetFilesToTransfer(ref string[] sFileNames)
        {
            _dictCHTC_ModelSpec.Add("should_transfer_files", "YES");
            if (_SimType == HTC_SimType.ZipFile)
            {
                _dictCHTC_ModelSpec.Add("transfer_input_files", _sZipFileName);
            }
            else
            {
                string sFilesToTransfer = CommonUtilities.cuCreateFileString(sFileNames);
                _dictCHTC_ModelSpec.Add("transfer_input_files", sFilesToTransfer);
            }

     //       _dictCHTC_ModelSpec.Add("transfer_output_files", Path.GetFileName(sTargetInputFile.Replace("_HTC.mox", ".mox")));
        }


        private void SetRequirements(bool bReqSoftware = true)
        {
            if (bReqSoftware)
            {
                _dictCHTC_ModelSpec.Add("requirements", HTC_REQ_SOFTWARE.HasSWMM.ToString());         //add the exe
            }
            argREQUIREMENT_Update();
        }

        private void SetExecutable(string sTargetInputFile)
        {
            bool bIsUNC = false; string sBAT_Call = "";
            if (bIsUNC)
            {
                sBAT_Call = System.IO.Path.GetDirectoryName(sTargetInputFile) + "\\" + _sSimBat;
            }
            else
            {
                sBAT_Call = _sSimBat;
            }
            _dictCHTC_ModelSpec.Add("executable", sBAT_Call);         //add the exe
        }

        public static void CreateBat(string sTargetPath, string sTargetInputFile, HTC_SimType htcSimType)
        {
            bool bIsUNC = false; string sBAT_Call = ""; string sEXE = ""; 
        //    string sBat = System.IO.Path.Combine(sTargetPath, _sSimBat);
            string sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTargetInputFile) + ".RPT";
            string sOUT = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTargetInputFile) + ".OUT";

            if (sTargetInputFile.Substring(0, 2) == @"\\") { bIsUNC = true; }         //explicit paths required if UNC

            if (bIsUNC)
            {
                sEXE = "swmm5.exe " + sTargetInputFile + " " + sSummaryFile + " " + sOUT;
                sBAT_Call = System.IO.Path.GetDirectoryName(sTargetInputFile) + "\\" + _sSimBat;
            }
            else
            {
                sEXE = "swmm5.exe " + System.IO.Path.GetFileName(sTargetInputFile) + " " + System.IO.Path.GetFileName(sSummaryFile) + " " + System.IO.Path.GetFileName(sOUT);
                sBAT_Call = "run_swmm5.bat";
            }

            //create batch file information for running the program
            string[] s = new string[] { sEXE };
            if (htcSimType==HTC_SimType.ZipFile)
            {
                s[0] = "7z x files.7z" + "\r\n" + s[0];
            }
            string sBat = System.IO.Path.Combine(sTargetPath, _sSimBat);
            File.WriteAllLines(sBat, s);

        }

        //passed list of loaded files, determine the .mox (or return undefined)
        private static string GetINP(string[] sFileNames)
        {
            string sReturn = "UNDEFINED";
            for (int i = 0; i < sFileNames.Length; i++)
            {
                if (Path.GetExtension(sFileNames[i]).ToLower() == ".inp")
                {
                    sReturn = sFileNames[i];
                }
            }
            return sReturn;
        }
    }
    #endregion
    

    //met blindly copied/replaced from swmm 2/5/14
    #region HTC_EPANET

    public class HTC_EPANET : CIRRUS_HTC
    {
        #region Constructors

        public HTC_EPANET()
        {
            _bOutputIsInput = false;
        }
        #endregion Constructors

        int _nExtendSimConfig;
        const string _sSimBat = "run_EPANET.bat";
        public Dictionary<string, string> _dictCHTC_ModelSpec = new Dictionary<string, string>();       //specific model updates that will then update the overall 

        public override void PreProcessCondorJob(string[] sFileNames, string[] args)
        {
            string sTargetInputFile = GetINP(sFileNames, ".inp", ".bat");
            updateWorkingDir(sTargetInputFile, false);                //set the activeDir (if already set, it will not be modified)
            if (Path.GetExtension(sTargetInputFile).ToLower() == ".bat")
            {
                _SimType = CIRRUS_HTC.HTC_SimType.ZipFile;
                //for now, assume that the .bat file is loaded
                //future: create the batch file
            }
            else
            {
                _SimType = CIRRUS_HTC.HTC_SimType.InputFile;                                                         //transfer back the ACTUAL.mox (not ORIG_HTC.mox in disugise)
                CreateBat(_sActiveHTCDir, sTargetInputFile, _SimType);
            }
            SetExecutable(sTargetInputFile);          //add to the dictionary
            SetRequirements();
            SetFilesToTransfer(ref sFileNames);
            UpdateHTC_DictionaryByDictionary(_dictCHTC_ModelSpec);
        }

        private void SetFilesToTransfer(ref string[] sFileNames)
        {
            _dictCHTC_ModelSpec.Add("should_transfer_files", "YES");
            if (_SimType == HTC_SimType.ZipFile)
            {
                _dictCHTC_ModelSpec.Add("transfer_input_files", _sZipFileName);
            }
            else
            {
                string sFilesToTransfer = CommonUtilities.cuCreateFileString(sFileNames);
                _dictCHTC_ModelSpec.Add("transfer_input_files", sFilesToTransfer);
            }

            //       _dictCHTC_ModelSpec.Add("transfer_output_files", Path.GetFileName(sTargetInputFile.Replace("_HTC.mox", ".mox")));
        }


        private void SetRequirements(bool bReqSoftware = true)
        {
            if (bReqSoftware)
            {
                _dictCHTC_ModelSpec.Add("requirements", HTC_REQ_SOFTWARE.HasEPANET.ToString());         //add the exe
            }
            argREQUIREMENT_Update();
        }

        private void SetExecutable(string sTargetInputFile)
        {
            bool bIsUNC = false; string sBAT_Call = "";
            if (bIsUNC)
            {
                sBAT_Call = System.IO.Path.GetDirectoryName(sTargetInputFile) + "\\" + _sSimBat;
            }
            else
            {
                sBAT_Call = _sSimBat;
            }
            _dictCHTC_ModelSpec.Add("executable", sBAT_Call);         //add the exe
        }

        public static void CreateBat(string sTargetPath, string sTargetInputFile, HTC_SimType htcSimType)
        {
            bool bIsUNC = false; string sBAT_Call = ""; string sEXE = "";
            //    string sBat = System.IO.Path.Combine(sTargetPath, _sSimBat);
            string sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTargetInputFile) + ".RPT";
            string sOUT = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTargetInputFile) + ".OUT";

            if (sTargetInputFile.Substring(0, 2) == @"\\") { bIsUNC = true; }         //explicit paths required if UNC

            if (bIsUNC)
            {
                sEXE = "EPANET2d.exe " + sTargetInputFile + " " + sSummaryFile + " " + sOUT;
                sBAT_Call = System.IO.Path.GetDirectoryName(sTargetInputFile) + "\\" + _sSimBat;
            }
            else
            {
                sEXE = "EPANET2d.exe " + System.IO.Path.GetFileName(sTargetInputFile) + " " + System.IO.Path.GetFileName(sSummaryFile) + " " + System.IO.Path.GetFileName(sOUT);
                sBAT_Call = "run_EPANET.bat";
            }

            //create batch file information for running the program
            string[] s = new string[] { sEXE };
            if (htcSimType == HTC_SimType.ZipFile)
            {
                s[0] = "7z x files.7z" + "\r\n" + s[0];
            }
            string sBat = System.IO.Path.Combine(sTargetPath, _sSimBat);
            File.WriteAllLines(sBat, s);

        }

        //passed list of loaded files, determine the .mox (or return undefined)
        private static string GetINP(string[] sFileNames, string sTargetExtensionPrimary = ".inp", string sTargetExtensionSecondary = ".bat")
        {
            string sReturn = "UNDEFINED";
            for (int i = 0; i < sFileNames.Length; i++)
            {
                if (Path.GetExtension(sFileNames[i]).ToLower() == sTargetExtensionPrimary)
                {
                    sReturn = sFileNames[i];
                }
            }
            if (sReturn == "UNDEFINED")
            {
                for (int i = 0; i < sFileNames.Length; i++)
                {
                    if (Path.GetExtension(sFileNames[i]).ToLower() == sTargetExtensionSecondary)
                    {
                        sReturn = sFileNames[i];
                    }
                }

            }
            return sReturn;
        }
    }
    #endregion


#region MODFLOW
    #region HTC_MODFLOW

    public class HTC_MODFLOW : CIRRUS_HTC
    {
        #region Constructors

        public HTC_MODFLOW()
        {
            _bOutputIsInput = false;
        }
        #endregion Constructors

        int _nExtendSimConfig;
        private string _sSimBat = "UNDEFINED";
        private string _sSimResults = "results.7z";
        public Dictionary<string, string> _dictCHTC_ModelSpec = new Dictionary<string, string>();       //specific model updates that will then update the overall 

        public override void PreProcessCondorJob(string[] sFileNames, string[] args)
        {
            string sBAT_Call="";
            string _sZipFileName = GetINP(sFileNames, out sBAT_Call);
            updateWorkingDir(sBAT_Call, false);                //set the activeDir (if already set, it will not be modified)
            _SimType = CIRRUS_HTC.HTC_SimType.ZipFile;
            SetExecutable(sBAT_Call);          //add to the dictionary
            SetRequirements();
            SetFilesToTransfer(ref sFileNames);
            UpdateHTC_DictionaryByDictionary(_dictCHTC_ModelSpec);
        }

        private void SetFilesToTransfer(ref string[] sFileNames)
        {
            _dictCHTC_ModelSpec.Add("should_transfer_files", "YES");
         //   string sFilesToTransfer = CommonUtilities.cuCreateFileString(sFileNames);
        //    _dictCHTC_ModelSpec.Add("transfer_input_files", sFilesToTransfer);
            _dictCHTC_ModelSpec.Add("transfer_input_files", CommonUtilities.cuGetUNCFileString(_sZipFileName));              //j
            _dictCHTC_ModelSpec.Add("transfer_output_files", _sSimResults);  
        }


        private void SetRequirements(bool bReqSoftware = true)
        {
            argREQUIREMENT_Update();
        }

        //for modflow, where the .bat is provided, the GetEXE functions somehwat differently than in other wrapper classe.
        private void SetExecutable(string sTargetInputFile)     //targetinput is  a BAT File
        {
            bool bIsUNC = false; 
            if (bIsUNC)
            {
                _sSimBat = sTargetInputFile;           
            }
            else
            {
                _sSimBat = Path.GetFileName(sTargetInputFile);
            }
            _dictCHTC_ModelSpec.Add("executable", _sSimBat);         //add the exe
        }

        /* for now SConfig is that the .bat is uploaded also
        public static void CreateBat(string sTargetPath, string sTargetInputFile, HTC_SimType htcSimType)
        {
            bool bIsUNC = false; string sBAT_Call = ""; string sEXE = "";
            //    string sBat = System.IO.Path.Combine(sTargetPath, _sSimBat);
            string sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTargetInputFile) + ".RPT";
            string sOUT = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTargetInputFile) + ".OUT";

            if (sTargetInputFile.Substring(0, 2) == @"\\") { bIsUNC = true; }         //explicit paths required if UNC

            if (bIsUNC)
            {
                sEXE = "MODFLOW5.exe " + sTargetInputFile + " " + sSummaryFile + " " + sOUT;
                sBAT_Call = System.IO.Path.GetDirectoryName(sTargetInputFile) + "\\" + _sSimBat;
            }
            else
            {
                sEXE = "MODFLOW5.exe " + System.IO.Path.GetFileName(sTargetInputFile) + " " + System.IO.Path.GetFileName(sSummaryFile) + " " + System.IO.Path.GetFileName(sOUT);
                sBAT_Call = "run_MODFLOW5.bat";
            }

            //create batch file information for running the program
            string[] s = new string[] { sEXE };
            if (htcSimType == HTC_SimType.ZipFile)
            {
                s[0] = "7z x files.7z" + "\r\n" + s[0];
            }
            string sBat = System.IO.Path.Combine(sTargetPath, _sSimBat);
            File.WriteAllLines(sBat, s);

        }
        */
        //passed list of loaded files, determine the .mox (or return undefined)
        private static string GetINP(string[] sFileNames, out string sBatCall)
        {
            string sReturn = "UNDEFINED";
            sBatCall = "UNDEFINED";
            for (int i = 0; i < sFileNames.Length; i++)
            {
                if (Path.GetExtension(sFileNames[i]).ToLower() == ".7z")
                {
                    sReturn = sFileNames[i];
                }
                if (Path.GetExtension(sFileNames[i]).ToLower() == ".bat")
                {
                    sBatCall = sFileNames[i];
                }
            }
            return sReturn;
        }
    }
    #endregion
#endregion
}
