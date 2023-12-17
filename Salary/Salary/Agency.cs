using System;
using System.Text;

namespace Salary
{
    // Класс "Агентство" - наследник от класса "Компания", который имеет все поля класса "Компания" и дополнительно тариф Агентства (% от зарплаты
    // каждого предоставленного работника) в оплату своих услуг, а также ставка НДС Агенства - 20% или 0%.
    // В классе также создается список объектов класса "Агентские налоги", где рассчитываются суммы оплаты услуг агентства и НДС исходя из
    // определенных здесь ставок.
    // Этот класс используется для регистрации и обработки информации об агентстве и его работниках

    class Agency: Company
    {
        static Random rand = new Random();
        private int _feeRate;
        private int _vatRate;
        List<AgencyTaxes> _agTax;

        // Конструктор по умолчанию (такой же, как у компании, но добавляется ввод тарифа за услуги и ставки НДС при регистрации агентства в программе
        public Agency(): base()
        {
            Console.Write("Enter Agency fee rate:");
            FeeRate = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Agency VAT rate:");
            VatRate = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            AgTax = new List<AgencyTaxes>();
        }

        // Конструктор, создающий рандомное агентство (использовался при тестировании)
        public Agency(bool random): base(random)
        {
       
            FeeRate = rand.Next(0, 10);

            int randvat = rand.Next(0, 2);
            if (randvat == 0) 
            {
                VatRate = 0;
            }
            else
            {
                VatRate = 20;
            }
            AgTax = new List<AgencyTaxes>();
        }
        public int FeeRate { get { return _feeRate; } set { _feeRate = value; } }
        public int VatRate { get { return _vatRate; } set { if (value == 0 || value == 20) _vatRate = value; } }
        public List<AgencyTaxes> AgTax { get { return _agTax; } set { _agTax = value; } }

        // переопределенный для агентства метод, позволяющий добавить работника в список работников агентства (добавляются агентские налоги)
        public override void addEmployee()
        {
            Person newP = new Person();
            Employees.Add(newP);
            CorpTax.Add(new CorporateTaxes(this, newP));
            AgTax.Add(new AgencyTaxes(this, Employees.Count - 1));
        }
        // переопределенный для агентства метод, позволяющий добавить строку со всеми подсчетами общих сумм в конце зарплатной таблицы (TOTAL)
        public override void addEmployee(string empty)
        {
            Person newP = new Person("empty");
            Employees.Add(newP);
            CorpTax.Add(new CorporateTaxes(this, newP));
            AgTax.Add(new AgencyTaxes(this, Employees.Count - 1));
        }
        // переопределенный для агентства метод, позволяющий удалить работника из списка работников агентства
        public override void deleteEmployee(int index)
        {
            Employees.Remove(Employees[index]);
            CorpTax.Remove(CorpTax[index]);
            AgTax.Remove(AgTax[index]);
        }
        // переопределенный для агентства метод, позволяющий добавить рандомного работника в список работников агентства(использовался только при тестировании)
        public override void addRandEmployees(int qty)
        {

            for (int i = 0; i < qty; i++)
            {
                Person newP = new Person(true);
                Employees.Add(newP);
                CorpTax.Add(new CorporateTaxes(this, newP));
                AgTax.Add(new AgencyTaxes(this, this.Employees.Count - 1));
            }
        }

