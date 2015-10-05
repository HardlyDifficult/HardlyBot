namespace Hardly.Games {
    public class PlayingCardDeck : Deck<PlayingCard> {
        static List<PlayingCard> standardDeckWithoutJokers = GenerateStandardDeck(false),
            standardDeckWithJokers = GenerateStandardDeck(true);

        public PlayingCardDeck(uint numberOfDecks = 1, bool includeJokers = false) : base(includeJokers ? standardDeckWithJokers : standardDeckWithoutJokers, numberOfDecks) {
        }
        
        static List<PlayingCard> GenerateStandardDeck(bool includeJokers) {
            int cardCount = 52 + (includeJokers ? 2 : 0);
            List<PlayingCard> cards = new List<PlayingCard>();
            for(int i = 0; i < cardCount; i++) {
                PlayingCard.Suit suit = (PlayingCard.Suit)(i % 4);
                PlayingCard.Value value = (PlayingCard.Value)((i - (int)suit) / 4);
                cards.Add(new PlayingCard(suit, value));
            }

            return cards;
        }
    }
}
