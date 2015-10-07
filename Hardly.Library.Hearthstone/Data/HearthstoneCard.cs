namespace Hardly.Library.Hearthstone {
    public interface HearthstoneCard : IDataList {
        string cardId {
            get;
        }

        string name {
            get;
            set;
        }
    }
}
