using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardly.Library.Twitch;

namespace TwitchChatBotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TwitchChatBot chatBot = new TwitchChatBot();
            chatBot.Run();
        }
    }
}
