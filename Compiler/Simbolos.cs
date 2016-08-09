using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Compiler
{
    class Simbolos
    {
        Regex letras = new Regex(@"[A-Za-z]");
        Regex numeros = new Regex(@"[0-9]");

    }
}
