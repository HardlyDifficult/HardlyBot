using System;

namespace Hardly.Games {
	public class TexasHoldem<PlayerIdType> : CardGame<PlayerIdType, TexasHoldemPlayer<PlayerIdType>> {
        public enum Round {
            PreFlop, Flop, Turn, River, GameOver
        }
        public Round round = Round.GameOver;
        public ulong bigBlind = 0;
        uint currentPlayerId = 0, endingPlayerId = 0;
        public PlayingCardList tableCards = new PlayingCardList();
        List<TexasHoldemPlayer<PlayerIdType>>
            // Players = players which may be seated or in the sidepot (not both, if neither they folded)
            seatedPlayers = new List<TexasHoldemPlayer<PlayerIdType>>(),
            sidepotPlayers = new List<TexasHoldemPlayer<PlayerIdType>>(), // BUG: fold doesn't put you in the sidepot yet.

            // Last game
            lastGameWinners = new List<TexasHoldemPlayer<PlayerIdType>>(),
            lastGameLosers = new List<TexasHoldemPlayer<PlayerIdType>>(),
            lastGameSidepotWinners = new List<TexasHoldemPlayer<PlayerIdType>>(),
            lastGameSidepotLosers = new List<TexasHoldemPlayer<PlayerIdType>>();

        public TexasHoldem() : this(1, 6) {
        }

        public TexasHoldem(uint numberOfDecks, uint maxPlayers) : base(numberOfDecks, maxPlayers) {
            Reset();
        }

        ulong currentBet {
            get {
                ulong maxBet = 0;
                foreach(var player in seatedPlayers) {
                    if(player.bet > maxBet) {
                        maxBet = player.bet;
                    }
                }

                return maxBet;
            }
        }

        public TexasHoldemPlayer<PlayerIdType> currentPlayer {
            get {
                if(seatedPlayers != null && currentPlayerId < seatedPlayers.Count) {
                    return seatedPlayers[(int)currentPlayerId];
                }

                return null;
            }
        }
       
        public override bool StartGame() {
            if(CanStart()) {
                round = Round.PreFlop;
                ShufflePlayersAndPutInSeats();

                for(int i = 0; i < 2; i++) {
                    foreach(var player in seatedPlayers) {
                        DealCard(player.hand.cards);
                    }
                }

                uint iBig = seatedPlayers.Count >= 3 ? 2u : 0u;
                if(bigBlind > 0) {
                    ulong littleBlind = bigBlind / 2;
                    seatedPlayers[1].PlaceBet(littleBlind, true);
                    seatedPlayers[(int)iBig].PlaceBet(bigBlind, true);
                }
                
                currentPlayerId = StartingPlayerId;
                endingPlayerId = iBig;
                tableCards.Clear();

                return true;
            }

            return false;
        }

        public bool Fold() {
            if(seatedPlayers.Remove(currentPlayer)) {
                if(currentPlayerId == endingPlayerId || seatedPlayers.Count == 1) {
                    NextRound();
                } else if(currentPlayerId == seatedPlayers.Count) {
                    currentPlayerId = 0;
                }

                if(currentPlayerId < endingPlayerId) {
                    endingPlayerId--;
                }

                return true;
            }
            return false;
        }

        public bool Raise(ulong value) {
            ulong bet = currentBet - currentPlayer.bet + value;
            if(currentPlayer.PlaceBet(bet, true)) {
                endingPlayerId = currentPlayerId > 0 ? currentPlayerId - 1 : (uint)seatedPlayers.Count - 1;
                SelectNextPlayerOrNextRound();
                return true;
            }

            return false;
        }

        public bool Call() {
            if(!canCheck) {
                ulong bet = currentBet - currentPlayer.bet;
                if(currentPlayer.PlaceBet(bet, true)) {
                    SelectNextPlayerOrNextRound();
                    return true;
                } else {
                    currentPlayer.PlaceBet(bet, false);
                    sidepotPlayers.Add(currentPlayer);
                    seatedPlayers.Remove(currentPlayer);
                    if(currentPlayerId == endingPlayerId || seatedPlayers.Count == 1) {
                        NextRound();
                    }
                }
            }

            return false;
        }

        public bool Bet(ulong value) {
            if(canCheck && value > 0) {
                if(currentPlayer.PlaceBet(value, true)) {
                    endingPlayerId = currentPlayerId > 0 ? currentPlayerId - 1 : (uint)seatedPlayers.Count - 1;
                    SelectNextPlayerOrNextRound();
                    return true;
                }
            }

            return false;
        }

        public bool Join(PlayerIdType playerId, PlayerPointManager pointManager) {
			if(round != Round.GameOver && !base.Contains(playerId) && pointManager.Points > bigBlind) {
                Log.info(playerId.ToString() + " joined");
                return base.Join(playerId, new TexasHoldemPlayer<PlayerIdType>(pointManager, playerId));
			}

            return false;
		}

        public bool Check() {
            if(canCheck) {
                SelectNextPlayerOrNextRound();
                return true;
            }

            return false;
        }

        private void SelectNextPlayerOrNextRound() {
            if(currentPlayerId == endingPlayerId) {
                NextRound();
            } else {
                currentPlayerId++;
                if(currentPlayerId >= seatedPlayers.Count) {
                    currentPlayerId = 0;
                }
            }
        }

