using Compiler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Compiler.Tree
{
    class Programa : IVerifier, ICode
    {
        #region Variaveis
        private Bloco bloco;
        private Regex regex;
        #endregion

        #region Properties
        public string Code { get; set; }
        #endregion

        public Programa(string program)
        {
            program = program.ToLower();
            this.Code = program;
            regex = new Regex(@"int\smain\s*\(\s*\)");

            bloco = new Bloco(this.GetBloco());
        }

        public bool isValid()
        {
            if (regex.Match(Code).Success)
                return bloco.isValid();
            else
                throw new Exception("");
        }

        private string GetBloco()
        {
            var regex = new Regex(@"int\smain\s*\(\s*\)");
            return regex.Replace(this.Code, string.Empty, 1);
        }

    }
}
