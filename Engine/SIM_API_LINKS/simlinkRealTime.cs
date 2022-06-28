using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SIM_API_LINKS
{
    /// <summary>
    /// functions to assist with data retrieval and scenario execution
    /// 
    /// some are expected to be overridden by the base simulator
    /// </summary>
    public partial class simlink
    {
        // met 12/10/16: added a more general dictrequest becaues there is no reason to believe request will generally be time derived. could be anything!
        public bool ExtractExternalData(Dictionary<string, string> dictAdditionalRequests = null, RetrieveCode nRetrieveCode = RetrieveCode.Aux, string sNewKey = "NONE")
        {
            // todo: pre-cursor step to open any conn   (avoid multi open/close per sim)
            bool bSuccess = true;
            // moved to ExternalData temporarily      List<DAL.DBContext_Parameter> lstParams = GetStartEndTimes(dtStart);

            //SP 13-Dec-2016 execute all ExternalDataRequests with AuxCode specified
            List<ExternalData> _lstExDSWithRetrieveCode = new List<ExternalData>();
            foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)nRetrieveCode).ToString())) //SP 15-Feb-2017 _dsEG_ResultTS_Request is now the combined ResultTS dataset
            {
                _lstExDSWithRetrieveCode.AddRange(_lstExternalDataSources.Where(x => x._nUID == Convert.ToInt32(dr["AuxID_FK"].ToString())).ToArray());
            }
            
            double[][,] dValsExternalData = ExternalData.ExecuteGetExternalDataSources(_lstExDSWithRetrieveCode, dictAdditionalRequests/*, sNewKey*/, _log); //SP 9-Mar-2017 sNewKey is now in params

            foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)nRetrieveCode).ToString()))  //SP 15-Feb-2017 Confirm with MET - Changed 'ts_code' to RetrieveCode //SP 15-Feb-2017 _dsEG_ResultTS_Request is now the combined ResultTS dataset   
            {
                int nAuxID = Convert.ToInt32(dr["AuxID_FK"].ToString());
                int nUID = Convert.ToInt32(dr["ResultTS_ID"].ToString());
                ExternalData ex = _lstExDSWithRetrieveCode.Find(x => x._nUID == nAuxID);
                try
                {
                    //double[,] dVals = ex.RetrieveData(dictAdditionalRequests); //retrieved all external data requests earlier in routine

                    _dResultTS_Vals[_dictResultTS_Indices[nUID]] = dValsExternalData[_lstExDSWithRetrieveCode.IndexOf(ex)]; //SP 13-Dec-2016 now reference from array of external data request 2D arrays //  put  in memory
                }
                catch (Exception except)
                {
                    _log.AddString("Failed to extract aux ts " + dr["ResultTS_ID"].ToString() + " msg: " + except.Message, Logging._nLogging_Level_1, true, true);
                    bSuccess = false;
                }
            }

            //todo : close all connections
            return bSuccess;
        }

        /// <summary>
        /// Set all vals of a timeseries equal to the value of the index defined in nStartIndex.
        /// Created for RT functinonality to represent no prediction data 
        /// </summary>
        /// <param name="nStartIndex"></param>
        /// <param name="nRetrieveCode"></param>
        public void SetAllAuxTSToVal(int nStartIndex, RetrieveCode nRetrieveCode = RetrieveCode.Aux)
        {
            foreach (DataRow dr in _dsEG_ResultTS_Request.Tables[0].Select("RetrieveCode =" + ((int)nRetrieveCode).ToString())) //SP 15-Feb-2017 _dsEG_ResultTS_Request is now the combined ResultTS dataset
            {
                int nAuxID = Convert.ToInt32(dr["AuxID_FK"].ToString());
                int nUID = Convert.ToInt32(dr["ResultTS_ID"].ToString());
                int nTS_Index = _dictResultTS_Indices[nUID];
                int nPeriods = _dResultTS_Vals[nTS_Index].GetLength(0);
                double dValRepeating = _dResultTS_Vals[nTS_Index][nStartIndex, 0];       //todo: better checks
                for (int i = nStartIndex+1; i < nPeriods; i++)
                {
                    _dResultTS_Vals[nTS_Index][i, 0] = dValRepeating;
                }

            }
        }
    }
}
