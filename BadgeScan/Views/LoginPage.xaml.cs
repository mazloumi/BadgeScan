using System;
using System.Threading.Tasks;
using Lottie.Forms;
using Xamarin.Forms;

namespace BadgeScan
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Hostname.Text = Settings.Resource;
            ApplicationId.Text = Settings.ApplicationId;
            Attribute.SelectedIndex = Attribute.Items.IndexOf(Settings.SearchAttribute);
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
                await Navigation.PushModalAsync(new ScanPage());
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
