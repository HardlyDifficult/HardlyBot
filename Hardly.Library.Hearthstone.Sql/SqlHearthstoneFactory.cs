using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Hearthstone {
    public class SqlHearthstoneFactory : IHearthstoneFactory {
        public HearthstoneCard CreateCard(string cardId, string cardName = null) {
            return new SqlHearthstoneCard(cardId, cardName);
        }
    }
}
