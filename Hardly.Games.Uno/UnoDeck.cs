using System;

namespace Hardly.Games.Uno {
    public class UnoDeck : Deck<UnoCard> {
        static List<UnoCard> standardDeck = ConstructStandardDeck();

        public UnoDeck() : base(standardDeck) {
        }

        static List<UnoCard> ConstructStandardDeck() {
            List<UnoCard> deck = new List<UnoCard>();

            foreach(UnoCard.Value type in Enum.GetValues(typeof(UnoCard.Value))) {
                foreach(UnoCard.Color color in Enum.GetValues(typeof(UnoCard.Color))) {
                    deck.Add(new UnoCard(color, type));
                    if(!type.Equals(UnoCard.Value.Zero) && !type.Equals(UnoCard.Value.Wild) && !type.Equals(UnoCard.Value.WildDraw4)) {
                        deck.Add(new UnoCard(color, type));
                    }
                }
            }

            return deck;
        }
    }
}
