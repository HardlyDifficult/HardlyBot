using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class TwitchPickANumber : TwitchGame<DiceGame<SqlTwitchUser>> {
		public TwitchPickANumber(TwitchChatRoom room) : base(room) {
			ChatCommand.Create(room, "pick", PickCommand, "Pick a number 1-10... !pick <guess> for <bet amount>.", null, false, TimeSpan.FromSeconds(10), true);
		}
        
        void PickCommand(SqlTwitchUser speaker, string message) {
            game.Reset();
            var playerObject = new GamePlayer<SqlTwitchUser>(room.pointManager.ForUser(speaker), speaker);
            if(game.Join(speaker, playerObject)) {
                if(game.StartGame()) {
                    uint pickedNumber;
                    string numberString = message?.GetBefore(" ");
                    if(numberString == null) {
                        numberString = message;
                    }
                    if(uint.TryParse(numberString, out pickedNumber)) {
                        if(pickedNumber >= 0 && pickedNumber <= 10) {
                            message = message?.GetAfter(" for");
                            ulong bet = 1;
                            if(message != null) {
                                bet = room.pointManager.GetPointsFromString(message);
                            }

                            if(playerObject.PlaceBet(bet, false) > 0) {
                                uint myNumber = Random.Uint.LessThan(11);
                                string chatMessage = "I guessed " + myNumber + "... ";
                                if(myNumber.Equals(pickedNumber)) {
                                    ulong winnings = playerObject.bet * 10UL;
                                    playerObject.Award((long)winnings);
                                    chatMessage += speaker.name + " won! " + room.pointManager.ToPointsString(winnings);
                                    room.SendChatMessage(chatMessage);
                                } else {
                                    playerObject.LoseBet();
                                    chatMessage += "You lost... " + room.pointManager.ToPointsString(bet);
                                    room.SendWhisper(speaker, chatMessage);
                                }
                            }
                        }
                    }
                }
            }
		}
	}
}
