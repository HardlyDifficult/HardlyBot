using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hardly.Tests {
	[TestClass()]
	public class ThreadHelpersTests {
		static int counter = 0;

		[TestMethod()]
		public void ThreadWaitTillAllCompleteTest() {
			counter = 0;

			Thread[] threads = new Thread[100];
			for(int i = 0; i < threads.Length; i++) {
				threads[i] = new Thread(() => {
                        Thread.SleepInSeconds(Random.Uint.LessThan(3));
					Interlocked.Increment(ref counter);
				});
			}

			threads.Run();
			Assert.IsTrue(counter == threads.Length);
		}
	}
}