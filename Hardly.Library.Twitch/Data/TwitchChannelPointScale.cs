namespace Hardly.Library.Twitch {
	public interface TwitchChannelPointScale : IDataList {
		TwitchChannel channel {
            get;
        }
        
		ulong unitValue {
            get;
        }

        string unitNameSingular {
            get;
            set;
        }

        string unitNamePlural {
            get;
            set;
        }
    }
}
