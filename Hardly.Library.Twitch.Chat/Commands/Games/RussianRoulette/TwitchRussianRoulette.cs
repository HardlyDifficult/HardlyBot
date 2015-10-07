using Hardly.Games.Betting;

namespace Hardly.Library.Twitch {
    public class TwitchRussianRoulette : TwitchGameStateMachine<RussianRoulette<TwitchUser>> {
        public TwitchRussianRoulette(TwitchChatRoom room) : base(room, typeof(RRStateOff)) {
        }
    }
}
