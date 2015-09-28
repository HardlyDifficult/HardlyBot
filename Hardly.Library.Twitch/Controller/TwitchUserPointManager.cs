using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class TwitchUserPointManager : PlayerPointManager {
		readonly SqlTwitchUserPoints sqlPoints;

		public TwitchUserPointManager(SqlTwitchChannel channel, SqlTwitchUser user) {
			sqlPoints = new SqlTwitchUserPoints(user, channel);
		}
    
        public override ulong Points {
            get {
                GiveBonusIfTime();
                return base.Points;
            }
        }

        protected override ulong TotalPointsInAccount {
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
