using System;

namespace Hardly.Library.Twitch {
	public abstract class GameStateAcceptingPlayers<TwitchGameController> : GameState<TwitchGameController> {
		Timer waitingToStartTimer;
		protected TimerSet roundTimer;

		public GameStateAcceptingPlayers(TwitchGameController controller) : base(controller) {
			waitingToStartTimer = new Timer(TimeSpan.FromMinutes(5), AnnounceGame);
			roundTimer = new TimerSet(
				new TimeSpan[] { TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(20) },
				new Action[] { TimeUp, FinalTimeUp });


            AnnounceGame();
        }

        protected virtual void AnnounceGame() {
            waitingToStartTimer.Start();
        }

		internal abstract void FinalTimeUp();
		internal abstract void TimeUp();

		protected void StopTimers() {
			roundTimer.Stop();
			waitingToStartTimer.Stop();
		}

		protected void MinHit_StartWaitingForAdditionalPlayers() {
            if(!roundTimer.isRunning) {
                waitingToStartTimer.Stop();
                roundTimer.Start();
            }
		}

        public override void Dispose() {
            base.Dispose();
			StopTimers();
		}
	}
}
