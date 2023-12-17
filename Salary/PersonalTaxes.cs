using System;
using System.Text;


namespace Salary
{
    // Класс, который высчитывает суммы индивидуальных налогов (подоходный, взносы ФСЗН), которые наниматель должен удержать из начисленной зарплаты работника,
    // за минусом этих сумм класс вычисляет размер зарплаты после вычета налогов, которую работник получит на руки. Отвечает за заполнение части таблицы, 
    // куда вносятся данные об индивидуальных налогах и зарплате после вычета налогов (зарплате на руки).
    // Класс содержит такие поля, как (1) стандартный налоговый вычет, зависящий от количества детей (вычисляется автоматически),
    // (2) стандартный налоговый вычет, который применяется, если начисленная зарплата меньше 944 бел. руб. (вычисляется автоматически),
    // (3) иные налоговые вычеты (кроме стандартных), например, на сумму уплаченного льготного кредита и прочие (налоговый кодекс меняется раз в год,
    // все виды вычетов предусмотреть нереально, поэтому пользователь вводит сумму остальных применимых к нему налоговых вычетов вручную.
    // (4) социальные вычеты (мне известно об одном, например, на сумму страховых взносов при заключении договора добровольного страхования (на доп пенсию)),
    // но допускаю, что есть или появятся и другие, поэтому сумма вычета тоже вводится вручную.
    // (5) PIT - подоходный налог 13%, удерживаемый из зарплаты работника, вычисляется автоматически (налоговая база - начисленная зп - уменьшается 
    // на сумму налоговых вычетов и от этой уменьшенной базы рассчитывается 13%.
    // (6) Social Tax - ФСЗН 1%, удерживаемый из зарплаты работника,  аналогично, налоговая база - начисленная зп - уменьшается 
    // на сумму социальеых вычетов и от этой уменьшенной базы рассчитывается 1%.

    class PersonalTaxes
    {
        static Random rnd = new Random();
        private double _childrenTaxDeduct;
        private double _lowSalaryTaxDeduct;
        private double _otherTaxDeduct;
        private double _socialDeduct;
        private double _PIT;
        private double _socialTax;
        private double _netSalary;

        // конструктор, которому в качестве параметра передаются данные о работнике и зарплате в текущем месяце.
        // используется для расчета индивидуальных налогов при подсчете зарплаты каждого работника в таблице.
        public PersonalTaxes(Person p)
        {
            if (p.ChildrenQty == 1)
            {
                СhildrenTaxDeduct = 46;
            }
            else if (p.ChildrenQty > 1)
            {
                СhildrenTaxDeduct = 87* p.ChildrenQty;
            }
            else if (p.ChildrenQty < 1)
            {
                СhildrenTaxDeduct = 0;
            }

            if (p.Salary.SalaryGross < 944)
            {
                LowSalaryTaxDeduct = 156;
            }
            else
            {
                LowSalaryTaxDeduct = 0;
            }
            Console.WriteLine("Enter tax deductions amount (Except standard deductions for children and low salary):");
            OtherTaxDeduct = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter social deductions amount (if any):");
            SocialDeduct = Convert.ToDouble(Console.ReadLine());

            PIT = (p.Salary.SalaryGross - p.Salary.SocialBenefits - СhildrenTaxDeduct - LowSalaryTaxDeduct - OtherTaxDeduct) * 0.13;
            if (PIT < 0)
            {
                PIT = 0;
            }

            if (p.Salary.SalaryGross - p.Salary.SocialBenefits - SocialDeduct - p.Salary.SickPay > StaticValues.AverageSalary * 5)
            {
                SocialTax = (StaticValues.AverageSalary * 5) * 0.01;

            }
            else
            {
                SocialTax = (p.Salary.SalaryGross - p.Salary.SocialBenefits - SocialDeduct - p.Salary.SickPay) * 0.01;
            }
            if (SocialTax < 0)
            {
                SocialTax = 0;
            }
            NetSalary = p.Salary.SalaryGross - PIT - SocialTax;

        }

        // конструктор, создающий объект, где все переменные равны нулю. Используется для подсчета TOTAL (общей суммы денег по каждому столбцу зарплатной таблицы)б
        // т.е. общую сумму  подоходного налога, ФСЗН и прочих параметров.
        public PersonalTaxes(string empty)
        {
            СhildrenTaxDeduct = 0;
            LowSalaryTaxDeduct = 0;
            OtherTaxDeduct = 0;
            SocialDeduct = 0;
            PIT = 0;
            SocialTax = 0;
            NetSalary = 0;

        }

        //  Рандомный конструктор, который использовался при тестировании программы, он создавал случайные налоговые и социальные вычеты,
        // чтобы сэкономить время тестировщику.
        public PersonalTaxes(Person p, bool rand)
        {
            if (p.ChildrenQty == 1)
            {
                СhildrenTaxDeduct = 46;
            }
            else if (p.ChildrenQty > 1)
            {
                СhildrenTaxDeduct = 87 * p.ChildrenQty;
            }
            else if (p.ChildrenQty < 1)
            {
                СhildrenTaxDeduct = 0;
            }

            if (p.Salary.SalaryGross < 944)
            {
                LowSalaryTaxDeduct = 156;
            }
            else
            {
                LowSalaryTaxDeduct = 0;
            }

            int randomizer = rnd.Next(0, 3);

            if (randomizer == 0)
            {
                OtherTaxDeduct = 0;
                SocialDeduct = 0;

            }
            else if (randomizer == 1)
            {
                OtherTaxDeduct = rnd.Next(100,1000);
                SocialDeduct = 0;
            }
            else if (randomizer == 2)
            {
                OtherTaxDeduct = 0;
                SocialDeduct = rnd.Next(100, 1000);
            }
            PIT = (p.Salary.SalaryGross - p.Salary.SocialBenefits - СhildrenTaxDeduct - LowSalaryTaxDeduct - OtherTaxDeduct) * 0.13;
            if (PIT < 0)
            {
                PIT = 0;
            }
            if (p.Salary.SalaryGross > StaticValues.AverageSalary * 5)
            {
                SocialTax = (StaticValues.AverageSalary * 5 - p.Salary.SocialBenefits - SocialDeduct - p.Salary.SickPay) * 0.01;
            }
            else
            {
                SocialTax = (p.Salary.SalaryGross - p.Salary.SocialBenefits - SocialDeduct - p.Salary.SickPay) * 0.01;
            }
            if (SocialTax < 0)
            {
                SocialTax = 0;
            }
            NetSalary = p.Salary.SalaryGross - PIT - SocialTax;
            NetSalary = p.Salary.SalaryGross - PIT - SocialTax;

        }


        public double СhildrenTaxDeduct { get { return _childrenTaxDeduct; } set { _childrenTaxDeduct = value; } }
        public double LowSalaryTaxDeduct { get { return _lowSalaryTaxDeduct; } set { _lowSalaryTaxDeduct = value; } }
        public double OtherTaxDeduct { get { return _otherTaxDeduct; } set { _otherTaxDeduct = value; } }
        public double SocialDeduct { get { return _socialDeduct; } set { _socialDeduct = value; } }
        public double PIT { get { return _PIT; } set { _PIT = value; } }
        public double SocialTax { get { return _socialTax; } set { _socialTax = value; } }
        public double NetSalary { get { return _netSalary; } set { _netSalary = value; } }

    }
}
