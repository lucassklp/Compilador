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
        private int DEBUG = 0;

        private int counter = -1;
        private string code;

        private int linha = 1;
        private int coluna = 0;

        private char PreviousCharacter;
        private char CurrentCharacter;
        private char? NextCharacter;


        public Scanner(string code)
        {
            this.code = code;
        }

        public TokenTable MontarTokenTable()
        {
            TokenTable tokenTable = new TokenTable();

            try
            {
                while (this.counter < this.code.Length)
                {
                    this.MoveToNextCharacter();
                    //Para fins de debug
                    if (this.CurrentCharacter == 'y')
                        this.DEBUG++;

                    Console.WriteLine(this.linha + " - " + this.coluna);

                    string lexema = string.Empty;
                    if (!Utils.isDelimitador(this.CurrentCharacter)) //Tratamento de delimitadores
                    {
                        if (this.NextCharacter != null) //Se não é último token
                        {
                            if (Utils.GetToken(this.CurrentCharacter) != null && this.NextCharacter != '/' && this.NextCharacter != '*') //Tratamento de tokens
                            {
                                Grammar grammar = (Grammar)Utils.GetToken(this.CurrentCharacter);
                                tokenTable.ListaTokens.Add(new Token(linha, coluna, grammar, this.CurrentCharacter.ToString()));
                            }
                            else
                            {
                                if (Utils.IsDigit(this.CurrentCharacter)) //Tratamento de tipos numericos literais
                                {
                                    do
                                    {
                                        lexema += this.CurrentCharacter;
                                        this.MoveToNextCharacter();
                                    }
                                    while (Utils.GetToken(this.CurrentCharacter) == null && !Utils.isDelimitador(this.CurrentCharacter) || Utils.IsNumericSymbol(this.CurrentCharacter));

                                    Grammar grammar = RegexLibrary.ValidateNumericRules(Utils.GetNumericTypes(), lexema);
                                    tokenTable.ListaTokens.Add(new Token(linha, coluna, grammar, lexema));
                                }
                                else if (Utils.IsLiteralCharDefinition(this.CurrentCharacter)) //Tratamento de tipo Char Literal
                                {
                                    do
                                    {
                                        lexema += this.CurrentCharacter;
                                        this.MoveToNextCharacter();
                                    }
                                    while (!Utils.IsLiteralCharDefinition(this.CurrentCharacter));
                                    lexema += this.CurrentCharacter;

                                    Grammar grammar = RegexLibrary.ValidateCharacterRule(Utils.GetCharacterType(), lexema);
                                    tokenTable.ListaTokens.Add(new Token(linha, coluna, grammar, lexema));

                                    if (Utils.GetToken(this.CurrentCharacter) != null)
                                        tokenTable.ListaTokens.Add(new Token(linha, coluna, (Grammar)Utils.GetToken(this.CurrentCharacter), this.CurrentCharacter.ToString()));
                                }

                                else if (Utils.IsComentarioDeLinha(this.CurrentCharacter, (char)this.NextCharacter)) //Comentário de linha
                                {
                                    while (this.CurrentCharacter != '\n')
                                        this.MoveToNextCharacter();
                                }
                                else if (Utils.IsInicioComentarioDeBloco(this.CurrentCharacter, (char)this.NextCharacter))
                                {
                                    while (!Utils.IsFimComentarioDeBloco(this.CurrentCharacter, (char)this.NextCharacter))
                                        this.MoveToNextCharacter();
                                }
                                else //Tratamento de palavras reservadas / identificadores
                                {
                                    while (Utils.GetToken(this.CurrentCharacter) == null && !Utils.isDelimitador(this.CurrentCharacter))
                                    {
                                        lexema += this.CurrentCharacter;
                                        this.MoveToNextCharacter();
                                    }
                                    this.BackToPreviousCharacter();

                                    if (Utils.GetPalavraReservada(lexema) != null)
                                    {
                                        Grammar palavraReservada = (Grammar)Utils.GetPalavraReservada(lexema);
                                        tokenTable.ListaTokens.Add(new Token(linha, coluna, palavraReservada, lexema));
                                    }
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


        /// <summary>
        /// Função usada para mover o ponteiro para o próximo caractere lido
        /// </summary>
        private void MoveToNextCharacter()
        {
            this.PreviousCharacter = this.CurrentCharacter;
            this.CurrentCharacter = this.code[++this.counter];
            this.NextCharacter = (this.counter > this.code.Length ? (char?)null : this.code[this.counter + 1]);
            IncrementRowColumn(this.CurrentCharacter, ref this.linha, ref this.coluna);
        }

        /// <summary>
        /// Funcção usada para retroceder o ponteiro para o caracter lido anterior
        /// </summary>
        private void BackToPreviousCharacter()
        {
            this.NextCharacter = (char?)this.CurrentCharacter;
            this.CurrentCharacter = this.code[--this.counter];
            this.PreviousCharacter = this.code[this.counter - 1];
            this.DecrementRowColumn(this.CurrentCharacter, ref this.linha, ref this.coluna);
        }

        /// <summary>
        /// Função usada para incrementar a posição atual da linha e da coluna do arquivo.
        /// </summary>
        /// <param name="caracter">Caractere lido</param>
        /// <param name="linha">Linha atual</param>
        /// <param name="coluna">Coluna atual</param>
        private void IncrementRowColumn(char caracter, ref int linha, ref int coluna)
        {
            if (Utils.isDelimitador(caracter))
            {
                if (caracter == ' ')
                    coluna++;
                else if (caracter == '\t')
                    coluna += 4;
                else if (caracter == '\n' || caracter == '\r')
                {
                    coluna = 0;
                    linha++;
                }
            }
            else
                coluna++;
        }

        /// <summary>
        /// Função usada para decrementar a posição atual da linha e da coluna do arquivo.
        /// </summary>
        /// <param name="caracter">Caractere lido</param>
        /// <param name="linha">Linha atual</param>
        /// <param name="coluna">Coluna atual</param>
        private void DecrementRowColumn(char caracter, ref int linha, ref int coluna)
        {
            if (Utils.isDelimitador(caracter))
            {
                if (caracter == ' ')
                    coluna--;
                else if (caracter == '\t')
                    coluna -= 4;
                else if (caracter == '\n' || caracter == '\r')
                {
                    coluna = 0;
                    linha--;
                }
            }
            else
                coluna--;
        }




    }
}
