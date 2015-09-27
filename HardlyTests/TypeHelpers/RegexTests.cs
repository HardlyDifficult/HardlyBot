using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class RegexTests {
		[TestMethod()]
		public void RegexAnyStringTest() {
			Assert.IsTrue(Regex.AnyString.IsMatch("hi"));
			Assert.IsTrue(Regex.AnyString.IsMatch(""));
			Assert.IsTrue(Regex.AnyString.IsMatch(" "));
			Assert.IsTrue(!Regex.AnyString.IsMatch(null));
		}

		[TestMethod()]
		public void RegexEmptyStringTest() {
			Assert.IsTrue(!Regex.EmptyString.IsMatch("hi"));
			Assert.IsTrue(Regex.EmptyString.IsMatch(""));
			Assert.IsTrue(!Regex.EmptyString.IsMatch(" "));
			Assert.IsTrue(!Regex.EmptyString.IsMatch(null));
		}
	}
}