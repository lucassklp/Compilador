using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Utils
    {

        public static Grammar? isPalavraReservada(string value)
        {
            return EnumUtils<Grammar>.GetFromCategory("PalavraReservada").
                Find(x => EnumUtils<Grammar>.GetDescription(x) == value);
        }

        public static bool isDelimitador(char delimit)
        {
            List<char> delimitadores = new List<char>();
            delimitadores.Add(' ');
            delimitadores.Add('\t');
            delimitadores.Add('\n');

            return delimitadores.Exists(x => x == delimit);

        }


    }
}
