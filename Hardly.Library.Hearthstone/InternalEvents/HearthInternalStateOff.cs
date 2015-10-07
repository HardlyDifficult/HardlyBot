namespace Hardly.Library.Hearthstone {
    public class HearthInternalStateOff : HearthInternalState {
        public HearthInternalStateOff(HearthstoneEventObserver eventObserver, bool? weWon = null) : base(eventObserver) {
            eventObserver.Observe(new EndOfGame(eventObserver.currentGame, weWon));
        }
        
        internal override HearthInternalState NewLogLine(string line, ref HearthGame game) {
            if(line?.EndsWith("CREATE_GAME") ?? false) {
                return new HearthInternalStartingNewGame(eventObserver);
            }

            return this;
        }
    }
}
