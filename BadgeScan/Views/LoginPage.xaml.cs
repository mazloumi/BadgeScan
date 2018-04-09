using System;
using System.Threading.Tasks;
using Lottie.Forms;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace BadgeScan
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            Hostname.Text = Settings.Resource;
            ApplicationId.Text = Settings.ApplicationId;
            Attribute.SelectedIndex = Attribute.Items.IndexOf(Settings.SearchAttribute);
            Keyboard.SelectedIndex = Keyboard.Items.IndexOf(Settings.Keyboard);
            UseScannerField.IsToggled = Settings.UseScanner;
            KeyboardField.IsVisible = !Settings.UseScanner;
            Settings.Reload = true;
        }

        void Handle_Keyboard(object sender, System.EventArgs e)
        {
            Settings.Keyboard = Keyboard.Items[Keyboard.SelectedIndex];
        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Settings.UseScanner = UseScannerField.IsToggled;
            KeyboardField.IsVisible = !UseScannerField.IsToggled;
        }

        void Handle_Hostname(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            Settings.Resource = e.NewTextValue;
        }

        void Handle_ApplicationId(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            Settings.ApplicationId = e.NewTextValue;
        }

        void Handle_Attribute(object sender, System.EventArgs e)
        {
            Settings.SearchAttribute = Attribute.Items[Attribute.SelectedIndex];
            Settings.Reload = true;
        }

        async void Handle_Login(object sender, System.EventArgs e)
        {
            Form.IsVisible = false;
            ScannerField.IsVisible = false;
            Animation.IsVisible = true;
            App.authCode = await ServiceProxy.Authenticate();
            Result.Text = $"Authentication: {App.authCode}";
            Animation.IsVisible = false;
            if (App.authCode == AuthCode.Successful)
            {
                await Navigation.PushModalAsync(new NavigationPage(new ScanPage()));
            }
        }

        private void Handle_Manual(object sender, System.EventArgs e)
        {
            Form.IsVisible = true;
            ToggleActions();
        }

        private void ToggleActions()
        {
            ActionsField.IsVisible = !ActionsField.IsVisible;
        }

        private void ToggleScanner()
        {
            ToggleActions();
            ScannerField.IsVisible = !ScannerField.IsVisible;
            Scanner.IsScanning = !Scanner.IsScanning;
            Scanner.IsAnalyzing = !Scanner.IsAnalyzing;
            //Scanner.IsEnabled = !Scanner.IsEnabled;
        }

        private void Handle_Scan(object sender, System.EventArgs e)
        {
            ToggleScanner();
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
                Extensions.LoadConfiguration(result.Text);
                InitializeForm();
                ToggleScanner();
            });
        }

        private void Handle_Cancel(object sender, System.EventArgs e)
        {
            ToggleScanner();
        }

        private void Handle_Cancel2(object sender, System.EventArgs e)
        {
            ToggleActions();
            Form.IsVisible = false;
        }
    }
}
