using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Scanner
{
    class InvalidNumericFormatException : Exception
    {
        const string invalidNumericFormat = "A sequencia numérica {0} está em um formato incorreto.";

        public InvalidNumericFormatException(string lexema, char currentCharacter, int linha, int coluna)
            : base(ExceptionHelper.FormatMessage(string.Format(invalidNumericFormat, ExceptionHelper.GetCharacterName(lexema)), linha, coluna, ExceptionHelper.GetCharacterName(currentCharacter)))
        {
        }
    }
}
