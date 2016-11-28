using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class Symbol
    {
        public Token Type { get; private set; }
        public Token ReturnType { get; private set; }
        public string Identifier { get{return this.LexicalToken.Lexema;}}
        public int Scope { get; private set; }
        public LexicalToken LexicalToken { get; private set; }

		private string name;
		public string Name 
		{ 
			get
			{
				if (this.Type == Token.CharValue)
					return string.Format ("'{0}'", this.name);
				else
					return this.name;
			} 
		}
        public Token? Operation { get; private set; }


        public Symbol(Token Type, LexicalToken LexicalToken, int Scope)
        {
            this.Type = Type;
            this.ReturnType = this.DefineReturnType(Type);
            this.LexicalToken = LexicalToken;
            this.Scope = Scope;
            this.name = LexicalToken.Lexema;
        }

        public void SetVariableName(string Name)
        {
            this.name = Name;
        }

        public void ChangeReturnType(Token Type)
        {
            this.ReturnType = Type;
        }

        public void SetOperation(Token op)
        {
            this.Operation = op;
        }

        private Token DefineReturnType(Token Type)
        {
            if (Type == Token.FloatValue)
            {
                return Token.Float;
            }
            else if (Type == Token.IntValue)
            {
                return Token.Int;
            }
            else if (Type == Token.CharValue)
            {
                return Token.Char;
            }
            else
            {
                return Type;
            }
        }

    }
}
