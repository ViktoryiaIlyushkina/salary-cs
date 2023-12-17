using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salary
{
    // Класс, который хранит в себе данные о зарплате, которые меняются ежемесячно. Это количество отработанных дней в месяце, 
    //количество часов переработок (и, соответственно, расчет суммы оплаты за переработки;
    //вычеты (например, связанные со штрафами за опоздание или с возмещением материального ущерба); отпускные (если был отпуск);
    // больничные (если работник был на больничном); социальные пособия; и общая начисленная зарплата (до вычета налогов), 
    // которая начисляется работнику в текущем месяце с учетом всех этих переменных.
    // объект данного класса создается в классе Человек при вызове метода расчета зарплаты.


    internal class GrossSalary
    {
        static Random rnd = new Random();
        private int _daysWorked;

        private int _overtimeHours;
        private double _overtimePayment;
        private double _otherPremium;
        private double _deduction;
        private double _vacationPay;
        private double _sickPay;
        private double _socialBenefits;
        private double _salaryGross;
        
        // конструктор с параметром "Человек", передает неизменяющуюся информацию о человеке (работнике), запрашивает изменяющиеся каждый месяц данные  
        // и с учетом этих данных вычисляет общую начисленную зарплату в текущем месяце до вычета налогов.
        public GrossSalary(Person p)
        {
            Console.Write("Enter quantity of days worked:");
            DaysWorked = Convert.ToInt32(Console.ReadLine());
            EarnedFixPart = p.FixPart / StaticValues.WorkingDays * DaysWorked;
            EarnedfloatingPart = p.FloatingPart / StaticValues.WorkingDays * DaysWorked;
            EarnedBasicSalary = (p.FixPart + p.FloatingPart) / StaticValues.WorkingDays * DaysWorked;
            Console.Write("Enter OT hours:");
            OvertimeHours = Convert.ToInt32(Console.ReadLine());
            OvertimePayment = (double)p.BasicSalary / StaticValues.WorkingHours * OvertimeHours * 2;
            Console.Write("Enter other premium amount(if no any = 0):");
            OtherPremium = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter deduction amount (if no any = 0):");
            Deduction = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter vacation pay amount (if no any = 0):");

            if (Deduction > EarnedFixPart)
            {
                Deduction = EarnedFixPart;
            }

            VacationPay = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter sick pay amount (if no any = 0):");
            SickPay = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter social benefits amount (if no any = 0):");
            SocialBenefits = Convert.ToDouble(Console.ReadLine());
            SalaryGross = EarnedBasicSalary + OvertimePayment + OtherPremium - Deduction + VacationPay + SickPay + SocialBenefits;
            Console.WriteLine();
        }

        // конструктор, создающий объект, где все переменные равны нулю. Используется для подсчета TOTAL (общей суммы денег по каждому столбцу зарплатной таблицы)б
        // т.е. общую сумму отпускных, больничных, подоходного налога, ФСЗН и прочих параметров.
        public GrossSalary(string empty)
        {
            DaysWorked = 0;
            EarnedFixPart = 0;
            EarnedfloatingPart = 0;
            EarnedBasicSalary = 0;
            OvertimeHours = 0;
            OvertimePayment = 0;
            OtherPremium = 0;
            Deduction = 0;
            VacationPay = 0;
            SickPay = 0;
            SocialBenefits = 0;
            SalaryGross = 0;
            Console.WriteLine();
        }

        // Рандомный конструктор, который использовался при тестировании программы, он создавал случайные параметры зарплаты, чтобы сэкономить время тестировщику.
        public GrossSalary(Person p, bool rand)
        {
            DaysWorked = rnd.Next(5, StaticValues.WorkingDays);
            EarnedFixPart = p.FixPart / StaticValues.WorkingDays * DaysWorked;
            EarnedfloatingPart = p.FloatingPart / StaticValues.WorkingDays * DaysWorked;
            EarnedBasicSalary = (p.FixPart + p.FloatingPart) / StaticValues.WorkingDays * DaysWorked;
            int randomizer = rnd.Next(0, 5);
            if (randomizer == 0)
            {
                OvertimeHours = rnd.Next(0, 40);
                OtherPremium = rnd.Next(0, 500);
                Deduction = 0;
                VacationPay = rnd.Next(0, 5000);
                SickPay = 0;
                SocialBenefits = rnd.Next(200, 500); 
            }
            else if (randomizer == 1)
            {
                OvertimeHours = 0;
                OtherPremium = 0;
                Deduction = rnd.Next(0, 500);
                VacationPay = 0;
                SickPay = 0;
                SocialBenefits = 0;
            }
            else if (randomizer == 2)
            {
                OvertimeHours = 0;
                OtherPremium = 0;
                Deduction = 0;
                VacationPay = 0;
                SickPay = rnd.Next(0, 1000);
                SocialBenefits = 0;
            }
            else 
            {
                OvertimeHours = 0;
                OtherPremium = 0;
                Deduction = 0;
                VacationPay = 0;
                SickPay = 0;
                SocialBenefits = 0;
            }

            if (Deduction > EarnedFixPart)
            {
                Deduction = EarnedFixPart;
            }

            OvertimePayment = (double)p.BasicSalary / StaticValues.WorkingHours * OvertimeHours * 2;
            SalaryGross = EarnedBasicSalary + OvertimePayment + OtherPremium - Deduction + VacationPay + SickPay + SocialBenefits;
        }

        public int DaysWorked { get { return _daysWorked; } set { _daysWorked = value; } }
        public double EarnedFixPart { get; set; }
        public double EarnedfloatingPart { get; set; }
        public double EarnedBasicSalary { get; set; }


       
        public int OvertimeHours { get { return _overtimeHours; } set { _overtimeHours = value; } }
        public double OvertimePayment { get { return _overtimePayment; } set { _overtimePayment = value; } }
        public double OtherPremium { get { return _otherPremium; } set { _otherPremium = value; } }
        public double Deduction { get { return _deduction; } set { _deduction = value; } }
        public double VacationPay { get { return _vacationPay; } set { _vacationPay = value; } }
        public double SickPay { get { return _sickPay; } set { _sickPay = value; } }
        public double SocialBenefits { get { return _socialBenefits; } set { _socialBenefits = value; } }
        public double SalaryGross { get { return _salaryGross; } set { _salaryGross = value; } }
    }
}
