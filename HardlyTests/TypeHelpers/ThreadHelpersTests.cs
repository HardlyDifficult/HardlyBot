using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class ThreadHelpersTests {
		static int counter = 0;

		[TestMethod()]
		public void ThreadWaitTillAllCompleteTest() {
			counter = 0;

			Thread[] threads = new Thread[10];
			for(int i = 0; i < threads.Length; i++) {
				threads[i] = new Thread(() => {
					Thread.SleepInSeconds(Random.Uint.LessThan(10));
					counter++;
				});
			}

			threads.Run();
			Assert.IsTrue(counter == 10);
		}
	}
}