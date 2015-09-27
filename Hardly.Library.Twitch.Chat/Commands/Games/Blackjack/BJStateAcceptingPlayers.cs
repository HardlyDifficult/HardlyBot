using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class BJStateAcceptingPlayers : GameStateAcceptingPlayers<TwitchBlackjack> {
		public BJStateAcceptingPlayers(TwitchBlackjack controller) : base(controller) {
			AddCommand(controller.room, "play", PlayCommand, "Joins a game of Blackjack, follow with your bet --- e.g. !play 10.", null, false, TimeSpan.FromSeconds(0), false);
			AddCommand(controller.room, "start", StartCommand, "Starts a game of Blackjack if there is at least one player", null, true, TimeSpan.FromSeconds(0), false);
			AddCommand(controller.room, "cancelplay", CancelPlayCommand, "Cancels a play, if it's not too late.", null, false, TimeSpan.FromSeconds(0), false);
		}

		private void CancelPlayCommand(SqlTwitchUser speaker, string additionalText) {
			BlackjackPlayer player;
			if(controller.game.players.TryGetValue(speaker, out player)) {
				UserPointManager userPoints = controller.pointManager.ForUser(speaker);
				userPoints.Award(player.totalBet, 0);
				controller.room.SendWhisper(speaker, "You're out, later dude.");
				if(controller.game.IsEmpty()) {
					StopTimers();
				}
			} else {
				controller.room.SendWhisper(speaker, "You weren't in the game, soooo...");
			}
		}

		void StartCommand(SqlTwitchUser speaker, string additionalText) {
			if(!controller.game.IsEmpty()) {
				controller.SetState(this, typeof(BJStatePlay));
			}
		}

		void PlayCommand(SqlTwitchUser speaker, string betMessage) {
			ulong bet = controller.pointManager.GetPointsFromString(betMessage);
			UserPointManager userPoints = controller.pointManager.ForUser(speaker);

			bet = userPoints.ReserveBet(bet);
			if(bet > 0) {
				controller.game.Join(speaker, bet);
				MinHit_StartWaitingForAdditionalPlayers();
				SendJoinMessage(speaker, bet);
				StartIfReady();
			} else {
				controller.room.SendWhisper(speaker, "You flat broke, come back later.");
			}
		}

		private void StartIfReady() {
			if(controller.game.IsFull()) {
				controller.SetState(this, typeof(BJStatePlay));
			}
		}

		internal override void Open() {
			base.Open();
			AnnounceGame();
		}

		void SendJoinMessage(SqlTwitchUser speaker, ulong bet = 0) {
			string chatMessage = "You're in";
			if(bet > 0) {
				chatMessage += " for ";
				chatMessage += controller.pointManager.ToPointsString(bet);
			}
			chatMessage += ", sit tight we start ";
			chatMessage += GetStartingInMessage();
			controller.room.SendWhisper(speaker, chatMessage);
		}

		string GetStartingInMessage() {
			TimeSpan timeRemaining = roundTimer.TimeRemaining();
			string chatMessage;
			int numberOfOpenSpots = (int)(controller.game.maxPlayers - controller.game.players.Count);
			if(numberOfOpenSpots > 0 && timeRemaining > TimeSpan.FromSeconds(5)) {
				chatMessage = "in " + timeRemaining.ToSimpleString();

				chatMessage += " or when " + numberOfOpenSpots + " more people !play.";
			} else {
				chatMessage = "sooon.";
			}

			return chatMessage;
		}

		internal override void AnnounceGame() {
			controller.room.SendChatMessage("Blackjack: !play to join in.");
			StartWaitingForSomeoneToJoin();
		}

		internal override void TimeUp() {
			controller.room.SendChatMessage("Blackjack: !play to join in, we start " + GetStartingInMessage());
		}

		internal override void FinalTimeUp() {
			controller.SetState(this, typeof(BJStatePlay));
		}
	}
}