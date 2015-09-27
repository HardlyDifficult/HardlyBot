using System;

namespace Hardly.Library.Twitch {
	public class UserPointManager {
		readonly SqlTwitchUserPoints sqlPoints;
		ulong reservedPoints;

		public UserPointManager(SqlTwitchChannel channel, SqlTwitchUser user) {
			sqlPoints = new SqlTwitchUserPoints(user, channel);
			reservedPoints = 0;
		}

		public ulong points {
			get {
				GiveBonusIfTime();
				return sqlPoints.points - reservedPoints;
			}
		}

		void GiveBonusIfTime() {
			if(sqlPoints.points == 0 && (DateTime.Now - sqlPoints.timeOfLastBonus) > TimeSpan.FromMinutes(1)) {
				sqlPoints.points = 420;
				sqlPoints.timeOfLastBonus = DateTime.Now;
				sqlPoints.Save();
			}
		}

		public void Award(ulong reservation, long winningsOrLosings) {
			FreeUp(reservation);
			
			if(winningsOrLosings < 0 && (winningsOrLosings * -1) > (long)sqlPoints.points) {
				sqlPoints.points = 0;
			} else {
				sqlPoints.points = (ulong)((long)sqlPoints.points + winningsOrLosings);
			}

			sqlPoints.Save();
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
				bet = Math.Min(points, bet);
			} else {
				if(bet > points) {
					bet = 0;
				}
			}

			reservedPoints += bet;
						
			return bet;
		}
	}
}
