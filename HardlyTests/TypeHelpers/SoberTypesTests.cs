using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class SoberTypesTests {
		[TestMethod()]
		public void IsDefaultValueTest() {
			DBNull dbnull = DBNull.Value;
			Assert.IsTrue(dbnull.IsDefaultValue());
		}

		[TestMethod()]
		public void GetAllSubclassesTest() {
			var classes = typeof(SqlAuthor).GetAllSubclassesInThisAssumbly<DataList>(true);
			Assert.IsTrue(classes.Contains(typeof(SqlDomain)));
		}
	}
}