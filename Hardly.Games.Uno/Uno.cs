namespace Hardly.Games.Uno {
    public class Uno<PlayerIdType> : CardGame<UnoPlayer<PlayerIdType>, PlayerIdType, UnoCard> {
        List<UnoCard> discardPile = new List<UnoCard>();

        public Uno() : base(new UnoDeck(), 2, 10) {
        }

        public override bool StartGame() {
            if(base.StartGame()) {
                foreach(var player in GetPlayers()) {
                    for(int i = 0; i < 7; i++) {
                        DealCard(player.hand);
                    }
                }

                var card = GetFirstCard();
                discardPile.Add(card);

                return true;
            }

            return false;
        }

        public override UnoCard DealCard(List<UnoCard> playerCards) {
            var card = base.DealCard(playerCards);

            if(deck.numberOfCardsRemaining == 0) {
                deck.ShuffleIn(discardPile);
                discardPile.Clear();
            }

            return card;
        }

        UnoCard GetFirstCard() {
            var card = deck.TakeTopCard();
            if(card.value.Equals(UnoCard.Value.WildDraw4)) {
                deck.ShuffleIn(card);
                return GetFirstCard();
            } else {
                return card;
            }
        }

        public override void Reset() {
            base.Reset();
            discardPile.Clear();
        } 
    }
}
