using System;

namespace Hardly.Library.Twitch {
	public interface TwitchUserInChannel : IDataList {
		TwitchUser user {
            get;
        }
		TwitchChannel channel {
            get;
        }

        DateTime createdAt {
            get;
        }

        bool isCurrentlyFollowing {
            get;
            set;
        }

        string greetingMessage {
            get;
            set;
        }
    }
}
