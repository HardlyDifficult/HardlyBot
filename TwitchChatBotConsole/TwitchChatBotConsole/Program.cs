using Hardly.Library.Twitch;
using Hardly;
using System.Windows.Forms;

namespace TwitchChatBotConsole {
    class Program {
        static void Main(string[] args) {
            new WakeUpAssembly();
            TwitchChatBot chatBot = new TwitchChatBot(new SqlTwitchFactory());
            new Thread(chatBot.Run).Start();
            //Application.Run(new Hardly.Games.Holdem.Gui.Holdem());
        }
    }
}
