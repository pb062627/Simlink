using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SIM_API_LINKS
{
    public partial class simlink
    {
        //private Dictionary<int, simlink> dictSL_XMODEL = new Dictionary<int, simlink>();    //todo: consider implementing for multiple linked EG
        #region XMODEL_LINK         // functions to help define links across model platforms

        //todo: this may require generalization in the future.
        //presently (7/2/2013) this assumes linked records time series output becomes the input for model element vals


        //met 10/14/2013: modify qryRMG001_Link002_JoinResultTS to NOT sort on typeID=3.... tblRestultTS records may be accidentally linked (for a model element val) but this is not used.
        //only TS information is pulled from the tblResultS
        public DataSet XMODEL_LoadDS_LinkedRecords(int nScenarioID)
        {
            //met v2.0 : xmodel002 was not working through access; different approach may be 
            string sql = "SELECT tblEvaluationGroup.EvaluationID AS EvalID_REF, tblEvaluationGroup.ModelFileLocation AS ModelFileLocation_REF, "+
                            "tblEvaluationGroup.ModelType_ID AS ModelType_ID_REF, tblElement_XREF.RefScenarioID, tblElement_XREF.LinkScenarioID, "+
                            "tblEvaluationGroup_1.ModelFileLocation AS ModelFileLocation_LINK, tblEvaluationGroup_1.ModelType_ID AS ModelType_ID_LINK, "+
                            "tblEvaluationGroup_1.EvaluationID AS EvalID_LINK, tblElement_XREF.RefTypeID, tblElement_XREF.RefID, "+
                            "tblElement_XREF.RefTypeCode, tblElement_XREF.LinkTypeCode, tblElement_XREF.LinkID, tblElement_XREF.IsSpecialCase, "+
                            "tblElement_XREF.IsTS, tblEvaluationGroup_1.TS_StartDate AS TS_StartDate_Link, "+
                            "tblEvaluationGroup_1.TS_Interval AS TS_Interval_Link, tblEvaluationGroup_1.TS_Interval_Unit AS TS_Interval_Unit_Link, "+
                            "tblElement_XREF.ShiftTSVal, tblElement_XREF.ShiftTSCode, tblElement_XREF.ShiftTSReverseAfterProcess, "+
                            "tblElement_XREF.RefID_Label, tblElement_XREF.LinkID_Label, tblElement_XREF.ElementXREF_ID, tblElement_XREF.LinkTypeID, "+
                            "tblElement_XREF.LinkFileIsScen, tblElement_XREF.RefFileIsScen, tblEvaluationGroup.TSFileIsScen AS TSFileIsScen_REF, "+
                            "tblEvaluationGroup_1.TSFileIsScen AS TSFileIsScen_Link, tblEvaluationGroup.TS_StartDate AS TS_StartDate_REF, "+
                            "tblEvaluationGroup.TS_Interval AS TS_Interval_REF, tblEvaluationGroup.TS_Interval_Unit AS TS_Interval_Unit_REF, "+
                            "tblElement_XREF.IsDV_Link, tblElement_XREF.LinkSimCode " +
                            "FROM (((tblElement_XREF INNER JOIN tblScenario ON tblElement_XREF.RefScenarioID = tblScenario.ScenarioID) INNER JOIN "+
                            "tblEvaluationGroup ON tblScenario.EvalGroupID_FK = tblEvaluationGroup.EvaluationID) INNER JOIN "+
                            "tblScenario AS tblScenario_1 ON tblElement_XREF.LinkScenarioID = tblScenario_1.ScenarioID) INNER JOIN "+
                            "tblEvaluationGroup AS tblEvaluationGroup_1 ON tblScenario_1.EvalGroupID_FK = tblEvaluationGroup_1.EvaluationID "+
                            "WHERE (((tblElement_XREF.LinkByCode)=1)) AND (RefScenarioID = " + nScenarioID + ")";



            //SP 4-Mar-2016 remove reliance on Queries in access to ensure compatible with SQL Server    
            /*"SELECT ElementXREF_ID, EvalID_REF, ModelFileLocation_REF, ModelType_ID_REF, RefScenarioID, LinkScenarioID, ModelFileLocation_LINK, "
                    + "ModelType_ID_LINK, EvalID_LINK, RefTypeID, RefID, LinkTypeID, RefTypeCode, LinkTypeCode, LinkTypeID, LinkID, IsSpecialCase, IsTS, RefID_Label, LinkID_Label, "
                    + "TS_StartDate_REF, TS_Interval_REF, TS_Interval_Unit_REF, TS_StartDate_LINK, TS_Interval_LINK, TS_Interval_Unit_LINK, ShiftTSVal, ShiftTSCode, ShiftTSReverseAfterProcess, "
                    + "TSFileIsScen_LINK, TSFileIsScen_REF, IsDV_Link, LinkSimCode"
                    + " FROM qryXMODEL001_LinkByScenario WHERE (RefScenarioID = " + nScenarioID + ")";*/

            DataSet ds = _dbContext.getDataSetfromSQL(sql);
            return ds;
        }

        //met 1/2/13: todo significant modification to bring current to 2.0- step 1 is to get running.
            //step 2: take advantage of everything that is stored in mem now...

        public bool XMODEL_ProcessLinkedRecords(int nRefScenarioID, int nScenarioOUT = -1)
        {
            bool bReturn = true;
   // sim2.0         hdf5_wrap hdfTS_REF = new hdf5_wrap();
            // sim2.0          hdf5_wrap hdfTS_LINK = new hdf5_wrap();
            // sim2.0            string sHDF_FileNameREF = "NOTHING"; string sHDF_FileNameREF_Previous = "NOTHING";
            // sim2.0          string sHDF_FileNameLINK = "NOTHING"; string sHDF_FileNameLINK_Previous = "NOTHING";
            bool bSuccessRetrieveRef = false; ; bool bSuccessRetrieveLink = false;

            foreach (DataRow drLR in _dsEG_XMODEL_LINKS.Tables[0].Rows)
            {
                int nScenarioID_LINK = Convert.ToInt32(drLR["LinkScenarioID"].ToString());
                int nElementXREF_ID = Convert.ToInt32(drLR["ElementXREF_ID"].ToString());
                string sModelFileLocation_REF = drLR["ModelFileLocation_REF"].ToString();
                string sModelFileLocation_LINK = drLR["ModelFileLocation_LINK"].ToString();
                int nModelTypeID_REF = Convert.ToInt32(drLR["ModelType_ID_REF"].ToString());
                int nModelTypeID_LINK = Convert.ToInt32(drLR["ModelType_ID_LINK"].ToString());
                int nEvalID_REF = Convert.ToInt32(drLR["EvalID_REF"].ToString());
                int nEvalID_LINK = Convert.ToInt32(drLR["EvalID_LINK"].ToString());
                int nShiftTSVal = Convert.ToInt32(drLR["ShiftTSVal"].ToString());
                int nShiftTSCode = Convert.ToInt32(drLR["ShiftTSCode"].ToString());
                int nVarTypeID_REF = Convert.ToInt32(drLR["RefTypeID"].ToString());
                int nVarTypeID_LINK = Convert.ToInt32(drLR["LinkTypeID"].ToString());

                //todo: support TSD varying by dataset
                TimeSeries.TimeSeriesDetail tsDetail_REF = new TimeSeries.TimeSeriesDetail(_tsdResultTS_SIM._dtStartTimestamp, _tsdResultTS_SIM._tsIntervalType, _tsdResultTS_SIM._nTSInterval);    //  Convert.ToDateTime(drLR["TS_StartDate_REF"].ToString()), TimeSeries.GetTSIntervalType(Convert.ToInt32(drLR["TS_Interval_Unit_REF"].ToString())), Convert.ToInt32(drLR["TS_Interval_REF"].ToString()));
                TimeSeries.TimeSeriesDetail tsDetail_LINK = new TimeSeries.TimeSeriesDetail(_slXMODEL._tsdResultTS_SIM._dtStartTimestamp, _slXMODEL._tsdResultTS_SIM._tsIntervalType, _slXMODEL._tsdResultTS_SIM._nTSInterval);       //(Convert.ToDateTime(drLR["TS_StartDate_LINK"].ToString()), TimeSeries.GetTSIntervalType(Convert.ToInt32(drLR["TS_Interval_Unit_LINK"].ToString())), Convert.ToInt32(drLR["TS_Interval_LINK"].ToString()));
                //       DateTime dtStartDate_REF;
                //      DateTime dtStartDate_LINK;
                string sElementLabel_REF = drLR["RefID_Label"].ToString();
                string sElementLabel_LINK = drLR["LinkID_Label"].ToString();
                int nElementID_REF = Convert.ToInt32(drLR["RefID"].ToString());
                int nElementID_LINK = Convert.ToInt32(drLR["LinkID"].ToString());
                bool bTS_FileIsScen_LINK = Convert.ToBoolean(drLR["TSFileIsScen_LINK"].ToString());
                bool bTS_FileIsScen_REF = Convert.ToBoolean(drLR["TSFileIsScen_REF"].ToString());
                bool bUseSameFile = false;

                string sGroupID_REF = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.MEV, drLR["RefTypeID"].ToString(), drLR["RefID"].ToString(), _nActiveBaselineScenarioID.ToString());

                if (Convert.ToBoolean(drLR["IsTS"].ToString()))
                {            //typ:true (passing ts info between applications)
                    string sHDF_FileNameREF = CommonUtilities.GetSimLinkFull_TS_FilePath(sModelFileLocation_REF, nModelTypeID_REF, nEvalID_REF, nRefScenarioID, bTS_FileIsScen_REF);

             /* sim2.0       if (sHDF_FileNameREF != sHDF_FileNameREF_Previous)
                    {
                        bSuccessRetrieveRef = hdfTS_REF.hdfOpen(sHDF_FileNameREF, false, false);        //should only have to open once
                        sHDF_FileNameREF_Previous = sHDF_FileNameREF;
                    }

                    sHDF_FileNameLINK = CommonUtilities.GetSimLinkFull_TS_FilePath(sModelFileLocation_LINK, nModelTypeID_LINK, nEvalID_LINK, nScenarioID_LINK, bTS_FileIsScen_LINK);
                    if (sHDF_FileNameLINK != sHDF_FileNameLINK_Previous)
                    {
                        if (true)
                        {          //may need to add code to handle if filenames are equal;
                            bSuccessRetrieveLink = hdfTS_LINK.hdfOpen(sHDF_FileNameLINK, true, false);        //not writing to the LINK
                            if (sHDF_FileNameLINK_Previous != "NOTHING") { hdfTS_LINK.hdfClose(); }                  //close HDF file if opening new one
                            sHDF_FileNameLINK_Previous = sHDF_FileNameLINK;
                        }
                        else
                        {
                            bUseSameFile = true;
                        }
                    }               */

                    int nArrayIndex_REF = -1;
                    List<TimeSeries> lstTS_Ref = XMODEL_RetrievedLinkedData(sGroupID_REF, out nArrayIndex_REF, true);       //  ref hdfTS_REF, tsDetail_REF, nModelTypeID_REF, nRefScenarioID, nVarTypeID_REF, nElementID_REF, sElementLabel_REF, sModelFileLocation_REF);
                    //   dtStartDate_REF = lstTS_Ref[0]._dtTime;

                    // good code commented out for debugging                  List<TimeSeries> lstTS_Ref = XMODEL_RetrievedLinkedData(nModelTypeID_REF, nEvalID_REF,nRefScenarioID,sModelFileLocation_REF,1, Convert.ToInt32(drLR["RefID"].ToString()),-1); //todo replace TESTING ONLY
                    List<TimeSeries> lstTS_Linked = new List<TimeSeries>();
                    List<TimeSeries> lstTS_Modified = new List<TimeSeries>();
                    double dTSInterval_Ref = TimeSeries.GetTSInterval(lstTS_Ref);
                    double dTSInterval_Linked = -1;

                    //met 10/14/2013: how does this happen only on the link....? 
                    //answer- REF side is happening in XMODEL_RetrievedLinkedData
                    //inconsistent, but works with current examples

                    if (Convert.ToBoolean(drLR["IsSpecialCase"].ToString()))
                    {
                        //todo: may need to add addtl fields to the XREF to support this; however there may not be that many special cases. For now, proceed ad-hoc
                        if (nModelTypeID_LINK == CommonUtilities._nModelTypeID_SimClim)
                        {
                            if (true)       //bSuccessRetrieveLink
                            {
                                string sGroupID_Link = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS,
                                                        nElementID_LINK.ToString(), "SKIP",
                                                        nScenarioID_LINK.ToString());           // CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, "13", "166", nScenarioID_LINK.ToString());       //bojangles
                                int nArray_LINK = -1;
                                double[,] dSC_Output = _slXMODEL.XMODEL_RetrievedLinkedData(sGroupID_Link, out nArray_LINK, false, 666);         //sim2 replaced SC specific funtion
                                // met 9/6/2013: this now comes from the Eval/ResultTS dtStartDate_LINK = new DateTime(Convert.ToInt32(dSC_Output[0, 0]), 1, 1);
                                TimeSeries.ShiftTimeSeries(ref lstTS_Ref, tsDetail_REF._dtStartTimestamp.Date, tsDetail_LINK._dtStartTimestamp.Date, nShiftTSCode, nShiftTSVal, true);     //shift ref TS in reference to 
                                bool bIsTemperatureVar = ((nVarTypeID_LINK != CommonUtilities._nSimClim_ResultsCodePrecip) && (nVarTypeID_LINK != CommonUtilities._nSimClim_ResultsCodePrecipPerturb));        //met7/3/2013: assumes only two types of results vars - precip stuff and temp stuff; may need to be refined.
                                //changed PerturbSimClimData to PerturbMonthlyTS_Data
                                lstTS_Modified = TimeSeries.PerturbMonthlyTS_Data(lstTS_Ref, dSC_Output, bIsTemperatureVar);
                                if (Convert.ToBoolean(drLR["ShiftTSReverseAfterProcess"].ToString()))
                                {
                                    TimeSeries.ShiftTimeSeries(ref lstTS_Modified, tsDetail_REF._dtStartTimestamp.Date, tsDetail_LINK._dtStartTimestamp.Date, nShiftTSCode, nShiftTSVal, false);
                                }
                            }
                            else
                            {
                                //TODO: log the issue
                               // Console.WriteLine("Could not load reference SimClim data from HDF5 archive: " + sHDF_FileNameLINK);
                            }
                        }
                        else
                        {
                            //todo: log "functionality not established yet"
                        }

                    }
                    else
                    {          //typical case    UNTESTED as of 7/15/2013
                      //  lstTS_Linked = XMODEL_RetrievedLinkedData(ref hdfTS_LINK, tsDetail_LINK, nModelTypeID_LINK, nScenarioID_LINK, nVarTypeID_LINK, nElementID_LINK,  sElementLabel_LINK, sModelFileLocation_LINK);
                        dTSInterval_Linked = TimeSeries.GetTSInterval(lstTS_Linked);

                        //untested                       if (dTSInterval_Linked<>dTSInterval_Ref){
                        //untestedd                 lstTS_Linked = TimeSeries.Interpolate(lstTS_Linked, dTSInterval_Linked, dTSInterval_Ref);        //get on the same ts interaval
                        //untested                          lstTS_Modified = TimeSeries.ModifyTS(lstTS_Ref,lstTS_Linked,nFunctionCode);
                        //untested                    }


                    }

                    if (XMODEL_Helper_LinkSuccessful(bSuccessRetrieveRef, bSuccessRetrieveLink))                    //check that link has been made, and write out as appropriate (TODO: enhance for various special cases)
                    {
                  //      string sGroupMOD = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.MEV, drLR["RefTypeID"].ToString(), drLR["RefID"].ToString(), nScenarioID_LINK.ToString());
                        _dMEV_Vals[nArrayIndex_REF, 1] = TimeSeries.tsTimeSeriesTo2DArray(lstTS_Modified);                       // TimeSeries.tsWriteTimeSeries(lstTS_Modified, ref hdfTS_REF, sGroupMOD);
                        if ((nModelTypeID_REF == CommonUtilities._nModelTypeID_ISIS2D) || (nModelTypeID_REF == CommonUtilities._nModelTypeID_ISIS_FAST))                                               //special case where domain is required on tblMEV
                        {
                            string sDomain = "to be implemented- SimLink2.0";   // isis_2dLink.GetDomainOrShapeFromConcat(sElementLabel_REF, true);        //get domain which must be encoded into the RefID_Label
                            InsertModelValList(CommonUtilities._nDV_ID_Code_LinkedData, nVarTypeID_REF, nRefScenarioID, sGroupID_REF, sElementLabel_REF, "DOMAIN/LABEL in ElementNote", nElementID_REF, nElementXREF_ID);          //, sDomain);
                        }
                        else
                        {
                            //sim2.0
                            InsertModelValList(CommonUtilities._nDV_ID_Code_LinkedData, nVarTypeID_REF, nRefScenarioID, sGroupID_REF, "Linked TS record in TS Repository", sElementLabel_REF, nElementID_REF, nElementXREF_ID);                  
                        }

                        // untested            rmgDB_link.InsertModVals();                                              //met: todo- insert a tblMEV cookie
                    }


                    // at this point, lstTS_Modified should be populated. Write to HDF5 and throw indicator val into tblModelElementVals 

                }

                else
                {           //case: non TS data; 
                    //todo: log "functionality not established yet"
                }
            }
    //sim2        hdfTS_REF.hdfClose();
         //sim2   hdfTS_LINK.hdfClose();

            return bReturn;
        }



        //overloaded function pulls from memory- returns 2D array converted to list.

        public List<TimeSeries> XMODEL_RetrievedLinkedData(string sGroupID, out int nIndex, bool bIsPrimarySL)
        {
           nIndex = -1;List<TimeSeries> lstTS_Return = new List<TimeSeries>();
           if (bIsPrimarySL)
           {
               nIndex = Array.IndexOf(_sMEV_GroupID, sGroupID);
               double[,] dVals = _dMEV_Vals[nIndex, 0];
               //todo: each arrray should have its own TS...
               lstTS_Return = TimeSeries.Array2DToTSList(_tsdResultTS_SIM._dtStartTimestamp, _tsdResultTS_SIM._nTSInterval, _tsdResultTS_SIM._tsIntervalType, dVals);
           }
           else  // pull from the linked simlink object.
           {
               nIndex = Array.IndexOf(_slXMODEL._sMEV_GroupID, sGroupID);
               double[,] dVals = _slXMODEL._dMEV_Vals[nIndex, 0];
               //todo: each arrray should have its own TS...
               lstTS_Return = TimeSeries.Array2DToTSList(_slXMODEL._tsdResultTS_SIM._dtStartTimestamp, _slXMODEL._tsdResultTS_SIM._nTSInterval, _slXMODEL._tsdResultTS_SIM._tsIntervalType, dVals);

           }
            
           return lstTS_Return;
        }


        //overloaded function returns 2D array of TS information (important for SimLink formatted data
        //overloaded function pulls from memory- returns 2D array converted to list.
        // met 1/8/2013: 
        public double[,] XMODEL_RetrievedLinkedData(string sGroupID, out int nIndex, bool bIsPrimarySL, int nStupidValToCompile)
        {
            nIndex = -1; double[,] dVals;
            if (bIsPrimarySL)
            {
                nIndex = Array.IndexOf(_sMEV_GroupID, sGroupID);
                dVals = _dMEV_Vals[nIndex, 0];
            }
            else  // pull from the linked simlink object.
            {
                nIndex = Array.IndexOf(_sTS_GroupID, sGroupID);
                dVals = _dResultTS_Vals[nIndex];
            }

            return dVals;
        }




        /// <summary>
        /// Return all linked TS for a specific reference scenario
        /// </summary>
        /// <param name="inputTS"></param>
        /// <param name="adblSimClimTS"></param>
        /// <returns> List of timeseries </TS></returns>
        ///  <date> 7/1/2013</date>
        public List<TimeSeries> XMODEL_RetrievedLinkedData(ref hdf5_wrap hdfTS, TimeSeries.TimeSeriesDetail tsdStart, int nModelTypeID, int nScenarioID, int nVarType_FK, int nElementID, string sElementLabel = "NOTHING", string sModelFile = "NOTHING", TSRepository tsRepoOVERRIDE = TSRepository.Undefined, SimLinkDataType_Major slDataType = SimLinkDataType_Major.ResultTS)
        {
            string sUnits;
            List<TimeSeries> lstTS_Return = new List<TimeSeries>();
            TSRepository tsR = tsRepoOVERRIDE;
            string sGroupID = CommonUtilities.GetSimLink_TS_GroupName(slDataType, nElementID.ToString(), "SKIP", nScenarioID.ToString());       //met 9/6/2013

            //todo sim2.2: get this at init
            if (tsRepoOVERRIDE == TSRepository.Undefined) { tsR = CommonUtilities.GetDefaultSimLinkTSFormatByModelType(nModelTypeID); }

            switch (tsR)
            {
                case TSRepository.HDF5:
                    lstTS_Return = TimeSeries.tsGetTimeSeries(ref hdfTS, tsdStart, sGroupID);             //
                    break;
                case TSRepository.NetworkTable:
                    switch (nModelTypeID)
                    {
                        case 7:                             //ISIS 1D
                //sim2            lstTS_Return = isis_link.ISIS_GetNetworkData(nScenarioID, nVarType_FK, nElementID, ref conn);
                            break;
                        //  case _nModelTypeID_ISIS2D:        // this is processed in the XML switch..

                        // lstTS_Return = isis_2dLink.getISIS2DTimeSeries(@"C:\Users\mthroneb\Documents\Optimization\SimClim\IsisLink\2D\Automation\Test1\SimCLIM_ISIS2Dexample.xml", @"Domain1 20m H##C:\isis\data\examples\ISIS 2D\Site 2\GIS\Active_Area1.shp",  out sUnits);
                        //   break;

                    }
                    break;
                case TSRepository.XML:
               //sim2     lstTS_Return = isis_2dLink.getISIS2DTimeSeries(sModelFile, sElementLabel, out sUnits);

                    break;
            }
            if (lstTS_Return.Count == 0)
            {
                //todo: log the issue
            }


            return lstTS_Return;
        }




        public bool XMODEL_Helper_LinkSuccessful(bool bSuccessRetrieveRef, bool bSuccessRetrieveLink)
        {
            return true;
        }


        //companion function to XMODEL_RetrieveLinkedRecords that retrieves the actual tabular data
        public DataSet XMODEL_RetrievedLinkedData(int nLinkTypeID, int nLinkID, int nScenarioID)
        {
            string sql = "";
            switch (nLinkTypeID)
            {
                case -1:
                    sql = "select valTS as val from tblResultTS_Detail where ((ScenarioID_FK = " + nScenarioID + ") AND (ResultTSID_FK = " + nLinkID + "))";
                    break;
            }
            DataTable oTable = new DataTable();
            DataSet dsMyDs = _dbContext.getDataSetfromSQL(sql);
            return dsMyDs;
        }

        #endregion 

        #region XMODEL_X-CUTE

        //cut 1: run all scenarios for the linked EG.
        // this will be improved in time as needed
        //todo: will hve to know which one

        protected void ExecuteLinkedSimLinkPrecursor(){
            if(_slXMODEL != null){
                _slXMODEL.ProcessEvaluationGroup(new string[0]);         //_slXMODEL._nActiveEvalID, _slXMODEL._nActiveModelTypeID);
            }
        }

        //override in derived classes
        protected virtual void XMODEL_PlatformSpecificFollowup(int nScenarioID)
        {


        }


        #endregion



    }




}
