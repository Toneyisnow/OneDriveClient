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
    public class VroomEndpoint : EndpointBase
    {
        private static string ENDPOINT_SOAK3 = @"soak3.test.api.onedrive.com";

        public VroomEndpoint(EnvironmentType envType) : base(envType)
        {

        }

        public async Task<Document> CreateFile(OneDriveAccount account, string fileName)
        {
            string requestUrl = "https://soak3.test.api.onedrive.com/v1.0/drive/root/children";
            string content = string.Format("{{ \"name\": \"{0}\", \"@content.sourceUrl\": \"data:application/text,foo\", \"file\": {{ }} }}", 
                                fileName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Application", "StorageTest");
            request.Headers.Add("Authorization", "Bearer " + account.BearerToken);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await webClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                CreateChildrenResponse responseObject = JsonConvert.DeserializeObject<CreateChildrenResponse>(responseString);


                Document document = new Document();
                document.FileName = fileName;
                document.Account = account;
                document.ResourceId = responseObject.id;

                return document;
            }

            return null;
        }

        public async Task<Link> CreateLink(OneDriveAccount account, Document item, LinkType type, string password = "")
        {
            string requestUrl = string.Format("https://soak3.test.api.onedrive.com/v1.0/drive/root:/{0}:/action.createLink", item.FileName);
            string content = string.Format("{{ \"type\": \"{0}\", \"password\": \"{1}\" }}",
                                (type == LinkType.Edit ? "edit" : "view"),
                                password);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Application", "StorageTest");
            request.Headers.Add("Authorization", "Bearer " + account.BearerToken);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await webClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                CreateLinkResponse responseObject = JsonConvert.DeserializeObject<CreateLinkResponse>(responseString);

                Link link = new Link();
                link.Item = item;
                link.ShareId = responseObject.shareId;
                link.Type = type;
                link.WebUrl = responseObject.link.webUrl;

                return link;
            }

            return null;
        }

        public async Task ValidatePermission(UserIdentity account, Link link, string password)
        {
            string requestUrl = string.Format("https://soak3.test.api.onedrive.com/v1.0/shares/{0}/root/action.validatePermission", link.ShareId);
            string content = string.Format("{{ \"password\": \"{0}\" }}", password);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Application", "StorageTest");
            request.Headers.Add("Authorization", account.GetVroomAuthentication());
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await webClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return;
            }

            return;
        }

        public async Task<string> GetDownloadUrl(UserIdentity account, Document document, string authKey)
        {
            string requestUrl = string.Format("https://{0}/v1.0/drives/{1}/items/{2}?select=id%2C%40content.downloadUrl&authkey={3}", 
                                    ENDPOINT_SOAK3, document.DriveId, document.ResourceId, authKey);
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Application", "StorageTest");
            request.Headers.Add("Authorization", account.GetVroomAuthentication());

            var response = await webClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return string.Empty;
            }

            return string.Empty;
             

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateChildrenResponse
    {
        public string createdDateTime;
        public string cTag;
        public string eTag;
        public string id;

    }

    public class CreateLinkResponse
    {
        public class CreateLinkResponse_Link
        {
            public string type;
            public string webUrl;
            public bool hasPassword; 
        };

        public string id;
        public string shareId;
        public CreateLinkResponse_Link link;
    }
}
