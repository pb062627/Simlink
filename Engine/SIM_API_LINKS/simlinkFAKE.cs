using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SIM_API_LINKS
{
    /// <summary>
    /// performs some simlink - like activities for cases where there is no database backend.
    /// the goal is to get some benefits of simlink processing (eg output reading, event definition etc) without the db backend
    /// 
    /// </summary>
    public partial class simlink 
    {

        /// <summary>
        /// in general, DO NOT use this- db handles
        /// if no db, grab
        /// </summary>
        /// <returns></returns>
        public int GetScenarioID_MANUAL()
        {
            if (_nActiveScenarioID == -1)
                return 1;
            else
                return _nActiveScenarioID++;
        }

        public void InitializeSimlinkVarsByConfig(string sConfig)
        {

        }

        public int InsertScenario_SimlinkLite(string sDir){
            string sFilename = Path.Combine(sDir, "scenario.txt");
            int nScenarioID = 1;
            if (File.Exists(sFilename))
            {
                using (StreamReader reader = new StreamReader(sFilename))
                {
                    string sbuf = reader.ReadLine().Trim();
                    bool bSuccess = int.TryParse(sbuf, out nScenarioID);            // get the new scenario
                    if (!bSuccess)
                        nScenarioID = -2;
                    _nActiveScenarioID = nScenarioID;
                }
                //
                using (StreamWriter writer = new StreamWriter(sFilename))
                {
                    string sToWrite = (nScenarioID + 1).ToString();
                    writer.WriteLine(sToWrite);
                }
            }
            else
            {
               // File.Create(sFilename);
                using (StreamWriter writer = new StreamWriter(sFilename,true))
                {
                    writer.WriteLine("nScenarioID");
                    _log.AddString("scenario.txt did not exist; added with base val of 1", Logging._nLogging_Level_2);
                }
            }
            _nActiveScenarioID = nScenarioID;
            return nScenarioID;
        }

    }
}
