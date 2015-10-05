using System.Diagnostics;

namespace Hardly.Library.Twitch {
	public class SqlTwitchChannel : SqlRow {
		public readonly SqlTwitchUser user;

		public SqlTwitchChannel(SqlTwitchUser user, bool isLive = false, string previewImageUrl = null, string game = null, uint liveViewers = 0, uint totalViews = 0, uint followers = 0)
				: base(new object[] { user.id, isLive, previewImageUrl, game, liveViewers, totalViews, followers }) {
			this.user = user;
		}

		internal static readonly SqlTable _table = new SqlTable("twitch_channels");
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

		public bool isLive {
			get {
				return Get<bool>(1);
			}
			set {
				Set(1, value);
			}
		}

		public string previewImageUrl {
			get {
				return Get<string>(2);
			}
			set {
				Set(2, value);
			}
		}

		public string game {
			get {
				return Get<string>(3);
			}
			set {
				Set(3, value);
			}
		}

		public uint liveViewers {
			get {
				return Get<uint>(4);
			}
			set {
				Set(4, value);
			}
		}

		public uint totalViews {
			get {
				return Get<uint>(5);
			}
			set {
				Set(5, value);
			}
		}

		public uint followers {
			get {
				return Get<uint>(6);
			}
			set {
				Set(6, value);
			}
		}

		public static SqlTwitchChannel[] GetAllLiveFollowers(SqlTwitchChannel channel) {
			string where = "twitch_followers.ChannelUserId=?a and IsLive=?b";
			string join = "join twitch_followers on twitch_followers.UserId=twitch_channels.UserId";
			object[] vars = new object[] { channel.user.id, true };

			List<object[]> results = _table.Select(join, null, where, vars, null, 0);
			if(results != null) {
				SqlTwitchChannel[] channels = new SqlTwitchChannel[results.Count];
				for(int i = 0; i < results.Count; i++) {
					channels[i] = FromSql(results[i]);
				}

				return channels;
			} else {
				return null;
			}
		}

		public static void ClearLiveFollowers(SqlTwitchChannel channel) {
			_table.Update("join twitch_followers on twitch_followers.UserId=twitch_channels.UserId", "IsLive=?a", "ChannelUserId=?b", new object[] { false, channel.user.id });
		}

		public static SqlTwitchChannel[] GetAll() {
			List<object[]> results = _table.Select(null, null, null, null, null, 0);
			if(results != null) {
				SqlTwitchChannel[] channels = new SqlTwitchChannel[results.Count];
				for(int i = 0; i < results.Count; i++) {
					channels[i] = FromSql(results[i]);
				}

				return channels;
			} else {
				return null;
			}
		}

		static SqlTwitchChannel FromSql(object[] results) {
			return new SqlTwitchChannel(new SqlTwitchUser(results[0].FromSql<uint>()),
													results[1].FromSql<bool>(),
													results[2].FromSql<string>(),
													results[3].FromSql<string>(),
													results[4].FromSql<uint>(),
													results[5].FromSql<uint>(),
													results[6].FromSql<uint>());
		}
	}
}
