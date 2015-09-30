using System;

namespace Hardly.Games {
    public class PokerPlayerHandEvaluator : PlayingCardListEvaluator {
        public enum HandType {
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
        public HandType handType {
            get;
            private set;
        }
        public ulong handValue {
            get;
            private set;
        }

        public PokerPlayerHandEvaluator(PlayingCardList playerCards, PlayingCardList tableCards) : base(null) {
            Debug.Assert(playerCards.Count == 2);
            Debug.Assert(tableCards.Count == 5);

            Tuple<HandType, ulong> bestHandValue = HandValue(tableCards);
            PlayingCardList bestHand = tableCards;

            // swap one, or the other player card for any one table card.
            foreach(var card in playerCards) {
                for(int i = 0; i < 5; i++) {
                    var cards = new PlayingCardList();
                    for(int iNewHand = 0; iNewHand < 5; iNewHand++) {
                        if(iNewHand == i) {
                            cards.Add(card);
                        } else {
                            cards.Add(tableCards[iNewHand]);
                        }
                    }
                    var newHand = new PlayingCardList(cards);
                    Tuple<HandType, ulong> newHandValue = HandValue(newHand);
                    if(newHandValue.Item2 > bestHandValue.Item2) {
                        bestHandValue = newHandValue;
                        bestHand = newHand;
                    }
                }
            }
            // swap both for any two table cards.
            for(int iCard1 = 0; iCard1 < 4; iCard1++) {
                for(int iCard2 = iCard1; iCard2 < 5; iCard2++) {
                    var cards = new PlayingCardList();
                    for(int iNewHand = 0; iNewHand < 5; iNewHand++) {
                        if(iNewHand == iCard1) {
                            cards.Add(playerCards[0]);
                        } else if(iNewHand == iCard2) {
                            cards.Add(playerCards[1]);
                        } else {
                            cards.Add(tableCards[iNewHand]);
                        }
                    }
                    var newHand = new PlayingCardList(cards);
                    Tuple<HandType, ulong> newHandValue = HandValue(newHand);
                    if(newHandValue.Item2 > bestHandValue.Item2) {
                        bestHandValue = newHandValue;
                        bestHand = newHand;
                    }
                }
            }

            this.cards = bestHand;
            this.handType = bestHandValue.Item1;
            this.handValue = bestHandValue.Item2;
        }

        static Tuple<HandType, ulong> HandValue(PlayingCardList cards) {
            Debug.Assert(cards.Count == 5);

            cards.Sort();

            PlayingCard.Value? firstPairValue, secondPairValue;
            uint firstPairCardCount, secondPairCardCount;
            bool isStraight;
            bool isFlush;
            CalcHandStats(cards, out isFlush, out firstPairValue, out secondPairValue, out firstPairCardCount, out secondPairCardCount, out isStraight);

            HandType myHandType;
            myHandType = GetHandType(firstPairCardCount, secondPairCardCount, isStraight, isFlush);

            ulong myHandValue;
            switch(myHandType) {
            case HandType.StraightFlush:
            case HandType.Flush:
            case HandType.HighCard:
            case HandType.Straight:
                myHandValue = GetValue(cards[4].value, cards[3].value, cards[2].value, cards[1].value, cards[0].value);
                break;
            case HandType.FourOfAKind:
            case HandType.ThreeOfAKind:
            case HandType.OnePair:
                int iStartOfPair = 0;
                for(int i = 0; i < 5; i++) {
                    if(cards[i].value == firstPairValue.Value) {
                        iStartOfPair = i;
                        break;
                    }
                }
                myHandValue = GetValue(cards[iStartOfPair].value, cards[iStartOfPair + 1].value,
                    cards[(iStartOfPair + 2) % 5].value, cards[(iStartOfPair + 3) % 5].value, cards[(iStartOfPair + 4) % 5].value);
                break;
            case HandType.FullHouse:
                PlayingCard.Value largerValue = firstPairValue.Value;
                PlayingCard.Value smallerValue = secondPairValue.Value;
                if(secondPairCardCount > firstPairCardCount) {
                    largerValue = secondPairValue.Value;
                    smallerValue = firstPairValue.Value;
                }
                myHandValue = GetValue(largerValue, largerValue, largerValue, smallerValue, smallerValue);
                break;
            case HandType.TwoPair:
                PlayingCard.Value lastCard = 0;
                foreach(var card in cards) {
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

            return Tuple.Create(myHandType, (ulong)myHandType * 100000000 + myHandValue);
        }
        
        static HandType GetHandType(uint firstPairCardCount, uint secondPairCardCount, bool isStraight, bool isFlush) {
            HandType myHandType;
            if(isStraight && isFlush) {
                myHandType = HandType.StraightFlush;
            } else if(firstPairCardCount == 4) {
                myHandType = HandType.FourOfAKind;
            } else if(secondPairCardCount == 3 ||
                (secondPairCardCount == 2 && firstPairCardCount == 3)) {
                myHandType = HandType.FullHouse;
            } else if(isFlush) {
                myHandType = HandType.Flush;
            } else if(isStraight) {
                myHandType = HandType.Straight;
            } else if(firstPairCardCount == 3) {
                myHandType = HandType.ThreeOfAKind;
            } else if(secondPairCardCount == 2) {
                myHandType = HandType.TwoPair;
            } else if(firstPairCardCount == 2) {
                myHandType = HandType.OnePair;
            } else {
                myHandType = HandType.HighCard;
            }

            return myHandType;
        }

        static ulong GetValue(PlayingCard.Value value1, PlayingCard.Value value2, PlayingCard.Value value3, PlayingCard.Value value4, PlayingCard.Value value5) {
            return (ulong)((ulong)value1 * Math.Pow(13,4) + (ulong)value2 * Math.Pow(13, 3) + (ulong)value3 * Math.Pow(13,2) + (ulong)value4 * 13 + (ulong)value5);
        }

        static void CalcHandStats(PlayingCardList playerCards, out bool isFlush, out PlayingCard.Value? firstPairValue, out PlayingCard.Value? secondPairValue, out uint firstPairCardCount, out uint secondPairCardCount, out bool isStraight) {
            PlayingCard.Value? lastCardValue = null;
            firstPairValue = null;
            secondPairValue = null;
            firstPairCardCount = 0;
            secondPairCardCount = 0;
            isStraight = true;
            isFlush = CheckForFlush(playerCards);

            foreach(var card in playerCards) {
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

        static bool CheckForFlush(PlayingCardList playerCards) {
            PlayingCard.Suit flushSuit = playerCards[0].suit;
            foreach(var card in playerCards) {
                if(card.suit != flushSuit) {
                    return false;
                }
            }

            return true;
        }
        
    }
}
