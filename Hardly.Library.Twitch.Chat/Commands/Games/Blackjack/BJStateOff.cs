using System;

namespace Hardly.Library.Twitch {
	public class BJStateOff : GameState<TwitchBlackjack> {
		Timer timer;

		public BJStateOff(TwitchBlackjack controller) : base(controller) {
			timer = new Timer(TimeSpan.FromSeconds(30), AutoStart);
			AddCommand(controller.room, "bj", BjCommand, "Starts a game of Blackjack", new[] { "blackjack" }, true, TimeSpan.FromSeconds(0), false);
		}

		private void BjCommand(SqlTwitchUser speaker, string additionalText) {
			controller.SetState(this.GetType(), typeof(BJStateAcceptingPlayers));
		}

		private void AutoStart() {
			controller.SetState(this.GetType(), typeof(BJStateAcceptingPlayers)); 
		}

		internal override void Close() {
			base.Close();
			timer?.Stop();
		}

		internal override void Open() {
			base.Open();
			controller.game.Reset();
			//timer.Start();
		}
	}
}
