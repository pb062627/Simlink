using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SIM_API_LINKS
{
    // partial class added due to length of code required to support cloning
    //todo
        //consider network topo? tons of tables.. maybe easier to re-import?
        // consider pulling results in? Also lots of tables, easier to regenerate?

    
    public partial class simlink
    {

        #region CLONE
        /// <summary>
        /// Take data from source simlink into currently open db
        ///         TODO: topology tables
        ///    
        /// </summary>
        /// <param name="simlinkSOURCE"></param>
        /// <param name="nProjID"></param>
        /// <returns></returns>
        public int Clone(int nProjID, simlink simlinkSOURCE)
        {
            int nNewProjID = CloneProjectTable(nProjID, ref simlinkSOURCE);
            int nScenarioID_CLONE = -1; string sSQL_EG = "";

            DataSet dsEG = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "eg", nProjID, out sSQL_EG);
            CloneProjectTables(nNewProjID, ref simlinkSOURCE);
            CloneUpdateForeignKeys_PROJECT(nProjID, ref simlinkSOURCE, nNewProjID);
            CloneEG(nNewProjID, ref simlinkSOURCE, nProjID);
            foreach (DataRow drEG in dsEG.Tables[0].Rows)
            {
                _dbContext.OpenCloseDBConnection();                         // not sure if this is required, but to be safe.
                int nActiveEvalID = Convert.ToInt32(drEG["EvaluationID"]);
                simlinkSOURCE.InitializeEG(nActiveEvalID);
                //  int nEvalID_CLONE = ClonePutEG(dsEG, drEG, out nScenarioID_CLONE, sSQL_EG);
                int nEvalID_CLONE = _cloneHash.RetrieveHashVal("eg", nActiveEvalID);
                CloneEG_Definition(nEvalID_CLONE, ref simlinkSOURCE);
                //           _cloneHash.InsertHashVal("eg", nActiveEvalID, nEvalID_CLONE);         //  track the EG
                int nBaseScenarioID = Convert.ToInt32(drEG["ScenarioID_Baseline_FK"]);
                CloneBaseScenario(nEvalID_CLONE, ref simlinkSOURCE, nBaseScenarioID);
                // now go clean up references
                CloneUpdateForeignKeys(nEvalID_CLONE, ref  simlinkSOURCE, nActiveEvalID);
            }
            CloneFK_EG(nNewProjID); // done outside of EG loop



            return nNewProjID;
        }

        #region CLONE_Updates
        public void CloneUpdateForeignKeys(int nEvalID, ref simlink simlinkSOURCE, int nEvalID_Source)
        {
            //CloneFK_EG(nNewProjID);, 
            CloneFK_DV(nEvalID);
            CloneFK_ResultTS(nEvalID);
            CloneFK_Event(nEvalID);
            CloneFK_Performance(nEvalID);
            CloneFK_PerfXREF(nEvalID, ref simlinkSOURCE, nEvalID_Source);
        }
        public void CloneUpdateForeignKeys_PROJECT(int nProjIDSOURCE, ref simlink simlinkSOURCE, int nProjID_CLONE)
        {
            CloneFK_OptDetails(nProjIDSOURCE, ref simlinkSOURCE, nProjID_CLONE);
            CloneFK_ElementDetails(nProjIDSOURCE, ref simlinkSOURCE, nProjID_CLONE);
        }
        private void CloneFK_OptDetails(int nProjIDSOURCE, ref simlink simlinkSOURCE, int nProjID_CLONE)
        {
            string sSQL = "";
            DataSet ds = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "option_detail", nProjIDSOURCE, out sSQL);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["optionid_fk"] = _cloneHash.RetrieveHashVal("option", Convert.ToInt32(dr["optionid_fk"]));
            }
            string sWHERE = CloneConstructDetailTableWhereClause(nProjIDSOURCE, "option_detail", ref simlinkSOURCE);
            //sSQL = "SELECT OptionID, OptionID_FK, OptionNo, val, VarID_FK FROM tblOptionDetails " + sWHERE;     //better to fix up from current string thanto dup codo. todo - revise
            sSQL = "SELECT OptionID, OptionID_FK, OptionNo, val FROM tblOptionDetails " + sWHERE;     //SP 13-Jul-2016 removed VarID_FK from DB Schema for tblOptionDetails
            _dbContext.InsertOrUpdateDBByDataset(false, ds, sSQL, true, false);
        }

        /// <summary>
        /// Encountered a problem on insering the detail tables using decent sql... the current 
        /// db adapater needs to be able to get the records from dbClone, to modify to be consisent with db clone...
        /// thus artificially create a where clause using the in clause
        /// </summary>
        /// <param name="nProjIDSOURCE"></param>
        /// <param name="sCODE"></param>
        /// <returns></returns>
        private string CloneConstructDetailTableWhereClause(int nProjID_or_Eval_SOURCE, string sCODE, ref simlink simlinkSOURCE)
        {
            string sSQL = "";
            string sReturn = "";
            switch (sCODE)
            {
                case "option_detail":
                    sSQL = "select optionID from tblOptionLists where (ProjID_FK=" + nProjID_or_Eval_SOURCE + ")";
                    sReturn = "WHERE OptionID_FK in (";
                    break;
                case "element_detail":
                    sSQL = "select ElementListID from tblElementLists where (ProjID_FK=" + nProjID_or_Eval_SOURCE + ")";
                    sReturn = "WHERE ElementListID_FK in (";
                    break;
                case "perf_xref":
                    sSQL = "select PerformanceID from tblPerformance where (EvalID_FK=" + nProjID_or_Eval_SOURCE + ")";
                    sReturn = "WHERE PerformanceID_FK in (";
                    break;
            }
            DataSet ds = simlinkSOURCE._dbContext.getDataSetfromSQL(sSQL);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sReturn += dr[0].ToString() + ",";
            }
            if (ds.Tables[0].Rows.Count>0)
                sReturn = sReturn.Substring(0, sReturn.Length - 1);      //clip trailing comma
            sReturn += ")";
            return sReturn;
        }

        private void CloneFK_ElementDetails(int nProjIDSOURCE, ref simlink simlinkSOURCE, int nProjID_CLONE)
        {
            string sSQL = "";
            DataSet ds = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "element_detail", nProjIDSOURCE, out sSQL);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["elementlistid_fk"] = _cloneHash.RetrieveHashVal("element", Convert.ToInt32(dr["elementlistid_fk"]));
            }
            string sWHERE = CloneConstructDetailTableWhereClause(nProjIDSOURCE, "element_detail", ref simlinkSOURCE);
            sSQL = "SELECT ElementListDetailID, ElementListID_FK, ElementID_FK, VarLabel FROM tblElementListDetails " + sWHERE;     //better to fix up from current string thanto dup codo. todo - revise

            _dbContext.InsertOrUpdateDBByDataset(false, ds, sSQL, true, false);
        }

        private void CloneFK_PerfXREF(int nEvalID, ref simlink simlinkSOURCE, int nEvalID_Source)
        {
            string sSQL = "";
            string sWHERE = CloneConstructDetailTableWhereClause(nEvalID_Source, "perf_xref", ref simlinkSOURCE);
            if(sWHERE.Substring(sWHERE.Length-2)=="()")       //empty where clause returned- nothing to update.
                return;     
            
            sSQL = "SELECT ID, PerformanceID_FK, LinkTableID_FK, ScalingFactor, LinkType, ApplyThreshold, Threshold, IsOver_Threshold FROM tblPerformance_ResultXREF " + sWHERE;
            DataSet dsEG = simlinkSOURCE._dbContext.getDataSetfromSQL(sSQL);            //CommonUtilities.GetDSbySQL(_dbContext, "perf_xref", nEvalID_Source, out sSQL);
            int nCounter = 0;
            foreach (DataRow dr in dsEG.Tables[0].Rows)
            {
                if (nCounter == 119)
                    nCounter = nCounter;
                nCounter++;
                dr["performanceid_fk"] = _cloneHash.RetrieveHashVal("performance", Convert.ToInt32(dr["performanceid_fk"]));
                switch (((LinkedDataType)Convert.ToInt32(dr["linktype"])))
                {
                    case LinkedDataType.DVOptions:
                        dr["linktableid_fk"] = _cloneHash.RetrieveHashVal("dv", Convert.ToInt32(dr["linktableid_fk"]));
                        break;
                    case LinkedDataType.ResultSummary:
                        dr["linktableid_fk"] = _cloneHash.RetrieveHashVal("result", Convert.ToInt32(dr["linktableid_fk"]));
                        break;
                    case LinkedDataType.ResultTS:
                        dr["linktableid_fk"] = _cloneHash.RetrieveHashVal("resultts", Convert.ToInt32(dr["linktableid_fk"]));
                        break;
                    case LinkedDataType.Event:
                        dr["linktableid_fk"] = _cloneHash.RetrieveHashVal("event", Convert.ToInt32(dr["linktableid_fk"]));
                        break;
                    case LinkedDataType.Performance:
                        dr["linktableid_fk"] = _cloneHash.RetrieveHashVal("performance", Convert.ToInt32(dr["linktableid_fk"]));
                        break;
                }
            }
            _dbContext.InsertOrUpdateDBByDataset(false, dsEG, sSQL, true, false);
        }

        /// <summary>
        /// updates the args to the new data.
        /// </summary>
        /// <param name="sFuncArgs"></param>
        /// <returns></returns>
        private string CloneFK_FunctionArgs(string sFuncArgs)
        {
            char[] charFunctions = new char[] { '_', '!', '@', '%', '~', '&' };
            //  int nStartIndex = sFuncArgs.IndexOf('?');           // grab second part... so we don't get scrwed up by bad names
            string sFuncArgs_ReturnError = sFuncArgs +"_ERROR";        //return this if problem
            try
            {
                int nIndex = sFuncArgs.IndexOf('?');
                if (nIndex > 0)
                {
                    string sPrefix = sFuncArgs.Substring(0, nIndex);
                    sFuncArgs = sFuncArgs.Substring(sFuncArgs.IndexOf('?'));
                    int nSearchIndex = 0;
                    while (sFuncArgs.Substring(nSearchIndex).IndexOfAny(charFunctions) > 0)         // assuems thes chars not in the func names  (Which is not enforced)
                    {
                        int nActiveIndex = sFuncArgs.Substring(nSearchIndex).IndexOfAny(charFunctions) + nSearchIndex;
                        int nEndIndex = sFuncArgs.IndexOf(',', nActiveIndex);
                        if (nEndIndex == -1)
                        {                      // if last string, find closing bracket } and this is where comma would be.
                            nEndIndex = sFuncArgs.Length - 1;
                        }
                        bool bIsBaseline = false;
                        string sID_FK = sFuncArgs.Substring(nActiveIndex + 1, nEndIndex - nActiveIndex - 2);
                        string sFuncArgs_SUFFIX = sFuncArgs.Substring(nEndIndex - 1);
                        if (sID_FK.Substring(0, 1) == "#")
                        {
                            bIsBaseline = true;
                            sID_FK = sID_FK.Substring(1);
                        }
                        string sCode = GetDataTypeFromChar(sFuncArgs.Substring(nActiveIndex, 1));
                        // get the new ID to be replaced into the function (may have diff len then before
                        string sID_FK_CLONE = _cloneHash.RetrieveHashVal(sCode, Convert.ToInt32(sID_FK)).ToString();
                        // put arg string back together
                        sFuncArgs = sFuncArgs.Substring(0, nActiveIndex + 1);
                        if (bIsBaseline)
                            sFuncArgs += "#";
                        nSearchIndex = sFuncArgs.Length + sID_FK_CLONE.Length + 1;     // update the place to search from  (was 2, why diff)
                        sFuncArgs += sID_FK_CLONE + sFuncArgs_SUFFIX;
                    }
                    sFuncArgs = sPrefix + sFuncArgs;        // add the prefix before returning it
                }
                return sFuncArgs;
            }
            catch(Exception ex){
                return sFuncArgs_ReturnError;
            }
        }

        /// <summary>
        /// Helper function to retrieve the delimiter associated with a certain data type.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetCharDelimiterFromDataType(SimLinkDataType_Major dt){
            switch(dt){
                case SimLinkDataType_Major.MEV:
                    return "_";
                case SimLinkDataType_Major.Event:
                    return "@";
                case SimLinkDataType_Major.ResultSummary:
                    return "!";
                case SimLinkDataType_Major.ResultTS:
                    return "!";    // not sure if this is right??
                case SimLinkDataType_Major.Performance:
                    return "%";
                case SimLinkDataType_Major.Network:
                    return "&";
                case SimLinkDataType_Major.XMODEL:
                    return "NOT_SUPPORTED";
                default:
                    return "NOT_SUPPORTED_NOT_DEFINED";

            }
        }
        private string GetDataTypeFromChar(string sChar)
        {
            switch (sChar)
            {
                case "_":
                    return "dv";
                case "!":
                    return "result";
                case "@":
                    return "event";
                case "%":
                    return "performance";
                case "~":
                    return "dv";
                case "&":
                    return "model network change- not yet supported";
                default:
                    return "unknown";
            }
        }


        private SimLinkDataType_Major GetSimlinkDataTypeFromChar(string sVal)
        {
            switch (sVal)
            {
                case "_":
                    return SimLinkDataType_Major.MEV;
                case "!":
                    return SimLinkDataType_Major.ResultSummary;
                case "@":
                    return SimLinkDataType_Major.Event;
                case "%":
                    return SimLinkDataType_Major.Performance;
                case "~":
                    return SimLinkDataType_Major.MEV;
                case "&":
                    return SimLinkDataType_Major.MEV;   //not sure if supported?
                default:
                    return SimLinkDataType_Major.XMODEL;  // not yet defined... 
            }
        }

        private void CloneFK_Performance(int nEvalID)
        {
            string sSQL = "";
            DataSet dsEG = CommonUtilities.GetDSbySQL(_dbContext, "performance", nEvalID, out sSQL);
            foreach (DataRow dr in dsEG.Tables[0].Rows)
            {
                if (CloneFieldUpdate(dr, "OptionID_FK"))
                {
                    dr["OptionID_FK"] = _cloneHash.RetrieveHashVal("option", Convert.ToInt32(dr["OptionID_FK"]));
                }
                if (CloneFieldUpdate(dr, "FunctionID_FK"))
                {
                    dr["FunctionID_FK"] = _cloneHash.RetrieveHashVal("function", Convert.ToInt32(dr["FunctionID_FK"]));
                }
                if (CloneFieldUpdate(dr, "DV_ID_FK"))
                {
                    dr["DV_ID_FK"] = _cloneHash.RetrieveHashVal("dv", Convert.ToInt32(dr["DV_ID_FK"]));
                }
                if (CloneFieldUpdateFARGS(dr, "FunctionArgs"))
                {
                    dr["FunctionArgs"] = CloneFK_FunctionArgs(dr["FunctionArgs"].ToString());      //update fargs to ref proper keys
                }
            }
            _dbContext.InsertOrUpdateDBByDataset(false, dsEG, sSQL, true, false);
        }
        private void CloneFK_Event(int nEvalID)
        {
            string sSQL = "";
            DataSet dsEG = CommonUtilities.GetDSbySQL(_dbContext, "event", nEvalID, out sSQL);
            foreach (DataRow dr in dsEG.Tables[0].Rows)
            {
                //    for now assume resultTs ...
                dr["ResultTS_or_Event_ID_FK"] = _cloneHash.RetrieveHashVal("resultts", Convert.ToInt32(dr["ResultTS_or_Event_ID_FK"]));
            }
            _dbContext.InsertOrUpdateDBByDataset(false, dsEG, sSQL, true, false);
        }
        private void CloneFK_EG(int nNewProjID)
        {
            string sSQL = "";
            DataSet dsEG = CommonUtilities.GetDSbySQL(_dbContext, "eg", nNewProjID, out sSQL);
            foreach (DataRow dr in dsEG.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr["ReferenceEvalID_FK"]) != -1)
                {
                    dr["ReferenceEvalID_FK"] = _cloneHash.RetrieveHashVal("eg", Convert.ToInt32(dr["ReferenceEvalID_FK"]));
                }
                //update the baseline scenario
                dr["ScenarioID_Baseline_FK"] = _cloneHash.RetrieveHashVal("scenario", Convert.ToInt32(dr["ScenarioID_Baseline_FK"]));
            }
            _dbContext.InsertOrUpdateDBByDataset(false, dsEG, sSQL, true, false);
        }
        private void CloneFK_ResultTS(int nEvalID)
        {
            string sSQL = "";
            DataSet ds = CommonUtilities.GetDSbySQL(_dbContext, "dv", nEvalID, out sSQL);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (CloneFieldUpdate(dr, "FunctionID_FK"))
                {
                    dr["FunctionID_FK"] = _cloneHash.RetrieveHashVal("function", Convert.ToInt32(dr["FunctionID_FK"]));
                }
                if (CloneFieldUpdateFARGS(dr, "FunctionArgs"))
                {
                    dr["FunctionArgs"] = CloneFK_FunctionArgs(dr["FunctionArgs"].ToString());      //update fargs to ref proper keys
                }
            }
        }
        private void CloneFK_DV(int nEvalID)
        {
            string sSQL = "";
            DataSet ds = CommonUtilities.GetDSbySQL(_dbContext, "dv", nEvalID, out sSQL);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["Option_FK"] = _cloneHash.RetrieveHashVal("option", Convert.ToInt32(dr["Option_FK"]));
                dr["ElementID_FK"] = _cloneHash.RetrieveHashVal("element", Convert.ToInt32(dr["ElementID_FK"]));
                if (CloneFieldUpdate(dr, "PrimaryDV_ID_FK"))
                {
                    dr["PrimaryDV_ID_FK"] = _cloneHash.RetrieveHashVal("dv", Convert.ToInt32(dr["PrimaryDV_ID_FK"]));
                }
                if (CloneFieldUpdate(dr, "FunctionID_FK"))
                {
                    dr["FunctionID_FK"] = _cloneHash.RetrieveHashVal("function", Convert.ToInt32(dr["FunctionID_FK"]));
                }
                if (CloneFieldUpdateFARGS(dr, "FunctionArgs"))
                {
                    dr["FunctionArgs"] = CloneFK_FunctionArgs(dr["FunctionArgs"].ToString());      //update fargs to ref proper keys
                }
                // todo: update func args!!
            }
            _dbContext.InsertOrUpdateDBByDataset(false, ds, sSQL, true, false);
        }

        private bool CloneFieldUpdate(DataRow dr, string sField, int nCompareVal = -1)
        {
            try
            {
                if (Convert.ToInt32(dr[sField]) == nCompareVal)
                    return false;  // val -1 (or sent) needs no update
                else
                    return true;    //real val- swap it.
            }
            catch (Exception ex)
            {
                Console.WriteLine("error updating field:" + sField);     //do better.
                return false;
            }
        }
        private bool CloneFieldUpdateFARGS(DataRow dr, string sField, int nCompareVal = -1)
        {
            try
            {
                if ((dr[sField]) is DBNull)
                    return false;
                else if (dr[sField] == nCompareVal.ToString())
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error updating field:" + sField);     //do better.
                return false;
            }
        }


        #endregion

        #region CLONE_PROJ
        private void CloneProjectTables(int nNewProjID, ref simlink simlinkSOURCE)
        {
            CloneProj_Elements(nNewProjID, ref simlinkSOURCE);
            CloneProj_Options(nNewProjID, ref simlinkSOURCE);
            CloneProj_Functions(nNewProjID, ref simlinkSOURCE);
            CloneProj_Consts(nNewProjID, ref simlinkSOURCE);
            CloneProj_ElementDetails(nNewProjID, ref simlinkSOURCE);
            CloneProj_OptionDetails(nNewProjID, ref simlinkSOURCE);
        }
        private void CloneProj_ElementDetails(int nNewProjID, ref simlink simlinkSOURCE)
        {
            string sSQL = "";
            DataSet dsResult = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "element_detail", simlinkSOURCE._nActiveProjID, out sSQL);
            DataSet dsResultCLONE = dsResult.Copy();
            CloneUpdateEval(dsResultCLONE, nNewProjID, "ProjID_FK", true, true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsResultCLONE, sSQL, true, false);
        }
        private void CloneProj_OptionDetails(int nNewProjID, ref simlink simlinkSOURCE)
        {
            string sSQL = "";
            DataSet dsResult = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "option_detail", simlinkSOURCE._nActiveProjID, out sSQL);
            DataSet dsResultCLONE = dsResult.Copy();
            CloneUpdateEval(dsResultCLONE, nNewProjID, "ProjID_FK", true, true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsResultCLONE, sSQL, true, false);
        }
        private void CloneProj_Functions(int nNewProjID, ref simlink simlinkSOURCE)
        {
            string sSQL = "";
            DataSet dsResult = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "function", simlinkSOURCE._nActiveProjID, out sSQL);
            DataSet dsResultCLONE = dsResult.Copy();
            CloneUpdateEval(dsResultCLONE, nNewProjID, "ProjID_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsResultCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsResultCLONE = CommonUtilities.GetDSbySQL(_dbContext, "function", nNewProjID, out sSQL);
            PushToHash(dsResult, dsResultCLONE, "function", "functionid");
        }

        /// <summary>
        /// IMPORTANT: 
        /// </summary>
        /// <param name="dsSource"></param>
        /// <param name="dsClone"></param>
        /// <param name="sTableCode"></param>
        /// <param name="sPK"></param>
        private void PushToHash(DataSet dsSource, DataSet dsClone, string sTableCode, string sPK)
        {
            int nCounter = 0;
            foreach (DataRow drSource in dsSource.Tables[0].Rows)
            {
                DataRow drClone = dsClone.Tables[0].Rows[nCounter];
                int nCloneID = Convert.ToInt32(drClone[sPK]);
                int nSourceID = Convert.ToInt32(drSource[sPK]);
                _cloneHash.InsertHashVal(sTableCode, nSourceID, nCloneID);
                nCounter++;
            }
        }

        private void CloneProj_Consts(int nNewProjID, ref simlink simlinkSOURCE)
        {
            string sSQL = "";
            DataSet dsResult = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "constant", simlinkSOURCE._nActiveProjID, out sSQL);
            DataSet dsResultCLONE = dsResult.Copy();
            CloneUpdateEval(dsResultCLONE, nNewProjID, "ProjID_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsResultCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsResultCLONE = CommonUtilities.GetDSbySQL(_dbContext, "constant", nNewProjID, out sSQL);
            PushToHash(dsResult, dsResultCLONE, "constant", "constantid");
        }
        private void CloneProj_Elements(int nNewProjID, ref simlink simlinkSOURCE)
        {
            string sSQL = "";
            DataSet dsResult = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "element", simlinkSOURCE._nActiveProjID, out sSQL);
            DataSet dsResultCLONE = dsResult.Copy();
            CloneUpdateEval(dsResultCLONE, nNewProjID, "ProjID_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsResultCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsResultCLONE = CommonUtilities.GetDSbySQL(_dbContext, "element", nNewProjID, out sSQL);
            PushToHash(dsResult, dsResultCLONE, "element", "elementlistid");
        }
        private void CloneProj_Options(int nNewProjID, ref simlink simlinkSOURCE)
        {
            string sSQL = "";
            DataSet dsResult = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "option", simlinkSOURCE._nActiveProjID, out sSQL);
            DataSet dsResultCLONE = dsResult.Copy();
            CloneUpdateEval(dsResultCLONE, nNewProjID, "ProjID_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsResultCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsResultCLONE = CommonUtilities.GetDSbySQL(_dbContext, "option", nNewProjID, out sSQL);
            PushToHash(dsResult, dsResultCLONE, "option", "optionid");
        }

        private int CloneProjectTable(int nProj, ref simlink simlinkSOURCE)
        {
            int nProjID_CLONE = -1;
            // no ds exists for proj, so you need to grab the data
            string sSQL = "";
            DataSet ds = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "project", simlinkSOURCE._nActiveProjID, out sSQL);
            DataSet dsClone = ds;   //copy the dataset
            SetRowStateAdded(ref dsClone);      //tell simlink these are really to be inserted.
            nProjID_CLONE = _dbContext.InsertOrUpdateDBByDataset(true, dsClone, sSQL, true, true);              // should pull back the id using new sql server identity code (met 7/16/16)
            return nProjID_CLONE;
        }

        /// <summary>
        /// Set all rows in a dataset table to state added 
        ///     todo: find out where this goes... CU?
        /// </summary>
        /// <param name="ds"></param>
        private void SetRowStateAdded(ref DataSet ds)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr.SetAdded();
            }
        }

        #endregion
        private void CloneBaseScenario(int nEvalID_CLONE, ref simlink simlinkSOURCE, int nScenarioBase_SOURCE)
        {
            string sSQL = "";
            DataSet ds = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "scenario_base", nScenarioBase_SOURCE, out sSQL);

            DataSet dsCLONE = ds.Copy();
            CloneUpdateEval(dsCLONE, nEvalID_CLONE, "EvalGroupID_FK", true);
            int nScenarioID_CLONE = _dbContext.InsertOrUpdateDBByDataset(true, dsCLONE, sSQL, true, true);
            _cloneHash.InsertHashVal("scenario", nScenarioBase_SOURCE, nScenarioID_CLONE);
        }

        private int ClonePutEG(DataSet dsEG, DataRow drEG, out int nScenarioID_CLONE, string sSQL)
        {
            nScenarioID_CLONE = -1;
            DataSet dsCLONE = dsEG.Copy();      //copy to avoid affecting orig dataset (!!)
            dsCLONE.Tables[0].Rows.Clear();     //inelegant to pass whole dataaset and delete rows then add          

            dsCLONE.Tables[0].Rows.Add(drEG.ItemArray);   // however, there will generally be few EG to do this for, so not terrible. in future, improve.
            int nPK = _dbContext.InsertOrUpdateDBByDataset(true, dsCLONE, sSQL, true, true);
            return nPK;
        }
        private int CloneEG_Definition(int nEvalID_CLONE, ref simlink simlinkSOURCE)
        {
            // 
            CloneDV(nEvalID_CLONE, ref simlinkSOURCE, simlinkSOURCE._nActiveEvalID);
            CloneResultVar(nEvalID_CLONE, ref simlinkSOURCE, simlinkSOURCE._nActiveEvalID);
            CloneResultTS(nEvalID_CLONE, ref simlinkSOURCE, simlinkSOURCE._nActiveEvalID);
            CloneEvents(nEvalID_CLONE, ref simlinkSOURCE, simlinkSOURCE._nActiveEvalID);
            ClonePerf(nEvalID_CLONE, ref simlinkSOURCE, simlinkSOURCE._nActiveEvalID);
            ClonePerfXREF(nEvalID_CLONE, ref simlinkSOURCE, simlinkSOURCE._nActiveEvalID);
            return -1;

        }
        // does not work to use the _dsEG object because this includes data from linked tlkp dict
        //therefore, need to grab the data and use that
        private void CloneResultVar(int nEvalID_CLONE, ref simlink simlinkSOURCE, int nEvalID_SOURCE)
        {
            string sSQL = "";
            DataSet dsResult = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "result", nEvalID_SOURCE, out sSQL);

            DataSet dsResultCLONE = dsResult.Copy();
            CloneUpdateEval(dsResultCLONE, nEvalID_CLONE, "EvaluationGroup_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsResultCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsResultCLONE = CommonUtilities.GetDSbySQL(_dbContext, "result", nEvalID_CLONE, out sSQL);
            PushToHash(dsResult, dsResultCLONE, "result", "result_id");
        }

        private void CloneEvents(int nEvalID_CLONE, ref simlink simlinkSOURCE, int nEvalID_SOURCE)
        {
            string sSQL = "";
            DataSet dsSOURCE = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "event", nEvalID_SOURCE, out sSQL);
            DataSet dsCLONE = dsSOURCE.Copy();
            CloneUpdateEval(dsCLONE, nEvalID_CLONE, "EvaluationGroupID_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            // TODO: figure out why the EG is not getting set properly for events only
            //commented out hash table insert for now.
            dsCLONE = CommonUtilities.GetDSbySQL(_dbContext, "event", nEvalID_CLONE, out sSQL);
            PushToHash(dsSOURCE, dsCLONE, "event", "eventsummaryid");
        }

        private void CloneDV(int nEvalID_CLONE, ref simlink simlinkSOURCE, int nEvalID_SOURCE)
        {
            string sSQL = "";
            DataSet dsSOURCE = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "dv", nEvalID_SOURCE, out sSQL);
            DataSet dsCLONE = dsSOURCE.Copy();
            CloneUpdateEval(dsCLONE, nEvalID_CLONE, "EvaluationGroup_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsCLONE = CommonUtilities.GetDSbySQL(_dbContext, "dv", nEvalID_CLONE, out sSQL);
            PushToHash(dsSOURCE, dsCLONE, "dv", "dvd_id");
        }

        private void CloneEG(int nProjID_CLONE, ref simlink simlinkSOURCE, int nProjID_SOURCE)
        {
            string sSQL = "";
            DataSet dsSOURCE = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "eg", nProjID_SOURCE, out sSQL);
            DataSet dsCLONE = dsSOURCE.Copy();
            CloneUpdateEval(dsCLONE, nProjID_CLONE, "ProjID_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsCLONE = CommonUtilities.GetDSbySQL(_dbContext, "eg", nProjID_CLONE, out sSQL);
            PushToHash(dsSOURCE, dsCLONE, "eg", "evaluationid");
        }
        private void ClonePerfXREF(int nEvalID_CLONE, ref simlink simlinkSOURCE, int nEvalID_SOURCE)
        {
            string sSQL = "";
            DataSet dsSOURCE = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "perf_xref", nEvalID_SOURCE, out sSQL);
            DataSet dsCLONE = dsSOURCE.Copy();
            CloneUpdateEval(dsCLONE, nEvalID_CLONE, "NOTHING", true, true);           // skip EG update 
            _dbContext.InsertOrUpdateDBByDataset(true, dsCLONE, sSQL, true, false);
        }
        private void ClonePerf(int nEvalID_CLONE, ref simlink simlinkSOURCE, int nEvalID_SOURCE)
        {
            string sSQL = "";
            DataSet dsSOURCE = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "performance", nEvalID_SOURCE, out sSQL);
            DataSet dsCLONE = dsSOURCE.Copy();
            CloneUpdateEval(dsCLONE, nEvalID_CLONE);
            _dbContext.InsertOrUpdateDBByDataset(true, dsCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsCLONE = CommonUtilities.GetDSbySQL(_dbContext, "performance", nEvalID_CLONE, out sSQL);
            PushToHash(dsSOURCE, dsCLONE, "performance", "performanceid");
        }
        private void ClonePerfXREF(int nEvalID_SOURCE, ref simlink simlinkSOURCE)
        {
            string sSQL = "";
            DataSet dsSOURCE = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "perf_xref", nEvalID_SOURCE, out sSQL);
            DataSet dsCLONE = dsSOURCE.Copy();
            CloneUpdateEval(dsCLONE, -1, "nothing", true, true);            // set rows to added, but no EG info to update
            _dbContext.InsertOrUpdateDBByDataset(true, dsCLONE, sSQL, true, false);
        }
        private void CloneResultTS(int nEvalID_CLONE, ref simlink simlinkSOURCE, int nEvalID_SOURCE)
        {
            string sSQL = "";
            DataSet dsResult = CommonUtilities.GetDSbySQL(simlinkSOURCE._dbContext, "resultts", nEvalID_SOURCE, out sSQL);
            DataSet dsResultCLONE = dsResult.Copy();
            CloneUpdateEval(dsResultCLONE, nEvalID_CLONE, "EvaluationGroup_FK", true);
            _dbContext.InsertOrUpdateDBByDataset(true, dsResultCLONE, sSQL, true, false);

            // now load hash table - first get new DS with new PKs
            dsResultCLONE = CommonUtilities.GetDSbySQL(_dbContext, "resultts", nEvalID_CLONE, out sSQL); //SP 26-Jul-2016 need to get the new ResultTS ids for the cloned dataset
            PushToHash(dsResult, dsResultCLONE, "resultts", "resultts_id");
        }


        private void CloneUpdateEval(DataSet ds, int nEvalToUpdate, string sEvalColName = "EvalID_FK", bool bSetIsAdded = true, bool bSkipEG_SET = false)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (!bSkipEG_SET)
                    dr[sEvalColName] = nEvalToUpdate;
                if (bSetIsAdded)
                {
                    dr.AcceptChanges();
                    dr.SetAdded();
                }
            }
        }

        #endregion


    }
}
