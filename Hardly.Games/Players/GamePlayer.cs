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

        public ulong PlaceBet(ulong amount, bool allOrNothing) {
            amount = pointManager?.ReserveBet(amount, allOrNothing) ?? amount;
            if(pointManager != null && amount > 0) {
                bet += amount;
                return amount;
            }

            return 0;
        }

        public void CanelBet() {
            pointManager?.Award(bet, 0);
            bet = 0;
        }

        public void Award(long winningsOrLosings) {
            Log.debug("Points gave " + winningsOrLosings.ToStringWithCommas() + " to " + idObject.ToString());
            pointManager?.Award(bet, winningsOrLosings);
        }

        public void LoseBet() {
            Award((long)bet * -1L);
        }

        public override bool Equals(object obj) {
            if(obj != null && obj is GamePlayer<PlayerIdObjectType>) {
                var other = obj as GamePlayer<PlayerIdObjectType>;
                return idObject.Equals(other.idObject);
            }

            return false;
        }

        public override int GetHashCode() {
            return idObject.GetHashCode();
        }
    }
}
