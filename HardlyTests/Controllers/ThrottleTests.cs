using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class ThrottleTests {
		[TestMethod()]
		public void ThrottleTest() {
			throttleTest(0);
		}

		private void throttleTest(uint id) {
			TimeSpan spanWithoutThrottle = calcTimeSpan(null, id);

			Throttle throttle = new Throttle(TimeSpan.FromSeconds(1));
			TimeSpan spanWithThrottle = calcTimeSpan(throttle, id);

			Assert.IsTrue(spanWithThrottle > spanWithoutThrottle);
			Assert.IsTrue(spanWithThrottle > TimeSpan.FromSeconds(9));
		}

		static uint counter = 0;

		[TestMethod()]
		public void ThrottleIdTest() {
			DateTime start = DateTime.Now;

			Thread[] threads = new Thread[10];
			for(int i = 0; i < 10; i++) {
				threads[i] = new Thread(() => {
					throttleTest(counter++);
				});
			}

			threads.Run();

			TimeSpan span = DateTime.Now - start;
			Assert.IsTrue(span > TimeSpan.FromSeconds(9));
			Assert.IsTrue(span < TimeSpan.FromSeconds(90));
		}

		TimeSpan calcTimeSpan(Throttle t, uint id) {
			DateTime start = DateTime.Now;
			for(int i = 0; i < 10; i++) {
				t?.SleepTillReady(id);
			}

			return DateTime.Now - start;
		}
	}
}