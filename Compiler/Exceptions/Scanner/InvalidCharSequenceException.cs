using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Scanner
{
    class InvalidCharSequenceException : Exception
    {
        const string invalidCharSequence = "O char '{0}' está em um formato incorreto.";

        public InvalidCharSequenceException(int linha, int coluna, string lexema, char currentCharacter)
            : base(ExceptionHelper.FormatMessage(string.Format(invalidCharSequence, ExceptionHelper.GetCharacterName(lexema)), linha, coluna, ExceptionHelper.GetCharacterName(currentCharacter)))
        {
        }
    }
}
