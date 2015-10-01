using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Library.Twitch.Data
{
    class SqlTwitchCommand : SqlRow {
        SqlTwitchChannel channel; 
        // creating the varibles you what to use
        public SqlTwitchCommand( uint id, SqlTwitchChannel channel, string Command, string Description = null, string Aliases = null, bool Mod = true, int TimeCooldownInSeconds = 0)
                : base(new object[] { id, channel.user.id, Command, Description, Aliases, Mod, TimeCooldownInSeconds })
        {
            this.channel = channel;
        }
        // Sellecting the table you want to interact with
        internal static readonly SqlTable _table = new SqlTable("twitch_commands");
        
        //
        public override SqlTable table
        {
            get
            {
                return _table;
            }
        }
        //get the data from the DB or write it to the DB
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

        public uint ChannelUserId
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

        public string Aliases
        {
            get
            {
                return Get<string>(4);
            }
            set
            {
                Set(4, value);
            }
        }

        public bool Mod
        {
            get
            {
                return Get<bool>(5);
            }
            set
            {
                Set(5, value);
            }
        }

        public int TimeCooldownInSeconds
        {
            get
            {
                return Get<int>(6);
            }
            set
            {
                Set(6, value);
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
                    commands[i] = new SqlTwitchCommand(results[i][0].FromSql<uint>(),/*this one gives me an error*/results[i][1].FromSql<uint>(), results[i][2].FromSql<string>(), results[i][3].FromSql<string>(), results[i][4].FromSql<string>(),
                        results[i][5].FromSql<bool>(), results[i][6].FromSql<int>());
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
