namespace Hardly.Games {
    public abstract class PlayingCardListEvaluator {
        public readonly PlayingCardList cards;

        public PlayingCardListEvaluator(PlayingCardList cards) {
            this.cards = cards;
        }
    }
}
