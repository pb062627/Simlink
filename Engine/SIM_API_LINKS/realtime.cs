using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Nini.Config;
using System.IO;

namespace SIM_API_LINKS
{
    public class realtime
    {
        /// <summary>
        /// defines methods for starting an RT analysis       (created 12/14/16)
        ///     Idea: could be its own class and handle all aspects of start time / time tracking. don't build till necessary
        /// </summary>
        public enum TriggerRT
        {
            manual_definestart,
            manual_currenttime,
            event_timeinterval,
            event_systemcondition
        }

        public TriggerRT _trigger = TriggerRT.manual_definestart;
        public TimeSeries.TimeSeriesDetail _tsdExecuteInterval;
        public DateTime _dtCreateRealTime_Object;
        public DateTime _dtBeginLastRun;
        public DateTime _dtStartRealTimeRun;
        public DateTime _dtSIM_StartSim;                                        // start of simulation event  (use SIM to denote)
        public TimeSpan _tsInterval_Seconds;                                    // time interval of results data
        public TimeSpan _tsSim_Duration = new TimeSpan(1, 0, 0, 0);              // default set to 1 day sim
        public TimeSpan _tsShiftTimeFromPresent = TimeSpan.FromSeconds(0);        //ofset from present time 
        public simlink _simlink;
        public int _nCheckForRunFreq_Seconds;
        public bool _bSkipSeconds = true;           // when true, start all sims at HH:MM:00
        protected const int _nDEFAULT_DURATION_MIN = 1440;
        protected const int _nDEFAULT_TS_INTERVAL_SEC = 15 * 60;
        public bool _bSkipTSCheck = false;                      // set to true if testing on historical TS and you don't want to wait between runs
                                                                //      protected bool bCheckExternalDataTimestamps = true;             // whether or not to check that you are in the period; faster not to, but 
        private List<DateTime> _lstdtStartDates = new List<DateTime>(); //SP 14-Apr-2017
        public bool _bRunDefinedDates = false;
        public int _nScenStart = -1; //SP 10-Oct-2017 - allowing user to modify through input dictionary
        public int _nScenEnd = 100; //SP 10-Oct-2017 - allowing user to modify through input dictionary
        public int _nScenStart_DefaultProcessScen = 2;
        private string _sHotStartLog ="UNDEFINED";     //location to search for record of hot start
        private bool _bManageHotStart = true;
        public string _sHotstart_filename_base="UNDEFINED";

