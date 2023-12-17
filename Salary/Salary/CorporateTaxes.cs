using System;
using System.Text;

namespace Salary
{
    // Класс "Корпоративные налоги", начисляемые сверху на зарплату(то, что компания должна заплатить за каждого работника).
    // Это ФСЗН 34%, страховые взносы "Белгосстрах" (обязательное страхование от несчастных случаев на производстве,
    // и общее количество денег (включая зарплату и все т.н. "зарплатные налоги") на выплату зарплаты каждого работника
    // Список объектов этого класса создается внутри класса "Компания", а также наследуется классом "Агентство" от класса "Компания"
    class CorporateTaxes
    {
        private double _socialTax;
        private double _BGS;
        private double _grandTotal;

        // Конструктор по умолчанию, создает строку с нулевыми значениями (до расчета зп)
        public CorporateTaxes()
        {
            SocialTax = 0;
            BGS = 0;
            GrandTotal = 0;
        }

        // конструктор с параметрами, принимающий в качестве параметра объекты "Компания" (чтобы узнать страховой тариф компании для расчета взносов 
        // "Белгосстрах") и "Человек" (чтобы рассчитать ФСЗН в зависимости от полей "Человека")
        public CorporateTaxes(Company c, Person p)
        {
            if (p.Salary.SalaryGross - p.Salary.SocialBenefits - p.PersonalTax.SocialDeduct - p.Salary.SickPay > StaticValues.AverageSalary * 5)
            {
                SocialTax = (StaticValues.AverageSalary * 5) * 0.34;
            }
            else
            {
                SocialTax = (p.Salary.SalaryGross - p.Salary.SocialBenefits - p.PersonalTax.SocialDeduct - p.Salary.SickPay) * 0.34;
            }
            if (SocialTax < 0)
            {
                SocialTax = 0;
            }

            BGS = p.Salary.SalaryGross * c.BgsRate;

            GrandTotal = p.Salary.SalaryGross + SocialTax + BGS;
        }

        public double SocialTax { get { return _socialTax; } set { _socialTax = value; } }
        public double BGS { get { return _BGS; } set { _BGS = value; } }
        public double GrandTotal { get { return _grandTotal; } set { _grandTotal = value; } }

        // Метод, позволяющий суммировать все значения столбцов корпоративных налогов для занесения в последнюю строку зарплатной таблицы "TOTAL"
        public CorporateTaxes clacTotal(Company c)
        {
            CorporateTaxes corp = new CorporateTaxes();

                for (int i = 0; i < c.Employees.Count; i++)
                {
                    corp.SocialTax += c.CorpTax[i].SocialTax;
                    corp.BGS += c.CorpTax[i].BGS;
                    corp.GrandTotal += c.CorpTax[i].GrandTotal;
                }

            return corp;
        }
    }
}
