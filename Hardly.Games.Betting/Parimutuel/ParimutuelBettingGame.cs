using System;

namespace Hardly.Games.Betting {
    public class ParimutuelBettingGame<PlayerIdType> : Game<PlayerIdType, ParimutuelPlayer<PlayerIdType>> {
        double vig = 0.05;
        double minusPool = 0.10;
        bool? iWon = null;

        public ParimutuelBettingGame() : base(uint.MaxValue) {
        }

        public override void EndGame() {
            foreach(var player in PlayerGameObjects) {
                if(iWon == null) {
                    player.CancelBet();
                } else if(player.toWin.Equals(iWon.Value)) {
                    double payoutRate = 1 - TotalBet(player.toWin) / TotalBet();  // 20 / 10 = 0.5.... 10/20 want 1:1 (1).  20/20 = want 0.  if 5/20 (.25) want 1:2 (.5)
                    payoutRate *= 2 - vig; // 0.5 * 0.05 = 0.025
                    payoutRate = Math.Max(payoutRate, minusPool); // 0.1

                    ulong winnings = (ulong)((1 + payoutRate) * (double)player.bet); // 1.1 * 10 = 11
                    player.Award((long)(winnings - player.bet));
                } else {
                    player.LoseBet();
                }
            }
        }

        private double TotalBet(bool? toWin = null) {
            ulong bet = 0;
            foreach(var player in PlayerGameObjects) {
                if(toWin == null || toWin.Value.Equals(player.toWin)) {
                    bet += player.bet;
                }
            }

            return bet;
        }

        public void SetWinner(bool? iWon) {
            this.iWon = iWon;
        }

    }
}
