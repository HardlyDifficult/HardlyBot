using System;

namespace Hardly.Library.Hearthstone {
    public class HearthstoneEventObserver {
        List<Action<HearthstoneEvent>> observers = new List<Action<HearthstoneEvent>>();

        public HearthstoneEventObserver() {
            FileObserver fileObserver = new FileObserver("C:\\Program Files (x86)\\Hearthstone\\Logs\\Power.log", true);
            fileObserver.RegisterObserver(NewLogLine);
        }

        private void NewLogLine(string line) {
            if(line?.EndsWith("CREATE_GAME") ?? false) {
                Observe(new NewGame());
            }
        }

        private void Observe(HearthstoneEvent hearthEvent) {
            foreach(var observer in observers) {
                observer(hearthEvent);
            }
        }

        public void RegisterObserver(Action<HearthstoneEvent> observer) {
            observers.Add(observer);
        }
    }
}
