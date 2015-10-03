using System;
using System.Collections.Generic;
using System.IO;

namespace Hardly.Library.Twitch {
	public class TwitchIrcConnection {
		public readonly SqlTwitchBot bot;

		IrcClient ircClient;
		static LinkedList<TwitchChatRoom> chatRooms = new LinkedList<TwitchChatRoom>();
		bool whisperServer;

		public TwitchIrcConnection(SqlTwitchBot bot, bool whisperServer) {
			this.bot = bot;
			this.whisperServer = whisperServer;

			Reconnect();
		}

		#region Methods
		internal void Join(TwitchChatRoom room) {
			ircClient.WriteLine("JOIN #" + room.twitchConnection.channel.user.userName);
			chatRooms.AddLast(room);
		}

		internal void Run() {
			while(true) {
				try {
					while(true) {
						TwitchChatEvent chatEvent = GetNextChatEvent();
						RespondToEvent(chatEvent);
					}
				} catch(IOException connectionException) {
					Log.error("Irc connection issue: ", connectionException);
				}

				Thread.SleepInSeconds(30);
				Reconnect();
			}
		}

		internal void SendIrcMessage(string message) {
			try {
				ircClient.WriteLine(message);
			} catch(IOException connectionException) {
				Log.error("Irc connection issue: ", connectionException);
			}
		}

		internal void SendChat(SqlTwitchConnection twitchConnection, string message) {
			SendIrcMessage(":" + bot.user.userName
				 + "!" + bot.user.userName + "@"
				 + bot.user.userName
				 + ".tmi.twitch.tv PRIVMSG #" + twitchConnection.channel.user.userName + " :" + message);
		}

		internal void SendWhisper(SqlTwitchUser speakee, string message) {
			SendIrcMessage("PRIVMSG #jtv :/w " + speakee.userName + " " + message);
		}
		#endregion

		#region Helpers
		private string ReplaceVars(string message) {
			return message.Replace(TwitchChatEvent.Var_BotUsername, bot.user.userName);
		}

		private TwitchChatEvent GetNextChatEvent() {
			string chatEventCommand = ircClient.ReadNextLine_BLOCKING();
			if(chatEventCommand == null) {
				if(Reconnect()) {
					return GetNextChatEvent();
				} else {
					throw new Exception();
				}
			} else {
				return TwitchChatEvent.Parse(chatEventCommand);
			}
		}

		private bool Reconnect() {
			try {
				string ircServer = "irc.twitch.tv";
				if(whisperServer) {
					ircServer = "192.16.64.180";//"199.9.253.119";
				}
				ircClient = new IrcClient(ircServer, 6667, bot.user.userName, bot.oauthPassword);

				if(whisperServer) {
					// Enable whispers
					ircClient.WriteLine("CAP REQ :twitch.tv/commands");
				} else {
					// Enable join/part
					ircClient.WriteLine("CAP REQ :twitch.tv/membership");
				}
				return true;
			} catch(Exception e) {
				Log.exception(e);
				return false;
			} 
		}

		internal void RespondToEvent(TwitchChatEvent chatEvent) {
			Log.info(chatEvent.ToString());
			chatEvent.RespondToEvent(chatRooms);
		}
		#endregion
	}
}