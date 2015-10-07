using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Hardly.Games.PlayingCard;
using static Hardly.Games.PokerPlayerHandEvaluator.HandType;

namespace Hardly.Games.Tests {
    [TestClass()]
    public class PokerPlayerHandEvaluatorTests {
        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestStraightFlush() {
            var tableCards = new List<PlayingCard>(new[] {
                c2c, cjc, c9c, c3d, c8c
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c0c, c7c,
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c0c, c6c,
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c0d, c6d,
            }), tableCards);

            Assert.IsTrue(h1.handType == StraightFlush);
            Assert.IsTrue(h2.handType == Flush);
            Assert.IsTrue(h1.handValue > h2.handValue);
            Assert.IsTrue(h3.handType == HighCard);
            Assert.IsTrue(h2.handValue > h3.handValue);
        }
        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestFourOfAKind() {
            var tableCards = new List<PlayingCard>(new[] {
                c2c, c6c, c9c, c6d, c6s
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cjh, c6h,
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c0c, c6c,
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2c, c6d,
            }), tableCards);

            Assert.IsTrue(h1.handType == FourOfAKind);
            Assert.IsTrue(h2.handType == FourOfAKind);
            Assert.IsTrue(h1.handValue > h2.handValue);
            Assert.IsTrue(h3.handType == FourOfAKind);
            Assert.IsTrue(h2.handValue > h3.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestFullHouse() {
            var tableCards = new List<PlayingCard>(new[] {
                ckh, c6c, c5c, c6d, c5s
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckc, c5d,
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckc, ckc,
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2c, c6d,
            }), tableCards);

            Assert.IsTrue(h1.handType == FullHouse);
            Assert.IsTrue(h2.handType == FullHouse);
            Assert.IsTrue(h1.handValue < h2.handValue);
            Assert.IsTrue(h3.handType == FullHouse);
            Assert.IsTrue(h2.handValue > h3.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestFlush() {
            var tableCards = new List<PlayingCard>(new[] {
                cjh, c0d, c9d, c7d, c2d
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckc, c5d,
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckd, ckc,
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2d, c6d,
            }), tableCards);

            Assert.IsTrue(h1.handType == Flush);
            Assert.IsTrue(h2.handType == Flush);
            Assert.IsTrue(h1.handValue < h2.handValue);
            Assert.IsTrue(h3.handType == Flush);
            Assert.IsTrue(h2.handValue > h3.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestStraight() {
            var tableCards = new List<PlayingCard>(new[] {
                c9h, cqc, cjs, c0h, c7d
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c9s, ckd
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c8d, cks
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2d, c8s
            }), tableCards);

            Assert.IsTrue(h1.handType == Straight);
            Assert.IsTrue(h2.handType == Straight);
            Assert.IsTrue(h1.handValue == h2.handValue);
            Assert.IsTrue(h3.handType == Straight);
            Assert.IsTrue(h1.handValue > h3.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestThreeOfAKind() {
            var tableCards = new List<PlayingCard>(new[] {
                cac, cah, cqd, c2s, c5h
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cas, cks
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c8d, cas
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3d, cas
            }), tableCards);

            Assert.IsTrue(h1.handType == ThreeOfAKind);
            Assert.IsTrue(h2.handType == ThreeOfAKind);
            Assert.IsTrue(h1.handValue > h2.handValue);
            Assert.IsTrue(h3.handType == ThreeOfAKind);
            Assert.IsTrue(h1.handValue > h3.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestTwoPair() {
            var tableCards = new List<PlayingCard>(new[] {
                cac, c8h, cqd, c2s, c5h
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cas, c8s
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, c5s
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2d, cas
            }), tableCards);

            Assert.IsTrue(h1.handType == TwoPair);
            Assert.IsTrue(h2.handType == TwoPair);
            Assert.IsTrue(h1.handValue > h2.handValue);
            Assert.IsTrue(h3.handType == TwoPair);
            Assert.IsTrue(h1.handValue > h3.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestOnePair() {
            var tableCards = new List<PlayingCard>(new[] {
                cac, c8h, cqd, c2s, c5h
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cas, cks
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckd, c5s
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2d, cjs
            }), tableCards);

            Assert.IsTrue(h1.handType == OnePair);
            Assert.IsTrue(h2.handType == OnePair);
            Assert.IsTrue(h1.handValue > h2.handValue);
            Assert.IsTrue(h3.handType == OnePair);
            Assert.IsTrue(h1.handValue > h3.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestHighCard() {
            var tableCards = new List<PlayingCard>(new[] {
                cjs, c8h, cqd, c2s, c5h
            });
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cas, cks
            }), tableCards);
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckd, c3s
            }), tableCards);
            var h3 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3d, c0s
            }), tableCards);

            Assert.IsTrue(h1.handType == HighCard);
            Assert.IsTrue(h2.handType == HighCard);
            Assert.IsTrue(h1.handValue > h2.handValue);
            Assert.IsTrue(h3.handType == HighCard);
            Assert.IsTrue(h1.handValue > h3.handValue);
        }


        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestStraightFlushLogic() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c7h, c5h
            }), new List<PlayingCard>(new[] {
                c8h, c6h, c4h, cad, ckc
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c5s, ckd
            }), new List<PlayingCard>(new[] {
                c6s, c4s, c3s, c2s, cad
            }));

            Assert.IsTrue(h1.handType == StraightFlush);
            Assert.IsTrue(h2.handType == StraightFlush);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestStraightFlushAceLow() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3h, c2h
            }), new List<PlayingCard>(new[] {
                c8h, c5h, c4h, cah, ckc
            }));

            Assert.IsTrue(h1.handType == StraightFlush);
        }
        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestStraightAceLow() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3h, c2d
            }), new List<PlayingCard>(new[] {
                c8h, c5h, c4d, cah, ckc
            }));

            Assert.IsTrue(h1.handType == Straight);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestSF2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, c2d
            }), new List<PlayingCard>(new[] {
                cjc, c0c, c9c, c8c, c7c
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cas, cks
            }), new List<PlayingCard>(new[] {
                cjd, c0d, c9d, c8d, c7d
            }));

            Assert.IsTrue(h1.handType == StraightFlush);
            Assert.IsTrue(h2.handType == StraightFlush);
            Assert.IsTrue(h1.handValue == h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest4ofak() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2h, c4s
            }), new List<PlayingCard>(new[] {
                c0c, c0d, c0d, c0s, cqd
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2h, c4s
            }), new List<PlayingCard>(new[] {
                c6d, c6h, c6s, c6c, c7s
            }));

            Assert.IsTrue(h1.handType == FourOfAKind);
            Assert.IsTrue(h2.handType == FourOfAKind);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest4ofak2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2d, c2h
            }), new List<PlayingCard>(new[] {
                c0c, c0d, c0h, c0s, c5c
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2d, c2s
            }), new List<PlayingCard>(new[] {
                c0c, c0d, c0h, c0s, c2c
            }));

            Assert.IsTrue(h1.handType == FourOfAKind);
            Assert.IsTrue(h2.handType == FourOfAKind);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestFH() {
            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2d, c3d
            }), new List<PlayingCard>(new[] {
                c0s, c0h, c0d, c4s, c4d
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2d, c3d
            }), new List<PlayingCard>(new[] {
                c9h, c9c, c9s, cah, cas
            }));

            Assert.IsTrue(h1.handType == FullHouse);
            Assert.IsTrue(h2.handType == FullHouse);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestFH2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3d, c5d
            }), new List<PlayingCard>(new[] {
                cas, cac, cah, c4d, c4c
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2h, c4d
            }), new List<PlayingCard>(new[] {
                cas, cah, cad, c3s, c3d
            }));

            Assert.IsTrue(h1.handType == FullHouse);
            Assert.IsTrue(h2.handType == FullHouse);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestFl() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, ckd
            }), new List<PlayingCard>(new[] {
                cah, cqh, c0h, c5h, c3h
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, ckd
            }), new List<PlayingCard>(new[] {
                cks, cqs, cjs, c9s, c6s
            }));

            Assert.IsTrue(h1.handType == Flush);
            Assert.IsTrue(h2.handType == Flush);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestFl2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cas, cks
            }), new List<PlayingCard>(new[] {
                cad, ckd, c7d, c6d, c2d
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, ckd
            }), new List<PlayingCard>(new[] {
                cah, cqh, c0h, c5h, c3h
            }));

            Assert.IsTrue(h1.handType == Flush);
            Assert.IsTrue(h2.handType == Flush);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestStraightLogic() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, ckh
            }), new List<PlayingCard>(new[] {
                c8s, c7s, c6h, c5h, c4s
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, ckh
            }), new List<PlayingCard>(new[] {
                c6d, c5s, c4d, c3h, c2c
            }));

            Assert.IsTrue(h1.handType == Straight);
            Assert.IsTrue(h2.handType == Straight);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestStraight2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, ckh
            }), new List<PlayingCard>(new[] {
                c8s, c7s, c6h, c5h, c4s
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                cad, ckh
            }), new List<PlayingCard>(new[] {
                c8h, c7s, c6c, c5c, c4h
            }));

            Assert.IsTrue(h1.handType == Straight);
            Assert.IsTrue(h2.handType == Straight);
            Assert.IsTrue(h1.handValue == h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest3ofk() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2h, c3h
            }), new List<PlayingCard>(new[] {
                cqs, cqc, cqd, c5s, c4c
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2h, c3s
            }), new List<PlayingCard>(new[] {
                c5c, c5h, c5d, cqd, c0c
            }));

            Assert.IsTrue(h1.handType == ThreeOfAKind);
            Assert.IsTrue(h2.handType == ThreeOfAKind);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest3ofk2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckc, cjc
            }), new List<PlayingCard>(new[] {
                c8c, c8h, c8d, cac, c2d
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckc, cjc
            }), new List<PlayingCard>(new[] {
                c8c, c8h, c8d, c5s, c3c
            }));

            Assert.IsTrue(h1.handType == ThreeOfAKind);
            Assert.IsTrue(h2.handType == ThreeOfAKind);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest2p() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3c, c4s
            }), new List<PlayingCard>(new[] {
                ckh, ckd, c2c, c2d, cjh
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3c, c4s
            }), new List<PlayingCard>(new[] {
                cjd, cjs, c0s, c0c, c9c
            }));

            Assert.IsTrue(h1.handType == TwoPair);
            Assert.IsTrue(h2.handType == TwoPair);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest2p2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3c, c4s
            }), new List<PlayingCard>(new[] {
                c9c, c9d, c7d, c7s, c6h
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3c, c4s
            }), new List<PlayingCard>(new[] {
                c9h, c9s, c5h, c5d, cks
            }));

            Assert.IsTrue(h1.handType == TwoPair);
            Assert.IsTrue(h2.handType == TwoPair);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest2p3() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2c, c5s
            }), new List<PlayingCard>(new[] {
                c4s, c4c, c3s, c3h, cks
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c2c, c5s
            }), new List<PlayingCard>(new[] {
                c4h, c4d, c3d, c3c, c0s
            }));

            Assert.IsTrue(h1.handType == TwoPair);
            Assert.IsTrue(h2.handType == TwoPair);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest1p() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3d, cjd
            }), new List<PlayingCard>(new[] {
                c0c, c0s, c6s, c4h, c2h
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                ckd, cjs
            }), new List<PlayingCard>(new[] {
                c9h, c9c, cah, cqd, c8d
            }));

            Assert.IsTrue(h1.handType == OnePair);
            Assert.IsTrue(h2.handType == OnePair);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTest1p2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c4s, c3s
            }), new List<PlayingCard>(new[] {
                c2d, c2h, c8s, c5c, c7c
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c4d, c6h
            }), new List<PlayingCard>(new[] {
                c2c, c2s, c8s, ckh, c3h
            }));

            Assert.IsTrue(h1.handType == OnePair);
            Assert.IsTrue(h2.handType == OnePair);
            Assert.IsTrue(h1.handValue < h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestHc() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3d, c2s
            }), new List<PlayingCard>(new[] {
                cad, c0d, c9s, c5c, c8c
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c6h, c3s
            }), new List<PlayingCard>(new[] {
                ckc, cqd, cjc, c8h, c7h
            }));

            Assert.IsTrue(h1.handType == HighCard);
            Assert.IsTrue(h2.handType == HighCard);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }

        [TestMethod()]
        public void PokerPlayerHandEvaluatorTestHc2() {

            var h1 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3c, c4s
            }), new List<PlayingCard>(new[] {
                cac, cqd, c7d, c6h, c2c
            }));
            var h2 = new PokerPlayerHandEvaluator(new List<PlayingCard>(new[] {
                c3s, c2s
            }), new List<PlayingCard>(new[] {
                cad, c0d, c9s, c6c, c4c
            }));

            Assert.IsTrue(h1.handType == HighCard);
            Assert.IsTrue(h2.handType == HighCard);
            Assert.IsTrue(h1.handValue > h2.handValue);
        }
    }
}