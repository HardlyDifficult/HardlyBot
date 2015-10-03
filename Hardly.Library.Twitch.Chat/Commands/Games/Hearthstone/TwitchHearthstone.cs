using System;
using Hardly.Library.Hearthstone;
using Hardly.Games.Betting;

namespace Hardly.Library.Twitch {
    public class TwitchHearthstone : TwitchGameStateMachine<ParimutuelBettingGame<SqlTwitchUser>> {
        internal HearthGame hearthstoneGame = null;
        internal EndOfGame lastGameEnding = null;

        public TwitchHearthstone(TwitchChatRoom room) : base(room) {
            HearthstoneEventObserver hearthObserver = new HearthstoneEventObserver();
            hearthObserver.RegisterObserver(HearthEvent);
        }

        private void HearthEvent(HearthstoneEvent hearthEvent) {
            if(hearthEvent is NewGame) {
                var newGameEvent = hearthEvent as NewGame;
                hearthstoneGame = newGameEvent.game;
                SetState(null, typeof(HSStateAcceptingBets));
            } else if(hearthEvent is EndOfGame) {
                var endGameEvent = hearthEvent as EndOfGame;
                lastGameEnding = endGameEvent;
                SetState(null, typeof(HSStateEndOfGame));
            } else if(hearthEvent is DrawCard) {
                var drawEvent = hearthEvent as DrawCard;
                if(state is HSStatePlaying) {
                    var playingState = state as HSStatePlaying;
                    playingState.NextTurn(drawEvent);
                }
            }
        }
    }
}
