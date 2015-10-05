using System;
using System.Diagnostics;

namespace Hardly.Library.Twitch {
    public class SqlTwitchUser : SqlRow {
        public SqlTwitchUser(uint id, string name = null, DateTime created = default(DateTime), string logo = null, string bio = null)
            : base(new object[] { id, name, created, logo, bio }) {
        }

        public static SqlTwitchUser GetFromName(string username) {
            try {
                SqlTwitchUser user = SqlTwitchUser.GetFromName(username);
                if(user != null) {
                    return user;
                } else {
                    return TwitchApi.GetUser(username);
                }
            } catch(Exception e) {
                Log.error("Twitch get user from name", e);
                return null;
            }
        }

        internal static readonly SqlTable _table = new SqlTable("twitch_users");
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

        public string name {
            get {
                return Get<string>(1);
            }
        }

        public string userName {
            get {
                return name.ToLower();
            }
        }

        public DateTime createdAt {
            get {
                return Get<DateTime>(2);
            }
            // set possible but not recommended
        }

        public string logo {
            get {
                return Get<string>(3);
            }
            set {
                Set(3, value);
            }
        }

        public string bio {
            get {
                return Get<string>(4);
            }
            set {
                Set(4, value);
            }
        }

        public static SqlTwitchUser GetFromName(string username) {
            try {
                object[] values = _table.Select(null, null, "Name=?a", new object[] { username }, null);
                if(values != null && values.Length > 0) {
                    return new SqlTwitchUser(values[0].FromSql<uint>(), values[1].FromSql<string>(),
                        values[2].FromSql<DateTime>(), values[3].FromSql<string>(), values[4].FromSql<string>());
                } else {
                    return null;
                }
            } catch(Exception e) {
                Log.exception(e);

                return null;
            }
        }

        public static SqlTwitchUser[] GetAll() {
            List<object[]> results = _table.Select(null, null, null, null, null, 0);
            if(results != null) {
                SqlTwitchUser[] users = new SqlTwitchUser[results.Count];
                for(int i = 0; i < results.Count; i++) {
                    users[i] = new SqlTwitchUser(results[i][0].FromSql<uint>(), results[i][1].FromSql<string>(), results[i][2].FromSql<DateTime>(),
                        results[i][3].FromSql<string>(), results[i][4].FromSql<string>());
                }

                return users;
            } else {
                return null;
            }
        }

        public override string ToString() {
            return name;
        }
    }
}
