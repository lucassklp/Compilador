using System;
using System.Collections.Generic;
using System.IO;
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

        private int linha = 1;
        private int coluna = 0;

        private char CurrentCharacter;
        private char? NextCharacter;


        private Queue<Token> tokenTable;


        public Scanner(string code)
        {
            this.tokenTable = new Queue<Token>();
            this.code = code;
            this.BuildTokenTable();

        }

        public Token NextToken()
        {
            if (this.tokenTable.Count > 0)
                return this.tokenTable.Dequeue();
            else return null;
        }

        public Token LookAtNextToken()
        {
            if (this.tokenTable.Count > 0)
                return this.tokenTable.Peek();
            else return null;
        }

        private void BuildTokenTable()
        {
            try
            {
                this.MoveToNextCharacter();

                while (!Utils.IsEndOfFile(this.counter, this.code.Length))
                {
                    string lexema = string.Empty;

                    if (!Utils.isDelimitador(this.CurrentCharacter)) //Tratamento de delimitadores
                    {
                        if (Utils.IsDigitOrPunto(this.CurrentCharacter)) //Tratamento de tipos numericos literais
                        {
                            while (!Utils.IsToken(this.CurrentCharacter, this.NextCharacter, false) &&
                                    !Utils.isDelimitador(this.CurrentCharacter) &&
                                    Utils.IsNumericSymbol(this.CurrentCharacter) &&
                                    !Utils.IsEndOfFile(this.counter, this.code.Length))
                            {
                                lexema += this.CurrentCharacter;
                                this.MoveToNextCharacter();
                            }

                            if(CurrentCharacter == '.')
                            {
                                lexema += CurrentCharacter;
                                this.MoveToNextCharacter();
                                while (!Utils.IsToken(this.CurrentCharacter, this.NextCharacter, false) &&
                                        !Utils.isDelimitador(this.CurrentCharacter) &&
                                        Utils.IsNumericSymbol(this.CurrentCharacter) &&
                                        !Utils.IsEndOfFile(this.counter, this.code.Length))
                                {
                                    lexema += this.CurrentCharacter;
                                    this.MoveToNextCharacter();
                                }
                            }


                            Gramatica grammar = RegexLibrary.ValidateNumericRules(Utils.GetNumericTypes(), lexema);
                            this.tokenTable.Enqueue(new Token(linha, coluna, grammar, lexema));
                        }
                        else if (Utils.IsLiteralCharDefinition(this.CurrentCharacter)) //Tratamento de tipo Char Literal
                        {
                            this.MoveToNextCharacter();
                            while (!Utils.IsLiteralCharDefinition(this.CurrentCharacter) &&
                                    !Utils.IsEndOfFile(this.counter, this.code.Length))
                            {
                                lexema += this.CurrentCharacter;
                                this.MoveToNextCharacter();
                            }


                            Gramatica grammar = RegexLibrary.ValidateCharacterRule(Utils.GetCharacterType(), lexema);
                            this.tokenTable.Enqueue(new Token(linha, coluna, grammar, lexema));

                            this.MoveToNextCharacter();

                        }
                        else if (Utils.IsComentarioDeLinha(this.CurrentCharacter, (char)this.NextCharacter)) //Comentário de linha
                        {
                            while (this.CurrentCharacter != '\n')
                                this.MoveToNextCharacter();
                        }
                        else if (Utils.IsInicioComentarioDeBloco(this.CurrentCharacter, (char)this.NextCharacter))
                        {
                            while (!Utils.IsFimComentarioDeBloco(this.CurrentCharacter, this.NextCharacter))
                                this.MoveToNextCharacter();
                            this.MoveToNextCharacter();
                            this.MoveToNextCharacter();
                        }
                        else if (Utils.IsToken(this.CurrentCharacter, this.NextCharacter, true))
                        {
                            Gramatica token = Utils.GetToken(Utils.Concat(this.CurrentCharacter, (char)this.NextCharacter));
                            tokenTable.Enqueue(new Token(this.linha, this.coluna, token, Utils.Concat(this.CurrentCharacter, (char)this.NextCharacter)));
                            this.MoveToNextCharacter();
                        }

                        else if (Utils.IsToken(this.CurrentCharacter, this.NextCharacter, false))
                        {
                            Gramatica token = Utils.GetToken(this.CurrentCharacter);
                            tokenTable.Enqueue(new Token(this.linha, this.coluna, token, this.CurrentCharacter.ToString()));
                            this.MoveToNextCharacter();
                        }
                        else if (Utils.IsLetter(this.CurrentCharacter))
                        {
                            while (!Utils.IsToken(this.CurrentCharacter, this.NextCharacter, false) &&
                                    !Utils.isDelimitador(this.CurrentCharacter) &&
                                    !Utils.IsEndOfFile(this.counter, this.code.Length))
                            {
                                lexema += this.CurrentCharacter;
                                this.MoveToNextCharacter();
                            }

                            if (Utils.IsPalavraReservada(lexema))
                            {
                                Gramatica palavraReservada = Utils.GetPalavraReservada(lexema);
                                this.tokenTable.Enqueue(new Token(linha, coluna, palavraReservada, lexema));
                            }
                            else if (Utils.IsIdentifier(lexema))
                                this.tokenTable.Enqueue(new Token(linha, coluna, Gramatica.Identificador, lexema));
                            else
                                throw new Exception(string.Format("Sequencia '{0}' inválida.", lexema, this.linha, this.coluna));
                        }
                        else
                        {
                            throw new Exception(string.Format("Token {0} não identificado.", this.CurrentCharacter, this.linha, this.coluna));
                        }
                    }
                    else
                    {
                        this.MoveToNextCharacter();
                    }
                }
                tokenTable.Enqueue(new Token(linha, coluna, Gramatica.EndOfFile, "EOF"));

            }
            catch(Exception ex)
            {
                Console.WriteLine("{0}: Linha: {1}, Coluna: {2}", ex.Message, linha, coluna);
            }
        }


        /// <summary>
        /// Função usada para mover o ponteiro para o próximo caractere lido
        /// </summary>
        private char MoveToNextCharacter()
        {
            try
            {
                this.CurrentCharacter = this.code[++this.counter];
                this.NextCharacter = (this.counter >= this.code.Length - 1 ? (char?)null : this.code[this.counter + 1]);
                IncrementRowColumn(this.CurrentCharacter, ref this.linha, ref this.coluna);
                return this.CurrentCharacter;
            }
            catch
            {
                this.CurrentCharacter = '\0';
                IncrementRowColumn(this.CurrentCharacter, ref this.linha, ref this.coluna);
            }
            return this.CurrentCharacter;
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
