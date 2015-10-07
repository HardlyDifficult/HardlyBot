using System;
using System.Collections.Generic;

namespace Hardly.Library.Twitch {
	public class TwitchChatMessage : TwitchChatChannelEvent {
		static Action<TwitchChatRoom, TwitchUser, string>[] observers = new Action<TwitchChatRoom, TwitchUser, string>[0];
		public readonly TwitchUser speaker;
		public readonly string message;

		public TwitchChatMessage(TwitchChannel channel, TwitchUser speaker, string message)
			 : base(channel) {
			this.speaker = speaker;
			this.message = message?.Trim();
		}

		internal static void RegisterObserver(Action<TwitchChatRoom, TwitchUser, string> observer) {
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

		static void Observe(TwitchChatRoom room, TwitchUser speaker, string message) {
			foreach(var observer in observers) {
				observer(room, speaker, message);
			}
		}
	}
}
