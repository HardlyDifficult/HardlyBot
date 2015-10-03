using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class TwitchHoldem : TwitchGameStateMachine<TexasHoldem<SqlTwitchUser>> {
		ChatCommand aboutCommand, cancelCommand;

		public TwitchHoldem(TwitchChatRoom room) : base(room, typeof(HoldemStateOff)) {
			aboutCommand = ChatCommand.Create(room, "aboutholdem", AboutHoldem, "How to play Texas Holdem", new[] { "aboutholdem", "howtoplayholdem", "howtoplayholdem" }, false, TimeSpan.FromMinutes(2), false);
            cancelCommand = ChatCommand.Create(room, "cancelholdem", CancelHoldem, "Cancels/ends any current games.  No money lost.", null, true, null, false);
		}

		private void CancelHoldem(SqlTwitchUser user, string additionalText) {
			SetState(null, typeof(HoldemStateOff));
		}

		void AboutHoldem(SqlTwitchUser user, string additionalText) {
			room.SendChatMessage("Watch for instructions in chat.  If you don’t see the bot’s whispers, follow " + room.twitchConnection.bot.user + ". If twitch says too soon, add text to the end of the command like !hit again.");
		}
	}
}