        private void NextRound() {
            Debug.Assert(round != Round.GameOver);

            foreach(var player in PlayerGameObjects) {
                if(player.bet < currentBet) {
                    seatedPlayers.Remove(player);
                }
            }

            if(seatedPlayers.Count <= 1) {
                round = Round.GameOver;
            } else {
                round++;
            }

            switch(round) {
            case Round.Flop:
                DealToTable(3);
                break;
            case Round.Turn:
                DealToTable(1);
                break;
            case Round.River:
                DealToTable(1);
                break;
            case Round.GameOver:
                EndGame();
                break;
            default:
                Debug.Fail();
                break;
            }
            
            currentPlayerId = StartingPlayerId;
            endingPlayerId = seatedPlayers.Count >= 3 ? 2u : 0u;
        }

        public ulong GetCallAmount() {
            if(currentPlayer != null) {
                return currentBet - currentPlayer.bet;
            } else {
                return 0;
            }
        }

        public ulong GetTotalPot() {
            ulong totalBet = 0;
            foreach(var player in PlayerGameObjects) {
                totalBet += player.bet;
            }

            return totalBet;
        }

        public override void EndGame() {
            lastGameWinners.Clear();
            lastGameLosers.Clear();
            lastGameSidepotWinners.Clear();
            lastGameSidepotLosers.Clear();

            foreach(var player in seatedPlayers) {
                player.EndGame(tableCards);
            }
            foreach(var player in sidepotPlayers) {
                player.EndGame(tableCards);
            }

            lastGameWinners = GetWinners(seatedPlayers);

            ulong sidepotTotalPayout = 0;
            while(sidepotPlayers.Count > 0) {
                ulong smallestSideBet = ulong.MaxValue;
                foreach(var player in sidepotPlayers) {
                    if(player.bet < smallestSideBet) {
                        smallestSideBet = player.bet;
                    }
                }

                var playersToTest = new List<TexasHoldemPlayer<PlayerIdType>>();
                foreach(var player in seatedPlayers) {
                    playersToTest.Add(player);
                }
                foreach(var player in sidepotPlayers) {
                    if(player.bet >= smallestSideBet) {
                        playersToTest.Add(player);
                    }
                }
                var sidePotWinners = GetWinners(playersToTest);
                ulong sidePayoutPerPerson = (ulong)(smallestSideBet * (ulong)playersToTest.Count) / (ulong)sidePotWinners.Count;
                foreach(var player in sidePotWinners) {
                    player.Award((long)sidePayoutPerPerson - (long)player.bet);
                    sidepotTotalPayout += sidePayoutPerPerson;
                    sidePotWinners.Add(player);
                }

                foreach(var player in playersToTest) {
                    if(player.bet == smallestSideBet) {
                        sidepotPlayers.Remove(player);
                    }
                }
            }

            ulong payoutPerPerson = (ulong)(GetTotalPot() - sidepotTotalPayout / (ulong)lastGameWinners.Count);
            foreach(var player in lastGameWinners) {
                player.Award((long)payoutPerPerson);
            }
            
            foreach(var player in seatedPlayers) {
                if(!lastGameWinners.Contains(player)) {
                    lastGameLosers.Add(player);
                }
            }
            foreach(var player in sidepotPlayers) {
                if(!lastGameSidepotWinners.Contains(player)) {
                    lastGameSidepotLosers.Add(player);
                }
            }
        }

        List<TexasHoldemPlayer<PlayerIdType>> GetWinners(List<TexasHoldemPlayer<PlayerIdType>> playersToCheck) {
            var winners = new List<TexasHoldemPlayer<PlayerIdType>>();
            if(playersToCheck.Count > 1) {
                ulong bestHandValue = 0;

                foreach(var player in playersToCheck) {
                    if(player.bestHand.handValue > bestHandValue) {
                        bestHandValue = player.bestHand.handValue;
                        winners.Clear();
                        winners.Add(player);
                    } else if(player.bestHand.handValue == bestHandValue) {
                        winners.Add(player);
                    }
                }
            } else if(playersToCheck.Count == 1) {
                winners.Clear();
                winners.Add(playersToCheck[0]);
            } else {
                Debug.Fail();
            }

            return winners;
        }

        private void DealToTable(int numberOfCards) {
            for(int i = 0; i < numberOfCards; i++) {
                DealCard(tableCards);
            }
        }
        
        uint StartingPlayerId {
            get {
                return seatedPlayers.Count > 3 ? 3u : seatedPlayers.Count == 3 ? 0u : 1u;
            }
        }

        public bool canCheck {
            get {
                return GetCallAmount() == 0 && round != Round.GameOver;
            }
        }
        
        private void ShufflePlayersAndPutInSeats() {
            seatedPlayers.Clear();
            sidepotPlayers.Clear();
                
            foreach(var player in PlayerGameObjects.ToArray().Shuffle()) {
                seatedPlayers.Add(player);
            }
        }

        public override bool CanStart() {
            return round == Round.GameOver && NumberOfPlayers() >= 2;
        }
    }
}
