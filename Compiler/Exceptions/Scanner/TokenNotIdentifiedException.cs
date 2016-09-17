using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Scanner
{
    class TokenNotIdentifiedException : Exception
    {
        const string tokenNotIdentified = "O caracter '{0}' é inválido pois não é um token válido.";
        public TokenNotIdentifiedException(int linha, int coluna, char ultimoCaracterLido)
            :base(ExceptionHelper.FormatMessage(string.Format(tokenNotIdentified, ultimoCaracterLido), linha, coluna, ExceptionHelper.GetCharacterName(ultimoCaracterLido)))
        {

        }
    }
}
