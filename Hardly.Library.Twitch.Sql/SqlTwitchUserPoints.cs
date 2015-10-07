using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch {
    public class SqlTwitchUserPoints : SqlRow, TwitchUserPoints {
        public SqlTwitchUserPoints(TwitchUser user, TwitchChannel channel, ulong points = 0, DateTime timeOfLastBonus = default(DateTime))
				: base(new object[] { user.id, channel.user.id, points, timeOfLastBonus }) {
            user.Save();
            this.user = user;
            this.channel = channel;
        }



        internal static readonly SqlTable _table = new SqlTable("twitch_user_points");
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

        uint channelUserId {
            get {
                return Get<uint>(1);
            }
        }

        public ulong points {
            get {
                return Get<ulong>(2);
            }
            set {
                Set(2, value);
            }
        }

        public DateTime timeOfLastBonus {
            get {
                return Get<DateTime>(3);
            }
            set {
                Set(3, value);
            }
        }

        public TwitchUser user {
            get;
            private set;
        }

        public TwitchChannel channel {
            get;
            private set;
        }

        public static TwitchUserPoints[] GetTopUsersForChannel(TwitchChannel channel, uint count) {
            List<object[]> results = _table.Select(null, null, "ChannelUserId=?a", new object[] { channel.user.id }, "Points Desc", count);

            if(results != null && results.Count > 0) {
                TwitchUserPoints[] points = new TwitchUserPoints[results.Count];
                for(int i = 0; i < results.Count; i++) {
                    points[i] = new SqlTwitchUserPoints(new SqlTwitchUser((uint)results[i][0]), channel, (ulong)results[i][2], (DateTime)results[i][3]);
                }

                return points;
            }

            return null;
        }
    }
}
