using System;
using System.Text;

namespace Salary
{
    // Метод "Агентские налоги" расчитывает суммы оплаты услуг агентства и НДС исходя из определенных в классе "Агентство" ставок.
    // Тут также расчитывается бщее количество денег (включая зарплату и все т.н. "зарплатные налоги" и "агентские налоги") 
    // на выплату зарплаты каждого работника
    // Список объектов данного класса создается в классе "Агентство"
    class AgencyTaxes
    {
        private double _fee;
        private double _VAT;
        private double _grandTotal;

        // Конструктор по умолчанию, создает строку с нулевыми значениями (до расчета зп)
        public AgencyTaxes()
        {
            Fee = 0;
            VAT = 0;
            GrandTotal = 0;
        }
        // конструктор с параметрами, принимающий в качестве параметра объекты "Агентство" (чтобы иметь доступ к списку работников агентства) 
        // и индекс работника, для которого будет выполнен расчет
        public AgencyTaxes(Agency c, int index)
        {
            Fee = c.CorpTax[index].GrandTotal * c.FeeRate / 100;
            VAT = (c.CorpTax[index].GrandTotal + Fee) * (c.VatRate / 100.0);
            GrandTotal = c.CorpTax[index].GrandTotal + Fee + VAT;
        }
        public double Fee { get { return _fee; } set { _fee = value; } }
        public double VAT { get { return _VAT; } set { _VAT = value; } }
        public double GrandTotal { get { return _grandTotal; } set { _grandTotal = value; } }

        // Метод, позволяющий суммировать все значения столбцов агентских налогов для занесения в последнюю строку зарплатной таблицы "TOTAL"
        public AgencyTaxes clacTotal(Agency c)
        {
            AgencyTaxes agTax = new AgencyTaxes();

            for (int i = 0; i < c.Employees.Count; i++)
            {
                agTax.Fee += c.AgTax[i].Fee;
                agTax.VAT += c.AgTax[i].VAT;
                agTax.GrandTotal += c.AgTax[i].GrandTotal;
            }

            return agTax;
        }
        
    }
}
