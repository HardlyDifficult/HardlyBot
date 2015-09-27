using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch
{
    public static class Twitch
    {
        static Throttle liveFollowersThrottle = new Throttle(TimeSpan.FromMinutes(5)),
            newFollowersThrottle = new Throttle(TimeSpan.FromMinutes(1)),
            refreshAllFollowersThrottle = new Throttle(TimeSpan.FromMinutes(5));

        public static SqlTwitchFollower[] GetNewFollowers(SqlTwitchAlert alerts)
        {
            try
            {
                if (newFollowersThrottle.ExecuteIfReady(alerts.connection.channel.user.id))
                {
                    TwitchApi.UpdateNewFollowers(alerts.connection, 25, 0);
                }
                return SqlTwitchFollower.GetNew(alerts);
            }
            catch (Exception e)
            {
                Log.error("Twitch get new followers", e);
                return null;
            }
        }

        public static SqlTwitchUser GetUserFromName(string username)
        {
            try
            {
                SqlTwitchUser user = SqlTwitchUser.GetFromName(username);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return TwitchApi.GetUser(username);
                }
            }
            catch (Exception e)
            {
                Log.error("Twitch get user from name", e);
                return null;
            }
        }

        public static SqlTwitchChannel[] GetLiveFollowers(SqlTwitchConnection connection)
        {
            try
            {
                if (liveFollowersThrottle.ExecuteIfReady(connection.channel.user.id))
                {
                    SqlTwitchChannel.ClearLiveFollowers(connection.channel);
                    TwitchApi.UpdateLiveFollowers(connection, 25, 0);
                }
                return SqlTwitchChannel.GetAllLiveFollowers(connection.channel);
            }
            catch (Exception e)
            {
                Log.error("Twitch live followers", e);
                return null;
            }
        }

        public static void RefreshAllFollowers(SqlTwitchConnection connection)
        {
            try
            {
                if (refreshAllFollowersThrottle.ExecuteIfReady(connection.channel.user.id))
                {
                    SqlTwitchFollower.ClearAll(connection.channel);
                    TwitchApi.UpdateNewFollowers(connection, 100, 0, true);
                }
            }
            catch (Exception e)
            {
                Log.exception(e);
            }
        }
    }
}
