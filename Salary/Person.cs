using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Salary
{
    // Класс Человек используется для регистрации работника в программе при запуске модуля "Работник", а также при создании списка работников каждой
    // компании и агентства. Класс хранит в себе постоянные переменные - фио работника, количество детей (влияет на размер стандартного налогового вычета),
    // фиксированная и премиальная часть заработной платы и их сумма (basic salary - это те цифры, которые прописываются в трудовом договоре)
    // В классе также создается объект класса GrossSalary, который хранит в себе параметры зарплаты, изменяющиеся каждый месяц и PersonalTaxes - 
    // индивидуальные налоги, которые удерживаются из зарплаты работника нанимателем. 
    class Person
    {
        Random rand = new Random();

        private string _fullname;
        private int _childrenQty;
        private double _fixPart;
        private double _floatingPart;
        private double _basicSalary;
        private GrossSalary _salary;
        private PersonalTaxes _personalTax;

        // конструктор по умолчанию, запрашивающий информацию о работнике при его регистрации или внесении в зарплатную таблицу.
        public Person()
        {
            Console.Write("Enter Fullname:");
            Fullname = Console.ReadLine();
            Console.Write("Enter Children quantity:");
            ChildrenQty = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter fix part:");
            FixPart = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter floating Part:");
            FloatingPart = Convert.ToDouble(Console.ReadLine());
            BasicSalary = FixPart + FloatingPart;
            Console.WriteLine();
            Salary = new GrossSalary("empty");
            PersonalTax = new PersonalTaxes("empty");
        }

        // конструктор, создающий объект, где все переменные равны нулю. Используется для подсчета TOTAL (общей суммы денег по каждому столбцу зарплатной таблицы)б
        // т.е. общую сумму отпускных, больничных, подоходного налога, ФСЗН и прочих параметров.
        public Person(string empty)
        {
            Salary = new GrossSalary("empty");
            PersonalTax = new PersonalTaxes("empty");
        }

        // Рандомный конструктор, который использовался при тестировании программы, он создавал случайных людей, чтобы сэкономить время тестировщику.
        public Person(bool random)
        {
            Fullname = PublicMethods.GenName();
            ChildrenQty = rand.Next(0, 4);
            FixPart = rand.Next(500, 8000);
            FloatingPart = rand.Next(500, 2000);
            BasicSalary = FixPart + FloatingPart;
            Salary = new GrossSalary(this, true);
            PersonalTax = new PersonalTaxes(this, true);
        }

        public string Fullname { get { return _fullname; } set { _fullname = value; } }
        public int ChildrenQty { get { return _childrenQty; } set { if (value >= 0 && value < 15) _childrenQty = value; } }
        public double FixPart { get { return _fixPart; } set { _fixPart = value; } }
        public double FloatingPart { get { return _floatingPart; } set { _floatingPart = value; } }
        public double BasicSalary { get { return _basicSalary; } set { _basicSalary = value; } }
        public GrossSalary Salary { get { return _salary; } set { _salary = value; } }
        public PersonalTaxes PersonalTax { get { return _personalTax; } set { _personalTax = value; } }

        // Метод, который запускает процесс подсчета зарплаты работника в текущем месяце.
        // При запуске этого метода создается объект класса GrossSalary, который хранит в себе параметры зарплаты, изменяющиеся каждый месяц 
        // и PersonalTaxes - индивидуальные налоги, которые удерживаются из зарплаты работника нанимателем. 
        public void calcSalary()
        {
            Salary = new GrossSalary(this);
            PersonalTax = new PersonalTaxes(this);
        }

        // Метод, который в табличном формате выводит в одну строку все данные о зарплате одного работника, зарплатная таблица компании и агентства состоит из
        // таких строк на каждого работника
        public void showSalaryString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Full Name " + new string(' ', 25) + "Fixed     " + "Float     " +
                         "Basic Sal  " + "OT pay    " + "Vacation  " + "Benef " + "Sick pay  " + "Decuct  " + "Sal Gross   " +
                         "PIT     " + "Social 1%  " + "Sal Net    \n\n");

            sb.Append(Fullname + new string(' ', 35 - Fullname.ToString().Length));

            sb.Append(Math.Round(Salary.EarnedFixPart, 2) + new string(' ', 10 - Math.Round(Salary.EarnedFixPart, 2).ToString().Length) +
                          Math.Round(Salary.EarnedfloatingPart, 2) + new string(' ', 10 - Math.Round(Salary.EarnedfloatingPart, 2).ToString().Length) +
                          Math.Round(Salary.EarnedBasicSalary, 2) + new string(' ', 11 - Math.Round(Salary.EarnedBasicSalary, 2).ToString().Length) +
                          Math.Round(Salary.OvertimePayment, 2) + new string(' ', 10 - Math.Round(Salary.OvertimePayment, 2).ToString().Length) +
                          Math.Round(Salary.VacationPay, 2) + new string(' ', 10 - Math.Round(Salary.VacationPay, 2).ToString().Length) +
                          Math.Round(Salary.SocialBenefits, 2) + new string(' ', 6 - Math.Round(Salary.SocialBenefits, 2).ToString().Length) +
                          Math.Round(Salary.SickPay, 2) + new string(' ', 10 - Math.Round(Salary.SickPay, 2).ToString().Length) +
                          Math.Round(Salary.Deduction, 2) + new string(' ', 8 - Math.Round(Salary.Deduction, 2).ToString().Length) +
                          Math.Round(Salary.SalaryGross, 2) + new string(' ', 12 - Math.Round(Salary.SalaryGross, 2).ToString().Length) +
                          Math.Round(PersonalTax.PIT, 2) + new string(' ', 8 - Math.Round(PersonalTax.PIT, 2).ToString().Length) +
                          Math.Round(PersonalTax.SocialTax, 2) + new string(' ', 11 - Math.Round(PersonalTax.SocialTax, 2).ToString().Length) +
                          Math.Round(PersonalTax.NetSalary, 2) + new string(' ', 11 - Math.Round(PersonalTax.NetSalary, 2).ToString().Length) +
                          "\n\n");


            Console.WriteLine(sb.ToString());
        }

        // Метод, который формирует расчетный листок работника, используя данные о его зарплате в текущем месяце.
        public string formPaySlip()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("----------------------------------------------------------------------------------------\n");
            sb.Append($"\t\t\tPay slip for {StaticValues.Today:MMMM yyyy}\n\n");
            sb.Append("Name: " + Fullname + "\n\n");
            sb.Append("Type of payment/withholding      " + "Days      " + "Payment amount      " + "Withholding amount\n\n");
            sb.Append($"Payed for working days" + new string(' ', 11) + $"{Salary.DaysWorked}"
                      + new string(' ', 10 - Salary.DaysWorked.ToString().Length) + $"{Math.Round(Salary.EarnedFixPart, 2)}"
                      + new string(' ', 20 - Math.Round(Salary.EarnedFixPart, 2).ToString().Length) + "\n\n");
            sb.Append("Premium" + new string(' ', 36) + Math.Round(Salary.EarnedfloatingPart + Salary.OtherPremium, 2)
                      + new string(' ', 20 - Math.Round(Salary.EarnedfloatingPart + Salary.OtherPremium, 2).ToString().Length) + "\n\n");

            if (Salary.OvertimePayment != 0)
            {
                sb.Append("OT payment" + new string(' ', 33) + Math.Round(Salary.OvertimePayment, 2) + "\n\n");
            }
            if (Salary.VacationPay != 0)
            {
                sb.Append("Vacation pay" + new string(' ', 31) + Math.Round(Salary.VacationPay, 2) + "\n\n");
            }
            if (Salary.SocialBenefits != 0)
            {
                sb.Append("Social Benefits" + new string(' ', 28) + Math.Round(Salary.SocialBenefits, 2) + "\n\n");
            }
            if (Salary.SickPay != 0)
            {
                sb.Append("Sick Pay" + new string(' ', 35) + Math.Round(Salary.SickPay, 2) + "\n\n");
            }
            if (Salary.Deduction != 0)
            {
                sb.Append("Deduction" + new string(' ', 54) + Math.Round(Salary.Deduction, 2) + "\n\n");
            }
            sb.Append("Personal Income Tax" + new string(' ', 44) + $"{Math.Round(PersonalTax.PIT, 2)}\n\n");
            sb.Append("Social Tax" + new string(' ', 53) + $"{Math.Round(PersonalTax.SocialTax, 2)}\n\n");
            sb.Append("Total" + new string(' ', 38)
                      + Math.Round((Salary.SalaryGross + Salary.Deduction), 2)
                      + new string(' ', 20 - Math.Round((Salary.SalaryGross + Salary.Deduction), 2).ToString().Length)
                      + $"{Math.Round((PersonalTax.PIT + PersonalTax.SocialTax + Salary.Deduction), 2)}\n\n");
            sb.Append("Salary Net" + new string(' ', 33) + Math.Round(PersonalTax.NetSalary, 2) + "\n\n");
            sb.Append("----------------------------------------------------------------------------------------\n");

            return sb.ToString();
        }

        // Метод, который сохраняет расчетный листок в папку Pay Slips, которая находится в папке с названием как название компании (агентства)
        // при этом в названии текстового файла (расчетного листка) используются месяц и год, чтобы в следующем месяце данные сохранялись в новый 
        // текстовый файл, а не перезаписывались
        public void savePaySlip(string CompanyName, string str)
        {
            string pathString = Path.Combine($"../../../{CompanyName}", $"Pay Slips");

            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }

            string path = Path.Combine(pathString, $"{Fullname} {StaticValues.Today:MMMM yyyy}.txt");

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    sw.WriteLine(str);
                }
            }
        }

        // Метод, который отображает данные о работнике и его зарплате, которые не меняются каждый месяц.
        // Нужен, чтобы отображать неизменные данные зарегистрированного или внесенного в зарплатную таблицу работника,
        // когда есть необходимость их поменять (если изменилась фамилия, или, например, было повышение зарплаты)
        public void showPerson()
        {
            Console.WriteLine($"Full Name: {Fullname}\nChildren: {ChildrenQty}\nFixed part: {FixPart}\n" +
                $"Floating Part:{FloatingPart}\nBasic Salary: {BasicSalary}\n\n");
        }
        public override string ToString()
        {
            return $"{Fullname}\n{ChildrenQty}\n{FixPart}\n{FloatingPart}\n{BasicSalary}";
        }

    }
    class Employee : Person
    {
        public override string ToString()
        {
            return $"\n\nEmployee " + base.ToString();
        }
        public Employee() : base() { }
        public Employee(bool rand) : base(rand) { }
    }
    class Out_staff : Person
    {
        public override string ToString()
        {
            return $"\n\nOut-staff " + base.ToString();
        }
    }
}