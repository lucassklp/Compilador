using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Semantico
{
    class VariableAlreadyDeclaredInScopeException : Exception
    {
        public VariableAlreadyDeclaredInScopeException(LexicalToken lastTokenTaken) : 
            base(string.Format("A variável '{0}' já foi definida nesse escopo. Linha: {1}, Coluna {2}", lastTokenTaken.Lexema, lastTokenTaken.Linha, lastTokenTaken.Coluna))
        {
        }
    }
}
