using System;

namespace Hardly.Library.Twitch {
    public class DynamicCommands : TwitchCommandController {
        public DynamicCommands(TwitchChatRoom room) : base(room) {
            var commands = SqlTwitchCommand.GetAll(room.twitchConnection);
            foreach(var command in commands) {
                ChatCommand.Create(room, command.command, ActionWithStaticData<string, SqlTwitchUser, string>.For(DynamicCommandResponse, command.response), command.description, null, command.isModOnly, command.coolDown, false);
            }
        }

        void DynamicCommandResponse(string staticData, SqlTwitchUser speaker, string additionalText) {
            room.SendChatMessage(staticData);
        }

        // TODO - in order to create a new command, it must be registered with the room (like shown above).. that allows it to start working right away.
        //and you should call .Save() on the SqlTwitchCommand object... that puts it in the DB for next app restart.
    }
}
