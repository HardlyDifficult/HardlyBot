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
            ChatCommand.Create(room, "timemeout", TimeMeOut, "Shhh...", null, false, TimeSpan.FromSeconds(0), false);
            ChatCommand.Create(room, "banme", BanYouBecauseYouAreStupidAndYouShouldntHaveDoneThat, "This will ban you. Dont do it unless you want to be benned.", null, false, TimeSpan.FromSeconds(0), false);
        }

        private void EchoCommand(SqlTwitchUser speaker, String additionalText) {
            string message = "You said ";
            if (additionalText == null) {
                message += "...nothing!";
            }
            else {
                if (additionalText.ToLower().Contains("fuck")) {
                    message = "Hey now.";
                }
                else {
                    message += additionalText;
                }
            }
            room.SendWhisper(speaker, message);
        }
        private void TimeMeOut(SqlTwitchUser speaker, String time) {
            try {
                if (time.IsEmpty()) {
                    room.SendChatMessage(".timeout " + speaker.userName + " " + Random.Uint.Between(1, 600));
                }
                else {
                    room.SendChatMessage(".timeout " + speaker.userName + " " + Int32.Parse(time));
                }
            }
            catch (Exception) {
                //*do nothing ever i want this to be (somewhat)secret*//
            }
        }
        private void BanYouBecauseYouAreStupidAndYouShouldntHaveDoneThat(SqlTwitchUser speaker, String lastWords) { //yay i changed my ide ti keep the brackets on this line!
            room.SendChatMessage(".ban " + speaker.userName); //if this line gets ran you deserve it.
            room.SendChatMessage(speaker.userName + " is now banned. Everyone in the chat should type 'F' or riPepperonis to pay respects."); //lol jk dont do this
            lastWords = null; //noone cares so im deleting your words
        }//Oh GOD do i feel dirty....
        
        private void HiCommand(SqlTwitchUser speaker, String additionalText) {
            room.SendWhisper(speaker, "Hello");
        }
    }
}
