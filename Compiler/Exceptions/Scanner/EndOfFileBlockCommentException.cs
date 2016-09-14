using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Scanner
{
    class EndOfFileBlockCommentException : Exception
    {
        const string endOfFileBlockComment = "Fim de comentário esperado não foi encontrado.";
        public EndOfFileBlockCommentException(int linha, int coluna, char ultimoCaractereLido) 
            : base(ExceptionHelper.FormatMessage(endOfFileBlockComment, linha, coluna, ExceptionHelper.GetCharacterName(ultimoCaractereLido)))
        {

        }
    }
}
