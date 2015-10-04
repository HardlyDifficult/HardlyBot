using System;
using Hardly.Games.Betting;
using Hardly.Library.Hearthstone;

namespace Hardly.Library.Twitch {
    public class HSStateAcceptingBets : HSStatePlaying {
        uint turnCounter = 0;

        public HSStateAcceptingBets(TwitchHearthstone controller) : base(controller) {
            AddCommand(controller.room, "bettowin", BetToWin, "Places a bet that the streamer will win.", null, false, null, false);
            AddCommand(controller.room, "bettolose", BetToLose, "Places a bet that the streamer will lose.", null, false, null, false);
        }

        internal override void Open() {
            controller.room.SendChatMessage("Hearthstone --- just started a new game: "
                + controller.hearthstoneGame.myPlayerName + " vs " + controller.hearthstoneGame.opponentPlayerName);

            controller.game.StartGame();
        }

        private void BetToLose(SqlTwitchUser speaker, string additionalText) {
            PlaceBet(speaker, additionalText, false);
        }

        private void BetToWin(SqlTwitchUser speaker, string additionalText) {
            PlaceBet(speaker, additionalText, true);
        }

        private void PlaceBet(SqlTwitchUser speaker, string additionalText, bool toWin) {
            if(controller.game.Contains(speaker)) {
                var bettingPlayer = controller.game.Get(speaker);
                if(bettingPlayer.toWin == toWin) {
                    ulong amount = controller.room.pointManager.GetPointsFromString(additionalText);
                    amount = bettingPlayer.PlaceBet(amount, false);
                    if(amount > 0) {
                        controller.room.SendWhisper(speaker, "You raised your bet " + controller.room.pointManager.ToPointsString(amount) 
                            + " to " + controller.room.pointManager.ToPointsString(bettingPlayer.bet));
                    } else {
                        // no more cash
                    }
                } else {
                    // no switching sides
                }
            } else {
                var bettingPlayer = new ParimutuelPlayer<SqlTwitchUser>(controller.room.pointManager.ForUser(speaker), speaker, toWin);
                ulong amount = controller.room.pointManager.GetPointsFromString(additionalText);
                amount = bettingPlayer.PlaceBet(amount, false);
                if(amount > 0) {
                    controller.game.Join(speaker, bettingPlayer);
                    controller.room.SendWhisper(speaker, "You bet " + controller.room.pointManager.ToPointsString(amount));
                } else {
                    // Broke dude.
                }
            }
        }

        internal override void NextTurn(DrawCard drawEvent) {
            base.NextTurn(drawEvent);

            if(turnCounter++ > 4) {
                controller.room.SendChatMessage("No more bets");
                controller.SetState(this, typeof(HSStateNoBets));
            }
        }
    }
}
