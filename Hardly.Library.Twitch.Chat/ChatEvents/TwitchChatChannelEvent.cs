using System;

namespace Hardly.Library.Twitch {
	public abstract class TwitchChatChannelEvent : TwitchChatEvent {
		public readonly SqlTwitchChannel channel;

		public TwitchChatChannelEvent(SqlTwitchChannel channel) {
			this.channel = channel;
		}
	}
}
