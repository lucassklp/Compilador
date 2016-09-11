using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Parser
    {
        private Scanner scanner = null;
        private Token NextToken
        {
            get
            {
                return this.scanner.NextToken();
            }
        }
        private Token LookAtNextToken
        {
            get
            {
                return this.scanner.LookAtNextToken();
            }
        }

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            try
            {
                Programa();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        
        #region Checkers
        private bool IsDeclaracaoVariavel()
        {
            return (LookAtNextToken.Gramatica == Gramatica.Int ||
                    LookAtNextToken.Gramatica == Gramatica.Float ||
                    LookAtNextToken.Gramatica == Gramatica.Char);
        }

        private bool IsComando()
        {

            return (LookAtNextToken.Gramatica == Gramatica.Do ||
                   LookAtNextToken.Gramatica == Gramatica.While ||
                   LookAtNextToken.Gramatica == Gramatica.Identificador ||
                   LookAtNextToken.Gramatica == Gramatica.If ||
                   LookAtNextToken.Gramatica == Gramatica.AbreChave);

        }

        private bool HasInlineDeclaracoes()
        {
            return LookAtNextToken.Gramatica == Gramatica.Vírgula;
        }

        #endregion

        #region Produções

        private void Programa()
        {
            //<programa> ::= int main"("")" <bloco>
            if (NextToken.Gramatica == Gramatica.Int)
            {
                if(NextToken.Gramatica == Gramatica.Main)
                {
                    if(NextToken.Gramatica == Gramatica.AbreParenteses)
                    {
                        if(NextToken.Gramatica == Gramatica.FechaParenteses)
                        {
                            this.Bloco();
                        }
                    }
                }
            }
        }

        private void Bloco()
        {
            //<bloco> ::= “{“ {<decl_var>}* {<comando>}* “}”
            if (NextToken.Gramatica == Gramatica.AbreChave)
            {
                while (this.IsDeclaracaoVariavel())
                    this.DeclaracaoVariavel();
                
                while(this.IsComando())
                    this.Comando();

                if (NextToken.Gramatica == Gramatica.FechaChave)
                    return;
            }
        }



        private void DeclaracaoVariavel()
        {
            var TipoVariavel = NextToken.Gramatica;

            if(NextToken.Gramatica == Gramatica.Identificador)
            {
                while (HasInlineDeclaracoes())
                {
                    if(NextToken.Gramatica == Gramatica.Vírgula)
                    {
                        if(NextToken.Gramatica == Gramatica.Identificador)
                        {
                            continue;
                        }
                    }
                }
                if (NextToken.Gramatica == Gramatica.PontoVírgula)
                    return;
            }
        }

        private void Comando()
        {
            throw new NotImplementedException();
        }
        



        #endregion

    }
}
