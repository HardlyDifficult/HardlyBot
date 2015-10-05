namespace Hardly.Games {
    public class PickANumberGame<PlayerIdType> : Game<PickANumberPlayer<PlayerIdType>, PlayerIdType> {
        public uint minNumberInclusive, maxNumberInclusive;
        public uint rolledNumber {
            get;
            private set;
        }

        public PickANumberGame() : this(1, 10) {
        }

        public PickANumberGame(uint minNumberInclusive, uint maxNumberInclusive) : base(1, 1) {
            this.minNumberInclusive = minNumberInclusive;
            this.maxNumberInclusive = maxNumberInclusive;
        }
        
        protected override void EndGame() {
            foreach(var player in GetPlayers()) {
                if(player.guessedNumber.Equals(rolledNumber)) {
                    player.Award((long)player.bet * (maxNumberInclusive - minNumberInclusive));
                    player.isWinner = true;
                } else {
                    player.LoseBet();
                    player.isWinner = false;
                }
            }

            base.EndGame();
        }

        public override bool Join(PickANumberPlayer<PlayerIdType> playerGameObject) {
            if(playerGameObject.guessedNumber >= minNumberInclusive && playerGameObject.guessedNumber <= maxNumberInclusive) {
                return base.Join(playerGameObject);
            }

            return false;
        }

        public override bool StartGame() {
            if(base.StartGame()) {
                rolledNumber = Random.Uint.LessThan(maxNumberInclusive) + minNumberInclusive;
                EndGame();
                return true;
            }

            return false;
        }
    }
}
