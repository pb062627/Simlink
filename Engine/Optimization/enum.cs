using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    public class OptimizationEnum
    {
        public enum AlgType
        {
            GA = 1,
            SFLA = 2,
            BORG = 3,
            Undefined = -1
        }
        public enum StoppingCriteria
        {
            MaxLoops = 1,
            MaxTime = 2,
            Tolerance = 3,
            Percentile = 4
        }

        public enum LogPopulationLevel
        {
            NONE = 1,
            BEST = 2,
            ALL = 3
        }
    }
}
