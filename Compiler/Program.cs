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
            string file = @"C:\arquivo_exemplo.txt";
            //string file = @"C:\test_scanner.txt";
            string content;
            using (StreamReader sr = new StreamReader(file))
            {
                content = sr.ReadToEnd();
            }

            Scanner scanner = new Scanner(content);
            TokenTable teste = scanner.MontarTokenTable();

            //Programa programa = new Programa(p);
            //programa.isValid();


        }
    }
}
