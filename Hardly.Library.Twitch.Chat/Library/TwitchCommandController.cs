using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch {
	public abstract class TwitchCommandController {
		internal TwitchChatRoom room;
		internal ChannelPointManager pointManager;

		public TwitchCommandController(TwitchChatRoom room) {
			this.room = room;
			pointManager = PointManager.ForChannel(room.twitchConnection.channel);
		}
	}
}
