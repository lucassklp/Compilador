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
        
        #region Checkers
        public static bool IsDigit(char caracter)
        {
            return Regex.IsMatch(caracter.ToString(), "[0-9]");
        }

        public static bool IsNumericSymbol(char caracter)
        {
            return Regex.IsMatch(caracter.ToString(), @"[0-9]|\.");
        }

        public static bool IsLetter(char caracter)
        {
            return Regex.IsMatch(caracter.ToString(), "[A-Za-z]");
        }
        
        #endregion

        #region Rules Validation
        public static Grammar ValidateNumericRules(List<Grammar> numericTypes, string lexema)
        {
            foreach (var item in numericTypes)
            {
                string pattern = EnumUtils<Grammar>.GetDescription(item);
                Regex numeric = new Regex(pattern);
                bool match = numeric.IsMatch(lexema);
                if (match)
                    return item;
            }

            throw new Exception("Not a Numeric Type");
        }

        public static Grammar ValidateCharacterRule(Grammar rule, string lexema)
        {
            string pattern = EnumUtils<Grammar>.GetDescription(rule);
            Regex character = new Regex(pattern);
            bool match = character.IsMatch(lexema);
            if (match)
                return rule;
            else
                throw new Exception("Not a Character Type");
            
        }
        #endregion

    }
}
