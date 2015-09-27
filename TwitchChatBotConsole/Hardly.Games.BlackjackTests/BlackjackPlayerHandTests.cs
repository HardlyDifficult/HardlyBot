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
            BlackjackPlayerHand hand = new BlackjackPlayerHand(0, false);
            hand.hand.GiveCard(new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.King));
            hand.hand.GiveCard(new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.King));
            hand.hand.GiveCard(new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.King));
            hand.hand.GiveCard(new PlayingCard(PlayingCard.Suit.Clubs, PlayingCard.Value.King));
            Assert.IsTrue(hand.IsBust());
        }
    }
}