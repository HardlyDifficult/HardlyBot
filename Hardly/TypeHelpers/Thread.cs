using System;

namespace Hardly {
	public sealed class Thread : Threadable {
		readonly Action run;

		public static Thread StartNewThread(Action run) {
			Thread t = new Thread(run);
			t.Start();

			return t;
		}

		public Thread(Action run) {
			this.run = run;
		}

		protected override void Run_Blocking() {
			run();
		}

		public static void SleepInSeconds(int seconds) {
			SleepInSeconds((uint)seconds);
		}
		public static void SleepInSeconds(uint seconds) {
			SleepInMilliseconds(seconds * 1000);
		}

		public static void SleepInMilliseconds(int milliseconds) {
			SleepInMilliseconds((uint)milliseconds);
		}
		public static void SleepInMilliseconds(uint milliseconds) {
            try {
                System.Threading.Thread.Sleep((int)milliseconds);
            } catch { }
		}
	}
}
