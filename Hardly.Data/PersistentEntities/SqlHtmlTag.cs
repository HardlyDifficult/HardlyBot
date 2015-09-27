namespace Hardly {
	public class SqlHtmlTag : SqlRow {
		SqlHtmlTagUse _tagUse = null;
		internal SqlHtmlExample[] _examples = null;
		internal SqlHtmlTagAttribute[] _attributes = null;

		public SqlHtmlTag(string name, string tagText = null, string shortDescription = null, string specUrl = null, SqlHtmlTagUse tagUse = null, string description = null, string semanticBenefits = null, string defaultCss = null, string defaultCssSpecUrl = null, string domInterface = null, string format = null)
			 : base(new object[] { name, tagText, description, specUrl, tagUse?.Id, description, semanticBenefits, defaultCss, defaultCssSpecUrl, domInterface, format }) {
			_tagUse = tagUse;
		}

		static SqlTable _table = new SqlTable("html_tags");
		public override SqlTable table {
			get {
				return _table;
			}
		}

		public string name {
			get {
				return Get<string>(0);
			}
			set {
				Set(0, value);
			}
		}

		public string TagText {
			get {
				return Get<string>(1);
			}
			set {
				Set(1, value);
			}
		}

		public string ShortDescription {
			get {
				return Get<string>(2);
			}
			set {
				Set(2, value);
			}
		}

		public string SpecUrl {
			get {
				return Get<string>(3);
			}
			set {
				Set(3, value);
			}
		}

		public SqlHtmlTagUse TagUse {
			get {
				if(_tagUse == null) {
					ushort id = Get<ushort>(4);
					if(id > 0) {
						_tagUse = new SqlHtmlTagUse(id);
					}
				}

				return _tagUse;
			}
			set {
				_tagUse = value;
				Set(4, _tagUse.Id);
			}
		}

		public string Description {
			get {
				return Get<string>(5);
			}
			set {
				Set(5, value);
			}
		}

		public string SemanticBenefits {
			get {
				return Get<string>(6);
			}
			set {
				Set(6, value);
			}
		}

		public string DefaultCss {
			get {
				return Get<string>(7);
			}
			set {
				Set(7, value);
			}
		}

		public string DefaultCssSpecUrl {
			get {
				return Get<string>(8);
			}
			set {
				Set(8, value);
			}
		}

		public string DomInterface {
			get {
				return Get<string>(9);
			}
			set {
				Set(9, value);
			}
		}

		public string Syntax {
			get {
				return Get<string>(10);
			}
			set {
				Set(10, value);
			}
		}

		public SqlHtmlExample[] examples {
			get {
				if(_examples == null) {
					_examples = SqlHtmlExample.ForTag(this);
				}

				return _examples;
			}
		}

		public SqlHtmlTagAttribute[] attributes {
			get {
				if(_attributes == null) {
					_attributes = SqlHtmlTagAttribute.ForTag(this);
				}

				return _attributes;
			}
		}
	}
}
