using System;
using System.Collections.Generic;
using System.Text;

namespace Hardly {
	public static class StringHelpers {
		static char[] AtoZLowercase = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
		static char[] AtoZUppercase = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
		static char[] AtoZBothcases = AtoZLowercase.Append(AtoZUppercase);
		static char[] Numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
		static char[] AtoZLowercaseWithNumbers = AtoZLowercase.Append(Numbers);
		static char[] AtoZUppercaseWithNumbers = AtoZUppercase.Append(Numbers);
		static char[] AtoZBothcasesWithNumbers = AtoZBothcases.Append(Numbers);

		public static string[] AppendStrings(this object[] source, string[] stringsToAppend, string textBetween = null) {
			if(source != null && stringsToAppend != null && source.Length == stringsToAppend.Length) {
				string[] mergedList = new string[source.Length];

				for(int i = 0; i < source.Length; i++) {
					if(i < stringsToAppend.Length) {
						mergedList[i] = source[i].ToString();
						if(textBetween != null) {
							mergedList[i] += textBetween;
						}
						mergedList[i] += stringsToAppend[i];
					} else {
						break;
					}
				}

				return mergedList;
			} else {
				Debug.Fail();
			}

			return null;
		}

		public static string GetAfter(this string source, string searchToken, bool firstOrLastInstance = true) {
			if(source != null) {
				if(searchToken != null && searchToken.Length > 0) {
					int i;
					if(firstOrLastInstance) {
						i = source.IndexOf(searchToken);
					} else {
						i = source.LastIndexOf(searchToken);
					}
					if(i >= 0) {
						i += searchToken.Length;
						if(i < source.Length) {
							return source.Substring(i);
						} else if(i == source.Length) {
							return "";
						}
					}

					return null;
				}

				Debug.Fail();
			}
			return null;
		}

		public static string GetBefore(this string source, string searchToken, bool firstOrLastInstance = true) {
			if(source != null) {
				if(searchToken != null && searchToken.Length > 0) {
					int i;
					if(firstOrLastInstance) {
						i = source.IndexOf(searchToken);
					} else {
						i = source.LastIndexOf(searchToken);
					}
					if(i >= 0) {
						if(i < source.Length) {
							return source.Substring(0, i);
						}
					}
				} else {
					Debug.Fail();
				}
			}

			return null;
		}

		public static string GetBetween(this string source, string searchToken1, string searchToken2, bool firstOrLastInstance1 = true, bool firstOrLastInstance2 = true) {
			if(source != null) {
				if(searchToken1 != null && searchToken1.Length > 0 && searchToken2 != null && searchToken2.Length > 0) {
					int iStart;
					if(firstOrLastInstance1) {
						iStart = source.IndexOf(searchToken1);
					} else {
						iStart = source.LastIndexOf(searchToken1);
					}
					if(iStart >= 0) {
						iStart += searchToken1.Length;
						int iEnd;
						if(firstOrLastInstance2) {
							iEnd = source.IndexOf(searchToken2, iStart);
						} else {
							iEnd = source.LastIndexOf(searchToken2);
						}
						if(iEnd > iStart) {
							return source.Substring(iStart, iEnd - iStart);
						} else if(iEnd == iStart) {
							return "";
						}
					}
				} else {
					Debug.Fail();
				}
			}

			return null;
		}

		public static string GetNextLine(this byte[] data, ref uint index) {
			if(index >= data.Length) {
				return null;
			}

			string line = "";

			do {
				char c = (char)data[index];
				if(c.Equals('\r')) {
					// do nothing, wait for \n
				} else if(c.Equals('\n')) {
					index++;
					break;
				} else {
					line += c;
				}
			} while(++index < data.Length);

			return line;
		}

		public static bool IsEmpty(this string value) {
			return value == null || value.Length == 0;
		}

		public static bool IsLowercase(this string value) {
			return value.Trim().Equals(value);
		}

		public static bool IsTrimmed(this string value) {
			return value.IsEmpty() || value.Trim().Equals(value);
		}

		public static string ToCsv(this object[] values, string insertBetweenEach = ",", string insertBeforeEachValue = null, string insertAfterEachValue = null) {
			string csv = "";

			for(int i = 0; i < values.Length; i++) {
				if(insertBetweenEach != null && i > 0) {
					csv += insertBetweenEach;
				}
				if(insertBeforeEachValue != null) {
					csv += insertBeforeEachValue;
				}

				csv += values[i];

				if(insertAfterEachValue != null) {
					csv += insertAfterEachValue;
				}
			}

			return csv;
		}

		public static string ToHtmlSafePlainText(this string value) {
			// TODO extend to cover all special chars, although this is enough for security
			return value?.Replace("<", "<wbr>&lt;").Replace(">", "><wbr>").Replace("=", "<wbr>=");
		}

		public static string ToHtmlSafeTagText(this string value) {
			// TODO extend to cover all special chars
			return value?.Replace("\"", "'");
		}

		public static string ToHtmlSafeUrl(this string value) {
			// TODO extend to cover all special chars
			return value?.Replace(" ", "%20");
		}
		
		public static string ToStringAzViaMod(this uint i, bool includeUppercase = false, bool includeNumbers = false, char[] includedSymbols = null) {
			char[] availableChars;
			if(includeUppercase && includeNumbers) {
				availableChars = AtoZBothcasesWithNumbers;
			} else if(includeUppercase) {
				availableChars = AtoZBothcases;
			} else if(includeNumbers) {
				availableChars = AtoZLowercaseWithNumbers;
			} else {
				availableChars = AtoZLowercase;
			}

			return ((int)i).ToStringAzViaMod(availableChars);
		}

		public static string ToStringAzViaMod(this int i, char[] availableChars) {
			string value = "";

			if(i >= 0) {
				int mod = i % availableChars.Length;
				value += availableChars[mod];
				if(i >= availableChars.Length) {
					value += (i - availableChars.Length - mod).ToStringAzViaMod(availableChars);
				}
			}

			return value;
		}

		public static string ToStringDecode(this byte[] data) {
			try {
				return Encoding.Default.GetString(data);
			} catch(Exception e) {
				Log.exception(e);

				return null;
			}
		}

		public static byte[] ToBytesEncoded(this string data) {
			try {
				return Encoding.UTF8.GetBytes(data);
			} catch(Exception e) {
				Log.exception(e);

				return null;
			}
		}

		public static string ToStringWithCommas(this int value) {
			return value.ToString("N0");
		}
		public static string ToStringWithCommas(this uint value) {
			return value.ToString("N0");
		}
		public static string ToStringWithCommas(this long value) {
			return value.ToString("N0");
		}
		public static string ToStringWithCommas(this ulong value) {
			return value.ToString("N0");
		}

		public static string ToStringWithCommaAndDecimals(this double value, uint numberOfDecimals) {
			if((long)value == value) {
				return value.ToString("N0");
			} else {
				return value.ToString("N" + numberOfDecimals);
			}
		}

		public static string[] Tokenize(this string source, string token = ",", bool includeToken = false) {
			if(token != null && token.Length > 0) {
				LinkedList<string> results = new LinkedList<string>();

				while(true) {
					string result = GetBefore(source, token);
					if(result == null) {
						break;
					}
					if(result.Length > 0) {
						if(includeToken) {
							result += token;
						}
						results.AddLast(result);
					}
					source = GetAfter(source, token);
				}

				if(includeToken && source.StartsWith(token)) {
					source += token;
				}
				results.AddLast(source);

				return results.ToArray();
			} else {
				Debug.Fail();
			}

			return null;
		}
	}
}