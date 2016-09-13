using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Scanner
{
    class TokenNaoIdentificadoException : Exception
    {
        const string tokenNaoIdentificadoMessage = ("O caractere {0} não é um token válido.");

        public TokenNaoIdentificadoException(char currentCharacter, int linha, int coluna)
            : base(ExceptionHelper.FormatMessage(string.Format(tokenNaoIdentificadoMessage, currentCharacter), linha, coluna, currentCharacter))
        {
        }
    }
}
