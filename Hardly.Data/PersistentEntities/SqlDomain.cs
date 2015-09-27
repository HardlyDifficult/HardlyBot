namespace Hardly {
	public class SqlDomain : SqlRow {
		public SqlDomain(ulong id, string name = null, Regex domainAliases = null)
			 : base(new object[] { id, name, domainAliases?.ToString() }) {
		}

		internal static readonly SqlTable _table = new SqlTable("domains");
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

		public Regex domainAliases {
			get {
				return new Regex(Get<string>(2));
			}
			set {
				Set(2, value.ToString());
			}
		}
	}
}
