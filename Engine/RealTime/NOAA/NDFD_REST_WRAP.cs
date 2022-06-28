using System;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SIM_API_LINKS;

namespace NOAA_Rainfall_Forecasting
{

    /// <summary>
    /// record that is used for a min / max val.
    /// </summary>
    public class SummaryDetail
    {
        public List<DateTimeOffset> _dtStartTime { get; set; }
        public List<DateTimeOffset> _dtEndTime { get; set; }
        public List<double> _dVal { get; set; }
    }


    /// <summary>
    /// todo: extend?
    /// </summary>
    public enum NOAA_DataParam
    {
        Precipitation,
        MaxDailyTemperature,
        MinDailyTemperature
    }


    public class NDFD_TS_Container : SIM_API_LINKS.RealTimeRecord
    {
     //   public string Label { get; set; }
     //   public string Longitude { get; set; }
    //    public string Latitude { get; set; }
        public List<NOAA_DataParam> _lstRequestParams = new List<NOAA_DataParam>();
        public NOAA_DataParam _dataParam;
        
        //todo: results structure should be better thought out;
            
        public SummaryDetail _SummaryVal;           // used for min /max records          
        public List<TimeSeries> _lstVals;           // used for Precipitation records


        public override void GetRealtime_Data(DateTime dtStartRequest, DateTime dtEndRequest){
            string sResponse = DataIO.GetNDFDDataforPoint(_sLatitude,_sLongitude, dtStartRequest, dtEndRequest);
            XmlSerializer _Deserializer = new XmlSerializer(typeof(dwml));
            TextReader txtReader = new StringReader(sResponse);
            var result = (dwml)_Deserializer.Deserialize(txtReader);

            //bojangles
            // create object using nfamilliar syntax and transfer to object
                //step 2: figure out the lambda expression
            var res = result.Data.Location.Select(x => new NDFD_TS_Container
            {
                _sLabel = x.Key           //adapated from amar1 code             
            }).ToList();

            string sLabel = res[0]._sLabel;     //placeholder
            _SummaryVal = _lstRequestParams.Contains(NOAA_DataParam.MaxDailyTemperature) ? GetMaxTemp(result, sLabel) : null;
            _lstVals = _lstRequestParams.Contains(NOAA_DataParam.Precipitation) ? GetPrecipitation(result, sLabel) : null;
            _SummaryVal = _lstRequestParams.Contains(NOAA_DataParam.MaxDailyTemperature) ? GetMaxTemp(result, sLabel) : null;
        }


        //met: this does some good stuff in case you have many points coming back
        //for now: assuming just one value coming bac
            //todo: handle the serialization once time, on the RT object..


        public static List<NDFD_TS_Container> PopulateNDFDData(string sResponse, List<NOAA_DataParam> lstRequestParams)
        {
            // Deserialize data from NDFDgenLatLonList Method       
            XmlSerializer _Deserializer = new XmlSerializer(typeof(dwml));
            TextReader _TextReader = new StringReader(sResponse);
            var result = (dwml)_Deserializer.Deserialize(_TextReader);

            // Populate Data in NdFD_TS_Container object
            NDFD_TS_Container _obj = new NDFD_TS_Container();
            var res = result.Data.Location.Select(x => new NDFD_TS_Container
            {
                _sLabel = x.Key,
                _sLatitude = x.Point.Latitude,
                _sLongitude = x.Point.Longitude,
                _SummaryVal = lstRequestParams.Contains(NOAA_DataParam.MaxDailyTemperature) ? GetMaxTemp(result, x.Key) : null,
                _lstVals = lstRequestParams.Contains(NOAA_DataParam.Precipitation) ? GetPrecipitation(result, x.Key) : null
            }).ToList();

            return res;
        }

              
        // Maximum Temperature Data
        public static SummaryDetail GetMaxTemp(dwml model, string sPointName)
        {
            if (model.Data != null && model.Data.Parameters != null)
            {
                var res = model.Data.Parameters.Where(x => x.PointName == sPointName).FirstOrDefault().TemperatureList.Where(x => x.Type == "maximum").FirstOrDefault();
                if (res == null)
                    return new SummaryDetail();
                var startTime = model.Data.TimeLayout.Where(x => x.LayoutKey == res.TimeLayout).Select(x => x.StartValidTime).FirstOrDefault();
                var endTime = model.Data.TimeLayout.Where(x => x.LayoutKey == res.TimeLayout).Select(x => x.EndValidTime).FirstOrDefault();
                SummaryDetail _ObjTime = new NOAA_Rainfall_Forecasting.SummaryDetail
                {
                    _dtStartTime = startTime.Select(x => DateTimeOffset.Parse(x)).ToList(),
                    _dtEndTime = endTime.Select(x => DateTimeOffset.Parse(x)).ToList(),
                    _dVal = res.Values
                };
                return _ObjTime;
            }
            return new SummaryDetail();
        }


        // Precipitation data
        public static List<TimeSeries> GetPrecipitation(dwml model, string sPointName)
        {
            if (model.Data != null && model.Data.Parameters != null)
            {
                var res = model.Data.Parameters.Where(x => x.PointName == sPointName).FirstOrDefault().PrecipitationList.FirstOrDefault();
                if (res == null)
                    return new List<TimeSeries>();
                var startTime = model.Data.TimeLayout.Where(x => x.LayoutKey == res.TimeLayout).Select(x => x.StartValidTime).FirstOrDefault();
                List<TimeSeries> _ObjTime = new List<TimeSeries>();
                for (int i = 0; i < res.Values.Count; i++)
                {
                    TimeSeries obj = new TimeSeries
                    {
                        _dblValue = res.Values[i],
                        _dtTime = DateTime.Parse(startTime[i])
                    };
                    _ObjTime.Add(obj);
                }
                return _ObjTime;
            }
            return new List<TimeSeries>();
        }
    }
}
