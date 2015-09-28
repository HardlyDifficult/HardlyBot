using System;
using System.Collections.Generic;

namespace Hardly.Games {
	public class TexasHoldem<PlayerIdType> : CardGame<PlayerIdType, TexasHoldemPlayer<PlayerIdType>> {
        public enum Round {
            PreFlop, Flop, Turn, River, GameOver
        }
        public Round round = Round.PreFlop;

        public ulong bigBlind = 0;
        ulong currentBet = 0;
        bool gameInProgress = false;
        public List<TexasHoldemPlayer<PlayerIdType>>
            seatedPlayers = new List<TexasHoldemPlayer<PlayerIdType>>();
        List<TexasHoldemPlayer<PlayerIdType>>
            sidepotPlayers = new List<TexasHoldemPlayer<PlayerIdType>>();
        uint currentPlayerId = 0, endingPlayerId = 0;
        public TexasHoldemPlayer<PlayerIdType> table;
        public List<TexasHoldemPlayer<PlayerIdType>>
            lastGameWinners = new List<TexasHoldemPlayer<PlayerIdType>>(),
            lastGameLosers = new List<TexasHoldemPlayer<PlayerIdType>>(),
            lastGameSidepotWinners = new List<TexasHoldemPlayer<PlayerIdType>>(),
            lastGameSidepotLosers = new List<TexasHoldemPlayer<PlayerIdType>>();

        public TexasHoldemPlayer<PlayerIdType> CurrentPlayer {
            get {
                if(seatedPlayers != null && currentPlayerId < seatedPlayers.Count) {
                    return seatedPlayers[(int)currentPlayerId];
                }

                return null;
            }
        }


        public TexasHoldem() : this(1, 6) {
		}

		public TexasHoldem(uint numberOfDecks, uint maxPlayers) : base(numberOfDecks, maxPlayers) {
			Reset();
		}

        public bool StartGame() {
            if(CanStart()) {
                gameInProgress = true;
                round = Round.PreFlop;

                ShufflePlayerOrder();

                for(int i = 0; i < 2; i++) {
                    foreach(var player in seatedPlayers) {
                        DealCard(player);
                    }
                }

                uint iBig = seatedPlayers.Count >= 3 ? 2u : 0u;
                if(bigBlind > 0) {
                    ulong littleBlind = bigBlind / 2;
                    seatedPlayers[1].placeBet(littleBlind, true);
                    seatedPlayers[(int)iBig].placeBet(bigBlind, true);
                }

                currentBet = bigBlind;
                currentPlayerId = StartingPlayerId;
                endingPlayerId = iBig;
                table = new TexasHoldemPlayer<PlayerIdType>(null, (PlayerIdType)typeof(PlayerIdType).GetDefaultValue());

                return true;
            }

            return false;
        }

        public bool Fold() {
            if(seatedPlayers.Remove(CurrentPlayer)) {
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

        public override void Reset() {
            base.Reset();
        }

        public bool Raise(ulong value) {
            ulong bet = currentBet - CurrentPlayer.bet + value;
            if(CurrentPlayer.placeBet(bet, true)) {
                currentBet = CurrentPlayer.bet;
                endingPlayerId = currentPlayerId > 0 ? currentPlayerId - 1 : (uint)seatedPlayers.Count - 1;
                SelectNextPlayerOrNextRound();
                return true;
            }

            return false;
        }

        public bool Call() {
            if(!canCheck) {
                ulong bet = currentBet - CurrentPlayer.bet;
                if(CurrentPlayer.placeBet(bet, true)) {
                    currentBet = CurrentPlayer.bet;
                    SelectNextPlayerOrNextRound();
                    return true;
                } else {
                    CurrentPlayer.placeBet(bet, false);
                    sidepotPlayers.Add(CurrentPlayer);
                    seatedPlayers.Remove(CurrentPlayer);
                    if(currentPlayerId == endingPlayerId || seatedPlayers.Count == 1) {
                        NextRound();
                    }
                }
            }

            return false;
        }

        public bool Bet(ulong value) {
            if(canCheck && value > 0) {
                if(CurrentPlayer.placeBet(value, true)) {
                    currentBet = CurrentPlayer.bet;
                    endingPlayerId = currentPlayerId > 0 ? currentPlayerId - 1 : (uint)seatedPlayers.Count - 1;
                    SelectNextPlayerOrNextRound();
                    return true;
                }
            }

            return false;
        }

        public bool Join(PlayerIdType playerId, PointManager pointManager) {
			if(!gameInProgress && !base.Contains(playerId) && pointManager.AvailablePoints > bigBlind) {
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

            foreach(var player in PlayerObjects) {
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
                CalculateWinners();
                break;
            default:
                Debug.Fail();
                break;
            }
            
            currentPlayerId = StartingPlayerId;
            endingPlayerId = seatedPlayers.Count >= 3 ? 2u : 0u;
        }

        public ulong GetCallAmount() {
            if(CurrentPlayer != null) {
                return currentBet - CurrentPlayer.bet;
            } else {
                return 0;
            }
        }

        public ulong GetTotalPot() {
            ulong totalBet = 0;
            foreach(var player in PlayerObjects) {
                totalBet += player.bet;
            }

            return totalBet;
        }

        private void CalculateWinners() {
            lastGameWinners.Clear();
            lastGameLosers.Clear();
            lastGameSidepotWinners.Clear();
            lastGameSidepotLosers.Clear();

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
                    player.pointManager.Award(player.bet, (long)sidePayoutPerPerson - (long)player.bet);
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
                player.pointManager.Award(player.bet, (long)payoutPerPerson - (long)player.bet);
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
            gameInProgress = false;
        }

        List<TexasHoldemPlayer<PlayerIdType>> GetWinners(List<TexasHoldemPlayer<PlayerIdType>> playersToCheck) {
            List<TexasHoldemPlayer<PlayerIdType>> winners = new List<TexasHoldemPlayer<PlayerIdType>>();
            if(playersToCheck.Count > 1) {
                ulong bestHandValue = 0;

                foreach(var player in playersToCheck) {
                    CardCollection bestPlayerHand = PokerPlayerHand.GetBestHand(player.hand, table.hand);
                    ulong playerHandValue = PokerPlayerHand.HandValue(bestPlayerHand);

                    if(playerHandValue > bestHandValue) {
                        bestHandValue = playerHandValue;
                        winners.Clear();
                        winners.Add(player);
                    } else if(playerHandValue == bestHandValue) {
                        winners.Add(player);
                    }
                }
            } else {
                winners.Add(playersToCheck[0]);
            }

            return winners;
        }

        private void DealToTable(int numberOfCards) {
            for(int i = 0; i < numberOfCards; i++) {
                DealCard(table);
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
        
        private void ShufflePlayerOrder() {
            seatedPlayers.Clear();
            sidepotPlayers.Clear();
                
            foreach(var player in PlayerObjects.ToArray().Shuffle()) {
                seatedPlayers.Add(player);
            }
        }

        public override bool CanStart() {
            return !gameInProgress && NumberOfPlayers() >= 2;
        }
    }
}
