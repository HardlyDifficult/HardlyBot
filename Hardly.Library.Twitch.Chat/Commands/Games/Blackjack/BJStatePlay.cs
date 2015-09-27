using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class BJStatePlay : GameState<TwitchBlackjack> {
		TimerSet timers;

		public BJStatePlay(TwitchBlackjack controller) : base(controller) {
			timers = new TimerSet(
				new TimeSpan[] { TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(5) },
				new Action[] { TimeUp1, TimeUp2, FinalTimeUp });

			AddCommand(controller.room, "hit", HitCommand, "Draw another card.", null, false, TimeSpan.FromSeconds(0), false);
			AddCommand(controller.room, "double", DoubleCommand, "Double your bet and get exactly one more card (then you stand).", null, false, TimeSpan.FromSeconds(0), false);
			AddCommand(controller.room, "split", SplitCommand, "Splits two cards of the same value.  You play each hand separately, and each hand is playing for your opening bet.", null, false, TimeSpan.FromSeconds(0), false);
			AddCommand(controller.room, "stand", StandCommand, "Ends your turn.", null, false, TimeSpan.FromSeconds(0), false);
		}

		private void StandCommand(SqlTwitchUser speaker, string message) {
			BlackjackPlayer player = GetPlayer(speaker);

			if(player != null) {
				if(!player.CurrentHand.standing && !player.CurrentHand.IsBust()) {
					player.CurrentHand.standing = true;
					string chatMessage = "Standing with " + player.CurrentHand.hand.ToChatString() + " (" + player.CurrentHand.GetValueString() + ").";
					chatMessage += AnnounceSplitHand(player);
					controller.room.SendWhisper(speaker, chatMessage);
					if(ReadyToEnd()) {
						controller.SetState(this.GetType(), typeof(BJStateDealerPlaying));
					}
				}
			} else {
				controller.room.SendWhisper(speaker, "Sorry, join the next game.");
			}
		}

		private void SplitCommand(SqlTwitchUser speaker, string message) {
			BlackjackPlayer player = GetPlayer(speaker);
			UserPointManager userPoints = controller.pointManager.ForUser(speaker);
			if(player.Split(controller.game, userPoints.ReserveBet(player.CurrentHand.bet, true) > 0)) {
				if(player.CurrentHand.standing) {
					controller.room.SendWhisper(speaker, "Hand split & standing with " + player.GetValueString());
				} else {
					controller.room.SendWhisper(speaker, "Hand split, in your first hand... " + player.CurrentHand.hand.ToChatString());
				}
			} else {
				controller.room.SendWhisper(speaker, "You can't split that hand.");
			}
		}

		private void DoubleCommand(SqlTwitchUser speaker, string message) {
			Hit(speaker, message, true);
		}

		private void HitCommand(SqlTwitchUser speaker, string message) {
			Hit(speaker, message);
		}

		string AnnounceSplitHand(BlackjackPlayer player) {
			if(player.ReadyToSwitchHands()) {
				string chatMessage = "";
				if(!player.CurrentHand.standing) {
					chatMessage += " Next hand: " + player.CurrentHand.hand.ToChatString();
				}

				return chatMessage;
			}

			return "";
		}

		void Hit(SqlTwitchUser speaker, string message, bool doubleDown = false) {
			BlackjackPlayer player = GetPlayer(speaker);
			if(player != null) {
				string chatMessage = "";
            if(!player.CurrentHand.standing && !player.CurrentHand.IsBust()) {
					if(doubleDown) {
						UserPointManager userPoints = controller.pointManager.ForUser(speaker);
						if(userPoints.ReserveBet(player.CurrentHand.bet, true) > 0) {
							player.CurrentHand.bet *= 2;
							chatMessage += "Doubled down! ";
							player.CurrentHand.standing = true;
						} else {
							chatMessage += "Can't afford a double, so you hit instead. ";
							doubleDown = false;
						}
					}

					PlayingCard card = controller.game.deck.TakeTopCard();
					player.CurrentHand.hand.GiveCard(card);

					chatMessage += "Dealt a " + card.ToChatString();

					if(player.CurrentHand.IsBust()) {
						chatMessage += " and BUSTED with " + player.CurrentHand.hand.ToChatString() + " (" + player.CurrentHand.GetValueString() + ")!";
					} else {
						chatMessage += ", you have " + player.CurrentHand.hand.ToChatString() + ".";
					}
					
					chatMessage += AnnounceSplitHand(player);
					controller.room.SendWhisper(speaker, chatMessage);

					if(ReadyToEnd()) {
						controller.SetState(this.GetType(), typeof(BJStateDealerPlaying));
					}
				} else {
					controller.room.SendWhisper(speaker, "Too late, you are standing with " + player.CurrentHand.hand.ToChatString());
				}
			} else {
				controller.room.SendWhisper(speaker, "Sorry, join the next game.");
			}
		}

		internal override void Close() {
			base.Close();
			timers.Stop();
		}

		internal override void Open() {
			base.Open();
         controller.game.Deal();

			string message = "Blackjack: Dealer ";
			message += controller.game.dealer.hand.ViewCard(0).ToChatString();
			message += " \uD83C\uDCA0 ";
			foreach(var player in controller.game.players) {
				message += ", ";
				message += (player.Key as SqlTwitchUser).name;
				message += " ";
				message += player.Value.CurrentHand.hand.ToChatString();
			}

			message += " -- !Hit, !Stand, !Split or !DoubleDown?";

			controller.room.SendChatMessage(message);

			timers.Start();
		}

		void TimeUp1() {
			foreach(var player in controller.game.players) {
				if(!player.Value.CurrentHand.standing && !player.Value.CurrentHand.IsBust()) {
					string chatMessage = "!Hit, !Stand or !DoubleDown?";
					controller.room.SendWhisper(player.Key as SqlTwitchUser, chatMessage);
				}
			}
		}

		void TimeUp2() {
			foreach(var player in controller.game.players) {
				if(!player.Value.CurrentHand.standing && !player.Value.CurrentHand.IsBust()) {
					string chatMessage = "!Hit, !Stand or !DoubleDown?  You have only seconds to respond.";
					controller.room.SendWhisper(player.Key as SqlTwitchUser, chatMessage);
				}
			}
		}

		void FinalTimeUp() {
			controller.SetState(this.GetType(), typeof(BJStateDealerPlaying));
		}

		bool ReadyToEnd() {
			bool allReady = true;

			foreach(var player in controller.game.players) {
				if(!player.Value.CurrentHand.standing && !player.Value.CurrentHand.IsBust()) {
					allReady = false;
					break;
				}
			}

			return allReady;
		}

		BlackjackPlayer GetPlayer(SqlTwitchUser speaker) {
			BlackjackPlayer player;
			if(controller.game.players.TryGetValue(speaker, out player)) {
				return player;
			}

			return null;
		}
	}
}