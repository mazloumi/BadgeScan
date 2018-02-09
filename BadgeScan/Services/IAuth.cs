using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace BadgeScan
{
    public interface IAuth
    {
        Task<AuthenticationResult> Authenticate(string authority, string resource, string applicationId, Uri redirectUri);
    }
}