        public realtime(int nModelTypeID, int nEvalID, int nTriggerMethod, string sCONN, int nDB_Type, Dictionary<string, string> dictArgs, int nLogLevel = 1)
        {
            _simlink = CommonUtilities.GetSimLinkObject(nModelTypeID);
            _simlink._runType = SimlinkRunType.realtime;
            _trigger = (TriggerRT)nTriggerMethod;                                   // set the type of trigger which will be used
            if (nDB_Type <= 1)              //note: cheap way of testing- do better
            {
                _simlink.InitializeModelLinkage(sCONN, nDB_Type, false, ".", nLogLevel);
                _simlink.InitializeEG(nEvalID);
            }
            else
            {
                XmlConfigSource configXML = new XmlConfigSource(@"\\hchfpp01\Groups\WBG\MillCreek\Analysis\SimLink\no_db_backend\169_data\run_job_169.xml");           //);         // init a config object
                _simlink.InitializeByConfig(configXML);
            }
            SetRT_ParamsFromDict(dictArgs);
            _dtCreateRealTime_Object = System.DateTime.Now;
            _nCheckForRunFreq_Seconds = 300;        //todo: parameterize
                                                    //           if (dtStartRealTime != default(DateTime))
                                                    //             _tsShiftTimeFromPresent = dtStartRealTime - System.DateTime.Now;        // calc  offset - used in ts params

            _simlink.SetRealTimeOffset(_tsShiftTimeFromPresent);                        // set offset for all external data sources 
            _simlink.SetScenarioDS_DeleteAtBeginning(DeleteScenDetails.Manual);            // calling app responsible for deleting details         }

            if (!_simlink._bIsSimLinkLite)
                _simlink.InitializeSimlinkVarsByConfig(sCONN);
        }
        bool bValid = true;         // use to test parsing to double/string to make sure we are good
        private void SetRT_ParamsFromDict(Dictionary<string, string> dictArgs)
        {
            double dOffset = 0; double dDuration = _nDEFAULT_DURATION_MIN; int nStartScen = _nScenStart; int nEndScen = _nScenEnd;
            int nTS_Interval = _nDEFAULT_TS_INTERVAL_SEC;           // default
            if (dictArgs.ContainsKey("offset"))
            {
                bValid = Double.TryParse(dictArgs["offset"].ToString(), out dOffset);
                if (!bValid)
                    _simlink._log.AddString(string.Format("Realtime offest value of {0} not convertible to double", dictArgs["offset"]), Logging._nLogging_Level_2, false, true);
            }
            if (dictArgs.ContainsKey("use_hotstart"))
            {
                if (dictArgs["use_hotstart"].ToLower() == "y")
                    _bManageHotStart = true;
                else
                    _bManageHotStart = false;
            }
            else
            {
                _bManageHotStart = false;
            }

            // manage text file where hotstart information is stored
            if (dictArgs.ContainsKey("hotstart_location"))
            {
                _sHotStartLog = dictArgs["hotstart_location"];
            }

            if (dictArgs.ContainsKey("hotstart_filename_base"))
            {
                _sHotstart_filename_base = dictArgs["hotstart_filename_base"];
            }

            if (dictArgs.ContainsKey("duration"))
            {
                bValid = Double.TryParse(dictArgs["duration"].ToString(), out dDuration);
                if (!bValid)
                    _simlink._log.AddString(string.Format("Realtime duration value of {0} not convertible to double", dictArgs["duration"]), Logging._nLogging_Level_2, false, true);
            }
            else
            {
                _simlink._log.AddString("no realtime 'duration' key found; using default duration (1440 min)", Logging._nLogging_Level_2, false, true);
            }
            ///////////////////////////////  TS INTERVAL        ///
            if (dictArgs.ContainsKey("ts_interval"))
            {
                bValid = Int32.TryParse(dictArgs["ts_interval"].ToString(), out nTS_Interval);
                if (!bValid)
                    _simlink._log.AddString(string.Format("Realtime ts inverval value of {0} not convertible to double", dictArgs["ts_interval"]), Logging._nLogging_Level_2, false, true);
            }
            else
            {
                _simlink._log.AddString("No realtime 'ts_interval' key found; using default duration (15 min)", Logging._nLogging_Level_2, false, true);
            }

            if (dictArgs.ContainsKey("start_timestamp"))
            {
                string sDateTime = dictArgs["start_timestamp"];

                if (sDateTime == "-1")
                    _dtSIM_StartSim = System.DateTime.Now;
                else if (sDateTime == "-2")     // this code means start from the beginning of the current day
                {
                    _dtSIM_StartSim = System.DateTime.Today;
                }
                else if (sDateTime == "-3")     // this code means start from the beginning of the current day
                {
                    _dtSIM_StartSim = System.DateTime.Today.AddHours(System.DateTime.Now.Hour); //+ System.DateTime.Now.AddHours(System.DateTime.
                }  
                else
                {
                    if (sDateTime.Substring(0, 1) == "z")           // special case offset from current time
                    {
                        string[] sVals = sDateTime.Split('_');
                        _dtSIM_StartSim = CommonUtilities.GetModifiedDateTime(System.DateTime.Now, sVals[1]);
                    }
                    else
                    {
                        bValid = DateTime.TryParse(dictArgs["start_timestamp"], out _dtSIM_StartSim);
                    }

                }
            }
            else
            {
                _simlink._log.AddString("no 'start_date' key found; using current timestamp", Logging._nLogging_Level_2, false, true);
                _dtSIM_StartSim = System.DateTime.Now;
            }

            if (dictArgs.ContainsKey("loops"))
            {
                int nLoops = Convert.ToInt32(dictArgs["loops"]);
                _bRunDefinedDates = true;
                _lstdtStartDates.Add(_dtSIM_StartSim);
                DateTime dtRunning = _dtSIM_StartSim;        
                for (int i = 0; i < nLoops -1; i++)
                {
                    dtRunning += TimeSpan.FromSeconds(Math.Round(dDuration * 60, 0));  //convert duration in minutes to sec
                    _lstdtStartDates.Add(dtRunning);
                }
            }
                // met: prefer loops as easier to define- defined_run_dates is retained, and we use the same bool to set it
            //SP 14-Apr-2017 processing the defined_run_dates
            else if (dictArgs["defined_run_dates"]!="undefined")
            {
                _bRunDefinedDates = true;
                //separate out the dates using comma delimiter
                List<string> lstsStartDates = new List<string>();
                lstsStartDates = dictArgs["defined_run_dates"].ToString().Split(',').ToList();

                //convert strings to dates if possible
                foreach (string sDate in lstsStartDates)
                {
                    DateTime dtDate;
                    if (DateTime.TryParse(sDate, out dtDate))
                    {
                        _lstdtStartDates.Add(dtDate);
                    }
                    else
                        _simlink._log.AddString(string.Format("Date {0} defined in defined_run_dates but is not a valid date", sDate), Logging._nLogging_Level_1, false, true);
                }

                _simlink._log.AddString(string.Format("Found {0} defined run dates. start_timestamp overridden with defined_run_dates", _lstdtStartDates.Count()), Logging._nLogging_Level_1, false, true);
            }
            else
            {
                _simlink._log.AddString("No realtime 'defined_run_dates' key found; using default of empty", Logging._nLogging_Level_2, false, true);
            }

            //SP 10-Oct-2017
            if (dictArgs.ContainsKey("ScenStart"))
            {
                bValid = Int32.TryParse(dictArgs["ScenStart"].ToString(), out nStartScen);
                if (!bValid)
                    _simlink._log.AddString(string.Format("Realtime ScenStart value of {0} not convertible to integer", dictArgs["ScenStart"]), Logging._nLogging_Level_2, false, true);
                else
                {
                    //modify both the ScenStart and the default ProcessScenario ScenStart
                    _nScenStart = nStartScen;
                    _nScenStart_DefaultProcessScen = nStartScen;
                }
            }
            else
            {
                _simlink._log.AddString(string.Format("No realtime 'ScenStart' key found; using default scenstart ({0})",_nScenStart), Logging._nLogging_Level_2, false, true);
            }

            //SP 10-Oct-2017
            if (dictArgs.ContainsKey("ScenEnd"))
            {
                bValid = Int32.TryParse(dictArgs["ScenEnd"].ToString(), out nEndScen);
                if (!bValid)
                    _simlink._log.AddString(string.Format("Realtime ScenEnd LC value of {0} not convertible to integer", dictArgs["ScenEnd"]), Logging._nLogging_Level_2, false, true);
                else
                    _nScenEnd = nEndScen;
            }
            else
            {
                _simlink._log.AddString(string.Format("No realtime 'ScenEnd' key found; using default scenend ({0})", _nScenEnd), Logging._nLogging_Level_2, false, true);
            }


            // having read the dictionary, set the appropriate values
            int nOffsetSec = Convert.ToInt32(Math.Round(dOffset * 60, 0));
            int nDurationSec = Convert.ToInt32(Math.Round(dDuration * 60, 0));
            _tsShiftTimeFromPresent = new TimeSpan(0, 0, nOffsetSec);
            _tsSim_Duration = new TimeSpan(0, 0, nDurationSec);
            _dtSIM_StartSim = _dtSIM_StartSim + _tsShiftTimeFromPresent;                            // shift the start time base upon the request   (may belong more elsewhere)
            _tsInterval_Seconds = new TimeSpan(0, 0, nTS_Interval);
            
        }


