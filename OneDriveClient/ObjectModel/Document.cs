using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.ObjectModel
{
    public class Document
    {
        public string FileName;

        public string ParentPath;

        public OneDriveAccount Account;

        public string ResourceId;

        public string DriveId
        {
            get
            {
                return !string.IsNullOrEmpty(ResourceId) ? ResourceId.Split(new string[] { "!" }, StringSplitOptions.None).FirstOrDefault<string>() : string.Empty;
            }
        }

    }

}
