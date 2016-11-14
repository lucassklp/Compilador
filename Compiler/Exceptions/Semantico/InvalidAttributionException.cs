using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions.Semantico
{
    public class InvalidAttributionException : Exception
    {
        public InvalidAttributionException(Symbol Token, Symbol expectedToken)
            : base(string.Format("Atribuição inválida: A variável '{0}' do tipo '{1}' não pode receber expressão do tipo '{2}'. Linha: {3}, Coluna: {4}", expectedToken.Identifier, expectedToken.Type.ToString(), Token.ReturnType.ToString(), expectedToken.LexicalToken.Linha, expectedToken.LexicalToken.Coluna))
        {

        }
    }
}
