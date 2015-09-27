using System;

namespace Hardly.Library.Twitch {
	public class TwitchPickANumber : TwitchCommandController {
		ChatCommand pickCommand;

		public TwitchPickANumber(TwitchChatRoom room) : base(room) {
			pickCommand = ChatCommand.Create(room, "pick", PickCommand, "Pick a number 1-10... !pick <guess> for <bet amount>.", null, false, TimeSpan.FromSeconds(10), true);
		}

		private void PickCommand(SqlTwitchUser speaker, string message) {
			uint pickedNumber;
			string numberString = message.GetBefore(" ");
			if(numberString == null) {
				numberString = message;
			}

			if(uint.TryParse(numberString, out pickedNumber)) {
				if(pickedNumber >= 0 && pickedNumber <= 10) {
					message = message.GetAfter(" for");
					ulong bet = 1;
					if(message != null) {
						bet = pointManager.GetPointsFromString(message);
					}

					UserPointManager userPoints = pointManager.ForUser(speaker);
					bet = userPoints.ReserveBet(bet);
					if(bet > 0) {
						uint myNumber = Random.Uint.LessThan(11);
						string chatMessage = "I guessed " + myNumber + "... ";
						if(myNumber.Equals(pickedNumber)) {
							userPoints.Award(bet, (long)bet * 10);
							chatMessage += speaker.name + " won! " + pointManager.ToPointsString(bet * 10);
							room.SendChatMessage(chatMessage);
						} else {
							userPoints.Award(bet, (long)bet * -1L);
							chatMessage += "You lost... " + pointManager.ToPointsString(bet);
							room.SendWhisper(speaker, chatMessage);
						}
					} else {
						room.SendWhisper(speaker, "You're broke.");
					}
				} else {
					room.SendWhisper(speaker, "Pick a number 0 to 10, inclusive.");
				}
			}
		}
	}
}
