using System;

namespace Hardly
{
    public class SqlImageMetadata : SqlRow
    {
        public readonly SqlStaticContent originalContent;
        SqlAuthor _author;

        public SqlImageMetadata(SqlStaticContent originalContent, string alt = null, SqlAuthor author = null, DateTime created = default(DateTime))
            : base(new object[] { originalContent.id, alt, author != null ? author.id : 0, created } )
        {
            this.originalContent = originalContent;
            _author = author;
        }

        internal static readonly SqlTable _table = new SqlTable("images");
        public override SqlTable table
        {
            get
            {
                return _table;
            }
        }

        ulong originalContentId
        {
            get
            {
                return Get<ulong>(0);
            }
        }

        public string alt
        {
            get
            {
                return Get<string>(1);
            }
            set
            {
                Set(1, value);
            }
        }

        ulong authorId
        {
            get
            {
                return Get<ulong>(2);
            }
            set
            {
                Set(2, value);
                _author = null;
            }
        }

        public SqlAuthor author
        {
            get
            {
                if(_author == null && authorId > 0)
                {
                    _author = new SqlAuthor(authorId);
                }

                return _author;
            }
            set
            {
                authorId = value != null ? value.id : 0;
                _author = value;
            }
        }

        public DateTime createdDate
        {
            get
            {
                return Get<DateTime>(3);
            }
            set
            {
                Set(3, value);
            }
        }
    }
}
