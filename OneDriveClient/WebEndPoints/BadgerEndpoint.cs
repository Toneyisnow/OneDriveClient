using Newtonsoft.Json;
using OneDriveClient.Common;
using OneDriveClient.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.WebEndPoints
{
    public class BadgerEndpoint : EndpointBase
    {
        private static string TestEnvironmentEndPoint = @"badgertst.cloudapp.net";

        public BadgerEndpoint(EnvironmentType envType) : base(envType)
        {

        }

        public async Task<BadgerAccount> CreateAccount()
        {
            string requestUrl = string.Format("https://{0}/v1.0/token", TestEnvironmentEndPoint);
            string content = "{\"givenName\":null,\"realm\":null,\"appid\":\"294848\"}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await webClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                BadgerResponse responseObject = JsonConvert.DeserializeObject<BadgerResponse>(responseString);

                BadgerAccount account = new BadgerAccount();
                account.BadgerId = responseObject.token;
                return account;
            }

            return null;
        }
    }

    public class BadgerResponse
    {
        public string token;
        public string expiryTimeUtc;
    }

}
