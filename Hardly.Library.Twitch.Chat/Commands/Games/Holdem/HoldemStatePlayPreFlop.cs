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
				foreach(var player in controller.game.PlayerObjects) {
					controller.game.DealCard(player);
				}
			}

			foreach(var player in controller.game.GetPlayersAndObjects()) {
				string chatMessage = "You have ";
				chatMessage += player.Value.hand.ToChatString();
				chatMessage += ".  Do you !bet <amount> or !stand?";
				controller.room.SendWhisper(player.Key, chatMessage);

                // Option 1 - Free for all.  Pro: Faster games.  Con: Not realistic.
                // Only command is !bet.  If noone bets, move to next round.
                // If a !bet <amount>, whisper to everyone else you must !bet at least x or fold.
                // reset timer..
                // 
                // Timer warns, you bet or you fold.

                // Option 2 - Correct order.  Pro: real.  Con: Could be slow as shit.
                // After deal, everyone is whispered cards. but only 1 person is told to act.
                // Once he acts, or timesout.  Then whisper to the next guy.
			}
		}
	}
}