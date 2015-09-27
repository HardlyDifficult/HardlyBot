namespace Hardly.Games {
	public class BlackjackPlayer {
		BlackjackPlayerHand mainHand, splitHand;
		bool currentHandIsMain = true;

		public ulong totalBet {
			get {
				ulong bet = mainHand.bet;
				if(splitHand != null) {
					bet += splitHand.bet;
				}

				return bet;
			}
		}

		public BlackjackPlayer(ulong bet) {
			mainHand = new BlackjackPlayerHand(bet, false);
			splitHand = null;
		}

		public BlackjackPlayerHand CurrentHand {
			get {
				return currentHandIsMain ? mainHand : splitHand;
			}
		}

		public bool? IsWinner(BlackjackPlayerHand dealer) {
			long winnings = GetWinningsOrLosings(dealer);
         return winnings != 0 ? winnings > 0 : (bool?)null;
		}

		public string GetValueString() {
			string valueString = mainHand.GetValueString();
			if(splitHand != null) {
				valueString += "/" + splitHand.GetValueString();
			}
			return valueString;
		}

		public long GetWinningsOrLosings(BlackjackPlayerHand dealer) {
			long winnings = mainHand.GetWinningsOrLosings(dealer);
			if(splitHand != null) {
				winnings += splitHand.GetWinningsOrLosings(dealer);
			}

			return winnings;
		}

		public bool Split<T>(Blackjack<T> controller, bool betReserved) {
			if(mainHand.hand.cards.Count == 2 && mainHand.hand.cards[0].BlackjackValue().Equals(mainHand.hand.cards[1].BlackjackValue()) && splitHand == null && betReserved) {
				splitHand = new BlackjackPlayerHand(mainHand.bet, true);
				mainHand.isSplit = true;
				var card = mainHand.hand.TakeTopCard();
				splitHand.hand.GiveCard(card);

				controller.DealCard(mainHand);
				controller.DealCard(splitHand);

				if(mainHand.hand.cards[0].value.Equals(PlayingCard.Value.Ace)) {
					mainHand.standing = true;
					splitHand.standing = true;
				}
				return true;
			}

			return false;
		}

		public bool ReadyToSwitchHands() {
			if(splitHand != null && (mainHand.IsBust() || mainHand.standing)) {
				currentHandIsMain = false;
				return true;
			}

			return false;
		}
	}
}
