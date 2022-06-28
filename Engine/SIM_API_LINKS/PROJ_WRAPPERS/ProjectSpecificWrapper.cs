using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace SIM_API_LINKS.PROJ_WRAPPERS
{
    #region ABQ MODFLOW
    public class modflow_ABQ : SIM_API_LINKS.modflow_link
    {
        #region MEMBERS

        #endregion

        #region INIT


        //project specific implementation for ABQ- results must come back in this order.
        protected override DataSet GetDataset_ResultTS(int nEvalID)
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

            // order the results for ABQ model
            dt.Rows.Add(771, 21, "Zone1: HEAD DEP BOUNDS:IN", 1, "Zone1");
            dt.Rows.Add(774, 24, "Zone1: HEAD DEP BOUNDS:OUT", 1, "Zone1");
            dt.Rows.Add(770, 20, "Zone1: RIVER LEAKAGE:IN", 1, "Zone1");
            dt.Rows.Add(773, 23, "Zone1: RIVER LEAKAGE:OUT", 1, "Zone1");
            ds.Tables.Add(dt);
            return ds;
        }
        #endregion


        public double TestDouble(double dVal)
        { return 2 * dVal; }


        #region COM-TESTING
        public bool Test1(string sFile)
        {
            return TestValidABQ_Setup(sFile);
        }

        //test OptionalArgs
        public bool Test2_OptionalArgs(string sConnRMG, int nDB_Type, int nEvalID, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            return TestValidABQ_Setup(sConnRMG);
        }

        public int InitializeModelLinkage_ABQ(string sConnRMG, int nDB_Type, int nEvalID, bool bSimCondor = false, string _sDelimiter = "", int nLogLevel = 1)
        {
            bool bIsValid = TestValidABQ_Setup(sConnRMG);

            if (bIsValid)
            {
                _nActiveModelTypeID = CommonUtilities._nModelTypeID_MODFLOW;
                _sActiveModelLocation = sConnRMG;
                _nActiveEvalID = nEvalID;
                base.InitializeModelLinkage(sConnRMG, nDB_Type, bSimCondor, _sDelimiter, nLogLevel);
                Load_EG_Tables(nEvalID);
                INIT_LoadZoneBudgetNavigation();
                return _nActiveModelTypeID;
            }
            else
            {
                return 0;
            }
        }



        #endregion



        public string QuickTest(string sInputFile)
        {
            return Path.GetDirectoryName(sInputFile) + "\\" + "Zones.dat";
        }

        public bool TestValidABQ_Setup(string sInputFile)
        {
            bool bReturn = false;
            string sTargetFileName = Path.GetDirectoryName(sInputFile) + "\\" + "Zones.dat";
            if (File.Exists(sTargetFileName))
            {
                StreamReader file = new StreamReader(sTargetFileName);
                string sbuf = file.ReadLine();
                if (sbuf.Trim() == "6 113 60")                          //validity check in Zones.dat per Steve Shultz
                    bReturn = true;
                file.Close();
            }
            return bReturn;
        }

    }

    #endregion
}
