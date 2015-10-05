using System.Collections.Generic;
using System;

namespace Hardly.Library.Twitch {
	public abstract class TwitchChatEvent {
		const string Var_Prefix = "```";
		internal const string Var_BotUsername = Var_Prefix + "BotUsername";

		internal static TwitchChatEvent Parse(string chatEventCommand) {
			string command;

            // TODO, support Mod events -- e.g. :jtv MODE #hardlysober +o arbedii

            if(chatEventCommand.StartsWith("PING")) {
				command = "PING";
			} else {
				command = chatEventCommand.GetBetween(" ", " ");
			}

			if(command.Equals("PRIVMSG")) {
				SqlTwitchChannel channel = ParseChannel(chatEventCommand);
				SqlTwitchUser user = ParseUser(chatEventCommand);
				string message = chatEventCommand.GetAfter("#" + channel.user.userName + " :");

				return new TwitchChatMessage(channel, user, message);
			} else if(command.Equals("WHISPER")) {
				SqlTwitchUser user = ParseUser(chatEventCommand);
				string message = chatEventCommand.GetAfter(" :");

				return new TwitchChatWhisper(user, message);
			} else if(command.Equals("PING")) {
				return new TwitchChatPing();
			} else {
				return new TwitchChatUnknownEvent(chatEventCommand);
			}
		}

		#region Helpers
		private static SqlTwitchUser ParseUser(string chatEventCommand) {
			return SqlTwitchUser.GetFromName(chatEventCommand.GetBetween(":", "!"));
		}

		private static SqlTwitchChannel ParseChannel(string chatEventCommand) {
			return new SqlTwitchChannel(SqlTwitchUser.GetFromName(chatEventCommand.GetBetween(" #", " ")));
		}

		internal abstract void RespondToEvent(LinkedList<TwitchChatRoom> chatRooms);
		#endregion
	}
}
