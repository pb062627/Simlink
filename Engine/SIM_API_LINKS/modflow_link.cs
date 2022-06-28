using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;


namespace SIM_API_LINKS
{
    public class modflow_link : simlink
    {

        #region MEMBERS

        #endregion

        #region INIT
        public void InitializeModelLinkage(string sConnRMG, int nDB_Type, int nEvalID, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_MODFLOW;
            _sActiveModelLocation = sConnRMG;
            _nActiveEvalID = nEvalID;
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);

            Load_EG_Tables(nEvalID);
            INIT_LoadZoneBudgetNavigation();
        }

        //step 1: getting this working in hard-coded way (ultimate delivery to the client this way too
        // step 2: 
        protected void Load_EG_Tables(int nEvalID)
        {
            _dsEG_ResultTS_Request = GetDataset_ResultTS(nEvalID);
        }

        protected virtual DataSet GetDataset_ResultTS(int nEvalID)
        {
            DataTable dt = new DataTable(); DataSet ds = new DataSet();
            dt.Columns.Add("ResultTS_ID", typeof(int));
            dt.Columns.Add("VarResultType_FK", typeof(int));
            dt.Columns.Add("Result_Label", typeof(string));
            dt.Columns.Add("ElementID_FK", typeof(int));
            dt.Columns.Add("Element_Label", typeof(string));

            //hard-code the data we want back
            // important: ElementID- this is the ZONE number
            //this is not fully implemented on zone budget (or at least not tested).


            //return all data, unless overide in derived class
            dt.Rows.Add(762, 12, "Zone1: CONSTANT HEAD: IN", 1, "Zone1");
            dt.Rows.Add(763, 13, "Zone1: CONSTANT HEAD: OUT", 1, "Zone1");
            dt.Rows.Add(764, 14, "Zone1: Total IN", 1, "Zone1");
            dt.Rows.Add(765, 15, "Zone1: Total OUT", 1, "Zone1");
            dt.Rows.Add(766, 16, "Zone1: IN - OUT", 1, "Zone1");
            dt.Rows.Add(767, 17, "Zone1: Percent Discrepancy", 1, "Zone1");
            dt.Rows.Add(768, 18, "Zone1: STORAGE: IN", 1, "Zone1");
            dt.Rows.Add(769, 19, "Zone1: STORAGE: OUT", 1, "Zone1");
            dt.Rows.Add(770, 20, "Zone1: RIVER LEAKAGE:IN", 1, "Zone1");
            dt.Rows.Add(771, 21, "Zone1: HEAD DEP BOUNDS:IN", 1, "Zone1");
            dt.Rows.Add(772, 22, "Zone1: RECHARGE:IN", 1, "Zone1");
            dt.Rows.Add(773, 23, "Zone1: RIVER LEAKAGE:OUT", 1, "Zone1");
            dt.Rows.Add(774, 24, "Zone1: HEAD DEP BOUNDS:OUT", 1, "Zone1");
            dt.Rows.Add(775, 25, "Zone1: RECHARGE:OUT", 1, "Zone1");

            ds.Tables.Add(dt);
            return ds;
        }

