using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using UIKit;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(BadgeScan.iOS.Auth))]
namespace BadgeScan.iOS
{
    public class Auth : IAuth
    {
        public async Task<AuthenticationResult> Authenticate(string authority, string resource, string applicationId, Uri returnUri)
        {
            var authContext = new AuthenticationContext(authority);
            if (authContext.TokenCache.ReadItems().Any()) authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);
            var controller = UIApplication.SharedApplication.KeyWindow.RootViewController;
            var platformParams = new PlatformParameters(controller);
            var authResult = await authContext.AcquireTokenAsync(resource, applicationId, returnUri, platformParams);
            return authResult;
        }
    }
}