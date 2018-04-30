using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.ObjectModel
{
    public class OneDriveAccount : UserIdentity
    {
        public string EmailAddress;

        public string Password;

        public string Puid;

        public string Cid;

        public string WLID;

        public string BearerToken;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetVroomAuthentication()
        {
            if (string.IsNullOrEmpty(BearerToken))
            {
                throw new InvalidOperationException("No BearerToken found for Authentication.");
            }

            return "Bearer " + BearerToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetSkyAPICookie()
        {
            if (string.IsNullOrEmpty(BearerToken))
            {
                throw new InvalidOperationException("No BearerToken found for Authentication.");
            }

            return "BearerAuth=" + BearerToken;
        }
    }
}
