using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardly.Library.Hearthstone;

namespace Hardly.Library.Twitch {
    public class SqlTwitchFactory : ITwitchFactory {
        TwitchApi twitchApi;

        public SqlTwitchFactory() {
            twitchApi = new TwitchApi(this);
        }

        public TwitchAlert CreateSqlTwitchAlert(SqlTwitchConnection connection, string alertGuid, DateTime lastFollowerNotification = default(DateTime)) {
            return new SqlTwitchAlert(connection, alertGuid, lastFollowerNotification);
        }

        public TwitchAlert FromGuid(string value) {
            return SqlTwitchAlert.FromGuid(value);
        }

        public TwitchUser GetUserFromName(string username) {
            try {
                var user = SqlTwitchUser.FromName(username);
                if(user != null) {
                    return user;
                } else {
                    return twitchApi.GetUser(username);
                }
            } catch(Exception e) {
                Log.error("Twitch get user from name", e);
                return null;
            }
        }

        public TwitchAlert GetAlert(TwitchConnection connection, string alertGuid, DateTime lastFollowerNotification = default(DateTime)) {
            return new SqlTwitchAlert(connection, alertGuid, lastFollowerNotification);
        }

        public TwitchBot[] GetAllBots() {
            return SqlTwitchBot.GetAll();
        }

        public TwitchUser GetUserForID(uint id) {
            return new SqlTwitchUser(id);
        }

        public TwitchUser GetUser(uint id, string name, DateTime created, string logo, string bio) {
            return new SqlTwitchUser(id, name, created, logo, bio);
        }

        public TwitchChannel CreateTwitchChannel(TwitchUser user) {
            return new SqlTwitchChannel(user);
        }

        public TwitchChannel CreateTwitchChannel(TwitchUser user, bool isLive, string previewImageUrl, string game, uint liveViewers, uint totalViews, uint followers) {
            return new SqlTwitchChannel(user, isLive, previewImageUrl, game, liveViewers, totalViews, followers);
        }

        public TwitchChannelPointScale[] GetChannelPointScale(TwitchChannel channel) {
            return SqlTwitchChannelPointScale.ForChannel(channel);
        }

        public TwitchChannel[] GetAllLiveFollowers(TwitchChannel channel) {
            return SqlTwitchChannel.GetAllLiveFollowers(channel);
        }

        public void ClearFollowingBitForAllUsers(TwitchChannel channel) {
            SqlTwitchChannel.ClearLiveFollowers(channel);
        }

        public TwitchUserInChannel[] GetAllUserInChannels(TwitchChannel channel) {
            return SqlTwitchUserInChannel.GetAllWithUsernames(channel);
        }

        public TwitchUserInChannel[] GetNewFollowers(TwitchAlert alerts) {
            return SqlTwitchUserInChannel.GetNew(alerts);
        }

        public TwitchUserInChannel GetUserInChannel(TwitchUser user, TwitchChannel channel) {
            return new SqlTwitchUserInChannel(user, channel);
        }

        public TwitchUserPointManager GetUserPointManager(TwitchChannel channel, TwitchUser user) {
            return new TwitchUserPointManager(this, channel, user);
        }

        public TwitchUserPoints GetUserPoints(TwitchUser user, TwitchChannel channel) {
            return new SqlTwitchUserPoints(user, channel);
        }

        public TwitchUserPoints[] GetTopUsers(TwitchChannel channel, uint count) {
            return SqlTwitchUserPoints.GetTopUsersForChannel(channel, count);
        }

        public TwitchCommand[] GetAllCommands(TwitchConnection twitchConnection) {
            return SqlTwitchCommand.GetAll(twitchConnection);
        }

        public TwitchConnection[] GetAllAutoConnectingConnections(TwitchBot bot) {
            return SqlTwitchConnection.GetAllAutoConnectingConnections(bot);
        }

        public TwitchChannel GetChannel(TwitchUser twitchUser) {
            return new SqlTwitchChannel(twitchUser);
        }

        public IHearthstoneFactory CreateHearthstoneFactory() {
            return new SqlHearthstoneFactory();
        }
    }
}
