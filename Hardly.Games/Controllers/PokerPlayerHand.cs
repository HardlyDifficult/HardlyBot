using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games {
    public class PokerPlayerHand<PlayerIdType> : CardPlayerHand<PlayerIdType> {
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

        public PokerPlayerHand(PointManager pointManager, PlayerIdType playerIdObject) : base(pointManager, playerIdObject) {
        }

        public uint HandValue {
            get {
                return 0;
            }
        }

    }
}
