using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Management;

namespace SS13AutoRecorder
{
	internal static class AutoRecorder
	{		
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

		private static TrayMenu mainMenu;

		[STAThread]
		static void Main()
		{
			Task.Run(() =>
			{
				SettingsHandler.ReadServerData();
				SettingsHandler.ReadSettingsData();
			});
			OBSHandler.InitializeSocket();
			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(mainMenu = new TrayMenu());
		}
		
		/// <summary>
		/// Searches for any active DreamSeeker processes connected to a server and returns the first match's IP address
		/// </summary>
		/// <returns>IP address of the connected server</returns>
		internal static string GetDreamseekerIP()
		{
			foreach (Process process in Process.GetProcessesByName("dreamseeker"))
			{
				try
				{
					string[] processArgs = process.GetCommandLine().Split(["\" \""], StringSplitOptions.None);
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

		private static string GetCommandLine(this Process process)
		{
			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
			using (ManagementObjectCollection objects = searcher.Get())
			{
				return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"].ToString();
			}

		}

		private static void OnApplicationExit(object sender, EventArgs e)
		{
			SettingsHandler.WriteServerData();
			SettingsHandler.WriteSettingsData();
			if (mainMenu != null)
			{
				mainMenu.StopRecording(mainMenu.lastServer?.Name, discard: SettingsHandler.settings.DiscardOnQuit);
				mainMenu = null;
			}
		}
		
		/// <summary>
		/// Creates a popup with exception info, for centralized handling.
		/// </summary>
		/// <param name="exception">Exception to fetch text info from</param>
		/// <param name="error">Error text to display. Nullable, will default to unhandled exception</param>
		public static void ErrorHandle(Exception exception, string error = null, MessageBoxIcon icon = MessageBoxIcon.Error)
		{
			MessageBox.Show((error ?? "An unhandled exception has occured: ") + exception?.ToString(), "Error", MessageBoxButtons.OK, icon);
		}
	}
}
