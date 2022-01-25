using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

namespace PowerPOS
{
    public partial class HoldController
    {
        SortedList<string,POSController> allPOS;

        public string HoldFilePrefix = "Hold-";
        public static string HoldLocation
        {
            get
            {
                string _HoldLocation = System.Windows.Forms.Application.StartupPath + "\\Hold";
                
                if (!System.IO.Directory.Exists(_HoldLocation))
                    System.IO.Directory.CreateDirectory(_HoldLocation);
                
                return _HoldLocation;
            }
        }

        private int _MaxHold;
        public int MaxHold
        {
            get { return _MaxHold; }
            set
            {
                _MaxHold = value;
                ReloadAll();
            }
        }

        public HoldController()
        {
            MaxHold = 10;
        }

        public void ReloadAll()
        {
            allPOS = new SortedList<string, POSController>();

            #region *) OBSOLETE - Reason: Now Hold info is saved in database
            //string[] HoldFiles = Directory.GetFiles(HoldLocation);

            //foreach (string OneFile in HoldFiles)
            //{
            //    FileInfo inf = new FileInfo(OneFile);

            //    if (!(inf.Extension == ".bin" && inf.Name.StartsWith(HoldFilePrefix))) continue;

            //    FileStream file = new FileStream(OneFile, FileMode.Open);

            //    BinaryFormatter bf = new BinaryFormatter();
            //    allPOS.Add(inf.Name.Substring(HoldFilePrefix.Length, inf.Name.Length - HoldFilePrefix.Length - inf.Extension.Length), bf.Deserialize(file) as POSController);

            //    file.Close();
            //}
            #endregion

            HoldTransactionCollection holdColl = new HoldTransactionCollection();
            holdColl.Load();
            foreach (HoldTransaction hold in holdColl)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream mStream = new MemoryStream(hold.POSControllerObject))
                {
                    mStream.Position = 0;
                    allPOS.Add(hold.HoldGuid.ToString(), bf.Deserialize(mStream) as POSController);
                }
            }
        }

        public POSController LoadHold(string Index)
        {
            POSController tmpData = allPOS[Index];

            #region *) OBSOLETE - Reason: Now Hold info is saved in database
            //if (tmpData != null && File.Exists(HoldLocation + "\\" + HoldFilePrefix + Index + ".bin"))
            //{
            //    File.Delete(HoldLocation + "\\" + HoldFilePrefix + Index + ".bin");
            //}
            #endregion

            if (tmpData != null)
            {
                HoldTransaction.Destroy(new Guid(Index));
            }

            allPOS.Remove(Index);

            return tmpData;
        }

        public void SaveHold(POSController Value)
        {
            Guid Index = Guid.NewGuid();
            Membership member = new Membership();
            if (Value.MembershipApplied()) 
                member = Value.GetMemberInfo();

            #region *) OBSOLETE - Reason: Now Hold info is saved in database
            //string FileLocation = HoldLocation + "\\" + HoldFilePrefix + Index + ".bin";

            //if (File.Exists(FileLocation))
            //{
            //    File.Delete(FileLocation);
            //}
            //Stream a = File.OpenWrite(FileLocation);

            //BinaryFormatter bf = new BinaryFormatter();
            //bf.Serialize(a, Value);
            //a.Close();
            #endregion

            byte[] orderData = null;
            using (MemoryStream mStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(mStream, Value);
                mStream.Seek(0, 0);
                orderData = mStream.ToArray();
            }

            HoldTransaction hold = new HoldTransaction();
            hold.HoldGuid = Index;
            hold.AppTime = DateTime.Now;
            hold.MembershipNo = member.MembershipNo;
            hold.MembershipName = member.NameToAppear;
            hold.Nric = member.Nric;
            hold.LineInfo = Value.GetLineInfoOfFirstLine();
            hold.POSControllerObject = orderData;
            hold.Deleted = false;
            hold.IsNew = true;
            hold.Save();
            
            allPOS.Add(Index.ToString(), Value);
        }

        public DataTable ToDataTable()
        {
            DataTable Output = new DataTable();
            Output.Columns.Add("HoldNo", Type.GetType("System.String"));
            Output.Columns.Add("AppTime", Type.GetType("System.DateTime"));
            Output.Columns.Add("MembershipNo", Type.GetType("System.String"));
            Output.Columns.Add("MembershipName", Type.GetType("System.String"));
            Output.Columns.Add("    ", Type.GetType("System.String"));
            Output.Columns.Add("LineInfo", Type.GetType("System.String"));
            Output.Columns.Add("TotalAmount", Type.GetType("System.Decimal"));
            string status = "";
            foreach (string oneKey in allPOS.Keys)
            {
                DataRow Rw = Output.NewRow();

                if (allPOS[oneKey] == null)
                {
                    Rw[0] = oneKey;
                    Rw[1] = null;
                    Rw[2] = "<<NEW>>";
                    Rw[3] = "<<NEW>>";
                    Rw[4] = "<<NEW>>";
                    Rw[5] = "<<NEW>>";
                    Rw[6] = "0";
                }
                else
                {
                    Membership ThisPerson = allPOS[oneKey].GetMemberInfo();

                    Rw[0] = oneKey;
                    Rw[1] = allPOS[oneKey].GetOrderDate();
                    Rw[2] = allPOS[oneKey].MembershipApplied() ? ThisPerson.MembershipNo : "<<NEW>>";
                    Rw[3] = allPOS[oneKey].MembershipApplied() ? ThisPerson.NameToAppear : "<<NEW>>";
                    Rw[4] = allPOS[oneKey].MembershipApplied() ? (ThisPerson.Nric ?? "") : "<<NEW>>";
                    Rw[5] = allPOS[oneKey].GetLineInfoOfFirstLine();
                    Rw[6] = allPOS[oneKey].CalculateTotalAmount(out status);
                }
                Output.Rows.Add(Rw);
            }

            return Output;
        }
    }
}
