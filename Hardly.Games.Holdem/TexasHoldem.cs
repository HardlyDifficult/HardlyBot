using System;

namespace Hardly.Games {
	public class TexasHoldem<PlayerIdType> : CardGame<PlayerIdType, TexasHoldemPlayer<PlayerIdType>> {
        enum Round {
            PreFlop, Flop, Turn, River, GameOver
        }
        Round round = Round.PreFlop;

        ulong bigBlind = 0;
        bool gameInProgress = false;
        TexasHoldemPlayer<PlayerIdType>[] seatedPlayers = null;
        uint currentPlayerId = 0;
        public TexasHoldemPlayer<PlayerIdType> table;


        public TexasHoldemPlayer<PlayerIdType> CurrentPlayer {
            get {
                if(seatedPlayers != null && currentPlayerId < seatedPlayers.Length) {
                    return seatedPlayers[currentPlayerId];
                }

                return null;
            }
        }

        public TexasHoldem() : this(1, 6) {
		}

		public TexasHoldem(uint numberOfDecks, uint maxPlayers) : base(numberOfDecks, maxPlayers) {
			Reset();
		}

        public override void Reset() {
            base.Reset();

            table = new TexasHoldemPlayer<PlayerIdType>(null, (PlayerIdType)typeof(PlayerIdType).GetDefaultValue());
        }

        public bool Join(PlayerIdType playerId, PointManager pointManager) {
			if(!gameInProgress && !base.Contains(playerId) && pointManager.AvailablePoints > bigBlind) {
                Log.info(playerId.ToString() + " joined");
                return base.Join(playerId, new TexasHoldemPlayer<PlayerIdType>(pointManager, playerId));
			}

            return false;
		}

        public void Check() {
            Debug.Assert(CurrentPlayer != null);

            SelectNextPlayerOrNextRound();
        }

        private void SelectNextPlayerOrNextRound() {
            currentPlayerId++;
            if(currentPlayerId >= seatedPlayers.Length) {
                currentPlayerId = 0;
            }

            if(currentPlayerId == StartingPlayerId) {
                NextRound();
            }
        }

        private void NextRound() {
            Debug.Assert(round != Round.GameOver);

            round++;

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

                break;
            default:
                Debug.Fail();
                break;
            }

            currentPlayerId = StartingPlayerId;
        }

        private void DealToTable(int numberOfCards) {
            for(int i = 0; i < numberOfCards; i++) {
                DealCard(table);
            }
        }

        public void SetBigBlind(ulong blind) {
            bigBlind = blind;
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

            if(bigBlind > 0) {
                ulong littleBlind = bigBlind / 2;
                seatedPlayers[1].placeBet(bigBlind, true);
                int iLittle = seatedPlayers.Length > 2 ? 2 : 0;
                seatedPlayers[iLittle].placeBet(littleBlind, true);
            }

            currentPlayerId = StartingPlayerId;
        }

        uint StartingPlayerId {
            get {
                return seatedPlayers.Length > 2 ? 2u : 0u;
            }
        }

        private void ShufflePlayerOrder() {
            seatedPlayers = new TexasHoldemPlayer<PlayerIdType>[NumberOfPlayers()];

            uint i = 0;
            foreach(var player in PlayerObjects.ToArray().Shuffle()) {
                seatedPlayers[i++] = player;
            }
        }

        public override bool CanStart() {
            return !gameInProgress && NumberOfPlayers() >= 2;
        }
    }
}
