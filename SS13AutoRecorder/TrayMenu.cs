using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types;
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
using System.Runtime.InteropServices;

namespace SS13AutoRecorder
{
    public partial class TrayMenu : Form
    {
        private static List<string> ServerListing => SettingsHandler.serverData?.Select(x => x.Name).ToList();
        private static List<string> SceneListing => OBSHandler.ListScenes?.Select(x => x.Name).ToList();

        private ServerData selectedServer;
        private string seekerIP;
        private DateTime? lastSeenSeeker;
        private int previousRoundID = -1;
        internal ServerStatus? lastStatus;
        internal ServerData lastServer;

        private readonly NotifyIcon trayIcon = new NotifyIcon();
        private readonly ContextMenuStrip trayMenu = new ContextMenuStrip();

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
            OBSHandler.RecordStateChanged += UpdateOBSStatus;
        }

        private void TrayMenu_Load(object sender, EventArgs e)
        {
            trayIcon.Icon = this.Icon;
            trayIcon.Visible = true;
            trayIcon.Text = this.Text;
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.DoubleClick += (s, _) =>
            {
                Show();
                WindowState = FormWindowState.Normal;
                Activate();
            };

            trayMenu.Items.Add("OBS: Offline", null, OnTrayOBSClick);
            trayMenu.Items.Add("DreamSeeker: Offline", null, null);
            trayMenu.Items.Add("Exit", null, ExitFromTray);

            UpdateSeekerStatus();
            UpdateOBSStatus();
            // Need to call it here as its async and will fail if the handle is not loaded yet
            if (selectedServer != null)
                UpdateServerStatus();

            Timer seekerLoop = new Timer();
            seekerLoop.Interval = 1000;
            seekerLoop.Tick += new EventHandler(UpdateSeekerStatus);
            seekerLoop.Start();

            Timer recordingLoop = new Timer();
            recordingLoop.Interval = 5000;
            recordingLoop.Tick += new EventHandler(ProcessRecorder);
            recordingLoop.Start();
        }

        private void OnTrayOBSClick(object sender, EventArgs e)
        {
            if (OBSHandler.RecordState?.IsActive() ?? false)
                OBSHandler.TogglePause();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                base.OnFormClosing(e);
                return;
            }

