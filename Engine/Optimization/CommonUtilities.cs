using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    //static class for performing general functions
    
    public static class CommonUtilities
    {
        public const double _dBAD_VALorNOT_SET = -99999.99;
        public const string _sBAD_VALorNOT_SET = "BAD_VAL_OR_NOT_SET";

        public static int GetValFromTriangularDistribution(double nInput) //baha note: arg is a real in Extend- don't think it is passed as one , though?      //That is right...  //Done! BYM
        {
            double dProbability = 0; Random _r = new Random();                                                                                                                                      //BYM
            double dRandom = _r.NextDouble();                                                                                                                                   //BYM
            int nReturn= 1;                 //default return val if not set in for loop
            for(int j=1;j<=nInput;j++)      //baha todo: plz check logic                                              //Looks good with little data type modifications          //Done! BYM
	        {                                                                                                          
                dProbability += 2 * (nInput + 1 - j) / (nInput * (nInput + 1));                                                                               //BYM
                if (dRandom <= dProbability)
                    return j;
            }
            return nReturn;
        }

        #region TEXT_HELPERS
        //given an incoming file name, create a filename that won't screw up windows.
        public static string RMV_FixFilename(string sIncoming)
        {
            return sIncoming.Replace(":", "_").Replace(",", "_").Replace("!", "_").Replace("*", "_").Replace(" ", "_").Replace("/", "_").Replace("#", "_").Replace(" ", "$").Replace("%", "_").Replace("&", "_").Replace("(", "_").Replace(")", "_").Replace("^", "_");
        }

        public static string GetFirstChar(string sbuf, bool bReturnSignChar = true)
        {
            if (sbuf.Length > 0)
            {
                return sbuf.Trim().Substring(0, 1);
            }
            else
            {
                return "@";
            }
        }
        #endregion

    }
}

