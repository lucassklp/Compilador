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

        #region Constantes
        private const string regularExpression = @"int\smain\s*\(\s*\)";
        #endregion

        public Programa(string program)
        {
            program = program.ToLower();
            this.Code = program;
            regex = new Regex(regularExpression);

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
            var regex = new Regex(regularExpression);
            return regex.Replace(this.Code, string.Empty, 1);
        }

    }
}
