using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BarcodePrinter;
using WinPowerPOS.OrderForms;
using PowerPOS;
using BarcodePrinter.Models;
using PowerPOS.Container;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
using System.IO;
using WinPowerPOS.Barcode_Printer;
//using DocumentFormat.OpenXml;

namespace WinPowerPOS.BarcodePrinter
{
    public partial class frmPrint : Form
    {
        private bool AllowEdit = false;
        private int defaultQty = 0;

        public frmPrint()
        {
            InitializeComponent();
        }

        private void frmPrint_Load(object sender, EventArgs e)
        {
            /*string SQLString =
                "Select * From ( "  +
                "SELECT ItemName, ItemNo, Barcode, RetailPrice, Attributes1, Attributes2, Attributes3 " +
                ",Attributes4, Attributes5, Attributes6, Attributes7, Attributes8, Barcode as BarcodeText " +
                ",Category.CategoryName, Category.ItemDepartmentID " + 
                "FROM Item " +
                    "INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                    "INNER JOIN ItemDepartment ON Category.ItemDepartmentID = ItemDepartment.ItemDepartmentID " +
                //"WHERE ItemDepartment.ItemDepartmentID <> 'SYSTEM' " +
                "WHERE Category.CategoryName <> 'SYSTEM' AND Category.Deleted = 0 AND Item.Deleted = 0 " +
                @"UNION ALL
                SELECT PromoCampaignName as ItemName, ISNULL(PromoCode, CAST(ph.PromoCampaignHdrID AS VARCHAR(MAX))) as ItemNo,
                ISNULL(ph.Barcode,'') as Barcode, SUM(ISNULL(pd.PromoPrice,0)) RetailPrice, '' Attributes1, 
                '' Attributes2, '' Attributes3, ''Attributes4, '' Attributes5, 
                '' Attributes6, '' Attributes7, '' Attributes8, ISNULL(ph.Barcode,'') as BarcodeText
                , 'Promo' as CategoryName, 'Promo' as ItemDepartmentID 
                FROM PromoCampaignHdr ph, PromoCampaignDet pd
                where ph.PromoCampaignHdrID = pd.PromoCampaignHdrID AND GETDATE() between ph.DateFrom and ph.DateTo 
                group by PromoCampaignName, ISNULL(PromoCode, CAST(ph.PromoCampaignHdrID AS VARCHAR(MAX))),
                ISNULL(ph.Barcode,'') " +" ) a " +
                "ORDER BY ItemName";

            DataTable dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));
            dt.Columns.Add("Qty", Type.GetType("System.Int32"));
            for (int Counter = 0; Counter < dt.Rows.Count; Counter++)
                dt.Rows[Counter]["Qty"] = 0;*/

            //Tabel.AutoGenerateColumns = false;
            //Tabel.DataSource = dt;

            dgAttributes1.Visible = false;
            dgAttributes2.Visible = false;
            dgAttributes3.Visible = false;
            dgAttributes4.Visible = false;
            dgAttributes5.Visible = false;
            dgAttributes6.Visible = false;
            dgAttributes7.Visible = false;
            dgAttributes8.Visible = false;

            #region *) Load Attributes Value
            string SQLString = "SELECT * FROM AttributesLabel";
            IDataReader Rdr = SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString));
            while (Rdr.Read())
            {
                switch (Rdr.GetInt32(0))
                {
                    case 1: dgAttributes1.HeaderText = Rdr.GetString(1); dgAttributes1.Visible = true; break;
                    case 2: dgAttributes2.HeaderText = Rdr.GetString(1); dgAttributes2.Visible = true; break;
                    case 3: dgAttributes3.HeaderText = Rdr.GetString(1); dgAttributes3.Visible = true; break;
                    case 4: dgAttributes4.HeaderText = Rdr.GetString(1); dgAttributes4.Visible = true; break;
                    case 5: dgAttributes5.HeaderText = Rdr.GetString(1); dgAttributes5.Visible = true; break;
                    case 6: dgAttributes6.HeaderText = Rdr.GetString(1); dgAttributes6.Visible = true; break;
                    case 7: dgAttributes7.HeaderText = Rdr.GetString(1); dgAttributes7.Visible = true; break;
                    case 8: dgAttributes8.HeaderText = Rdr.GetString(1); dgAttributes8.Visible = true; break;
                }
            }
            #endregion

            //Tabel.SortedColumn = Tabel.Columns[0];
            //Tabel.Sort(Tabel.Columns[0], ListSortDirection.Ascending);

