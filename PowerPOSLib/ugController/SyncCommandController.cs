using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PowerPOS.Container;
using SubSonic;
using System.Data;
using System.IO.Compression;

namespace PowerPOS
{
    public partial class SyncCommandController
    {
        public static void DoInitialization()
        {
            SetupDatabase();
        }
        private static void SetupDatabase()
        {
            string SQLString =
                "IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SyncCommand]') AND type in (N'U')) " +
                    "CREATE TABLE [dbo].[SyncCommand]( " +
                        "[SyncCommandID] [uniqueidentifier] NOT NULL, " +
                        "[Description] [varchar](100) NOT NULL, " +
                        "[TheCommand] [image] NOT NULL, " +
                        "[CreatedOn] [datetime] NOT NULL, " +
                        "[CreatedBy] [varchar](50) NOT NULL, " +
                        "[ExecutedOn] [datetime] NULL, " +
                        "[ExecutedBy] [varchar](50) NULL, " +
                        "[Remarks] [nchar](20) NULL " +
                    ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ";

            DataService.ExecuteQuery(new QueryCommand(SQLString));
        }

        public static QueryCommand AddCommand(string Description, QueryCommand Cmd, string UserID)
        {
            QueryCommandCollection Cmds = new QueryCommandCollection();
            Cmds.Add(Cmd);

            return AddCommand(Description, Cmds, UserID);
        }
        public static QueryCommand AddCommand(string Description, QueryCommandCollection Cmd, string UserID)
        {
            #region *) Initialization: Clean the Command Collection from null value
            /* Reason to do this is to make the size to be transfered smaller 
             * and to prevent error in the future */

            for (int Counter = Cmd.Count - 1; Counter >= 0; Counter--)
                if (Cmd[Counter] == null)
                    Cmd.RemoveAt(Counter);
            #endregion

            SyncCommand Inst = new SyncCommand();
            Inst.SyncCommandID = Guid.NewGuid();
            Inst.Description = Description;
            Inst.CreatedBy = UserID;
            Inst.CreatedOn = DateTime.Now;
            Inst.ExecutedBy = null;
            Inst.ExecutedOn = null;
            Inst.QueryCommands = Cmd;

            return Inst.GetSaveCommand();
        }
        public static QueryCommand CopyCommand(SyncCommand Cmd)
        {
            SyncCommand Inst = new SyncCommand();
            Inst.SyncCommandID = Cmd.SyncCommandID;
            Inst.Description = Cmd.Description;
            Inst.CreatedBy = Cmd.CreatedBy;
            Inst.CreatedOn = Cmd.CreatedOn;
            Inst.ExecutedBy = null;
            Inst.ExecutedOn = null;
            Inst.QueryCommands = Cmd.QueryCommands;

            return Inst.GetSaveCommand();
        }

        public static SyncCommand GetCommand(Guid SyncCommandID)
        {
            SyncCommand Inst = new SyncCommand(SyncCommandID);

            if (Inst == null || Inst.IsNew) return null;
            
            return Inst;
        }

        public static List<string> GetUnexecutedCommandIDs()
        {
            List<string> Rst = new List<string>();

            string SQLString = "SELECT SyncCommandID FROM SyncCommand WHERE ExecutedOn IS NULL";
            IDataReader Rdr = DataService.GetReader(new QueryCommand(SQLString));
            while (Rdr.Read())
            {
                object OneSyncCommandID = Rdr[0];
                if (OneSyncCommandID != null)
                    Rst.Add(OneSyncCommandID.ToString());
            }

            return Rst;
        }

        public static void ExecuteCommands()
        {
            List<string> CommandIDList = GetUnexecutedCommandIDs();
            foreach (string oneCommandID in CommandIDList)
            {
                SyncCommand ToBeExecuted = GetCommand(new Guid(oneCommandID));
                ToBeExecuted.ExecuteCommand("SYSTEM");
            }
        }
    }
}
