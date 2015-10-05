using System;

namespace Hardly.Games {
	public class Deck<CardType> where CardType : ICard {
        List<CardType> fullDeck, currentDeck;

        public Deck(List<CardType> cardsInAStandardDeck, uint numberOfDecks = 1) {
            this.fullDeck = cardsInAStandardDeck;
            this.fullDeck.DuplicateEntities(numberOfDecks - 1);
            Reset();
        }

        public void Reset() {
            currentDeck = new List<CardType>(fullDeck);
            currentDeck.Shuffle();
        }

        public void ShuffleIn(CardType newCard) {
            currentDeck.Add(newCard);
            currentDeck.Shuffle();
        }

        public void ShuffleIn(List<CardType> newCards) {
            currentDeck.Add(newCards);
            currentDeck.Shuffle();
        }

        public CardType TakeTopCard() {
            return currentDeck.Pop();
        }

        public uint numberOfCardsRemaining {
            get {
                return (uint)currentDeck.Count;
            }
        }
    }
}
