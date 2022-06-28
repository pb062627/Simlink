using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizeDV;
using Optimization;
using System.IO;

namespace borg_wrap
{
    public class wrap
    {
      //  public int _nNumberVars;
      //  public int _nNumberObjectives;
        public double[,] _dMinMax;
        public Borg _borg;
        public Result _result;

        // todo- you want to figure out how to parameterize that third function! for now it's hard coded.        
        public wrap(int nNumberVars, int nObjectives, int nConstraints,  string sFileLimits = "dv.txt") 
        {
            _borg = new Borg(nNumberVars, nObjectives, nConstraints, Optimization.model_interface.Custom);      //hard-coded: need to get working with myMethod           
         //   _borg = new Borg(nNumberVars, nObjectives, nConstraints, DTLZ2_CHEATING);   // function built into wrapper which is unacceptable long term         
            _dMinMax = ReadFile(sFileLimits);
            SetBounds();
            SetEpsilon();

        }
        private void SetBounds()
        {
            for (int i = 0; i < _borg.GetNumberOfVariables(); i++)       //set var bounds
            {
            //    _borg.SetBounds(i, 0, 1);                             //save for debug
                _borg.SetBounds(i,_dMinMax[i,0], _dMinMax[i,1]);
            }
        }

        //todo: parametreize and/or overload with array setting
        private void SetEpsilon()
        {
            for (int i = 0; i < _borg.GetNumberOfObjectives(); i++)      
            {
                _borg.SetEpsilon(i, 0.01);
            }
        }
        //assumes param is nVars,nObj,nConstraint
        public static int GetBorgParams(string sParam, out int nObj, out int nConstraints)
        {
            string[] sVals = sParam.Split(',');
            nObj = Convert.ToInt32(sVals[1]);
            nConstraints = Convert.ToInt32(sVals[2]);
            return Convert.ToInt32(sVals[0]);
        }

        // read a little text file that provides parameterization
        private double[,] ReadFile(string sFileLimits)
        {
            if(!File.Exists(sFileLimits))
                sFileLimits= Path.Combine(Directory.GetCurrentDirectory(),sFileLimits); //if relative path, add full path
            double[,] dReturn = CommonUtilities.ReadDVLimits(sFileLimits);
 
            return dReturn;
        }

        //testing only
            //       dReturn[0, 0] = 1;
            //dReturn[0, 1] = 6;
            //dReturn[1, 0] = 4;
            //dReturn[1, 1] = 10;

        public void Solve(int nEvals = 1000000)
        {
            _result = _borg.Solve(nEvals);
        }

        //todo: option to push to repo
        public void WriteOut()
        {
            int nSolutionCounter = 1;
            foreach (Solution solution in _result)
            {
                Console.WriteLine("Solution: " + nSolutionCounter);
                Console.Write("Objectives: ");
                for (int i = 0; i < _borg.GetNumberOfObjectives();i++ )
                {
                    System.Console.Write(solution.GetObjective(i) + " ");
                }
                Console.WriteLine();
                Console.Write("Variables: ");
                for (int i = 0; i < _borg.GetNumberOfVariables(); i++)
                {
                    System.Console.Write(solution.GetVariable(i) + " ");
                }
                Console.WriteLine();
                nSolutionCounter++;
            }
        }
    }
}
