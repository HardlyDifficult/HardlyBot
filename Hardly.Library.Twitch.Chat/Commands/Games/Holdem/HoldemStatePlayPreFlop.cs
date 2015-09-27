using System;

namespace Hardly.Library.Twitch {
	public class HoldemStatePlayPreFlop : GameState<TwitchHoldem> {
		TimerSet timer = new TimerSet(new System.TimeSpan[] { TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10) }, new Action[] { WarningTimer, FinalTimer });

		private static void FinalTimer() {
			throw new NotImplementedException();
		}

		private static void WarningTimer() {
			throw new NotImplementedException();
		}

		public HoldemStatePlayPreFlop(TwitchHoldem controller) : base(controller) {
		}

		internal override void Open() {
			base.Open();
			for(int i = 0; i < 2; i++) {
				foreach(var player in controller.game.players) {
					controller.game.DealCard(player.Value);
				}
			}

			foreach(var player in controller.game.players) {
				string chatMessage = "You have ";
				chatMessage += player.Value.hand.ToChatString();
				chatMessage += ".  Do you !bet <amount>?  Otherwise, just sit tight.";
				controller.room.SendWhisper(player.Key, chatMessage);
			}
		}
	}
}