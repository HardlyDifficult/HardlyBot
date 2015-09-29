using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardly.Games;

namespace Hardly.Library.Twitch {
    public abstract class TwitchGame<GameLogicController> 
        : IAutoJoinTwitchRooms where GameLogicController : Game, new() {
        public readonly GameLogicController game;
        public readonly TwitchChatRoom room;

        public TwitchGame(TwitchChatRoom room) {
            this.room = room;
            game = new GameLogicController();
        }
    }
}
