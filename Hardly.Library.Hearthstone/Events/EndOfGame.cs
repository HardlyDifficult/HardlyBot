namespace Hardly.Library.Hearthstone {
    public class EndOfGame : HearthstoneEvent {
        public bool? iWon;

        public EndOfGame(HearthGame game, bool? iWon) : base(game) {
            this.iWon = iWon;
        }
    }
}
