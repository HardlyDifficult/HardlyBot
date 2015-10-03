using System;

namespace Hardly.Library.Twitch {
	public class BJStateOff : GameState<TwitchBlackjack> {
		Timer timer;

		public BJStateOff(TwitchBlackjack controller) : base(controller) {
			timer = new Timer(TimeSpan.FromSeconds(30), AutoStart);
			AddCommand(controller.room, "bj", BjCommand, "Starts a game of Blackjack", new[] { "blackjack" }, false, null, false);
            controller.game.Reset();
        }

		private void BjCommand(SqlTwitchUser speaker, string additionalText) {
			controller.SetState(this.GetType(), typeof(BJStateAcceptingPlayers));
		}

		private void AutoStart() {
			controller.SetState(this.GetType(), typeof(BJStateAcceptingPlayers)); 
		}

        public override void Dispose() {
            base.Dispose();
			timer?.Stop();
		}
	}
}