        //SP 14-Apr-2017 - Run defined dates
        public void RunDefinedDates()
        {
            //iterate through dates in the provided list
            int nCounter = 1;
            foreach (DateTime dtStartDate in _lstdtStartDates)
            {
                _dtSIM_StartSim = dtStartDate;
                Console.WriteLine("Begin run {0} for start time {1}:{2}", nCounter, dtStartDate, System.DateTime.Now);
                Run(1);
                nCounter++;
            }
        }


        /// <summary>
        /// Run the realtime sim
        ///     12/26/16:  added ability to support SWMM init/sim need
        ///     TODO: consider whether we should have a RUN and RUN_COHORT or RUN_wINIT   
        /// </summary>
        /// <param name="nLoops"></param>
        /// <param name="bCreateStartEndModChanges"></param>
        public void Run(int nLoops = -1, int nMinutesToAdd = 60, bool bCreateStartEndModChanges = true, bool bSingleExternalDataRequest = true, int nInitPeriods = 1)
        {
            int nCount = 0;
            // if nLoops == -1, this will run until somebody stops it
            bool bValid = true;

            DateTime dtCheckWhenToRun = System.DateTime.Now;
            TimeSpan tAdd = new TimeSpan(0, nMinutesToAdd, 0);
            int nEvalID = _simlink._nActiveEvalID;

            //DateTime dtCheckWhenToRun = System.DateTime.Now; //SP 28-Dec-2016 - changed to use the new StartTime
            //todoSP          DateTime dtCheckWhenToRun = _dtSIM_StartSim;

            //SP 28-Dec-2016 For temporary modification of TS for real time requirements
            TimeSeries.TimeSeriesDetail _tsdFirstSim = new TimeSeries.TimeSeriesDetail(_dtSIM_StartSim, IntervalType.Second, (int)_tsInterval_Seconds.TotalSeconds, _dtSIM_StartSim + _tsSim_Duration);


            while (nCount < nLoops || nLoops == -1)
            {
                _simlink._nActiveEvalID = nEvalID;        // for multiple loops, may need to set eval... todo - better job of thiss.


                // met 1/25/17 bojangles figure out how this is intended to work
                /*               while (!CheckIfRun(dtCheckWhenToRun) || _bSkipTSCheck)
                               {
                                   System.Threading.Thread.Sleep(_nCheckForRunFreq_Seconds);    // consider async options that don't lock ui if needed
                               }  */

                Console.WriteLine("");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Begin run {0} at {1}", nCount + 1, System.DateTime.Now);
                dtCheckWhenToRun += _tsInterval_Seconds;  //SP 28-Dec-2016 re-enabled this, maybe commented out by MET for testing purposes?

                _dtBeginLastRun = System.DateTime.Now;
                DateTime dtStartSim = _dtSIM_StartSim; //+ TimeSpan.FromTicks(_tsInterval_Seconds.Ticks * nCount); //SP 28-Dec-2016 - set the sim start time to now - CONFIRM WITH MET
                //DateTime dtEndSim = _dtSIM_StartSim + _tsSim_Duration; //SP 28-Dec-2016 - changed to use the new StartTime
                DateTime dtEndSim = dtStartSim + _tsSim_Duration;

                Console.WriteLine("Sim Start Time = {0}, Sim End Time = {1}", dtStartSim.ToString("yyyy-MM-dd HH:mm:ss"), dtEndSim.ToString("yyyy-MM-dd HH:mm:ss"));

                Dictionary<string, string> dictRequest = new Dictionary<string, string>(){
                   //{ "start_date", _dtSIM_StartSim.ToString("yyyy-MM-dd HH:mm:ss")}, //SP 28-Dec-2016 - changed to use the new StartTime
                   { "start_date", dtStartSim.ToString("yyyy-MM-dd HH:mm:ss")},
                   { "end_date", dtEndSim.ToString("yyyy-MM-dd HH:mm:ss")},
                };

                bool bRunInitRun = _simlink.IsCohortTypeInit();
                ///// INIT RUN IF NEEDED
                if (bRunInitRun)        // perform an initial run
                {
                    // step 0 : Get Dict set so we have available
                    DateTime dtStartInit = _dtSIM_StartSim - _tsInterval_Seconds;         // todo- multiply by nInitPeriods
                    DateTime dtEndInit = _dtSIM_StartSim;
                    dictRequest["start_date"] = dtStartInit.ToString("yyyy-MM-dd HH:mm:ss"); //set dictRequest so the external data is accurate
                    dictRequest["end_date"] = dtEndInit.ToString("yyyy-MM-dd HH:mm:ss");    // todo: time format should be stored somewhere and referneced throughout?

                    // step 1: insert a new scenario
                    _simlink._nActiveEvalID = Convert.ToInt32(_simlink._dsEG_Cohort.Tables[0].Rows[0]["EvaluationID"].ToString());        // get the actual scne EVAL ID (just one assumed for now 12/26/16)
                    _simlink._sActiveModelLocation = _simlink._dsEG_Cohort.Tables[0].Rows[0]["ModelFileLocation"].ToString();
                    _simlink.InsertScenario(dtStartInit.ToString("yyyy-MM-dd HH:mm:ss"), _nScenStart, _nScenEnd);
                    _simlink.SetActiveScenarioID(_simlink._nActiveScenarioID, 1);      // active model file location etc

                    // step 2: Set simlink start and end dates for sim
                    _simlink.SetSimTimeSeries(bCreateStartEndModChanges, dtStartInit, dtEndInit, (int)_tsInterval_Seconds.TotalSeconds);
                    //_simlink.SetSimTimeSeries(bCreateStartEndModChanges, _dtSIM_StartSim, dtEndSim, (int)_tsInterval_Seconds.TotalSeconds); //SP 28-Dec-2016 - changed to use the new StartTime
                    // todo: SP to review and del line if not needed _simlink.SetSimTimeSeries(bCreateStartEndModChanges, dtStartSim, dtEndSim, (int)_tsInterval_Seconds.TotalSeconds);

                    // step 3: Extract external data
                    //SP 15-Feb-2017 Extract External Data is now performed as part of ProcessScenario
                    //dictrequest currently gets populated with _tsdSimDetails._dtStartTimestamp and _tsdSimDetails._dtEndTimestamp

                    //SP 9-Mar-2017 This is now called as part of the standard simlink process
                    /*bValid = _simlink.ExtractExternalData(dictRequest, RetrieveCode.Aux, "tag_names");    // retrieve any external data */

                    // step 5: Execute the run   (dat files are written if needed in step 2 of process scenario
                    //SP 10-Oct-2017 - maintaining hardcoding start Scen = 2. modified start and end Scen LC steps (previously ScenStart = 2 and ScenEnd = 100)
                    _simlink.ProcessScenario(_simlink._nActiveProjID, _simlink._nActiveEvalID, _simlink._nActiveReferenceEvalID, _simlink._sActiveModelLocation, _simlink._nActiveScenarioID, _nScenStart_DefaultProcessScen, _nScenEnd);  // create a new scenario

                    if (false)
                    {
                        //Export INIT data (it is possible this would be desired)
                    }
                    //step 7: clear simlink data for next run
                    _simlink.ScenDS_ClearAfterScenario(_simlink._nActiveScenarioID);       // now  delete the scen info

                    // step 8:              set EG stuff to next line
                    if (_simlink.IsCohortTypeInit())
                    {
                        // perform a lightweight EG init update.
                        // it is good to avoid going back and initializing all the dsEG unless needed
                        // still, it is possible that all simlink functionality/vars are not exactly kosher via this method
                        // todo: formalize this approach
                        int nNewEvalID = Convert.ToInt32(_simlink._dsEG_Cohort.Tables[0].Rows[1]["EvaluationID"].ToString());
                        _simlink.SetActiveEvalID(nNewEvalID, simlink.EvalActivationCode.Cohort);
                    }
                }
                //<---                      END THE INIT RUN                -->>

                ///////////////////////////                         THE ACTUAL RUN                        .....................................

                // STEP 100             adjust time series for actual run
                dictRequest["start_date"] = _dtSIM_StartSim.ToString("yyyy-MM-dd HH:mm:ss"); //set dictRequest so the external data is accurate
                dictRequest["end_date"] = dtEndSim.ToString("yyyy-MM-dd HH:mm:ss");

                // step 101             insert a new scenario

                // step 102.5: insert the scenario now that the timestamps are updated.
                _simlink.InsertScenario(_dtSIM_StartSim.ToString("yyyy-MM-dd HH:mm:ss"), _nScenStart, _nScenEnd);
                _simlink.SetActiveScenarioID(_simlink._nActiveScenarioID, 1);
                // step 102             Set simlink start and end dates for sim
                //todoSP : figure out which of these is the right one....
                _simlink.SetSimTimeSeries(bCreateStartEndModChanges, _dtSIM_StartSim, dtEndSim, (int)_tsInterval_Seconds.TotalSeconds);
                //_simlink.SetSimTimeSeries(bCreateStartEndModChanges, _dtSIM_StartSim, dtEndSim, (int)_tsInterval_Seconds.TotalSeconds); //SP 28-Dec-2016 - changed to use the new StartTime
                //    _simlink.SetSimTimeSeries(bCreateStartEndModChanges, dtStartSim, dtEndSim, (int)_tsInterval_Seconds.TotalSeconds);

                // step 103             Extract external data        
                // where possible, use the data that has already been obtained and covers the init period.
                //SP 9-Mar-2017 Extract External Data is now called as part of the standard Simlink Procedure
                /*if (true)           //todo: know when we have to get more data           !bSingleExternalDataRequest || !bValid)
                {
                    bValid = _simlink.ExtractExternalData(dictRequest, RetrieveCode.Aux, "tag_names");    // retrieve any external data             
                }*/
                if (false)   //todo: paramterize
                {
                    Console.WriteLine("Setting all aux TS to the first value");
                    _simlink.SetAllAuxTSToVal(0);
                }
                //step 4 
                _simlink.PreProcessTimeseriesData(_tsdFirstSim, _dtSIM_StartSim);

                string sHSF = "";
                // simlink object must manage needed input files
                if (_bManageHotStart)
                {
                    sHSF = ManageHotStart(_dtSIM_StartSim - _tsSim_Duration);    // take last hot start (if found) and put in new directory   met 11/1/17
                }

                // step 105:            Execute run 
                //SP 10-Oct-2017 - maintaining hardcoding start Scen = 2. modified start and end Scen LC steps (previously ScenStart = 2 and ScenEnd = 100)
                _simlink.ProcessScenario(_simlink._nActiveProjID, _simlink._nActiveEvalID, _simlink._nActiveReferenceEvalID, _simlink._sActiveModelLocation, _simlink._nActiveScenarioID, _nScenStart_DefaultProcessScen, _nScenEnd);  // create a new scenario
                              
                if (_bManageHotStart)
                {
                    if (sHSF == "UNDEFINED")
                    {// this should only be round 1 - no good hsf, so you need to create the hsf filename
                        sHSF = CommonUtilities.GetSimLinkDirectory(_simlink._sActiveModelLocation, _simlink._nActiveScenarioID, _simlink._nActiveEvalID, true);
                        sHSF = Path.Combine(sHSF, _sHotstart_filename_base);
                    }

                    WriteHotStartLog(sHSF, _dtSIM_StartSim);
                }

                // step 106              Export Results
                //         _simlink.WriteOutputData();
                // should already be done by the process scenario... simlink.WriteOutputData_Grouped();

                //step 7: clear simlink data for next run
                _simlink.ScenDS_ClearAfterScenario(_simlink._nActiveScenarioID);       // now  delete the scen info
                nCount++;
                _dtSIM_StartSim = _dtSIM_StartSim + tAdd;
            }
        }


