namespace Hardly.Games {
    public class CardPlayer<PlayerIdObjectType> : GamePlayer<PlayerIdObjectType> {
        CardPlayerHand<PlayerIdObjectType> hand;

        public CardPlayer(PlayerPointManager pointManager, PlayerIdObjectType playerObject) : base(pointManager, playerObject) {
        }
    }
}
