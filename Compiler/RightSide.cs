using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class RightSide
    {
        public Token Type { get; private set; }
        public string Expression { get; private set; }

        public RightSide(Token Type, string Expression)
        {
            this.Expression = Expression;
            this.Type = Type;
        }
    }
}
