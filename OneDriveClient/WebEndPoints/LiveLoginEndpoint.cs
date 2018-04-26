using OneDriveClient.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OneDriveClient.WebEndPoints
{
    public class LiveLoginEndpoint : EndPointBase
    {
        public async Task LoginRST(OneDriveAccount account)
        {
            // Send Web Request
            string bodyContent;
            using (StreamReader sr = new StreamReader("RequestTemplates\\LiveLogin.xml"))
            {
                bodyContent = await sr.ReadToEndAsync();
            }

            bodyContent = bodyContent.Replace("@@USER_NAME@@", account.EmailAddress);
            bodyContent = bodyContent.Replace("@@PASSWORD@@", account.Password);

            /// Console.WriteLine("Login Result: " + bodyContent);

            var httpContent = new StringContent(bodyContent, Encoding.UTF8, "application/xml");
            HttpResponseMessage response = await webClient.PostAsync(@"https://login.live.com/RST2.srf", httpContent);

            var responseString = await response.Content.ReadAsStringAsync();
            
            try
            {
                responseString = responseString.Split(new string[] { "<wsse:BinarySecurityToken Id=\"Compact0\">t=" }, StringSplitOptions.None)[1];
                responseString = responseString.Split(new string[] { "</wsse:BinarySecurityToken>" }, StringSplitOptions.None)[0];

                Console.WriteLine("Before:" + responseString);
                responseString = HttpUtility.HtmlDecode(responseString);
                Console.WriteLine("After:" + responseString);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Response string format error:" + ex.ToString());
            }

            account.WLID = responseString;
        }

        public async Task LoginPPSecure(OneDriveAccount account)
        {

            //// await webClient.GetAsync("https://login.live.com/oauth20_authorize.srf?client_id=0000000044092EEC&redirect_uri=http://wave5.runner.com/&scope=onedrive.readwrite&response_type=token");
            

            //// string requestUrl = @"https://login.live.com/ppsecure/post.srf?client_id=0000000044092EEC&redirect_uri=http://wave5.runner.com/&scope=onedrive.readwrite&response_type=token&contextid=8441E6D24BF4A4C3&bk=1524778041&uaid=70df55196ee44be8a98e7bb90ecd93e6&pid=15216";
            string requestUrl = @"https://login.live.com/ppsecure/post.srf?client_id=0000000044092EEC&scope=onedrive.readwrite&response_type=token&contextid=8441E6D24BF4A4C3&bk=1524778041&uaid=70df55196ee44be8a98e7bb90ecd93e6&pid=15216";

            string content =
                string.Format("login={0}&passwd={1}&type=11&LoginOptions=3&NewUser=1&MEST=&PPSX=P&PPFT=DRcYWBag4fMXYHZU%2A%2ACQOTat%21%2AVnmsQ3qG9ts60F%2AILVAxVUGJjx2maa%2Ajx3LgSdTOsLnhakdoeyXh8UEr1kVwba0MNmQiKV4HvqjpHnP6tu0TLHyahR0nZ0QZAdvH%2AcW03uGaaZooYk1YvdaQxUCiOlbm6okONgrDBmEV5l8c9y67d9lP4QBp2%2AM%21kHm5FbU05YsoDh%21EpHkJJy7anzWx4%24&idsbho=1&PwdPad=&sso=&i1=&i2=1&i4=&i12=1",
                    HttpUtility.HtmlEncode(account.EmailAddress),
                    account.Password
                    );
            
            var httpContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "requestUrl");
            request.Content = httpContent;
            
            cookieContainer.SetCookies(new Uri("https://login.live.com/ppsecure/post.srf"), "MSPRequ=lt=1524778041&co=1&id=N");
            cookieContainer.SetCookies(new Uri("https://login.live.com/ppsecure/post.srf"), "MSPOK=$uuid-321a7839-9e1a-4d60-9e13-54b4046bc7e0");

            HttpResponseMessage response = await webClient.PostAsync(requestUrl, httpContent);

            string location = response.Headers.GetValues("Location").FirstOrDefault<string>();

            Console.WriteLine("Location:" + location);

            string accessToken = location.Split(new string[] { "#access_token=" }, StringSplitOptions.None)[1];
            accessToken = accessToken.Split(new string[] { "&token_type=bearer&" }, StringSplitOptions.None).FirstOrDefault<string>();

            Console.WriteLine("Access_Token:" + accessToken);

            account.BearerToken = accessToken;
        }
    }
}
