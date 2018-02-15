using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing;

namespace BadgeScan
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
            Foto.Source = ImageSource.FromResource("Person.png");
            SearchField.IsEnabled = !Settings.UseScanner;
            ScannerField.IsVisible = Settings.UseScanner;
        }

        void OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toggle(null, null);

                SearchField.Text = result.Text;
                Task.FromResult(Search(SearchField.Text));
            });
        }

        protected override void OnDisappearing()
        {
            Scanner.IsScanning = false;
            Scanner.IsAnalyzing = false;
            base.OnDisappearing();
        }

        void Toggle(object sender, EventArgs e)
        {
            if (Settings.UseScanner)
            {
                SearchButton.Text = Scanner.IsEnabled ? "Scan" : "Stop";
                Scanner.IsEnabled = !Scanner.IsEnabled;
                Scanner.IsScanning = !Scanner.IsScanning;
                Scanner.IsAnalyzing = !Scanner.IsAnalyzing;
            }
            Foto.Source = ImageSource.FromResource("Person.png");

            if (!Settings.UseScanner && SearchField.Text != string.Empty)
            {
                var code = SearchField.Text.Trim();
                SearchField.Text = string.Empty;
                Task.FromResult(Search(code));
            }
        }

        public async Task Search(string code)
        {
            Name.Text = $"Searching for {code}";

            Image img = new Image();
            try
            {
                Foto.IsVisible = false;
                Animation.IsVisible = true;
                var contact = await ServiceProxy.GetContact(code);
                Name.Text = $"{contact.firstname} {contact.lastname}";
                img.Source = ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(contact.entityimage)));
            }
            catch
            {
                Foto.IsVisible = true;
                Animation.IsVisible = false;
                Name.Text = "Person not found";
                img.Source = ImageSource.FromResource("Person.png");
            }
            Foto.IsVisible = true;
            Animation.IsVisible = false;
            Foto.Source = img.Source;
        }
    }
}
