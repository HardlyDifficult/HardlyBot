namespace Hardly.Random {
	public abstract class RandomHelper {
		protected static System.Random random = new System.Random();
	}

	public class Uint : RandomHelper {
		public static uint LessThan(uint max) {
			return (uint)random.Next(0, (int)max);
		}
	}

	public class String : RandomHelper {
		public static string CharsAndNumbers(uint length) {
			uint rand = Uint.LessThan(26 + 26 + 10);

			string value;
			if(rand >= 26 + 26) {
				value = (rand - 26 - 26).ToString();
			} else if(rand >= 26) {
				value = ((char)('A' + (rand - 26))).ToString();
			} else {
				value = ((char)('a' + Uint.LessThan(26))).ToString();
			}
			
			if(length > 1) {
				value += CharsAndNumbers(length - 1);
			}

			return value;
		}

		public static string LowerCaseChars(uint length) {
			string value = ((char)('a' + Uint.LessThan(26))).ToString();
			if(length > 1) {
				value += LowerCaseChars(length - 1);
			}

			return value;
		}
	}
}
