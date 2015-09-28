using System;
using System.Collections.Generic;

namespace Hardly.Games {
	public class TexasHoldem<PlayerIdType> : CardGame<PlayerIdType, TexasHoldemPlayer<PlayerIdType>> {
        enum Round {
            PreFlop, Flop, Turn, River, GameOver
        }
        Round round = Round.PreFlop;

        ulong bigBlind = 0, currentBet = 0;
        bool gameInProgress = false;
        List<TexasHoldemPlayer<PlayerIdType>> seatedPlayers = new List<TexasHoldemPlayer<PlayerIdType>>();
        uint currentPlayerId = 0, endingPlayerId = 0;
        public TexasHoldemPlayer<PlayerIdType> table;
        public List<TexasHoldemPlayer<PlayerIdType>> 
            lastGameWinners = new List<TexasHoldemPlayer<PlayerIdType>>(),
            lastGameLosers = new List<TexasHoldemPlayer<PlayerIdType>>();

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

        public void StartGame() {
            Debug.Assert(CanStart());

            gameInProgress = true;
            round = Round.PreFlop;

            ShufflePlayerOrder();

            for(int i = 0; i < 2; i++) {
                foreach(var player in seatedPlayers) {
                    DealCard(player);
                }
            }

            uint iBig = seatedPlayers.Count > 3 ? 2u : seatedPlayers.Count == 3 ? 1u : 0u;
            if(bigBlind > 0) {
                ulong littleBlind = bigBlind / 2;
                seatedPlayers[1].placeBet(littleBlind, true);
                seatedPlayers[(int)iBig].placeBet(bigBlind, true);
            }

            currentBet = bigBlind;
            currentPlayerId = StartingPlayerId;
            endingPlayerId = iBig;
            table = new TexasHoldemPlayer<PlayerIdType>(null, (PlayerIdType)typeof(PlayerIdType).GetDefaultValue());
        }

        public bool Fold() {
            if(seatedPlayers.Remove(CurrentPlayer)) {
                if(currentPlayerId == endingPlayerId || seatedPlayers.Count == 1) {
                    NextRound();
                }

                return true;
            }
            return false;
        }

        public override void Reset() {
            base.Reset();
        }

        public bool Raise(ulong value) {
            if(!canCheck) {
                ulong bet = currentBet - CurrentPlayer.bet + value;
                if(CurrentPlayer.placeBet(bet, true)) {
                    currentBet = CurrentPlayer.bet;
                    SelectNextPlayerOrNextRound();
                    return true;
                }
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
            if(seatedPlayers.Count > 1) {
                ulong bestHandValue = 0;

                foreach(var player in seatedPlayers) {
                    CardCollection bestPlayerHand = PokerPlayerHand.GetBestHand(player.hand, table.hand);
                    ulong playerHandValue = PokerPlayerHand.HandValue(bestPlayerHand);

                    if(playerHandValue > bestHandValue) {
                        bestHandValue = playerHandValue;
                        lastGameWinners.Clear();
                        lastGameWinners.Add(player);
                    } else if(playerHandValue == bestHandValue) {
                        lastGameWinners.Add(player);
                    }
                }
            } else {
                lastGameWinners.Add(seatedPlayers[0]);
            }

            lastGameLosers.Clear();
            foreach(var player in seatedPlayers) {
                if(!lastGameWinners.Contains(player)) {
                    lastGameLosers.Add(player);
                }
            }

            gameInProgress = false;
        }

        private void DealToTable(int numberOfCards) {
            for(int i = 0; i < numberOfCards; i++) {
                DealCard(table);
            }
        }

        public void SetBigBlind(ulong blind) {
            bigBlind = blind;
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
                
            foreach(var player in PlayerObjects.ToArray().Shuffle()) {
                seatedPlayers.Add(player);
            }
        }

        public override bool CanStart() {
            return !gameInProgress && NumberOfPlayers() >= 2;
        }
    }
}
