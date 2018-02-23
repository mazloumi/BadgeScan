using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Xamarin.Forms;

namespace BadgeScan
{
    public partial class App : Application
    {
        public static AuthCode authCode = AuthCode.Failed;

        public App()
        {
            InitializeComponent();
            if (authCode == AuthCode.Successful)
                MainPage = new NavigationPage(new ScanPage());
            else
                MainPage = new NavigationPage(new LoginPage());
        }
    }
}
