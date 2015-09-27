using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games {
	public class Deck : CardCollection {
        static PlayingCard[] standardDeck = GenerateStandardDeck(false),
            standardDeckWithJokers = GenerateStandardDeck(true);

        private static PlayingCard[] GenerateStandardDeck(bool includeJokers) {
            int cardCount = 52 + (includeJokers ? 2 : 0);
			List<PlayingCard> cards = new List<PlayingCard>(cardCount);
			for(int i = 0; i < cardCount; i++) {
				PlayingCard.Suit suit = (PlayingCard.Suit)(i % 4);
				PlayingCard.Value value = (PlayingCard.Value)((i - (int)suit)/4);
				cards.Add(new PlayingCard(suit, value));
			}

			return cards.ToArray();
		}

		public Deck(uint numberOfDecks = 1, bool includeJokers = false) : base((includeJokers ? standardDeckWithJokers : standardDeck).DuplicateEntities(numberOfDecks - 1).Shuffle()) {
		}
	}
}
