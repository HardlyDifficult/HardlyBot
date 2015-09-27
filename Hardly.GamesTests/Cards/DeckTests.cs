using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games.Tests {
	[TestClass()]
	public class DeckTests {
		[TestMethod()]
		public void DeckTest() {
			Deck deck = new Deck(1);
			Deck deck2 = new Deck(2);
			Deck deck5 = new Deck(5);
		}
	}
}