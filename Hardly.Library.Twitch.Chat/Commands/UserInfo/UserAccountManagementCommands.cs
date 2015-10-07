using System;

namespace Hardly.Library.Twitch {
    public class UserAccountManagementCommands : TwitchCommandController {
        public UserAccountManagementCommands(TwitchChatRoom room) : base(room) {
            ChatCommand.Create(room, "setgreeting", SetGreetingCommand, "Sets the welcome greeting message", null, false, null, false);
            ChatCommand.Create(room, "cleargreeting", ClearGreetingCommand, "Clears your greeting message", null, false, null, false);
        }

        private void ClearGreetingCommand(TwitchUser speaker, string additionalText) {
            TwitchUserInChannel inChannel = room.factory.GetUserInChannel(speaker, room.twitchConnection.channel);
            inChannel.Load();
            inChannel.greetingMessage = null;
            inChannel.Save();
        }

        private void SetGreetingCommand(TwitchUser speaker, string additionalText) {
            if(additionalText != null) {
                TwitchUserInChannel inChannel = room.factory.GetUserInChannel(speaker, room.twitchConnection.channel);
                inChannel.Load();
                inChannel.greetingMessage = additionalText;
                var test = inChannel.Save();
                Debug.Assert(test);
            }
        }
    }
}