        /// <summary>
        /// Function to prepare the hsf needed for new run.
        /// Expects a new scenario, and a single line CSV of form datetime, location
        /// Performs a simple cpy to new location
        /// </summary>
        /// <param name="dtSeek"></param>
        private string ManageHotStart(DateTime dtSeek)
        {
            if (File.Exists(_sHotStartLog)){
                string[] sLines = File.ReadAllLines(_sHotStartLog);
                string[] sKVP = sLines[0].Split(',');
                if(sKVP.Length==2){
                    if(sKVP[0].Trim()==dtSeek.ToString("yyyy-MM-dd HH:mm:ss")){   // check that the date recorded for last hsf is same as the one we seek (don't just use whatever was there)
                        string sHSF_PreviousRun = sKVP[1];
                        string sTargetPath = CommonUtilities.GetSimLinkDirectory(_simlink._sActiveModelLocation, _simlink._nActiveScenarioID, _simlink._nActiveEvalID, true);  //path does not yet exist.
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        string sNewHSF = Path.Combine(sTargetPath,Path.GetFileName(sHSF_PreviousRun));
                        sNewHSF = sNewHSF.Replace("_OUT","_IN");  // todo: perform replacements based upon a dictionary
                        _simlink._sHotStart_ToUse = sNewHSF;
                        _simlink._sHotStart_ToCopy = sHSF_PreviousRun;
               //         Console.WriteLine("Copying prev hsf {0} to location {1}",sHSF_PreviousRun,sNewHSF);
             //           File.Copy(sHSF_PreviousRun,sNewHSF);
           //             _simlink._nScenarioHotStart
                        return sNewHSF;
                    }
                    else{
                        Console.WriteLine("No previous hot start with correct date found; this is likely the case if the previous day simulation was not run. Default to hsf in base dir");
                        return "UNDEFINED";
                    }
                }
                else{
                    Console.WriteLine("hot start log must be a single line,of format DATE,file_path");
                    return "UNDEFINED";
                }
            }
            else{
                Console.WriteLine("Hot start requested but no hot start log found in file {0}",_sHotStartLog);
                return "UNDEFINED";
            }
        }


