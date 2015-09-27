using System;

namespace Hardly.Games {
	public class TexasHoldem<PlayerIdType> : CardGame<PlayerIdType, TexasHoldemPlayer<PlayerIdType>> {
        ulong bigBlind = 0;
        bool gameInProgress = false;
        TexasHoldemPlayer<PlayerIdType>[] seatedPlayers = null;
        uint currentPlayerId = 0;

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

		public bool Join(PlayerIdType playerId, PointManager pointManager) {
			if(!gameInProgress && !base.Contains(playerId) && pointManager.AvailablePoints > bigBlind) {
                Log.info(playerId.ToString() + " joined");
                return base.Join(playerId, new TexasHoldemPlayer<PlayerIdType>(pointManager, playerId));
			}

            return false;
		}

        public void SetBigBlind(ulong blind) {
            bigBlind = blind;
        }

        public void StartGame() {
            gameInProgress = true;

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
