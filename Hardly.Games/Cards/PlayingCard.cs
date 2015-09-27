using System;

namespace Hardly.Games {
	public class PlayingCard : IComparable {
		public enum Suit {
			Clubs,
			Diamonds,
			Hearts,
			Spades
		}

		public enum Value {
			Two,
			Three,
			Four,
			Five,
			Six,
			Seven,
			Eight,
			Nine,
			Ten,
			Jack,
			Queen,
			King,
            Ace,
            Joker
        }
        
		public string ToChatString(bool showSymbols = false) {
			string chatMessage = showSymbols ? "" : "[";
			switch(value) {
			case Value.Ace:
				if(!showSymbols) {
					chatMessage += "A";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD1";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC1";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB1";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA1";
						break;
					}
				}
				break;
			case Value.Two:
				if(!showSymbols) {
					chatMessage += "2";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD2";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC2";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB2";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA2";
						break;
					}
				}
				break;
			case Value.Three:
				if(!showSymbols) {
					chatMessage += "3";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD3";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC3";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB3";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA3";
						break;
					}
				}
				break;
			case Value.Four:
				if(!showSymbols) {
					chatMessage += "4";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD4";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC4";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB4";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA4";
						break;
					}
				}
				break;
			case Value.Five:
				if(!showSymbols) {
					chatMessage += "5";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD5";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC5";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB5";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA5";
						break;
					}
				}
				break;
			case Value.Six:
				if(!showSymbols) {
					chatMessage += "6";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD6";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC6";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB6";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA6";
						break;
					}
				}
				break;
			case Value.Seven:
				if(!showSymbols) {
					chatMessage += "7";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD7";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC7";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB7";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA7";
						break;
					}
				}
				break;
			case Value.Eight:
				if(!showSymbols) {
					chatMessage += "8";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD8";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC8";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB8";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA8";
						break;
					}
				}
				break;
			case Value.Nine:
				if(!showSymbols) {
					chatMessage += "9";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCD9";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCC9";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCB9";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCA9";
						break;
					}
				}
				break;
			case Value.Ten:
				if(!showSymbols) {
					chatMessage += "10";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCDA";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCCA";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCBA";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCAA";
						break;
					}
				}
				break;
			case Value.Jack:
				if(!showSymbols) {
					chatMessage += "J";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCDB";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCCB";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCBB";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCAB";
						break;
					}
				}
				break;
			case Value.Queen:
				if(!showSymbols) {
					chatMessage += "Q";
				} else {
					switch(suit) {
					case Suit.Clubs:
						chatMessage += "\uD83C\uDCDD";
						break;
					case Suit.Diamonds:
						chatMessage += "\uD83C\uDCCD";
						break;
					case Suit.Hearts:
						chatMessage += "\uD83C\uDCBD";
						break;
					case Suit.Spades:
						chatMessage += "\uD83C\uDCAD";
						break;
					}
				}
				break;
            case Value.King:
                if(!showSymbols) {
                    chatMessage += "K";
                } else {
                    switch(suit) {
                    case Suit.Clubs:
                        chatMessage += "\uD83C\uDCDE";
                        break;
                    case Suit.Diamonds:
                        chatMessage += "\uD83C\uDCCE";
                        break;
                    case Suit.Hearts:
                        chatMessage += "\uD83C\uDCBE";
                        break;
                    case Suit.Spades:
                        chatMessage += "\uD83C\uDCAE";
                        break;
                    }
                }
                break;
            case Value.Joker:
                chatMessage += "Joker";
                break;
            }

			if(!showSymbols) {
				switch(suit) {
				case Suit.Clubs:
					chatMessage += "\u2667";
					break;
				case Suit.Diamonds:
					chatMessage += "\u2662";
					break;
				case Suit.Hearts:
					chatMessage += "\u2661";
					break;
				case Suit.Spades:
					chatMessage += "\u2664";
					break;
				}
			}

			chatMessage += showSymbols ? "" : "]";

			return chatMessage;
		}

		public uint BlackjackValue() {
			switch(value) {
			case Value.Ace:
				return 11;
			case Value.Two:
				return 2;
			case Value.Three:
				return 3;
			case Value.Four:
				return 4;
			case Value.Five:
				return 5;
			case Value.Six:
				return 6;
			case Value.Seven:
				return 7;
			case Value.Eight:
				return 8;
			case Value.Nine:
				return 9;
			case Value.Ten:
				return 10;
			case Value.Jack:
				return 10;
			case Value.Queen:
				return 10;
			case Value.King:
				return 10;
			default:
				Debug.Fail();
				return 0;
			}
		}

		public readonly Suit suit;
		public readonly Value value;

		public PlayingCard(Suit suit, Value value) {
			this.suit = suit;
			this.value = value;
		}

		public override string ToString() {
			return suit.ToString() + value.ToString();
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

        public int CompareTo(object obj) {
            return value.CompareTo(obj);
        }
    }
}
