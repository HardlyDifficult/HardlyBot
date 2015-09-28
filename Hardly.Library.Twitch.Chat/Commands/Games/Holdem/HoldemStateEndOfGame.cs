namespace Hardly.Library.Twitch {
	class HoldemStateEndOfGame : GameState<TwitchHoldem> {
		public HoldemStateEndOfGame(TwitchHoldem controller) : base(controller) {
		}

        internal override void Open() {
            base.Open();
            controller.room.SendChatMessage("Holdem, game over... winner: " + controller.game.lastGameWinners[0].playerIdObject.name);
        }
    }
}