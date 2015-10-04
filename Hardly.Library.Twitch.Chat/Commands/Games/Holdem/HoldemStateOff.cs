using System;

namespace Hardly.Library.Twitch {
	internal class HoldemStateOff : GameState<TwitchHoldem> {
		public HoldemStateOff(TwitchHoldem controller) : base(controller) {
			AddCommand(controller.room, "holdem", HoldemCommand, "Starts a game of Texas Holdem", new[] { "texasholdem" }, false, null, false);
        }

        protected override void OpenState() {
            controller.game.Reset();
            controller.game.bigBlind = 2;
        }

        private void HoldemCommand(SqlTwitchUser speaker, string additionalText) {
			controller.SetState(this, typeof(HoldemStateAcceptingPlayers));
		}
	}
}