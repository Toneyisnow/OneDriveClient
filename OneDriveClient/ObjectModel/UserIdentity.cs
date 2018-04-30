using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.ObjectModel
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UserIdentity
    {
        public abstract string GetVroomAuthentication();

        public abstract string GetSkyAPICookie();

    }
}
