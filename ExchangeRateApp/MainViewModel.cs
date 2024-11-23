using ExchangeRateApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
            ExchangeSource();
        }

        public void ExchangeSource()
        {
            if (ValuteSource != null && ValuteTarget != null)
            {
                amountTarget = Math.Round((AmountSource * ValuteSource.Value / ValuteSource.Nominal) / (ValuteTarget.Value / ValuteTarget.Nominal), 2);
                OnPropertyChanged(nameof(AmountTarget));
            }
        }
        public void ExchangeTarget()
        {
            if (ValuteSource != null && ValuteTarget != null)
            {
                amountSource = Math.Round((AmountTarget * ValuteTarget.Value / ValuteTarget.Nominal) / (ValuteSource.Value / ValuteSource.Nominal), 2);
                OnPropertyChanged(nameof(AmountSource));
            }
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
                    LabelDate = $"Курс валют на {Date.Day}.{Date.Month}.{Date.Year}";
                    ExchangeSource();
                    OnPropertyChanged();
                }
            }
        }

        string labelDate;
        public string LabelDate
        {
            get => labelDate;
            set
            {
                if (value != labelDate) 
                {
                    labelDate = value;
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
                    ExchangeSource();
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
                    ExchangeSource();
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
                    ExchangeSource();
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
                    ExchangeTarget();
                }
            }
        }

        


        public MainViewModel()
        {
            Date = DateTime.Now;
            exchangeRateApiService = new();
            Valutes = new();

            UpdateValCursCommand = new Command(
                () => UpdateValCurs()
            );

            ExchangeCommand = new Command(
                execute: () =>
                {
                    ExchangeSource();
                },
                canExecute: () => 
                { 
                    return ValuteSource != null && ValuteTarget != null; 
                }
            ); 

        }

    }
}
