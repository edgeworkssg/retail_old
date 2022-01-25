using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using System.Configuration;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;

namespace PowerInventory
{
    public partial class frmStockOut : frmInventoryParent
    {
        public frmStockOut()
        {
            InitializeComponent();
            LoadInventoryController();
        }
        
        private void frmStockOut_Load(object sender, EventArgs e)
        {
            try
            {
                if (!isTherePendingStockTake)
                {
                    showCostPrice = false;
                    
                    showOnHandQty = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false);
                    HideStockBalanceAndFactoryPrice();

                    btnSave.Text = LanguageManager.ISSUE;
                    AddAdditionalInformation();
                    LoadInventoryController();
                    this.Text = LanguageManager.STOCK_ISSUE;
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
        ComboBox cmbStockOutReason;
        private void AddAdditionalInformation()
        {
            try
            {
                //Add Additional Information//
                Label lb;

                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Stock_Out_Reason;
                cmbStockOutReason = CreateInventoryComboBox();
                cmbStockOutReason.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbStockOutReason.Name = "cmbStockOutReason";

                //populate the combo box...
                InventoryStockOutReasonCollection col = new InventoryStockOutReasonCollection();
                col.Where(InventoryStockOutReason.Columns.Deleted, false);
                col.Where(InventoryStockOutReason.Columns.ReasonID, Comparison.GreaterThan, 2);
                col.Load();
                InventoryStockOutReason t = new InventoryStockOutReason();
                t.ReasonName = LanguageManager.Select_Reason;
                t.ReasonID = -1;
                col.Insert(0, t);
                cmbStockOutReason.DataSource = col;
                tblInventory.Controls.Add(lb, 0, 6);
                tblInventory.Controls.Add(cmbStockOutReason, 1, 6);

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
                //         
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
                if (cmbStockOutReason.SelectedIndex == 0)
                {
                    MessageBox.Show(LanguageManager.Please_specify_the_stock_out_reason_);
                    tbControl.SelectedIndex = 0;
                    cmbStockOutReason.Select();
                    return;
                }

                
                string status;

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

                //Remove unticked to temporary storage////
                InventoryDetCollection tmpDetCol;
                RemoveUnticked(out status, out tmpDetCol);

                invCtrl.SetInventoryHeaderInfo
                    ("", "", txtRemark.Text,
                    0.0M, 0, 0.0M);
                invCtrl.InvHdr.MovementType = InventoryController.InventoryMovementType_StockOut;
                bool isSuccess = false;
                ShowPanelPleaseWait();
                string newInventoryHdrRefNo = "";
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    //DataTable dt = invCtrl.InvHdrToDataTable();
                    //dt = invCtrl.InvDetToDataTable();
                    DataSet myDataSet = new DataSet();
                    myDataSet.Tables.Add(invCtrl.InvHdrToDataTable());
                    myDataSet.Tables.Add(invCtrl.InvDetToDataTable());
                    byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);
                    
                    if (ws.StockOutCompressed
                        (data,
                        UserInfo.username, ((InventoryStockOutReason)(cmbStockOutReason.SelectedValue)).ReasonID,
                        ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID, false, true, 
                        out newInventoryHdrRefNo, out status))
                    {
                        //download inventoryhdr and inventorydet                        
                        //if (SyncClientController.GetCurrentInventoryRealTime())
                        //{
                            isSuccess = true;
                            pnlLoading.Visible = false;
                            invCtrl.SetInventoryHdrRefNo(newInventoryHdrRefNo);
                            MessageBox.Show(LanguageManager.Stock_Issue_successful);
                        //}
                        //else
                        /*{
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
                    if (invCtrl.StockOut(UserInfo.username, ((InventoryStockOutReason)(cmbStockOutReason.SelectedValue)).ReasonID,
                        ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID, false, true, out status))
                    {
                        isSuccess = true;
                        pnlLoading.Visible = false;
                        newInventoryHdrRefNo = invCtrl.GetInvHdrRefNo();
                        MessageBox.Show(LanguageManager.Stock_Issue_successful);
                    }
                    else
                    {
                        isSuccess = false;
                        pnlLoading.Visible = false;
                        MessageBox.Show(LanguageManager.Error_encounter_ + status);
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
                        SaveToDisk(true);
                    }
                    else
                    {
                        //clear control.....                               
                        ClearControls();
                        cmbStockOutReason.SelectedIndex = 0;
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
                    tbControl.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
                SaveToDisk(true);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (invCtrl.GetNumberOfLineItem() == 0)
                {
                    MessageBox.Show(LanguageManager.There_is_no_item_to_be_printed_);
                    return;
                }
                ShowPanelPleaseWait();
                string stockOutReason;

                stockOutReason = ((ComboBox)this.Controls.Find("cmbStockOutReason", true)[0]).SelectedItem.ToString();
                string StockActivity;
                StockActivity = this.Text;

                frmStockOut.PrintStockOutSheet(invCtrl, stockOutReason, StockActivity, showOnHandQty, true, ChangeCostPriceStkAdjOut);



                pnlLoading.Visible = false;
                this.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        public static void PrintStockOutSheet
            (InventoryController invController,string stockOutReason, string StockActivity, bool displayStockOnHand, bool displayCostPrice, bool showAlternateCostPrice)
        {
            try
            {
                frmViewInventorySheet f = new frmViewInventorySheet();
                f.invCtrl = invController;
                f.StockActivity = StockActivity;
                f.showCostPrice = displayCostPrice;
                f.showOnHand = displayStockOnHand;
                f.showAlternateCostPrice = showAlternateCostPrice;
                //
                PrintOutParameters printOutParameter = new PrintOutParameters();
                printOutParameter.UserField1Label = "Stock Out Reason";
                printOutParameter.UserField1Value = stockOutReason;
                if (printOutParameter.UserField1Value == "") printOutParameter.UserField1Value = "-";
                f.printOutParameters = printOutParameter;
                CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                f.Dispose();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }
    }
}
