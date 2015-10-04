using System;

namespace Hardly {
    public class TimeSpan {
        System.TimeSpan timeSpan;

        public TimeSpan(long milliseconds) {
            this.timeSpan = System.TimeSpan.FromMilliseconds(milliseconds);
        }

        public static TimeSpan FromMinutes(long minutes) {
            return new TimeSpan(minutes * 60000);
        }

        public long TotalMilliseconds {
            get {
                return (long)timeSpan.TotalMilliseconds;
            }
        }

        public int Days {
            get {
                return timeSpan.Days;
            }
        }
        public int Hours {
            get {
                return timeSpan.Hours;
            }
        }
        public int Minutes {
            get {
                return timeSpan.Minutes;
            }
        }
        public int Seconds {
            get {
                return timeSpan.Seconds;
            }
        }

        public long TotalSeconds {
            get {
                return (long)timeSpan.TotalSeconds;
            }
        }

        public static TimeSpan FromSeconds(long seconds) {
            return new TimeSpan(seconds * 1000);
        }

        internal static TimeSpan FromMilliseconds(double milliseconds) {
            return FromMilliseconds((long)milliseconds);
        }
        internal static TimeSpan FromMilliseconds(long milliseconds) {
            return new TimeSpan(milliseconds);
        }

        public static implicit operator TimeSpan(System.TimeSpan span) {
            return new TimeSpan((long)span.TotalMilliseconds);
        }

        public static bool operator <(TimeSpan span1, TimeSpan span2) {
            return span1.TotalMilliseconds < span2.TotalMilliseconds;
        }
        public static bool operator >(TimeSpan span1, TimeSpan span2) {
            return span1.TotalMilliseconds > span2.TotalMilliseconds;
        }
        public static TimeSpan operator -(TimeSpan span1, TimeSpan span2) {
            return new TimeSpan(span1.TotalMilliseconds - span2.TotalMilliseconds);
        }
        public static TimeSpan operator +(TimeSpan span1, TimeSpan span2) {
            return new TimeSpan(span1.TotalMilliseconds + span2.TotalMilliseconds);
        }
    }
}