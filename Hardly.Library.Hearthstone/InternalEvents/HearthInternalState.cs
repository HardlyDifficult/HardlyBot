namespace Hardly.Library.Hearthstone {
    public abstract class HearthInternalState {
        protected readonly HearthstoneEventObserver eventObserver;

        public HearthInternalState(HearthstoneEventObserver eventObserver) {
            this.eventObserver = eventObserver;
        }

        internal abstract HearthInternalState NewLogLine(string line, ref HearthGame game);
    }
}
