using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch.Data
{
    public class SqlTwitchCommand : SqlRow
    {
        public readonly SqlTwitchUser user;
        //
        public SqlTwitchCommand(uint id,SqlTwitchChannel channel , string Command, string Description = null, bool Mod = true, int Time = 0)
                : base(new object[] { id, channel.user.id, Command, Description, Mod, Time })
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
            set
            {
                Set(0, value);
            }
        }
        public uint ChannelId
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
        public string Command
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

        public string Description
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



        public bool Mod
        {
            get
            {
                return Get<bool>(4);
            }
            set
            {
                Set(4, value);
            }
        }

        public int CoolDownInSeconds
        {
            get
            {
                return Get<int>(5);
            }
            set
            {
                Set(5, value);
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
                    //uint id[0], string ChannelId = null[1], string Command[2], string Discription = null[3], string Aliases = null[4], bool Mod = true[5], int CoolDownInSeconds = 0[6]
                    commands[i] = new SqlTwitchCommand(results[i][0].FromSql<uint>(), results[i][1].FromSql<uint>(), results[i][1].FromSql<string>(), results[i][3].FromSql<string>(),
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