        protected void INIT_LoadZoneBudgetNavigation()
        {
            _dictSL_TableNavigation.Add(12, new simlinkTableHelper(12, "n/a", -1, "CONSTANT HEAD_IN", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(13, new simlinkTableHelper(13, "n/a", -1, "CONSTANT HEAD_OUT", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(14, new simlinkTableHelper(14, "n/a", -1, "TOTAL IN_IN", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(15, new simlinkTableHelper(15, "n/a", -1, "TOTAL OUT_OUT", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(16, new simlinkTableHelper(16, "n/a", -1, "IN-OUT_TOTAL", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(17, new simlinkTableHelper(17, "n/a", -1, "PERCENT ERROR_TOTAL", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(18, new simlinkTableHelper(18, "n/a", -1, "STORAGE_IN", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(19, new simlinkTableHelper(19, "n/a", -1, "STORAGE_OUT", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(20, new simlinkTableHelper(20, "n/a", -1, "RIVER LEAKAGE_IN", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(21, new simlinkTableHelper(21, "n/a", -1, "HEAD DEP BOUNDS_IN", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(22, new simlinkTableHelper(22, "n/a", -1, "RECHARGE_IN", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(23, new simlinkTableHelper(23, "n/a", -1, "RIVER LEAKAGE_OUT", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(24, new simlinkTableHelper(24, "n/a", -1, "HEAD DEP BOUNDS_OUT", "ZONEBUDGET"));
            _dictSL_TableNavigation.Add(25, new simlinkTableHelper(25, "n/a", -1, "RECHARGE_OUT", "ZONEBUDGET"));
        }



        #endregion

        #region PROCESS_SCENARIO

        public string[] ProcessScenario_TEMPLATE(string[] sTemplateNames, string[] sTemplatVals, string sResultsFile, int nLoopNo = -1)
        {


            if (nLoopNo != -1)
                _nLoopNumber = nLoopNo;
            else
                _nLoopNumber++;
            //SP 25-Jul-2016 now can be read from config
            //string sLogPath = System.IO.Path.GetDirectoryName(_sActiveModelLocation);
            _log.Initialize("logEG_" + _nActiveEvalID.ToString() + "_" + _nLoopNumber.ToString(), _log._nLogging_ActiveLogLevel, _sLogLocation_Simlink);

            TEMPLATE_InsertMEV(sTemplateNames, sTemplatVals, _nLoopNumber);         //TODO: consider how this will work with 
            _log.AddString("Template var dict created.", Logging._nLogging_Level_2);
            ProcessScenario(_sActiveModelLocation, _nLoopNumber, CommonUtilities.nScenLCBaselineFileSetup, CommonUtilities.nScenLCModelResultsRead, "-1", sResultsFile);
            string[] sReturn = CreateStringFromResults(_nLoopNumber);
            ScenDS_ClearAfterScenario(_nLoopNumber);                             //remove no longer needed details

            _log.WriteLogFile();                            //file only written if >0 lines to be written
            return sReturn;

        }
        private string[] CreateStringFromResults(int nScenarioID)
        {
            var slDT_FilteredList = _lstSimLinkDetail
                 .Where(x => x._nScenarioID == nScenarioID)
                 .Where(x => x._slDataType == SimLinkDataType_Major.ResultTS);       //zbudget results are inserted in this fashion
            int nCount = slDT_FilteredList.Count();
            string[] sReturn = new string[nCount];
            int i = 0;
            foreach (var slDT in slDT_FilteredList)
            {
                sReturn[i] = slDT._sVal;
                i++;
            }
            return sReturn;
        }


        public int ProcessScenario(string sMOD_BAT, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1", string sResultFile = "NOTHING")       //, ref rmgDB_link)
        {
            string sPath; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName;
            int nCurrentLoc = nScenStartAct;

            try
            {
                if (nScenarioID != -1)     //we should have a valid ScenarioID at this point.
                {

                    //todo: break out into cu function with parameters for customization
                    sPath = System.IO.Path.GetDirectoryName(sMOD_BAT);
                    if (_bIsSimLinkLite)
                    {
                        sTargetPath = sPath;
                    }
                    else
                    {
                        sTargetPath = sPath.Substring(0, sPath.LastIndexOf("\\")) + "\\" + _nActiveEvalID.ToString() + "\\" + nScenarioID.ToString();

                    }

                    //SP 15-Feb-2017 ExtractExternalData for RetrieveCode = AUX
                    ScenarioGetExternalData();

                    bool bContinue = true;              //identifies whether to continue
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModElementExist) && (nScenEndAct >= CommonUtilities.nScenLCModElementExist) && bContinue)       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
                    {
                        if (sDNA != "-1")
                        {           //not an optimization run, no DNA is passed
                            nScenarioID = DistribDNAToScenario(sDNA, _nActiveEvalID, _nActiveReferenceEvalID, _nActiveProjID, _nActiveModelTypeID - 1, nScenarioID);
                            if (nScenarioID == -1) { bContinue = false; }       // some failure in the DNA distribution
                            else
                            {
                                nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;
                            }
                            //sim2.1 rmgDB_link.cu.cuLogging_UpdateFileOutName("logEG_" + nEvalID.ToString() + "_" + nScenarioID.ToString());
                        }
                        else
                        {
                            nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;          //
                        }

                        //SP 15-Jun-2016 no longer needed - tested with EPANET and IW
                        /*if (true)
                        {
                            _dbContext.OpenCloseDBConnection();
                        }*/
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineFileSetup) && (nScenEndAct >= CommonUtilities.nScenLCBaselineFileSetup))
                    {
                        //sim2.1 _log.AddString("MODFLOW File Setup Begin: " + System.DateTime.Now.ToString(), Logging._nLogging_Level_2);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        if (sPath != sTargetPath)
                            CommonUtilities.CopyDirectoryAndSubfolders(sPath, sTargetPath, true);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;          //
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineModified) && (nScenEndAct >= CommonUtilities.nScenLCBaselineModified))
                    {
                        _log.AddString("Begin baseline modify step", Logging._nLogging_Level_3);
                        //sim2.1 _log.AddString("MODFLOW File Update Begin: " + System.DateTime.Now.ToString(), Logging._nLogging_Level_2);      //log begin scenario step
                        //sim2.1:  already done in new function TEMPLATE_MEV    Dictionary<string, string> dict = modflowGetVarDict(nScenarioID);
                        string sArchive = "SKIP";
                        if (_bIsSimCondor)
                            sArchive = "files.7z";      //untested

                        _template.TEMPLATE_ProcessTemplateFiles(sTargetPath, _dictTemplate, ref _log, sArchive);                 //todo: grab the archive name from somewhere
                        nCurrentLoc = CommonUtilities.nScenLCBaselineModified;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        if (_bIsSimCondor) //run the MODFLOW job as a Condor job.
                        {               //note: 
                            /*sim2          htc = new CIRRUS_HTC_NS.CIRRUS_HTC();
                                      Dictionary<string, string> dictHTC_MODFLOW = new Dictionary<string, string>();
                                      dictHTC_MODFLOW.Add("transfer_input_files", "files.7z, 7za.exe");
                                      dictHTC_MODFLOW.Add("getenv", "true");

                                      if (false)       //option 1: Do NOT pas exe - look for requirement that it is installed.
                                      {               //todo: parameterize this
                                          /*                  //uncomment as soon as Condor instances updated                     dictHTC_MODFLOW.Add("requirements", "(MODFLOW_INSTALLED =?= True)");              //todo: consider how to suppor other versions of MODFLOW. maybe other versions must have exe passed
                                                            dictHTC_MODFLOW.Add("executable", "run_MODFLOW5.bat");
                                                            string[] s = new string[] { "MODFLOW5.exe " + System.IO.Path.GetFileName(sTarget_INP) + " " + System.IO.Path.GetFileName(sSummaryFile) + " " + System.IO.Path.GetFileName(sOUT) };
                                                            string sBat = System.IO.Path.Combine(sTargetPath, "run_MODFLOW5.bat");
                                                            File.WriteAllLines(sBat, s);                                 //output the .bat file to run as a MODFLOW file.               */
                            /*           }
                                       else
                                       {
                                           // this option works if you need to pass the exe
                                           dictHTC_MODFLOW.Add("executable", "model.bat");
                                           dictHTC_MODFLOW.Add("transfer_output_files", "results.7z");
                                       }
                                       htc.InitHTC_Vars(dictHTC_MODFLOW, sTargetPath + "\\", nActiveModelID_MODFLOW, true);
                                       htc.HTC_Submit();       */
                        }
                        else
                        {   //run within SimLink

                            CommonUtilities.RunBatchFile(sMOD_BAT);                         // cu.cuRunBatchFile(sMOD_BAT);
                        }
                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        //sim2.1 _log.AddString("MODFLOW Results Read Begin: " + System.DateTime.Now.ToString(), Logging._nLogging_Level_2);      //log begin scenario step
                        ReadZoneBudget(_nActiveEvalID, nScenarioID, sResultFile);
                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                        //?
                        nCurrentLoc = CommonUtilities.nScenLModelResultsTS_Read;
                    }
                }
                return nCurrentLoc;
            }

            catch (Exception ex)                //log the error
            {
                _log.AddString("MODFLOW Exception: " + ex.Message, Logging._nLogging_Level_1);
                //sim2.1 _log.AddString("MODFLOW Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return 0;   //TODO: refine based upon code succes met 6/12/2012
            }
        }
        #endregion

        #region RESULTS

        #region ZONE_BUDGET
        private void ReadZoneBudget(int nEvalID, int nScenarioID, string sZBLFile, int nTimestepPerPeriod = -1)
        {
            if (File.Exists(sZBLFile))
            {
                StreamReader zblFile = new StreamReader(sZBLFile);
                if (nTimestepPerPeriod < 0) { nTimestepPerPeriod = -1; }         // sim2.1 this helper function was not implemented    zblHELP_CountStressPeriods(ref zblFile); }        //if the number of stress periods is not passed as arg, we need to find the highest one in the file. may not be wise, becaue there could be more that aren't output?
                try
                {
                    string sbuf = "";
                    string[] sbufVALS;
                    sbuf = zblFile.ReadLine();
                    var dictFields = zblHELP_GetDictionary(nEvalID, sbuf);
                    int nZone = -1; int nTimesStep = -1; int nResultTSID = -1; int nCol = -1; double dVal = -1.0;

                    while (!zblFile.EndOfStream)
                    {
                        DataSet dsTS = new DataSet();
                        sbuf = zblFile.ReadLine();
                        sbufVALS = sbuf.Split(',');
                        nZone = Convert.ToInt32(sbufVALS[3]);               //zone is used to grab the right dictionary (one per zone, because each links to a different "results" record)
                        nTimesStep = (Convert.ToInt32(sbufVALS[1]) - 1) * nTimestepPerPeriod + Convert.ToInt32(sbufVALS[2]);               //  (CurrentPeriod-1)*Timesteps + Timesteps
                        int nRowInsertCounter = 0;
                        foreach (var entry in dictFields[nZone])
                        {           //each entry prescribed by the Results info in the SimLink db.
                            nCol = entry.Key;
                            nResultTSID = entry.Value;
                            dVal = Convert.ToDouble(sbufVALS[nCol]);         //grab the value based on the key of the dictionary for the specific zone
                            _lstSimLinkDetail.Add(new simlinkDetail(SimLinkDataType_Major.ResultTS, sbufVALS[nCol], nResultTSID, -1, nScenarioID));
                        }
                    }
                    double[] dReturnVars;
                }
                finally
                {
                    if (zblFile != null)
                        zblFile.Close();
                }
            }
        }

        //one of the complications of the ZBL field type is that different results types have the same column headings
        // this is a one time pass through to get the right indices for the field types (we then iterate over only these)
        private Dictionary<string, int[]> zblHELP_getInOutTotal(string sbufHeader)          //int[]
        {
            int[] nArray = new int[] { -1, -1 };
            string[] sColumnHeads = sbufHeader.Split(',');

            string[] sArray = new string[] { "IN", "Out" };
            int nIndex = 0;
            for (int i = 0; i < sColumnHeads.Count(); i++)
            {
                if (sColumnHeads[i].IndexOf(sArray[nIndex]) > 0)           //if we find the search target
                {
                    nArray[nIndex] = i;                                 //columns indexed from 1
                    nIndex++;
                    if (nIndex == 2)
                    {
                        break;      //only two values we're looking for;  this will be our general exit.
                    }
                }

            }
            //create a dictionary
            var dictFields = new Dictionary<string, int[]>();
            int[] nIN = new int[] { 4, -1 };         //nArray[0]];
            int[] nOUT = new int[] { -1, -1 };
            int[] nTotal = new int[] { -1, -1 };
            nIN[1] = nArray[0];
            nOUT[0] = nArray[0] + 1;
            nOUT[1] = nArray[1];
            nTotal[0] = nArray[1] + 1;
            nTotal[1] = sColumnHeads.Count();


            dictFields.Add("IN", nIN);         //  [4,nArray[0]]); 
            dictFields.Add("OUT", nOUT);
            dictFields.Add("TOTAL", nTotal);
            return dictFields;
        }
        //returns a dictionary of column headings / ResultTSID
        private Dictionary<int, Dictionary<int, int>> zblHELP_GetDictionary(int nEvaldID, string sbufHeader)
        {
            var dictFields = new Dictionary<int, Dictionary<int, int>>();

            string[] sColumnHeads = sbufHeader.Split(',');
            var colLimitDICT = zblHELP_getInOutTotal(sbufHeader);
            int nActiveZone = -1; bool bFirstPass = true;                           // wee need a dictionary for each zone  (this allows the code to run fast, and more flexibility in future; alternaive is to just import more stuff.
            if (_dsEG_ResultTS_Request.Tables[0].Rows.Count > 0)            // check for at least one row
            {

                var zoneDict = new Dictionary<int, int>();                                      //insert the new dictionary for this zone
                foreach (DataRow drResults in _dsEG_ResultTS_Request.Tables[0].Rows)
                {

                    if (Convert.ToInt32(drResults["ElementID_FK"].ToString()) != nActiveZone)       //sim2.1  not fully tested
                    {
                        if (bFirstPass) { bFirstPass = false; }
                        else
                        {               //we have a zone dictionary ready for insert
                            dictFields.Add(nActiveZone, zoneDict.ToDictionary(entry => entry.Key,
                                                                                entry => entry.Value));
                        }
                        nActiveZone = Convert.ToInt32(drResults["ElementID_FK"].ToString());
                        zoneDict.Clear();         //insert the new dictionary for this zone
                    }

                    int nVarResultType_FK = Convert.ToInt32(drResults["VarResultType_FK"].ToString());
                    string sResultFieldName;
                    string sQualifier = zblHELP_GetQualifier(nVarResultType_FK, out sResultFieldName);                            //retrieve whether in/outtotal from result label
                    int[] nColLimits = colLimitDICT[sQualifier];         //gets in/out/total for the row which defines which columns we will loop over

                    for (int i = nColLimits[0]; i <= nColLimits[1]; i++)
                    {
                        string sColTargetName = sColumnHeads[i].Trim().ToUpper() + "_" + sQualifier;
                        int nResultID = Convert.ToInt32(drResults["ResultTS_ID"].ToString());          //GetResultDictIDbyString(sColName);
                        //met 12/13/13: the _dsResults dataset no longer contains the field name
                        //zonebudget has unusual case where we don't have resultID, but we read the FieldName- so need to get the ResultID back.

                        if (sResultFieldName == sColTargetName)         //nResultID > 0)       //the column heading equals the FieldName (not alias) in the RMG DB
                        {
                            Console.WriteLine(sResultFieldName);
                            zoneDict.Add(i, nResultID);
                            break;      //exit loop
                        }
                    }

                }
                dictFields.Add(nActiveZone, zoneDict);

            }
            return dictFields;

        }

        //todo: handle case where id not in dictionary
        //note CONVENTION: the result label must include underscore _IN, _OUT , _TOTAL- 
        private string zblHELP_GetQualifier(int nVarResultType_FK, out string sFieldName)
        {
            string sReturn = "IN";
            sFieldName = "NOTHING";
            simlinkTableHelper sl = _dictSL_TableNavigation[nVarResultType_FK];
            string sResult = sl._sFieldName;
            int nIndex = sResult.LastIndexOf('_');
            if (nIndex > 0)
            {
                sReturn = sResult.Substring(nIndex + 1);
                sFieldName = sl._sFieldName;
            }
            return sReturn;
        }

        private string zblHELP_GetFieldName(int nVarResultType_FK)
        {
            return _dictSL_TableNavigation[nVarResultType_FK]._sFieldName;
        }




        //gets the ResultTS_ID from the dict having xyz fieldname
        //todo: figure out clever way to do this in linq 
        private int GetResultDictIDbyString(string sColumnName)
        {
            int nReturn = -1;       //indicates 
            foreach (KeyValuePair<int, simlinkTableHelper> pair in _dictSL_TableNavigation)
            {
                if (pair.Value._sFieldName == sColumnName)
                {
                    nReturn = pair.Value._nVarType_FK;
                }
            }
            return nReturn;
        }



        #endregion




        #endregion



        #region HELPERS             //eg BATWRAP
        private string modflowWriteBatchFile(string sModelBat)
        {
            try
            {
                string sPath = System.IO.Path.GetDirectoryName(sModelBat);
                string sFileToRun = System.IO.Path.GetFileName(sModelBat);
                string sBat_Write = sPath + "\\" + "BAT_WRAP.bat";
                System.IO.StreamWriter fileBatchOut = new System.IO.StreamWriter(sBat_Write);
                fileBatchOut.WriteLine("CD " + sPath);          //move to the proper directory
                fileBatchOut.WriteLine(sFileToRun);          //move to the proper directory
                fileBatchOut.WriteLine("ECHO DONE");
                fileBatchOut.Close();
                return sBat_Write;                              //return the string file name
            }

            catch (Exception ex)
            {
                //sim2.1 cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return "FAIL";
            }
        }

        #endregion



    }
}
