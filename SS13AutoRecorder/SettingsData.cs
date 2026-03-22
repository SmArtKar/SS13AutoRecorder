using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS13AutoRecorder
{
	[Serializable]
	internal class SettingsData
	{
		/// <summary>Port of the OBS websocket server</summary>
		public int ObsPort { get; set; } = 0;
		/// <summary>Access password to the OBS websocket</summary>
		public string ObsPassword { get; set; } = string.Empty;
		/// <summary>Name of the scene in the OBS to switch to whenever recording a round</summary>
		public string ObsScene { get; set; } = "SS13";
		/// <summary>Folder path where all the recordings are saved</summary>
		public string RecordingsFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\SS13";
		/// <summary>Delay before the recording is cut after dreakseeker connection is shut</summary>
		public int StopRecordingDelay { get; set; } = 95;
		/// <summary>User agent passed to REST APIs, such as BeeStation's</summary>
		public string UserAgent { get; set; } = "SS13 AutoRecorder";
	}
}
