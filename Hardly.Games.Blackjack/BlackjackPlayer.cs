using System;

namespace Hardly.Games {
	public class BlackjackPlayer<PlayerIdType> : CardPlayer<PlayerIdType> {
        Blackjack<PlayerIdType> controller;
        BlackjackCardListEvaluator mainHandEvaluator, splitHandEvaluator;
        ulong amountBetOnSplitHand;
		bool currentHandIsMain = true;
        
		public BlackjackPlayer(Blackjack<PlayerIdType> controller, PlayerPointManager pointManager, PlayerIdType playerIdObject)
            : base(pointManager, playerIdObject) {
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

		public bool? IsWinner(BlackjackCardListEvaluator dealer) {
			long winnings = GetWinningsOrLosings(dealer);
            return winnings != 0 ? winnings > 0 : (bool?)null;
		}

        public long GetWinningsOrLosings(BlackjackCardListEvaluator dealer) {
            long winningsOrLosings;

            winningsOrLosings = GetWinnings(dealer, mainHandEvaluator, bet);

            if(splitHandEvaluator != null) {
                winningsOrLosings -= (long)amountBetOnSplitHand;
                winningsOrLosings += GetWinnings(dealer, splitHandEvaluator, amountBetOnSplitHand);
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

        public bool CanSplit {
            get {
                return hand.cards.Count == 2 && hand.cards[0].BlackjackValue().Equals(hand.cards[1].BlackjackValue()) && splitHandEvaluator == null;
            }
        }

        public bool Split() {
			if(CanSplit) {
                splitHandEvaluator = new BlackjackCardListEvaluator(new PlayingCardList(hand.cards.Pop()));
                amountBetOnSplitHand = bet;

                if(PlaceBet(bet, true)) {
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
            if(PlaceBet(amount, true)) {
                if(CurrentHandEvaluator.Equals(splitHandEvaluator)) {
                    amountBetOnSplitHand += amount;
                }
                CurrentHandEvaluator.isStanding = true;
                controller.DealCard(hand.cards);

                return true;
            }

            return false;
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
