using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Semantico
{
    public class IncompatibleTypesException : Exception
    {
        public IncompatibleTypesException(Symbol var1, Token Op, Symbol var2) : 
            base(string.Format("Os tipos '{0}' e '{1}' são incompatíveis para a operação '{2}'. Linha: {3}, Coluna: {4}", var1.Type.ToString(), var2.Type.ToString(), Op.ToString(), var1.LexicalToken.Linha, var1.LexicalToken.Coluna))
        {

        }
    }
}
