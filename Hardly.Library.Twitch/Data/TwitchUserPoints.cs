using System;

namespace Hardly.Library.Twitch {
	public interface TwitchUserPoints : IDataList {
		TwitchUser user {
            get;
        }
		TwitchChannel channel {
            get;
        }
		ulong points {
            get;
            set;
		}
		
		DateTime timeOfLastBonus {
            get;
            set;
		}
        
	}
}
