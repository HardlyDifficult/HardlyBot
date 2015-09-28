using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games {
    public static class PokerPlayerHand {
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

        public static CardCollection GetBestHand(CardCollection playerCards, CardCollection tableCards) {
            Debug.Assert(playerCards.cards.Count == 2);
            Debug.Assert(tableCards.cards.Count == 5);

            ulong bestHandValue = HandValue(tableCards);
            CardCollection bestHand = tableCards;

            // swap one, or the other player card for any one table card.
            foreach(var card in playerCards.cards) {
                for(int i = 0; i < 5; i++) {
                    List<PlayingCard> cards = new List<PlayingCard>();
                    for(int iNewHand = 0; iNewHand < 5; iNewHand++) {
                        if(iNewHand == i) {
                            cards.Add(card);
                        } else {
                            cards.Add(tableCards.cards[iNewHand]);
                        }
                    }
                    CardCollection newHand = new CardCollection(cards.ToArray());
                    ulong newHandValue = HandValue(newHand);
                    if(newHandValue > bestHandValue) {
                        bestHandValue = newHandValue;
                        bestHand = newHand;
                    }
                }
            }
            // swap both for any two table cards.
            for(int iCard1 = 0; iCard1 < 4; iCard1++) {
                for(int iCard2 = iCard1; iCard2 < 5; iCard2++) {
                    List<PlayingCard> cards = new List<PlayingCard>();
                    for(int iNewHand = 0; iNewHand < 5; iNewHand++) {
                        if(iNewHand == iCard1) {
                            cards.Add(playerCards.cards[0]);
                        } else if(iNewHand == iCard2) {
                            cards.Add(playerCards.cards[1]);
                        } else {
                            cards.Add(tableCards.cards[iNewHand]);
                        }
                    }
                }
            }

            return bestHand;
        }

        public static ulong HandValue(CardCollection cards) {
            Debug.Assert(cards.cards.Count == 5);

            cards.cards.Sort();

            PlayingCard.Value? firstPairValue, secondPairValue;
            uint firstPairCardCount, secondPairCardCount;
            bool isStraight;
            bool isFlush;
            CalcHandStats(cards, out isFlush, out firstPairValue, out secondPairValue, out firstPairCardCount, out secondPairCardCount, out isStraight);

            HandTypes myHandType;
            if(isStraight && isFlush) {
                myHandType = HandTypes.StraightFlush;
            } else if(firstPairCardCount == 4) {
                myHandType = HandTypes.FourOfAKind;
            } else if(secondPairCardCount == 3 ||
                (secondPairCardCount == 2 && firstPairCardCount == 3)) {
                myHandType = HandTypes.FullHouse;
            } else if(isFlush) {
                myHandType = HandTypes.Flush;
            } else if(isStraight) {
                myHandType = HandTypes.Straight;
            } else if(firstPairCardCount == 3) {
                myHandType = HandTypes.ThreeOfAKind;
            } else if(secondPairCardCount == 2) {
                myHandType = HandTypes.TwoPair;
            } else if(firstPairCardCount == 2) {
                myHandType = HandTypes.OnePair;
            } else {
                myHandType = HandTypes.HighCard;
            }

            ulong myHandValue; 
            switch(myHandType) {
            case HandTypes.StraightFlush:
            case HandTypes.Flush:
            case HandTypes.HighCard:
            case HandTypes.Straight:
                myHandValue = GetValue(cards.cards[4].value, cards.cards[3].value, cards.cards[2].value, cards.cards[1].value, cards.cards[0].value);
                break;
            case HandTypes.FourOfAKind:
            case HandTypes.ThreeOfAKind:
            case HandTypes.OnePair:
                int iStartOfPair = 0;
                for(int i = 0; i < 5; i++) {
                    if(cards.cards[i].value == firstPairValue.Value) {
                        iStartOfPair = i;
                        break;
                    }
                }
                myHandValue = GetValue(cards.cards[iStartOfPair].value, cards.cards[iStartOfPair + 1].value, 
                    cards.cards[(iStartOfPair + 2) % 5].value, cards.cards[(iStartOfPair + 3) % 5].value, cards.cards[(iStartOfPair + 4) % 5].value);
                break;
            case HandTypes.FullHouse:
                PlayingCard.Value largerValue = firstPairValue.Value;
                PlayingCard.Value smallerValue = secondPairValue.Value;
                if(secondPairCardCount > firstPairCardCount) {
                    largerValue = secondPairValue.Value;
                    smallerValue = firstPairValue.Value;
                }
                myHandValue = GetValue(largerValue, largerValue, largerValue, smallerValue, smallerValue);
                break;
            case HandTypes.TwoPair:
                PlayingCard.Value lastCard = 0;
                foreach(var card in cards.cards) {
                    if(card.value != firstPairValue.Value && card.value != secondPairValue.Value) {
                        lastCard = card.value;
                        break;
                    }
                }
                myHandValue = GetValue(secondPairValue.Value, secondPairValue.Value, firstPairValue.Value, firstPairValue.Value, lastCard);
                break;
            default:
                myHandValue = 0;
                Debug.Fail();
                break;
            }

            return (ulong)myHandType * 100000000 + myHandValue;
        }

        private static ulong GetValue(PlayingCard.Value value1, PlayingCard.Value value2, PlayingCard.Value value3, PlayingCard.Value value4, PlayingCard.Value value5) {
            return (ulong)value1 * 13 ^ 4 + (ulong)value2 * 13 ^ 3 + (ulong)value3 * 13 ^ 2 + (ulong)value4 * 13 ^ 1 + (ulong)value5;
        }

        static void CalcHandStats(CardCollection playerCards, out bool isFlush, out PlayingCard.Value? firstPairValue, out PlayingCard.Value? secondPairValue, out uint firstPairCardCount, out uint secondPairCardCount, out bool isStraight) {
            PlayingCard.Value? lastCardValue = null;
            firstPairValue = null;
            secondPairValue = null;
            firstPairCardCount = 0;
            secondPairCardCount = 0;
            isStraight = true;
            isFlush = CheckForFlush(playerCards);

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
        }

        

       static bool CheckForFlush(CardCollection playerCards) {
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
