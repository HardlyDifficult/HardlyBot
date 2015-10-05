using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games.Uno {
    public class UnoDeck : Deck<UnoCard> {
        static List<UnoCard> standardDeck = ConstructStandardDeck();

        public UnoDeck() : base(standardDeck) {
        }

        static List<UnoCard> ConstructStandardDeck() {
            throw new NotImplementedException();
        }
    }
}
