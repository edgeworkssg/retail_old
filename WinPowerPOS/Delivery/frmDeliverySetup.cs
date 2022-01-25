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
using WinPowerPOS.OrderForms;
using SubSonic;

namespace WinPowerPOS.Delivery
{
    public partial class frmDeliverySetup : Form
    {
        public frmDeliverySetup()
        {
            InitializeComponent();
        }
        public bool IsNew;
        public string orderHdrID;
        public string membershipNo;
        public DeliveryOrderDetailCollection delOrderDetColl;
        public DeliveryOrder delOrderHdr;
        public DeliveryOrderCollection delOrderHdrColl;
        public Membership myMember;
        public POSController pos;
        DataTable dt;

        public BackgroundWorker SyncSendDeliveryOrderThread;

        public bool IsUnsavedOrder = false; // IsUnsavedOrder = true when loading this form from frmOrderTaking
        public bool AllowSplitDelivery = false;

        private decimal depositBalance = 0;
        private decimal assignedDeposit = 0;
        private decimal totalItemPrice = 0;
        private decimal assignedDepositForThisDO = 0;

        DeliveryController doc;
        private void BindGrid()
        {
            dt = doc.FetchDeliveryItems();

            // Add additional column for Recipient Name, Address, etc
            dt.Columns.Add("ItemPrice", Type.GetType("System.Decimal"));
            dt.Columns.Add("DepositPaid", Type.GetType("System.Decimal"));
            dt.Columns.Add("RecipientName", Type.GetType("System.String"));
            dt.Columns.Add("MobileNo", Type.GetType("System.String"));
            dt.Columns.Add("HomeNo", Type.GetType("System.String"));
            dt.Columns.Add("PostalCode", Type.GetType("System.String"));
            dt.Columns.Add("DeliveryAddress", Type.GetType("System.String"));
            dt.Columns.Add("UnitNo", Type.GetType("System.String"));
            dt.Columns.Add("DeliveryDate", Type.GetType("System.String"));
            dt.Columns.Add("DeliveryTime", Type.GetType("System.String"));
            dt.Columns.Add("DeliveryDateTime", Type.GetType("System.String"));
            dt.Columns.Add("IsTicked", Type.GetType("System.Boolean"));

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["IsTicked"] = true;

                    OrderDet od = new OrderDet(dt.Rows[i]["OrderDetID"].ToString());
                    dt.Rows[i]["ItemPrice"] = od.Amount;
                    dt.Rows[i]["DepositPaid"] = od.DepositAmount;
                }

                dgvDeliverySetup.DataSource = dt;
                dgvDeliverySetup.Refresh();
            }

            if (string.IsNullOrEmpty(doc.myDeliveryOrderHdr.SalesOrderRefNo))
            {
                depositBalance = 0;
            }
            else
            {
                string status;
                depositBalance = POSController.GetDepositBalance(doc.myDeliveryOrderHdr.SalesOrderRefNo, out status);
            }

