namespace Hardly.Library.Twitch {
	class HoldemStatePlayFlop : HoldemStatePlay {
		public HoldemStatePlayFlop(TwitchHoldem controller) : base(controller) {
		}

        internal override void Open() {
            string chatMessage = "Holdem" + OnTheTable() + " ";
            bool first = true;
            foreach(var player in controller.game.seatedPlayers) {
                if(!first) {
                    chatMessage += ", ";
                }
                first = false;
                chatMessage += player.idObject.name;
            }
            chatMessage += " are playing, time for that flop... " + controller.game.tableCards.ToString();
            controller.room.SendChatMessage(chatMessage);

            base.Open();
        }
    }
}