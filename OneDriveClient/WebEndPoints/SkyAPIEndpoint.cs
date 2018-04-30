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
    public class SkyAPIEndpoint : EndpointBase
    {
        private static string ENDPOINT_SOAK3 = @"soak3.test.skyapi.live.net";
        private static string ENDPOINT_SOAK2 = @"soak2.test.skyapi.live.net";


        public SkyAPIEndpoint(EnvironmentType envType) : base(envType)
        {

        }


        public async Task<Item> GetItems(UserIdentity user, Document doc, string authKey)
        {
            string requestBase = string.Format("https://{0}/API/2/GetItems", ENDPOINT_SOAK3);
            string requestUrl = string.Format("{0}?authKey={1}&d=1&ft=All&ps=100&id={2}&q=&tagFilter=",
                requestBase, authKey, doc.ResourceId);
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Application", "StorageTest");
            request.Headers.Add("AppId", "1614633998");
            request.Headers.Add("Accept", "application/json");
            
            cookieContainer.SetCookies(new Uri(requestBase), user.GetSkyAPICookie());

            var response = await webClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                GetItemsResponse responseObject = JsonConvert.DeserializeObject<GetItemsResponse>(responseString);

                Item item = new ObjectModel.Item();
                item.ItemType = responseObject.items.FirstOrDefault().itemType;
                item.Name = responseObject.items.FirstOrDefault().name;
                return item;
            }

            return null;
        }

    }

    public class GetItemsResponse
    {
        public class GetItemsResponse_Item
        {
            public string commands;
            public string id;
            public int itemType;
            public string name;
            public string ownerName;
            public string parentId;

        };

        public class GetItemsResponse_ItemUrls
        {
            public string viewInBrowser;
        }

        public List<GetItemsResponse_Item> items;
    }
}
