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
        public Gramatica Gramatica{ get; private set;}
        public string Lexema { get; private set; }

        public Token(int Linha, int Coluna, Gramatica Gramatica, string Lexema)
        {
            this.Linha = Linha;
            this.Coluna = Coluna;
            this.Gramatica = Gramatica;
            this.Lexema = Lexema;
        }


    }
}
