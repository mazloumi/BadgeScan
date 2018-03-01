using System;
using System.Threading.Tasks;
using Lottie.Forms;
using Xamarin.Forms;

namespace BadgeScan
{
    public partial class LoginPage : ContentPage
    {
        void Handle_Keyboard(object sender, System.EventArgs e)
        {
            Settings.Keyboard = Keyboard.Items[Keyboard.SelectedIndex];
        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Settings.UseScanner = UseScannerField.IsToggled;
            KeyboardField.IsVisible = !UseScannerField.IsToggled;
        }

        public LoginPage()
        {
            InitializeComponent();
            Hostname.Text = Settings.Resource;
            ApplicationId.Text = Settings.ApplicationId;
            Attribute.SelectedIndex = Attribute.Items.IndexOf(Settings.SearchAttribute);
            Keyboard.SelectedIndex = Keyboard.Items.IndexOf(Settings.Keyboard);
            UseScannerField.IsToggled = Settings.UseScanner;
            KeyboardField.IsVisible = !Settings.UseScanner;
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
            App.authCode = await ServiceProxy.Authenticate();
            Result.Text = $"Authentication: {App.authCode}";
            Toggle();
            if (App.authCode == AuthCode.Successful)
            {
                await Navigation.PushModalAsync(new NavigationPage(new ScanPage()));
            }
        }

        void Toggle()
        {
            Animation.IsVisible = !Animation.IsVisible;
            Form.IsVisible = !Form.IsVisible;
        }

    }
}
