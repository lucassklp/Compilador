using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Parser
{
    class ExpectedTokenException : Exception
    {
        public ExpectedTokenException(string NomeFuncao, LexicalToken TokenLido, params Token[] TokensEsperados)
           : base(ExceptionHelper.GenerateExpectedTokenExceptionMessage(TokenLido, TokensEsperados, NomeFuncao))
        {

        }

    }
}
