namespace Hardly.Games {
	public sealed class Blackjack<PlayerIdType> : CardGame<PlayerIdType, BlackjackPlayer> {
      public BlackjackPlayerHand dealer;
		CardCollection lastDealerHand = null;

		public Blackjack() : this(1, 6) {
		}

		public Blackjack(uint numberOfDecks, uint maxPlayers) : base(numberOfDecks, maxPlayers) {
			Reset();
		}

		public void Deal() {
			for(int numberOfCards = 0; numberOfCards < 2; numberOfCards++) {
				foreach(var player in players) {
					DealCard(player.Value.CurrentHand);
				}
				DealCard(dealer);
			}
		}

		public override void Reset() {
			lastDealerHand = dealer?.hand;
			base.Reset();
			dealer = new BlackjackPlayerHand(0, false);
		}

		public void Join(PlayerIdType playerId, ulong bet) {
			if(!base.Contains(playerId)) {
				base.Join(playerId, new BlackjackPlayer(bet));
				Log.info(playerId.ToString() + " joined the table.");
			}
		}

		public CardCollection LastDealerHand() {
			if(dealer?.hand != null && dealer.hand.cards.Count > 0) {
				return dealer.hand;
			} else {
				return lastDealerHand;
			}
      }
	}
}
