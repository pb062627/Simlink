using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIM_API_LINKS;
using Nini.Config;
using System.IO;    

namespace SimLink_CLI
{

    /// <summary>
    /// MET 4/11/17: add partial class for segregating workflow and making easier to manage.
    ///     todo: consider stronger db structure
    /// </summary>
    public partial class Program
    {

        public static void ExecutePrimaryFunction(ref simlink mySimlink, IConfig config, string sPrimaryFunction)
        {
            Console.WriteLine("<------------   Begin execution of special function {0}    ------------->", sPrimaryFunction);

            switch (sPrimaryFunction)
            {
                case "delete":
                    Delete(ref mySimlink, config);
	                break;
                case "batch":
                    Batch(ref mySimlink, config);
                    break;
                case "read_out":
                    ReadOut(ref mySimlink, config);
                    break;
                case "run":
                    Run(ref mySimlink, config);
                    break;
                case "export_schema_to_xml":
                    ExportSchemaToXML(ref mySimlink, config);
                    break;
                case "push":
                    Push(ref mySimlink, config);
                    break;
                case "external_data":
                    GetExternalData(config);
                    break;
                case "synth":
                    SynthTimeSeries(ref mySimlink, config);
                    break;
                case "version":
                    GetVersion();
                    break;
            }
        }

        public static void SyncConfigs(ref IConfigSource configXML, IConfig config){

            int i = 0;
            string[] sVals  = config.GetValues();
            foreach(string s in config.GetKeys())
            {
                if (configXML.Configs["simlink"].Contains(s))
                {
                    Console.WriteLine("Note: command line arg duplicated in config; config file value superceds");
                }
                else
                {
                    configXML.Configs["simlink"].Set(s,sVals[i]);       // add the cli arg to the config.
                }
                i++;
            }
        }

        /// <summary>
        /// Update version when significant changes made. todo: come up with better way to track version and tie to db
        /// </summary>
        public static void GetVersion()
        {
            Console.WriteLine("Simlink version 1.1.0 build 4/9/2019");
        }

        public static void SynthTimeSeries (ref simlink mySimlink, IConfig config)
        {
            int nStartScenario = config.GetInt("scenario_start", -2);
            int nEndScenario = config.GetInt("scenario_end", -2);
            int nEval_Push = config.GetInt("eval_synth", -2);
            if ((nStartScenario == -2) || (nEndScenario == -2) || (nEval_Push == -2))
            {
                Console.WriteLine("scenario_start, scenario_end, and eval_synth must be set in xml config.");
            }
            else
            {
                Console.WriteLine("Synthesizing ts data from scenario {0} to {1} into eval {2}", nStartScenario, nEndScenario, nEval_Push);
                mySimlink.SynthTimeSeries_Head(nStartScenario, nEndScenario, nEval_Push);
            }
        }


        /// <summary>
        /// Extract data from an external data source
        /// </summary>
        /// <param name="config"></param>
        public static void GetExternalData(IConfig config)
        {
            string sVal = "web_provider";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            SIM_API_LINKS.ExternalData ex = Commons.GetExternalData(sVal, dict);
            Commons.GetDictValsFromConfig(config,dict);
            ex.SetKeyDataValsByDict_AfterCreate(dict);
            if (dict.ContainsKey("file_out"))
            {
                ex._sFileOut = dict["file_out"];
            }
            ex._bWriteIntermediate = true;
            double[][,] dVals = ex.RetrieveData(null, new int[]{1,2});    // todo paramterize
            Console.WriteLine("Writing {0} series to file {1}",dVals.GetLength(0),ex._sFileOut);
        }

        public static void Delete(ref simlink mySimlink, IConfig config)
        {
            //var config = config.Configs["Base"];
            string sScenConcat = config.GetString("scen", "");
            bool bDeleteAll = config.Contains("delete_all");
            int nEval = config.GetInt("delete", -1);
            bool bSpecial = config.Contains("special");
            int nStart = config.GetInt("start", -1);
            int nEnd = config.GetInt("end", 100);
            mySimlink.DeleteScenariosWrapper(true, nEval, nStart, nEnd, sScenConcat, bSpecial, bDeleteAll);
        }

