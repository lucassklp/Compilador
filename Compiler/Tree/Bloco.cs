using Compiler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Tree
{
    class Bloco : IVerifier, ICode
    {
        #region Variaveis
        List<DeclaracaoVariavel> declaracoesVariaveis = null;
        List<Comando> comandos = null;
        #endregion

        #region Properties
        public string Code { get; set; }
        #endregion



        
        public Bloco(string block)
        {
            this.Code = block;
            this.declaracoesVariaveis = GetDeclaracoesVariaveis();
            this.comandos = GetCommandos();
            
        }

        private List<DeclaracaoVariavel> GetDeclaracoesVariaveis()
        {
            var listaDeclaracaoVariavel = new List<DeclaracaoVariavel>();
            //    /((int|float|char)\s*([A-Za-z]\s*,?\s*)*;)/g


            return listaDeclaracaoVariavel;
        }


        private List<Comando> GetCommandos()
        {
            var listaComando = new List<Comando>();
            


            return listaComando;

        }

        public bool isValid()
        {
            return true;
        }
    }
}
