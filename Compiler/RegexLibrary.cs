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
            return (new Regex("[0-9]")).IsMatch(caracter.ToString());
        }

        public static bool IsNumericSymbol(char caracter)
        {
            return (new Regex(@"[0-9]|\.")).IsMatch(caracter.ToString());
        }

        public static bool IsLetter(char caracter)
        {
            return (new Regex("[A-Za-z]")).IsMatch(caracter.ToString());
        }

        public static bool IsIdentifier(string lexema)
        {
            return (new Regex(@"/([A-Za-z]|\_)([A-Za-z]|\_|[0-9])*/")).IsMatch(lexema);
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
