using Compiler.Exceptions.Parser;
using Compiler.Exceptions.Semantico;
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
        private Semantic semantic;

        private LexicalToken NextToken
        {
            get
            {
                if (GetLookAhead.Token == Token.EndOfFile)
                    return GetLookAhead;
                else
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

        internal void Analisar()
        {
            this.Programa();
        }

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            this.semantic = new Semantic();
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
            Token type = this.Tipo();
            if (GetLookAhead.Token == Token.Identificador)
            {
                this.semantic.AddToSymbolTable(type, GetLookAhead);
                this.GetNextToken();

                while (HasInlineDeclaracoes())
                {
                    if (NextToken.Token == Token.Vírgula)
                    {
                        if (GetLookAhead.Token == Token.Identificador)
                        {
                            this.semantic.AddToSymbolTable(type, GetLookAhead);
                            this.GetNextToken();
                            continue;
                        }
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

        private Token Tipo()
        {
            const string nomeFuncao = "Tipo";
            if (GetLookAhead.Token == Token.Int ||
                GetLookAhead.Token == Token.Float ||
                GetLookAhead.Token == Token.Char)
            {
                Token type = GetLookAhead.Token;
                this.GetNextToken();
                return type;
            }
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
            this.semantic.CreateScope();
            const string nomeFuncao = "Bloco";
            //<bloco> ::= “{“ {<decl_var>}* {<comando>}* “}”
            if (NextToken.Token == Token.AbreChave)
            {
                while (this.IsFirstDeclaracaoVariavel())
                    this.DeclaracaoVariavel();
                
                while(this.IsFirstComando())
                    this.Comando();

                if (NextToken.Token == Token.FechaChave)
                {
                    this.semantic.RemoveCurrentScope();
                    return;
                }
                    
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
            LexicalToken variable;

            const string nomeFuncao = "Atribuicao";
            //<atribuição> ::= < id > "=" < expr_arit > ";"
            if (GetLookAhead.Token == Token.Identificador)
            {
                variable = NextToken;
                Token Type = this.semantic.GetVariable(variable.Lexema).Type;
                if (NextToken.Token == Token.Atribuição)
                {
                    if (!this.semantic.VariableIsDeclared(variable.Lexema))
                        throw new VariableNotDeclaredException(variable);
                    else
                    {

                        var exp = this.ExpressaoAritmetica();
                        
                        if (Type != exp.Type)
                            throw new Exception("atribuição invalida!!!11!!1");


                        if (NextToken.Token == Token.PontoVírgula)
                            return;
                        else
                            throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.PontoVírgula);

                }
            }
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Atribuição);
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Identificador);
        }

        private void ExpressaoRelacional()
        {
            //<expr_relacional> ::= <expr_arit> <op_relacional> <expr_arit>
            Symbol op1 = this.ExpressaoAritmetica();
            Token operador = this.OperadorRelacional();
            Symbol op2 = this.ExpressaoAritmetica();

            if (this.semantic.IsCompatible(op1, op2))
                return;
            else
                throw new IncompatibleTypesException(op1.Type, operador, op2.Type);


        }

        private Symbol ExpressaoAritmetica()
        {
            //<expr_arit> ::= <expr_arit> "+" <termo>   | <expr_arit> "-" <termo> | <termo>
            
            Symbol Operador1 = this.Termo();
            Token Operador = GetLookAhead.Token;
            Symbol Operador2 = this.ExpressaoAritmetica(true);

            if (Operador2 != null)
            {
                if (this.semantic.IsCompatible(Operador1, Operador2))
                {
                    var resultingType = this.semantic.GetResultingType(Operador1, Operador, Operador2);
                    Operador1.ChangeType(resultingType);
                }
                else
                    throw new IncompatibleTypesException(Operador1.Type, Operador, Operador2.Type);
            }
            return Operador1;
        }

        private Symbol ExpressaoAritmetica(bool trick)
        {
            Symbol Operador1 = null;
            if (GetLookAhead.Token == Token.Soma || 
               GetLookAhead.Token == Token.Subtração)
            {
                this.GetNextToken();
                Operador1 = this.Termo();
                var Operador2 = this.ExpressaoAritmetica(trick);
            }

            return Operador1;
        }

        private Symbol Termo()
        {
            //< termo > ::= < termo > "*" < fator > | < termo > “/” < fator > | < fator >
            var op1 = this.Fator();
            while (GetLookAhead.Token == Token.Multiplicação || GetLookAhead.Token == Token.Divisão)
            {
                Token op = GetLookAhead.Token;
                this.GetNextToken();
                var op2 = this.Fator();
                if (op2 != null)
                {
                    if (this.semantic.IsCompatible(op1, op2))
                    {
                        var resultingType = this.semantic.GetResultingType(op1, op, op2);
                        op1.ChangeType(resultingType);
                    }
                    else
                        throw new IncompatibleTypesException(op1.Type, op, op2.Type);
                }
            }

            return op1;
        }

        private Symbol Fator()
        {
            const string nomeFuncao = "Fator";
            //< fator > ::= “(“ < expr_arit > “)” | < id > | < real > | < inteiro > | < char >
            if (GetLookAhead.Token == Token.AbreParenteses)
            {
                this.GetNextToken();
                var Symbol = this.ExpressaoAritmetica();
                if(NextToken.Token == Token.FechaParenteses)
                {
                    return Symbol;
                }
            }
            else if (GetLookAhead.Token == Token.Identificador ||
                     GetLookAhead.Token == Token.FloatValue ||
                     GetLookAhead.Token == Token.IntValue ||
                     GetLookAhead.Token == Token.CharValue)
            {
                var Symbol = new Symbol(GetLookAhead.Token, GetLookAhead.Lexema, semantic.Scope);
                this.GetNextToken();
                return Symbol;
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.AbreParenteses, 
                            Token.Identificador, Token.Float, Token.Int, Token.Char);

            return null;
        }

        private Token OperadorRelacional()
        {
            const string nomeFuncao = "OperadorRelacional";
            if (EnumUtils<Token>.GetFromCategory("Comparador").Exists(x => x == GetLookAhead.Token))
            {
                Token Operador = GetLookAhead.Token;
                this.GetNextToken();
                return Operador;
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, EnumUtils<Token>.GetFromCategory("Comparador").ToArray());
        }

        #endregion

    }
}