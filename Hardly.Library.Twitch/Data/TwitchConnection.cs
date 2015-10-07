namespace Hardly.Library.Twitch {
	public interface TwitchConnection : IDataList {
        TwitchBot bot {
            get;
        }
		TwitchChannel channel {
            get;
        }

        bool autoConnectToChat {
            get;
            set;
		}
	}
}
