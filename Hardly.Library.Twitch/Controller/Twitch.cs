using System;

namespace Hardly.Library.Twitch {
    public class Twitch {
        Throttle liveFollowersThrottle = new Throttle(TimeSpan.FromMinutes(5)),
            newFollowersThrottle = new Throttle(TimeSpan.FromMinutes(1)),
            refreshAllFollowersThrottle = new Throttle(TimeSpan.FromMinutes(5));
        ITwitchFactory factory;
        TwitchApi twitchApi;

        public Twitch(ITwitchFactory factory) {
            this.factory = factory;
            twitchApi = new TwitchApi(factory);
        }

        public TwitchUserInChannel[] GetNewFollowers(TwitchAlert alerts) {
            try {
                if(newFollowersThrottle.ExecuteIfReady(alerts.connection.channel.user.id)) {
                    twitchApi.UpdateNewFollowers(alerts.connection, 25, 0);
                }
                return factory.GetNewFollowers(alerts);
            } catch(Exception e) {
                Log.error("Twitch get new followers", e);
                return null;
            }
        }

        public TwitchChannel[] GetLiveFollowers(TwitchConnection connection) {
            try {
                if(liveFollowersThrottle.ExecuteIfReady(connection.channel.user.id)) {
                    twitchApi.UpdateLiveFollowers(connection, 25, 0);
                }
                return factory.GetAllLiveFollowers(connection.channel);
            } catch(Exception e) {
                Log.error("Twitch live followers", e);
                return null;
            }
        }

        public void RefreshAllFollowers(TwitchConnection connection) {
            try {
                if(refreshAllFollowersThrottle.ExecuteIfReady(connection.channel.user.id)) {
                    factory.ClearFollowingBitForAllUsers(connection.channel);
                    twitchApi.UpdateNewFollowers(connection, 100, 0, true);
                }
            } catch(Exception e) {
                Log.exception(e);
            }
        }
    }
}
