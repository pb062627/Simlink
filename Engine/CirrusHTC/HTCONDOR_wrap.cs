using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;


namespace CIRRUS_HTC_NS
{
    public static class HTC_CONSTANTS
    {
        public const string _sSUBMIT_FILE = "condor.submit";
        public const string _sCONDOR_BAT_FILE = "condor.bat";

    }

    //provdes 1:1 mapping of the htCondor submit file
    //includes var on both the job / platform
    public class HTC_SUBMIT_SPEC
    {
        public string _sUniverse = CommonUtilities._sDATA_UNDEFINED;
        public string _sExecutable = CommonUtilities._sDATA_UNDEFINED;
        public string _sArguments = CommonUtilities._sDATA_UNDEFINED;
        public string _sRequirement = CommonUtilities._sDATA_UNDEFINED;
        public string _sOutput = CommonUtilities._sDATA_UNDEFINED;
        public string _sError = CommonUtilities._sDATA_UNDEFINED;
        public string _sLog = CommonUtilities._sDATA_UNDEFINED;
        public string _sShould_transfer_files = CommonUtilities._sDATA_UNDEFINED;
        public string _sWhen_to_transfer_output = CommonUtilities._sDATA_UNDEFINED;
        public string _sTransfer_input_files = CommonUtilities._sDATA_UNDEFINED;                                   //was _sTransfer_output_files
        public string _sInitialDir = CommonUtilities._sDATA_UNDEFINED;
    }

    //5/11/14: specifies how to handle any Cirrus type application
    public class HTC_PLATFORM_SPEC
    {
        public string _sSoftwareName;       //match to _sPlatformName name...  (need better?)
        public string _sSoftwareZip;
        public string _sRunFileExt;
        public string _sRunCommand;
        public string _sOutputExt;
        public string _sSetPathCommand;
        public string _sCommandLineBAT;
        public bool _bOutputIsInput;
        public bool _bUnzipInput;
        public bool _bZipOutput;
        public string _sUnzipInputCommand;
        public string _sUnzipOutputCommand;
        public string _sZipInputFilename;
        public string _sZipOutputFilename;

        public HTC_SUBMIT_SPEC _htcSubmitSpec;          //platform defaults for submit
    }

    //job specification is  tied to specific req of job; may overwrite general platform issues
    public class HTC_JOB_SPEC
    {
        public string _sPlatformName = "GENERIC";       ////match to _sSoftwareName name...  (need better?)  - GENERIC for not specified 
        public bool _bIsSimLink = false;
        public bool _bCreateCondorRunFolder = false;           //set true if loading to a central location (like server)
        public string _sActiveHTCDir = CommonUtilities._sDATA_UNDEFINED;                // the directory where the htc job will be assembled
        public string _sCUST_ModelRunName = CommonUtilities._sDATA_UNDEFINED;
        public string _sCUST_Requirements = CommonUtilities._sDATA_UNDEFINED;               //met 9/14/14: are we duplicating submit.req? 
        public string _sZipFileName = "files.7z";
        public bool _IsUNC = false;                        //don't need to track on job spec... but helper
        public bool _bInvokedByCLI = false;                //false - web UI  true- CLI
        public HTC_SUBMIT_SPEC _htcSubmitSpec;          //platform defaults for submit
        public bool _bIsValid = true;
    }

    public class CIRRUS_HTC
    {
        #region CONSTANTS
        private const string _sXML_JobSpec_IndicateByFilename = "HTC_SPEC";         //trick to find files XML specification file if more than one .XML

        private const string _sNS_XML = "http://www.halcrow.com/ISIS2D";
        //private const XNamespace _nsXML = new XNamespace(); 
        #endregion


        public List<HTC_PLATFORM_SPEC> _lstHTC_Spec = new List<HTC_PLATFORM_SPEC>();             //supported platform specifications from XML (load on init)
        public List<HTC_JOB_SPEC> _lstHTC_JobSpec = new List<HTC_JOB_SPEC>();
        public HTC_PLATFORM_SPEC _htcPlatformSpecActive;                       //contains the active platform specification
        public HTC_JOB_SPEC _htcJobSpecActive;


        public Dictionary<string, string> _dictCHTC_Args;
        private string sJobDirectory;
        protected string _sCirrusHTC_WorkingDir = @"C:\Program Files\SimLink\CirrusHTC\JobCenter";      //todo: read from config
        protected const string _sCirrusHTC_ConfigFile = @"C:\Program Files\SimLink\CirrusHTC\config\simLib2.xml";

        private long lTimestampBeginHTC;                    //used to track when a 

        #region enum
        protected enum HTC_REQ_SOFTWARE
        {
            HasEXTENDSIM,
            HasSWMM,
            HasEPANET,
            HasPREVIEW
        }

        public enum HTC_SimType
        {
            InputFile,
            ZipFile,
            Undefined
        }


        #endregion

        #region INITIALIZATION

