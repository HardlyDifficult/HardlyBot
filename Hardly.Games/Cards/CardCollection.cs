using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Games {
	public class CardCollection {
		public List<PlayingCard> cards;

		public CardCollection(PlayingCard[] playingCard = null) {
			this.cards = new List<PlayingCard>(playingCard ?? new PlayingCard[] { });
		}

		public PlayingCard ViewCard(uint cardId) {
			return cards[(int)cardId];
		}

		public PlayingCard TakeTopCard() {
			if(cards.Count() > 0) {
				PlayingCard card = cards[0];
				cards.RemoveAt(0);

				return card;
			}

			return null;
		}

		public string ToChatString(bool showSymbols = false) {
			string message = "";
			foreach(var card in cards) {
				message += card.ToChatString(showSymbols);
			}

			return message;
		}

		public void GiveCard(PlayingCard card) {
			Log.info(card.ToChatString() + " given.");
			cards.Add(card);
		}
	}
}
