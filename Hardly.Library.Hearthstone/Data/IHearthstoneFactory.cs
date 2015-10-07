namespace Hardly.Library.Hearthstone {
    public interface IHearthstoneFactory {
        HearthstoneCard CreateCard(string cardId, string cardName = null);
    }
}
