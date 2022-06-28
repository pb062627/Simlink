using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;    
using  Microsoft.Office.Interop.Excel;

namespace SIM_API_LINKS
{
    public class excel_link : simlink
    {
        private Application _app = null;
        private Workbook _workbook = null;
        private Worksheet _wksht = null;
        private string _filename = null;  

        #region INIT
        public void InitializeEG(int nEvalID)
        {
            base.InitializeEG(nEvalID);
            nEvalID = GetReferenceEvalID();                                         //get correct EG for loading datasets
      //      _dsEG_ResultSummary_Request = LoadResultSummaryDS(nEvalID);
       //     _dsEG_ResultTS_Request = ReadOut_GetDataSet(nEvalID);
        //    _dsEG_ResultSummary_Request.Tables[0].Columns["val"].ReadOnly = false;                  //used to store vals
            base.LoadReference_EG_Datasets();               //  must follow _dsEG_ResultTS_Request
            SetTSDetails();                                 // load simulation/reporting timesereis information
            LoadAndInitDV_TS();                             //load any reference TS information needed for DV and/or tblElementXREF

            //SP 15-Feb-2017 - Extract External Data at EG level for AUXEG retrieve codes
            EGGetExternalData();

            //   LoadScenarioDatasets(_nActiveBaselineScenarioID, 100, true);       //Load any datasets for the baseline, if applicable

        }

        public Worksheet ActiveWorksheet
        {
            get { return _workbook.ActiveSheet as Worksheet; }
        }
        public void excel_InitWorkbook(string filename)
        {
            _app = new Application();
            _app.Visible = false;
            _filename = filename;
            _workbook = _app.Workbooks.Open(filename);
        }  


        public void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_Excel;
            _sTS_FileNameRoot = "EXCEL_TS.h5";
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
            //moved to initEG becaueys this is EG specific         if (true)
            //                    PopulateLID_UsageDictionary();                      //   maybe todo: consider loading only if it will be needed..
        }

        public void SetValue(string sWorksheetName, int nRow, int nCol, string sVal)
        {
            _wksht = _workbook.Worksheets[sWorksheetName];
            _wksht.Cells[nRow, nCol].Value = sVal;
        }

        public string GetValue(string sWorksheetName, int nRow, int nCol)
        {
            _wksht = _workbook.Worksheets[sWorksheetName];
            string sReturn = Convert.ToString(_wksht.Cells[nRow, nCol].Value);
            return sReturn;
        }
        public void Close()
        {
            // nothing to do if there's no app object  
            if (_app == null) return;
            // close app and wait for finalization  
            _app.Quit();
            _app = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();  
        }

        public void Save()
        {
            _workbook.Save();  
        }

        public void SaveAs(string sFileName)
        {
            _workbook.SaveAs(sFileName);  
            //  FileInfo newFile = new FileInfo(sFileName);
          //   _xlPackage.SaveAs(newFile);
        }

        #endregion

        #region ExcelWrap


        #endregion

        private void UpdateExcelWorkbook()
        {


        }
        #region RunProcessing
        //SP 5-Aug-2016 Can now use the virtual procedure in SimlinkScenario.cs - commented out to avoid making changes to this specific class when can now be using virtual class
        /*public override void ProcessEvaluationGroup(string[] astrScenario2Run)
        {
            DataSet dsEvals = ProcessEG_GetGS_Initialize(_nActiveEvalID, astrScenario2Run);       //, nRefScenarioID);
            //now performed in init... LoadReferenceDatasets();            //initialize datasets

            //SP 25-Jul-2016 now can be read from config
            //string sLogPath = System.IO.Path.GetDirectoryName(_sActiveModelLocation);
            //sLogPath = sLogPath.Substring(0, sLogPath.LastIndexOf("\\")) + "\\LOGS";

            foreach (DataRow dr in dsEvals.Tables[0].Rows)
            {
                _log.Initialize("logEG_" + _nActiveEvalID.ToString() + "_" + dr["ScenarioID"].ToString(), _log._nLogging_ActiveLogLevel, _sLogLocation_Simlink);   //begin a logging session
                int nProjID = Convert.ToInt32(dsEvals.Tables[0].Rows[0]["ProjID"].ToString());
                string sFileLocation = dsEvals.Tables[0].Rows[0]["ModelFile_Location"].ToString();         //todo: proj/sFile should only be set once; minor time penalty so no issue
                int nScenarioID = Convert.ToInt32(dr["ScenarioID"].ToString());
                int nScenStart = Convert.ToInt32(dr["ScenStart"].ToString());
                int nScenEnd = Convert.ToInt32(dr["ScenEnd"].ToString());
                string sDNA = dr["DNA"].ToString();

                try
                {
                    ProcessScenario(nProjID, _nActiveEvalID, _nActiveReferenceEvalID, sFileLocation, nScenarioID, nScenStart, nScenEnd, sDNA);

                }
                catch (Exception ex)
                {
                    //todo: log the issue
                }

            }
        }*/

