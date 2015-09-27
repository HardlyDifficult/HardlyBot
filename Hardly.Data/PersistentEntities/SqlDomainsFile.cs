namespace Hardly
{
    public class SqlDomainsFile : SqlRow
    {
        public readonly SqlDomain domain;
        SqlStaticContent _staticContent;

        public SqlDomainsFile(SqlDomain domain, string fileName, SqlStaticContent staticContent = null)
            : base(new object[] { domain.id, fileName, staticContent != null ? staticContent.id : 0 })
        {
            this.domain = domain;
            _staticContent = staticContent;
        }

        internal static readonly SqlTable _table = new SqlTable("domains_files");
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

        public string fileName
        {
            get
            {
                return Get<string>(1);
            }
        }

        ulong contentId
        {
            get
            {
                return Get<ulong>(2);
            }
            set
            {
                Set(2, value);
                _staticContent = null;
            }
        }

        public SqlStaticContent staticContent
        {
            get
            {
                if(_staticContent == null && contentId > 0)
                {
                    _staticContent = new SqlStaticContent(contentId);
                }

                return _staticContent;
            }
            set
            {
                contentId = value != null ? value.id : 0;
                _staticContent = value;
            }
        }
    }
}
