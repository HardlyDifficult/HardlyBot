using System;

namespace Hardly.Library.Twitch {
	class HoldemStatePlayTurn : HoldemStatePlay {
		public HoldemStatePlayTurn(TwitchHoldem controller) : base(controller) {
            string chatMessage = "Holdem" + OnTheTable() + " ";
            bool first = true;
            foreach(var player in controller.game.seatedPlayers) {
                if(!first) {
                    chatMessage += ", ";
                }
                first = false;
                chatMessage += player.idObject.name;
            }
            chatMessage += " are playing, the turn card is " + controller.game.tableCards.Last + " -- Board: " + controller.game.tableCards.ToString();
            controller.room.SendChatMessage(chatMessage);
        }
    }
}