using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_API_LINKS
{
    public  class swmm502_link : simlink
    {

        protected override void InitNavigationDict()
        {

        }
        /// <summary>
        /// must override on derived class
        /// </summary>
        /// <param name="sDNA"></param>
        /// <param name="nScenarioID"></param>
        /// <param name="nScenStartAct"></param>
        /// <param name="nScenEndAct"></param>
        /// <returns></returns>
        public override int ProcessScenarioWRAP(string sDNA, int nScenarioID, int nScenStartAct, int nScenEndAct)
        {
            return -4;
        }

        public override void InitializeModelLinkage(string sConnRMG,  int nDB_Type,  bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {

        }
        public override void ProcessEvaluationGroup()                //int nEvalID, int nModelTypeID, int nRefScenarioID = -1, bool bSkipDictionaryPopulate = false, int nSingleScenario = -1, string sOptionalFileLocation = "NOTHING")
        {

        }
    }
}
