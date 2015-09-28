using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch.Commands.System {
    class HelloCommand : TwitchCommandController {
        public HelloCommand(TwitchChatRoom room) : base(room) {
            ChatCommand.Create(room, "hi", HiCommand, "Says hello", null, false, TimeSpan.FromSeconds(0), false);
            ChatCommand.Create(room, "echo", EchoCommand, "Echos what you say", null, false, TimeSpan.FromSeconds(0), false);
        }

        private void EchoCommand(SqlTwitchUser speaker, String additionalText) {
            string message = "You said ";
            if(additionalText == null) {
                message += "...nothing!";
            } else {
                if(additionalText.ToLower().Contains("fuck")) {
                    message = "Hey now.";
                } else {
                    message += additionalText;
                }
            }
            room.SendWhisper(speaker, message);
        }

        private void HiCommand(SqlTwitchUser speaker, String additionalText) {
            room.SendWhisper(speaker, "Hello");
        }
    }
}
