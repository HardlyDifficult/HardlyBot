using System;

namespace Hardly
{
    public class SqlDomainsCommentAndFile : SqlRow
    {
        public readonly SqlDomain domain;
        SqlStaticContent _file;

        public SqlDomainsCommentAndFile(SqlDomain domain, DateTime created, string from = null, string message = null, SqlStaticContent file = null, string filename = null)
            : base(new object[] { domain.id, created, from, message, file != null ? file.id : 0, filename })
        {
            this.domain = domain;
            _file = file;
        }
        
        internal static readonly SqlTable _table = new SqlTable("domains_comment_and_files");
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

        public DateTime dateCreated
        {
            get
            {
                return Get<DateTime>(1);
            }
        }

        public string fromName
        {
            get
            {
                return Get<string>(2);
            }
            set
            {
                Set(2, value);
            }
        }

        public string message
        {
            get
            {
                return Get<string>(3);
            }
            set
            {
                Set(3, value);
            }
        }

        ulong fileContentId
        {
            get
            {
                return Get<ulong>(4);
            }
            set
            {
                Set(4, value);
                _file = null;
            }
        }

        public SqlStaticContent file
        {
            get
            {
                if(_file == null && fileContentId > 0)
                {
                    _file = new SqlStaticContent(fileContentId);
                }

                return _file;
            }
            set
            {
                fileContentId = value != null ? value.id : 0;
                _file = value;
            }
        }

        public string fileName
        {
            get
            {
                return Get<string>(5);
            }
            set
            {
                Set(5, value);
            }
        }
    }
}
