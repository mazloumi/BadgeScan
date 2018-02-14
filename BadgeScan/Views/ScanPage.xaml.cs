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
        }

        void OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toggle(null, null);
                var res = result.Text;
                Task.FromResult(Search(res));
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
            ScanButton.Text = Scanner.IsEnabled ? "Scan" : "Stop";
            Scanner.IsScanning = !Scanner.IsScanning;
            Scanner.IsAnalyzing = !Scanner.IsAnalyzing;
            Scanner.IsEnabled = !Scanner.IsEnabled;
            Foto.Source = ImageSource.FromResource("Person.png");
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