     public int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINP_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1")
        {
            string sPath; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName; string sINIFile; string sSummaryFile; string sOUT;
            int nCurrentLoc = nScenStartAct; string sTS_Filename = "";

            ScenDS_ClearAfterScenario(nScenarioID); //SP 9-Mar-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

            try
            {
                if (nScenarioID != -1)     //we should have a valid ScenarioID at this point.
                {
                   
                    _hdf5._sHDF_FileName = sTS_Filename;    //met 1/16/14 - sl object should know it's repository
                    LoadScenarioDatasets(nScenarioID, nScenStartAct);                       //, sTS_Filename);           //loads datasets needed for the scenario if not starting from beginning (in which case ds are constructed through process);

                    //SP 15-Feb-2017 ExtractExternalData for RetrieveCode = AUX
                    ScenarioGetExternalData();

                    sPath = System.IO.Path.GetDirectoryName(sINP_File);
                    bool bContinue = true;              //identifies whether to continue
                    if ((nCurrentLoc <=CommonUtilities.nScenLCModElementExist) && (nScenEndAct >=CommonUtilities.nScenLCModElementExist) && bContinue)       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
                    {
                        if (sDNA != "-1")
                        {           //not an optimization run, no DNA is passed
                            nScenarioID = DistribDNAToScenario(sDNA, nEvalID, nReferenceEvalID, nProjID, 1, -1, nScenarioID);
                            if (nScenarioID == -1) { bContinue = false; }       // some failure in the DNA distribution
                            else
                            {
                                nCurrentLoc =CommonUtilities.nScenLCModElementExist;
                            }
           
                                _log.UpdateFileOutName("logEG_" + nEvalID.ToString() + "_" + nScenarioID.ToString());
                        }
                        else
                        {
                            nCurrentLoc = CommonUtilities.nScenLCModElementExist;          //
                        }

                        //SP 15-Jun-2016 no longer needed - tested with EPANET and IW
                        /*if (_dbContext._DBContext_DBTYPE==0)      //if access
                        {
                            _dbContext.OpenCloseDBConnection(); 
                        }*/
                    }

                    if ((nCurrentLoc <=CommonUtilities.nScenLCBaselineFileSetup) && (nScenEndAct >=CommonUtilities.nScenLCBaselineFileSetup))
                    {
                /*        _log.AddString("EPANET File Setup Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        CommonUtilities.CopyDirectoryAndSubfolders(sPath, sTargetPath, true);
                        nCurrentLoc =CommonUtilities.nScenLCBaselineFileSetup;          //*/
                    }

                    //XMODEL: consider adding handler for this?
                    //lots of potential for complexity in this... first cut keep simple
                    if (_slXMODEL != null)
                    {
                        ExecuteLinkedSimLinkPrecursor();        //check and evaluate any linked runs...
                        XMODEL_ProcessLinkedRecords(nScenarioID);                //primary data linkage
                        XMODEL_PlatformSpecificFollowup(nScenarioID);
                        //now, must write out the TS  (do for raingage, ET, LEVEL
                    }

                    if ((nCurrentLoc <=CommonUtilities.nScenLCBaselineModified) && (nScenEndAct >=CommonUtilities.nScenLCBaselineModified))
                    {
                        _log.AddString("SWMM File Update Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        UpdateExcelWorkbook();                  
                        nCurrentLoc =CommonUtilities.nScenLCBaselineModified;
                    }
                    if ((nCurrentLoc <=CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >=CommonUtilities.nScenLCModelExecuted))
                    {
                        //todo: calculate if manual
                        nCurrentLoc =CommonUtilities.nScenLCModelExecuted;
                    }

                    if ((nCurrentLoc <=CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >=CommonUtilities.nScenLCModelResultsRead))
                  {
                        _log.AddString("SWMM Results Read Begin: " , Logging._nLogging_Level_2);      //log begin scenario step
                 //       ResultsReadReport(sSummaryFile, nReferenceEvalID, nScenarioID);
                        nCurrentLoc =CommonUtilities.nScenLCModelResultsRead;
                    }
                    if ((nCurrentLoc <=CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >=CommonUtilities.nScenLModelResultsTS_Read))
                    {
                        //todo: implement based on Excel
                        nCurrentLoc =CommonUtilities.nScenLModelResultsTS_Read;
                    }

                    ProcessScenario_COMMON(nReferenceEvalID, nScenarioID, nCurrentLoc, nScenEndAct, sTS_Filename);        //call base function to perform modeltype independent actions
                    
                    if (_slXMODEL != null)
                    {
      
                    }
                                                    //clear scenario following execute
                    UpdateScenarioStamp(nScenarioID, nCurrentLoc);                 //store the time the scenario is completed, along with the "stage" of the Life Cycle
                    //SP 14-Jun-2016 Collated all routines writing back to the DB to the end of the ProcessScenario routine
                    //SP 10-Jun-2016 TODO Ideally want to shift writing / reading from the database out of this routine - initial step, integrate into one routine at the end of process scenario
                    WriteResultsToDB(nScenarioID);

                    //SP 14-Jun-2016 moved this to after writing back to the DB //18-Jul-2016 moved again!! clear at the start of the routine - _dsSCEN_PerformanceDetails is needed to obtain the objective value for the optimization
                    //ScenDS_ClearAfterScenario(nScenarioID);

                    _log.WriteLogFile();                            //file only written if >0 lines to be written
                }
                return nCurrentLoc;
            }

            catch (Exception ex)                //log the error
            {
                _log.AddString("Exception: " + ex.Message + " : ", Logging._nLogging_Level_1);
                return 0;   //TODO: refine based upon code succes met 6/12/2012
            }
        }

        #endregion

    }
}
