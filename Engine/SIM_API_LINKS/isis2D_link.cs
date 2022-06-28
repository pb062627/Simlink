using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;

namespace SIM_API_LINKS
{
    public class isis_2DLink : simlink
    {

        public bool bIsISIS_FAST = false;       //set to true if ISIS fast is the model..

        #region INIT
        public override void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_ISIS2D;
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
        }

        public void InitializeEG(int nEvalID)
        {
            base.InitializeEG(nEvalID);
            nEvalID = GetReferenceEvalID();                                         //get correct EG for loading datasets
    //sim2 ResultsRead not implemented?        _dsEG_ResultSummary_Request = LoadResultSummaryDS(nEvalID);
            //sim2 ResultsRead not implemented?           _dsEG_ResultTS_Request = ReadOut_GetDataSet(nEvalID);
            //sim2 ResultsRead not implemented?          _dsEG_ResultSummary_Request.Tables[0].Columns["val"].ReadOnly = false;                  //used to store vals
            base.LoadReference_EG_Datasets();               //  must follow _dsEG_ResultTS_Request
            SetTSDetails();                                 // load simulation/reporting timesereis information
            LoadAndInitDV_TS();                             //load any reference TS information needed for DV and/or tblElementXREF

            //SP 15-Feb-2017 - Extract External Data at EG level for AUXEG retrieve codes
            EGGetExternalData();
        }

        private void SetTSDetails()
        {

            if (true)
            {
                DateTime dtSim = DateTime.Parse("1/1/2000");
                DateTime dtRPT = DateTime.Parse("1/1/2000");
            //    int nSecInterval = TimeSeries.CONVERT_GetSecFromHHMM(ds.Tables[0].Rows[0]["REPORT_STEP"].ToString());
                _tsdResultTS_SIM = new TimeSeries.TimeSeriesDetail(dtRPT, IntervalType.Minute, 15);
                _tsdSimDetails = new TimeSeries.TimeSeriesDetail(dtSim, IntervalType.Minute, 15);
            }

        }

        #endregion

        #region SimLink     //get model changes, stuff like that



