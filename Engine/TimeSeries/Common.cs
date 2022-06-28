using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_API_LINKS
{
    public class Common
    {
        #region DATATYPES
        public static bool IsDouble(string value)
        {
            // Return true if this is a number.
            double number1;
            return double.TryParse(value, out number1);
        }

        //public helper for testing for a double
        public static bool IsInteger(string value)
        {
            // Return true if this is a number.
            int number1;
            return int.TryParse(value, out number1);
        }
        #endregion

    }
}
