using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    public class GA : Optimization
    {
        #region MEMBERS
        private int _nBestMember;               //use instead of GlobalBestFrog=0;**index for best frog in population is always zero

     //0.5 i guess is sensible ; wonder if this is ever paramterized    private double _dCrossoverProb;  //      use this in crossover function and set per args
        private double _dMutationProb;
        private double _dMutationIsElite;           //if mutation, instead of rnd, take val from leading dude ??
        private int _nEliteParents;
        #endregion

        #region init
        //override in base class
        //
        public override void InitOptimization(Dictionary<string, string> dictArgs)
        {
            _nLoopNo = 0;
            _algType = OptimizationEnum.AlgType.SFLA;

            //initialize the Stopping Criteria
            bool bAtLeastOneStopCriteria = InitStopCriteria(dictArgs);
            //initialize SFLA specific params
            InitGA_Params(dictArgs);
            //initialize SFLA specific defaults
            SetGA_Defaults(dictArgs);
            //_dSubMemeplexDA = new double[_nSubMemeplexSize, 2];
            _dtStartOpt = System.DateTime.Now;
            _bIsValidOpt = ValidateOptimization(bAtLeastOneStopCriteria);
        }

        
        private bool InitGA_Params(Dictionary<string, string> dictArgs)
        {
            return false;
        }
        private bool SetGA_Defaults(Dictionary<string, string> dictArgs)
        {
            bool bReturnValid = true;
            try
            {
                // _dCrossoverProb = 0.35;               //set defaults first, so if error on dictionary (from cli, excel, elsewhere), we can 1)warn the user 2) proceed
                _dMutationProb = 0.08;
                _dMutationIsElite = 0.25;
                _nEliteParents = 2;

                if (dictArgs.ContainsKey("elite_parents"))
                {
                    _nEliteParents = Convert.ToInt32(dictArgs["elite_parents"]);
                }

                if (dictArgs.ContainsKey("mutation_rate"))
                {
                    _dMutationProb = Convert.ToDouble(dictArgs["mutation_rate"]);
                }

                if (dictArgs.ContainsKey("mutation_is_elite"))
                {
                    _dMutationIsElite = Convert.ToInt32(dictArgs["mutation_is_elite"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion


        #region RunOpt

        public override void EvolvePopulation()
        {

            SelectionOperator();                                         //(BYM2014) In the future we need to consider different selection criteria/options, currently selection is totally based on fitness
            if (_nEliteParents > 0) ElitismOperator();                   //not in ga.h  (BYM2014) it was in an older version of ga.h
            CrossOverOperator();                                         //(BYM2014) In the future we need to consider more than one crossover options
            MutationOperator();                                          //(BYM2014) In the future we need to consider more than one mutation options
            ExecuteSimulationsAll();
            //evaluate_virtual_population();  //met- we need this        //(BYM2014) We do not need this one here, it is already included in the SelectionOperator
            if (_nLoopNo == _nStopCrit_NumberLoops) _populatation.SortIndividuals(_bMaximize);
        }

       #endregion


        #region EVOLVE

        public void SelectionOperator()
        {
            _populatation.SortIndividuals(_bMaximize);          //baha todo: verify  sort_virtual_population();  Checked BYM2014                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        select_evolutionary_population();
            _dBestObjective = _populatation._lstDNA[0]._dObjective;
            Console.WriteLine("Loop No: " + _nLoopNo + ", BestObjective: " + _dBestObjective + ", DNA: " + _populatation._lstDNA[0].GetDNABitsAsString());
        }

        private void ElitismOperator()
        {
            int nPopulationCount = _populatation._nPopulationSize;
            int nParameters = _populatation._lstDNA[0]._DNA_Array.Length;
            int nParent_DNAbit;
            
            for (int e = 0; e < _nEliteParents; e ++)  
            {
                for (int p = 0; p < nParameters; p++)
                {
                    nParent_DNAbit = Convert.ToInt32(_populatation._lstDNA[e]._DNA_Array[p].dVal);      
                    _populatation._lstDNA[nPopulationCount-1-e]._DNA_Array[p].dVal = nParent_DNAbit;              
                }
            }
        }

        private void CrossOverOperator()
        {
            int nPopulationCount = _populatation._nPopulationSize;
            int nParameters = _populatation._lstDNA[0]._DNA_Array.Length;
            Random rnd = new Random();
            //   int e = -1;     //whence ;e?   baha confirm : formerly : EliteParents
            int  nParentA_DNAbit, nParentB_DNAbit;
            //baha confirm: should not be operator's responsibility to sort population       sort_evolutionary_population(); (BYM2014) Selection Operator is doing the sorting
            for (int e = 0; e < _populatation._nPopulationSize - 1 - _nEliteParents; e = e + 2)  //  baha todo: confirm:: PopulnParametersation; e++)
            {
                //if (e % 2 == 0)         //baha- is crossover always applied between two subsequents? (BYM2014) Yes for this version. 
                //{                       //may be worth considering less rigid crossover?? (BYM2014) We need to develope a crossover selection criteria with variety of options
                                          //rmv mod in favor of explicit indexing- plz confirm intent
                for (int p = 0; p < nParameters; p++)
                {
                    nParentA_DNAbit = Convert.ToInt32(_populatation._lstDNA[e]._DNA_Array[p].dVal);      
                    nParentB_DNAbit = Convert.ToInt32(_populatation._lstDNA[e+1]._DNA_Array[p].dVal);             
                    //important: met- i don't yet understand whether this creates new individual or overwrites? (BYM2014) Overwriting 
                    
                    if (p < (nParameters / 2))          //baha- should this be randomizded   eg rnd >0.5?? (BYM2014) You are right, that can be randamized 
                    {                                   //met understanding: should only need to set for one condition 
                        _populatation._lstDNA[e]._DNA_Array[p].dVal = nParentB_DNAbit;
                        _populatation._lstDNA[e+1]._DNA_Array[p].dVal = nParentA_DNAbit;   
                    }
                    else
                    {
                       // why is any action needed?  (BYM2014) You are right, nothing is needed                                     
                    }
                }
            }
        }

        private void MutationOperator()
        {
            //This mutation is different than the standard GA
            //It may have a little chance for pre-mature convergence in early stages
            //I think we need to have a little talk here
            int nParameters = _populatation._lstDNA[0]._DNA_Array.Length;
            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < _populatation._nPopulationSize - 1 - _nEliteParents; i++)
            {
                for (int j = 0; j<nParameters; j++){
                    double dRND = rnd.Next(0,100);       
                    if (dRND/100 > _dMutationProb){
                        dRND = rnd.Next(0,100);
                        if(dRND/100>_dMutationIsElite)         
                        {
                            _populatation._lstDNA[i]._DNA_Array[j].dVal = _populatation._lstDNA[_nBestMember]._DNA_Array[j].dVal;
                        }
                        else
                        {
                            _populatation._lstDNA[i]._DNA_Array[j].SetBit_Random(ref rnd);  //random mutation
                        }
                    }
                }
            }
        }

        #endregion


    }
}
