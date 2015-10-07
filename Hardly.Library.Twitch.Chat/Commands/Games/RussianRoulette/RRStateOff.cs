using System;

namespace Hardly.Library.Twitch {
    public class RRStateOff : GameState<TwitchRussianRoulette> {
        public RRStateOff(TwitchRussianRoulette controller) : base(controller) {
            AddCommand(controller.room, "russianroulette", StartGame, "Starts a game of Russian Roulette", null, false);
        }

        protected override void OpenState() {
        }

        private void StartGame(TwitchUser speaker, string additionalText) {
            controller.game.Reset();

            ulong amount = controller.room.pointManager.GetPointsFromString(additionalText);
            if(amount > 0) {
                controller.game.bet = amount;

                controller.SetState(this, typeof(RRStateAcceptingPlayers));
            }
        }
    }
}
