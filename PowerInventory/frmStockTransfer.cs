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
using LanguageManager = PowerInventory.Properties.InventoryLanguage;

namespace PowerInventory
{
    public partial class frmStockTransfer : frmInventoryParent
    {
        public frmStockTransfer()
        {
            InitializeComponent();
        }

        private void frmStockTransfer_Load(object sender, EventArgs e)
        {
            try
            {
                if (!isTherePendingStockTake)
                {
                    //showCostPrice = PrivilegesController.HasPrivilege("GOODS RECEIVE", UserInfo.privileges);
                    showOnHandQty = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityForStockTransfer), false);
                    HideStockBalanceAndFactoryPrice();

                    //string status, deptID, role;                        
                    this.Text = LanguageManager.TRANSFER;
                    btnSave.Text = LanguageManager.TRANSFER;
                    AddAdditionalInformation();
                    LoadInventoryController();
                    dgvStock.Columns["Currency"].Visible = false;

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
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }
        /*
        private TextBox txtPurchaseOrder;
        private TextBox txtSupplier;
        private TextBox txtFreightCharges;
        private TextBox txtDiscount;
        private TextBox txtExchangeRate;*/
        private ComboBox cmbDestination;
        private void AddAdditionalInformation()
        {
            try
            {
                //Add Additional Information//
                Label lb;

                lb = CreateInventoryLabel();
                lb.Text =LanguageManager.Transfer_to;
                cmbDestination = CreateInventoryComboBox();
                cmbDestination.Name = "cmbDestination";

                //Populate with information....
                InventoryLocationCollection inv = new InventoryLocationCollection();
                inv.Where(InventoryLocation.Columns.Deleted, false);
                inv.Load();
                cmbDestination.DataSource = inv;
                cmbDestination.Refresh();
                //cmbDestination.SelectedIndex = 0;
                cmbDestination.DropDownStyle = ComboBoxStyle.DropDownList;

                tblInventory.Controls.Add(lb, 0, 6);
                tblInventory.Controls.Add(cmbDestination, 1, 6);


                tblInventory.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbLocation.Items.Count > 1 && cmbLocation.SelectedIndex == 0)
                {
                    MessageBox.Show(LanguageManager.Please_select_the_location_);
                    tbControl.SelectedIndex = 0;
                    cmbLocation.Focus();
                    return;
                }
                if (IsLineItemEmpty())
                {
                    MessageBox.Show(LanguageManager.Please_tick_the_item_you_wish_to_process);
                    return;
                }

                int transferFrom, transferTo;
                transferFrom = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID;
                transferTo = ((InventoryLocation)(cmbDestination.SelectedValue)).InventoryLocationID;

                //
                if (transferTo == transferFrom)
                {
                    //Unable to tramsfer to tje same location
                    MessageBox.Show(LanguageManager.Cannot_transfer_to_the_same_location__Select_another_destination_location_);
                    cmbDestination.Select();
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
                
                string status;

                if (!CommonUILib.ShowAreYouSure()) return;

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    ShowPanelPleaseWait();
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    if (ws.IsThereUnAdjustedStockTake(transferFrom) && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take for this source location. No inventory movement is allowed! Please adjust stock take first");
                        pnlLoading.Visible = false;
                        return;
                    }
                    if (ws.IsThereUnAdjustedStockTake(transferTo) && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take for this destination location. No inventory movement is allowed! Please adjust stock take first");
                        pnlLoading.Visible = false;
                        return;
                    }
                    pnlLoading.Visible = false;
                }
                else
                {
                    if (StockTakeController.IsThereUnAdjustedStockTake(transferFrom) && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take for this source location. No inventory movement is allowed! Please adjust stock take first");
                        return;
                    }
                    if (StockTakeController.IsThereUnAdjustedStockTake(transferTo) && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take for this destination location. No inventory movement is allowed! Please adjust stock take first");
                        return;
                    }
                }

                //Remove unticked to temporary storage////
                InventoryDetCollection tmpDetCol;
                RemoveUnticked(out status, out tmpDetCol);

                invCtrl.SetInventoryMovementType(InventoryController.InventoryMovementType_TransferOut);

                invCtrl.SetInventoryHeaderInfo
                    ("", "", txtRemark.Text,
                    0, 0.0, 0);
                bool isSuccess = false;
                ShowPanelPleaseWait();
                string newInventoryHdrRefNo = "";
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
                    
                    if (ws.TransferOutAutoReceiveCompressed
                        (data, UserInfo.username,
                        transferFrom, transferTo, 
                        out newInventoryHdrRefNo, out status))
                    {
                        //download inventoryhdr and inventorydet                        
                        //if (SyncClientController.GetCurrentInventoryRealTime())
                        //{
                            isSuccess = true;
                            pnlLoading.Visible = false;
                            invCtrl.SetInventoryHdrRefNo(newInventoryHdrRefNo);
                            invCtrl.SetIsNew(false);
                            MessageBox.Show(LanguageManager.Transfer_successful);
                        //}
                        /*else
                        {
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
                    if (invCtrl.TransferOutAutoReceive(UserInfo.username, transferFrom, transferTo, out status))
                    {                        
                        pnlLoading.Visible = false;
                        newInventoryHdrRefNo = invCtrl.GetInvHdrRefNo();
                        MessageBox.Show(LanguageManager.Transfer_successful);
                        isSuccess = true;
                    }
                    else
                    {
                        pnlLoading.Visible = false;
                        MessageBox.Show(LanguageManager.Error_encounter_ + status);
                        isSuccess = false;
                        return;
                    }
                }
                if (isSuccess)
                {
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Stock Transfer : " + invCtrl.GetInvHdrRefNo(), "");
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
                        SaveToDisk(true);
                    }
                    else
                    {
                        //clear control.....
                        cmbDestination.SelectedIndex = 0;
                        ClearAdditionalInformation();
                        ClearControls();
                        //cmbLocation.SelectedIndex = defaultLoc;
                        cmbLocation.SelectedIndex = 0;
                        //invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                    }

                    BindGrid();
                    tbControl.TabIndex = 0;
                }                
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
                Logger.writeLog(ex);
                SaveToDisk(true);                
            }
        }
        private void ClearAdditionalInformation()
        {
            cmbDestination.SelectedIndex = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (invCtrl.GetNumberOfLineItem() == 0)
            {
                MessageBox.Show(LanguageManager.There_is_no_item_to_be_printed_);
                return;
            }
            ShowPanelPleaseWait();
            string transferDestination;
            transferDestination = ((ComboBox)this.Controls.Find("cmbDestination", true)[0]).SelectedItem.ToString();
            string StockActivity = this.Text;
            frmStockTransfer.PrintTransferSheet
                (invCtrl, transferDestination, StockActivity,showOnHandQty,showCostPrice); 
            pnlLoading.Visible = false;
            this.Refresh();
        }

        public static void PrintTransferSheet
            (InventoryController invController, string transferDestination, string StockActivity, 
            bool displayStockOnHand, bool displayCostPrice)
        {
            
            frmViewInventorySheet f = new frmViewInventorySheet();
            f.invCtrl = invController;
            f.StockActivity = StockActivity;
            f.showCostPrice = displayCostPrice;
            f.showOnHand = displayStockOnHand;
            //
            PrintOutParameters printOutParameter = new PrintOutParameters();
            printOutParameter.UserField1Label = "Transfer Destination";
            printOutParameter.UserField1Value = transferDestination;
            if (printOutParameter.UserField1Value == "") printOutParameter.UserField1Value = "-";

            InventoryLocation loc = new InventoryLocation(InventoryLocation.Columns.InventoryLocationName, transferDestination);
            if (loc != null && loc.InventoryLocationName == transferDestination)
            {
                printOutParameter.UserField2Label = "Destination Address";
                printOutParameter.UserField2Value = loc.Address1;
                printOutParameter.UserField3Label = "Destination Address";
                printOutParameter.UserField3Value = loc.Address2;
                printOutParameter.UserField4Label = "Destination Address";
                printOutParameter.UserField4Value = loc.Address3;
                printOutParameter.UserField5Label = "Destination City";
                printOutParameter.UserField5Value = loc.City;
                printOutParameter.UserField6Label = "Destination Country";
                printOutParameter.UserField6Value = loc.Country + " " + loc.PostalCode;
            }

            f.printOutParameters = printOutParameter;
            CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            f.Dispose();

        }
    }
}
