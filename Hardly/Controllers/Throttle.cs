using System;
using System.Collections.Generic;

namespace Hardly {
	public class Throttle {
		double timeBetweenActions;
		readonly Dictionary<ulong, DateTime> lastRequestTime = new Dictionary<ulong, DateTime>();

		public Throttle(TimeSpan timeBetweenActions) {
			Debug.Assert(timeBetweenActions.TotalMilliseconds > 0);

			this.timeBetweenActions = timeBetweenActions.TotalMilliseconds;
		}

		public bool ExecuteIfReady(ulong key) {
			bool ready = TimeToSleepFor(key) <= 0;

			if(ready) {
				lastRequestTime[key] = DateTime.Now;
			}

			return ready;
		}

		public void SleepTillReady(ulong key) {
			int timeToSleep = TimeToSleepFor(key);

			if(timeToSleep > 0) {
				Thread.SleepInMilliseconds(timeToSleep);
			}

			lastRequestTime[key] = DateTime.Now;
		}

		int TimeToSleepFor(ulong key) {
			DateTime lastRequest;
			int timeSpan = (int)timeBetweenActions;

			if(lastRequestTime.TryGetValue(key, out lastRequest)) {
				timeSpan = (int)(DateTime.Now - lastRequest).TotalMilliseconds;
			}

			return (int)timeBetweenActions - timeSpan;
		}

		public TimeSpan TimeRemaining() {
			throw new NotImplementedException();
		}
	}
}
