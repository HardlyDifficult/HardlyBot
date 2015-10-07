using System;
using System.Collections.Generic;

namespace Hardly.Library.Twitch {
	public class TwitchChatWhisper : TwitchChatEvent {
		static Action<TwitchChatRoom, TwitchUser, string>[] observers = new Action<TwitchChatRoom, TwitchUser, string>[0];
		readonly TwitchUser speaker;
		readonly string message;

		public TwitchChatWhisper(TwitchUser speaker, string message) {
			this.speaker = speaker;
			this.message = message;
		}

		public override string ToString() {
			return "*" + speaker.userName + "*: " + message;
		}

		internal static void RegisterObserver(Action<TwitchChatRoom, TwitchUser, string> observer) {
			observers = observers.Append(observer);
		}

		internal override void RespondToEvent(LinkedList<TwitchChatRoom> chatRooms) {
			Observe(chatRooms.First.Value, speaker, message);
		}

		void Observe(TwitchChatRoom room, TwitchUser speaker, string message) {
			foreach(var observer in observers) {
				observer(room, speaker, message);
			}
		}
	}
}
