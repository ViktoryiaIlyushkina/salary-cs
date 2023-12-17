using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salary
{
    // Служебные методы
    public class PublicMethods
    {
        static Random rand = new Random();

        // Метод для генерации имени работника/компании/агентства (для тестирования)
        public static string GenName()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 3; i++)
            {
                int length = rand.Next(3, 8);
                sb.Append((char)rand.Next('A', 'Z'));
                for (int j = 0; j < length; j++)
                {
                    sb.Append((char)rand.Next('a', 'z'));
                }
                sb.Append(' ');
            }
            return sb.ToString();
        }
        // Метод для обрезания строки html кода (используется в парсинге)
        public static string getSubString(ref string html, string from, string to, int skip)
        {
            for (int i = 0; i < skip; i++)
            {
                html = html.Remove(0, html.IndexOf(from) + from.Length);
            }
            
            string substring = html[..html.IndexOf(to)];

            return substring;
        }
        // Метод для обрезания строки html кода (используется в парсинге)
        public static void removeUnnecessary(ref string html, string el)
        {
            html = html.Remove(1, html.IndexOf(el) + el.Length-1);
        }

    }
}
