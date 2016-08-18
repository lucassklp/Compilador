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

        private char CurrentCharacter;
        private char? NextCharacter;

        private bool increment;

        private Queue<Token> tokenTable;


        public Scanner(string code)
        {
            this.tokenTable = new Queue<Token>();
            this.code = code;
            this.GetTokenTable();

        }

        public Token NextToken()
        {
            if (this.tokenTable.Count > 0)
                return this.tokenTable.Dequeue();
            else return null;
        }

        private void GetTokenTable()
        {

            try
            {
                while (this.counter < this.code.Length)
                {
                    this.MoveToNextCharacter();
                    string lexema = string.Empty;

                    if (!Utils.isDelimitador(this.CurrentCharacter)) //Tratamento de delimitadores
                    {
                        if (this.NextCharacter != null) //Se não é último token
                        {
                            if (Utils.IsDigit(this.CurrentCharacter)) //Tratamento de tipos numericos literais
                            {
                                do
                                {
                                    lexema += this.CurrentCharacter;
                                    this.MoveToNextCharacter();
                                }
                                while (!Utils.IsToken(this.CurrentCharacter, this.NextCharacter, false) && !Utils.isDelimitador(this.CurrentCharacter) || Utils.IsNumericSymbol(this.CurrentCharacter));

                                Grammar grammar = RegexLibrary.ValidateNumericRules(Utils.GetNumericTypes(), lexema);
                                this.tokenTable.Enqueue(new Token(linha, coluna, grammar, lexema));
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
                                this.tokenTable.Enqueue(new Token(linha, coluna, grammar, lexema));

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
                            else if(Utils.IsToken(this.CurrentCharacter, this.NextCharacter, false))
                            {
                                if(Utils.IsToken(this.CurrentCharacter, this.NextCharacter, true))
                                {
                                    Grammar token = Utils.GetToken(Utils.Concat(this.CurrentCharacter, (char)this.NextCharacter));
                                    tokenTable.Enqueue(new Token(this.linha, this.coluna, token, Utils.Concat(this.CurrentCharacter, (char)this.NextCharacter)));
                                }
                                else
                                {
                                    Grammar token = Utils.GetToken(this.CurrentCharacter);
                                    tokenTable.Enqueue(new Token(this.linha, this.coluna, token, this.CurrentCharacter.ToString()));
                                }
                            }
                            else //Tratamento de palavras reservadas / identificadores
                            {
                                while (!Utils.IsToken(this.CurrentCharacter, this.NextCharacter, false) && !Utils.isDelimitador(this.CurrentCharacter))
                                {
                                    lexema += this.CurrentCharacter;
                                    this.MoveToNextCharacter();
                                }
                                this.BackToPreviousCharacter();

                                if (Utils.IsPalavraReservada(lexema))
                                {
                                    Grammar palavraReservada = Utils.GetPalavraReservada(lexema);
                                    this.tokenTable.Enqueue(new Token(linha, coluna, palavraReservada, lexema));
                                }
                                else if (Utils.IsIdentifier(lexema))
                                    this.tokenTable.Enqueue(new Token(linha, coluna, Grammar.Identificador, lexema));
                                else
                                    throw new Exception(string.Format("Token {0} não identificado. \tLinha: {1}, Coluna: {2}", this.CurrentCharacter, this.linha, this.coluna));
                            }
                            
                        }
                    }
                }

                tokenTable.Enqueue(new Token(linha, coluna, Grammar.EndOfFile, "EOF"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


        /// <summary>
        /// Função usada para mover o ponteiro para o próximo caractere lido
        /// </summary>
        private void MoveToNextCharacter()
        {
            this.CurrentCharacter = this.code[++this.counter];
            this.NextCharacter = (this.counter > this.code.Length ? (char?)null : this.code[this.counter + 1]);
            if(this.increment)
                IncrementRowColumn(this.CurrentCharacter, ref this.linha, ref this.coluna);
            increment = true;
        }

        /// <summary>
        /// Funcção usada para retroceder o ponteiro para o caracter lido anterior
        /// </summary>
        private void BackToPreviousCharacter()
        {
            this.CurrentCharacter = this.code[(--this.counter)];
            this.NextCharacter = this.code[this.counter + 1];
            increment = false;
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