            e.Cancel = true;
            BeginInvoke((Action)(() => WindowState = FormWindowState.Minimized));
        }

        private void ExitFromTray(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
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
            string connectionLabel = "Offline";
            seekerIP = AutoRecorder.GetDreamseekerIP();
            if (!String.IsNullOrEmpty(seekerIP))
            {
                connectionLabel = String.Format("Connected to {0}", SettingsHandler.serverData.FirstOrDefault(x => x.MatchIP(seekerIP))?.Name ?? seekerIP);
                lastSeenSeeker = DateTime.Now;
            }

            Label_SeekerStatus.Text = connectionLabel;
            trayMenu.Items[1].Text = "DreamSeeker: " + connectionLabel;
        }

        private void ProcessRecorder(object sender, EventArgs e)
        {
            if (!OBSHandler.IsConnected || !OBSHandler.RecordState.HasValue)
                return;

            ServerData connectedServer = seekerIP != null ? SettingsHandler.serverData.FirstOrDefault(x => x.MatchIP(seekerIP)) : null;

            ServerStatus? maybeStatus = connectedServer?.
                serverAPIType?
                .GetMethod("GetServerStatus").
                Invoke(null, [connectedServer?.ServerIP, connectedServer?.ServerPort])
                as ServerStatus?;

            if (connectedServer == null || maybeStatus == null)
            {
                OutputState? state = OBSHandler.RecordState;
                bool wasRecording = state?.IsActive(includeStarting: true) ?? false;
                if (!wasRecording || previousRoundID == -1 || lastSeenSeeker == null)
                    return;

                if (DateTime.Now.Subtract(lastSeenSeeker.Value).TotalMilliseconds < SettingsHandler.settings.StopRecordingDelay * 100)
                    return;

                StopRecording(lastServer?.Name);
                previousRoundID = -1;
                lastSeenSeeker = null;
                return;
            }

            // Safety for the status fetch delay
            if (!OBSHandler.IsConnected || !OBSHandler.RecordState.HasValue)
                return;

            ServerStatus status = maybeStatus.Value;
            OutputState? initialState = OBSHandler.RecordState;
            bool isRecording = initialState?.IsActive() ?? false;
            if (isRecording && status.gamestate == Gamestate.Roundend)
            {
                lastStatus = status;
                // TODO: Log roundend
                StopRecording(lastServer?.Name);
                previousRoundID = -1;
                // Async these out to give OBS a bit of time to think
                _ = Task.Run(async () =>
                {
                    await Task.Delay(200);
                    initialState = OBSHandler.RecordState;
                    if (initialState?.IsActive() ?? false)
                        AutoRecorder.ErrorHandle(null, "Failed to stop recording upon roundend!");
                });
                return;
            }

            // Local server/invalid packet, do not start recording
            if (status.roundID == -1)
            {
                previousRoundID = -1;
                return;
            }

            if (previousRoundID == -1)
            {
                // TODO: Log round detection
                previousRoundID = status.roundID;
                lastServer = connectedServer;
                OBSHandler.ChangeOBSScene(SettingsHandler.settings.ObsScene);
                if (isRecording)
                    return;

                if (initialState == OutputState.OBS_WEBSOCKET_OUTPUT_PAUSED)
                    OBSHandler.TogglePause();
                else if (initialState == OutputState.OBS_WEBSOCKET_OUTPUT_STOPPED)
                    OBSHandler.ToggleRecording();

                _ = Task.Run(async () =>
                {
                    await Task.Delay(200);
                    OutputState? newState = OBSHandler.RecordState;
                    if (newState != OutputState.OBS_WEBSOCKET_OUTPUT_STARTING && newState != OutputState.OBS_WEBSOCKET_OUTPUT_STARTED)
                        AutoRecorder.ErrorHandle(null, "Failed to start recording!");
                });
                return;
            }

            if (previousRoundID == status.roundID)
                return;

            // TODO: Log round change
            if (isRecording)
            {
                // TODO: Log (missing) roundend
                StopRecording(lastServer?.Name);
                // Wait a bit before trying to continue to record, then check the connection again
                System.Threading.Thread.Sleep(200);
                if (!OBSHandler.IsConnected || !OBSHandler.RecordState.HasValue)
                    return;

                OutputState? preRecordState = OBSHandler.RecordState;
                if (preRecordState?.IsActive() ?? false)
                {
                    AutoRecorder.ErrorHandle(null, "Failed to stop recording upon round change!");
                    return;
                }

            }

            lastServer = connectedServer;
            previousRoundID = status.roundID;

            OBSHandler.ChangeOBSScene(SettingsHandler.settings.ObsScene);
            OBSHandler.ToggleRecording();
            _ = Task.Run(async () =>
            {
                await Task.Delay(200);
                OutputState? recordingState = OBSHandler.RecordState;
                if (recordingState != OutputState.OBS_WEBSOCKET_OUTPUT_STARTING && recordingState != OutputState.OBS_WEBSOCKET_OUTPUT_STARTED)
                    AutoRecorder.ErrorHandle(null, "Failed to start recording!");
                // Pause again if we were paused before
                else if (initialState == OutputState.OBS_WEBSOCKET_OUTPUT_PAUSED)
                    OBSHandler.TogglePause();
            });
        }

        public void StopRecording(string server = null, bool discard = false)
        {
            string savedPath = OBSHandler.EndRecording();

            if (savedPath == null || !File.Exists(savedPath))
                return;

            if (lastStatus == null || lastStatus?.roundID == -1 || discard)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await WaitUntilFree(savedPath);
                        File.Delete(savedPath);
                    }
                    catch (Exception ex)
                    {
                        AutoRecorder.ErrorHandle(ex, String.Format("Failed to delete recording {0}: ", savedPath));
                    }
                });
                return;
            }

            string extension = Path.GetExtension(savedPath);
            ServerStatus status = lastStatus.Value;
            server ??= status.version;
            string filename;
            // Valid RIDs get round written in, invalid ones only get the map
            if (status.roundID > 0)
                filename = String.Format("{0} - Round {1} on {2}", server, status.roundID, status.mapName);
            else
                filename = String.Format("{0} - {1}", server, status.mapName);

            string saveName = filename;
            int attempt = 1;
            while (File.Exists(SettingsHandler.settings.RecordingsFolder + "\\" + filename + extension))
                if (status.roundID > 0)
                    saveName = String.Format("{0} (Part {1})", filename, attempt);
                else
                    saveName = String.Format("{0}, Recording {1}", filename, attempt);

            // Get rid of invalid symbols like slashes in /tg/station
            saveName = string.Concat(saveName.Split(Path.GetInvalidFileNameChars()));

            DirectoryInfo targetDir = Directory.CreateDirectory(SettingsHandler.settings.RecordingsFolder);
            Task.Run(async () =>
            {
                try
                {
                    await WaitUntilFree(savedPath);
                    File.Move(savedPath, SettingsHandler.settings.RecordingsFolder + "\\" + saveName + extension);
                }
                catch (Exception ex)
                {
                    AutoRecorder.ErrorHandle(
                        ex, 
                        String.Format("Failed to move recording {0} to {1}: ", 
                        savedPath, 
                        SettingsHandler.settings.RecordingsFolder + "\\" + saveName + extension
                    ));
                }
            });
        }

        private async Task WaitUntilFree(string filepath)
        {
            bool continueAccessing = true;
            int attemptNum = 1;
            while (continueAccessing)
            {
                try
                {
                    using (FileStream testFileStream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        if (testFileStream != null)
                        {
                            testFileStream.Close();
                            continueAccessing = false;
                        }
                }
                catch (IOException ex)
                {
                    const int ERROR_SHARING_VIOLATION = 32;
                    const int ERROR_LOCK_VIOLATION = 33;

                    int errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
                    // Non-file lock errors get thrown out
                    if (errorCode != ERROR_SHARING_VIOLATION && errorCode != ERROR_LOCK_VIOLATION)
                        throw;

                    // Try for a minute, crash afterwards
                    if (attemptNum > 300)
                    {
                        continueAccessing = false;
                        continue;
                    }

                    attemptNum++;
                    await Task.Delay(200);
                }
            }
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

        public void UpdateOBSStatus(object sender = null, RecordStateChangedEventArgs e = null)
        {
            if (!IsHandleCreated)
                return;

            if (!OBSHandler.IsConnected)
            {
                Invoke((MethodInvoker)(() => {
                    Label_OBSStatus.Text = "Offline";
                    Button_Stop.Visible = false;
                    Button_Discard.Visible = false;
                    Button_Pause.Visible = false;
                }));
                return;
            }

            string state = "Connected, Unknown";
            bool displayButtons = false;
            switch (OBSHandler.RecordState)
            {
                case OutputState.OBS_WEBSOCKET_OUTPUT_STARTING:
                    state = "Starting recording...";
                    displayButtons = true;
                    break;

                case OutputState.OBS_WEBSOCKET_OUTPUT_STARTED:
                case OutputState.OBS_WEBSOCKET_OUTPUT_RESUMED:
                    state = "Recording";
                    displayButtons = true;
                    break;

                case OutputState.OBS_WEBSOCKET_OUTPUT_STOPPING:
                    state = "Stopping...";
                    break;

                case OutputState.OBS_WEBSOCKET_OUTPUT_STOPPED:
                    state = "Connected";
                    break;

                case OutputState.OBS_WEBSOCKET_OUTPUT_PAUSED:
                    state = "Recording paused";
                    displayButtons = true;
                    break;
            }

            Invoke((MethodInvoker)(() =>
            {
                Label_OBSStatus.Text = state;
                Button_Pause.Text = OBSHandler.RecordState == OutputState.OBS_WEBSOCKET_OUTPUT_PAUSED ? "Unpause" : "Pause";
                Button_Stop.Visible = displayButtons;
                Button_Discard.Visible = displayButtons;
                Button_Pause.Visible = displayButtons;
                trayMenu.Items[0].Text = "OBS: " + state;
            }));
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
            Input_DiscardOnExit.Checked = SettingsHandler.settings.DiscardOnQuit;
        }

        private void UpdateServerStatus()
        {
            if (selectedServer == null || String.IsNullOrEmpty(selectedServer?.ServerIP) || selectedServer?.ServerPort == 0 || selectedServer?.serverAPIType == null)
            {
                ApplyServerStatus(null);
                return;
            }

            Task.Run(() =>
            {
                ServerStatus? status = selectedServer?.
                    serverAPIType?
                    .GetMethod("GetServerStatus").
                    Invoke(null, [selectedServer?.ServerIP, selectedServer?.ServerPort])
                    as ServerStatus?;

                BeginInvoke((MethodInvoker)(() =>
                {
                    ApplyServerStatus(status);
                }));
            });
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
    }
}
