using System;
using Xamarin.Forms;

namespace BadgeScan
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public async void Button_Pressed(object sender, EventArgs e)
        {
            LoginButton.IsEnabled = false;
            LoginButton.Text = "Hold On";
            await ServiceProxy.Authenticate();
            await Navigation.PushModalAsync(new ScanPage());
        }
    }
}
