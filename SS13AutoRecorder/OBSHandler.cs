using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types;
using OBSWebsocketDotNet.Types.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS13AutoRecorder
{
	internal static class OBSHandler
	{
		private static OBSWebsocket obsSocket;

		/// <summary>An OBS websocket connection has been established</summary>
		public static event EventHandler Connected;
		/// <summary>Active OBS websocket connection has been lost</summary>
		public static event EventHandler<ObsDisconnectionInfo> Disconnected;
		/// <summary>The list of scenes has changed</summary>
        public static event EventHandler<SceneListChangedEventArgs> SceneListChanged;
		/// <summary>The currently active scene has changed</summary>
        public static event EventHandler<ProgramSceneChangedEventArgs> CurrentProgramSceneChanged;
		/// <summary>The recording state has changed</summary>
        public static event EventHandler<RecordStateChangedEventArgs> RecordStateChanged;

		/// <summary>Returns a list of all existing OBS scenes</summary>
		public static List<SceneBasicInfo> ListScenes => IsConnected ? obsSocket.ListScenes() : null;
		public static bool IsConnected => obsSocket?.IsConnected ?? false;
		public static OutputState? RecordState => IsConnected ? _recordState : null;
		private static OutputState? _recordState = null;
		public static string RecordDirectory => IsConnected ? obsSocket.GetRecordDirectory() : null; 

		/// <summary>
		/// Initializes an OBS websocket connection from the settings data, or signs up for latter's event if it is not loaded yet
		/// </summary>
		public static void InitializeSocket()
		{
			obsSocket = new OBSWebsocket();

			obsSocket.Connected += OnConnected;
			obsSocket.Disconnected += OnDisconnected;
			obsSocket.SceneListChanged += OnSceneListChanged;
			obsSocket.CurrentProgramSceneChanged += OnCurrentProgramSceneChanged;
			obsSocket.RecordStateChanged += OnRecordStateChanged;

            if (SettingsHandler.settings?.ObsPort > 0)
                ConnectOBS();
			else if (SettingsHandler.settings == null)
				SettingsHandler.OnSettingsLoaded += OnSettingsLoaded;
		}

		private static void OnConnected(object sender, EventArgs e)
		{
			RecordingStatus status = obsSocket.GetRecordStatus();
			if (status == null)
				_recordState = null;
			else if (status.IsRecordingPaused)
				_recordState = OutputState.OBS_WEBSOCKET_OUTPUT_PAUSED;
			else if (status.IsRecording)
				_recordState = OutputState.OBS_WEBSOCKET_OUTPUT_STARTED;
			else
				_recordState = OutputState.OBS_WEBSOCKET_OUTPUT_STOPPED;

            Connected?.Invoke(sender, e);
		}

		// Use event relays as the socket is not guaranteed to exist until initialization has been called for
        private static void OnDisconnected(object sender, ObsDisconnectionInfo e) => Disconnected?.Invoke(sender, e);
        private static void OnSceneListChanged(object sender, SceneListChangedEventArgs e) => SceneListChanged.Invoke(sender, e);
		private static void OnCurrentProgramSceneChanged(object sender, ProgramSceneChangedEventArgs e) => CurrentProgramSceneChanged?.Invoke(sender, e);
		
        private static void OnRecordStateChanged(object sender, RecordStateChangedEventArgs e)
        {
			_recordState = e.OutputState.State;
			RecordStateChanged?.Invoke(sender, e);
        }

        private static void OnSettingsLoaded(object sender, EventArgs e)
		{
			if (SettingsHandler.settings.ObsPort > 0)
                ConnectOBS();
		}

		public static void ToggleRecording() => obsSocket.ToggleRecord();
		public static void TogglePause() => obsSocket.ToggleRecordPause();

		/// <summary>
		/// End an active recording
		/// </summary>
		/// <returns>Filepath for the recording, or null if there was no active recording</returns>
		public static string EndRecording() => (
                RecordState == OutputState.OBS_WEBSOCKET_OUTPUT_STARTING || 
                RecordState == OutputState.OBS_WEBSOCKET_OUTPUT_STARTED || 
                RecordState == OutputState.OBS_WEBSOCKET_OUTPUT_RESUMED || 
                RecordState == OutputState.OBS_WEBSOCKET_OUTPUT_PAUSED
			) ? obsSocket.StopRecord() : null;
		
		/// <summary>
		/// Attempt connection to an OBS websocket.
		/// Asynchronous.
		/// </summary>
		/// <param name="displayError">Should error response exceptions be thrown or silenced?</param>
		/// <returns>true if connection was successful, false if it has failed.</returns>
		public static bool ConnectOBS(bool displayError = true)
		{
			if (obsSocket == null)
				return false;

			Task.Run(() =>
			{
				try
				{
					obsSocket.ConnectAsync("ws://localhost:" + SettingsHandler.settings.ObsPort, SettingsHandler.settings.ObsPassword);
				}
				catch (ErrorResponseException ere)
				{
					if (displayError)
						AutoRecorder.ErrorHandle(ere, "Failed to connect to the OBS websocket: ", System.Windows.Forms.MessageBoxIcon.Exclamation);
				}
				catch (Exception e)
				{
					AutoRecorder.ErrorHandle(e);
				}
			});

			return obsSocket.IsConnected;
		}

		public static void ChangeOBSScene(string newScene) => obsSocket.SetCurrentProgramScene(newScene);

		public static bool IsActive(this OutputState outputState, bool includeStarting = false) 
		{
			if (includeStarting && outputState == OutputState.OBS_WEBSOCKET_OUTPUT_STARTING)
				return true;
			return outputState == OutputState.OBS_WEBSOCKET_OUTPUT_STARTED || outputState == OutputState.OBS_WEBSOCKET_OUTPUT_PAUSED || outputState == OutputState.OBS_WEBSOCKET_OUTPUT_RESUMED;
		}
	}
}