        public static void Batch(ref simlink mySimlink, IConfig config)
        {
            //var config = config.Configs["Base"];
            bool bRunAll = config.Contains("run_all");
            int nEval2 = config.GetInt("evaluationgroup", -1);
            int nRunsPerBatch = config.GetInt("batch", 10);
            if (nEval2 == -1)
            {
                Console.WriteLine("try simlink -batch [runs_per_batch] -config [config_loc- includes EG]");
            }
            else
            {
                mySimlink.CreateBatchFile_ByEval(nEval2, nRunsPerBatch);                //todo support more specific exe calls?
            }        
        }
        public static void ReadOut(ref simlink mySimlink, IConfig config)
        {
            //var config = config.Configs["Base"];
            string sOUT = config.GetString("file", "UNDEFINED");
            int nScen = config.GetInt("scen", -1);
            bool bArraylike = config.Contains("arraylike");
            if (bArraylike)
                mySimlink.SetTSOutToArrayLike();
            Console.WriteLine("Begin ts read at {0}", System.DateTime.Now.ToString());
            mySimlink.ReadTimeSeriesOutput(mySimlink._nActiveEvalID, nScen, sOUT);
        }

        public static void Run(ref simlink mySimlink, IConfig config)
        {
            //var config = config.Configs["Base"];
            string sDNA = config.GetString("dna", "UNDEFINED");
            double dObjective = -1;  // todo: change scenarios to return double
            if (sDNA != "UNDEFINED")
            {
                Console.WriteLine("begin process eval {0} w dna {1}: {2}", mySimlink._nActiveEvalID, sDNA, System.DateTime.Now.ToString());
                mySimlink.ProcessScenarioWRAP(sDNA, -1, -1, 100);
                dObjective = mySimlink.GetObjectiveVal();
                Console.WriteLine("#Objective:" + dObjective.ToString());
                mySimlink.CloseModelLinkage();
            }
            else
            {
                dObjective = -666;
                //    mySimlink._log.AddString("test", 1, true);
                Console.WriteLine("Must add -dna flag with run request");
            }
        }

        public static void ExportSchemaToXML(ref simlink mySimlink, IConfig config)
        {
            //var config = config.Configs["Base"];
            string sDir = config.GetString("output_path", "UNDEFINED");
            if (sDir == "UNDEFINED")
                sDir = Directory.GetCurrentDirectory();
            mySimlink.WriteXML(sDir, "nada");
            Console.WriteLine("XML exported to directory: {0}", sDir);
        }

        public static void Template(ref simlink mySimlink, IConfig config)
        {
            //var config = config.Configs["Base"];

        }


        /// <summary>
        /// Take an input file/model/folder (future, built for SWMM case) and push into scenario
        /// optionally, execute
        /// 
        /// 7/24/17- made this a function in simlink. kept this function, but potentially need to convert to wrapper only
        /// </summary>
        /// <param name="mySimlink"></param>
        /// <param name="config"></param>
        public static void Push(ref simlink mySimlink, IConfig config, bool bCopyAllFilesInDirectory = false)
        {
            string sLabel = config.GetString("label", System.DateTime.Now.ToString());
            string sFile = config.GetString("file","UNDEFINED");    //HAS TO BE IN SIMLINK config for now.. not ideal!!
            int nScenStart = config.GetInt("start", 5);
            int nScenEnd = config.GetInt("end", 20);
            bool bNoExecute = false;        // todo: figure out how to say this  config.GetString("no_run", "n");
            if(File.Exists(sFile)){
            
            // insert your scenario
            int nScenarioID = mySimlink.InsertScenario(sLabel);
            // copy file to target location  - todo manage all directory files...
            mySimlink.SetupScenarioFiles(sFile, nScenarioID);


            // RUN THE SCENARIO
            mySimlink.ProcessScenario(mySimlink._nActiveProjID, mySimlink._nActiveEvalID, mySimlink._nActiveReferenceEvalID, mySimlink._sActiveModelLocation, nScenarioID, nScenStart, nScenEnd);

            }
            else{
                mySimlink._log.AddString(string.Format("File {0} does not exist. Please pass a valid filename.", sFile), SIM_API_LINKS.Logging._nLogging_Level_3, true, true);
            }
        }

    }
}
