using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly {
	public class SqlHtmlTagAttributeValue : SqlRow {
		SqlHtmlTagAttribute _attribute = null;

		internal static readonly SqlTable _table = new SqlTable("html_tag_attribute_values");
		public override SqlTable table {
			get {
				return _table;
			}
		}

		public SqlHtmlTagAttributeValue(SqlHtmlTagAttribute attribute, string valueSyntax, bool isPlaceholder, string description, string remarks)
			 : base(new object[] { attribute.name, valueSyntax, isPlaceholder, description, remarks }) {
			_attribute = attribute;
		}

		internal static SqlHtmlTagAttributeValue[] ForAttribute(SqlHtmlTagAttribute attribute) {
			object[][] results = _table.Select(null, null, "AttributeName=?a", new[] { attribute.name }, "ValueSyntax", 0);
			if(results != null && results.Length > 0) {
				SqlHtmlTagAttributeValue[] attributeValues = new SqlHtmlTagAttributeValue[results.Length];
				for(int i = 0; i < results.Length; i++) {
					attributeValues[i] = new SqlHtmlTagAttributeValue(attribute,
						results[i][1].FromSql<string>(),
						results[i][2].FromSql<bool>(),
						results[i][3].FromSql<string>(),
						results[i][4].FromSql<string>());
				}

				return attributeValues;
			}

			return null;
		}

		public SqlHtmlTagAttribute attribute {
			get {
				if(_attribute == null) {
					string name = Get<string>(0);
					if(name != null) {
						_attribute = new SqlHtmlTagAttribute(name);
					}
				}
				return _attribute;
			}
			set {
				_attribute = value;
				Set(0, _attribute?.name);
			}
		}

		public string valueSyntax {
			get {
				return Get<string>(1);
			}
			set {
				Set(1, value);
			}
		}

		public bool isPlaceholder {
			get {
				return Get<bool>(2);
			}
			set {
				Set(2, value);
			}
		}

		public string description {
			get {
				return Get<string>(3);
			}
			set {
				Set(3, value);
			}
		}

		public string remarks {
			get {
				return Get<string>(4);
			}
			set {
				Set(4, value);
			}
		}
	}
}
