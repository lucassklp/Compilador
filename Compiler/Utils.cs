using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Compiler
{
    class Utils
    {
        public static bool IsDigit(char caracter)
        {
            return RegexLibrary.IsDigit(caracter);
        }

        public static bool IsToken(char firstCharacter)
        {
            return EnumUtils<Grammar>.List().Exists(x => EnumUtils<Grammar>.GetDescription(x) == firstCharacter.ToString());
        }

        public static bool IsToken(char firstCharacter, char? nextCharacter, bool testComposition)
        {
            bool isSingleToken = IsToken(firstCharacter);
  
            if (testComposition)
            {
                string composition;
                if (nextCharacter != null)
                    composition = Concat(firstCharacter, (char)nextCharacter);
                else
                    return false;

                return EnumUtils<Grammar>.List().Exists(x => EnumUtils<Grammar>.GetDescription(x) == composition);
            }
            else
                return isSingleToken;
            
        }

        public static bool IsPalavraReservada(string palavraReservada)
        {
            return EnumUtils<Grammar>.GetFromCategory("PalavraReservada").
                Exists(x => EnumUtils<Grammar>.GetDescription(x) == palavraReservada);
        }

        public static bool IsIdentifier(string identifier)
        {
            return RegexLibrary.IsIdentifier(identifier);
        }

        public static bool IsNumericSymbol(char caracter)
        {
            return RegexLibrary.IsNumericSymbol(caracter);
        }

        public static bool IsLetter(char caracter)
        {
            return RegexLibrary.IsLetter(caracter);
        }

        public static bool IsComentarioDeLinha(char first, char second)
        {
            return first == '/' && second == '/';
        }

        public static bool IsLiteralCharDefinition(char caracter)
        {
            return caracter == '\'';
        }

        public static bool IsInicioComentarioDeBloco(char first, char second)
        {
            return first == '/' && second == '*';
        }

        public static bool IsFimComentarioDeBloco(char first, char second)
        {
            return first == '*' && second == '/';
        }

        public static List<Grammar> GetNumericTypes()
        {
            return EnumUtils<Grammar>.GetFromCategory("NumericRules");
        }

        public static Grammar GetCharacterType()
        {
            return EnumUtils<Grammar>.GetFromCategory("CharRules").First();
        }


        public static string Concat(char first, char last)
        {
            return (first.ToString() + last.ToString());
        }

        public static Grammar GetToken(string token)
        {
            return EnumUtils<Grammar>.List().
                    Find(x => x == EnumUtils<Grammar>.GetFromDescription(token));
        }
        public static Grammar GetToken(char token)
        {
            return EnumUtils<Grammar>.List().
                    Find(x => x == EnumUtils<Grammar>.GetFromDescription(token.ToString()));    
        }

        public static Grammar GetPalavraReservada(string value)
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
            delimitadores.Add('\r');
            bool retorno = delimitadores.Exists(x => x == delimit);
            return retorno;

        }


    }
}
