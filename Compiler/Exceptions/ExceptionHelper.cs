using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions
{
    class ExceptionHelper : Exception
    {
        public static string FormatMessage(string Error, int Linha, int Coluna, string ultimoCaracterLido)
        {
            return string.Format("{0} \nLinha: {1}, Coluna: {2}, Ultimo caractere lido: \'{3}\'.", Error, Linha, Coluna, GetCharacterName(ultimoCaracterLido));
        }


        public static string GetCharacterName(char character)
        {
            string characterName;
            if (character == '\t')
                characterName = @"\t";
            else if (character == '\n')
                characterName = @"\n";
            else if (character == '\r')
                characterName = @"\r";
            else if (character == '\0')
                characterName = @"\0";
            else
                characterName = character.ToString();

            return characterName;
        }


        public static string GetCharacterName(string lexema)
        {
            string result = string.Empty;
            foreach (char element in lexema)
                result += GetCharacterName(element);
            return result;
        }


    }
}
