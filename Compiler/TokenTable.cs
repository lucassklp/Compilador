using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class TokenTable
    {
        public List<Token> ListaTokens { get; set; }

        public TokenTable()
        {
            this.ListaTokens = new List<Token>();
        }



        
    }
}
