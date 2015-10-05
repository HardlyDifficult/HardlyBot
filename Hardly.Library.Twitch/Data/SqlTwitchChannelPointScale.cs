namespace Hardly.Library.Twitch {
	public class SqlTwitchChannelPointScale : SqlRow {
		SqlTwitchChannel channel;

      static SqlTable _table = new SqlTable("twitch_channel_point_scale");
		public override SqlTable table {
			get {
				return _table;
			}
		}

		public SqlTwitchChannelPointScale(SqlTwitchChannel channel, ulong unitValue, string unitNameSingular = null, string unitNamePlural = null) : base(new object[] {
			channel.user.id, unitValue, unitNameSingular, unitNamePlural
		}) {
			this.channel = channel;
		}

		public static SqlTwitchChannelPointScale[] ForChannel(SqlTwitchChannel channel) {
			List<object[]> results = _table.Select(null, null, "ChannelUserId=?a", new object[] { channel.user.id }, "UnitValue", 0);

			if(results != null && results.Count > 0) {
				SqlTwitchChannelPointScale[] points = new SqlTwitchChannelPointScale[results.Count];
				for(int i = 0; i < results.Count; i++) {
					points[i] = new SqlTwitchChannelPointScale(channel,  results[i][1].FromSql<ulong>(), results[i][2].FromSql<string>(), results[i][3].FromSql<string>());
				}

				return points;
         }

			return null;
		}

		uint channelUserId {
			get {
				return Get<uint>(0);
			}
		}

		public ulong unitValue {
			get {
				return Get<ulong>(1);
			}
        }

        public string unitNameSingular {
            get {
                return Get<string>(2);
            }
            set {
                Set(2, value);
            }
        }

        public string unitNamePlural {
            get {
                return Get<string>(3);
            }
            set {
                Set(3, value);
            }
        }
    }
}
