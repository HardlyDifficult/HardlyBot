namespace Hardly.Library.Twitch {
    public class SqlTwitchBot : SqlRow, TwitchBot {
        public SqlTwitchBot(SqlTwitchUser user, string oauthPassword = null)
            : base(new object[] { user.id, oauthPassword }) {
            this.user = user;
        }

        internal static readonly SqlTable _table = new SqlTable("twitch_bots");
        public override SqlTable table {
            get {
                return _table;
            }
        }

        uint userId {
            get {
                return Get<uint>(0);
            }
        }

        public string oauthPassword {
            get {
                return Get<string>(1);
            }
            set {
                Set(1, value);
            }
        }

        public TwitchUser user {
            get;
            private set;
        }

        public static TwitchBot[] GetAll() {
            List<object[]> results = _table.Select(null, null, null, null, null, 0);
            if(results != null) {
                TwitchBot[] bots = new TwitchBot[results.Count];
                for(int i = 0; i < results.Count; i++) {
                    bots[i] = new SqlTwitchBot(new SqlTwitchUser(results[i][0].FromSql<uint>()), results[i][1].FromSql<string>());
                }

                return bots;
            } else {
                return null;
            }
        }
    }
}
