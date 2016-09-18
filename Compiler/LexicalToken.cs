using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class LexicalToken
    {
        public int Linha { get; private set; }
        public int Coluna { get; private set; }
        public Token Token{ get; private set;}
        public string Lexema { get; private set; }

        public LexicalToken(int Linha, int Coluna, Token Gramatica, string Lexema)
        {
            this.Linha = Linha;
            this.Coluna = Coluna;
            this.Token = Gramatica;
            this.Lexema = Lexema;
        }
    }
}
