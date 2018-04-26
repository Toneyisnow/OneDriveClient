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
    public class VroomEndpoint : EndPointBase
    {

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
