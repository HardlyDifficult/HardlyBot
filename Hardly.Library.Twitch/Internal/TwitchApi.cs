namespace Hardly.Library.Twitch {
    public class TwitchApi {
        ITwitchFactory factory;
        TwitchJson twitchJson;

        public TwitchApi(ITwitchFactory factory) {
            this.factory = factory;
            twitchJson = new TwitchJson(factory);
        }

        public TwitchUser GetUser(string username) {
            if(username != null && username.Length > 0) {
                var user = twitchJson.ParseUser(WebClient.GetHTML("https://api.twitch.tv/kraken/users/" + username));
                user.Save(true);
                return user;
            } else {
                return null;
            }
        }

        internal void UpdateLiveFollowers(TwitchConnection connection, uint limit, uint offset) {
            string json = GetLiveFollowersJson(connection, limit, offset);
            TwitchChannel[] liveChannels = twitchJson.ParseStreams(json);

            if(liveChannels.Length >= limit) {
                Log.info("Live Followers, requesting another page...");
                UpdateLiveFollowers(connection, 100, offset + (uint)liveChannels.Length);
            }
        }

        internal void UpdateNewFollowers(TwitchConnection connection, uint limit, uint offset, bool forceNextPage = false) {
            string json = GetFollowerJson(connection, limit, offset);
            TwitchUserInChannel[] followers = twitchJson.ParseFollowers(json);

            if(followers != null && followers.Length > 0) {
                bool nextPage = forceNextPage;
                foreach(TwitchUserInChannel follower in followers) {
                    follower.Save(false);
                    nextPage = nextPage || follower.HasChangedDb;
                }

                if(nextPage) {
                    Log.info("Followers, requesting another page...");
                    UpdateNewFollowers(connection, 100, offset + (uint)followers.Length, forceNextPage);
                }
            }
        }

        string GetLiveFollowersJson(TwitchConnection connection, uint limit, uint offset) {
            TwitchUserInChannel[] followers = factory.GetAllUserInChannels(connection.channel);
            return GetJson(connection.bot, limit, offset, "streams/?channel="
                    + TwitchHelpers.GetAllAsCsv(followers));
        }

        static string GetFollowerJson(TwitchConnection connection, uint limit, uint offset) {
            return GetJson(connection.bot, limit, offset, "channels/" + connection.channel.user.userName + "/follows?");
        }

        static string GetJson(TwitchBot bot, uint limit, uint offset, string path) {
            return WebClient.GetHTML("https://api.twitch.tv/kraken/" + path + "&oauth_token=" + bot.oauthPassword + "&limit=" + limit
                + "&offset=" + offset);
        }
    }
}
