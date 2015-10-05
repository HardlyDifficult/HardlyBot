using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hardly.Games.Uno.UnoCard;

namespace Hardly.Games.Uno {
    public static class UnoCardHelpers {
            public static string ToString(this Color color) {
                return color.ToString().Substring(0, 1);
            }
            public static string ToString(this Value value) {
                switch(value) {
                case Value.Zero:
                    return "0";
                case Value.One:
                    return "1";
                case Value.Two:
                    return "2";
                case Value.Three:
                    return "3";
                case Value.Four:
                    return "4";
                case Value.Five:
                    return "5";
                case Value.Six:
                    return "6";
                case Value.Seven:
                    return "7";
                case Value.Eight:
                    return "8";
                case Value.Nine:
                    return "9";
                case Value.Skip:
                    return "Skip";
                case Value.Reverse:
                    return "Reverse";
                case Value.Draw2:
                    return "Draw2";
                case Value.Wild:
                    return "Wild";
                case Value.WildDraw4:
                    return "WildDraw4";
                default:
                    Debug.Fail();
                    return "";
                }
            }
        
    }
}
