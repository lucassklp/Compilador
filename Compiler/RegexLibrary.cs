using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Compiler
{
    class RegexLibrary
    {
        public static bool IsIdentifier(string lexema)
        {
            return (new Regex(@"/([A-Za-z]|\_)([A-Za-z]|\_|[0-9])*/")).IsMatch(lexema);
        }

    }
}
