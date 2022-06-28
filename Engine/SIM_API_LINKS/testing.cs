// 1/3/2013: bring in from own project. simlink xmodel needs to be able to call this.
            //unloaded own project which can be copied from here if distributing solo...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIM_API_LINKS.DAL;
using System.Data;
using System.IO;
using OptimizationWrapper;
using Nini.Config;
using System.Data.OleDb;

namespace SIM_API_LINKS
{
    
    // this class is only for testing and should be removed before distributing the software
    class testing
    {
        #region SWMM

        private static string sModelFile_BWSC = @"C:\Users\Mason\Documents\WORK\BWSC\MODELS\Base_251\SDO174_BMP_StdUnit_1inch.inp";
        private static string sConnMOD_SWMM = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\BWSC\Analysis\SimLink\SWMM_5.0.022_Database_v003.1_OC.mdb";
        private static string sConnMOD_BWSC = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\BWSC\Analysis\SimLink\RunScenarioMGR_V002_OC_V02_BWSC_v02.mdb";
        private static string sConnMOD_EPANET = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\Optimization\SimLink\DB\EPANET\RunScenarioMGR_V002_OC_V02_BWSC_v02.mdb";
        private static string sSQL_CONN_BWSC=("Data Source=HCHHL5JSN1;"
                                + "User ID=simlink_user;"
                                + "Password=admin;"
                                + "Initial Catalog = SimLink2;"
                                + "Integrated Security=True;");

        private static string sConn_CHC_SQL_SERVER = @"Data Source=chcapp10\sqlexpress;"
                                            + "Initial Catalog = simlink_test1;"
                                            + "Integrated Security = false;"
                                            + "User ID=test1;"
                                            + "Password=test1;";

        private static string sConn_CHC_SQL_SERVER_2 = @"Data Source=chcapp10\sqlexpress;"
                                    + "Initial Catalog = simlink_test2;"
                                    + "Integrated Security = True;"
                                    + "User ID=test1;"
                                    + "Password=test1;";

        private static string sConnMOD_HARTFORD_EPANET = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\Hartford\Analysis\SimLink\SimLink2.0_Hartford_LOCAL.mdb";

        private static string sConnMOD_Norfolk_MSAccess_Template = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\phlfpp01\Users\sp017586\Norfolk\Optimization6\Simlink1_FromTemplate_161028_Norfolk_Opt6.accdb";
        
        public static void ImportINPToDB()
        {
            swmm5022_link swmm = new swmm5022_link();
            swmm.InitializeModelLinkage(sConnMOD_BWSC, 1, false);
            swmm.ReadINP_ToDB(sModelFile_BWSC, 12345, 107);
            swmm.CloseModelLinkage();
        }

        public static void BWSC_RunEG(int nEvalID = 249)
        {
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 1;
            if (nContext == 0)
            {
                swmm.InitializeModelLinkage(sConnMOD_BWSC, 0, false);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);           //sql server
            }
            swmm._sDNA_Delimiter = ".";
            swmm.InitializeEG(nEvalID);

            swmm.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            swmm.CloseModelLinkage();
        }

        #endregion

        #region New Haven
        private static string sSQL_CONN_NH= ("Data Source=HCHHL5JSN1;"            //ALIAS\\SQLEXPRESS;"
                + "User ID=simlink_user;"
                + "Password=admin;"
                + "Initial Catalog = SimLink2_NewHaven;"
                + "Integrated Security=True;");

        private static string sModelFile_NH = @"C:\Users\mthroneb\Documents\NewHaven\Model\Automation\Base_270\NH_2014.v10.2_Baseline_Design_Storm_2Yr6Hr_in_May.inp";
   //     private static string sModelFile_LNC = @"C:\a\LID_stuff.inp";      // "C:\Users\Mason\Documents\WORK\LNC\Model\Automation\Base_259\LNC_LTCPU_20140403.inp";
     //   private static string sModelFile_LNC_ACCESS = @"\\hchfpp01\groups\WBG\Lancaster\Analysis\SimLink\SimLink2.0_LNC.mdb";

        public static void ImportINPToDB_NH()
        {
            swmm5022_link swmm = new swmm5022_link();
            swmm.InitializeModelLinkage(sSQL_CONN_NH, 1, false);
            swmm.ReadINP_ToDB(sModelFile_NH, 8478, 111);
            swmm.CloseModelLinkage();
        }
        public static void RunEG_NH(int nEvalID = 259)
        {
            swmm5022_link swmm = new swmm5022_link();
            swmm._bIsSimCondor = true;

            int nContext = 1;
            if (nContext == 0)
            {
                swmm.InitializeModelLinkage(sSQL_CONN_NH, 0, swmm._bIsSimCondor);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_NH, 1, swmm._bIsSimCondor);           //sql server
            }
            swmm._sDNA_Delimiter = ".";
            swmm.InitializeEG(nEvalID);

