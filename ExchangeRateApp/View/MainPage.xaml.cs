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

        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text == "")
            {
                entry.Text = "0";
            }
            else if (!double.TryParse(entry.Text, out _))
            {
                entry.Text = e.OldTextValue;
            } 
        }
    }
}
