using Hardly.Library.Twitch;
using Hardly;
using System.Windows.Forms;

namespace TwitchChatBotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //TwitchChatBot chatBot = new TwitchChatBot();
            //new Thread(chatBot.Run).Start();
            Application.Run(new Hardly.Games.Holdem.Gui.Holdem());
        }
    }
}
