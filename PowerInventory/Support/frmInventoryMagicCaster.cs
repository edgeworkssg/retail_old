using SubSonic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace PowerInventory.Support
{
    public partial class frmInventoryMagicCaster : Form
    {
        InventoryGuild No1Guild;

        public frmInventoryMagicCaster()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            Caster.RunWorkerAsync();
        }

        private void Caster_DoWork(object sender, DoWorkEventArgs e)
        {
            IDataReader Rdr;
            string sqlString;

            Caster.ReportProgress(0, "Generating all list of Items");

            List<string> Items = new List<string>();
            #region *) Fetch: Generate All Items
            sqlString =
                "DECLARE @Search NVARCHAR(MAX) " +
                "SET @Search = '" + tSearch.Text + "' " +
                "SELECT ItemNo FROM Item INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                "WHERE ItemDepartmentID <> 'SYSTEM' AND IsInInventory = 1 " +
                    "AND ( "+
                        "ItemNo LIKE '%' + @Search + '%' "+
                        "OR Barcode LIKE '%' + @Search + '%' " +
                        "OR ItemName LIKE '%' + @Search + '%' " +
                        "OR Item.CategoryName LIKE '%' + @Search + '%' " +
                        "OR ItemDepartmentID LIKE '%' + @Search + '%' " +
                        "OR ISNULL(Attributes1,'') LIKE '%' + @Search + '%' " +
                        "OR ISNULL(Attributes2,'') LIKE '%' + @Search + '%' " +
                        "OR ISNULL(Attributes3,'') LIKE '%' + @Search + '%' " +
                        "OR ISNULL(Attributes4,'') LIKE '%' + @Search + '%' " +
                        "OR ISNULL(Attributes5,'') LIKE '%' + @Search + '%' " +
                        "OR ISNULL(Attributes6,'') LIKE '%' + @Search + '%' " +
                        "OR ISNULL(Attributes7,'') LIKE '%' + @Search + '%' " +
                        "OR ISNULL(Attributes8,'') LIKE '%' + @Search + '%' " +
                    ") ";
            Rdr = DataService.GetReader(new QueryCommand(sqlString));
            while (Rdr.Read())
                Items.Add(Rdr[0].ToString());
            #endregion

            Caster.ReportProgress(0, "Generating all list of Inventory Locations");

            List<string> ILocs = new List<string>();
            #region *) Fetch: Generate All Inventory Locations
            sqlString = "SELECT InventoryLocationID FROM InventoryLocation";
            Rdr = DataService.GetReader(new QueryCommand(sqlString));
            while (Rdr.Read())
                ILocs.Add(Rdr[0].ToString());
            #endregion

            InventoryGuild TempGuild = new InventoryGuild();

            for (int iCounter = 0; iCounter < Items.Count; iCounter++)
            {
                for (int lCounter = 0; lCounter < ILocs.Count; lCounter++)
                {
                    if (iCounter + 1 == Items.Count && lCounter + 1 == ILocs.Count)
                        Caster.ReportProgress(99, "Item No " + Items[iCounter] + " | Location " + ILocs[lCounter]);
                    else
                        Caster.ReportProgress((int)decimal.Floor(100 * ((decimal)(iCounter * ILocs.Count) + (lCounter + 1)) / (Items.Count * ILocs.Count)), "Item No " + Items[iCounter] + " | Location " + ILocs[lCounter]);

                    TempGuild.RegisterMage(Items[iCounter], int.Parse(ILocs[lCounter]));
                }
            }

            Caster.ReportProgress(99, "Generating report. Please wait as this process may take a few minutes");

            e.Result = TempGuild;
        }

        private void Caster_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                No1Guild = (InventoryGuild)e.Result;

                pbValidation.Value = 0;
                lblValidation.Text = "DONE -- " + DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");

                panel1.Enabled = true;
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private void Caster_DoWork2(object sender, DoWorkEventArgs e)
        {
            QueryCommandCollection TransactionExecutor;
            string sqlString = "";
            IDataReader Rdr;

            #region *) Validation: No pending stock take
            sqlString = "SELECT COUNT(*) FROM StockTake WHERE IsAdjusted = 0";
            if (((int)DataService.ExecuteScalar(new QueryCommand(sqlString))) > 0)
                throw new Exception("(error)There are some pending stock take waiting to be cleared.\nPlease either confirm the stock take or delete it");
            #endregion

            #region *) Backup database
            DbUtilityController.DoDBBackUp(Environment.CurrentDirectory + "\\Backup\\", "Before Casting Inventory Magic");
            #endregion

            #region *) Copy the Table
            TransactionExecutor = new QueryCommandCollection();

            sqlString = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestInventoryHdr]') AND type in (N'U')) DROP TABLE [dbo].[TestInventoryHdr] ";
            TransactionExecutor.Add(new QueryCommand(sqlString));

            sqlString = "SELECT * INTO TestInventoryHdr FROM InventoryHdr ";
            TransactionExecutor.Add(new QueryCommand(sqlString));

            DataService.ExecuteTransaction(TransactionExecutor);
            #endregion

            sqlString = "DELETE FROM InventoryDet WHERE Remark LIKE 'Stock Take Adj. Id:%'";
            DataService.ExecuteQuery(new QueryCommand(sqlString));

            sqlString = "DELETE FROM InventoryHdr WHERE MovementType = 'Stock Out' AND StockOutReasonID = 0";
            DataService.ExecuteQuery(new QueryCommand(sqlString));

            sqlString = "UPDATE OrderDet SET InventoryHdrRefNo = ''";
            DataService.ExecuteQuery(new QueryCommand(sqlString));

            sqlString = "UPDATE OrderDet SET InventoryHdrRefNo = 'NONINVENTORY' WHERE ItemNo IN (SELECT ItemNo FROM Item WHERE CategoryName = 'SYSTEM' OR IsInInventory = 0)";
            DataService.ExecuteQuery(new QueryCommand(sqlString));

            sqlString = "DELETE FROM InventoryHdr WHERE InventoryHdrRefNo NOT IN (SELECT InventoryHdrRefNo FROM InventoryDet)";
            DataService.ExecuteQuery(new QueryCommand(sqlString));

            sqlString = "UPDATE InventoryDet SET StockInRefNo = NULL";
            DataService.ExecuteQuery(new QueryCommand(sqlString));

            List<string> ItemNos = new List<string>();
            sqlString = "SELECT DISTINCT ItemNo FROM Item WHERE CategoryName <> 'SYSTEM' AND IsInInventory = 1";
            Rdr = DataService.GetReader(new QueryCommand(sqlString));
            while (Rdr.Read())
            {
                ItemNos.Add(Rdr[0].ToString());
            }

            List<int> Inventories = new List<int>();
            sqlString = "SELECT DISTINCT InventoryLocationID FROM InventoryLocation";
            Rdr = DataService.GetReader(new QueryCommand(sqlString));
            while (Rdr.Read())
            {
                Inventories.Add((int)Rdr[0]);
            }

            for (int ItemNoCounter = 0; ItemNoCounter < ItemNos.Count; ItemNoCounter++)
            {
                for (int InventoryCounter = 0; InventoryCounter < Inventories.Count; InventoryCounter++)
                {
                    string ItemNo = ItemNos[ItemNoCounter];
                    int Inventory = Inventories[InventoryCounter];

                }
            }
        }

        private void Caster_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbValidation.Value = e.ProgressPercentage;
            lblValidation.Text = e.UserState.ToString();
        }

        private void btnAllOutMagic_Click(object sender, EventArgs e)
        {
            cbErrorStockOut.Checked = true;
            cbNormalTransaction.Checked = true;
            cbUndeductedSales.Checked = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            progressBar1.Show();
            progressBar1.Value = 100;

            Search.RunWorkerAsync(No1Guild);
        }

        private void Search_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = No1Guild.CastMagix("", cbNormalTransaction.Checked, cbUndeductedSales.Checked, cbErrorStockOut.Checked);
        }

        private void Search_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            InventoryMagix1.SetDataSource((DataTable)e.Result);
            crystalReportViewer1.ReportSource = InventoryMagix1;
            crystalReportViewer1.RefreshReport();

            progressBar1.Hide();
        }
    }
}