        /// <summary>
        /// Store the latest hsf that was created, so that subsequent run may use.
        /// 
        /// Added replacement script 2/7/2018
        /// </summary>
        /// <param name="sHSF"></param>
        /// <param name="dtCurrent"></param>
        private void WriteHotStartLog(string sHSF, DateTime dtCurrent){
            using (StreamWriter writer = new StreamWriter(_sHotStartLog)){
                writer.WriteLine(dtCurrent.ToString("yyyy-MM-dd HH:mm:ss") + "," + sHSF.Replace("_IN", "_OUT"));
            }
        }


        private bool CheckIfRun(DateTime dtNext)
        {
            bool bProceed=false;
            if(dtNext<=System.DateTime.Now)
                bProceed = true;
            return bProceed;
        }
        public void Close()
        {
            _simlink.CloseModelLinkage();
        }

        /// <summary>
        /// Return a realtime object that is initialized based on config file
        /// </summary>
        /// <param name="config"></param>
        /// <param name="bValid"></param>
        /// <returns></returns>
        public static realtime InitializeByConfig(IConfigSource config, out bool bValid)
        {
            Dictionary<string, string> dictArgs = GetDictArgsFromConfig(config.Configs["realtime"]);
            string sModelType = config.Configs["simlink"].GetString("type", "swmm");
            int nModelTypeID = CommonUtilities.GetSimLinkObjectAsInt(sModelType);
            int nEvalID = Convert.ToInt32(config.Configs["simlink"].GetString("evaluationgroup", "-1"));
    //        TriggerRT t = (TriggerRT)Enum.Parse(typeof(TriggerRT), dictArgs["trigger"]);
            int nTriggerMethod = (int)(TriggerRT)Enum.Parse(typeof(TriggerRT), dictArgs["trigger"]);
            string sConn = ""; SIM_API_LINKS.DAL.DB_Type dbType;
            bValid = simlink.HelperGetConn(config, out sConn, out dbType);    //// TODO: carry through that the simlink initialization is actually working.

            realtime rt = new realtime(nModelTypeID, nEvalID, nTriggerMethod, sConn, (int)dbType, dictArgs);

            return rt;          
        }
        // extract dictionary from the realtime section of a config
        // met 10/31/17: This step seems unnecesary - why not use config straight?
        private static  Dictionary<string, string> GetDictArgsFromConfig(IConfig config)
        {
            string start_timestamp = config.GetString("start_timestamp",System.DateTime.Now.ToString());
            string trigger = config.GetString("trigger", "manual_definestart");
            string offset = config.GetString("offset", "0");
            string duration = config.GetString("duration", "1440");
            string ts_interval = config.GetString("ts_interval", "3600");
            string skip_ts_check = config.GetString("skip_ts_check", "0");
            string defined_run_dates = config.GetString("defined_run_dates", "undefined");      //set to undefined unless you want
            string sStartScen = config.GetString("ScenStart", "-1"); //SP 10-Oct-2017
            string sEndScen = config.GetString("ScenEnd", "100"); //SP 10-Oct-2017
            Dictionary<string, string> dictArgs = new Dictionary<string, string>();
            dictArgs.Add("start_timestamp", start_timestamp);
            dictArgs.Add("trigger", trigger);
            dictArgs.Add("offset", offset);
            dictArgs.Add("duration", duration);
            dictArgs.Add("ts_interval", ts_interval);
            dictArgs.Add("skip_ts_check", skip_ts_check);
            dictArgs.Add("defined_run_dates", defined_run_dates);
            dictArgs.Add("ScenStart", sStartScen);      //mettodo: chnange to lowercase and add only as option? 
            dictArgs.Add("ScenEnd", sEndScen);
            string sloops = config.GetString("loops", "undefined");
            if (sloops!="undefined")
                dictArgs.Add("loops", sloops);   //only add if present
            dictArgs.Add("use_hotstart",config.GetString("use_hotstart", "n"));
            dictArgs.Add("hotstart_location",config.GetString("hotstart_location", "undefined"));
            dictArgs.Add("hotstart_filename_base", config.GetString("hotstart_filename_base", "undefined"));
            return dictArgs;
        }
    }
}
