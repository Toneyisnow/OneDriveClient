using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.WebEndPoints
{
    public class EndPointBase
    {
        protected static readonly HttpClient webClient = new HttpClient();


    }
}
