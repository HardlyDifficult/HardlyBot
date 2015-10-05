using System;

namespace Hardly.Games {
	public class TexasHoldem<PlayerIdType> : CardGame<TexasHoldemPlayer<PlayerIdType>, PlayerIdType, PlayingCard> {
        public enum Round {
            PreFlop, Flop, Turn, River, GameOver
        }
        public Round round = Round.GameOver;
        public ulong bigBlind = 0;
        public TexasHoldemPlayer<PlayerIdType> currentPlayer = null;
        TexasHoldemPlayer<PlayerIdType> endingPlayer = null, firstToAct = null;
        public List<PlayingCard> tableCards = new List<PlayingCard>();
        public readonly List<TexasHoldemPlayer<PlayerIdType>>
            // Players = players which may be seated or in the sidepot (not both, if neither they folded)
            seatedPlayers = new List<TexasHoldemPlayer<PlayerIdType>>();

        public List<TexasHoldemPlayer<PlayerIdType>>
            lastGameEndedInSeatPlayers = new List<TexasHoldemPlayer<PlayerIdType>>(),
            lastGameEndedInSidepotPlayers = new List<TexasHoldemPlayer<PlayerIdType>>();

        public List<Tuple<TexasHoldemPlayer<PlayerIdType>, ulong>>
            sidepotPlayers = new List<Tuple<TexasHoldemPlayer<PlayerIdType>, ulong>>(),
            lastGameWinners = new List<Tuple<TexasHoldemPlayer<PlayerIdType>, ulong>>(),
            lastGameSidepotWinners = new List<Tuple<TexasHoldemPlayer<PlayerIdType>, ulong>>();

        public TexasHoldem() : this(1, 6) {
        }

