namespace Hardly {
	public class SqlHtmlTagUse : SqlRow {
		public SqlHtmlTagUse(ushort id, string name = null, string description = null)
			 : base(new object[] { id, name, description }) {
		}

		static SqlTable _table = new SqlTable("html_tag_uses");
		public override SqlTable table {
			get {
				return _table;
			}
		}

		public ushort Id {
			get {
				return Get<ushort>(0);
			}
			set {
				Set(0, value);
			}
		}

		public string Name {
			get {
				return Get<string>(1);
			}
			set {
				Set(1, value);
			}
		}

		public string Description {
			get {
				return Get<string>(2);
			}
			set {
				Set(2, value);
			}
		}
	}
}
