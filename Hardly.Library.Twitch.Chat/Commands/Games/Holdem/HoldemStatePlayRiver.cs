namespace Hardly.Library.Twitch {
	class HoldemStatePlayRiver : HoldemStatePlay {
		public HoldemStatePlayRiver(TwitchHoldem controller) : base(controller) {
		}

        internal override void Open() {
            string chatMessage = "Holdem" + OnTheTable()+ " ";
            bool first = true;
            foreach(var player in controller.game.seatedPlayers) {
                if(!first) {
                    chatMessage += ", ";
                }
                first = false;
                chatMessage += player.idObject.name;
            }
            chatMessage += " are playing, the river card is " + controller.game.tableCards.Last + " -- Board: " + controller.game.tableCards.ToString();

            controller.room.SendChatMessage(chatMessage);
            base.Open();
        }
    }
}