using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Hearthstone {
    public class HearthGame {
        public readonly string myPlayerName, opponentPlayerName;
        public readonly bool iStartFirst;

        public HearthGame(string myPlayerName, string opponentPlayerName, bool iStartFirst) {
            this.myPlayerName = myPlayerName;
            this.opponentPlayerName = opponentPlayerName;
            this.iStartFirst = iStartFirst;
        }
    }
}
