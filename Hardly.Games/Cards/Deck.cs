using System;

namespace Hardly.Games {
	public class Deck : PlayingCardList {
        static PlayingCardList standardDeck = GenerateStandardDeck(false),
            standardDeckWithJokers = GenerateStandardDeck(true);

        public Deck(uint numberOfDecks = 1, bool includeJokers = false) : base(includeJokers ? standardDeckWithJokers : standardDeck) {
            DuplicateEntities(numberOfDecks - 1);
            Shuffle();
		}

        static PlayingCardList GenerateStandardDeck(bool includeJokers) {
            int cardCount = 52 + (includeJokers ? 2 : 0);
            PlayingCardList cards = new PlayingCardList();
            for(int i = 0; i < cardCount; i++) {
                PlayingCard.Suit suit = (PlayingCard.Suit)(i % 4);
                PlayingCard.Value value = (PlayingCard.Value)((i - (int)suit) / 4);
                cards.Add(new PlayingCard(suit, value));
            }

            return cards;
        }
    }
}