        private DataSet isis2D_GetModelChanges(int nScenarioID)
        {
            string sql = "SELECT tlkpISIS_2D_TableDictionary.ParentNode, tlkpISIS_2D_TableDictionary.Node, tlkpISIS_2D_FieldDictionary.FieldName, tlkpISIS_2D_TableDictionary.NodeLevel, tlkpISIS_2D_FieldDictionary.FieldTypeID,"
                        + " tlkpISIS_2D_FieldDictionary.IsAttributeNode, tblModElementVals.ScenarioID_FK, tblModElementVals.ElementName, tblModElementVals.ElementID, tblModElementVals.element_note, DV_ID_FK, tlkpISIS_2D_FieldDictionary.ID as VarType_FK"                    //, tblModElementVals.DomainQual"
                        +" FROM (tblModElementVals INNER JOIN tlkpISIS_2D_FieldDictionary ON tblModElementVals.TableFieldKey_FK=tlkpISIS_2D_FieldDictionary.ID) INNER JOIN tlkpISIS_2D_TableDictionary ON tlkpISIS_2D_FieldDictionary.TableID_FK=tlkpISIS_2D_TableDictionary.ID"
                        +" WHERE (((ScenarioID_FK)="+ nScenarioID + "));";
            
      //      string sql = "SELECT ParentNode, Node, DV_ID_FK, FieldName, NodeLevel, TableFieldKey_FK, FieldTypeID, val, ElementName, DomainQual,ElementID, ScenarioID_FK, IsAttributeNode"
        //                    +" FROM qryRMG001_ISIS2D_LinkModelChanges WHERE (((ScenarioID_FK)="+ nScenarioID + "));";
            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        public override void ProcessEvaluationGroup(string[] astrScenario2Run)
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
                    double dVal = testing.DoubleMyVal(4.3);

                    //SP 5-Aug-2016 TODO Can we now use the virtual procedure in SimlinkScenario.cs? this function call name is different to the others. Can probably be changed
                    int nID = ProcessScenario_FIX(nScenarioID, nScenStart, nScenEnd, sDNA);
                }
                catch (Exception ex)
                {
                    //todo: log the issue
                }
            }
        }
        
      //met- significant effort to call this function through polymorphism, however failed.. causing memory access exception
        //for now, call _FIX
        //todo: implement processeval on simlink.cs and call this through poly
        public int ProcessScenario_FIX(int nScenarioID, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1")
        {
            string sPath; string sTargetPath; string sTargetINP; string sIncomingINP; string sTargetINP_FileName;
            int nCurrentLoc = nScenStartAct;

            try
            {
                if (nScenarioID != -1)     //we should have a valid ScenarioID at this point.
                {
                   // int nEvalID = GetReferenceEvalID();
                    //todo: break out into cu function with parameters for customization
                    sPath = System.IO.Path.GetDirectoryName(_sActiveModelLocation);
                    sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, _nActiveEvalID, true);
                    sTargetINP = sTargetPath + "\\" + CommonUtilities.GetSimLinkFileName(_sActiveModelLocation, nScenarioID);
                    sIncomingINP = System.IO.Path.Combine(sTargetPath, System.IO.Path.GetFileName(_sActiveModelLocation));
			
                    bool bContinue = true;              //identifies whether to continue
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModElementExist) && (nScenEndAct >= CommonUtilities.nScenLCModElementExist) && bContinue)       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
                    {
                        if (sDNA != "-1")
                        {           //not an optimization run, no DNA is passed
                            nScenarioID = DistribDNAToScenario(sDNA, _nActiveEvalID, _nActiveReferenceEvalID, _nActiveProjID, 1, -1, nScenarioID);
                            if (nScenarioID == -1) { bContinue = false; }       // some failure in the DNA distribution
                            else
                            {
                                nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup; 
                            }
                            _log.UpdateFileOutName("logEG_" + _nActiveEvalID.ToString() + "_" + nScenarioID.ToString());
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
                        _log.AddString("ISIS2D File Setup Begin: " + System.DateTime.Now.ToString(), Logging._nLogging_Level_2);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        CommonUtilities.CopyDirectoryAndSubfolders(sPath, sTargetPath, true);
                    //    File.Move(sIncomingINP, sTargetINP);           //
                        nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;          //
                    }

                    if (_slXMODEL != null)
                    {
                        ExecuteLinkedSimLinkPrecursor();        //check and evaluate any linked runs...
                        XMODEL_ProcessLinkedRecords(nScenarioID);                //primary data linkage
                        XMODEL_PlatformSpecificFollowup(nScenarioID);
                        //now, must write out the TS  (do for raingage, ET, LEVEL
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineModified) && (nScenEndAct >= CommonUtilities.nScenLCBaselineModified))
                    {
                        _log.AddString("SWMM File Update Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                      //  string sIncomingFile = Path.GetDirectoryName(sTargetINP) + "\\" + Path.GetFileName()
                        if(File.Exists(sTargetINP)){File.Delete(sTargetINP);}
                        UpdateBaselineModel(_nActiveEvalID, nScenarioID, sIncomingINP);
                        File.Move(sIncomingINP, sTargetINP);
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        ExecuteISIS2D_or_FAST(sTargetINP, sTargetPath);                //run the model
                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                    }
                }
                return nCurrentLoc;  
            }
                
            catch (Exception ex)                //log the error
            {
                _log.AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), Logging._nLogging_Level_1);
                return 0;   //TODO: refine based upon code succes met 6/12/2012
            }
        }
        
   private void ExecuteISIS2D_or_FAST(string sTarget_INP, string sTargetPath)
   {
        bool bIsUNC = false; string sISIS2D_EXE = ""; string sBAT=""; string sBAT_Path="";string sEXE_Name ="NOTHING";
        if (sTarget_INP.Substring(0, 2) == @"\\") { bIsUNC = true; }         //explicit paths required if UNC

        if (bIsISIS_FAST)
            sEXE_Name = "isis2d";
        else
            sEXE_Name = "fast";

			string sXML_ModelFilePath = "";			//store whether we are referencing full path or not
			if (bIsUNC)
                        {
                            sXML_ModelFilePath = sTarget_INP;
                            sBAT = System.IO.Path.GetDirectoryName(sTarget_INP) + "\\" + "run_isis2D_FAST.bat";
                        }
                        else{			//local system
                            sXML_ModelFilePath = System.IO.Path.GetFileName(sTarget_INP);
                            sBAT = "run_isis2D_FAST.bat";
                        }
                         sISIS2D_EXE = sEXE_Name+ " " + sXML_ModelFilePath;
                        //create batch file information for running the program
                        string[] s = new string[] { sISIS2D_EXE };
                        sBAT_Path = System.IO.Path.Combine(sTargetPath, sBAT);
                        
                        if (_bIsSimCondor) //run the SWMM job as a Condor job.
                        {               //note: 
                            _htc = new CIRRUS_HTC_NS.CIRRUS_HTC();
                            Dictionary<string, string> dictHTC_ISIS = new Dictionary<string, string>();
                            dictHTC_ISIS.Add("transfer_input_files", System.IO.Path.GetFileName(sTarget_INP));

                            if (true)       //option 1: Do NOT pas exe - look for requirement that it is installed.
                            {               //todo: parameterize this
                                //uncomment as soon as Condor instances updated                     dictHTC_ISIS.Add("requirements", "(SWMM_INSTALLED =?= True)");              //todo: consider how to suppor other versions of swmm. maybe other versions must have exe passed
                                
                                
                                dictHTC_ISIS.Add("executable", sBAT);
                                File.WriteAllLines(sBAT_Path, s);                                 //output the .bat file to run as a SWMM file.
                            }
                            else
                            {
                                dictHTC_ISIS.Add("executable", sEXE_Name);
                                dictHTC_ISIS.Add("arguments", sXML_ModelFilePath);
                            }
                        //bojangles    _htc.InitHTC_Vars(dictHTC_ISIS, sTargetPath + "\\", _nActiveModelTypeID, true);
                            _htc.SubmitCondorJob();
                        }
                        else
                        {   //run within SimLink
                            s[0] = "cd %~dp0 \r\n" + s[0];
                            File.WriteAllLines(sBAT_Path, s);
                            CommonUtilities.RunBatchFile(sBAT_Path);
                        }
   }

        #endregion


        #region XML_Support

        public string UpdateBaselineModel(int nEvalID, int nScenarioID, string sTargetXML)
        {
            string sReturn = "provide some feedback";
            string sOutputXML = sTargetXML;                      // Path.GetDirectoryName(sTargetXML) + Path.GetFileNameWithoutExtension(sTargetXML) + nScenarioID.ToString() + ".xml";       //create a copy of the input XML and place it somewhere
            DataTable dtChanges = isis2D_GetModelChanges(nScenarioID).Tables[0];    //todo: implement IENUMERABLE similar to swmm.

            XDocument xdoc = XDocument.Load(sOutputXML); // load document

            //leng- you'll need to define how to handle this    XElement d = XElement.Load(sTargetXML);        //use whatever approach is most appropriate here..
            //   XElement dCreateNewXML = XElement.Load(sOutputXML) ;        //use whatever approach is most appropriate here.
            DataRow[] rows = dtChanges.Select("FieldName<> 'VALUE'"); // detect for non-recordset
            XNamespace ns = "http://www.halcrow.com/ISIS2D";
            string sNodeName = "", sParentNode = "", sDomain = "", sElementName = "", sFieldName = "";
            foreach (DataRow drModelChange in rows)
            {
                sNodeName = drModelChange["Node"].ToString();
                sParentNode = drModelChange["ParentNode"].ToString();
                sDomain = GetDomainOrShapeFromConcat(drModelChange["element_note"].ToString(), true) ;
                sFieldName = drModelChange["FieldName"].ToString();
                bool blnIsAttributeNode = bool.Parse(drModelChange["IsAttributeNode"].ToString());
                int nTS_Period = -1;
                if (drModelChange["ElementID"].ToString() != null) { nTS_Period = Convert.ToInt32(drModelChange["ElementID"].ToString()); }      //element id is normally used to link to a databse value; may want to change this
                string sVal = drModelChange["val"].ToString();

                // check for values to update element
                var nodes = from c in xdoc.Root.Descendants(ns + "domain")
                            where c.Attribute("domain_id").Value.Contains(sDomain)
                            select c;
                if (blnIsAttributeNode == false)
                {
                    nodes.Elements(ns + sNodeName).Elements(ns + sFieldName).FirstOrDefault().SetValue(sVal);
                }
                else
                {
                    if (sFieldName.ToLower() == "units")
                    {
                        nodes.Elements(ns + sParentNode).Elements(ns + sNodeName).Elements(ns + "value").FirstOrDefault().SetAttributeValue(sFieldName, sVal); // set attribute value
                    }
                    else
                    {
                        nodes.Elements(ns + sNodeName).FirstOrDefault().SetAttributeValue(sFieldName, sVal); // set attribute value
                    }
                }
            }
            rows = dtChanges.Select("FieldName='VALUE'"); // get all time-series and write in one go
            string strTempDomainID = ""; string strValue = ""; int intCounter = 0;
            
            //met 8/7/2013: Change this because TS vals may not be stored in data table, but in HDF holder
            //would be best if this works with HDF or tblMEV 
            
            hdf5_wrap hdfISIS_2D = new hdf5_wrap();                 //keep this open until exit for loop, because all will be in this file.
            foreach (DataRow drModelChange in rows)
            {
                sNodeName = drModelChange["Node"].ToString();
                sParentNode = drModelChange["ParentNode"].ToString();
                sDomain = GetDomainOrShapeFromConcat(drModelChange["element_note"].ToString(), true);
                sElementName = drModelChange["ElementName"].ToString();
                sFieldName = drModelChange["FieldName"].ToString();
                bool blnIsAttributeNode = bool.Parse(drModelChange["IsAttributeNode"].ToString());
                if (strTempDomainID == "") strTempDomainID = sDomain;
                string sVal = "";
                if (Convert.ToInt32(drModelChange["DV_ID_FK"].ToString()) == CommonUtilities._nDV_ID_Code_LinkedData)
                {
                    string sGroupKey = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.MEV, drModelChange["VarType_FK"].ToString(), drModelChange["ElementID"].ToString(), nScenarioID.ToString());               
                    strValue = ISIS2D_GetAssociatedTSValList(sGroupKey);            //2.0: modify to pull TS info from memory
                }
                else
                {   //this condition may never happen- this is the original code.
                    sVal = drModelChange["val"].ToString();
                    strValue += intCounter.ToString() + "\t" + sVal + "\r\n";
                }

                intCounter++;
                if (strTempDomainID != sDomain) // domain
                {
                    if (strValue.Length > 2) strValue = strValue.Substring(0, strValue.Length - 2);
                    // check for values to update element
                    var nodes = from c in xdoc.Root.Descendants(ns + "domain")
                                where c.Attribute("domain_id").Value.Contains(sDomain)
                                select c;
                    nodes.Elements(ns + sParentNode).Elements(ns + sNodeName).FirstOrDefault().SetElementValue(ns + sFieldName, strValue);
                    strTempDomainID = sDomain;
                    strValue = ""; intCounter = 0;
                }
            
                // update the last value for the domain
                // met 7/8/2013: moved this INSIDE the for loop, because now an inner for loop gets the TS string from the repository
                if (strValue.Length > 2)
                {
                    strValue = strValue.Substring(0, strValue.Length - 2);
                    // check for values to update element
                    var nodesLast = from c in xdoc.Root.Descendants(ns + "domain")
                                    where c.Attribute("domain_id").Value.Contains(sDomain)
                                    select c;
                    nodesLast.Elements(ns + sParentNode).Elements(ns + sNodeName).FirstOrDefault().SetElementValue(ns + sFieldName, strValue);
                }
            }
            xdoc.Save(sOutputXML); // save back to output XML file

         //   if (hdfISIS_2D._bHDF_IsOpen) { hdfISIS_2D.hdfClose(); }              //met 87/7/2013: assumes that all TS info is in one file
 
            return sReturn;
        }

        private string ISIS2D_GetAssociatedTSValList(string sGroupID, string sDataset = "1", bool bIsModified = true)
        {
            bool bIsOpen=true;
            string sVal; string sValConcat="";;
            int nSecondIndex = 1; if (!bIsModified) nSecondIndex = 0;
            int nIndex = Array.IndexOf(_sMEV_GroupID, sGroupID);         
                                                  //   if (!hdfISIS_2D._bHDF_IsOpen) { bIsOpen = hdfISIS_2D.hdfOpen(sTS_Path, true, false); }             //open file- this should already exist based upon 
            if (nIndex>=0)
            {
                double[,] dVals = _dMEV_Vals[nIndex, nSecondIndex];
                for (int i = 0; i<dVals.GetLength(0);i++){
                    sVal = dVals[i,0].ToString();
                    sValConcat += i.ToString() + "\t" + sVal + "\r\n";
                }
            }
            return sValConcat;
        }


        #endregion

        /// <summary>
        /// Get the source section based on the ElementName
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="xdoc"></param>
        /// <param name="sParentNode"></param>
        /// <param name="sNodeName"></param>
        /// <param name="sDomain"></param>
        /// <param name="sElementName"></param>
        /// <returns></returns>
        private IEnumerable<XElement> GetSourceFileSection(XNamespace ns, XDocument xdoc, string sParentNode,
            string sNodeName, string sDomain, string sElementName)
        {
            var nodes = from c in xdoc.Root.Descendants(ns + "domain").Elements(ns + sParentNode).Elements(ns + sNodeName)
                        where xdoc.Root.Descendants(ns + "domain").Attributes("domain_id").First().Value.Contains(sDomain)
                        && c.Element(ns + "file").Value.Contains(sElementName)
                        select c.Elements(ns + "value").Single();
            return nodes;
        }
        /// <summary>
        /// Get simulation time-stamp by domain
        /// </summary>
        /// <param name="strXMLFile"></param>
        /// <param name="strDomain"></param>
        private static DateTime getSimulationTimestampByDomain(XDocument xdoc, string strDomain)
        {
            XNamespace ns = "http://www.halcrow.com/ISIS2D";
            var nodes = from c in xdoc.Root.Descendants(ns + "domain").Elements(ns + "time")
                        where xdoc.Root.Descendants(ns + "domain").Attributes("domain_id").First().Value.Contains(strDomain)
                        select c.Elements(ns + "start_date").FirstOrDefault().Value + " " + c.Elements(ns + "start_time").FirstOrDefault().Value;
            if (nodes.Count() > 0)
            {
                return DateTime.Parse(nodes.FirstOrDefault().ToString());
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// Get time-series source detail
        /// </summary>
        /// <param name="strFullXMLFilePath"></param>
        /// <param name="strDomain"></param>
        /// <param name="strShapefileName"></param>
        /// <param name="strUnit"></param>
        public static List<TimeSeries> getSourceTimeSeries(string strFullXMLFilePath, string strDomain, string strShapefileName, out string strUnit)
        {
            List<TimeSeries> tsOut = new List<TimeSeries>();
            XDocument xdoc = XDocument.Load(strFullXMLFilePath); // load document
            XNamespace ns = "http://www.halcrow.com/ISIS2D";
            strUnit = "";
            // get start date & end date
            DateTime datStart = getSimulationTimestampByDomain(xdoc, strDomain); // get start-date and end-date

            var nodes = from c in xdoc.Root.Descendants(ns + "domain").Elements(ns + "hydrology").Elements(ns + "source")
                        where xdoc.Root.Descendants(ns + "domain").Attributes("domain_id").First().Value.Contains(strDomain)
                        && c.Element(ns + "file").Value.Contains(strShapefileName)
                        select c.Elements(ns + "value").Single();
            if (nodes.Count() > 0)
            {
                strUnit = nodes.Attributes("units").FirstOrDefault().Value.ToString();
                string strTimeUnit = nodes.Attributes("time_units").FirstOrDefault().Value.ToString();
                string strValue = nodes.First().Value;
                tsOut = TimeSeries.Transform2Timeseries(datStart, strTimeUnit, strValue); // transform to time-series
            }
            return tsOut;
        }


        //met 1/20/14: adapted to call getISIS2DTimeSeries
        public override double[,] GetNetworkTS_Data(int nElementID, int nVarType_FK, string sElementLabel="NECESSARYFORISIS2D", string sFileLocation = "NOTHING")
        {
            double[,] dReturn = null;
            int nRecordID = -1;
            if (sFileLocation=="NOTHING"){          //user could pass name for specific sceanrio? unlikely. todo if needed
                sFileLocation = _sActiveModelLocation;
            }
            string sOut;
            List<TimeSeries> tsOut = getISIS2DTimeSeries(sFileLocation, sElementLabel, out sOut);
            dReturn = TimeSeries.tsTimeSeriesTo2DArray(tsOut);
            return dReturn;
        }


        //level of abstraction, as we could have more than one source
        //todo: fill in with check against database
        public static List<TimeSeries> getISIS2DTimeSeries(string strFullXMLFilePath, string strDomainConcatShape, out string strUnit)
        {
            string sDomain = GetDomainOrShapeFromConcat(strDomainConcatShape, true);
            string sShapefileName = GetDomainOrShapeFromConcat(strDomainConcatShape, false);

            List<TimeSeries> tsOut = new List<TimeSeries>();

            if (true)
            {
                tsOut = getSourceTimeSeries(strFullXMLFilePath, sDomain, sShapefileName, out strUnit);
            }
            return tsOut;
        }

        public static string GetDomainOrShapeFromConcat(string sDomainConcatShape, bool bReturnDomain = true)
        {
            string sDelimit = "##"; string sReturn = "ERROR";
            if (sDomainConcatShape.Length >= (sDelimit.Length + 2))
            {
                int nDelimiterPos = sDomainConcatShape.IndexOf(sDelimit);
                if (bReturnDomain)
                {
                    sReturn = sDomainConcatShape.Substring(0, nDelimiterPos);
                }
                else
                {
                    sReturn = sDomainConcatShape.Substring(nDelimiterPos + 2, sDomainConcatShape.Length - 2 - nDelimiterPos);
                }
            }
            else
            {
                sReturn = "ERROR_MIN_LEN";
            }
            return sReturn;
        }
    }
}
