using System;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace Salary
{
    
    class Program
    {
        // Метод для добавления работника (компании или агентства)
        static void Add <T>(T entity) where T : Company
        {
         entity.addEmployee();
         Console.WriteLine(entity);
         entity.savePersonsList();
        }
        // Метод для удаления работника (компании или агентства)
        static void Delete <T> (T entity) where T : Company
        {
            Console.WriteLine("Enter ID of Employee to delete: ");
            int index = Convert.ToInt32(Console.ReadLine());
            index--;
            entity.deleteEmployee(index);
            Console.WriteLine(entity);
            entity.savePersonsList();
        }
        // Метод для подсчета зп работника (компании или агентства)
        static void CalcSalary <T>(T entity, out int index0) where T : Company
        {
            Console.WriteLine("Enter ID of Employee to calculate: ");
            index0 = Convert.ToInt32(Console.ReadLine());
            index0--;
            Console.WriteLine($"For {entity.Employees[index0].Fullname}:");
            entity.Employees[index0].calcSalary();
            entity.CorpTax[index0] = new CorporateTaxes(entity, entity.Employees[index0]);
        }
        // Метод для сохранения статистики в бинарном виде после сохранения новой зарплатной таблицы
        public static void saveStat(Tax_Ministry stat)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = $"{StaticValues.Today:MMMM yyyy} statistics.bin";
            if (File.Exists(path))
            {
                using (Stream fStream = File.Open(path, FileMode.Open))
                {
                    bf.Serialize(fStream, stat);
                }
            }
            else
            {
                using (Stream fStream = File.Create(path))
                {
                    bf.Serialize(fStream, stat);
                }
            }
        }
        // Метод для получения статистики из бинарного файла перед его дополнением новой компанией и сохранением обновленной статистики
        public static void getStat(Tax_Ministry stat)
        {
            BinaryFormatter bf = new BinaryFormatter();


            using (Stream fStream = File.OpenRead($"{StaticValues.Today:MMMM yyyy} statistics.bin"))
            {
                stat = (Tax_Ministry)bf.Deserialize(fStream);
                Console.WriteLine(stat);
            }
        }
        // Метод для сохранения зарплатной таблицы и расчетных листкой (компании или агентства)

        static void Save <T> (T entity, Tax_Ministry stat) where T: Company
        {
            entity.saveSalaryTable();
            entity.savePaySlipsCompany();
            stat.addStatistics(entity);
            saveStat(stat);
            Console.WriteLine($"Saved successfully. Check folder \"{entity.CompanyName}\"\n\n");
        }
        // Метод для изменения информации о работнике, внесенном в зарплатную таблицу (компании или агентства)
        static void ChangeInfo <T> (T entity) where T: Company
        {
            Console.WriteLine("Enter ID of Employee to change his information: ");
            ShowInfo<T>(entity, out int index1);
            Console.WriteLine("Change:\n" +
                "1. Name.\n" +
                "2. Children quantity.\n" +
                "3. Salary.\n");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                Console.WriteLine("Enter new name: ");
                entity.Employees[index1].Fullname = Console.ReadLine();
                Console.WriteLine();
            }
            else if (choice == 2)
            {
                Console.WriteLine("Enter children quantity: ");
                entity.Employees[index1].ChildrenQty = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
            }
            else if (choice == 3)
            {
                Console.WriteLine("Enter Fixed Part: ");
                entity.Employees[index1].FixPart = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Floating Part: ");
                entity.Employees[index1].FloatingPart = Convert.ToDouble(Console.ReadLine());
                entity.Employees[index1].BasicSalary = entity.Employees[index1].FixPart + entity.Employees[index1].FloatingPart;
                Console.WriteLine();
            }
        }

        // Метод для отображения информации о работнике, внесенном в зарплатную таблицу (компании или агентства)
        static void ShowInfo <T> (T entity, out int index1) where T: Company
        {
            index1 = Convert.ToInt32(Console.ReadLine());
            index1--;
            entity.Employees[index1].showPerson();
        }

        // Метод вывода меню для модуля компания или агентство
        public static void showMenuCompAg(ref int choice)
        {
            Console.WriteLine("Menu:\n" +
            "1. Add employee.\n" +
            "2. Delete employee.\n" +
            "3. Calculate Salary\n" +
            "4. Save Salary Table\n" +
            "5. Change Employee's information\n" +
            "6. Show Employee's information.\n" +
            "7. Exit\n\n");

            choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
        }
        // Метод вывода меню для модуля работник (человек)
        public static void showMenuPers(ref int choice)
        {
            Console.WriteLine("Menu:\n" +
            "1. Calculate Salary.\n" +
            "2. Form Pay Slip.\n" +
            "3. Save Pay Slip.\n" +
            "4. Exit.\n\n");

            choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
        }
        public static void Main(string[] args)
        {
            StaticValues now = new StaticValues();
            Tax_Ministry stat = new Tax_Ministry();
            bool use = true;
            int choice = 0;


            while (use)
            {
                Console.WriteLine("Program user:\n" +
                    "1. Person\n" +
                    "2. Company\n" +
                    "3. Agency\n"+
                    "4. Tax Ministry (see statistics)\n\n");

                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    // модуль "Человек" (работник)
                    case 1:
                        bool person = true;
                        Console.WriteLine("Let's start with registration!");
                        Console.WriteLine("We need some information about you and your salary.\n");

                        Employee p = new Employee();
                        Console.WriteLine();

                        while (person)
                        {
                            showMenuPers(ref choice);

                            switch (choice)
                            {
                                case 1:
                                    p.calcSalary();
                                    p.showSalaryString();
                                    break;
                                case 2:
                                    Console.WriteLine(p.formPaySlip());
                                    break;
                                case 3:
                                    p.savePaySlip("Other Pay Slips", p.formPaySlip());
                                    Console.WriteLine("Saved successfully. Check folder \"Other Pay Slips\"\n\n");
                                    break;
                                case 4:
                                    person = false;
                                    break;
                            }
                        }
                        break;
                    case 2:
                        // модуль "Компания"
                        bool company = true;
                        Console.WriteLine("Let's start with registration!");
                        Console.WriteLine("We need some information about company.\n");
                        
                        Company comp = new Company();

                        comp.getPersonList(comp.CompanyName);
                        Console.WriteLine(comp);
                        Console.WriteLine();

                        while (company)
                        {
                            showMenuCompAg(ref choice);

                            switch (choice)
                            {
                                case 1:
                                    Add<Company>(comp);
                                    break;
                                case 2:
                                    Delete<Company>(comp);
                                    break;
                                case 3:
                                    int index0;
                                    CalcSalary<Company>(comp, out index0);
                                    Console.WriteLine(comp);
                                    break;
                                case 4:
                                    Save<Company>(comp, stat);
                                    break;
                                case 5:
                                    ChangeInfo<Company>(comp);
                                    break;
                                case 6:
                                    int index1;
                                    Console.WriteLine("Enter ID of Employee to see his information: ");
                                    ShowInfo<Company>(comp, out index1);
                                    break;
                                case 7:
                                    company = false;
                                    break;
                            }
                        }
                        break;
                    case 3:
                        // модуль "Агентство"
                        bool agency = true;
                        Console.WriteLine("Let's start with registration!");
                        Console.WriteLine("We need some information about agency.\n");

                        Agency ag = new Agency();

                        ag.getPersonList(ag.CompanyName);
                        Console.WriteLine(ag);
                        Console.WriteLine();

                        while (agency)
                        {
                            showMenuCompAg(ref choice);

                            switch (choice)
                            {
                                case 1:
                                    Add<Agency>(ag);
                                    break;
                                case 2:
                                    Delete<Agency>(ag);
                                    break;
                                case 3:
                                    int index0;
                                    CalcSalary<Agency>(ag, out index0);
                                    ag.AgTax[index0] = new AgencyTaxes(ag, index0);
                                    Console.WriteLine(ag);
                                    break;
                                case 4:
                                    Save<Agency>(ag, stat);
                                    break;
                                case 5:
                                    ChangeInfo<Agency>(ag);
                                    break;
                                case 6:
                                    int index1;
                                    Console.WriteLine("Enter ID of Employee to see his information: ");
                                    ShowInfo<Agency>(ag, out index1);
                                    break;
                                case 7:
                                    agency = false;
                                    break;
                            }
                        }
                        break;
                    case 4:
                        // модуль "МНС"
                        getStat(stat);
                        break;
                }
            }
        }
    }
}