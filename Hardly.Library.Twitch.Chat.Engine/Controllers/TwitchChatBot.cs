using System;

namespace Hardly.Library.Twitch {
	public class TwitchChatBot {
		public static TwitchChatRoom[] rooms {
			get;
			private set;
		}
        ITwitchFactory factory;

        public TwitchChatBot(ITwitchFactory factory) {
            this.factory = factory;
        }
		
		public void Run() {
			Thread[] ircThreads = null;
			rooms = null;

            var bots = factory.GetAllBots();
			foreach(var bot in bots) {
				var connections = factory.GetAllAutoConnectingConnections(bot);
				if(connections != null && connections.Length > 0) {
					var chatConnection = new TwitchIrcConnection(factory, bot, false);
					var whisperConnection = new TwitchIrcConnection(factory, bot, true);

					foreach(var connection in connections) {
						var room = new TwitchChatRoom(factory, chatConnection, whisperConnection, connection);
						rooms = rooms.Append(room);
					}

					ircThreads = ircThreads.Append(new[] {
						new Thread(whisperConnection.Run),
						new Thread(chatConnection.Run)
						});
				}
			}

			ircThreads.Run();
		}
	}
}