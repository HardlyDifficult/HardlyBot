using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Hearthstone {
    public class DrawCard : HearthstoneEvent {
        public readonly bool myTurn;
        public readonly SqlHearthstoneCard card;

        public DrawCard(HearthGame game, bool myTurn, SqlHearthstoneCard card) : base(game) {
            this.myTurn = myTurn;
            this.card = card;
        }
    }
}
