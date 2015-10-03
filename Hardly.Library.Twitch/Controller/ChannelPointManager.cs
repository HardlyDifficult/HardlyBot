using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch {
	public class ChannelPointManager {
		public class PointUnit {
			public readonly string nameSingular, namePlural;
			public readonly ulong value;

			public PointUnit(string nameSingular, string namePlural, ulong value) {
				this.nameSingular = nameSingular;
                this.namePlural = namePlural;
				this.value = value;
			}
		}

		readonly SqlTwitchChannel channel;
		readonly PointUnit[] units;
		Dictionary<SqlTwitchUser, TwitchUserPointManager> userManagers = new Dictionary<SqlTwitchUser, TwitchUserPointManager>();

		public ChannelPointManager(SqlTwitchChannel channel) {
			this.channel = channel;
			units = LoadUnits(channel);			
		}

		static PointUnit[] LoadUnits(SqlTwitchChannel channel) {
			SqlTwitchChannelPointScale[] points = SqlTwitchChannelPointScale.ForChannel(channel);
			if(points != null && points.Length > 0) {
				PointUnit[] units = new PointUnit[points.Length];

				for(int i = 0; i < points.Length; i++) {
					units[i] = new PointUnit(points[i].unitNameSingular, points[i].unitNamePlural, points[i].unitValue);
				}

				return units;
			}

			return null;
		}
		
		public ulong GetPointsFromString(string message) {
			message = message?.Trim();
			ulong pointsValue = 0;

			if(message != null) {
				if(message.StartsWith("all", StringComparison.CurrentCultureIgnoreCase)) {
					return ulong.MaxValue;
				} else {
					do {
						double numberValue;
						string numberString = message.GetBefore(" ")?.Trim();
						if(numberString == null) {
							numberString = message;
						}
						if(!double.TryParse(numberString, out numberValue)) {
							numberValue = (ulong)(pointsValue == 0 ? 1 : 0);
						} else {
							message = message.GetAfter(" ")?.Trim();
						}

						string numberUnit = message.GetBefore(" ")?.Trim();
						if(numberUnit == null) {
							numberUnit = message;
							message = null;
						} else {
							message = message.GetAfter(" ")?.Trim();
						}

						PointUnit unit = units[0];
						if(numberUnit != null) {
							for(uint i = 0; i < units.Length; i++) {
								if(numberUnit.Equals(units[i].nameSingular, StringComparison.CurrentCultureIgnoreCase) || numberUnit.Equals(units[i].namePlural, StringComparison.CurrentCultureIgnoreCase)) {
									unit = units[i];
									if(numberValue == 0) {
										numberValue = 1;
									}
									break;
								}
							}
						}

						pointsValue += (ulong)(numberValue * unit.value);
					} while(message != null);

					return pointsValue;
				}
			} else {
				return 1;
			}
		}

		public string GetAboutPoints() {
			string description = "";

			for(int i = 0; i < units.Length; i++) {
				if(i > 0) {
					description += " == 1 ";
                    description += units[i].nameSingular;
                }
                if(i < units.Length - 1) {
                    if(i > 0) {
                        description += ", ";
                    }
                    description += ((double)units[i + 1].value / units[i].value).ToStringWithCommaAndDecimals(1);
                    description += " ";
                    description += units[i].namePlural;
                } 
			}

			return description;
		}
		
		public string ToPointsString(ulong points, bool justTopDollar = false) {
			string value = "";
			ulong pointValue = points;
			for(int i = units.Length - 1; i >= 0; i--) {
				ulong unitMin = units[i].value;
				if(pointValue >= unitMin) {
					double unitValue = (double)pointValue / unitMin;
					if(unitValue > 0) {
						if(justTopDollar) {
							value += unitValue.ToStringWithCommaAndDecimals(1) + " " + (unitValue == 1 ? units[i].nameSingular : units[i].namePlural) + " ";
							break;
						} else {
							pointValue -= (ulong)unitValue * unitMin;

							value += (ulong)unitValue + " " + (unitValue == 1 ? units[i].nameSingular : units[i].namePlural);
						}
					}
				}
			}

			if(value.IsEmpty()) {
				value = "0 " + units[0].namePlural;
			}

			return value;
		}

		public TwitchUserPointManager ForUser(SqlTwitchUser user) {
			TwitchUserPointManager manager;
			if(!userManagers.TryGetValue(user, out manager)) {
				manager = new TwitchUserPointManager(channel, user);
				userManagers.Add(user, manager);
			}

			return manager;
		}
	}
}
