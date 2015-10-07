using Hardly.Games;
using System;
using System.Collections.Generic;


namespace Hardly.Library.Twitch {
	class HoldemStateEndOfGame : GameState<TwitchHoldem> {
        int iPlayer;
        Timer timer;
        PokerPlayerHandEvaluator lastHand = null;

		public HoldemStateEndOfGame(TwitchHoldem controller) : base(controller) {
            timer = new Timer(TimeSpan.FromSeconds(5), TimeUp);
            iPlayer = 0;
        }

        protected override void OpenState() {
            if(controller.game.lastGameEndedInSidepotPlayers.Count + controller.game.lastGameEndedInSeatPlayers.Count > 1) {
                string chatMessage = PlayerHand();
                chatMessage += "...";
                controller.room.SendChatMessage(chatMessage);
                timer.Start();
            } else {
                string chatMessage = "Holdem: ";
                TexasHoldemPlayer<TwitchUser> player;
                if(iPlayer > controller.game.lastGameEndedInSidepotPlayers.Count - 1) {
                    player = controller.game.lastGameEndedInSeatPlayers[iPlayer - controller.game.lastGameEndedInSidepotPlayers.Count];
                } else {
                    player = controller.game.lastGameEndedInSidepotPlayers[iPlayer];
                }
                chatMessage += player.idObject.name;
                chatMessage += " won ";
                chatMessage += controller.room.pointManager.ToPointsString(controller.game.lastGameWinners.First.Item2);
                controller.room.SendChatMessage(chatMessage);
                // TODO option to show.
                controller.SetState(this, typeof(HoldemStateOff));
            }
        }

        private void TimeUp() {
            iPlayer++;

            string chatMessage = PlayerHand();
            if(controller.game.lastGameEndedInSidepotPlayers.Count + controller.game.lastGameEndedInSeatPlayers.Count > iPlayer + 1) {
                chatMessage += "...";
                controller.room.SendChatMessage(chatMessage);
                timer.Start();
            } else {
                chatMessage += ". Winners: ";

                Dictionary<TexasHoldemPlayer<TwitchUser>, ulong> winners = new Dictionary<TexasHoldemPlayer<TwitchUser>, ulong>();
                AddWinnings(controller.game.lastGameSidepotWinners, ref winners);
                AddWinnings(controller.game.lastGameWinners, ref winners);

                foreach(var winner in winners) {
                    chatMessage += winner.Key.idObject.name;
                    chatMessage += " won ";
                    chatMessage += controller.room.pointManager.ToPointsString(winner.Value);
                }
                
                controller.room.SendChatMessage(chatMessage);
                controller.SetState(this, typeof(HoldemStateOff));
            }
        }

        private static void AddWinnings(List<Tuple<TexasHoldemPlayer<TwitchUser>, ulong>> winnersList, ref Dictionary<TexasHoldemPlayer<TwitchUser>, ulong> winners) {
            for(int i = 0; i < winnersList.Count; i++) {
                var player = winnersList[i];
                ulong winnings;
                if(winners.TryGetValue(player.Item1, out winnings)) {
                    winners.Remove(player.Item1);
                    winners.Add(player.Item1, winnings + player.Item2);
                } else {
                    winners.Add(player.Item1, player.Item2);
                }
            }
        }
        
        private string PlayerHand() {
            string chatMessage = "Holdem";
            TexasHoldemPlayer<TwitchUser> player;
            if(iPlayer > controller.game.lastGameEndedInSidepotPlayers.Count - 1) {
                chatMessage += ": ";
                player = controller.game.lastGameEndedInSeatPlayers[iPlayer - controller.game.lastGameEndedInSidepotPlayers.Count];
            } else {
                chatMessage += " sidepot: ";
                player = controller.game.lastGameEndedInSidepotPlayers[iPlayer];
            }
            chatMessage += player.idObject.name;


            if(lastHand == null || player.bestHand.handValue > lastHand.handValue) {
                lastHand = player.bestHand;

                chatMessage += " has ";
                chatMessage += player.hand.ToString();
                if(player.bestHand != null) {
                    chatMessage += " for ";
                    chatMessage += player.bestHand.handType.ToString();
                    chatMessage += " (";
                    chatMessage += player.bestHand.cards.ToString();
                    chatMessage += ")";
                }
            } else {
                chatMessage += " mucks";
                // TODO option to show.
            }

            return chatMessage;
        }
    }
}