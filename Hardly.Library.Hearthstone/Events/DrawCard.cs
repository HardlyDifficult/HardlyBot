namespace Hardly.Library.Hearthstone {
    public class DrawCard : HearthstoneEvent {
        public readonly bool myTurn;
        public readonly HearthstoneCard card;

        public DrawCard(HearthGame game, bool myTurn, HearthstoneCard card) : base(game) {
            this.myTurn = myTurn;
            this.card = card;
        }
    }
}
