using System;

namespace Hardly.Library.Twitch {
	class PointCommands : TwitchCommandController {
		public PointCommands(TwitchChatRoom room) : base(room) {
            ChatCommand.Create(room, "points", PointCommand, "View how many points you, or another user, has. !points <username>", new[] { "point", "kappas" }, false, TimeSpan.FromSeconds(30), true);
			ChatCommand.Create(room, "brag", BragCommand, "Shows everyone how many points you have.  This costs 50 to run.", null, false, TimeSpan.FromMinutes(1), true);
			ChatCommand.Create(room, "leaderboard", LeaderboardCommand, "Displays the peeps with the most points.", null, false, TimeSpan.FromMinutes(2), false);
            ChatCommand.Create(room, "aboutpoints", AboutPointsCommand, "Displays the point units and values.", null, false, TimeSpan.FromMinutes(2), true);
            ChatCommand.Create(room, "givepoints", GivePointsCommand, "Gives someone points from your account.", null, false, TimeSpan.FromMinutes(2), true);
            ChatCommand.Create(room, "awardpoints", AwardPointsCommand, "Gives someone points from the house.", null, true, TimeSpan.FromMinutes(2), true);
        }

        private void AwardPointsCommand(SqlTwitchUser speaker, string additionalText) {
            if(additionalText != null) {
                var otherUser = SqlTwitchUser.GetFromName(additionalText.GetBefore(" "));
                if(otherUser != null) {
                    var amount = room.pointManager.GetPointsFromString(additionalText.GetAfter(" "));

                    if(amount > 0) {
                        var otherPoints = room.pointManager.ForUser(otherUser);
                        otherPoints.Award(0, (long)amount);
                    } else {
                        room.SendWhisper(speaker, "How much?");
                    }
                } else {
                    room.SendWhisper(speaker, "Who are you giving points to?");
                }
            } else {
                room.SendWhisper(speaker, "Who and how much?");
            }
        }

        private void GivePointsCommand(SqlTwitchUser speaker, string additionalText) {
            if(additionalText != null) {
                var otherUser = SqlTwitchUser.GetFromName(additionalText.GetBefore(" "));
                if(otherUser != null) {
                    var amount = room.pointManager.GetPointsFromString(additionalText.GetAfter(" "));

                    if(amount > 0) {
                        var myPoints = room.pointManager.ForUser(speaker);
                        var otherPoints = room.pointManager.ForUser(otherUser);
                        if(myPoints.ReserveBet(amount, true) > 0) {
                            otherPoints.Award(0, (long)amount);
                            myPoints.Award(amount, -1 * (long)amount);
                        }
                    } else {
                        room.SendWhisper(speaker, "How much?");
                    }
                } else {
                    room.SendWhisper(speaker, "Who are you giving points to?");
                }
            } else {
                room.SendWhisper(speaker, "Who and how much?");
            }
        }

        private void AboutPointsCommand(SqlTwitchUser speaker, string message) {
			room.SendWhisper(speaker, room.pointManager.GetAboutPoints());
		}

		private void LeaderboardCommand(SqlTwitchUser speaker, string message) {
			SqlTwitchUserPoints[] leadersPoints = SqlTwitchUserPoints.GetTopUsersForChannel(room.twitchConnection.channel, 5);
			if(leadersPoints != null) {
				string chatMessage = "";
				foreach(var points in leadersPoints) {
					chatMessage += points.user.name + " has " + room.pointManager.ToPointsString(points.points, true) + "  ";
				}

				room.SendChatMessage(chatMessage);
			}
		}

		private void BragCommand(SqlTwitchUser speaker, string message) {
			TwitchUserPointManager yourPoints = room.pointManager.ForUser(speaker);
			if(yourPoints.Points >= 50) {
				yourPoints.Award(0, -50);
				string chatMessage = speaker.name + " has " + room.pointManager.ToPointsString(yourPoints.Points);
				room.SendChatMessage(chatMessage);
			} else {
				room.SendWhisper(speaker, "You can't afford a brag...");
			}
		}

		private void PointCommand(SqlTwitchUser speaker, string message) {
			string otherUserName = message.GetBefore(" ");
			if(otherUserName == null) {
				otherUserName = message;
			}
			otherUserName = otherUserName?.Trim().ToLower();
			SqlTwitchUser otherUser = SqlTwitchUser.GetFromName(otherUserName);

			TwitchUserPointManager yourPoints = room.pointManager.ForUser(speaker);
			string chatMessage = "You have ";
			chatMessage += room.pointManager.ToPointsString(yourPoints.Points);

			if(otherUser != null && !otherUser.id.Equals(speaker.id)) {
				TwitchUserPointManager otherPoints = room.pointManager.ForUser(otherUser);

				chatMessage += " & " + otherUser.name + " has " + room.pointManager.ToPointsString(otherPoints.Points);
			}

			room.SendWhisper(speaker, chatMessage);
		}
	}
}
