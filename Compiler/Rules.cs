using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Rules
    {
        public static Grammar NumericFormat(string lexema)
        {
            List<Grammar> numeric = EnumUtils<Grammar>.GetFromCategory("NumericRules");
            return RegexLibrary.ValidateNumericRules(numeric, lexema);
        }
    }
}
