namespace Hardly.Library.Twitch {
    public interface TwitchBot : IDataList {
        TwitchUser user {
            get;
        }

        string oauthPassword {
            get;
            set;
        }
    }
}
