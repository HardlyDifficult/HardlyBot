namespace Hardly {
	public class SqlCollection : SqlRow {
		public SqlCollection(ulong id, string name = null)
			 : base(new object[] { id, name }) {
		}

		internal static readonly SqlTable _table = new SqlTable("collections");
		public override SqlTable table {
			get {
				return _table;
			}
		}

		public ulong id {
			get {
				return Get<ulong>(0);
			}
		}

		public string name {
			get {
				return Get<string>(1);
			}
			set {
				Set(1, value);
			}
		}
	}
}
