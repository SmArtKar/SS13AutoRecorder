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
	internal class ServerAPI_goon : ServerAPI
	{
		public new static string APIName() => "GoonStation";

		public new static ServerStatus? GetServerStatus(string address, int port)
		{
			try
			{
				string topicResponse = Topic(address, port, "?status");
				NameValueCollection parsedTopic = HttpUtility.ParseQueryString(topicResponse);
				Dictionary<string, string> response = parsedTopic.AllKeys.Where(x => x != null).ToDictionary(k => k, k => parsedTopic[k]);
				Gamestate gamestate = Gamestate.Startup;
				int roundDuration = -1;
				if (int.TryParse(response["elapsed"], out roundDuration))
					gamestate = Gamestate.Playing;
				else
				{
					switch (response["elapsed"])
					{
						case "pre":
							gamestate = Gamestate.Pregame;
							break;
						case "post":
							gamestate = Gamestate.Roundend;
							break;
						case "welp":
							throw new BadServerResponseException();
					}
				}

				return new ServerStatus()
				{
					roundID = 0, // Goons do not respond with their round ID, but -1 hints at a local server
					gamestate = gamestate,
					mapName = response["map_name"],
					roundDuration = roundDuration,
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
