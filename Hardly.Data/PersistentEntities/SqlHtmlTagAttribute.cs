using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly {
	public class SqlHtmlTagAttribute : SqlRow {
		SqlHtmlTagUse _tagUse = null;
		SqlHtmlTagAttributeValue[] _possibleValues = null;

		internal static readonly SqlTable _table = new SqlTable("html_tag_attributes");
		public override SqlTable table {
			get {
				return _table;
			}
		}

		SqlHtmlTagAttribute(object[] values)
			 : base(values) {
		}

		public SqlHtmlTagAttribute(string name, SqlHtmlTagUse tagUse = null, bool isGlobal = false, string description = null, string syntax = null, string remarks = null, string excludeIf = null, string specUrl = null)
			 : base(new object[] { name, tagUse?.Id, isGlobal, description, syntax, remarks, excludeIf, specUrl }) {
			_tagUse = tagUse;
		}

		internal static SqlHtmlTagAttribute[] ForTag(SqlHtmlTag tag) {
			object[][] results = _table.Select("join html_tag_attribute_connections on html_tag_attribute_connections.AttributeName=Name", null, "TagName=?a", new[] { tag.name }, "IsGlobal, UseId, Name", 0);
			if(results != null && results.Length > 0) {
				SqlHtmlTagAttribute[] attributes = new SqlHtmlTagAttribute[results.Length];
				for(int i = 0; i < results.Length; i++) {
					attributes[i] = new SqlHtmlTagAttribute(results[i]);
				}

				return attributes;
			}

			return null;
		}
		

		public string name {
			get {
				return Get<string>(0);
			}
			set {
				Set(0, value);
			}
		}

		public SqlHtmlTagUse tagUse {
			get {
				if(_tagUse == null) {
					ushort id = Get<ushort>(1);
					if(id > 0) {
						_tagUse = new SqlHtmlTagUse(id);
					}
				}

				return _tagUse;
			}
			set {
				_tagUse = value;
				Set(1, _tagUse.Id);
			}
		}

		public bool isGlobal {
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
		
		public string excludeIf {
			get {
				return Get<string>(5);
			}
			set {
				Set(5, value);
			}
		}

		public string specUrl {
			get {
				return Get<string>(6);
			}
			set {
				Set(6, value);
			}
		}

		public SqlHtmlTagAttributeValue[] possibleValues {
			get {
				if(_possibleValues == null) {
					_possibleValues = SqlHtmlTagAttributeValue.ForAttribute(this);
				}

				return _possibleValues;
			}
		}
	}
}
