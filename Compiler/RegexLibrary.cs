﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Compiler
{
    class RegexLibrary
    {

        #region Checkers

        internal static bool IsDigitOrPunto(char currentCharacter)
        {
            return Regex.IsMatch(currentCharacter.ToString(), @"[0-9]|\.");
        }

        public static bool IsDigit(char caracter)
        {
            return Regex.IsMatch(caracter.ToString(), "[0-9]");
        }

        public static bool IsNumericSymbol(char caracter)
        {
            return Regex.IsMatch(caracter.ToString(), @"[0-9]");
        }

        public static bool IsLetter(char caracter)
        {
            return Regex.IsMatch(caracter.ToString(), @"[A-Za-z]");
        }

        public static bool IsLetterOrUnderline(char caracter)
        {
            return Regex.IsMatch(caracter.ToString(), @"[A-Za-z]|_");
        }



        #endregion

        #region Type Checkers


        public static bool IsNumericType(string lexema)
        {
            var numericTypes = EnumUtils<Token>.GetFromCategory("NumericRules");
            foreach (var item in numericTypes)
            {
                string pattern = EnumUtils<Token>.GetDescription(item);
                Regex numeric = new Regex(pattern);
                bool match = numeric.IsMatch(lexema);
                if (match)
                    return true;
            }

            return false;
        }




        public static Token GetNumericType(string lexema)
        {
            var numericTypes = EnumUtils<Token>.GetFromCategory("NumericRules");
            foreach (var item in numericTypes)
            {
                string pattern = EnumUtils<Token>.GetDescription(item);
                Regex numeric = new Regex(pattern);
                bool match = numeric.IsMatch(lexema);
                if (match)
                    return item;
            }
             
            return default(Token);
        }

        public static bool IsValidCharacter(string lexema)
        {
            var rule = EnumUtils<Token>.GetFromCategory("CharRules").First();

            string pattern = EnumUtils<Token>.GetDescription(rule);
            Regex character = new Regex(pattern);
            bool match = character.IsMatch(lexema);
            if (match)
                return true;
            else
                return false;
            
        }

        #endregion

    }
}
