
namespace Hardly.Library.Twitch {
    public class SqlTwitchCommand : SqlRow, TwitchCommand {

        public SqlTwitchCommand(
            uint id,
            TwitchConnection twitchConnection = null,
            string command = null,
            string description = null,
            bool isModOnly = true,
            TimeSpan coolDown = default(TimeSpan),
            string response = null)
                : base(new object[] {
                    id,
                    twitchConnection.channel.user.id,
                    twitchConnection.bot.user.id,
                    command,
                    description, 
                    isModOnly,
                    coolDown.TotalSeconds,
                    response
                }) {
            this.connection = twitchConnection;
        }

        static readonly SqlTable _table = new SqlTable("twitch_commands");
        public override SqlTable table {
            get {
                return _table;
            }

        }

        public uint id {
            get {
                return Get<uint>(0);
            }
        }

        uint channelUserId {
            get {
                return Get<uint>(1);
            }
        }

        uint botUserId {
            get {
                return Get<uint>(2);
            }
        }
        

        public string command {
            get {
                return Get<string>(3);
            }
            set {
                Set(3, value);
            }
        }

        public string description {
            get {
                return Get<string>(4);
            }
            set {
                Set(4, value);
            }
        }

        public bool isModOnly {
            get {
                return Get<bool>(5);
            }
            set {
                Set(5, value);
            }
        }

        public TimeSpan coolDown {
            get {
                return TimeSpan.FromSeconds(Get<int>(6));
            }
            set {
                Set(6, value.TotalSeconds);
            }
        }

        public string response {
            get {
                return Get<string>(7);
            }
            set {
                Set(7, value);
            }
        }

        public TwitchConnection connection {
            get;
            private set;
        }

        public static TwitchCommand[] GetAll(TwitchConnection connection) {
            List<object[]> results = _table.Select(null, null, "ChannelUserId=?a AND BotUserId=?b",
                new object[] { connection.channel.user.id, connection.bot.user.id }, null, 0);
            if(results != null) {
                TwitchCommand[] commands = new TwitchCommand[results.Count];
                for(int i = 0; i < results.Count; i++) {
                    commands[i] = new SqlTwitchCommand(
                        results[i][0].FromSql<uint>(),
                        connection,
                        results[i][3].FromSql<string>(),
                        results[i][4].FromSql<string>(),
                        results[i][5].FromSql<bool>(),
                        TimeSpan.FromSeconds(results[i][6].FromSql<int>()),
                        results[i][7].FromSql<string>());
                }

                return commands;
            } else {
                return null;
            }
        }
    }
}
