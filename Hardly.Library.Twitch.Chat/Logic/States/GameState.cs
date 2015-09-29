using System;

namespace Hardly.Library.Twitch {
    public abstract class GameState {
        internal abstract void Close();
        internal abstract void Open();
    }

	public abstract class GameState<GameLogicController> : GameState {
		protected GameLogicController controller;
		List<ChatCommand> commands = new List<ChatCommand>();

		public GameState(GameLogicController controller) {
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
		
		internal override void Close() {
			foreach(var command in commands) {
				command.Disable();
			}
		}

		internal override void Open() {
			foreach(var command in commands) {
				command.Enable();
			}
		}
	}
}
