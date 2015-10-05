using System;

namespace Hardly.Games.Uno {
    public class UnoCard : ICard {
        public enum Color {
            Red, Yellow, Green, Blue
        }
        public enum Value {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Skip,
            Reverse,
            Draw2,
            Wild,
            WildDraw4
        }
        public readonly Color color;
        public readonly Value value;

        public UnoCard(Color color, Value value) {
            this.color = color;
            this.value = value;
        }

        public static UnoCard FromString(string cardText) {
            if(cardText != null) {
                Color? color = null;
                Value? type = null;

                foreach(Value t in Enum.GetValues(typeof(Value))) {
                    if(cardText.EndsWith(t.ToString(), StringComparison.CurrentCultureIgnoreCase)) {
                        type = t;
                    }
                }
                foreach(Color c in Enum.GetValues(typeof(Color))) {
                    if(cardText.EndsWith(c.ToString(), StringComparison.CurrentCultureIgnoreCase)) {
                        color = c;
                    }
                }

                if((color != null && type != null) 
                    || (type != null && (type.Equals(UnoCard.Value.Wild) || type.Equals(UnoCard.Value.WildDraw4)))) {
                    return new UnoCard(color.GetValueOrDefault(Color.Blue), type.Value);
                }
            }

            return null;
        }

        public int CompareTo(object obj) {
            var otherCard = obj as UnoCard;
            var comparision = value.CompareTo(otherCard.value);
            if(comparision == 0) {
                return color.CompareTo(otherCard.color);
            } else {
                return comparision;
            }
        }

        public override string ToString() {
            var result = value.ToString();
            if(!value.Equals(UnoCard.Value.Wild) && !value.Equals(UnoCard.Value.WildDraw4)) {
                result += color.ToString();
            }
            return result;
        }
    }
}
