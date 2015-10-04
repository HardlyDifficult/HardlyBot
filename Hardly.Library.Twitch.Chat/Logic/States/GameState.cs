using System;

namespace Hardly.Library.Twitch {
    public abstract class GameState {
        public abstract void Close();

        /// <summary>
        /// This is called right after the constructor.  
        /// Anything that could change state must be in this method and NOT the constructor.
        /// </summary>
        internal abstract void Open();
    }

	public abstract class GameState<GameLogicController> : GameState {
		protected GameLogicController controller;
		List<ChatCommand> commands = new List<ChatCommand>();

		public GameState(GameLogicController controller) {
			this.controller = controller;
		}

		protected ChatCommand AddCommand(TwitchChatRoom room, string name, Action<SqlTwitchUser, string> commandAction, string description, string[] aliases, bool modOnly, TimeSpan throttleTimeSpan = null, bool throttlePerUser = false) {
			ChatCommand command = ChatCommand.Create(room, name, commandAction, description, aliases, modOnly, throttleTimeSpan, throttlePerUser, true);
			if(command != null) {
				if(!commands.Contains(command)) {
					commands.Add(command);
				}
			}

            return command;
		}
		
		public override void Close() {
			foreach(var command in commands) {
				command.Remove();
			}
		}
	}
}
