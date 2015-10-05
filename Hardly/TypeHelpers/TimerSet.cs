using System;

namespace Hardly {
	public class TimerSet {
		public bool isRunning {
            get;
            private set;
        }
		TimerBase[] timers;
		Action[] actions;
		
		public TimerSet(TimeSpan[] timeSpan, Action[] action) {
            isRunning = false;

			if(timeSpan != null && action != null && timeSpan.Length == action.Length) {
				this.actions = action;
				timers = new TimerBase[timeSpan.Length];
				for(uint i = 0; i < timers.Length; i++) {
					timers[i] = new Timer<uint>(timeSpan[i], Tick, i);
				}
			} else {
				Debug.Fail();
			}
		}

		private void Tick(uint i) {
			actions[i]();
			if(i + 1 < timers.Length) {
				timers[i + 1].Start();
			}
		}

		public void Start() {
			if(!isRunning && timers != null && timers.Length > 0) {
				isRunning = true;
				timers[0].Start();
			}
		}

		public TimeSpan TimeRemaining() {
			TimeSpan timeRemaining = TimeSpan.FromSeconds(0);
			bool foundActiveTimer = !isRunning;
			for(int i = 0; i < timers.Length; i++) {
				if(foundActiveTimer || timers[i].IsRunning()) {
					foundActiveTimer = true;
					timeRemaining += timers[i].TimeRemaining();
				}
			}

			return timeRemaining;
		}

		public void Stop() {
            isRunning = false;

            if(timers != null && timers.Length > 0) {
				foreach(var timer in timers) {
					timer.Stop();
				}
			}
		}
    }
}
