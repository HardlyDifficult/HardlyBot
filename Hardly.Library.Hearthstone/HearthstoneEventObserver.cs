using System;

namespace Hardly.Library.Hearthstone {
    public class HearthstoneEventObserver {
        List<Action<HearthstoneEvent>> observers = new List<Action<HearthstoneEvent>>();
        FileObserver fileObserver;
        internal HearthGame currentGame = null;
        HearthInternalState currentState;
        public readonly IHearthstoneFactory factory;

        public HearthstoneEventObserver(IHearthstoneFactory factory) {
            this.factory = factory;
            currentState = new HearthInternalStateOff(this);
            fileObserver = new FileObserver("C:\\Program Files (x86)\\Hearthstone\\Logs\\Power.log", true);
            new Timer(TimeSpan.FromSeconds(10), DelayedObserving).Start();
        }

        private void DelayedObserving() {
            Log.debug("Hearthstone Event Observer has begun watching the log.");
            fileObserver.RegisterObserver(NewLogLine);
        }

        private void NewLogLine(string line) {
            currentState = currentState.NewLogLine(line, ref currentGame);
        }

        internal void Observe(HearthstoneEvent hearthEvent) {
            foreach(var observer in observers) {
                observer(hearthEvent);
            }
        }

        public void RegisterObserver(Action<HearthstoneEvent> observer) {
            observers.Add(observer);
        }
    }
}
