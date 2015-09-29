﻿using System;

namespace Hardly.Games {
	public class TexasHoldemPlayer<PlayerIdType> : CardPlayer<PlayerIdType> {
        public TexasHoldemPlayer(PlayerPointManager pointManager, PlayerIdType playerIdObject) : base(pointManager, playerIdObject) {
            bestHand = null;
		}


        public PokerPlayerHandEvaluator bestHand {
            get;
            private set;
        }

        internal void EndGame(PlayingCardList tableCards) {
            Debug.Assert(bestHand == null);
            bestHand = new PokerPlayerHandEvaluator(hand.cards, tableCards);
        }
    }
}