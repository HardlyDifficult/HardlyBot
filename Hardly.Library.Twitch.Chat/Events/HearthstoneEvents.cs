using System;
using Hardly.Library.Hearthstone;

namespace Hardly.Library.Twitch {
    public class HearthstoneEvents : TwitchEventHandler {
        public HearthstoneEvents(TwitchChatRoom room) : base(room) {
            HearthstoneEventObserver hearthObserver = new HearthstoneEventObserver();
            hearthObserver.RegisterObserver(HearthEvent);
        }

        private void HearthEvent(HearthstoneEvent hearthEvent) {
            if(hearthEvent is NewGame) {
                var newGameEvent = hearthEvent as NewGame;
                room.SendChatMessage("Hearthstone --- just started a new game: " + newGameEvent.game.myPlayerName + " vs " + newGameEvent.game.opponentPlayerName);
            } else if(hearthEvent is EndOfGame) {
                var endGameEvent = hearthEvent as EndOfGame;

                new Timer(TimeSpan.FromSeconds(20), () => {
                    room.SendChatMessage("Hearthstone game over - " + (endGameEvent.iWon == null ? "ended in a draw" : endGameEvent.iWon.Value ? "we won!" : endGameEvent.game.opponentPlayerName + " won..."));
                }).Start();
            } else if(hearthEvent is DrawCard) {
                var drawEvent = hearthEvent as DrawCard;
                if(drawEvent.myTurn) {
                    room.SendChatMessage("Hearthstone - It's my turn, drew: " + (drawEvent.card.name == null ? "unknown card.." : drawEvent.card.name) + ".");
                }
            }
        }
    }
}
