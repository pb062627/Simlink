using System.Collections.Generic;
using NOAA_Rainfall_Forecasting;
using System;
using Quartz;

namespace ConsumeWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            // Establish new web service connection
            NOAA_Rainfall_Forecasting.weather.ndfdXML _Service = new NOAA_Rainfall_Forecasting.weather.ndfdXML();
          
            TestCall1();

            //NOAA_Rainfall_Scheduler.JobScheduler.Start();
        }

        private static void TestCall1()
        {
            DateTime dtStartRequest = DateTime.Now + TimeSpan.FromHours(-24);
            DateTime dtEndRequest = DateTime.Now + TimeSpan.FromHours(168);
            List<NOAA_DataParam> lstRequestParams = new List<NOAA_DataParam>();
            lstRequestParams.Add(NOAA_DataParam.Precipitation);
            lstRequestParams.Add(NOAA_DataParam.MaxDailyTemperature);

            // Test case to retrieve a list of records for a point
            string sLatLongPoint = "38.99,-77.02";
            string sResponse = DataIO.GetNDFDDataforPoint(sLatLongPoint, dtStartRequest, dtEndRequest);
            List<NDFD_TS_Container> ndfdPointResult = NDFD_TS_Container.PopulateNDFDData(sResponse, lstRequestParams);

            // Test case to retrieve a list of records for a grid
            string sLatLongBox = "38.99,-77.02,39.70,-104.80";
            string sResponseBox = DataIO.GetNDFDDataforGrid(sLatLongBox, dtStartRequest, dtEndRequest);
            List<NDFD_TS_Container> ndfdGridResult = NDFD_TS_Container.PopulateNDFDData(sResponseBox, lstRequestParams);

            //retieve a list of result records (For each point, for each param requested
            //note: a utility function is needed to convert current time in
            //it is possiblet/ hat the latlong box crosses a timezone, but this can be neglected
            //sResponse = DataIO.GetNDFDDataforPoint(sLatLong, dtStartRequest, dtEndRequest);
            //List<NDFD_TS_Container> lstResults = NDFD_TS_Container.PopulateNDFDDataRequest(sResponse, lstRequestParams);

            ////many times the user will just want to get pecip data for an area
            //sResponse = DataIO.GetNDFDDataforGrid(sLatLongBox, dtStartRequest, dtEndRequest);
            //List<NDFD_TS_Container> lstPrecipResults = NDFD_TS_Container.PopulateNDFDDataPrecipListRequest(sResponse);
        }
    }
}


