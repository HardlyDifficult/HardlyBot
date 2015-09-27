namespace Hardly.Games {
	public class TexasHoldemPlayer : CardPlayerHand {
		ulong bet;

		public TexasHoldemPlayer(ulong ante) {
			this.bet = ante;
		}

		public ulong totalBet {
			get {
				return bet;
			}
		}
	}
}