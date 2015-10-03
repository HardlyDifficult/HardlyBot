using System;

namespace Hardly.Library.Twitch {
    class AboutCommands : TwitchCommandController {
        public AboutCommands(TwitchChatRoom room) : base(room) {
            ChatCommand.Create(room, "commands", ListCommands, "Lists all active commands", null, false, null, false);
        }

        void ListCommands(SqlTwitchUser speaker, string additionalText) {
            var commands = ChatCommand.ForRoom(room);

            string chatMessage = "";
            if(commands != null) {
                foreach(var command in commands) {
                    if(!command.modOnly && command.enabled) {
                        if(chatMessage.Length > 0) {
                            chatMessage += ", ";
                        }
                        chatMessage += command.commandName;
                    }
                }
            }

            room.SendWhisper(speaker, chatMessage);
        }
    }
}
