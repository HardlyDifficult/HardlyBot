namespace Hardly.Games.Uno {
    public class UnoPlayer<PlayerIdType> : GamePlayer<PlayerIdType> {
        public readonly List<UnoCard> hand = new List<UnoCard>();

        public UnoPlayer(PlayerPointManager pointManager, PlayerIdType id) : base(pointManager, id) {
        }
    }
}
