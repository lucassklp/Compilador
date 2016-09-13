using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions
{
    class ExceptionHelper : Exception
    {
        public static string FormatMessage(string Error, int Linha, int Coluna, char ultimoCaracterLido)
        {
            string ultimoCaractereLido_String;
            if (ultimoCaracterLido == '\t')
                ultimoCaractereLido_String = @"Tabulação (\t)";
            else if (ultimoCaracterLido == '\r' || ultimoCaracterLido == '\n')
                ultimoCaractereLido_String = "Return (Enter)";
            else
                ultimoCaractereLido_String = ultimoCaracterLido.ToString();
            string b = string.Format("{0} \nLinha: {1}, Coluna: {2}, Ultimo caractere lido: \'{3}\'.", Error, Linha, Coluna, ultimoCaractereLido_String);

            return b;
        }
    }
}
