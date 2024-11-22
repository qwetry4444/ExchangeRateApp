using ExchangeRateApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        readonly ExchangeRateApiService exchangeRateApiService;
        public Command UpdateValCursCommand { get; private set; }
        public Command ExchangeCommand {  get; private set; }

        public async Task UpdateValCurs()
        {
            ValCurs = await exchangeRateApiService.GetValCurs(date);
            Valutes.Clear();
            foreach (var valute in ValCurs.Valute.Values)
            {
                Valutes.Add(valute);
            }
            Exchange();
        }

        public void Exchange()
        {
            if (ValuteSource != null && ValuteTarget != null)
                AmountTarget = (AmountSource * ValuteSource.Value / ValuteSource.Nominal) / (ValuteTarget.Value / ValuteTarget.Nominal);
        }


        public ValCurs ValCurs { get; private set; }
        public ObservableCollection<Valute> Valutes { get; private set; }

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
                    ExchangeCommand.ChangeCanExecute();
                    Exchange();
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
                    ExchangeCommand.ChangeCanExecute();
                    Exchange();
                }
            }
        }

        double amountSource;
        public double AmountSource
        {
            get => amountSource;
            set
            {
                if (amountSource != value)
                {
                    amountSource = value;
                    OnPropertyChanged();
                    Exchange();
                }
            }
        }

        double amountTarget;
        public double AmountTarget
        {
            get => amountTarget;
            set
            {
                if (amountTarget != value)
                {
                    amountTarget = value;
                    OnPropertyChanged();
                    Exchange();
                }
            }
        }


        public MainViewModel()
        {
            date = DateTime.Now;
            exchangeRateApiService = new();
            Valutes = new();

            UpdateValCursCommand = new Command(
                () => UpdateValCurs()
            );

            ExchangeCommand = new Command(
                execute: () =>
                {
                    Exchange();
                },
                canExecute: () => 
                { 
                    return ValuteSource != null && ValuteTarget != null; 
                }
            ); 

        }

    }
}
