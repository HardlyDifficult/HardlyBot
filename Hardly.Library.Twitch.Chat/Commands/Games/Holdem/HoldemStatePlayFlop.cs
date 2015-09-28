namespace Hardly.Library.Twitch {
	class HoldemStatePlayFlop : HoldemStatePlay {
		public HoldemStatePlayFlop(TwitchHoldem controller) : base(controller) {
		}

        internal override void Open() {
            base.Open();

            controller.room.SendChatMessage("Holdem, time for that flop... " + controller.game.table.hand.ToChatString());
        }
    }
}