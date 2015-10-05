using System;

namespace Hardly.Games {
	public class PlayingCard : ICard {
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

        public static readonly PlayingCard
            c2c = new PlayingCard(Suit.Clubs, Value.Two),
            c2d = new PlayingCard(Suit.Diamonds, Value.Two),
            c2h = new PlayingCard(Suit.Hearts, Value.Two),
            c2s = new PlayingCard(Suit.Spades, Value.Two),
            c3c = new PlayingCard(Suit.Clubs, Value.Three),
            c3d = new PlayingCard(Suit.Diamonds, Value.Three),
            c3h = new PlayingCard(Suit.Hearts, Value.Three),
            c3s = new PlayingCard(Suit.Spades, Value.Three),
            c4c = new PlayingCard(Suit.Clubs, Value.Four),
            c4d = new PlayingCard(Suit.Diamonds, Value.Four),
            c4h = new PlayingCard(Suit.Hearts, Value.Four),
            c4s = new PlayingCard(Suit.Spades, Value.Four),
            c5c = new PlayingCard(Suit.Clubs, Value.Five),
            c5d = new PlayingCard(Suit.Diamonds, Value.Five),
            c5h = new PlayingCard(Suit.Hearts, Value.Five),
            c5s = new PlayingCard(Suit.Spades, Value.Five),
            c6c = new PlayingCard(Suit.Clubs, Value.Six),
            c6d = new PlayingCard(Suit.Diamonds, Value.Six),
            c6h = new PlayingCard(Suit.Hearts, Value.Six),
            c6s = new PlayingCard(Suit.Spades, Value.Six),
            c7c = new PlayingCard(Suit.Clubs, Value.Seven),
            c7d = new PlayingCard(Suit.Diamonds, Value.Seven),
            c7h = new PlayingCard(Suit.Hearts, Value.Seven),
            c7s = new PlayingCard(Suit.Spades, Value.Seven),
            c8c = new PlayingCard(Suit.Clubs, Value.Eight),
            c8d = new PlayingCard(Suit.Diamonds, Value.Eight),
            c8h = new PlayingCard(Suit.Hearts, Value.Eight),
            c8s = new PlayingCard(Suit.Spades, Value.Eight),
            c9c = new PlayingCard(Suit.Clubs, Value.Nine),
            c9d = new PlayingCard(Suit.Diamonds, Value.Nine),
            c9h = new PlayingCard(Suit.Hearts, Value.Nine),
            c9s = new PlayingCard(Suit.Spades, Value.Nine),
            c0c = new PlayingCard(Suit.Clubs, Value.Ten),
            c0d = new PlayingCard(Suit.Diamonds, Value.Ten),
            c0h = new PlayingCard(Suit.Hearts, Value.Ten),
            c0s = new PlayingCard(Suit.Spades, Value.Ten),
            cjc = new PlayingCard(Suit.Clubs, Value.Jack),
            cjd = new PlayingCard(Suit.Diamonds, Value.Jack),
            cjh = new PlayingCard(Suit.Hearts, Value.Jack),
            cjs = new PlayingCard(Suit.Spades, Value.Jack),
            cqc = new PlayingCard(Suit.Clubs, Value.Queen),
            cqd = new PlayingCard(Suit.Diamonds, Value.Queen),
            cqh = new PlayingCard(Suit.Hearts, Value.Queen),
            cqs = new PlayingCard(Suit.Spades, Value.Queen),
            ckc = new PlayingCard(Suit.Clubs, Value.King),
            ckd = new PlayingCard(Suit.Diamonds, Value.King),
            ckh = new PlayingCard(Suit.Hearts, Value.King),
            cks = new PlayingCard(Suit.Spades, Value.King),
            cac = new PlayingCard(Suit.Clubs, Value.Ace),
            cad = new PlayingCard(Suit.Diamonds, Value.Ace),
            cah = new PlayingCard(Suit.Hearts, Value.Ace),
            cas = new PlayingCard(Suit.Spades, Value.Ace);
    
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
			case Value.Jack:
			case Value.Queen:
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
            return ToChatString();
		}
        
        public int CompareTo(object obj) {
            if(obj != null && obj.GetType().Equals(typeof(PlayingCard))) {
                PlayingCard other = obj as PlayingCard;
                return ((int)value).CompareTo((int)other.value);
            }

            return -1;
        }
    }
}
