namespace Hardly.Games {
	public abstract class CardGame<PlayerIdType, PlayerGameType> : Game<PlayerIdType, PlayerGameType> {
		uint numberOfDecks = 1;
		public Deck deck;

		public CardGame(uint numberOfDecks, uint maxPlayers) : base(maxPlayers) {
			this.numberOfDecks = numberOfDecks;
		}

		public override void Reset() {
			base.Reset();
			deck = new Deck(numberOfDecks);
		}

		public void DealCard(CardPlayerHand player) {
			PlayingCard card = deck.TakeTopCard();
			player.hand.GiveCard(card);
			Log.info("Dealt " + card.ToChatString());
		}
	}
}
