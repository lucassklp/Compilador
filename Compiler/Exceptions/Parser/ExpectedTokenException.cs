using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Parser
{
    class ExpectedTokenException : Exception
    {


        public ExpectedTokenException(Token TokenLido, Gramatica TokenEsperado)
            : base(string.Format("Token '{0}' de lexema(ou formato) '{1}' esperado não foi encontrado.\n"+
                                 "Linha: {2}, Coluna: {3}, Ultimo token lido: '{4}', Lexema do último token lido: '{5}'",
                                 TokenEsperado.ToString(), EnumUtils<Gramatica>.GetDescription(TokenEsperado),
                                 TokenLido.Linha, TokenLido.Coluna, TokenLido.Gramatica, TokenLido.Lexema))
        {

        }


        public ExpectedTokenException(Token TokenLido, params Gramatica[] TokenEsperado)
           : base(string.Format(@"Token '{0}' esperado não foi encontrado." +
                                 "Linha: {1}, Coluna: {2}, Ultimo token lido: '{3}', Lexema do último token lido: '{4}'",
                                 TokenEsperado.ToString(), TokenLido.Linha, TokenLido.Coluna, 
                                 TokenLido.Gramatica, TokenLido.Lexema))
        {

        }

    }
}
