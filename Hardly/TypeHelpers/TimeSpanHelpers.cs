using System;

namespace Hardly {
	public static class TimeSpanHelpers {

		public static string ToSimpleString(this TimeSpan time) {
			string message;

			if(time.Days > 0) {
				message = time.Days + " days";
				if(time.Hours > 2) {
					message += " " + time.Hours + " hours";
				}
			} else if(time.Hours > 0) {
				message = time.Hours + " hours";
				if(time.Minutes > 2) {
					message += " " + time.Minutes + " mins";
				}
			} else if(time.Minutes > 0) {
				message = time.Minutes + " mins";
				if(time.Seconds > 2) {
					message += " " + time.Seconds + " secs";
				}
			} else {
				if(time.Seconds > 2) {
					message = time.Seconds + " secs";
				} else {
					message = "soon";
				}
			}

			return message;
		}
	}
}
