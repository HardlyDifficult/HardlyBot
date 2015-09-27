namespace Hardly.Games {
	public class TexasHoldem<PlayerIdType> : CardGame<PlayerIdType, TexasHoldemPlayer> {
		public TexasHoldem() : this(1, 6) {
		}
		public TexasHoldem(uint numberOfDecks, uint maxPlayers) : base(numberOfDecks, maxPlayers) {
			Reset();
		}

		public void Join(PlayerIdType playerId, ulong bet) {
			if(!base.Contains(playerId)) {
				base.Join(playerId, new TexasHoldemPlayer(bet));
				Log.info(playerId.ToString() + " joined");
			}
		}
	}
}
