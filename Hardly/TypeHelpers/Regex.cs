using System;

namespace Hardly {
	public class Regex {
		public static readonly Regex AnyString = new Regex("[\\s\\S]*");
		public static readonly Regex EmptyString = new Regex("^$");

		System.Text.RegularExpressions.Regex regex;

		public Regex(string pattern) {
			if(pattern != null) {
				this.regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			} else {
				throw new ArgumentNullException();
			}
		}

		public bool IsMatch(string value) {
			if(value != null) {
				return regex.IsMatch(value);
			}

			return false;
		}
	}
}
