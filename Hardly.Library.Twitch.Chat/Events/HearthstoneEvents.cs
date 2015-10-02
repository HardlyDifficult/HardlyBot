using System;
using Hardly.Library.Hearthstone;

namespace Hardly.Library.Twitch {
    public class HearthstoneEvents : TwitchEventHandler {
        Throttle tempThrottle = new Throttle(TimeSpan.FromSeconds(5));
        public HearthstoneEvents(TwitchChatRoom room) : base(room) {
            HearthstoneEventObserver hearthObserver = new HearthstoneEventObserver();
            hearthObserver.RegisterObserver(HearthEvent);
        }

        private void HearthEvent(HearthstoneEvent hearthEvent) {
            if(hearthEvent is NewGame) {
                if(tempThrottle.ExecuteIfReady(0)) {
                    room.SendChatMessage("Hearthstone --- just started a new game!!");
                }
            }
        }
    }
}
