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
        public static bool IsEndOfFile(int counter, int length)
        {
            return !(counter < length);
        }

        public static bool IsDigit(char caracter)
        {
            return RegexLibrary.IsDigit(caracter);
        }

        public static bool IsToken(char firstCharacter)
        {
            return EnumUtils<Gramatica>.List().Exists(x => EnumUtils<Gramatica>.GetDescription(x) == firstCharacter.ToString());
        }

        public static bool IsToken(char firstCharacter, char? nextCharacter)
        {
            if (nextCharacter == null)
                return false;
            else
            {
                string composition = Concat(firstCharacter, nextCharacter);
                return EnumUtils<Gramatica>.List().
                    Exists(x => EnumUtils<Gramatica>.GetDescription(x) == composition &&
                    EnumUtils<Gramatica>.GetCategory(x) != "PalavraReservada");
            }
        }

        public static bool IsPalavraReservada(string palavraReservada)
        {
            return EnumUtils<Gramatica>.GetFromCategory("PalavraReservada").
                Exists(x => EnumUtils<Gramatica>.GetDescription(x) == palavraReservada);
        }

        public static bool IsDigitOrPunto(char currentCharacter)
        {
            return RegexLibrary.IsDigitOrPunto(currentCharacter);
        }

        public static bool IsIdentifier(string identifier)
        {
            string pattern = EnumUtils<Gramatica>.GetDescription(Gramatica.Identificador);

            return Regex.IsMatch(identifier, pattern);
        }

        public static bool IsNumber(char caracter)
        {
            return RegexLibrary.IsNumericSymbol(caracter);
        }

        public static bool IsLetter(char caracter)
        {
            return RegexLibrary.IsLetter(caracter);
        }

        public static bool IsComentarioDeLinha(char first, char? second)
        {
            if (second == null)
                return false;
            else
                return first == '/' && second == '/';
        }

        public static bool IsLiteralCharDefinition(char caracter)
        {
            return caracter == '\'';
        }

        public static bool IsInicioComentarioDeBloco(char first, char? second)
        {
            if (second == null)
                return false;
            else
                return first == '/' && second == '*';
        }

        public static bool IsFimComentarioDeBloco(char first, char? second)
        {
            if (second == null)
                throw new Exception("Fim de comentário esperado não foi encontrado");
            else
                return first == '*' && ((char)second) == '/';
        }


        public static string Concat(char first, char last)
        {
            return (first.ToString() + last.ToString());
        }

        public static string Concat(char first, char? last)
        {
            if (last == null)
                return first.ToString();
            else
                return (first.ToString() + ((char)last).ToString());
        }



        public static Gramatica GetToken(string token)
        {
            return EnumUtils<Gramatica>.List().
                    Find(x => x == EnumUtils<Gramatica>.GetFromDescription(token));
        }
        public static Gramatica GetToken(char token)
        {
            return EnumUtils<Gramatica>.List().
                    Find(x => x == EnumUtils<Gramatica>.GetFromDescription(token.ToString()));    
        }

        public static Gramatica GetPalavraReservada(string value)
        {
            return EnumUtils<Gramatica>.GetFromCategory("PalavraReservada").
                Find(x => EnumUtils<Gramatica>.GetDescription(x) == value);
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
