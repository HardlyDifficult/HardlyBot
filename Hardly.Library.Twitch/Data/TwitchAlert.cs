using System;

namespace Hardly.Library.Twitch {
    public interface TwitchAlert : IDataList {
        TwitchConnection connection {
            get;
        }

        string alertGuid {
            get;
        }

        DateTime lastFollowerNotification {
            get;
            set;
        }
    }
}
