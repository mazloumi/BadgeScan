using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Xamarin.Forms;

namespace BadgeScan
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new LoginPage();
        }
    }
}
