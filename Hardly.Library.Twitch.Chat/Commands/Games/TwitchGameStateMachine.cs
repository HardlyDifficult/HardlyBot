using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
    public abstract class TwitchGameStateMachine<GameLogicController> : TwitchGame<GameLogicController> where GameLogicController : Game, new() {
        public GameState currentState {
            get;
            private set;
        }
        object myLock = new object();

        public TwitchGameStateMachine(TwitchChatRoom room, Type startingStateType) : base(room) {
            SetState(null, startingStateType);
        }

        public bool SetState(GameState theStateIThinkWeAreIn, Type theStateToChangeToIfInTheStateIExpect) {
            Debug.Assert(theStateIThinkWeAreIn?.GetType() != theStateToChangeToIfInTheStateIExpect);
            
            lock (myLock) {
                if(theStateIThinkWeAreIn == null || currentState == null || currentState.Equals(theStateIThinkWeAreIn)) {
                    Debug.Assert(!theStateToChangeToIfInTheStateIExpect.Equals(currentState?.GetType()));
                    Log.debug("Twitch Game: Setting next state to " + theStateToChangeToIfInTheStateIExpect.ToString());
                    currentState?.Close();
                    currentState = (GameState)theStateToChangeToIfInTheStateIExpect.GetConstructor(new Type[] { this.GetType() }).Invoke(new object[] { this });
                    currentState.Open();
                    return true;
                }
            }
                   
            return false;
        }
    }
}