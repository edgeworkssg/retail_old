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
    public partial class SyncCommand
    {
        public QueryCommandCollection QueryCommands
        {
            get
            {
                if (TheCommand == null) return null;

                MemoryStream ms2 = new MemoryStream();
                byte[] buf = (byte[])TheCommand;
                ms2.Write(buf, 0, buf.Length);
                ms2.Seek(0, 0);

                BinaryFormatter b = new BinaryFormatter();
                object obj = b.Deserialize(ms2);

                ms2.Close();

                if (!(obj is QueryCommandCollection)) return null;

                return (QueryCommandCollection)obj;
            }
            set 
            {
                if (value == null) return;

                MemoryStream a = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(a, value);
                a.Seek(0, 0);
                TheCommand = a.ToArray();
                a.Close();
            }
        }

        public void ExecuteCommand(string UserID)
        {
            ExecutedBy = "";
            ExecutedOn = DateTime.Now;

            QueryCommandCollection SaveCommands = QueryCommands;
            SaveCommands.Add(GetSaveCommand(UserID));

            DataService.ExecuteTransaction(SaveCommands);

            MarkClean();
        }
    }
}
