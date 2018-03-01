using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(BadgeScan.Droid.Auth))]
namespace BadgeScan.Droid
{
    public class Auth : IAuth
    {
        public async Task<AuthenticationResult> Authenticate(string authority, string resource, string applicationId, Uri returnUri)
        {
            var authContext = new AuthenticationContext(authority, true, null);
            var platformParams = new PlatformParameters(CrossCurrentActivity.Current.Activity);
            var authResult = await authContext.AcquireTokenAsync(resource, applicationId, returnUri, platformParams);
            return authResult;
        }
    }
}