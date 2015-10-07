using System;

namespace Hardly.Library.Twitch
{
    internal static class TwitchHelpers
    {
        internal static string GetAllAsCsv(TwitchUserInChannel[] followers)
        {
            string csv = "";
            if (followers != null)
            {
                for (int i = 0; i < followers.Length; i++)
                {
                    if (i > 0)
                    {
                        csv += ",";
                    }
                    csv += followers[i].user.userName;
                }
            }

            return csv;
        }
    }
}
