using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch.Data
{
    class SqlTwitchCommand : SqlRow
    {
        //
        public SqlTwitchCommand(uint id, string Command, string Discription = null, string Aliases = null, bool Mod = true, int Time = 0)
                : base(new object[] { id, Command, Discription, Aliases, Mod, Time })
        {

        }
        // 
        internal static readonly SqlTable _table = new SqlTable("twitch_Command");
        
        //
        public override SqlTable table
        {
            get
            {
                return _table;
            }
        }
        //get the data from the DB
        public uint id
        {
            get
            {
                return Get<uint>(0);
            }
        }

        public string Command
        {
            get
            {
                return Get<string>(1);
            }
        }

        public string Discription
        {
            get
            {
                return Get<string>(2);
            }
        }

        public string Aliases
        {
            get
            {
                return Get<string>(3);
            }
        }

        public bool Mod
        {
            get
            {
                return Get<bool>(4);
            }
            set
            {
                Set(3, value);
            }
        }

        public int Time
        {
            get
            {
                return Get<int>(5);
            }
        }

        //
        public static SqlTwitchCommand[] GetAll()
        {
            List<object[]> results = _table.Select(null, null, null, null, null, 0);
            if (results != null)
            {
                SqlTwitchCommand[] commands = new SqlTwitchCommand[results.Count];
                for (int i = 0; i < results.Count; i++)
                {
                    //uint id[0], string Command[1], string Discription = null[2], string Aliases = null[3], bool Mod = true[4], int Time = 0[5]
                    commands[i] = new SqlTwitchCommand(results[i][0].FromSql<uint>(), results[i][1].FromSql<string>(), results[i][2].FromSql<string>(), results[i][3].FromSql<string>(),
                        results[i][4].FromSql<bool>(), results[i][5].FromSql<int>());
                }

                return commands;
            }
            else
            {
                return null;
            }
        }
    }
}
