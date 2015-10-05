using System;

namespace Hardly.Games.Betting {
    public class ParimutuelBettingGame<PlayerIdType> : Game<ParimutuelPlayer<PlayerIdType>, PlayerIdType> {
        public double vig = 0.05;
        public double minusPool = 0.10;
        bool? houseWon;

        public ParimutuelBettingGame() : base(1, uint.MaxValue) {
        }

        public void EndGame(bool? houseWon) {
            this.houseWon = houseWon;
            EndGame();
        }

        protected override void EndGame() {
            foreach(var player in GetPlayers()) {
                if(houseWon == null) {
                    player.CancelBet();
                } else if(player.toWin.Equals(houseWon.Value)) {
                    double payoutRate = 1 - TotalBet(player.toWin) / TotalBets();  // 20 / 10 = 0.5.... 10/20 want 1:1 (1).  20/20 = want 0.  if 5/20 (.25) want 1:2 (.5)
                    payoutRate *= 2 - vig; // 0.5 * 0.05 = 0.025
                    payoutRate = Math.Max(payoutRate, minusPool); // 0.1

                    ulong winnings = (ulong)((1 + payoutRate) * (double)player.bet); // 1.1 * 10 = 11
                    player.Award((long)(winnings - player.bet));
                } else {
                    player.LoseBet();
                }
            }

            base.EndGame();
        }

        public ulong TotalBet(bool? toWin) {
            ulong bet = 0;
            foreach(var player in GetPlayers()) {
                if(toWin == null || toWin.Value.Equals(player.toWin)) {
                    bet += player.bet;
                }
            }

            return bet;
        }
    }
}
