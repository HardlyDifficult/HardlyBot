using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hardly.Games.Tests {
    [TestClass()]
    public class DeckTests {
        [TestMethod()]
        public void DeckTest1() {
            PlayingCardDeck deckWithout = new PlayingCardDeck(1, false);
            PlayingCardDeck deckWith = new PlayingCardDeck(1, true);

            Assert.IsTrue(deckWithout.numberOfCardsRemaining == 52);
            Assert.IsTrue(deckWith.numberOfCardsRemaining == 54);
        }
    }
}