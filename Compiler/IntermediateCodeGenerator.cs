using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class IntermediateCodeGenerator
    {

        #region Static Fields
        private static int varCount = -1;
        private static int lblCount = -1;
        private static StringBuilder result = new StringBuilder();

        public static string CurrentVariableName
        {
            get
            {
                return string.Format("F{0}", varCount);
            }
        }

        public static string CurrentLabelName
        {
            get
            {
                return string.Format("L{0}", lblCount);
            }
        }

        public static string GenerateVariableName()
        {
            return string.Format("F{0}", ++varCount);
        }

        

        public static string GenerateLabelName()
        {
            return string.Format("L{0}", ++lblCount);
        }



        public static void GenerateCode(string text)
        {
            Console.WriteLine(text);
            result.AppendLine(text);
        }

        public static void Print()
        {
            Console.WriteLine(result);
        }




        #endregion










    }
}
