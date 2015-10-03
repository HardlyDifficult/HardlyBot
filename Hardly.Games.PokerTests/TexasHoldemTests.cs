using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games.Tests {
    [TestClass()]
    public class TexasHoldemTests {
        [TestMethod()]
        public void TexasHoldemTest() {
            var game = CreateGame();

            Assert.IsTrue(!game.canBet);
            Assert.IsTrue(!(game.Bet(10) > 0));
            Assert.IsTrue(!game.canCall);
            Assert.IsTrue(!(game.Call() > 0));
            Assert.IsTrue(!game.canCheck);
            Assert.IsTrue(!game.Check());
            Assert.IsTrue(!game.canFold);
            Assert.IsTrue(!game.Fold());
            Assert.IsTrue(!game.canRaise);
            Assert.IsTrue(!(game.Raise(10) > 0));

            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);
            Assert.IsTrue(game.CanStart());
            Assert.IsTrue(game.StartGame());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);

            Assert.IsTrue(!game.canBet);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.canCall);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(!game.canCheck);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.canFold);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.canRaise);

            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(!game.canCall);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);

            
        }

        [TestMethod()]
        public void TexasHoldemTest2() {
            var game = CreateGame();
            game.StartGame();

            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(!game.canCall);
            Assert.IsTrue(game.Bet(10) > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);

            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Bet(10) > 0);
            Assert.IsTrue(!game.Check());
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Bet(10) > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Raise(10) > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Bet(10) > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Fold());
            Assert.IsTrue(game.Fold());
            Assert.IsTrue(game.Raise(10) > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Raise(10) > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);
        }

        [TestMethod()]
        public void TexasHoldemTest3() {
            var game = CreateGame();
            game.StartGame();

            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Raise(10) > 0);
            Assert.IsTrue(game.Fold());
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Bet(10) > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(!game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Bet(10) > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Raise(10) > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Bet(10) > 0);
            Assert.IsTrue(game.Fold());
            Assert.IsTrue(game.Fold());
            Assert.IsTrue(game.Raise(10) > 0);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);
            Assert.IsTrue(!(game.Raise(10) > 0));
        }

        [TestMethod()]
        public void TexasHoldemTest4() {
            var game = CreateGame(true);
            game.StartGame();

            game.Call();
            game.Call();
            game.Call();
            game.Call();
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            game.Check();
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);

            for(int i = 0; i < 5; i++) {
                if(game.currentPlayer.idObject == 0) {
                    game.Bet(8);
                    for(int j = i + 1; j < 5; j++) {
                        game.Call();
                    }
                    for(int j = 0; j < i; j++) {
                        game.Call();
                    }
                    break;
                } else {
                    game.Check();
                }
            }
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);

            Assert.IsTrue(game.lastGameSidepotWinners.IsEmpty);
            Assert.IsTrue(game.lastGameWinners.Count > 0);
        }

        [TestMethod()]
        public void TexasHoldemTest5() {
            var game = CreateGame(true);
            game.StartGame();

            game.Call();
            game.Call();
            game.Call();
            game.Call();
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            game.Check();
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);

            for(int i = 0; i < 5; i++) {
                if(game.currentPlayer.idObject == 0) {
                    game.Bet(8);
                    for(int j = i + 1; j < 5; j++) {
                        game.Call();
                    }
                    for(int j = 0; j < i; j++) {
                        game.Call();
                    }
                    break;
                } else {
                    game.Check();
                }
            }
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);

            if(game.currentPlayer.idObject == 0) {
                game.Check();
                game.Bet(2);
                game.Call();
                game.Call();
                Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
                game.Call();
                Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
                game.Call();
            } else {
                game.Bet(2);
                game.Call();
                game.Call();
                Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
                game.Call();
                Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
                game.Call();
            }
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);

            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            Assert.IsTrue(game.Check());
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);

            Assert.IsFalse(game.lastGameSidepotWinners.IsEmpty);
            Assert.IsTrue(game.lastGameWinners.Count > 0);
        }

        [TestMethod()]
        public void TexasHoldemTest6() {
            var game = CreateGame(true);
            game.StartGame();

            game.Call();
            game.Call();
            game.Call();
            game.Call();
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.PreFlop);
            game.Check();
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);

            for(int i = 0; i < 5; i++) {
                if(game.currentPlayer.idObject == 0) {
                    Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
                    game.Bet(8);
                    for(int j = i + 1; j < 5; j++) {
                        Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
                        game.Call();
                    }
                    for(int j = 0; j < i; j++) {
                        Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
                        game.Call();
                    }
                    break;
                } else {
                    Assert.IsTrue(game.round == TexasHoldem<int>.Round.Flop);
                    game.Check();
                }
            }
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);

            if(game.currentPlayer.idObject == 0) {
                game.Check();
                Assert.IsTrue(game.sidepotPlayers.Count == 0);
                game.Bet(2);
                Assert.IsTrue(game.sidepotPlayers.Count == 0);
                Assert.IsTrue(game.seatedPlayers.Count == 5);
                game.Call();
                game.Call();
                game.Call();
                Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
                game.Call();
                Assert.IsTrue(game.sidepotPlayers.Count == 1);
                Assert.IsTrue(game.seatedPlayers.Count == 4);
                Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            } else {
                Assert.IsTrue(game.sidepotPlayers.Count == 0);
                game.Bet(2);
                Assert.IsTrue(game.sidepotPlayers.Count == 0);
                Assert.IsTrue(game.seatedPlayers.Count == 5);
                game.Call();
                game.Call();
                game.Call();
                Assert.IsTrue(game.round == TexasHoldem<int>.Round.Turn);
                game.Call();
                Assert.IsTrue(game.sidepotPlayers.Count == 1);
                Assert.IsTrue(game.seatedPlayers.Count == 4);
                Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            }
            Assert.IsTrue(game.seatedPlayers.Count == 4);
            Assert.IsTrue(game.sidepotPlayers.Count == 1);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);

            Assert.IsTrue(game.Bet(10) > 0);
            Assert.IsTrue(game.seatedPlayers.Count == 4);
            Assert.IsTrue(game.sidepotPlayers.Count == 1);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            Assert.IsTrue(game.Fold());
            Assert.IsTrue(game.seatedPlayers.Count == 3);
            Assert.IsTrue(game.sidepotPlayers.Count == 2);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.seatedPlayers.Count == 3);
            Assert.IsTrue(game.sidepotPlayers.Count == 2);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.River);
            Assert.IsTrue(game.Call() > 0);
            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);

            Assert.IsTrue(game.lastGameSidepotWinners.Count >= 1);
            Assert.IsTrue(game.lastGameWinners.Count > 0);
        }

        [TestMethod()]
        public void TexasHoldemTest7() {
            TexasHoldem<int> game = new TexasHoldem<int>();
            game.bigBlind = 2;

            var pointManager1 = new PlayerPointManager();
            pointManager1.TotalPointsInAccount = 100;
            var pointManager2 = new PlayerPointManager();
            pointManager2.TotalPointsInAccount = 1000;
            game.Join(0, pointManager1);
            game.Join(1, pointManager2);

            game.StartGame();
            if(game.currentPlayer.idObject == 0) {
                game.Raise(ulong.MaxValue);
                game.Raise(ulong.MaxValue);
                game.Call();
            } else {
                game.Raise(ulong.MaxValue);
                game.Call();
            }

            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);
            Assert.IsTrue(game.lastGameWinners.Count == 1);
            Assert.IsTrue(game.lastGameSidepotWinners.Count >= 1);

            if(game.lastGameSidepotWinners.Count == 1) {
                if(game.lastGameSidepotWinners.First.Item1.idObject == 0) {
                    Assert.IsTrue(pointManager1.TotalPointsInAccount == 200);
                    Assert.IsTrue(pointManager2.TotalPointsInAccount == 900);
                } else {
                    Assert.IsTrue(pointManager1.TotalPointsInAccount == 0);
                    Assert.IsTrue(pointManager2.TotalPointsInAccount == 1100);
                }
            }
        }


        [TestMethod()]
        public void TexasHoldemTest8() {
            TexasHoldem<int> game = new TexasHoldem<int>();
            game.bigBlind = 2;

            var pointManager1 = new PlayerPointManager();
            pointManager1.TotalPointsInAccount = 100;
            var pointManager2 = new PlayerPointManager();
            pointManager2.TotalPointsInAccount = 100;
            game.Join(0, pointManager1);
            game.Join(1, pointManager2);

            game.StartGame();
            for(int i = 0; i < 2 * 4; i++) {
                Assert.IsTrue(game.round != TexasHoldem<int>.Round.GameOver);
                if(game.canCall) {
                    game.Call();
                } else {
                    game.Check();
                }
            }

            Assert.IsTrue(game.round == TexasHoldem<int>.Round.GameOver);

            if(game.lastGameWinners.First.Item1.idObject == 0) {
                Assert.IsTrue(pointManager1.TotalPointsInAccount == 102);
                Assert.IsTrue(pointManager2.TotalPointsInAccount == 98);
            } else {
                Assert.IsTrue(pointManager1.TotalPointsInAccount == 98);
                Assert.IsTrue(pointManager2.TotalPointsInAccount == 102);
            }
        }

        private static TexasHoldem<int> CreateGame(bool setPlayer1To10 = false) {
            TexasHoldem<int> game = new TexasHoldem<int>();
            game.bigBlind = 2;
            for(int i = 0; i < 5; i++) {
                var pointManager = new PlayerPointManager();
                if(setPlayer1To10 && i == 0) {
                    pointManager.TotalPointsInAccount = 10;
                } else {
                    pointManager.TotalPointsInAccount = 100;
                }
                game.Join(i, pointManager);
            }

            return game;
        }
    }
}