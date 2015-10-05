using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly.Games.Uno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games.Uno.Tests {
    [TestClass()]
    public class UnoDeckTests {
        [TestMethod()]
        public void UnoDeckTest() {
            UnoDeck deck = new UnoDeck();
            Assert.IsTrue(deck.numberOfCardsRemaining == 108);
        }
    }
}