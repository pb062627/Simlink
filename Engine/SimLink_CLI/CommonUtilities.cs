using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SIM_API_LINKS;
using System.IO;


namespace SimLink_CLI
{
    //met 1/29/2014- I have set this up to house functionality needed for 
    
    public class CommonUtilities
    {
        #region INIT
        public static void InitializeSimLinkFromCLI(ref simlink simlink, string sPlatform){
            int nActiveModelType = SIM_API_LINKS.CommonUtilities.GetModelTypeIDFromString(sPlatform);
            if (nActiveModelType > 0)
            {
                simlink = simlink.GetSimLinkObjByModelType(nActiveModelType);
            }
            else
            {
                Console.WriteLine(sPlatform + " is not a supported platform. Please check spelling and resubmit");
            }
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


        // add cli to a basic nini
            // not 100% sure, but thinsk this avoids the need for the modified nini build.
        public static void AddCommandLineSwtiches(string[] args, Nini.Config.ArgvConfigSource source)
        {
            foreach (var item in args)
            {
                if (item.StartsWith("-"))
                {
                    source.AddSwitch("Base", item.Substring(1));
                }
            }
        }
        #endregion

        #region CONDOR_PROCESS_EG
        public static void ProcessSimLinkEG(ref SIM_API_LINKS.simlink simlink, string[] args, int nEvalID)
        {   
            //1: Get File which includes the SimLink connection information
            string sSimLinkFile = Directory.GetCurrentDirectory() + "\\simlink.con";         //todo- potentially get in other fashion
            if (true)               //File.Exists(sSimLinkFile))
            {
                string sPlatform = args[0];
                
                
                bool bCondor = false;
                if (Array.IndexOf(args,"-condor") > 0 )
                {
                    bCondor = true;
                }
                simlink.InitializeHTC_ByPlatform(-1);             //initialize the HTC object
                string[] sFileNames = simlink._htc.InitArgDict(args);
                if (bCondor){
  
                    if (sFileNames.Length > 0)
                    {
                        simlink._htc._htcJobSpecActive._sActiveHTCDir = SIM_API_LINKS.CommonUtilities.GetWorkingDirectoryFromFile(sFileNames[0]) + "\\";  //m
              //met- this may not be correct; to be evaluated          simlink._bIsSimCondor = bCondor;                //set the Condor flag
              //          sSimLinkFile = SIM_API_LINKS.CommonUtilities.GetWorkingDirectoryFromFile(sFileNames[0]) + "\\simlink.con";      //get the correct file locati0n
                   
                        //     simlink.InitializeConn_ByFileCon(sSimLinkFile);
                     //supplied by user   simlink._htc.PreProcessCondorJob(sFileNames, args);
                        simlink._htc.SubmitCondorJob(true);

                    }
                }
                else
                {       //execute simlink without condor
                        //use the current directory for the simlink file (will not work so well in debugging)
                    if (sFileNames.Length > 0)      //you must tell simlink where to findthe .con...
                    {
                        InitializeSimLinkFromCLI(ref simlink, sPlatform);
                        simlink._bIsSimCondor = bCondor;                //set the Condor flag
                        sSimLinkFile = SIM_API_LINKS.CommonUtilities.GetWorkingDirectoryFromFile(sFileNames[0]) + "\\simlink.con";      //get the correct file locati0n
                        simlink.InitializeConn_ByFileCon(sSimLinkFile);
                        simlink.InitializeEG(nEvalID);           //todo get EG from args you dipshit
                        simlink.ProcessEvaluationGroup(new string[0]);      //MET: do not like this. PEG shoudl not require a param.
                        simlink.CloseModelLinkage();
                    }
                    else
                    {
                        Console.Write("SimLink EG requires '-f filename.ext' to identify where simlink.con init file is located");
                    }
                    
                }
            }
            else
            {

            }
            
            
            
        }
        #endregion

        #region CONDOR_PROCESS_SINGLE

        public static void ProcessSingleCondorRun(ref SIM_API_LINKS.simlink simlink, string[] args){
            string[] sFileNames = null;
            int nActiveModelType = SIM_API_LINKS.CommonUtilities.GetModelTypeIDFromString(args[0]);
            simlink.InitializeHTC_ByPlatform(nActiveModelType);             //initialize the HTC object
            sFileNames = simlink._htc.InitArgDict(args);
            simlink._htc.PreProcessCondorJob(sFileNames, null);
            simlink._htc.SubmitCondorJob();
        }

        #endregion


    }
}
