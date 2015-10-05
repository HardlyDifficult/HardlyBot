using System;

namespace Hardly.Games {
    /// <summary>
    /// Game Player object should live for the life of one full (e.g. one game of holdem, then new GamePlayers for the next)
    /// </summary>
    public class GamePlayer<PlayerIdType> {
        public readonly PlayerIdType idObject;
        public bool? isWinner;
        public readonly PlayerPointManager pointManager;
        public long winningsOrLosings {
            get;
            private set;
        }

        public GamePlayer(PlayerPointManager pointManager, PlayerIdType idObject) {
            this.pointManager = pointManager;
            this.idObject = idObject;
            this.bet = 0;
            this.winningsOrLosings = 0;
        }

        public ulong bet {
            get;
            internal set;
        }

        public void Award(long winningsOrLosings) {
            Log.debug("Points gave " + winningsOrLosings.ToStringWithCommas() + " to " + idObject.ToString());
            pointManager?.Award(bet, winningsOrLosings);

            this.winningsOrLosings += winningsOrLosings;
        }

        public void AwardPartialBet(ulong betAmount, long winningsOrLosings) {
            Log.debug("Points gave " + winningsOrLosings.ToStringWithCommas() + " to " + idObject.ToString());
            pointManager?.Award(betAmount, winningsOrLosings);
            bet -= betAmount;

            this.winningsOrLosings += winningsOrLosings;
        }

        public void CancelBet() {
            if(isWinner == null) {
                pointManager?.Award(bet, 0);
                bet = 0;
            }
        }

        public void CancelPartialBet(ulong amount) {
            amount = Math.Min(amount, bet);
            pointManager?.Award(amount, 0);
            bet -= amount;
        }

        public void LoseBet() {
            long amount = (long)bet * -1L;
            Award(amount);
            isWinner = false;
            winningsOrLosings += amount;
        }

        public void LosePartialBet(ulong amount) {
            amount = Math.Min(amount, bet);
            pointManager?.Award(amount, (long)amount * -1);
            bet -= amount;

            winningsOrLosings -= (long)amount;
        }

        public ulong PlaceBet(ulong amount, bool allOrNothing) {
            amount = pointManager?.ReserveBet(amount, allOrNothing) ?? amount;
            if(pointManager != null && amount > 0) {
                bet += amount;
                return amount;
            }

            return 0;
        }
    }
}
