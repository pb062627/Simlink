using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ModelSim;

namespace Optimization
{
    //optimization is the base class for an algorithm
    //an algorithm base class be derivated from optimization
    public class Optimization
    {
        #region MEMBERS
        // this is stored on the population member itself...        /public int _nPopulationSize;
        public Population _populatation;
        public int _nLoopNo = -1;                    //the number of loops (generations)
        public double _dBestObjective;
        //public double _dCurrentObjective;
        public int _nEvaluations;

        public bool _bOptDriveModel = true;         //true- sim executed from Opt  //false- objective array sent from 
        public DateTime _dtStartOpt;                //set to start time for tracking elapsed time
        public OptimizationEnum.AlgType _algType;               //what type of algorithm is being used
        // user may set more than one.... private OptimizationEnum.StoppingCriteria _StopType;        //  what type 
        public long _lStopCrit_ElaspsedSeconds = -1;               //-1 means NOT Applicable
        public double _dStopCritTolerance = -1;                    //-1 means NOT Applicable
        public double _dStopCritToleranceGA = -1;                  //-1 means NOT Applicable
        public double _dStopCritPercent = -1;                      //-1 means NOT Applicable
        public int _nStopCrit_NumberLoops = -1;                    //-1 means NOT Applicable
        public int _nPopulationSize = 100;            

        public bool _bStoppingCriteriaMet = false;
        public bool _bAbortOptimization = false;                //flag to be set if optimization must be aborted
        public bool _bMaximize = false;                         //set to true if we are tyring to maximize the function
        public bool _bIsSimLink = false;                        //whether links to SimLink
        public bool _bIsValidOpt = false;                       //check that minimum data reqs are met for opt
        public bool _bDEBUG_SetDNA_Copy = false;                //if true- use to set a DNA copy for easy tracking of DNA 

        //
        public ModelSim.ModelSim _modelsim = new ModelSim.ModelSim();
        public List<string> _lstPopulationRecord = new List<string>();
        private string _sLogDir = @"C:\Users\bmirghan\Development\2014\Optimization Development\C#OptimizationCodes\v02.1\";        // todo: read from config (Customize to your sys)
        bool _bLogging = false;
        public OptimizationEnum.LogPopulationLevel _logPopLevel = new OptimizationEnum.LogPopulationLevel();
         
        //event handler used for borg calls
        public EventHandler _evhProcessScenario;
        public Action _methodProcessScenario;                       //simLinkCall;
        public delegate void delProcessScenario(double[] vars, double[] objs, double[] constrs);
        public delProcessScenario _delProcessScenario;
     //   public static delProcessScenario _delProcessScenarioSTATIC;

        #endregion
        #region INIT
        //override in base class
        public virtual void InitOptimization(Dictionary<string, string> dictArgs)
        {

        }

        //4/1/14: require DNAbounds passed (otherwise has no meaning)
        //rmv DNA bit count which is redundat with DNA bounds array
        public void InitStartingPopulation(Dictionary<string, string> dictArgs, double[,] dDNAbounds)       //met 4/1/14  -int nDNA_BitCount, double[,] dDNAbounds = null)        //, double[] dInitPopulation = null, string[] sInitPopulation)
        {
            if (dictArgs.ContainsKey("population_size"))
            {
                _nPopulationSize = Convert.ToInt32(dictArgs["population_size"]);
            }

            _populatation = new Population();
            _populatation._lstDNA = new List<Individual>();
            int nDNA_BitCount = dDNAbounds.GetLength(0);
            for (int i = 0; i < _nPopulationSize; i++)
            {
                Individual ind = new Individual();
                ind._DNA_Array = new DNAbit[nDNA_BitCount];         //create an empty array of bits
                for (int j = 0; j < nDNA_BitCount; j++)
                {
                    DNAbit dna = new DNAbit();
                    //todo: validate the 2d array has all three elements (required)
                    if (dDNAbounds != null)                             //typically user will supply this array
                    {
                        dna.dMinVal = dDNAbounds[j, 0];       //bounds are the same for each member of the population
                        dna.dMaxVal = dDNAbounds[j, 1];
                        dna.dInterval = dDNAbounds[j, 2];
                    }
                    ind._DNA_Array[j] = dna;
                }
                _populatation._lstDNA.Add(ind);               //add the new array to a list of arrays
            }
            _populatation._nPopulationSize = _nPopulationSize;
        }

