using System;

namespace Hardly.Games {
	public class BlackjackPlayer<PlayerIdType> : CardPlayer<PlayerIdType> {
        Blackjack<PlayerIdType> controller;
        BlackjackCardListEvaluator mainHandEvaluator, splitHandEvaluator;
        ulong amountBetOnSplitHand;
		bool currentHandIsMain = true;
        public bool boughtInsurance {
            get;
            private set;
        }
        
		public BlackjackPlayer(Blackjack<PlayerIdType> controller, PlayerPointManager pointManager, PlayerIdType playerIdObject)
            : base(pointManager, playerIdObject) {
            boughtInsurance = false;
            this.controller = controller;
            mainHandEvaluator = new BlackjackCardListEvaluator(hand.cards);
            splitHandEvaluator = null;
            amountBetOnSplitHand = 0;
		}

		public BlackjackCardListEvaluator CurrentHandEvaluator {
			get {
				return currentHandIsMain ? mainHandEvaluator : splitHandEvaluator;
			}
		}

		public bool? IsWinner() {
			long winnings = GetWinningsOrLosings();
            return winnings != 0 ? winnings > 0 : (bool?)null;
		}

        internal long GetWinningsOrLosings() {
            long winningsOrLosings;


            if(splitHandEvaluator != null) {
                winningsOrLosings = GetWinnings(controller.dealer, mainHandEvaluator, bet - amountBetOnSplitHand);
                winningsOrLosings += GetWinnings(controller.dealer, splitHandEvaluator, amountBetOnSplitHand);
            } else {
                winningsOrLosings = GetWinnings(controller.dealer, mainHandEvaluator, bet);
            }

            return winningsOrLosings;
        }

        public bool Stand() {
            if(!CurrentHandEvaluator.isStanding && !CurrentHandEvaluator.isBust) {
                CurrentHandEvaluator.isStanding = true;
                return true;
            }

            return false;
        }

        static long GetWinnings(BlackjackCardListEvaluator dealer, BlackjackCardListEvaluator cardListEvaluator, ulong bet) {
            long winningsOrLosings;
            switch(cardListEvaluator.IsWinner(dealer)) {
            case true:
                winningsOrLosings = cardListEvaluator.isBlackjack ? (long)(bet * 1.5): (long)bet;
                break;
            case false:
                winningsOrLosings = (long)bet * -1L;
                break;
            default:
                winningsOrLosings = 0;
                break;
            }

            return winningsOrLosings;
        }

        public bool BuyInsurance() {
            if(PlaceBet(Math.Max(bet / 2, 1), true) > 0) {
                boughtInsurance = true;
                return true;
            }

            return false;
        }

        public bool canSplit {
            get {
                return hand.cards.Count == 2 && hand.cards[0].BlackjackValue().Equals(hand.cards[1].BlackjackValue()) && splitHandEvaluator == null;
            }
        }

        public bool canSurrender {
            get {
                return hand.cards.Count == 2 && splitHandEvaluator == null;
            }
        }

        public string HandValueString() {
            string message = mainHandEvaluator.HandValueString();
            if(splitHandEvaluator != null) {
                message += "/";
                message += splitHandEvaluator.HandValueString();
            }

            return message;
        }

        public bool Split() {
			if(canSplit) {
                splitHandEvaluator = new BlackjackCardListEvaluator(new PlayingCardList(hand.cards.Pop()));
                amountBetOnSplitHand = bet;

                if(PlaceBet(bet, true) > 0) {
                    mainHandEvaluator.isSplit = true;
                    splitHandEvaluator.isSplit = true;
                    controller.DealCard(hand.cards);
                    controller.DealCard(splitHandEvaluator.cards);

                    if(hand.cards[0].value.Equals(PlayingCard.Value.Ace)) {
                        mainHandEvaluator.isStanding = true;
                        splitHandEvaluator.isStanding = true;
                    }

                    return true;
                } else {
                    splitHandEvaluator = null;
                    amountBetOnSplitHand = 0;
                }
			}

			return false;
		}

        public bool DoubleDown() {
            ulong amount = bet;
            if(PlaceBet(amount, true) > 0) {
                if(CurrentHandEvaluator.Equals(splitHandEvaluator)) {
                    amountBetOnSplitHand += amount;
                }
                CurrentHandEvaluator.isStanding = true;
                Hit();

                return true;
            }

            return false;
        }

        public bool Hit() {
            controller.DealCard(hand.cards);

            return true;
        }

        public bool ReadyToSwitchHands() {
			if(splitHandEvaluator != null && mainHandEvaluator.isDone) {
				currentHandIsMain = false;
				return true;
			}

			return false;
		}

        public override string ToString() {
            string valueString = mainHandEvaluator.cards.ToString();
            if(splitHandEvaluator != null) {
                valueString += "/" + splitHandEvaluator.cards.ToString();
            }

            return valueString;
        }

    }
}
