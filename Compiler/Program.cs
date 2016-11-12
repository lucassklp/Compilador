﻿using System;
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
                //Lê o arquivo
                StreamReader sr = new StreamReader(file);
                content = sr.ReadToEnd();
                
                Scanner scanner = new Scanner(content);
                //scanner.PrintTokens();
                Parser parser = new Parser(scanner);
                parser.Analizar();

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
