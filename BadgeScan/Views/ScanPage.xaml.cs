using System;
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
            Foto.Source = "https://nimamazloumi.files.wordpress.com/2018/02/person.png?h=200";
        }

        public async Task Search(string code)
        {
            Name.Text = $"Searching for {code}";
            var contact = await ServiceProxy.GetContact(code);
            Name.Text = $"{contact.firstname} {contact.lastname}";
            Foto.Source = $"{Settings.Resource}{contact.entityimage_url}";
        }
    }
}
