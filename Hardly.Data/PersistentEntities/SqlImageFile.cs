namespace Hardly
{
    public class SqlImageFile : SqlRow
    {
        public readonly SqlStaticContent content;

        public SqlImageFile(SqlStaticContent content, uint width = 0, uint height = 0)
            : base(new object[] { content.id, width, height })
        {
            this.content = content;
        }

        internal static readonly SqlTable _table = new SqlTable("image_files");
        public override SqlTable table
        {
            get
            {
                return _table;
            }
        }

        ulong contentId
        {
            get
            {
                return Get<ulong>(0);
            }
        }

        public uint width
        {
            get
            {
                return Get<uint>(1);
            }
            set
            {
                Set(1, value);
            }
        }

        public uint height
        {
            get
            {
                return Get<uint>(2);
            }
            set
            {
                Set(2, value);
            }
        }
    }
}
