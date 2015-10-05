namespace Hardly.Library.Twitch {
    public class RRStatePlaying : GameState<TwitchRussianRoulette> {
        Timer turnTimer;
        SqlTwitchUser lastPlayer = null;

        public RRStatePlaying(TwitchRussianRoulette controller) : base(controller) {
            AddCommand(controller.room, "pullthetrigger", PullTrigger, "Points the gun at your head and pulls the trigger.  Good luck fool.", new[] { "pulltrigger" }, false);
            AddCommand(controller.room, "pussyout", PussyOut, "Points the gun at your head and pulls the trigger.  Good luck fool.", new[] { "pulltrigger" }, false);
        }

        protected override void OpenState() {
            turnTimer = new Timer(TimeSpan.FromSeconds(30), TimeUp);
            controller.game.StartGame();
            string chatMessage = "Russian Roulette has begun - ";
            GunMessage(chatMessage);
        }

        private void TimeUp() {
            PussyOut(controller.game.currentPlayer.idObject, null);
        }

        protected override void CloseState() {
            base.CloseState();

            turnTimer.Stop();
        }

        private void PussyOut(SqlTwitchUser speaker, string arg2) {
            if(CheckPlayer(speaker)) {
                if(controller.game.PussyOut()) {
                    string chatMessage = speaker.name;
                    chatMessage += " pussied out... ";
                    GunMessage(chatMessage);
                }
            }
        }

        private bool CheckPlayer(SqlTwitchUser speaker) {
            if(speaker.id.Equals(controller.game.currentPlayer?.idObject.id)) {
                return true;
            } else {
                controller.room.SendWhisper(speaker, "Not your turn");
                // TODO - could be never joined or already pussied out.

                return false;
            }
        }

        private void PullTrigger(SqlTwitchUser speaker, string arg2) {
            if(CheckPlayer(speaker)) {
                var player = controller.game.GetPlayer(speaker);
                controller.game.PullTrigger();
                if(player.isWinner.GetValueOrDefault(true) == false) {
                    controller.room.Timeout(speaker, TimeSpan.FromMinutes(5));
                    string chatMessage = speaker.name;
                    chatMessage += " died... ";
                    GunMessage(chatMessage);
                } else {
                    string chatMessage = speaker.name;
                    chatMessage += " lives to fight another day. ";
                    GunMessage(chatMessage);
                }
            }
        }

    
        private void GunMessage(string chatMessage) {
            if(!controller.game.gameOver) {
                if(lastPlayer == null || !lastPlayer.id.Equals(controller.game.currentPlayer?.idObject.id)) {
                    lastPlayer = controller.game.currentPlayer?.idObject;
                    turnTimer.Start();
                }

                bool first = true;
                foreach(var player in controller.game.GetPlayers()) {
                    if(!first) {
                        chatMessage += ", ";
                    }
                    first = false;
                    chatMessage += player.idObject.name;
                }

                chatMessage += " are playing.. ";

                if(!controller.game.gunHasBeenFired) {
                    chatMessage += "Gun's loaded and handed to ";
                } else {
                    chatMessage += "Handed gun to ";
                }

                chatMessage += controller.game.currentPlayer.idObject.name;

                chatMessage += " !pullthetrigger or !pussyout?";

            } else {
                chatMessage += "Game's over - ";
                chatMessage += controller.game.GetPlayers().First.idObject.name;
                chatMessage += " won!";

                controller.SetState(this, typeof(RRStateOff));
            }

            controller.room.SendChatMessage(chatMessage);
        }
    }
}