using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelSim
{
    
    // a linkage is the real class for doing this
    //separate testing class for model helps keep code clean
    //future: add specific more realistic test problems
    
    public class ModelSim
    {
        public double ModelExecute(double[] dVals)
        {
            //simply return the squares of the values
            int testc;
            double dObjective = 0;
            if (dVals[0] == 0 && dVals[1] == 0 && dVals[2] == 0 && dVals[3] == 0 && dVals[4] == 0 && dVals[5] == 0)
                testc = 0;
            for (int i = 0; i < dVals.Length; i++)
            {   
                dObjective = dObjective + (dVals[i]-0) * (dVals[i]-0);
            }
            
            return dObjective;
        }
    }
}
