using System;

namespace Hardly.Games {
	public class BlackjackPlayerHand : CardPlayerHand {
		public bool standing = false;
		public ulong bet;
		public bool isSplit;
		
		public BlackjackPlayerHand(ulong bet, bool isSplit) {
			this.bet = bet;
			this.isSplit = isSplit;
		}

		public bool IsBust() {
			return HandValue() > 21;
		}

		public uint HandValue() {
			uint aceCount = 0;
			uint value = 0;

			foreach(var card in hand.cards) {
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

		public long GetWinningsOrLosings(BlackjackPlayerHand dealer) {
			bool? winner = IsWinner(dealer);
			if(winner.HasValue) {
				long winnings = (long)bet;
				if(winner.Value && HasBlackjack()) {
					winnings = (long)(winnings * 1.5);
				} else if(!winner.Value) {
					winnings *= -1;
				}

				return winnings;
			} else {
				return 0;
			}
		}

		public bool HasBlackjack() {
			return !isSplit && HandValue() == 21 && hand.cards.Count == 2;
		}

		public string GetValueString() {
			if(HasBlackjack()) {
				return "blackjack";
			} else {
				return HandValue().ToString();
			}
		}

		public bool? IsWinner(BlackjackPlayerHand dealer) {
			uint handValue = HandValue();
			if(!IsBust() && HandValue() == dealer.HandValue() && HasBlackjack() == dealer.HasBlackjack()) {
				return null;
			} else {
				return !IsBust() 
					&& (dealer.IsBust() 
						|| (handValue > dealer.HandValue()
								|| (HasBlackjack() && !dealer.HasBlackjack())));
			}
      }
	}
}
