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
            scanner.IsScanning = false;
            scanner.IsAnalyzing = false;
            base.OnDisappearing();
        }

        void Toggle(object sender, EventArgs e)
        {
            ScanButton.Text = scanner.IsEnabled ? "Start" : "Stop";
            scanner.IsScanning = !scanner.IsScanning;
            scanner.IsAnalyzing = !scanner.IsAnalyzing;
            scanner.IsEnabled = !scanner.IsEnabled;
            Foto.Source = ImageSource.FromResource("Person.png");
        }

        public async Task Search(string code)
        {
            Name.Text = $"Searching for {code}";

            Image img = new Image();
            try
            {
                var contact = await ServiceProxy.GetContact(code);
                Name.Text = $"{contact.firstname} {contact.lastname}";
                img.Source = ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(contact.entityimage)));
            }
            catch
            {
                Name.Text = "Person not found";
                img.Source = ImageSource.FromResource("Person.png");
            }
            Foto.Source = img.Source;
        }
    }
}
