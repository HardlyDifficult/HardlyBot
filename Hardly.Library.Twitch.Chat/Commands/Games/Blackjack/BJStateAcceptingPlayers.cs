using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class BJStateAcceptingPlayers : GameStateAcceptingPlayers<TwitchBlackjack> {
		public BJStateAcceptingPlayers(TwitchBlackjack controller) : base(controller) {
            AddCommand(controller.room, "play", PlayCommand, "Joins a game of Blackjack, follow with your bet --- e.g. !play 10.", new[] { "join" }, false, null, false);
			AddCommand(controller.room, "start", StartCommand, "Starts a game of Blackjack if there is at least one player", null, true, null, false);
			AddCommand(controller.room, "cancelplay", CancelPlayCommand, "Cancels a play, if it's not too late.", new[] { "leave" }, false, null, false);
        }

		private void CancelPlayCommand(SqlTwitchUser speaker, string additionalText) {
			var player = controller.game.Get(speaker);
			if(player != null) {
                controller.game.LeaveGame(speaker);   
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
				controller.SetState(this.GetType(), typeof(BJStatePlay));
			}
		}

		void PlayCommand(SqlTwitchUser speaker, string betMessage) {
			ulong bet = controller.room.pointManager.GetPointsFromString(betMessage);
			TwitchUserPointManager userPoints = controller.room.pointManager.ForUser(speaker);
            
			if(bet > 0) {
				bet = controller.game.Join(speaker, userPoints, bet);
				MinHit_StartWaitingForAdditionalPlayers();
				SendJoinMessage(speaker, bet);
				StartIfReady();
			} else {
				controller.room.SendWhisper(speaker, "You flat broke, come back later.");
			}
		}

		private void StartIfReady() {
			if(controller.game.IsFull()) {
				controller.SetState(this.GetType(), typeof(BJStatePlay));
			}
		}
        
		void SendJoinMessage(SqlTwitchUser speaker, ulong bet = 0) {
			string chatMessage = "You're in";
			if(bet > 0) {
				chatMessage += " for ";
				chatMessage += controller.room.pointManager.ToPointsString(bet);
			}
			chatMessage += ", sit tight we start ";
			chatMessage += GetStartingInMessage();
			controller.room.SendWhisper(speaker, chatMessage);
		}

		string GetStartingInMessage() {
			TimeSpan timeRemaining = roundTimer.TimeRemaining();
			string chatMessage;
			int numberOfOpenSpots = controller.game.NumberOfOpenSpots();
			if(numberOfOpenSpots > 0 && timeRemaining > TimeSpan.FromSeconds(5)) {
				chatMessage = "in " + timeRemaining.ToSimpleString();

				chatMessage += " or when " + numberOfOpenSpots + " more people !play.";
			} else {
				chatMessage = "sooon.";
			}

			return chatMessage;
		}

		protected override void AnnounceGame() {
            base.AnnounceGame();

			controller.room.SendChatMessage("Blackjack: !play to join in.");
		}

		internal override void TimeUp() {
			controller.room.SendChatMessage("Blackjack: !play to join in, we start " + GetStartingInMessage());
		}

		internal override void FinalTimeUp() {
			controller.SetState(this.GetType(), typeof(BJStatePlay));
		}
	}
}