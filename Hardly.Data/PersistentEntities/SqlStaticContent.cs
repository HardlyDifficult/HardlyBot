namespace Hardly {
	public class SqlStaticContent : SqlRow {
		public SqlStaticContent(ulong id, byte[] content = null, string contentType = null)
			 : base(new object[] { id, content, contentType }) {
		}

		internal static readonly SqlTable _table = new SqlTable("static_content");
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

		public byte[] content {
			get {
				return Get<byte[]>(1);
			}
			set {
				Set(1, value);
			}
		}

		public HttpContentType contentType {
			get {
				return new HttpContentType(Get<string>(2));
			}
			set {
				Set(2, value.ToString());
			}
		}

		public static SqlStaticContent Get(SqlDomain domain, string fileName) {
			object[] results = _table.Select("join domains_files on domains_files.ContentId=Id", null, "domains_files.DomainId=?a and Filename=?b", new object[] { domain.id, fileName }, null);

			if(results != null && results.Length > 0) {
				return new SqlStaticContent(results[0].FromSql<ulong>(), results[1].FromSql<byte[]>(), results[2].FromSql<string>());
			} else {
				return null;
			}
		}
	}
}
