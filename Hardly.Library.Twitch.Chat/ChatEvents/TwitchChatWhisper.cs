using System;
using System.Collections.Generic;

namespace Hardly.Library.Twitch {
	public class TwitchChatWhisper : TwitchChatEvent {
		static Action<TwitchChatRoom, SqlTwitchUser, string>[] observers = new Action<TwitchChatRoom, SqlTwitchUser, string>[0];
		readonly SqlTwitchUser speaker;
		readonly string message;

		public TwitchChatWhisper(SqlTwitchUser speaker, string message) {
			this.speaker = speaker;
			this.message = message;
		}

		public override string ToString() {
			return "*" + speaker.userName + "*: " + message;
		}

		internal static void RegisterObserver(Action<TwitchChatRoom, SqlTwitchUser, string> observer) {
			observers = observers.Append(observer);
		}

		internal override void RespondToEvent(LinkedList<TwitchChatRoom> chatRooms) {
			Observe(chatRooms.First.Value, speaker, message);
		}

		void Observe(TwitchChatRoom room, SqlTwitchUser speaker, string message) {
			foreach(var observer in observers) {
				observer(room, speaker, message);
			}
		}
	}
}
