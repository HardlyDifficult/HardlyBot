using System.Collections.Generic;

namespace Hardly.Games {
	public abstract class Game<PlayerIdType, PlayerGameType> {
		public readonly uint maxPlayers;
		public Dictionary<PlayerIdType, PlayerGameType> players = new Dictionary<PlayerIdType, PlayerGameType>();

		public Game(uint maxPlayers) {
			this.maxPlayers = maxPlayers;
		}

		public virtual void Reset() {
			players.Clear();
		}

		public bool IsFull() {
			return players.Count >= maxPlayers;
		}

		public bool IsEmpty() {
			return players.Count == 0;
		}

		public bool Contains(PlayerIdType playerId) {
			return players.ContainsKey(playerId);
		}

		public void Join(PlayerIdType playerId, PlayerGameType gameObject) {
			players.Add(playerId, gameObject);
		}
	}
}
