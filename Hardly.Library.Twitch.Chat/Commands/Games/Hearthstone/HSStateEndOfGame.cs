using System;

namespace Hardly.Library.Twitch {
    public class HSStateEndOfGame : GameState<TwitchHearthstone> {
        public HSStateEndOfGame(TwitchHearthstone controller) : base(controller) {
            new Timer(TimeSpan.FromSeconds(20), AnnounceEnd).Start();
        }

        protected override void OpenState() {
        }

        private void AnnounceEnd() {
            string chatMessage = "Hearthstone Game Over - " + (controller.lastGameEnding.iWon == null ? "ended in a draw"
                : controller.lastGameEnding.iWon.Value ? "we won!" : controller.hearthstoneGame.opponentPlayerName + " won...");

            controller.game.SetWinner(controller.lastGameEnding.iWon);
            controller.game.EndGame();

            controller.room.SendChatMessage(chatMessage);
        }
    }
}
