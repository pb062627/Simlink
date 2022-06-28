//XVO!MODFLOW!-;   //XVO!SWMM_OUTREADER!+;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SIM_API_LINKS
{
    // created 5/22/2012 MET
    //time series class is a dedicated class to performing operations on time series data
    //initial implementations will be for collection systems / wet-weather processing, but the class will be implemented more generically such that different time series users (water, energy, resource flows etc)
    //can build upon them

    public enum SimLinkDataType_Major       //met 9/6/2013: added vals that correspond to db stored vals
    {
        MEV = 1,
        ResultSummary = 2,
        ResultTS = 3,
        Event = 4,
        Performance = 5,
        Network = 6,
        XMODEL = 7                          //added 1/2/13: XMODEL is intended to work both with and without DV definition...
    }


    public enum TSRepository
    {
        HDF5 = 1,
        DB = 2,
        NetworkTable =3,
        XML = 4,
        Undefined = -1
    }


    //
    public enum TS_StandardFunctions
    {
        filter_by_hour_of_day = -1,
        filter_by_ref_ts = -2,
    }

    public enum IntervalType                //1: seconds  2: minutes  3: hours 4: days 5: Months 6: Years
    {
        Second,
        Minute,
        Hour,
        Day,
        Month,
        Year
    }

    public enum AverageFunction
    {
        Hour,
        Day,
        Month,
        Year
    }
    public enum MathNetInterpolateMethod
    {
        Linear,
        AkimaSpline,
        LinearWithPoles,
        LinearWithoutPoles
    }

    public enum SimLinkModelTypeID
    {
        EPA_SWMM5 = 1,
        InfoworksCS = 2,
        EPANET = 3,
        MODFLOW = 4,
        MikeUrban = 5,
        XP_SWMM = 6,
        ISIS1D = 7,
        SimClim = 8,
        ISIS2D = 9,
        ICM = 15
    }

    //SP 18-Feb-2016 - EventFunctionID options
    public enum Event_FunctionOnTimeSeries
    {
        ThresholdAndDurationCalculations = -1,
        DefinedDuration,
        AverageValAndPercentile                 //1/10/17 MET: placeholder for test- needs better thought out
    }


    //SP 18-Feb-2016
    public enum Perf_FunctionOnLinkedData
    {
        NotSet = -1,
        Sum = 1,
        Min = 2,
        Max = 3,
        Count = 4,
    }

    public class TimeSeries
    {

        //adapted from SimLinkLib.cs by Leng; MET integrate into time series class.
        #region public properties
        public static double MISSING_VALUE = -9999.0d; // the default missing value
        public static DateTime _DefaultDate = new DateTime(1901, 2, 3);               // use this is we just need a blank date; unlikely any analysis would begin on this date.
        public double _dblValue { get; set; }
        public DateTime _dtTime { get; set; }
        public TimeSeriesDetail _tsDetail;
        #endregion public properties

        #region Constructors
        public TimeSeries(DateTime time, double dblValue)
        {
            _dblValue = dblValue;
            _dtTime = time;
        }
        public TimeSeries()
        {
        }
        #endregion Constructors

        #region TimeSeriesDetail
        // time series detail stores the start date of a TS and its interval- implies the date range for an array or disk-stored vals.
        public class TimeSeriesDetail
        {
            public DateTime _dtStartTimestamp;
            public DateTime _dtEndTimestamp;
            public IntervalType _tsIntervalType;
            public int _nTSInterval;
            public double _nTSIntervalInSeconds; //SP 23-Feb-2016 Added a property to the class to store the time in seconds

            public TimeSeriesDetail(DateTime time, IntervalType Interval, int nInterval, DateTime? dtEndTime = null)
            {
                _dtStartTimestamp = time;
                _tsIntervalType = Interval;
                _nTSInterval = nInterval;

                _nTSIntervalInSeconds = CalculateDurationInSeconds(); //SP 23-Feb-2016 Added a property to the class to store the time in seconds

                //SP 12-Aug-2016 - added an end time for DSS conversion
                if (dtEndTime == null)
                    _dtEndTimestamp = _dtStartTimestamp;
                else
                    _dtEndTimestamp = Convert.ToDateTime(dtEndTime);  
            }
            public TimeSeriesDetail()
            {
            }

            public void InitializeTimeSeriesDetail(DateTime dtStart, int nStartHour, int nStartMin, int nStartSecond, int nTS_Interval, int nIntervalUnit)
            {
                _dtStartTimestamp = dtStart.AddHours(nStartHour).AddMinutes(nStartMin).AddSeconds(nStartSecond);
                _nTSInterval = nTS_Interval;
                _tsIntervalType = GetTSIntervalType(nIntervalUnit);

                _nTSIntervalInSeconds = CalculateDurationInSeconds(); //SP 23-Feb-2016 Added a property to the class to store the time in seconds
            }

            //SP 23-Feb-2016 Calculation to obtain the time in seconds
            private double CalculateDurationInSeconds()
            {
                switch (_tsIntervalType)
                {
                    case IntervalType.Second:
                        return _nTSInterval;
                    case IntervalType.Minute:
                        return TimeSpan.FromMinutes(_nTSInterval).TotalSeconds;
                    case IntervalType.Hour:
                        return TimeSpan.FromHours(_nTSInterval).TotalSeconds;
                    case IntervalType.Day:
                        return TimeSpan.FromDays(_nTSInterval).TotalSeconds;
                    default: 
                        return 99999.999;
                }
            }

            //SP 18-Apr-2017 Assumption that standard simulation starts at midnight
            public int GetTSPeriodNumberFromDateTime(DateTime FromDateTime)
            {
                return Convert.ToInt32(Math.Round((FromDateTime - FromDateTime.Date).TotalSeconds / _nTSIntervalInSeconds, 0));
            }

            protected static IntervalType GetTSIntervalType(int nIntervalType)          //1: seconds  2: minutes  3: hours 4: days 5: Months 6: Years
            {
                IntervalType ntReturn = IntervalType.Second;        //default
                switch (nIntervalType)
                {
                    case 1:
                        ntReturn = IntervalType.Second;
                        break;
                    case 2:
                        ntReturn = IntervalType.Minute;
                        break;
                    case 3:
                        ntReturn = IntervalType.Hour;
                        break;
                    case 4:
                        ntReturn = IntervalType.Day;
                        break;
                    case 5:
                        ntReturn = IntervalType.Month;
                        break;
                    case 6:
                        ntReturn = IntervalType.Year;
                        break;
                }
                return ntReturn;
            }

            public static int GetTSIntervalType(IntervalType intType )          //1: seconds  2: minutes  3: hours 4: days 5: Months 6: Years
            {
                int ntReturn = -1;        //default
                switch (intType)
                {
                    case IntervalType.Second:
                        ntReturn = 1;
                        break;
                    case IntervalType.Minute:
                        ntReturn = 2;
                        break;
                    case IntervalType.Hour:
                        ntReturn = 3;
                        break;
                    case IntervalType.Day:
                        ntReturn = 4;
                        break;
                    case IntervalType.Month:
                        ntReturn = 5;
                        break;
                    case IntervalType.Year:
                        ntReturn = 6;
                        break;
                }
                return ntReturn;
            }



        }


        public class FilterAttributeByTime
        {
            private int _dtMinStartHour;

            public int MinTime
            {
                get { return _dtMinStartHour; }
                set { _dtMinStartHour = value; }
            }
            private int _dtMaxHour;

            public int MaxTime
            {
                get { return _dtMaxHour; }
                set { _dtMaxHour = value; }
            }
            private double _dblDefaultValue;

            public double DefaultValue
            {
                get { return _dblDefaultValue; }
                set { _dblDefaultValue = value; }
            }
        }
        #endregion
        // public static OleDbConnection connRMG;
        //public static SIM_API_Links.CommonUtilities cu = new SIM_API_Links.CommonUtilities();


        public void InitTS()
        {
            //sim2             Logging._nLogging_ActiveLogLevel = 4;
            //  cu.lo
        }

        # region RMG / Data Mgmgt

        #region TS-HDF5 Write     <<Data mgmt Detail>>>
        public static void tsWriteTimeSeries( List<TimeSeries> lstToWrite, ref hdf5_wrap hdf5_write, string sHDFGroupID, string sDataSetID="1")
        {
            double[,] dVals = tsTimeSeriesTo2DArray(lstToWrite);
            hdf5_write.hdfWriteDataSeries(dVals,sHDFGroupID,sDataSetID);
        }
        #endregion

        #region TS-HDF5 IMPORT     <<Data mgmt Detail>>>
        public static List<TimeSeries> tsImportTimeSeries(string sFileName, TimeSeriesDetail tsDtl, int nDataCol = 1)       // zero-indexed Data col
        {
            List<TimeSeries> lstTS = new List<TimeSeries>();
            if (File.Exists(sFileName))
            {
                StreamReader file = null; string sbuf; bool bFirstPass = true; double dVal =  -1;
                try
                {
                    List<double> lstdVals = new List<double>();
                    file = new StreamReader(sFileName);
                    while (!file.EndOfStream)
                    {
                        if (bFirstPass)
                        {
                            sbuf = file.ReadLine();
                            bFirstPass = false;
                        }
                        sbuf = file.ReadLine();
                        string[] s = sbuf.Split(',');
                        if (s.Length >= nDataCol)
                        {
                            if (Common.IsDouble(s[nDataCol]))
                            {
                                dVal = Convert.ToDouble(s[nDataCol]);
                            }
                            else
                            {
                                dVal = MISSING_VALUE;
                            }
                        }
                        else
                        {
                            dVal = MISSING_VALUE;
                        }
                        lstdVals.Add(dVal);
                    }
                    lstTS = Array1DToTSList(tsDtl, lstdVals.ToArray());
                }
                catch
                {
                    //some message.
                }
            }
            return lstTS;
        }
        #endregion
        #region TS-HDF5 Get     <<Data mgmt Detail>>>

        //1: retrieve array from repo
        //2: set time series based on tsd from SimLink
        public static List<TimeSeries> tsGetTimeSeries(ref hdf5_wrap hdfTS, TimeSeriesDetail tsdStartInfo, string sHDFGroupID, string sHDFDatasetID ="1")
        {
            List<TimeSeries> lstReturn = new List<TimeSeries>();
            double[,] dVals = hdfTS.hdfGetDataSeries(sHDFGroupID, sHDFDatasetID);               //todo: get may need to customize the group definition 
    // not needed        double[] dVals1D = ts2DTo1DArray(dVals);              //general case: 1D array comes back from the 
            lstReturn = TimeSeries.Array2DToTSList(tsdStartInfo._dtStartTimestamp, tsdStartInfo._nTSInterval, tsdStartInfo._tsIntervalType, dVals);     //todo: get the ts info from record some how                                                                                     //todo: consider how to keep this open
            return lstReturn;
        }

        //overloaded function returns the 2D dbl array
        public static double[,] tsGetTimeSeries(ref hdf5_wrap hdfTS, string sHDFGroupID, bool bDoNothing, string sHDFDatasetID ="1")
        {
            List<TimeSeries> lstReturn = new List<TimeSeries>();
            double[,] dVals = hdfTS.hdfGetDataSeries(sHDFGroupID, sHDFDatasetID);
            return dVals;
        }

        //return the first dimension slice of the 2D array.
        public static double[] ts2DTo1DArray(double[,] dVals)
        {
                double[] dVals1D = new double[dVals.GetLength(0)]; //SP 23-Feb-2016 changed from dVals.GetUpperBound(0) - was 1 short due to 0 indexing
            for (int i = 0; i < dVals1D.Length; i++)
            {
                dVals1D[i] = dVals[i, 0];
            }

            //perform quick validation- we expect only one dimension
            if (dVals.GetUpperBound(1)>1)       
            {
                //todo: LOG "WARNING: TS results retrieved have more dimensions than anticipated"       //todo: improve to reference scenario/Group
            }
            return dVals1D;
        }

        //return the first dimension slice of the 2D array.
        public static double[,] tsTimeSeriesTo2DArray(List<TimeSeries> lst)
        {
            double[,] dVals = new double[lst.Count,1]; int i = 0;
            foreach (TimeSeries ts in lst)
            {
                dVals[i, 0] = ts._dblValue;
                i++;
            }

            return dVals;
        }


        #endregion




        //code readability function: reset 
        public static void tsResetTSLocals(ref int nCurrentEvent_BeginPeriod, ref int nCurrentEvent_PeriodCounter, 
            ref int nCurrentEvent_BelowThresholdCounter, ref int nCurrentEvent_SubEventThreshold_Counter, 
            ref double dMaxVal, ref double dRunningTotalCurrentEvent,
            bool IsAboveThreshold, bool bCalcValueInExcessOfThreshold)
        {
            nCurrentEvent_BeginPeriod = 0;
            nCurrentEvent_PeriodCounter = 0;
            nCurrentEvent_BelowThresholdCounter = 0;
            nCurrentEvent_SubEventThreshold_Counter = 0;
            
            //set the extreme values based on whether triggering above or below - SP 23-Feb-2016
            if (IsAboveThreshold || bCalcValueInExcessOfThreshold)
                dMaxVal = -9999.99;
            else
                dMaxVal = 9999.99;
            
            dRunningTotalCurrentEvent = 0;
            nCurrentEvent_BeginPeriod = 0;
        }


        //sim2: this tsDetail must occur outside the class
        //these functions must either take a DS and update our output a DS.... no DAL access
        //public void tsInsertTS_EventDetail()
        //public void tsInsertTS_EventSummary(int nScenarioID,
        //public void tsUpdateRank(


        // TODO :
        //more fully consider where the time series container info (start date, duration etc is best held (currently on the evaluation group)
        //may want to make more flexible for scenario and/or result profile itself variability
        //potential resolution (in the future): Timeseries datatable, which can link EITHER to a RESULT_TS_ID, Scenario, or Evaluation Group.

        //step through the statistics and process the results  
        //based on 1)IW_Write_LevelOrInflow_ByEval    and/or cuResultTS_Process

        //sql data adapters should be wrapped up into simple functinos that can be called (perhaps initializing a more robust time series object) eg cu.Eval_getSimDetails(nEvalID, connRMG);

        //met 3/19/2013 update statistics
        //met 5/14/2013 : split data select from process (step 1)


        # endregion


        #region Time-series functions

        /// <summary>
        /// return agg value for a ts  (2d array that is actually a 1d array)
        /// </summary>
        /// <param name="dVals"></param>
        /// <param name="nFunctionCode"></param>
        /// <returns></returns>
        public static double SummaryStatForTS(double[,] dVals, int nFunctionCode) /*, int nStartAtPeriodNo*/ //SP 1-Dec-2016 TS will only return from BeginPeriodNo)
        {
            double[] dVals1D = ts2DTo1DArray(dVals);                //unnecessary if you have the proper 1D to start; todo
            double dReturn = -666.6789;
            int nStartAtPeriodNo = 1; //SP 1-Dec-2016 TS will only return values from nStartAtPeriodNo therefore include all values in TS for aggregation
            switch ((Perf_FunctionOnLinkedData)nFunctionCode)                                                                  //for some reason, must treat val as decimal.... don't know why.
            {
                case Perf_FunctionOnLinkedData.Sum:                
                    dReturn = dVals1D.Where((x, idx) => idx >= nStartAtPeriodNo - 1).Sum(); //SP 7-Oct-2016 added consideration for start period number
                    break;
                case Perf_FunctionOnLinkedData.Min:
                    dReturn = dVals1D.Where((x, idx) => idx >= nStartAtPeriodNo - 1).Min();            //min
                    break;
                case Perf_FunctionOnLinkedData.Max:
                    dReturn = dVals1D.Where((x, idx) => idx >= nStartAtPeriodNo - 1).Max();
                    break;
                case Perf_FunctionOnLinkedData.Count:
                    dReturn = dVals1D.Where((x, idx) => idx >= nStartAtPeriodNo - 1).Count();           //count
                    break;
            }
            return dReturn;
        }

        //SP23-Feb-2016 - Timeseries aggregation with Threshold - this is probably why events were created with aggregates
        /*public static double SummaryStatForTS(double[,] dVals, int nFunctionCode, bool bApplyThreshold, double dThreshold, bool bExceedsThreshold)
        {
            double[] dVals1D = ts2DTo1DArray(dVals);                //unnecessary if you have the proper 1D to start; todo
            double dReturn = -666.6789;
            switch ((Perf_FunctionOnLinkedData)nFunctionCode)                                                                  //for some reason, must treat val as decimal.... don't know why.
            {
                case Perf_FunctionOnLinkedData.Sum:
                    if (bApplyThreshold)
                    {
                        if (bExceedsThreshold)
                            dReturn = dVals1D.Where(x => x > dThreshold).Sum();
                        else
                            dReturn = dVals1D.Where(x => x < dThreshold).Sum();         //Sum
                    }
                    else
                        dReturn = dVals1D.Sum();
                    break;
                case Perf_FunctionOnLinkedData.Min:
                    if (bApplyThreshold)
                    {
                        if (bExceedsThreshold)
                            dReturn = dVals1D.Where(x => x > dThreshold).Min();
                        else
                            dReturn = dVals1D.Where(x => x < dThreshold).Min();           //min
                    }
                    else
                        dReturn = dVals1D.Min();
                    break;
                case Perf_FunctionOnLinkedData.Max:
                    if (bApplyThreshold)
                    {
                        if (bExceedsThreshold)
                            dReturn = dVals1D.Where(x => x > dThreshold).Max();
                        else
                            dReturn = dVals1D.Where(x => x < dThreshold).Max();           //Max
                    }
                    else
                        dReturn = dVals1D.Max();
                    break;
                case Perf_FunctionOnLinkedData.Count:
                    if (bApplyThreshold)
                    {
                        if (bExceedsThreshold)
                            dReturn = dVals1D.Where(x => x > dThreshold).Count();
                        else
                            dReturn = dVals1D.Where(x => x < dThreshold).Count();         //count
                    }
                    else
                        dReturn = dVals1D.Count();
                    break;
            }
            return dReturn;
        }*/

        //sim2: this must provide a DataSet that can be returned so that we 
        //todo: segregate out the TS components
            //updated 4/4/14
            // 1- remove all references to data stores
            //rmv scenario loop
            //todo: some scaling in the loop is essentially hard coded, needs to be passed as param
            //still an ugly function, but now can be called in more instancs
            //for now, process 1 dr at time; would be better to perform ALL event def on given dVals ts...
        

        //SP 23-Feb-2016 Subroutine to record events
        private static void LogEventAndResetValues(int nTimeStep, int nBeginPeriodNumber,
            ref int nCurrentEvent_BeginPeriod, ref int nCurrentEvent_PeriodCounter, ref int nCurrentEvent_ZeroValues, 
            ref int nCurrentEvent_BelowThresholdCounter, ref int nCurrentEvent_SubEventThreshold_Counter, ref double dExtremeVal, 
            ref double dRunningTotalCommonUtilitiesCurrentEvent, ref DataSet dsEvent_Return, ref DataSet dsCurrentEventDetails, int nScenarioID, 
            int nEventID, TimeSeriesDetail tsdEvent, bool IsInstantaneous, bool IsAboveThreshold,
            bool bCalcValueInExcessOfThreshold, bool bIncludeBeginPeriodNumber, int nEventCounter)
        {
               
            //if instantaneous and cumulative  
            nCurrentEvent_BeginPeriod = (nTimeStep + 1) - nCurrentEvent_PeriodCounter;
            if (bIncludeBeginPeriodNumber)
                nCurrentEvent_BeginPeriod+=nBeginPeriodNumber;          // standard case for PRIMARY event definition
                                                                        // secondary event definition - DO NOT add the begin period...
            //met 5/15/2013 - scale the running total to get the correct units
            //bojangles- need to pass parameter for event scaling

            //met todo: not sure that "(nCurrentEvent_PeriodCounter - nCurrentEvent_ZeroValues)" is correct?
            //only report the sum of the data values when the accumulation is above the threshold
            tsInsertTS_EventDetail(ref dsEvent_Return, ref dsCurrentEventDetails, nScenarioID, nEventID, nCurrentEvent_BeginPeriod, 
                (nCurrentEvent_PeriodCounter - nCurrentEvent_ZeroValues), nCurrentEvent_BelowThresholdCounter, nCurrentEvent_SubEventThreshold_Counter,
                dExtremeVal, dRunningTotalCommonUtilitiesCurrentEvent, true, nEventCounter);       // met added explicit bool so could add event counter arg (2/2/17)
            
            //reset the variables for the next event within the time series
            tsResetTSLocals(ref nCurrentEvent_BeginPeriod, ref nCurrentEvent_PeriodCounter, ref nCurrentEvent_BelowThresholdCounter,
                ref nCurrentEvent_SubEventThreshold_Counter, ref dExtremeVal, ref dRunningTotalCommonUtilitiesCurrentEvent,
                IsAboveThreshold, bCalcValueInExcessOfThreshold); //BYM tsResetLocals=tsResetTSLocals

        }


        //SP 23-Feb-2016 Given the thresholds and IsOver / IsUnder, detect whether the current period inst value or cumulated value should trigger an event
        private static bool IsEventTriggered(double dInstValue, double dCumulativeValue, double dThreshold, bool bInstTriggerIfOver,
            bool bApplyInstThreshold, bool bApplyCumulativeThreshold)
        {
            //check if Instantaneous is active and outside threshold
            if (bApplyInstThreshold)
                if ((dInstValue >= dThreshold && bInstTriggerIfOver) || (dInstValue <= dThreshold && !bInstTriggerIfOver))
                    return true;

            //check if Cumulative is active and outside threshold
            if (bApplyCumulativeThreshold)
                if ((dCumulativeValue >= dThreshold && bInstTriggerIfOver) || (dCumulativeValue <= dThreshold && !bInstTriggerIfOver))
                    return true;

            return false;
        }

        /// <summary>
        /// Added to help determine the start of a rainfall event associated with a reference event.
        ///     //STAGE 1: Supports front/back search, but ONLY 
        /// TODO: Consider this as a helper to smooth the logic in 
        ///     IE: If you had the start/stop index of a TS, you could get the totals with simple aggregation?
        ///     
        ///     MET 11/17/16: update to be a little bit more general.
        ///     
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int ScanForEventTerminus(ref double[,] dVals, int nOriginIndex, double dThreshold, int nSubEventPeriodsTarget, bool bSearchForward=true)
        {
            bool bContinue = true; double dVal = 0;
            int nIndex = nOriginIndex;
            int nIncrementer = 1;
            int nSubEventCounter = 0;
            int nTS_Length = dVals.GetLength(0);
            if (!bSearchForward)
                nIncrementer = -1;
            while (bContinue)
            {
                dVal = dVals[nIndex,0];
                if (dVal <= dThreshold)    // todo: apply complete threshold test
                    nSubEventCounter++;
                else
                    nSubEventCounter = 0;   // send it back

                if (nSubEventCounter == nSubEventPeriodsTarget)    // done
                {
                    bContinue = false;
                    return nIndex;   // met: make sure you shouldn't increment/decrement before sending back?
                }
                else
                {
                    nIndex += nIncrementer;
                    if (nIndex < 0 || nIndex >= nTS_Length)         // at either end of the TS
                    {
                        bContinue = false;
                        nIndex -= nIncrementer;                                 // set the index back?
                    }                      
                }
            }
            return nIndex;
        }

        
        /// <summary>
        /// Calc event summary when start/end (origin/terminus) are already known.
        /// 
        /// Overloaded operator forom the original event definition
        /// 1/10/17: Added the bGetAverage switch to perform an optional data transofmration. Tihs really needs to be thought through better.
            //2/2/17: add bdonothing- does nothing, distinguishes overload, and ability to pass an int for tracking events...
        
        /// </summary>
        /// <param name="nScenarioID"></param>
        /// <param name="dVals"></param>
        /// <param name="dr_TS"></param>
        /// <param name="tsdEvent"></param>
        /// <param name="dsSCEN_EventDetails"></param>
        /// <param name="dsCurrentEventDetails"></param>
        /// <param name="nBeginPeriodOverride"></param>
        /// <param name="bProcessOneEventOnly"></param>
        public static void EventDefine(int nScenarioID, double[,] dVals, int nCurrentEvent_BeginPeriod, int nDurationPeriods, int nResultTS_ID, int nEventID, ref DataSet dsSCEN_EventDetails,
            ref DataSet dsCurrentEventDetails, bool bGetAverage = false, bool bDoNOTHING=false, int nEventCounterGiven=-1)
        {
            //int nResultTS_ID = nResultEvent_FK;     // Convert.ToInt32(dr_TS["ResultTS_or_Event_ID_FK"].ToString());
           // pass as arg int nEventID = Convert.ToInt32(dr_TS["EventSummaryID"].ToString());
            double dExtremeVal = -666;
            double dTotalVal = 0;
            // throw a for loop in there for now but would like to do w linq
            for (int i = 0; i < nDurationPeriods; i++)
            {
                dTotalVal += dVals[i + nCurrentEvent_BeginPeriod, 0];
                if (dVals[i + nCurrentEvent_BeginPeriod, 0] > dExtremeVal)
                    dExtremeVal = dVals[i + nCurrentEvent_BeginPeriod, 0];
            }

            if (bGetAverage)
            {
                dTotalVal = dTotalVal / nDurationPeriods;       // convert to an average
                double[] dVals_Single = ts2DTo1DArray(dVals);
                dExtremeVal = mathdotnet.QuantileRank(dVals_Single, dTotalVal);         // get the quantile
            }


            tsInsertTS_EventDetail(ref dsSCEN_EventDetails, ref dsCurrentEventDetails, nScenarioID, nEventID, nCurrentEvent_BeginPeriod,
                    nDurationPeriods, nDurationPeriods, -1, dExtremeVal, dTotalVal, true, nEventCounterGiven);
        }


        //SP 23-Feb-2016 Added functionality to account for both above and below threshold
        // met 11/25/16: add ability to start at a different period than 0/beginperiod
            //rationale: secondary event processing....
        public static void EventDefine(int nScenarioID, double[,] dVals, DataRow dr_TS, TimeSeriesDetail tsdEvent, ref DataSet dsSCEN_EventDetails,
            ref DataSet dsCurrentEventDetails, int nBeginPeriodOverride = -1, bool bProcessOneEventOnly = false, bool bCountEvents=false, int nEventCounterGiven =-1)
        {
            int nTS_Timesteps=-1;
            //int nTScounter = 0; // BYM //SP 12-Dec-2016 - not used anywhere I don't believe.
            int nTS_CountRecords = dVals.GetLength(0);
            double dTS;
            double dInterEventDuration = Convert.ToDouble(dr_TS["InterEvent_Threshold"].ToString());
            
            //threshold parameters
            double dThreshold = -9999.99;
            //double dThreshold_Cumulative = -9999.99;
            bool ApplyThreshold = false;
            bool ApplyThreshold_Cumulative = false;
            bool bTriggerIfOverThreshold = true;

            //can only have one or the other - either inst or cumulative
            if (Common.IsDouble(dr_TS["Threshold_Inst"].ToString()))
            {
                dThreshold = Convert.ToDouble(dr_TS["Threshold_Inst"].ToString());
                bTriggerIfOverThreshold = Convert.ToBoolean(dr_TS["IsOver_Threshold_Inst"].ToString());
                ApplyThreshold = true;
            }
            else //test whether cumulative thresholds apply
            {
                if (Common.IsDouble(dr_TS["Threshold_Cumulative"].ToString()))
                {
                    dThreshold = Convert.ToDouble(dr_TS["Threshold_Cumulative"].ToString());
                    bTriggerIfOverThreshold = Convert.ToBoolean(dr_TS["IsOver_Threshold_Cumulative"].ToString());
                    ApplyThreshold_Cumulative = true;
                }
            }
            bool bCalcValueInExcessOfThreshold = Convert.ToBoolean(dr_TS["CalcValueInExcessOfThreshold"].ToString());
            bool bIsFullEventCycle = true;     //
            //Get IDs and StartingPeriod Info
            int nBeginPeriodNumber = 1;             // comes from tblResultTS- need to mod base tblResultTS if want to support this. Not sure relevant..? Convert.ToInt32(dr_TS["BeginPeriodNo"].ToString());
            if (nBeginPeriodOverride > 0)
            {
                nBeginPeriodNumber = nBeginPeriodOverride;
                bIsFullEventCycle = false;          // affects whether the TS is recorded from the "beginPeriodNo. Generally, for secondary events, this should be false
                
                //SP 12-Dec-2016 overwritten below so I don't believe this is needed here nTS_Timesteps = dVals.GetLength(0);
                //nTS_Timesteps = nTS_Timesteps - nBeginPeriodOverride;   // adjust timestep number so  we don't go beyond bounds. may be off by 1? q  
            }

            // int nScenarioID = Convert.ToInt32(dr_scen["ScenarioID"].ToString());
            int nResultTS_ID = Convert.ToInt32(dr_TS["ResultTS_or_Event_ID_FK"].ToString());
            int nEventID = Convert.ToInt32(dr_TS["EventSummaryID"].ToString());
            bool bInsideEvent = false;              //whether we are inside an event

            //initialise values
            bool bWithinThreshold = true;           //whether below threshold
            int nCurrentEvent_ZeroValues = 0;         //counter for reading with zero values within an event, that is important for the stopping criteria
            int nCurrentEvent_BeginPeriod = 0;      //begining of an event, the TimesSeries starts from 0                                                                                         
            int nCurrentEvent_PeriodCounter = 0;    //counts the # of periods where are within the threshold                           
            int nCurrentEvent_BelowThresholdCounter = 0;            //number of times within 'event' that we have a no-storm "values less than the threshold"                                                                      
            double dExtremeVal = 9999.99;                                   //max val for current event                                                     
            double dRunningTotalCurrentEvent = 0.0;                //sum total flow within the event
            double dRunningTotal = 0.0;                                    
            int nCurrentEvent_SubEventThreshold_Counter = 0;        //number of times within 'event' that we have a storm "values more than the threshold"  
            int nEventCounter = nEventCounterGiven;                              // added for tracking events; only used for primary
            if (bCountEvents)               // add nEventCounterGiven: this only wors if bCountEvents = false for now.. used for secondary event processing
                nEventCounter = 1;
            //SP 11-Feb-2016 TODO interevent duration is always in seconds, so needs converting to the correct interval
            //Convert interval into seconds
            int nStoppingCriteria = Convert.ToInt32(Math.Ceiling(dInterEventDuration / tsdEvent._nTSIntervalInSeconds)); //number of times within 'event' that a minimum is achieved. for now, we assume this minimum is zero
            
            //todo perhaps log if the number of steps is not a whole factor of the duration;
            nTS_Timesteps = dVals.GetLength(0);                                                             // dsTS.Tables[0].Rows.Count;  //met            / 10;

            if (ApplyThreshold || ApplyThreshold_Cumulative)
            {
                //set extreme value very large if triggering event for under threshold
                if (bTriggerIfOverThreshold || bCalcValueInExcessOfThreshold)
                    dExtremeVal = -9999.99;

                // met 11/25/16: shouldn't j< nTS_Timesteps be altered in case where nBeginPerioNumber >1?  not changed for now.
                for (int j = nBeginPeriodNumber - 1; j < nTS_Timesteps; j++) //SP 9-Mar-2016 Changed to start at BeginPeriodNumber
                {
                    dTS = Convert.ToDouble(dVals[j, 0]);                                            //dsTS.Tables[0].Rows[j]["valTS"].ToString());     //current val
                    
                    //running total is used for cumulative triggering - not reset - TODO Cumulative triggering needs to be tested SP 23-Feb-2016
                    dRunningTotal = dRunningTotal + dTS;

                    if (IsEventTriggered(dTS, dRunningTotal, dThreshold, bTriggerIfOverThreshold,
                        ApplyThreshold, ApplyThreshold_Cumulative))   //outside the threshold
                    //((dTS >= dThreshold && bTriggerIfOverThreshold) || (dTS <= dThreshold && !bTriggerIfOverThreshold))       
                    {
                        double CumulativeExtreme = dRunningTotal; 
                        //Adjust the datapoint to be in excess of threshold
                        if (bCalcValueInExcessOfThreshold)
                        {
                            dTS = Math.Abs(dTS - dThreshold);
                            CumulativeExtreme = Math.Abs(CumulativeExtreme - dThreshold);
                        }

                        //increment / add to tracking variables
                        nCurrentEvent_PeriodCounter++;

                        //calcuate the cumulated value in the current event
                        dRunningTotalCurrentEvent += dTS;
                        
                        nCurrentEvent_ZeroValues = 0;           //reset the counter
                        bInsideEvent = true;
                        bWithinThreshold = false;

                        //within an instantaneous event - find the maximum / minimum value
                        if (bTriggerIfOverThreshold || bCalcValueInExcessOfThreshold)
                        {
                            if (ApplyThreshold)
                            {
                                if (dTS > dExtremeVal)                      //calcuate the max value in the current event
                                    dExtremeVal = dTS;
                            }
                            else //Cumulative Threshold is being applied
                            {
                                if (CumulativeExtreme > dExtremeVal)                      //calcuate the max value in the current event
                                    dExtremeVal = CumulativeExtreme;
                            }
                        }
                        else
                        {
                            if (ApplyThreshold)
                            {
                                if (dTS < dExtremeVal)                      //calcuate the min value in the current event
                                    dExtremeVal = dTS;
                            }
                            else //Cumulative Threshold is being applied
                            {
                                if (CumulativeExtreme < dExtremeVal)                      //calcuate the max value in the current event
                                    dExtremeVal = CumulativeExtreme;
                            }
                        }

                        if (j == (nTS_Timesteps - 1))       //in case the last value in the time series is larger than the threshold
                        {
                            nCurrentEvent_SubEventThreshold_Counter++;

                            LogEventAndResetValues(j, nBeginPeriodNumber, ref nCurrentEvent_BeginPeriod, ref nCurrentEvent_PeriodCounter,
                                ref nCurrentEvent_ZeroValues, ref nCurrentEvent_BelowThresholdCounter, ref nCurrentEvent_SubEventThreshold_Counter,
                                ref dExtremeVal, ref dRunningTotalCurrentEvent, ref dsSCEN_EventDetails, ref dsCurrentEventDetails, nScenarioID, nEventID, tsdEvent,
                                ApplyThreshold, bTriggerIfOverThreshold, bCalcValueInExcessOfThreshold, bIsFullEventCycle, nEventCounter);

                            if (bCountEvents)
                                nEventCounter++;
                        
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                    else         //within the threshold
                    {
                        if (bInsideEvent)       //within the threshold and within an event
                        {
                            nCurrentEvent_PeriodCounter++;

                            //calcuate the cumulated value in the current event
                            if (bCalcValueInExcessOfThreshold)
                                dRunningTotalCurrentEvent += Math.Abs(dTS - dThreshold);
                            else
                                dRunningTotalCurrentEvent += dTS;

                            if (!bWithinThreshold)
                            {
                                nCurrentEvent_SubEventThreshold_Counter++;
                                nCurrentEvent_BelowThresholdCounter++;              // don't see the need for both of these?
                            }
                            else
                            {
                                //do nothing
                            }

                            nCurrentEvent_ZeroValues++;     //met 5/26/2012: use same threshold for starting and ending an event.
                            //SP 23-Feb-2016 This is now considered in IsEventTriggered  (MET code removed below)

                            if ((nCurrentEvent_ZeroValues == nStoppingCriteria) || (j == nTS_Timesteps - 1))
                            {
                                LogEventAndResetValues(j, nBeginPeriodNumber, ref nCurrentEvent_BeginPeriod, ref nCurrentEvent_PeriodCounter,
                                    ref nCurrentEvent_ZeroValues, ref nCurrentEvent_BelowThresholdCounter, ref nCurrentEvent_SubEventThreshold_Counter,
                                    ref dExtremeVal, ref dRunningTotalCurrentEvent, ref dsSCEN_EventDetails, ref dsCurrentEventDetails, nScenarioID, nEventID, tsdEvent, ApplyThreshold,
                                    bTriggerIfOverThreshold, bCalcValueInExcessOfThreshold, bIsFullEventCycle, nEventCounter);

                                bInsideEvent = false;
                                if (bCountEvents)
                                    nEventCounter++;
                                if (bProcessOneEventOnly)
                                    return;
                            }
                            else
                            {
                                //do nothing            
                            }
                            bWithinThreshold = true;
                        }

                        else    //below the threshold and not within an event                                                                      
                        {
                            //not in event (e.g. zero flow) so keep moving do nothing
                        }

                    }

                }
            } //Endif ApplyThreshold or ApplyThreshold_Cumulative
            
            //nTScounter++;       //increment for new scenarios //SP 12-Dec-2016 - not used anywhere I don't believe.
        }

        public static void ScaleArrayByMaxVal(double[,] dVals, bool bMaxNegative=false){
            double dMax = 0;
            for (int i = 0; i < dVals.GetLength(0); i++)
            {
                if (dVals[i, 0] > dMax)
                    dMax = dVals[i, 0];
            }
            if (dMax != 0)          // cannot scale by zero; no action needed
            {
                for (int i = 0; i < dVals.GetLength(0); i++)
                {
                    dVals[i, 0] = dVals[i, 0] / dMax;
                }
            }
        }

        //met 5/13/2013 updated based upon use of tblResultTS_EventSummary
        //met 4/4/14: updeate to receive dataset and insert new vals
        // 2/4/17: add the event number
        public static void tsInsertTS_EventDetail(ref DataSet dsTS, ref DataSet dsCurrentTS, int nScenarioID, int nEventSummaryID, int nCurrentEvent_BeginPeriod, int nCurrentEvent_PeriodCounter, int nCurrentEvent_BelowThresholdCounter, int nCurrentEvent_SubEventThreshold_Counter, double dMaxVal, double dRunningTotal_CurrentEvent, bool bSetSecondaryDS = true, int nEventCounter = -1)
        {
            dsTS.Tables[0].Rows.Add(dsTS.Tables[0].NewRow());
            int nInsertRow = dsTS.Tables[0].Rows.Count - 1;
            dsTS.Tables[0].Rows[nInsertRow]["ScenarioID_FK"] = nScenarioID;
            dsTS.Tables[0].Rows[nInsertRow]["EventSummary_ID"] = nEventSummaryID;
            dsTS.Tables[0].Rows[nInsertRow]["EventDuration"] = nCurrentEvent_PeriodCounter;             //index periods from 0
            dsTS.Tables[0].Rows[nInsertRow]["EventBeginPeriod"] = nCurrentEvent_BeginPeriod;
            dsTS.Tables[0].Rows[nInsertRow]["MaxVal"] = dMaxVal;
            dsTS.Tables[0].Rows[nInsertRow]["TotalVal"] = dRunningTotal_CurrentEvent;
            dsTS.Tables[0].Rows[nInsertRow]["SubEventThresholdPeriods"] = nCurrentEvent_SubEventThreshold_Counter;
            dsTS.Tables[0].Rows[nInsertRow]["EventNo"] = nEventCounter;
            //duplicate, keep a record of the current details so they can be saved to the list
            if (bSetSecondaryDS)
            {
                dsCurrentTS.Tables[0].Rows.Add(dsCurrentTS.Tables[0].NewRow());
                int nInsertCurrentTSRow = dsCurrentTS.Tables[0].Rows.Count - 1;
                dsCurrentTS.Tables[0].Rows[nInsertCurrentTSRow]["ScenarioID_FK"] = nScenarioID;
                dsCurrentTS.Tables[0].Rows[nInsertCurrentTSRow]["EventSummary_ID"] = nEventSummaryID;
                dsCurrentTS.Tables[0].Rows[nInsertCurrentTSRow]["EventDuration"] = nCurrentEvent_PeriodCounter;             //index periods from 0
                dsCurrentTS.Tables[0].Rows[nInsertCurrentTSRow]["EventBeginPeriod"] = nCurrentEvent_BeginPeriod;
                dsCurrentTS.Tables[0].Rows[nInsertCurrentTSRow]["MaxVal"] = dMaxVal;
                dsCurrentTS.Tables[0].Rows[nInsertCurrentTSRow]["TotalVal"] = dRunningTotal_CurrentEvent;
                dsCurrentTS.Tables[0].Rows[nInsertCurrentTSRow]["SubEventThresholdPeriods"] = nCurrentEvent_SubEventThreshold_Counter;
                dsCurrentTS.Tables[0].Rows[nInsertCurrentTSRow]["EventNo"] = nEventCounter;
            }
        }

        /// <summary>
        /// Overloaded function for non period based event summary-a
        /// 
        /// </summary>
        /// <param name="dsTS"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="nEventSummaryID"></param>
        /// <param name="nCurrentEvent_BeginPeriod"></param>
        /// <param name="nCurrentEvent_PeriodCounter"></param>
        /// <param name="nCurrentEvent_BelowThresholdCounter"></param>
        /// <param name="nCurrentEvent_SubEventThreshold_Counter"></param>
        /// <param name="dMaxVal"></param>
        /// <param name="dRunningTotal_CurrentEvent"></param>
        public static void tsInsertTS_EventDetail(ref DataSet dsTS, int nScenarioID, int nEventSummaryID, double dVal, int nEventCounter=-1)
        {
            dsTS.Tables[0].Rows.Add(dsTS.Tables[0].NewRow());
            int nInsertRow = dsTS.Tables[0].Rows.Count - 1;
            dsTS.Tables[0].Rows[nInsertRow]["ScenarioID_FK"] = nScenarioID;
            dsTS.Tables[0].Rows[nInsertRow]["EventSummary_ID"] = nEventSummaryID;
            dsTS.Tables[0].Rows[nInsertRow]["EventDuration"] = -1;             //index periods from 0
            dsTS.Tables[0].Rows[nInsertRow]["EventBeginPeriod"] = -1;
            dsTS.Tables[0].Rows[nInsertRow]["MaxVal"] = -1;
            dsTS.Tables[0].Rows[nInsertRow]["TotalVal"] = dVal;
            dsTS.Tables[0].Rows[nInsertRow]["SubEventThresholdPeriods"] = -1;
            dsTS.Tables[0].Rows[nInsertRow]["EventNo"] = -1;
        }


        public static IntervalType GetTSIntervalType(string sIntervalType)
        {
            IntervalType intervalReturn = IntervalType.Second;
            if ((sIntervalType.Substring(sIntervalType.Length - 1, 1).ToLower() == "s"))
            {
                sIntervalType = sIntervalType.Substring(0, sIntervalType.Length - 1).ToLower();
            }
            switch (sIntervalType){
                case "second":
                    intervalReturn= IntervalType.Second;
                    break;
                case "minute":
                    intervalReturn= IntervalType.Minute;
                    break;
                case "hour":
                    intervalReturn= IntervalType.Hour;
                    break;
                case "day":
                    intervalReturn= IntervalType.Day;
                    break;
                case "month":
                    intervalReturn = IntervalType.Month;
                    break;
                case "year":
                    intervalReturn= IntervalType.Year;
                    break;
            }
            return intervalReturn;
        }
        public static IntervalType GetTSIntervalType(int nIntervalDB_Val)
        {
            IntervalType intervalReturn = IntervalType.Second;

            switch (nIntervalDB_Val)
            {
                case 1:
                    intervalReturn = IntervalType.Second;
                    break;
                case 2:
                    intervalReturn = IntervalType.Minute;
                    break;
                case 3:
                    intervalReturn = IntervalType.Hour;
                    break;
                case 4:
                    intervalReturn = IntervalType.Day;
                    break;
                case 5:
                    intervalReturn = IntervalType.Day;
                    break;
                case 6:
                    intervalReturn = IntervalType.Year;
                    break;
            }
            return intervalReturn;
        }


        public static int GetSecInterval(IntervalType Interval, int nIntervalVal)
        {
            int secInterval = nIntervalVal;
            if (Interval == IntervalType.Minute)
                secInterval = nIntervalVal * 60;
            else if (Interval == IntervalType.Hour)
                secInterval = nIntervalVal * 3600;
            else if (Interval == IntervalType.Day)
                secInterval = nIntervalVal * 3600 * 24;

            return secInterval;
        }

        public static List<TimeSeries> FilterTimeSeriesByPeakValue(List<TimeSeries> inputTS, List<TimeSeries> refTS, double dblPeak, bool bSelectExceedsPeak, double dblDefaultValue = 0.0)
        {
            if (bSelectExceedsPeak)
            {
            
                IEnumerable<TimeSeries> tsOutputMatch = from x in inputTS
                                                        from y in refTS
                                                        where y._dblValue >= dblPeak
                                                        select new TimeSeries
                                                        {
                                                            _dtTime = x._dtTime,
                                                            _dblValue = x._dblValue
                                                        };
                IEnumerable<TimeSeries> tsOutputUnmatch = from x in inputTS
                                                          from y in refTS
                                                          where y._dblValue < dblPeak
                                                          select new TimeSeries
                                                          {
                                                              _dtTime = x._dtTime,
                                                              _dblValue = dblDefaultValue
                                                          };
                IEnumerable<TimeSeries> finallist = tsOutputMatch.Union(tsOutputUnmatch);
                return finallist.ToList();
            }
            else
            {       // if val lower than ref val
                IEnumerable<TimeSeries> tsOutputMatch = from x in inputTS
                                                        from y in refTS
                                                        where y._dblValue <= dblPeak
                                                        select new TimeSeries
                                                        {
                                                            _dtTime = x._dtTime,
                                                            _dblValue = x._dblValue
                                                        };
                IEnumerable<TimeSeries> tsOutputUnmatch = from x in inputTS
                                                          from y in refTS
                                                          where y._dblValue > dblPeak
                                                          select new TimeSeries
                                                          {
                                                              _dtTime = x._dtTime,
                                                              _dblValue = dblDefaultValue
                                                          };
                IEnumerable<TimeSeries> finallist = tsOutputMatch.Union(tsOutputUnmatch);
                return finallist.ToList();
            }
        }
        public static List<TimeSeries> FilterTimeSeriesByTime(List<TimeSeries> inputTS, int nMinAcceptHour = 0, int nMaxAcceptHour = 24, double dDefaultVal = 0)
        {
            IEnumerable<TimeSeries> tsOutputMatch = from x in inputTS
                                                    where x._dtTime.Hour >= nMinAcceptHour
                                                    && x._dtTime.Hour < nMaxAcceptHour
                                                    select new TimeSeries
                                                    {
                                                        _dtTime = x._dtTime,
                                                        _dblValue = x._dblValue
                                                    };
            IEnumerable<TimeSeries> tsOutputUnMatch = from x in inputTS
                                                      where x._dtTime.Hour < nMinAcceptHour
                                                      || x._dtTime.Hour >= nMaxAcceptHour
                                                      select new TimeSeries
                                                      {
                                                          _dtTime = x._dtTime,
                                                          _dblValue = dDefaultVal
                                                      };
            IEnumerable<TimeSeries> finallist = tsOutputMatch.Union(tsOutputUnMatch).OrderBy(x => x._dtTime);
            return finallist.ToList();
        }


        //met - fron Leng- not sure the filterbytime is really needed.
        public static List<TimeSeries> FilterTimeSeriesByTime(List<TimeSeries> inputTS, FilterAttributeByTime filter)
        {
            IEnumerable<TimeSeries> tsOutputMatch = from x in inputTS
                                                    where x._dtTime.Hour >= filter.MinTime
                                                    && x._dtTime.Hour <= filter.MaxTime
                                                    select new TimeSeries
                                                    {
                                                        _dtTime = x._dtTime,
                                                        _dblValue = x._dblValue
                                                    };
            IEnumerable<TimeSeries> tsOutputUnMatch = from x in inputTS
                                                      where x._dtTime.Hour < filter.MinTime
                                                      || x._dtTime.Hour > filter.MaxTime
                                                      select new TimeSeries
                                                      {
                                                          _dtTime = x._dtTime,
                                                          _dblValue = filter.DefaultValue
                                                      };
            IEnumerable<TimeSeries> finallist = tsOutputMatch.Union(tsOutputUnMatch);
            return finallist.ToList();
        }

        /// <summary>
        /// Transforme a 2D array to a timeseris list.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="intervalValue"></param>
        /// <param name="type"></param>
        /// <param name="adblValue"></param>
        /// <param name="nDataIndex"></param>  which index to use; in general, we will use index zero.
        /// <returns></returns>
        public static List<TimeSeries> Array1DToTSList(TimeSeriesDetail tsDtl,double[] adblValue)
        {
            if (adblValue.Length == 0)
                return null;
            DateTime dtStart = tsDtl._dtStartTimestamp.Date;
            IntervalType type = tsDtl._tsIntervalType;
            int nInterval = tsDtl._nTSInterval;

            //Convert Day,Hour,Minute, to second
            nInterval = GetSecInterval(type, nInterval);        //just use seconds

            List<TimeSeries> obj = new List<TimeSeries>();
            DateTime time = dtStart;

     //       obj.Add(new TimeSeries(dtStart, adblValue[0])); // first records
            for (int i = 0; i < adblValue.Length; i++)                                //met 7/15/2013: modified for 2D array
            {
                obj.Add(new TimeSeries(time, adblValue[i]));
                time = time.AddSeconds(nInterval);
            }
            return obj;
        }

        /// <summary>
        /// Transforme a 2D array to a timeseris list.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="intervalValue"></param>
        /// <param name="type"></param>
        /// <param name="adblValue"></param>
        /// <param name="nDataIndex"></param>  which index to use; in general, we will use index zero.
        /// <returns></returns>
        public static List<TimeSeries> Array2DToTSList(DateTime start, int intervalValue, IntervalType type, double[,] adblValue, int nDataIndex = 0)
        {

            if (adblValue == null)
            {
                return null;
            }
            if (adblValue.Length == 0)
                return null;

            //Convert Day,Hour,Minute, to second
            double secInteval = GetSecInterval(type, intervalValue);

            List<TimeSeries> obj = new List<TimeSeries>();
            DateTime time = start;

 //           obj.Add(new TimeSeries(start, adblValue[0, nDataIndex])); // first records        //met 9/10/2013: original code duplicated first value.
            for (int i = 0; i < adblValue.GetLength(0); i++)                                //met 7/15/2013: modified for 2D array
            {
                obj.Add(new TimeSeries(time, adblValue[i, nDataIndex]));
                if (type == IntervalType.Month)
                    time = time.AddMonths(intervalValue);
                else if (type == IntervalType.Year)
                    time = time.AddYears(intervalValue);
                else if (type == IntervalType.Day)
                    time = time.AddDays(intervalValue);
                else if (type == IntervalType.Minute)
                    time = time.AddMinutes(intervalValue);
                else
                    time = time.AddSeconds(secInteval);
               
            }
            return obj;
        }
        /// <summary>
        /// Get average by frequency
        /// </summary>
        /// <param name="function"></param>
        /// <param name="timeseries"></param>
        /// <returns></returns>
        public static List<TimeSeries> GetFrequencyAverageTS(AverageFunction function, List<TimeSeries> timeseries)
        {
            IEnumerable<TimeSeries> tsOutput = null;
            if (function == AverageFunction.Hour) // hourly average
            {
                tsOutput = from x in timeseries
                           group x by new DateTime(x._dtTime.Year, x._dtTime.Month, x._dtTime.Day, x._dtTime.Hour, 0, 0)
                               into a
                               select new TimeSeries
                               {
                                   _dtTime = a.Key,
                                   _dblValue = a.Average(x => x._dblValue)
                               };
            }
            else if (function == AverageFunction.Month) // monthly average
            {
                tsOutput = from x in timeseries
                           group x by new DateTime(x._dtTime.Year, x._dtTime.Month,
                           DateTime.DaysInMonth(x._dtTime.Year, x._dtTime.Month)) into a
                           select new TimeSeries
                           {
                               _dtTime = a.Key,
                               _dblValue = a.Average(x => x._dblValue)
                           };
            }
            else if (function == AverageFunction.Year) // yearly average
            {
                tsOutput = from x in timeseries
                           group x by new DateTime(x._dtTime.Year, 12, 31) into a
                           select new TimeSeries
                           {
                               _dtTime = a.Key,
                               _dblValue = a.Average(x => x._dblValue)
                           };
            }
            else// if (function == AverageFunction.Day) the default to daily
            {
                tsOutput = from x in timeseries
                           group x by new DateTime(x._dtTime.Year, x._dtTime.Month, x._dtTime.Day)
                               into a
                               select new TimeSeries
                               {
                                   _dtTime = a.Key,
                                   _dblValue = a.Average(x => x._dblValue)
                               };
            }
            return tsOutput.ToList();
        }


        /// <summary>
        /// Transform the string to time-series
        /// </summary>
        /// <returns></returns>
        public static List<TimeSeries> Transform2Timeseries(DateTime datStart, string strTimeUnit, string strValue)
        {
            List<TimeSeries> tsOut = new List<TimeSeries>();
            string[] astrValue = strValue.Split('\n');
            foreach (string strLine in astrValue)
            {
                if (strLine.Trim() != "")
                {
                    string[] astrLineValue = strLine.Split('\t');
                    int intStartValue = int.Parse(astrLineValue[0].Trim());
                    double dblValue = double.Parse(astrLineValue[1].Trim());
                    DateTime dat = new DateTime();
                    if (strTimeUnit.ToLower() == "year")
                    {
                        dat = datStart.AddYears(intStartValue);
                    }
                    else if (strTimeUnit.ToLower() == "month")
                    {
                        dat = datStart.AddMonths(intStartValue);
                    }
                    else if (strTimeUnit.ToLower() == "hour")
                    {
                        dat = datStart.AddHours(intStartValue);
                    }
                    else // day as default
                    {
                        dat = datStart.AddDays(intStartValue);
                    }
                    TimeSeries ts = new TimeSeries(dat, dblValue);
                    tsOut.Add(ts); //add to result
                }
            }
            return tsOut;
        }


        /// <summary>
        /// MET 10/10/16: added this so that we could quickly output a large 2D timeseries
        /// Assumes that the 2D array is really just a 1D array.
        /// Note: jagged array allows this NOT to be the case - however we only use in this manner at present
        ///     TODO: much better to test for this rather than assume...
        /// </summary>
        /// <param name="dJagged"></param>
        /// <returns></returns>
        public static double[,] Get2DArrayFromJagged(double[][,] dJagged)
        {
            int nCount = dJagged.Length;
            int nRows = dJagged[0].Length;
            double[,] dReturn = new double[nRows, nCount];

            for (int i = 0; i < nCount; i++)
            {
                double[,] dTS = dJagged[i];
                for (int j = 0; j < nRows; j++)
                {
                    dReturn[j,i] = dJagged[i][j, 0];
                }
            }
            return dReturn;
        }


        /// <summary>
        /// Calculate average multiplier
        /// </summary>
        /// <param name="intMultiplierInput"></param>
        /// <param name="intWarmingPeriod"></param>
        /// <param name="InputTS"></param>
        /// <returns></returns>
        public static List<TimeSeries> GetTimeSeriesAVG_Multiplier(int intMultiplierInput, int intWarmingPeriod, List<TimeSeries> InputTS)
        {
            return GetTimeSeriesAVG_MA_Multiplier(true, intMultiplierInput, intWarmingPeriod, InputTS);
        }
        /// <summary>
        /// Calculate Moving average
        /// </summary>
        /// <param name="intMultiplierInput"></param>
        /// <param name="intWarmingPeriod"></param>
        /// <param name="InputTS"></param>
        /// <returns></returns>
        public static List<TimeSeries> GetTimeSeriesMA_Multiplier(int intMultiplierInput, int intWarmingPeriod, List<TimeSeries> InputTS)
        {
            return GetTimeSeriesAVG_MA_Multiplier(false, intMultiplierInput, intWarmingPeriod, InputTS);
        }
        /// <summary>
        /// Calculate moving average
        /// </summary>
        /// <param name="intMultiplierInput"></param>
        /// <param name="intWarmingPeriod"></param>
        /// <param name="InputTS"></param>
        /// <returns></returns>
        private static List<TimeSeries> GetTimeSeriesAVG_MA_Multiplier(bool blnCalAverage, int intMultiplierInput, int intWarmingPeriod, List<TimeSeries> InputTS)
        {
            string strError = ""; List<TimeSeries> tsOutput = new List<TimeSeries>();
            try
            {
                bool blnIsWarningAdded = false;
                // check if it's average or moving average
                int intSkip = 0; // skip value
                // update multiplier first of all
                int intMultiplier = (intWarmingPeriod == 0 ? intMultiplierInput : intWarmingPeriod); // check if we get multiplier from the warming period
                while (true) // run the average
                {
                    var timeseries = (from t in InputTS
                                      select new { t }).Skip(intSkip).Take(intMultiplier);
                    if (timeseries.Count() == 0) break; // exit while
                    if (blnCalAverage == false) // for moving average
                    {
                        if (timeseries.Count() < intMultiplier) break; // exit when there is not enought multiplier at the end of record
                    }
                    double fltSumValue = 0.0d; int intAvgCounter = 0; bool blnAvgDate = false;
                    double dblValue = 0.0d;
                    DateTime datTimeAvg = DateTime.Now;
                    DateTime datWarming = DateTime.Now;
                    foreach (var ts in timeseries)
                    {
                        DateTime datTime = ts.t._dtTime;
                        if (blnCalAverage)
                        {
                            datTimeAvg = datTime;
                            if (blnAvgDate == false)
                            {
                                datWarming = datTime;
                                blnAvgDate = true;
                            }
                        }
                        else // calculate moving average
                        {
                            datTimeAvg = datTime; // take the last one
                        }
                        fltSumValue += ts.t._dblValue;
                        intAvgCounter++; // counter
                    }
                    dblValue = (intAvgCounter > 0 ? fltSumValue / intAvgCounter : MISSING_VALUE);
                    if (blnCalAverage) // Calculate average
                    {
                        intSkip += intMultiplier;   // multiplier by the average multiplier value                        
                    }
                    else
                    {
                        intSkip++;   // increase multiplier by 1 for moving average
                    }
                    // data row value
                    if (intWarmingPeriod > 0 && blnIsWarningAdded == false)
                    {   // add warming period in if any
                        TimeSeries ts = new TimeSeries(datWarming, dblValue);
                        tsOutput.Add(ts); // add time-series
                        blnIsWarningAdded = true;
                    }
                    // add another record
                    TimeSeries tsAvg = new TimeSeries(datTimeAvg, dblValue);
                    tsOutput.Add(tsAvg); // add time-series
                    // update multiplier and this is important in the case warming period is used only
                    intMultiplier = intMultiplierInput; // multiplier input
                }
            }
            catch (Exception ex)
            {
                strError = "Error in calculating moving average " + ex.Source + ": " + ex.Message + "!";
            }
            return tsOutput;
        }

        public static void ShiftTimeSeries(ref List<TimeSeries> lstTS_Ref, DateTime dtStartDate_REF, DateTime dtStartDate_LINK, int nShiftTSCode, int nShiftTSVal, bool bIsIncoming)
        {
            switch (nShiftTSCode)
            {
                case 0:
                    break;
                case -1:
                    int nYearOffset = (dtStartDate_LINK.Year - dtStartDate_REF.Year);
                    if (!bIsIncoming) { nYearOffset *= -1; }
                    ShiftTimeSeries(ref lstTS_Ref, IntervalType.Year, nYearOffset);
                    break;
                default:      //units code
                    break;
            }
        }

        public static void ShiftTimeSeries(ref List<TimeSeries> lstTS_Ref, IntervalType tsInterval, int nOffset)
        {
            for (int i = 0; i < lstTS_Ref.Count; i++ )
            {
                switch (tsInterval){
                    case IntervalType.Year:
                        lstTS_Ref[i]._dtTime = lstTS_Ref[i]._dtTime.AddYears(nOffset);
                        break;
                    case IntervalType.Month:
                        lstTS_Ref[i]._dtTime = lstTS_Ref[i]._dtTime.AddMonths(nOffset);
                        break;
                    case IntervalType.Day:
                        break;
                }               
             }
        }


          // #endregion



        /**
        // add a DateTS column with the explicit dates 
        public void tsAddDateToTS(ref DataTable dtTS_IN, int nEvaldID, int nResultTS_ID, ref OleDbConnection connRMG){
            DateTime dtStartDate; int nTS_Interval_Sec = -1;
            dtStartDate = tcGetStartDate(ref nTS_Interval_Sec , nEvaldID,  nResultTS_ID, ref connRMG);
            
            DataColumn dcDates = new DataColumn();
            dcDates.DataType = DbType.DateTime;
            dcDates.ColumnName = "DateTS";
            
            dtTS_IN.Columns.Add(dcDates);

            for (int i = 0; i<dtTS_IN.Rows.Count; i++){
                dtTS_IN.Rows[i]["DateTS"] = dtStartDate + i*nTS_Interval_Sec;
            }
        }

        //check tables which may include ts date information from 'more specific' to more general
        // sets ts_interval too
        // todo: implement BeginPeriod - allows the TS to begin on value other than the first stored record
        public DateTime tcGetStartDate(ref int nInterval_Sec , int nEvaldID, int nResultTS_ID, ref OleDbConnection connRMG){
            DateTime dateReturn = System.DateTime.Now;
            string sql = ""; int nParam =0; bool bExit = false;
            for (int i = 0; i<2; i++){
                switch (i){
                    case 0:
                        sql = "select TS_StartDate, TS_StartHour, TS_StartMin, TS_Interval, BeginPeriod from tblResultTS where (ResultTS_ID = @Param)";
                        nParam = nResultTS_ID;
                        break;
                    case 1:
                        sql = "select TS_StartDate, TS_StartHour, TS_StartMin, TS_Interval, BeginPeriod from tblResultTS where (ResultTS_ID = @Param)";
                        nParam = nEvaldID;
                        break;

                    case 2:     //ADD to scenario? not yet supported

                        break;
                }
                OleDbCommand cmd = new OleDbCommand(sql, connRMG);
                cmd.Parameters.Add("@Param", OleDbType.Integer).Value = nParam;
                OleDbDataReader drEvals = cmd.ExecuteReader();
                DataTable oTable = new DataTable();
                DataSet dsEvals = new DataSet();
                dsEvals.Tables.Add(oTable);
                dsEvals.Tables[0].Load(drEvals);

                if (dsEvals.Tables[0].Rows.Count > 0){
                    if (dsEvals.Tables[0].Rows[0]["TS_StartDate"].ToString() == null){
                        dateReturn = Convert.ToDateTime( dsEvals.Tables[0].Rows[0]["TS_StartDate"].ToString());
                        nInterval_Sec = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["TS_Interval"].ToString());
                        return dateReturn;
                    }

                }
            }
            return dateReturn;
        }


        // code 1 : standard interpolation 
        //
        // code 3: SimLink
        public DataTable tsInterpolateTS(DataTable dtTS_In,int nTS_Interval_CMP, int nCODE = 1)
        {
            int nRecordTS_In = 0; int nRecordTS_Out = 0; int nRecourdCountIN;
            DataTable dtOut = new DataTable();
            nRecourdCountIN = dtTS_In.Rows.Count;
            if (nRecourdCountIN <= 1)
            {
                //todo: log message about the problem
                return dtOut;       //t
            }

            switch (nCODE)
            {
                case 1:
                    // look online to see if there is some sort of interpolation function
                    break;

                case 3:             //SimClim comparison with every month available
                    // can this be done using the 'xdoc' type of functionality in DTT CommonUtils??
                    break;
                case 4:             //SimClim quarterly data Jan_Feb_March.... April_May_June etc. 
                                    //rare (?) case when 
                    break;
            }

        }
 
 */

        //met 10/31/2013 made for SimLink- need all sorts of more robust functionality here- this is a start
        // creates a standard interval time series, starting at requested datetime; input need not be 
        //todo: consider a faster alternative that takes standard-interval output (just padding)
        public static double[,] CreateStandardIntervalTimeSeries(List<TimeSeries> lstTS, TimeSeriesDetail tsdSimStart)
        {
            DateTime dtLastEntry = lstTS[lstTS.Count() - 1]._dtTime;
            TimeSpan dt = dtLastEntry - tsdSimStart._dtStartTimestamp;
            int nSecondsInterval = GetSecInterval(tsdSimStart._tsIntervalType, tsdSimStart._nTSInterval);
            double dSecondsInterval = dt.TotalSeconds;                              //todo: potentially check that this is an integer
            int nDataPoints = Convert.ToInt32(Math.Floor(dSecondsInterval) / nSecondsInterval) + 1;

            if (nDataPoints <= 0)
            {
                //todo: log it
                return null;
            }
            double[,] dOut = new double[nDataPoints, 1];                 //2-d array because that is what hdf5 wrapper currrently supports
            int nIndex = 0;
            TimeSpan tDiff;
            foreach (TimeSeries ts in lstTS)
            {
                tDiff = ts._dtTime - tsdSimStart._dtStartTimestamp;
                nIndex = Convert.ToInt32(tDiff.TotalSeconds / nSecondsInterval);     // should be int ; need to do better checking
                dOut[nIndex, 0] = ts._dblValue;
            }
            return dOut;

        }

        #endregion

        #region time-series  (SimClim like)
        /// <summary>
        /// Interpolate SimClim raw result to annual data
        /// </summary>
        /// <param name="adblRawSimClimResult"></param>
        /// <returns></returns>
        public static double[,] InterpolateSimClimResult(double[,] adblRawSimClimResult)
        {
            int intTotalYear = Convert.ToInt32(adblRawSimClimResult[adblRawSimClimResult.GetUpperBound(0), 0] - adblRawSimClimResult[0, 0]) + 1;
            double[,] adblSimClimOutput = new double[intTotalYear, 13]; // 12 months 
            // the first index is Year
            for (int intMonthIndex = 1; intMonthIndex <= adblRawSimClimResult.GetUpperBound(1); intMonthIndex++) // loop by month
            {
                int intArrayIndex = 0; //Console.WriteLine(adblRawSimClimResult.GetUpperBound(1));
                for (int intYearIndex = 0; intYearIndex < adblRawSimClimResult.GetUpperBound(0); intYearIndex++) // loop by year
                {
                    int intYearStart = Convert.ToInt32(adblRawSimClimResult[intYearIndex, 0]);
                    int intYearEnd = Convert.ToInt32(adblRawSimClimResult[intYearIndex + 1, 0]);
                    int intYearDiff = intYearEnd - intYearStart;
                    double dblIncremental = (adblRawSimClimResult[intYearIndex + 1, intMonthIndex] - adblRawSimClimResult[intYearIndex, intMonthIndex]) / intYearDiff; // yearly incremental
                    if (intYearEnd == adblRawSimClimResult[adblRawSimClimResult.GetUpperBound(0), 0]) intYearEnd++; // add till the last one
                    for (int intYear = intYearStart; intYear < intYearEnd; intYear++) // start yearly interpolation
                    {
                        double dblValue = adblRawSimClimResult[intYearIndex, intMonthIndex] + dblIncremental * (intYear - intYearStart);
                        adblSimClimOutput[intArrayIndex, 0] = intYear; // yearly data
                        adblSimClimOutput[intArrayIndex, intMonthIndex] = dblValue; // interpolated value
                        intArrayIndex++; // increase array index
                    }
                }
            }
            return adblSimClimOutput;
        }

        /// <summary>
        /// Multiply time-series with SimClim data
        /// </summary>
        /// <param name="inputTS"></param>
        /// <param name="adblSimClimTS"></param>
        /// <returns></returns>
        public static List<TimeSeries> PerturbMonthlyTS_Data(List<TimeSeries> inputTS, double[,] adblRawSimClimResult, bool bIsTemperatureVar)
        {
            double[,] adblSimClimResult = InterpolateSimClimResult(adblRawSimClimResult); // interpolate SimClim result first
            int[] aintYear = GetSimClimYearly(adblSimClimResult); // get the yearly from start to end year
            // get first

            // get time-series multiply using LINQ
            IEnumerable<TimeSeries> outputTS = from c in inputTS
                                               select new TimeSeries()
                                               {
                                                   _dtTime = c._dtTime,
                                                   _dblValue = (bIsTemperatureVar ? c._dblValue + GetSimClimDataAt(adblSimClimResult, aintYear, c._dtTime.Year, c._dtTime.Month)
                                                   : c._dblValue + (c._dblValue * GetSimClimDataAt(adblSimClimResult, aintYear, c._dtTime.Year, c._dtTime.Month)) / 100.0d)
                                               };
            return outputTS.ToList();
        }

        /// <summary>
        /// Return yearly array from the start to end period
        /// </summary>
        /// <param name="adblSimClimResult"></param>
        /// <returns></returns>
        private static int[] GetSimClimYearly(double[,] adblSimClimResult)
        {
            int[] aintYear = new int[adblSimClimResult.GetUpperBound(0) + 1];
            for (int intYearIndex = 0; intYearIndex <= adblSimClimResult.GetUpperBound(0); intYearIndex++)
            {
                aintYear[intYearIndex] = Convert.ToInt32(adblSimClimResult[intYearIndex, 0]);
            }
            return aintYear;
        }


        /// <summary>
        /// Get SimClim data at a given year/month
        /// </summary>
        /// <param name="adblSimClimResult"></param>
        /// <param name="intMonth"></param>
        /// <param name="intYear"></param>
        /// <returns></returns>
        private static double GetSimClimDataAt(double[,] adblSimClimResult, int[] aintYear, int intYear, int intMonth)
        {
            // find year index
            int intYearKeyIndex = aintYear.ToList().FindIndex(y => y == intYear);
            return (aintYear[intYearKeyIndex] >= 0 ? adblSimClimResult[intYearKeyIndex, intMonth] : TimeSeries.MISSING_VALUE); // return missing value
        }

        #endregion


        #region Conversion

        /// <summary>
        /// 2D double is actually just 1d.
        /// met 11/15/16
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static double[,] ConvertListToDouble2D(List<double> lst)
        {
            double[,] dVals = new double[lst.Count(), 1];
            int nCounter = 0;
            foreach (double d in lst)
            {
                dVals[nCounter, 0] = d;
                nCounter++;
            }
            return dVals;
        }

        //returns number of seconds in string of type HH:MM    (todo: handle case where no colon)
        public static int CONVERT_GetSecFromHHMM(string sHHMM, bool bContainsDelimiter = true)
        {
            int nReturn = -9999;
            if (bContainsDelimiter)
            {
                string[] s = sHHMM.Split(':');
                nReturn = Convert.ToInt32(s[0]) * 3600 + Convert.ToInt32(s[1]) * 60;
            }
            return nReturn;
        }

        public static string Convert_DateTimeToSWMM_DAT_String(DateTime dt)
        {
            string sReturn = dt.Year.ToString() + "\t" + dt.Month.ToString() + "\t" + dt.Day.ToString() + "\t" + dt.Hour.ToString() + "\t" + dt.Minute.ToString() + "\t" + dt.Second.ToString();
            return sReturn;
        }

        #endregion


        #region TS_Helper


        /// <summary>
        /// Return time interval of an equal interval time series.
        /// </summary>
        /// <param name="lstTS_Ref" ></param>
        /// <returns> double interval between two timeseries</returns>
        public static double GetTSInterval(List<TimeSeries> lstTS_Ref)
        {
            double dReturn = -1;
            if (lstTS_Ref.Count == 0)
            {
                dReturn = MISSING_VALUE;
            }
            else if (lstTS_Ref.Count == 1)
            {
                dReturn = -1;
            }
            else
            {
                TimeSpan dt = lstTS_Ref[1]._dtTime -  lstTS_Ref[0]._dtTime;
                dReturn = dt.TotalSeconds;
            }
            return dReturn;
        }
        #endregion

    }
}
