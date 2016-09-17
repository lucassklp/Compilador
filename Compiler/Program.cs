using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
			
            string file = @"C:\arquivo_exemplo.txt";

            string content;
            using (StreamReader sr = new StreamReader(file))
            {
                content = sr.ReadToEnd();
            }

            Scanner scanner = new Scanner(content);
            Parser parser = new Parser(scanner);




            Console.ReadKey();
        }

        private static string doTabs(int length)
        {
            string response = string.Empty;
            int result = length / 8;
            for (int i = result; i < 4; i++)
            {
                response += "\t";
            }

            return response;
        }
    }
}
