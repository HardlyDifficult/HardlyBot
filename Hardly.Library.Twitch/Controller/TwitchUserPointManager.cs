using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class TwitchUserPointManager : PointManager {
		readonly SqlTwitchUserPoints sqlPoints;

		public TwitchUserPointManager(SqlTwitchChannel channel, SqlTwitchUser user) {
			sqlPoints = new SqlTwitchUserPoints(user, channel);
		}
    
        public override ulong AvailablePoints {
            get {
                GiveBonusIfTime();
                return base.AvailablePoints;
            }
        }

        public override ulong TotalPointsInAccount {
            get {
                return sqlPoints.points;
            }
            set {
                sqlPoints.points = value;
                sqlPoints.Save();
            }
        }

        void GiveBonusIfTime() {
			if(sqlPoints.points == 0 && (DateTime.Now - sqlPoints.timeOfLastBonus) > TimeSpan.FromMinutes(1)) {
				sqlPoints.points = 420;
				sqlPoints.timeOfLastBonus = DateTime.Now;
				sqlPoints.Save();
			}
		}

		
	}
}
