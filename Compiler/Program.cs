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
            string file = args[0];

            string content;
            using (StreamReader sr = new StreamReader(file))
            {
                content = sr.ReadToEnd();
            }

            Scanner scanner = new Scanner(content);
            Token k = scanner.NextToken();
            while(k != null)
            {
                string msg = k.Lexema + "  =>  " + k.Gramatica.ToString();
                Console.WriteLine(msg + doTabs(msg.Length) + string.Format("Linha: {0}, Coluna: {1}", k.Linha, k.Coluna));
                k = scanner.NextToken();
            }


            //Parser parser = new Parser(scanner);




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
