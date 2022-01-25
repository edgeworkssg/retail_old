using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;
using PowerPOS.Container;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System.Configuration;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;

namespace PowerInventory
{
    public partial class frmStockTake : frmInventoryParent
    {
        public frmStockTake()
        {
            InitializeComponent();
            LoadInventoryController();
        }
        protected override void dgvStock_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            /*
            if (dgvStock.Rows[e.RowIndex].Cells[5].Value.ToString().Length > 0 &&
                dgvStock.Rows[e.RowIndex].Cells[5].Value.ToString()[0] != '$')
            {
                decimal tmp;
                if (decimal.TryParse(dgvStock.Rows[e.RowIndex].Cells[5].Value.ToString(), out tmp))                
                {
                    dgvStock.Rows[e.RowIndex].Cells[5].Value = "$" + tmp.ToString("N2");
                }
            }*/
        }

        private void frmStockTake_Load(object sender, EventArgs e)
        {
            showCostPrice = false;
            //showOnHandQty = false;
            showOnHandQty = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false);
            HideStockBalanceAndFactoryPrice();

            
            btnSave.Text = LanguageManager.SAVE_STOCK_TAKE;
            dtpInventoryDate.Enabled = true;
            dtpInventoryDate.Value = DateTime.Today.AddHours(23).AddMinutes(59);                     
            AddAdditionalInformation();
            LoadInventoryController();

