using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public abstract class TwitchGameController<GameType, TwitchController> : TwitchCommandController where GameType : new() {
      public readonly GameType game;
		public GameState<TwitchController> state = null;
		object myLock;

		public TwitchGameController(TwitchChatRoom room) : base(room) {
			game = new GameType();
			myLock = new object();
		}

		public void SetState(GameState<TwitchController> startingState, Type nextStateType) {
			bool changed = false;
			lock (myLock) {
				if(startingState == null || (state != null && state.Equals(startingState))) {
					GameState<TwitchController> nextState = (GameState<TwitchController>)nextStateType.GetConstructor(new Type[] { this.GetType() }).Invoke(new object[] { this });
					if(!nextState.Equals(state)) {
						Log.debug("Twitch Game: Setting next state to " + nextState.GetType().ToString());
						state?.Close();
						state = nextState;
						changed = true;
					}
				}
			}

			if(changed) {
				state.Open();
			}
		}
	}
}