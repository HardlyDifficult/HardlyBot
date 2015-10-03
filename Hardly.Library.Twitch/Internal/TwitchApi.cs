using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch
{
    internal static class TwitchApi
    {
        internal static SqlTwitchUser GetUser(string username)
        {
            var user = TwitchJson.ParseUser(WebClient.GetHTML("https://api.twitch.tv/kraken/users/" + username));
            user.Save(true);
            return user;
        }

        internal static void UpdateLiveFollowers(SqlTwitchConnection connection, uint limit, uint offset)
        {
            string json = GetLiveFollowersJson(connection, limit, offset);
            SqlTwitchChannel[] liveChannels = TwitchJson.ParseStreams(json);

            if (liveChannels.Length >= limit)
            {
                Log.info("Live Followers, requesting another page...");
                UpdateLiveFollowers(connection, 100, offset + (uint)liveChannels.Length);
            }
        }

        internal static void UpdateNewFollowers(SqlTwitchConnection connection, uint limit, uint offset, bool forceNextPage = false)
        {
            string json = GetFollowerJson(connection, limit, offset);
            SqlTwitchFollower[] followers = TwitchJson.ParseFollowers(json);

            if (followers != null && followers.Length > 0)
            {
                bool nextPage = forceNextPage;
                foreach (SqlTwitchFollower follower in followers)
                {
                    follower.Save(false);
                    nextPage = nextPage || follower.HasChangedDb;
                }

                if (nextPage)
                {
                    Log.info("Followers, requesting another page...");
                    UpdateNewFollowers(connection, 100, offset + (uint)followers.Length, forceNextPage);
                }
            }
        }

        static string GetLiveFollowersJson(SqlTwitchConnection connection, uint limit, uint offset)
        {
            SqlTwitchFollower[] followers = SqlTwitchFollower.GetAllWithUsernames(connection.channel);
            return GetJson(connection.bot, limit, offset, "streams/?channel="
                    + TwitchHelpers.GetAllAsCsv(followers));
        }

        static string GetFollowerJson(SqlTwitchConnection connection, uint limit, uint offset)
        {
            return GetJson(connection.bot, limit, offset, "channels/" + connection.channel.user.userName + "/follows?");
        }

        static string GetJson(SqlTwitchBot bot, uint limit, uint offset, string path)
        {
            return WebClient.GetHTML("https://api.twitch.tv/kraken/" + path + "&oauth_token=" + bot.oauthPassword + "&limit=" + limit
                + "&offset=" + offset);
        }
    }
}
