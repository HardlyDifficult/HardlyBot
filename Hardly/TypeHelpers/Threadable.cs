using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly {
	public abstract class Threadable {
		bool _isRunning = false;
		System.Threading.Thread thread = null;

		public void Run() {
			_isRunning = true;
			Run_Blocking();
			_isRunning = false;
		}

		protected abstract void Run_Blocking();

		public void Start() {
			Start(Run);
		}

		public void Start(Action run) {
			Debug.Assert(!isRunning);

			thread = new System.Threading.Thread(new System.Threading.ThreadStart(run));
			thread.Start();
		}

		internal bool isRunning {
			get {
				return _isRunning || (thread != null && thread.IsAlive);
			}
		}
	}

	public static class ThreadHelpers {
		public static void Run(this Threadable[] threads) {
			if(threads != null) {
				for(int i = threads.Length - 1; i >= 0; i--) {
					if(i > 0) {
						threads[i].Start();
					} else {
						threads[i].Run();
					}
				}

				threads.WaitTillAllComplete();
			}
		}

		public static void WaitTillAllComplete(this Threadable[] threads) {
			bool end = false;
			while(!end) {
				end = true;
				foreach(Threadable thread in threads) {
					if(thread.isRunning) {
						end = false;
						break;
					}
				}

				if(!end) {
					Thread.SleepInSeconds(3);
				}
			}
		}
	}
}
