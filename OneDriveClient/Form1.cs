﻿using OneDriveClient.ObjectModel;
using OneDriveClient.WebEndPoints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
            await liveLogin.LoginRST(account);

            RampEndpoint ramp = new RampEndpoint();
            await ramp.SetRamp(account, "EnablePermissionPremiumAccountCheck", RampEndpoint.RampOption.OptOut);
            await ramp.SetRamp(account, "EnablePermissionPassword", RampEndpoint.RampOption.OptIn);

            Thread.Sleep(10000);

            await liveLogin.LoginPPSecure(account);

            VroomEndpoint vroom = new VroomEndpoint();
            Document document = await vroom.CreateFile(account, "test.txt");

            Link link = await vroom.CreateLink(account, document, LinkType.Edit, "test");

            BadgerEndpoint badger = new BadgerEndpoint();
            BadgerAccount badgerUser = await badger.CreateAccount();

            await vroom.ValidatePermission(badgerUser, link, "test");

            SkyAPIEndpoint skyAPI = new SkyAPIEndpoint();
            await skyAPI.GetItems(badgerUser, document, link.Authkey);

            // Get Download URL
            await vroom.GetDownloadUrl(badgerUser, document, link.Authkey);

        }
    }
}
