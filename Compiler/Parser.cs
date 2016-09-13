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
        private Token LookAtNextToken
        {
            get
            {
                return this.scanner.LookAtNextToken();
            }
        }

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            try
            {
                Programa();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        
        #region Checkers
        private bool IsDeclaracaoVariavel()
        {
            return (LookAtNextToken.Gramatica == Gramatica.Int ||
                    LookAtNextToken.Gramatica == Gramatica.Float ||
                    LookAtNextToken.Gramatica == Gramatica.Char);
        }

        private bool IsComando()
        {

            return (LookAtNextToken.Gramatica == Gramatica.Do ||
                   LookAtNextToken.Gramatica == Gramatica.While ||
                   LookAtNextToken.Gramatica == Gramatica.Identificador ||
                   LookAtNextToken.Gramatica == Gramatica.If ||
                   LookAtNextToken.Gramatica == Gramatica.AbreChave);

        }

        private bool IsComandoBasico()
        {
            return (LookAtNextToken.Gramatica == Gramatica.Identificador ||
                    LookAtNextToken.Gramatica == Gramatica.AbreChave);
        }

        private bool IsIteracao()
        {
            return (LookAtNextToken.Gramatica == Gramatica.While ||
                    LookAtNextToken.Gramatica == Gramatica.Do);
        }

        private bool HasInlineDeclaracoes()
        {
            return LookAtNextToken.Gramatica == Gramatica.Vírgula;
        }

        #endregion

        #region Produções

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
                    }
                }
            }
        }

        private void Bloco()
        {
            //<bloco> ::= “{“ {<decl_var>}* {<comando>}* “}”
            if (NextToken.Gramatica == Gramatica.AbreChave)
            {
                while (this.IsDeclaracaoVariavel())
                    this.DeclaracaoVariavel();
                
                while(this.IsComando())
                    this.Comando();

                if (NextToken.Gramatica == Gramatica.FechaChave)
                    return;
            }
        }



        private void DeclaracaoVariavel()
        {
            var TipoVariavel = NextToken.Gramatica;

            if(NextToken.Gramatica == Gramatica.Identificador)
            {
                while (HasInlineDeclaracoes())
                {
                    if(NextToken.Gramatica == Gramatica.Vírgula)
                    {
                        if(NextToken.Gramatica == Gramatica.Identificador)
                        {
                            continue;
                        }
                    }
                }
                if (NextToken.Gramatica == Gramatica.PontoVírgula)
                    return;
            }
        }

        private void Comando()
        {
            //<comando> ::= <comando_básico> | <iteração> | if "("<expr_relacional>")" <comando> {else <comando>}?

            if (IsComandoBasico())
                this.ComandoBasico();
            else if (IsIteracao())
                this.Iteracao();
            if(NextToken.Gramatica == Gramatica.If)
            {
                if(NextToken.Gramatica == Gramatica.AbreParenteses)
                {
                    this.ExpressaoRelacional();
                    if(NextToken.Gramatica == Gramatica.FechaParenteses)
                    {
                        this.Comando();
                        if (LookAtNextToken.Gramatica == Gramatica.Else)
                        {
                            //Force to Dequeue
                            if (NextToken.Gramatica == Gramatica.Else)
                            {
                                this.Comando();
                            }

                        }
                    }

                }
            }

        }

        private void Iteracao()
        {
            //< iteração > ::= while "(" < expr_relacional > ")" < comando > | do < comando > while "(" < expr_relacional > ")"";"

            Gramatica token = NextToken.Gramatica;

            if(token == Gramatica.Do)
            {
                this.Comando();
                if(NextToken.Gramatica == Gramatica.While)
                {
                    if(NextToken.Gramatica == Gramatica.AbreParenteses)
                    {
                        this.ExpressaoRelacional();
                        if(NextToken.Gramatica == Gramatica.FechaParenteses)
                        {
                            if(NextToken.Gramatica == Gramatica.PontoVírgula)
                            {
                                return;
                            }
                        }
                    }
                }
                
            }
            else if(token == Gramatica.While)
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
                    }
                }
            }
        }

        private void ExpressaoRelacional()
        {
            throw new NotImplementedException();
        }

        private void ComandoBasico()
        {
            if (LookAtNextToken.Gramatica == Gramatica.AbreChave)
                this.Bloco();
            else if (LookAtNextToken.Gramatica == Gramatica.Identificador)
                this.Atribuicao();
        }

        private void Atribuicao()
        {
            //<atribuição> ::= < id > "=" < expr_arit > ";"
            if(NextToken.Gramatica == Gramatica.Identificador)
            {
                if(NextToken.Gramatica == Gramatica.Atribuição)
                {
                    this.ExpressaoAritmetica();
                    if (NextToken.Gramatica == Gramatica.PontoVírgula)
                        return;
                }
            }
        }


        //TODO:
        private void ExpressaoAritmetica()
        {
            //<expr_arit> ::= <expr_arit> "+" <termo>   | <expr_arit> "-" <termo> | <termo>
            throw new NotImplementedException();
        }




        #endregion

    }
}
