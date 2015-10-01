using System;

namespace Hardly.Games {
    public class PlayerPointManager {
        ulong reservedPoints = 0;
        
        #region Public interface
        public virtual ulong Points {
            get {
                return TotalPointsInAccount - reservedPoints;
            }
        }

        public void Award(ulong reservation, long winningsOrLosings) {
            FreeUp(reservation);

            if(winningsOrLosings < 0 && (winningsOrLosings * -1) > (long)TotalPointsInAccount) {
                TotalPointsInAccount = 0;
            } else {
                TotalPointsInAccount = (ulong)((long)TotalPointsInAccount + winningsOrLosings);
            }
        }

        public ulong ReserveBet(ulong bet, bool allOrNothing = false) {
            if(!allOrNothing) {
                bet = Math.Min(Points, bet);
            } else {
                if(bet > Points) {
                    bet = 0;
                }
            }

            reservedPoints += bet;

            return bet;
        }
        #endregion

        #region Protected interface
        public virtual ulong TotalPointsInAccount {
            get;
            set;
        }
        #endregion

        #region Private helpers
        void FreeUp(ulong reservation) {
            if(reservedPoints >= reservation) {
                reservedPoints -= reservation;
            } else {
                reservedPoints = 0;
            }
        }
        #endregion
    }
}
