using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class EnumUtils<T>
    {
        public static List<T> List()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static string GetDescription(T value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return null;
            }
        }

        public static T GetFromDescription(string description)
        {
            List<T> listaEnum = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return listaEnum.Find(x => GetDescription(x) == description);
        }

        public static List<T> GetFromCategory(string category)
        {
            List<T> listaEnum = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            
            List<T> Retorno = listaEnum.FindAll(x => GetCategory(x) == category);

            return Retorno;
        }

        public static string GetCategory(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            CategoryAttribute[] attributes =
                (CategoryAttribute[])fi.GetCustomAttributes(
                    typeof(CategoryAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Category;
            }
            else
            {
                return null;
            }
        }

    }
}
