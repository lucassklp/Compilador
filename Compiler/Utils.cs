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

        public static bool IsLetterOrUnderline(char currentChar)
        {
            return RegexLibrary.IsLetterOrUnderline(currentChar);
        }

        public static bool IsDigit(char caracter)
        {
            return RegexLibrary.IsDigit(caracter);
        }

        public static bool IsToken(char firstCharacter)
        {
            return EnumUtils<Token>.List().Exists(x => EnumUtils<Token>.GetDescription(x) == firstCharacter.ToString());
        }

        public static bool IsToken(char firstCharacter, char? nextCharacter)
        {
            if (nextCharacter == null)
                return false;
            else
            {
                string composition = Concat(firstCharacter, nextCharacter);
                return EnumUtils<Token>.List().
                    Exists(x => EnumUtils<Token>.GetDescription(x) == composition &&
                    EnumUtils<Token>.GetCategory(x) != "PalavraReservada");
            }
        }

        public static bool IsPalavraReservada(string palavraReservada)
        {
            return EnumUtils<Token>.GetFromCategory("PalavraReservada").
                Exists(x => EnumUtils<Token>.GetDescription(x) == palavraReservada);
        }

        public static bool IsDigitOrPunto(char currentCharacter)
        {
            return RegexLibrary.IsDigitOrPunto(currentCharacter);
        }

        public static bool IsIdentifier(string identifier)
        {
            string pattern = EnumUtils<Token>.GetDescription(Token.Identificador);
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
                return false;
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



        public static Token GetToken(string token)
        {
            return EnumUtils<Token>.List().
                    Find(x => x == EnumUtils<Token>.GetFromDescription(token));
        }
        public static Token GetToken(char token)
        {
            return EnumUtils<Token>.List().
                    Find(x => x == EnumUtils<Token>.GetFromDescription(token.ToString()));    
        }

        public static Token GetPalavraReservada(string value)
        {
            return EnumUtils<Token>.GetFromCategory("PalavraReservada").
                Find(x => EnumUtils<Token>.GetDescription(x) == value);
        }

        public static bool IsDelimitador(char delimit)
        {
            char[] Delimitadores = { ' ', '\t', '\n', '\r' };
            return Delimitadores.Contains(delimit);
        }


    }
}
