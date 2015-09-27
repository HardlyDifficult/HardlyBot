using System;

namespace Hardly.Library.Twitch {
	public class TwitchChatBot {
		public static TwitchChatRoom[] rooms {
			get;
			private set;
		}
		
		public void Run() {
			Thread[] ircThreads = null;
			rooms = null;

			var bots = SqlTwitchBot.GetAll();
			foreach(var bot in bots) {
				var connections = SqlTwitchConnection.GetAllAutoConnectingConnections(bot);
				if(connections != null && connections.Length > 0) {
					var chatConnection = new TwitchIrcConnection(bot, false);
					var whisperConnection = new TwitchIrcConnection(bot, true);

					foreach(var connection in connections) {
						var room = new TwitchChatRoom(chatConnection, whisperConnection, connection);
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