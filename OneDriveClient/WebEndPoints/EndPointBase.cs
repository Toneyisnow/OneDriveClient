using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.WebEndPoints
{
    public class EndPointBase
    {

        protected static CookieContainer cookieContainer = null;
        protected static HttpClientHandler clientHandler = null;
        protected static HttpClient webClient = null;


        public EndPointBase()
        {
            cookieContainer = new CookieContainer();
            clientHandler = new HttpClientHandler() { CookieContainer = cookieContainer };
            clientHandler.AllowAutoRedirect = false;


            webClient = new HttpClient(clientHandler);

        }
    }
}