        // Метод, суммирующий все значения в столбце зарплатной таблицы (для нижней строки таблицы - "TOTAL")
        public Person clacTotal(Agency c, out CorporateTaxes corp, out AgencyTaxes ag)
        {
            Person total = new Person("empty");
            corp = new CorporateTaxes();
            ag = new AgencyTaxes();
            for (int i = 0; i < c.Employees.Count; i++)
            {
                total.Salary.EarnedFixPart += c.Employees[i].Salary.EarnedFixPart;
                total.Salary.EarnedfloatingPart += c.Employees[i].Salary.EarnedfloatingPart;
                total.Salary.EarnedBasicSalary += c.Employees[i].Salary.EarnedBasicSalary;
                total.Salary.OvertimePayment += c.Employees[i].Salary.OvertimePayment;
                total.Salary.VacationPay += c.Employees[i].Salary.VacationPay;
                total.Salary.SocialBenefits += c.Employees[i].Salary.SocialBenefits;
                total.Salary.SickPay += c.Employees[i].Salary.SickPay;
                total.Salary.Deduction += c.Employees[i].Salary.Deduction;
                total.Salary.SalaryGross += c.Employees[i].Salary.SalaryGross;
                total.PersonalTax.PIT += c.Employees[i].PersonalTax.PIT;
                total.PersonalTax.SocialTax += c.Employees[i].PersonalTax.SocialTax;
                total.PersonalTax.NetSalary += c.Employees[i].PersonalTax.NetSalary;
            }
            corp = corp.clacTotal(c);
            ag = ag.clacTotal(c);

            return total;
        }
        // Метод, формирующий одну строку на одного работника в зарплатной таблице
        public string formSalaryString(Person p, CorporateTaxes corp, AgencyTaxes ag)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(    Math.Round(p.Salary.EarnedBasicSalary, 2) + new string(' ', 11 - Math.Round(p.Salary.EarnedBasicSalary, 2).ToString().Length) +
                          Math.Round(p.Salary.OvertimePayment, 2) + new string(' ', 8 - Math.Round(p.Salary.OvertimePayment, 2).ToString().Length) +
                          Math.Round(p.Salary.VacationPay, 2) + new string(' ', 10 - Math.Round(p.Salary.VacationPay, 2).ToString().Length) +
                          Math.Round(p.Salary.SocialBenefits, 2) + new string(' ', 6 - Math.Round(p.Salary.SocialBenefits, 2).ToString().Length) +
                          Math.Round(p.Salary.SickPay, 2) + new string(' ', 7 - Math.Round(p.Salary.SickPay, 2).ToString().Length) +
                          Math.Round(p.Salary.Deduction, 2) + new string(' ', 7 - Math.Round(p.Salary.Deduction, 2).ToString().Length) +
                          Math.Round(p.Salary.SalaryGross, 2) + new string(' ', 11 - Math.Round(p.Salary.SalaryGross, 2).ToString().Length) +
                          Math.Round(p.PersonalTax.PIT, 2) + new string(' ', 8 - Math.Round(p.PersonalTax.PIT, 2).ToString().Length) +
                          Math.Round(p.PersonalTax.SocialTax, 2) + new string(' ', 10 - Math.Round(p.PersonalTax.SocialTax, 2).ToString().Length) +
                          Math.Round(p.PersonalTax.NetSalary, 2) + new string(' ', 10 - Math.Round(p.PersonalTax.NetSalary, 2).ToString().Length) +
                          Math.Round(corp.SocialTax, 2) + new string(' ', 11 - Math.Round(corp.SocialTax, 2).ToString().Length) +
                          Math.Round(corp.BGS, 2) + new string(' ', 7 - Math.Round(corp.BGS, 2).ToString().Length) +
                          Math.Round(corp.GrandTotal, 2) + new string(' ', 10 - Math.Round(corp.GrandTotal, 2).ToString().Length) +
                          Math.Round(ag.Fee, 2) + new string(' ', 9 - Math.Round(ag.Fee, 2).ToString().Length) +
                          Math.Round(ag.VAT, 2) + new string(' ', 9 - Math.Round(ag.VAT, 2).ToString().Length) +
                          Math.Round(ag.GrandTotal, 2) + new string(' ', 11 - Math.Round(ag.GrandTotal, 2).ToString().Length) + "\n\n");


            return sb.ToString();
        }

        // Метод, выводящий зарплатную таблицу в консоль
        public override string ToString()
        {
            string name;
            CorporateTaxes corp = new CorporateTaxes();
            AgencyTaxes ag = new AgencyTaxes();
            Person total = clacTotal(this, out corp, out ag);

            StringBuilder sb = new StringBuilder();
            sb.Append("----------------------------------------------------------------------------------------" +
                "--------------------------------------------------------------------------------------\n");
            sb.Append($"\t\t\t\t\t\t\t\t\tSalary Table for {StaticValues.Today:MMMM yyyy}\n\n");
            sb.Append("Agency Name: " + CompanyName + "\n\n");

            sb.Append("ID   " + "Full Name " + new string(' ', 10) + "Basic Sal  " + "OT pay  " + 
                "Vacation  " + "Benef " + "Sick   " + "Decuct " + "Sal Gross  " + "PIT     " + "Social 1% " 
                + "Sal Net   " + "Social 34% " + "BGS    " + "Total     " + "Fee      " + "VAT      " + "Grand Total" +"\n\n");

            for (int i = 0; i < Employees.Count; i++)
            {
                if (Employees[i].Fullname.Length > 19)
                {
                    name = Employees[i].Fullname.Substring(0, 18);
                }
                else
                {
                    name = Employees[i].Fullname;
                }
                sb.Append((i + 1) + new string(' ', 5 - (i + 1).ToString().Length) +
                      name + new string(' ', 20 - name.ToString().Length));
                sb.Append(formSalaryString(Employees[i], CorpTax[i], AgTax[i]) + "\n\n");

            }
            sb.Append("TOTAL" + new string(' ', 20));
            sb.Append(formSalaryString(total, corp, ag) + "\n\n");

            sb.Append("----------------------------------------------------------------------------------------" +
                   "--------------------------------------------------------------------------------------\n");
            return sb.ToString();
        }
    }
}
