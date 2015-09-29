using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public abstract class TwitchGameStateMachine<GameLogicController> : TwitchGame<GameLogicController> where GameLogicController : Game, new() {
        public GameState state = null;
		object myLock = new object();

        public TwitchGameStateMachine(TwitchChatRoom room) : base(room) {
		}

		public bool SetState(Type startingState, Type nextStateType) {
            if(startingState != nextStateType) {
                bool changed = false;
                lock (myLock) {
                    if(startingState == null || (state != null && state.GetType().Equals(startingState))) {
                        var nextState = (GameState)nextStateType.GetConstructor(new Type[] { this.GetType() }).Invoke(new object[] { this });
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
                    return true;
                }
            }

            return false;
		}
	}
}