using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games {
    public class PokerPlayerHand {
        enum HandTypes {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush
        }

        public uint HandValue(CardCollection playerCards, CardCollection tableCards) {
            Debug.Assert(playerCards.cards.Count == 2);
            Debug.Assert(tableCards.cards.Count == 5);



            return 0;
        }

        public uint HandValue(CardCollection playerCards) {
            Debug.Assert(playerCards.cards.Count == 5);

            bool isFlush = CheckForFlush(playerCards);
            playerCards.cards.Sort();

            GetHandType(playerCards, isFlush);

            return 0;
        }

        private static HandTypes GetHandType(CardCollection playerCards, bool isFlush) {
            PlayingCard.Value? lastCardValue = null, firstPairValue = null, secondPairValue = null;
            uint firstPairCardCount = 0, secondPairCardCount = 0;
            bool isStraight = true;
            foreach(var card in playerCards.cards) {
                if(lastCardValue != null) {
                    if(card.value != lastCardValue.Value + 1) {
                        isStraight = false;
                    }

                    if(card.value == lastCardValue.Value) {
                        if(firstPairValue == null) {
                            firstPairValue = card.value;
                            firstPairCardCount = 2;
                        } else if(card.value == firstPairValue) {
                            firstPairCardCount++;
                        } else if(card.value == secondPairValue) {
                            secondPairCardCount++;
                        } else {
                            Debug.Assert(secondPairValue == null);
                            secondPairValue = card.value;
                            secondPairCardCount = 2;
                        }
                    }
                }
                lastCardValue = card.value;
            }

            if(isStraight && isFlush) {
                return HandTypes.StraightFlush;
            } else if(firstPairCardCount == 4) {
                return HandTypes.FourOfAKind;
            } else if(secondPairCardCount == 3 ||
                (secondPairCardCount == 2 && firstPairCardCount == 3)) {
                return HandTypes.FullHouse;
            } else if(isFlush) {
                return HandTypes.Flush;
            } else if(isStraight) {
                return HandTypes.Straight;
            } else if(firstPairCardCount == 3) {
                return HandTypes.ThreeOfAKind;
            } else if(secondPairCardCount == 2) {
                return HandTypes.TwoPair;
            } else if(firstPairCardCount == 2) {
                return HandTypes.OnePair;
            } else {
                return HandTypes.HighCard;
            }

        }

        bool CheckForFlush(CardCollection playerCards) {
            PlayingCard.Suit flushSuit = playerCards.cards[0].suit;
            foreach(var card in playerCards.cards) {
                if(card.suit != flushSuit) {
                    return false;
                }
            }

            return true;
        }
        
    }
}
