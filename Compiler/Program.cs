using Compiler.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
            string p = @"int main  (    ) 
                        {something...}";
            Programa programa = new Programa(p);
            programa.isValid();


        }
    }
}
