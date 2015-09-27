using System.Collections.Generic;

namespace Hardly.Library.Twitch {
	public class TwitchChatPing : TwitchChatEvent {
		internal override void RespondToEvent(LinkedList<TwitchChatRoom> chatRooms) {
			chatRooms.First.Value.SendIrcMessage("PONG  tmi.twitch.tv");
		}
	}
}
