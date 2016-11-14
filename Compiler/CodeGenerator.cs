using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public static class CodeGenerator
    {
        private static StringBuilder Result = new StringBuilder();
        public static string Code
        {
            get
            {
                return Result.ToString();
            }
        }

        public static void Append(string text)
        {
            Result.AppendLine(text);
        }

        public static void SaveToFile()
        {

        }

    }
}
