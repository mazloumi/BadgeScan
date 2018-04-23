using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BadgeScan.ViewModels;
using Xamarin.Forms;
using Xfx;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace BadgeScan
{
    public partial class ScanPage : ContentPage
    {
        private string SearchPhrase = string.Empty;

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
            var vm = BindingContext as ScanPageViewModel;
            if (vm != null && vm.Lookup.Count > 0)
            {

                Name.Text = $"Searching for {code}";
                Foto.Source = ImageSource.FromResource("Person.png");

                Image img = new Image();
                try
                {
                    Foto.IsVisible = false;
                    SearchLoop.IsVisible = true;
                    var contact = await ServiceProxy.GetContact(vm.Lookup[code]);
                    Name.Text = $"{contact.firstname} {contact.lastname}";
                    Account.Text = (contact.parentcustomerid_account != null) ? $"{contact.parentcustomerid_account.name}" : "";
                    img.Source = ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(contact.entityimage)));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"FindContact Error: {ex.Message}: {ex.StackTrace}");
                    Foto.IsVisible = true;
                    SearchLoop.IsVisible = false;
                    Name.Text = "Person not found";
                    Account.Text = string.Empty;
                    img.Source = ImageSource.FromResource("Person.png");
                }
                Foto.IsVisible = true;
                SearchLoop.IsVisible = false;
                Foto.Source = img.Source;
            }
            else
            {
                await Navigation.PopModalAsync();
            }
        }

        private void OnFocused(object sender, FocusEventArgs e)
        {
            var text = SearchPhrase;
            OnClear(null, null);
            SearchField.Text = text;
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            //Debug.WriteLine("Email OnUnfocused");
        }

        private async void ItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var vm = BindingContext as ScanPageViewModel;
            if (vm != null)
            {
                var code = $"{args.SelectedItem}".Trim();
                await FindContact(code);
            }
        }

        void OnClear(object sender, System.EventArgs e)
        {
            SearchField.Text = string.Empty;
            Account.Text = string.Empty;
            Name.Text = string.Empty;
            Foto.Source = ImageSource.FromResource("Person.png");
            SearchPhrase = string.Empty;
        }

        void OnTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SearchPhrase = SearchField.Text;
        }
    }
}
