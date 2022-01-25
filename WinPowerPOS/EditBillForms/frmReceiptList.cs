using System;
using PowerPOS;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using POSDevices;
using PowerPOS.Container;
using WinPowerPOS.LoginForms;
using WinPowerPOS.OrderForms;

namespace WinPowerPOS
{
    public partial class frmReceiptList : Form
    {
        public const int VIEW_BILL_LIMIT = 5000; //Number of days behind when we can see the bill....
        QueryCommandCollection cmd;
        private string lineInfoCaption;
        public BackgroundWorker SyncSalesThread;
        public BackgroundWorker SyncMemberThread;
        public frmOrderTaking fOrderTaking;
        

        //bool EditBillAllowed;
        #region "Form initializations"
        public frmReceiptList()
        {
            InitializeComponent();
            //AssignPrivileges();
            lineInfoCaption = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith));

        }        
        private void frmReceiptList_Load(object sender, EventArgs e)
        {
            try
            {
                cmbStatus.SelectedIndex = 0;
                CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);
                dgvRcpt.AutoGenerateColumns = false;

                if (!string.IsNullOrEmpty(lineInfoCaption))
                {
                    lblLineInfo.Text = lineInfoCaption;
                    dgvRcpt.Columns["LineInfo"].HeaderText = lineInfoCaption;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ShowInViewReceipt), false))
                {
                    lblLineInfo.Visible = true;
                    txtLineInfo.Visible = true;
                    dgvRcpt.Columns["LineInfo"].Visible = true;
                }
                else
                {
                    lblLineInfo.Visible = false;
                    txtLineInfo.Visible = false;
                    dgvRcpt.Columns["LineInfo"].Visible = false;
                }

                btnRefund.Visible = false;
                if (PrivilegesController.HasPrivilege(PrivilegesController.GIVE_REFUND, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username)))
                {
                    btnRefund.Visible = true;
                }

                displayData();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        #endregion
        
        #region "Privileges Related"
        private void AssignPrivileges()
        {
            /*
            //Privilege management....
            if (!PrivilegesController.HasPrivilege(PrivilegesController.REPRINT_BILL, UserInfo.privileges))
            {
                btnReprint.Enabled = false;
            }
            //Privilege management....
            if (!PrivilegesController.HasPrivilege(PrivilegesController.VIEW_PAST_BILL, UserInfo.privileges))
            {
                btnSearch.Enabled = false;               
            }*/
        }
        #endregion
        #region "Void and Re-Print Logic"
        private void btnReprint_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList myRcpt = new ArrayList();
                for (int i = 0; i < dgvRcpt.Rows.Count; i++)
                {
                    if (dgvRcpt.Rows[i].Cells[0].Value is bool && (bool)dgvRcpt.Rows[i].Cells[0].Value == true)
                    {
                        myRcpt.Add(dgvRcpt.Rows[i].Cells["OrderHdrId"].Value.ToString());
                    }
                }
                RePrint(myRcpt);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new QueryCommandCollection();
                for (int i = 0; i < dgvRcpt.Rows.Count; i++)
                {
                    if (dgvRcpt.Rows[i].Cells[0].Value is bool && (bool)dgvRcpt.Rows[i].Cells[0].Value == true)
                    {
                        bool tmpIsVoided;
                        if (bool.TryParse(dgvRcpt.Rows[i].Cells["IsVoided"].Value.ToString(), out tmpIsVoided))
                        {
                            dgvRcpt.Rows[i].Cells[0].Value = false;
                            tmpIsVoided = !tmpIsVoided;
                            dgvRcpt.Rows[i].Cells["IsVoided"].Value = tmpIsVoided;
                            VoidReceipt(dgvRcpt.Rows[i].Cells["OrderHdrId"].Value.ToString(), tmpIsVoided);
                            if (tmpIsVoided)
                            {
                                for (int j = 0; j < dgvRcpt.ColumnCount; ++j)
                                {
                                    if (dgvRcpt.Columns[j].Visible == true)
                                    {
                                        dgvRcpt.Rows[i].Cells[j].Style.ForeColor = System.Drawing.Color.White;
                                        dgvRcpt.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.DarkRed;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < dgvRcpt.ColumnCount; ++j)
                                {
                                    if (dgvRcpt.Columns[j].Visible == true)
                                    {
                                        dgvRcpt.Rows[i].Cells[j].Style.ForeColor = System.Drawing.Color.Black;
                                        dgvRcpt.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.White;
                                    }
                                }
                            }
                        }
                    }
                }
                if (cmd.Count > 0) { DataService.ExecuteTransaction(cmd); }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void RePrint(ArrayList orderHdrIDs)
        {
            try
            {
                //Pop up number of copies....
                frmKeypad f = new frmKeypad();
                f.textMessage = "NUMBER OF COPIES";
                f.initialValue = "1";
                f.IsInteger = true;
                f.ShowDialog();
                int numOfCopies = 1;
                if (!int.TryParse(f.value, out numOfCopies))
                {
                    return;
                }
                POSController pos;
                for (int i = 0; i < orderHdrIDs.Count; i++)
                {
                    pos = new POSController(orderHdrIDs[i].ToString());
                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0,true, ReceiptSizes.A4,numOfCopies);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //mark voided as red
        private void dgvRcpt_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                for (int j = 0; j < dgvRcpt.ColumnCount; ++j)
                {
                    if (dgvRcpt.Columns[j].Visible == true && bool.Parse(dgvRcpt["IsVoided", e.RowIndex].Value.ToString()) == true)
                    {
                        dgvRcpt.Rows[e.RowIndex].Cells[j].Style.ForeColor = System.Drawing.Color.White;
                        dgvRcpt.Rows[e.RowIndex].Cells[j].Style.BackColor = System.Drawing.Color.DarkRed;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region "Search Logic"
        
        private void displayData()
        {
            try
            {

                DateTime StartDate;
                DateTime EndDate;

                //if (dtpStartDate.Checked)
                //{
                    StartDate = dtpStartDate.Value;//.AddSeconds(-dtpStartDate.Value.TimeOfDay.TotalSeconds + 1);
                //}
                //else
                //{
                //    StartDate = DateTime.MinValue;
                //}
                //if (dtpEndDate.Checked)
                //{
                    EndDate = dtpEndDate.Value; //.AddSeconds(86399 - dtpEndDate.Value.TimeOfDay.TotalSeconds);
                //}
                //else
                //{
                //    EndDate = DateTime.MaxValue;
                //}

                if (VIEW_BILL_LIMIT >=0 && StartDate < DateTime.Now.AddDays(-VIEW_BILL_LIMIT))
                {
                    StartDate = DateTime.Now.AddDays(-VIEW_BILL_LIMIT);
                    dtpStartDate.Value = StartDate;
                }
                string isVoidedValue = "";
                if (cmbStatus.SelectedItem.ToString() == "Voided")
                {
                    isVoidedValue = "1";
                }
                else if (cmbStatus.SelectedItem.ToString() == "Not Voided")
                {
                    isVoidedValue = "0";
                }
                string refNo = txtRefNo.Text.Replace("RCP", "").Replace("OR", "");
                DataTable dt = ReportController.FetchTransactionReportForViewSales(
                                StartDate, EndDate, //EndDate.AddSeconds(86399)
                                refNo, txtCashier.Text, "", PointOfSaleInfo.OutletName, txtRemark.Text, isVoidedValue,txtName.Text, txtLineInfo.Text);
                dgvRcpt.DataSource = dt;
                dgvRcpt.Refresh();
                /*
                OrderHdrController ord = new OrderHdrController();
                dgvRcpt.DataSource = ord.FetchOrderSummary
                    (dtpStartDate.Checked, dtpEndDate.Checked, 
                     StartDate, EndDate, 
                     refNo, txtCashier.Text, txtRemark.Text, PointOfSaleInfo.PointOfSaleID, 
                     cmbStatus.SelectedItem.ToString());
                dgvRcpt.Refresh();*/
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            displayData();
        }
        #endregion

        #region "edit bill button handler"
        private void dgvRcpt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    if (dgvRcpt.Rows[e.RowIndex].Cells[0].Value is bool)
                    {
                        dgvRcpt.Rows[e.RowIndex].Cells[0].Value = !(bool)dgvRcpt.Rows[e.RowIndex].Cells[0].Value;
                    }
                    else
                    {
                        dgvRcpt.Rows[e.RowIndex].Cells[0].Value = true;
                    }
                }
                else if (e.ColumnIndex == 1)
                {
                    EditBillForms.frmViewBillDetail myfrm = new EditBillForms.frmViewBillDetail();
                    myfrm.SyncSalesThread = SyncSalesThread;
                    myfrm.OrderHdrID = dgvRcpt.Rows[e.RowIndex].Cells["OrderHdrID"].Value.ToString();
                    myfrm.ShowDialog();
                    myfrm.Dispose();
                    displayData();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void VoidReceipt(string orderID, bool val)
        {
            try
            {
                Query qry = OrderHdr.CreateQuery();
                qry.QueryType = QueryType.Update;
                qry.AddUpdateSetting(OrderHdr.Columns.IsVoided, val);
                qry.WHERE(OrderHdr.Columns.OrderHdrID, orderID);
                cmd.Add(qry.BuildUpdateCommand());

                Query qryDet = OrderDet.CreateQuery();
                qryDet.QueryType = QueryType.Update;
                qryDet.AddUpdateSetting(OrderDet.Columns.IsVoided, val);
                qryDet.WHERE(OrderDet.Columns.OrderHdrID, orderID);
                cmd.Add(qryDet.BuildUpdateCommand());


                Query rQry = ReceiptHdr.CreateQuery();
                rQry.QueryType = QueryType.Update;
                rQry.AddUpdateSetting(ReceiptHdr.Columns.IsVoided, val);
                rQry.WHERE(ReceiptHdr.Columns.OrderHdrID, orderID);
                cmd.Add(rQry.BuildUpdateCommand());

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmReceiptList_Activated(object sender, EventArgs e)
        {
            //displayData();
        }

        

        private void btnExport_Click(object sender, EventArgs e)
        {
            
            if (dgvRcpt != null && dgvRcpt.Rows.Count > 0)
            {
                fsdExportToExcel.ShowDialog();                
            }
            else
            {
                MessageBox.Show("There is no data to export");
            }            
        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcel(dgvRcpt, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmKeyboard fKeypad = new frmKeyboard();
            fKeypad.textMessage = "Please Insert the Order Ref No";
            fKeypad.ShowDialog();
            string status;
            if (fKeypad.value != "")
            {
                POSController posRefund = new POSController();
                //check if it was refunded
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromSameOutlet), false))
                {
                    OrderHdrCollection ohCol = new OrderHdrCollection();
                    ohCol.Where(OrderHdr.Columns.Userfld5, fKeypad.value);
                    ohCol.Load();

                    if (ohCol.Count == 0)
                    {
                        ohCol.Where(OrderHdr.Columns.OrderRefNo, fKeypad.value);
                        ohCol.Load();
                    }

                    if (ohCol.Count > 0)
                    {
                        if (ohCol[0].IsVoided)
                        {
                            MessageBox.Show("Order " + ohCol[0].Userfld5 + ".Already Voided.");
                            return;
                        }
                        POSController posTemp = new POSController(ohCol[0].OrderHdrID);

                        int isRefunded = 0;
                        if (posTemp != null && posTemp.myOrderHdr != null && posTemp.myOrderHdr.OrderHdrID != "")
                        {
                            ArrayList tmpRemove = new ArrayList();
                            foreach (OrderDet od in posTemp.myOrderDet)
                            {
                                decimal qtyRefunded = 0;
                                if (POSController.isRefunded(od.OrderDetID, out qtyRefunded) || (od.Quantity < 0 && od.ItemNo != "LINE_DISCOUNT"))
                                {
                                    if (qtyRefunded < od.Quantity)
                                    {
                                        od.Quantity = (od.Quantity - qtyRefunded) * -1;
                                        od.ReturnedReceiptNo = "OR" + od.OrderHdrID;
                                        od.RefundOrderDetID = od.OrderDetID;
                                        od.InventoryHdrRefNo = "";
                                        OrderDet tempOD = od;
                                        posTemp.CalculateLineAmount(ref tempOD);
                                    }
                                    else
                                    {
                                        tmpRemove.Add(od.OrderDetID);
                                    }
                                }
                                else
                                {
                                    if (!POSController.isRefunded(od.OrderDetID, out qtyRefunded))
                                    {
                                        od.Quantity = od.Quantity * -1;
                                        od.ReturnedReceiptNo = "OR" + od.OrderHdrID;
                                        od.RefundOrderDetID = od.OrderDetID;
                                        od.InventoryHdrRefNo = "";
                                        OrderDet tempOD = od;
                                        posTemp.CalculateLineAmount(ref tempOD);
                                    }
                                }
                            }
                            if (tmpRemove.Count == posTemp.myOrderDet.Count)
                            {
                                MessageBox.Show("All Item in the order " + posTemp.myOrderHdr.Userfld5 + " already Refunded");
                                return;
                            }
                            else
                            {
                                if (tmpRemove.Count > 0)
                                {
                                    for (int i = 0; i < tmpRemove.Count; i++)
                                    {
                                        OrderDet od = (OrderDet)posTemp.myOrderDet.Find(tmpRemove[i].ToString());
                                        if (od != null && od.OrderDetID != "")
                                            posTemp.myOrderDet.Remove(od);
                                    }
                                    status = "Warning. Some items has already refunded.";
                                }
                            }
                            /*if (isRefunded == posTemp.myOrderDet.Count)
                            {
                                MessageBox.Show("All Item in the order " + posTemp.myOrderHdr.Userfld5 + " already Refunded");
                                return false;
                            }*/
                            if (posTemp.myOrderHdr.MembershipNo != "WALK-IN")
                            {
                                posTemp.AssignMembership(posTemp.myOrderHdr.MembershipNo, out status);
                            }
                            posTemp.DeleteAllReceiptLinePayment();
                            posTemp.myOrderHdr.IsPointAllocated = false; // Make sure IsPointAllocated is false
                            posRefund = posTemp;
                        }
                        fOrderTaking = new frmOrderTaking(true, posRefund);
                        fOrderTaking.SyncSalesThread = SyncSalesThread;
                        fOrderTaking.SyncMemberThread = SyncMemberThread;
                        fOrderTaking.Show();
                    }
                    else
                    {
                        MessageBox.Show("Order " + fKeypad.value + " is not valid");
                    }
                    
                }
                else
                {
                    DataSet ds = SyncClientController.GetOrderForRefund(fKeypad.value, PointOfSaleInfo.PointOfSaleID, true, out status);
                    if (ds != null && ds.Tables.Count > 0)
                    {

                        POSController posTemp = new POSController();
                        posTemp.myOrderHdr.Load(ds.Tables[0]);
                        posTemp.myOrderDet.Load(ds.Tables[1]);
                        //obj = new JavaScriptSerializer().Deserialize<InventoryController>(result);
                        bool isRefund = false;
                        if (posTemp != null && posTemp.myOrderHdr != null && posTemp.myOrderHdr.OrderHdrID != "")
                        {
                            /*if (isRefund)
                            {
                                MessageBox.Show("Orders contain refund item.");
                                return false;
                            }*/
                            

                            if (status != "" && status.StartsWith("Warning."))
                            {
                                MessageBox.Show(status.Replace("Warning.", ""));
                            }
                            if (posTemp.myOrderHdr.MembershipNo != "WALK-IN")
                            {
                                posTemp.AssignMembership(posTemp.myOrderHdr.MembershipNo, out status);
                            }
                            posTemp.DeleteAllReceiptLinePayment();
                            posTemp.myOrderHdr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                            posTemp.myOrderHdr.IsPointAllocated = false; // Make sure IsPointAllocated is false
                            posRefund = posTemp;

                            fOrderTaking = new frmOrderTaking(true, posRefund);
                            fOrderTaking.SyncSalesThread = SyncSalesThread;
                            fOrderTaking.SyncMemberThread = SyncMemberThread;
                            fOrderTaking.Show();
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show(status);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter value for the Order Ref No");
                return;
            }


        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }        
    }
}