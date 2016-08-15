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
            TokenTable tokenTable = new TokenTable();

            try
            {
                while(this.counter < this.code.Length)
                {
                    string lexema = string.Empty;
                    char caracter = getNext();
                    IncrementRowColumn(caracter, ref linha, ref coluna);

                    if (!Utils.isDelimitador(caracter))
                    {
                        if (Utils.GetToken(caracter) != null)
                        {
                            Grammar grammar = (Grammar)Utils.GetToken(caracter);
                            tokenTable.ListaTokens.Add(new Token(linha, coluna, grammar, caracter.ToString()));
                        }
                        else
                        {
                            if (Utils.IsDigit(caracter))
                            {
                                do
                                    lexema += getNext();
                                while (Utils.isDelimitador(caracter) || !Utils.IsNumericSymbol(caracter));

                                Grammar grammar = RegexLibrary.ValidateNumericRules(Utils.GetNumericTypes(), lexema);
                                tokenTable.ListaTokens.Add(new Token(linha, coluna, grammar, lexema));
                            }
                            else
                            {
                                do
                                {
                                    lexema += caracter;
                                    caracter = getNext();
                                }
                                while (!Utils.isDelimitador(caracter) || Utils.GetToken(caracter) != null);
                                
                                   
                                
                                    
                                

                                if (Utils.GetPalavraReservada(lexema) != null)
                                {
                                    Grammar palavraReservada = (Grammar)Utils.GetPalavraReservada(lexema);
                                    tokenTable.ListaTokens.Add(new Token(linha, coluna, palavraReservada, lexema));
                                }
                            }
                        }
                    }
                }
                tokenTable.ListaTokens.Add(new Token(linha, coluna, Grammar.EndOfFile, "EOF"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }


            return tokenTable;
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
