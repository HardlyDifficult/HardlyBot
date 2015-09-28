using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch {
    /// <summary>
    /// Every concrete instance of this class is automatically Constructed given each Chat room we connect to (via TwitchChatRoom)
    /// </summary>
	public abstract class TwitchCommandController : IAutoJoinTwitchRooms {
		internal TwitchChatRoom room;

		public TwitchCommandController(TwitchChatRoom room) {
			this.room = room;
		}
	}
}
