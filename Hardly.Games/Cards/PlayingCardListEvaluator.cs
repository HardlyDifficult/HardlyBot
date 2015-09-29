namespace Hardly.Games {
    public abstract class PlayingCardListEvaluator {
        public PlayingCardList cards {
            get;
            protected set;
        }

        public PlayingCardListEvaluator(PlayingCardList cards) {
            this.cards = cards;
        }
    }
}
