namespace Hardly.Library.Twitch {
	class HoldemStatePlayRiver : HoldemStatePlay {
		public HoldemStatePlayRiver(TwitchHoldem controller) : base(controller) {
		}

        internal override void Open() {
            base.Open();
            controller.room.SendChatMessage("Holdem, time for that river... " + controller.game.table.hand.ToChatString());
        }
    }
}