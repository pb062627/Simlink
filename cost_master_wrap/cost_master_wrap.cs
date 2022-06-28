using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using cost_master;
//using c
using assets;       //cost master assets


namespace cost_master_wrap
{
    public class cost_master_simlink
    {
        assets.system _system = new system();

        public void Initialize(int nScenario, string sConn )
        {
            _system.Initialize(nScenario, sConn);
        }
        public void Close()
        {
            _system.Close();
        }

        public double CostAsset(string sUID, double dParam1, double dParam2, Dictionary<string, double> dictParams)
        {
            double dValCost = _system.CostAssetsByUID(sUID, dParam1, dParam2, dictParams);
            return dValCost;
        }

        public void UpdateCostingForScenario(int nScenarioID)
        {
            _system.UpdateScenario(nScenarioID);
        }

        public void WriteCostingResultsForScenario()
        {
            _system.WriteCosts();
        }

    }
}
