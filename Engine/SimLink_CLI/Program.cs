using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRRUS_HTC_NS;
using SIM_API_LINKS;
using Nini.Config;
using OptimizationWrapper;
using Optimization;             // this is only here for the ArgsToSource big..  todo: this would be better placed in OptWrapper.CommmonUtilities

namespace SimLink_CLI
{
    #region TEST ARGS
    //swmm -f C:\Users\Mason\Documents\Optimization\Condor\JobCenter\SimLinkEG\Example1.inp -eg 254    
    //swmm -f C:\Users\Mason\Documents\Optimization\Condor\JobCenter\SimLinkEG\Example1.inp -eg 254 -condor   (will submit the condor job)
    //epanet -f C:\Users\Mason\Documents\Optimization\Condor\JobCenter\SimLinkEG\Example1.inp -eg 254
    //epanet -f C:\Users\Mason\Documents\Optimization\Condor\JobCenter\EPANET\run_epanet.bat,C:\Users\Mason\Documents\Optimization\Condor\JobCenter\EPANET\files.7z
    //isis2d -f files.7z 

    #endregion 


    public partial class Program
    {
        static void Main(string[] args)
        {
            bool bReadKey = true; 
            if (true){              //Validation.ValidateDomainName("CH2MHILL")){
                if (args.Length > 0)
                {
                    var source = new ArgvConfigSource(args);
                    
                    CommonUtilities.AddCommandLineSwtiches(args, source);         // read flags into a var called source
                    var config = source.Configs["Base"];
                    IConfigSource configXML = null;
                    bReadKey = !config.GetKeys().Contains("silent");
             // retiring dictArgs       Dictionary<string, string> dictArgs = CommonUtilities.Arguments_ToDict(args);           //todo: avoid processing the arg array twice
                    if (false)  //todo: see if help is passed...            dictArgs.ContainsKey("help"))                                                       //simple request for help
                    {
                        // rethink this- is the htc object the place to store the help?
                        SIM_API_LINKS.simlink simlink = new SIM_API_LINKS.simlink();
                        simlink._htc = new CIRRUS_HTC_NS.CIRRUS_HTC();
                // todo: replace w non dictArgs usage        simlink._htc.htcGetHelp(dictArgs["help"], true);
                    }
                    else
                    {
                        if (config.GetKeys().Contains("config"))                    //met 11/15: everythign interesting from cli requires a config.
                        {
                            try
                            {
                                configXML = new XmlConfigSource(config.GetString("config"));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("xml not found or ill-formed; please verify XML input: " + config.GetString("config"));
                                Console.WriteLine("Press your favorite key to continue");
                                Console.ReadKey();
                                return;                 //nothing wworks wout config, so end.
                            }
                            // if this point is reached, we have a config. (whether it is valid isn't assured)
                            SyncConfigs(ref configXML, config);

                            if(configXML.Configs["simlink"].GetString("silent","n").ToLower()=="y")         //add ability to set in xml...
                                bReadKey=false;

                            if (true)           //config.GetKeys().Contains("Simlink"))
                            {
                                if (IsRealTimeRun(configXML.Configs["simlink"]))      // a realtime run - init simlink THROUGH the realtime init (not oni its own. 
                                {                                                     // observation 4/11/17: This will run even if you have a special function... so need to remov
                                    bool bValid = false;
                                    realtime rt = realtime.InitializeByConfig(configXML, out bValid);            //   new realtime(1, nEvalID, nTriggerMethod, sConn, 0, dictArgs);
                                    //SP 14-Apr-2017 based on new config argument defined_run_dates
                                    if (rt._bRunDefinedDates)
                                        rt.RunDefinedDates();
                                    else
                                        rt.Run(1);   // assuming that we have the 

                                    //SP 8-Sept-2017 Incorporate the option to write to DSS

                                    rt.Close();
                                }
                                else
                                {
                                    simlink mySimlink = SIM_API_LINKS.CommonUtilities.GetSimLinkObject(configXML.Configs["simlink"].GetString("type", "simlink"));

									mySimlink._sConfigFileLocation = config.GetString("config");


									Console.WriteLine("Beginning initialization using file:" + config.GetString("config")); // let user know we begin init.

                                    bool bValidInit = mySimlink.InitializeByConfig(configXML);
                                    string sPrimaryFunction = "undefined";
                                    if (!CheckContainsSpecialAction(config, ref sPrimaryFunction))
                                    {
                                        switch (mySimlink._runType)
                                        {
                                            case SimlinkRunType.Optimization:
                                                // met to get correct opt lib on phl..           Console.WriteLine("Beginning optimization. Algorightm:" + mySimlink._optWrapper._optAltoActive.ToString());     //note: change to _optAlgoActive
                                                mySimlink._optWrapper.Execute();
                                                break;
                                            case SimlinkRunType.UserDefinedRuns:
                                                string[] sScenarios = new string[0];
                                                if (configXML.Configs["simlink"].GetKeys().Contains("scenarios"))
                                                {
                                                    sScenarios =configXML.Configs["simlink"].GetString("scenarios").Split(',');
                                                }
                                                if (configXML.Configs["simlink"].GetKeys().Contains("cohort"))
                                                {
                                                    int nCohort = configXML.Configs["simlink"].GetInt("cohort",-1);
                                                    int nProject = configXML.Configs["simlink"].GetInt("project", -1);
                                                    if((nCohort<=0) || (nProject<=0))
                                                    {
                                                        Console.WriteLine("Both cohort and project ids are requied (>0)");
                                                    }
                                                    else
                                                    {
                                                        // step 1: Load the cohort.. this should be done on init, and not redone- but we don't generally have project
                                                        mySimlink._dsEG_Cohort = mySimlink.EG_GetCohortDS(nCohort, nProject,true);  //pass cohort- even though first arg is called eval.
                                                        Console.WriteLine("Begin processing cohort {0}", nCohort);
                                                        mySimlink.ProcessCohort(nProject, nCohort);
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    mySimlink.ProcessEvaluationGroup(sScenarios);
                                                    break;
                                                }
                                        }

                                        //SP 8-Sept-2017 Incorporate the option to write to evaluation group to DSS
                                        if (mySimlink._sWriteEGToDSSLocation != "")
                                        {
                                            Console.WriteLine("Attempting to write results to DSS");
                                            mySimlink.ConvertEGCompletedResultsToDSS();
                                            Console.WriteLine("Completed writing results to DSS");
                                        }
                                    }
                                    else
                                    {
                                        //   todo: consider passing all ConfigXML
                                        // for now implementing external data, which I would prefer to place in a different section (MET 10/21/2017)

                                        ExecutePrimaryFunction(ref mySimlink, configXML.Configs["simlink"], sPrimaryFunction);  //currently only works with configXML - todo - provide method if there is no xml provided
            
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (config.GetKeys().Contains("version"))
                            {
                                GetVersion();                              
                               // Console.WriteLine("simlink installed. current time: " + System.DateTime.Now.ToString());
                            }
                            else
                            {
                                Console.WriteLine("no keys");
                            }

                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Must be on 'CH2MHILL' domain");
            }

            if (bReadKey)
            {
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// pass simlink config section
        /// return true if run is set up as a realtime.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        static bool IsRealTimeRun(IConfig config)
        {
            if (config.GetString("run_type", "undefined") == "realtime")
                return true;
            else
                return false;
        }

        // helper function to check for special actions
        // can only process one at a time..
        static bool CheckContainsSpecialAction(IConfig config, ref string sPrimaryFunction)
        {
            bool bReturn = false;
            if (config.GetKeys().Contains("delete"))
            {
                sPrimaryFunction = "delete";
                return true;
            }
            if (config.GetKeys().Contains("synth"))
            {
                sPrimaryFunction = "synth";
                return true;
            }
            if (config.GetKeys().Contains("batch"))
            {
                sPrimaryFunction = "batch";
                return true;
            }
            if (config.GetKeys().Contains("test"))
            {
                sPrimaryFunction = "test";
                return true;
            }
            if (config.GetKeys().Contains("run"))
            {
                sPrimaryFunction = "run";
                return true;
            }
            if (config.GetKeys().Contains("export_schema_to_xml"))        
            {
                sPrimaryFunction = "export_schema_to_xml";
                return true;
            }
            if (config.GetKeys().Contains("push"))
            {
                sPrimaryFunction = "push";
                return true;
            }
            if (config.GetKeys().Contains("external_data"))
            {
                sPrimaryFunction = "external_data";
                return true;
            }
            if (config.GetKeys().Contains("version"))
            {
                sPrimaryFunction = "version";
                return true;
            }

            return bReturn;
        }
       
        /*static SIM_API_Links.SLWrap SimLink = new SIM_API_Links.SLWrap();
        
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "-processeval":
                        string sResult = SimLink.SL_RMG_ProcessEval(args);
                        Console.WriteLine(sResult);
                        break;
                    case "-test":
                        Console.WriteLine(System.DateTime.Now.ToString());
                        
                        break;
                }
            }
        }*/
    }
}
