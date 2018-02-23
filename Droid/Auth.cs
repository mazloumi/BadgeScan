using System;
using Android.App;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(BadgeScan.Droid.Auth))]
namespace BadgeScan.Droid
{
    public class Auth : IAuth
    {
        public async Task<AuthenticationResult> Authenticate(string authority, string resource, string applicationId, Uri returnUri)
        {
            var authContext = new AuthenticationContext(authority);
            if (authContext.TokenCache.ReadItems().Count() > 1) authContext.TokenCache.Clear();
            var platformParams = new PlatformParameters((Activity)Forms.Context);
            var authResult = await authContext.AcquireTokenAsync(resource, applicationId, returnUri, platformParams);
            return authResult;
        }
    }
}