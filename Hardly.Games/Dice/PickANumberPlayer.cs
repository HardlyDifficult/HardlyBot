namespace Hardly.Games {
    public class PickANumberPlayer<PlayerIdType> : GamePlayer<PlayerIdType> {
        internal uint guessedNumber {
            get;
            private set;
        }

        public PickANumberPlayer(PlayerPointManager pointManager, PlayerIdType id, uint guessedNumber) : base(pointManager, id) {
            this.guessedNumber = guessedNumber;
        }
    }
}
