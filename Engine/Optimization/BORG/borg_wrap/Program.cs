using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizeDV;
using borg_wrap;

namespace borg_wrap
{
    class Program
    {
        static void Main(string[] args)
        {
            int nvars = 11;
            int nobjs = 2;


            Borg borg = new Borg(11, 2, 0,  model.DTLZ2);      //met: changed nam as unfamiliar 

            Borg borg2 = new Borg(2, 3, 0, Optimization.model_interface.DTLZ2_in_mod);


            for (int i = 0; i < nvars; i++)
            {
                borg.SetBounds(i, 0.0, 1.0);
            }

            for (int i = 0; i < nobjs; i++)
            {
                borg.SetEpsilon(i, 0.01);
            }
            try
            {
                Result result = borg.Solve(1000000);
                foreach (Solution solution in result)
                {
                    System.Console.WriteLine(solution.GetObjective(0) + " " + solution.GetObjective(1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}
