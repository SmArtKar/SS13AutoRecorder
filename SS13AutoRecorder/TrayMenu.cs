using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types.Events;
using SS13AutoRecorder.ServerAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private static List<string> ServerListing => SettingsHandler.serverData?.Select(x => x.Name).ToList();
        private static List<string> SceneListing => OBSHandler.ListScenes?.Select(x => x.Name).ToList();

        private ServerData selectedServer;
        private string seekerIP;

        public TrayMenu()
        {
            InitializeComponent();
            Input_ServerAPIType.DataSource = AutoRecorder.ServerAPIs.Keys.ToList();
            Input_ServerAPIType.SelectedIndexChanged += new EventHandler(Input_ServerAPIType_SelectedIndexChanged);

            if (SettingsHandler.serverData != null)
                List_Servers.DataSource = ServerListing;
            else
                SettingsHandler.OnServersLoaded += AssignServerListing;

            if (SettingsHandler.settings != null)
                LoadSettingsData();
            else
                SettingsHandler.OnSettingsLoaded += LoadSettingsData;

            OBSHandler.Connected += OnOBSConnected;
            OBSHandler.Disconnected += OnOBSDisconnected;
            OBSHandler.SceneListChanged += OnOBSScenesChanged;
        }

        private void TrayMenu_Load(object sender, EventArgs e)
        {
            Timer seekerTimer = new Timer();
            seekerTimer.Interval = 1000;
            seekerTimer.Tick += new EventHandler(UpdateSeekerStatus);
            seekerTimer.Start();
            UpdateSeekerStatus();
            UpdateOBSStatus();
            // Need to call it here as its async and will fail if the handle is not loaded yet
            if (selectedServer != null)
                UpdateServerStatus();
        }

        private void AssignServerListing(object sender, EventArgs e)
        {
            List_Servers.DataSource = null;
            List_Servers.DataSource = ServerListing;
            string selectedName = List_Servers.SelectedItem?.ToString();
            selectedServer = SettingsHandler.serverData.FirstOrDefault(x => x.Name == selectedName);
            LoadServerData();
            if (IsHandleCreated)
                UpdateServerStatus();
        }

        public void UpdateSeekerStatus(object sender = null, EventArgs e = null)
        {
            seekerIP = AutoRecorder.GetDreamseekerIP();
            if (String.IsNullOrEmpty(seekerIP))
                Label_SeekerStatus.Text = "Offline";
            else
                Label_SeekerStatus.Text = String.Format("Connected to {0}", SettingsHandler.serverData.FirstOrDefault(x => x.MatchIP(seekerIP))?.Name ?? seekerIP);
        }

        private void OnOBSConnected(object sender, EventArgs e)
        {
            UpdateOBSStatus();
            Input_OBSScene.Invoke((MethodInvoker)(() =>
            {
                // Racecon safety
                if (!OBSHandler.IsConnected)
                    return;
                // Safe change without triggering index update
                Input_OBSScene.SelectedIndexChanged -= Input_OBSScene_SelectedIndexChanged;
                Input_OBSScene.DataSource = SceneListing;
                Input_OBSScene.SelectedIndex = SceneListing?.IndexOf(SettingsHandler.settings.ObsScene) ?? -1;
                Input_OBSScene.SelectedIndexChanged += Input_OBSScene_SelectedIndexChanged;
            }));
        }

        private void OnOBSDisconnected(object sender, ObsDisconnectionInfo e)
        {
            UpdateOBSStatus();
            // TODO: Possible race conditions here? May want to put this into the queue instead of an IsConnected check
            Input_OBSScene.Invoke((MethodInvoker)(() => { if (!OBSHandler.IsConnected) Input_OBSScene.DataSource = null; }));
       }

        public void UpdateOBSStatus(object sender = null, EventArgs e = null)
        {
            if (!IsHandleCreated)
                return;

            if (OBSHandler.IsConnected)
                Label_OBSStatus.Invoke((MethodInvoker)(() => Label_OBSStatus.Text = "Connected"));
            else
                Label_OBSStatus.Invoke((MethodInvoker)(() => Label_OBSStatus.Text = "Offline"));
        }

        private void OnOBSScenesChanged(object sender, SceneListChangedEventArgs e)
        {
            Input_OBSScene.DataSource = SceneListing;
        }

        private void ServerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = List_Servers.SelectedItem?.ToString();
            selectedServer = SettingsHandler.serverData.FirstOrDefault(x => x.Name == selectedName);
            LoadServerData();
            if (IsHandleCreated)
                UpdateServerStatus();
        }

        private void LoadServerData()
        {
            Input_ServerName.Text = selectedServer?.Name;
            Input_ServerIP.Text = selectedServer?.ServerIP;
            Input_ServerPort.Text = selectedServer?.ServerPort.ToString();
            Input_ServerAPIType.SelectedIndex = AutoRecorder.ServerAPIs.Keys.ToList().IndexOf((string)selectedServer?.serverAPIType?.GetMethod("APIName").Invoke(null, null));
            List_ServerKeywords.DataSource = selectedServer?.DreamseekerIPs;
        }

        private void LoadSettingsData(object sender = null, EventArgs e = null)
        {
            Input_OBSPort.Value = SettingsHandler.settings.ObsPort;
            Input_OBSPassword.Text = SettingsHandler.settings.ObsPassword;
            // Safe change without triggering index update
            Input_OBSScene.SelectedIndexChanged -= Input_OBSScene_SelectedIndexChanged;
            Input_OBSScene.SelectedIndex = SceneListing?.IndexOf(SettingsHandler.settings.ObsScene) ?? -1;
            Input_OBSScene.SelectedIndexChanged += Input_OBSScene_SelectedIndexChanged;

            Input_OBSDirectory.Text = SettingsHandler.settings.RecordingsFolder;
            Input_StopRecordingDelay.Value = SettingsHandler.settings.StopRecordingDelay;
            Input_UserAgent.Text = SettingsHandler.settings.UserAgent;
        }

        private void UpdateServerStatus()
        {
            if (selectedServer == null || String.IsNullOrEmpty(selectedServer?.ServerIP) || selectedServer?.ServerPort == 0 || selectedServer?.serverAPIType == null)
            {
                ApplyServerStatus(null);
                return;
            }

            BeginInvoke((MethodInvoker)(() =>
            {
                ServerStatus? status = selectedServer?.
                    serverAPIType?
                    .GetMethod("GetServerStatus").
                    Invoke(null, [selectedServer?.ServerIP, selectedServer?.ServerPort])
                    as ServerStatus?;
                ApplyServerStatus(status);
            }));
        }

        private void ApplyServerStatus(ServerStatus? possibleStatus)
        {
            if (possibleStatus == null)
            {
                Label_ServerStatus.Text = "Unreachable";

                _Text_RoundID.Visible = false;
                _Text_GameState.Visible = false;
                _Text_MapName.Visible = false;
                _Text_RoundDuration.Visible = false;

                Label_RoundID.Visible = false;
                Label_GameState.Visible = false;
                Label_MapName.Visible = false;
                Label_RoundDuration.Visible = false;
                return;
            }

            _Text_RoundID.Visible = true;
            _Text_GameState.Visible = true;
            _Text_MapName.Visible = true;
            _Text_RoundDuration.Visible = true;

            Label_RoundID.Visible = true;
            Label_GameState.Visible = true;
            Label_MapName.Visible = true;
            Label_RoundDuration.Visible = true;

            ServerStatus status = possibleStatus.Value;

            Label_ServerStatus.Text = "Online";
            Label_RoundID.Text = status.roundID.ToString();

            switch (status.gamestate)
            {
                case Gamestate.Startup:
                    Label_GameState.Text = "Startup";
                    break;

                case Gamestate.Pregame:
                    Label_GameState.Text = "Pre-round";
                    break;

                case Gamestate.SettingUp:
                    Label_GameState.Text = "Setting Up";
                    break;

                case Gamestate.Playing:
                    Label_GameState.Text = "Round In Progress";
                    break;

                case Gamestate.Roundend:
                    Label_GameState.Text = "Roundend";
                    break;
            }

            Label_MapName.Text = status.mapName;
            int hours = (int)Math.Floor(status.roundDuration / 3600f);
            int minutes = (int)Math.Floor(status.roundDuration / 60f - hours * 60f);
            if (hours > 0)
                Label_RoundDuration.Text = String.Format("{0}h, {1}m", hours, minutes);
            else
                Label_RoundDuration.Text = String.Format("{0}m", minutes);
        }

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
    }
}