            // Check If Copy Custom Qty Option is Active
            dgCopyCustomQty.Visible = false;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.CustomQtyActive), false))
            {
                dgCopyCustomQty.Visible = true;
            }

            #region Load ComboBox Category
            cbCategory.Items.Clear();
            string sqlString = "Select CategoryName from category where deleted = 0 and CategoryName <> 'SYSTEM'";
            DataTable dtCategory = new DataTable();
            dtCategory.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sqlString)));
            cbCategory.Items.Add("ALL");
            cbCategory.Items.Add("PROMO");
            foreach (DataRow dr in dtCategory.Rows)
            {
                cbCategory.Items.Add(dr[0].ToString());
            }
            cbCategory.SelectedIndex = 0;
            #endregion

            #region *) Load Tool Tip
            this.toolTip1.SetToolTip(this.tstxtSearch.Control, this.tstxtSearch.ToolTipText);

            #endregion
        }

        private void BindGrid()
        {
            string categoryFilter = cbCategory.Items[cbCategory.SelectedIndex].ToString();



            string SQLString =
                "Select * From ( " +
                "SELECT Item.ItemName, Item.ItemNo, Item.Barcode, ab.Barcode as AlternateBarcode, Item.RetailPrice, Item.Attributes1, Item.Attributes2, Item.Attributes3 " +
                ",Item.Attributes4, Item.Attributes5, Item.Attributes6, Item.Attributes7, Item.Attributes8, Item.Barcode as BarcodeText " +
                ",Category.CategoryName, Category.ItemDepartmentID " +
                "FROM Item " +
                    "INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                    "INNER JOIN ItemDepartment ON Category.ItemDepartmentID = ItemDepartment.ItemDepartmentID " + 
                    "LEFT JOIN AlternateBarcode ab ON ab.ItemNo = Item.ItemNo and ISNULL(ab.Deleted,0) = 0 " + 
                //"WHERE ItemDepartment.ItemDepartmentID <> 'SYSTEM' " +
                "WHERE Category.CategoryName <> 'SYSTEM' AND Category.Deleted = 0 AND Item.Deleted = 0 ";
            if (!String.IsNullOrEmpty(categoryFilter) && categoryFilter != "ALL")
            {
                SQLString += " and Category.CategoryName = '" + categoryFilter + "' ";
            }

            if (!String.IsNullOrEmpty(categoryFilter) && (categoryFilter == "ALL" || categoryFilter == "PROMO"))
            {
                SQLString +=
                @"UNION ALL
                SELECT PromoCampaignName as ItemName, ISNULL(PromoCode, CAST(ph.PromoCampaignHdrID AS VARCHAR(MAX))) as ItemNo,
                ISNULL(ph.Barcode,'') as Barcode, '' as AlternateBarcode, SUM(ISNULL(pd.PromoPrice,0)) RetailPrice, '' Attributes1, 
                '' Attributes2, '' Attributes3, ''Attributes4, '' Attributes5, 
                '' Attributes6, '' Attributes7, '' Attributes8, ISNULL(ph.Barcode,'') as BarcodeText
                , 'Promo' as CategoryName, 'Promo' as ItemDepartmentID 
                FROM PromoCampaignHdr ph, PromoCampaignDet pd
                where ph.PromoCampaignHdrID = pd.PromoCampaignHdrID AND GETDATE() between ph.DateFrom and ph.DateTo 
                group by PromoCampaignName, ISNULL(PromoCode, CAST(ph.PromoCampaignHdrID AS VARCHAR(MAX))),
                ISNULL(ph.Barcode,'') " + " ) a " +
                "ORDER BY ItemName";
            }
            else
            {
                SQLString += " ) a ";
                SQLString += "ORDER BY ItemName";
            }


            DataTable dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));
            dt.Columns.Add("Qty", Type.GetType("System.Int32"));
            for (int Counter = 0; Counter < dt.Rows.Count; Counter++)
                dt.Rows[Counter]["Qty"] = defaultQty;

            Tabel.AutoGenerateColumns = false;
            Tabel.DataSource = dt;

            Tabel.Sort(Tabel.Columns[0], ListSortDirection.Ascending);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                #region *) Load: Try to load template
                string URL = AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.Template);
                if (URL.StartsWith("\\")) URL = Application.StartupPath.TrimEnd('\\') + URL;
                #endregion

                #region *) Update: Make sure the changes is commited
                Tabel.CommitEdit(DataGridViewDataErrorContexts.Commit);
                #endregion

                List<DataRow> dgvDataSource = (from r in this.Tabel.Rows.Cast<DataGridViewRow>()
                                               where int.Parse(r.Cells[dgQuantity.Name].Value.ToString()) > 0
                                               select (r.DataBoundItem as DataRowView).Row).ToList();

                BarcodePrinterController prnt = new BarcodePrinterController(new Uri(URL));

                Tabel.Sort(Tabel.Columns[0], ListSortDirection.Ascending);

                prnt.PrintBarcodeLabel(dgvDataSource, Tabel.SortedColumn.DataPropertyName, Tabel.SortOrder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ": \n" + ex.StackTrace);
            }

        }

        private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Tabel.DataSource == null) return;

                DataTable dt = (DataTable)Tabel.DataSource;

                for (int Counter = 0; Counter < dt.Rows.Count; Counter++)
                    dt.Rows[Counter]["Qty"] = 0;

                Tabel.Refresh();
            }
            catch
            {
            }

        }

        private void Tabel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (e.ColumnIndex == Tabel.Columns[dgPlus.Name].Index)
            {
                Tabel[dgQuantity.Name, e.RowIndex].Value = int.Parse(Tabel[dgQuantity.Name, e.RowIndex].Value.ToString()) + 1;
            }
            else if (e.ColumnIndex == Tabel.Columns[dgMinus.Name].Index)
            {
                Tabel[dgQuantity.Name, e.RowIndex].Value = int.Parse(Tabel[dgQuantity.Name, e.RowIndex].Value.ToString()) - 1;
            }

            // When Button Copy Custom Qty is Clicked
            else if (e.ColumnIndex == Tabel.Columns[dgCopyCustomQty.Name].Index)
            {
                Tabel[dgQuantity.Name, e.RowIndex].Value = Tabel[AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.CustomQtyOrigin), e.RowIndex].Value;
            }

            Tabel.RefreshEdit();
        }

        private void Tabel_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == Tabel.Columns[dgQuantity.Name].Index)
            {
                MessageBox.Show("Cannot read the quantity value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void editTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmPreview inst = new frmPreview())
            {
                inst.ShowDialog();
            }
        }

        private void loadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openTemplateDialog.ShowDialog();
        }

        private void openTemplateDialog_FileOk(object sender, CancelEventArgs e)
        {
            string FileName = openTemplateDialog.FileName;
            if (FileName.StartsWith(Application.StartupPath))
                FileName = FileName.Substring(Application.StartupPath.Length);

            AppSetting.SetSetting(AppSetting.SettingsName.BarcodePrinter.Template, FileName);
            //Properties.Settings.Default.Template = FileName;
            //Properties.Settings.Default.Save();
        }

        private void fromLastGoodsReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string status;

            DataTable dt = new DataTable();
            if (PointOfSaleInfo.IntegrateWithInventory)
            {
                dt = ReportController.GetGoodsReceiveList("", out status);
            }
            else
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                byte[] data = ws.GetGoodsReceiveList("", out status);
                DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                if (myDataSet.Tables.Count > 0)
                {
                    dt = myDataSet.Tables[0];
                    //dgvPurchase.Refresh();
                }
            }

            dt.Columns.Add("Qty", Type.GetType("System.Int32"));
            for (int Counter = 0; Counter < dt.Rows.Count; Counter++)
            {
                dt.Rows[Counter]["Qty"] = dt.Rows[Counter]["Quantity"];
            }

            Tabel.AutoGenerateColumns = false;
            Tabel.DataSource = dt;

            Tabel.Sort(Tabel.Columns[0], ListSortDirection.Ascending);
        }

        private void Tabel_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (AllowEdit)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "\\d+"))
                {
                    if (e.KeyChar != (char)8)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void Tabel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Tabel.EditingControlShowing += Tabel_EditingControlShowing;
            if (e.ColumnIndex == 0)
            {
                AllowEdit = true;
                //Tabel.Editi
            }
            else
            {
                AllowEdit = false;
                //Tabel.EditingControlShowing -=Tabel_EditingControlShowing;
            }
            Tabel.Refresh();
        }

        private void tstxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (string.IsNullOrEmpty(this.tstxtSearch.Text))
                {
                    ProcessRefreshDataSource(SortByColumn.ItemName, ListSortDirection.Ascending);
                }
                else
                {
                    ProcessSearch(this.tstxtSearch.Text);
                }
            }
        }

        private void ProcessSearch(string searchText)
        {
            var dataSource = (from dr in Tabel.Rows.Cast<DataGridViewRow>()
                              select new RowModel
                              {
                                  Qty = dr.Cells[dgQuantity.Name].Value,
                                  ItemName = dr.Cells[dgItemName.Name].Value,
                                  ItemNo = dr.Cells[dgItemNo.Name].Value,
                                  CategoryName = dr.Cells[dgCategory.Name].Value,
                                  ItemDepartmentID = dr.Cells[dgDepartment.Name].Value,
                                  Barcode = dr.Cells[dgBarcode.Name].Value,
                                  AlternateBarcode = dr.Cells[dgAlternateBarcode.Name].Value,
                                  RetailPrice = dr.Cells[dgPrice.Name].Value,
                                  Attributes1 = dr.Cells[dgAttributes1.Name].Value,
                                  Attributes2 = dr.Cells[dgAttributes2.Name].Value,
                                  Attributes3 = dr.Cells[dgAttributes3.Name].Value,
                                  Attributes4 = dr.Cells[dgAttributes4.Name].Value,
                                  Attributes5 = dr.Cells[dgAttributes5.Name].Value,
                                  Attributes6 = dr.Cells[dgAttributes6.Name].Value,
                                  Attributes7 = dr.Cells[dgAttributes7.Name].Value,
                                  Attributes8 = dr.Cells[dgAttributes8.Name].Value,
                                  BarcodeText = dr.Cells[dgBarcodeText.Name].Value,
                                  MatchLevel =
                                  ((dr.Cells[dgItemName.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgItemNo.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgBarcode.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAlternateBarcode.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgDepartment.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgCategory.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAttributes1.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAttributes2.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAttributes3.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAttributes4.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAttributes5.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAttributes6.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAttributes7.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0) +
                                   (dr.Cells[dgAttributes8.Name].Value.ToString().ToLower().Contains(searchText.ToLower()) ? 1 : 0))
                              }).ToList();

            var orderedDS = (from dr in dataSource.OrderByDescending(x => x.MatchLevel).ToList()
                             orderby dr.MatchLevel descending
                             select dr).ToList();

            var countRow = (from dr in orderedDS
                            where dr.MatchLevel > 0
                            select dr).ToList().Count;

            DataTable dt = PowerPOSLib.Helper.UtilityHelper.ToDataTable<RowModel>(orderedDS);

            Tabel.DataSource = dt;

            foreach (DataGridViewRow item in Tabel.Rows)
            {
                if (countRow > 0)
                {
                    item.DefaultCellStyle.BackColor = (Color)ColorTranslator.FromHtml("#f5ffe7");
                }
                countRow--;
            }
        }

        private void ProcessRefreshDataSource(SortByColumn column, ListSortDirection sortDirection)
        {
            var dataSource = (from dr in Tabel.Rows.Cast<DataGridViewRow>()
                              select new RowModel
                              {
                                  Qty = dr.Cells[dgQuantity.Name].Value,
                                  ItemName = dr.Cells[dgItemName.Name].Value,
                                  ItemNo = dr.Cells[dgItemNo.Name].Value,
                                  CategoryName = dr.Cells[dgCategory.Name].Value,
                                  ItemDepartmentID = dr.Cells[dgDepartment.Name].Value,
                                  Barcode = dr.Cells[dgBarcode.Name].Value,
                                  AlternateBarcode = dr.Cells[dgAlternateBarcode.Name].Value,
                                  RetailPrice = dr.Cells[dgPrice.Name].Value,
                                  Attributes1 = dr.Cells[dgAttributes1.Name].Value,
                                  Attributes2 = dr.Cells[dgAttributes2.Name].Value,
                                  Attributes3 = dr.Cells[dgAttributes3.Name].Value,
                                  Attributes4 = dr.Cells[dgAttributes4.Name].Value,
                                  Attributes5 = dr.Cells[dgAttributes5.Name].Value,
                                  Attributes6 = dr.Cells[dgAttributes6.Name].Value,
                                  Attributes7 = dr.Cells[dgAttributes7.Name].Value,
                                  Attributes8 = dr.Cells[dgAttributes8.Name].Value,
                                  BarcodeText = dr.Cells[dgBarcodeText.Name].Value
                              }).ToList();

            switch (sortDirection)
            {
                case ListSortDirection.Ascending:
                    switch (column)
                    {
                        case SortByColumn.ItemName:
                            dataSource = dataSource.OrderBy(x => x.ItemName).ToList();
                            break;
                        case SortByColumn.ItemNo:
                            dataSource = dataSource.OrderBy(x => x.ItemNo).ToList();
                            break;
                        case SortByColumn.Barcode:
                            dataSource = dataSource.OrderBy(x => x.Barcode).ToList();
                            break;
                        case SortByColumn.Department:
                            dataSource = dataSource.OrderBy(x => x.ItemDepartmentID).ToList();
                            break;
                        case SortByColumn.Category:
                            dataSource = dataSource.OrderBy(x => x.CategoryName).ToList();
                            break;
                        case SortByColumn.Qty:
                            dataSource = dataSource.OrderBy(x => x.Qty).ToList();
                            break;
                        default:
                            dataSource = dataSource.ToList();
                            break;
                    }
                    break;
                case ListSortDirection.Descending:
                    switch (column)
                    {
                        case SortByColumn.ItemName:
                            dataSource = dataSource.OrderByDescending(x => x.ItemName).ToList();
                            break;
                        case SortByColumn.ItemNo:
                            dataSource = dataSource.OrderByDescending(x => x.ItemNo).ToList();
                            break;
                        case SortByColumn.Barcode:
                            dataSource = dataSource.OrderByDescending(x => x.Barcode).ToList();
                            break;
                        case SortByColumn.Department:
                            dataSource = dataSource.OrderByDescending(x => x.ItemDepartmentID).ToList();
                            break;
                        case SortByColumn.Category:
                            dataSource = dataSource.OrderByDescending(x => x.CategoryName).ToList();
                            break;
                        case SortByColumn.Qty:
                            dataSource = dataSource.OrderByDescending(x => x.Qty).ToList();
                            break;
                        default:
                            dataSource = dataSource.ToList();
                            break;
                    }
                    break;
                default:
                    break;
            }

            DataTable dt = PowerPOSLib.Helper.UtilityHelper.ToDataTable<RowModel>(dataSource);

            Tabel.DataSource = dt;
        }

        private void tstxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tstxtSearch.Text))
            {
                ProcessRefreshDataSource(SortByColumn.ItemName, ListSortDirection.Ascending);
            }
        }

        private void fromGoodsReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGoodsReceiveList fmg = new frmGoodsReceiveList();
            DialogResult res = fmg.ShowDialog();
            if (res == DialogResult.Yes)
            {
                string status = "";
                string refno = fmg.RefNo;

                DataTable dt = new DataTable();
                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    dt = ReportController.GetGoodsReceiveList(refno, out status);
                }
                else
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.GetGoodsReceiveList(refno, out status);
                    DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                    if (myDataSet.Tables.Count > 0)
                    {
                        dt = myDataSet.Tables[0];
                        //dgvPurchase.Refresh();
                    }
                }

                dt.Columns.Add("Qty", Type.GetType("System.Int32"));
                for (int Counter = 0; Counter < dt.Rows.Count; Counter++)
                {
                    dt.Rows[Counter]["Qty"] = dt.Rows[Counter]["Quantity"];
                }

                Tabel.AutoGenerateColumns = false;
                Tabel.DataSource = dt;

                Tabel.Sort(Tabel.Columns[0], ListSortDirection.Ascending);
            }
            fmg.Dispose();
        }

        private void checkBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCheck f = new frmCheck();
            f.ShowDialog();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKeypad fKeypad = new frmKeypad();
            fKeypad.IsInteger = true;
            fKeypad.initialValue = "0";
            fKeypad.textMessage = "Please Insert Quantity";
            fKeypad.ShowDialog();

            if (fKeypad.DialogResult == DialogResult.OK)
            {
                int tmp = 0;
                if (int.TryParse(fKeypad.value, out tmp))
                {
                    defaultQty = tmp;
                    BindGrid();
                }
            }
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

           
        }

        private void printableFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExportBarcodePrinter frmExport = new frmExportBarcodePrinter();
            frmExport.ShowDialog();
            frmExport.Dispose();
        }

        private void selectedItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportSaveDialog.ShowDialog();
        }

        private void exportSaveDialog_FileOk(object sender, CancelEventArgs e)
        {
            List<DataRow> dgvDataSource = (from r in this.Tabel.Rows.Cast<DataGridViewRow>()
                                           where int.Parse(r.Cells[dgQuantity.Name].Value.ToString()) > 0
                                           select (r.DataBoundItem as DataRowView).Row).ToList();
            string message;

            if (!BarcodePrinterController.ExportSelectedItems(dgvDataSource, exportSaveDialog.FileName, out message))
            {
                MessageBox.Show(message);
            }
            else
                MessageBox.Show("File exported successfully");

        }
    }

    public enum SortByColumn
    {
        ItemName,
        ItemNo,
        Barcode,
        Department,
        Category,
        Qty
    }
}
