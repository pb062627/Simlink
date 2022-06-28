using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_API_LINKS
{
    public enum TSRepository
    {
        HDF5 = 1,
        DB = 2,
        NetworkTable = 3,
        XML = 4,
        Undefined = -1
    }

    public enum ResultsAggregation                      //9/19/14
    {                                                   //  1: sum 2: min 3: max  4: count 5: avg
        SUM = 1,
        MIN = 2,
        MAX = 3,
        COUNT = 4,
        AVERAGE = 5
    }

    public class SimLinkENUM
    {

        #region DBCONTEXT
       
        
      //SimLink classes must be agnostic about their data provider
        // these are mapped in DBContext to specific data providers.
        enum DataTypeSL
        {
            INTEGER,
            LONG,
            STRING,
            DOUBLE,
            DATETIME
        };
        #endregion


        public enum UIDictionary
        {
            ModelType,
            FieldDictionaryTable,
            UnitSettings,
            SWMM_Out,
            ElementLibrary,
            DistribStrategyCat,
            SystemType
        }
    }
}
