using System;

namespace Hardly.Library.Twitch {
	public class TwitchChatRoom {
		readonly TwitchIrcConnection chatIrcConnection, whisperIrcConnection;
		public readonly TwitchConnection twitchConnection;
        public readonly ChannelPointManager pointManager;
        public readonly ITwitchFactory factory;

        public TwitchChatRoom(ITwitchFactory factory, TwitchIrcConnection chatConnection,
			 TwitchIrcConnection whisperConnection, TwitchConnection twitchConnection) {
            this.factory = factory;
			this.chatIrcConnection = chatConnection;
			this.whisperIrcConnection = whisperConnection;
			this.twitchConnection = twitchConnection;
            this.pointManager = new ChannelPointManager(factory, twitchConnection.channel);

			chatConnection.Join(this);

            TypeHelpers.InstantiateEachSubclass<IAutoJoinTwitchRooms, TwitchChatRoom>(this);
        }

		#region Methods
		internal void SendIrcMessage(string message) {
			chatIrcConnection.SendIrcMessage(message);
		}

		public void SendChatMessage(string message) {
			chatIrcConnection.SendChat(twitchConnection, message);
		}

		public void SendWhisper(TwitchUser speakee, string message) {
			whisperIrcConnection.SendWhisper(speakee, message);
		}

        public void Timeout(TwitchUser speaker, TimeSpan timeSpan) {
            if(timeSpan.TotalSeconds > 0) {
                SendChatMessage(".timeout " + speaker.userName + " " + timeSpan.TotalSeconds);
            }
        }
        #endregion
    }
}