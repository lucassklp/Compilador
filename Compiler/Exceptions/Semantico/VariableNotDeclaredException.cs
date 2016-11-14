using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Semantico
{
    class VariableNotDeclaredException : Exception
    {
        public VariableNotDeclaredException(LexicalToken lastToken) :
            base(string.Format("A variável '{0}' não foi declarada. Linha {1}, Coluna {2}", lastToken.Lexema, lastToken.Linha, lastToken.Coluna))
        {

        }
    }
}
