using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveClient.ObjectModel
{
    public class GlobalStore
    {
        private static GlobalStore instance = null;

        private GlobalStore()
        {

        }

        public static GlobalStore GetInstance()
        {
            if (instance == null)
            {
                instance = new GlobalStore();
            }

            return instance;
        }


        private List<OneDriveAccount> accounts = new List<OneDriveAccount>();

        public void AddAccount(OneDriveAccount account)
        {
            accounts.Add(account);
        }

        public OneDriveAccount DefaultAccount
        {
            get
            {
                return accounts.Count > 0 ? accounts.ElementAt(0) : null;
            }
        }

    }
}
