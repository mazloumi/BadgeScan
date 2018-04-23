using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;
using System.Diagnostics;

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
                    case "multiple_matching_tokens_detected":
                        resultCode = AuthCode.Failed;
                        break;
                    default:
                        resultCode = AuthCode.Failed;
                        break;
                }
            }

            catch (Exception ex)
            {
                resultCode = AuthCode.Failed;
                Debug.WriteLine($"{resultCode} - {ex.Message}: {ex.StackTrace}");
            }
            return resultCode;
        }

        public static async Task<Contact> GetContact(string contactid)
        {
            try
            {
                var queryOptions = $"contacts?$select=firstname,lastname,entityimage_url,entityimage&$filter=contactid%20eq%20{contactid}&$expand=parentcustomerid_account($select=name)";
                Debug.WriteLine(queryOptions);
                HttpResponseMessage response = await client.GetAsync(queryOptions);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"{json}");
                var contacts = JsonConvert.DeserializeObject<Contacts>(json);
                return contacts.value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}: {ex.StackTrace}");
                return null;
            }

        }

        public static async Task<IEnumerable<Contact>> GetAllContacts()
        {
            try
            {
                var queryOptions = $"contacts?$select=fullname,employeeid,externaluseridentifier,governmentid&$expand=parentcustomerid_account($select=name)";
                Debug.WriteLine(queryOptions);
                HttpResponseMessage response = await client.GetAsync(queryOptions);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"{json}");
                var contacts = JsonConvert.DeserializeObject<Contacts>(json);
                return contacts.value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}: {ex.StackTrace}");
                return null;
            }
        }
    }
}