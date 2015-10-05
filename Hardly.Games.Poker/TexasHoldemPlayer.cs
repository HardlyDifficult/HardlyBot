namespace Hardly.Games {
	public class TexasHoldemPlayer<PlayerIdType> : GamePlayer<PlayerIdType> {
        public readonly List<PlayingCard> hand;

        public TexasHoldemPlayer(PlayerPointManager pointManager, PlayerIdType playerIdObject) : base(pointManager, playerIdObject) {
            hand = new List<PlayingCard>();
            bestHand = null;
		}

        public PokerPlayerHandEvaluator bestHand {
            get;
            private set;
        }

        internal void EndGame(List<PlayingCard> tableCards) {
            if(bestHand == null && tableCards.Count == 5) {
                bestHand = new PokerPlayerHandEvaluator(hand, tableCards);
            }
        }
    }
}