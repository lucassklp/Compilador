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
            try
            {
                //Lê o arquivo
                StreamReader sr = new StreamReader(file);
                content = sr.ReadToEnd();
                
                Scanner scanner = new Scanner(content);
                scanner.PrintTokens();
                Parser parser = new Parser(scanner);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
