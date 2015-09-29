namespace Hardly.Games {
    public class PlayingCardList : List<PlayingCard> {
        public PlayingCardList(PlayingCardList existingCardList = null) : base(existingCardList) {
        }

        public PlayingCardList(PlayingCard[] existingCardList) : base(existingCardList) {
        }

        public PlayingCardList(PlayingCard existingCard) : base(existingCard) {
        }
    }
}
