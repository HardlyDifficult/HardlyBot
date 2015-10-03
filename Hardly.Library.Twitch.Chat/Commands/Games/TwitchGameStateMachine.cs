using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public abstract class TwitchGameStateMachine<GameLogicController> : TwitchGame<GameLogicController> where GameLogicController : Game, new() {
        public GameState state = null;
		object myLock = new object();

        public TwitchGameStateMachine(TwitchChatRoom room, Type startingStateType) : base(room) {
            SetState(null, startingStateType);
		}

		public bool SetState(Type startingState, Type nextStateType) {
            if(startingState != nextStateType) {
                bool changed = false;
                lock (myLock) {
                    if(startingState == null || (state != null && state.GetType().Equals(startingState))) {
                        if(!nextStateType.Equals(state)) {
                            Log.debug("Twitch Game: Setting next state to " + nextStateType.ToString());
                            state?.Dispose();
                            changed = true;
                        }
                    }
                }

                if(changed) {
                    state = (GameState)nextStateType.GetConstructor(new Type[] { this.GetType() }).Invoke(new object[] { this });
                    return true;
                }
            }

            return false;
		}
	}
}