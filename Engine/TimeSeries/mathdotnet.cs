using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;


namespace SIM_API_LINKS
{
    public static class mathdotnet
    {
        public static double Quantile(double[] dArr, double dThreshold)
        {
            double dVal = Statistics.Quantile(dArr, dThreshold);
            return dVal;
        }
        public static double QuantileRank(double[] dArr, double dThreshold)
        {
            double dVal = Statistics.QuantileRank(dArr, dThreshold);
            return dVal;
        }
    }
}
