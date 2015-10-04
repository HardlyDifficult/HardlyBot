namespace Hardly.Library.Hearthstone {
    public class SqlHearthstoneCard : SqlRow {

        public SqlHearthstoneCard(string cardId, string name = null) : base(new[] { cardId, name }) {
        }

        static readonly SqlTable _table = new SqlTable("hearthstone_cards");
        public override SqlTable table {
            get {
                return _table;
            }
        }

        public string cardId {
            get {
                return Get<string>(0);
            }
        }

        public string name {
            get {
                return Get<string>(1);
            }
            set {
                Set(1, value);
            }
        }
    }
}
