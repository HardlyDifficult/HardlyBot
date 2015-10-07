using System.Collections.Generic;
using System;

namespace Hardly.Library.Twitch {
	public abstract class TwitchChatEvent {
		const string Var_Prefix = "```";
		internal const string Var_BotUsername = Var_Prefix + "BotUsername";

		internal static TwitchChatEvent Parse(ITwitchFactory factory, string chatEventCommand) {
			string command;

            // TODO, support Mod events -- e.g. :jtv MODE #hardlysober +o arbedii

            if(chatEventCommand.StartsWith("PING")) {
				command = "PING";
			} else {
				command = chatEventCommand.GetBetween(" ", " ");
			}

			if(command.Equals("PRIVMSG")) {
				TwitchChannel channel = ParseChannel(factory, chatEventCommand);
				TwitchUser user = ParseUser(factory, chatEventCommand);
				string message = chatEventCommand.GetAfter("#" + channel.user.userName + " :");

				return new TwitchChatMessage(channel, user, message);
			} else if(command.Equals("WHISPER")) {
				TwitchUser user = ParseUser(factory, chatEventCommand);
				string message = chatEventCommand.GetAfter(" :");

				return new TwitchChatWhisper(user, message);
			} else if(command.Equals("PING")) {
				return new TwitchChatPing();
			} else {
				return new TwitchChatUnknownEvent(chatEventCommand);
			}
		}

		#region Helpers
		private static TwitchUser ParseUser(ITwitchFactory factory, string chatEventCommand) {
			return factory.GetUserFromName(chatEventCommand.GetBetween(":", "!"));
		}

		private static TwitchChannel ParseChannel(ITwitchFactory factory, string chatEventCommand) {
			return factory.GetChannel(factory.GetUserFromName(chatEventCommand.GetBetween(" #", " ")));
		}

		internal abstract void RespondToEvent(LinkedList<TwitchChatRoom> chatRooms);
		#endregion
	}
}
