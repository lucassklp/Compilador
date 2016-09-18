using Compiler.Exceptions.Scanner;
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


        private Queue<LexicalToken> tokenTable;


        public Scanner(string code)
        {
            this.tokenTable = new Queue<LexicalToken>();
            this.code = code;
            this.BuildTokenTable();
        }

        internal void PrintTokens()
        {
            foreach (var item in this.tokenTable)
            {
                Console.WriteLine(item.Lexema +"  =>  " + item.Token);
            }
        }

        public LexicalToken NextToken()
        {
            if (this.tokenTable.Count > 0)
                return tokenTable.Dequeue();
            else return null;
        }

        public LexicalToken LookAtNextToken()
        {
            if (this.tokenTable.Count > 0)
                return this.tokenTable.Peek();
            else return null;
        }

        private void BuildTokenTable()
        {
            this.MoveToNextCharacter();
            while (!Utils.IsEndOfFile(this.counter, this.code.Length))
            {
                string lexema = string.Empty;
                if (!Utils.IsDelimitador(this.CurrentCharacter)) //Tratamento de delimitadores
                {
                    if (Utils.IsDigitOrPunto(this.CurrentCharacter)) //Tratamento de tipos numericos literais
                    {
                        while (Utils.IsNumber(this.CurrentCharacter) &&
                                !Utils.IsEndOfFile(this.counter, this.code.Length))
                        {
                            lexema += this.CurrentCharacter;
                            this.MoveToNextCharacter();
                        }
                        if(CurrentCharacter == '.')
                        {
                            lexema += CurrentCharacter;
                            this.MoveToNextCharacter();
                            while (Utils.IsNumber(this.CurrentCharacter) &&
                                    !Utils.IsEndOfFile(this.counter, this.code.Length))
                            {
                                lexema += this.CurrentCharacter;
                                this.MoveToNextCharacter();
                            }
                        }
                        if (RegexLibrary.IsNumericType(lexema))
                        {
                            Token grammar = RegexLibrary.GetNumericType(lexema);
                            this.tokenTable.Enqueue(new LexicalToken(linha, coluna, grammar, lexema));
                        }
                        else
                            throw new InvalidNumericFormatException(lexema, this.CurrentCharacter, linha, coluna);
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

                        if (Utils.IsEndOfFile(this.counter, this.code.Length))
                            throw new ExpectedEndOfCharacterException(linha, coluna, this.CurrentCharacter);

                        if (RegexLibrary.IsValidCharacter(lexema))
                        {
                            this.tokenTable.Enqueue(new LexicalToken(linha, coluna, Token.CharValue, lexema));
                            this.MoveToNextCharacter();
                        }
                        else
                            throw new InvalidCharSequenceException(linha, coluna, lexema, this.CurrentCharacter);
                    }
                    else if (Utils.IsComentarioDeLinha(this.CurrentCharacter, this.NextCharacter)) //Comentário de linha
                    {
                        while (this.CurrentCharacter != '\n' &&
                                !Utils.IsEndOfFile(this.counter, this.code.Length))
                            this.MoveToNextCharacter();
                    }
                    else if (Utils.IsInicioComentarioDeBloco(this.CurrentCharacter, this.NextCharacter))
                    {
                        while (!Utils.IsFimComentarioDeBloco(this.CurrentCharacter, this.NextCharacter) &&
                                !Utils.IsEndOfFile(this.counter, this.code.Length))
                            this.MoveToNextCharacter();

                        if (Utils.IsEndOfFile(this.counter, this.code.Length))
                            throw new EndOfFileBlockCommentException(linha, coluna, this.CurrentCharacter);
                        else
                        {
                            this.MoveToNextCharacter();
                            this.MoveToNextCharacter();
                        }
                    }
                    else if (Utils.IsToken(this.CurrentCharacter, this.NextCharacter))
                    {
                        Token token = Utils.GetToken(Utils.Concat(this.CurrentCharacter, this.NextCharacter));
                        tokenTable.Enqueue(new LexicalToken(this.linha, this.coluna, token, Utils.Concat(this.CurrentCharacter, (char)this.NextCharacter)));
                        this.MoveToNextCharacter();
                        this.MoveToNextCharacter();
                    }

                    else if (Utils.IsToken(this.CurrentCharacter))
                    {
                        Token token = Utils.GetToken(this.CurrentCharacter);
                        tokenTable.Enqueue(new LexicalToken(this.linha, this.coluna, token, this.CurrentCharacter.ToString()));
                        this.MoveToNextCharacter();
                    }
                    else if (Utils.IsLetterOrUnderline(this.CurrentCharacter))
                    {
                        while (Utils.IsLetterOrUnderline(this.CurrentCharacter) ||
                                Utils.IsNumber(this.CurrentCharacter) &&
                                !Utils.IsEndOfFile(this.counter, this.code.Length))
                        {
                            lexema += this.CurrentCharacter;
                            this.MoveToNextCharacter();
                        }

                        if (Utils.IsPalavraReservada(lexema))
                        {
                            Token palavraReservada = Utils.GetPalavraReservada(lexema);
                            this.tokenTable.Enqueue(new LexicalToken(linha, coluna, palavraReservada, lexema));
                        }
                        else if (Utils.IsIdentifier(lexema))
                            this.tokenTable.Enqueue(new LexicalToken(linha, coluna, Token.Identificador, lexema));
                    }
                    else
                    {
                        throw new TokenNotIdentifiedException(linha, coluna, this.CurrentCharacter);
                    }
                }
                else
                {
                    this.MoveToNextCharacter();
                }
            }
            tokenTable.Enqueue(new LexicalToken(linha, coluna, Token.EndOfFile, "EOF"));
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
            if (Utils.IsDelimitador(caracter))
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