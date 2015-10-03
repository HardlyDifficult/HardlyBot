namespace Hardly.Games.Betting {
    public class ParimutuelPlayer<PlayerIdType> : GamePlayer<PlayerIdType> {
        public readonly bool toWin;

        public ParimutuelPlayer(PlayerPointManager pointManager, PlayerIdType playerId, bool toWin) : base(pointManager, playerId) {
            this.toWin = toWin;
        }
    }
}