            this.Text = LanguageManager.STOCK_TAKE;

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.OnlyAllowInCurrentInventoryLocation), false))
            {
                //InventoryLocation tmp = new InventoryLocation(PointOfSaleInfo.InventoryLocationID);
                for (int i = 0; i < cmbLocation.Items.Count; i++)
                {
                    if (((InventoryLocation)(cmbLocation.Items[i])).InventoryLocationID == PointOfSaleInfo.InventoryLocationID)
                    {
                        cmbLocation.SelectedIndex = i;
                        invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                        cmbLocation.Enabled = false;
                    }
                }

            }

            timer1.Enabled = true;
        }

        private static int getStockTakeID()
        {
            return 1;
            /*
            object ob = (StockTake.CreateQuery()).GetMax(StockTake.Columns.StockTakeID);
            int stockTakeID;
            if (ob != null && ob is int)
            {
                stockTakeID = (int)ob + 1;
            }
            else
            {
                stockTakeID = 0;
            }
            return stockTakeID;*/
        }
        TextBox txtTakenBy;
        TextBox txtVerifiedBy;

        private void AddAdditionalInformation()
        {
            //Add Additional Information//
            Label lb;

            lb = CreateInventoryLabel();
            lb.Text = LanguageManager.Taken_By;
            txtTakenBy = CreateInventoryTextBox();
            txtTakenBy.Name = "txtTakenBy";
            tblInventory.Controls.Add(lb, 0, 6);
            tblInventory.Controls.Add(txtTakenBy, 1, 6);

            lb = CreateInventoryLabel();
            lb.Text = LanguageManager.Verified_By;
            txtVerifiedBy = CreateInventoryTextBox();
            txtVerifiedBy.Name = "txtVerifiedBy";
            tblInventory.Controls.Add(lb, 3, 6);
            tblInventory.Controls.Add(txtVerifiedBy, 4, 6);

            tblInventory.Refresh();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string status;
            if (IsLineItemEmpty())
            {
                MessageBox.Show(LanguageManager.Please_tick_the_item_you_wish_to_process);
                tbControl.SelectedIndex = 1;                
                return;
            }
            if (cmbLocation.Items.Count > 1 && cmbLocation.SelectedIndex == 0)
            {
                    MessageBox.Show(LanguageManager.Please_select_the_location_);
                    tbControl.SelectedIndex = 0;
                    cmbLocation.Focus();
                    return;
            }
            if (txtTakenBy.Text == "")
            {
                MessageBox.Show(LanguageManager.Please_specify_stock_take_verifier_);
                tbControl.SelectedIndex = 0;
                txtTakenBy.Focus();
                return;
            }

            if (txtVerifiedBy.Text == "") 
            {
                MessageBox.Show(LanguageManager.Please_specify_stock_take_personnel_);
                tbControl.SelectedIndex = 0;
                txtVerifiedBy.Focus();
                return;
            }

            var listInvDet = new List<string>();
            for (int i = 0; i < dgvStock.Rows.Count; i++)
            {
                if ((dgvStock.Rows[i].Cells[0].Value + "").IsEqual("true"))
                    listInvDet.Add(dgvStock.Rows[i].Cells["InventoryDetRefNo"].Value + "");
            }

            string msgSerialNo = "";
            if (!invCtrl.IsSerialNoValid(listInvDet, out msgSerialNo))
            {
                MessageBox.Show(msgSerialNo);
                return;
            }

            //if (!CommonUILib.ShowAreYouSure()) return;
            frmConfirmStockTakeDateTime d = new frmConfirmStockTakeDateTime();
            d.stockTakeDate = dtpInventoryDate.Value;
            DialogResult res = d.ShowDialog();
            invCtrl.SetInventoryDate(d.stockTakeDate);
            invCtrl.SetRemark(txtRemark.Text);
            d.Dispose();

            //add validation if cancel
            if (res == DialogResult.Cancel)
                return;

            //Check with Existing UnAdjusted Stock Take
            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                //Check Ws.for unadjusted stock take
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                string LastStockTakeDateStr = ws.GetUnAdjustedStockTakeDate(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID);
                if (LastStockTakeDateStr != "")
                {
                    DateTime lastStockTakeDate;
                    if (DateTime.TryParse(LastStockTakeDateStr, out lastStockTakeDate))
                    {
                        if (DateTime.Compare(lastStockTakeDate, invCtrl.GetInventoryDate()) != 0)
                        {
                            MessageBox.Show(
                               "There is an unadjusted stock take for this location with different Stock Take Date. No inventory movement is allowed! Please adjust stock take first");
                            return;
                        }
                    }
                }
            }
            else
            {
                string LastStockTakeDateStr = StockTakeController.GetUnAdjustedStockTakeDate(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID);
                if (LastStockTakeDateStr != "")
                {
                    DateTime lastStockTakeDate;
                    if (DateTime.TryParse(LastStockTakeDateStr, out lastStockTakeDate))
                    {
                        if (DateTime.Compare(lastStockTakeDate, invCtrl.GetInventoryDate()) != 0)
                        {
                            MessageBox.Show(
                               "There is an unadjusted stock take for this location with different Stock Take Date. No inventory movement is allowed! Please adjust stock take first");
                            return;
                        }
                    }
                }
            }
            //Remove unticked to temporary storage////
            InventoryDetCollection tmpDetCol;
            RemoveUnticked(out status, out tmpDetCol);
            bool isSuccess = false;
            ShowPanelPleaseWait();
            
            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                DataSet myDataSet = new DataSet();
                myDataSet.Tables.Add(invCtrl.InvHdrToDataTable());
                myDataSet.Tables.Add(invCtrl.InvDetToDataTable());
                byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);
                    
                if (ws.CreateStockTakeEntriesCompressed
                    (data, 
                    UserInfo.username, txtTakenBy.Text, txtVerifiedBy.Text, out status))
                {
                    isSuccess = true;
                    pnlLoading.Visible = false;
                    MessageBox.Show(LanguageManager.Stock_Take_successful);
                    //download inventoryhdr and inventorydet                        
                    /*if (SyncClientController.GetCurrentInventory())
                    {
                        isSuccess = true;
                        pnlLoading.Visible = false;
                        MessageBox.Show(LanguageManager.Stock_Take_successful);
                    }
                    else
                    {
                        pnlLoading.Visible = false;
                        Logger.writeLog("Unable to download data from server: " + status);
                        isSuccess = false;
                        this.Close();
                        return;
                    }*/
                }
                else
                {
                    isSuccess = false;
                    pnlLoading.Visible = false;
                    MessageBox.Show(LanguageManager.Some_Error_Encountered_Contact_Admin + status);
                    return;
                }
            }
            else
            {
                if (invCtrl.CreateStockTakeEntries
                    (UserInfo.username, txtTakenBy.Text, txtVerifiedBy.Text, out status))
                {
                    pnlLoading.Visible = false;
                    isSuccess = true;
                    MessageBox.Show(LanguageManager.Stock_Take_Saved);                    
                }
                else
                {
                    pnlLoading.Visible = false;
                    isSuccess = false;
                    MessageBox.Show(LanguageManager.Error_encounter_ + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (isSuccess)
            {
                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Stock Take : " + invCtrl.GetInvHdrRefNo(), "");
                if (printAfterConfirm)
                {                    
                    btnPrint_Click(this, new EventArgs());
                }
                invCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);

                if (tmpDetCol.Count > 0)
                {
                    invCtrl.AddInvDet(tmpDetCol);
                    invCtrl.SetInventoryLocation((
                        (InventoryLocation)(cmbLocation.SelectedValue)).
                        InventoryLocationID);
                    SaveToDisk(true);
                }
                else
                {
                    ClearControls();
                    txtTakenBy.Text = "";
                    txtVerifiedBy.Text = "";
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.OnlyAllowInCurrentInventoryLocation), false))
                    {
                        //InventoryLocation tmp = new InventoryLocation(PointOfSaleInfo.InventoryLocationID);
                        for (int i = 0; i < cmbLocation.Items.Count; i++)
                        {
                            if (((InventoryLocation)(cmbLocation.Items[i])).InventoryLocationID == PointOfSaleInfo.InventoryLocationID)
                            {
                                cmbLocation.SelectedIndex = i;
                                invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                                cmbLocation.Enabled = false;
                            }
                        }

                    }
                    invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                }
                BindGrid();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SaveInventoryController();
        }

        private void SaveInventoryController()
        {
            invCtrl.setInventoryStockTakeTakenBy(txtTakenBy.Text, txtVerifiedBy.Text);
            if (File.Exists(Application.StartupPath + "\\temp.bin"))
            {
                File.Delete(Application.StartupPath + "\\temp.bin");
            }
            Stream a = File.OpenWrite(Application.StartupPath + "\\temp.bin");

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(a, invCtrl);
            a.Close();
        }
        /*
        private bool LoadInventoryController()
        {            
            if (File.Exists(Application.StartupPath + "\\temp.bin"))
            {
                FileStream file = new FileStream
                    (Application.StartupPath + "\\temp.bin",
                    FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                invCtrl = bf.Deserialize(file) as InventoryController;                
                file.Close();
                BindGrid();
                dtpInventoryDate.Value = invCtrl.GetInventoryDate();
                
                string takenBy = "", verifiedBy = "";
                
                invCtrl.GetInventoryStockTakeTakenBy(out takenBy, out verifiedBy);
                if (takenBy != null)
                    txtTakenBy.Text = takenBy;
                if (verifiedBy != null)
                    txtVerifiedBy.Text = verifiedBy.ToString();

                int LocID = invCtrl.GetInventoryLocationID();
                for (int i = 0; i < cmbLocation.Items.Count; i++)
                {
                    if (((InventoryLocation)cmbLocation.Items[i]).InventoryLocationID == LocID)
                    {
                        cmbLocation.SelectedIndex = i;
                        break;
                    }
                }
                                
                return true;
            }
            return false;
        }
        */
        private void btnAddFullStockTake(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(LanguageManager.Are_you_sure_you_want_to_add_full_stock_take_item_list_to_the_current_list_, "",  MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                //load item collections
                ItemCollection it = new ItemCollection();
                it.Where(Item.Columns.Deleted, false);
                it.Where(Item.Columns.IsInInventory, true);
                it.OrderByAsc(Item.Columns.ItemNo);
                it.Load();
                string status;

                //add those item into the inventory
                for (int i = 0; i < it.Count; i++)
                {
                    invCtrl.AddItemIntoInventory(it[i].ItemNo, 0, out status);
                }
                BindGrid();
            }
        }

        private void frmStockTake_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveInventoryController();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (invCtrl.GetNumberOfLineItem() == 0)
            {
                MessageBox.Show(LanguageManager.There_is_no_item_to_be_printed_);
                return;
            }
            ShowPanelPleaseWait();
            string TakenBy = "-";
            string VerifiedBy = "-";
            
            if (txtTakenBy != null) TakenBy = txtTakenBy.Text;
            if (txtVerifiedBy != null) VerifiedBy = txtVerifiedBy.Text;            
            string StockActivity = this.Text;
            frmStockTake.PrintStockTakeSheet
                (invCtrl, TakenBy, VerifiedBy, StockActivity, 
                showOnHandQty,showCostPrice);
            pnlLoading.Visible = false;
            this.Refresh();
        }

        public static void PrintStockTakeSheet
            (InventoryController invController, string TakenBy,
            string VerifiedBy, string StockActivity, bool displayStockOnHand, bool displayCostPrice)
        {
            frmViewInventorySheet f = new frmViewInventorySheet();
            f.invCtrl = invController;
            f.StockActivity = StockActivity;
            f.showCostPrice = displayCostPrice;
            f.showOnHand = displayStockOnHand;
            //use find control...
            PrintOutParameters printOutParameter = new PrintOutParameters();
            printOutParameter.UserField1Label = "Taken By";
            printOutParameter.UserField1Value = TakenBy;
            if (printOutParameter.UserField1Value == "") printOutParameter.UserField1Value = "-";
            printOutParameter.UserField2Label = "Verified By";
            printOutParameter.UserField2Value = VerifiedBy;
            if (printOutParameter.UserField2Value == "") printOutParameter.UserField2Value = "-";            
            f.printOutParameters = printOutParameter;
            CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            f.Dispose();
        }
    }
}
