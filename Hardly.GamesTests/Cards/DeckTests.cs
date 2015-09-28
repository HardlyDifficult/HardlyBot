using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hardly.Games.Tests {
    [TestClass()]
    public class DeckTests {
        [TestMethod()]
        public void DeckTest() {
            Deck deck = new Deck(1);
            Deck deck2 = new Deck(2);
            Deck deck5 = new Deck(5);
        }

        [TestMethod()]
        public void DeckTest1() {
            Deck deckWithout = new Deck(1, false);
            Deck deckWith = new Deck(1, true);

            Assert.IsTrue(deckWithout.Count == 52);
            Assert.IsTrue(deckWith.Count == 54);
        }
    }
}