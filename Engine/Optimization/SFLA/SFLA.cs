using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    public class SFLA : Optimization                        //derives from base optimization class
    {
        private int _nMemeplexQuantity;
        private int _nMemeplexSize;
        private int _nSubMemeplexSize;
        private int _nSubMemeplexEvolutions;
        private double _dMaxJumpFraction;
        private int _nJumpFactor;
        private double _dBoundaryProbability;
        private bool _bIsDiscreteVals = true;                                                   //todo: handle non-discrete cases also.
        private double[,] _dSubMemeplexDA;
        private double[,] _dMemberDA;                                                       
        private LinkedList<int> _lMemeplex = new LinkedList<int>();                             //MemeplexDA
        private int _nBestMember;                                                               
        private long _lSecondsElapsedSimulation;                                                //for debuging purposes
        private int nMissEvaluation = 0;                                                        //for debuging purposes
        private int nTest;                                                                      //for debuging purposes
       

        #region defaults
        //met 3/31/2014: these are constants that default the params
        //todo: would be good to set using an ini
        private  double _dMaxJumpFractionDEFAULT = 0.5;                                                                        
        private  int _nJumpFactorDEFAULT = 2;
        private const double _dBoundaryProbabilityDEFAULT = 0.3;
        private const int _nMemeplexQuantityDEFAULT = 20;        //Assume Population 80, Memplexes Quantity = 10 and Memplexes Size = 8     
        private const int _nMemeplexSizeDEFAULT = 10;
        private const int _nSubMemeplexSizeDEFAULT = 5;
        private const int _nSubMemeplexEvolutionsDEFAULT = 3;
        #endregion

        #region INIT
        //override in base class
        //
        public override void InitOptimization(Dictionary<string, string> dictArgs)
        {
            _nLoopNo = 0;
            _algType = OptimizationEnum.AlgType.SFLA;

            //initialize the Stopping Criteria
            bool bAtLeastOneStopCriteria = InitStopCriteria(dictArgs);
            //initialize SFLA specific params
            InitSFLA_Params(dictArgs);
            //initialize SFLA specific defaults
            SetSFLA_Defaults(dictArgs);
            _dSubMemeplexDA = new double[_nSubMemeplexSize, 2];
            _dtStartOpt = System.DateTime.Now;
            _bIsValidOpt = ValidateOptimization(bAtLeastOneStopCriteria);
        }

        private bool InitSFLA_Params(Dictionary<string, string> dictArgs)
        {
            return false;   
        }
        //set SFLA vars using dictionary
        //alternatively, one can set the SFLA objects directly
        // this function allows user to use dictionary, or automaically get the defaults
        //try catch to avoid easy errors in dictionary
        private bool SetSFLA_Defaults(Dictionary<string, string> dictArgs)
        {
            bool bReturnValid = true;
            try
            {
                _dMaxJumpFraction = _dMaxJumpFractionDEFAULT;               //set defaults first, so if error on dictionary (from cli, excel, elsewhere), we can 1)warn the user 2) proceed
                _nJumpFactor = _nJumpFactorDEFAULT;
                _dBoundaryProbability = _dBoundaryProbabilityDEFAULT;
                _nMemeplexQuantity = _nMemeplexQuantityDEFAULT;
                _nMemeplexSize = _nMemeplexSizeDEFAULT;
                _nSubMemeplexSize = _nSubMemeplexSizeDEFAULT;
                _nSubMemeplexEvolutions = _nSubMemeplexEvolutionsDEFAULT;

                if (dictArgs.ContainsKey("max_jump_fraction"))
                {
                    _dMaxJumpFraction = Convert.ToDouble(dictArgs["max_jump_fraction"]);
                }

                if (dictArgs.ContainsKey("jump_factor"))
                {
                    _nJumpFactor = Convert.ToInt32(dictArgs["jump_factor"]);
                }

                if (dictArgs.ContainsKey("boundary_probability"))
                {
                    _dBoundaryProbability = Convert.ToDouble(dictArgs["boundary_probability"]);
                }

                if (dictArgs.ContainsKey("memeplex_quantity"))
                {
                    _nMemeplexQuantity = Convert.ToInt32(dictArgs["memeplex_quantity"]);
                }

                if (dictArgs.ContainsKey("memeplex_size"))
                {
                    _nMemeplexSize = Convert.ToInt32(dictArgs["memeplex_size"]);                       //added sub_memeplex_size
                }

                if (dictArgs.ContainsKey("submemeplex_size"))
                {
                    _nSubMemeplexSize = Convert.ToInt32(dictArgs["submemeplex_size"]);                 
                }

                if (dictArgs.ContainsKey("memeplex_evolutions"))
                {
                    _nSubMemeplexEvolutions = Convert.ToInt32(dictArgs["memeplex_evolutions"]);
                }
            }
            catch (Exception ex)
            {
                bReturnValid = false;
                //todo: log the issue
                Console.WriteLine("error in sfla customization from dictionary; optimization will proceed with default vals where necessary");
            }
            return bReturnValid;
        }

        #endregion

        #region Validation
        //todo- elaborate this functionality to avoid mistakes
        public override bool ValidateOptimization(bool bAtLeastOneStopCriteria)
        {
            bool bValid = base.ValidateOptimization(bAtLeastOneStopCriteria);

            // this should not be buried in a validate function  _dSubMemeplexDA = new double[_nSubMemeplexSize, 2];

            //todo: add additional SFLA specific checks
            return bValid;

        }

        #endregion

        #region RunOpt


        public override void EvolvePopulation()
        {
            int nLocalBestFrog; int nLocalWorstFrog; double dLocalWorstObjective; double dLocalBestObjective;     //todo: comment diff btwn the two latter?  
            bool bEvolve;                                                                                                                           
            //  while (!StoppingCriteriaMet() && !_bAbortOptimization)                  //met- consider moving this into calling function: ExecuteOptimization
            //  {
            if (_nLoopNo == 1)
            {
                SortVirtualPopulation();
                _dBestObjective = _populatation._lstDNA[0]._dObjective;
                Console.WriteLine("BestObjective: " + _dBestObjective + ", DNA: " + _populatation._lstDNA[0].GetDNABitsAsString());
            }
            
            for (int m = 0; m < _nMemeplexQuantity; m++)
            {
                CreateMemeplex(m);
                for (int e = 0; e < _nSubMemeplexEvolutions; e++)
                { 
                    SortSubMemeplex();
                    SubMemeplexIdentify(out nLocalBestFrog, out nLocalWorstFrog, out dLocalBestObjective, out dLocalWorstObjective);            //BYM added out dLocalBestObjective;
                    bEvolve = true;
                    if (dLocalBestObjective == dLocalWorstObjective)
                    {
                        if (DNAMAtch(nLocalBestFrog, nLocalWorstFrog) == 1)
                        {
                            bEvolve = false;
                            nMissEvaluation++;
                        }                                                                                                            
                    }                                                                                                                           

                    if (bEvolve)                                                                                                                  
                    {                                                                                                                                  
                        double dCurrentObjective = ProcessFrog(nLocalBestFrog, nLocalWorstFrog, dLocalWorstObjective);                       //wraps around the FrogJump calls
                        CheckIfBestMember(nLocalWorstFrog, dCurrentObjective);
                        ReturnFrogIntoPopulation(nLocalWorstFrog, dCurrentObjective);                                                                                                                                 
                        _dSubMemeplexDA[_nSubMemeplexSize - 1, 1] = dCurrentObjective;
                       //Console.WriteLine("CurrentObjective: " + dCurrentObjective + ", BestObjective: " + _dBestObjective + ", m: " + m + ", e: " + e + ", DNA: " + _populatation._lstDNA[nLocalWorstFrog].GetDNABitsAsString() + ", Evaluation No: " + _nEvaluations + ", Loop No: " + _nLoopNo + ", missing No: " + nMissEvaluation);                             
                    }                                                                                                             
                }
            }                                                                                   
            SortVirtualPopulation();
            Console.WriteLine(" BestObjective: " + _dBestObjective + ", DNA: " + _populatation._lstDNA[0].GetDNABitsAsString() + ", Evaluation No: " + _nEvaluations + ", Loop No: " + _nLoopNo + ", missing No: " + nMissEvaluation);                                                                                 
            //}             //end while
        }

        #endregion

        #region Population

        //this function is to create both Memeplex and SubMemeplex                                                                                                                    
        private void CreateMemeplex(int nMemeplexID)
        {
            //this is to create Memeplex
            _lMemeplex.Clear();
            int f = 0;
            while (f * _nMemeplexQuantity + nMemeplexID < _populatation._nPopulationSize)
            {
                _lMemeplex.AddLast(f * _nMemeplexQuantity + nMemeplexID);
                f++;
            }
            //this is to create SubMemeplex
            LoadSubMemeplex();            
        }

        //this function is to create SubMemeplex                                                                                                                 
        private void LoadSubMemeplex()
        {
            int nNoInMemeplex; int nMemeplexSlot; int nChosenFrog;
            for (int i = 0; i < _nSubMemeplexSize; i++)
            {
                var node = _lMemeplex.First;
                nNoInMemeplex = _lMemeplex.Count;                                                                                               
                nMemeplexSlot = (CommonUtilities.GetValFromTriangularDistribution(Convert.ToDouble(nNoInMemeplex))) - 1;                                         
                nChosenFrog = _lMemeplex.ElementAt(nMemeplexSlot);                                                                              
                for (int j = 0; j < nNoInMemeplex; j++)
                {
                    var next = node.Next;
                    if (j == nMemeplexSlot)
                    {
                        _lMemeplex.Remove(node);
                        break;
                    }
                    else
                    {
                        node = next;
                    }
                }
                //put chosen frog index and its objective in submemeplex
                _dSubMemeplexDA[i, 0] = nChosenFrog;
                _dSubMemeplexDA[i, 1] = _populatation._lstDNA[nChosenFrog]._dObjective;                                     
            }
            //if (Convert.ToDouble(_dSubMemeplexDA[0, 1]) == Convert.ToDouble(_dSubMemeplexDA[_nSubMemeplexSize - 1, 1]))
            //    nTest = 0;  

        }

        //this is to sort SubMemeplex
        private void SortSubMemeplex()                                                                     
        {
            double[] dim1 = new double[_nSubMemeplexSize];          
            double[] dim2 = new double[_nSubMemeplexSize];              
            for (int i = 0; i < _dSubMemeplexDA.GetLength(0); i++)
            {
                dim1[i] = _dSubMemeplexDA[i, 0];
                dim2[i] = _dSubMemeplexDA[i, 1];
            }
            Array.Sort(dim2, dim1);
            if (_bMaximize)
            {
                Array.Reverse(dim1);
                Array.Reverse(dim2);
            }
            for (int i = 0; i < _dSubMemeplexDA.GetLength(0); i++)
            {
                _dSubMemeplexDA[i, 0] = dim1[i];
                _dSubMemeplexDA[i, 1] = dim2[i];
            }
        }                                                                                                                                       

        //for code clarity                                                                     
        private void SubMemeplexIdentify(out int nLocalBestFrog, out  int nLocalWorstFrog, out double dLocalBestObjective, out double dLocalWorstObjective)//BYM added , out double dLocalBestObjective
        {
            //make first jump attempt = local
            nLocalBestFrog = Convert.ToInt32(_dSubMemeplexDA[0, 0]);                                                                            
            nLocalWorstFrog = Convert.ToInt32(_dSubMemeplexDA[_nSubMemeplexSize - 1, 0]);                                                 
            dLocalBestObjective = Convert.ToDouble(_dSubMemeplexDA[0, 1]); //Convert.ToDouble(_populatation._lstDNA[nLocalBestFrog]._dObjective);
            dLocalWorstObjective = Convert.ToDouble(_dSubMemeplexDA[_nSubMemeplexSize - 1, 1]); //Convert.ToDouble(_populatation._lstDNA[nLocalWorstFrog]._dObjective); 
        }

        private double DNAMAtch(int nLocalBestFrog, int nLocalWorstFrog)
        {
            double tmp;
            int nLen = _populatation._lstDNA[0]._DNA_Array.Length;
            int nMatch = 0;
            for (int p = 0; p < nLen; p++)
            {
                if (_populatation._lstDNA[nLocalBestFrog]._DNA_Array[p].dVal == _populatation._lstDNA[nLocalWorstFrog]._DNA_Array[p].dVal) nMatch++;
            }
            return (Convert.ToDouble(nMatch) / Convert.ToDouble(nLen));
        }


        private double ProcessFrog(int nLocalBestFrog, int nLocalWorstFrog, double dLocalWorstObjective) // change from double nLocalWorstObjective to dLocalWorstObjective
        {
            //start with a local frog leap                                                                                                 
            int nGlobalBestFrog = 0;
            bool bOutofBounds = FrogJump(nLocalBestFrog, nLocalWorstFrog);
            bool bWorseLocation = false;
            double dObjective = -1;
            //evaluate jump result
            int nMemberNumber = nLocalWorstFrog;
            if (!bOutofBounds)
            {
                dObjective = EvaluateFitness(nLocalWorstFrog);                                                                                             
                bWorseLocation = MemberIsWorse(dObjective, dLocalWorstObjective); 
            }
            //if first jump is unsatisfactory make another jump = global jump
            if (bOutofBounds || bWorseLocation)
            {
                bOutofBounds = FrogJump(nGlobalBestFrog, nLocalWorstFrog);           //global frog leap                        
                //evaluate jump result
                if (!bOutofBounds)
                {
                    dObjective = EvaluateFitness(nLocalWorstFrog);                                                                           
                    bWorseLocation = MemberIsWorse(dObjective, dLocalWorstObjective);                                                   
                }
            }
            //if second jump is unsatisfactory create a random frog
            if (bOutofBounds || bWorseLocation)
            {
                //Generate_random_member();                                                      
                GenerateRandomMember();
                dObjective = EvaluateFitness(nLocalWorstFrog);                                                                                        
            }
            return dObjective;
        }

        //_lMemeplex replaces MemeplexDA (Queu, right?)
        //_dSubMemeplexDA replaces SubMemeplexDA
        //ParameterDefinitionsDA[p][0] is replaced by info on the dna object,like-->_populatation._lstDNA[i]._DNA_Array[j].dMinVal  (dMaxVal, dInterval (usually 1)...
        //suggest we work on 'integer'/discrete version first, while keep in mind continuous functions..
        private bool FrogJump(int nBestFrog, int nWorstFrog)        
        {
            double pb, pw;
            double dJump, dMaxJump, dUpperBound, dLowerBound, dRandomJ, dRandom;
            Random r = new Random(DateTime.Now.Millisecond); //
            bool bOutOfBounds = false;
            int nLen = _populatation._lstDNA[0]._DNA_Array.Length;
            _dMemberDA = new double[nLen,2];
            
            for (int p = 0; p < nLen; p++)
            {
                pb = _populatation._lstDNA[nBestFrog]._DNA_Array[p].dVal;
                pw = _populatation._lstDNA[nWorstFrog]._DNA_Array[p].dVal;
                if (_bIsDiscreteVals) dLowerBound = _populatation._lstDNA[0]._DNA_Array[p].dMinVal;//1;
                else dLowerBound = 0;
                
                //For Discrete variables Upper Bound = 1, while for continous variables Upper Bound need to be set
                //if (_bIsDiscreteVals) 
                dUpperBound = _populatation._lstDNA[0]._DNA_Array[p].dMinVal; //1
                //else dUpperBound = TBD;

                if (pb >= pw) dMaxJump = dUpperBound - pw;
                else dMaxJump = dLowerBound - pw; //negative jump
                dMaxJump *= _dMaxJumpFraction;

                if (_bIsDiscreteVals)
                {
                    if (dMaxJump-Math.Floor(dMaxJump) >= 0.5) dMaxJump = Math.Floor(dMaxJump) + 1;
                    else dMaxJump = Math.Floor(dMaxJump);
                }
                
                dRandomJ = r.NextDouble();
                if (_bIsDiscreteVals)
                {
                    dJump = _nJumpFactor * dRandomJ * (pb - pw);
                    if (dJump-Math.Floor(dJump) >= 0.5) dJump = Math.Floor(dJump) + 1;
                    else dJump = Math.Floor(dJump);
                }
                else
                {
                    dJump = _nJumpFactor * dRandomJ * (pb - pw);
                }
                
                if (dJump < 0) dJump = Math.Max(dJump, dMaxJump);
                else dJump = Math.Min(dJump, dMaxJump);
                _dMemberDA[p,0] = pw + dJump;

                if (_dMemberDA[p,0] < dLowerBound || _dMemberDA[p,0] > dUpperBound)
                {
                    dRandom = r.NextDouble();
                    if (dRandom < _dBoundaryProbability / 100.0)
                    {
                        _dMemberDA[p,0] = Math.Min(_dMemberDA[p,0], dUpperBound);
                        _dMemberDA[p,0] = Math.Max(_dMemberDA[p,0], dLowerBound);
                    }
                    else bOutOfBounds = true;
                }
            }
            return bOutOfBounds;
        }

        //to evaluate frog fitness similar to evaluate_fitness()
        private double EvaluateFitness(int nWorstFrog)
        {
            int p, c, nConnections;
            double Obj = 0;
            int nMember = _populatation._nPopulationSize - 1;
            int nLen = _populatation._lstDNA[0]._DNA_Array.Length;

            for (p = 0; p < nLen; p++)
            {
                _dMemberDA[p, 1] = _populatation._lstDNA[nWorstFrog]._DNA_Array[p].dVal; 
                _populatation._lstDNA[nMember]._DNA_Array[p].dVal = _dMemberDA[p,0];     
            }
            nConnections = 1;                                                       //for now, assume we have only one connector       
            
            //int[,] Temp1DV_DA;
            //int nCurrentDVs;
            //nCurrentDVs = nLen;             
            //Temp1DV_DA = new int[nCurrentDVs, 2];
            //for (c = 0; c < nConnections; c++)
            //{
            //    for (p = 0; p < nCurrentDVs; p++)
            //    {
            //        if (c == 0)
            //        {
            //            Temp1DV_DA[p, 1] = Convert.ToInt32(_populatation._lstDNA[nMember]._DNA_Array[p].dVal);
            //        }
            //        else
            //        {
            //            //Temp1DV_DA[p, 2] = _nParameterDefinitionsDA[_nLen-nCurrentDVs + p, 2];
            //        }
            //    }
            //}

            ExecuteSimulationsOnce(nMember);                                                                                        
            for (c = 0; c < nConnections; c++)
            {
                Obj += _populatation._lstDNA[nMember]._dObjective;                                                                                             
            }
            UpdateTimer();
            return Obj;
        }

        //
        private void UpdateTimer()
        {
            TimeSpan tsInterval = DateTime.Now - _dtStartOpt;
            _lSecondsElapsedSimulation = Convert.ToInt64(tsInterval.TotalSeconds);
        }

        //to check if the frog make any improvement formerly check_for_member_improvement
        private bool MemberIsWorse(double dNewObjective, double dExistingObjective)
        {
            bool bIsWorseLocation = false;
            if (_bMaximize && (dNewObjective < dExistingObjective))  bIsWorseLocation = true;
            else if (!_bMaximize && (dNewObjective > dExistingObjective)) bIsWorseLocation = true; 
            else bIsWorseLocation = false;
            return bIsWorseLocation;
        }

        // to generate random members (frog) formerly generate_random_member()
        private void GenerateRandomMember()
        {
            Random random = new Random(DateTime.Now.Millisecond);                                   //for a unique random seeds                                                                          
            int nLen = _populatation._lstDNA[0]._DNA_Array.Length;
            if (_bIsDiscreteVals)
            {
                for (int p = 0; p < nLen; p++)
                {
                    int max = Convert.ToInt32(_populatation._lstDNA[0]._DNA_Array[p].dMaxVal+ 1);
                    int min = Convert.ToInt32(_populatation._lstDNA[0]._DNA_Array[p].dMinVal);
                    _dMemberDA[p,0] = random.Next(min, max);
                }
            }
            else
            {
                //for continuous DV
            }
        }

        //
        private void CheckIfBestMember(int nLocalWorstFrog, double dObjective)                  
        {
            if (!_bMaximize && (dObjective < _dBestObjective) || _bMaximize && (dObjective > _dBestObjective))
            {
                _dBestObjective = dObjective;
                _nBestMember = nLocalWorstFrog;
            }
        }

        //to return frog to the pool similar to return_member_into_population
        private void ReturnFrogIntoPopulation(int nLocalWorstFrog, double dObjective)                                        
        {
            int nLen = _populatation._lstDNA[0]._DNA_Array.Length;
            for (int p = 0; p < nLen; p++)
            {
                _populatation._lstDNA[nLocalWorstFrog]._DNA_Array[p].dVal = _populatation._lstDNA[_populatation._nPopulationSize-1]._DNA_Array[p].dVal;
                _populatation._lstDNA[_populatation._nPopulationSize - 1]._DNA_Array[p].dVal = _dMemberDA[p, 1];
            }
            _populatation._lstDNA[nLocalWorstFrog]._dObjective = dObjective;
        }

        //formerly
        private void SortVirtualPopulation()                                                                                      
        {
            _populatation.SortIndividuals(_bMaximize);
        }

        #endregion

        #region StoppingCriteria
        #endregion
    }
}
