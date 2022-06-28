using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    public class Population
    {
        public List<Individual> _lstDNA;
        public int _nPopulationSize;
        Random _rnd = new Random(DateTime.Now.Millisecond);

        //creates individuals for any that are not defined
        public void Fill()
        {
            for (int i = 0; i < _lstDNA.Count; i++)
            {
                if (_lstDNA[i]._dObjective == CommonUtilities._dBAD_VALorNOT_SET)
                {
                    _lstDNA[i].Fill(ref _rnd);
                }
            }
        }

        //for ease of debugging set the DNA strings on the individual (so you can easily see the concat)
        public void UpdateDNAStrings()
        {
            for (int i = 0; i < _lstDNA.Count; i++)
            {
                _lstDNA[i]._sDNA_Copy = _lstDNA[i].GetDNABitsAsString();
             }
        }

        //returns whole new list; ways to just sort the exiting?  Should not be too bad
        //http://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object
        public void SortIndividuals(bool bMaximize = true)
        {
            if (bMaximize)
               _lstDNA = _lstDNA.OrderByDescending(o => o._dObjective).ToList();       //untested 
            else
            {
                _lstDNA = _lstDNA.OrderBy(o => o._dObjective).ToList();
            }
        }                       
    }

    public class Individual
    {
        public double _dObjective = CommonUtilities._dBAD_VALorNOT_SET;
        public string _sDNA_Copy = CommonUtilities._sBAD_VALorNOT_SET;
        public DNAbit[] _DNA_Array;

        public void Fill(ref Random rnd)
        {
            for (int i = 0; i < _DNA_Array.Length; i++)
            {
                _DNA_Array[i].SetBit_Random(ref rnd);
            }
        }

        public double[] GetBitsAsArray()
        {
            double[] dReturn = new double[_DNA_Array.Length];
            for (int i = 0; i < _DNA_Array.Length; i++)
            {
                dReturn[i] = _DNA_Array[i].dVal;
            }
            return dReturn;
        }


        //todo: handle case where 0 bits? practically meaningless, so ignored (though will err)
        public string GetDNABitsAsString(string sDelim = ".")
        {
            int nLen = _DNA_Array.Length;

            string sReturn = _DNA_Array[0].dVal.ToString();
            for (int i = 1; i < _DNA_Array.Length; i++)
            {
                sReturn = sReturn + sDelim + _DNA_Array[i].dVal.ToString();
            }
            return sReturn;
        }

    }
    //use doubles to enable continuous functions in the future... m
    public class DNAbit
    {
        public double dVal;                                //default to 1
        public double dMinVal = 1;                         //default to 1   (Set from SimLink)
        public double dMaxVal = 5;
        public double dInterval = 1;
       
        //sets bit to random INTEGER value
        public void SetBit_Random(ref Random rnd)
        {
            
            dVal = rnd.Next(Convert.ToInt32(dMinVal), Convert.ToInt32(dMaxVal)+1);
        }
    }

}
