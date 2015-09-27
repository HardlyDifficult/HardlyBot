using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hardly.Tests {
	[TestClass()]
	public class SqlCollectionTests {
		[TestMethod()]
		public void SqlCollectionTest() {
			Assert.AreEqual(new SqlCollection(1).name, "Hardly Sober Fan Art");
		}
	}
}