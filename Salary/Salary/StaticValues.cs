using System;
using System.Text;
using System.Net;

namespace Salary
{
    // Класс "статические переменные" - хранит информацию о текущей дате, средней зп по республике, количестве рабочих дней и часов в текущем месяце
    // все эти переменные парсятся с правового ресурса, т.к. они меняются каждый месяц каждого года и влияют на расчет зп.
    // Количество рабочих дней в месяце важно для расчета той части определенной в контракте зарплаты, которую работник заработал за месяц
    // пропорционально доле отработанных дней от количества рабочих дней в месяце.
    // Количество рабочих часов в месяце важно для расчета стоимости 1 часа переработок.
    // Средняя зп по Республике важна, т.к. ФСЗН не начисляется на доходы, превышающие пятикратный размер этой самой средней зп
    // при запуске программы в эти поля заносятся значения, считанные с правового ресурса, а потом поля этого класса используются при расчете зп
    public class StaticValues
    {
        private static DateTime _today;
        private static double _averageSalary;
        private static int _workingDays;
        private static int _workingHours;

        public StaticValues()
        {
            Today = DateTime.Today;
            AverageSalary = parseAverageSalary();
            WorkingDays = parseDays("\"width:19.7%; padding:0cm ",2);
            WorkingHours = parseDays("\"width:15.16%; padding:0cm ", 3);

        }
        public static DateTime Today { get { return _today; } set { _today = value; } }
        public static double AverageSalary { get { return _averageSalary; } set { _averageSalary = value; } }
        public static int WorkingDays { get { return _workingDays; } set { _workingDays = value; } }
        public static int WorkingHours { get { return _workingHours; } set { _workingHours = value; } }

        // Метод, осуществляющий парсинг средней зп с правового ресурса
        public static double parseAverageSalary()
        {
            Console.OutputEncoding = Encoding.UTF8;
            WebClient client = new();

            string path = "https://etalonline.by/spravochnaya-informatsiya/u01405017/";
            string html = client.DownloadString(path);

            string substring = PublicMethods.getSubString(ref html, "<p class=\"table10\" style=\"TEXT-ALIGN: center;\">", "</p>", 3);
            PublicMethods.removeUnnecessary(ref substring, "&nbsp;");
            double val = Convert.ToDouble(substring);
 
            return val;
        }

        //// Метод, осуществляющий парсинг количества рабочих дней и часов с правового ресурса
        public static int parseDays(string from, int qty)
        {
            Console.OutputEncoding = Encoding.UTF8;
            WebClient client = new();

            string path = "https://etalonline.by/spravochnaya-informatsiya/u01405003/";
            string html = client.DownloadString(path);

            DateTime now = DateTime.Today;
            int mon = Convert.ToInt32(now.ToString("MM"));

            string substring = " ";
            mon = defineIndex(mon);

            substring = PublicMethods.getSubString(ref html, from, "</p>", mon);
            substring = PublicMethods.getSubString(ref html, "class=\"table10\" style=\"TEXT-ALIGN: center;\">", " </p>", 1);
            substring = html[..qty];

            mon = Convert.ToInt32(substring);

            return mon;
        }
        //// Метод, определяющий индекс ячейки таблицы на правовом сайте, из которой нужно извлечь нужную информацию (зависит от текущей даты)
        public static int defineIndex(int index)
        {
            if (index < 4)
            {
                index += 1;
            }
            else if (index >= 4 && index < 7)
            {
                index += 2;
            }
            else if (index >= 7 && index < 10)
            {
                index += 4;
            }
            else if (index >= 10)
            {
                index += 5;
            }
            return index;

        }

    }
}