        public CIRRUS_HTC()
        {
            try
            {
                // no longer relevant; remoeved..  needs to be in overall xml. met 11/23/16
                //InitCirrusHTC_byXML();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error creating HTC object; check whether Cirrus XML is found. Msg: " + ex.Message);
                //todo: log the issue
            }
        }

        public void InitCirrusHTC_byXML(string sInputXML = _sCirrusHTC_ConfigFile)
        {
            //  _nsXML = "http://www.halcrow.com/ISIS2D";
            XDocument xdoc = XDocument.Load(sInputXML); // load document

            XNamespace ns = _sNS_XML;   // _nsXML;
            IEnumerable<XElement> xe = from c in xdoc.Root.Elements(ns + "platform")
                                       select c;

            foreach (XElement x in xe)
            {
                HTC_PLATFORM_SPEC htcPlatform = new HTC_PLATFORM_SPEC();
                htcPlatform._sSoftwareName = x.Element(ns + "Name").Value;
                htcPlatform._sSoftwareZip = x.Element(ns + "SoftwareZip").Value;
                htcPlatform._sRunFileExt = x.Element(ns + "RunFileExt").Value;
                htcPlatform._sRunCommand = x.Element(ns + "RunCommand").Value;
                htcPlatform._sOutputExt = x.Element(ns + "OutputExt").Value;
                htcPlatform._sSetPathCommand = x.Element(ns + "SetPathCommand").Value;
                htcPlatform._sCommandLineBAT = x.Element(ns + "CommandLineBAT").Value;
                //zipping specifications
                htcPlatform._bUnzipInput = Convert.ToBoolean(x.Element(ns + "ZIP_UnzipInputFiles").Value);
                htcPlatform._bZipOutput = Convert.ToBoolean(x.Element(ns + "ZIP_ZipOutputFiles").Value);
                htcPlatform._sUnzipInputCommand = x.Element(ns + "ZIP_UnzipInputCommand").Value;
                htcPlatform._sUnzipOutputCommand = x.Element(ns + "ZIP_ZipOutputCommand").Value;
                htcPlatform._sZipInputFilename = x.Element(ns + "ZIP_InputZipName").Value;
                htcPlatform._sZipOutputFilename = x.Element(ns + "ZIP_OutputZipName").Value;

                IEnumerable<XElement> xeSubmit = from c in x.Descendants(ns + "submit")
                                                 select c;

                htcPlatform._htcSubmitSpec = ReadSubmitSpec(xeSubmit, ns);
                _lstHTC_Spec.Add(htcPlatform);
            }
        }

        private HTC_SUBMIT_SPEC ReadSubmitSpec(IEnumerable<XElement> xeSubmit, XNamespace ns)
        {
            HTC_SUBMIT_SPEC htcSubmitSpec = new HTC_SUBMIT_SPEC();
            IEnumerable<XElement> xe = from c in xeSubmit.Elements()
                                       select c;

            foreach (XElement x in xe)
            {
                string sTargt = x.Name.ToString();

                //todo: make case sensitive   (in-sensitive?)
                //complication: the switch likes to be CONST - not completely trivial for the ns
                switch (sTargt)
                {
                    case "{" + _sNS_XML + "}" + "universe":             //must be better e
                        htcSubmitSpec._sUniverse = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "executable":
                        htcSubmitSpec._sExecutable = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "arguments":
                        htcSubmitSpec._sArguments = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "requirements":             //must be better e
                        htcSubmitSpec._sRequirement = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "output":
                        htcSubmitSpec._sOutput = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "error":
                        htcSubmitSpec._sError = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "log":
                        htcSubmitSpec._sLog = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "should_transfer_files":
                        htcSubmitSpec._sShould_transfer_files = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "when_to_transfer_output":
                        htcSubmitSpec._sWhen_to_transfer_output = x.Value.ToString();
                        break;
                    default:
                        Console.WriteLine("Param not found: " + x.Name);
                        break;
                }

            }
            return htcSubmitSpec;
        }


        // if xml not passed, will search in working dir
        //from website: we can pass the XML directly
        //from CLI: preferable not to have to specify...
        public void InitJob(string sJobXML = CommonUtilities._sDATA_UNDEFINED)
        {
            ReadXML_JobSpecification(sJobXML);
        }
        #region Job Specification Details
        public void ReadXML_JobSpecification(string sJobXML_Spec)
        {
            if (sJobXML_Spec == CommonUtilities._sDATA_UNDEFINED)           //
            {
                string sDir = Directory.GetCurrentDirectory() + "\\";
                sJobXML_Spec = FindXML_JobSpecification(sDir);
            }
            if (sJobXML_Spec != CommonUtilities._sDATA_UNDEFINED)
            {
                ReadTheActualJobSpec(sJobXML_Spec);
            }
            else
            {
                //is generic..  this is by default
            }
        }

