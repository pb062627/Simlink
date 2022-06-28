using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NOAA_Rainfall_Forecasting
{   // Utilized to deserialize XML string returned from NDFD 
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
