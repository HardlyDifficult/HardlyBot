using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
	public class BJStatePlay : GameState<TwitchBlackjack> {
		TimerSet timers;

		public BJStatePlay(TwitchBlackjack controller) : base(controller) {
			timers = new TimerSet(
				new TimeSpan[] { TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(5) },
				new Action[] { TimeUp1, TimeUp2, FinalTimeUp });

			AddCommand(controller.room, "hit", HitCommand, "Draw another card.", null, false, null, false);
			AddCommand(controller.room, "double", DoubleCommand, "Double your bet and get exactly one more card (then you stand).", null, false, null, false);
			AddCommand(controller.room, "split", SplitCommand, "Splits two cards of the same value.  You play each hand separately, and each hand is playing for your opening bet.", null, false, null, false);
			AddCommand(controller.room, "stand", StandCommand, "Ends your turn.", null, false, null, false);

            controller.game.StartGame();

            string message = "Blackjack: Dealer ";
            message += controller.game.dealer.cards.First.ToString();
            message += " \uD83C\uDCA0 ";
            foreach(var player in controller.game.PlayerGameObjects) {
                message += ", ";
                message += player.idObject.name;
                message += " ";
                message += player.CurrentHandEvaluator.cards.ToString();
            }

            message += " -- !Hit, !Stand, !Split or !DoubleDown?";

            controller.room.SendChatMessage(message);

            timers.Start();
        }

		private void StandCommand(SqlTwitchUser speaker, string message) {
			var player = GetPlayer(speaker);

			if(player != null) {
				if(player.Stand()) {
					string chatMessage = "Standing with " + player.CurrentHandEvaluator.cards.ToString() + " (" + player.CurrentHandEvaluator.HandValueString() + ").";
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
			var player = GetPlayer(speaker);
			if(player.Split()) {
				if(player.CurrentHandEvaluator.isStanding) {
					controller.room.SendWhisper(speaker, "Hand split & standing with " + player.HandValueString());
				} else {
					controller.room.SendWhisper(speaker, "Hand split, in your first hand... " + player.CurrentHandEvaluator.cards.ToString());
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

		string AnnounceSplitHand(BlackjackPlayer<SqlTwitchUser> player) {
			if(player.ReadyToSwitchHands()) {
				string chatMessage = "";
				if(!player.CurrentHandEvaluator.isStanding) {
					chatMessage += " Next hand: " + player.CurrentHandEvaluator.cards.ToString();
				}

				return chatMessage;
			}

			return "";
		}

		void Hit(SqlTwitchUser speaker, string message, bool doubleDown = false) {
			BlackjackPlayer<SqlTwitchUser> player = GetPlayer(speaker);
			if(player != null) {
				string chatMessage = "";
                if(!player.CurrentHandEvaluator.isDone) {
                    if(doubleDown) {
                        if(player.DoubleDown()) {
                            chatMessage += "Doubled down! ";
                        } else {
                            chatMessage += "Can't afford a double, so you hit instead. ";
                            doubleDown = false;
                            player.Hit();
                        }
                    } else {
                        player.Hit();
                    }

					chatMessage += "Dealt a " + player.hand.cards.Last.ToString();

					if(player.CurrentHandEvaluator.isBust) {
						chatMessage += " and BUSTED with " + player.CurrentHandEvaluator.cards.ToString() + " (" + player.CurrentHandEvaluator.HandValueString() + ")!";
					} else {
						chatMessage += ", you have " + player.CurrentHandEvaluator.cards.ToString() + ".";
					}
					
					chatMessage += AnnounceSplitHand(player);
					controller.room.SendWhisper(speaker, chatMessage);

					if(ReadyToEnd()) {
						controller.SetState(this.GetType(), typeof(BJStateDealerPlaying));
					}
				} else {
					controller.room.SendWhisper(speaker, "Too late, you are standing with " + player.CurrentHandEvaluator.HandValueString());
				}
			} else {
				controller.room.SendWhisper(speaker, "Sorry, join the next game.");
			}
		}

        public override void Dispose() {
            base.Dispose();
			timers.Stop();
		}
        
		void TimeUp1() {
			foreach(var player in controller.game.PlayerGameObjects) {
				if(!player.CurrentHandEvaluator.isDone) {
					string chatMessage = "!Hit, !Stand or !DoubleDown?";
					controller.room.SendWhisper(player.idObject, chatMessage);
				}
			}
		}

		void TimeUp2() {
			foreach(var player in controller.game.PlayerGameObjects) {
				if(!player.CurrentHandEvaluator.isDone) {
					string chatMessage = "!Hit, !Stand or !DoubleDown?  You have only seconds to respond.";
					controller.room.SendWhisper(player.idObject, chatMessage);
				}
			}
		}

		void FinalTimeUp() {
			controller.SetState(this.GetType(), typeof(BJStateDealerPlaying));
		}

		bool ReadyToEnd() {
			bool allReady = true;

			foreach(var player in controller.game.PlayerGameObjects) {
				if(!player.CurrentHandEvaluator.isDone) {
					allReady = false;
					break;
				}
			}

			return allReady;
		}

		BlackjackPlayer<SqlTwitchUser> GetPlayer(SqlTwitchUser speaker) {
			return controller.game.Get(speaker);
		}
	}
}