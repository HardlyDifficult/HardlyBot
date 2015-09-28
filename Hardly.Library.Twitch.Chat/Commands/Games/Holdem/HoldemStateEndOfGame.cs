using Hardly.Games;

namespace Hardly.Library.Twitch {
	class HoldemStateEndOfGame : GameState<TwitchHoldem> {
		public HoldemStateEndOfGame(TwitchHoldem controller) : base(controller) {
		}

        internal override void Open() {
            base.Open();

            string chatMassege = "Holdem: ";

            chatMassege += BuildMessage(true, "won!") ?? "";
            chatMassege += BuildMessage(false, "lost.") ?? "";
            string sideWinners = BuildMessage(true, "won the sidepot", true);
            string sideLosers = BuildMessage(false, "lost the sidepot", true);
            if(sideWinners != null || sideLosers != null) {
                chatMassege += " ...And for the sidepot: ";
                chatMassege += sideWinners ?? "";
                chatMassege += sideLosers ?? "";
            }

            controller.room.SendChatMessage(chatMassege);
        }

        private string BuildMessage(bool isWinner, string message, bool sidePot = false) {
            string chatMassege = "";

            bool first = true;
            foreach(var player in 
                sidePot ?
                    (isWinner ? controller.game.lastGameSidepotWinners : controller.game.lastGameSidepotLosers)
                    : (isWinner ? controller.game.lastGameWinners : controller.game.lastGameLosers)) {
                if(!first) {
                    chatMassege += ", ";
                }

                chatMassege += player.Item1.playerIdObject.name;
                chatMassege += " ";
                chatMassege += player.Item1.hand.ToChatString();
                chatMassege += " (";
                chatMassege += PokerPlayerHand.GetHandType(player.Item2).ToString();
                chatMassege += ")";
            }
            if(chatMassege.Length > 0) {
                chatMassege += " " + message;
                return chatMassege;
            } else {
                return null;
            }
        }
    }
}