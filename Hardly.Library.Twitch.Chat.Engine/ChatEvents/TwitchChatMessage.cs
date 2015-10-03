using System;
using System.Collections.Generic;

namespace Hardly.Library.Twitch {
	public class TwitchChatMessage : TwitchChatChannelEvent {
		static Action<TwitchChatRoom, SqlTwitchUser, string>[] observers = new Action<TwitchChatRoom, SqlTwitchUser, string>[0];
		public readonly SqlTwitchUser speaker;
		public readonly string message;

		public TwitchChatMessage(SqlTwitchChannel channel, SqlTwitchUser speaker, string message)
			 : base(channel) {
			this.speaker = speaker;
			this.message = message?.Trim();
		}

		internal static void RegisterObserver(Action<TwitchChatRoom, SqlTwitchUser, string> observer) {
			observers = observers.Append(observer);
		}

		public override string ToString() {
			return "[" + channel + "] " + speaker + ": " + message;
		}

		internal override void RespondToEvent(LinkedList<TwitchChatRoom> chatRooms) {
			foreach(TwitchChatRoom room in chatRooms) {
				if(room.twitchConnection.channel.user.userName.Equals(channel.user.userName)) {
					Observe(room, speaker, message);

					break;
				}
			}
		}

		static void Observe(TwitchChatRoom room, SqlTwitchUser speaker, string message) {
			foreach(var observer in observers) {
				observer(room, speaker, message);
			}
		}
	}
}
