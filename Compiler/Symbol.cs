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
        public string Identifier { get; private set; }
        public int Scope { get; private set; }

        public Symbol(Token Type, string Identifier, int Scope)
        {
            this.Type = Type;
            this.Identifier = Identifier;
            this.Scope = Scope;
        }

        public void ChangeType(Token Type)
        {
            this.Type = Type;
        }
    }
}
