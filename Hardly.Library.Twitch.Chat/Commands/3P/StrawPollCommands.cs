using System;
using Hardly.Library.Strawpoll;

namespace Hardly.Library.Twitch {
    public class StrawPollCommands : TwitchCommandController {
        public StrawPollCommands(TwitchChatRoom room) : base(room) {
            ChatCommand.Create(room, "getvote", GetVote, "Gets a strawpoll winner", new[] { "viewpoll" }, false, TimeSpan.FromSeconds(30), false);
        }

        private void GetVote(SqlTwitchUser speaker, string additionalText) {
            uint pollNumber;
            if(uint.TryParse(additionalText, out pollNumber)) {
                string pollResults = Strawpoll.Strawpoll.GetWinner(pollNumber);
                room.SendChatMessage(pollResults);
            } else {
                room.SendWhisper(speaker, "Sorry, I didn't catch that poll number... try again - !getvote 123");
            }
        }
    }
}
