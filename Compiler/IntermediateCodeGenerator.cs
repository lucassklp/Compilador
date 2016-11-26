using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class IntermediateCodeGenerator
    {
        private int varCount = -1;
        private int lblCount = -1;
        private StringBuilder result = new StringBuilder();

        public string CurrentVariableName
        {
            get
            {
                return string.Format("T{0}", varCount);
            }
        }

        public string CurrentLabelName
        {
            get
            {
                return string.Format("L{0}", lblCount);
            }
        }

        public string GenerateVariableName()
        {
            return string.Format("T{0}", ++varCount);
        }

        

        public string GenerateLabelName()
        {
            return string.Format("L{0}", ++lblCount);
        }



        /// <summary>
        /// Função usada para imprimir uma nova operação no código intermediário
        /// </summary>
        /// <param name="op1">Operador 1</param>
        /// <param name="op">Operação</param>
        /// <param name="op2">Operador 2</param>
        /// <returns>O valor do endereço da operação</returns>
        public string AppendOperation(Symbol op1, Token op, Symbol op2)
        {
            string varName = this.GenerateVariableName();
            this.GenerateCode(string.Format("{0} = {1} {2} {3}", varName, op1.Name, EnumUtils<Token>.GetDescription(op), op2.Name));
            return varName;
        }


        public string AppendConvertTo(Token resultingType, Symbol op1)
        {
            string varName = this.GenerateVariableName();
            this.GenerateCode(string.Format("{0} = ConvertTo{1}({2})", varName, resultingType.ToString(), op1.Name));
            return varName;
        }

        public void AppendConditionalGoto(Symbol exp, string label)
        {
            this.GenerateCode(string.Format("if {0} == 0 goto {1}", exp.Name, label));
        }

        public void AppendInconditionalGoto(string label)
        {
            this.GenerateCode(string.Format("goto {0}", label));
        }

        public void AppendLabel(string label)
        {
            this.GenerateCode(string.Format("{0}:", label));
        }

        private void GenerateCode(string text)
        {
            Console.WriteLine(text);
            result.AppendLine(text);
        }

        public void Print()
        {
            Console.WriteLine(result);
        }

        internal void AppendAttribuition(Symbol variable, Symbol exp)
        {
            this.GenerateCode(string.Format("{0} = {1}", variable.Name, exp.Name));
        }
    }
}
