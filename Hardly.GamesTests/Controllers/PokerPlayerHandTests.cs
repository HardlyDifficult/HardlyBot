using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hardly.Games.Tests {
    [TestClass()]
    public class PokerPlayerHandTests {
        [TestMethod()]
        public void HandValueTest() {
            var straightFlush = new List<PlayingCard>(new[] {
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Jack),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Ten),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Seven),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Nine),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Eight),
            });
            var fourOfAKind = new List<PlayingCard>(new[] {
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Six),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Jack),
                new PlayingCard(PlayingCard.Suit.Diamonds, PlayingCard.Value.Six),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Six),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Six),
            });

           // Assert.IsTrue(PokerPlayerHand.HandValue(straightFlush) > PokerPlayerHand.HandValue(fourOfAKind));
        }
    }
}