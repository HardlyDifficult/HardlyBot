using System;

namespace Hardly.Library.Twitch {
	public class HoldemStatePlayPreFlop : HoldemStatePlay {
		public HoldemStatePlayPreFlop(TwitchHoldem controller) : base(controller) {
        }

        protected override void OpenState() {
            string chatMassege = "Texas Holdem has begun! ";
            bool first = true;
            foreach(var player in controller.game.seatedPlayers) {
                if(!first) {
                    chatMassege += ", ";
                }
                first = false;
                chatMassege += player.idObject.name;
            }
            chatMassege += " are playing.  Up first is " + controller.game.currentPlayer.idObject.name + ".";
            controller.room.SendChatMessage(chatMassege);

            foreach(var player in controller.game.seatedPlayers) {
                if(player.idObject.id != controller.game.currentPlayer.idObject.id) {
                    string playerMessage = "You have ";
                    playerMessage += player.hand.cards.ToString();
                    playerMessage += ".";
                    playerMessage += " Sit tight while others play.  I'll whisper again shortly.";
                    controller.room.SendWhisper(player.idObject, playerMessage);
                }
            }
        }
    }
}