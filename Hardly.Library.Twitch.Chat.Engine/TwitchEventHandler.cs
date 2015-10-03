namespace Hardly.Library.Twitch {
    public class TwitchEventHandler : IAutoJoinTwitchRooms {
        protected TwitchChatRoom room;

        public TwitchEventHandler(TwitchChatRoom room) {
            this.room = room;
        }
    }
}
