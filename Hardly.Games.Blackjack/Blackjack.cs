namespace Hardly.Games {
	public sealed class Blackjack<PlayerIdType> : CardGame<BlackjackPlayer<PlayerIdType>, PlayerIdType, PlayingCard> {
        public BlackjackCardListEvaluator dealer {
            get;
            private set;
        }
		public List<PlayingCard> lastDealerHand {
            get;
            private set;
        }

        public Blackjack() : this(1, 6) {
		}

		public Blackjack(uint numberOfDecks, uint maxPlayers) : base(new PlayingCardDeck(numberOfDecks), 1, maxPlayers) {
            dealer = null;
            lastDealerHand = null;
            Reset();
		}

        public ulong Join(PlayerIdType playerId, PlayerPointManager pointManager, ulong bet) {
            if(!base.Contains(playerId)) {
                var player = new BlackjackPlayer<PlayerIdType>(this, pointManager, playerId);
                    if(base.Join(player)) {

                    if(player.PlaceBet(bet, false) > 0) {
                        Log.info(playerId.ToString() + " joined Blackjack!");
                        return player.bet;
                    }
                } 
            } else {
                var player = GetPlayer(playerId);
                player.CancelBet();
                if(player.PlaceBet(bet, false) > 0) {
                    Log.info(playerId.ToString() + " changed their Blackjack bet.");
                    return player.bet;
                }
            }

            return 0;
        }

        public override void Reset() {
            base.Reset();

            lastDealerHand = dealer?.cards;
            dealer = new BlackjackCardListEvaluator(new List<PlayingCard>());
        }

        public override bool StartGame() {
            if(isReadyToStart) {
                for(int numberOfCards = 0; numberOfCards < 2; numberOfCards++) {
                    foreach(var player in GetPlayers()) {
                        DealCard(player.CurrentHandEvaluator.cards);
                    }
                    DealCard(dealer.cards);
                }

                return true;
            }

            return false;
		}

        public void Surrender(BlackjackPlayer<PlayerIdType> player) {
            if(player.canSurrender) {
                player.Award((long)(player.bet * -0.5));
                player.isWinner = false;
                LeaveGame(player.idObject);
            }
        }

        protected override void EndGame() {
            foreach(var player in GetPlayers()) {
                if(player.boughtInsurance) {
                    if(dealer.isBlackjack) {
                        player.LosePartialBet(player.bet / 3);
                    } else {
                        player.AwardPartialBet(player.bet / 3, (long)player.bet / 3 * 2);
                    }
                }
                player.Award(player.GetWinningsOrLosings());
            }
        }

        public bool ReadyToEnd() {
            bool allReady = true;

            foreach(var player in GetPlayers()) {
                if(!player.CurrentHandEvaluator.isDone) {
                    allReady = false;
                    break;
                }
            }

            return allReady;
        }

        public bool InsuranceAvailable() {
            if(dealer.cards.First.value.Equals(PlayingCard.Value.Ace)) {
                foreach(var player in GetPlayers()) {
                    if(player.CurrentHandEvaluator.cards.Count != 2) {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
