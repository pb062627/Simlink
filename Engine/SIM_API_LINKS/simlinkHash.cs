using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SIM_API_LINKS
{
    /// <summary>
    /// Developed to support cloning, this class makes it easy to request specific simlink types from the cloned and new db
    /// </summary>
    public class simlinkHash
    {
        public Hashtable _hashEG = new Hashtable();
        public Hashtable _hashMEV= new Hashtable();
        public Hashtable _hashResultVar = new Hashtable();
        public Hashtable _hashResultTS = new Hashtable();
        public Hashtable _hashEvent = new Hashtable();
        public Hashtable _hashPerformance = new Hashtable();
        public Hashtable _hashOption = new Hashtable();
        public Hashtable _hashElement = new Hashtable();
        public Hashtable _hashFunction = new Hashtable();
        public Hashtable _hashConstant = new Hashtable();
        public Hashtable _hashScenario = new Hashtable();
        
        public void InsertHashVal(string sVal, int nSourceVal, int nNewVal){
            switch(sVal.ToLower()){
                case "scenario":
                    _hashScenario.Add(nSourceVal, nNewVal);
                    break;
                case "eg":
                    _hashEG.Add(nSourceVal, nNewVal);
                    break;
                case "dv":
                    _hashMEV.Add(nSourceVal, nNewVal);
                    break;
                case "result":
                    _hashResultVar.Add(nSourceVal, nNewVal);
                    break;
                case "resultts":
                    _hashResultTS.Add(nSourceVal, nNewVal);
                    break;
                case "event":
                    _hashEvent.Add(nSourceVal, nNewVal);
                    break;
                case "performance":
                    _hashPerformance.Add(nSourceVal, nNewVal);
                    break;
                case "option":
                    _hashOption.Add(nSourceVal, nNewVal);
                    break;
                case "element":
                    _hashElement.Add(nSourceVal, nNewVal);
                    break;
                case "function":
                    _hashFunction.Add(nSourceVal, nNewVal);
                    break;
                case "constant":
                    _hashConstant.Add(nSourceVal, nNewVal);
                    break;
            }
        }

        public int RetrieveHashVal(string sVal, int nSourceVal)
        {
            int nReturn = -1;
            try
            {
                switch (sVal.ToLower())
                {
                    case "scenario":
                        nReturn = (int)_hashScenario[nSourceVal];
                        break;
                    case "eg":
                        nReturn = (int)_hashEG[nSourceVal];
                        break;
                    case "dv":
                        nReturn = (int)_hashMEV[nSourceVal];
                        break;
                    case "result":
                        nReturn = (int)_hashResultVar[nSourceVal];
                        break;
                    case "resultts":
                        nReturn = (int)_hashResultTS[nSourceVal];
                        break;
                    case "event":
                        nReturn = (int)_hashEvent[nSourceVal];
                        break;
                    case "performance":
                        nReturn = (int)_hashPerformance[nSourceVal];
                        break;
                    case "option":
                        nReturn = (int)_hashOption[nSourceVal];
                        break;
                    case "element":
                        nReturn = (int)_hashElement[nSourceVal];
                        break;
                    case "function":
                        nReturn = (int)_hashFunction[nSourceVal];
                        break;
                    case "constant":
                        nReturn = (int)_hashConstant[nSourceVal];
                        break;
                }
                return nReturn;
            }
            catch (Exception ex)
            {
                //todo: log the issue
                Console.WriteLine("simlink inconsistrncy object {0} code {1}", sVal, nSourceVal);
                return CommonUtilities._nCLONE_ERROR_CODE;       // 
            }
        }
    }
}