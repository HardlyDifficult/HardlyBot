using System;

namespace Hardly {
	public class SqlTwitchUserPoints : SqlRow {
		public readonly SqlTwitchUser user;
		public readonly SqlTwitchChannel channel;

		public SqlTwitchUserPoints(SqlTwitchUser user, SqlTwitchChannel channel, ulong points = 0, DateTime timeOfLastBonus = default(DateTime))
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

		public static SqlTwitchUserPoints[] GetTopUsersForChannel(SqlTwitchChannel channel, uint count) {
			List<object[]> results = _table.Select(null, null, "ChannelUserId=?a", new object[] { channel.user.id }, "Points Desc", count);

			if(results != null && results.Count > 0) {
				SqlTwitchUserPoints[] points = new SqlTwitchUserPoints[results.Count];
				for(int i = 0; i < results.Count; i++) {
					points[i] = new SqlTwitchUserPoints(new SqlTwitchUser((uint)results[i][0]), channel, (ulong)results[i][2], (DateTime)results[i][3]);
				}

				return points;
			}

			return null;
		}
	}
}
