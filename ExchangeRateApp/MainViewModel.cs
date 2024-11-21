using ExchangeRateApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public ValCurs valCurs;

        DateTime date;
        public DateTime Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged();
                }
            }
        }


        Valute valuteSource;
        public Valute ValuteSource
        {
            get => valuteSource;
            set
            {
                if (valuteSource != value)
                {
                    valuteSource = value;
                    OnPropertyChanged();
                }
            }
        }


        Valute valuteTarget;
        public Valute ValuteTarget
        {
            get => valuteTarget;
            set
            {
                if (valuteTarget != value)
                {
                    valuteTarget = value;
                    OnPropertyChanged();
                }
            }
        }

        ExchangeRateApiService exchangeRateApiService;

        public MainViewModel()
        {
            exchangeRateApiService = new();
        }

    }
}
