using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using VISSIMLIB;
//using Nini.Config;

namespace VissimWrapper
{
    class Program
    {
        static void Main(string[] args)
        {

            string sModelFileLocation = "";
            if (args.Count() > 0)
                sModelFileLocation = args[0];

            if (sModelFileLocation != "")
            {
                Console.WriteLine("Beginning opening, executing and saving Vissim model file:" + sModelFileLocation); // let user know we begin init.

                //create Vissim Instance

                VissimObject Vissim = new VissimObject();


                //not sure if this is needed - load a layout
                //string LayoutFilename = @"C:\Users\Public\Documents\PTV Vision\PTV Vissim 9\Examples Demo\Roundabout London.UK\" + "Roundabout London.layx";
                //Vissim.LoadLayout(LayoutFilename);

                //open model
                Console.Write("opening model...");
                Vissim.VISSIM_OpenModel(sModelFileLocation, true);
                Console.WriteLine("completed");

                //set simulation time to 100
                int nRunTimeParam = Vissim.VISSIM_GetRunTimeParameter();
                Vissim.VISSIM_SetRunTimeParameter(600);

                //change the desired speed distribution
                //Vissim.VISSIM_PokeVal("14", "MinGapTime", "10", VissimElements.ConflictMarker);

                //run simulation
                Console.Write("running simulation...");
                Vissim.VISSIM_RunSimulationToNextBreakPoint(0);
                Console.WriteLine("completed");

                //read the queue counter
                object nQLen = Vissim.VISSIM_Request("253251", "QLen(Avg,Avg)", VissimElements.QueueCounter);
                object nQLenMax = Vissim.VISSIM_Request("253251", "QLenMax(Avg,Avg)", VissimElements.QueueCounter);

                object nVolume = Vissim.VISSIM_Request("25", @"Concatenate:LinkEvalSegs\Volume(Avg, Last, All)", VissimElements.LinkEvalSegment);

                object nTravTm = Vissim.VISSIM_Request("243402", "TravTm(Avg,Avg,All)", VissimElements.VehicleTravelTimeMeasurement);

                //save model
                Console.Write("saving model...");
                Vissim.VISSIM_SaveModel(@"C:\Users\Public\Documents\PTV Vision\PTV Vissim 9\Examples Demo\Roundabout London.UK\Test.inpx");
                
                //pause before closing to ensure save was completed - sometimes save takes longer as control is passed back to simlink immediately after initiating the SaveModel 
                //Thread.Sleep(5000);
                Console.WriteLine("completed");

                //close the model
                Console.Write("closing model...");
                Vissim.VISSIM_CloseApplication();
                Console.WriteLine("completed");
            }
            else
            {
                Console.WriteLine("No Vissim model file specified in config");
            }

        }
    }
}
