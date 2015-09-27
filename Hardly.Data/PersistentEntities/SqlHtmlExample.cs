using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly {
	public class SqlHtmlExample : SqlRow {
		SqlHtmlExample(object[] data) : base(data) {
		}
		public SqlHtmlExample(string name, string title, string description, string html, string remarks) : this(new object[] { name, title, description, html, remarks }) {
		}

		public static SqlHtmlExample[] ForAttributeValue(SqlHtmlTag tag, SqlHtmlTagAttributeValue attributeValue) {
			return fromSql(_table.Select("join html_tag_attribute_value_examples on ExampleName=Name", null, "TagName=?a and AttributeName=?b and ValueSyntax=?c", new object[] { tag.name, attributeValue.attribute.name, attributeValue.valueSyntax }, "OrderId", 0));
		}

		internal static SqlHtmlExample[] ForTag(SqlHtmlTag tag) {
			return fromSql(_table.Select("join html_tag_examples on ExampleName=Name", null, "TagName=?a", new object[] { tag.name }, "OrderId", 0));
		}

		static SqlHtmlExample[] fromSql(object[][] exampleObjects) {
			if(exampleObjects != null) {
				SqlHtmlExample[] examples = new SqlHtmlExample[exampleObjects.Length];
				for(int i = 0; i < exampleObjects.Length; i++) {
					examples[i] = new SqlHtmlExample(exampleObjects[i]);
				}

				return examples;
			}

			return null;
		}

		static SqlTable _table = new SqlTable("html_examples");
		public override SqlTable table {
			get {
				return _table;
			}
		}

		public string Name {
			get {
				return Get<string>(0);
			}
			set {
				Set(0, value);
			}
		}

		public string Title {
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

		public string Html {
			get {
				return Get<string>(3);
			}
			set {
				Set(3, value);
			}
		}

		public string Remarks {
			get {
				return Get<string>(4);
			}
			set {
				Set(4, value);
			}
		}
	}
}
