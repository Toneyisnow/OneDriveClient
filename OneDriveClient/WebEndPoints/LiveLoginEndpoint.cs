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
        public async Task LoginAccount(OneDriveAccount account)
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
    }
}
