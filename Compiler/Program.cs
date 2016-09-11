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
            Token token = scanner.NextToken();
            //while (token != null)
            //{
            //    string lexemaGrammar = string.Format("{0} => {1}", token.Lexema, token.Gramatica.ToString());
            //    string linhaColuna = string.Format("Linha: {0}, Coluna: {1}", token.Linha, token.Coluna);
            //    Console.WriteLine(lexemaGrammar + doTabs(lexemaGrammar.Length) + linhaColuna);
            //    token = scanner.NextToken();
            //}

            Console.WriteLine("\n ----------- FIM DO SCANNER -------------");
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
