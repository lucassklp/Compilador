using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Scanner
{
    class ExpectedEndOfCharacterException : Exception
    {
        const string expectedEndOfCharacter = "Fim de arquivo. Não foi possível encontrar fim do caracter definido";
        public ExpectedEndOfCharacterException(int linha, int coluna, char ultimoCaracterLido)
            : base(ExceptionHelper.FormatMessage(expectedEndOfCharacter, linha, coluna, ExceptionHelper.GetCharacterName(ultimoCaracterLido)))
        {
        }

    }
}
