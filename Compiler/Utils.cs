using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Utils
    {
        public static bool IsDigit(char caracter)
        {
            return RegexLibrary.IsDigit(caracter);
        }

        public static bool IsNumericSymbol(char caracter)
        {
            return RegexLibrary.IsNumericSymbol(caracter);
        }

        public static bool IsLetter(char caracter)
        {
            return RegexLibrary.IsLetter(caracter);
        }


        public static Grammar? GetToken(char caracter)
        {
            return EnumUtils<Grammar>.List().
                Find(x => x == EnumUtils<Grammar>.GetFromDescription(caracter.ToString()));
        }

        public static Grammar? GetPalavraReservada(string value)
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
