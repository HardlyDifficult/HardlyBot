using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games.Tests {
    [TestClass()]
    public class PokerPlayerHandTests {
        [TestMethod()]
        public void HandValueTest() {
            CardCollection straightFlush = new CardCollection(new[] {
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Jack),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Ten),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Seven),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Nine),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Eight),
            });
            CardCollection fourOfAKind = new CardCollection(new[] {
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Six),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Jack),
                new PlayingCard(PlayingCard.Suit.Diamonds, PlayingCard.Value.Six),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Six),
                new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.Six),
            });

            Assert.IsTrue(PokerPlayerHand.HandValue(straightFlush) > PokerPlayerHand.HandValue(fourOfAKind));
        }
    }
}