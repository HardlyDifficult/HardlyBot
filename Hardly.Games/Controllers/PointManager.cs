using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games {
    public class PointManager {
        ulong reservedPoints = 0;

        public PointManager() {
        }

        public virtual ulong TotalPointsInAccount {
            get;
            set;
        }

        public virtual ulong AvailablePoints {
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

        void FreeUp(ulong reservation) {
            if(reservedPoints >= reservation) {
                reservedPoints -= reservation;
            } else {
                reservedPoints = 0;
            }
        }

        public ulong ReserveBet(ulong bet, bool allOrNothing = false) {
            if(!allOrNothing) {
                bet = Math.Min(AvailablePoints, bet);
            } else {
                if(bet > AvailablePoints) {
                    bet = 0;
                }
            }

            reservedPoints += bet;

            return bet;
        }
    }
}
