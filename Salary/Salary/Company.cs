using System;
using System.Text;

namespace Salary
{
    // Класс "Компания" используется для регистрации компании и создания зарплатной таблицы компании.
    // Содержит поля: 
    // (1) Название компании
    // (2) Размер тарифной ставки по взносам в Белгосстрах (обязательное страхование от несчастных случаев на производстве.
    // Для гос компаний составляет 0.1%, для частных компаний - 0.6%, при этом иногда есть льготные ставки и, наоборот, повышающие коэффициенты, поэтому 
    // программой предусмотрена возможность ввода с клавиатуры иной ставки, если она не стандартна.
    // (3) Список объектов класса "Корпоративные налоги", начисляемых сверху на зарплату (то, что компания должна заплатить за каждого работника)
    // (4) Список объектов класса "Работники", хранящий информацию о работниках компании
    class Company
    {
        static Random rand = new Random();
        private string _companyName;
        private double _bgsRate;
        private List<CorporateTaxes> _corpTax;
        private List<Person> _employees;

        // Конструктор по умолчанию, запрашивает данные о компании и создает компанию
        public Company()
        {
            Console.Write("Enter Company/Agency name:");
            CompanyName = Console.ReadLine();

            Console.Write("\nChoose Belgosstrakh tariff rate:\n" +
            "(1) state company = 0.1%\n" +
            "(2) private company = 0.6%\n" +
            "(3) other rate\n");

            int choose = Convert.ToInt32(Console.ReadLine());

            switch (choose)
            {
                case 1:
                    BgsRate = 0.001;
                    break;
                case 2:
                    BgsRate = 0.006;
                    break;
                case 3:
                    BgsRate = Convert.ToDouble(Console.ReadLine()) / 100;
                    break;
            }

            Employees = new List<Person>();
            CorpTax = new List<CorporateTaxes>();

        }

        // Конструктор, создающий компанию с рандомными параметрами, использовался для тестирования, чтобы сэкономить время тестировщика
        public Company(bool random)
        {
            CompanyName = PublicMethods.GenName();

            int randbgs = rand.Next(0, 2);
            if (randbgs == 0)
            {
                BgsRate = 0.001;
            }
            else
            {
                BgsRate = 0.006;
            }

            Employees = new List<Person>();
            CorpTax = new List<CorporateTaxes>();
        }


    // Метод, позволяющий добавить работника в список работников компании

        public virtual void addEmployee()
        {
            Person newP = new Person();
            Employees.Add(newP);
            CorpTax.Add(new CorporateTaxes(this, newP));
        }

        // Метод, позволяющий добавить строку со всеми подсчетами общих сумм в конце зарплатной таблицы (TOTAL)
        public virtual void addEmployee(string empty)
        {
            Person newP = new Person("empty");
            Employees.Add(newP);
            CorpTax.Add(new CorporateTaxes(this, newP));
        }
        // Метод, позволяющий удалить работника из списка работников компании
        public virtual void deleteEmployee(int index)
        {
            Employees.Remove(Employees[index]);
            CorpTax.Remove(CorpTax[index]);
        }

        // Метод, позволяющий добавить рандомного работника в список работников компании (использовался только при тестировании)
        public virtual void addRandEmployees(int qty)
        {

            for (int i = 0; i < qty; i++)
            {
                Person newP = new Person(true);
                Employees.Add(newP);
                CorpTax.Add(new CorporateTaxes(this, newP));
            }
        }
        public string CompanyName { get { return _companyName; } set { _companyName = value; } }
        public double BgsRate { get { return _bgsRate; } set { _bgsRate = value; } }
        public List<CorporateTaxes> CorpTax { get { return _corpTax; } set { _corpTax = value; } }
        public List<Person> Employees { get { return _employees; } set { _employees = value; } }