        private void ReadTheActualJobSpec(string sJobXML_Spec)
        {
            XDocument xdoc = XDocument.Load(sJobXML_Spec); // load document

            XNamespace ns = _sNS_XML;
            IEnumerable<XElement> xe = from c in xdoc.Root.Elements(ns + "job")
                                       select c;

            foreach (XElement x in xe)
            {
                HTC_JOB_SPEC htcJob = new HTC_JOB_SPEC();
                //todo: in future, may suppoort multiple jobs, or multiple TYPES of jobes --> would need addtl info

                IEnumerable<XElement> xe2 = x.Elements();

                foreach (XElement x2 in xe2)
                {
                    JobSpec_Helper(ref htcJob, x2);
                }

                //set the default spec based upon platform
                htcJob._htcSubmitSpec = GetSubmitByPlatform(htcJob._sPlatformName);  // set to defaults ... 

                // check for submit
                IEnumerable<XElement> xeSubmit = from c in x.Descendants(ns + "submit")
                                                 select c;

                htcJob._htcSubmitSpec = ReadSubmitSpec(xeSubmit, _sNS_XML);
                htcJob._sActiveHTCDir = Path.GetDirectoryName(sJobXML_Spec);    //todo: is this right? Figure 
                _lstHTC_JobSpec.Add(htcJob);
            }
        }

        private void JobSpec_Helper(ref HTC_JOB_SPEC theJob, XElement x)
        {
            try
            {
                string sTargt = x.Name.ToString();

                //todo: make case sensitive
                //complication: the switch likes to be CONST - not completely trivial for the ns
                switch (sTargt)
                {
                    case "{" + _sNS_XML + "}" + "Platform":             //must be better e
                        theJob._sPlatformName = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "Requirement":
                        theJob._sCUST_Requirements = x.Value.ToString();
                        break;
                    case "{" + _sNS_XML + "}" + "IsSimLink":
                        theJob._bIsSimLink = Convert.ToBoolean(x.Value.ToString());
                        break;
                    case "{" + _sNS_XML + "}" + "submit":           // this is now handled after everything else has been read
                        // IEnumerable<XElement> xeSubmit = from c in x.Elements()
                        //                                 select c;

                        //      theJob._htcSubmitSpec = ReadSubmitSpec(xeSubmit, _sNS_XML);
                        break;
                }
            }
            catch (Exception ex)
            {
                //todo: log
                Console.WriteLine("Error adding job spec: " + x.Name.ToString());
            }

        }

        //return undefined code if nothing found.
        private string FindXML_JobSpecification(string sDir)
        {
            string sReturn = CommonUtilities._sDATA_UNDEFINED;
            string[] sXML_Files = GetXmlFiles(sDir);
            int nCountXML = sXML_Files.Length;
            if (nCountXML == 0)
            {
                sReturn = CommonUtilities._sDATA_UNDEFINED;
            }
            else if (nCountXML == 1)
            {
                sReturn = sXML_Files[0];
            }
            else
            {
                foreach (string s in sXML_Files)
                {
                    if (s.IndexOf(_sXML_JobSpec_IndicateByFilename) >= 0)           //test whether file contains the magic letters
                    {
                        sReturn = s;
                    }
                }
            }
            return sReturn;
        }

        //return array of all XMLS files in a dir
        private string[] GetXmlFiles(string sDir)
        {
            string[] sFiles = Directory.GetFiles(sDir);
            List<string> lstToReturn = new List<string>();
            foreach (string sFile in sFiles)
            {
                if (Path.GetExtension(sFile) == ".xml")
                {
                    lstToReturn.Add(sFile);
                }
            }
            return lstToReturn.ToArray();
        }
        #endregion
        //met 8/28/2013 - initialize arg array
        //returns the filenames, which are critical for the call to PreProcessCondor
        public string[] InitArgDict(string[] args)                 // pause on this for now    , out bool bIsValid)
        {
            List<string> lstFileNames = new List<string>();
            _dictCHTC_Args = CommonUtilities.Arguments_ToDict(args);
            if (args.Length > 0)
                _dictCHTC_Args.Add("Platform", args[0]);


            if (_dictCHTC_Args.ContainsKey("f"))
            {
                chtcArgsHelper_GetFilename(_dictCHTC_Args["f"].ToString(), ref lstFileNames);
            }
            //   bIsValid = lstFileNames.Count>0;
            return lstFileNames.ToArray();
        }
        #endregion

        #region PROCESS CONDOR JOBS

        //analagous to Process EG
        //5/18/14: assume SINGLE job, or simple series
        //TODO: consider packaging several jobs
        public void ProcessCondorJobs()
        {
            foreach (HTC_JOB_SPEC job in _lstHTC_JobSpec)
            {
                _htcJobSpecActive = job;
                SyncSpecification();        //get CLI, JOB, and Platform specs squared
                //     UpdateWorkingDir(_htcJobSpecActive._htcSubmitSpec._sExecutable, true);    ///todo: better understand this; sort of left over from before
                ProcessCondorJob();
            }
        }


        //9/14/14: requires that calling program has already set _activejob to whatever it will be.
        public void ProcessCondorJobAndSync()
        {
            SyncSpecification();
            ProcessCondorJob();
        }


        private void ProcessCondorJob()
        {
            PrepareFiles();
            SubmitCondorJob();
        }

