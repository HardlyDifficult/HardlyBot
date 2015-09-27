using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class RandomTests {
		[TestMethod()]
		public void IntLessThanTest() {
			bool multipleValuesFound = false;
			uint? lastValueFound = null;
			uint maxValueFound = 0;
			uint minValueFound = 10;

			for(uint i = 0; i < 100; i++) {
				uint value = Random.Uint.LessThan(10);

				if(lastValueFound.HasValue) {
					if(lastValueFound.Value != value) {
						multipleValuesFound = true;
					}
				}
				lastValueFound = value;

				if(value > maxValueFound) {
					maxValueFound = value;
				}
				if(value < minValueFound) {
					minValueFound = value;
				}

				Assert.IsTrue(value < 10);
				Assert.IsTrue(value >= 0);
			}

			Assert.IsTrue(multipleValuesFound);
			Assert.IsTrue(minValueFound == 0);
			Assert.IsTrue(maxValueFound == 9);
		}

		[TestMethod()]
		public void StringLowerCaseCharsTest() {
			string value = Random.String.LowerCaseChars(10);
         Assert.IsTrue(value.Length == 10);
			Assert.IsTrue(new Regex("^[a-z]{10}$").IsMatch(value));
			Assert.IsTrue(value.IsLowercase());
		}
	}
}