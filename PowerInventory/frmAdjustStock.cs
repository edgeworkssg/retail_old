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
using System.Configuration;
using System.Globalization;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;

namespace PowerInventory
{
    public partial class frmAdjustStock : frmInventoryParent
    {
        //protected int defaultLoc;
        public frmAdjustStock()
        {
            InitializeComponent();
            LoadInventoryController();
        }

        private void frmAdjustStock_Load(object sender, EventArgs e)
        {
            try
            {
                if (!isTherePendingStockTake)
                {
                    btnSave.Text = "ADJUST";
                    AddAdditionalInformation();
                    LoadInventoryController();
                    this.Text = "ADJUSTMENT";
                   // showCostPrice = true;
                    showOnHandQty = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false);
                    HideStockBalanceAndFactoryPrice();

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
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }
        ComboBox cmbAdjustDirection;
        private void AddAdditionalInformation()
        {
            try
            {
                //Add Additional Information//
                Label lb;

                lb = CreateInventoryLabel();
                lb.Text = "Type";
                cmbAdjustDirection = CreateInventoryComboBox();
                cmbAdjustDirection.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbAdjustDirection.Items.Add("Adjustment IN");
                cmbAdjustDirection.Items.Add("Adjustment OUT");
                cmbAdjustDirection.Name = "cmbAdjustDirection";
                cmbAdjustDirection.SelectedIndexChanged += cmbAdjustDirection_SelectedIndexChanged;
                cmbAdjustDirection.SelectedIndex = 0;

                tblInventory.Controls.Add(lb, 0, 6);
                tblInventory.Controls.Add(cmbAdjustDirection, 1, 6);

                tblInventory.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        private void cmbAdjustDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbAdjustDirection.SelectedIndex == 0)
                {
                    this.btnSave.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
                    btnSave.ForeColor = Color.White;
                    btnSave.Text = "ADJUST IN";
                    /*if (showCostPrice)
                    {
                        dgvStock.Columns["FactoryPrice"].Visible = true;
                        dgvStock.Columns["TotalCostPrice"].Visible = false;
                    }*/
                }
                else
                {
                    this.btnSave.BackgroundImage = global::PowerInventory.Properties.Resources.lightorange;
                    btnSave.ForeColor = Color.Black;
                    btnSave.Text = "ADJUST OUT";
                    /*if (showCostPrice)
                    {
                        dgvStock.Columns["FactoryPrice"].Visible = false;
                        dgvStock.Columns["TotalCostPrice"].Visible = false;
                    }*/
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsLineItemEmpty())
                {
                    MessageBox.Show("Please tick the item you wish to process.");
                    return;
                }
                if (cmbLocation.Items.Count > 1 && cmbLocation.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select the location.");
                    tbControl.SelectedIndex = 0;
                    cmbLocation.Focus();
                    return;
                }
                string status;

                if (txtRemark.Text == "")
                {
                    MessageBox.Show("Please enter remark.");
                    tbControl.SelectedIndex = 0;
                    txtRemark.Focus();
                    return;
                }

                

                if (!CommonUILib.ShowAreYouSure()) return;

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    ShowPanelPleaseWait();
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    if (ws.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                        pnlLoading.Visible = false;
                        return;

                    }
                    pnlLoading.Visible = false;
                }
                else
                {
                    if (StockTakeController.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                        return;
                    }
                }

                InventoryDetCollection tmpDetCol;
                RemoveUnticked(out status, out tmpDetCol);

                bool isSuccess = false;
                //pnlLoading.Parent = this;

                ShowPanelPleaseWait();
                this.Refresh();
                string newInventoryHdrRefNo = "";
                string newCustomRefNo = "";
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    if (cmbAdjustDirection.SelectedIndex == 0)
                    {
                        invCtrl.SetInventoryHeaderInfo
                        ("", "", txtRemark.Text,
                        0.0M, 0.0, 0.0M);

                        SyncClientController.Load_WS_URL();
                        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        ws.Timeout = 100000;
                        ws.Url = SyncClientController.WS_URL;
                        DataSet myDataSet = new DataSet();
                        myDataSet.Tables.Add(invCtrl.InvHdrToDataTable());
                        myDataSet.Tables.Add(invCtrl.InvDetToDataTable());
                        byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);
                    

                        if (ws.StockInCompressed(data, UserInfo.username,
                            ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                            true, true, out newInventoryHdrRefNo, out newCustomRefNo, out status))
                        {
                            //download inventoryhdr and inventorydet                        
                            /*if (SyncClientController.GetCurrentInventoryRealTime())
                            {
                                isSuccess = true;
                                pnlLoading.Visible = false;
                                MessageBox.Show("Adjustment successful");
                            }
                            else
                            {*/
                             //   Logger.writeLog("Unable to download data from server: " + status);
                            isSuccess = true;
                            pnlLoading.Visible = false;
                            invCtrl.SetInventoryHdrRefNo(newInventoryHdrRefNo);
                            invCtrl.SetCustomRefNo(newCustomRefNo);
                            invCtrl.SetIsNew(false);
                            MessageBox.Show("Adjustment successful");
                            //}
                        }
                        else
                        {
                            pnlLoading.Visible = false;
                            MessageBox.Show(LanguageManager.Some_Error_Encountered_Contact_Admin + status);
                            return;
                        }
                    }
                    else
                    {
                        invCtrl.SetInventoryHeaderInfo
                           ("", "", txtRemark.Text,
                           0.0M, 0.0, 0.0M);
                        SyncClientController.Load_WS_URL();
                        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        ws.Timeout = 100000;
                        ws.Url = SyncClientController.WS_URL;
                        DataSet myDataSet = new DataSet();
                        myDataSet.Tables.Add(invCtrl.InvHdrToDataTable());
                        myDataSet.Tables.Add(invCtrl.InvDetToDataTable());
                        byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);
                    
