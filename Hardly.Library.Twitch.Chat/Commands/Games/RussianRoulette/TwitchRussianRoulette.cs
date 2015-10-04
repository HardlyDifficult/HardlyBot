using Hardly.Games.Betting;

namespace Hardly.Library.Twitch {
    public class TwitchRussianRoulette : TwitchGameStateMachine<RussianRoulette<SqlTwitchUser>> {
        public TwitchRussianRoulette(TwitchChatRoom room) : base(room, typeof(RRStateOff)) {
        }
    }
}
