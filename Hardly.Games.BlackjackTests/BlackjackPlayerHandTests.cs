using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games.Tests {
    [TestClass()]
    public class BlackjackPlayerHandTests {
        [TestMethod()]
        public void IsBustTest() {
            var hand = new BlackjackCardListEvaluator(new List<PlayingCard>());
            hand.cards.Add(new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.King));
            hand.cards.Add(new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.King));
            hand.cards.Add(new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.King));
            hand.cards.Add(new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.King));
            Assert.IsTrue(hand.isBust);
        }
    }
}