namespace Hardly.Games {
    /// <summary>
    /// Game Player object should live for the life of one full (e.g. one game of holdem, then new GamePlayers for the next)
    /// </summary>
    public class GamePlayer<PlayerIdObjectType> {
        public readonly PointManager pointManager;
        public readonly PlayerIdObjectType playerIdObject;

        public ulong bet {
            get;
            internal set;
        }

        public GamePlayer(PointManager pointManager, PlayerIdObjectType playerObject) {
            this.pointManager = pointManager;
            this.playerIdObject = playerObject;
        }

        public bool placeBet(ulong amount, bool allIn) {
            amount = pointManager?.ReserveBet(amount, allIn) ?? amount;
            if(pointManager != null && amount > 0) {
                bet += amount;
                return true;
            }

            return false;
        }
    }
}