        //optionally, the user may load a starting population
        public void LoadStartingPopulation(double[,] dInitPopulation, double[] dVals)
        {
            // test that user has not provvided more Vals than there is population
            // note that it's ok to provide more initpop than dVals--> those would represent runs the user requests (which need to be evaluated)
            int nIntPopCount = dInitPopulation.GetLength(0);
            int nDNAbitCount = dInitPopulation.GetLength(1);
            if ((dVals.Length > nIntPopCount) || (nIntPopCount > _populatation._nPopulationSize))
            {
                Console.WriteLine("error in initial population");
            }
            else if (nDNAbitCount != _populatation._lstDNA[0]._DNA_Array.Length)
            {
                Console.WriteLine("bit val count does not match array size");
            }
            else
            {
                for (int i = 0; i < nIntPopCount; i++)
                {
                    for (int j = 0; j < nDNAbitCount; j++)
                    {
                        _populatation._lstDNA[i]._DNA_Array[j].dVal = dInitPopulation[i, j];
                    }
                    if (i < dVals.Length)             //dvals coulld be SHORTER
                        _populatation._lstDNA[i]._dObjective = dVals[i];
                }
            }
        }

        //performs two functions
        //init the stopping criteria
        //return true if at least one is set (there must be a stopping criteria
        //try catch added because the args could be lousy (if coming from CLI, linked program (non-ui)
        public bool InitStopCriteria(Dictionary<string, string> dictArgs)
        {
            bool bAtLeastOneStopCriteria = false;
            int nEliteParents;
            if (dictArgs.ContainsKey("stop_loops"))
            {
                _nStopCrit_NumberLoops = Convert.ToInt32(dictArgs["stop_loops"]);
                bAtLeastOneStopCriteria = true;
            }

            if (dictArgs.ContainsKey("stop_time"))
            {
                _lStopCrit_ElaspsedSeconds = Convert.ToInt32(dictArgs["stop_time"]);
                bAtLeastOneStopCriteria = true;
            }

            if (dictArgs.ContainsKey("stop_tolerance"))
            {
                _dStopCritTolerance = Convert.ToInt32(dictArgs["stop_tolerance"]);
                bAtLeastOneStopCriteria = true;
            }

            if (dictArgs.ContainsKey("stop_tolerance_GA"))
            {
                _dStopCritToleranceGA = Convert.ToInt32(dictArgs["stop_tolerance_GA"]);
                bAtLeastOneStopCriteria = true;
            }

            if (dictArgs.ContainsKey("stop_tolerance_percent"))
            {
                _dStopCritPercent = Convert.ToInt32(dictArgs["stop_tolerance_percent"]);
            }

            //set a default case if nothing passed
            if (!bAtLeastOneStopCriteria)
            {
                _lStopCrit_ElaspsedSeconds = 3600;      //todo: make easier to override defaults
                bAtLeastOneStopCriteria = true;
            }

            return bAtLeastOneStopCriteria;
        }
        #endregion

        #region Validation
        //todo- elaborate this functionality to avoid mistakes
        public virtual bool ValidateOptimization(bool bAtLeastOneStopCriteria)
        {
            bool bValid = bAtLeastOneStopCriteria;
            //todo: add additional checks
            return bValid;
        }

        #endregion

        #region RunOpt
        public virtual void ExecuteOptimization()           // rmv from virtual methods; do as much as possible in base class
        {
            bool bFirstPass = true;
            if (_bIsValidOpt)
            {
                _populatation.Fill();               //create initial population (for those not initialized)
                while (!StoppingCriteriaMet() && !_bAbortOptimization)
                {                    //only check
                    // 1: Any pre-processing  (non-init)
                    // 2: Execute Model   -excel, SimLink etc.  Will need to wait 
                    if (_bOptDriveModel && _nLoopNo == 0)
                    {
                        //Evaluate All Population
                        ExecuteSimulationsAll();
                    }
                    else
                    {
                        // send control back to calling application?
                        // TBD
                    }
                    _nLoopNo++;
                    //can put in dummy function for now ; in near future,  link this to Simlink

                    //2.5 track optimization between execute and evolve
                    LogOptimizationData();          //capture info for easy comparison of behavior (Set to none for increased speed)
                    // 3: Evolve The Population                                                                           
                    EvolvePopulation();

                    if (_bDEBUG_SetDNA_Copy)                    // skip if not needed (default)
                        _populatation.UpdateDNAStrings();       //note this skips the first round which is fine

                    //4: (optional) Visual ouput
                    //5: (optional) logging

                    bFirstPass = false;
                }
                WriteLogFile();

            }
            else
            {           //notify the user, etc etc

            }
        }
        public virtual void EvolvePopulation()
        {

        }

        #region ExecuteSimulations
        //simulation is responsible for returning its objective function
        //options
        //simlink- model linkage to numerous simulation packages
        //model - ad-hoc class for testing and/or specific problem solutions
        //cirrus- execute excel spreadsheet

