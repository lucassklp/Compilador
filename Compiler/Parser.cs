using Compiler.Exceptions.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Parser
    {
        private Scanner scanner;
        private LexicalToken NextToken
        {
            get
            {
                return this.scanner.NextToken();
            }
        }
        private LexicalToken GetLookAhead
        {
            get
            {
                return this.scanner.LookAtNextToken();
            }
        }

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            Programa();
        }

        public void GetNextToken()
        {
            //Force to dequeue
            this.scanner.NextToken();
        }

        #region First's
        private bool IsFirstDeclaracaoVariavel()
        {
            return (GetLookAhead.Token == Token.Int ||
                    GetLookAhead.Token == Token.Float ||
                    GetLookAhead.Token == Token.Char);
        }

        private bool IsFirstComando()
        {

            return (GetLookAhead.Token == Token.Do ||
                   GetLookAhead.Token == Token.While ||
                   GetLookAhead.Token == Token.Identificador ||
                   GetLookAhead.Token == Token.If ||
                   GetLookAhead.Token == Token.AbreChave);

        }

        private bool IsFirstComandoBasico()
        {
            return (GetLookAhead.Token == Token.Identificador ||
                    GetLookAhead.Token == Token.AbreChave);
        }

        private bool IsFirstIteracao()
        {
            return (GetLookAhead.Token == Token.While ||
                    GetLookAhead.Token == Token.Do);
        }

        private bool HasInlineDeclaracoes()
        {
            return GetLookAhead.Token == Token.Vírgula;
        }

        private bool IsFirstAtribuicao()
        {
            return GetLookAhead.Token == Token.Identificador;
        }

        private bool IsFirstBloco()
        {
            return GetLookAhead.Token == Token.AbreChave;
        }

        #endregion

        #region Produções

        private void DeclaracaoVariavel()
        {
            const string nomeFuncao = "DeclaracaoVariavel";
            this.Tipo();
            if (NextToken.Token == Token.Identificador)
            {
                while (HasInlineDeclaracoes())
                {
                    if (NextToken.Token == Token.Vírgula)
                    {
                        if (NextToken.Token == Token.Identificador)
                            continue;
                        else
                            throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Identificador);
                    }
                    else
                        throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Vírgula);

                }
                if (NextToken.Token == Token.PontoVírgula)
                    return;
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.PontoVírgula);
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Identificador);
        }

        private void Tipo()
        {
            const string nomeFuncao = "Tipo";
            if (GetLookAhead.Token == Token.Int ||
                GetLookAhead.Token == Token.Float ||
                GetLookAhead.Token == Token.Char)
                this.GetNextToken();
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Int, Token.Float, Token.Char);
        }

        private void Programa()
        {
            const string nomeFuncao = "Programa";
            //<programa> ::= int main"("")" <bloco>
            if (NextToken.Token == Token.Int)
            {
                if(NextToken.Token == Token.Main)
                {
                    if(NextToken.Token == Token.AbreParenteses)
                    {
                        if(NextToken.Token == Token.FechaParenteses)
                        {
                            this.Bloco();
                            if (NextToken.Token == Token.EndOfFile)
                            {
                                return;
                            }
                            else
                                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.EndOfFile);
                        }
                        else
                            throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.FechaParenteses);
                    }
                    else
                        throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.AbreParenteses);
                }
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Main);
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Int);

        }

        private void Bloco()
        {
            const string nomeFuncao = "Bloco";
            //<bloco> ::= “{“ {<decl_var>}* {<comando>}* “}”
            if (NextToken.Token == Token.AbreChave)
            {
                while (this.IsFirstDeclaracaoVariavel())
                    this.DeclaracaoVariavel();
                
                while(this.IsFirstComando())
                    this.Comando();

                if (NextToken.Token == Token.FechaChave)
                    return;
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.FechaChave);
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.AbreChave);
        }

        private void Comando()
        {
            const string nomeFuncao = "Comando";
            //<comando> ::= <comando_básico> | <iteração> | if "("<expr_relacional>")" <comando> {else <comando>}?

            if (IsFirstComandoBasico())
                this.ComandoBasico();
            else if (IsFirstIteracao())
                this.Iteracao();
            else if (NextToken.Token == Token.If)
            {
                if (NextToken.Token == Token.AbreParenteses)
                {
                    this.ExpressaoRelacional();
                    if (NextToken.Token == Token.FechaParenteses)
                    {
                        this.Comando();
                        if (GetLookAhead.Token == Token.Else)
                        {
                            this.GetNextToken();
                            this.Comando();
                        }
                    }
                    else
                        throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.FechaParenteses);
                }
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.AbreParenteses);
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.If, Token.Identificador, Token.AbreChave, Token.While, Token.Do);

        }

        private void ComandoBasico()
        {
            const string nomeFuncao = "ComandoBasico";
            //<comando_básico> ::= <atribuição> | <bloco>

            if (IsFirstAtribuicao())
                this.Atribuicao();
            else if (IsFirstBloco())
                this.Bloco();
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Identificador, Token.AbreChave);

        }

        private void Iteracao()
        {
            const string nomeFuncao = "Iteracao";
            //< iteração > ::= while "(" < expr_relacional > ")" < comando > | do < comando > while "(" < expr_relacional > ")"";"

            Token token = NextToken.Token;

            if (token == Token.Do)
            {
                this.Comando();
                if (NextToken.Token == Token.While)
                {
                    if (NextToken.Token == Token.AbreParenteses)
                    {
                        this.ExpressaoRelacional();
                        if (NextToken.Token == Token.FechaParenteses)
                        {
                            if (NextToken.Token == Token.PontoVírgula)
                            {
                                return;
                            }
                            else
                                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.PontoVírgula);
                        }
                        else
                            throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.FechaParenteses);
                    }
                    else
                        throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.AbreParenteses);
                }
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.While);

            }
            else if (token == Token.While)
            {
                if (NextToken.Token == Token.AbreParenteses)
                {
                    this.ExpressaoRelacional();

                    if (NextToken.Token == Token.FechaParenteses)
                    {
                        this.Comando();
                    }
                    else
                        throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.FechaParenteses);
                }
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.AbreParenteses);
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Do, Token.While);

        }

        private void Atribuicao()
        {
            const string nomeFuncao = "Atribuicao";
            //<atribuição> ::= < id > "=" < expr_arit > ";"
            if (NextToken.Token == Token.Identificador)
            {
                if (NextToken.Token == Token.Atribuição)
                {
                    this.ExpressaoAritmetica();
                    if (NextToken.Token == Token.PontoVírgula)
                        return;
                    else
                        throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.PontoVírgula);
                }
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Identificador);
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Identificador);
        }

        private void ExpressaoRelacional()
        {
            //<expr_relacional> ::= <expr_arit> <op_relacional> <expr_arit>
            this.ExpressaoAritmetica();
            this.OperadorRelacional();
            this.ExpressaoAritmetica();
        }

        //To-do 
        private void ExpressaoAritmetica()
        {
            //<expr_arit> ::= <expr_arit> "+" <termo>   | <expr_arit> "-" <termo> | <termo>
            this.Termo();
            this.ExpressaoAritmetica(true);
        }

        private void ExpressaoAritmetica(bool trick)
        {
            if(GetLookAhead.Token == Token.Soma || 
               GetLookAhead.Token == Token.Subtração)
            {
                this.GetNextToken();
                this.Termo();
                this.ExpressaoAritmetica(true);
            }
        }

        private void Termo()
        {
            //< termo > ::= < termo > "*" < fator > | < termo > “/” < fator > | < fator >
            this.Fator();
            this.Termo(true);
        }

        private void Termo(bool trick)
        {
            if(GetLookAhead.Token == Token.Multiplicação ||
               GetLookAhead.Token == Token.Divisão)
            {
                this.GetNextToken();
                this.Fator();
                this.Termo(trick);
            }
        }


        private void Fator()
        {
            const string nomeFuncao = "Fator";
            //< fator > ::= “(“ < expr_arit > “)” | < id > | < real > | < inteiro > | < char >
            if (GetLookAhead.Token == Token.AbreParenteses)
            {
                this.GetNextToken();
                this.ExpressaoAritmetica();
                if(NextToken.Token == Token.FechaParenteses)
                {
                    return;
                }
            }
            else if (GetLookAhead.Token == Token.Identificador ||
                     GetLookAhead.Token == Token.FloatValue ||
                     GetLookAhead.Token == Token.IntValue ||
                     GetLookAhead.Token == Token.CharValue)
            {
                this.GetNextToken();
                return;
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.AbreParenteses, 
                            Token.Identificador, Token.Float, Token.Int, Token.Char);
        }

        private void OperadorRelacional()
        {
            const string nomeFuncao = "OperadorRelacional";
            if (EnumUtils<Token>.GetFromCategory("Comparador").Exists(x => x == GetLookAhead.Token))
                this.GetNextToken();
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, EnumUtils<Token>.GetFromCategory("Comparador").ToArray());
        }

        #endregion

    }
}