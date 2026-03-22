using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS13AutoRecorder
{
	internal struct ServerStatus
	{
		public int roundID;
		public Gamestate gamestate;
		public string mapName;
		public int roundDuration;
		public string version;
		public int players;
	} 

	internal enum Gamestate
	{
		Startup = 0,
		Pregame = 1,
		SettingUp = 2,
		Playing = 3,
		Roundend = 4,
	} 
}
