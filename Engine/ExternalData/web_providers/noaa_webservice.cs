using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalData.weather;             // namespace to webservice
using System.Xml.Serialization;
using System.IO;




namespace SIM_API_LINKS
{
    /// <summary>
    /// Class provides access to NCDC / NOAA precipitation forecast
    /// A lot of additional data is available and can be added, however in PHASE1 this supports
    /// point-scale precip only.
    /// This website provides a great testing against the code, and shows what is available:
    ///     http://graphical.weather.gov/xml/sample_products/browser_interface/ndfdXML.htm
    /// Secondly, there are significant issues with the datetime request.
    /// It appears precip forecast can only be accessed for times from the current hour up to 24 hrs in advance
    /// Data is hourly.
    /// This is fine, but the dtsart/end request to the api call are funky, and often cause failure for unclear reasons.
    /// In the future, it is worth further exploring this API.
    /// </summary>



    public class noaa_webservice : ExternalData    // don't believe that web_provider at this time provides any needed functinoality as base class- so derive from core base class
    {
        public bool _bIsPoint = true;
        public double _dLatitude = 0;   // used for point (see sLatLongList for grid)
        public double _dLongitude = 0;
        public string _sLatLongList = "";   //lat,long for point, else gridded box
        public ndfdXML _Service = new ndfdXML();
        public productType _prodType = new productType();
        public unitType _unitType = new unitType();
        public weatherParametersType _weatherParams = new weatherParametersType();
        public DateTime _dtStart = new DateTime();
        public DateTime _dtEnd = new DateTime();


            #region INIT
        public noaa_webservice(int nID, int nSourceType, int nFormat, Dictionary<string, string> dictArgs, int nSQN = 1, int nColumnNumber = 1, string sColumnName = "1", bool bIsInput = false, bool bIsColIDName = false)
            : base(nID, nSourceType, nFormat, dictArgs, nSQN, nColumnNumber, sColumnName, bIsInput, bIsColIDName)
        {
            ProcessDictArgs(dictArgs);
            _weatherParams.precipa_r = true;            // set to precip for now
            _prodType = productType.timeseries;
         //   _prodType.precipa_r = true;

            _dtStart = System.DateTime.Now + TimeSpan.FromHours(-40);
            _dtEnd = System.DateTime.Now + TimeSpan.FromHours(24);
            //todo: process dictArgs to set key params
            //if (dictArgs.ContainsKey("url"))
            //    _sURL = dictArgs["url"];
            //else
            //{
            //    Console.WriteLine("web provider data source defined without key 'http'");
            //}
        }

        // search for specific keys in dict and set vars
        private void ProcessDictArgs(Dictionary<string, string> dictArgs)
        {
            if (dictArgs.ContainsKey("type"))       ////////////////////////////////   POINT OR GRID
            {
                if (dictArgs["type"] == "point")
                {
                    _bIsPoint = true;
                }
                else
                {
                    _bIsPoint = false;      // grid- not supported yet
                }
            }
            else
                _bIsPoint = true;
            if (dictArgs.ContainsKey("latlong"))   ////////////////////////////////   latLONG
            {
                if (_bIsPoint)
                {
                    string[] sVals = dictArgs["latlong"].Split('!');
                    bool bValid = Double.TryParse(sVals[0], out _dLatitude);
                    bValid = Double.TryParse(sVals[1], out _dLongitude);
                    if (!bValid) { }
                        // let somebody now
                }
                else
                {
                    _sLatLongList = dictArgs["latlong"];
                }
            }
            else
                {
                // problem- let user know!
                }


            ////////////////////////////////////////
            // met 1/23/17: do not read time in from external... too limited.
/*            bool b = DateTime.TryParse(dictArgs["start_date"], out _dtStart);
            b = DateTime.TryParse(dictArgs["end_date"], out _dtEnd);
 * 
 * */
        }
            
