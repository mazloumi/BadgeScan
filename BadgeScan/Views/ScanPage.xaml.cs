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
            scanner.IsScanning = false;
            scanner.IsAnalyzing = false;

            Device.BeginInvokeOnMainThread(() =>
            {
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

        void Button_Pressed(object sender, EventArgs e)
        {
            scanner.IsScanning = !scanner.IsScanning;
            scanner.IsAnalyzing = !scanner.IsAnalyzing;
            scanner.IsEnabled = !scanner.IsEnabled;
            Name.Text = "...";
            Foto.Source = "https://nimamazloumi.files.wordpress.com/2018/02/person.png?h=200";
            ScanButton.Text = scanner.IsEnabled ? "Start" : "Stop";
        }

        public async Task Search(string code)
        {
            var contact = await ServiceProxy.GetContact(code);
            Name.Text = $"{contact.firstname} {contact.lastname}";
            Foto.Source = $"{Settings.Resource}{contact.entityimage_url}";
        }
    }
}
