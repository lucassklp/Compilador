using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class Token
    {
        public int Linha { get; private set; }
        public int Coluna { get; private set; }
        public Grammar Gramatica{ get; private set;}
        public string Lexema { get; private set; }

        public Token(int Linha, int Coluna, Grammar Gramatica, string Lexema)
        {

        }


    }
}
