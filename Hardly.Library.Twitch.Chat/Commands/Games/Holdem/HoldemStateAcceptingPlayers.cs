using Hardly.Games;
using System;

namespace Hardly.Library.Twitch {
	internal class HoldemStateAcceptingPlayers : GameStateAcceptingPlayers<TwitchHoldem> {
		public HoldemStateAcceptingPlayers(TwitchHoldem controller) : base(controller) {
			AddCommand(controller.room, "playholdem", PlayCommand, "Joins a game of Holdem.", null, false, TimeSpan.FromSeconds(0), false);
			AddCommand(controller.room, "startholdem", StartCommand, "Starts a game of Holdem if there is at least two players", null, true, TimeSpan.FromSeconds(0), false);
			AddCommand(controller.room, "cancelplayholdem", CancelPlayCommand, "Cancels a play, if it's not too late.", null, false, TimeSpan.FromSeconds(0), false);
		}

		private void CancelPlayCommand(SqlTwitchUser speaker, string additionalText) {
			TexasHoldemPlayer player;
			if(controller.game.players.TryGetValue(speaker, out player)) {
				UserPointManager userPoints = controller.pointManager.ForUser(speaker);
				userPoints.Award(player.totalBet, 0);
				controller.room.SendWhisper(speaker, "You're out, later dude.");
				if(controller.game.players.Count <= 1) {
					StopTimers();
				}
			} else {
				controller.room.SendWhisper(speaker, "You weren't in the game, soooo...");
			}
		}

		private void StartCommand(SqlTwitchUser speaker, string additionalText) {
			if(controller.game.players.Count > 1) {
				controller.SetState(this.GetType(), typeof(HoldemStatePlayPreFlop));
			}
		}

		private void PlayCommand(SqlTwitchUser speaker, string additionalText) {
			ulong bet = 10; // Ante
			UserPointManager userPoints = controller.pointManager.ForUser(speaker);

			bet = userPoints.ReserveBet(bet);
			if(bet > 0) {
				controller.game.Join(speaker, bet);
				if(controller.game.players.Count > 1) {
					MinHit_StartWaitingForAdditionalPlayers();
				}
				SendJoinMessage(speaker, bet);
				StartIfReady();
			} else {
				controller.room.SendWhisper(speaker, "You flat broke, come back later.");
			}
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

		private void StartIfReady() {
			if(controller.game.IsFull()) {
				controller.SetState(this.GetType(), typeof(HoldemStatePlayPreFlop));
			}
		}

		internal override void AnnounceGame() {
			controller.room.SendChatMessage("Holdem: !playholdem to join in.");
			StartWaitingForSomeoneToJoin();
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

		internal override void FinalTimeUp() {
			controller.SetState(this.GetType(), typeof(HoldemStatePlayPreFlop));
		}

		internal override void TimeUp() {
			controller.room.SendChatMessage("Holdem: !playholdem to join in, we start " + GetStartingInMessage());
		}

		internal override void Open() {
			base.Open();
			AnnounceGame();
		}
	}
}