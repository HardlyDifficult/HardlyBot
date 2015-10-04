using System;

namespace Hardly.Games.Betting {
    public class RussianRoulette<PlayerIdType> : Game<PlayerIdType, RussianRoulettePlayer<PlayerIdType>> {
        public ulong bet;
        public readonly List<RussianRoulettePlayer<PlayerIdType>> seatedPlayers = new List<RussianRoulettePlayer<PlayerIdType>>();
        uint iRound, iBulletPosition, iCurrentPlayer;

        public RussianRoulette() : base(6) {
            iRound = 0;
            iBulletPosition = 0;
            iCurrentPlayer = 0;
        }

        public bool PussyOut() {
            if(currentPlayer != null && !GameOver()) {
                if(Contains(currentPlayer.idObject)) {
                    currentPlayer.LoseBet();
                    if(seatedPlayers.Remove(currentPlayer)) {
                        CheckForEndGame();
                        return true;
                    }
                }
            }

            return false;
        }

        private void CheckForEndGame() {
            if(iCurrentPlayer >= seatedPlayers.Count) {
                iCurrentPlayer = 0;
            }

            if(seatedPlayers.Count <= 1) {
                EndGame();
            }
        }

        public override void Reset() {
            base.Reset();

            iCurrentPlayer = 0;
        }

        public override bool CanStart() {
            return NumberOfPlayers() >= 2;
        }

        public override void EndGame() {
            if(seatedPlayers.Count >= 1) {
                ulong winnings = TotalPot() / (ulong)seatedPlayers.Count;
                foreach(var player in seatedPlayers) {
                    player.Award((long)(winnings - player.bet));
                }
            }
        }

        private ulong TotalPot() {
            ulong pot = 0;
            foreach(var player in PlayerGameObjects) {
                pot += player.bet;
            }

            return pot;
        }

        public override bool Join(PlayerIdType playerId, RussianRoulettePlayer<PlayerIdType> gameObject) {
            if(!Contains(playerId)) {
                if(gameObject.PlaceBet(bet, true) > 0) {
                    if(base.Join(playerId, gameObject)) {
                        return true;
                    } else {
                        gameObject.CancelBet();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// True you died, false you lived.
        /// </summary>
        public bool PullTrigger() {
            iRound++;

            if(iRound >= 6) {
                iRound = 0;
            }

            if(iRound == iBulletPosition) {
                currentPlayer.LoseBet();
                seatedPlayers.Remove(currentPlayer);
                CheckForEndGame();
                ReloadGun();
                return true;
            } else {
                iCurrentPlayer++;
                CheckForEndGame();

                return false;
            }
        }

        public bool GameOver() {
            return seatedPlayers.Count <= 1;
        }

        public override bool StartGame() {
            if(base.StartGame()) {
                seatedPlayers.Clear();
                seatedPlayers.Add(PlayerGameObjects);
                seatedPlayers.Shuffle();

                ReloadGun();

                return true;
            }

            return false;
        }

        private void ReloadGun() {
            iBulletPosition = 0;
            iRound = Random.Uint.LessThan(6);
        }

        public bool gunHasBeenFired {
            get {
                return iBulletPosition != 0;
            }
        }

        public RussianRoulettePlayer<PlayerIdType> currentPlayer {
            get {
                return seatedPlayers[iCurrentPlayer];
            }
        }

        public override void LeaveGame(PlayerIdType playerId) {
            seatedPlayers.Remove(Get(playerId));
            base.LeaveGame(playerId);
        }
    }
}
