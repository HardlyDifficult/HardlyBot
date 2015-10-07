using System;

namespace Hardly.Library.Twitch {
	public class SqlTwitchUserInChannel : SqlRow, TwitchUserInChannel {
		public TwitchUser user {
            get;
            private set;
        }
		public TwitchChannel channel {
            get;
            private set;
        }

		public SqlTwitchUserInChannel(TwitchUser user, TwitchChannel channel, DateTime created = default(DateTime), bool isCurrentlyFollowing = false, string greetingMessage = null)
				: base(new object[] { user.id, channel.user.id, created, isCurrentlyFollowing, greetingMessage }) {
			this.user = user;
			this.channel = channel;
		}

		internal static readonly SqlTable _table = new SqlTable("twitch_user_in_channel");

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

		public DateTime createdAt {
			get {
				return Get<DateTime>(2);
			}
            // set possible but not recommended
        }

        public bool isCurrentlyFollowing {
            get {
                return Get<bool>(3);
            }
            set {
                Set(3, value);
            }
        }

        public string greetingMessage {
            get {
                return Get<string>(4);
            }
            set {
                Set(4, value);
            }
        }

        public static SqlTwitchUserInChannel GetMostRecent(TwitchChannel channel) {
			try {
				object[] results = _table.Select(null, null, "ChannelUserId=?a", new object[] { channel.user.id }, "CreatedAt Desc");
				if(results != null && results.Length > 0) {
					return FromSql(channel, results);
				} else {
					return null;
				}
			} catch(Exception e) {
				Log.exception(e);

				return null;
			}
		}

		public static SqlTwitchUserInChannel[] GetAllWithUsernames(TwitchChannel channel) {
			try {
				List<object[]> results = _table.Select("join twitch_users on id=" + _table.tableName + ".UserId", _table.tableName + ".*, name", "ChannelUserId=?a", new object[] { channel.user.id }, null, 0);

				if(results != null) {
					SqlTwitchUserInChannel[] followers = new SqlTwitchUserInChannel[results.Count];
					for(int i = 0; i < followers.Length; i++) {
						followers[i] = FromSql(channel, results[i]);
					}

					return followers;
				} else {
					return null;
				}
			} catch(Exception e) {
				Log.exception(e);

				return null;
			}
		}

		public static void ClearAll(TwitchChannel channel) {
			_table.Update(null, "IsCurrentlyFollowing=?a", "ChannelUserId=?b", new object[] { false, channel.user.id });
		}

		public static SqlTwitchUserInChannel[] GetNew(TwitchAlert twitchAlerts) {
			try {
				DateTime latestCreatedAt = twitchAlerts.lastFollowerNotification;

				List<object[]> results = _table.Select(null, null, "ChannelUserId=?a AND CreatedAt>?b", 
						new object[] { twitchAlerts.connection.channel.user.id, twitchAlerts.lastFollowerNotification }, null, 3);

				if(results != null && results.Count > 0) {
					SqlTwitchUserInChannel[] followers = new SqlTwitchUserInChannel[results.Count];
					for(int i = 0; i < followers.Length; i++) {
						followers[i] = FromSql(twitchAlerts.connection.channel, results[i]);
						if(latestCreatedAt < followers[i].createdAt) {
							latestCreatedAt = followers[i].createdAt;
						}
					}

					twitchAlerts.lastFollowerNotification = latestCreatedAt;
					twitchAlerts.Save(false);

					return followers;
				} else {
					return null;
				}
			} catch(Exception e) {
				Log.exception(e);

				return null;
			}
		}

		static SqlTwitchUserInChannel FromSql(TwitchChannel channel, object[] results) {
			if(results != null && results.Length > 0) {
				if(results.Length > 4) {
					return new SqlTwitchUserInChannel(new SqlTwitchUser(results[0].FromSql<uint>(), results[4].FromSql<string>()), channel, results[2].FromSql<DateTime>(), results[3].FromSql<bool>());
				} else {
					return new SqlTwitchUserInChannel(new SqlTwitchUser(results[0].FromSql<uint>()), channel, results[2].FromSql<DateTime>(), results[3].FromSql<bool>());
				}
			} else {
				return null;
			}
		}
	}
}
