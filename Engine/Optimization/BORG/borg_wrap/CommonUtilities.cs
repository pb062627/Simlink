using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Optimization
{
    public class CommonUtilities
    {
        #region File IO

        /// <summary>
        /// Reads in simple file of format below for defining decision variable limits.
        /// Todo: handle definition of real vs integer vals
        /// </summary>
        /// <param name="sFile"></param>
        /// <returns></returns>
        public static double[,] ReadDVLimits(string sFile)
        {
            if (File.Exists(sFile)){
                string[] sLines = File.ReadAllLines(sFile);
                double[,] dReturn = new double[sLines.Length, 2];
                for (int i = 0; i < sLines.Length; i++)
                {
                    string[] sBuf = sLines[i].Split(',');
                    dReturn[i, 0] = Convert.ToDouble(sBuf[0]);          //, 0);
                    dReturn[i, 1] = Convert.ToDouble(sBuf[1]); 
                }
                return dReturn;
            }
            else{
                //todo: handle this somehow
                return null;
            }

        }

        #endregion

        #region ARGUMENTS
        public static Dictionary<string, string> Arguments_ToDict(string[] Args)                          //adapted from: http://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>();       // 
            //  StringDictionary Parameters;
            //  Parameters = new StringDictionary();
            Regex Spliter = new Regex(@"^-{1,2}|^/|=",                          // met replace 8/28/2013 to allow req statement (@"^-{1,2}|^/|=",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);             //orig (@"^-{1,2}|^/|=|:",

            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string Parameter = null;
            string[] Parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string Txt in Args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                Parts = Spliter.Split(Txt, 3);

                switch (Parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[0] =
                                    Remover.Replace(Parts[0], "$1");

                                Parameters.Add(Parameter, Parts[0]);
                            }
                            Parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }
                        Parameter = Parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (Parameter == "req")
                            {
                                Parameters.Add(Parameter, Txt);         //met 8/28/2013: hard-coded crutch due to not understanding regex
                            }
                            else if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }

                        if (Parameter != "req") //met 8/28/2013: hard-coded crutch due to not understanding regex
                        {


                            Parameter = Parts[1];

                            // Remove possible enclosing characters (",')
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[2] = Remover.Replace(Parts[2], "$1");
                                Parameters.Add(Parameter, Parts[2]);
                            }
                        }
                        Parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (Parameter != null)
            {
                if (!Parameters.ContainsKey(Parameter))
                    Parameters.Add(Parameter, "true");
            }

            return Parameters;
        }
        #endregion

    }
}
