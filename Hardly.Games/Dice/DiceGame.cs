using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games {
    public class DiceGame<PlayerIdType> : Game<PlayerIdType, GamePlayer<PlayerIdType>> {
        public DiceGame() : base(1) {
        }

        public override void EndGame() {
        }
    }
}
