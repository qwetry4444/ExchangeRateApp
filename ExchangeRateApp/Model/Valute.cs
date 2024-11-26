using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateApp.Model
{
    public class Valute
    {
        public Valute(string iD, string numCode, string charCode, int nominal, string name, double value, double previous)
        {
            ID = iD;
            NumCode = numCode;
            CharCode = charCode;
            Nominal = nominal;
            Name = name;
            Value = value;
            Previous = previous;
        }

        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Previous { get; set; }
    }
}
