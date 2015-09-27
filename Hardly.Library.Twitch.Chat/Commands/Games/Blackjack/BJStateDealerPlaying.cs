using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class BJStateDealerPlaying : GameState<TwitchBlackjack> {
		Timer timer;

		public BJStateDealerPlaying(TwitchBlackjack controller) : base(controller) {
			timer = new Timer(TimeSpan.FromSeconds(10), DrawCard);
		}

		internal override void Close() {
			timer?.Stop();
		}

		internal override void Open() {
			string chatMessage = "Blackjack: Dealer has ";
			chatMessage += controller.game.dealer.hand.ToChatString();
			CheckDone(chatMessage);
		}

		void CheckDone(string chatMessage) {
			if(controller.game.dealer.IsBust()) {
				chatMessage += " and busts.";
				
				Announce(chatMessage);
			} else if(controller.game.dealer.HandValue() > 17 || (controller.game.dealer.HandValue() == 17 && controller.game.dealer.hand.cards.Count == 2)) {
				controller.game.dealer.standing = true;
				chatMessage += " and stands with " + controller.game.dealer.GetValueString() + ".";

				Announce(chatMessage);
			} else {
				chatMessage += "...";

				controller.room.SendChatMessage(chatMessage);
				timer.Start();
			}
		}

		void DrawCard() {
			PlayingCard card = controller.game.deck.TakeTopCard();
			controller.game.dealer.hand.GiveCard(card);

			string chatMessage = "Blackjack: Dealer hits, gets ";
			chatMessage += card.ToChatString();
			CheckDone(chatMessage);
		}

		void Announce(string chatMessage) {
			chatMessage += " ";
			
			string winners = GetPlayerList(true);
			string tied = GetPlayerList(null);
			string losers = GetPlayerList(false);
			if(winners != null) {
				chatMessage += winners + " won! ";
			}
			if(tied != null) {
				chatMessage += tied + " tied. ";
			}
			if(losers != null) {
				chatMessage += "The losers: " + losers;
			}
			
			controller.room.SendChatMessage(chatMessage);

			controller.SetState(this.GetType(), typeof(BJStateOff));
		}

		private string GetPlayerList(bool? winnerOrLoser) {
			string chatMessage = "";
			bool first = true;
			foreach(var player in controller.game.players) {
				if(player.Value.IsWinner(controller.game.dealer) == winnerOrLoser) {
					if(!first) {
						chatMessage += ", ";
					}
					chatMessage += (player.Key as SqlTwitchUser).name;
					chatMessage += " (" + player.Value.GetValueString() + ")";
					UpdatePoints(player);

					first = false;
				}
			}

			if(first) {
				return null;
			} else {
				return chatMessage;
			}
		}

		private long UpdatePoints(System.Collections.Generic.KeyValuePair<SqlTwitchUser, BlackjackPlayer> player) {
			long changeInPoints = player.Value.GetWinningsOrLosings(controller.game.dealer);
			UserPointManager userPoints = controller.pointManager.ForUser(player.Key);
			userPoints.Award(player.Value.totalBet, changeInPoints);

			return changeInPoints;
		}
	}
}
