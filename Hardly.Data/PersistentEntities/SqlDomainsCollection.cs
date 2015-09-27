namespace Hardly
{
    public class SqlDomainsCollection : SqlRow
    {
        public readonly SqlDomain domain;
        public readonly SqlCollection collection;
		
        public SqlDomainsCollection(SqlDomain domain, SqlCollection collection)
            : base(new object[] { domain.id, collection.id })
        {
            this.domain = domain;
            this.collection = collection;
        }

        internal static readonly SqlTable _table = new SqlTable("domains_collections");
        public override SqlTable table
        {
            get
            {
                return _table;
            }
        }

        ulong domainId
        {
            get
            {
                return Get<ulong>(0);
            }
        }
        
        ulong collectionId
        {
            get
            {
                return Get<ulong>(1);
            }
        }
    }
}
