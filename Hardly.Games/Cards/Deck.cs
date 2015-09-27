using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games {
	public class Deck : CardCollection {
		static PlayingCard[] standardDeck = GenerateStandardDeck();

		private static PlayingCard[] GenerateStandardDeck() {
			List<PlayingCard> cards = new List<PlayingCard>();
			for(int i = 0; i < 52; i++) {
				PlayingCard.Suit suit = (PlayingCard.Suit)(i % 4);
				PlayingCard.Value value = (PlayingCard.Value)((i - (int)suit)/4);
				cards.Add(new PlayingCard(suit, value));
			}

			return cards.ToArray();
		}

		public Deck(uint numberOfDecks = 1) : base(standardDeck.DuplicateEntities(numberOfDecks - 1).Shuffle()) {
		}
	}
}
