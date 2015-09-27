using System;

namespace Hardly.Library.Twitch {
	public class TwitchChatRoom {
		readonly TwitchIrcConnection chatIrcConnection, whisperIrcConnection;
		public readonly SqlTwitchConnection twitchConnection;
		public readonly TwitchCommandController[] commandControllers;
        public readonly ChannelPointManager pointManager;

        public TwitchChatRoom(TwitchIrcConnection chatConnection,
			 TwitchIrcConnection whisperConnection, SqlTwitchConnection twitchConnection) {
			this.chatIrcConnection = chatConnection;
			this.whisperIrcConnection = whisperConnection;
			this.twitchConnection = twitchConnection;
            this.pointManager = new ChannelPointManager(twitchConnection.channel);

			chatConnection.Join(this);

			commandControllers = GetType().InstantiateEachSubclass<TwitchCommandController, TwitchChatRoom>(false, this);
		}

		#region Methods
		internal void SendIrcMessage(string message) {
			chatIrcConnection.SendIrcMessage(message);
		}

		public void SendChatMessage(string message) {
			chatIrcConnection.SendChat(twitchConnection, message);
		}

		public void SendWhisper(SqlTwitchUser speakee, string message) {
			whisperIrcConnection.SendWhisper(speakee, message);
		}
		#endregion
	}
}