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

        private void Form1_Load(object sender, EventArgs e)
        {
            // Parameters When The Programm Opens
            //
            // Disable The Connect Button
            btnConnectInstance.Enabled = false;

            // ***************************************************************************
            // Default Connect Autofill + Connect Button Enabling (test/Debug only)
            //
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
            // Disable The Server Min. Numbers Of Client In Channel To Force Silence Edit TextBox
            txtBoxServerEditVirtServMinNbClInChnlToFrceSilce.Enabled = false;
            // Disable The Server Name Edit TextBox
            txtBoxServerEditVirtServName.Enabled = false;
            // Disable The Server Port Edit TextBox
            txtBoxServerVirtServEditPort.Enabled = false;
            // Disable The Server Security Level Edit TextBox
            txtBoxServerEditVirtServIdentSecuLvl.Enabled = false;
            // Disable The Server Welcome Message Edit TextBox
            txtBoxServerEditVirtServWelcMsg.Enabled = false;
            // Disable The Server AutoStart No "Edit" Radio Button
            rdBtnServerEditAutoStartNo.Enabled = false;
            // Disable The Server AutoStart Yes "Edit" Radio Button
            rdBtnServerEditAutoStartYes.Enabled = false;
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
                    using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher(strAddress, uShortPort)))
                    {
                        // Login Using The Provided Username And Password And Store The Response In LoginResponse Object Of "SimpleResponse" Type
                        SimpleResponse LoginResponse = queryRunner.Login(strLogin, strPassword);
                        
                        // If Their Is An Error With The Login
                        if (LoginResponse.IsErroneous)
                        {
                            // Display A MessageBox With The Error Number And The Error Message Associated To This Number
                            MessageBox.Show(LoginResponse.ErrorMessage + "\r\n" + LoginResponse.ErrorId);
                        }

                        // The Programm Is Now Connected, Getting The Server List From The Teamspeak 3 Server
                        //
                        // Ask For The Server List And Store The Result (Of "ServerListItem Type) In ServerList Object Of "ListResponse" Type
                        ListResponse<ServerListItem> ServerList = queryRunner.GetServerList();
                        
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

                        SimpleResponse logoutResponse = queryRunner.Logout();

                        if (logoutResponse.IsErroneous)
                        {
                            // login failed! Use loginResponse.ErrorMessage and loginResponse.ErrorId to get the reason

                            MessageBox.Show(LoginResponse.ErrorId + " : " + LoginResponse.ErrorMessage);
                        }

                    }
                }
                catch
                {
                    MessageBox.Show("An Error Occured.\r\nPlease, Check Your Connection Information(s) !");
                }
            }
            else
            {
                MessageBox.Show("Information(s) missing !");
            }
          }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
            {
                try
                {
                   string strLogin = txtBoxQueryLogin.Text;
                   string strPassword = txtBoxQueryPassword.Text;
                   string strAddress = txtBoxAddressIP.Text;
                   ushort uShortPort = ushort.Parse(txtBoxPort.Text);
                   ushort iPort = uShortPort;
                   using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher(strAddress, iPort)))
                   {
                       // login using the provided username and password and show a dump-output of the response in a textbox
                       SimpleResponse loginResponse = queryRunner.Logout();

                       if (loginResponse.IsErroneous)
                       {
                           // login failed! Use loginResponse.ErrorMessage and loginResponse.ErrorId to get the reason

                           MessageBox.Show(loginResponse.ErrorId + " : " + loginResponse.ErrorMessage);
                       }
                   }
                    

                }
                catch
                {
                    MessageBox.Show("An Error Occured.\r\nPlease, Check Your Connection Information(s) !");
                }
            }
            else
            {
                MessageBox.Show("Information(s) missing !");
            }
        }

        private void btnEditSelectedServer_Click(object sender, EventArgs e)
        {
            txtBoxServerEditStdGrpsDefChnlAdminGrp.Enabled = true;
            txtBoxServerEditStdGrpsDefChnlGrp.Enabled = true;
            txtBoxServerEditStdGrpsDefServGrp.Enabled = true;
            txtBoxServerEditHostMessage.Enabled = true;
            txtBoxServerEditVirtServIconID.Enabled = true;
            txtBoxServerEditVirtServMaxClient.Enabled = true;
            txtBoxServerEditVirtServMinClVer.Enabled = true;
            txtBoxServerEditVirtServMinNbClInChnlToFrceSilce.Enabled = true;
            txtBoxServerEditVirtServName.Enabled = true;
            txtBoxServerVirtServEditPort.Enabled = true;
            txtBoxServerEditVirtServIdentSecuLvl.Enabled = true;
            txtBoxServerEditVirtServWelcMsg.Enabled = true;
            rdBtnServerEditAutoStartNo.Enabled = true;
            rdBtnServerEditAutoStartYes.Enabled = true;
            rdBtnServerEditModalMsgAndExit.Enabled = true;
            rdBtnServerEditNoMessage.Enabled = true;
            rdBtnServerEditShowModalMsg.Enabled = true;
            rdBtnServerEditShowMsgInLog.Enabled = true;
            btnServerEditApply.Enabled = true;

            try
            {
                int ModifServId = lstBoxServerID.SelectedIndex;
                //MessageBox.Show(Convert.ToString(ModifServId));
                ModifServId++;
                if (ModifServId >= 1)
                {
                    //MessageBox.Show(Convert.ToString(ModifServId));
                    tabControl1.SelectTab(1);
                    if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
                    {
                        try
                        {
                            string strLogin = txtBoxQueryLogin.Text;
                            string strPassword = txtBoxQueryPassword.Text;
                            string strAddress = txtBoxAddressIP.Text;
                            ushort uShortPort = ushort.Parse(txtBoxPort.Text);
                            ushort iPort = uShortPort;

                            using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher(strAddress, iPort)))
                            {
                                // login using the provided username and password and show a dump-output of the response in a textbox
                                SimpleResponse loginResponse = queryRunner.Login(strLogin, strPassword);

                                if (loginResponse.IsErroneous)
                                {
                                    // login failed! Use loginResponse.ErrorMessage and loginResponse.ErrorId to get the reason

                                    MessageBox.Show(loginResponse.ErrorMessage + "\r\n" + loginResponse.ErrorId);
                                }

                                int iSelectedID = lstBoxServerID.SelectedIndex;
                                iSelectedID++;

                                string SelectServerID = queryRunner.SelectVirtualServerById(Convert.ToUInt32(iSelectedID)).GetDumpString();
                                //MessageBox.Show(SelectServerID);
                                ServerInfoResponse serverinfo = queryRunner.GetServerInfo();
                                //MessageBox.Show(serverinfo.AutoStart.ToString());
                                //Thread.Sleep(3000);
                               /* if (serverinfo.AutoStart == true)
                                {
                                    lstBoxServerEditAutoStart.SelectedItem = 0;
                                }
                                else
                                {
                                    lstBoxServerEditAutoStart.SelectedItem = 1;
                                }
                                if (serverinfo.AutoStart == true)
                                {
                                    rdBtnServerEditAutoStartYes.Checked = true;
                                    rdBtnServerEditAutoStartNo.Checked = false;
                                }
                                else
                                {
                                    rdBtnServerEditAutoStartNo.Checked = true;
                                    rdBtnServerEditAutoStartYes.Checked = false;
                                }*/
                                txtBoxServerEditStdGrpsDefChnlAdminGrp.Text = serverinfo.DefaultChannelAdminGroupId.ToString();
                                txtBoxServerEditStdGrpsDefChnlGrp.Text = serverinfo.DefaultChannelGroupId.ToString();
                                txtBoxServerEditStdGrpsDefServGrp.Text = serverinfo.DefaultServerGroupId.ToString();
                                if(serverinfo.HostMessage == null)
                                {
                                    txtBoxServerEditHostMessage.Text = "";
                                }
                                else
                                {
                                    txtBoxServerEditHostMessage.Text = serverinfo.HostMessage.ToString();
                                }
                                txtBoxServerEditVirtServIconID.Text = serverinfo.IconId.ToString();
                                txtBoxServerEditVirtServMaxClient.Text = serverinfo.MaximumClientsAllowed.ToString();
                                txtBoxServerEditVirtServMinClVer.Text = serverinfo.MinClientVersion.ToString();
                                txtBoxServerEditVirtServMinNbClInChnlToFrceSilce.Text = serverinfo.MinClientsBeforeForcedSilence.ToString();
                                txtBoxServerEditVirtServName.Text = serverinfo.Name.ToString();
                                txtBoxServerVirtServEditPort.Text = serverinfo.Port.ToString();
                                txtBoxServerEditVirtServIdentSecuLvl.Text = serverinfo.NeededIdentitySecurityLevel.ToString();
                                txtBoxServerEditVirtServWelcMsg.Text = serverinfo.WelcomeMessage.ToString();
                                if (serverinfo.HostMessageMode == HostMessageMode.HostMessageModeNone)
                                {
                                    rdBtnServerEditNoMessage.Checked = true;
                                }
                                else if (serverinfo.HostMessageMode == HostMessageMode.HostMessageModeLog)
                                {
                                    rdBtnServerEditShowMsgInLog.Checked = true;
                                }
                                else if (serverinfo.HostMessageMode == HostMessageMode.HostMessageModeModal)
                                {
                                    rdBtnServerEditShowModalMsg.Checked = true;
                                }
                                else
                                {
                                    rdBtnServerEditModalMsgAndExit.Checked = true;
                                }

                               
                                

                                
                                SimpleResponse logoutResponse = queryRunner.Logout();
                                if (logoutResponse.IsErroneous)
                                {
                                    // login failed! Use loginResponse.ErrorMessage and loginResponse.ErrorId to get the reason

                                    MessageBox.Show(loginResponse.ErrorId + " : " + loginResponse.ErrorMessage);
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("An Error Occured.\r\nPlease, Check Your Connection Information(s) !");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Information(s) missing !");
                    }
                }
                else
                {
                    MessageBox.Show("Please choose a server ID");
                }
            }
            catch
            {
                MessageBox.Show("An error occured !");
            }


        }

        private void btnServerEditApply_Click(object sender, EventArgs e)
        {
            if (txtBoxAddressIP.Text != "" && txtBoxPort.Text != "" && txtBoxQueryLogin.Text != "" && txtBoxQueryPassword.Text != "")
            {
                try
                {
                    string strLogin = txtBoxQueryLogin.Text;
                    string strPassword = txtBoxQueryPassword.Text;
                    string strAddress = txtBoxAddressIP.Text;
                    ushort uShortPort = ushort.Parse(txtBoxPort.Text);
                    ushort iPort = uShortPort;

                    using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher(strAddress, iPort)))
                    {
                        // login using the provided username and password and show a dump-output of the response in a textbox
                        SimpleResponse loginResponse = queryRunner.Login(strLogin, strPassword);

                        if (loginResponse.IsErroneous)
                        {
                            // login failed! Use loginResponse.ErrorMessage and loginResponse.ErrorId to get the reason

                            MessageBox.Show(loginResponse.ErrorMessage + "\r\n" + loginResponse.ErrorId);
                        }

                        int iSelectedID = lstBoxServerID.SelectedIndex;
                        iSelectedID++;

                        string SelectServerID = queryRunner.SelectVirtualServerById(Convert.ToUInt32(iSelectedID)).GetDumpString();
                        //MessageBox.Show(SelectServerID);
                        
                        VirtualServerModification ServEdit = new VirtualServerModification();

                        /*if (rdBtnServerEditAutoStartYes.Checked == true)
                        {
                            ServEdit.AutoStart = true;
                        }
                        else if (rdBtnServerEditAutoStartNo.Checked == true)
                        {
                            ServEdit.AutoStart = false;
                        }
                        else
                        {
                            ServEdit.AutoStart = true;
                        }*/

                        ServEdit.DefaultChannelAdminGroupId = Convert.ToUInt32(txtBoxServerEditStdGrpsDefChnlAdminGrp.Text);
                        ServEdit.DefaultChannelGroupId = Convert.ToUInt32(txtBoxServerEditStdGrpsDefChnlGrp.Text);
                        ServEdit.DefaultServerGroupId = Convert.ToUInt32(txtBoxServerEditStdGrpsDefServGrp.Text);
                        ServEdit.HostMessage = txtBoxServerEditHostMessage.Text;
                        ServEdit.IconId = Convert.ToUInt32(txtBoxServerEditVirtServIconID.Text);
                        ServEdit.MaxClients = Convert.ToInt32(txtBoxServerEditVirtServMaxClient.Text);
                        ServEdit.MinClientVersion = txtBoxServerEditVirtServMinClVer.Text;
                        ServEdit.MinClientsInChannelBeforeForcedSilence = Convert.ToInt32(txtBoxServerEditVirtServMinNbClInChnlToFrceSilce.Text);
                        ServEdit.Name = txtBoxServerEditVirtServName.Text;
                        ServEdit.Port = ushort.Parse(txtBoxServerVirtServEditPort.Text);
                        ServEdit.NeededIdentitySecurityLevel = Convert.ToInt32(txtBoxServerEditVirtServIdentSecuLvl.Text);
                        ServEdit.WelcomeMessage = txtBoxServerEditVirtServWelcMsg.Text;
                        if(rdBtnServerEditNoMessage.Checked == true)
                        {
                            ServEdit.HostMessageMode = HostMessageMode.HostMessageModeNone;
                        }
                        else if(rdBtnServerEditShowMsgInLog.Checked == true)
                        {
                            ServEdit.HostMessageMode = HostMessageMode.HostMessageModeLog;
                        }
                        else if(rdBtnServerEditShowModalMsg.Checked == true)
                        {
                            ServEdit.HostMessageMode = HostMessageMode.HostMessageModeModal;
                        }
                        else
                        {
                            ServEdit.HostMessageMode = HostMessageMode.HostMessageModeModalQuit;
                        }

                        queryRunner.EditServer(ServEdit);


                        SimpleResponse logoutResponse = queryRunner.Logout();
                        if (logoutResponse.IsErroneous)
                        {
                            // login failed! Use loginResponse.ErrorMessage and loginResponse.ErrorId to get the reason

                            MessageBox.Show(loginResponse.ErrorId + " : " + loginResponse.ErrorMessage);
                        }

                    }
                }
                catch
                {
                    MessageBox.Show("An Error Occured.\r\nPlease, Check Your Connection Information(s) !");
                }
            }
            else
            {
                MessageBox.Show("Information(s) missing !");
            }
        }

      

        
    }
}
