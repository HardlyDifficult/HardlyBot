using System;
using Hardly.Games.Betting;

namespace Hardly.Library.Twitch {
    public class RRStateAcceptingPlayers : GameStateAcceptingPlayers<TwitchRussianRoulette> {
        public RRStateAcceptingPlayers(TwitchRussianRoulette controller) : base(controller) {
            AddCommand(controller.room, "joinroulette", JoinGame, "Join the Russian Roulette game", null, false);
        }

        private void JoinGame(TwitchUser speaker, string additionalText) {
            var pointManager = controller.room.pointManager.ForUser(speaker);
            var player = new RussianRoulettePlayer<TwitchUser>(pointManager, speaker);
            if(controller.game.Join(player)) {
                controller.room.SendWhisper(speaker, "You're in");

                if(controller.game.isFull) {
                    StartGame();
                } else if(controller.game.isReadyToStart) {
                    MinHit_StartWaitingForAdditionalPlayers();
                }
            } else {
                if(controller.game.Contains(speaker)) {
                    controller.room.SendWhisper(speaker, "You're already in.");
                } else {
                    controller.room.SendWhisper(speaker, "You can't afford to play.");
                }
            }
        }

        private void StartGame() {
            controller.SetState(this, typeof(RRStatePlaying));
        }

        protected override void AnnounceGame() {
            base.AnnounceGame();

            controller.room.SendChatMessage("Russian Roulette !joinroulette to play (costs " + controller.room.pointManager.ToPointsString(controller.game.bet) + " to play)");
        }

        internal override void FinalTimeUp() {
            StartGame();
        }

        internal override void TimeUp() {
            controller.room.SendChatMessage("Russian Roulette starts soon...");
        }
    }
}
