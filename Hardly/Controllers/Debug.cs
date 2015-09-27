using System;

namespace Hardly {
	public static class Debug {
		[System.Diagnostics.Conditional("DEBUG")]
		public static void AssertEquals(this object a, object b) {
			if(!(
				(a == null && b == null)
				|| (a != null && a.Equals(b))
				)) {
				System.Diagnostics.Debug.Assert(false);
			}
		}
		
		public static void Fail() {
			System.Diagnostics.Debug.Assert(false);
		}

		public static void Assert(bool result) {
			System.Diagnostics.Debug.Assert(result);
		}
	}
}
