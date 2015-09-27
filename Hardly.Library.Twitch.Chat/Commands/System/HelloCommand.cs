using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch.Commands.System {
    class HelloCommand : TwitchCommandController {
        public HelloCommand(TwitchChatRoom room) : base(room) {
            ChatCommand.Create(room, "hi", HiCommand, "Says hello", null, false, TimeSpan.FromSeconds(0), false);
        }

        private void HiCommand(SqlTwitchUser speaker, String additionalText) {
            room.SendWhisper(speaker, "Hello");
        }
    }
}
