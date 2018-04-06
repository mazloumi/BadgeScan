using System;
using System.IO;
using System.Threading.Tasks;
using BadgeScan.ViewModels;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace BadgeScan
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
            Foto.Source = ImageSource.FromResource("Person.png");
            SearchField.Text = string.Empty;

            if (Settings.UseScanner) {
                SearchField.IsEnabled = false;
                ScannerField.IsVisible = true;
                SearchField.Placeholder = "Code appears here";
                Scanner.IsScanning = true;
                Scanner.IsAnalyzing = true;
            } else {
                SearchField.IsEnabled = true;
                ScannerField.IsVisible = false;
                SearchField.Placeholder = "Enter search phase here";
                SearchField.Keyboard = (Settings.Keyboard == "Numeric") ? Keyboard.Numeric : Keyboard.Text;
                Scanner.IsScanning = false;
                Scanner.IsAnalyzing = false;
            }
        }

        void OnScanResult(Result result)
        {
            Scanner = new ZXingScannerView
            {
                IsEnabled = true,
                IsAnalyzing = true,
                IsScanning = true
            };
            Scanner.OnScanResult += OnScanResult;
            Device.BeginInvokeOnMainThread(() =>
            {
                SearchField.Text = result.Text;
                Task.FromResult(FindContact(SearchField.Text));
            });
        }

        void Search(object sender, EventArgs e)
        {
            if (SearchField.Text != string.Empty)
            {
                Name.Text = string.Empty;
                Foto.Source = ImageSource.FromResource("Person.png");
                var code = SearchField.Text.Trim();
                SearchField.Text = string.Empty;
                Task.FromResult(FindContact(code));
            }
        }

        async void Back(object sender, EventArgs e)
        {
            Scanner.IsEnabled = false;
            Scanner.IsScanning = false;
            Scanner.IsAnalyzing = false;
            await Navigation.PopModalAsync();
        }

        public async Task FindContact(string code)
        {
            Name.Text = $"Searching for {code}";
            Foto.Source = ImageSource.FromResource("Person.png");

            Image img = new Image();
            try
            {
                Foto.IsVisible = false;
                SearchLoop.IsVisible = true;
                var contact = await ServiceProxy.GetContact(code);
                Name.Text = $"{contact.firstname} {contact.lastname}";
                img.Source = ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(contact.entityimage)));
            }
            catch
            {
                Foto.IsVisible = true;
                SearchLoop.IsVisible = false;
                Name.Text = "Person not found";
                img.Source = ImageSource.FromResource("Person.png");
            }
            Foto.IsVisible = true;
            SearchLoop.IsVisible = false;
            Foto.Source = img.Source;
        }

        private void OnFocused(object sender, FocusEventArgs e)
        {
            //Debug.WriteLine("Email Focused");
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            //Debug.WriteLine("Email OnUnfocused");
        }

        private async void ItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var code = $"{args.SelectedItem}".Trim();
            await FindContact(code);
        }
    }
}
