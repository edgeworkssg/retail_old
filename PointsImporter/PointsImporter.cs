using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.IO;
using SubSonic;

namespace PointsImporter
{
    public partial class PointsImporter : Form
    {

        private DataTable dt;
        public PointsImporter()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent();
            OpenDialog.FileName = "";
            OpenDialog.ShowDialog();
            CommonUILib.hideTransparent();
        }

        private void OpenDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                DataTable message = null;
                DataTable ErrorDb;
                //DataSet ds = null;

                FileInfo inf = new FileInfo(OpenDialog.FileName);
                if (inf.Extension.ToLower() == ".xls")
                {
                    if (!ExcelController.ImportExcelXLS(OpenDialog.FileName, out message, true))
                        throw new Exception("");
                }
                if (message == null || message.Rows.Count < 1)
                    throw new Exception("No data inside");
                if (ImportFromDataTable(message, out dt, out ErrorDb))
                {
                    MessageBox.Show("Loaded Successfully");
                }
                else
                {
                    MessageBox.Show("File cannot be imported correctly. Please see the log");
                    frmImportErrorMessage f = new frmImportErrorMessage();
                    f.source = ErrorDb;
                    CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                }
                BindGrid();
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); }

        }

        private DataTable CreateCSVImportErrorMessageDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("row", System.Type.GetType("System.Int32"));
            dt.Columns.Add("membershipno");
            dt.Columns.Add("itemno");
            dt.Columns.Add("timestamp");
            dt.Columns.Add("error");

            return dt;
        }

        public void AddNewImportErrorMessage(int row, string membershipno, string itemno, string timestamp, string error, ref DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["row"] = row;
            dr["membershipno"] = membershipno;
            dr["itemno"] = itemno;
            dr["timestamp"] = timestamp;
            dr["error"] = error;
            dt.Rows.Add(dr);

            return;

        }

        private bool ImportFromDataTable(DataTable source, out DataTable result, out DataTable errorDB)
        {
            result = new DataTable();
            errorDB = new DataTable();
            errorDB = CreateCSVImportErrorMessageDataTable();
           
            try
            {
                bool valid = true;
                int Counter = 1;
                foreach (DataRow dr in source.Rows)
                {
                    string membershipno = dr["MembershipNo"].ToString();
                    string ItemNo = dr["ItemNo"].ToString();
                    int Points = int.Parse(dr["Points"].ToString());
                    MembershipCollection mc = new MembershipCollection();
                    mc.Where(Membership.Columns.MembershipNo, dr["MembershipNo"].ToString());
                    mc.Load();
                    if (mc.Count < 1)
                    {
                        valid = false;
                        AddNewImportErrorMessage(Counter, membershipno, ItemNo, "", "Unable to add Membership Points. MembershipNo Not Found ", ref errorDB);
                    }
                    ItemCollection it = new ItemCollection();
                    it.Where(Item.Columns.ItemNo, ItemNo);
                    it.Load();
                    if (it.Count < 1)
                    {
                        valid = false;
                        AddNewImportErrorMessage(Counter, membershipno, ItemNo, "", "Unable to add Membership Points. ItemNo Not Found ", ref errorDB);
                    }
                    else
                    {
                        Item i = it[0];
                        if (i.Userfld10 != "T" && i.Userfld10 != "D")
                        {
                            valid = false;
                            AddNewImportErrorMessage(Counter, membershipno, ItemNo, "", "Unable to add Membership Points. Item is not point / course package  ", ref errorDB);
                        }
                    }

                    Counter++;
                }

                if (valid)
                { result = source; }
                return valid ;
            }
            catch (Exception ex)
            { Logger.writeLog(ex.Message); return false; }
        }

        private void PointsImporter_Load(object sender, EventArgs e)
        {
            dt = null;
        }

        private void BindGrid()
        {
            dgvPoints.AutoGenerateColumns = false;
            dgvPoints.DataSource = dt;
        }

        private bool SaveData(DataTable source ,out string status)
        {
            try
            {
                status = "";
                QueryCommandCollection cmd = new QueryCommandCollection();
                foreach (DataRow dr in source.Rows)
                {
                    MembershipPointCollection mp = new MembershipPointCollection();
                    mp.Where(MembershipPoint.Columns.MembershipNo, dr["MembershipNo"].ToString());
                    mp.Where(MembershipPoint.Columns.Userfld1, dr["ItemNo"].ToString());
                    mp.Load();
                    if (mp.Count < 1)
                    {
                        Item i = new Item(dr["ItemNo"].ToString());
                        if (i != null && i.ItemName != "")
                        {
                            MembershipPoint memberpoint = new MembershipPoint();
                            memberpoint.MembershipNo = dr["MembershipNo"].ToString();
                            memberpoint.Userfld1 = i.ItemNo;
                            memberpoint.Userfld2 = i.Userfld10;
                            memberpoint.Points = decimal.Parse(dr["Points"].ToString());
                            memberpoint.StartValidPeriod = DateTime.Parse(dr["DateStart"].ToString());
                            memberpoint.EndValidPeriod = memberpoint.StartValidPeriod.AddYears(100);
                            memberpoint.Userfloat1 = decimal.Parse(dr["AmountPerTimes"].ToString());
                            cmd.Add(memberpoint.GetInsertCommand("Point Importer"));
                            
                        }

                    }
                    else
                    {
                        Item i = new Item(dr["ItemNo"].ToString());
                        if (i != null && i.ItemName != "")
                        {
                            MembershipPoint memberpoint = mp[0];
                            memberpoint.MembershipNo = dr["MembershipNo"].ToString();
                            memberpoint.Userfld1 = i.ItemNo;
                            memberpoint.Userfld2 = i.Userfld10;
                            memberpoint.Points = memberpoint.Points + decimal.Parse(dr["Points"].ToString());
                            memberpoint.StartValidPeriod = DateTime.Parse(dr["DateStart"].ToString());
                            memberpoint.EndValidPeriod = memberpoint.StartValidPeriod.AddYears(100);
                            memberpoint.Userfloat1 = decimal.Parse(dr["AmountPerTimes"].ToString());
                            cmd.Add(memberpoint.GetUpdateCommand("Point Importer"));
                        }
                    }
                    PointAllocationLog pl = new PointAllocationLog();
                    pl.AllocationDate = DateTime.Parse(dr["DateStart"].ToString());
                    pl.MembershipNo = dr["MembershipNo"].ToString();
                    pl.PointAllocated = decimal.Parse(dr["Points"].ToString());
                    pl.OrderHdrID = "-";
                    pl.Userfld1 = dr["ItemNo"].ToString();
                    pl.Userfld2 = "Point Importer";
                    cmd.Add(pl.GetInsertCommand("Point Importer"));
                }

                DataService.ExecuteTransaction(cmd);

                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); status = ex.Message;  return false; }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count < 1)
            {
                MessageBox.Show("Cannot Save. No Data Provided");
                return;
            }

            string status = "";
            if (SaveData(dt, out status))
            {
                MessageBox.Show("Data Saved Successfully");

            }
            else
            {
                MessageBox.Show("Save Failed." + status);
            }

        }
    }
}
