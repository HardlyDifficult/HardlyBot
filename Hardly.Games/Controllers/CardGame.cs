namespace Hardly.Games {
	public abstract class CardGame<PlayerGameObjectType, PlayerIdType, CardType> 
        : Game<PlayerGameObjectType, PlayerIdType> 
        where PlayerGameObjectType : GamePlayer<PlayerIdType> 
        where CardType : ICard {
		Deck<CardType> deck;

		public CardGame(Deck<CardType> dealerDeck, uint minPlayers, uint maxPlayers) : base(minPlayers, maxPlayers) {
            deck = dealerDeck;
		}

		public override void Reset() {
			base.Reset();
            deck.Reset();
		}

		public CardType DealCard(List<CardType> playerCards) {
            CardType card = deck.TakeTopCard();
			playerCards.Add(card);

            return card;
		}
	}
}