            Calculate_AssignedDeposit();
        }

        private void Calculate_AssignedDeposit()
        {
            totalItemPrice = 0;
            assignedDepositForThisDO = 0;
            DataTable dt = (DataTable)dgvDeliverySetup.DataSource;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dt.Rows[i]["IsTicked"]) == true)
                    {
                        totalItemPrice += decimal.Parse(dt.Rows[i]["ItemPrice"].ToString());
                        assignedDepositForThisDO += decimal.Parse(dt.Rows[i]["DepositPaid"].ToString());
                    }
                }
            }

            assignedDeposit = 0;

            if (pos != null)
            {
                OrderDetCollection odets = pos.myOrderDet;
                foreach (OrderDet od in odets)
                {
                    assignedDeposit += od.DepositAmount;
                }
            }

            if (assignedDeposit == 0)
            {
                if (AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoAssignMode) == AppSetting.SettingsName.Order.AutoAssignWeightAge)
                {
                    decimal totalprice = 0;
                    decimal countticked = 0;
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[i]["IsTicked"]) == true)
                            {
                                totalprice += decimal.Parse(dt.Rows[i]["ItemPrice"].ToString());
                                countticked++;
                            }
                        }

                        decimal weightage = depositBalance / countticked;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[i]["IsTicked"]) == true)
                            {
                                dt.Rows[i]["DepositPaid"] = weightage;
                                assignedDeposit += weightage;
                            }
                        }


                        dgvDeliverySetup.DataSource = dt;
                        dgvDeliverySetup.Refresh();
                    }
                }
                else if (AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoAssignMode) == AppSetting.SettingsName.Order.AutoAssignFirstItem)
                {
                    decimal totaldeposit = 0;
                    if (dt != null)
                    {
                        decimal firstitem = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[i]["IsTicked"]) == true)
                            {
                                firstitem = decimal.Parse(dt.Rows[0]["ItemPrice"].ToString());
                                break;
                            }
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[i]["IsTicked"]) == true)
                            {
                                dt.Rows[i]["DepositPaid"] = firstitem;
                                totaldeposit += firstitem;
                                assignedDeposit += firstitem;
                            }
                        }

                        dgvDeliverySetup.DataSource = dt;
                        dgvDeliverySetup.Refresh();
                    }
                }
            }

            lblUnassignedDeposit.Text = (depositBalance - assignedDeposit).ToString("$0.00");
            if (assignedDeposit > depositBalance)
            {
                lblUnassignedDeposit.ForeColor = Color.Red;
            }
            else
            {
                lblUnassignedDeposit.ForeColor = Color.Black;
            }
        }

        private void Load_Membership_Info()
        {
            if (myMember.IsLoaded && !myMember.IsNew)
            {
                txtMembershipNo.Text = myMember.MembershipNo;
                txtName.Text = myMember.NameToAppear;
                txtMobileNo.Text = myMember.Mobile;
                txtHomeNo.Text = myMember.Home;
                txtPostalCode.Text = myMember.ZipCode;
                txtAddress.Text = myMember.StreetName + Environment.NewLine + 
                                  myMember.StreetName2 + Environment.NewLine + 
                                  myMember.Country + " " + myMember.ZipCode;
                txtUnitNo.Text = myMember.UnitNo;
            }

            //LOAD DELIVERY ADDRESS FROM DELIVER ORDER
            
            DeliveryOrder ord = new DeliveryOrder(txtReceiptNumber.Text);
            if (ord != null)
            {
                if (!string.IsNullOrEmpty(ord.RecipientName))
                {
                    txtName.Text = ord.RecipientName;
                }

                if (!string.IsNullOrEmpty(ord.DeliveryAddress))
                {
                    txtAddress.Text = ord.DeliveryAddress;
                }

                if (!string.IsNullOrEmpty(ord.Remark))
                {
                    txtRemarks.Text = ord.Remark;
                }
                
                if (ord.TimeSlotFrom != null)
                {
                    dtpDeliveryDate.Value = (DateTime) ord.TimeSlotFrom;
                    dtpDeliveryDate.Checked = true;

                    if (ord.TimeSlotFrom.Value.Hour == 10)
                    {
                        cmbDeliveryTime.SelectedIndex = 1;
                    }
                    else if (ord.TimeSlotFrom.Value.Hour == 12)
                    {
                        cmbDeliveryTime.SelectedIndex = 2;
                    }
                    else if (ord.TimeSlotFrom.Value.Hour == 14)
                    {
                        cmbDeliveryTime.SelectedIndex = 3;
                    }
                }
            }
        }

        private void LoadDeliveryTimeComboBox()
        {
            DataTable dt = PointOfSaleInfo.DeliveryTimes.Copy();
            DataRow dr = dt.NewRow();
            dr.ItemArray = new string[] {"", ""};
            dt.Rows.InsertAt(dr, 0);
            cmbDeliveryTime.DataSource = dt;
            cmbDeliveryTime.DisplayMember = "Text";
            cmbDeliveryTime.ValueMember = "Value";
            cmbDeliveryTime.SelectedIndex = 0;
        }

        private void frmDeliverySetup_Load(object sender, EventArgs e)
        {
            IsNew = true;
            doc = new DeliveryController();
            string status;

            AllowSplitDelivery = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AllowSplitDelivery), false);

            //Load membership information
            myMember = new Membership(Membership.Columns.MembershipNo, membershipNo);
            Load_Membership_Info();
            
            dgvDeliverySetup.AutoGenerateColumns = false;
            dtpDeliveryDate.Value = DateTime.Today.AddDays(1);
            dtpDeliveryDate.Checked = false;
            LoadDeliveryTimeComboBox();

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowVendorDelivery), false))
            {
                pnlVendorDelivery.Visible = true;
            }

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowDeliveryOutlet), false))
            {
                pnlDeliveryOutlet.Visible = true;
                cmbDeliveryOutlet.DataSource = PointOfSaleController.FetchOutletNames();
                cmbDeliveryOutlet.Refresh();
            }

            if (IsUnsavedOrder)
            {
                txtReceiptNumber.Enabled = false;
                btnClose.Visible = false;  // Hide the CLOSE button
                lblReceiptNo.Text = "PENDING";
                
                
                doc.LoadFromPOS(pos, out status);
                if (status != "")
                {
                    MessageBox.Show("Load Failed. " + status);
                    //return;
                }
            }
            else
            {
                if (delOrderHdr != null)
                {
                    // Edit Mode
                    // NOTE: in EDIT MODE, split delivery is not allowed.
                    IsNew = false;
                    AllowSplitDelivery = false;

                    txtReceiptNumber.Enabled = false;
                    lblTickItems.Visible = false;

                    #region *) Deal with the GridView
                    dgvDeliverySetup.Columns["cbColumn"].Visible = false;
                    dgvDeliverySetup.Columns["RecipientName"].Visible = false;
                    dgvDeliverySetup.Columns["MobileNo"].Visible = false;
                    dgvDeliverySetup.Columns["HomeNo"].Visible = false;
                    dgvDeliverySetup.Columns["DeliveryAddress"].Visible = false;
                    dgvDeliverySetup.Columns["DeliveryDateTime"].Visible = false;
                    dgvDeliverySetup.Columns["Remarks"].Visible = false;

                    dgvDeliverySetup.Columns["ItemNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDeliverySetup.Columns["ItemName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDeliverySetup.Columns["Quantity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDeliverySetup.Columns["ItemPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDeliverySetup.Columns["DepositPaid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    #endregion

                    doc = new DeliveryController(delOrderHdr.OrderNumber);
                    pos = new POSController(delOrderHdr.SalesOrderRefNo);

                    #region *) Fill the form objects
                    lblReceiptNo.Text = doc.myDeliveryOrderHdr.PurchaseOrderRefNo;
                    txtReceiptNumber.Text = new OrderHdr(doc.myDeliveryOrderHdr.SalesOrderRefNo).Userfld5;
                    txtMembershipNo.Text = doc.myDeliveryOrderHdr.MembershipNo;
                    txtName.Text = doc.myDeliveryOrderHdr.RecipientName;
                    txtMobileNo.Text = doc.myDeliveryOrderHdr.MobileNo;
                    txtHomeNo.Text = doc.myDeliveryOrderHdr.HomeNo;
                    txtPostalCode.Text = doc.myDeliveryOrderHdr.PostalCode;
                    txtAddress.Text = doc.myDeliveryOrderHdr.DeliveryAddress;
                    txtUnitNo.Text = doc.myDeliveryOrderHdr.UnitNo;

                    if (doc.myDeliveryOrderHdr.IsVendorDelivery == null || (bool)doc.myDeliveryOrderHdr.IsVendorDelivery == false)
                    {
                        rbNoVendorDelivery.Checked = true;
                    }
                    else
                    {
                        rbYesVendorDelivery.Checked = false;
                    }

                    if (doc.myDeliveryOrderHdr.DeliveryOutlet != null)
                    {
                        cmbDeliveryOutlet.SelectedValue = doc.myDeliveryOrderHdr.DeliveryOutlet;
                    }

                    if (doc.myDeliveryOrderHdr.DeliveryDate.HasValue)
                    {
                        dtpDeliveryDate.Value = doc.myDeliveryOrderHdr.DeliveryDate.Value;
                        dtpDeliveryDate.Checked = true;
                    }
                    else
                    {
                        dtpDeliveryDate.Value = DateTime.Today.AddDays(1);
                        dtpDeliveryDate.Checked = false;
                    }
                    if (doc.myDeliveryOrderHdr.TimeSlotFrom.HasValue && doc.myDeliveryOrderHdr.TimeSlotTo.HasValue)
                    {
                        string timeFrom = doc.myDeliveryOrderHdr.TimeSlotFrom.Value.ToString("HH:mm");
                        string timeTo = doc.myDeliveryOrderHdr.TimeSlotTo.Value.ToString("HH:mm");

                        DataRow[] dr = PointOfSaleInfo.DeliveryTimes.Select(string.Format("Value='{0}-{1}'", timeFrom, timeTo));
                        if (dr.Length > 0)
                        {
                            cmbDeliveryTime.SelectedValue = dr[0]["Value"].ToString();
                        }
                        else
                        {
                            // If not registered in PointOfSaleInfo.DeliveryTimes, then add new list to the combobox
                            string value = timeFrom + "-" + timeTo;
                            string text = doc.myDeliveryOrderHdr.TimeSlotFrom.Value.ToString("h:mmtt").ToLower().Replace(":00", "") +
                                          " - " +
                                          doc.myDeliveryOrderHdr.TimeSlotTo.Value.ToString("h:mmtt").ToLower().Replace(":00", "");
                            DataTable dt = (DataTable)cmbDeliveryTime.DataSource;
                            dt.Rows.Add(value, text);
                            cmbDeliveryTime.SelectedValue = value;
                        }

                        //if (doc.myDeliveryOrderHdr.TimeSlotFrom.Value.Hour == 10 && doc.myDeliveryOrderHdr.TimeSlotTo.Value.Hour == 13)
                        //{
                        //    cmbDeliveryTime.SelectedIndex = 1;
                        //}
                        //else if (doc.myDeliveryOrderHdr.TimeSlotFrom.Value.Hour == 12 && doc.myDeliveryOrderHdr.TimeSlotTo.Value.Hour == 15)
                        //{
                        //    cmbDeliveryTime.SelectedIndex = 2;
                        //}
                        //else if (doc.myDeliveryOrderHdr.TimeSlotFrom.Value.Hour == 14 && doc.myDeliveryOrderHdr.TimeSlotTo.Value.Hour == 17)
                        //{
                        //    cmbDeliveryTime.SelectedIndex = 3;
                        //}
                        //else
                        //{
                        //    cmbDeliveryTime.SelectedIndex = 0;
                        //}
                    }
                    else
                    {
                        cmbDeliveryTime.SelectedIndex = 0;
                    }
                    txtRemarks.Text = doc.myDeliveryOrderHdr.Remark;
                    #endregion

                    #region *) if Delivery Date older than today, disable editing or already delivered
                    if (doc.myDeliveryOrderHdr.DeliveryDate < DateTime.Today || (doc.myDeliveryOrderHdr.IsDelivered != null && (bool)doc.myDeliveryOrderHdr.IsDelivered))
                    {
                        txtName.ReadOnly = true;
                        txtMobileNo.ReadOnly = true;
                        txtHomeNo.ReadOnly = true;
                        txtPostalCode.ReadOnly = true;
                        txtAddress.ReadOnly = true;
                        txtUnitNo.ReadOnly = true;
                        dtpDeliveryDate.Enabled = false;
                        cmbDeliveryTime.Enabled = false;
                        txtRemarks.ReadOnly = true;

                        btnDone.Visible = false;
                        btnClear.Visible = false;
                        btnClose.Left = btnDone.Left;
                    }
                    #endregion
                }
                else
                {
                    lblReceiptNo.Text = doc.myDeliveryOrderHdr.PurchaseOrderRefNo;
                    //loadDeliveryInformation();
                }
            }

            if (!AllowSplitDelivery)
            {
                btnAssign.Visible = false;
            }
            BindGrid();

            //set deposit paid
         }

        

        #region ===== OBSOLETE??? =====
        //private void loadDeliveryInformation()
        //{
        //    //is there any delivery information exist for this particular order?
        //    delOrderHdr = new DeliveryOrder(DeliveryOrder.Columns.SalesOrderRefNo, orderHdrID);                                    
            
        //    OrderDetCollection tmp = new OrderDetCollection();
        //    tmp.Where(OrderDet.Columns.OrderHdrID, orderHdrID);
        //    tmp.Load();
            
        //    dt = tmp.ToDataTable();
        //    dt.Columns.Add("ItemName");
        //    dt.Columns.Add("IsTicked", System.Type.GetType("System.Boolean"));
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        dt.Rows[i]["ItemName"] = tmp[i].Item.ItemName;
        //        dt.Rows[i]["IsTicked"] = false;
        //    }

        //    if (delOrderHdr.IsLoaded && !delOrderHdr.IsNew)
        //    {
        //        IsNew = false;
                
        //        //load the header information.....
        //        if (delOrderHdr.DeliveryDate.HasValue)
        //            dtpDeliveryDate.Value = delOrderHdr.DeliveryDate.Value;

                
        //        txtAddress.Text = delOrderHdr.DeliveryAddress;
        //        //Load Delivery Time...
        //        if (delOrderHdr.TimeSlotFrom.HasValue && delOrderHdr.TimeSlotTo.HasValue)
        //        {
        //            //10am to 13pm
        //            if (delOrderHdr.TimeSlotFrom.Value.Hour == 10 &&
        //                delOrderHdr.TimeSlotFrom.Value.Minute == 00 &&
        //                delOrderHdr.TimeSlotTo.Value.Hour == 13 &&
        //                delOrderHdr.TimeSlotTo.Value.Minute == 00)
        //            {
        //                cmbDeliveryTime.SelectedIndex = 0;
        //            }
        //            else if (delOrderHdr.TimeSlotFrom.Value.Hour == 13 &&
        //                delOrderHdr.TimeSlotFrom.Value.Minute == 00 &&
        //                delOrderHdr.TimeSlotTo.Value.Hour == 15 &&
        //                delOrderHdr.TimeSlotTo.Value.Minute == 00) //13pm -15pm
        //            {
        //                cmbDeliveryTime.SelectedIndex = 1;
        //            }
        //            else if (delOrderHdr.TimeSlotFrom.Value.Hour == 15 &&
        //                delOrderHdr.TimeSlotFrom.Value.Minute == 00 &&
        //                delOrderHdr.TimeSlotTo.Value.Hour == 17 &&
        //                delOrderHdr.TimeSlotTo.Value.Minute == 00) //15pm - 17pm
        //            {
        //                cmbDeliveryTime.SelectedIndex = 2;
        //            }
        //        }

        //        //load the ticks...........
        //        DeliveryOrderDetailCollection tmpSavedDel = delOrderHdr.DeliveryOrderDetails();

        //        for (int j = 0; j < dt.Rows.Count; j++)
        //        {
        //            for (int k = 0; k < tmpSavedDel.Count; k++)
        //            {
        //                if (dt.Rows[j]["OrderDetID"].ToString() == tmpSavedDel[k].OrderDetID)
        //                {
        //                    dt.Rows[j]["IsTicked"] = true;   
        //                }
        //            }
        //        }                
        //    }
        //}
        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            Calculate_AssignedDeposit();

            #region *) Validation
            if (assignedDeposit > depositBalance)
            {
                MessageBox.Show("Total of Assigned Deposit cannot be greater than Deposit Balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.DepositAssignmentValidation), false))
                {
                    if (assignedDeposit < depositBalance && assignedDepositForThisDO < totalItemPrice)
                    {
                        MessageBox.Show("All of the Deposit must be assigned, and total of Assigned Deposit cannot be greater than Deposit Balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            #endregion

            if (IsNew)
            {
                Create_DO();
            }
            else
            {
                Update_DO();
            }

            #region *) Try To Sync to server if is enabled
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncDeliveryOrder), false))
                if (!SyncSendDeliveryOrderThread.IsBusy)
                    SyncSendDeliveryOrderThread.RunWorkerAsync();
            #endregion

            #region *) Old code
            //if (cmbDeliveryTime.SelectedIndex == 0)
            //{
            //    //10am - 1pm
            //    doc.myDeliveryOrderHdr.TimeSlotFrom = dtpDeliveryDate.Value.Date.AddHours(10);
            //    doc.myDeliveryOrderHdr.TimeSlotTo = dtpDeliveryDate.Value.Date.AddHours(13);
            //}
            //else if (cmbDeliveryTime.SelectedIndex == 1)
            //{
            //    //1pm - 3pm
            //    doc.myDeliveryOrderHdr.TimeSlotFrom = dtpDeliveryDate.Value.Date.AddHours(13);
            //    doc.myDeliveryOrderHdr.TimeSlotTo = dtpDeliveryDate.Value.Date.AddHours(15);
            //}
            //else if (cmbDeliveryTime.SelectedIndex == 2)
            //{
            //    //3pm - 5pm
            //    doc.myDeliveryOrderHdr.TimeSlotFrom = dtpDeliveryDate.Value.Date.AddHours(15);
            //    doc.myDeliveryOrderHdr.TimeSlotTo = dtpDeliveryDate.Value.Date.AddHours(17);
            //}
            //else
            //{
            //    MessageBox.Show("Please select a time slot");
            //    return;
            //}
            //doc.myDeliveryOrderHdr.PersonAssigned = -1;
            //doc.myDeliveryOrderHdr.DeliveryDate = dtpDeliveryDate.Value;
            //doc.myDeliveryOrderHdr.DeliveryAddress = txtAddress.Text;

            //if (IsNew)
            //{

            //    DeliveryOrderDetailCollection finalDet;
            //    finalDet = new DeliveryOrderDetailCollection();

            //    DeliveryOrderDetail tmpDet;

            //    //for loop the ticked order.....
            //    for (int i = 0; i < dgvDeliverySetup.Rows.Count; i++)
            //    {
            //        //
            //        if (dgvDeliverySetup.Rows[i].Cells[0].Value != null
            //            && (bool)dgvDeliverySetup.Rows[i].Cells[0].Value == true)
            //        {
            //            tmpDet = new DeliveryOrderDetail();

            //            tmpDet.ItemNo = dgvDeliverySetup.Rows[i].Cells["ItemNo"].Value.ToString();
            //            tmpDet.Quantity = int.Parse(dgvDeliverySetup.Rows[i].Cells["Quantity"].Value.ToString());
            //            tmpDet.Dohdrid = doc.myDeliveryOrderHdr.OrderNumber;
            //            tmpDet.DetailsID = tmpDet.Dohdrid.ToString() + "." + i.ToString();
            //            tmpDet.OrderDetID = dgvDeliverySetup.Rows[i].Cells["OrderDetID"].Value.ToString();
            //            finalDet.Add(tmpDet);
            //        }
            //    }
            //    if (finalDet.Count == 0)
            //    {
            //        MessageBox.Show("No item selected for the delivery");
            //        return;
            //    }
            //    DeliveryOrderController.SaveOrder(doc.myDeliveryOrderHdr, finalDet);

            //    MessageBox.Show("Save Successful.");
            //    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.DO_UseCustomNo), false))
            //    {
            //        CustNumUpdate();//Graham
            //    }
            //    POSDevices.Receipt rcpt = new POSDevices.Receipt();
            //    rcpt.PrintDeliveryOrder(doc.myDeliveryOrderHdr, finalDet, pos);


            //    this.Close();

            //}
            //else
            //{
            //    //Save time slot...                    
            //    DeliveryOrderDetailCollection finalDet;
            //    finalDet = new DeliveryOrderDetailCollection();

            //    DeliveryOrderDetail tmpDet;

            //    //for loop the ticked order.....
            //    for (int i = 0; i < dgvDeliverySetup.Rows.Count; i++)
            //    {
            //        //
            //        if (dgvDeliverySetup.Rows[i].Cells[0].Value != null
            //            && (bool)dgvDeliverySetup.Rows[i].Cells[0].Value == true)
            //        {
            //            tmpDet = new DeliveryOrderDetail();

            //            tmpDet.ItemNo = dgvDeliverySetup.Rows[i].Cells["ItemNo"].Value.ToString();
            //            tmpDet.Quantity = int.Parse(dgvDeliverySetup.Rows[i].Cells["Quantity"].Value.ToString());
            //            tmpDet.Dohdrid = doc.myDeliveryOrderHdr.OrderNumber;
            //            tmpDet.DetailsID = tmpDet.Dohdrid.ToString() + "." + i.ToString();
            //            tmpDet.OrderDetID = dgvDeliverySetup.Rows[i].Cells["OrderDetID"].Value.ToString();
            //            finalDet.Add(tmpDet);
            //        }
            //    }

            //    DeliveryOrderController.UpdateOrder(delOrderHdr, finalDet);
            //    MessageBox.Show("Save Successful.");
            //    /*
            //    POSDevices.Receipt rcpt = new POSDevices.Receipt();
            //    rcpt.PrintDeliveryOrder(delOrderHdr, finalDet, pos);
            //    */

            //    Form[] form = this.MdiParent.MdiChildren;

            //    int count = form.Length;
            //    if (count <= 1)
            //    {
            //        this.MdiParent.Close();
            //    }
            //    this.Close();
            //}
            #endregion
        }

        private void Create_DO()
        {
            try
            {
                int count = 0;

                #region *) For NON split delivery, assign the address etc first
                if (!AllowSplitDelivery)
                {
                    btnAssign_Click(null, null);
                }
                #endregion

                #region *) Show confirmation message if not all items are ticked
                for (var i = 0; i < dgvDeliverySetup.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvDeliverySetup.Rows[i];
                    if (row.Cells["cbColumn"].Value != null && row.Cells["cbColumn"].Value != DBNull.Value && (bool)row.Cells["cbColumn"].Value == true)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    MessageBox.Show("No item selected for the delivery.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (count < dgvDeliverySetup.Rows.Count)  // Not all items are ticked. Show confirmation message
                {
                    if (MessageBox.Show("Not all items are ticked. Are you sure you want to continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        return;
                }
                #endregion

                DataTable tmpDT = (DataTable)dgvDeliverySetup.DataSource;
                DataView dv = new DataView(tmpDT);
                dv.Sort = "RecipientName, MobileNo, HomeNo, PostalCode, DeliveryAddress, UnitNo, DeliveryDate, DeliveryTime, Remarks";
                tmpDT = dv.ToTable();

                int hdrNo = 0;
                int detNo = 0;
                string str = null;
                delOrderHdrColl = new DeliveryOrderCollection();
                delOrderDetColl = new DeliveryOrderDetailCollection();

                for (int i = 0; i < tmpDT.Rows.Count; i++)
                {
                    DataRow dr = tmpDT.Rows[i];

                    if (dr["IsTicked"] == null || dr["IsTicked"] == DBNull.Value)
                        dr["IsTicked"] = false;

                    string newstr = dr["RecipientName"].ToString().Trim() + dr["MobileNo"].ToString().Trim() +
                                 dr["HomeNo"].ToString().Trim() + dr["PostalCode"].ToString().Trim() +
                                 dr["DeliveryAddress"].ToString().Trim() + dr["UnitNo"].ToString().Trim() +
                                 dr["DeliveryDate"].ToString().Trim() + dr["DeliveryTime"].ToString().Trim() +
                                 dr["Remarks"].ToString().Trim();

                    if ((bool)dr["IsTicked"] == true) //  && !string.IsNullOrEmpty(newstr)
                    {
                        // If Recipient Name or Address or other fields is different, create new Header
                        if (str != newstr)
                        {
                            str = newstr;
                            hdrNo++;
                            detNo = 0;

                            DeliveryOrder doHdr = new DeliveryOrder();
                            doHdr.CopyFrom(doc.myDeliveryOrderHdr);
                            doHdr.OrderNumber = hdrNo.ToString();
                            doHdr.PersonAssigned = -1;
                            doHdr.RecipientName = string.IsNullOrEmpty(dr["RecipientName"].ToString().Trim()) ? txtName.Text : dr["RecipientName"].ToString().Trim();
                            doHdr.MobileNo = string.IsNullOrEmpty(dr["MobileNo"].ToString().Trim()) ? txtMobileNo.Text : dr["MobileNo"].ToString().Trim();
                            doHdr.HomeNo = string.IsNullOrEmpty(dr["HomeNo"].ToString().Trim()) ? txtHomeNo.Text : dr["HomeNo"].ToString().Trim();
                            doHdr.PostalCode = string.IsNullOrEmpty(dr["PostalCode"].ToString().Trim()) ? txtPostalCode.Text : dr["PostalCode"].ToString().Trim();
                            doHdr.DeliveryAddress = string.IsNullOrEmpty(dr["DeliveryAddress"].ToString().Trim()) ? txtAddress.Text : dr["DeliveryAddress"].ToString().Trim();
                            doHdr.UnitNo = string.IsNullOrEmpty(dr["UnitNo"].ToString().Trim()) ? txtUnitNo.Text : dr["UnitNo"].ToString().Trim();
                            doHdr.Remark = string.IsNullOrEmpty(dr["Remarks"].ToString().Trim()) ? txtRemarks.Text : dr["Remarks"].ToString().Trim();
                            doHdr.IsVendorDelivery = false;
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowVendorDelivery), false))
                            {
                                if (rbYesVendorDelivery.Checked)
                                {
                                    doHdr.IsVendorDelivery = true;
                                }
                            }

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowDeliveryOutlet), false))
                            {
                                doHdr.DeliveryOutlet = cmbDeliveryOutlet.SelectedItem.ToString();
                            }

                            DateTime deliveryDate;
                            if (DateTime.TryParse(dr["DeliveryDate"].ToString(), out deliveryDate))
                            {
                                deliveryDate = deliveryDate.Date;   // Make sure we only get the date, not including the time
                                doHdr.DeliveryDate = deliveryDate;
                            }
                            else
                            {
                                doHdr.DeliveryDate = null;
                                deliveryDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value.Date;
                            }

                            string deliveryTime = dr["DeliveryTime"].ToString().Trim();
                            if (string.IsNullOrEmpty(deliveryTime))
                            {
                                doHdr.TimeSlotFrom = null;
                                doHdr.TimeSlotTo = null;
                            }
                            else
                            {
                                string timeFrom = deliveryTime.Split('-')[0].Trim();
                                string timeTo = deliveryTime.Split('-')[1].Trim();
                                doHdr.TimeSlotFrom = deliveryDate.Add(TimeSpan.Parse(timeFrom));
                                doHdr.TimeSlotTo = deliveryDate.Add(TimeSpan.Parse(timeTo));
                            }

                            //string deliveryTime = dr["DeliveryTime"].ToString().Trim();
                            //if (deliveryTime == "10am - 1pm")
                            //{
                            //    doHdr.TimeSlotFrom = deliveryDate.AddHours(10);
                            //    doHdr.TimeSlotTo = deliveryDate.AddHours(13);
                            //}
                            //else if (deliveryTime == "12pm - 3pm")
                            //{
                            //    doHdr.TimeSlotFrom = deliveryDate.AddHours(12);
                            //    doHdr.TimeSlotTo = deliveryDate.AddHours(15);
                            //}
                            //else if (deliveryTime == "2pm - 5pm")
                            //{
                            //    doHdr.TimeSlotFrom = deliveryDate.AddHours(14);
                            //    doHdr.TimeSlotTo = deliveryDate.AddHours(17);
                            //}

                            delOrderHdrColl.Add(doHdr);
                        }

                        DeliveryOrderDetail doDet = new DeliveryOrderDetail();
                        doDet.Dohdrid = hdrNo.ToString();
                        doDet.ItemNo = dr["ItemNo"].ToString();
                        doDet.Quantity = decimal.Parse(dr["Quantity"].ToString());
                        doDet.DetailsID = doDet.Dohdrid + "." + detNo.ToString();
                        doDet.OrderDetID = dr["OrderDetID"].ToString();

                        delOrderDetColl.Add(doDet);
                    }
                }

                if (IsUnsavedOrder)
                {
                    this.Close();
                }
                else
                {
                    // Save the Delivery Order
                    DeliveryOrderController.SaveMultipleOrder(ref delOrderHdrColl, ref delOrderDetColl);

                    // Update OrderDet.DepositAmount
                    pos.myOrderDet.SaveAll();

                    MessageBox.Show("Save Successful.");

                    // Print the DO
                    foreach (DeliveryOrder doHdr in delOrderHdrColl)
                    {
                        DeliveryOrderDetailCollection doDets = new DeliveryOrderDetailCollection();
                        doDets = delOrderDetColl.Clone().Where("Dohdrid", doHdr.OrderNumber).Filter();

                        POSDevices.Receipt rcpt = new POSDevices.Receipt();
                        rcpt.PrintDeliveryOrder(doHdr, doDets, pos);
                        AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Create DO : {0}", doHdr.OrderNumber), "");
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed. Error encountered: " + ex.Message);
                Logger.writeLog(ex);
            }
        }

        private void Update_DO()
        {
            try
            {
                DeliveryOrder doHdr = doc.myDeliveryOrderHdr;
                doHdr.RecipientName = txtName.Text.Trim();
                doHdr.MobileNo = txtMobileNo.Text.Trim();
                doHdr.HomeNo = txtHomeNo.Text.Trim();
                doHdr.PostalCode = txtPostalCode.Text.Trim();
                doHdr.DeliveryAddress = txtAddress.Text.Trim();
                doHdr.UnitNo = txtUnitNo.Text.Trim();
                doHdr.Remark = txtRemarks.Text.Trim();

                DateTime deliveryDate = dtpDeliveryDate.Value.Date;   // Make sure we only get the date, not including the time
                if (dtpDeliveryDate.Checked)
                {
                    doHdr.DeliveryDate = deliveryDate;
                }
                else
                {
                    doHdr.DeliveryDate = null;
                    deliveryDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value.Date;
                    //cmbDeliveryTime.SelectedIndex = 0;  // Make the time slot null too
                }

                string deliveryTime = cmbDeliveryTime.SelectedValue.ToString();
                if (string.IsNullOrEmpty(deliveryTime))
                {
                    doHdr.TimeSlotFrom = null;
                    doHdr.TimeSlotTo = null;
                }
                else
                {
                    string timeFrom = deliveryTime.Split('-')[0].Trim();
                    string timeTo = deliveryTime.Split('-')[1].Trim();
                    doHdr.TimeSlotFrom = deliveryDate.Add(TimeSpan.Parse(timeFrom));
                    doHdr.TimeSlotTo = deliveryDate.Add(TimeSpan.Parse(timeTo));
                }

                doHdr.IsVendorDelivery = false;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowVendorDelivery), false))
                {
                    if (rbYesVendorDelivery.Checked)
                    {
                        doHdr.IsVendorDelivery = true;
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowDeliveryOutlet), false))
                {
                    doHdr.DeliveryOutlet = cmbDeliveryOutlet.SelectedItem.ToString();
                }


                //if (deliveryTime == "10am - 1pm")
                //{
                //    doHdr.TimeSlotFrom = deliveryDate.AddHours(10);
                //    doHdr.TimeSlotTo = deliveryDate.AddHours(13);
                //}
                //else if (deliveryTime == "12pm - 3pm")
                //{
                //    doHdr.TimeSlotFrom = deliveryDate.AddHours(12);
                //    doHdr.TimeSlotTo = deliveryDate.AddHours(15);
                //}
                //else if (deliveryTime == "2pm - 5pm")
                //{
                //    doHdr.TimeSlotFrom = deliveryDate.AddHours(14);
                //    doHdr.TimeSlotTo = deliveryDate.AddHours(17);
                //}
                //else
                //{
                //    doHdr.TimeSlotFrom = null;
                //    doHdr.TimeSlotTo = null;
                //}

                // Save the Delivery Order
                DeliveryOrderController.UpdateOrder(doHdr, doc.myDeliveryOrderDet);
                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Update DO : {0}", doHdr.OrderNumber), "");
                // Update OrderDet.DepositAmount
                pos.myOrderDet.SaveAll();

                MessageBox.Show("Save Successful.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed. Error encountered: " + ex.Message);
                Logger.writeLog(ex);
            }
        }

        private void CustNumUpdate()
        {
            #region customNo Update
            int runningNo = 0;

            string selectmaxno = "select AppSettingValue from AppSetting where AppSettingKey='DO_CurrentReceiptNo'";
            string currentReceiptNo = DataService.ExecuteScalar(new QueryCommand(selectmaxno)).ToString();

            int.TryParse(currentReceiptNo, out runningNo);

            string updatemaxnum1 = "update appsetting set AppSettingValue='" + ++runningNo + "' where AppSettingKey='DO_CurrentReceiptNo'";
            DataService.ExecuteQuery(new QueryCommand(updatemaxnum1));

            //default max receiptno is 4
            string sql2 = "select case AppSettingValue when '' then '4' else AppSettingValue end from AppSetting where AppSettingKey='DO_ReceiptLength'";
            QueryCommand Qcmd2 = new QueryCommand(sql2);
            int maxReceiptNo = 4;
            int.TryParse(DataService.ExecuteScalar(Qcmd2).ToString(), out maxReceiptNo);

            //check if it has reached the max no for that digit (99,999,9999.. etc)
            bool maximumReached = false;
            if (maxReceiptNo != 0)
            {
                maximumReached = true;
                for (int i = 0; i < maxReceiptNo; i++)
                {
                    if (currentReceiptNo[i] != '9')
                    {
                        maximumReached = false;
                        break;
                    }
                }

            }

            //if it has reached, update the maxreceiptno
            if (maximumReached)
            {
                string sql3 = "update appsetting set AppSettingValue = " + ++maxReceiptNo + " where AppSettingKey='DO_ReceiptLength'";
                QueryCommand Qcmd3 = new QueryCommand(sql3);
                DataService.ExecuteQuery(Qcmd3);
            }
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtReceiptNumber_Leave(object sender, EventArgs e)
        {
            if (txtReceiptNumber.Modified)
            {
                txtReceiptNumber.Modified = false;

                if (!LoadNewDelivery(txtReceiptNumber.Text))
                {
                    MessageBox.Show("Delivery Order Failed To Load");
                    return;
                }

                //Load membership information
                myMember = doc.getMemberInfo();
                Load_Membership_Info();

                BindGrid(); 
            }
        }

        private bool LoadNewDelivery(string orderhdrid)
        {
            string status = "";
            doc = new DeliveryController();

            /*check if there delivery order exist for receipt number*/
            
            OrderHdrCollection oh = new OrderHdrCollection();
            oh.Where(OrderHdr.Columns.Userfld5, orderhdrid);
            oh.Load();
            if (oh.Count > 0)
            {
                var docol = new DeliveryOrderCollection();
                docol.Where(DeliveryOrder.Columns.SalesOrderRefNo, oh[0].OrderHdrID);
                docol.Where(DeliveryOrder.Columns.Deleted, false);
                docol.Load();

                if (docol.Count() > 0)
                {
                    MessageBox.Show("Create Delivery Order failed. Delivery Order for Receipt Number " + orderhdrid + " are already exist.");
                    txtReceiptNumber.Text = "";
                    return false;
                }

                doc.LoadFromPOS(oh[0].OrderHdrID, out status);
                pos = new POSController(oh[0].OrderHdrID);
            }
            else
            {
                var docol = new DeliveryOrderCollection();
                docol.Where(DeliveryOrder.Columns.SalesOrderRefNo, orderhdrid);
                docol.Where(DeliveryOrder.Columns.Deleted, false);
                docol.Load();

                if (docol.Count() > 0)
                {
                    MessageBox.Show("Create Delivery Order failed. Delivery Order for Receipt Number " + orderhdrid + " are already exist.");
                    txtReceiptNumber.Text = "";
                    return false;
                }

                doc.LoadFromPOS(orderhdrid, out status);
                pos = new POSController(orderhdrid);
            }

            if (status != "")
            {
                MessageBox.Show("Load Failed. " + status);
                return false;
            }
            
            return true;
        }

        private void btnChooseMember_Click(object sender, EventArgs e)
        {
            if (txtMembershipNo.Text == "")
            {
                return;
            }
            frmAddMember f = new frmAddMember();
            f.searchReq = txtMembershipNo.Text;// txtSearch.Text;
            f.ShowDialog();
            string tmpMembershipNo = f.membershipNo;
            if (tmpMembershipNo != "")
            {
                bool hasExpired;
                DateTime ExpiryDate;
                if (MembershipController.IsExistingMember(tmpMembershipNo, out hasExpired, out ExpiryDate))
                {
                    addMemberToDelivery(tmpMembershipNo);
                }
                
                           
            }
            f.Dispose();
            
        }

        private void addMemberToDelivery(string tmpMembershipNo)
        {
            if (doc.AssignMembership(tmpMembershipNo))
            {
                txtMembershipNo.Text = tmpMembershipNo;
                txtName.Text = doc.getMemberInfo().NameToAppear;
            }
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                for (var i = 0; i < dgvDeliverySetup.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvDeliverySetup.Rows[i];
                    if (row.Cells["cbColumn"].Value != null && row.Cells["cbColumn"].Value != DBNull.Value && (bool)row.Cells["cbColumn"].Value == true)
                    {
                        row.Cells["RecipientName"].Value = txtName.Text;
                        row.Cells["MobileNo"].Value = txtMobileNo.Text;
                        row.Cells["HomeNo"].Value = txtHomeNo.Text;
                        row.Cells["PostalCode"].Value = txtPostalCode.Text;
                        row.Cells["DeliveryAddress"].Value = txtAddress.Text;
                        row.Cells["UnitNo"].Value = txtUnitNo.Text;
                        row.Cells["DeliveryDate"].Value = dtpDeliveryDate.Checked ? dtpDeliveryDate.Value.ToString("yyyy-MM-dd") : "";
                        row.Cells["DeliveryTime"].Value = cmbDeliveryTime.SelectedValue.ToString();
                        row.Cells["DeliveryDateTime"].Value = (dtpDeliveryDate.Checked ? dtpDeliveryDate.Value.ToString("dd/MM/yyyy") : "") + "\r\n" + cmbDeliveryTime.Text;
                        row.Cells["Remarks"].Value = txtRemarks.Text;
                        count++;
                    }
                }
                if (count == 0) MessageBox.Show("Please tick the items that will be assigned.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(
                    "Some error occurred. Please contact your administrator.", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtMobileNo.Text = "";
            txtHomeNo.Text = "";
            txtPostalCode.Text = "";
            txtAddress.Text = "";
            txtUnitNo.Text = "";
            dtpDeliveryDate.Value = DateTime.Today.AddDays(1);
            dtpDeliveryDate.Checked = false;
            cmbDeliveryTime.SelectedIndex = 0;
            txtRemarks.Text = "";
        }

        private void UpdateDeliveryAddress(PostalCodeDB postalCode)
        {
            txtAddress.Text = "";
            if (!string.IsNullOrEmpty(postalCode.Area1))
            {
                if (txtUnitNo.Text == "")
                    txtAddress.Text += postalCode.Area1 + Environment.NewLine;
                else
                    txtAddress.Text += postalCode.Area1 + " " + txtUnitNo.Text + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(postalCode.Area2))
            {
                if (txtAddress.Text == "")
                    txtAddress.Text += postalCode.Area2 + " " + txtUnitNo.Text + Environment.NewLine;
                else
                    txtAddress.Text += postalCode.Area2 + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(postalCode.City))
                txtAddress.Text += postalCode.City + " ";
            if (!string.IsNullOrEmpty(txtAddress.Text))
                txtAddress.Text += postalCode.Zip;

        }

        private void txtPostalCode_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtPostalCode.Modified) 
                {
                    if (txtAddress.Text != ""){
                        if (MessageBox.Show("Do you want to replace the address?", "Replace Address", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            txtPostalCode.Modified = false;
                            return;
                        }
                    }

                    if (string.IsNullOrEmpty(txtPostalCode.Text))
                    {
                        txtAddress.Text = "";
                        txtPostalCode.Tag = null;
                        txtPostalCode.Modified = false;
                    }
                    else
                    {
                        PostalCodeDB postalCode = new PostalCodeDB(txtPostalCode.Text);
                        if (string.IsNullOrEmpty(postalCode.Zip))
                        {
                            MessageBox.Show("Postal Code not found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtPostalCode.Tag = null;
                            txtAddress.Text = "";
                        }
                        else
                        {
                            UpdateDeliveryAddress(postalCode);
                            txtPostalCode.Tag = postalCode.City;
                            txtPostalCode.Modified = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(
                    "Some error occurred. Please contact your administrator.", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUnitNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtUnitNo.Modified)
                {
                    if (txtPostalCode.Text != "")
                    {
                        PostalCodeDB postalCode = new PostalCodeDB(txtPostalCode.Text);
                        if (!string.IsNullOrEmpty(postalCode.Zip))
                        {
                            UpdateDeliveryAddress(postalCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(
                    "Some error occurred. Please contact your administrator.", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpDeliveryDate_MouseUp(object sender, MouseEventArgs e)
        {
            //if (!dtpDeliveryDate.Checked) cmbDeliveryTime.SelectedIndex = 0;
        }

        private void dtpDeliveryDate_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!dtpDeliveryDate.Checked) cmbDeliveryTime.SelectedIndex = 0;
        }

        private void dgvDeliverySetup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string status = "";
                if (doc.myDeliveryOrderHdr.DeliveryDate.GetValueOrDefault(DateTime.MaxValue) >= DateTime.Today)
                {
                    if (e.RowIndex >= 0 && e.ColumnIndex == dgvDeliverySetup.Columns["DepositPaid"].Index)
                    {
                        //prompt keypad
                        frmKeypad f = new frmKeypad();
                        f.IsInteger = false;
                        f.initialValue = dgvDeliverySetup.CurrentCell.Value == null ? "0" : dgvDeliverySetup.CurrentCell.Value.ToString();
                        f.ShowDialog();

                        decimal deposit = decimal.Parse(f.value.ToString());
                        if (deposit > decimal.Parse(dgvDeliverySetup.CurrentRow.Cells["ItemPrice"].Value.ToString()))
                        {
                            MessageBox.Show("Deposit cannot be greater than Item Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            dgvDeliverySetup.CurrentCell.Value = decimal.Parse(f.value.ToString());
                            OrderDet od = pos.GetLine(dgvDeliverySetup.CurrentRow.Cells["OrderDetID"].Value.ToString(), out status);
                            if (od != null)
                            {
                                od.DepositAmount = decimal.Parse(f.value.ToString());
                            }
                            Calculate_AssignedDeposit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
