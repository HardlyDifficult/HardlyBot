using Hardly.Games;

namespace Hardly.Library.Twitch {
    public class TwitchPickANumber : TwitchGame<PickANumberGame<TwitchUser>> {
        public TwitchPickANumber(TwitchChatRoom room) : base(room) {
            ChatCommand.Create(room, "pick", PickCommand, "Pick a number 1-10... !pick <guess> for <bet amount>.", null, false, TimeSpan.FromSeconds(10), true);
        }

        void PickCommand(TwitchUser speaker, string message) {
            game.Reset();

            uint pickedNumber;
            string numberString = message?.GetBefore(" ");
            if(numberString == null) {
                numberString = message;
            }

            if(uint.TryParse(numberString, out pickedNumber)) {
                var playerObject = new PickANumberPlayer<TwitchUser>(room.pointManager.ForUser(speaker), speaker, pickedNumber);
                if(game.Join(playerObject)) {
                    message = message?.GetAfter(" for");
                    ulong bet = 1;
                    if(message != null) {
                        bet = room.pointManager.GetPointsFromString(message);
                    }

                    if(playerObject.PlaceBet(bet, false) > 0) {
                        if(game.StartGame()) {
                            string chatMessage = "I guessed " + game.rolledNumber + "... ";
                            if(playerObject.isWinner.GetValueOrDefault(false)) {
                                Debug.Assert(playerObject.winningsOrLosings >= 1);
                                chatMessage += speaker.name + " won! " + room.pointManager.ToPointsString((ulong)playerObject.winningsOrLosings);
                                room.SendChatMessage(chatMessage);
                            } else {
                                Debug.Assert(playerObject.winningsOrLosings <= -1);
                                chatMessage += "You lost... " + room.pointManager.ToPointsString((ulong)(-1 * playerObject.winningsOrLosings));
                                room.SendWhisper(speaker, chatMessage);
                            }
                        }
                    }
                }
            }
        }
    }
}