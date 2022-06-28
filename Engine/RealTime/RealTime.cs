using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIM_API_LINKS;

namespace SIM_API_LINKS
{
    /// <summary>
    ///  generalized wrapper for any source of real time information coming in
    ///     a simlink can have a host a number of rt objects 
    ///     the rt object knows how to interact with its intended dataset....
    /// </summary>
    public class  RealTime : simlink
    {

        DateTime _dtStartActive;         // these hold the current times
        DateTime _dtEndActive;   


        public override void InitializeModelLinkage(string sConnRMG, int nDB_Type, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            _nActiveModelTypeID = CommonUtilities._nModelTypeID_REALTIME;
            _sTS_FileNameRoot = "RT_TS.h5";
            base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
        }

        public override void InitializeEG(int nEvalID)
        {
            base.InitializeEG(nEvalID);
            nEvalID = GetReferenceEvalID();
        }

        public override int ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINP_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1")
        {
            string sPath = ""; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName; string sINIFile; string sSummaryFile; string sOUT;
            int nCurrentLoc = nScenStartAct; string sTS_Filename = "";

            ScenDS_ClearAfterScenario(nScenarioID); //SP 9-Mar-2016 Moved to the start of process scenario as Optimization functions require some of datasets that get reset here

            try
            {
                if (true)     //we should have a valid ScenarioID at this point.
                {
                    if (_bIsOptimization || (nScenarioID == -1))           //nScenarioID  = -1
                    {
                        nScenarioID = InsertScenario(nEvalID, nProjID, System.DateTime.Now.ToString(), "", sDNA);       //pass the current date time to enable easy retrieval of PK (should be better wya to do this)
                        _nActiveScenarioID = nScenarioID;
                    }
                    ScenarioPrepareFilenames(nScenarioID, nEvalID, out sTargetPath);
                            //del  sPath = System.IO.Path.GetDirectoryName(sINP_File);
                    bool bContinue = true;              //identifies whether to continue
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModElementExist) && (nScenEndAct >= CommonUtilities.nScenLCModElementExist))       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
                    {
                        //irrelevant for this case  
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineFileSetup) && (nScenEndAct >= CommonUtilities.nScenLCBaselineFileSetup))
                    {
                                              if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        CommonUtilities.CopyDirectoryAndSubfolders(sPath, sTargetPath, true);
                        nCurrentLoc = CommonUtilities.nScenLCBaselineFileSetup;          //
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
                    if ((nCurrentLoc <= CommonUtilities.nScenLCBaselineModified) && (nScenEndAct >= CommonUtilities.nScenLCBaselineModified))
                    {
 //not relevant
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelExecuted) && (nScenEndAct >= CommonUtilities.nScenLCModelExecuted))
                    {
                        // get the data
                        GetRealtime_Data(_dtStartActive, _dtEndActive);

                        nCurrentLoc = CommonUtilities.nScenLCModelExecuted;
                    }

                    if ((nCurrentLoc <= CommonUtilities.nScenLCModelResultsRead) && (nScenEndAct >= CommonUtilities.nScenLCModelResultsRead))
                    {
                        _log.AddString("EPANET Results Read Begin: ", Logging._nLogging_Level_2);      //log begin scenario step
                        //       ResultsReadReport(sSummaryFile, nReferenceEvalID, nScenarioID);
                        nCurrentLoc = CommonUtilities.nScenLCModelResultsRead;
                    }
                    if ((nCurrentLoc <= CommonUtilities.nScenLModelResultsTS_Read) && (nScenEndAct >= CommonUtilities.nScenLModelResultsTS_Read))
                    {
                       
                        //1/5/15: updated code to read TS above, and then write below if requested
                        if ((_tsRepo ==  .TSRepository.HDF5) && (_IntermediateStorageSpecification._bResultTS))           //at present, this is the only supported TS repo
                        {
                            _hdf5 = new hdf5_wrap();
                            _hdf5.hdfCheckOrCreateH5(sTS_Filename);
                            _hdf5.hdfOpen(sTS_Filename, false, true);
                        //todo    WriteTimeSeriesToRepo();
                            _hdf5.hdfClose();
                        }

                        nCurrentLoc = CommonUtilities.nScenLModelResultsTS_Read;
                    }

                    ProcessScenario_COMMON(nReferenceEvalID, nScenarioID, nCurrentLoc, nScenEndAct, sTS_Filename);        //call base function to perform modeltype independent actions
                    //ScenDS_ClearAfterScenario(nScenarioID); //SP 9-Mar-2016 some of datasets that are cleared in this routine are required for postprocessing for opt
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


        /// <summary>
        ///   get target dir
        /// </summary>

        private void ScenarioPrepareFilenames(int nScenarioID, int nEvalID, out string sTargetPath)
        {
            //string sPath = System.IO.Path.GetDirectoryName(sINP_File);         
            sTargetPath = CommonUtilities.GetSimLinkDirectory(_sActiveModelLocation, nScenarioID, nEvalID, true);   //met 4/29/14- was being done manually. confirm and delete prev line
        }

    }

    public class RealTimeRecord
    {
        public string _sLabel;
        public string _sType;    //todo: convert to enum
        public string _sLongitude { get; set; }
        public string _sLatitude { get; set; }

        //override this
        public virtual void GetRealtime_Data(DateTime dtStartRequest, DateTime dtEndRequest)
        {

        }
    }


}
