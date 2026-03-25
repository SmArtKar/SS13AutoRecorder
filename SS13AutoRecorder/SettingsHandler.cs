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
			try
			{
				using (FileStream storedServers = new FileStream(Application.LocalUserAppDataPath + "\\userServers.json", FileMode.OpenOrCreate))
				{
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
			}
			catch (IOException e)
			{
				AutoRecorder.ErrorHandle(e, "Unable to read or create the server data file: ");
				Application.ExitThread();
			}
		}

		public static void WriteServerData()
		{
			try
			{
				using (FileStream storedServers = new FileStream(Application.LocalUserAppDataPath + "\\userServers.json", FileMode.Create)) 
                {
                    string serversJSON = JsonSerializer.Serialize(serverData);
                    byte[] buffer = new UTF8Encoding(true).GetBytes(serversJSON);
                    storedServers.Write(buffer, 0, buffer.Length);
                    storedServers.Close();
				}
			}
			catch (IOException e)
			{
				AutoRecorder.ErrorHandle(e, "Unable to save the server data file: ");
				Application.ExitThread();
			}		}

		public static void ReadSettingsData()
		{
			try
			{
				using (FileStream storedSettings = new FileStream(Application.LocalUserAppDataPath + "\\settings.json", FileMode.OpenOrCreate))
				{
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
			}
			catch (IOException e)
			{
				AutoRecorder.ErrorHandle(e, "Unable to read or create the server data file: ");
				Application.ExitThread();
			}
		}

		public static void WriteSettingsData()
		{
			try
			{
				using (FileStream storedSettings = new FileStream(Application.LocalUserAppDataPath + "\\settings.json", FileMode.Create))
				{
                    string settingsJSON = JsonSerializer.Serialize(settings);
                    byte[] buffer = new UTF8Encoding(true).GetBytes(settingsJSON);
                    storedSettings.Write(buffer, 0, buffer.Length);
                    storedSettings.Close();
				}
			}
			catch (IOException e)
			{
				AutoRecorder.ErrorHandle(e, "Unable to save the server data file: ");
				Application.ExitThread();
			}
		}
	}
}
