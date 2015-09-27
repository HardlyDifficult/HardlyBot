namespace Hardly.Library.Twitch {
	public abstract class TwitchCommandListener {
		internal TwitchChatRoom room;

      public TwitchCommandListener(TwitchChatRoom room) {
			this.room = room;
			TwitchChatMessage.RegisterObserver(this.ObserveChatMessage);
		}

		public void ObserveChatMessage(TwitchChatRoom room, SqlTwitchUser speaker, string message) {
			if(this.room.Equals(room)) {
				if(message != null && message.StartsWith("!")) {
					ObserveCommand(speaker, message.Substring(1));
				}
			}
		}

		//public void ObserveWhisperMessage(TwitchChatRoom room, SqlTwitchUser speaker, string message) {
		//	if(this.room.Equals(room)) {
		//		ObserveCommand(speaker, message);
		//	}
		//}

		internal abstract void ObserveCommand(SqlTwitchUser speaker, string message);
	}
}
