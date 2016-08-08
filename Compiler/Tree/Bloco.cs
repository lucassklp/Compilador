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
        #region Properties
        public string Code { get; set; }
        #endregion

        public Bloco(string block)
        {
            this.Code = block;
        }

        public bool isValid()
        {
            return true;
        }
    }
}
