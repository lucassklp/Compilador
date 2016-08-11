using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Compiler
{
    class Scanner
    {
        private int counter = -1;
        private string code;
        public Scanner(string code)
        {
            this.code = code;
        }

        public TokenTable MontarTokenTable()
        {
            int linha = 0, coluna = 0;
            TokenTable p = new TokenTable();

            while(this.counter < this.code.Length)
            {
                string lexema = string.Empty;
                char caracter = this.code[++counter];
                IncrementRowColumn(caracter, ref linha, ref coluna);

                if (!Utils.isDelimitador(caracter))
                {
                    if (Utils.isToken(caracter))
                    {

                    }
                    else if()
                    do
                    {
                        lexema += getNext();
                    }
                    while(Utils.isDelimitador(caracter))
                }


            }
            p.ListaTokens.Add(new Token(linha, coluna, Grammar.EndOfFile, "EOF"));



            return p;
        }


        private char getNext()
        {
            return this.code[++counter];
        }

        private void IncrementRowColumn(char caracter, ref int linha, ref int coluna)
        {
            if (Utils.isDelimitador(caracter))
            {
                if (char.IsWhiteSpace(caracter))
                    coluna++;
                else if (caracter == '\t')
                    coluna += 4;
                else if (caracter == '\n')
                {
                    coluna = 0;
                    linha++;
                }
            }
            else
                coluna++;
        }



    }
}
