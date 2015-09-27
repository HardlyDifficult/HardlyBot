using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class StringHelpersTests {
		[TestMethod()]
		public void AppendStringsTest() {
			string[] list = new[] { "a", "b" };
			Assert.AreEqual(list.AppendStrings(list)[0], "aa");
			Assert.AreEqual(list.AppendStrings(list)[1], "bb");
			Assert.AreEqual(list.AppendStrings(list).Length, 2);
			Assert.AreEqual(list.AppendStrings(list, "=")[0], "a=a");
		}

		[TestMethod()]
		public void StringGetAfterTest() {
			string message = "abcdef";
			Assert.AreEqual(message.GetAfter("c"), "def");
			Assert.AreEqual(message.GetAfter("f"), "");
			Assert.AreEqual(message.GetAfter("aoeu"), null);
			Assert.AreEqual(message.GetAfter("a"), "bcdef");
			Assert.AreEqual(message.GetAfter("cd"), "ef");
		}

		[TestMethod()]
		public void StringGetBeforeTest() {
			string message = "abcdef";
			Assert.AreEqual(message.GetBefore("c"), "ab");
			Assert.AreEqual(message.GetBefore("f"), "abcde");
			Assert.AreEqual(message.GetBefore("aoeu"), null);
			Assert.AreEqual(message.GetBefore("a"), "");
			Assert.AreEqual(message.GetBefore("de"), "abc");
		}

		[TestMethod()]
		public void StringGetBetweenTest() {
			string message = "abcdef";
			Assert.AreEqual(message.GetBetween("c", "f"), "de");
			Assert.AreEqual(message.GetBetween("f", "a"), null);
			Assert.AreEqual(message.GetBetween("c", "d"), "");
			Assert.AreEqual(message.GetBetween("a", "f"), "bcde");
		}

		[TestMethod()]
		public void StringGetNextLineTest() {
			string message = "a\r\n\r\nb\r\nc";
			byte[] bytes = message.ToBytesEncoded();
			uint index = 0;
			Assert.AreEqual(bytes.GetNextLine(ref index), "a");
			Assert.AreEqual(bytes.GetNextLine(ref index), "");
			Assert.AreEqual(bytes.GetNextLine(ref index), "b");
			Assert.AreEqual(bytes.GetNextLine(ref index), "c");
			Assert.AreEqual(bytes.GetNextLine(ref index), null);
		}

		[TestMethod()]
		public void StringToCsvTest() {
			string[] messages = new[] { "a", "b", "c" };
			Assert.AreEqual(messages.ToCsv(), "a,b,c");
			Assert.AreEqual(messages.ToCsv("-", "! ", " ."), "! a .-! b .-! c .");
		}

		[TestMethod()]
		public void StringIntToStringAzViaModTest() {
			Assert.AreEqual(((uint)0).ToStringAzViaMod(), "a");
			Assert.AreEqual(((uint)2).ToStringAzViaMod(), "c");
			Assert.AreEqual(((uint)25).ToStringAzViaMod(), "z");
			Assert.AreEqual(((uint)26).ToStringAzViaMod(), "aa");
			Assert.AreEqual(((uint)27).ToStringAzViaMod(), "ba");
			Assert.AreEqual(((uint)26).ToStringAzViaMod(true), "A");
		}

		[TestMethod()]
		public void StringBytesToStringEncodeDecodeTest() {
			string message = "abc";
			byte[] bytes = message.ToBytesEncoded();
			Assert.AreEqual(bytes.ToStringDecode(), message);
		}

		[TestMethod()]
		public void StringTokenizeTest() {
			string message = "a,b ,c";
			string[] tokens = message.Tokenize();
			Assert.IsTrue(tokens.Length == 3);
			Assert.AreEqual(tokens[0], "a");
			Assert.AreEqual(tokens[1], "b ");
			Assert.AreEqual(tokens[2], "c");

			tokens = message.Tokenize(",", true);
			Assert.IsTrue(tokens.Length == 3);
			Assert.AreEqual(tokens[0], "a,");
			Assert.AreEqual(tokens[1], "b ,");
			Assert.AreEqual(tokens[2], "c");
		}
	}
}