                        if (ws.StockOutCompressed(data, UserInfo.username, 0,
                            ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                            true, true, out newInventoryHdrRefNo, out status))
                        {

                            //download inventoryhdr and inventorydet                        
                            /*if (SyncClientController.GetCurrentInventory())
                            {
                                isSuccess = true;
                                pnlLoading.Visible = false;
                                MessageBox.Show("Adjustment successful");
                            }
                            else
                            {*/
                            isSuccess = true;
                            pnlLoading.Visible = false;
                            invCtrl.SetInventoryHdrRefNo(newInventoryHdrRefNo);
                            invCtrl.SetCustomRefNo(newCustomRefNo);
                            invCtrl.SetIsNew(false);
                            MessageBox.Show("Adjustment successful");
                            //}
                        }
                        else
                        {
                            pnlLoading.Visible = false;
                            MessageBox.Show(LanguageManager.Some_Error_Encountered_Contact_Admin + status);
                            return;
                        }
                    }
                }
                else
                {
                    if (cmbAdjustDirection.SelectedIndex == 0)
                    {
                        invCtrl.SetInventoryHeaderInfo
                        ("", "", txtRemark.Text,
                        0.0M, 0.0, 0.0M);

                        if (invCtrl.StockIn(UserInfo.username,
                            ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                            true, true, out status))
                        {
                            isSuccess = true;
                            pnlLoading.Visible = false;
                            MessageBox.Show("Adjustment Successful");
                            newInventoryHdrRefNo = invCtrl.GetInvHdrRefNo();
                        }
                        else
                        {
                            MessageBox.Show("Error!" + status);
                            pnlLoading.Visible = false;
                            isSuccess = false;
                        }
                    }
                    else
                    {
                        invCtrl.SetInventoryHeaderInfo
                           ("", "", txtRemark.Text,
                           0.0M, 0.0, 0.0M);
                        if (invCtrl.StockOut(UserInfo.username, 0,
                            ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                            true, true, out status))
                        {
                            isSuccess = true;
                            pnlLoading.Visible = false;
                            MessageBox.Show("Adjustment Successful");
                            newInventoryHdrRefNo = invCtrl.GetInvHdrRefNo();
                        }
                        else
                        {
                            isSuccess = false;
                            pnlLoading.Visible = false;
                            MessageBox.Show("Error!" + status);
                        }
                    }
                }
                if (isSuccess)
                {
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Stock Issue : " + invCtrl.GetInvHdrRefNo(), "");
                    if (printAfterConfirm)
                    {
                        //invCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod, newInventoryHdrRefNo); //reload...                    
                        btnPrint_Click(this, new EventArgs());
                    }
                    invCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                    if (tmpDetCol.Count > 0)
                    {
                        invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                        invCtrl.AddInvDet(tmpDetCol);
                        pnlLoading.Visible = false;
                        SaveToDisk(true);
                    }
                    else
                    {
                        //clear control.....       
                        pnlLoading.Visible = false;
                        ClearControls();
                        cmbLocation.SelectedIndex = 0;
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

                        //invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                    }
                    BindGrid();
                    tbControl.TabIndex = 0;
                }
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                Logger.writeLog(ex);
                SaveToDisk(true);                
                MessageBox.Show("Error encounter." + ex.Message);
                
            }
        }
        

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (invCtrl.GetNumberOfLineItem() == 0)
                {
                    MessageBox.Show("There is no item to be printed.");
                    return;
                }
                ShowPanelPleaseWait();
                string StockActivity = "STOCK ADJUSTMENT";
                string AdjustmentDirection = ((ComboBox)this.Controls.Find("cmbAdjustDirection", true)[0]).SelectedItem.ToString();
                frmAdjustStock.PrintAdjustmentSheet(invCtrl, AdjustmentDirection, StockActivity, showOnHandQty, showCostPrice, ChangeCostPriceStkAdjOut);
                pnlLoading.Visible = false;
                this.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        public static void PrintAdjustmentSheet(InventoryController invController, string AdjustmentDirection, string StockActivity,
            bool displayStockOnHand, bool displayCostPrice, bool displayAlternateCostPrice)
        {
            try
            {
                frmViewInventorySheet f = new frmViewInventorySheet();
                f.showCostPrice = displayCostPrice;
                f.showOnHand = displayStockOnHand;
                f.showAlternateCostPrice = displayAlternateCostPrice;
                f.invCtrl = invController;
                f.StockActivity = StockActivity;
                PrintOutParameters printOutParameter = new PrintOutParameters();
                printOutParameter.UserField1Label = "Adjustment Type";
                printOutParameter.UserField1Value = AdjustmentDirection;
                if (printOutParameter.UserField1Value == "") printOutParameter.UserField1Value = "-";
                f.printOutParameters = printOutParameter;
                CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                f.Dispose();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }        
    }
}
