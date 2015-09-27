using System;
using System.Collections.Generic;


namespace Hardly.Library.Twitch
{
    class TwitchChatUnknownEvent : TwitchChatEvent
    {
        string _command;

        public TwitchChatUnknownEvent(string command) 
        {
            _command = command;
        }

        public override string ToString()
        {
            return _command;
        }

        internal override void RespondToEvent(LinkedList<TwitchChatRoom> chatRooms)
        {
            Log.info("Twitch unknown command: " + _command);

            // Do nothing
        }
    }
}
