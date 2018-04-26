using OneDriveClient.ObjectModel;
using OneDriveClient.WebEndPoints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneDriveClient
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient webClient = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            CampEndpoint camp = new CampEndpoint();
            OneDriveAccount account = await camp.CheckoutAccount();
            GlobalStore.GetInstance().AddAccount(account);

            LiveLoginEndpoint liveLogin = new LiveLoginEndpoint();
            await liveLogin.LoginAccount(account);

            RampEndpoint ramp = new RampEndpoint();
            await ramp.SetRamp(account, "EnablePermissionPremiumAccountCheck", RampEndpoint.RampOption.OptOut);
            await ramp.SetRamp(account, "EnablePermissionPassword", RampEndpoint.RampOption.OptIn);




        }
    }
}
