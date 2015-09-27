using System.Collections.Generic;

namespace Hardly.Games {
	public abstract class Game<PlayerIdType, PlayerGameType> {
		public readonly uint maxPlayers;
		Dictionary<PlayerIdType, PlayerGameType> players = new Dictionary<PlayerIdType, PlayerGameType>();

		public Game(uint maxPlayers) {
			this.maxPlayers = maxPlayers;
		}

		public virtual void Reset() {
			players.Clear();
		}

		public bool IsFull() {
			return players.Count >= maxPlayers;
		}
        
        public virtual bool CanStart() {
            return NumberOfPlayers() >= 1;
        }

        public int NumberOfPlayers() {
            return players.Count;
        }

        public bool IsEmpty() {
			return players.Count == 0;
		}

		public bool Contains(PlayerIdType playerId) {
			return players.ContainsKey(playerId);
		}

        public Dictionary<PlayerIdType, PlayerGameType> GetPlayersAndObjects() {
            return players;
        }

        public bool Join(PlayerIdType playerId, PlayerGameType gameObject) {
            if(!IsFull()) {
                players.Add(playerId, gameObject);
                return true;
            }

            return false;
        }

        public int NumberOfOpenSpots() {
            return (int)(maxPlayers - players.Count);
        }
        
        public Dictionary<PlayerIdType, PlayerGameType>.ValueCollection PlayerObjects {
            get {
                return players.Values;
            }
        }

        public PlayerGameType Get(PlayerIdType player) {
            PlayerGameType results;
            if(players.TryGetValue(player, out results)) {
                return results;
            }

            return (PlayerGameType)typeof(PlayerGameType).GetDefaultValue();
        }
    }
}
