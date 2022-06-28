using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Threading;
using VISSIMLIB;

namespace VissimWrapper
{

    public enum VissimTimeUnits
    {
        Generic = 1,
        Milliseconds = 2,
        Seconds = 3,
        Minutes = 4,
        Hours = 5,
        Days = 6,
        Weeks = 7,
        Months = 8,
        Years = 9
    }

    public enum VissimElements
    {
        Node,
        Link,
        Area,
        DesSpeedDecision,
        Lane,
        PriorityRule,
        ConflictMarker,
        VAP,
        QueueCounter,
        LinkEvalSegment,
        VehicleTravelTimeMeasurement,
        Unknown
    }


    //create an object which holds the Vissim connection
    public class VissimObject
    {

        #region Initiate

        Vissim VSInstance = new Vissim();
        Dictionary<string, string> dictObjKeyNoAndName = new Dictionary<string, string>();
        #endregion

        private string sLinkNumLanes = "NumLanes";
        private string sLink2DLength = "Length2D";
        private string sLinkSegmentLength = "LinkEvalSegLen";

        #region VissimApplicationFunctions

        public void VISSIM_RunSimulationToNextBreakPoint(double nNextSimulationBreak)
        {
            VSInstance.Graphics.CurrentNetworkWindow.set_AttValue("QuickMode", 1);
            VSInstance.SuspendUpdateGUI(); // stop updating of the complete Vissim workspace (network editor, list, chart and signal time table windows)
            VSInstance.Simulation.set_AttValue("SimBreakAt", nNextSimulationBreak);
            // Set maximum speed:
            VSInstance.Simulation.set_AttValue("UseMaxSimSpeed", true);
            VSInstance.Simulation.RunContinuous();

            //if the next simulation break = 0, then continue to the end
            if (nNextSimulationBreak == 0)
            {
                VSInstance.ResumeUpdateGUI(false); // allow updating of the complete Vissim workspace (network editor, list, chart and signal time table windows)
                VSInstance.Graphics.CurrentNetworkWindow.set_AttValue("QuickMode", 0); // deactivate QuickMode            
            }
        }

        public void VISSIM_SaveModel(string sSaveModelAsName)
        {
            VSInstance.SaveNetAs(sSaveModelAsName);
        }

        public void VISSIM_CloseApplication()
        {
            VSInstance.Exit();
        }

        public void VISSIM_OpenModel(string sModelFileLocation, bool bGetKeyAndNameForAllObjects = false)
        {
            VSInstance.LoadNet(sModelFileLocation, false);
            if (bGetKeyAndNameForAllObjects)
            {
                //dictObjKeyNoAndName = VISSIM_GetObjectDictionaryOfNameAndKey();
            }
        }

        #endregion



        #region VissimAsServerAutomationFunctions
        //get Max Simulation Time
        public int VISSIM_GetRunTimeParameter()
        {
            return (int)VSInstance.Simulation.AttValue["SimPeriod"];
        }

        //get Simulation Reporting Step
        public int VISSIM_GetReportingStep()
        {
            return (int)VSInstance.Simulation.AttValue["SimBreakAt"];
        }

        //set Max Simulation Time
        public void VISSIM_SetRunTimeParameter(double nMaxSimulationTime)
        {
            VSInstance.Simulation.set_AttValue("SimPeriod", nMaxSimulationTime);
        }

        //set Random Seed
        public void VISSIM_SetRandomSeed(int nRandomSeedNumber)
        {
            VSInstance.Simulation.set_AttValue("RandSeed", nRandomSeedNumber);
        }

        public void VISSIM_ClearAllExistingSimulations()
        { 
            // Delete all previous simulation runs first:
            foreach (ISimulationRun simRun in VSInstance.Net.SimulationRuns)
            {
                try
                {
                    //VSInstance.Net.SimulationRuns.RemoveSimulationRun(simRun); //SP 2-Oct-2017 not compatible with Vissim 7.0. Only 9.0 
                }
                catch (Exception ex)
                {
                    ///todo
                }
            }
        }

    //----------------NODES-----------
    //Request a value from Vissim for NODE 
    public object VISSIM_Request(string sObjectID, string sAttrIdentifier, VissimElements vsElementType)
        {
            //int nKeyNumber = VISSIM_GetKeyNumOfObjectFromName(dictObjKeyNoAndName, sObjectName);
            string[] sComponentsToKey = null;

