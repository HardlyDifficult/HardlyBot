namespace Hardly.Library.Hearthstone {
    public abstract class HearthstoneEvent {
        public readonly HearthGame game;

        public HearthstoneEvent(HearthGame game) {
            this.game = game;
        }
    }
}