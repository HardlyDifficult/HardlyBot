using System;
using System.Collections.Generic;

namespace Hardly.Library.Twitch {
	public abstract class GameState<TwitchGameController> {
		protected TwitchGameController controller;
		List<ChatCommand> commands = new List<ChatCommand>();

		public GameState(TwitchGameController controller) {
			this.controller = controller;
		}

		protected void AddCommand(TwitchChatRoom room, string name, Action<SqlTwitchUser, string> commandAction, string description, string[] aliases, bool modOnly, TimeSpan throttleTimeSpan, bool throttlePerUser) {
			ChatCommand command = ChatCommand.Create(room, name, commandAction, description, aliases, modOnly, throttleTimeSpan, throttlePerUser, true);
			if(command != null) {
				if(!commands.Contains(command)) {
					commands.Add(command);
				}
			}
		}
		
		internal virtual void Close() {
			foreach(var command in commands) {
				command.Disable();
			}
		}

		internal virtual void Open() {
			foreach(var command in commands) {
				command.Enable();
			}
		}
	}
}
