using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace BadgeScan
{
    public static class ServiceProxy
    {
        private static HttpClient client;
        private static AuthCode resultCode;

        public static async Task<AuthCode> Authenticate()
        {
            try
            {
                var service = DependencyService.Get<IAuth>();
                var auth = await service.Authenticate(Settings.Authority, $"https://{Settings.Resource}", Settings.ApplicationId, new Uri($"https://{Settings.Resource}"));
                resultCode = AuthCode.Successful;
                client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
                client.BaseAddress = new Uri($"https://{Settings.Resource}/api/data/v8.1/");
                client.Timeout = new TimeSpan(0, 2, 0);
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (AdalException adalEx)
            {
                switch (adalEx.ErrorCode)
                {
                    case "authentication_canceled":
                        resultCode = AuthCode.Cancelled;
                        break;
                    case "access_denied":
                        resultCode = AuthCode.Denied;
                        break;
                    default:
                        resultCode = AuthCode.Failed;
                        break;
                }
            }

            catch (Exception ex)
            {
                resultCode = AuthCode.Failed;
                Console.WriteLine($"{resultCode} - {ex.Message}: {ex.StackTrace}");
            }
            return resultCode;
        }

        public static async Task<Contact> GetContact(string code)
        {
            var queryOptions = $"contacts?$select=firstname,lastname,entityimage_url,entityimage&$filter=contains({Settings.SearchAttribute},'{code}')";
            HttpResponseMessage response = await client.GetAsync(queryOptions);
            var json = await response.Content.ReadAsStringAsync();
            var contacts = JsonConvert.DeserializeObject<Contacts>(json);
            return contacts.value.FirstOrDefault();
        }
    }
}
