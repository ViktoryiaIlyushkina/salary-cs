using System;
using System.Collections.Generic;
using System.Text;

namespace Salary
{
    // Класс, собирающий статистику по ФСЗН и подоходному налогу, уплаченными компаниями и агентствами, для Министерства 

    [Serializable]
    class Tax_Ministry
    {
        private List<double> _socialTax;
        private List<double> _PIT;
        private List<string> _company;


        public Tax_Ministry()
        {
            SocialTax = new List<double>(0);
            PIT = new List<double>(0);
            Company = new List<string>(0);
        }

        public List<double> SocialTax { get { return _socialTax; } set { _socialTax = value; } }
        public List<double> PIT { get { return _PIT; } set { _PIT = value; } }
        public List<string> Company { get { return _company; } set { _company = value; } }

        // Метод, вычисляющий общую сумму ФСЗН и подоходного налога, уплаченных одной компанией или агентством 
        public void addStatistics(Company c)
        {
            double socialCount = 0;
            double pitCount = 0;
            for (int i = 0; i < c.Employees.Count; i++)
            {
                socialCount += c.Employees[i].PersonalTax.SocialTax;
                socialCount += c.CorpTax[i].SocialTax;
                pitCount += c.Employees[i].PersonalTax.PIT;

            }
            Company.Add(c.CompanyName);
            SocialTax.Add(socialCount);
            PIT.Add(pitCount);
        }
        // Метод, выводящий статистику в консоль
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\t\t\t Tax statistics for {StaticValues.Today:MMMM yyyy}" + "\n\n");
            sb.Append("Company name " + new string(' ', 20) + "PIT" + new string(' ', 7) + "Social Tax" + new string(' ', 7) + "\n\n");
            for (int i = 0; i < Company.Count; i++)
            {
                sb.Append(Company[i] + new string(' ', 33 - Company[i].Length));
                sb.Append(Math.Round(PIT[i],2) + new string(' ', 10 - Math.Round(PIT[i], 2).ToString().Length));
                sb.Append(Math.Round(SocialTax[i],2) + new string(' ', 17 - Math.Round(SocialTax[i], 2).ToString().Length) + "\n\n");
            }
            sb.Append("TOTAL: " + new string(' ', 26));
            sb.Append(Math.Round(PIT.Sum(), 2) + new string(' ', 10 - Math.Round(PIT.Sum(), 2).ToString().Length));
            sb.Append (Math.Round(SocialTax.Sum(), 2) + new string(' ', 17 - Math.Round(SocialTax.Sum(), 2).ToString().Length) + "\n\n");


            return sb.ToString();
        }






    }
}
