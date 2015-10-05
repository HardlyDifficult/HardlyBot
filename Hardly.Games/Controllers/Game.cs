namespace Hardly.Games {
    public abstract class Game {
    }

	public abstract class Game<PlayerGameObjectType, PlayerIdType> : Game where PlayerGameObjectType : GamePlayer<PlayerIdType> {
		readonly uint minPlayers, maxPlayers;
        List<PlayerGameObjectType> players = new List<PlayerGameObjectType>();
        public bool gameOver {
            get;
            private set;
        }

		public Game(uint minPlayers, uint maxPlayers) {
            this.gameOver = false;
            this.minPlayers = minPlayers;
			this.maxPlayers = maxPlayers;
		}

        public bool Contains(PlayerGameObjectType playerGameObject) {
            return players.Contains(playerGameObject);
        } 

        public bool Contains(PlayerIdType playerId) {
            foreach(var player in players) {
                if(player.idObject.Equals(playerId)) {
                    return true;
                }
            }

            return false;
        }

        protected virtual void EndGame() {
            gameOver = true;
        }

        public PlayerGameObjectType GetPlayer(PlayerIdType playerId) {
            foreach(var player in players) {
                if(player.idObject.Equals(playerId)) {
                    return player;
                }
            }

            return null;
        }

        public List<PlayerGameObjectType> GetPlayers() {
            return players;
        }

        public virtual bool Join(PlayerGameObjectType playerGameObject) {
            if(!isFull) {
                players.Add(playerGameObject);
                return true;
            }

            return false;
        }

        public bool isEmpty {
            get { 
                return players.Count == 0;
            }
        }

        public bool isFull {
            get {
                return players.Count >= maxPlayers;
            }
        }

        public bool isReadyToStart {
            get {
                return numberOfPlayers >= minPlayers;
            }
        }

        public virtual void LeaveGame(PlayerIdType playerId) {
            var player = GetPlayer(playerId);
            player.CancelBet();
            players.Remove(player);
        }

        public uint numberOfOpenSpots {
            get {
                Debug.Assert(maxPlayers >= players.Count);
                return (uint)(maxPlayers - players.Count);
            }
        }

        public uint numberOfPlayers {
            get {
                return (uint)players.Count;
            }
        }

        public bool RemovePlayer(PlayerGameObjectType playerGameObject) {
            playerGameObject.CancelBet();
            return players.Remove(playerGameObject);
        }

        public virtual void Reset() {
            players.Clear();
        }

        public virtual bool StartGame() {
            return isReadyToStart;
        }

        public ulong TotalBets() {
            ulong bet = 0;
            foreach(var player in GetPlayers()) {
                bet += player.bet;
            }

            return bet;
        }
    }
}
