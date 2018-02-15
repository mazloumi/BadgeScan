using System;
using System.Threading.Tasks;
using Lottie.Forms;
using Xamarin.Forms;

namespace BadgeScan
{
    public partial class LoginPage : ContentPage
    {
        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Settings.UseScanner = UseScannerField.IsToggled;
        }

        public LoginPage()
        {
            InitializeComponent();
            Hostname.Text = Settings.Resource;
            ApplicationId.Text = Settings.ApplicationId;
            Attribute.SelectedIndex = Attribute.Items.IndexOf(Settings.SearchAttribute);
            UseScannerField.IsToggled = Settings.UseScanner;
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
        }

        async void Handle_Login(object sender, System.EventArgs e)
        {
            Toggle();

            var code = await ServiceProxy.Authenticate();
            Result.Text = $"Authentication: {code}";
            if (code == AuthCode.Successful)
            {
                Toggle();
                await Navigation.PushModalAsync(new NavigationPage(new ScanPage()));
            }
            else
            {
                Toggle();
            }

        }

        void Toggle()
        {
            Animation.IsVisible = !Animation.IsVisible;
            Form.IsVisible = !Form.IsVisible;
        }

    }
}
