using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardly.Games;

namespace Hardly.Library.Twitch {
    public abstract class TwitchGame<GamePlayerObjectType> : Game<SqlTwitchUser, GamePlayerObjectType>, IAutoJoinTwitchRooms {
        protected TwitchChatRoom room;

        public TwitchGame(TwitchChatRoom room, uint maxPlayers) : base(maxPlayers) {
            this.room = room;
        }
    }
}
