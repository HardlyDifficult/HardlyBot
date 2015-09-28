namespace Hardly.Library.Twitch {
	class HoldemStatePlayTurn : HoldemStatePlay {
		public HoldemStatePlayTurn(TwitchHoldem controller) : base(controller) {
		}

        internal override void Open() {
            base.Open();
            controller.room.SendChatMessage("Holdem, time for that turn... " + controller.game.table.hand.ToChatString());
        }
    }
}