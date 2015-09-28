using System;

namespace Hardly.Games {
    /// <summary>
    /// Game Player object should live for the life of one full (e.g. one game of holdem, then new GamePlayers for the next)
    /// </summary>
    public class GamePlayer<PlayerIdObjectType> {
        readonly PlayerPointManager pointManager;
        public readonly PlayerIdObjectType idObject;

        public ulong bet {
            get;
            internal set;
        }

        public GamePlayer(PlayerPointManager pointManager, PlayerIdObjectType idObject) {
            this.pointManager = pointManager;
            this.idObject = idObject;
        }

        public bool PlaceBet(ulong amount, bool allOrNothing) {
            amount = pointManager?.ReserveBet(amount, allOrNothing) ?? amount;
            if(pointManager != null && amount > 0) {
                bet += amount;
                return true;
            }

            return false;
        }

        public void CanelBet() {
            pointManager?.Award(bet, 0);
            bet = 0;
        }

        public void Award(long winningsOrLosings) {
            pointManager?.Award(bet, winningsOrLosings);
        }

        public void LoseBet() {
            pointManager?.Award(bet, (long)bet * -1L);
        }
    }
}
