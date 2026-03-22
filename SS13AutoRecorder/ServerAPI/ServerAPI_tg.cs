using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace SS13AutoRecorder.ServerAPI
{
	internal class ServerAPI_tg : ServerAPI
	{
		public new static string APIName() => "/tg/station";

		public new static ServerStatus? GetServerStatus(string address, int port)
		{
			try
			{
				string topicResponse = Topic(address, port, "?status");
				NameValueCollection parsedTopic = HttpUtility.ParseQueryString(topicResponse);
				Dictionary<string, string> response = parsedTopic.AllKeys.Where(x => x != null).ToDictionary(k => k, k => parsedTopic[k]);
				return new ServerStatus()
				{
					roundID = int.Parse(response["round_id"]),
					gamestate = (Gamestate)int.Parse(response["gamestate"]),
					mapName = response["map_name"],
					roundDuration = int.Parse(response["round_duration"]),
					version = response["version"],
					players = int.Parse(response["players"]),
				};
			}
			catch (BadServerResponseException bse)
			{
				AutoRecorder.ErrorHandle(bse, String.Format("The server at {0}:{1} has responded with a bad package: ", address, port));
			}
			catch (Exception e)
			{
				AutoRecorder.ErrorHandle(e);
			}
			return null;
		}
	}
}