        public TexasHoldem(uint numberOfDecks, uint maxPlayers) : base(new PlayingCardDeck(numberOfDecks), 2, maxPlayers) {
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
       
        public override bool StartGame() {
            if(isReadyToStart) {
                round = Round.PreFlop;
                ShufflePlayersAndPutInSeats();

                for(int i = 0; i < 2; i++) {
                    foreach(var player in seatedPlayers) {
                        DealCard(player.hand);
                    }
                }

                uint iBig = seatedPlayers.Count >= 3 ? 2u : 0u;
                if(bigBlind > 0) {
                    ulong littleBlind = bigBlind / 2;
                    seatedPlayers[1].PlaceBet(littleBlind, true);
                    seatedPlayers[(int)iBig].PlaceBet(bigBlind, true);
                }

                uint iFirstToAct = iBig + 1;
                if(iFirstToAct >= seatedPlayers.Count) {
                    iFirstToAct = 0;
                }
                firstToAct = seatedPlayers[(int)iFirstToAct];
                currentPlayer = firstToAct;
                endingPlayer = seatedPlayers[(int)iBig];
                tableCards.Clear();

                return true;
            }

            return false;
        }

        public bool canFold {
            get {
                return round != Round.GameOver && currentPlayer != null && !canCheck;
            }
        }

        public bool Fold() {
            if(canFold) {
                uint iPlayerSeat = seatedPlayers.IndexOf(currentPlayer);
                var player = currentPlayer;
                if(seatedPlayers.Remove(player)) {
                    if(sidepotPlayers.Count > 0) {
                        sidepotPlayers.Add(Tuple.Create(player, sidepotPlayers.Last.Item2));
                    }

                    UpdateWhenPlayerDrops(iPlayerSeat);

                    return true;
                }
            }
            return false;
        }

        private void UpdateWhenPlayerDrops(uint iPlayerSeat) {
            int iNext = (int)iPlayerSeat;
            if(iNext >= seatedPlayers.Count) {
                iNext = 0;
            }
            if(currentPlayer.idObject.Equals(firstToAct.idObject)) {
                firstToAct = seatedPlayers[iNext];
            }

            if(currentPlayer.idObject.Equals(endingPlayer.idObject) || seatedPlayers.Count == 1) {
                NextRound();
            } else {
                currentPlayer = seatedPlayers[iNext];
            }
        }
        
        public bool canRaise {
            get {
                return round != Round.GameOver && currentPlayer != null && !canCheck;
            }
        }

        public ulong Raise(ulong value) {
            if(canRaise) {
                ulong bet = value == ulong.MaxValue ? ulong.MaxValue : currentBet - currentPlayer.bet + value;
                bet = currentPlayer.PlaceBet(bet, false);
                if(bet > 0) {
                    UpdateWhenPlayerBets();
                    SelectNextPlayerOrNextRound();
                    return bet;
                }
            }

            return 0;
        }

        private void UpdateWhenPlayerBets() {
            int iPrevious = (int)seatedPlayers.IndexOf(currentPlayer) - 1;
            if(iPrevious < 0) {
                iPrevious = seatedPlayers.Count - 1;
            }
            endingPlayer = seatedPlayers[iPrevious];
        }

        public bool canCall {
            get {
                return round != Round.GameOver && !canCheck && currentPlayer != null;
            }
        }

        public ulong Call() {
            if(canCall) {
                ulong bet = currentBet - currentPlayer.bet;
                Debug.Assert(bet > 0);
                ulong placedBet = currentPlayer.PlaceBet(bet, true);
                if(placedBet == 0) {
                    bet = PlaceSideBet(bet);
                } else {
                    SelectNextPlayerOrNextRound();
                }
                return bet;
            }

            return 0;
        }

        private ulong PlaceSideBet(ulong bet) {
            bet = currentPlayer.PlaceBet(bet, false);
            sidepotPlayers.Add(Tuple.Create(currentPlayer, GetTotalPot(currentPlayer.bet)));
            uint iPlayerSeat = seatedPlayers.IndexOf(currentPlayer);
            seatedPlayers.Remove(currentPlayer);
            UpdateWhenPlayerDrops(iPlayerSeat);
            
            return bet;
        }

        public bool canBet {
            get {
                return round != Round.GameOver && canCheck;
            }
        }

        public ulong Bet(ulong bet) {
            if(canBet && bet > 0) {
                bet = currentPlayer.PlaceBet(bet, false);
                if(bet > 0) {
                    UpdateWhenPlayerBets();
                    SelectNextPlayerOrNextRound();
                    return bet;
                }
            }

            return 0;
        }

        public override bool Join(TexasHoldemPlayer<PlayerIdType> player) {
            if(player.pointManager.Points > bigBlind) {
                return base.Join(player);
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
            if(currentPlayer == endingPlayer) {
                NextRound();
            } else {
                int iNext = (int)seatedPlayers.IndexOf(currentPlayer) + 1;
                if(iNext >= seatedPlayers.Count) {
                    iNext = 0;
                }
                currentPlayer = seatedPlayers[iNext];
            }
        }

        private void NextRound() {
            Debug.Assert(round != Round.GameOver);

            foreach(var player in GetPlayers()) {
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
            
            currentPlayer = firstToAct;
            int iPrevious = (int)seatedPlayers.IndexOf(firstToAct) - 1;
            if(iPrevious < 0) {
                iPrevious = seatedPlayers.Count - 1;
            }
            endingPlayer = seatedPlayers[iPrevious];
        }

        public ulong GetCallAmount() {
            if(currentPlayer != null) {
                return currentBet - currentPlayer.bet;
            } else {
                return 0;
            }
        }

        public ulong GetTotalPot(ulong lessThanOrEqualTo = ulong.MaxValue) {
            ulong totalBet = 0;
            foreach(var player in GetPlayers()) {
                totalBet += Math.Min(player.bet, lessThanOrEqualTo);
            }

            return totalBet;
        }

        protected override void EndGame() {
            while(tableCards.Count < 5) {
                DealToTable(1);
            }

            lastGameWinners.Clear();
            lastGameSidepotWinners.Clear();
            lastGameEndedInSeatPlayers = new List<TexasHoldemPlayer<PlayerIdType>>(seatedPlayers);
            lastGameEndedInSidepotPlayers = new List<TexasHoldemPlayer<PlayerIdType>>();
            foreach(var player in sidepotPlayers) {
                lastGameEndedInSidepotPlayers.Add(player.Item1);
            }

            foreach(var player in seatedPlayers) {
                player.EndGame(tableCards);
            }
            foreach(var player in sidepotPlayers) {
                player.Item1.EndGame(tableCards);
            }

            var seatedWinners = GetWinners(seatedPlayers);

            ulong sidepotTotalPayout = 0;
            while(sidepotPlayers.Count > 0) {
                ulong smallestPot = ulong.MaxValue;
                foreach(var player in sidepotPlayers) {
                    if(player.Item2 < smallestPot) {
                        smallestPot = player.Item2;
                    }
                }

                var playersToTest = new List<TexasHoldemPlayer<PlayerIdType>>();
                foreach(var player in seatedPlayers) {
                    playersToTest.Add(player);
                }
                for(int i = 0; i < sidepotPlayers.Count; i++) {
                    var player = sidepotPlayers[i];
                    if(player.Item2 >= smallestPot) {
                        playersToTest.Add(player.Item1);
                        sidepotPlayers.Remove(player);
                        i--;
                    }
                }
                var sideWinners = GetWinners(playersToTest);
                ulong sidePayoutPerPerson = (ulong)(smallestPot / (ulong)sideWinners.Count); 
                foreach(var player in sideWinners) {
                    sidepotTotalPayout += sidePayoutPerPerson;
                    ulong amount = sidePayoutPerPerson - (ulong)((double)smallestPot / playersToTest.Count);
                    lastGameSidepotWinners.Add(Tuple.Create(player, amount));
                    player.AwardPartialBet((ulong)((double)smallestPot / playersToTest.Count), (long)amount);
                }

                foreach(var player in playersToTest) {
                    if(!sideWinners.Contains(player)) {
                        player.LosePartialBet((ulong)((double)smallestPot / playersToTest.Count));
                    }
                }
            }

            ulong payoutPerPerson = (ulong)(GetTotalPot() / (ulong)seatedWinners.Count); 
            foreach(var player in seatedWinners) {
                ulong amount = payoutPerPerson - player.bet;
                lastGameWinners.Add(Tuple.Create(player, amount));
                player.Award((long)amount);
            }
            
            foreach(var player in GetPlayers()) {
                if(!(lastGameSidepotWinners.Contains(player) || lastGameWinners.Contains(player))){
                    player.LoseBet();
                }
            }

            base.EndGame();
        }

        List<TexasHoldemPlayer<PlayerIdType>> GetWinners(List<TexasHoldemPlayer<PlayerIdType>> playersToCheck) {
            List<TexasHoldemPlayer<PlayerIdType>> winners = new List<TexasHoldemPlayer<PlayerIdType>>();
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
        
        public bool canCheck {
            get {
                return GetCallAmount() == 0 && round != Round.GameOver;
            }
        }
        
        private void ShufflePlayersAndPutInSeats() {
            seatedPlayers.Clear();
            sidepotPlayers.Clear();
            seatedPlayers.Add(GetPlayers());
            seatedPlayers.Shuffle();
        }

        public override void LeaveGame(PlayerIdType playerId) {
            seatedPlayers.Remove(GetPlayer(playerId));
            base.LeaveGame(playerId);
        }
    }
}
