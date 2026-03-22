using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Management;
using System.IO;
using System.Text.Json;
using System.Text;

namespace SS13AutoRecorder
{
	internal static class AutoRecorder
	{
		/// <summary>List of saved servers</summary>
		public static List<ServerData> serverData;
		/// <summary>User settings such as OBS data and user agent</summary>
		public static SettingsData settings;
		
		private static Dictionary<string, Type> _serverAPICache;
		/// <summary>Dictionary of API Type objects per key</summary>
		public static Dictionary<string, Type> ServerAPIs => _serverAPICache ??= typeof(AutoRecorder).Assembly.GetTypes()
			.Where(x =>
            {
                Type derived = x;
                do
                {
                    derived = derived.BaseType;
                } while (derived != null && derived != typeof(ServerAPI.ServerAPI));
                return derived == typeof(ServerAPI.ServerAPI);
            })
			.Where(x => x.GetMethod("APIName").Invoke(null, null) as string != string.Empty)
			.ToDictionary(x => x.GetMethod("APIName").Invoke(null, null) as string, x => x);
		
		/// <summary>List of server names to be used as a data source.</summary>
		public static List<string> ServerListing => serverData?.Select(x => x.Name).ToList();

		private static string GetCommandLine(this Process process)
		{
			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
			using (ManagementObjectCollection objects = searcher.Get())
			{
				return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"].ToString();
			}

		}

		[STAThread]
		static void Main()
		{
			ReadServerData();
			ReadSettingsData();
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new TrayMenu());
		}
		
		/// <summary>
		/// Searches for any active dreamseeker processes connected to a server and returns the first match's IP address
		/// </summary>
		/// <returns>IP address of the connected server</returns>
		internal static string GetDreamseekerIP()
		{
			foreach (Process process in Process.GetProcessesByName("dreamseeker"))
			{
				try
				{
					string[] processArgs = process.GetCommandLine().Split(new[] { "\" \"" }, StringSplitOptions.None);
					if (processArgs.Length >= 2)
						return processArgs[1].Split('\"')[0];
				}
				catch (Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
				{
					// Intentionally empty - no security access to the process.
				}
				catch (InvalidOperationException)
				{
					// Intentionally empty - the process exited before getting details.
				}
			}

			return null;
		}

		public static void ReadServerData()
		{
			FileStream storedServers = null;
			try
			{
				storedServers = new FileStream(Application.LocalUserAppDataPath + "\\userServers.json", FileMode.OpenOrCreate);
			}
			catch (IOException e)
			{
				ErrorHandle(e, "Unable to read or create the server data file: ");
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
				ErrorHandle(e, "Unable to save the server data file: ");
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
				ErrorHandle(e, "Unable to read or create the server data file: ");
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
				ErrorHandle(e, "Unable to save the server data file: ");
				Application.ExitThread();
			}

			string settingsJSON = JsonSerializer.Serialize(settings);
			byte[] buffer = new UTF8Encoding(true).GetBytes(settingsJSON);
			storedSettings.Write(buffer, 0, buffer.Length);
			storedSettings.Close();
		}

		private static void OnApplicationExit(object sender, EventArgs e)
		{
			WriteServerData();
			WriteSettingsData();
		}

		public static void ErrorHandle(Exception exception, string error = null)
		{
            MessageBox.Show((error ?? "An unhandled exception has occured: ") + exception.ToString());
		}

		public static void ReconnectOBS()
		{

		}

		public static void ChangeOBSScene()
		{

		}
	}
}
