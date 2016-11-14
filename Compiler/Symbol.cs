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


        public Symbol(Token Type, LexicalToken LexicalToken, int Scope)
        {
            this.Type = Type;
            this.ReturnType = Type;

            this.LexicalToken = LexicalToken;
            this.Scope = Scope;
        }

        public void ChangeReturnType(Token Type)
        {
            this.ReturnType = Type;
        }
    }
}
