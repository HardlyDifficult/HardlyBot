using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch {
    public class SqlTwitchAlert : SqlRow, TwitchAlert {
        public SqlTwitchAlert(TwitchConnection connection, string alertGuid, DateTime lastFollowerNotification = default(DateTime))
            : base(new object[] { connection.bot.user.id, connection.channel.user.id, alertGuid, lastFollowerNotification }) {
            this.connection = connection;
        }

        internal static readonly SqlTable _table = new SqlTable("twitch_alerts");
        public override SqlTable table {
            get {
                return _table;
            }
        }

        uint botUserId {
            get {
                return Get<uint>(0);
            }
        }

        uint channelUserId {
            get {
                return Get<uint>(1);
            }
        }

        public string alertGuid {
            get {
                return Get<string>(2);
            }
        }

        public virtual DateTime lastFollowerNotification {
            get {
                return Get<DateTime>(3);
            }
            set {
                Set(3, value);
            }
        }

        public TwitchConnection connection {
            get;
            private set;
        }

        public static SqlTwitchAlert FromGuid(string value) {
            object[] results = _table.Select(null, null, "AlertGuid=?a", new object[] { value }, null);
            if(results != null && results.Length > 0) {
                return new SqlTwitchAlert(new SqlTwitchConnection(new SqlTwitchBot(new SqlTwitchUser(results[0].FromSql<uint>())),
                    new SqlTwitchChannel(new SqlTwitchUser(results[1].FromSql<uint>()))),
                    results[2].FromSql<string>(),
                    results[3].FromSql<DateTime>());
            }

            return null;
        }
    }
}
