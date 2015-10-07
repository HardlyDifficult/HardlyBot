namespace Hardly.Library.Twitch {
	public interface TwitchChannel : IDataList {
		TwitchUser user {
            get;
        }
        
		bool isLive {
            get;
            set;
		}

		string previewImageUrl {
            get;
            set;
		}

		string game {
            get;
            set;
		}

		uint liveViewers {
            get;
            set;
		}

		uint totalViews {
            get;
            set;
		}

		uint followers {
            get;
            set;
		}
	}
}
