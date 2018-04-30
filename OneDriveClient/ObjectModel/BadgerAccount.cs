using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.ObjectModel
{
    public class BadgerAccount : UserIdentity
    {
        public string BadgerId;


        public override string GetVroomAuthentication()
        {
            if (string.IsNullOrEmpty(BadgerId))
            {
                throw new InvalidOperationException("No BadgerId found for Authentication.");
            }

            return "Badger " + BadgerId;
        }

        public override string GetSkyAPICookie()
        {
            if (string.IsNullOrEmpty(BadgerId))
            {
                throw new InvalidOperationException("No BadgerId found for Authentication.");
            }

            return "BadgerAuth=" + BadgerId;
        }


    }
}
