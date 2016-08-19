using Compiler.Tree;
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

            string file = @"C:\ERROS_SCANNER.txt";
            //string file = @"C:\arquivo_exemplo.txt";
            //string file = @"C:\test_scanner.txt";
            string content;
            using (StreamReader sr = new StreamReader(file))
            {
                content = sr.ReadToEnd();
            }

            Scanner scanner = new Scanner(content);
            Token token = scanner.NextToken();
            while (token != null)
            {
                Console.WriteLine(token.Lexema + " => " + token.Gramatica);
                token = scanner.NextToken();
            }

            Console.WriteLine("\n ----------- FIM DO SCANNER -------------");
            Console.ReadKey();

            //Programa programa = new Programa(p);
            //programa.isValid();


        }
    }
}
