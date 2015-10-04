using System;
using System.Timers;

namespace Hardly {
    public abstract class TimerBase {
        protected readonly System.Timers.Timer timer;
        DateTime startTime = DateTime.MinValue;

        public TimerBase(TimeSpan timeSpan) {
            this.timer = new System.Timers.Timer(timeSpan.TotalMilliseconds);
            this.timer.AutoReset = false;
        }

        public void Stop() {
            timer.Stop();
            startTime = DateTime.MinValue;
        }

        public bool IsRunning() {
            return timer.Enabled;
        }

        public void Start() {
            timer.Stop();
            startTime = DateTime.Now;
            timer.Start();
            Debug.Assert(timer.Enabled);
        }

        public TimeSpan TimeRemaining() {
            if(timer.Enabled && startTime != DateTime.MinValue) {
                TimeSpan timeElapsed = (DateTime.Now - startTime);
                if(timeElapsed > TimeSpan.FromMilliseconds(0)) {
                    return TimeSpan.FromMilliseconds(timer.Interval) - timeElapsed;
                } else {
                    return TimeSpan.FromMilliseconds(timer.Interval);
                }
            } else {
                return TimeSpan.FromMilliseconds(timer.Interval);
            }
        }

        public TimeSpan Interval() {
            return TimeSpan.FromMilliseconds(timer.Interval);
        }
    }

    public class Timer : TimerBase {
        Action timeUp;

        public Timer(TimeSpan timeSpan, Action timeUp) : base(timeSpan) {
            this.timeUp = timeUp;
            this.timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e) {
            timeUp();
        }
    }

    public class Timer<ParamType> : TimerBase {
        Action<ParamType> timeUp;
        ParamType param;

        public Timer(TimeSpan timeSpan, Action<ParamType> timeUp, ParamType param) : base(timeSpan) {
            this.timeUp = timeUp;
            this.timer.Elapsed += TimerElapsed;
            this.param = param;
        }

        void TimerElapsed(object sender, ElapsedEventArgs e) {
            timeUp(param);
        }
    }
}
