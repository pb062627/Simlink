using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIM_API_LINKS;

namespace SimClim_WRAP
{
    public class Program
    {
        //todo: add main code body here if you want to test simclim functionality

        #region SimClim Test Functions

        public static void SimClimTest1()
        {
            string sSQL_CONN_BWSC = "add valid connection string here";
            DateTime dtStart = new DateTime(1959, 1, 1);
            TimeSeries.TimeSeriesDetail tsDtl = new TimeSeries.TimeSeriesDetail(dtStart, IntervalType.Year, 1);
            string sFile_Precip = @"C:\Users\Mason\Documents\Optimization\SimLink\SimClimLink\Data\Naperville_Precip_1950_Daily.csv";
            List<TimeSeries> lstTS_Precip = TimeSeries.tsImportTimeSeries(sFile_Precip, tsDtl);
            // List<TimeSeries> lstTS_Temp = TimeSeries.tsImportTimeSeries(sFile_Temp, tsDtl);
            int nScenarioID = 7008;
            int nEvalID = 255;
            simclim_link simclim = new simclim_link();
            simclim.InitializeModelLinkage(sSQL_CONN_BWSC, 1, false);
            simclim.InitializeEG(nEvalID);
     // need to adapt to use updated simlink function added 9/21/16       simclim.ImportResultsTimeSeries(-1, nEvalID, nScenarioID, 2, 112, lstTS_Precip, tsDtl, "PRECIP Data Import- Victoria 1959", true);
            simclim.SC_CloseObject();
        }
        public static void SimClimSolo2()
        {
            string sModelFile_LNC_ACCESS = "add valid connection string here";
            int nScenarioID = 827;
            //     string sConnMOD_BWSC = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Mason\Documents\Optimization\SimLink\DB\OldVersions\SimClim-ISIS\RunScenarioMGR_V002_ISIS_v02_BE.mdb";

            int nEvalID = 262;
            simclim_link simclim = new simclim_link();
            simclim.test1();

            simclim.InitializeModelLinkage(sModelFile_LNC_ACCESS, 0, false);        //simclim scenario not yet in the db
            simclim.InitializeEG(nEvalID);
            simclim.ExecuteSimClimScenario();               //nEvalID);
            simclim.SC_CloseObject();
        }

        #endregion
    }
}
