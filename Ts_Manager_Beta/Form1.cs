using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Threading;
using TS3QueryLib.Core;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Communication;
using TS3QueryLib.Core.Query;
using TS3QueryLib.Core.Query.HelperClasses;
using TS3QueryLib.Core.Query.Notification;
using TS3QueryLib.Core.Query.Responses;


namespace Ts_Manager_Beta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();  
        }
        // Declaring queryRunner Of "QueryRunner" Type As Public
        QueryRunner queryRunner;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Parameters When The Programm Opens
            //
            // Disable The Connect Button
            btnConnectInstance.Enabled = false;

            // ***************************************************************************
            // Default Connect Autofill + Connect Button Enabling + Debug Things (test/Debug only)
            //
            // Disable Disconnect Button (See The Disconnect On Click Part Later In The Code For Explaination
            btnDisconnect.Enabled = false;
            // Address/IP
            txtBoxAddressIP.Text = "127.0.0.1";
            // Query Port
            txtBoxPort.Text = "10011";
            // Query Login
            txtBoxQueryLogin.Text = "serveradmin";
            // Query Password
            txtBoxQueryPassword.Text = "123456";
            // If The Address/IP AND Port AND Query Login AND Query Password Are NOT Empty
            if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
            {
                // Then Enable The Connect Button
                btnConnectInstance.Enabled = true;
            }
            // Else Do Nothing (The Button Is Disabled Per Default At Programm Launch)
            // *****************************************************************************

            // Get The Programm Version And Add It In The Help Section In Version Label
            //
            lblVersion.Text += ProductVersion.ToString();

            // Disable TextBoxes In Then Edit Tab (Prevents Error If Not Connected)
            //
            // Disable The Server Standard Groups Default Channel Admin Group Edit TextBox
            txtBoxServerEditStdGrpsDefChnlAdminGrp.Enabled = false;
            // Disable The Server Standard Groups Default Channel Group Edit TextBox
            txtBoxServerEditStdGrpsDefChnlGrp.Enabled = false;
            // Disable The Server Standard Groups Default Server Group Edit TextBox
            txtBoxServerEditStdGrpsDefServGrp.Enabled = false;
            // Disable The Server Host Message Edit TextBox
            txtBoxServerEditHostMessage.Enabled = false;
            // Disable The Server Icon ID Edit TextBox
            txtBoxServerEditVirtServIconID.Enabled = false;
            // Disable The Server Max. Client Edit TextBox
            txtBoxServerEditVirtServMaxClient.Enabled = false;
            // Disable The Server Min. Client Version Edit TextBox
            txtBoxServerEditVirtServMinClVer.Enabled = false;
            // Disable The Server Min. Numbers Of Client In Channel Before Force Silence Edit TextBox
            txtBoxServerEditVirtServMinNbClInChnlToFrceSilce.Enabled = false;
            // Disable The Server Name Edit TextBox
            txtBoxServerEditVirtServName.Enabled = false;
            // Disable The Server Port Edit TextBox
            txtBoxServerVirtServEditPort.Enabled = false;
            // Disable The Server Security Level Edit TextBox
            txtBoxServerEditVirtServIdentSecuLvl.Enabled = false;
            // Disable The Server Welcome Message Edit TextBox
            txtBoxServerEditVirtServWelcMsg.Enabled = false;
            // Disable The Server Modal Message And Exit "Edit" Radio Button
            rdBtnServerEditModalMsgAndExit.Enabled = false;
            // Disable The Server No Message "Edit" Radio Button
            rdBtnServerEditNoMessage.Enabled = false;
            // Disable The Server Show Modal Message "Edit" Radio Button
            rdBtnServerEditShowModalMsg.Enabled = false;
            // Disable The Server Show Message In Log "Edit" Radio Button
            rdBtnServerEditShowMsgInLog.Enabled = false;
            // Disable The Server Edit Apply Button
            btnServerEditApply.Enabled = false;
        }

        // On Address/IP TextBox Changes
        private void txtBoxAdressIP_TextChanged(object sender, EventArgs e)
        {
            // Little Check To Verify If The Min. Infos Are Present Or Not To Connect,
            // When The Address/IP TextBox Is Used (Changed).
            // This Is To Have The Connect Button Disabled If Nothing Is Filled,
            // And To Enable It If Then Min. Infos Are Filled.
            //
            // If The Address/IP AND Port AND Query Login AND Query Password TextBoxes Are NOT Empty
            if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
            {
                // Then Enable The Connect Button
                btnConnectInstance.Enabled = true;
            }
            else
            {
                // Else If Those Are Empty, Disable The Connect Button
                btnConnectInstance.Enabled = false;
            }
        }

        // On Port TextBox Changes
        private void txtBoxPort_TextChanged(object sender, EventArgs e)
        {
            // Little Check To Verify If The Min. Infos Are Present Or Not To Connect,
            // When The Port TextBox Is Used (Changed).
            // This Is To Have The Connect Button Disabled If Nothing Is Filled,
            // And To Enable It If Then Min. Infos Are Filled.
            //
            // If The Address/IP AND Port AND Query Login AND Query Password TextBoxes Are NOT Empty
            if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
            {
                // Then Enable The Connect Button
                btnConnectInstance.Enabled = true;
            }
            else
            {
                // Else If Those Are Empty, Disable The Connect Button
                btnConnectInstance.Enabled = false;
            }
        }

        // When Clicking The Connect Button
        private void btnConnectInstance_Click(object sender, EventArgs e)
        {
            // Clearing All The ListBoxes
            //
            // Clear The Server Id ListBox
            lstBoxServerID.Items.Clear();
            // Clear The Server Name ListBox
            lstBoxServerName.Items.Clear();
            // Clear The Server Port ListBox
            lstBoxServerPort.Items.Clear();
            // Clear The Server Status ListBox
            lstBoxServerStatus.Items.Clear();
            // Clear The Server UpTime ListBox
            lstBoxServerUpTime.Items.Clear();
            // Clear The Server Online Client ListBox
            lstBoxServerOnlineClients.Items.Clear();
            // Clear The Server AutoStart ListBox
            lstBoxServerAutoStart.Items.Clear();
            // Clear The Server Reserved Slots ListBox
            lstBoxServerReservedSlots.Items.Clear();
            // Clear The Server Platform List Box
            lstBoxServerPlatform.Items.Clear();
            // Clear The Server Version ListBox
            lstBoxServerVersion.Items.Clear();
            // Clear The Server Creation Date ListBox
            lstBoxServerCreationDate.Items.Clear();
            // Clear The Server "Is Password Protected?" ListBox
            lstBoxServerIsPassProtected.Items.Clear();
            // Clear The Server Machine ID ListBox
            lstboxServerMachineID.Items.Clear();
            // Clear The Server Min. Client Version ListBox
            lstBoxServerMinClientVersion.Items.Clear();
            // Clear The Server Identity Security Level ListBox
            lstBoxServerIdentSecuLvl.Items.Clear();

            // If The Address/IP AND Port AND Query Login AND Query Password TextBoxes Are NOT Empty
            if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
            {
                // Then Try To Do :
                try
                {
                    // Put The Content Of The Query Login TextBox In The strLogin Variable Of "string" Type
                    string strLogin = txtBoxQueryLogin.Text;
                    // Put The Content Of The Query Password TextBox In The strPassword Variable Of "string" Type
                    string strPassword = txtBoxQueryPassword.Text;
                    // Put The Content Of The Address/IP TextBox In The strAddress Variable Of "string" Type
                    string strAddress = txtBoxAddressIP.Text;
                    // Convert The Content Of The Port TextBox ("string") Into "ushort" Type,
                    // And Put It In The uShortPort Variable Of "ushort" Type
                    ushort uShortPort = ushort.Parse(txtBoxPort.Text);
                    
                    // Create A New queryRunner Object Of QueryRunner Type And Within A New SyncTcpDispatcher Object
                    // With The Parameters (Address/IP And Port) Needed For Its Constructor
                    using (queryRunner = new QueryRunner(new SyncTcpDispatcher(strAddress, uShortPort)))
                    {
                        // Login Using The Provided Username And Password And Store The Response In LoginResponse Of "SimpleResponse" Type
                        SimpleResponse LoginResponse = queryRunner.Login(strLogin, strPassword);
                        
                        // If Their Is An Error With The Login
                        if (LoginResponse.IsErroneous)
                        {
                            // Then, Display A MessageBox With The Error Number And The Error Message Associated To This Number
                            MessageBox.Show(LoginResponse.ErrorId + " : " + LoginResponse.ErrorMessage);
                        }

                        // The Programm Is Now Connected, Getting The Server List From The Teamspeak 3 Server
                        //
                        // Ask For The Server List (All Server, So That If More Instance Use The Same Database All Server Off All Instance Will Be Listed And Recognized With The Machine ID)
                        // And Store The Result (Of "ServerListItem Type) In ServerList Of "ListResponse" Type
                        ListResponse<ServerListItem> ServerList = queryRunner.GetServerList(includeAll:true);
                        
                        // For Each Item (Server) In The ServerList, Do : 
                        foreach (ServerListItem item in ServerList)
                        {
                            // Add The Server ID Of The Current Selected Server In The List Into The Server ID ListBox
                            lstBoxServerID.Items.Add(item.ServerId);
                            // Add The Server Name Of The Current Selected Server In The List Into The Server Name ListBox
                            lstBoxServerName.Items.Add(item.ServerName);
                            // Add The Server Port Of The Current Selected Server In The List Into The Server Port ListBox
                            lstBoxServerPort.Items.Add(item.ServerPort);
                            // Add The Server Status Of The Current Selected Server In The List Into The Server Status ListBox
                            lstBoxServerStatus.Items.Add(item.ServerStatus);
                            // Add The Server Uptime Of The Current Selected Server In The List Into The Server Uptime ListBox
                            lstBoxServerUpTime.Items.Add(item.ServerUptime);
                            // Add The Server Currently Online Clients Of The Current Selected Server In The List Into The Server Online Clients ListBox
                            lstBoxServerOnlineClients.Items.Add(item.ServerNumberOfClientsOnline + " / " + item.ServerMaximumClientsAllowed);
                            // Add The Server AutoStart Status Of The Current Selected Server In The List Into The Server AutoStart ListBox
                            lstBoxServerAutoStart.Items.Add(item.ServerAutoStart);
                            // Select via Query (=use x) The Current Selected Server In The List By The Server ID
                            // (And Not By The Server Port, Even If It Is Possible Too With Another Command : queryRunner.SelectVirtualServerByPort(PortNumber))
                            queryRunner.SelectVirtualServerById(item.ServerId);
                            // Get The Server Infos Of The Current Selected Server In The List
                            // And Put The Result In ServerInfos Object Of "ServerInfoResponse Type
                            ServerInfoResponse ServerInfos = queryRunner.GetServerInfo();
                            // Add The Server Reserved Slot(s) Number(s) Of The Current Selected Server In The List Into The Reserved Slots ListBox
                            lstBoxServerReservedSlots.Items.Add(ServerInfos.ReservedSlots.ToString());
                            // Add The Server Platform Of The Current Selected Server In The List Into The Platform ListBox
                            lstBoxServerPlatform.Items.Add(ServerInfos.Platform.ToString());
                            // Add The Server Version Of The Current Selected Server In The List Into The Version ListBox
                            lstBoxServerVersion.Items.Add(ServerInfos.Version.ToString());
                            // Add The Server Creation Date Of The Current Selected Server In The List Into The Creation Date ListBox
                            lstBoxServerCreationDate.Items.Add(ServerInfos.DateCreatedUtc.ToString());
                            // Ad The Server "Is Password Protected?" Of The Current Selected Server In The List Into The "Is Password Protected?" ListBox
                            lstBoxServerIsPassProtected.Items.Add(ServerInfos.IsPasswordProtected.ToString());
                            // Add The Server Machine ID Of The Current Selected Server In The List Into The Machine ID ListBox
                            lstboxServerMachineID.Items.Add(ServerInfos.MachineId.ToString());
                            // Add The Server Min. Client Version Of The Current Selected Server In The List Into The Min. Client Version ListBox
                            lstBoxServerMinClientVersion.Items.Add(ServerInfos.MinClientVersion.ToString());
                            // Add The Server Identity Security Level Of The Current Selected Server In The List Into The Identity Security Level ListBox
                            lstBoxServerIdentSecuLvl.Items.Add(ServerInfos.NeededIdentitySecurityLevel.ToString());
                        }

                        // /!\ The Next Part Is Commented And thus Disabled For Debugging /!\
                        //
                        // Logout From The Server And Store The Answer In LogoutResponse Of "SimpleResponse" Type
                        //SimpleResponse LogoutResponse = queryRunner.Logout();

                        // If Their Is An Error With The Logout
                        /* if (LogoutResponse.IsErroneous)
                        {
                            // Then, Show A MessageBox With The Error Number And The Error Message Associated To This Number
                            MessageBox.Show(LogoutResponse.ErrorId + " : " + LogoutResponse.ErrorMessage);
                        }*/
                    }
                }
                catch // If The Try Is UnSuccessfull
                {
                    // Then, Show A MessageBox With An Error Message
                    MessageBox.Show("An Error Occured.\r\nPlease, Check Your Connection Information(s) !");
                }
            }
            else // If The Address/IP OR Port OR Query Login OR Query Password TextBoxes Is Empty (See The Related If Upper In The Code)
            {
                // Show A MessageBox Wiht An Error Message
                MessageBox.Show("Information(s) missing !");
            }
          }

        // /!\ This Button Is Actually Buggy ! The Library Is Somehow Disconnecting The Client Before I Told It. I Have To Check Were It Comes From. /!\
        // /!\ This Is Why This Button Is Not Activated For Now /!\
        //
        // When Clicking The Disconnect Button
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // If The Address/IP & Port & Query Login & Query Password Are NOT Empty
            if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
            {
                // Then, Try this :
                try
                {
                    // Defining The Variables
                    //
                    // Put The Content Of The Query Login TextBox Into The strLogin Variable Of "string" Type
                    string strLogin = txtBoxQueryLogin.Text;
                    // Put The Content Of The Query Password TextBox In The strPassword Variable Of "string" Type
                    string strPassword = txtBoxQueryPassword.Text;
                    // Put The Content Of The Address/IP TextBox In The strAddress Variable Of "string" Type
                    string strAddress = txtBoxAddressIP.Text;
                    // Convert The Content Of The Port TextBox ("string") Into "ushort" Type,
                    // And Put It In The uShortPort Variable Of "ushort" Type 
                    ushort uShortPort = ushort.Parse(txtBoxPort.Text);
                    // Create A New queryRunner Object Of QueryRunner Type And Within A New SyncTcpDispatcher Object
                    // With The Parameters (Address/IP And Port) Needed For Its Constructor
                    // using (queryRunner = new QueryRunner(new SyncTcpDispatcher(strAddress, uShortPort)))     // Disabled For Debug
                    //{                                                                                         // Disabled For Debug
                        // Logout From The Server And Store The Answer In LogoutResponse Of "SimpleResponse" Type
                        SimpleResponse LogoutResponse = queryRunner.Logout();

                        // If Their Is An Error With The Logout
                        if (LogoutResponse.IsErroneous)
                        {
                            // Then, Show A MessageBox With The Error Number And The Error Message Associated To This Number 
                            MessageBox.Show(LogoutResponse.ErrorId + " : " + LogoutResponse.ErrorMessage);
                        }
                    //}                                                                                         // Disabled For Debug
                 }
                 catch // If The Try Is UnSuccessfull
                 {
                     // Then, Show A MessageBox With An Error Message
                     MessageBox.Show("An Error Occured.\r\nPlease, Check Your Connection Information(s) !");
                 }
             }
             else // If The Address/IP OR Port OR Query Login OR Query Password TextBoxes Is Empty (See The Related If Upper In The Code)
             {
                // Then, Show A MessageBox With An Error Message 
                MessageBox.Show("Information(s) missing !");
             }
        }

        // When Clicking The Edit Selected Server Button
        private void btnEditSelectedServer_Click(object sender, EventArgs e)
        {
            // Activating All The TextBoxes In The Server Edition Tab
            //
            // Enable The Standard Groups Default Channel Admin Group Edit TextBox
            txtBoxServerEditStdGrpsDefChnlAdminGrp.Enabled = true;
            // Enable The Standard Groups Default Channel Group Edit TextBox
            txtBoxServerEditStdGrpsDefChnlGrp.Enabled = true;
            // Enable The Standard Groups Default Server Group Edit TextBox
            txtBoxServerEditStdGrpsDefServGrp.Enabled = true;
            // Enable The Server Host Message Edit TextBox
            txtBoxServerEditHostMessage.Enabled = true;
            // Enable The Server Icon ID Edit TextBox
            txtBoxServerEditVirtServIconID.Enabled = true;
            // Enable The Server Max. Client Edit TextBox
            txtBoxServerEditVirtServMaxClient.Enabled = true;
            // Enable The Server Min. Client Version Edit TextBox
            txtBoxServerEditVirtServMinClVer.Enabled = true;
            // Enable The Server Min. Number Of Client In Channel Before Force Silence Edit TextBox
            txtBoxServerEditVirtServMinNbClInChnlToFrceSilce.Enabled = true;
            // Enable The Server Name Edit TextBox
            txtBoxServerEditVirtServName.Enabled = true;
            // Enable The Server Port Edit TextBox
            txtBoxServerVirtServEditPort.Enabled = true;
            // Enable The Server Identity Security Level Edit TextBox
            txtBoxServerEditVirtServIdentSecuLvl.Enabled = true;
            // Enable The Server Welcome Message Edit TextBox
            txtBoxServerEditVirtServWelcMsg.Enabled = true;
            // Enable The Server Modal Message And Exit "Edit" Radio Button
            rdBtnServerEditModalMsgAndExit.Enabled = true;
            // Enable The Server No Message "Edit" Radio Button
            rdBtnServerEditNoMessage.Enabled = true;
            // Enable The Server Show Modal Message "Edit" Radio Button
            rdBtnServerEditShowModalMsg.Enabled = true;
            // Enable The Server Show Message In Log "Edit" Radio Button
            rdBtnServerEditShowMsgInLog.Enabled = true;
            // Enable The Apply Modification Button
            btnServerEditApply.Enabled = true;
            // Enable The Server Host Url Edit TextBox
            txtBoxServerEditHostUrl.Enabled = true;
            // Enable The Server Host Banner Url Edit TextBox
            txtBoxServerEditHostBnrUrl.Enabled = true;
            // Enable The The Server Host Banner Interval Edit TextBox
            txtBoxServerEditHostBnrInter.Enabled = true;
            // Enable The Server Host Button Tooltip Edit TextBox
            txtBoxServerEditHostBtnTooltip.Enabled = true;
            // Enable The Server Host Button Url Edit TextBox
            txtBoxServerEditHostBtnUrl.Enabled = true;
            // Enable The Server AutoBan Count Edit TextBox
            txtBoxServerEditAutoBanCount.Enabled = true;
            // Enable The Server AutoBan Time Edit TextBox
            txtBoxServerEditAutoBanTime.Enabled = true;
            // Enable The Server AutoBan Remove Time Edit TextBox
            txtBoxServerEditAutoBanRemoveTime.Enabled = true;
            // Enable The Server AntiFlood Points Tick Reduce Edit TextBox
            txtBoxServerEditAntiFloodPtsTickReduce.Enabled = true;
            // Enable The Server AntiFlood Points Need Warning Edit TextBox
            txtBoxServerEditAntiFloodPtsNeededWarn.Enabled = true;
            // Enable The Server AntiFlood Points Needed Kick Edit TextBox
            txtBoxServerEditAntiFloodPtsNeededKick.Enabled = true;
            // Enable The Server AntiFlood Points Needed Ban Edit TextBox
            txtBoxServerEditAntiFloodPtsNeededBan.Enabled = true;
            // Enable The Server AntiFlood Points Ban Time Edit TextBox
            txtBoxServerEditAntiFloodPtsBanTime.Enabled = true;
            // Enable The Server Transfer Max. Upload Bandwidth Limit Edit TextBox
            txtBoxServerEditTransfersUpldBandwthLimit.Enabled = true;
            // Enable The Server Transfer Upload Quota Edit TextBox
            txtBoxServerEditTransfersUpldQuota.Enabled = true;
            // Enable The Server Transfer Max. Download Bandwidth Limit Edit TextBox
            txtBoxServerEditTransfersDownldBandwthLimit.Enabled = true;
            // Enable The Server Transfer Download Quota Edit TextBox
            txtBoxServerEditTransfersDownldQuota.Enabled = true;
            // Enable The Server New Password Edit TextBox
            txtBoxServerEditServerPasswordNewPassword.Enabled = true;
            // Enable The Server AutoStart Edit TextBox
            txtBoxServerEditAutoStart.Enabled = true;

            // Try this :
            try
            {
                int ModifServId = lstBoxServerID.SelectedIndex;
                ModifServId++;
                if (ModifServId >= 1)
                {
                    //MessageBox.Show(Convert.ToString(ModifServId));
                    tabControl1.SelectTab(1);
                    if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
                    {
                        try
                        {
                            // Put The Content Of The Query Login TextBox In The strLogin Variable Of "string" Type
                            string strLogin = txtBoxQueryLogin.Text;
                            // Put The Content Of The Query Password TextBox In The strPassword Variable Of "string" Type
                            string strPassword = txtBoxQueryPassword.Text;
                            // Put The Content Of The Address/IP TextBox In The strAddress Variable Of "string" Type
                            string strAddress = txtBoxAddressIP.Text;
                            // Convert The Content Of The Port TextBox ("string") Into "ushort" Type,
                            // And Put It In The uShortPort Variable Of "ushort" Type
                            ushort uShortPort = ushort.Parse(txtBoxPort.Text);

                            // Create A New queryRunner Object Of QueryRunner Type And Within A New SyncTcpDispatcher Object
                            // With The Parameters (Address/IP And Port) Needed For Its Constructor
                            using (queryRunner = new QueryRunner(new SyncTcpDispatcher(strAddress, uShortPort)))
                            {
                                // Login Using The Provided Username And Password And Store The Response In LoginResponse Of "SimpleResponse" Type
                                SimpleResponse LoginResponse = queryRunner.Login(strLogin, strPassword);

                                // If Their Is An Error With The Login
                                if (LoginResponse.IsErroneous)
                                {
                                    // Then, Display A MessageBox With The Error Number And The Error Message Associated To This Number
                                    MessageBox.Show(LoginResponse.ErrorId + " : " + LoginResponse.ErrorMessage);
                                }

                                // We Now Get The Selected Server Item ID In The Server ID ListBox
                                int iSelectedID = Convert.ToInt32(lstBoxServerID.SelectedItem.ToString());

                                // Select The Virtual Server And Store The Server Response In SelectResponse Of "SimpleResponse" Type
                                SimpleResponse SelectResponse = queryRunner.SelectVirtualServerById(Convert.ToUInt32(iSelectedID));
                                // If Their Is An Error With The Selection
                                if (SelectResponse.IsErroneous)
                                {
                                    // Then, Display A MessageBox With The Error Number And The Error Message Associated To This Number
                                    MessageBox.Show(SelectResponse.ErrorId + " : " + SelectResponse.ErrorMessage);
                                }
                                else
                                {

                                    // Get The Server Infos And Store The Infos In serverinfo Of "ServerIfnoResponse" Type
                                    ServerInfoResponse serverinfo = queryRunner.GetServerInfo();
                                    // Get The Server Default Channel Admin Group ID, Convert The Value Into "string" And
                                    // Put It In The Defautl Channel Admin Group ID Edit TextBox
                                    txtBoxServerEditStdGrpsDefChnlAdminGrp.Text = serverinfo.DefaultChannelAdminGroupId.ToString();
                                    // Get The Server Default Chanel Group ID, Convert The Value Into "string" And Put It
                                    // In The Default Channel Group ID Edit TextBox
                                    txtBoxServerEditStdGrpsDefChnlGrp.Text = serverinfo.DefaultChannelGroupId.ToString();
                                    // Get The Server Default Server Group ID, Convert The Value Into "string" And Put It
                                    // In The Default Server Group ID Edit TextBox
                                    txtBoxServerEditStdGrpsDefServGrp.Text = serverinfo.DefaultServerGroupId.ToString();

                                    // If The Server Host Message Is Empty (In This Case When It's Empty The Value Is "null")
                                    if (serverinfo.HostMessage == null)
                                    {
                                        // Then Put An Empty String Value In The Server Host Message Edit TextBox
                                        txtBoxServerEditHostMessage.Text = "";
                                    }
                                    else // Else, Do :
                                    {
                                        // Put The Value Of The Server Host Message And Put It In The Server Host Message Edit TextBox
                                        txtBoxServerEditHostMessage.Text = serverinfo.HostMessage.ToString();
                                    }

                                    // Get The Server Icon ID And Put It In The Server Icon ID Edit TextBox
                                    txtBoxServerEditVirtServIconID.Text = serverinfo.IconId.ToString();
                                    // Get The Server Max. Client And Put It In The Server Max. Client Edit TextBox
                                    txtBoxServerEditVirtServMaxClient.Text = serverinfo.MaximumClientsAllowed.ToString();
                                    // Get The Server Min.Client Version And Put It In The Min. Client Version Edit TextBox
                                    txtBoxServerEditVirtServMinClVer.Text = serverinfo.MinClientVersion.ToString();
                                    // Get The Server Min. Number Client In Channel Before Force Silence And Put It In The
                                    // Min. Number Client In Channel Before Force Silence Edit TextBox
                                    txtBoxServerEditVirtServMinNbClInChnlToFrceSilce.Text = serverinfo.MinClientsBeforeForcedSilence.ToString();
                                    // Get The Server Name And Put It In The Server Name Edit TextBox
                                    txtBoxServerEditVirtServName.Text = serverinfo.Name.ToString();
                                    // Get The Server Port And Put It In The Server Port Edit TextBox
                                    txtBoxServerVirtServEditPort.Text = serverinfo.Port.ToString();
                                    // Get The Server Identity Security Level And Put It In The Server Identity Security Level Edit TextBox
                                    txtBoxServerEditVirtServIdentSecuLvl.Text = serverinfo.NeededIdentitySecurityLevel.ToString();

                                    // If The Server Welcome Message Is Empty (In Our Case Empty = "null")
                                    if (serverinfo.WelcomeMessage == null)
                                    {
                                        // Put An Empty String Value In The Server Welcome Message Edit TextBox
                                        txtBoxServerEditVirtServWelcMsg.Text = "";
                                    }
                                    else
                                    {
                                        // Get The Server Welcome Message And Put It In The Server Welcome Message Edit TextBox
                                        txtBoxServerEditVirtServWelcMsg.Text = serverinfo.WelcomeMessage.ToString();
                                    }

                                    HostMessageMode MessageMode = serverinfo.HostMessageMode;
                                    // If The Server Host Message Mode Is "None"
                                    if (MessageMode == HostMessageMode.HostMessageModeNone)
                                    {
                                        // Enable The No Message Radio Button
                                        rdBtnServerEditNoMessage.Checked = true;
                                    }
                                    else if (MessageMode == HostMessageMode.HostMessageModeLog) // Else, If The Server Host Message Mode Is "Log"
                                    {
                                        // Enable The "Log" Radio Button
                                        rdBtnServerEditShowMsgInLog.Checked = true;
                                    }
                                    else if (MessageMode == HostMessageMode.HostMessageModeModal) // Else, If The Server Host Message Mode Is "Modal"
                                    {
                                        // Enable The "Modal" Radio Button
                                        rdBtnServerEditShowModalMsg.Checked = true;
                                    }
                                    else // If The Server Host Message Mode Is None Of The Others, Then It's "Modal And Quit" :
                                    {
                                        // So, We Enable The "Modal And Quit" Radio Button
                                        rdBtnServerEditModalMsgAndExit.Checked = true;
                                    }

                                    // Get The Server Host Url And Put It In The Server Host Url Edit TextBox
                                    txtBoxServerEditHostUrl.Text = serverinfo.HostBannerUrl.ToString();
                                    // Get The Server Host Banner Url And Put It In The Server Host Banner Url Edit TextBox
                                    txtBoxServerEditHostBnrUrl.Text = serverinfo.HostBannerGraphicsUrl.ToString();
                                    // Get The Server Host Banner Interval And Put It In The Server Host Banner Interval Edit TextBox
                                    txtBoxServerEditHostBnrInter.Text = serverinfo.HostBannerGraphicsInterval.ToString();
                                    // Get The Server Host Button Tooltop And Put It In The Server Host Button Edit TextBox
                                    txtBoxServerEditHostBtnTooltip.Text = serverinfo.HostButtonTooltip.ToString();
                                    // Get The Server Host Button Url And Put It In The Server Host Button Url Edit TextBox
                                    txtBoxServerEditHostBtnUrl.Text = serverinfo.HostButtonUrl.ToString();
                                    // Get The Server AutoBan Count And Put It In the Server AutoBan Count Edit TextBox
                                    txtBoxServerEditAutoBanCount.Text = serverinfo.ComplainAutoBanCount.ToString();
                                    // Get The Server AutoBan Time And Put It In The Server AutoBan Time Edit TextBox
                                    txtBoxServerEditAutoBanTime.Text = serverinfo.ComplainAutoBanTime.ToString();
                                    // Get The Server AutoBan Remove Time And Put It In The Server AutoBan Remove Time Edit TextBox
                                    txtBoxServerEditAutoBanRemoveTime.Text = serverinfo.ComplainRemoveTime.ToString();
                                    // Get The Server AntiFlood Points Tick Reduce And Put It In The Server AntiFlood Points Tick Reduce Edit TextBox
                                    txtBoxServerEditAntiFloodPtsTickReduce.Text = serverinfo.AntiFloodPointsTickReduce.ToString();
                                    // Get The Server AntiFlood Needed Warning And Put It In The Server AntiFlood Needed Warning Edit TextBox
                                    txtBoxServerEditAntiFloodPtsNeededWarn.Text = serverinfo.AntiFloodPointsNeededWarning.ToString();
                                    // Get The Server AntiFlood Needed Kick And Put It In The Server AntiFlood Needed Kick Edit TextBox
                                    txtBoxServerEditAntiFloodPtsNeededKick.Text = serverinfo.AntiFloodPointsNeededKick.ToString();
                                    // Get The Server AntiFlood Needed Ban And Put It In The Server AntiFlood Needed Ban Edit TextBox
                                    txtBoxServerEditAntiFloodPtsNeededBan.Text = serverinfo.AntiFloodPointsNeededBan.ToString();
                                    // Get The Server AntiFlood Points Ban Time And Put It In The Server AntiFlood Ban Time Edit TextBox
                                    txtBoxServerEditAntiFloodPtsBanTime.Text = serverinfo.AntiFloodBanTime.ToString();
                                    // Get The Server Upload Bandwidth Limit And Put It In The Server Upload Bandwidth Edit TextBox
                                    txtBoxServerEditTransfersUpldBandwthLimit.Text = serverinfo.MaxUploadTotalBandwidth.ToString();
                                    // Get The Server Upload Quota And Put It In The Server Upload Quota Edit TextBox
                                    txtBoxServerEditTransfersUpldQuota.Text = serverinfo.UploadQuota.ToString();
                                    // Get The Server Download Bandwidth Limit And Put It In The Server Download Bandwidth Edit TextBox
                                    txtBoxServerEditTransfersDownldBandwthLimit.Text = serverinfo.MaxDownloadTotalBandwidth.ToString();
                                    // Get The Server Download Quota And Put It In The Server Download Quota Edit TextBox
                                    txtBoxServerEditTransfersDownldQuota.Text = serverinfo.DownloadQuota.ToString();
                                    // Get The Server Password And Put It In The Server Password Edit TextBox
                                    //
                                    // If The Server Password Is Empty (In This Case The "Empty" Value Is "null")
                                    if (serverinfo.PasswordHash == null)
                                    {
                                        // Then Put An Empty String Value In The Server New Password Edit TextBox
                                        txtBoxServerEditServerPasswordNewPassword.Text = "";
                                    }
                                    else // Else
                                    {
                                        // Put The Value Of The Server Password And Put It In The Server New Password Edit TextBox
                                        txtBoxServerEditServerPasswordNewPassword.Text = serverinfo.PasswordHash.ToString();
                                    }

                                    // Get The AutoStart Value And Put It In The AutoStart Edit TextBox
                                    txtBoxServerEditAutoStart.Text = serverinfo.AutoStart.ToString();

                                    // /!\ The Next Part Is Commented And thus Disabled For Debugging /!\
                                    //
                                    // Logout From The Server And Store The Answer In LogoutResponse Of "SimpleResponse" Type
                                    //SimpleResponse LogoutResponse = queryRunner.Logout();

                                    // If Their Is An Error With The Logout
                                    /* if (LogoutResponse.IsErroneous)
                                    {
                                        // Then, Show A MessageBox With The Error Number And The Error Message Associated To This Number
                                        MessageBox.Show(LogoutResponse.ErrorId + " : " + LogoutResponse.ErrorMessage);
                                    }*/
                                }
                            }
                        }
                        catch
                        {
                            // Then, Show A MessageBox With An Error Message
                            MessageBox.Show("An Error Occured.\r\nPlease, Check Your Connection Information(s) !");
                        }
                    }
                    else
                    {
                        // Then, Show A MessageBox With An Error Message
                        MessageBox.Show("Information(s) missing !");
                    }
                }
                else
                {
                    // Then, Show A MessageBox With An Error Message
                    MessageBox.Show("Please choose a server ID");
                }
            }
            catch
            {
                // Then, Show A MessageBox With An Error Message
                MessageBox.Show("An error occured !");
            }


        }

        // When Clicking The Apply Editions Button
        private void btnServerEditApply_Click(object sender, EventArgs e)
        {
            // If Address/IP & Port & Query Login & Query Pasword Are NOT Empty
            if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
            {
                // Then Try :
                try
                {
                    // Put The Content Of The Query Login TextBox In The strLogin Variable Of "string" Type
                    string strLogin = txtBoxQueryLogin.Text;
                    // Put The Content Of The Query Password TextBox In The strPassword Variable Of "string" Type
                    string strPassword = txtBoxQueryPassword.Text;
                    // Put The Content Of The Address/IP TextBox In The strAddress Variable Of "string" Type
                    string strAddress = txtBoxAddressIP.Text;
                    // Convert The Content Of The Port TextBox ("string") Into "ushort" Type,
                    // And Put It In The uShortPort Variable Of "ushort" Type
                    ushort uShortPort = ushort.Parse(txtBoxPort.Text);

                    // Create A New queryRunner Object Of QueryRunner Type And Within A New SyncTcpDispatcher Object
                    // With The Parameters (Address/IP And Port) Needed For Its Constructor
                    using (queryRunner = new QueryRunner(new SyncTcpDispatcher(strAddress, uShortPort)))
                    {
                        // Login Using The Provided Username And Password And Store The Response In LoginResponse Of "SimpleResponse" Type
                        SimpleResponse LoginResponse = queryRunner.Login(strLogin, strPassword);

                        // If Their Is An Error With The Login
                        if (LoginResponse.IsErroneous)
                        {
                            // Then, Display A MessageBox With The Error Number And The Error Message Associated To This Number
                            MessageBox.Show(LoginResponse.ErrorId + " : " + LoginResponse.ErrorMessage);
                        }

                        // We Now Get The Selected Server Item ID In The Server ID ListBox
                        int iSelectedID = Convert.ToInt32(lstBoxServerID.SelectedItem.ToString());

                        // Select The Virtual Server And Store The Server Response In SelectResponse Of "SimpleResponse" Type
                        SimpleResponse SelectResponse = queryRunner.SelectVirtualServerById(Convert.ToUInt32(iSelectedID));
                        // If Their Is An Error With The Selection
                        if (SelectResponse.IsErroneous)
                        {
                            // Then, Display A MessageBox With The Error Number And The Error Message Associated To This Number
                            MessageBox.Show(SelectResponse.ErrorId + " : " + SelectResponse.ErrorMessage);
                        }
                        else
                        {

                            // Create A New Object Named ServEdit Of "VirtualServerModification" Type That Will Apply Our Modification To The Server
                            VirtualServerModification ServEdit = new VirtualServerModification();

                            // Get The Value Of The Default Server Channel Admin Group ID Edit TextBox, Convert It Into "UInt32",
                            // And Store It In ServEdit
                            ServEdit.DefaultChannelAdminGroupId = Convert.ToUInt32(txtBoxServerEditStdGrpsDefChnlAdminGrp.Text);
                            // Get The Value Of The Default Server Channel Group ID Edit TextBox, Convert It Into "UInt32",
                            // And Store It In ServEdit
                            ServEdit.DefaultChannelGroupId = Convert.ToUInt32(txtBoxServerEditStdGrpsDefChnlGrp.Text);
                            // Get The Value Of The Default Server Group ID Edit TextBox, Convert It Into "UInt32", And Store It In ServEdit
                            ServEdit.DefaultServerGroupId = Convert.ToUInt32(txtBoxServerEditStdGrpsDefServGrp.Text);
                            // Get The Value Of The Server Host Message Edit TextBox And Apply It In ServEdit
                            ServEdit.HostMessage = txtBoxServerEditHostMessage.Text;
                            // Get The Value Of The Server Virtual Icon ID Edit TextBox, Convert It Into "UInt32", And Store It In ServEdit
                            ServEdit.IconId = Convert.ToUInt32(txtBoxServerEditVirtServIconID.Text);
                            // Get The Value Of The Server Max. Client Edit TextBox, Convert It Into "Int32", And Store It In ServEdit
                            ServEdit.MaxClients = Convert.ToInt32(txtBoxServerEditVirtServMaxClient.Text);
                            // Get The Value Of The Server Min. Client Version Edit TextBox And Store It In ServEdit
                            ServEdit.MinClientVersion = txtBoxServerEditVirtServMinClVer.Text;
                            // Get The Value Of The Server Min. Number Client In Channel Before Force Silence Edit TextBox,
                            // Convert It Into "Int32", And Store It In ServEdit
                            ServEdit.MinClientsInChannelBeforeForcedSilence = Convert.ToInt32(txtBoxServerEditVirtServMinNbClInChnlToFrceSilce.Text);
                            // Get The Value Of The Server Name Edit TextBox And Store It In ServEdit
                            ServEdit.Name = txtBoxServerEditVirtServName.Text;
                            // Get The Value Of The Server Port Edit TextBox, Convert It Into "ushort", And Store It In ServEdit
                            ServEdit.Port = ushort.Parse(txtBoxServerVirtServEditPort.Text);
                            // Get The Value Of The Server Identity Security Level, Convert It Into "Int32", And Store It In ServEdit
                            ServEdit.NeededIdentitySecurityLevel = Convert.ToInt32(txtBoxServerEditVirtServIdentSecuLvl.Text);
                            // Get The Value Of The Server Welcome Message Edit TextBox And Store It In ServEdit
                            ServEdit.WelcomeMessage = txtBoxServerEditVirtServWelcMsg.Text;

                            // If "No Message" Radio Button Is Selected
                            if (rdBtnServerEditNoMessage.Checked == true)
                            {
                                // Store The "None" Message Mode In ServEdit
                                ServEdit.HostMessageMode = HostMessageMode.HostMessageModeNone;
                            }
                            else if (rdBtnServerEditShowMsgInLog.Checked == true) // Else, If "Log" Radio Button Is Selected
                            {
                                // Store The "Log" Message Mode In ServEdit
                                ServEdit.HostMessageMode = HostMessageMode.HostMessageModeLog;
                            }
                            else if (rdBtnServerEditShowModalMsg.Checked == true) // Else, If "Modal" Radio Button Is Selected
                            {
                                // Store The "Modal" Message Mode In ServEdit
                                ServEdit.HostMessageMode = HostMessageMode.HostMessageModeModal;
                            }
                            else // Else (If It Is Not The Others, Then It's The "Modal And Quit" Mode)
                            {
                                // Store The "Modal And Quit" Message Mode In ServEdit
                                ServEdit.HostMessageMode = HostMessageMode.HostMessageModeModalQuit;
                            }

                            // Get The Value Of The Server Host Url And Store It In The ServEdit
                            ServEdit.HostBannerUrl = txtBoxServerEditHostUrl.Text;
                            // Get The Value Of The Server Host Banner Url And Store It In The ServEdit
                            ServEdit.HostBannerGraphicsUrl = txtBoxServerEditHostBnrUrl.Text;
                            // Get The Value Of The Server Host Banner Interval, Convert It Into "UInt32", And Store It In The ServEdit
                            ServEdit.HostBannerGraphicsInterval = Convert.ToUInt32(txtBoxServerEditHostBnrInter.Text);
                            // Get The Value Of The Server Host Button Tooltop And Store It In The ServEdit
                            ServEdit.HostButtonTooltip = txtBoxServerEditHostBtnTooltip.Text;
                            // Get The Value Of The Server Host Button Url And Store It In The ServEdit
                            ServEdit.HostButtonUrl = txtBoxServerEditHostBtnUrl.Text;
                            // Get The Value Of The Server AutoBan Count, Convert It Into "Int32", And Store It In The ServEdit
                            ServEdit.ComplainAutoBanCount = Convert.ToInt32(txtBoxServerEditAutoBanCount.Text);
                            // Get The Value Of The Server AutoBan Time, Convert It Into "Int32", And Store It In The ServEdit
                            ServEdit.ComplainAutoBanTime = Convert.ToInt32(txtBoxServerEditAutoBanTime.Text);
                            // Get The Value Of The Server AutoBan Remove Time, Convert It Into "Int32", And Store It In The ServEdit
                            ServEdit.ComplainRemoveTime = Convert.ToInt32(txtBoxServerEditAutoBanRemoveTime.Text);
                            // Get The Value Of The Server AntiFlood Points Tick Reduce, Convert It Into "Int32", And Store It In The ServEdit
                            ServEdit.AntiFloodPointsTickReduce = Convert.ToInt32(txtBoxServerEditAntiFloodPtsTickReduce.Text);
                            // Get The Value Of The Server AntiFlood Needed Warning, Convert It Into "Int32", And Store It In The ServEdit
                            ServEdit.AntiFloodPointsNeededWarning = Convert.ToInt32(txtBoxServerEditAntiFloodPtsNeededWarn.Text);
                            // Get The Value Of The Server AntiFlood Needed Kick, Convert It Into "Int32", And Store It In The ServEdit
                            ServEdit.AntiFloodPointsNeededKick = Convert.ToInt32(txtBoxServerEditAntiFloodPtsNeededKick.Text);
                            // Get The Value Of The Server AntiFlood Needed Ban, Convert It Into "Int32", And Store It In The ServEdit
                            ServEdit.AntiFloodPointsNeededBan = Convert.ToInt32(txtBoxServerEditAntiFloodPtsNeededBan.Text);
                            // Get The Value Of The Server AntiFlood Points Ban Time, Convert It Into "Int32", And Store It In The ServEdit
                            ServEdit.AntiFloodBanTime = Convert.ToInt32(txtBoxServerEditAntiFloodPtsBanTime.Text);
                            // Get The Value Of The Server Upload Bandwidth Limit, Convert It Into "ulong", And Store It In The ServEdit
                            ServEdit.MaxUploadTotalBandwidth = ulong.Parse(txtBoxServerEditTransfersUpldBandwthLimit.Text);
                            // Get The Value Of The Server Upload Quota, Convert It Into "ulong", And Store It In The ServEdit
                            ServEdit.UploadQuota = ulong.Parse(txtBoxServerEditTransfersUpldQuota.Text);
                            // Get The Value Of The Server Download Bandwidth Limit, Convert It Into "ulong", And Store It In The ServEdit
                            ServEdit.MaxDownloadTotalBandwidth = ulong.Parse(txtBoxServerEditTransfersDownldBandwthLimit.Text);
                            // Get The Value Of The Server Download Quota, Convert It Into "ulong", And Store It In The ServEdit
                            ServEdit.DownloadQuota = ulong.Parse(txtBoxServerEditTransfersDownldQuota.Text);
                            // Get The Value Of The Server New Password And Store It In The ServEdit
                            ServEdit.Password = txtBoxServerEditServerPasswordNewPassword.Text;
                            // Get The Value Of The Server AutoStart And Store It In The ServEdit
                            ServEdit.AutoStart = Convert.ToBoolean(txtBoxServerEditAutoStart.Text);

                            // Apply All Our Modification Stored In ServEdit On Our Teamspeak 3 Server
                            queryRunner.EditServer(ServEdit);

                            // /!\ The Next Part Is Commented And thus Disabled For Debugging /!\
                            //
                            // Logout From The Server And Store The Answer In LogoutResponse Of "SimpleResponse" Type
                            //SimpleResponse LogoutResponse = queryRunner.Logout();

                            // If Their Is An Error With The Logout
                            /* if (LogoutResponse.IsErroneous)
                            {
                                // Then, Show A MessageBox With The Error Number And The Error Message Associated To This Number
                                MessageBox.Show(LogoutResponse.ErrorId + " : " + LogoutResponse.ErrorMessage);
                            }*/
                        }
                    }
                }
                catch
                {
                    // Then, Show A MessageBox With An Error Message
                    MessageBox.Show("An Error Occured.\r\nPlease, Check Your Connection Information(s) !");
                }
            }
            else // Else,
            {
                // Then, Show A MessageBox With An Error Message
                MessageBox.Show("Information(s) missing !");
            }
        }    
    }
}
