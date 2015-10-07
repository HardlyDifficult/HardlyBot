using System;

namespace Hardly.Library.Twitch {
    public interface ITwitchFactory {
        // Alerts
        TwitchAlert GetAlert(TwitchConnection connection,
            string alertGuid, DateTime lastFollowerNotification = default(DateTime));
        TwitchCommand[] GetAllCommands(TwitchConnection twitchConnection);

        // Bots
        TwitchBot[] GetAllBots();

        // Users
        TwitchUser GetUserForID(uint id);
        TwitchConnection[] GetAllAutoConnectingConnections(TwitchBot bot);
        TwitchUser GetUserFromName(string name);
        TwitchUser GetUser(uint id, string name, DateTime created, string logo, string bio);
       
        // Channels
        TwitchChannel CreateTwitchChannel(TwitchUser user);
        TwitchChannel CreateTwitchChannel(TwitchUser user, bool isLive, string previewImageUrl, string game, uint liveViewers, uint totalViews, uint followers);
        TwitchChannelPointScale[] GetChannelPointScale(TwitchChannel channel);

        // Followers
        TwitchChannel[] GetAllLiveFollowers(TwitchChannel channel);
        void ClearFollowingBitForAllUsers(TwitchChannel channel);

        // Users in channel
        TwitchUserInChannel[] GetAllUserInChannels(TwitchChannel channel);
        TwitchUserInChannel[] GetNewFollowers(TwitchAlert alerts);
        TwitchUserInChannel GetUserInChannel(TwitchUser user, TwitchChannel channel);

        // Points
        TwitchUserPointManager GetUserPointManager(TwitchChannel channel, TwitchUser user);
        TwitchUserPoints GetUserPoints(TwitchUser user, TwitchChannel channel);
        TwitchChannel GetChannel(TwitchUser twitchUser);
        TwitchUserPoints[] GetTopUsers(TwitchChannel channel, uint count);
    }
}