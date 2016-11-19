using Compiler.Exceptions.Semantico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{

    class Semantic
    {
        private List<Symbol> symbolTable;
        private int scope;


        public List<Symbol> SymbolsCurrentScope
        {
            get
            {
                return this.symbolTable.FindAll(x => x.Scope == this.scope);
            }
        }

        public int Scope
        {
            get
            {
                return this.scope;
            }
        }


        public Semantic()
        {
            this.symbolTable = new List<Symbol>();
            this.scope = 0;
        }


        public void CreateScope()
        {
            this.scope++;
        }

        public void RemoveCurrentScope()
        {
            this.RemoveSymbolsCurrentScope();
            this.scope--;
            
        }

        public void AddToSymbolTable(Token Type, LexicalToken LookAhead)
        {

            if (this.symbolTable.Exists(x => x.Identifier == LookAhead.Lexema && x.Scope == this.scope))
                throw new VariableAlreadyDeclaredInScopeException(LookAhead);
            else
                this.symbolTable.Add(new Symbol(Type, LookAhead, this.scope));
        }



        public Symbol GetVariable(Symbol symbol)
        {
            if (this.symbolTable.Exists(x => x.Identifier == symbol.Identifier))
            {
                return this.symbolTable.FindAll(x => x.Identifier == symbol.Identifier)
                    .OrderByDescending(x => x.Scope).First();
            }
            else
                throw new VariableNotDeclaredException(symbol.LexicalToken);

        }

        public Symbol GetVariable(LexicalToken lt)
        {
            if (this.symbolTable.Exists(x => x.Identifier == lt.Lexema))
            {
                return this.symbolTable.FindAll(x => x.Identifier == lt.Lexema)
                    .OrderByDescending(x => x.Scope).First();
            }
            else
                throw new VariableNotDeclaredException(lt);

        }

        private void RemoveSymbolsCurrentScope()
        {
            this.symbolTable.RemoveAll(x => x.Scope == this.scope);
        }


        public bool IsCompatible(Symbol left, Symbol right)
        {

            Token leftToken = left.Type, rightToken = right.Type;
            if(left.Type == Token.Identificador)
            {
                leftToken = this.GetVariable(left).Type;
            }

            if (right.Type == Token.Identificador)
            {
                rightToken = this.GetVariable(right).Type;
            }


            if (((leftToken == Token.Char || leftToken == Token.CharValue) && (rightToken == Token.Char || rightToken == Token.CharValue)) ||
               ((leftToken == Token.Int || leftToken == Token.IntValue) && (rightToken == Token.Int || rightToken == Token.IntValue)) ||
               ((leftToken == Token.Float || leftToken == Token.FloatValue) && (rightToken == Token.Float || rightToken == Token.FloatValue)) ||
               ((leftToken == Token.Int || leftToken == Token.IntValue) && (rightToken == Token.Float || rightToken == Token.FloatValue)) ||
               ((leftToken == Token.Float || leftToken == Token.FloatValue) && (rightToken == Token.Int || rightToken == Token.IntValue)))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public Token GetResultingType(Symbol s1, Token op, Symbol s2)
        {

            Token t1 = s1.ReturnType, t2 = s2.ReturnType;
            if (t1 == Token.Identificador)
            {
                t1 = this.GetVariable(s1).ReturnType;
            }

            if (t2 == Token.Identificador)
            {
               t2 = this.GetVariable(s2).ReturnType;     
            }

            if (op == Token.Divisão)
            {
                return Token.Float;
            }
            else if (op == Token.Soma || op == Token.Multiplicação || op == Token.Subtração)
            {
                if ((t1 == Token.Float || t1 == Token.FloatValue) || (t2 == Token.Float || t2 == Token.FloatValue))
                    return Token.Float;
                else if ((t1 == Token.Int || t1 == Token.IntValue) && (t2 == Token.Int || t2 == Token.IntValue))
                    return Token.Int;
                else if ((t1 == Token.Char || t1 == Token.CharValue) || (t2 == Token.Char || t2 == Token.CharValue))
                    return Token.Char;
                else
                    throw new Exception("Impossível pegar o tipo resultante.");
            }
            else
                throw new Exception("Impossível pegar o tipo resultante.");
        }

        public bool IsCorrectAttribution(Token leftSide, Token rightSide)
        {
            if ((leftSide == Token.Char) && (rightSide == Token.Char || rightSide == Token.CharValue))
            {
                return true;
            }
            else if ((leftSide == Token.Float) &&
                    ((rightSide == Token.Float || rightSide == Token.FloatValue) ||
                    (rightSide == Token.Int || rightSide == Token.IntValue)))
            {
                return true;
            }
            else if ((leftSide == Token.Int) &&
                (rightSide == Token.IntValue || rightSide == Token.Int))
            {
                return true;
            }
            else 
            {
                return false;
            }
                
        }





    }
}