        //for prelim testing, we just use model
        public void ExecuteSimulationsAll()
        {
            int nPopulationSize = _populatation._lstDNA.Count();
            for (int i = 0; i < nPopulationSize; i++)
            {
                double[] dDNA = _populatation._lstDNA[i].GetBitsAsArray();
                double[] dObj = new double[1];        //allows overloading with MOEAs
              //  _populatation._lstDNA[i]._dObjective = 
                SimLink_ProcessScenario(dDNA, dObj, null);                 //_modelsim.ModelExecute(dDNA);
                _populatation._lstDNA[i]._dObjective = dObj[0];     //only single objective supported
                _nEvaluations++;
            //    Console.WriteLine("Objective: " + _populatation._lstDNA[i]._dObjective + ", DNA: " + _populatation._lstDNA[i].GetDNABitsAsString() + ", Evaluation No: " + _nEvaluations + ", Loop No: " + _nLoopNo);
            }
        }

        // similar to how borg is called
        public void SimLink_ProcessScenario(double[] vars, double[] objs, double[] constrs)
        {
            _delProcessScenario.Invoke(vars, objs, constrs);            // call delegate
        }

        public void ExecuteSimulationsOnce(int Member)
        {
            double[] dDNA = _populatation._lstDNA[Member].GetBitsAsArray();
            _populatation._lstDNA[Member]._dObjective = _modelsim.ModelExecute(dDNA);
            _nEvaluations++;
        }


        #endregion

        #endregion
        #region LOGGiNG


        //for now called after STOP- better to call incrementally and flush the buffer
        public void WriteLogFile(bool bClearLog = true)
        {
            if (_logPopLevel != OptimizationEnum.LogPopulationLevel.NONE && _bLogging)
            {
                string[] s = _lstPopulationRecord.ToArray();
                string sFile = Directory.GetCurrentDirectory() + "OptLog_" + CommonUtilities.RMV_FixFilename(System.DateTime.Now.ToString()) + ".log";
                File.WriteAllLines(sFile, s);

                if (bClearLog)
                    _lstPopulationRecord.Clear();
            }
        }

        public void LogOptimizationData()
        {
            if (_logPopLevel == OptimizationEnum.LogPopulationLevel.NONE)
                return;
            else if (_logPopLevel == OptimizationEnum.LogPopulationLevel.ALL)
            {
                for (int i = 0; i < _populatation._lstDNA.Count; i++)
                {
                    string sOut = _nLoopNo + "," + i + "," + _populatation._lstDNA[i].GetDNABitsAsString() + "," + _populatation._lstDNA[i]._dObjective;
                    _lstPopulationRecord.Add(sOut);
                }
            }
            else if (_logPopLevel == OptimizationEnum.LogPopulationLevel.BEST)
            {
                //todo add this
            }
        }
        #endregion


        #region StoppingCriteria

        //met- may be necessary to override for specific algorithm; try to avoid if possible
        public bool StoppingCriteriaMet()
        {
            bool bReturn = false;
            if (_nLoopNo > 1)
            {
                if (_nStopCrit_NumberLoops != -1)               // test on loops if that flag is set
                {
                    if (_nLoopNo >= _nStopCrit_NumberLoops)
                        bReturn = true;
                }

                if (_lStopCrit_ElaspsedSeconds != -1)
                {
                    TimeSpan tsInterval = DateTime.Now - _dtStartOpt;
                    long lSecondsElapsed = Convert.ToInt64(tsInterval.TotalSeconds);
                    if (lSecondsElapsed >= _lStopCrit_ElaspsedSeconds)
                        bReturn = true;
                }

                if (_dStopCritTolerance != -1)
                {
                    int nRank = Convert.ToInt32(Math.Floor(_populatation._nPopulationSize * _dStopCritPercent / 100) - 1);
                    double dEvaluationObjective = _populatation._lstDNA[nRank]._dObjective;
                    if (Math.Abs(_dBestObjective - dEvaluationObjective) <= _dStopCritTolerance)
                        bReturn = true;
                }

                if (_dStopCritToleranceGA != -1)
                {
                    int nRank = Convert.ToInt32(Math.Floor(_populatation._nPopulationSize * _dStopCritPercent / 100) - 1);
                    //Mason: I need to pass number of elite here
                    double dEvaluationObjective = _populatation._lstDNA[nRank - 2]._dObjective;
                    if (Math.Abs(_dBestObjective - dEvaluationObjective) <= _dStopCritToleranceGA)
                        bReturn = true;
                }
            }
            return bReturn;
        }

        #endregion

    }
}

