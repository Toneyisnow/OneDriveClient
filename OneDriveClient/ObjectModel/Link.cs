using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.ObjectModel
{
    public enum LinkType
    {
        Edit,
        ReadOnly
    }

    public class Link
    {
        public LinkType Type;


        public Document Item;
        public string ShareId;

        public string Authkey;

        public string WebUrl;

    }
}
