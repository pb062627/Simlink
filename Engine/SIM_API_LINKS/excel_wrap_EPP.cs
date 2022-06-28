using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;    
using  Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace SIM_API_LINKS
{
    public class excel_link : simlink
    {
        Excel.Application xlApp = new Excel.Application();
        Excel.Workbook xlWorkbook;
        Excel._Worksheet xlWorksheet;
        ExcelPackage _xlPackage;
        ExcelWorksheet _xlWkshtActive;
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
         //   LoadScenarioDatasets(_nActiveBaselineScenarioID, 100, true);       //Load any datasets for the baseline, if applicable

        }




        public void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_Excel;
            _sTS_FileNameRoot = "EXCEL_TS.h5";
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
            //moved to initEG becaueys this is EG specific         if (true)
            //                    PopulateLID_UsageDictionary();                      //   maybe todo: consider loading only if it will be needed..
        }

        public void OpenFile(string sFileName)
        {
            FileInfo newFile = new FileInfo(sFileName);
            _xlPackage = new ExcelPackage(newFile);

        }

        public void SetValue(string sWorksheetName, int nRow, int nCol, string sVal)
        {
            
        }


        public void SetValueEPP(string sWorksheetName, int nRow, int nCol, string sVal)
        {
            _xlWkshtActive = _xlPackage.Workbook.Worksheets[sWorksheetName];
            _xlWkshtActive.Cells[nRow, nCol].Value = sVal;
        }

        public string GetValue(string sWorksheetName, int nRow, int nCol)
        {
            _xlWkshtActive = _xlPackage.Workbook.Worksheets[sWorksheetName];
            string sReturn = Convert.ToString(_xlWkshtActive.Cells[nRow, nCol].Value);
            return sReturn;
        }
        public void Close()
        {
            _xlPackage.Dispose();
        }

        public void Save()
        {
            _xlPackage.Save();
        }

        public void SaveAs(string sFileName)
        {
             FileInfo newFile = new FileInfo(sFileName);
             _xlPackage.SaveAs(newFile);
        }

        #endregion

        #region ExcelWrap


        #endregion

        private void UpdateExcelWorkbook()
        {


        }
        #region RunProcessing
        public override void ProcessEvaluationGroup()
        {
            DataSet dsEvals = ProcessEG_GetGS_Initialize(_nActiveEvalID);       //, nRefScenarioID);
            //now performed in init... LoadReferenceDatasets();            //initialize datasets

            string sLogPath = System.IO.Path.GetDirectoryName(_sActiveModelLocation);
            sLogPath = sLogPath.Substring(0, sLogPath.LastIndexOf("\\")) + "\\LOGS";

            foreach (DataRow dr in dsEvals.Tables[0].Rows)
            {
                _log.Initialize("logEG_" + _nActiveEvalID.ToString() + "_" + dr["ScenarioID"].ToString(), _log._nLogging_ActiveLogLevel, sLogPath);   //begin a logging session
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
        }
     public int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINP_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1")
        {
            string sPath; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName; string sINIFile; string sSummaryFile; string sOUT;
            int nCurrentLoc = nScenStartAct; string sTS_Filename = "";

            try
            {
                if (nScenarioID != -1)     //we should have a valid ScenarioID at this point.
                {
                   
                    _hdf5._sHDF_FileName = sTS_Filename;    //met 1/16/14 - sl object should know it's repository
                    LoadScenarioDatasets(nScenarioID, nScenStartAct);                       //, sTS_Filename);           //loads datasets needed for the scenario if not starting from beginning (in which case ds are constructed through process);

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


                        if (_dbContext._DBContext_DBTYPE==0)      //if access
                        {
                            _dbContext.OpenCloseDBConnection();
                        }
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
                        _log.AddString("SWMM File Update Begin: " + System.DateTime.Now.ToString(), Logging._nLogging_Level_2);      //log begin scenario step
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
                    ScenDS_ClearAfterScenario(nScenarioID);
                    if (_slXMODEL != null)
                    {
      
                    }
                                                    //clear scenario following execute
                    UpdateScenarioStamp(nScenarioID, nCurrentLoc);                 //store the time the scenario is completed, along with the "stage" of the Life Cycle
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
