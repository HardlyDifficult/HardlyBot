namespace Hardly.Games {
    public class CardPlayer<PlayerIdObjectType> : GamePlayer<PlayerIdObjectType> {
        public readonly CardPlayerHand<PlayerIdObjectType> hand;

        public CardPlayer(PlayerPointManager pointManager, PlayerIdObjectType playerObject) : base(pointManager, playerObject) {
            hand = new CardPlayerHand<PlayerIdObjectType>();
        }
    }
}
