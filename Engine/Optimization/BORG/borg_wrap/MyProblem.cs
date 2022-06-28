using System;
using System.Runtime.InteropServices;
using OptimizeDV;

namespace borg_wrap
{
    public class model
    {
        public static int nvars = 11;
        public static int nobjs = 2;

        public static void DTLZ2(double[] vars, double[] objs, double[] constrs)
        {
            int k = nvars - nobjs + 1;
            double g = 0.0;

            for (int i = nvars - k; i < nvars; i++)
            {
                g += Math.Pow(vars[i] - 0.5, 2.0);
            }

            for (int i = 0; i < nobjs; i++)
            {
                objs[i] = 1.0 + g;

                for (int j = 0; j < nobjs - i - 1; j++)
                {
                    objs[i] *= Math.Cos(0.5 * Math.PI * vars[j]);
                }

                if (i != 0)
                {
                    objs[i] *= Math.Sin(0.5 * Math.PI * vars[nobjs - i - 1]);
                }
            }
        }

        // expects two objectivs and one variable
        public static void Test1(double[] vars, double[] objs, double[] constrs)
        {




        }
    }
}