            swmm.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            swmm.CloseModelLinkage();
        }
        public static void DeleteScenarioDataNH()
        {
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 1;
            if (nContext == 0)
            {
                swmm.InitializeModelLinkage(sSQL_CONN_NH, 0, false);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_NH, 1, false);           //sql server
            }
            DateTime dtNow = DateTime.Now;
            int nDayOfYear = dtNow.DayOfYear;
            int nHour = dtNow.Hour;
            int nDayKey = 309;
            int nHourCode = 15;
            if ((nDayOfYear == nDayKey) && (nHour == nHourCode))
            {
                swmm.DeleteScenarioData(270, 1, 20, "", true);        //true: use special ops del list
            }
            swmm.CloseModelLinkage();
        }

        #endregion

        #region ICM
        
        public static void icm()
        {
          //  string sConfig = @"C:\Users\mthroneb\opt\simlink\testing\icm\icm_input_config.XML";
       //    IConfigSource configXML = new XmlConfigSource(sConfig);
            //    simlink icm = SIM_API_LINKS.CommonUtilities.GetSimLinkObject(configXML.Configs["simlink"].GetString("type", "simlink"));   //  simlink.InitializeByConfig(configXML);
     //   original dev     string sMDB_CONN = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\mthroneb\opt\simlink\testing\icm\simlink_millCreek_icm2.accdb";
            string sMDB_CONN = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Kitchener\Proj\Analysis\simlink\simlink1.0.5_Shoemaker.accdb";
            int nEvalID = 178;

            nEvalID = 179;      // shoemaker

            icm my_icm = new icm();
            my_icm.InitializeModelLinkage(sMDB_CONN, 0, false);
            my_icm._sDNA_Delimiter = ".";
            my_icm.InitializeEG(nEvalID);

            //my_icm.ProcessScenario(42, 177, 177, "x", -1, -1, 5);
            my_icm.ProcessEvaluationGroup(new string[0]);
           // my_icm.ProcessScenario(42, 177, 177, "x", 3, -1, 5, "2.2");

            //     my_icm.ProcessEvaluationGroup(new string[0]);

        }
        

        #endregion


        #region OC
        public static void SWMM_OC()
        {
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\OC\proj\Analysis\SimLink\simlink_millCreek_OC_v004.accdb";
            string sINP = @"c:\a\input.inp";     //\\hchfpp01\Groups\WBG\OC\proj\Model\test\Base_177\OC_2017.Baseline_v1.05_(TY)_FutureConditions.v2.9_Trimmedv1.inp";
            int nEvalID = 177;    // 
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 1;
            swmm.InitializeModelLinkage(sConn, 0, false);           //access
            swmm._sDNA_Delimiter = ".";
            swmm.InitializeEG(nEvalID);
            if (true)
                swmm.ReadINP_ToDB(sINP, 1, 38);     // need weir information

            swmm.ProcessEvaluationGroup(new string[0]);
            swmm.CloseModelLinkage();
        }
        #endregion

        #region LNC SWMM

        private static string sSQL_CONN_LNC = ("Data Source=HCHHL5JSN1;"            //ALIAS\\SQLEXPRESS;"
                        + "User ID=simlink_user;"
                        + "Password=admin;"
                        + "Initial Catalog = SimLink2;"
                        + "Integrated Security=True;");
        private static string sModelFile_LNC = @"C:\a\LID_stuff.inp";      // "C:\Users\Mason\Documents\WORK\LNC\Model\Automation\Base_259\LNC_LTCPU_20140403.inp";
        private static string sModelFile_LNC_ACCESS = @"\\hchfpp01\groups\WBG\Lancaster\Analysis\SimLink\SimLink2.0_LNC.mdb";
        //@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\LNC\Analysis\SimLink\SimLink2.0_LNC.mdb";

        public static void ImportINPToDB_LNC()
        {
            swmm5022_link swmm = new swmm5022_link();
            swmm.InitializeModelLinkage(sSQL_CONN_LNC, 1, false);
            swmm.ReadINP_ToDB(sModelFile_LNC, 8177, 110);
            swmm.CloseModelLinkage();
        }

        public static void RunEG_LNC(int nEvalID = 259)
        {
            swmm5022_link swmm = new swmm5022_link();
            swmm._bIsSimCondor = true;


            int nContext = 0;
            string sCONN = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\c\SimLink2.0_NewHaven_LOCAL.mdb";
            if (nContext == 0)
            {
                swmm.InitializeModelLinkage(sCONN, 0, swmm._bIsSimCondor);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_BWSC, 1, swmm._bIsSimCondor);           //sql server
            }
            swmm._sDNA_Delimiter = ".";
            swmm.InitializeEG(nEvalID);



            swmm.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            swmm.CloseModelLinkage();
        }

        public static void TestBorg(int nEvalID = 266)
        {
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 1;
            //if (nContext == 0)
            //{
            //    swmm.InitializeModelLinkage(sSQL_CONN_LNC, 0, false);           //access
            //}
            //else
            //{
            //    swmm.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);           //sql server
            //}
            swmm._sDNA_Delimiter = ".";
            //swmm.InitializeEG(nEvalID);
            swmm.InitializeOptimization(OptWrap.OptAlgo.DecViz_BORG, null);
            swmm.ExecuteOptimization();         //swmm.ProcessEvaluationGroup();      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            swmm.CloseModelLinkage();

        }

        /*public static void RunEG_LNC_Optimization(int nEvalID = 266)
        {
            string s = @"-test 3  -stop_loops 0100  -stop_time 86400 -stop_tolerance 0 -stop_tolerance_percent 100 -max_jump_fraction 0.5 
                       -jump_factor 2 -boundary_probability 0.3 -population_size 150 -memeplex_quantity 15 -memeplex_size 10 -submemeplex_size 5 -memeplex_evolutions 3";
            Dictionary<string, string> dictArgs = CommonUtilities.Arguments_ToDict(s.Split(' '));

            swmm5022_link swmm = new swmm5022_link();
            int nContext = 1;
            if (nContext == 0)
            {
                swmm.InitializeModelLinkage(sSQL_CONN_LNC, 0, false);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);           //sql server
            }
            swmm._sDNA_Delimiter = ".";
            swmm.InitializeEG(nEvalID);
            swmm.InitializeOptimization(OptimizationWrapper.OptWrap.OptAlgo.CH2M);
            swmm.ExecuteOptimization();         //swmm.ProcessEvaluationGroup();      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            swmm.CloseModelLinkage();
        }*/

        public static void TestSWMMResultRead()
        {
            string sOutFile = @"C:\Users\Mason\Documents\WORK\LNC\Model\Automation\263\8232\LNC_20140603_ALTS_Base_8232.OUT";
            string sHDF_toWrite = @"c:\a\test_results.hdf5";
            hdf5_wrap hdf = new hdf5_wrap();
            hdf.hdfCheckOrCreateH5(sHDF_toWrite);
            bool b = hdf.hdfOpen(sHDF_toWrite, false);
            swmm5022_link swmm = new swmm5022_link();
            swmm.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);         //
            swmm.TestReadOut(ref hdf, sOutFile, 10);
            hdf.hdfClose();
            swmm.CloseModelLinkage();
        }

        public static void CreateBat(int nEvalID, int nRunNumber)
        {

            swmm5022_link swmm = new swmm5022_link();

            int nContext = 1;
            if (nContext == 0)
            {
                swmm.InitializeModelLinkage(sConnMOD_BWSC, 0, false);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_LNC, 1, false);           //sql server
            }
            swmm.CreateBatchFile_ByEval(nEvalID, nRunNumber);
            swmm.CloseModelLinkage();
        }

        public static void CreateBatNH(int nEvalID, int nRunNumber)
        {

            swmm5022_link swmm = new swmm5022_link();

            int nContext = 1;
            if (nContext == 0)
            {
                //    swmm.InitializeModelLinkage(sConnMOD_BWSC, 0, false);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_NH, 1, false);           //sql server
            }
            swmm.CreateBatchFile_ByEval(nEvalID, nRunNumber);
            swmm.CloseModelLinkage();
        }

        public static void RunExcel()
        {
            excel_link excel = new excel_link();
            excel.excel_InitWorkbook(@"C:\a\myrain2.xlsx");
            Console.WriteLine("Val: {0}", excel.GetValue("Sheet1", 2, 1));
            excel.SetValue("Sheet1", 1, 1, "550");
            Console.WriteLine("Val: {0}", excel.GetValue("Sheet1", 1, 1));
            Console.WriteLine("Val: {0}", excel.GetValue("Sheet1", 2, 1));
            //         excel.SetValue("Sheet1", 1, 10, "plutocracy");
            excel.SaveAs(@"c:\a\myrain2.xlsx");
            Console.WriteLine("Val: {0}", excel.GetValue("Sheet1", 2, 1));
            excel.Close();
        }

        public static void DeleteScenarioData()
        {
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 1;
            if (nContext == 0)
            {
                swmm.InitializeModelLinkage(sSQL_CONN_LNC, 0, false);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);           //sql server
            }
            DateTime dtNow = DateTime.Now;
            int nDayOfYear = dtNow.DayOfYear;
            int nHour = dtNow.Hour;
            int nDayKey = 320;
            int nHourCode = 17;
            if ((nDayOfYear == nDayKey) && (nHour == nHourCode))
            {
                swmm.DeleteScenarioData(269, 20, 20, "", false);    //true: use special ops del list
            }
            swmm.CloseModelLinkage();

        }

        #endregion

        #region HARTFORD
        public static void RunEG_Hartford(int nEvalID = 277)
        {
            EPANET_link epanet = new EPANET_link();
            epanet._bIsSimCondor = false;                   //change manually as needed

            int nContext = 0;
            if (nContext == 0)
            {
                epanet.InitializeModelLinkage(sConnMOD_HARTFORD_EPANET, 0, epanet._bIsSimCondor);           //access
            }
            else
            {
                epanet.InitializeModelLinkage(sConnMOD_HARTFORD_EPANET, 1, epanet._bIsSimCondor);           //sql server
            }
            epanet._sDNA_Delimiter = ".";
            epanet.InitializeEG(nEvalID);
            epanet.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            epanet.CloseModelLinkage();
        }

        public static void RunOptimization_Hartford(int nEvalID = 277)
        {
            string s = @"-test 3  -stop_loops 0100  -stop_time 86400 -stop_tolerance 0 -stop_tolerance_percent 100 -max_jump_fraction 0.5 
                       -jump_factor 2 -boundary_probability 0.3 -population_size 150 -memeplex_quantity 15 -memeplex_size 10 -submemeplex_size 5 -memeplex_evolutions 3";
            Dictionary<string, string> dictArgs = CommonUtilities.Arguments_ToDict(s.Split(' '));

            EPANET_link epanet = new EPANET_link();
            epanet._bIsSimCondor = false;                   //change manually as needed

            int nContext = 0;
            if (nContext == 0)
            {
                epanet.InitializeModelLinkage(sConnMOD_HARTFORD_EPANET, 0, epanet._bIsSimCondor);           //access
            }
            else
            {
                epanet.InitializeModelLinkage(sConnMOD_HARTFORD_EPANET, 1, epanet._bIsSimCondor);           //sql server
            }
            epanet._sDNA_Delimiter = ".";
            epanet.InitializeEG(nEvalID);
            epanet.InitializeOptimization(OptimizationWrapper.OptWrap.OptAlgo.CH2M_SFLA, dictArgs);
            epanet.ExecuteOptimization();
            epanet.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            epanet.CloseModelLinkage();
        }

        public static void DeleteScenarioData_HARTFORD()
        {
            EPANET_link epanet = new EPANET_link();
            int nContext = 0;
            if (nContext == 0)
            {
                epanet.InitializeModelLinkage(sConnMOD_HARTFORD_EPANET, 0, false);           //access
            }
            else
            {
                epanet.InitializeModelLinkage(sConnMOD_HARTFORD_EPANET, 1, false);           //sql server
            }
            DateTime dtNow = DateTime.Now;
            int nDayOfYear = dtNow.DayOfYear;
            int nHour = dtNow.Hour;
            int nDayKey = 361;
            int nHourCode = 23;
            if ((nDayOfYear == nDayKey) && (nHour == nHourCode))
            {
                epanet.DeleteScenarioData(277, -1, 20, "", false);    //true: use special ops del list
            }
            epanet.CloseModelLinkage();

        }

        public static void TestDLL_Import()
        {
            string sFile = @"C:\Users\Mason\Documents\Hartford\Model\Automation\277\8837\MDC_DEMAND2030_09092014_SKEL_UPDATED_CNPS_8837.inp";
            string sFile1 = @"C:\Users\Mason\Documents\Hartford\Model\Automation\277\8837\MDC_DEMAND2030_09092014_SKEL_UPDATED_CNPS_8837.rpt";
            string sFile2 = @"C:\Users\Mason\Documents\Hartford\Model\Automation\277\8837\MDC_DEMAND2030_09092014_SKEL_UPDATED_CNPS_8837.out";
            string sLabel = "GIS6052848";
            long lReturn;
            long nOut = -1;
            //epanetDLL_import.DLL_Import epanet = new epanetDLL_import.DLL_Import();
            //lReturn = EPANET_link.ENopen(sFile, sFile1, sFile2);
            //lReturn = EPANET_link.ENgetnodeindex(sLabel, ref nOut);
            //lReturn = EPANET_link.ENclose();

        }

        #endregion

        #region DSS

        public static void WriteToDSS_Version2()
        {
            int nEvalID = 100;
            int nActiveScenarioID = 14834;
            string sFile = @"c:\a\dssTest.dss";
            swmm5022_link swmm = new swmm5022_link();
            string sCONN_Surface = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\MSD_Cinci\analysis\simlink\simlink_millCreek.accdb";
            swmm.InitializeModelLinkage(sCONN_Surface, 0, false);
            swmm.InitializeEG(nEvalID);
            int nCode = 2;
            switch (nCode)
            {
                case 1:
                    swmm.SetActiveScenarioID(nActiveScenarioID);
                    swmm.ExportScenarioToDSS(sFile);
                    break;
                case 2:
                    swmm.ExportTimeseriestoDSSByEval(nEvalID, sFile, false);
                    break;
            }

        }

        public static void TestWriteToDSS()
        {
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\MSD_Cinci\analysis\simlink\simlink_millCreek.accdb";
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 0;
            if (nContext == 0)
            {
                swmm.InitializeModelLinkage(sConn, 0, false);           //access
            }
            else
            {
                swmm.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);           //sql server
            }
         //   swmm.ExportTimeseriesToDSSByEval(263, @"C:\Users\Mason\Documents\WORK\LNC\Results\LNC_OptResults_v04.dss", false);
   //         swmm.ExportTimeseriesToDSSByEval(264, @"C:\Users\Mason\Documents\WORK\LNC\Results\LNC_OptResults_v04.dss", false);
            swmm.ExportTimeseriestoDSSByEval(99, @"C:\a\C:\a\LNC_OptResults_v06.dss", false);
            //  swmm.ExportTimeseriesToDSSByEval(265, @"C:\Users\Mason\Documents\WORK\LNC\Results\LNC_OptResults_v05.dss", false);
        }


        public static void TestWriteToDSS_EPANET()
        {
            EPANET_link epa = new EPANET_link();
            int nEvalGroupID = 55;

            int nContext = 0;
            if (nContext == 0)
            {
                epa.InitializeModelLinkage(sConnMOD_Norfolk_MSAccess_Template, 0, false);           //access
            }
            else
            {
                //epa.InitializeModelLinkage(sConnMOD_NORFOLK_EPANET_Subset, 1, false);           //SQLServer
            }

            //create a string of scenarios to export
            string[] sScenariosToEvaluate = new string[] {"14266","15385","15395","18987","21484","17186","20942","21780","15881","20322","18909","17866","16294","20329","20076","21906","17185","17181","15906","20471", //lowest capital
                                                                "14268","15122","15021","15108","15129","15136","15053","15234","14702","14955","15242","14951","15029","16214","14517","17251","14974","15249","14721","14532", //lowest deficiency
                                                                "20975","20775","21275","22075","20875","21375","20635","21475","21175","21975","21575","21075","21775","21875","21675","20503","20675","20738","20737","20732","21074","22046","21950","21459","21344","20914","22030","21839","21191","20961","21179","21038","20475","20130","21338","20334","21374","20375","20275","21471","21368","20575","20558","20094","20851","20672","21541","21542","21440","21674","21436","21422","21730","20968","21249","20604","21263","20867","22037","20764","21335","21164","20903","21157","21533","20744","20870","22051","20564","20774","20175","20075","19975","19875","21271","19675","19475","19775","19375","19275","19175","19066","19575","21606","21623","21442","19145","21949","21970","22047","21239","20956","20826","21264","21267","20661","19441","21332","21174","21656"}; //lowest objective
            epa.ExportTimeseriestoDSSByEval(nEvalGroupID, @"C:\a\Optimization1Results.dss", false, false, sScenariosToEvaluate);
        }


        public static void TestWriteToDSS_ExtendSim()
        {
            string sConnMOD_ExtendSim = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\phlfpp01\users\sp017586\Optimization\Simlink_ExtendSim\Simlink_ExtendSim_160910.mdb";
            extend_link extendlink = new extend_link();
            int nEvalGroupID = 71;

            int nContext = 0;
            if (nContext == 0)
            {
                extendlink.InitializeModelLinkage(sConnMOD_ExtendSim, 0, false);           //access
            }
            else
            {
                //epa.InitializeModelLinkage(sConnMOD_NORFOLK_EPANET_Subset, 1, false);           //SQLServer
            }

            extendlink.InitializeEG(nEvalGroupID);

            //create a string of scenarios to export
            extendlink.ExportTimeseriestoDSSByEval(nEvalGroupID, @"C:\a\ScenarioResults_Chevron.dss", false);
        }

        #endregion


        #region MODFLOW
        public static void TEST_MODFLOW1()
        {
            string sResultsFile = @"C:\Users\Mason\Documents\WORK\ABQ\Model\Test1\MFGSIM\MFGSIM_WB.2.csv";
            string sModel = @"C:\Users\Mason\Documents\WORK\ABQ\Model\Test1\MFGSIM\model.bat";
            SIM_API_LINKS.PROJ_WRAPPERS.modflow_ABQ mod = new SIM_API_LINKS.PROJ_WRAPPERS.modflow_ABQ();
            mod.InitializeModelLinkage(sModel, 3, CommonUtilities._nEvalID_SL_LITE, false,"",3);
            string[] sNames = new string[] { "ALAM1_5", "ALAM1_6", "CORON1_3", "InitialFileinj!" };
            string[] sVals =new string[] { "2.1", "1", "-1.234","CrazyFile.xls" };
            mod.ProcessScenario_TEMPLATE(sNames, sVals, sResultsFile, -1); ;
            mod.CloseModelLinkage();
        }


        #endregion

        #region XMODEL
        //v1 more like sim1... call processlinkedRecords directly
            
        public static void XMODEL_TEST01_Simple(int nRefScenarioID){
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 0;
            if (nContext==0){
                swmm.InitializeModelLinkage(sConnMOD_BWSC, 0, false);           //access
            }
            else{
                swmm.InitializeModelLinkage(sSQL_CONN_BWSC,1, false);           //sql server
            }
            swmm.InitializeEG(254);
            string sFile = @"C:\Users\Mason\Documents\WORK\BWSC\MODELS\ExternalData\BWSC_LT.15m";
          //  TimeSeries.TimeSeriesDetail tsd = new TimeSeries.TimeSeriesDetail(new DateTime(2007,1,1),IntervalType.Second,900);
            //swmm.DatFile_ReadTimeSeries(sFile, "4", tsd);
            swmm.CloseModelLinkage();
       //     swmm.ProcessEG_GetGS_Initialize(254, 1);
      //      swmm.LoadReferenceDatasets();            //TODO: include in EG load?
      //      swmm.XMODEL_ProcessLinkedRecords(nRefScenarioID);

        }
        //v2: incorporate into life cycle of scenario... (process EG etc..)



        #endregion


        #region BRUCE

        //todo: generalize this or integrate into overall code.
        public static void ReadFileToHDF5(string sFilInput, string sFileOutput, string sDelimiter = ",")
        {
            string[] sSeriesNames;
            StreamReader file = new StreamReader(sFilInput);
            int nRows = 0; int nCol = -1;
            using (file)
            {
                string sbuf = file.ReadLine();
                sSeriesNames = sbuf.Split('\t');

                nCol = sSeriesNames.Length;
                sSeriesNames = new string[nCol];
                double[][,] dVals = new double[nCol][,];
                while (!file.EndOfStream)
                {      //first get the number of rows
                    nRows++;
                    file.ReadLine();
                }

                for (int i = 0; i < nCol; i++)
                {
                    dVals[i] = new double[nRows, 1];
                }

                using (file)
                {
                    int nRow = 0;
                    string sBuf = file.ReadLine();        //read first line (labels)
                    if (!file.EndOfStream) sBuf = file.ReadLine();       //read second line
                    while (!file.EndOfStream)
                    {      //first get the number of rows
                        string[] sDat = sBuf.Split('\t');
                        for (int i = 0; i < nCol; i++)
                        {
                            dVals[i][nRow, 0] = Convert.ToDouble(sDat[i]);
                        }
                        sBuf = file.ReadLine();

                    }

                }

            }


        }

        #endregion

        #region NOLA
        public static void NOLA()
        {
            int nEvalID = -1;
            int nProjID = 42;
            swmm5022_link swmm = new swmm5022_link();
            string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\NOLA\analysis\simlink\simlink_millCreek_GMT_NOLA_v3.accdb";    //\\hchfpp01\Groups\WBG\NOLA\analysis\simlink\from_nola\simlink_millCreek_GMT_NOLA_SAC_CHC.accdb";       // the same 
      //      string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\Germantown\Analaysis\Simlink\v2\simlink_millCreek_GMT.accdb";  

            bool bUseMDB = true;
            if(bUseMDB)
                swmm.InitializeModelLinkage(sCONN_MDB, 0, false);           //access    (set for server or met personal)    
            else
                swmm.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);           //sql server                 
            swmm._sDNA_Delimiter = ".";
            int nAction =1;
            //  7;

            switch (nAction)
            {
                case 1:     // BASIC EG RUN
                    nEvalID = 213;
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessEvaluationGroup(new string[0]);
                    break;
            }
        }
        #endregion

        #region ALCOSAN
        public static void ALCOSAN()
        {
            int nEvalID = -1;
            int nProjID = 42;
            swmm5022_link swmm = new swmm5022_link();

            string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Alcosan\Analysis\Simlink\simlink_millCreek_GMT_Alcosan.accdb";       // the same 
            //  string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Germantown_Testing\simlink_millCreek_GMT.accdb";
            //      string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\Germantown\Analaysis\Simlink\v2\simlink_millCreek_GMT.accdb";  

            bool bUseMDB = true;
            if (bUseMDB)
                swmm.InitializeModelLinkage(sCONN_MDB, 0, false);           //access    (set for server or met personal)    
            else
                swmm.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);           //sql server                 
            swmm._sDNA_Delimiter = ".";

            int nAction =1;
            //  7;

            switch (nAction)
            {
                case 1:     // BASIC EG RUN
                    nEvalID = 209;
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessEvaluationGroup(new string[0]);
                    break;
                case 2:     // COHORT RUN
                    int nCohortID = 100;
                    nEvalID = 160;
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessCohort(nProjID, nCohortID);
                    break;
                case 3:     // DSS Export
                    nCohortID = 1;
                    string sDSS = @"c:\a\germantown_cohort1.dss";
                    nEvalID = 178;
                    swmm.InitializeEG(nEvalID);
                    swmm.ExportTimeseriestoDSSByCohort(nCohortID, sDSS, false);
                    break;
                case 4:               // READ THE SWMM data into the model.. need defnitely for imp reduction stuff.
                    nEvalID = 206;     // updated based upon revised baseline model 6/29/17 by MET
                    string sFilename = @"\\hchfpp01\Groups\WBG\Alcosan\Model\ForImportOnly\Base206\ALCOSAN-Full-SWM-EC-TY-v51011_ForImport.inp";
                    int nBaselineScenarioID = 21333;
                    swmm.ReadINP_ToDB(sFilename, nBaselineScenarioID, nProjID);
                    break;
                case 5:
                    nEvalID = 206;
                    int nCohort = 10;
                    bool bUseCohort = true;
                    if (!bUseCohort)
                        swmm.CreateBatchFile_ByEval(nEvalID, 4);
                    else
                        swmm.CreateBatchFile_ByEval(nEvalID, 1,nCohort, "swmm5.exe", "DEFAULT");
                    break;
                case 6:
                    nEvalID = 197;
                    string sDSS_Out = @"c:\a\gmt_eg196v01.dss";
                    Dictionary<string, string> dictArgs = new Dictionary<string, string>
                    {
	                    {"dss_start_time", "1990-1-2 00:00:00"},         //change back to starting at zero  (11PM based on hot start)
                        {"ts_interval","60"},  //IMPORTANT_ change to reporting time interval!
	                };
                    swmm.InitializeEG(nEvalID);
                    swmm.ExportTimeseriestoDSSByEval(nEvalID, sDSS_Out, false, false, null, null, dictArgs);
                    break;
                case 7:
                    nEvalID = 187;
                    int cohort_id = 2;
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessCohort(nProjID, cohort_id);
                    break;
                case 8:
                    int eval = 187;
                    int nRunsPerGroup = 2;
                    for (int i = 0; i < 8; i++)
                    {
                        int nNewEval = eval + i;
                        swmm.CreateBatchFile_ByEval(nNewEval, nRunsPerGroup);
                    }
                    break;
                case 9:
                    bool bRunBaseline = false;
                    int eval_push = 197;
                    string[] sAlts = { "Baseline", "C1", "C2", "C5b", "C6a", "C8", "C9" };
                    string[] sFiles = {@"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\Baseline\Germantown_Baseline_5yr.inp"
                                           , @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C1\5year\Germantown_DesignEvents_C1_5yr.inp", @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C2\5year\Germantown_DesignEvents_C2_5yr.inp"
                                          ,  @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C5\5yr\Germantown_DesignEvents_C5_5yr.inp"
                                          ,  @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C6\10yr\Germantown_DesignEvents_C6_5yr.inp"
                                          ,  @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C8\5yr\Germantown_DesignEvents_C8_5yr.inp"
                                          ,  @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C9\5yr\Germantown_DesignEvents_C9_5yr.inp"};
                    swmm.InitializeEG(eval_push);
                    if (bRunBaseline)
                    {
                        swmm.Push(@"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\Baseline\Germantown_Baseline_5yr.inp", "Baseline_AIM1");
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            string sLabel = sAlts[i];
                            string sFile = sFiles[i];
                            swmm.Push(sFile, sLabel, -1, -1, true);
                        }

                    }
                    break;
            }
            swmm.CloseModelLinkage();
        }
        #endregion


       # region Germantown

        public static void Gtown_proxy()
        {
             int nProjID = 41;
            swmm5022_link swmm = new swmm5022_link();

            string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Germantown\analysis\Simlink\PROXY\simlink_millCreek_GMT_proxy.accdb";       // the same 
          //  string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Germantown_Testing\simlink_millCreek_GMT.accdb";
      //      string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\Germantown\Analaysis\Simlink\v2\simlink_millCreek_GMT.accdb";  

            bool bUseMDB = true;
            if(bUseMDB)
                swmm.InitializeModelLinkage(sCONN_MDB, 0, false);           //access    (set for server or met personal)    
            else
                swmm.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);           //sql server                 
            swmm._sDNA_Delimiter = ".";

            int nAction = 1;
            //nEvalID = 206;
            swmm.InitializeEG(220);
            swmm.ProcessEvaluationGroup(new string[0]);
        }

        public static void ClevelandSQLServer()
        {
            bool bUseMDB = false;
            swmm5022_link swmm = new swmm5022_link();
            if (bUseMDB)
            {
                int n = 1;
            }
            //swmm.InitializeModelLinkage(sCONN_MDB, 0, false);           //access    (set for server or met personal)    
            else
                swmm.InitializeModelLinkage(sConn_CHC_SQL_SERVER_2, 1, false);

            int nEvalID = 1;
            swmm.InitializeEG(nEvalID);
            swmm.ProcessEvaluationGroup(new string[0]);

        }

        public static void Germantown()
        {
            int nEvalID = -1;
            int nProjID = 41;
            swmm5022_link swmm = new swmm5022_link();

            //string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Germantown\analysis\Simlink\simlink_millCreek_GMT.accdb";       // the same 
          //  string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Germantown_Testing\simlink_millCreek_GMT.accdb";
            string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Germantown\analysis\Simlink\cost_test\simlink_millCreek_GMT_cost_test.accdb";  
            
            bool bUseMDB = true;
            if(bUseMDB)
                swmm.InitializeModelLinkage(sCONN_MDB, 0, false);           //access    (set for server or met personal)    
            else
                swmm.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);           //sql server                 
            swmm._sDNA_Delimiter = ".";

            int nAction = 1;
            //  7;

            switch (nAction)
            {
                case 1:     // BASIC EG RUN
                    nEvalID = 216;

                    if (true)
                    {
                        swmm._bUseCostingModule = true;
                   //     string sCostMDBConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\Optimization\cost\cost_master\db\cost_master.accdb";
                        string sCostMDBConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Optimization\SimLink\code\cost_master\db\cost_master.accdb";                     
                        swmm._cost_wrap.Initialize(swmm._nActiveScenarioID, sCostMDBConn);
                    }

                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessEvaluationGroup(new string[0]);
                    break;
                case 2:     // COHORT RUN
                    int nCohortID = 100;
                    nEvalID = 160;
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessCohort(nProjID, nCohortID);
                    break;
                case 3:     // DSS Export
                    nCohortID = 1;
                    string sDSS = @"c:\a\germantown_cohort1.dss";
                    nEvalID = 178;
                    swmm.InitializeEG(nEvalID);
                    swmm.ExportTimeseriestoDSSByCohort(nCohortID, sDSS, false);
                    break;
                case 4:               // READ THE SWMM data into the model.. need defnitely for imp reduction stuff.
                    nEvalID = 169;     // updated based upon revised baseline model 6/29/17 by MET
                    string sFilename = @"\\hchfpp01\Groups\WBG\Germantown\models\Base_169_forImport\Gtown_UpdBase_v13.inp";   
                    int nBaselineScenarioID = 19798;
                    swmm.ReadINP_ToDB(sFilename, nBaselineScenarioID, nProjID);
                    break;
                case 5:
                    nEvalID = 203;
                    int nCohort = 2;
                    bool bUseCohort = false;
                    if(!bUseCohort)
                        swmm.CreateBatchFile_ByEval(nEvalID,8);
                    else
                        swmm.CreateBatchFile_ByEval(nEvalID, 4, nCohort,"swmm5.exe","DEFAULT");
                    break;
                case 6:
                    nEvalID = 197;
                    string sDSS_Out = @"c:\a\gmt_eg196v01.dss";
                    Dictionary<string, string> dictArgs = new Dictionary<string, string>
                    {
	                    {"dss_start_time", "1990-1-2 00:00:00"},         //change back to starting at zero  (11PM based on hot start)
                        {"ts_interval","60"},  //IMPORTANT_ change to reporting time interval!
	                };
                    swmm.InitializeEG(nEvalID);
                    swmm.ExportTimeseriestoDSSByEval(nEvalID, sDSS_Out, false, false, null, null, dictArgs);
                    break;
                case 7:
                    nEvalID = 187;
                    int cohort_id = 2;
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessCohort(nProjID, cohort_id);
                    break;
                case 8:
                    int eval = 187;
                    int nRunsPerGroup=2;
                    for (int i = 0; i < 8; i++)
                    {
                        int nNewEval = eval + i;
                        swmm.CreateBatchFile_ByEval(nNewEval, nRunsPerGroup);
                    }
                    break;
                case 9:
                    bool bRunBaseline = false;
                    int eval_push = 197;
                    string[] sAlts = { "Baseline", "C1", "C2", "C5b", "C6a", "C8", "C9" };
                    string[] sFiles = {@"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\Baseline\Germantown_Baseline_5yr.inp"
                                           , @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C1\5year\Germantown_DesignEvents_C1_5yr.inp", @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C2\5year\Germantown_DesignEvents_C2_5yr.inp"
                                          ,  @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C5\5yr\Germantown_DesignEvents_C5_5yr.inp"
                                          ,  @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C6\10yr\Germantown_DesignEvents_C6_5yr.inp"
                                          ,  @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C8\5yr\Germantown_DesignEvents_C8_5yr.inp"
                                          ,  @"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\C9\5yr\Germantown_DesignEvents_C9_5yr.inp"};
                    swmm.InitializeEG(eval_push);
                    if (bRunBaseline)
                    {
                        swmm.Push(@"\\bosfpp01\proj\684420_PhiladelphiaDepartmentofWater\proj\Models\ReceivedFromClient\Germantown_Area_SFR_Runs\InputsOutputs\Germantown_Area_SFR_Runs\Baseline\Germantown_Baseline_5yr.inp", "Baseline_AIM1");
                    }
                    else{
                        for (int i = 0; i<7;i++){
                            string sLabel = sAlts[i];
                            string sFile = sFiles[i];
                            swmm.Push(sFile,sLabel,-1,-1,true);
                        }

                    }
                    break;
            }
            swmm.CloseModelLinkage();
        }
        #endregion
        #region MillCreek
        private static string sConnMOD_MillCreek = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\MillCreek\Analysis\SimLink\SimLink2.0_NewHaven_LOCAL.mdb";

        
        /// <summary>
        /// testing function for mill creek
        /// updated 9/16/16
        /// </summary>
        public static void MillCreek()
        {
            int nEvalID = -1;

            swmm5022_link swmm = new swmm5022_link();
            string sCONN_Surface = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\MSD_Cinci\analysis\simlink\simlink_millCreek.accdb";

            string sFile = @"\\hchfpp01\Groups\WBG\MillCreek\Model\Base72\Mill_Creek_TY_1of4_rmPolygon_rmVertices.inp";

            string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\MillCreek\Analysis\SimLink\ModifiedForMSDDeployment\simlink_millCreekDeploy_v0.1.accdb";   //\\hchfpp01\Groups\WBG\MillCreek\Analysis\SimLink\simlink_millCreek.accdb";
            bool bUseMDB = true;
            if(bUseMDB)
                swmm.InitializeModelLinkage(sCONN_MDB, 0, false);           //access    (set for server or met personal)    
            else
                swmm.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);           //sql server      
            
            swmm._sDNA_Delimiter = ".";


            int nAction = 1;        // 9;    //  7;

            switch (nAction){
                case 1:
                    nEvalID =172;   //112;// tes read direct 94;   //95;       //94
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessEvaluationGroup(new string[0]);   
                //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
                    break;

                case  2:    // read in the ts
                    swmm.InitializeEG(nEvalID);
                    swmm.SetTSOutToArrayLike();                 // define preference for outputing to 2D array
                    swmm._nTS_WriteMessageToConsoleFreq = 100;        //test
                    swmm.ReadTimeSeriesOutput(nEvalID, 14642, @"C:\d\Mill Creek_TY_4of4.out");
                    break;
                case 3:

                    swmm.InitializeEG(nEvalID);
                    swmm.SetTSOutToArrayLike();                 // define preference for outputing to 2D array
                    swmm._nTS_WriteMessageToConsoleFreq = 5000;        //test
            //        string sCSV = @"\\hchfpp01\Groups\WBG\MillCreek\Analysis\storm_definition_test.csv";
            //        string sHDF = @"\\hchfpp01\Groups\WBG\MillCreek\Analysis\millcreek_storm_data_test.h5";
                    string sCSV = @"\\hchfpp01\Groups\WBG\MillCreek\Analysis\storm_definition_5events.csv";
                    string sHDF = @"\\hchfpp01\Groups\WBG\MillCreek\Analysis\storm_definition_5events_v2.h5";
                    swmm.ReadStormsToHDF5(sCSV, sHDF);
                    break;
                case 4:
                    int nCohorID = 300;
                    nEvalID = 102; 
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessCohort(37,nCohorID);
                    break;
                case 5:
               //     nEvalID = 90;
                //    swmm.InitializeEG(nEvalID);
                    int nCohortID = 3;              //typical year!
                    swmm.ProcessCohort(37, nCohortID);
                    break;
                case 6:     // test quantile stuff
                    swmm.InitializeEG(nEvalID);
                    swmm.SetTS_Filename2(@"C:\Users\Mason\Documents\WORK\MSD_Cinci\model\TypicalYear\SWMM_TS_14654.h5");
                    swmm.EGDS_GetTS_Details(14654);

                    string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, "63305", "SKIP", "SKIP");
                    double[,] dVals = swmm.GetTS_FromMemory(sGroupID);
                    QuartileFunction(dVals);
                    Console.ReadLine();
                    break;
                case 7:
                    nEvalID = 90;
                    swmm.InitializeEG(nEvalID);
                    nCohortID = 300;              //typical year!
                    swmm.ProcessCohort(37, nCohortID);
                    break;
                case 8:     // create the 
                    swmm.InitializeEG(102);
                    swmm.CreateBatchFile_ByEval(102, 5, 300, "swmm5.exe", "DEFAULT");
                    break;
                case 9:             // export concatenated events to a file for statistical processing
                    swmm.InitializeEG(102);
                    swmm.LoadScenarioDatasets(15489, 100, false);       // this should load the needed events
                    List<string> lstOut = swmm.ConcatenateEvents(new int[] { 15489 });
                    string[] sOut = lstOut.ToArray();
                    string sFileOUT = @"C:\a\millcreek_events_" + CommonUtilities.RMV_FixFilename(System.DateTime.Now.ToString()) + ".csv";
                    File.WriteAllLines(sFileOUT, sOut);
                    break;
                case 10:
                    int nScenarioID = 15489;
                    swmm.InitializeEG(142);
                    swmm.SetActiveScenarioID(nScenarioID);
                    swmm.LoadScenarioDatasets(nScenarioID, 100, false);
                    swmm.CreateMissingEvents(nScenarioID, 2317,3);
                    break;
                case 11:
                    swmm.InitializeEG(142);
                    swmm.DeleteScenarioData(142, 17, 17,"");
                    break;
                case 12:
                    string sDSS = @"c:\a\millcreekDSS.dss";
                    swmm.InitializeEG(142);

                    Console.WriteLine(string.Format("Begin DSS Export 1 at time {0}", System.DateTime.Now));
                    swmm.ExportTimeseriestoDSSByEval(nEvalID, sDSS, false);
                    DSS_CustomPart custom = new DSS_CustomPart("normalized",true);
                    Dictionary<string, DSS_CustomPart> dict = new Dictionary<string, DSS_CustomPart>();
                    dict.Add("a", custom);
                    Console.WriteLine(string.Format("Begin DSS Export 2 at time {0}", System.DateTime.Now));
                    swmm.ExportTimeseriestoDSSByEval(nEvalID, sDSS,false,true,null,dict);  //export a normalized version of the values
                    break;
            }
            swmm.CloseModelLinkage();
        }

        private static void QuartileFunction(double[,] dVals)
        {
            double[] dThresholds = new double[] { 0.01, 0.1, 0.25, 0.5, 0.75, 0.9, 0.95, 0.99, 0.999 };
            double[] dSeek = new double[] { 0.2, 0.258, 5, 10};
            double[] dArray = TimeSeries.ts2DTo1DArray(dVals);
            double dVal;
            for (int i = 0; i < dThresholds.Length; i++)
            {
                double dThreshold = dThresholds[i];
                dVal = mathdotnet.Quantile(dArray, dThreshold);
                Console.WriteLine(string.Format("threshold {0}, val {1}",dThreshold,dVal));
            }
            Console.WriteLine("------");
            for (int i = 0; i < dSeek.Length; i++)
            {
                double dTarget = dSeek[i];
                dVal = mathdotnet.QuantileRank(dArray, dTarget);
                Console.WriteLine(string.Format("seek {0}, loc {1}", dTarget, dVal));
            }
        }


        #endregion


        #region DAL
        public static void TestOleBDInsert()
        {
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\Optimization\bin\testdb1.accdb;Persist Security Info=False;";
            DBContext db = new SIM_API_LINKS.DAL.DBContext();
            db.Init(sConn, DB_Type.OLEDB);

            string sql = "select * from tblMyTable where false";
            DataSet ds = db.getDataSetfromSQL(sql);
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[0][1] = "clown";
            ds.Tables[0].Rows[0][2] = "xxx";
            ds.Tables[0].Rows[0][3] = 33;
            db.InsertOrUpdateDBByDataset(true, ds, sql);
        }

        public static void TestCompactAndRepair()
        {
      //      string sFileTest = @"C:\c\TestCR.mdb";
       //     string sFileOut = @"C:\c\TestCR_OUT.mdb";
        //    Oledb_Maintenance db = new Oledb_Maintenance();
         //   long l = db.GetFileSize(sFileTest);
          //  db.CompactDb(sFileTest, sFileOut);

            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\c\TestCR.mdb";
            DAL.DBContext db = new DBContext();
            db.Init(sConn, DB_Type.OLEDB, 0.01);
            bool bTestOut;
            db.RectifySizeLimitation(out bTestOut);
        }

        #endregion

        #region SIMCLIM

        #region SOLO_WORKFLOWS


        #endregion
        #endregion

        #region ISIS2D

        public static void ISIS2D_TEST1()
        {
            isis_2DLink isis2D = new isis_2DLink();
            int nContext = 0;
            if (nContext == 0)
            {
                isis2D.InitializeModelLinkage(sConnMOD_BWSC, 0, false);           //access
            }
            else
            {
                isis2D.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);           //sql server
            }
            isis2D._sDNA_Delimiter = ".";
            isis2D.InitializeEG(256);

            isis2D.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_isis2D, -1, true);
            isis2D.CloseModelLinkage();
        }
        #endregion

        #region EPANET
        public static void RunEPANET_EG(int nEvalID = 258)
        {
            EPANET_link epanet = new EPANET_link();
            int nContext = 0;
            if (nContext == 0)
            {
                epanet.InitializeModelLinkage(sConnMOD_HARTFORD_EPANET, 0, false);           //access
            }
            else
            {
                //not yet supported (just doens't have epanet tables) epanet.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);           //sql server
            }
            // epanet._sDNA_Delimiter = ".";
            epanet.InitializeEG(nEvalID);

            epanet.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            epanet.CloseModelLinkage();
        }

        public static void TestReadOut()
        {
            string sFile = @"C:\Users\Mason\Documents\Optimization\SimLink\JobCenter\Automation\258\8163\Net1_8163.OUT";
            EPANET_link epanet = new EPANET_link();
            epanet.EPANET_OpenOutFile(sFile);
        }
        #endregion

        #region CirrusHTC
        public static void CirrusHTC_Test()
        {
            CIRRUS_HTC_NS.CIRRUS_HTC htc = new CIRRUS_HTC_NS.CIRRUS_HTC();
            //   htc.__sActiveHTCDir = @"C:\Users\Mason\Documents\Optimization\SimLink\SimLink2_ProjSpecificBuilds\XML_CirrusHTC\Test";
            htc.InitCirrusHTC_byXML();
            htc.InitJob(@"C:\Users\Mason\Documents\Optimization\SimLink\SimLink2_ProjSpecificBuilds\XML_CirrusHTC\Test\JobSpec_HTC_SPEC_V01.xml");
            htc.ProcessCondorJobs();
        }

        #endregion

        #region dumb
        public static double DoubleMyVal(double dVal)
        {
            return 2 * dVal;
        }
        #endregion

        #region MathParser

        public static void TestConditional()
        {
            string sFunction = "#IF(4>3, 800*12, 2^10)";
            swmm5022_link swmm = new swmm5022_link();
            string sTheVal = swmm.Parse_EvaluateExpression(1, 2, sFunction);
        }

        #endregion

        #region EPANet_TEST_PROBLEM


        public static void RunEG_EPANETDemo1(int nEvalID = 54)
        {

            string sConnMOD_EPANETDemo1 = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Optimization\SimlinkUI_MSAccessDB\Simlink_Template_160823.mdb";
            string sConnMOD_EPANETDemo1_SQLServer = @"Server = PHLDNRZ9G2\SQLEXPRESS2014; Database = Simlink_Template_FromScripts_EPANETDemo1; Trusted_Connection = True;";

            EPANET_link epa = new EPANET_link();


            int nContext = 0;

            if (nContext == 1)
            {
                nEvalID = 54;
                epa.InitializeModelLinkage(sConnMOD_EPANETDemo1, 0, epa._bIsSimCondor);           //access
            }
            else
            {
                nEvalID = 286;

                epa.InitializeModelLinkage(sConnMOD_EPANETDemo1_SQLServer, 1, epa._bIsSimCondor); //connected to CHC server
                //epa.InitializeModelLinkage(sConnMOD_EPANETDemo1_SQLServer, 1, epa._bIsSimCondor);           //sql server
            }

            epa._sDNA_Delimiter = ".";
            epa.InitializeEG(nEvalID);

            epa.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            epa.CloseModelLinkage();
        }


        #endregion

        #region Infoworks
        public static void TestIWSimple()
        {
            InfoWorksLib.InfoWorks iw = new InfoWorksLib.InfoWorks();
            iw = new InfoWorksLib.InfoWorks();
            iw.InitForTest(0, "", "");
            string b = iw.BrowseForObjects("Rainfall Event", "Hello From My Dialog", ">", 0, 0);
            string a = "";      // a;
        }
        public static void TestIW()
        {
            int nSimID = 7;             //reset if you use a new database...
            //    int nSimID = 10;

            string sMasterDB = @"\\hchfpp01\Groups\WBG\Dekalb\Model\Base280\eg280_v4.iwm";
            string sSimLinkDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Optimization\Simlink_NewHaven\SimLink2.0_NewHaven_LOCAL_V2.mdb";
            int nEvalID = 280;
            iw iw = new SIM_API_LINKS.iw();
            iw._sMasterDatabase = sMasterDB;            //todo: should come from 
            iw._nSimID = nSimID;
            iw._bSKIP_IW_Init = true;                   // rare - cannot run sim
            iw.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);
            iw.InitializeIW_ModelLinkage("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sMasterDB, 0, 0.9);
            iw.InitializeEG(nEvalID);
            string[] a = new string[] { };
            iw.ProcessEvaluationGroup(a);
            //    iw.ProcessScenario(121, nEvalID, nEvalID, @"c:\a\nothing.txt",8849);
            iw.CloseModelLinkage();
        }
        public static void IW_ImportModel()
        {
            int nSimID = 10;
            string sMasterDB = @"\\hchfpp01\Groups\WBG\Dekalb\Model\Base280\eg280_v4.iwm";
            string sSimLinkDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Optimization\Simlink_NewHaven\SimLink2.0_NewHaven_LOCAL_V2.mdb";
            int nEvalID = 280;
            int nBaseScenarioID = 8848;       //important!!
            iw iw = new SIM_API_LINKS.iw();
            iw._sMasterDatabase = sMasterDB;            //todo: should come from 
            iw._nSimID = nSimID;
            iw.InitializeModelLinkage(sSimLinkDB, 0, false);
            iw.InitializeIW_ModelLinkage("", 0);
            //  iw.ReadIW_NetworkToDB(sMasterDB,nSimID, nBaseScenarioID,120);
            iw.testc();
            iw.CloseModelLinkage();
        }

        public static void iw2()
        {

        }
        #endregion


        #region PITT
        public static void PittTesting()
        {
            swmm5022_link swmm = new swmm5022_link();
            //     string sINP = @"C:\Users\Mason\Documents\WORK\Alcosan\Models\Base\for_import\TC_Alt_TT-BA16A_2YrSummerStorm_Jun2011_v2._REMOVE_ExternalFiles.inp";
            //     string sMDB_CONN = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Mason\Documents\WORK\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";
            //     string sMDB_CONN = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\\hchfpp01\Groups\WBG\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";
            string sMDB_CONN_SQL = @"Server = PHLC799J72\MSSQLSERVER2014; Database = simlink_test1; Trusted_Connection = True;";
            string[] a = new string[] { };
            int nEvalID = 52;           // 281;
            int nEvalID_SQL = 3;

            //swmm.InitializeModelLinkage(sMDB_CONN, 0, false);
            swmm.InitializeModelLinkage(sMDB_CONN_SQL, 1, false);
            // pitt stuff   swmm.ReadINP_ToDB(sINP, 8848, 122);
            //swmm.InitializeEG(nEvalID);
            swmm.InitializeEG(nEvalID_SQL);
            swmm.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            //       swmm.CreateBatchFile_ByEval(nEvalID, 30);

            swmm.CloseModelLinkage();
        }
        #endregion

        #region MWRD
        public static void testMWRD()
        {
            string sMDB_CONN = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\\hchfpp01\PROJ\MetroWaterReclamDist\494887CombinedSewer\proj\Analysis\AlternativeDB\simlink\simlink1_v80_forMWRD.accdb";
            string sMasterDB = @"\\HCHFPP01\proj\MetroWaterReclamDist\494887CombinedSewer\proj\Models\CTSM_CS-TARP_IntegratedModel\CTSM_CS-TARP_Integrated_DB03_v7.iwm";
            int nEvalID = 30;           // 281;

            //first test is using swmm even though it is iw, just cause more likely to have the built in functions
            iw myModel = new iw();
            myModel._bSKIP_IW_Init = false;
            myModel._sMasterDatabase = sMasterDB;
            myModel._nSimID = 7;
            myModel.InitializeModelLinkage(sMDB_CONN, 0, false);
            // pitt stuff   swmm.ReadINP_ToDB(sINP, 8848, 122);
            myModel.InitializeIW_ModelLinkage("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sMasterDB, 0, 0.9);
            myModel.InitializeEG(nEvalID);
            myModel.ProcessEvaluationGroup(new string[0]);   
        }


        /// <summary>
        /// more than typical testing function (more like a proj spec build)
        /// MWRDGC System flow balance tool wrapper...
        /// </summary>
        public static void testSFBT()
        {
            int nEvalID = 83;           // 281;
            int nProjID = 36;
            swmm5022_link swmm = new swmm5022_link();
            swmm.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);
            int nCode =4;
            int[] nEvals = new int[]{71,73,74,75};      //needs updated, because somehow these records were deleted...
            switch (nCode)
            {
                case 1:    // do the original model runs
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessCohort(nProjID,1);
                    break;
                case 2:             // bring in the XLS data for comparison
                    string[] sFiles = new string[] { @"\\hchfpp01\PROJ\MetroWaterReclamDist\494887CombinedSewer\proj\Analysis\SystemFlowBalance\CSV_Export\2007_ImportSpec.csv", @"\\hchfpp01\PROJ\MetroWaterReclamDist\494887CombinedSewer\proj\Analysis\SystemFlowBalance\CSV_Export\2010_ImportSpec.csv", @"\\hchfpp01\PROJ\MetroWaterReclamDist\494887CombinedSewer\proj\Analysis\SystemFlowBalance\CSV_Export\POR_ImportSpec.csv", @"\\hchfpp01\PROJ\MetroWaterReclamDist\494887CombinedSewer\proj\Analysis\SystemFlowBalance\CSV_Export\10yr_ImportSpec.csv" };

                    int[] nResultTS = new int[] { 2243, 2244, 2246, 2247 };     //result array (if already known
               //     int[] nResultTS = new int[0];
                    for (int i=0;i<=3;i++){
                        string sFile = sFiles[i];
                        nEvalID=nEvals[i];
                        swmm.InitializeEG(nEvalID);
                        if (nEvalID == 71)          //note : set nResultTS to null on first pass to insert records.
                            nResultTS = swmm.ImportAuxiliaryTimeSeriesByFileSpec(sFile, true, nResultTS, -1);   // do ref eg first, and get the newly imported TS
                        else
                            swmm.ImportAuxiliaryTimeSeriesByFileSpec(sFile, true, nResultTS, -1);              // use the proper 
                    }
                    break;
                case 3:     // to dss
                    for (int i = 0; i <= 3; i++)
                    {
                        nEvalID = nEvals[i];
                        swmm.InitializeEG(nEvalID);             // not sure if full init needed, but do need to set 
                        swmm.ExportTimeseriestoDSSByEval(nEvalID, @"C:\a\SFBT_v01.dss", false, false, null, null);
                    }
                    break;     
                case 4:
                    swmm.InitializeEG(nEvalID);
                    swmm.ProcessEvaluationGroup(new string[0]);
                    break;
            }
            swmm.CloseModelLinkage();
        }

        #endregion

        # region Clone

        /// <summary>
        /// test ability to clone a proj into a diff database
        /// should work with various different datasets
        /// </summary>
        /// <param name="bCleanCloneBeforeRun"></param>
        public static void CloneProject()
        {

            string sConn_SOURCE = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";

            //string sConn_CLONE = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL_CLONE.mdb";
            int nProjID = 122;

            /*        string sConn_SOURCE = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";
                    string sConn_CLONE = @"Data Source=chcapp10\sqlexpress;"
                                      //  + "User ID=simlink_user;"
                                      //  + "Password=admin;"
                                        + "Initial Catalog = simlink_test1;"
                                        + "Integrated Security=True;";  */



            swmm5022_link swmm_source = new swmm5022_link();
            swmm5022_link swmm_clone = new swmm5022_link();
            swmm_source.InitializeModelLinkage(sConn_SOURCE, 0, false);
            swmm_clone.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);

            if (true)
            {
                Console.WriteLine("testing func set up to auto delete prev attempt. make sure you want this.");
                swmm_clone.DeleteProj(true, 28);       //user can set this to auto delete the (bloody) failed prev attempt- usually do to failed db schema that gets partways through

            }
            swmm_source._nActiveProjID = nProjID;                           // not set in init... (prob on EG init)
            swmm_clone.Clone(nProjID, swmm_source);
        }

        public static void CloneProject_2()
        {

            string sConn_SOURCE = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Norfolk\Optimization4\simlink1_empty_local_Norfolk_161025_Optimization4_Skeletonized.accdb";
            string sConn_CLONE = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Norfolk\Optimization4\simlink1_template_161028.accdb";
            //string sConnMOD_Local_SQLServer = @"Server = PHLC799J72\SQLExpress; Database = Simlink_Template_FromScripts; Trusted_Connection = True;";

            int nProjID = 31;

            /*string sConn_SOURCE = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";*/
            /*string sConn_CLONE = @"Data Source=PHLC799J72\sqlexpress;"
                                      //  + "User ID=simlink_user;"
                                      //  + "Password=admin;"
                                        + "Initial Catalog = Simlink_Template_FromScripts;"
                                        + "Integrated Security=True;";  */



            EPANET_link epa_source = new EPANET_link();
            EPANET_link epa_clone = new EPANET_link();
            epa_source.InitializeModelLinkage(sConn_SOURCE, 0, false);
            epa_clone.InitializeModelLinkage(/*sConnMOD_Local_SQLServer*/sConn_CLONE, 0, false);

            if (false)
            {
                Console.WriteLine("testing func set up to auto delete prev attempt. make sure you want this.");
                epa_clone.DeleteProj(true, 31);       //user can set this to auto delete the (bloody) failed prev attempt- usually do to failed db schema that gets partways through

            }
            epa_source._nActiveProjID = nProjID;                           // not set in init... (prob on EG init)
            epa_clone.Clone(nProjID, epa_source);
        }


        public static void CloneProject_3()
        {

            string sConn_SOURCE = sConn_CHC_SQL_SERVER; // @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Optimization\Simlink_EPANETDemo1\SimLink2.0_EPANETDemo1.mdb";
            //string sConn_CLONE = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Optimization\Simlink_NewHaven\SimLink2.0_NewHaven_LOCAL.mdb";
            string sConnMOD_Local_MsAccess = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Optimization\Simlink_ExtendSim_Completed\Simlink_ExtendSim_160910.mdb";

            int nProjID = 36;

            /*string sConn_SOURCE = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";*/
            /*string sConn_CLONE = @"Data Source=PHLC799J72\sqlexpress;"
                                      //  + "User ID=simlink_user;"
                                      //  + "Password=admin;"
                                        + "Initial Catalog = Simlink_Template_FromScripts;"
                                        + "Integrated Security=True;";  */



            extend_link ExtendSim_source = new extend_link();
            extend_link ExtendSim_clone = new extend_link();
            ExtendSim_source.InitializeModelLinkage(sConnMOD_Local_MsAccess, 0, false);
            ExtendSim_clone.InitializeModelLinkage(sConn_CHC_SQL_SERVER/*sConnMOD_Local_SQLServer*/, 1, false);

            if (true)
            {
                Console.WriteLine("testing func set up to auto delete prev attempt. make sure you want this.");
                ExtendSim_clone.DeleteProj(true, 36);       //user can set this to auto delete the (bloody) failed prev attempt- usually do to failed db schema that gets partways through
            }

            ExtendSim_source._nActiveProjID = nProjID;                           // not set in init... (prob on EG init)
            ExtendSim_clone.Clone(nProjID, ExtendSim_source);
        }


        #endregion

        #region ExtendSim
        /// <summary>
        /// test ExtendSim linkages 
        /// </summary>
        public static void ExtendSimTesting(int nEvalID = 58)
        {
            string sConnMOD = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\TRWD\Analysis\simlink\reference\extendsim\Devon_Simlink.mdb";   //C:\Users\sp017586\Documents\Optimization\Simlink_ExtendSim\Simlink_ExtendSim_160910.mdb";
            string sConnMOD_SQLServer = @"Server = PHLC799J72\SQLExpress; Database = Simlink_Template_FromScripts_EPANETDemo1; Trusted_Connection = True;";

            extend_link ExtendSim_Link = new extend_link();

            int nContext = 0;
            if (nContext == 0)
            {
                ExtendSim_Link.InitializeModelLinkage(sConnMOD, 0, ExtendSim_Link._bIsSimCondor);           //access
            }
            else
            {
                ExtendSim_Link.InitializeModelLinkage(sConnMOD_SQLServer, 1, ExtendSim_Link._bIsSimCondor);           //sql server
            }

            ExtendSim_Link._sDNA_Delimiter = ".";
            ExtendSim_Link.InitializeEG(nEvalID);

            ExtendSim_Link.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            ExtendSim_Link.CloseModelLinkage();
        }
        #endregion

        #region DELETE
        public static void DeleteProj()
        {
            string sMDB = @"C:\a\SimLink2.0_NewHaven_LOCAL_TEST_DELETE.mdb";
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sMDB;
            int nProjID = 26;
            swmm5022_link swmm_source = new swmm5022_link();
            swmm_source.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);
            swmm_source.DeleteProj(true, nProjID);
            // clean up the bad setups
            //         for (int i = 3; i <= 15; i++)
            ////        {
            //          swmm_source.DeleteProj(true, i);
            //    }
        }
        #endregion

        #region hpc

        public static void TestHPC_Class_InitByConfig_Exe(bool useAWS = false)
        {
            // first, some code from cli.... this should be running there really
            //alcosan test case
            //eg: 
            string argstr = @"\\HCHFPP01\groups\WBG\Optimization\SimLink\code\simlink\Engine\ExampleProjects\swmm_via_condor\swmm_hpc_condor.xml";
            if (useAWS)
            {
                argstr = @"\\HCHFPP01\groups\WBG\Optimization\SimLink\code\simlink\Engine\ExampleProjects\swmm_via_aws\swmm_hpc_aws.xml";
            }
            string[] args = new string[] { "-config", argstr };
            var source = new ArgvConfigSource(args);
            //Nini.NiniExtend.AddCommandLineSwtiches(args, source);         // read flags into a var called source
            CommonUtilities.AddCommandLineSwtiches(args, source);
            var config = source.Configs["Base"];
            IConfigSource configXML = null;
            configXML = new XmlConfigSource(config.GetString("config"));
            // 
            simlink mySimlink = SIM_API_LINKS.CommonUtilities.GetSimLinkObject(configXML.Configs["simlink"].GetString("type", "simlink"));
            Console.WriteLine("Beginning initialization using file:" + config.GetString("config")); // let user know we begin init.
            bool bValidInit = mySimlink.InitializeByConfig(configXML);

            // initialized. now do something!
            mySimlink.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            mySimlink.CloseModelLinkage();


            // close it down

        }


        public static void TestHPC_Class_InitByConfig_Exe_EPANET(bool useAWS = false)
        {
            // first, some code from cli.... this should be running there really
            //eg: 
            //string[] args = new string[] { "-config", @"Z:\Optimization\SimLink\code\simlink\Engine\ExampleProjects\EPANET_via_condor\EPANET_hpc_condor.xml" };
            string argstr = "";
            if (useAWS)
            {
                argstr = @"\\hchfpp01\Groups\WBG\Optimization\SimLink\code\simlink\Engine\ExampleProjects\EPANET_via_aws\EPANET_hpc_aws.xml";
            }
            // met added 10/28/16
            argstr = @"C:\Users\Mason\Documents\Optimization\SimLink\SimLink_v2.6\simlink2\Engine\ExampleProjects\call_simlink_external_PYTHON\epanet_config.xml";
            string[] args = new string[] { "-config", argstr };
            var source = new ArgvConfigSource(args);
            //Nini.NiniExtend.AddCommandLineSwtiches(args, source);         // read flags into a var called source
            CommonUtilities.AddCommandLineSwtiches(args, source);         // read flags into a var called source
            var config = source.Configs["Base"];
            IConfigSource configXML = null;
            configXML = new XmlConfigSource(config.GetString("config"));
            // 
            simlink mySimlink = SIM_API_LINKS.CommonUtilities.GetSimLinkObject(configXML.Configs["simlink"].GetString("type", "simlink"));
            Console.WriteLine("Beginning initialization using file:" + config.GetString("config")); // let user know we begin init.
            bool bValidInit = mySimlink.InitializeByConfig(configXML);

            // initialized. now do something!
            mySimlink.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            mySimlink.CloseModelLinkage();

        }


        public static void TestHPC_Class_InitByConfig_Exe_EXTENDSIM(bool useAWS = false)
        {
            // first, some code from cli.... this should be running there really
            //eg: 
            string[] args = new string[] { "-config", @"\\hchfpp01\Groups\WBG\Optimization\SimLink\code\simlink\Engine\ExampleProjects\ExtendSim_via_Condor\ExtendSim_hpc_Condor.xml" };
            if (useAWS)
            {
                string argstr = @"\\hchfpp01\Groups\WBG\Optimization\SimLink\code\simlink\Engine\ExampleProjects\ExtendSim_via_aws\ExtendSim_hpc_aws.xml";
                args = new string[] { "-config", argstr };
            }

            var source = new ArgvConfigSource(args);
            //Nini.NiniExtend.AddCommandLineSwtiches(args, source);         // read flags into a var called source
            CommonUtilities.AddCommandLineSwtiches(args, source);         // read flags into a var called source
            var config = source.Configs["Base"];
            IConfigSource configXML = null;
            configXML = new XmlConfigSource(config.GetString("config"));
            // 
            simlink mySimlink = SIM_API_LINKS.CommonUtilities.GetSimLinkObject(configXML.Configs["simlink"].GetString("type", "simlink"));
            Console.WriteLine("Beginning initialization using file:" + config.GetString("config")); // let user know we begin init.
            bool bValidInit = mySimlink.InitializeByConfig(configXML);

            // initialized. now do something!
            mySimlink.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            mySimlink.CloseModelLinkage();

        }

        #endregion

        public static void Write_XML()
        {
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\MSD_Cinci\analysis\simlink\Simlink1_nofo_msd.accdb";
            string sConfig = @"";

            swmm5022_link swmm = new swmm5022_link();
            int nEvalID = 2;
            swmm.InitializeModelLinkage(sConn, 0);
            swmm.InitializeEG(nEvalID);
            string sDir = @"C:\Users\Mason\Documents\WORK\MSD_Cinci\analysis\simlink\simlink_xml\";
            swmm.WriteXML(sDir, "nada");

            DataSet ds2 = new DataSet();
            ds2.ReadXml(@"c:\a\slite_result_ts.xml");
            ds2.WriteXml(@"c:\a\test_2ndgen.xml");
        }

        public static void ReadXML_Schema()
        {
            DataSet ds = new DataSet();
            string sFilename = @"C:\a\test_xml_out.xml";
            ds.ReadXml(sFilename);
            
        }

        /// <summary>
        /// Basic testing of NOAA web service (updating/simplifying Amar's work)
        /// start with supporting just one var type and one data type
        /// </summary>
        public static void NOAA_WebService1_ExternalData()
        {
            Dictionary<string,string> dictParams = new Dictionary<string,string>();

 /*           DateTime dtStartRequest = DateTime.Now + TimeSpan.FromHours(-40);      //+TimeSpan.FromHours(-600);
            DateTime dtEndRequest = DateTime.Now + TimeSpan.FromHours(24);

      //      dictParams.Add("start_date", dtStart.ToString());
     //       dictParams.Add("end_date", dtEnd.ToString());

            dictParams.Add("start_date", dtStartRequest.ToString());
            dictParams.Add("end_date", dtEndRequest.ToString());

  * 
  */ 
            dictParams.Add("type", "point");        // this is the default, but better to say it
            dictParams.Add("latlong", "41.88,-87.63");     // add as comma separted double

            noaa_webservice noaa = new noaa_webservice(-1, -1, -1, dictParams, 1);
            double[][,] dVals = noaa.RetrieveData();
            if (dVals[0] != null) 
            {
                double dSum = TimeSeries.SummaryStatForTS(dVals[0],(int)Perf_FunctionOnLinkedData.Sum);
                Console.WriteLine("total val: {0}", dSum);
            }
        }
        

        public static void WebProvider()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("url", "https://dashboard.cincywsd.com/watershed_ops/pulldata/pulldata_test.py");  //test location (test of test)  : https://dashboard.cincywsd.com/watershed_ops/pulldata/test.htm
            web_provider web = new web_provider(1, 1, 1, dict, 1, 2);
            TimeSpan t = new TimeSpan(0, 6, 0, 0);
            // create a dictionary with your request.
            dict.Add("start_date", "2017-07-27 05:00:00");
            dict.Add("end_date", "2017-07-27 12:00:00");
            dict.Add("tag_names", "M2_28308017_930002_LI_EL_CALC,M2_28308017_930003_LI_EL_CALC");
            dict.Add("sampling_interval", "0");
            dict.Add("test_not_handled_by_web_service", "please_work");     //did work; extra items no problem
            double[][,] dVals = web.RetrieveData(null, new int[]{1,2});
        }


        public static void WebPushing()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("url", "https://dashboard.cincywsd.com/watershed_ops/pulldata/push.py");
            web_provider web = new web_provider(1, 1, 1, dict, 1, 2);
            // create a dictionary with your pushing data.
            Dictionary<string, string> dictPushingValues = new Dictionary<string, string>();
            dictPushingValues.Add("csv_data", "timestamp,L3_42613018_930001_FQI_5MIN_SWMM\n2017-09-12 13:50:00,13.50\n2017-09-12 13:55:00,13.55\n2017-09-12 14:00:00,14.00\n2017-09-12 14:05:00,14.05");
        //    string sReponse = web.Push(dictPushingValues);
            string s2 = "timestamp,L3_42613018_930001_FQI_5MIN_SWMM,L3_40412023_930001_FQI_5MIN_SWMM,L3_45309036_930001_FQI_5MIN_SWMM,L3_45301016_930001_FQI_5MIN_SWMM,L3_45306015_930001_FQI_5MIN_SWMM,L3_45306033_930001_FQI_5MIN_SWMM,L3_47606046_930001_FQI_5MIN_SWMM,L3_47604069_930001_FQI_5MIN_SWMM,L3_47603008_930001_FQI_5MIN_SWMM,L3_47514007_930001_FQI_5MIN_SWMM,L3_47514002_930001_FQI_5MIN_SWMM,L3_47507013_930001_FQI_5MIN_SWMM,L3_47501029_930001_FQI_5MIN_SWMM,L3_47501006_930001_FQI_5MIN_SWMM,L3_45411006_930001_FQI_5MIN_SWMM,L3_45402013_930001_FQI_5MIN_SWMM,L3_45607005_930001_FQI_5MIN_SWMM,L3_45601033_930001_FQI_5MIN_SWMM,L3_45209009_930001_FQI_5MIN_SWMM,L3_45407022_930001_FQI_5MIN_SWMM,L3_40505016_930001_FQI_5MIN_SWMM,L3_46905017_930001_FQI_5MIN_SWMM,L3_42716037_930001_FQI_5MIN_SWMM,L3_42605031_930001_FQI_5MIN_SWMM,L3_45406004_930001_FQI_5MIN_SWMM,L3_45301004_930001_FQI_5MIN_SWMM,L3_42706010_930001_FQI_5MIN_SWMM,L3_40903009_930001_FQI_5MIN_SWMM,L3_40903003_930001_FQI_5MIN_SWMM,L3_40905004_930001_FQI_5MIN_SWMM,L3_45709011_930001_FQI_5MIN_SWMM,L3_45805003_930001_FQI_5MIN_SWMM,L3_40504008_930001_FQI_5MIN_SWMM,L3_40613032_930001_FQI_5MIN_SWMM,L3_45506003_930001_FQI_5MIN_SWMM,L3_40610039_930001_FQI_5MIN_SWMM,L3_40611032_930001_FQI_5MIN_SWMM,L3_42610011_930001_FQI_5MIN_SWMM,L3_42614013_930001_FQI_5MIN_SWMM,L3_42611024_930001_FQI_5MIN_SWMM,L3_47901015_930001_FQI_5MIN_SWMM,L3_47901008_930001_FQI_5MIN_SWMM,L3_47801004_930001_FQI_5MIN_SWMM,L3_47401003_930001_FQI_5MIN_SWMM,L3_42101004_930001_FQI_5MIN_SWMM,L3_42611016_930001_FQI_5MIN_SWMM,L3_45514044_930001_FQI_5MIN_SWMM\r\n"
            + "2017-09-12 13:50:00,2.75317663207247E-07,0,1.98929996535717E-06,0,2.31295658981168E-06,0,0.00144670168880089,0,0,0,0,0,0,0,1.98813500369991E-06,0,0,0,0.0219431039219399,0,9.20942263543627,0,0,0.00161644454114167,5.78256116602945E-05,0,0,0.398379696485081,0,0,0,0,0,4.74842888655655E-07,0,0,0,0,0,0,0,0,-0.00197563527232462,0,0,0,0";


            Dictionary<string, string> d2 = new Dictionary<string, string>();
            d2.Add("csv_data", s2);
            string sResponse2 = web.Push(d2);
        }


        public static void CSVProvider()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("file", @"C:\Users\sp017586\Documents\BitbucketProjects\Simlink\Engine\ExampleProjects\TestingCSVReadingData.csv");
            external_csv csv = new external_csv(1, 1, 1, dict, 1, 3);
            double[][,] dVals = csv.RetrieveData();
        }

        public static void RealTime_Chicago_SFBT()
        {

    //        string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\PROJ\MetroWaterReclamDist\494887CombinedSewer\proj\Analysis\Simlink\Realtime\simlink1_ocal_v128a_mwrdgc.accdb";
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\MWRDGC\SFBT\simlink\analysis\simlink1_ocal_v128a_mwrdgc.accdb";
            swmm5022_link swmm = new swmm5022_link();
            int nCode = 3;  // 3;
            switch(nCode){
                case 1:
                    realtime rt;
                    int nEvalID = 95;
                    int nTriggerMethod = 0;
                    Dictionary<string, string> dictArgs = new Dictionary<string, string>
                    {
	                    {"start_timestamp", "z_-1h"},         //use a code: first letter z- means special code.... -h - start at most recent hour
	                    {"duration", "1440"},
	                    {"offset", "0"},
                        {"ts_interval", "3600"},
                        {"check_timestamp","y"}
	                };
                    rt = new realtime(1, nEvalID, nTriggerMethod, sConn, 0, dictArgs);
                    rt._bSkipTSCheck = false;
                    int nRunLoops = 1;
                    rt.Run(nRunLoops, 30);
                    //rt.Run(1);      //run for a few loops
                    rt.Close();
                    break;
                case 2:// test from config
                    string sConfigFile = @"\\hchfpp01\Groups\WBG\Optimization\SimLink\code\simlink\Engine\ExampleProjects\realtime\realtime_init_swmm_sfbt.xml";
                    bool bValid = true;
                    IConfigSource configXML = new XmlConfigSource(sConfigFile);
                    realtime rt_fromconfig = realtime.InitializeByConfig(configXML, out bValid);            //   new realtime(1, nEvalID, nTriggerMethod, sConn, 0, dictArgs);
                    rt_fromconfig.Run(1);   // assuming that we have the 
                    rt_fromconfig.Close();           
                    break;
                case 3:
                    swmm.InitializeModelLinkage(sConn, 0);
                    swmm.InitializeEG(96);
                    swmm.ProcessEvaluationGroup(new string[0]);   
                    swmm.CloseModelLinkage();
                    break;
                case 4:         // extract external dta
                    swmm.InitializeModelLinkage(sConn, 0);
                    swmm.InitializeEG(96);
                    swmm.ExtractExternalData();
                    swmm.WriteTimeSeriesToRepo(new[] { RetrieveCode.Secondary, RetrieveCode.Aux, RetrieveCode.AuxEG }); //WriteSecondaryAndAuxTS_ToRepo(); //SP 15-Feb-2017 Use refactors function to pass in TS types to save
                    break;
                case 5:         // create zero events
                    int nScenarioID = 15029;        //14979;
                    swmm.InitializeModelLinkage(sConn, 0);
                    swmm.InitializeEG(97);      //96);
                    swmm.SetActiveScenarioID(nScenarioID);
                    swmm.LoadScenarioDatasets(nScenarioID, 100, false);
                    swmm.CreateMissingEvents(nScenarioID, 2147,1);
                    break; 
                case 6:             // dome DSS export
                    string sDSS96 = @"C:\Users\Mason\Documents\WORK\MWRDGC\SFBT\simlink\model\DSS\dss96.dss";
                    string sDSS97 = @"C:\Users\Mason\Documents\WORK\MWRDGC\SFBT\simlink\model\DSS\dss97.dss";

                    swmm.InitializeModelLinkage(sConn, 0);
                    swmm.InitializeEG(96);      //96);
                    Console.WriteLine(string.Format("Begin DSS Export 1 at time {0}", System.DateTime.Now));
                    swmm.ExportTimeseriestoDSSByEval(96, sDSS96, false);
                    DSS_CustomPart custom = new DSS_CustomPart("normalized",true);
                    Dictionary<string, DSS_CustomPart> dict = new Dictionary<string, DSS_CustomPart>();
                    dict.Add("a", custom);
                    Console.WriteLine(string.Format("Begin DSS Export 2 at time {0}", System.DateTime.Now));
                    swmm.ExportTimeseriestoDSSByEval(96, sDSS96,false,true,null,dict);  //export a normalized version of the values
                    break;
                    swmm.InitializeEG(97);  
                    swmm.ExportTimeseriestoDSSByEval(97, sDSS97, false);
                    swmm.ExportTimeseriestoDSSByEval(97, sDSS97, false, true, null, dict);
                    break;
             }
        }

        /// <summary>
        /// Extract timeseries and 
        /// //todo : parameterize
        /// </summary>
        public static void SynthTimeSeries_ToScenario_ToDSS(bool bForceUseSameTSDict=false)
        {
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\MillCreek\Analysis\SimLink\simlink_millCreek.accdb";
            int nEvalID = 167;
            int nEvalID_Simlink = 173;          //synthetic
            string sDSS_Out = @"c:\a\combined_flows.dss";
            int nStartScenario = 16339;
            int nEndScenario = 16344;
            string sLabel = "SYNTH_" + nStartScenario+ "_" + nEndScenario + "_eg_" + nEvalID;
            bForceUseSameTSDict = true;     // coming up one short otherwise- need to figure out why the "simlink" var does not initialize the same way as the swmm var

            swmm5022_link swmm = new swmm5022_link();
            swmm.InitializeModelLinkage(sConn, 0);
            swmm.InitializeEG(nEvalID);
            swmm._dResultTS_Vals = swmm.SynthTimeSeries(nStartScenario, nEndScenario, false);  // get everything combined
            swmm5022_link simlink_swmm = new swmm5022_link();
            simlink_swmm._nActiveModelTypeID = CommonUtilities._nModelTypeID_Simlink;        //BOJANGLES: this should be set in the initmodellinkage- however need to figure out how to test if called as base class.
            simlink_swmm.InitializeModelLinkage(sConn, 0);
            simlink_swmm.InitializeEG(nEvalID_Simlink);
            if (bForceUseSameTSDict)
                simlink_swmm._dictResultTS_Indices = swmm._dictResultTS_Indices;     // set the dict indices directly
            int nNewScenarioID = simlink_swmm.InsertScenario(sLabel,swmm._dResultTS_Vals,swmm._sTS_GroupID, true);
       // now stored on insert per last arg     simlink.WriteTimeSeriesToRepo();            // store as hdf5
            string[] sScenARR = new string[] { nNewScenarioID.ToString() };

            Dictionary<string, string> dictArgs = new Dictionary<string, string>
            {
	            {"dss_start_time", "2017-8-4 00:00:00"},         //change back to starting at zero
                {"ts_interval", "300"},                     //
	            {"cpart_lookup", "lkp"}
	        };

        //    simlink.ExportTimeseriestoDSSByEval(nEvalID_Simlink, sDSS_Out, true, false, sScenARR,null, dictArgs);
         //   simlink.ExportTimeseriestoDSSByEval(nEvalID_Simlink, sDSS_Out, true, true, sScenARR, null, dictArgs);       //export normalized
        }

        /// <summary>
        /// Test rt
        /// 
        /// Add ability to run with no db backend.
        /// </summary>
        /// <param name="bUseDB"></param>
        public static void MillCreek_RealTime_Test01_ExternalData(bool bUseDB=true)
        {
            // local vals from original dev 
     //       string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\MSD_Cinci\analysis\simlink\simlink_millCreek.accdb";
            //string sConfig = @"C:\Users\Mason\Documents\Optimization\SimLink\SimLink_v2.6\simlink2\Engine\ExampleProjects\NoDB_Backend\init_simlink_lite.xml";
            //string sDir = @"C:\Users\Mason\Documents\Optimization\SimLink\SimLink_v2.6\simlink2\Engine\ExampleProjects\NoDB_Backend\data";

            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\MillCreek\Analysis\SimLink\simlink_millCreek.accdb";
            //string sConn = @"Provider =Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\MillCreek\simlink_millCreek.accdb";
            string sConfig = @"\\hchfpp01\Groups\WBG\MillCreek\Analysis\SimLink\SSO700\init_simlink_lite.xml";
            string sDir = @"\\HCHFPP01\groups\WBG\Optimization\SimLink\code\simlink\Engine\ExampleProjects\NoDB_Backend\data";

            swmm5022_link swmm = new swmm5022_link();
            //int nEvalID = 95;  // small test case
          //  int nEvalID = 167;  // ful model - Mill Creek
            //int nEvalID = 170;  // full model - Muddy Creek
            int nEvalID = 169;  // full model - Little Miami

            int nTriggerMethod =0;


            //SP 13-Mar-2017 Test basic
            //int nContext = 1;
            //swmm.InitializeModelLinkage(sConn, 0, false);           //access
            //swmm._sDNA_Delimiter = ".";
            //swmm.InitializeEG(nEvalID);
            //swmm.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            //swmm.CloseModelLinkage();



            /*        Dictionary<string,string> dictArgs = new Dictionary<string,string>
                    {
                        {"start_timestamp", "11/28/2016 15:35"},
                        {"duration", "60"},
        >>>>>>> 32d1c6409c9e6857d93cc9e83510b34ea24ecbdf
                        {"offset", "0"},
                        {"ts_interval", "300"}
                    };*/

            Dictionary<string, string> dictArgs = new Dictionary<string, string>
            {
	            {"start_timestamp", "2017-8-6 00:00:00"},         //change back to starting at zero
	            {"duration", "1440"},                 // 60
	            {"offset", "0"},
                {"ts_interval", "300"},
                {"check_timestamp","y"} //,
                //{"export_group_scalar", "{2,0.003472222},{3,1234}"},     //embed a dictionary, with scalars per group. - SP 10-Oct-2017 now part of ExternalGroup parameters
      //          {"ScenStart", "2"},     //embed a dictionary, with scalars per group.
        //        {"ScenEnd", "3"}     //embed a dictionary, with scalars per group.
	        };

            realtime rt;
            bool bLoop = true;

      
            int nRunLoops =  30;
            if (!bLoop) //   run a single test run            
            {
                if (bUseDB)
                {
                    rt = new realtime(1, nEvalID, nTriggerMethod, sConn, 0, dictArgs, 3);
                    rt._bSkipTSCheck = true;
                }
                else
                {
                    swmm.LoadReference_EG_DatasetsFromXML(sDir);
                    rt = new realtime(1, nEvalID, nTriggerMethod, sConfig, (int)DB_Type.NONE, dictArgs);
                }
                rt.Run(1);      //run for a few loops
                rt.Close();
            }
            else
            {
                if (false)     // run a list
                {
                    dictArgs["duration"] = "1440";    //
                    //DateTime startdate = new DateTime(2017, 2, 1); //SP 14-Mar-2017 now creating a list of start dates

                    //SP 14-Mar-2017 specify dates we want to run by loading into a list
                    List<DateTime> lstdtStartDates = new List<DateTime>();
                  //  lstdtStartDates.Add(new DateTime(2017, 1, 3));
                    lstdtStartDates.Add(new DateTime(2017, 1, 11));
                    lstdtStartDates.Add(new DateTime(2017, 1, 12));
                  //  lstdtStartDates.Add(new DateTime(2017, 1, 14));
                    //lstdtStartDates.Add(new DateTime(2017, 1, 17));
                    //lstdtStartDates.Add(new DateTime(2017, 1, 20));
                    //lstdtStartDates.Add(new DateTime(2017, 2, 07));
                    //lstdtStartDates.Add(new DateTime(2017, 2, 12));
                    lstdtStartDates.Add(new DateTime(2017, 2, 28));
                    lstdtStartDates.Add(new DateTime(2017, 3, 1));
                    lstdtStartDates.Add(new DateTime(2017, 3, 20));
                    lstdtStartDates.Add(new DateTime(2017, 3, 26));

                    int nCounter = 1;
                    foreach (DateTime dtStartDate in lstdtStartDates)
                    {
                        DateTime dtCurrentDate = dtStartDate;
                        dictArgs["start_timestamp"] = dtCurrentDate.ToString("yyyy-MM-dd HH:mm:ss");
                        Console.WriteLine("Begin run {0} for start time {1}:{2}", nCounter, dtCurrentDate, System.DateTime.Now);
                        rt = new realtime(1, nEvalID, nTriggerMethod, sConn, 0, dictArgs, 2);       // this method has a db backend
                        rt._bSkipTSCheck = true;
                        rt.Run(1);
                        dtCurrentDate = dtCurrentDate + TimeSpan.FromDays(1);
                        rt.Close();
                        nCounter++;
                    }
                }
                else   // run over the loops
                {
                    dictArgs["duration"] = "1440";    // "1440";    //
                    DateTime dtCurrentDate = Convert.ToDateTime("2017-10-5 00:00:00");// enter date
                    int nLoops = 4;
                    for (int i = 0; i < nLoops; i++)
                    {
                        dictArgs["start_timestamp"] = dtCurrentDate.ToString("yyyy-MM-dd HH:mm:ss");
                        Console.WriteLine("Begin run {0} for start time {1}:{2}", i + 1, dtCurrentDate, System.DateTime.Now);
                        rt = new realtime(1, nEvalID, nTriggerMethod, sConn, 0, dictArgs, 2);       // this method has a db backend
                        rt._bSkipTSCheck = true;
                        rt.Run(1);
                        dtCurrentDate = dtCurrentDate + TimeSpan.FromDays(1);   //todo : adjust based upon the model duratoin
                        rt.Close();
                    }
                }
            }
            //swmm.ExportTimeseriestoDSSByEval(nEvalID, @"C:\a\MillCreek.dss", false);
        }

        public static void Kinsall()
        {
			string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\mthroneb\Documents\Kinsall_Local\Analysis\simlink_millCreek_GMT_Kinsall.accdb";		//\\hchfpp01\Groups\WBG\Kinsall\Analysis\Simlink\simlink_millCreek_GMT_Kinsall.accdb";

            EPANET_link _epa = new EPANET_link();
            int nEvalID = 304;
            int nTriggerMethod = 0;

            _epa.InitializeModelLinkage(sConn, 0, false);
            _epa.InitializeEG(nEvalID);
            _epa.ProcessEvaluationGroup(new string[] { });
        }


        /// <summary>
        /// Test rt
        /// </summary>
        /// <param name="bUseDB"></param>
        public static void RealTime_TestEPANET(bool bUseDB = true)
        {
            // local vals from original dev 
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\phlfpp01\Users\sp017586\SmartWater\SmartWater_Simlink.mdb";

            EPANET_link _epa = new EPANET_link();
            int nEvalID = 57;
            int nTriggerMethod = 0;

            Dictionary<string, string> dictArgs = new Dictionary<string, string>
            {
                {"start_timestamp", "5/29/2014 00:00"}, //start time of simulation - use default of NOW if ommited
	            //{"duration", "1440"}, //length of simulation use default of 1440
	            {"offset", "0"}, //not sure what this is used for. MET?
                {"ts_interval", "3600"} // frequency at which to initiate a simulation 
	        };

            int nAction = 1;

            switch (nAction)
            {
                case 1:
                    realtime rt = new realtime((int)SimulationType.EPANET, nEvalID, nTriggerMethod, sConn, 0, dictArgs);
                    rt.Run(24);      //run for a few loops
                    rt.Close();
                    break;

                case 2:

                    string sDSS = @"C:\a\RealTimeResults.dss";
                    _epa.InitializeModelLinkage(sConn, 0, false);
                    _epa.InitializeEG(nEvalID);
                    Console.WriteLine(string.Format("Begin DSS Export 1 at time {0}", System.DateTime.Now));
                    _epa.ExportTimeseriestoDSSByEval(nEvalID, sDSS, false);
                    //DSS_CustomPart custom = new DSS_CustomPart("normalized", true);
                    //Dictionary<string, DSS_CustomPart> dict = new Dictionary<string, DSS_CustomPart>();
                    //dict.Add("a", custom);
                    //Console.WriteLine(string.Format("Begin DSS Export 2 at time {0}", System.DateTime.Now));
                    //_epa.ExportTimeseriestoDSSByEval(nEvalID, sDSS, false, true, null, dict);  //export a normalized version of the values
                    break;

                default:
                    break;
            }
      }

        public static void Norfolk_EvaluateEG()
        {
            string sConnMOD_EPANETDemo1 = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\norfolk\simlink\Simlink1_FromTemplate_161028_Norfolk.accdb"; 
            EPANET_link epa = new EPANET_link();
            int nEvalID = 1;
            epa._sDNA_Delimiter = ".";
            epa.InitializeModelLinkage(sConnMOD_EPANETDemo1, 0); 
            epa.InitializeEG(nEvalID);
            epa.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            epa.CloseModelLinkage();

        }


        /// <summary>
        /// Testing operations on a very simple model
        /// </summary>
        public static void SWMM_Basic()
        {
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\NeuralNet_Test\simlink\Simlink_neuralnet.accdb";
            int nEvalID = 5;    // 
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 1;
            swmm.InitializeModelLinkage(sConn, 0, false);           //access
            swmm._sDNA_Delimiter = ".";
            swmm.InitializeEG(nEvalID);
            swmm.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            swmm.CloseModelLinkage();
        }

        public static void ExportEG_toCSV_Rough(){
            string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\WORK\NeuralNet_Test\simlink\Simlink_neuralnet.accdb";
            int nEvalID = 4;    // 
            string sFilename = @"c:\a\swmm_cso_out.csv";
            swmm5022_link swmm = new swmm5022_link();
            int nContext = 1;
            swmm.InitializeModelLinkage(sConn, 0, false);           //access
            swmm._sDNA_Delimiter = ".";
            swmm.InitializeEG(nEvalID);
            swmm.WriteTS_to_CSV(nEvalID,sFilename);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            swmm.CloseModelLinkage();

        }
        public static void TestIW_UserDefinedCSV(){

            int nSimID = 2;             //reset if you use a new database...
            //    int nSimID = 10;
            string sMasterDB = @"\\hchfpp01\PROJ\MetroWaterReclamDist\494887CombinedSewer\proj\Models\CTSM_CS-TARP_IntegratedModel\TypYear_Test.iwm";
            //           string sSimLinkDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\Dekalb\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL_V2.mdb";
            int nEvalID = 29;
            iw iw = new SIM_API_LINKS.iw();
            iw._sMasterDatabase = sMasterDB;            //todo: should come from 
            iw._nSimID = nSimID;
            iw._bSKIP_IW_Init = false;                   // rare - cannot run sim
            iw.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);
            iw.InitializeIW_ModelLinkage("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sMasterDB, 0, 0.9);
            iw.InitializeEG(nEvalID);
            string[] a = new string[] { };
            iw.ProcessEvaluationGroup(a);
            //    iw.ProcessScenario(121, nEvalID, nEvalID, @"c:\a\nothing.txt",8849);
            iw.CloseModelLinkage();

        }



        #region ModifyEPANETINPForFireflow

        public static void ModifyEPANETINPForFireflow()
        {
            EPANET_link _epa = new EPANET_link();
            string sFile = @"C:\Users\sp017586\Documents\Optimization\Simlink_EPANETDemo1\EPANETModel\WDS_Training_Model_optimization.inp";
            string sRTPFile = @"C:\Users\sp017586\Documents\Optimization\Simlink_EPANETDemo1\EPANETModel\WDS_Training_Model_optimization.rpt";
            string sXMLFFConfig = @"C:\Users\sp017586\Documents\BitbucketProjects\Simlink\Engine\ExampleProjects\EPANET_ModifyModelForFireFlow\Simlink_EPANET_FireFlow_config.xml";

            Console.WriteLine(_epa.ModifyEPANETINPFileForFireFlow(sFile, sRTPFile, sXMLFFConfig));
        }

        #endregion

        #region COHORT
            public static void   testCohort1(){
                swmm5022_link swmm = new swmm5022_link();
            //     string sINP = @"C:\Users\Mason\Documents\WORK\Alcosan\Models\Base\for_import\TC_Alt_TT-BA16A_2YrSummerStorm_Jun2011_v2._REMOVE_ExternalFiles.inp";
                string sMDB_CONN = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Mason\Documents\WORK\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";
            // old    string sMDB_CONN = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\\hchfpp01\Groups\WBG\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";
                string[] a= new string[] { };
                int nProjID = 122;
                swmm.InitializeModelLinkage(sMDB_CONN,0, false);
                swmm.ProcessCohort(nProjID,1);
            }


            public static void testCohort2_IW()
            {
                int nProjID = 16;
                int nEvalID = 61;
                string sMasterDB = @"\\HCHFPP01\proj\MetroWaterReclamDist\494887CombinedSewer\proj\Models\CTSM_CS-TARP_IntegratedModel\CTSM_CS-TARP_Integrated_DB04b.iwm";          //3_v7.iwm";
                string sSimLlinkDB_Access = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\PROJ\MetroWaterReclamDist\494887CombinedSewer\proj\Analysis\AlternativeDB\simlink\simlink1_v80_forMWRD.accdb";
                iw iw = new SIM_API_LINKS.iw();
                iw._sMasterDatabase = sMasterDB;            //todo: should come from 
                iw._bSKIP_IW_Init = false;                   // rare - cannot run sim
                //use the following line for running off of chcapp10 (preferred)
                                    //    iw.InitializeModelLinkage(sConn_CHC_SQL_SERVER, 1, false);
                //use the following line for running off of access db for opt runs when necessary)
                iw.InitializeModelLinkage(sSimLlinkDB_Access, 0, false);   
                
                iw.InitializeIW_ModelLinkage("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sMasterDB, 0, 0.95);


             //   string sSQL = "insert into project (name,comment,[Catchment Group]) values ('new-x','comment2',3)";
             //   iw._dbContextMODEL.ExecuteNonQuerySQL(sSQL);

             //   string sSQL = "SELECT id,name FROM Project where (name = '" + "Base" + "')";
        //        string sSQL = "SELECT ID, [Catchment Group], Archived, Comment, CreationGUID, Frozen, ModificationGUID, ModifiedBy, Name, WhenArchived, WhenCreated, WhenModified, hotlinks, Hidden, Archive FROM project";
            
          //      DataSet ds = iw._dbContextMODEL.getDataSetfromSQL(sSQL);        //search on cg too?
            //    ds.Tables[0].Rows[0]["Name"] = "NEW-NAMe";
            //      ds.Tables[0].AcceptChanges;
             //   ds.Tables[0].Rows[0].AcceptChanges();
             //   ds.Tables[0].Rows[0].SetAdded();
           //     int nKey = iw._dbContextMODEL.InsertOrUpdateDBByDataset(true, ds, sSQL, true, true);
            //    Console.WriteLine(nKey);

                iw._bUseAltFrameworkScen = true;    
                iw.InitializeEG(nEvalID);
                iw.ProcessCohort(nProjID, 3);
            }
        #endregion


        #region TransferDefaultsFromSQLServerToMSAccess

        public static void TransferDefaultsFromSQLServerToMSAccess()
        {
            //string sConn_SOURCE = @"Server = PHLC799J72\SQLExpress; Database = Simlink_Template_FromScripts; Trusted_Connection = True;";
            string sConn_DEST = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Norfolk\Optimization4\simlink1_template_161028.accdb";
            //string sConnMOD_EPANETDemo1_SQLServer = @"Server = PHLC799J72\SQLExpress; Database = Simlink_Template_FromScripts; Trusted_Connection = True;";

            Oledb_Maintenance _dbMaintenance = new Oledb_Maintenance();

            _dbMaintenance.TransferDefaultValues(sConn_CHC_SQL_SERVER, 1, sConn_DEST, 0);
        }

        #endregion


        #region EPANet_Norfolk


        public static void RunEG_EPANET_Norfolk()
        {

            string sConnMOD_Norfolk_MSAccess = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\phlfpp01\Users\sp017586\Norfolk\Optimization7\Simlink1_FromTemplate_161028_Norfolk_Opt7.accdb";
        
            //string sConnMOD_EPANETDemo1_SQLServer = @"Server = PHLC799J72\SQLExpress; Database = Simlink_Template_FromScripts; Trusted_Connection = True;";

            EPANET_link epa = new EPANET_link();
            //swmm5022_link swmm = new swmm5022_link();
            //     swmm._bIsSimCondor = true;

            int nEvalID = -1;
            int nDBLocation = 1;

            if (nDBLocation == 2)
            {
                nEvalID = 55;
                epa.InitializeModelLinkage(sConnMOD_Norfolk_MSAccess, 0, epa._bIsSimCondor, "", 1);           //access
            }
            else
            {
                nEvalID = 1;
                epa.InitializeModelLinkage(sConnMOD_Norfolk_MSAccess, 0, epa._bIsSimCondor, "", 1);           //access

                //epa.InitializeModelLinkage(sConnMOD_EPANETDemo1_SQLServer, 1, epa._bIsSimCondor); //connected to CHC server
                //epa.InitializeModelLinkage(sConnMOD_EPANETDemo1_SQLServer, 1, epa._bIsSimCondor);           //sql server
            }

            epa._sDNA_Delimiter = ".";
            epa.InitializeEG(nEvalID);

            epa.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            epa.CloseModelLinkage();
        }


        #endregion

        #region EPANet_CityOfTracey


        public static void RunEG_EPANET_CityOfTracey()
        {

            string sConnMOD_Norfolk_MSAccess = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\phlfpp01\Users\sp017586\CityOfTracey\CityOfTracey_simlink.mdb";
            string sConnMOD_Norfolk_MSAccess_Local = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\phlfpp01\Users\sp017586\CityOfTracey\Results_Optimization6\CityOfTracey_simlink_OpenOptions.mdb";
            //string sConnMOD_EPANETDemo1_SQLServer = @"Server = PHLC799J72\SQLExpress; Database = Simlink_Template_FromScripts; Trusted_Connection = True;";

            EPANET_link epa = new EPANET_link();
            //swmm5022_link swmm = new swmm5022_link();
            //     swmm._bIsSimCondor = true;

            int nEvalID = -1;
            int nContext = 1;

            if (nContext == 1)
            {
                nEvalID = 55;
                epa.InitializeModelLinkage(sConnMOD_Norfolk_MSAccess_Local, 0, epa._bIsSimCondor, "", 1);           //access
            }
            else
            {
                nEvalID = 25;

                //epa.InitializeModelLinkage(sConnMOD_EPANETDemo1_SQLServer, 1, epa._bIsSimCondor); //connected to CHC server
                //epa.InitializeModelLinkage(sConnMOD_EPANETDemo1_SQLServer, 1, epa._bIsSimCondor);           //sql server
            }

            epa._sDNA_Delimiter = ".";
            epa.InitializeEG(nEvalID);

            epa.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            epa.CloseModelLinkage();
        }


        #endregion


        #region EPANet_Minneapolis

        public static void RunEG_EPANET_Minneapolis()
        {
            string sConnMOD_MSAccess_Local = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\MSP\Minneapolis_Simlink_170224.mdb";
            //string sConnMOD_EPANETDemo1_SQLServer = @"Server = PHLC799J72\SQLExpress; Database = Simlink_Template_FromScripts; Trusted_Connection = True;";

            EPANET_link epa = new EPANET_link();
            //swmm5022_link swmm = new swmm5022_link();
            //     swmm._bIsSimCondor = true;

            int nEvalID = -1;
            int nContext = 1;
            nEvalID = 56;



            epa.InitializeModelLinkage(sConnMOD_MSAccess_Local, 0, epa._bIsSimCondor, "", 1);           //access

            string sSQL = "Update tblScenario set hasbeenrun = false where evalgroupid_fk = " + nEvalID.ToString();
            epa._dbContext.ExecuteNonQuerySQL(sSQL);

            epa._sDNA_Delimiter = ".";
            epa.InitializeEG(nEvalID);

            epa.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            epa.CloseModelLinkage();
        }

        #endregion

        #region TS
        public static void TestTS_Import()
        {
            //string sConn = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Mason\Documents\WORK\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";
            // make relative so works for others...
            string sFileName = @"\\HCHFPP01\groups\WBG\Optimization\SimLink\code\simlink\Engine\ExampleProjects\Utilities\TimeSeriesImport\import_files.csv";

            swmm5022_link swmm = new swmm5022_link();
            string sMDB_CONN = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Mason\Documents\WORK\Alcosan\Analysis\Simlink\SimLink2.0_NewHaven_LOCAL.mdb";
            string[] a= new string[] { };
            int nEvalID = 71;
            swmm.InitializeModelLinkage(sConn_CHC_SQL_SERVER,1, false);
            swmm.InitializeEG(nEvalID);
            swmm.ImportAuxiliaryTimeSeriesByFileSpec(sFileName,true,null,-1);
            swmm.CloseModelLinkage();
        }

        #endregion


        #region ExtendSim_Devon

        public static void RunEG_ExtendSim_Devon()
        {
            string sConnMOD = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Devon\Devon_Simlink_Demo.mdb";
       

            extend_link ExtendSim_Link = new extend_link();
            int nEvalID = 58;

            int nContext = 0;
            if (nContext == 0)
            {
                ExtendSim_Link.InitializeModelLinkage(sConnMOD, 0, ExtendSim_Link._bIsSimCondor,"", Logging._nLogging_Level_3);           //access
            }
            /*else
            {
                ExtendSim_Link.InitializeModelLinkage(sConnMOD_SqlServer, 1, ExtendSim_Link._bIsSimCondor);           //sql server
            }*/

            ExtendSim_Link._sDNA_Delimiter = ".";
            ExtendSim_Link.InitializeEG(nEvalID);

            ExtendSim_Link.ProcessEvaluationGroup(new string[0]);      //  everything should be loaded on the object by now... nEvalID, CommonUtilities._nModelTypeID_SWMM, -1, true);
            ExtendSim_Link.CloseModelLinkage();
        }

        #endregion


        # region VISSIM Testing

        public static void VISSIM_DEMO()
        {
            int nEvalID = 189;
            vissim_link vissim = new vissim_link();

            //string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\phlfpp01\users\sp017586\Optimization\VISSIM\simlink_millCreek_GMT_VISSIM.accdb";
            //string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\sp017586\Documents\Vissim_Demo_temp\simlink_millCreek_GMT_VISSIM.accdb";
            string sCONN_MDB = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\hchfpp01\Groups\WBG\I270_Traffic\Analysis\Simlink\simlink_millCreek_GMT_VISSIM.accdb";

            bool bUseMDB = true;
            if (bUseMDB)
                vissim.InitializeModelLinkage(sCONN_MDB, 0, false);           //access    (set for server or met personal)    
               
            vissim._sDNA_Delimiter = ".";
            int nAction = 1;   

            switch (nAction)
            {
                case 1:     // BASIC EG RUN
                    //reset all demo cases HasBeenRun = 0
                    //string sSQL = "Update tblScenario set hasbeenrun = false where evalgroupid_fk = " + nEvalID.ToString();
                    //vissim._dbContext.ExecuteNonQuerySQL(sSQL);

                    vissim.InitializeEG(nEvalID);
                    vissim.ProcessEvaluationGroup(new string[0]);
                    break;
            }
            vissim.CloseModelLinkage();
        }
        #endregion
    }
}
