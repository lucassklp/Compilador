﻿using Compiler.Exceptions.Parser;
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

        #region Helpers, Variables, Properties and Constructor
        private Scanner scanner;
        private Semantic semantic;
        private IntermediateCodeGenerator codeGenerator;

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

        public Parser(Scanner scanner, IntermediateCodeGenerator codeGenerator)
        {
            this.scanner = scanner;
            this.semantic = new Semantic();
            this.codeGenerator = codeGenerator;
        }

        public void GetNextToken()
        {
            //Force to dequeue
            this.scanner.NextToken();
        }
        #endregion

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
                    string after = codeGenerator.GenerateLabelName();

                    var exp = this.ExpressaoRelacional();

                    codeGenerator.AppendConditionalGoto(exp, after);
                    
                    if (NextToken.Token == Token.FechaParenteses)
                    {
                        this.Comando();

                        string begin = codeGenerator.GenerateLabelName();
                        codeGenerator.AppendInconditionalGoto(begin);
                        codeGenerator.AppendLabel(after);

                        if (GetLookAhead.Token == Token.Else)
                        {
                            this.GetNextToken();
                            this.Comando();
                        }

                        codeGenerator.AppendLabel(begin);
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
                string begin = codeGenerator.GenerateLabelName();
                string after = codeGenerator.GenerateLabelName();

                codeGenerator.AppendLabel(begin);

                this.Comando();
                if (NextToken.Token == Token.While)
                {
                    if (NextToken.Token == Token.AbreParenteses)
                    {
                        var exp = this.ExpressaoRelacional();

                        codeGenerator.AppendConditionalGoto(exp, after);
                        codeGenerator.AppendInconditionalGoto(begin);
                        codeGenerator.AppendLabel(after);

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
                    string begin = codeGenerator.GenerateLabelName();
                    string after = codeGenerator.GenerateLabelName();

                    codeGenerator.AppendLabel(begin);

                    var exp = this.ExpressaoRelacional(); 
                    codeGenerator.AppendConditionalGoto(exp, after);
                    
                    if (NextToken.Token == Token.FechaParenteses)
                    {
                        this.Comando();

                        codeGenerator.AppendInconditionalGoto(begin);
                        codeGenerator.AppendLabel(after);
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
            Symbol variable;

            const string nomeFuncao = "Atribuicao";
            //<atribuição> ::= < id > "=" < expr_arit > ";"
            if (GetLookAhead.Token == Token.Identificador)
            {
                var temp = NextToken;

                variable = new Symbol(this.semantic.GetVariable(temp).Type, temp, this.semantic.Scope);

                if (NextToken.Token == Token.Atribuição)
                {
                    var exp = this.ExpressaoAritmetica();

                    this.TypeHandler(variable, Token.Atribuição, exp);

                    codeGenerator.AppendAttribuition(variable, exp);
                    

                    if (!this.semantic.IsCorrectAttribution(variable.Type, exp.ReturnType))
                        throw new InvalidAttributionException(exp, variable);


                    if (NextToken.Token == Token.PontoVírgula)
                        return;
                    else
                        throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.PontoVírgula);
                }
                else
                    throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Atribuição);
            }
            else
                throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.Identificador);
        }

        private Symbol ExpressaoRelacional()
        {
            //<expr_relacional> ::= <expr_arit> <op_relacional> <expr_arit>
            Symbol op1 = this.ExpressaoAritmetica();
            Token operador = this.OperadorRelacional();
            Symbol op2 = this.ExpressaoAritmetica();
            
            this.TypeHandler(op1, operador, op2);
            this.OperationHandler(op1, operador, op2);
            
            return op1;
        }

        private Symbol ExpressaoAritmetica()
        {
            //<expr_arit> ::= <expr_arit> "+" <termo>   | <expr_arit> "-" <termo> | <termo>
            
            Symbol Operador1 = this.Termo();
            Token Operador = GetLookAhead.Token;
            Symbol Operador2 = this.ExpressaoAritmetica(true);

            this.TypeHandler(Operador1, Operador, Operador2);
            this.OperationHandler(Operador1, Operador, Operador2);


            return Operador1;
        }

        private Symbol ExpressaoAritmetica(bool trick)
        {
            Symbol Operador1 = null;
            if (GetLookAhead.Token == Token.Soma || 
               GetLookAhead.Token == Token.Subtração)
            {
                Token Operador = GetLookAhead.Token;
                this.GetNextToken();
                Operador1 = this.Termo();
                var Operador2 = this.ExpressaoAritmetica(trick);

                this.TypeHandler(Operador1, Operador, Operador2);
                this.OperationHandler(Operador1, Operador, Operador2);                
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

                this.TypeHandler(op1, op, op2);
                this.OperationHandler(op1, op, op2);
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
            else if (GetLookAhead.Token == Token.Identificador)
            {
                Token Type;

                Type = this.semantic.GetVariable(GetLookAhead).Type;

                var Symbol = new Symbol(Type, GetLookAhead, semantic.Scope);
                this.GetNextToken();
                return Symbol;
            }
            else if (GetLookAhead.Token == Token.FloatValue ||
                     GetLookAhead.Token == Token.IntValue ||
                     GetLookAhead.Token == Token.CharValue)
            {
                var Symbol = new Symbol(GetLookAhead.Token, GetLookAhead, semantic.Scope);
                this.GetNextToken();
                return Symbol;
            }


            throw new ExpectedTokenException(nomeFuncao, GetLookAhead, Token.AbreParenteses,
                        Token.Identificador, Token.Float, Token.Int, Token.Char);

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

        #region Handles
        private void TypeHandler(Symbol Operator1, Token Operator, Symbol Operator2)
        {
            if (Operator2 != null)
            {
                if (this.semantic.IsCompatible(Operator1, Operator2))
                { 
                    var resultingType = this.semantic.GetResultingType(Operator1, Operator, Operator2);

					if (Operator != Token.Divisão)
					{
						if (resultingType != Operator1.ReturnType) 
						{
							string varName = this.codeGenerator.AppendConvertTo (resultingType, Operator1);
							Operator1.SetVariableName (varName);
							Operator1.ChangeReturnType (resultingType);
						}

						if (resultingType != Operator2.ReturnType) 
						{
							string varName = this.codeGenerator.AppendConvertTo (resultingType, Operator2);
							Operator2.SetVariableName (varName);
							Operator2.ChangeReturnType (resultingType);
						}
					} 
					else 
					{
					
						if (Operator1.ReturnType != Operator2.ReturnType) 
						{
							if (Operator1.ReturnType == Token.Int) 
							{
								string varName = this.codeGenerator.AppendConvertTo (Token.Float, Operator1);
								Operator1.SetVariableName (varName);
								Operator1.ChangeReturnType(Token.Float);	
							}

							if (Operator2.ReturnType == Token.Int) 
							{
								string varName = this.codeGenerator.AppendConvertTo (Token.Float, Operator2);
								Operator2.SetVariableName (varName);
								Operator2.ChangeReturnType(Token.Float);	
							}
						}

						if (resultingType != Operator1.ReturnType) 
						{
							Operator1.ChangeReturnType (Token.Float);
						}
					}

                }
                else
                    throw new IncompatibleTypesException(Operator1, Operator, Operator2);
            }
        }


        private void OperationHandler(Symbol Operator1, Token Operator, Symbol Operator2)
        {
            if (Operator1 != null && Operator2 != null)
            {
                string CurrentVariableName;
                if (Operator2.Operation != null)
                    CurrentVariableName = codeGenerator.AppendOperation(Operator1, Operator2.Operation.Value, Operator2);
                else
                    CurrentVariableName = codeGenerator.AppendOperation(Operator1, Operator, Operator2); 

                Operator1.SetVariableName(CurrentVariableName);
            }
            else if (Operator1 != null)
            {
                if(EnumUtils<Token>.GetFromCategory("OperadorAritmetrico").Exists(x => x == Operator) ||
                   EnumUtils<Token>.GetFromCategory("Comparador").Exists(x => x == Operator))
                    Operator1.SetOperation(Operator);
            }

        }
        #endregion

    }
}