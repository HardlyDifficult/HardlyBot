namespace Hardly.Games {
    public class CardPlayerHand<PlayerIdType> {
        public readonly PlayingCardList cards;

        public CardPlayerHand() {
            cards = new PlayingCardList();
        }
    }
}
