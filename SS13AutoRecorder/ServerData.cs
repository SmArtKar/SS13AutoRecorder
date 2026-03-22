using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS13AutoRecorder
{
	[Serializable]
	internal class ServerData
	{
		/// <summary>Name of the server we're using</summary>
		public string Name { get; set; }
		/// <summary>ServerAPI child which will resolve our status API requests</summary>
		public Type serverAPIType;
		/// <summary>Wrapper for serverAPIType for serialization</summary>
		public string ServerAPIType_String
		{
			get => AutoRecorder.ServerAPIs.First(x => x.Value == serverAPIType).Key;
			set => serverAPIType = AutoRecorder.ServerAPIs[value];
		}
		/// <summary>Address we'll be polling the server at</summary>
		public string ServerIP { get; set; }
		/// <summary>Port we'll be polling the server at</summary>
		public int ServerPort { get; set; }
		/// <summary>List of keywords to look in the dreamseeker address besides the actual IP</summary>
		public List<string> DreamseekerIPs { get; set; }

		public ServerData()
		{
			DreamseekerIPs = new List<string>();
		}

		/// <summary>Check if target IP matches data's own IP:port or contains any of the keyword patterns</summary>
		public bool MatchIP (string ip)
		{
			if (ip.StartsWith("byond://"))
				ip = ip.Remove(0, 8);

			if (ip.Last() == '/')
				ip = ip.Substring(0, ip.Length - 1);

			if (ip.Equals(String.Format("{0}:{1}", ServerIP, ServerPort)))
				return true;

			return DreamseekerIPs?.Any(x => ip.Contains(x)) ?? false;
		}
	}
}
