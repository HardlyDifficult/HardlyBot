namespace Hardly.Library.Twitch {
    public interface TwitchCommand : IDataList {
        uint id {
            get;
        }
        
        TwitchConnection connection {
            get;
        }

        string command {
            get;
            set;
        }

        string description {
            get;
            set;
        }
        
        bool isModOnly {
            get;
            set;
        }

        TimeSpan coolDown {
            get;
            set;
        }

        string response {
            get;
            set;
        }
    }
}