        // Метод, суммирующий все значения в столбце зарплатной таблицы (для нижней строки таблицы - "TOTAL")
        public Person clacTotal(Company c, out CorporateTaxes corp)
        {
            Person total = new Person("empty");
            corp = new CorporateTaxes();
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

            return total;
        }
        // Метод, сохраняющий количество и список работников компании в текстовый файл, чтобы не вносить всех работников каждый месяц заново
        public void savePersonsList()
        {
            string pathString = Path.Combine("../../../", $"{CompanyName}");

            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }
            string path = $"{pathString}/{CompanyName} Persons List.txt";


            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    sw.WriteLine(Employees.Count);
                    for (int i = 0; i < this.Employees.Count; i++)
                    {
                        sw.WriteLine(this.Employees[i]);
                    }
                }
            }
        }

        // Метод, считывающий из файла список работников компании, если в зарплатную таблицу этой компании когда-либо уже вносились работники
        public void getPersonList(string companyName)
        {
            string pathString = Path.Combine("../../../", $"{companyName}");
            if (Directory.Exists(pathString))
            {
                string path = $"{pathString}/{companyName} Persons List.txt";
                if (File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
                        {

                            int qty = Convert.ToInt32(sr.ReadLine());

                            for (int i = 0; i < qty; i++)
                            {
                                this.addEmployee("empty");
                                this.Employees[i].Fullname = sr.ReadLine();
                                this.Employees[i].ChildrenQty = Convert.ToInt32(sr.ReadLine());
                                this.Employees[i].FixPart = Convert.ToDouble(sr.ReadLine());
                                this.Employees[i].FloatingPart = Convert.ToDouble(sr.ReadLine());
                                this.Employees[i].BasicSalary = Convert.ToDouble(sr.ReadLine());
                            }
                        }
                    }
                }
            }
        }

        // Метод, сохраняющий зарплатную таблицу в текстовый файл
        public void saveSalaryTable()
        {
            string pathString = Path.Combine("../../../", $"{CompanyName}");
            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }
            string path = $"{pathString}/{CompanyName} Salary Table {StaticValues.Today:MMMM yyyy}.txt";


            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    sw.WriteLine(this);
                }
            }
        }

        // Метод, сохраняющий зарплатные листки для всех работников компании, внесенных в таблицу
        public void savePaySlipsCompany()
        {
            for (int i = 0; i < Employees.Count; i++)
            {
                string str = Employees[i].formPaySlip();
               Employees[i].savePaySlip(CompanyName, str);
            }
        }

        // Метод, формирующий одну строку на одного работника в зарплатной таблице

        public string formSalaryString (Person p, CorporateTaxes corp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Math.Round(p.Salary.EarnedFixPart, 2) + new string(' ', 10 - Math.Round(p.Salary.EarnedFixPart, 2).ToString().Length) +
                          Math.Round(p.Salary.EarnedfloatingPart, 2) + new string(' ', 10 - Math.Round(p.Salary.EarnedfloatingPart, 2).ToString().Length) +
                          Math.Round(p.Salary.EarnedBasicSalary, 2) + new string(' ', 11 - Math.Round(p.Salary.EarnedBasicSalary, 2).ToString().Length) +
                          Math.Round(p.Salary.OvertimePayment, 2) + new string(' ', 10 - Math.Round(p.Salary.OvertimePayment, 2).ToString().Length) +
                          Math.Round(p.Salary.VacationPay, 2) + new string(' ', 10 - Math.Round(p.Salary.VacationPay, 2).ToString().Length) +
                          Math.Round(p.Salary.SocialBenefits, 2) + new string(' ', 6 - Math.Round(p.Salary.SocialBenefits, 2).ToString().Length) +
                          Math.Round(p.Salary.SickPay, 2) + new string(' ', 10 - Math.Round(p.Salary.SickPay, 2).ToString().Length) +
                          Math.Round(p.Salary.Deduction, 2) + new string(' ', 8 - Math.Round(p.Salary.Deduction, 2).ToString().Length) +
                          Math.Round(p.Salary.SalaryGross, 2) + new string(' ', 12 - Math.Round(p.Salary.SalaryGross, 2).ToString().Length) +
                          Math.Round(p.PersonalTax.PIT, 2) + new string(' ', 8 - Math.Round(p.PersonalTax.PIT, 2).ToString().Length) +
                          Math.Round(p.PersonalTax.SocialTax, 2) + new string(' ', 11 - Math.Round(p.PersonalTax.SocialTax, 2).ToString().Length) +
                          Math.Round(p.PersonalTax.NetSalary, 2) + new string(' ', 11 - Math.Round(p.PersonalTax.NetSalary, 2).ToString().Length) +
                          Math.Round(corp.SocialTax, 2) + new string(' ', 12 - Math.Round(corp.SocialTax, 2).ToString().Length) +
                          Math.Round(corp.BGS, 2) + new string(' ', 7 - Math.Round(corp.BGS, 2).ToString().Length) +
                          Math.Round(corp.GrandTotal, 2) + new string(' ', 10 - Math.Round(corp.GrandTotal, 2).ToString().Length) + "\n\n");


            return sb.ToString();
        }

        // Метод, выводящий зарплатную таблицу в консоль

        public override string ToString()
        {
            string name;
            CorporateTaxes corp = new CorporateTaxes();
            Person total = clacTotal(this, out corp);

           StringBuilder sb = new StringBuilder();
            sb.Append("----------------------------------------------------------------------------------------" +
                "--------------------------------------------------------------------------------------\n");
            sb.Append($"\t\t\t\t\t\t\t\t\tSalary Table for {StaticValues.Today:MMMM yyyy}\n\n");
            sb.Append("Company Name: " + CompanyName + "\n\n");

            sb.Append("ID   " + "Full Name " + new string(' ', 15) + "Fixed     " + "Float     " +
                          "Basic Sal  " + "OT pay    " + "Vacation  " + "Benef " + "Sick pay  " + "Decuct  " + "Sal Gross   " +
                          "PIT     " + "Social 1%  " + "Sal Net    " + "Social 34%  " + "BGS    " + "Total\n\n");

            for (int i = 0; i < Employees.Count; i++)
            {
                if (Employees[i].Fullname.Length > 24)
                {
                    name = Employees[i].Fullname.Substring(0, 23);
                }
                else
                {
                    name = Employees[i].Fullname;
                }
                sb.Append((i+1) + new string(' ', 5 - (i+1).ToString().Length) +
                      name + new string(' ', 25 - name.ToString().Length));
                sb.Append(formSalaryString(Employees[i], CorpTax[i]) + "\n\n");
               
            }
            sb.Append("TOTAL" + new string(' ', 25));
            sb.Append(formSalaryString(total, corp) + "\n\n");

            sb.Append("----------------------------------------------------------------------------------------" +
                   "--------------------------------------------------------------------------------------\n");
            return sb.ToString();
        }
    }


}
