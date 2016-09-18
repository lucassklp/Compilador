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
        private Token NextToken
        {
            get
            {
                return this.scanner.NextToken();
            }
        }
        private Token GetLookAhead
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
            return (GetLookAhead.Gramatica == Gramatica.Int ||
                    GetLookAhead.Gramatica == Gramatica.Float ||
                    GetLookAhead.Gramatica == Gramatica.Char);
        }

        private bool IsFirstComando()
        {

            return (GetLookAhead.Gramatica == Gramatica.Do ||
                   GetLookAhead.Gramatica == Gramatica.While ||
                   GetLookAhead.Gramatica == Gramatica.Identificador ||
                   GetLookAhead.Gramatica == Gramatica.If ||
                   GetLookAhead.Gramatica == Gramatica.AbreChave);

        }

        private bool IsFirstComandoBasico()
        {
            return (GetLookAhead.Gramatica == Gramatica.Identificador ||
                    GetLookAhead.Gramatica == Gramatica.AbreChave);
        }

        private bool IsFirstIteracao()
        {
            return (GetLookAhead.Gramatica == Gramatica.While ||
                    GetLookAhead.Gramatica == Gramatica.Do);
        }

        private bool HasInlineDeclaracoes()
        {
            return GetLookAhead.Gramatica == Gramatica.Vírgula;
        }

        private bool IsFirstAtribuicao()
        {
            return GetLookAhead.Gramatica == Gramatica.Identificador;
        }

        private bool IsFirstBloco()
        {
            return GetLookAhead.Gramatica == Gramatica.AbreChave;
        }

        #endregion

        #region Produções

        private void DeclaracaoVariavel()
        {
            this.Tipo();
            if (NextToken.Gramatica == Gramatica.Identificador)
            {
                while (HasInlineDeclaracoes())
                {
                    if (NextToken.Gramatica == Gramatica.Vírgula)
                    {
                        if (NextToken.Gramatica == Gramatica.Identificador)
                            continue;
                        else
                            throw new ExpectedTokenException(GetLookAhead, Gramatica.Identificador);
                    }
                    else
                        throw new ExpectedTokenException(GetLookAhead, Gramatica.Vírgula);

                }
                if (NextToken.Gramatica == Gramatica.PontoVírgula)
                    return;
                else
                    throw new ExpectedTokenException(GetLookAhead, Gramatica.PontoVírgula);
            }
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.Identificador);
        }

        private void Tipo()
        {
            if (GetLookAhead.Gramatica == Gramatica.Int ||
                GetLookAhead.Gramatica == Gramatica.Float ||
                GetLookAhead.Gramatica == Gramatica.Char)
                this.GetNextToken();
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.Int, Gramatica.Float, Gramatica.Char);
        }

        private void Programa()
        {
            //<programa> ::= int main"("")" <bloco>
            if (NextToken.Gramatica == Gramatica.Int)
            {
                if(NextToken.Gramatica == Gramatica.Main)
                {
                    if(NextToken.Gramatica == Gramatica.AbreParenteses)
                    {
                        if(NextToken.Gramatica == Gramatica.FechaParenteses)
                        {
                            this.Bloco();
                        }
                        else
                            throw new ExpectedTokenException(GetLookAhead, Gramatica.FechaParenteses);
                    }
                    else
                        throw new ExpectedTokenException(GetLookAhead, Gramatica.AbreParenteses);
                }
                else
                    throw new ExpectedTokenException(GetLookAhead, Gramatica.Main);
            }
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.Int);

        }

        private void Bloco()
        {
            //<bloco> ::= “{“ {<decl_var>}* {<comando>}* “}”
            if (NextToken.Gramatica == Gramatica.AbreChave)
            {
                while (this.IsFirstDeclaracaoVariavel())
                    this.DeclaracaoVariavel();
                
                while(this.IsFirstComando())
                    this.Comando();

                if (NextToken.Gramatica == Gramatica.FechaChave)
                    return;
                else
                    throw new ExpectedTokenException(GetLookAhead, Gramatica.FechaChave);
            }
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.AbreChave);
        }

        private void Comando()
        {
            //<comando> ::= <comando_básico> | <iteração> | if "("<expr_relacional>")" <comando> {else <comando>}?

            if (IsFirstComandoBasico())
                this.ComandoBasico();
            else if (IsFirstIteracao())
                this.Iteracao();
            if (NextToken.Gramatica == Gramatica.If)
            {
                if (NextToken.Gramatica == Gramatica.AbreParenteses)
                {
                    this.ExpressaoRelacional();
                    if (NextToken.Gramatica == Gramatica.FechaParenteses)
                    {
                        this.Comando();
                        if (GetLookAhead.Gramatica == Gramatica.Else)
                        {
                            this.GetNextToken();
                            this.Comando();
                        }
                    }
                    else
                        throw new ExpectedTokenException(GetLookAhead, Gramatica.FechaParenteses);
                }
                else
                    throw new ExpectedTokenException(GetLookAhead, Gramatica.AbreParenteses);
            }
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.If);

        }

        private void ComandoBasico()
        {
            //<comando_básico> ::= <atribuição> | <bloco>

            if (IsFirstAtribuicao())
                this.Atribuicao();
            else if (IsFirstBloco())
                this.Bloco();
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.Identificador, Gramatica.AbreChave);

        }

        private void Iteracao()
        {
            //< iteração > ::= while "(" < expr_relacional > ")" < comando > | do < comando > while "(" < expr_relacional > ")"";"

            Gramatica token = NextToken.Gramatica;

            if (token == Gramatica.Do)
            {
                this.Comando();
                if (NextToken.Gramatica == Gramatica.While)
                {
                    if (NextToken.Gramatica == Gramatica.AbreParenteses)
                    {
                        this.ExpressaoRelacional();
                        if (NextToken.Gramatica == Gramatica.FechaParenteses)
                        {
                            if (NextToken.Gramatica == Gramatica.PontoVírgula)
                            {
                                return;
                            }
                            else
                                throw new ExpectedTokenException(GetLookAhead, Gramatica.PontoVírgula);
                        }
                        else
                            throw new ExpectedTokenException(GetLookAhead, Gramatica.FechaParenteses);
                    }
                    else
                        throw new ExpectedTokenException(GetLookAhead, Gramatica.AbreParenteses);
                }
                else
                    throw new ExpectedTokenException(GetLookAhead, Gramatica.While);

            }
            else if (token == Gramatica.While)
            {
                if (NextToken.Gramatica == Gramatica.AbreParenteses)
                {
                    this.ExpressaoRelacional();

                    if (NextToken.Gramatica == Gramatica.FechaParenteses)
                    {
                        this.Comando();
                        if (NextToken.Gramatica == Gramatica.PontoVírgula)
                        {
                            return;
                        }
                        else
                            throw new ExpectedTokenException(GetLookAhead, Gramatica.PontoVírgula);
                    }
                    else
                        throw new ExpectedTokenException(GetLookAhead, Gramatica.FechaParenteses);
                }
                else
                    throw new ExpectedTokenException(GetLookAhead, Gramatica.AbreParenteses);
            }
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.Do, Gramatica.While);

        }

        private void Atribuicao()
        {
            //<atribuição> ::= < id > "=" < expr_arit > ";"
            if (NextToken.Gramatica == Gramatica.Identificador)
            {
                if (NextToken.Gramatica == Gramatica.Atribuição)
                {
                    this.ExpressaoAritmetica();
                    if (NextToken.Gramatica == Gramatica.PontoVírgula)
                        return;
                    else
                        throw new ExpectedTokenException(GetLookAhead, Gramatica.PontoVírgula);
                }
                else
                    throw new ExpectedTokenException(GetLookAhead, Gramatica.Identificador);
            }
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.Identificador);
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
            if(GetLookAhead.Gramatica == Gramatica.Soma || 
               GetLookAhead.Gramatica == Gramatica.Subtração)
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
            if(GetLookAhead.Gramatica == Gramatica.Multiplicação ||
               GetLookAhead.Gramatica == Gramatica.Divisão)
            {
                this.GetNextToken();
                this.Fator();
                this.Termo(trick);
            }
        }


        private void Fator()
        {
            //< fator > ::= “(“ < expr_arit > “)” | < id > | < real > | < inteiro > | < char >
            if(GetLookAhead.Gramatica == Gramatica.AbreParenteses)
            {
                this.GetNextToken();
                this.ExpressaoAritmetica();
                if(NextToken.Gramatica == Gramatica.FechaParenteses)
                {
                    return;
                }
            }
            else if (GetLookAhead.Gramatica == Gramatica.Identificador ||
                     GetLookAhead.Gramatica == Gramatica.FloatValue ||
                     GetLookAhead.Gramatica == Gramatica.IntValue ||
                     GetLookAhead.Gramatica == Gramatica.CharValue)
            {
                this.GetNextToken();
                return;
            }
            else
                throw new ExpectedTokenException(GetLookAhead, Gramatica.AbreParenteses, 
                            Gramatica.Identificador, Gramatica.Float, Gramatica.Int, Gramatica.Char);
        }

        private void OperadorRelacional()
        {
            if (EnumUtils<Gramatica>.GetFromCategory("Comparador").Exists(x => x == GetLookAhead.Gramatica))
                this.GetNextToken();
            else
                throw new ExpectedTokenException(GetLookAhead, EnumUtils<Gramatica>.GetFromCategory("Comparador").ToArray());
        }

        #endregion

    }
}