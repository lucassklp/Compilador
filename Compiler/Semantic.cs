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
            this.scope--;
            this.RemoveSymbolsCurrentScope();
        }

        public void AddToSymbolTable(Token Type, LexicalToken GetLookAhead)
        {

            if (this.symbolTable.Exists(x => x.Identifier == GetLookAhead.Lexema && x.Scope == this.scope))
                throw new VariableAlreadyDeclaredInScopeException(GetLookAhead);
            else
                this.symbolTable.Add(new Symbol(Type, GetLookAhead.Lexema, this.scope));
        }



        public Symbol GetVariable(string name)
        {
            if (this.symbolTable.Exists(x => x.Identifier == name))
            {
                return this.symbolTable.Find(x => x.Identifier == name);
            }
            else
                return null;

        }

        public bool VariableIsDeclared(string name)
        {
            return GetVariable(name) != null;
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
                if (this.VariableIsDeclared(left.Identifier))
                    leftToken = this.GetVariable(left.Identifier).Type;
                else
                    throw new Exception("Variavel nao declarada");//VariableNotDeclaredException(left);
            }

            if (right.Type == Token.Identificador)
            {
                if (this.VariableIsDeclared(right.Identifier))
                    rightToken = this.GetVariable(right.Identifier).Type;
                else
                    throw new Exception("Variavel nao declarada");//VariableNotDeclaredException(left);
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

            Token t1 = s1.Type, t2 = s2.Type;
            if (t1 == Token.Identificador)
            {
                if (this.VariableIsDeclared(s1.Identifier))
                    t1 = this.GetVariable(s1.Identifier).Type;
                else
                    throw new Exception("Variavel nao declarada");//VariableNotDeclaredException(left);
            }

            if (t2 == Token.Identificador)
            {
                if (this.VariableIsDeclared(s2.Identifier))
                    t2 = this.GetVariable(s2.Identifier).Type;
                else
                    throw new Exception("Variavel nao declarada");//VariableNotDeclaredException(left);
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
            






    }
}
