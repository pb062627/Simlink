using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;

namespace NOAA_Rainfall_Forecasting
{
    class DataIO
    {
        public static string GetNDFDDataforPoint(string sLat, string sLong, DateTime dtStartRequest, DateTime dtEndRequest)
        {
            weather.ndfdXML _Service = new weather.ndfdXML();
            string sResponse = "xml";
            NOAA_Rainfall_Forecasting.weather.productType prodType = NOAA_Rainfall_Forecasting.weather.productType.TimeSeriesOrig;
            DateTime dStartTime = dtStartRequest;
            DateTime dEndTime = dtEndRequest;
            
            // This parameter is used to specify the output units - set to metric units
            NOAA_Rainfall_Forecasting.weather.unitType unitType = NOAA_Rainfall_Forecasting.weather.unitType.e;

            // Parameters for which data is requested
            NOAA_Rainfall_Forecasting.weather.weatherParametersType _MyParams = new NOAA_Rainfall_Forecasting.weather.weatherParametersType();
            _MyParams.maxt = true;
            //_MyParams.mint = true;
            _MyParams.precipa_r = true;
            //<decimal> PointIds = sLatLong.Split(',').Select(decimal.Parse).ToList();
            decimal dLat = Convert.ToDecimal(sLat);       // PointIds.ElementAt(0);
            decimal dLng = Convert.ToDecimal(sLong);    // PointIds.ElementAt(1);
            
            // Call NDFDgen Method - returns data for a single point (lat/long)
            sResponse = _Service.NDFDgen(dLat, dLng, prodType, dStartTime, dEndTime, unitType, _MyParams);
            return sResponse;
        }


        public static string GetNDFDDataforGrid(string sLatLongBox, DateTime dtStartRequest, DateTime dtEndRequest)
        {
            weather.ndfdXML _Service = new weather.ndfdXML();
            string sResponseBox = "xml";
            
            // This parameter controls the resolution of the grid, set to default 5km
            decimal dResolution = 5.0m;
            NOAA_Rainfall_Forecasting.weather.productType prodType = NOAA_Rainfall_Forecasting.weather.productType.TimeSeriesOrig;
            DateTime dStartTime = dtStartRequest;
            DateTime dEndTime = dtEndRequest;
            
            // This parameter is used to specify the output units - set to metric units
            NOAA_Rainfall_Forecasting.weather.unitType unitType = NOAA_Rainfall_Forecasting.weather.unitType.e;
            
            // Parameters for which data is requested
            NOAA_Rainfall_Forecasting.weather.weatherParametersType _MyParams = new NOAA_Rainfall_Forecasting.weather.weatherParametersType();
            _MyParams.maxt = true;
            // _MyParams.mint = true;
            _MyParams.precipa_r = true;
            List<decimal> PointIds = sLatLongBox.Split(',').Select(decimal.Parse).ToList();
            decimal dLowerLeftLat = PointIds.ElementAt(0);
            decimal dLowerLeftLng = PointIds.ElementAt(1);
            decimal dUpperRightLat = PointIds.ElementAt(2);
            decimal dUpperRightLng = PointIds.ElementAt(3);

            // Call LatLonListSubgrid Method to get a list of lat/long within the grid
            string sLatLonListSubgrid = _Service.LatLonListSubgrid(dLowerLeftLat, dLowerLeftLng, dUpperRightLat, dUpperRightLng, dResolution);

            // Deserialize data from LatLonListSubgrid Method
            XmlSerializer _DeserializerLatLonList = new XmlSerializer(typeof(dwml));
            TextReader _TextReaderLatLonList = new StringReader(sLatLonListSubgrid);
            var resultLatLonListSubgrid = (dwml)_DeserializerLatLonList.Deserialize(_TextReaderLatLonList);

            // Get List of Latitudes and Longitudes from LatLonListSubgrid Method as a deserialized string
            string sListLat = resultLatLonListSubgrid.LatLonList;

            // Call NDFDgenLatLonList Method
            sResponseBox = _Service.NDFDgenLatLonList(sListLat, prodType, dStartTime, dEndTime, unitType, _MyParams);
            return sResponseBox;
        }
    }
}
