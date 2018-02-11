using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using System.IO;

namespace BadgeScan
{
    public static class ServiceProxy
    {
        private static HttpClient client;

        public static async Task Authenticate()
        {
            var service = DependencyService.Get<IAuth>();
            var authResult = await service.Authenticate(Settings.Authority, Settings.Resource, Settings.ApplicationId, new Uri(Settings.Resource));

            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            client.BaseAddress = new Uri(Settings.Resource + "/api/data/v8.1/");
            client.Timeout = new TimeSpan(0, 2, 0);
            client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            client.DefaultRequestHeaders.Add("OData-Version", "4.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
