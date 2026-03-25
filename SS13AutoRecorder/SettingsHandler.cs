using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS13AutoRecorder
{
	internal static class SettingsHandler
	{
		/// <summary>List of saved servers</summary>
		public static List<ServerData> serverData;
		/// <summary>User settings such as OBS data and user agent</summary>
		public static SettingsData settings;
		/// <summary>Event invoked when server data finishes loading</summary>
		public static event EventHandler OnServersLoaded;
		/// <summary>Event invoked when settings data finishes loading</summary>
		public static event EventHandler OnSettingsLoaded;

		public static void ReadServerData()
		{
			// TODO: Convert to using
			FileStream storedServers = null;
			try
			{
				storedServers = new FileStream(Application.LocalUserAppDataPath + "\\userServers.json", FileMode.OpenOrCreate);
			}
			catch (IOException e)
			{
				AutoRecorder.ErrorHandle(e, "Unable to read or create the server data file: ");
				Application.ExitThread();
			}

			string serversJSON = string.Empty;
			byte[] buffer = new byte[1024];
			UTF8Encoding encoding = new UTF8Encoding(true);
			int bytesRead = 0;
			while ((bytesRead = storedServers.Read(buffer, 0, buffer.Length)) > 0)
			{
				serversJSON += encoding.GetString(buffer, 0, bytesRead);
			}

			if (serversJSON.Length > 0)
				serverData = JsonSerializer.Deserialize<List<ServerData>>(serversJSON);
			else
				serverData = new List<ServerData>();
			storedServers.Close();
			OnServersLoaded?.Invoke(typeof(SettingsHandler), EventArgs.Empty);
			OnServersLoaded = null;
		}

		public static void WriteServerData()
		{
			FileStream storedServers = null;
			try
			{
				storedServers = new FileStream(Application.LocalUserAppDataPath + "\\userServers.json", FileMode.Create);
			}
			catch (IOException e)
			{
				AutoRecorder.ErrorHandle(e, "Unable to save the server data file: ");
				Application.ExitThread();
			}

			string serversJSON = JsonSerializer.Serialize(serverData);
			byte[] buffer = new UTF8Encoding(true).GetBytes(serversJSON);
			storedServers.Write(buffer, 0, buffer.Length);
			storedServers.Close();
		}

		public static void ReadSettingsData()
		{
			FileStream storedSettings = null;
			try
			{
				storedSettings = new FileStream(Application.LocalUserAppDataPath + "\\settings.json", FileMode.OpenOrCreate);
			}
			catch (IOException e)
			{
				AutoRecorder.ErrorHandle(e, "Unable to read or create the server data file: ");
				Application.ExitThread();
			}

			string settingsJSON = string.Empty;
			byte[] buffer = new byte[1024];
			UTF8Encoding encoding = new UTF8Encoding(true);
			int bytesRead = 0;
			while ((bytesRead = storedSettings.Read(buffer, 0, buffer.Length)) > 0)
			{
				settingsJSON += encoding.GetString(buffer, 0, bytesRead);
			}

			if (settingsJSON.Length > 0)
				settings = JsonSerializer.Deserialize<SettingsData>(settingsJSON);
			else
				settings = new SettingsData();
			storedSettings.Close();
			OnSettingsLoaded?.Invoke(typeof(SettingsHandler), EventArgs.Empty);
			OnSettingsLoaded = null;
		}

		public static void WriteSettingsData()
		{
			FileStream storedSettings = null;
			try
			{
				storedSettings = new FileStream(Application.LocalUserAppDataPath + "\\settings.json", FileMode.Create);
			}
			catch (IOException e)
			{
				AutoRecorder.ErrorHandle(e, "Unable to save the server data file: ");
				Application.ExitThread();
			}

			string settingsJSON = JsonSerializer.Serialize(settings);
			byte[] buffer = new UTF8Encoding(true).GetBytes(settingsJSON);
			storedSettings.Write(buffer, 0, buffer.Length);
			storedSettings.Close();
		}

	}
}
