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

		/// <summary>Returns a list of all existing OBS scenes</summary>
		public static List<SceneBasicInfo> ListScenes => (obsSocket != null && obsSocket.IsConnected) ? obsSocket.ListScenes() : null;
		public static bool IsConnected => obsSocket != null && obsSocket.IsConnected;

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

            if (SettingsHandler.settings?.ObsPort > 0)
                ConnectOBS();
			else if (SettingsHandler.settings == null)
				SettingsHandler.OnSettingsLoaded += OnSettingsLoaded;
		}

		// Use event relays as the socket is not guaranteed to exist until initialization has been called for
        private static void OnConnected(object sender, EventArgs e) => Connected?.Invoke(sender, e);
        private static void OnDisconnected(object sender, ObsDisconnectionInfo e) => Disconnected?.Invoke(sender, e);
        private static void OnSceneListChanged(object sender, SceneListChangedEventArgs e) => SceneListChanged.Invoke(sender, e);
		private static void OnCurrentProgramSceneChanged(object sender, ProgramSceneChangedEventArgs e) => CurrentProgramSceneChanged?.Invoke(sender, e);

        private static void OnSettingsLoaded(object sender, EventArgs e)
		{
			if (SettingsHandler.settings.ObsPort > 0)
                ConnectOBS();
		}
		
		/// <summary>
		/// Attempt connection to an OBS websocket.
		/// Asynchronous.
		/// </summary>
		/// <param name="displayError">Should error response exceptions be thrown or silenced?</param>
		public static void ConnectOBS(bool displayError = true)
		{
			if (obsSocket == null)
				return;

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
		}

		public static void ChangeOBSScene(string newScene) => obsSocket.SetCurrentProgramScene(newScene);
	}
}
