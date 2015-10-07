using System;

namespace Hardly.Library.Twitch {
    public interface TwitchUser : IDataList {
       uint id {
            get;
        }

        string name {
            get;
        }

        string userName {
            get;
        }

        DateTime createdAt {
            get;
        }

        string logo {
            get;
            set;
        }

        string bio {
            get;
            set;
        }
    }
}