            switch (vsElementType)
            {
                case VissimElements.Node:
                    //get the node object
                    INode INodeObj = (INode)VSInstance.Net.Nodes.ItemByKey[sObjectID];
                    return INodeObj.AttValue[sAttrIdentifier];
                case VissimElements.Link:
                    //get the link object
                    ILink ILinkObj = (ILink)VSInstance.Net.Links.ItemByKey[sObjectID];
                    return ILinkObj.AttValue[sAttrIdentifier];
                case VissimElements.DesSpeedDecision:
                    //get the DesSpeedDecision object
                    IDesSpeedDecision IDesSpeedDecisionObj = (IDesSpeedDecision)VSInstance.Net.DesSpeedDecisions.ItemByKey[sObjectID];
                    return IDesSpeedDecisionObj.AttValue[sAttrIdentifier];
                //----------------TODO ADD MORE OBJECT TYPES AS THEY ARE NEEDED -----------
                case VissimElements.PriorityRule:
                case VissimElements.QueueCounter:
                    //get the Queue counter
                    IQueueCounter IQueueCounterObj = (IQueueCounter)VSInstance.Net.QueueCounters.ItemByKey[sObjectID];
                    return IQueueCounterObj.AttValue[sAttrIdentifier];
                case VissimElements.LinkEvalSegment:
                    sComponentsToKey = sObjectID.Split('-').ToArray(); //format should be link number, lane number, segment number

                    //get the Link Segment
                    ILink ILinkObj2 = (ILink)VSInstance.Net.Links.ItemByKey[sComponentsToKey[0]];
                    object sCSVResults = ILinkObj2.AttValue[sAttrIdentifier];
                    //split the comma separate list into an array
                    string[] arrsLinkSegmentResults = sCSVResults.ToString().Split(',');
                    double[] arrdLinkSegmentResults = Array.ConvertAll(sCSVResults.ToString().Split(','), double.Parse);

                    //pull out the required component from the array - the array contains all values for each segement for each lane in a comma separated list
                    //find the number of lanes and number of segments
                    object nNumLanes = ILinkObj2.AttValue[sLinkNumLanes];
                    object dSegLength = ILinkObj2.AttValue[sLinkSegmentLength];
                    object dLength = ILinkObj2.AttValue[sLink2DLength];
                    int nNumSegments = (int)Math.Ceiling((double)dLength / (double)dSegLength);

                    //process the lane to return. If ommitted, then assume user wants to return the 1st / only lane
                    int nLaneToReturn = 1;
                    if (sComponentsToKey.Count() > 1)
                        nLaneToReturn = Math.Max(Convert.ToInt32(sComponentsToKey[1]), 1); //if a 0 or a 1 is entered, return results for the only / first lane

                    //process the lane to return. If ommitted, then assume user wants to return the 1st / only segment
                    int nSegmentToReturn = 1;
                    if (sComponentsToKey.Count() > 2)
                        nSegmentToReturn = Math.Max(Convert.ToInt32(sComponentsToKey[2]), 1);

                    //get the index for the lane and segment number
                    int nIndex = (nLaneToReturn - 1) * nNumSegments + nSegmentToReturn - 1;
                    return arrdLinkSegmentResults[nIndex];
                case VissimElements.VehicleTravelTimeMeasurement:
                    //get the DesSpeedDecision object
                    IVehicleTravelTimeMeasurement IVehTravelTimeMeasurementObj = (IVehicleTravelTimeMeasurement)VSInstance.Net.VehicleTravelTimeMeasurements.ItemByKey[sObjectID];
                    return IVehTravelTimeMeasurementObj.AttValue[sAttrIdentifier];

                default:
                    return "NOTHING";

            }
        }

        //Poke a value into Vissim model for NODE
        public void VISSIM_PokeVal(string sObjectID, string sAttrIdentifier, string sAttrValue, VissimElements vsElementType)
        {
            //int nKeyNumber = VISSIM_GetKeyNumOfObjectFromName(dictObjKeyNoAndName, sObjectName);
            string[] sComponentsToKey = null;

            switch (vsElementType)
            {
                case VissimElements.Node:
                    //get the node object
                    INode INodeObj = (INode)VSInstance.Net.Nodes.ItemByKey[sObjectID];
                    INodeObj.set_AttValue(sAttrIdentifier, sAttrValue);
                    break;
                case VissimElements.Link:
                    //get the link object
                    ILink ILinkObj = (ILink)VSInstance.Net.Links.ItemByKey[sObjectID];
                    ILinkObj.set_AttValue(sAttrIdentifier, sAttrValue);
                    break;
                case VissimElements.DesSpeedDecision:
                    //get the DesSpeedDecision object
                    IDesSpeedDecision IDesSpeedDecisionObj = (IDesSpeedDecision)VSInstance.Net.DesSpeedDecisions.ItemByKey[sObjectID];
                    IDesSpeedDecisionObj.set_AttValue(sAttrIdentifier, sAttrValue);
                    break;
                case VissimElements.Lane:
                    sComponentsToKey = sObjectID.Split('-').ToArray();
                    //get the parent Link object
                    ILink ILinkObj2 = (ILink)VSInstance.Net.Links.ItemByKey[sComponentsToKey[0]];
                    ILane ILaneObj = (ILane)ILinkObj2.Lanes.ItemByKey[sComponentsToKey[1]];
                    ILaneObj.set_AttValue(sAttrIdentifier, sAttrValue);
                    break;
                case VissimElements.PriorityRule:
                    IPriorityRule IPrRule = (IPriorityRule)VSInstance.Net.PriorityRules.ItemByKey[sObjectID];
                    IPrRule.set_AttValue(sAttrIdentifier, sAttrValue);
                    break;
                case VissimElements.ConflictMarker:
                    IPriorityRule IPrRule2 = (IPriorityRule)VSInstance.Net.PriorityRules.ItemByKey[sObjectID];
                    IConflictMarkerContainer ICnfltMrkrCol = (IConflictMarkerContainer)IPrRule2.ConflictMarkers;
                    ICnfltMrkrCol.SetAllAttValues(sAttrIdentifier, sAttrValue);
                    break;
                case VissimElements.QueueCounter:
                    IQueueCounter IQCounter = (IQueueCounter)VSInstance.Net.QueueCounters.ItemByKey[sObjectID];
                    IQCounter.set_AttValue(sAttrIdentifier, sAttrValue);
                    break;
                    //----------------TODO ADD MORE OBJECT TYPES AS THEY ARE NEEDED -----------
            }

        }

        #endregion

    }
}
