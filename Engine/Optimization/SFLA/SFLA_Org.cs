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
        private bool _bIsDiscreteVals = true;           //todo: handle non-discrete cases also.
        private double[,] _dSubMemeplexDA;               //m
        //private Queue<int> _qMemeplex = new Queue<int>();                                     //BYM            
        private LinkedList<int> _lMemeplex = new LinkedList<int>();                             //BYM this is the  MemberDA
        private int _nBestMember;                                                               //BYM
        private int _nEvaluations;                                                              //BYM can be deleted
        private long _lSecondsElapsedSimulation;                                                //BYM can be deleted

        #region defaults
        //met 3/31/2014: these are constants that default the params
        //todo: would be good to set using an ini
        private const double _dMaxJumpFractionDEFAULT = 0.5;                                                                                //BYM
        private const int _nJumpFactorDEFAULT = 2;
        private const double _dBoundaryProbabilityDEFAULT = 0.3;
        private const int _nMemeplexQuantityDEFAULT = 4;        //Assume Population 8, Memplexes Quantity = 4 and Memplexes Size = 2     
        private const int _nSubMemeplexSizeDEFAULT = 2;
        private const int _nSubMemeplexEvolutionsDEFAULT = 2;
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

            
            SetSFLA_Defaults(dictArgs);
            _dSubMemeplexDA = new double[_nSubMemeplexSize, 2];
            _dtStartOpt = System.DateTime.Now;
            _bIsValidOpt = ValidateOptimization(bAtLeastOneStopCriteria);
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
                    _nSubMemeplexSize = Convert.ToInt32(dictArgs["memeplex_size"]);
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
            int nLocalBestFrog; int nLocalWorstFrog; double dLocalWorstObjective; double dLocalBestObjective;     //todo: comment diff btwn the two latter? //BYM added double dLocalBestObjective; 
            bool bEvolve;                                                                                                                           //BYM
            //  while (!StoppingCriteriaMet() && !_bAbortOptimization)                  //met- consider moving this into calling function: ExecuteOptimization
            //  {
            for (int m = 0; m < _nMemeplexQuantity; m++)
            {
                CreateMemeplex(m);
                for (int e = 0; e < _nSubMemeplexEvolutions; e++)
                { 
                    SortSubMemeplex();
                    SubMemeplexIdentify(out nLocalBestFrog, out nLocalWorstFrog, out dLocalWorstObjective, out dLocalBestObjective);            //BYM added out dLocalBestObjective;
                    bEvolve = true;
                    if (nLocalWorstFrog == nLocalBestFrog)
                    {                                                                                     //BYM 
                        if (DNAMAtch(nLocalBestFrog, nLocalWorstFrog) == 1) bEvolve = false;                                                    //BYM                                                                                                            
                    }                                                                                                                           //BYM

                    if (bEvolve)                                                                                                                //BYM  
                    {                                                                                                                           //BYM       
                        double dCurrentObjective = ProcessFrog(m, nLocalBestFrog, nLocalWorstFrog, dLocalWorstObjective);                       //wraps around the FrogJump calls
                        //baha todo: translate this function and add                check_if_best_member(); CheckIfBesN_Member()                //Done! BYM
                        CheckIfBestMember(nLocalWorstFrog, dCurrentObjective);                                         //BYM
                        //baha todo: translate this function and add                return_frog_into_population(nLocalWorstFrog);               //Done! BYM
                        ReturnFrogIntoPopulation(nLocalWorstFrog, dCurrentObjective);                                                           //BYM                                                                       
                        _dSubMemeplexDA[_nSubMemeplexSize - 1, 1] = dCurrentObjective;
                    }                                                                                                                           //BYM      
                }
            }
            //baha todo       sort_virtual_population();                                                                                        //Done! BYM
            SortVirtualPopulation();                                                                                                            //BYM
            //}             //end while
        }

        #endregion

        #region Population
        //to create Memeplex                                                                                                                    //BYM
        private void CreateMemeplex(int nMemeplexID)
        {               //todo: replaces create mem and submem
            //BAHA TODO: please check carefully that this works as intended (untested)                                                          //Done! BYM
            _lMemeplex.Clear();
            int f = 0;
            while (f * _nMemeplexQuantity + nMemeplexID < _populatation._nPopulationSize)
            {
                //_qMemeplex.Enqueue(f * _nMemeplexQuantity + nMemeplexID);                                                                     //BYM
                _lMemeplex.AddLast(f * _nMemeplexQuantity + nMemeplexID);
                f++;
            }
            //to create SubMemeplex                                                                                                             //BYM
            LoadSubMemeplex();
        }

        //to create SubMemeplex                                                                                                                 //BYM
        private void LoadSubMemeplex()
        {
            int nNoInMemeplex; int nMemeplexSlot; int nChosenFrog;
            for (int i = 0; i < _nSubMemeplexSize; i++)
            {
                var node = _lMemeplex.First;
                //nNoInMemeplex = -1; // baha todo: get this from 'queue'- i think this will be a list?                                         //Done!BYMT 
                nNoInMemeplex = _lMemeplex.Count;                                                                                               //BYM
                nMemeplexSlot = CommonUtilities.GetValFromTriangularDistribution(nNoInMemeplex) - 1;
                //int nChosenFrog = -1;       //baha todo: resolve this QueGetN(MemeplexDA, nChosenFrog);                                       //Done!BYM       
                nChosenFrog = _lMemeplex.ElementAt(nMemeplexSlot);                                                                              //BYM
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
                //_lMemeplex
                //////_lMemeplex.Remove(nMemeplexSlot);                                                                                               //BYM    
                //put chosen frog index and its objective in submemeplex
                _dSubMemeplexDA[i, 0] = nChosenFrog;
                //_dSubMemeplexDA[i, 1] = -1;//baha todo: resolve this GAGetReal(PopulationIndex, nChosenFrog, 1);//baha todo: resolve this     //Done!  BYM
                _dSubMemeplexDA[i, 1] = Convert.ToDouble(_populatation._lstDNA[nChosenFrog]._dObjective);                                       //BYM
            }
        }

        private void SortSubMemeplex()      //baha todo: replace sort_submemeplex                                                               //BYM*
        {
            double[] dim1 = new double[_nSubMemeplexSize];          //  _dSubMemeplexDA.GetLength(0)];
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
        }                                                                                                                                       //*BYM

        //baha todo: this replaces the commented code below for code clarity                                                                    //Done! BYM 
        private void SubMemeplexIdentify(out int nLocalBestFrog, out  int nLocalWorstFrog, out double dLocalWorstObjective, out double dLocalBestObjective)//BYM added , out double dLocalBestObjective
        {
            //make first jump attempt = local
            //	LocalBestFrog=SubMemeplexDA[0][0];**get index for best frog (first) in Submemeplex
            nLocalBestFrog = Convert.ToInt32(_dSubMemeplexDA[0, 0]);                                                                            //BYM
            //	LocalWorstFrog=SubMemeplexDA[SubmemeplexSize-1][0];**get index for worst frog (last) in Submemeplex
            nLocalWorstFrog = Convert.ToInt32(_dSubMemeplexDA[_nSubMemeplexSize - 1, 0]);                                                         //BYM
            //	LocalWorstObjective=GAGetReal(VirtualPopulationIndex,LocalWorstFrog,1);
            dLocalWorstObjective = Convert.ToDouble(_populatation._lstDNA[nLocalWorstFrog]._dObjective);                                        //BYM            //important: is nLocalWorstFrog correct to look at
            //   = nLocalWorstFrog = nLocalWorstObjective = -1;       //baha todo: replace this                                                 //Done! BYM
            dLocalBestObjective = Convert.ToDouble(_populatation._lstDNA[nLocalBestFrog]._dObjective);                                          //BYM 
        }

        private double DNAMAtch(int nLocalBestFrog, int nLocalWorstFrog)
        {
            int nNoOfParameters = _populatation._lstDNA[0]._DNA_Array.Length;
            int nMatch = 0;
            for (int p = 0; p < nNoOfParameters; p++)
            {
                if (_populatation._lstDNA[nLocalBestFrog]._DNA_Array[p].dVal == _populatation._lstDNA[nLocalWorstFrog]._DNA_Array[p].dVal) nMatch++;
            }
            return (nMatch / nNoOfParameters);
        }


        private double ProcessFrog(int nMemeplexNo, int nLocalBestFrog, int nLocalWorstFrog, double dLocalWorstObjective) //BYM change from double nLocalWorstObjective to dLocalWorstObjective
        {
            //start with a local frog leap                                                                                                 //BYM
            int nGlobalBestFrog = 0;
            bool bOutofBounds = FrogJump(nMemeplexNo, nLocalBestFrog, nLocalWorstFrog);
            bool bWorseLocation = false;
            double dObjective = -1;
            //evaluate jump result
            int nMemberNumber = nLocalWorstFrog;
            if (!bOutofBounds)
            {
                //dObjective = -1;        //baha todo: add this function:  evaluate_fitness();                                              //Done! BYM    
                dObjective = EvaluateFitness(nMemeplexNo);                                                                                             //BYM
                bWorseLocation = MemberIsWorse(dObjective, dLocalWorstObjective); //BYM change to dLocalWorstObjective
            }
            //if first jump is unsatisfactory make another jump = global jump
            if (bOutofBounds || bWorseLocation)
            {
                bOutofBounds = FrogJump(nMemeplexNo, nGlobalBestFrog, nLocalWorstFrog);   //baha- check first arg        //global frog leap                        //BYM change from dLocalWorstObjective to nLocalWorstFrog
                //evaluate jump result
                if (!bOutofBounds)
                {
                    //dObjective = -1;     //baha todo: add this function: evaluate_fitness();                                          //Done! BYM
                    dObjective = EvaluateFitness(nMemeplexNo);                                                                                     //BYM
                    bWorseLocation = MemberIsWorse(dObjective, dLocalWorstObjective);                                                   //BYM change to dLocalWorstObjective
                }
            }
            //if second jump is unsatisfactory create a random frog
            if (bOutofBounds || bWorseLocation)
            {
                //baha todo: add this function:			generate_random_member();                                                       //Done! BYM
                GenerateRandomMember();                                                                                                 //BYM
                //baha todo: add this function:	evaluate_fitness();                                                                     //Done! BYM
                dObjective = EvaluateFitness(nMemeplexNo);                                                                                         //BYM    
            }
            return dObjective;
        }
        //BYM*

        // baha notes to
        //_qMemeplex replaces MemeplexDA (Queu, right?)
        // _dSubMemeplexDA replaces SubMemeplexDA
        //ParameterDefinitionsDA[p][0]    is replaced by info on the dna object,
        //like-->_populatation._lstDNA[i]._DNA_Array[j].dMinVal  (dMaxVal, dInterval (usually 1)...
        //suggest we work on 'integer'/discrete version first, while keep in mind continuous functions..
        private bool FrogJump(int m, int nBestFrog, int nWorstFrog)        //baha todo: need to adapt this to C# syntax //BYM change to dLocalWorstObjective
        {
            double pb, pw;
            double dJump, dMaxJump, dUpperBound, dLowerBound, dRandomJ;
            Random r = new Random();
            bool bOutOfBounds = false;
            int nNoOfParameters = _populatation._lstDNA[m]._DNA_Array.Length;
            dRandomJ = r.NextDouble();
            for (int p = 0; p < nNoOfParameters; p++)
            {
                pb = _populatation._lstDNA[nBestFrog]._DNA_Array[p].dVal;
                pw = _populatation._lstDNA[nWorstFrog]._DNA_Array[p].dVal;

                if (_bIsDiscreteVals) dLowerBound = 1;
                else dLowerBound = 0;
                
                //For Discrete variables Upper Bound = 1, while for continous variables Upper Bound need to be set
                //if (_bIsDiscreteVals) 
                    dUpperBound = 1;
                //else dUpperBound = TBD;

                if (pb >= pw) dMaxJump = dUpperBound - pw;
                else dMaxJump = dLowerBound - pw; //negative jump
                dMaxJump *= _dMaxJumpFraction;

                if (_bIsDiscreteVals)
                {
                    if (Math.Floor(dMaxJump) >= 0.5) dMaxJump = Math.Floor(dMaxJump) + (dMaxJump - Math.Floor(dMaxJump));
                    else dMaxJump = Math.Floor(dMaxJump) + (dMaxJump);
                }

                if (_bIsDiscreteVals)
                {
                    dJump = _nJumpFactor * dRandomJ * (pb - pw);
                    if (Math.Floor(dJump) >= 0.5) dJump = Math.Floor(dJump) + (dJump - Math.Floor(dJump));
                    else dJump = Math.Floor(dJump) + (dJump);
                }
                else
                {
                    dJump = _nJumpFactor * dRandomJ * (pb - pw);
                }
                
                if (dJump > 0) dJump = Math.Max(dJump, dMaxJump);
                else dJump = Math.Min(dJump, dMaxJump);
                _populatation._lstDNA[m]._DNA_Array[p].dVal = pw + dJump;
                if (pw + dJump < dLowerBound || pw + dJump > dUpperBound)
                {
                    if (dRandomJ < _dBoundaryProbability / 100.0)
                    {
                        _populatation._lstDNA[m]._DNA_Array[p].dVal = Math.Min(_populatation._lstDNA[m]._DNA_Array[p].dVal, dUpperBound);
                        _populatation._lstDNA[m]._DNA_Array[p].dVal = Math.Max(_populatation._lstDNA[m]._DNA_Array[p].dVal, dLowerBound);
                    }
                    else bOutOfBounds = true;
                }
            }
            return bOutOfBounds;
        }

        private double EvaluateFitness(int nMember)
        {
            int p, c, nConnections, nCurrentDVs;
            int[,] Temp1DV_DA;
            double Obj = 0;
            int nNoOfParameters = _populatation._lstDNA[0]._DNA_Array.Length;

            //for (p = 0; p < nNoOfParameters; p++)
            //{
            //    _populatation._lstDNA[nMember]._DNA_Array[p].dVal =  _populatation._lstDNA[nMember]._DNA_Array[p].dVal;
            //}
            //Assume we have one link for now
            nConnections = 1;                           //for now, assume we have only one connector        BYM?
            nCurrentDVs = nNoOfParameters;             //for now, assume we have only one connector        BYM?
            Temp1DV_DA = new int[nCurrentDVs, 2];
            for (c = 0; c < nConnections; c++)
            {
                for (p = 0; p < nCurrentDVs; p++)
                {
                    if (c == 0)
                    {
                        Temp1DV_DA[p, 1] = Convert.ToInt32(_populatation._lstDNA[nMember]._DNA_Array[p].dVal);
                    }
                    else
                    {
                        //Temp1DV_DA[p, 2] = _nParameterDefinitionsDA[_nNoOfParameters-nCurrentDVs + p, 2];
                    }
                }
            }
            RunSimulationModel();                                                                                       //BYM?
            //GetObjective function GetObjective();                                                                     //BYM?        
            for (c = 0; c < nConnections; c++)
            {
                Obj += 100;                                                                                             //BYM?
            }
            UpdateTimer();
            _nEvaluations++;
            return Obj;
        }

        private void RunSimulationModel()
        {
            //run the simulaton model
        }

        private void UpdateTimer()
        {
            TimeSpan tsInterval = DateTime.Now - _dtStartOpt;
            _lSecondsElapsedSimulation = Convert.ToInt64(tsInterval.TotalSeconds);
        }

        //formerly check_for_member_improvement(
        private bool MemberIsWorse(double dNewObjective, double dExistingObjective)
        {
            bool bIsWorseLocation = false;
            if (_bMaximize && (dNewObjective < dExistingObjective))            //baha todo: check logic... is this right?                       //Done! BYM
                bIsWorseLocation = true;
            else if (!_bMaximize && (dNewObjective > dExistingObjective))
                bIsWorseLocation = true;
            else
                bIsWorseLocation = false;
            return bIsWorseLocation;
        }

        private void GenerateRandomMember()
        {
            int nOptions;
            Random random = new Random(DateTime.Now.Millisecond);
            //Random seed                                                                           //BYM?, search on Random Seed   
            int nNoOfParameters = _populatation._lstDNA[0]._DNA_Array.Length;
            nOptions = Convert.ToInt32((_populatation._lstDNA[0]._DNA_Array[0].dMaxVal - _populatation._lstDNA[0]._DNA_Array[0].dMinVal + 1) / (_populatation._lstDNA[0]._DNA_Array[0].dInterval));
            if (_bIsDiscreteVals)
            {
                for (int p = 0; p < nNoOfParameters; p++)
                {
                    _populatation._lstDNA[0]._DNA_Array[p].dVal = random.Next(1, nOptions + 1);      //BYM? lstDNA[0] 
                }
            }
            else
            {
                //for continuous         
            }
        }

        private void CheckIfBestMember(int nLocalWorstFrog, double dCurrentObjective)                  //BYM
        {
            if (!_bMaximize && (dCurrentObjective < _dBestObjective) || _bMaximize && (dCurrentObjective > _dBestObjective))
            {
                _dBestObjective = dCurrentObjective;
                _nBestMember = Convert.ToInt32(_populatation._lstDNA[nLocalWorstFrog]);
            }
        }

        //This function is to return frog to the pool
        private void ReturnFrogIntoPopulation(int nLocalWorstFrog, double dCurrentObjective)                                        //BYM
        {
            int nNoOfParameters = _populatation._lstDNA[0]._DNA_Array.Length;
            for (int p = 0; p < nNoOfParameters; p++)
            {
                //_populatation._lstDNA[nLocalWorstFrog]._DNA_Array[p].dVal = _lMemeplex.ElementAt(p);//P+2 //Please check if you need to have this one local versus global
                _populatation._lstDNA[nLocalWorstFrog]._dObjective = dCurrentObjective;
            }
        }

        private void SortVirtualPopulation()                                                                                       //*BYM
        {
            if (_bMaximize) _populatation._lstDNA.OrderByDescending(x => x._dObjective);
            else _populatation._lstDNA.OrderBy(x => x._dObjective);
        }

        #endregion

        #region StoppingCriteria
        #endregion
    }
}
