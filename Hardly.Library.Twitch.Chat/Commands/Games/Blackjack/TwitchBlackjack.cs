using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class TwitchBlackjack : TwitchGameStateMachine<Blackjack<SqlTwitchUser>> {
		public TwitchBlackjack(TwitchChatRoom room) : base(room) {
			ChatCommand.Create(room, "aboutbj", AboutBJ, "How to play Blackjack", new[] { "aboutblackjack", "howtoplaybj", "howtoplayblackjack" }, false, TimeSpan.FromMinutes(2), false);
			ChatCommand.Create(room, "cancelbj", CancelBJ, "Cancels/ends any current games.  No money lost.", null, true, TimeSpan.FromSeconds(0), false);

			SetState(null, typeof(BJStateOff));
		}

		private void CancelBJ(SqlTwitchUser user, string additionalText) {
			SetState(null, typeof(BJStateOff));
		}

		void AboutBJ(SqlTwitchUser user, string additionalText) {
			room.SendChatMessage("Watch for instructions in chat.  If you don’t see the bot’s whispers, follow " 
                + room.twitchConnection.bot.user + ". If twitch says too soon, add text to the end of the command like !hit again.");
		}
	}
}
