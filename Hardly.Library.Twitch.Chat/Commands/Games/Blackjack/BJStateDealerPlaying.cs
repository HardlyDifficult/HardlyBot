using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class BJStateDealerPlaying : GameState<TwitchBlackjack> {
		Timer timer;

		public BJStateDealerPlaying(TwitchBlackjack controller) : base(controller) {
			timer = new Timer(TimeSpan.FromSeconds(10), DrawCard);

            string chatMessage = "Blackjack: Dealer has ";
            chatMessage += controller.game.dealer.cards.ToString();
            CheckDone(chatMessage);
        }

        public override void Dispose() {
            base.Dispose();
			timer?.Stop();
		}
        
		void CheckDone(string chatMessage) {
			if(controller.game.dealer.isBust) {
				chatMessage += " and busts.";
				
				Announce(chatMessage);
			} else if(controller.game.dealer.handValue > 17 || (controller.game.dealer.handValue == 17 && controller.game.dealer.cards.Count == 2)) {
				controller.game.dealer.isStanding = true;
				chatMessage += " and stands with " + controller.game.dealer.HandValueString() + ".";

				Announce(chatMessage);
			} else {
				chatMessage += "...";

				controller.room.SendChatMessage(chatMessage);
				timer.Start();
			}
		}

		void DrawCard() {
            var card = controller.game.DealCard(controller.game.dealer.cards);
			string chatMessage = "Blackjack: Dealer hits, gets ";
			chatMessage += card.ToChatString();
			CheckDone(chatMessage);
		}

		void Announce(string chatMessage) {
            controller.game.EndGame();

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
			foreach(var player in controller.game.PlayerGameObjects) {
				if(player.IsWinner() == winnerOrLoser) {
					if(!first) {
						chatMessage += ", ";
					}
					chatMessage += player.idObject.name;
                    chatMessage += " has ";
                    chatMessage += player.hand.cards.ToString();
                    chatMessage += " (";
                    chatMessage += player.HandValueString();
                    chatMessage += ")";

					first = false;
				}
			}

			if(first) {
				return null;
			} else {
				return chatMessage;
			}
		}
	}
}
