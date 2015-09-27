namespace Hardly
{
    public class SqlStaticContentCollection : SqlRow
    {
        public readonly SqlCollection collection;
        public readonly SqlStaticContent content;

        public SqlStaticContentCollection(SqlCollection collection, SqlStaticContent content)
            : base(new object[] { collection.id, content.id })
        {
            this.collection = collection;
            this.content = content;
        }

        internal static readonly SqlTable _table = new SqlTable("static_content_collections");
        public override SqlTable table
        {
            get
            {
                return _table;
            }
        }

        ulong collectionId
        {
            get
            {
                return Get<ulong>(0);
            }
        }

        ulong contentId
        {
            get
            {
                return Get<ulong>(1);
            }
        }
    }
}
