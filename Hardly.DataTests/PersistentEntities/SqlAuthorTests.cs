using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class SqlAuthorTests {
		[TestMethod()]
		public void SqlAuthorTest() {
			SqlAuthor a1 = new SqlAuthor(1);
			SqlAuthor a2 = new SqlAuthor(0, "Hardly Sober");
			Assert.AreEqual(a1.id, a2.id);
			Assert.AreEqual(a1.name, a2.name);
		}
	}
}