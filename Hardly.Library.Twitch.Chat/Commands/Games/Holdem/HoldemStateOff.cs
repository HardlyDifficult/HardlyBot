using System;

namespace Hardly.Library.Twitch {
	internal class HoldemStateOff : GameState<TwitchHoldem> {
		public HoldemStateOff(TwitchHoldem controller) : base(controller) {
			AddCommand(controller.room, "holdem", HoldemCommand, "Starts a game of Texas Holdem", new[] { "texasholdem" }, true, TimeSpan.FromSeconds(0), false);
		}

		private void HoldemCommand(SqlTwitchUser speaker, string additionalText) {
			controller.SetState(this.GetType(), typeof(HoldemStateAcceptingPlayers));
		}

		internal override void Open() {
			base.Open();
			controller.game.Reset();
            controller.game.bigBlind = 2;
        }
	}
}