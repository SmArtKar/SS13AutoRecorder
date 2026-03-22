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
		ServerData selectedServer;
		string seekerIP;

		public TrayMenu()
		{
			InitializeComponent();
			Input_ServerAPIType.DataSource = AutoRecorder.ServerAPIs.Keys.ToList();
            Input_ServerAPIType.SelectedIndexChanged += new EventHandler(Input_ServerAPIType_SelectedIndexChanged);
			List_Servers.DataSource = AutoRecorder.ServerListing;
			LoadSettingsData();
		}

		private void TrayMenu_Load(object sender, EventArgs e)
		{
			Timer seekerTimer = new Timer();
			seekerTimer.Interval = 1000;
			seekerTimer.Tick += new EventHandler(SeekerTimer_Tick);
			seekerTimer.Start();
			SeekerTimer_Tick(null, null);
		}

		private void SeekerTimer_Tick(object sender, EventArgs e)
		{
			seekerIP = AutoRecorder.GetDreamseekerIP();
			if (String.IsNullOrEmpty(seekerIP))
				Label_SeekerStatus.Text = "Offline";
			else
				Label_SeekerStatus.Text = String.Format("Connected to {0}", AutoRecorder.serverData.FirstOrDefault(x => x.MatchIP(seekerIP))?.Name ?? seekerIP);
		}

		private void ServerList_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedName = List_Servers.SelectedItem?.ToString();
			selectedServer = AutoRecorder.serverData.FirstOrDefault(x => x.Name == selectedName);
			LoadServerData();
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

		private void LoadSettingsData()
		{
			Input_OBSPort.Value = AutoRecorder.settings.ObsPort;
			Input_OBSPassword.Text = AutoRecorder.settings.ObsPassword;
			Input_OBSScene.Text = AutoRecorder.settings.ObsScene;
			Input_OBSDirectory.Text = AutoRecorder.settings.RecordingsFolder;
			Input_StopRecordingDelay.Value = AutoRecorder.settings.StopRecordingDelay;
			Input_UserAgent.Text = AutoRecorder.settings.UserAgent;
		}

		private void UpdateServerStatus()
		{
			if (selectedServer == null || String.IsNullOrEmpty(selectedServer?.ServerIP) || selectedServer?.ServerPort == 0 || selectedServer?.serverAPIType == null)
			{
				ApplyServerStatus(null);
				return;
			}

			ApplyServerStatus(selectedServer?.
				serverAPIType?
				.GetMethod("GetServerStatus").
				Invoke(null, [selectedServer?.ServerIP, selectedServer?.ServerPort])
				as ServerStatus?
			);
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
			int hours = (int) Math.Floor(status.roundDuration / 3600f);
			int minutes = (int) Math.Floor(status.roundDuration / 60f - hours * 60f);
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

			if (AutoRecorder.ServerListing.Contains(Input_ServerName.Text))
			{
				MessageBox.Show("A server with this name already exists!", "Error", MessageBoxButtons.OK);
				return;
			}

			selectedServer.Name = Input_ServerName.Text;
			List_Servers.DataSource = null;
			List_Servers.DataSource = AutoRecorder.ServerListing;
			List_Servers.SelectedIndex = AutoRecorder.serverData.IndexOf(selectedServer);
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
			UpdateServerStatus();
		}

		private void Button_AddServer_Click(object sender, EventArgs e)
		{
			ServerData newServer = new ServerData();
			
			int newIndex = 1; 
			while (AutoRecorder.serverData.Any(x => x.Name == String.Format("New Server {0}", newIndex))) newIndex++; 
			newServer.Name = String.Format("New Server {0}", newIndex);
			newServer.serverAPIType = typeof(ServerAPI_tg);

			AutoRecorder.serverData.Add(newServer);
			List_Servers.DataSource = null;
			List_Servers.DataSource = AutoRecorder.ServerListing;
			List_Servers.SelectedIndex = AutoRecorder.serverData.IndexOf(newServer);
			selectedServer = newServer;
			LoadServerData();
			UpdateServerStatus();
		}

		private void Button_DelServer_Click(object sender, EventArgs e)
		{
			if (selectedServer == null) 
				return;
			
			AutoRecorder.serverData.Remove(selectedServer);
			List_Servers.DataSource = null;
			List_Servers.DataSource = AutoRecorder.ServerListing;
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
			SeekerTimer_Tick(null, null);
        }

        private void Button_DeleteServerKeyword_Click(object sender, EventArgs e)
        {
			if (selectedServer == null || List_ServerKeywords.SelectedIndex == -1) 
				return;

			selectedServer.DreamseekerIPs.RemoveAt(List_ServerKeywords.SelectedIndex);
			List_ServerKeywords.DataSource = null;
			List_ServerKeywords.DataSource = selectedServer?.DreamseekerIPs;
			SeekerTimer_Tick(null, null);
        }

        private void Input_OBSPort_ValueChanged(object sender, EventArgs e)
        {
			AutoRecorder.settings.ObsPort = (int)Input_OBSPort.Value;
			AutoRecorder.ReconnectOBS();
        }

        private void Input_OBSPassword_TextChanged(object sender, EventArgs e)
        {
			AutoRecorder.settings.ObsPassword = Input_OBSPassword.Text;
			AutoRecorder.ReconnectOBS();
        }

        private void Input_OBSScene_TextChanged(object sender, EventArgs e)
        {
			AutoRecorder.settings.ObsScene = Input_OBSScene.Text;
			AutoRecorder.ChangeOBSScene();
        }

        private void Input_StopRecordingDelay_ValueChanged(object sender, EventArgs e)
        {
			AutoRecorder.settings.StopRecordingDelay = (int)Input_StopRecordingDelay.Value;
        }


        private void Input_UserAgent_TextChanged(object sender, EventArgs e)
        {
			AutoRecorder.settings.UserAgent = Input_UserAgent.Text;
        }

        private void Input_OBSDirectory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
			using (FolderBrowserDialog fbd = new FolderBrowserDialog())
			{
				fbd.SelectedPath = AutoRecorder.settings.RecordingsFolder;
				if (fbd.ShowDialog() == DialogResult.OK)
				{
					AutoRecorder.settings.RecordingsFolder = fbd.SelectedPath;
					Input_OBSDirectory.Text = AutoRecorder.settings.RecordingsFolder;
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
                Input_OBSDirectory.Text = AutoRecorder.settings.RecordingsFolder;
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
                Input_OBSDirectory.Text = AutoRecorder.settings.RecordingsFolder;
				return;
			}

            AutoRecorder.settings.RecordingsFolder = Input_OBSDirectory.Text;
        }
    }
}
