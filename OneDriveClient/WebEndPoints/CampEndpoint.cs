using Newtonsoft.Json;
using OneDriveClient.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.WebEndPoints
{
    public class CampEndpoint : EndPointBase
    {
        public async Task<OneDriveAccount> CheckoutAccount()
        {
            // Send Web Request

            var values = new Dictionary<string, string>
            {
               { "Environment", "SOAK3" },
               { "TenantName", "MSA" },
               { "Template", "SynchronousAccount" },
               { "AccountTimeoutSeconds", "1020" },
               { "AllowDirty", "false" },
               { "IncludeAttributes", "false" },
               { "Comment", "Test" }
            };

            var content = new FormUrlEncodedContent(values);


            var response = await webClient.PostAsync("http://camp.redmond.corp.microsoft.com/api/Account/Checkout ", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            CampResponse responseObject = JsonConvert.DeserializeObject<CampResponse>(responseString);


            OneDriveAccount account = new OneDriveAccount();

            account.EmailAddress = responseObject.Email;
            account.Password = responseObject.Password;
            account.Puid = responseObject.Puid;
            account.Cid = responseObject.Cid;

            return account;
        }
    }

    public class CampResponse
    {
        public string Attributes;
        public string Email;
        public string Password;
        public string Puid;
        public string Cid;
    }
}
