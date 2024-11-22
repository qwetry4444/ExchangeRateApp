using ExchangeRateApp.Model;

namespace ExchangeRateApp
{
    public partial class MainPage : ContentPage
    {
        MainViewModel vm = new();
        public MainPage()
        {
            BindingContext = vm;
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await vm.UpdateValCurs();
        }
    }
}
