using System;

namespace Hardly.Library.Twitch {
    public abstract class GameState {
        bool isClosed = false;
        object myLock = new object();

        public void Close() {
            lock(myLock) {
                if(!isClosed) {
                    isClosed = true;

                    CloseState();
                }
            }
        }

        public void Open() {
            lock(myLock) {
                if(!isClosed) {
                    OpenState();
                } else {
                    Log.debug("State issue - this would have been bad...");
                }
            }
        }

        protected abstract void CloseState();

        /// <summary>
        /// This is called right after the constructor.  
        /// Anything that could change state must be in this method and NOT the constructor.
        /// </summary>
        protected abstract void OpenState();
    }

	public abstract class GameState<GameLogicController> : GameState {
		protected GameLogicController controller;
		List<ChatCommand> commands = new List<ChatCommand>();

		public GameState(GameLogicController controller) {
			this.controller = controller;
		}

		protected ChatCommand AddCommand(TwitchChatRoom room, string name, Action<TwitchUser, string> commandAction, string description, string[] aliases, bool modOnly, TimeSpan throttleTimeSpan = null, bool throttlePerUser = false) {
			ChatCommand command = ChatCommand.Create(room, name, commandAction, description, aliases, modOnly, throttleTimeSpan, throttlePerUser, true);
			if(command != null) {
				if(!commands.Contains(command)) {
					commands.Add(command);
				}
			}

            return command;
		}
		
		protected override void CloseState() {
			foreach(var command in commands) {
				command.Remove();
			}
		}
	}
}
