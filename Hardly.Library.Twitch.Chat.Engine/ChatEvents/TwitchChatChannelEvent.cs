using System;

namespace Hardly.Library.Twitch {
	public abstract class TwitchChatChannelEvent : TwitchChatEvent {
		public readonly TwitchChannel channel;

		public TwitchChatChannelEvent(TwitchChannel channel) {
			this.channel = channel;
		}
	}
}
