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
			
            string file = @"C:\arquivo_exemplo.c";

            string content;
            try
            {
                StreamReader sr = new StreamReader(file);
                content = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                
                Scanner scanner = new Scanner(content);

                Parser parser = new Parser(scanner);
                parser.Analisar();

                Console.WriteLine("Compilado com sucesso!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
            }

            Console.ReadKey();
        }
    }
}