            #endregion
        public override double[][,] RetrieveData(Dictionary<string, string> dictRequestToAdd = null, int[] arrReturnColumns = null, Logging log = null)
        {
       //     AddRequestParams(dictRequestToAdd);
            double[][,] dValsReturn= new double[1][,];  // todo: support multiple returns

            string sResponse = "";
            try
            {
                if (_bIsPoint)
                    sResponse = _Service.NDFDgen((decimal)_dLatitude, (decimal)_dLongitude, _prodType, _dtStart, _dtEnd, _unitType, _weatherParams);
                else
                {
                    // grid not yet supported ; pull from 2016 lib
                }
                //retrieve the data
                dValsReturn[0] = ReadResponseToArray(sResponse);            //, lstRequestParams)
            }
            catch (Exception ex)
            {
                // log the issue
                Console.WriteLine("error receiving xml from noaa web service");
            }
            return dValsReturn;
        }

        public double[,] ReadResponseToArray(string sResponse){
             // Deserialize data from NDFDgenLatLonList Method       
            XmlSerializer _Deserializer = new XmlSerializer(typeof(dwml));
            TextReader _TextReader = new StringReader(sResponse);
            var result = (dwml)_Deserializer.Deserialize(_TextReader);
            double[,] dVals = GetPrecipitation(result,"unknown");
            return dVals;

        }

                // Precipitation data
        public static double[,] GetPrecipitation(dwml model, string sPointName)
        {
            if (model.Data != null && model.Data.Parameters != null)
            {
               // var res = model.Data.Parameters.Where(x => x.PointName == sPointName).FirstOrDefault().PrecipitationList.FirstOrDefault();
                var res = model.Data.Parameters.FirstOrDefault().PrecipitationList.FirstOrDefault();
 //               if (res == null)
                double[,] dReturn = new double[res.Values.Count,1];
                var startTime = model.Data.TimeLayout.Where(x => x.LayoutKey == res.TimeLayout).Select(x => x.StartValidTime).FirstOrDefault();
                for (int i = 0; i < res.Values.Count; i++)
                {
                    dReturn[i, 0] = res.Values[i];
                }
                return dReturn;
            }
            return null;
        }
    }
    

// deserialization support
 public class dwml
    {
        [XmlElement("data")]
        public Data Data { get; set; }
        [XmlElement("latLonList")]
        public string LatLonList { get; set; }
    }


    public class Data
    {
        [XmlElement("location")]
        public List<Location> Location { get; set; }
        [XmlElement("moreWeatherInformation")]
        public List<MoreWeatherInformation> MoreWeatherInformation { get; set; }
        [XmlElement("time-layout")]
        public List<TimeLayout> TimeLayout { get; set; }
        [XmlElement("parameters")]
        public List<Parameters> Parameters { get; set; }
    }

    public class Location
    {
        [XmlElement("location-key")]
        public string Key { get; set; }
        [XmlElement("point")]
        public Point Point { get; set; }
    }

    public class Point
    {
        [XmlAttribute("longitude")]
        public string Longitude { get; set; }

        [XmlAttribute("latitude")]
        public string Latitude { get; set; }
    }

    public class MoreWeatherInformation
    {
        [XmlText]
        public string Information { get; set; }
        [XmlAttribute("applicable-location")]
        public string ApplicableLocation { get; set; }
    }

    public class TimeLayout
    {
        [XmlAttribute("summarization")]
        public string Summarization { get; set; }
        [XmlAttribute("time-coordinate")]
        public string TimeCoordinate { get; set; }

        [XmlElement("layout-key")]
        public string LayoutKey { get; set; }

        [XmlElement("start-valid-time")]
        public List<string> StartValidTime { get; set; }
        [XmlElement("end-valid-time")]
        public List<string> EndValidTime { get; set; }
    }

    public class Parameters
    {
        [XmlElement("temperature")]
        public List<Temperature> TemperatureList { get; set; }
        [XmlElement("precipitation")]
        public List<Precipitation> PrecipitationList { get; set; }

        [XmlAttribute("applicable-location")]
        public string PointName { get; set; }
    }

    public class Temperature
    {
        [XmlAttribute("time-layout")]
        public string TimeLayout { get; set; }

        [XmlAttribute("units")]
        public string Unit { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("value")]
        public List<double> Values { get; set; }
    }

    public class Precipitation
    {
        [XmlAttribute("time-layout")]
        public string TimeLayout { get; set; }

        [XmlAttribute("units")]
        public string Unit { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("value")]
        public List<double> Values { get; set; }
    }

}
