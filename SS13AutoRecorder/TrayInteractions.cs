using OBSWebsocketDotNet.Types;
using SS13AutoRecorder.ServerAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS13AutoRecorder
{
    public partial class TrayMenu : Form
    {
        private void Input_ServerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Input_ServerName_Submit(sender, e);
        }

        private void Input_ServerName_Submit(object sender, EventArgs e)
        {
            if (selectedServer == null)
            {
                Input_ServerName.Text = string.Empty;
                return;
            }

            if (selectedServer.Name == Input_ServerName.Text)
                return;

            if (ServerListing.Contains(Input_ServerName.Text))
            {
                MessageBox.Show("A server with this name already exists!", "Error", MessageBoxButtons.OK);
                return;
            }

            selectedServer.Name = Input_ServerName.Text;
            List_Servers.DataSource = null;
            List_Servers.DataSource = ServerListing;
            List_Servers.SelectedIndex = SettingsHandler.serverData.IndexOf(selectedServer);
        }

        private void Input_ServerIP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Input_ServerIP_Submit(sender, e);
        }

        private void Input_ServerIP_Submit(object sender, EventArgs e)
        {
            if (selectedServer == null)
            {
                Input_ServerIP.Text = string.Empty;
                return;
            }

            selectedServer.ServerIP = Input_ServerIP.Text;
            UpdateServerStatus();
        }

        private void Input_ServerPort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Input_ServerPort_Submit(sender, e);
        }

        private void Input_ServerPort_Submit(object sender, EventArgs e)
        {
            if (selectedServer == null)
            {
                Input_ServerPort.Value = 0;
                return;
            }

            selectedServer.ServerPort = (int)Input_ServerPort.Value;
            UpdateServerStatus();
        }

        private void Input_ServerAPIType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedServer == null)
            {
                Input_ServerAPIType.SelectedIndex = 0;
                return;
            }

            selectedServer.serverAPIType = AutoRecorder.ServerAPIs[AutoRecorder.ServerAPIs.Keys.ToList()[Input_ServerAPIType.SelectedIndex]];
            if (IsHandleCreated)
                UpdateServerStatus();
        }

        private void Button_AddServer_Click(object sender, EventArgs e)
        {
            ServerData newServer = new ServerData();

            int newIndex = 1;
            while (SettingsHandler.serverData.Any(x => x.Name == String.Format("New Server {0}", newIndex))) newIndex++;
            newServer.Name = String.Format("New Server {0}", newIndex);
            newServer.serverAPIType = typeof(ServerAPI_tg);

            SettingsHandler.serverData.Add(newServer);
            List_Servers.DataSource = null;
            List_Servers.DataSource = ServerListing;
            List_Servers.SelectedIndex = SettingsHandler.serverData.IndexOf(newServer);
            selectedServer = newServer;
            LoadServerData();
            UpdateServerStatus();
        }

        private void Button_DelServer_Click(object sender, EventArgs e)
        {
            if (selectedServer == null)
                return;

            SettingsHandler.serverData.Remove(selectedServer);
            List_Servers.DataSource = null;
            List_Servers.DataSource = ServerListing;
            List_Servers.SelectedIndex = -1;
            selectedServer = null;
        }

        private void Button_AddServerKeyword_Click(object sender, EventArgs e)
        {
            if (selectedServer == null || Input_ServerKeyword.Text == string.Empty || selectedServer.DreamseekerIPs.Contains(Input_ServerKeyword.Text))
                return;
            selectedServer.DreamseekerIPs.Add(Input_ServerKeyword.Text);
            List_ServerKeywords.DataSource = null;
            List_ServerKeywords.DataSource = selectedServer?.DreamseekerIPs;
            UpdateSeekerStatus();
        }

        private void Button_DeleteServerKeyword_Click(object sender, EventArgs e)
        {
            if (selectedServer == null || List_ServerKeywords.SelectedIndex == -1)
                return;

            selectedServer.DreamseekerIPs.RemoveAt(List_ServerKeywords.SelectedIndex);
            List_ServerKeywords.DataSource = null;
            List_ServerKeywords.DataSource = selectedServer?.DreamseekerIPs;
            UpdateSeekerStatus();
        }

        private void Input_OBSPort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Input_OBSPort_Submit(sender, e);
        }

        private void Input_OBSPort_Submit(object sender, EventArgs e)
        {
            if (SettingsHandler.settings.ObsPort == Input_OBSPort.Value)
                return;

            SettingsHandler.settings.ObsPort = (int)Input_OBSPort.Value;
            OBSHandler.ConnectOBS();
        }

        private void Input_OBSPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Input_OBSPassword_Submit(sender, e);
        }

        private void Input_OBSPassword_Submit(object sender, EventArgs e)
        {
            if (SettingsHandler.settings.ObsPassword == Input_OBSPassword.Text)
                return;

            SettingsHandler.settings.ObsPassword = Input_OBSPassword.Text;
            OBSHandler.ConnectOBS();
        }

        private void Input_StopRecordingDelay_ValueChanged(object sender, EventArgs e)
        {
            SettingsHandler.settings.StopRecordingDelay = (int)Input_StopRecordingDelay.Value;
        }

        private void Input_UserAgent_TextChanged(object sender, EventArgs e)
        {
            SettingsHandler.settings.UserAgent = Input_UserAgent.Text;
        }

        private void Input_OBSDirectory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = SettingsHandler.settings.RecordingsFolder;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SettingsHandler.settings.RecordingsFolder = fbd.SelectedPath;
                    Input_OBSDirectory.Text = SettingsHandler.settings.RecordingsFolder;
                }
            }
        }

        private void Input_OBSDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Input_OBSDirectory_Submit(sender, e);
        }

        private void Input_OBSDirectory_Submit(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Input_OBSDirectory.Text))
            {
                Input_OBSDirectory.Text = SettingsHandler.settings.RecordingsFolder;
                return;
            }

            // Easy way to check for path validity without it needing to exist
            try
            {
                Path.GetFullPath(Input_OBSDirectory.Text);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is SecurityException || ex is PathTooLongException || ex is NotSupportedException)
                    MessageBox.Show(String.Format("Invalid recordings directory: {0}", Input_OBSDirectory.Text), "Error", MessageBoxButtons.OK);
                else
                    AutoRecorder.ErrorHandle(ex);
                Input_OBSDirectory.Text = SettingsHandler.settings.RecordingsFolder;
                return;
            }

            SettingsHandler.settings.RecordingsFolder = Input_OBSDirectory.Text;
        }

        private void Input_OBSScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Input_OBSScene.DataSource == null)
                return;

            string sceneName = Input_OBSScene.SelectedItem as string;
            if (sceneName == SettingsHandler.settings.ObsScene || sceneName == string.Empty)
                return;

            SettingsHandler.settings.ObsScene = sceneName;
            OBSHandler.ChangeOBSScene(sceneName);
        }

        private void Input_DiscardOnExit_CheckedChanged(object sender, EventArgs e)
        {
            SettingsHandler.settings.DiscardOnQuit = Input_DiscardOnExit.Checked;
        }

        private void Button_Stop_Click(object sender, EventArgs e)
        {
            if (!OBSHandler.IsConnected)
                return;

            OutputState? status = OBSHandler.RecordState;
            if (status?.IsActive() ?? false)
                StopRecording(lastServer?.Name);
        }

        private void Button_Pause_Click(object sender, EventArgs e)
        {
            if (!OBSHandler.IsConnected)
                return;

            OutputState? status = OBSHandler.RecordState;
            if (status?.IsActive() ?? false)
                OBSHandler.TogglePause();
        }

        private void Button_Discard_Click(object sender, EventArgs e)
        {
            if (!OBSHandler.IsConnected)
                return;

            OutputState? status = OBSHandler.RecordState;
            if (status?.IsActive() ?? false)
                StopRecording(lastServer?.Name, discard: true);
        }

    }
}
