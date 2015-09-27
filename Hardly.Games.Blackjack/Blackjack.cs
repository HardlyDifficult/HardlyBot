namespace Hardly.Games {
	public sealed class Blackjack<PlayerIdType> : CardGame<PlayerIdType, BlackjackPlayer<PlayerIdType>> {
      public BlackjackPlayerHand<PlayerIdType> dealer;
		CardCollection lastDealerHand = null;

        public Blackjack() : this(1, 6) {
		}

		public Blackjack(uint numberOfDecks, uint maxPlayers) : base(numberOfDecks, maxPlayers) {
			Reset();
		}

		public void Deal() {
			for(int numberOfCards = 0; numberOfCards < 2; numberOfCards++) {
				foreach(var player in PlayerObjects) {
					DealCard(player.CurrentHand);
				}
				DealCard(dealer);
			}
		}

		public override void Reset() {
			lastDealerHand = dealer?.hand;
			base.Reset();
			dealer = new BlackjackPlayerHand<PlayerIdType>(null, (PlayerIdType)typeof(PlayerIdType).GetDefaultValue(), 0, false);
		}

		public ulong Join(PlayerIdType playerId, PointManager pointManager, ulong bet) {
			if(!base.Contains(playerId)) {
                var player = new BlackjackPlayer<PlayerIdType>(pointManager, playerId, bet);
                base.Join(playerId, player);
				Log.info(playerId.ToString() + " joined");
                return player.totalBet;
			}

            return 0;
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