        //CLI: allows advanced user to quickly run without creating XML if very standard job
        //

        #region SYNC INPUTS
        #region SYNC JOB SPEC

        // build submit specs based upon other flags passed by the user.
        private void SyncJobSpecToSubmit()
        {


        }

        #endregion



        private void SyncSpecification()
        {
            SyncCLIandJobSpec();            // set ActivePlatform helper...
            SyncJobSubmitToPlatform();
            SyncJobSpecToSubmit();

        }

        //fill in default vals from platform onto job where undefine
        private void SyncJobSubmitToPlatform()
        {
            if (_htcJobSpecActive._htcSubmitSpec._sUniverse == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sUniverse = _htcPlatformSpecActive._htcSubmitSpec._sUniverse;
            if (_htcJobSpecActive._htcSubmitSpec._sExecutable == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sExecutable = _htcPlatformSpecActive._htcSubmitSpec._sExecutable;
            if (_htcJobSpecActive._htcSubmitSpec._sRequirement == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sRequirement = _htcPlatformSpecActive._htcSubmitSpec._sRequirement;
            if (_htcJobSpecActive._htcSubmitSpec._sOutput == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sOutput = _htcPlatformSpecActive._htcSubmitSpec._sOutput;
            if (_htcJobSpecActive._htcSubmitSpec._sError == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sError = _htcPlatformSpecActive._htcSubmitSpec._sError;
            if (_htcJobSpecActive._htcSubmitSpec._sLog == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sLog = _htcPlatformSpecActive._htcSubmitSpec._sLog;
            if (_htcJobSpecActive._htcSubmitSpec._sShould_transfer_files == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sShould_transfer_files = _htcPlatformSpecActive._htcSubmitSpec._sShould_transfer_files;
            if (_htcJobSpecActive._htcSubmitSpec._sWhen_to_transfer_output == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sWhen_to_transfer_output = _htcPlatformSpecActive._htcSubmitSpec._sWhen_to_transfer_output;
            if (_htcJobSpecActive._htcSubmitSpec._sTransfer_input_files == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sTransfer_input_files = _htcPlatformSpecActive._htcSubmitSpec._sTransfer_input_files;
            if (_htcJobSpecActive._htcSubmitSpec._sInitialDir == CommonUtilities._sDATA_UNDEFINED)
                _htcJobSpecActive._htcSubmitSpec._sInitialDir = _htcPlatformSpecActive._htcSubmitSpec._sInitialDir;

        }

        //set active platform
        // added coded to not set platform if set on init.  met 9/14/14
        private void SyncCLIandJobSpec()
        {
            string sPlatform;
            if (_htcJobSpecActive == null)
            {

                if (_htcJobSpecActive._bInvokedByCLI && (_dictCHTC_Args.Count > 0))
                {
                    sPlatform = "x"; //SyncHelper_ModelPlatform("X");      //_dictCHTC_Args.Get`.ToString());
                }
                else
                {
                    sPlatform = _htcJobSpecActive._sPlatformName;
                }
                _htcPlatformSpecActive = SyncGetPlatformItem(sPlatform);
            }
        }

        //returns the SUBMIT spec of the requested platform (defautls that can be overwritten by job file)
        //todo: better to grab with LIN
        public HTC_SUBMIT_SPEC GetSubmitByPlatform(string sPlatform)
        {
            foreach (HTC_PLATFORM_SPEC htcPlatform in _lstHTC_Spec)
            {
                if (htcPlatform._sSoftwareName == sPlatform)
                {
                    return htcPlatform._htcSubmitSpec;
                }
            }
            return null;
        }
        //todo: implement with linq
        public HTC_PLATFORM_SPEC SyncGetPlatformItem(string sPlatform)
        {
            foreach (HTC_PLATFORM_SPEC htcPlatform in _lstHTC_Spec)
            {
                if (htcPlatform._sSoftwareName == sPlatform)
                {
                    return htcPlatform;
                }
            }
            return null; //should never reach here, because of GENERIC fallback.
        }



        //arg: the first argument of a CLI call- which should be a valid platform
        //if not a valid platform, set to GENERIC
        //assume: platform config is well formed and does not include duplicates
        private string SyncHelper_ModelPlatform(string sCLI_Platform)
        {
            string sPlatformReturn = "GENERIC";         //default
            int nCLI_PlatformCount = (_lstHTC_Spec
                                 .Where(x => x._sSoftwareName == sCLI_Platform)).Count();
            int nJob_PlatformCount = (_lstHTC_Spec
                                 .Where(x => x._sSoftwareName == _htcJobSpecActive._sPlatformName)).Count();

            if (nCLI_PlatformCount > nJob_PlatformCount)               //defined (well) in cli
                sPlatformReturn = sCLI_Platform;
            else if (nJob_PlatformCount > nCLI_PlatformCount)         // defined in job spec
                sPlatformReturn = _htcJobSpecActive._sPlatformName;
            else
            {
                if (nJob_PlatformCount == 1)
                {                          //both vals 1; use job spec (trumps)
                    sPlatformReturn = _htcJobSpecActive._sPlatformName;
                }
                else
                {
                    sPlatformReturn = "GENERIC";         //both zero- handle as generic job
                    Console.WriteLine("No sim platform defined; using generic condor setup");
                }
            }
            return sPlatformReturn;
        }

        #endregion
        #endregion




        #region CLI
        //met 8/28/2013
        //input: assume a well-formed REQUIREMENT string
        //update the req dict entry to work with this
        protected void argREQUIREMENT_Update()
        {
            if (_dictCHTC_Args.ContainsKey("req"))
            {
                //todo: ovewrite the requirements
            }
        }

        //adds files to list of strings (if there is a good filename)
        private void chtcArgsHelper_GetFilename(string sFilesConcat, ref List<string> lstFiles)
        {
            char sDelimiter = ',';                    //todo: support "file1.inp file2.inp" in cases where full path does not contain strings
            string[] sFiles = sFilesConcat.Split(sDelimiter);
            foreach (string s in sFiles)
            {
                if (Path.GetFileName(s).IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
                {        //check for valid file name. not a complete check, but a start.
                    lstFiles.Add(s);
                }
            }
        }
        #endregion


        #region PREPARE FILES
        // 1: Check or Create Executable

        public void PrepareFiles()
        {
            PrepareExecutable();
        }

        public void PrepareExecutable()
        {
            bool bEXE_FileExists = CheckForExecutable();
            if (!bEXE_FileExists)
                CreateExecutable();
        }


        public void CreateExecutable()
        {
            string sEXE_File = GetEXEFileByPlatform();
            if (sEXE_File == CommonUtilities._sDATA_UNDEFINED)
            {
                _htcJobSpecActive._bIsValid = false;
                return;
            }
            String sFileName = Path.GetFileNameWithoutExtension(sEXE_File);
            if (_htcJobSpecActive._IsUNC)
                sFileName = Path.GetDirectoryName(sEXE_File) + Path.GetFileNameWithoutExtension(sEXE_File);

            string sEXE = _htcPlatformSpecActive._sCommandLineBAT.Replace("[RunFileName]", sFileName);
            CreateBATforEXE(sEXE);

        }

        //todo: add support for zipping
        private void CreateBATforEXE(string sEXE)
        {
            string sFileNameOut = Path.Combine(_htcJobSpecActive._sActiveHTCDir, _htcJobSpecActive._htcSubmitSpec._sExecutable);
            //     if (_htcJobSpecActive._IsUNC)
            //    Path.Combine(_htcJobSpecActive._sActiveHTCDir, sFileNameOut);

            string[] sLinesOut = new string[] { sEXE };
            File.WriteAllLines(sFileNameOut, sLinesOut);
        }

        private string GetEXEFileByPlatform()
        {
            string sReturn = CommonUtilities._sDATA_UNDEFINED;
            string[] sFileNames = Directory.GetFiles(_htcJobSpecActive._sActiveHTCDir);

            // option 1: search for .bat  - exclude 'condor.bat', which should not be there, but could be especially if troubleshooting
            for (int i = 0; i < sFileNames.Length; i++)
            {
                if (((Path.GetExtension(sFileNames[i]) == ".bat") && (Path.GetFileName(sFileNames[i].ToLower()) != HTC_CONSTANTS._sCONDOR_BAT_FILE)))
                {
                    return sReturn;
                }
            }

            // option 2: search for EXE 
            for (int i = 0; i < sFileNames.Length; i++)
            {
                if (Path.GetExtension(sFileNames[i]).ToLower().Replace(".", "") == _htcPlatformSpecActive._sRunFileExt)
                {
                    sReturn = sFileNames[i];
                    return sReturn;
                }
            }
            return sReturn;
        }


        //todo: consider flag which indicates with exe is full path... assume generally not.
        private bool CheckForExecutable()
        {
            //1: Check working dir
            if (File.Exists(Path.Combine(_htcJobSpecActive._sActiveHTCDir, _htcJobSpecActive._htcSubmitSpec._sExecutable)))
            {
                return true;
            }
            //check full path
            else if (File.Exists(_htcJobSpecActive._htcSubmitSpec._sExecutable))
            {
                return true;
            }
            return false;
        }



        #endregion
        public void SubmitCondorJob(bool bSkipCreateSubmit = false)
        {
            string sSubCondorLocal = HTC_CONSTANTS._sSUBMIT_FILE;                // overwrite with path if needed for UNC execution
            if (!bSkipCreateSubmit)
                WriteSubmitFile();

            if (_htcJobSpecActive._IsUNC)
            {
                sSubCondorLocal = HTC_CONSTANTS._sSUBMIT_FILE;
            }
            string sBatchRun = CreateCondorBat(sSubCondorLocal);            //write batch file to submit condor job
            CommonUtilities.cuRunBatchFile(sBatchRun, false);               // run bat process
        }

        private string CreateCondorBat(string sCondorSubmitFile)
        {
            string sBatchRun = Path.Combine(_htcJobSpecActive._sActiveHTCDir, HTC_CONSTANTS._sCONDOR_BAT_FILE);
            using (StreamWriter writer = new StreamWriter(sBatchRun))
            {
                writer.WriteLine("cd %~dp0");              //change to current location
                writer.WriteLine("condor_submit " + sCondorSubmitFile);     //pass arg because may require special case if UNC
                // writer.WriteLine("pause");
            }
            return sBatchRun;

        }

        //write current submit_spec to file. 
        private void WriteSubmitFile()
        {
            string sSubmitFileName = Path.Combine(_htcJobSpecActive._sActiveHTCDir, HTC_CONSTANTS._sSUBMIT_FILE);
            StreamWriter file = new StreamWriter(sSubmitFileName);
            using (file)
            {
                if (_htcJobSpecActive._htcSubmitSpec._sUniverse != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("universe = " + _htcJobSpecActive._htcSubmitSpec._sUniverse);
                if (_htcJobSpecActive._htcSubmitSpec._sExecutable != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("executable = " + _htcJobSpecActive._htcSubmitSpec._sExecutable);
                if (_htcJobSpecActive._htcSubmitSpec._sRequirement != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("requirements = " + _htcJobSpecActive._htcSubmitSpec._sRequirement);
                if (_htcJobSpecActive._htcSubmitSpec._sOutput != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("output = " + _htcJobSpecActive._htcSubmitSpec._sOutput);
                if (_htcJobSpecActive._htcSubmitSpec._sError != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("error = " + _htcJobSpecActive._htcSubmitSpec._sError);
                if (_htcJobSpecActive._htcSubmitSpec._sLog != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("log = " + _htcJobSpecActive._htcSubmitSpec._sLog);
                if (_htcJobSpecActive._htcSubmitSpec._sShould_transfer_files != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("should_transfer_files = " + _htcJobSpecActive._htcSubmitSpec._sShould_transfer_files);
                if (_htcJobSpecActive._htcSubmitSpec._sWhen_to_transfer_output != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("when_to_transfer_output = " + _htcJobSpecActive._htcSubmitSpec._sWhen_to_transfer_output);
                if (_htcJobSpecActive._htcSubmitSpec._sTransfer_input_files != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("transfer_input_files = " + _htcJobSpecActive._htcSubmitSpec._sTransfer_input_files);
                if (_htcJobSpecActive._htcSubmitSpec._sInitialDir != CommonUtilities._sDATA_UNDEFINED)
                    file.WriteLine("initdir = " + _htcJobSpecActive._htcSubmitSpec._sInitialDir);


                file.WriteLine("queue");
            }
        }

        //todo : consider how this will work differently when driven by SimLink

        private void HTC_PrepareFiles()
        {
            string[] sFiles;

            //////////////STEP 1: Transfer the needed files
            if (_htcJobSpecActive._htcSubmitSpec._sShould_transfer_files.ToLower() == "YES")
            {
                sFiles = _htcJobSpecActive._htcSubmitSpec._sShould_transfer_files.Split(';');   // bojangles- this is not set up to work yet......
                foreach (string sFile in sFiles)
                {
                    string sTarget = _htcJobSpecActive._sActiveHTCDir + System.IO.Path.GetFileName(sFile);
                    if (File.Exists(sFile) && !File.Exists(sTarget))
                    {
                        File.Copy(sFile, sTarget);
                    }
                    else
                    {
                        //log the error 
                    }
                }
            }

            /////////////STEP 2: Transfer the executable if needed
            string sExecutableClientLocation = "NONE";
            switch (1)
            {
                case 0:

                    break;
                case 1:
                    sExecutableClientLocation = "BOJANGLES";        //sLOC_SWMM;
                    break;
                case 4:
                    // 'no exe needed
                    break;

            }
            if (sExecutableClientLocation != "NONE")            //make file transfer as needed
            {
                string sEXE_Target = _htcJobSpecActive._sActiveHTCDir + System.IO.Path.GetFileName(sExecutableClientLocation);
                if (File.Exists(sExecutableClientLocation) && !File.Exists(sEXE_Target))
                {
                    File.Copy(sExecutableClientLocation, sEXE_Target);
                }
            }
        }

        #region OUTDATED- needs to be fixed
        //overloaded HTC init: pass a dictionary with relevant information

        //called by ISIS 2D_ commented out for now
        public void InitHTC_Vars(Dictionary<string, string> dictCHTC_Input, string sHTC_DIR, int nStandardConfiguration = -1, bool bJobIsSimLink = false, int nTemplate = 1)
        {
            //met 8/42/2013:  this is now done in constructor          HTC_InitCommon(sHTC_DIR, bJobIsSimLink, nTemplate);
            //      UpdateHTC_DictionaryByDictionary(dictCHTC_Input);
            //   _sActiveHTCDir = getWorkingDir();
            //   condorStandardConfiguration();

        }

        #endregion

        #region workingDir

        //met todo: understand distinction between this and update working dir.. why isn't this done a single time?
        public string GetWorkingDir()
        {
            if (!_htcJobSpecActive._bIsSimLink && _htcJobSpecActive._bCreateCondorRunFolder)
            {
                string sGUID = System.Guid.NewGuid().ToString();
                string sDir = _htcJobSpecActive._sActiveHTCDir + sGUID + "\\";
                //initialdir is not required; do not include if not requested.
                if (_htcJobSpecActive._htcSubmitSpec._sInitialDir != CommonUtilities._sDATA_UNDEFINED)
                    sDir = sDir + "\\" + _htcJobSpecActive._htcSubmitSpec._sInitialDir;
                Directory.CreateDirectory(sDir);
                return sDir + "\\";
            }
            else
            {
                return _htcJobSpecActive._sActiveHTCDir;           //working directory is the SimLink scenario directory   (or run from submit loc)
            }

        }

        //met 8/24/2013: activeDir may not be set by getWorkingDir if using model-specific generation... 
        public void UpdateWorkingDir(string sTargetFile, bool bOverWriteIfAlreadySet = false)
        {
            if ((_htcJobSpecActive._sActiveHTCDir == CommonUtilities._sDATA_UNDEFINED) || bOverWriteIfAlreadySet)
            {
                if (CommonUtilities.cuIsFullPath(sTargetFile))
                {
                    _htcJobSpecActive._sActiveHTCDir = Path.GetDirectoryName(sTargetFile) + "\\";
                }
                else
                {
                    _htcJobSpecActive._sActiveHTCDir = Directory.GetCurrentDirectory() + "\\";
                }
            }
        }
        #endregion

        //met 2/3/2014: quick adapt of this code from htc_swmm
        //this runs a particular simlink command ... (like process EG or something)
        // round 1: very limited, files must be accessible on server, working directory is not generated
        // 2/5/14- revised to expect the user to supply some of this ish  (round 1)
        public virtual void PreProcessCondorJob(string[] sFileNames, string[] args)
        {
        }
        /*       public virtual void PreProcessCondorJob(string[] sFileNames, string[] args)
               {
                   string sTargetInputFile = sFileNames[0];
                   updateWorkingDir(sTargetInputFile, false);                //set the activeDir (if already set, it will not be modified)

                   CreateBat(_sActiveHTCDir, sTargetInputFile, _SimType);
                   SetExecutable(sTargetInputFile);          //add to the dictionary
                   SetRequirements();
                   SetFilesToTransfer(ref sFileNames);
                   UpdateHTC_DictionaryByDictionary(_dictCHTC_ModelSpec);
               }
               */


        //do application specific things to prepare the Condor 
        public void condorStandardConfiguration()
        {
            switch (1)              //nStandardConfig)
            {
                case -1:
                    //todo: log that there are no files to be managed
                    break;
                case 1:
                    /* 
                    
                     string sTarget_INP = dictCHTC_Spec["transfer_input_file"];
                     string sArg = System.IO.Path.GetFileName(sTarget_INP) + " " + sTarget_INP.Replace(".inp", ".rpt") + " " + sTarget_INP.Replace(".inp", ".out");
                     string[] s = new string[] { "swmm5.exe " + sArg };
                     string sBat = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(sTarget_INP), "run_swmm5.bat");
                     File.WriteAllLines(sBat, s); 
                     */
                    break;
            }
        }


        #region CONDOR JOB QUERY
        //v1 of this function assumes ONE set of log information per run.log , ie DAG not supported at this point
        public string GetCondorJobStatus(string sFilePath, out string sCondorID, out DateTime dtCondorDate, out string sHost, out int nReturnValue)
        {
            string sCode = "xxx"; string sbuf; string sDateTemp = "";
            sHost = "NONE";
            dtCondorDate = System.DateTime.FromFileTime(1);
            nReturnValue = -1;
            sCondorID = "-1";
            StreamReader file = null;
            try
            {
                file = new StreamReader(sFilePath);
                while (!file.EndOfStream)
                {
                    sbuf = file.ReadLine();
                    if (htcLog_IsDataRow(sbuf))
                    {
                        sCode = sbuf.Substring(0, 3);
                        sDateTemp = sbuf.Substring(18, 5) + @"/" + System.DateTime.Now.Year.ToString() + sbuf.Substring(23, 9); //yr not recorded; add current year (could be wrong!)
                        dtCondorDate = Convert.ToDateTime(sDateTemp);
                        sCondorID = sbuf.Substring(5, 11);

                        switch (sCode)
                        {
                            case "000":
                                sHost = sbuf.Substring(sbuf.IndexOf("host: <") + 7, 18);
                                break;
                            case "001":
                                sHost = sbuf.Substring(sbuf.IndexOf("host: <") + 7, 18);
                                break;
                            case "005":
                                sbuf = file.ReadLine();
                                sbuf = sbuf.Substring(sbuf.IndexOf("return value") + 13, 1);
                                nReturnValue = Convert.ToInt32(sbuf);
                                break;
                        }
                    }
                }
                return sCode;
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }

        private bool htcLog_IsDataRow(string sBuf)
        {
            bool bReturn = false;
            if (sBuf.Length > 3)
            {
                if (sBuf.Substring(0, 2) == "00")
                {
                    bReturn = true;
                }
            }
            return bReturn;
        }

        #endregion

        #region help

        public List<string> htcGetHelp(string sArg, bool bWriteToConsole = true)
        {
            List<string> lstReturn = new List<string>();
            switch (sArg.ToLower())
            {
                case "true":
                    lstReturn.Add("Welcome to CirrusHTC interface for high-throughput comuting");
                    lstReturn.Add("Developed by the CH2M HILL WBG, 2013. Version 1.0.");
                    lstReturn.Add("You must be on the CH2M HILL domain to run CirrusHTC");
                    lstReturn.Add("Specific info: CirrusHTC -help <platform>, e.g. SWMM, ExtendSim etc...");
                    lstReturn.Add("*********************************");
                    lstReturn.Add("CirrusHTC helps facilitate creation and submission of CondorHTC jobs.");
                    lstReturn.Add("Condor must be installed on your computer to use the CLI");
                    lstReturn.Add("****************   Parameters      *****************");
                    lstReturn.Add("_____    ____________________________________________");
                    lstReturn.Add("-f       File(s) to be included in Condor job. Required. ");
                    lstReturn.Add("         Comma separated list with no spaces");
                    lstReturn.Add("-req     Custom requirements in Condor format. CirrusHTC will define basic parameters, but the user can define more specific requirements. type cirrushtc -help req for examples");
                    lstReturn.Add("         type cirrushtc -help req for examples");
                    lstReturn.Add("-nosub   (NI) Create but do not submit to the pool.");
                    break;
                case "modflow":
                    lstReturn.Add("CirrusHTC supports Modflow simulations.");
                    lstReturn.Add("Input1: model.bat: this must unzip files.7z, execute any required files, and then (optionally) recompress the results into a results.7z archive");
                    lstReturn.Add("Input2: files.7z:  includes all necessary model files and exe");
                    lstReturn.Add("Ouput1: results.7z:  compressed folder for model ouptput");
                    lstReturn.Add("Zipping note: example batch code or including in model.bat");
                    lstReturn.Add("              Unzip: '7z x files.7z'");
                    lstReturn.Add("              Zip (and exclude): '7z a -t7z results.7z *.* -x!files.7z'");
                    lstReturn.Add("Future functionality: increased integration with SimLink for ModFlow automation");
                    break;
                case "swmm":
                    lstReturn.Add("CirrusHTC supports EPA-SWMM simulations.  ");
                    lstReturn.Add("Input1: filename.inp");
                    lstReturn.Add("Outputs: filename.rpt, filename.out");
                    lstReturn.Add("Advanced functionality: CirrusHTC can be used with SimLink to automate");
                    lstReturn.Add("SWMM evaluations. Type cirrushtc -help simlink for more information");
                    lstReturn.Add("SWMM Params");
                    lstReturn.Add("-out     (NI) identify specific filenames to be passed back");
                    lstReturn.Add("         Not required; enables passback of specific file only");
                    break;
                case "extendsim":
                    lstReturn.Add("CirrusHTC supports ExtendSim imulations. The standard configuration for a CirrusHTC SWMM job is for the user to provide a .mox file and any .lix libraries required.");
                    lstReturn.Add("Because of the way Condor processes files, the input files must either be zipped or renamed during the operation (this is odd, but a requirement  for any condor job where no output file is gneerated- e.g. Excel");
                    lstReturn.Add("Input1: filename.mox; any .lix files should be referenced as well");
                    lstReturn.Add("Example: cirrushtc -extendsim -f file1.mox,file1.lix");
                    break;
                case "condor":
                    lstReturn.Add("CirrusHTC is a wrapper around HTCondor, an open-source platform for high throughput computing. CirrusHTC is  customized to support frequently perfomrmed simulation jobs");
                    lstReturn.Add("Additional information about HTCondor is available at http://research.cs.wisc.edu/htcondor/ ");
                    lstReturn.Add(@"Test that Condor is running by typing 'condor_status', or 'condor_q'. If it is not running, go to Computer\Manage\Services and start the Condor service");
                    lstReturn.Add("Condor commands can be run from the command line to query the current status of the pool.");
                    lstReturn.Add("condor_submit filename       Submit condor job");
                    lstReturn.Add("condor_history      get history of jobs submittd from that machine");
                    lstReturn.Add("condor_q             name condor jobs that are waiting for execution");
                    lstReturn.Add("condor_q -analyze    Analyze why a specific job has not executed");
                    break;
                case "simlink":
                    lstReturn.Add("todo");
                    break;
                case "req":
                    lstReturn.Add("todo");
                    break;
            }
            if (bWriteToConsole)
            {
                foreach (string s in lstReturn)
                {
                    Console.WriteLine(s);
                }
            }
            return lstReturn;
        }

        #endregion
    }



}
