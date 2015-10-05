namespace Hardly.Games.Betting {
    public class RussianRoulette<PlayerIdType> : Game<RussianRoulettePlayer<PlayerIdType>, PlayerIdType> {
        public ulong bet;
        uint iRound, iBulletPosition, iCurrentPlayer;

        public RussianRoulette() : base(2, 6) {
            bet = 1;
            iRound = 0;
            iBulletPosition = 0;
            iCurrentPlayer = 0;
        }

        public RussianRoulettePlayer<PlayerIdType> currentPlayer {
            get {
                return GetPlayers()[iCurrentPlayer];
            }
        }

        public override bool Join(RussianRoulettePlayer<PlayerIdType> playerGameObject) {
            if(!Contains(playerGameObject)) {
                if(base.Join(playerGameObject)) {
                    if(playerGameObject.PlaceBet(bet, true) > 0) {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool gunHasBeenFired {
            get {
                return iBulletPosition != 0;
            }
        }

        public bool PullTrigger() {
            if(currentPlayer != null && !gameOver) {
                iRound++;

                if(iRound >= 6) {
                    iRound = 0;
                }

                if(iRound == iBulletPosition) {
                    currentPlayer.isWinner = false;
                    currentPlayer.LoseBet();
                    if(RemovePlayer(currentPlayer)) {
                        ReloadGun();
                    }
                } else {
                    iCurrentPlayer++;
                }

                CheckForEndGame();
                return true;
            }

            return false;
        }

        public bool PussyOut() {
            if(currentPlayer != null && !gameOver) {
                if(Contains(currentPlayer.idObject)) {
                    currentPlayer.LoseBet();
                    if(RemovePlayer(currentPlayer)) {
                        CheckForEndGame();
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool StartGame() {
            if(base.StartGame()) {
                iCurrentPlayer = 0;
                GetPlayers().Shuffle();
                ReloadGun();

                return true;
            }

            return false;
        }

        protected override void EndGame() {
            if(numberOfPlayers >= 1) {
                ulong winnings = TotalBets() / numberOfPlayers;
                foreach(var player in GetPlayers()) {
                    player.isWinner = true;
                    player.Award((long)(winnings - player.bet));
                }
            }

            base.EndGame();
        }

        void CheckForEndGame() {
            if(iCurrentPlayer >= numberOfPlayers) {
                iCurrentPlayer = 0;
            }

            if(numberOfPlayers <= 1) {
                EndGame();
            }
        }

        void ReloadGun() {
            iBulletPosition = 0;
            iRound = Random.Uint.LessThan(6);
        }
    }
}
