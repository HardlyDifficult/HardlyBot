using System;
using System.Collections.Generic;

namespace Hardly.Library.Twitch {
	public class ChatCommand : TwitchCommandListener {
		static Dictionary<SqlTwitchChannel, List<ChatCommand>> roomCommands = new Dictionary<SqlTwitchChannel, List<ChatCommand>>();
		public readonly string commandName;
        public readonly string description;
        public readonly string[] aliases;
        public readonly bool modOnly, throttlePerUser;
        public bool enabled;
		Action<SqlTwitchUser, string> action;
		Throttle throttle;

        internal static List<ChatCommand> ForRoom(TwitchChatRoom room) {
            List<ChatCommand> commands;
            if(roomCommands.TryGetValue(room.twitchConnection.channel, out commands)) {
                return commands;
            }

            return null;
        }

        public static ChatCommand Create(TwitchChatRoom room, string name, Action<SqlTwitchUser, string> action, string description, string[] aliases, bool modOnly, TimeSpan timeToThrottleFor, bool throttlePerUser, bool enabled = true) {
			List<ChatCommand> commands;
			if(roomCommands.TryGetValue(room.twitchConnection.channel, out commands)) {
				//foreach(var command in commands) {
				//	if(command.commandName.Equals(name)) {
				//		return command;
				//	}
				//}
			} else {
				commands = new List<ChatCommand>(1);
                roomCommands.Add(room.twitchConnection.channel, commands);
			}

			// TODO, ensure no name/alias conflicts
			var newCommand = new ChatCommand(room, name, action, description, aliases, modOnly, timeToThrottleFor, throttlePerUser, enabled);
         commands.Add(newCommand);
			return newCommand;
		}

		ChatCommand(TwitchChatRoom room, string name, Action<SqlTwitchUser, string> action, string description, string[] aliases, bool modOnly, TimeSpan timeToThrottleFor, bool throttlePerUser, bool enabled) : base(room) {
			this.commandName = name;
			this.action = action;
			this.description = description;
			this.aliases = aliases;
			this.modOnly = modOnly;
			if(timeToThrottleFor > TimeSpan.FromSeconds(0)) {
				throttle = new Throttle(timeToThrottleFor); 
			} else {
				throttle = null;
			}
			this.throttlePerUser = throttlePerUser;

			this.enabled = enabled;
		}

		internal void Disable() {
			enabled = false;
		}

		internal void Enable() {
			enabled = true;
		}

		internal override void ObserveCommand(SqlTwitchUser speaker, string message) {
			if(enabled) {
				message = message?.Trim();
				string reqestedCommandName = message?.GetBefore(" ");
				if(reqestedCommandName == null) {
					reqestedCommandName = message;
				}

				if(reqestedCommandName != null) {
					bool match = reqestedCommandName.Equals(commandName, StringComparison.CurrentCultureIgnoreCase);
					if(!match) {
						if(aliases != null) {
							foreach(var alias in aliases) {
								if(reqestedCommandName.Equals(alias, StringComparison.CurrentCultureIgnoreCase)) {
									match = true;
									break;
								}
							}
						}
					}

					if(match) {
						if(!modOnly || speaker.id.Equals(room.twitchConnection.channel.user.id)) {
							ulong id = 0;
							if(throttlePerUser) {
								id = speaker.id;
							}
							if(throttle == null || throttle.ExecuteIfReady(id)) {
								string additionalText = message.GetAfter(" ")?.Trim();
								action(speaker, additionalText);
							} else {
								room.SendWhisper(speaker, "Too soon... wait at least " + throttle.TimeRemaining(id).ToSimpleString() + " before running !" + commandName + " again.");
							}
						} else {
							room.SendWhisper(speaker, "Sorry, !" + commandName + " is for mods only.");
						}
					}
				}
			}
		}
	}
}
