using ExchangeRateApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public ValCurs ValCurs { get; private set; }
        public ObservableCollection<Valute> Valutes { get; private set; }

        public async Task UpdateValCurs()
        {
            ValCurs = await exchangeRateApiService.GetValCurs(date);
            ValCurs.Valute.Add("RUB", new Valute("0", "0", "RUB", 1, "Российский рубль", 1, 1));
            LabelDate = $"Курс валют на {ValCurs.Date.Day}.{ValCurs.Date.Month}.{ValCurs.Date.Year}";
            Valutes.Clear();
            foreach (var valute in ValCurs.Valute.Values)
            {
                Valutes.Add(valute);
            }

            if (valuteSourceCharCode != null)
            {
                ValuteSource = ValCurs.Valute[valuteSourceCharCode];
            }

            if (valuteTargetCharCode != null)
            {
                ValuteTarget = ValCurs.Valute[valuteTargetCharCode];
            }
            ExchangeSource();
        }

        public void ExchangeSource()
        {
            if (ValuteSource != null && ValuteTarget != null)
            {
                amountTarget = Math.Round((AmountSource * ValuteSource.Value / ValuteSource.Nominal) / (ValuteTarget.Value / ValuteTarget.Nominal), 4);
                OnPropertyChanged(nameof(AmountTarget));
            }
        }
        public void ExchangeTarget()
        {
            if (ValuteSource != null && ValuteTarget != null)
            {
                amountSource = Math.Round((AmountTarget * ValuteTarget.Value / ValuteTarget.Nominal) / (ValuteSource.Value / ValuteSource.Nominal), 4);
                OnPropertyChanged(nameof(AmountSource));
            }
        }

        string valuteSourceCharCode;
        Valute valuteSource;
        public Valute ValuteSource
        {
            get => valuteSource;
            set
            {
                if (valuteSource != value)
                {
                    if (value != null)
                    {
                        valuteSourceCharCode = value.CharCode;
                    }
                    valuteSource = value;
                    OnPropertyChanged();
                    ExchangeCommand.ChangeCanExecute();
                    ExchangeSource();
                }
            }
        }

        string valuteTargetCharCode;
        Valute valuteTarget;
        public Valute ValuteTarget
        {
            get => valuteTarget;
            set
            {
                if (valuteTarget != value)
                {
                    if (value != null)
                    {
                        valuteTargetCharCode = value.CharCode;
                    }
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


        DateTime date;
        public DateTime Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    UpdateValCurs();
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

        public MainViewModel()
        {
            exchangeRateApiService = new();
            
            Date = DateTime.Now;
            Valutes = new();
            UpdateValCurs();

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
