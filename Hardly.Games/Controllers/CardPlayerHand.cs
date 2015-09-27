namespace Hardly.Games {
	public class CardPlayerHand<PlayerIdType> {
		public readonly CardCollection hand;
        public readonly PointManager pointManager;
        public readonly PlayerIdType playerIdObject;

        public ulong bet {
            get;
            internal set;
        }

        public bool placeBet(ulong amount, bool allIn) {
            amount = pointManager?.ReserveBet(amount, allIn) ?? amount;
            if(pointManager != null && amount > 0) {
                bet += amount;
                return true;
            }

            return false;
        }

        public CardPlayerHand(PointManager pointManager, PlayerIdType playerObject) {
			hand = new CardCollection();
            this.pointManager = pointManager;
            this.playerIdObject = playerObject;
		}
	}
}
