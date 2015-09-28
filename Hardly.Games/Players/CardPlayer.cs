namespace Hardly.Games {
    public class CardPlayer<PlayerIdObjectType> : GamePlayer<PlayerIdObjectType> {
        CardPlayerHand<PlayerIdObjectType> hand;

        public CardPlayer(PointManager pointManager, PlayerIdObjectType playerObject) : base(pointManager, playerObject) {
        }
    }
}
