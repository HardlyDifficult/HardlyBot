using System;

namespace Hardly {
	public class SqlAuthor : SqlRow {
		public SqlAuthor(ulong id, string name = null)
			 : base(new object[] { id, name }) {
		}

		internal static readonly SqlTable _table = new SqlTable("authors");
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
