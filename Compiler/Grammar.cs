using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public enum Grammar : int
    {

        [Category("IdentifierRules")]
        [Description(@"^([A-Za-z]|_)([A-Za-z]|_|[0-9])*$")]
        Identificador,

        [Category("NumericRules")]
        [Description(@"^[0-9]+$")]
        TypeIntLiteral,

        [Category("NumericRules")]
        [Description(@"^[0-9]*\.[0-9]+$")]
        TypeFloatLiteral,

        [Category("CharRules")]
        [Description(@"^\'([A-Za-z]|[0-9])\'$")]
        TypeCharLiteral,


        [Category("PalavraReservada")]
        [Description("main")]
        Main,

        [Category("PalavraReservada")]
        [Description("int")]
        Int,

        [Category("PalavraReservada")]
        [Description("float")]
        Float,

        [Category("PalavraReservada")]
        [Description("char")]
        Char,

        [Category("PalavraReservada")]
        [Description("if")]
        If,

        [Category("PalavraReservada")]
        [Description("else")]
        Else,

        [Category("PalavraReservada")]
        [Description("while")]
        While,

        [Category("PalavraReservada")]
        [Description("do")]
        Do,

        [Category("Especial")]
        [Description(";")]
        PontoVírgula,

        [Category("Especial")]
        [Description(",")]
        Vírgula,

        [Category("Especial")]
        [Description("{")]
        AbreChave,

        [Category("Especial")]
        [Description("}")]
        FechaChave,

        [Category("Especial")]
        [Description("(")]
        AbreParenteses,

        [Category("Especial")]
        [Description(")")]
        FechaParenteses,

        [Category("OperadorAritmetrico")]
        [Description("+")]
        Soma,

        [Category("OperadorAritmetrico")]
        [Description("-")]
        Subtração,

        [Category("OperadorAritmetrico")]
        [Description(@"/")]
        Divisão,

        [Category("OperadorAritmetrico")]
        [Description("*")]
        Multiplicação,

        [Category("OperadorAritmetrico")]
        [Description("=")]
        Atribuição,

        [Category("Comparador")]
        [Description(">")]
        Maior,

        [Category("Comparador")]
        [Description(">=")]
	    MaiorOuIgual,

        [Category("Comparador")]
        [Description("<")]
        Menor,

        [Category("Comparador")]
        [Description("<=")]
        MenorOuIgual,

        [Category("Comparador")]
        [Description("==")]
        ComparadorIgual,

        [Category("Comparador")]
        [Description("!=")]
        ComparadorDiferente,

        EndOfFile
    }
}
