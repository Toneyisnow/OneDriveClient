using OneDriveClient.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using OneDriveClient.Common;

namespace OneDriveClient.WebEndPoints
{
    public class RampEndpoint : EndpointBase
    {
        private readonly string SOAK3_ENDPOINT = "sudo-soak3.test.storage.live.com";
        private readonly string DEV_ENDPOINT = "sudo.dev.storage.live-int.com";

        public enum RampOption
        {
            OptIn,
            OptOut
        }


        public RampEndpoint(EnvironmentType envType) : base(envType)
        {

        }

        public async Task SetRamp(OneDriveAccount account, string rampName, RampOption option)
        {
            var method = new HttpMethod("PATCH");

            string requestUri = string.Format(@"https://{0}/ramps/{1}/me", SOAK3_ENDPOINT, rampName);

            var jsonObject = new { DesiredRampStatus = (option == RampOption.OptIn ? "Included" : "Excluded"), IsInRamp = (string)null };

            String content = string.Format("{{\"DesiredRampStatus\":\"{0}\", \"IsInRamp\":null}}", (option == RampOption.OptIn ? "Included" : "Excluded"));


            HttpRequestMessage request = new HttpRequestMessage(method, requestUri);
            request.Headers.Add("Application", "StorageTest");
            request.Headers.Add("Authorization", "WLID1.1 t=" + account.WLID);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await webClient.SendAsync(request);
            
            if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return;
            }
            
        }

    }
}
