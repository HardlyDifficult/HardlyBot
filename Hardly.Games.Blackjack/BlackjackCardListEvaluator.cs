namespace Hardly.Games {
    public class BlackjackCardListEvaluator : PlayingCardListEvaluator  {
        public bool isStanding = false;
        public bool isSplit = false;

        public BlackjackCardListEvaluator(PlayingCardList cards) : base(cards) {
        }

        public uint handValue {
            get {
                uint aceCount = 0;
                uint value = 0;

                foreach(var card in cards) {
                    value += card.BlackjackValue();
                    if(card.value.Equals(PlayingCard.Value.Ace)) {
                        aceCount++;
                    }
                }

                while(value > 21 && aceCount > 0) {
                    aceCount--;
                    value -= 10;
                }

                return value;
            }
        }

        public bool isBlackjack {
            get {
                return !isSplit && handValue == 21 && cards.Count == 2;
            }
        }

        public bool isBust {
            get {
                return handValue > 21;
            }
        }

        public bool isDone {
            get {
                return isBust || isStanding;
            }
        }

        /// <summary>
        /// True = win; Null = tie; False = lose.
        /// </summary>
        public bool? IsWinner(BlackjackCardListEvaluator dealer) {
            if(!isBust && handValue == dealer.handValue && isBlackjack == dealer.isBlackjack) {
                return null;
            } else {
                return !isBust
                    && (dealer.isBust
                        || (handValue > dealer.handValue
                                || (isBlackjack && !dealer.isBlackjack)));
            }
        }

        public override string ToString() {
            if(isBlackjack) {
                return "blackjack";
            } else if(isBust) {
                return "bust";
            } else {
                return handValue.ToString();
            }
        }
    }
}
