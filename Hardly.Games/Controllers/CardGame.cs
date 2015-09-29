namespace Hardly.Games {
	public abstract class CardGame<PlayerIdType, PlayerGameType> : Game<PlayerIdType, PlayerGameType> {
		uint numberOfDecks = 1;
		Deck deck;

		public CardGame(uint numberOfDecks, uint maxPlayers) : base(maxPlayers) {
			this.numberOfDecks = numberOfDecks;
		}

		public override void Reset() {
			base.Reset();
			deck = new Deck(numberOfDecks);
		}

		public PlayingCard DealCard(PlayingCardList playerCards) {
            PlayingCard card = deck.Pop();
			playerCards.Add(card);

            return card;
		}
	}
}
