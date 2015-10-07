using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class TwitchUserPointManager : PlayerPointManager {
		readonly TwitchUserPoints sqlPoints;

		public TwitchUserPointManager(ITwitchFactory factory, TwitchChannel channel, TwitchUser user) {
			sqlPoints = factory.GetUserPoints(user, channel);
		}
    
        public override ulong Points {
            get {
                GiveBonusIfTime();
                return base.Points;
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
