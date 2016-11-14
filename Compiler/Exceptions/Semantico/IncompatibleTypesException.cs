using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Semantico
{
    public class IncompatibleTypesException : Exception
    {
        public IncompatibleTypesException(Token Type1,  Token Op, Token Type2) : 
            base(string.Format("Os tipos '{0}' e '{1}' são incompatíveis para a operação '{2}'", Type1.ToString(), Type2.ToString(), Op.ToString()))
        {

        }
    }
}
