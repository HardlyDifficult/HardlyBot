using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class ArrayHelpersTests {
		[TestMethod()]
		public void ArraySubArrayTest() {
			string[] test = { "a", "b", "c", "d" };
			Assert.AreEqual(test.SubArray(1)[0], "b");
			Assert.AreEqual(test.SubArray(1).Length, 3);
			Assert.AreEqual(test.SubArray(1, 3)[0], "b");
			Assert.AreEqual(test.SubArray(1, 99)[0], "b");
			Assert.AreEqual(test.SubArray(2, 1)[0], "c");
			Assert.AreEqual(test.SubArray(3)[0], "d");
			Assert.AreEqual(test.SubArray(4).Length, 0);
			Assert.AreEqual(test.SubArray(99), null);
		}

		[TestMethod()]
		public void ArrayMergeTest() {
			Assert.AreEqual(new[] { "a", "b" }.Append(new[] { "c" })[2],
				"c");
			Assert.AreEqual(new[] { "a" }.Append(new[] { "b", "c" })[0],
				"a");
			Assert.AreEqual(new string[] { }.Append(new[] { "a", "b", "c" })[1],
				 "b");
			Assert.AreEqual(new[] { "a", "b", "c" }.Append(new string[] { })[1],
				 "b");
		}

		[TestMethod()]
		public void ArrayToArrayTest() {
			LinkedList<string> list = new LinkedList<string>();
			list.AddLast("a");
			list.AddLast("b");
			Assert.AreEqual(list.ToArray()[1], "b");
		}

		[TestMethod()]
		public void ContainsTest() {
			string[] list = new string[] { "a", "b", "c" };
			Assert.IsTrue(list.Contains("a"));
			Assert.IsTrue(list.Contains("b"));
			Assert.IsTrue(list.Contains("c"));
			Assert.IsTrue(!list.Contains("d"));
			Assert.IsTrue(!list.Contains(""));
			Assert.IsTrue(!list.Contains(null));

			list = new string[] { "a", "b", null, "c" };
			Assert.IsTrue(list.Contains("a"));
			Assert.IsTrue(list.Contains("b"));
			Assert.IsTrue(list.Contains("c"));
			Assert.IsTrue(!list.Contains("d"));
			Assert.IsTrue(!list.Contains(""));
			Assert.IsTrue(list.Contains(null));

			list = new string[] { "a", "b", "", "c" };
			Assert.IsTrue(list.Contains("a"));
			Assert.IsTrue(list.Contains("b"));
			Assert.IsTrue(list.Contains("c"));
			Assert.IsTrue(!list.Contains("d"));
			Assert.IsTrue(list.Contains(""));
			Assert.IsTrue(!list.Contains(null));
		}

		[TestMethod()]
		public void DuplicateEntitiesTest() {
			string[] list = new string[] { "a", "b", "", "c" };
			var list2 = list.DuplicateEntities(1);
			Assert.IsTrue(list2.Length == 4 * 2);
			var list3 = list.DuplicateEntities(3);
			Assert.IsTrue(list3.Length == 4 * 4);
		}

		[TestMethod()]
		public void ShuffleTest() {
			string[] list = new string[] { "a", "b", "c" };
			bool orderChanged = false;

			for(int i = 0; i < 100; i++) {
				list = list.Shuffle();
				if(!list.GetValue(0).Equals("a")) {
					orderChanged = true;
					break;
				}
			}

			Assert.IsTrue(orderChanged);
		}
	}
}