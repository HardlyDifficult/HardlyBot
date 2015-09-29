namespace Hardly.Games {
	public sealed class Blackjack<PlayerIdType> : CardGame<PlayerIdType, BlackjackPlayer<PlayerIdType>> {
        public BlackjackCardListEvaluator dealer {
            get;
            private set;
        }
		public PlayingCardList lastDealerHand {
            get;
            private set;
        }

        public Blackjack() : this(1, 6) {
		}

		public Blackjack(uint numberOfDecks, uint maxPlayers) : base(numberOfDecks, maxPlayers) {
            dealer = null;
            lastDealerHand = null;
            Reset();
		}

        public ulong Join(PlayerIdType playerId, PlayerPointManager pointManager, ulong bet) {
            if(!base.Contains(playerId)) {
                var player = new BlackjackPlayer<PlayerIdType>(this, pointManager, playerId);
                if(player.PlaceBet(bet, false)) {
                    base.Join(playerId, player);
                    Log.info(playerId.ToString() + " joined Blackjack!");
                    return player.bet;
                } 
            } else {
                var player = Get(playerId);
                player.CanelBet();
                if(player.PlaceBet(bet, false)) {
                    Log.info(playerId.ToString() + " changed their Blackjack bet.");
                    return player.bet;
                }
            }

            return 0;
        }

        public override void Reset() {
            base.Reset();

            lastDealerHand = dealer?.cards;
            dealer = new BlackjackCardListEvaluator(new PlayingCardList());
        }

        public override bool StartGame() {
            if(CanStart()) {
                for(int numberOfCards = 0; numberOfCards < 2; numberOfCards++) {
                    foreach(var player in PlayerGameObjects) {
                        DealCard(player.CurrentHandEvaluator.cards);
                    }
                    DealCard(dealer.cards);
                }

                return true;
            }

            return false;
		}

        public override void EndGame() {
            foreach(var player in PlayerGameObjects) {
                player.Award(player.GetWinningsOrLosings());
            }
        }
    }
}
