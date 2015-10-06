using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch
{
    internal static class TwitchHelpers
    {
        internal static string GetAllAsCsv(SqlTwitchUserInChannel[] followers)
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
