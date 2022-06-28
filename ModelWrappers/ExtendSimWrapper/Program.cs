using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
//using Nini.Config;

namespace ExtendSimWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            //SP 25-Jul-2016 this code can be used for reading an XML file for tidier passing of arguments - for now, restrict to standard CLI arguments 
            /*var source = new ArgvConfigSource(args);
            CommonUtilities.AddCommandLineSwtiches(args, source);         
            var config = source.Configs["Base"];
            IConfigSource configXML = null;

            if (config.GetKeys().Contains("config"))
            {
                try
                {
                    configXML = new XmlConfigSource(config.GetString("config"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("xml not found: " + config.GetString("config"));
                    Console.WriteLine("Press your favorite key to continue");
                    Console.ReadKey();
                    return;
                }*/

            // if this point is reached, we have a config. (whether it is valid isn't assured)
            //string sModelFileLocation = configXML.Configs["ExtendSim"].GetString("modelfilelocation", "");

            //string sModelFileLocation = @"\\hchfpp01\Groups\WBG\TRWD\Model\Replica\TRWD_IPL_Phase1A\TRWD_IPL_Phase1A.mox";
            string sModelFileLocation = @"\\HCHFPP01\groups\WBG\TRWD\Model\Replica\Replica_CondorTest\TRWD_IPL_Phase1A.mox";
            //string sModelFileLocation = @"\\HCHFPP01\groups\WBG\TRWD\Model\Replica\Replica_CondorTest\Calibration\Wilimington_Treatment_Plant_2021_8-6_WR_MODS.mox";

            if (args.Count() > 0)
                sModelFileLocation = args[0];

            if (sModelFileLocation != "")
            {
                Console.WriteLine("Beginning opening, executing and saving ExtendSim model file:" + sModelFileLocation); // let user know we begin init.

                //open model
                Console.Write("opening model...");
                ExtendSimFunctions.EXTEND_OpenExtendInstance(sModelFileLocation);
                Console.WriteLine("completed");
                        
                //run simulation
                Console.Write("running simulation...");
                ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.RunSimulation);
                Console.WriteLine("completed");
                       
                //save model
                Console.Write("saving model...");
                ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.SaveModel);
                
                //pause before closing to ensure save was completed - sometimes save takes longer as control is passed back to simlink immediately after initiating the SaveModel 
                Thread.Sleep(5000);
                Console.WriteLine("completed");

                //close the model
                Console.Write("closing model...");
                ExtendSimFunctions.EXTEND_Execute(ExtendExecuteCommandType.MenuCommand, new object[] { ExtendMenuCommandType.Close });
                Console.WriteLine("completed");
            }
            else
            {
                Console.WriteLine("No ExtendSim model file specified in config");
            }

            //just close after finishing the execution
            //Console.WriteLine("Press your favorite key to continue");
            //Console.ReadKey();
        }
    }
}
