using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Parser
{
    class ExpectedTokenException : Exception
    {
        public ExpectedTokenException(Token TokenLido, params Gramatica[] TokensEsperados)
           : base(ExceptionHelper.GenerateExpectedTokenExceptionMessage(TokenLido, TokensEsperados))
        {

        }

    }
}
