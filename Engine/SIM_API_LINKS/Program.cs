using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIM_API_LINKS;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SIM_API_LINKS
{
    class Program
    {
        static void Main(string[] args)
        {
            //      string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\myFolder\myAccessFile.accdb;Persist Security Info=False;";

            int nArg = 73;// 67; // 7;

            switch (nArg)
            {
                #region PRETTYOLD
                case 0:             //SWMM, basic EG processing
                    SIM_API_LINKS.testing.BWSC_RunEG(254);          // tied to SimClim stuff.
                    break;
                case 1:             //MODFLOW
                    testing.TEST_MODFLOW1();
                    break;
                case 2:             // basic XMODEL
                    testing.XMODEL_TEST01_Simple(8066);         //test with BWSC eg 254
                    break;
                case 3:             // basic XMODEL
                // moved to simclim_wrap folder    testing.SimClimSolo2();         //test with BWSC eg 254
                    break;
                case 4:
                    testing.ISIS2D_TEST1();
                    break;
                case 5:
                    string sFileToRead =@"Cv:\Users\Mason\Documents\WORK\Regina\Pro2D2Rev5DynamicNoRecy_Out.tsv";
                    string sFileToWrite =@"C:\Users\Mason\Documents\WORK\Regina\Pro2D2Rev5DynamicNoRecy_Out.h5";
                    testing.ReadFileToHDF5(sFileToRead,sFileToRead);
                    break;
                case 6:     //test EPA
                    
                    testing.RunEPANET_EG(258);
                //testing.TestReadOut();                  //RunEPANET_EG(258);
                    break;
                case 7:               
                //    testing.ImportINPToDB_LNC();            //import swmm - last performed 4/3/14
              //  testing.RunEG_LNC(269);        
                  //   testing.CreateBat(269, 2);
                     testing.RunEG_LNC(269); 
                    break;
                case 8:     //test XML loa d of CirrusHTC
                    testing.CirrusHTC_Test();
                    break;
                case 9:     //test XML load of CirrusHTC
                    testing.TestWriteToDSS();
                    break;
                case 10:
                    testing.TestSWMMResultRead();
                    break;
                case 11:
                //  don't know why this function disappeared...    testing.RunEG_LNC_Optimization(266);
                    break;
                case 12:
                    testing.DeleteScenarioData();
                    break;
                case 13:
                    testing.RunExcel();
                    break;
                case 14:
                    testing.TestConditional();
                    break;
                case 15:
                    testing.ImportINPToDB_NH();
                    break;
               case 16:
             //     testing.CreateBatNH(271, 2);
                    testing.RunEG_NH(271);
                    break;
               case 17:
                    testing.DeleteScenarioDataNH();
                    break;
               case 18:
                    testing.RunEG_Hartford(277);
                    break;
                case 19:
                    testing.DeleteScenarioData_HARTFORD();
                    break;
                case 20:
                    testing.TestDLL_Import();
                    break;
                case 21:
                    testing.RunOptimization_Hartford();
                    break;
                case 22:
                    testing.TestBorg();
                    break;

                case 24:
                    testing.RunEG_EPANETDemo1(54);
                    break;


                #endregion
                #region GETTING OLD
                case 25:
                    testing.TestIW();
                    break;
                case 26:
                    testing.IW_ImportModel();
                    break;
                case 27:
                    testing.TestCompactAndRepair();
                    break;
                case 28:
                    testing.PittTesting();
                    break;
                case 29:
                    testing.CloneProject();
                    break;
                case 30:
                    testing.ExtendSimTesting();
                    break;
                case 31:
                    testing.DeleteProj();
                    break;
                case 32:
                    testing.TestIW_UserDefinedCSV();
                    break;
                case 33:
                    testing.TestHPC_Class_InitByConfig_Exe();
                    break;
                case 34:
                    testing.ModifyEPANETINPForFireflow();
                    break;  
                case 35:
                    testing.CloneProject_2();
                    break;
                case 36:
                    testing.testMWRD();
                    break;
                case 37:
                    testing.testCohort1();
                    break;
                case 38:
                    testing.TransferDefaultsFromSQLServerToMSAccess();
                    break;
                case 39:
                    testing.RunEG_EPANET_Norfolk();
                    break;
                case 40:
                    testing.testCohort2_IW();
                    break;
                case 41:
                    testing.TestHPC_Class_InitByConfig_Exe_EPANET();
                    break;
                 case 42:
                    testing.TestWriteToDSS_EPANET();
                    break;
                 case 43:
                    testing.TestHPC_Class_InitByConfig_Exe_EPANET(useAWS:true);
                    break;
                 case 44:
                    testing.TestHPC_Class_InitByConfig_Exe(useAWS: true);
                    break;
                 case 45:
                    testing.CloneProject_3();
                    break;
                 case 46:
                    testing.TestHPC_Class_InitByConfig_Exe_EXTENDSIM();
                    break;

                 case 47:
                    testing.testSFBT();
                    break;
                 case 48:
                    testing.TestTS_Import();
                    break;
                #endregion
                #region somewhat_old

                 case 49:
                    testing.MillCreek();
                    break;
                 case 50:
                    testing.TestWriteToDSS_ExtendSim();
                    break;
                 case 51:
                    testing.TestHPC_Class_InitByConfig_Exe_EPANET();        //Norfolk_EvaluateEG();
                    break;
                case 53:
                    testing.RunEG_EPANET_CityOfTracey();
                    break;
                case 54:
                    testing.SWMM_Basic();
                    break;
                case 55:
                    testing.ExportEG_toCSV_Rough();
                    break;
                case 56:
                    testing.Write_XML();
                    //   testing.ReadXML_Schema();
                    break;
                case 57:
                    testing.WebProvider();
                    break;
                case 58:
                    testing.CSVProvider();
                    break;
                case 59:
                    testing.RunEG_EPANET_Minneapolis();
                    break;
                case 60:
                    testing.RealTime_TestEPANET();
                    break;
                case 61:
                    testing.WriteToDSS_Version2();
                    break;
                case 63:
                    testing.RunEG_ExtendSim_Devon();
                    break;
                case 64:
                    testing.NOAA_WebService1_ExternalData();
                    break;
                case 65:
                    testing.RealTime_Chicago_SFBT();
                    break;
                case 66:
                    testing.SynthTimeSeries_ToScenario_ToDSS();
                    break;
                case 67:
                    testing.VISSIM_DEMO();
                    break;
#endregion
                case 68:
                    testing.WebPushing();
                    break;
                case 62:
                    testing.Germantown();
                    break;
                case 69:
                    testing.ALCOSAN();
                    break;
                case 52:
                    bool bUseDb = true;
                    testing.MillCreek_RealTime_Test01_ExternalData(bUseDb);
                    break;
                case 70:
                    testing.NOLA();
                    break;
                case 71:
                    testing.Gtown_proxy();
                    break;
                case 72:
                    testing.ClevelandSQLServer();
                    break;
                case 73:
                    testing.Kinsall();
                    break;
                case 74:
                    testing.icm();
                    Console.WriteLine("worked this much");
                    break;
                case 75:
                    testing.MillCreek();
                    break;
                case 76:
                    testing.SWMM_OC();
                    break;
            }
            Console.WriteLine("Press any key to end");
            Console.ReadLine();
        }
    }
}
