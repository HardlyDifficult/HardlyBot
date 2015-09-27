using System;
using System.Collections.Generic;

namespace Hardly.Library.Twitch {
	public static class PointManager {
		static Dictionary<SqlTwitchChannel, ChannelPointManager> channelManagers = new Dictionary<SqlTwitchChannel, ChannelPointManager>();
		
		public static ChannelPointManager ForChannel(SqlTwitchChannel channel) {
			ChannelPointManager manager;
			if(!channelManagers.TryGetValue(channel, out manager)) {
				manager = new ChannelPointManager(channel);
				channelManagers.Add(channel, manager);
			}

			return manager;
		}
	}
}
