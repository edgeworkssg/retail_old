using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using LanguageManager = WinPowerPOS.Properties.Language;
using SubSonic;
using PowerPOS.Container;
using WinPowerPOS.LoginForms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmDiscountSpecial : Form
    {
        public bool IsSuccessful;
        public decimal DiscApplied;
        public string DiscChoosed;

        private POSController pos;
        private string LineID = "";
        private bool IsApplyToAll = false;
        private bool IsMultiTierPrice = false;
        private bool IsPromtPasswordOnDiscLimit = false;
        private bool UseDiscountReason = false;
        private bool EnableSecondDiscount = false;


        public frmDiscountSpecial(POSController pos, string lineID, bool isApplyToAll, bool isMultiTierPrice)
        {
            InitializeComponent();
            this.pos = pos;
            this.LineID = lineID;
            this.IsApplyToAll = isApplyToAll;
            this.IsMultiTierPrice = isMultiTierPrice;
            IsPromtPasswordOnDiscLimit = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PromtPasswordOnDiscLimit), true);
            UseDiscountReason = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseDiscountReason), false);
            EnableSecondDiscount = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableSecondDiscount), false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void frmDiscounts_Load(object sender, EventArgs e)
        {
            pnlAddDiscount.Visible = EnableSecondDiscount && !IsMultiTierPrice;
            btnDiscDollar.Visible = !IsMultiTierPrice;
            btnDiscPercent.Visible = !IsMultiTierPrice;
            btnDiscount.Text = IsMultiTierPrice ? "DEFAULT PRICE" : "NO DISCOUNT";
            lblTitle.Text = IsMultiTierPrice ? "MULTI-TIER PRICING : " : "DISCOUNT : ";
            if (IsApplyToAll)
            {
                btnDiscount.Text = "NO DISCOUNT";
                lblTitle.Text = "DISCOUNT : ";
                btnAddDiscDollar.Visible = false;
                btnFinalPrice.Visible = false;
            }


            //load category list from database
            SpecialDiscountCollection sp = new SpecialDiscountCollection();
            string query = @"DECLARE @IsMultiTierPrice BIT;
                            DECLARE @IsApplyToAll BIT;

                            SET @IsMultiTierPrice = {0};

                            SELECT  SD.*
                            FROM	SpecialDiscounts SD
                            WHERE	SD.Deleted = 0
		                            AND SD.Enabled = 1
		                            AND SD.StartDate <= GETDATE()
		                            AND SD.EndDate >= DATEADD(DAY,-1,GETDATE())
		                            AND SD.isBankPromo = 0
		                            AND ((@IsMultiTierPrice = 1 AND SD.DiscountName IN ('P1','P2','P3','P4','P5'))
			                            OR (@IsMultiTierPrice = 0 AND SD.DiscountName NOT IN ('P1','P2','P3','P4','P5')))
                            ORDER BY SD.PriorityLevel ";
            query = string.Format(query, IsMultiTierPrice ? 1 : 0);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(query)));
            sp.Load(dt);

            //populate the flow lay out with programmable button
            for (int i = 0; i < sp.Count; i++)
            {
                Button b = new Button();
                b.Width = 110;
                b.Height = 45;
                b.BackColor = System.Drawing.Color.Orange;
                b.Font = new Font("Verdana", 9, FontStyle.Bold);

                if (sp[i].ShowLabel)
                {
                    b.Text = String.IsNullOrEmpty(sp[i].DiscountLabel) ? sp[i].DiscountName : sp[i].DiscountLabel;
                    if (IsMultiTierPrice)
                        b.Text = getNewButtonNameForMultiTier(b.Text, sp[i].DiscountName);

                }
                else
                {
                    b.Text = sp[i].DiscountPercentage.ToString("N2") + "%";
                }
                b.Tag = sp[i].DiscountName;

                b.Click += delegate
                {
                    btnDiscount_Click(b, new EventArgs());
                };
                flowLayoutPanel1.Controls.Add(b);
            }
        }

        private string getNewButtonNameForMultiTier(string oldText, string discName)
        {
            if (!IsMultiTierPrice)
                return oldText;

            if (String.IsNullOrEmpty(LineID))
                return oldText;

            if (discName != "P1"
                                && discName != "P2"
                                && discName != "P3"
                                && discName != "P4"
                                && discName != "P5")
                return oldText;


            string stat = "";
            OrderDet od = pos.GetLine(LineID, out stat);
            if (od == null)
                return oldText;

            if (String.IsNullOrEmpty(od.ItemNo))
                return oldText;

            Item i = new Item(od.ItemNo);

            if (discName == "P1")
                return oldText + Environment.NewLine + "(" + i.Userfloat6.GetValueOrDefault(0).ToString("N2") + ")";

            if (discName == "P2")
                return oldText + Environment.NewLine + "(" + i.Userfloat7.GetValueOrDefault(0).ToString("N2") + ")";
            if (discName == "P3")
                return oldText + Environment.NewLine + "(" + i.Userfloat8.GetValueOrDefault(0).ToString("N2") + ")";
            if (discName == "P4")
                return oldText + Environment.NewLine + "(" + i.Userfloat9.GetValueOrDefault(0).ToString("N2") + ")";
            if (discName == "P5")
                return oldText + Environment.NewLine + "(" + i.Userfloat10.GetValueOrDefault(0).ToString("N2") + ")";

            return oldText;
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                string DiscountName = ((Button)sender).Tag.ToString();
                DiscChoosed = DiscountName;

                #region *) Fetch Discount Detail
                string Discount1 = "";
                SpecialDiscount spDisc = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, DiscountName);
                if (!spDisc.IsNew)
                {
                    if (spDisc.DiscountName != "P1"
                        && spDisc.DiscountName != "P2"
                        && spDisc.DiscountName != "P3"
                        && spDisc.DiscountName != "P4"
                        && spDisc.DiscountName != "P5")
                    {
                        Discount1 = string.Format("{0}%", spDisc.DiscountPercentage.ToString("N2"));
                    }
                }

                #endregion

                string status = "";
                bool isSuccess = false;
                string authorizedBy = "";
                string discountReason = "";
                if (IsApplyToAll)
                {
                    #region *) Check Privileges To All
                    bool isAllowed = false;
                    if (string.IsNullOrEmpty(Discount1))
                        isAllowed = pos.CheckPriceLevelPrivilegesToAll(UserInfo.username, DiscountName, out status);
                    else
                        isAllowed = pos.CheckDiscount1PrivilegesToAll(UserInfo.username, Discount1, out status);
                    authorizedBy = UserInfo.username;
                    if (!isAllowed)
                    {
                        if (IsPromtPasswordOnDiscLimit)
                        {
                            MessageBox.Show(status);
                            frmSupervisorLogin fSpv = new frmSupervisorLogin();
                            fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                            fSpv.ShowDialog();
                            if (fSpv.IsAuthorized)
                            {
                                if (string.IsNullOrEmpty(Discount1))
                                    isAllowed = pos.CheckPriceLevelPrivilegesToAll(fSpv.mySupervisor, DiscountName, out status);
                                else
                                    isAllowed = pos.CheckDiscount1PrivilegesToAll(fSpv.mySupervisor, Discount1, out status);
                                if (isAllowed)
                                    authorizedBy = fSpv.mySupervisor;
                            }
                        }
                        if (!isAllowed)
                        {
                            MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                            return;
                        }
                    }
                    #endregion

                    #region *) Discount Reason
                    if (UseDiscountReason && DiscountName != "0" && !IsMultiTierPrice)
                    {
                        frmDiscountReason frmDiscReason = new frmDiscountReason();
                        frmDiscReason.ShowDialog();
                        if (frmDiscReason.IsSuccessful)
                            discountReason = frmDiscReason.DiscountReason;
                        else
                            return;
                    }
                    #endregion

                    List<string> listDetID = new List<string>();
                    for (int i = 0; i < pos.myOrderDet.Count; i++)
                    {
                        listDetID.Add(pos.myOrderDet[i].OrderDetID);
                    }

                    List<string> listStatus = new List<string>();
                    string linediscountOrderDetID = "";
                    for (int i = 0; i < listDetID.Count; i++)
                    {
                        //Made like this to cater with the sorting in the promo campaign checking
                        OrderDet tempDet = new OrderDet();
                        for (int j = 0; j < pos.myOrderDet.Count; j++)
                            if (listDetID[i] == pos.myOrderDet[j].OrderDetID)
                                tempDet = pos.myOrderDet[j];
                        try
                        {
                            #region *) Validation
                            //Logger.writeLog(tempDet.ItemNo + "," + tempDet.UnitPrice.ToString() + "," + tempDet.SpecialDiscount); 
                            if (tempDet.UnitPrice == 0)
                            {
                                throw new Exception("Cannot apply discount to $0 item");
                            }
                            if (tempDet.Item.IsNonDiscountable)
                            {
                                continue;
                            }

                            if (DiscountName != "P1"
                                && DiscountName != "P2"
                                && DiscountName != "P3"
                                && DiscountName != "P4"
                                && DiscountName != "P5"
                                && DiscountName != "0"
                                && (tempDet.SpecialDiscount == "P1"
                                || tempDet.SpecialDiscount == "P2"
                                || tempDet.SpecialDiscount == "P3"
                                || tempDet.SpecialDiscount == "P4"
                                || tempDet.SpecialDiscount == "P5"))
                            {
                                throw new Exception("Discount cannot overwrite Multi Tier Price");
                            }
                            if (tempDet.IsSpecial == true && DiscountName != "0")
                                continue;
                            #endregion

                            pos.ApplyDiscountOrderDet(false, DiscountName, ref tempDet);
                            pos.ApplyAuthorizedBy(tempDet.OrderDetID, authorizedBy);
                            pos.ApplyDiscountReason(tempDet.OrderDetID, discountReason);
                            if (DiscountName == "0" && tempDet.ItemNo == "LINE_DISCOUNT")
                            {
                                linediscountOrderDetID = tempDet.OrderDetID;
                            }

                        }
                        catch (Exception ex)
                        {
                            listStatus.Add(string.Format("Error giving Item:{0} Discount. {1}", tempDet.ItemNo, ex.Message));
                            Logger.writeLog(ex);
                        }
                    }
                    if (linediscountOrderDetID != "")
                    {
                        OrderDet od = (OrderDet)pos.myOrderDet.Find(linediscountOrderDetID);
                        pos.myOrderDet.Remove(od);
                    }

                    ApplyPromotionController promoCtrl = new ApplyPromotionController(pos);
                    promoCtrl.UndoPromoToOrder();
                    promoCtrl.ApplyPromoToOrder();

                    isSuccess = listStatus.Count == 0;
                    if (!isSuccess)
                        status = string.Join("\n", listStatus.ToArray());
                }
                else
                {
                    #region *) Validation
                    OrderDet tempDet = pos.GetLine(LineID, out status);
                    if (tempDet.UnitPrice == 0)
                    {
                        //MessageBox.Show("Cannot apply discount to $0 item");
                        this.Close();
                        return;
                    }
                    if (tempDet.Item.IsNonDiscountable)
                    {
                        //MessageBox.Show("Cannot apply discount to non discountable item");
                        this.Close();
                        return;
                    }
                    if (DiscountName != "P1"
                        && DiscountName != "P2"
                        && DiscountName != "P3"
                        && DiscountName != "P4"
                        && DiscountName != "P5"
                        && DiscountName != "0"
                        && (tempDet.SpecialDiscount == "P1"
                        || tempDet.SpecialDiscount == "P2"
                        || tempDet.SpecialDiscount == "P3"
                        || tempDet.SpecialDiscount == "P4"
                        || tempDet.SpecialDiscount == "P5"))
                    {
                        MessageBox.Show(string.Format("Error giving Item:{0} Discount. Discount cannot overwrite Multi Tier Price", tempDet.ItemNo));
                        return;
                    }
                    // No Validation to check if got existing discount on item discount level 
                    /*if (tempDet.IsSpecial == true  && DiscountName != "0")
                    {
                        MessageBox.Show("Cannot apply discount to item with existing discount");
                        return;
                    }*/
                    #endregion

                    #region *) Check Privileges To Single LineID
                    bool isAllowed = false;
                    if (string.IsNullOrEmpty(Discount1))
                        isAllowed = pos.CheckPriceLevelPrivileges(UserInfo.username, LineID, DiscountName, out status);
                    else
                        isAllowed = pos.CheckDiscount1Privileges(UserInfo.username, LineID, Discount1, out status);

                    authorizedBy = UserInfo.username;
                    if (!isAllowed)
                    {
                        if (IsPromtPasswordOnDiscLimit)
                        {
                            MessageBox.Show(status);
                            frmSupervisorLogin fSpv = new frmSupervisorLogin();
                            fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                            fSpv.ShowDialog();
                            if (fSpv.IsAuthorized)
                            {
                                if (string.IsNullOrEmpty(Discount1))
                                    isAllowed = pos.CheckPriceLevelPrivileges(fSpv.mySupervisor, LineID, DiscountName, out status);
                                else
                                    isAllowed = pos.CheckDiscount1Privileges(fSpv.mySupervisor, LineID, Discount1, out status);
                                if (isAllowed)
                                    authorizedBy = fSpv.mySupervisor;
                            }
                        }
                        if (!isAllowed)
                        {
                            MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                            return;
                        }
                    }
                    #endregion

                    #region *) Discount Reason
                    if (UseDiscountReason && DiscountName != "0" && !IsMultiTierPrice)
                    {
                        frmDiscountReason frmDiscReason = new frmDiscountReason();
                        frmDiscReason.ShowDialog();
                        if (frmDiscReason.IsSuccessful)
                            discountReason = frmDiscReason.DiscountReason;
                        else
                            return;
                    }
                    #endregion

                    try
                    {
                        isSuccess = pos.ApplyDiscountOrderDet(DiscountName, ref tempDet);
                        if (isSuccess)
                        {
                            pos.ApplyAuthorizedBy(LineID, authorizedBy);
                            pos.ApplyDiscountReason(LineID, discountReason);
                        }
                        if (tempDet.ItemNo == "LINE_DISCOUNT")
                        {
                            pos.myOrderDet.Remove(tempDet);
                        }
                    }
                    catch (Exception ex)
                    {
                        status = ex.Message;
                        isSuccess = false;
                        Logger.writeLog(ex);
                    }
                }

                if (!isSuccess)
                    MessageBox.Show(status);

                IsSuccessful = true;
                this.Close();
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
                IsSuccessful = false;
            }
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            frmKeypad f = new frmKeypad();
            f.IsInteger = false;
            f.ShowDialog();
            decimal discount;

            if (!string.IsNullOrEmpty(f.value) && decimal.TryParse(f.value.ToString(), out discount))
            {
                string Discount1 = "";
                if (tag == "$")
                    Discount1 = "$" + discount.ToString("N2");
                else if (tag == "%")
                    Discount1 = discount.ToString("N2") + "%";

                string status = "";
                bool success = false;
                string authorizedBy = "";
                string discountReason = "";

                if (IsApplyToAll)
                {




                    List<string> listStatus = new List<string>();
                    if (tag != "$")
                    {
                        #region *) Check Privileges To All
                        bool isAllowed = pos.CheckDiscount1PrivilegesToAll(UserInfo.username, Discount1, out status);
                        authorizedBy = UserInfo.username;
                        if (!isAllowed)
                        {
                            if (IsPromtPasswordOnDiscLimit)
                            {
                                MessageBox.Show(status);
                                frmSupervisorLogin fSpv = new frmSupervisorLogin();
                                fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                                fSpv.ShowDialog();
                                if (fSpv.IsAuthorized)
                                {
                                    isAllowed = pos.CheckDiscount1PrivilegesToAll(fSpv.mySupervisor, Discount1, out status);
                                    if (isAllowed)
                                        authorizedBy = fSpv.mySupervisor;
                                }
                            }
                            if (!isAllowed)
                            {
                                MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                                return;
                            }
                        }
                        #endregion

                        #region *) Discount Reason
                        if (UseDiscountReason && (Discount1 != "0%" || Discount1 != "$0"))
                        {
                            frmDiscountReason frmDiscReason = new frmDiscountReason();
                            frmDiscReason.ShowDialog();
                            if (frmDiscReason.IsSuccessful)
                                discountReason = frmDiscReason.DiscountReason;
                            else
                                return;
                        }
                        #endregion

                        for (int i = 0; i < pos.myOrderDet.Count; i++)
                        {
                            #region *) Validation
                            if (pos.myOrderDet[i].SpecialDiscount == "P1"
                                || pos.myOrderDet[i].SpecialDiscount == "P2"
                                || pos.myOrderDet[i].SpecialDiscount == "P3"
                                || pos.myOrderDet[i].SpecialDiscount == "P4"
                                || pos.myOrderDet[i].SpecialDiscount == "P5")
                            {
                                listStatus.Add(string.Format("Error giving Item:{0} Discount. Discount cannot overwrite Multi Tier Price",
                                    pos.myOrderDet[i].ItemNo));
                                continue;
                            }
                            if (pos.myOrderDet[i].ItemNo == "LINE_DISCOUNT")
                                continue;

                            #endregion

                            string theStatus = "";
                            pos.ApplyDiscountLevel1(Discount1, pos.myOrderDet[i].OrderDetID, out status);
                            if (!string.IsNullOrEmpty(theStatus))
                                listStatus.Add(theStatus);
                            else
                            {
                                pos.ApplyAuthorizedBy(pos.myOrderDet[i].OrderDetID, authorizedBy);
                                pos.ApplyDiscountReason(pos.myOrderDet[i].OrderDetID, discountReason);
                            }
                        }
                    }
                    else
                    {
                        #region *) Check Privileges To All
                        bool isAllowed = pos.CheckDiscount1PrivilegesToGlobal(UserInfo.username, tag, discount, out status);
                        authorizedBy = UserInfo.username;
                        if (!isAllowed)
                        {
                            if (IsPromtPasswordOnDiscLimit)
                            {
                                MessageBox.Show(status);
                                frmSupervisorLogin fSpv = new frmSupervisorLogin();
                                fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                                fSpv.ShowDialog();
                                if (fSpv.IsAuthorized)
                                {
                                    isAllowed = pos.CheckDiscount1PrivilegesToGlobal(fSpv.mySupervisor, tag, discount, out status);
                                    if (isAllowed)
                                        authorizedBy = fSpv.mySupervisor;
                                }
                            }
                            if (!isAllowed)
                            {
                                MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                                return;
                            }
                        }
                        #endregion

                        #region *) Discount Reason
                        if (UseDiscountReason && (Discount1 != "0%" || Discount1 != "$0"))
                        {
                            frmDiscountReason frmDiscReason = new frmDiscountReason();
                            frmDiscReason.ShowDialog();
                            if (frmDiscReason.IsSuccessful)
                                discountReason = frmDiscReason.DiscountReason;
                            else
                                return;
                        }
                        #endregion

                        string lineDiscountID = pos.IsItemIsInOrderLine("LINE_DISCOUNT");
                        if (lineDiscountID == "")
                        {
                            if (pos.AddItemToOrder(new Item("LINE_DISCOUNT"), -1, 0, false, out status))
                            {
                                string lastID = pos.myOrderDet[pos.myOrderDet.Count - 1].OrderDetID;
                                if (pos.ChangeOrderLineUnitPrice(lastID, discount, out status))
                                {
                                    IsSuccessful = true;
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Error:" + status);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + status);
                            }
                        }
                        else
                        {
                            if (pos.ChangeOrderLineUnitPrice(lineDiscountID, discount, out status))
                            {
                                IsSuccessful = true;
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Error:" + status);
                            }
                        }
                    }
                    success = listStatus.Count == 0;
                    if (!success)
                        status = string.Join("\n", listStatus.ToArray());
                }
                else
                {
                    #region *) Validation
                    var myDet = pos.GetLine(LineID, out status);
                    if (myDet.SpecialDiscount == "P1"
                        || myDet.SpecialDiscount == "P2"
                        || myDet.SpecialDiscount == "P3"
                        || myDet.SpecialDiscount == "P4"
                        || myDet.SpecialDiscount == "P5")
                    {
                        MessageBox.Show(string.Format("Error giving Item:{0} Discount. Discount cannot overwrite Multi Tier Price",
                           myDet.ItemNo));
                        return;
                    }

                    #endregion

                    #region *) Check Privileges To Single LineID
                    bool isAllowed = pos.CheckDiscount1Privileges(UserInfo.username, LineID, Discount1, out status);
                    authorizedBy = UserInfo.username;
                    if (!isAllowed)
                    {
                        if (IsPromtPasswordOnDiscLimit)
                        {
                            MessageBox.Show(status);
                            frmSupervisorLogin fSpv = new frmSupervisorLogin();
                            fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                            fSpv.ShowDialog();
                            if (fSpv.IsAuthorized)
                            {
                                isAllowed = pos.CheckDiscount1Privileges(fSpv.mySupervisor, LineID, Discount1, out status);
                                if (isAllowed)
                                    authorizedBy = fSpv.mySupervisor;
                            }
                        }
                        if (!isAllowed)
                        {
                            MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                            return;
                        }
                    }
                    #endregion

                    #region *) Discount Reason
                    if (UseDiscountReason && (Discount1 != "0%" || Discount1 != "$0"))
                    {
                        frmDiscountReason frmDiscReason = new frmDiscountReason();
                        frmDiscReason.ShowDialog();
                        if (frmDiscReason.IsSuccessful)
                            discountReason = frmDiscReason.DiscountReason;
                        else
                            return;
                    }
                    #endregion

                    success = pos.ApplyDiscountLevel1(Discount1, LineID, out status);
                    if (success)
                    {
                        pos.ApplyAuthorizedBy(LineID, authorizedBy);
                        pos.ApplyDiscountReason(LineID, discountReason);
                    }
                }

                if (success)
                {
                    IsSuccessful = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(status);
                }
            }
        }

        private void btnAddDisc_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            frmKeypad f = new frmKeypad();
            f.IsInteger = false;

            string Discount2 = "";
            string authorizedBy = "";
            string discountReason = "";

            if (!string.IsNullOrEmpty(Discount2) && Discount2.Contains(tag))
            {
                f.initialValue = Discount2.Replace(tag, "");
            }

            f.ShowDialog();
            decimal discount;
            if (!string.IsNullOrEmpty(f.value) && decimal.TryParse(f.value.ToString(), out discount))
            {
                if (discount <= 0)
                {
                    Discount2 = "";
                }
                else
                {
                    if (tag == "%")
                    {
                        if (discount <= 100)
                            Discount2 = discount.ToString() + tag;
                        else
                            MessageBox.Show("Discount cannot exceed 100%");
                    }
                    else
                    {
                        Discount2 = tag + discount.ToString();
                    }
                }

                string status = "";
                bool success = false;
                if (IsApplyToAll)
                {
                    #region *) Check Privileges To All
                    bool isAllowed = pos.CheckDiscount2PrivilegesToAll(UserInfo.username, Discount2, out status);
                    authorizedBy = UserInfo.username;
                    if (!isAllowed)
                    {
                        if (IsPromtPasswordOnDiscLimit)
                        {
                            MessageBox.Show(status);
                            frmSupervisorLogin fSpv = new frmSupervisorLogin();
                            fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                            fSpv.ShowDialog();
                            if (fSpv.IsAuthorized)
                            {
                                isAllowed = pos.CheckDiscount2PrivilegesToAll(fSpv.mySupervisor, Discount2, out status);
                                if (isAllowed)
                                    authorizedBy = fSpv.mySupervisor;
                            }
                        }
                        if (!isAllowed)
                        {
                            MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                            return;
                        }
                    }
                    #endregion

                    #region *) Discount Reason
                    if (UseDiscountReason && (Discount2 != "0%" || Discount2 != "$0"))
                    {
                        frmDiscountReason frmDiscReason = new frmDiscountReason();
                        frmDiscReason.ShowDialog();
                        if (frmDiscReason.IsSuccessful)
                            discountReason = frmDiscReason.DiscountReason;
                        else
                            return;
                    }
                    #endregion

                    List<string> listStatus = new List<string>();
                    for (int i = 0; i < pos.myOrderDet.Count; i++)
                    {
                        if (pos.myOrderDet[i].ItemNo == "LINE_DISCOUNT")
                            continue;

                        if (pos.myOrderDet[i].UnitPrice == 0)
                        {
                            listStatus.Add("Cannot apply disc to the $0 item");
                        }
                        else
                        {
                            string theStatus = "";
                            pos.ApplyDiscountLevel2(Discount2, pos.myOrderDet[i].OrderDetID, out theStatus);
                            if (!string.IsNullOrEmpty(theStatus))
                                listStatus.Add(theStatus);
                            else
                            {
                                pos.ApplyAuthorizedBy(pos.myOrderDet[i].OrderDetID, authorizedBy);
                                pos.ApplyDiscountReason(pos.myOrderDet[i].OrderDetID, discountReason);
                            }
                        }
                    }
                    success = listStatus.Count == 0;
                    if (!success)
                        status = string.Join("\n", listStatus.ToArray());
                }
                else
                {
                    var myDet = pos.GetLine(LineID, out status);
                    if (myDet.UnitPrice == 0)
                    {
                        MessageBox.Show("Cannot apply disc to the $0 item");
                        return;
                    }
                    #region *) Check Privileges To Single LineID
                    bool isAllowed = pos.CheckDiscount2Privileges(UserInfo.username, LineID, Discount2, out status);
                    authorizedBy = UserInfo.username;
                    if (!isAllowed)
                    {
                        if (IsPromtPasswordOnDiscLimit)
                        {
                            MessageBox.Show(status);
                            frmSupervisorLogin fSpv = new frmSupervisorLogin();
                            fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                            fSpv.ShowDialog();
                            if (fSpv.IsAuthorized)
                            {
                                isAllowed = pos.CheckDiscount2Privileges(fSpv.mySupervisor, LineID, Discount2, out status);
                                if (isAllowed)
                                    authorizedBy = fSpv.mySupervisor;
                            }
                        }
                        if (!isAllowed)
                        {
                            MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                            return;
                        }
                    }
                    #endregion

                    #region *) Discount Reason
                    if (UseDiscountReason && (Discount2 != "0%" || Discount2 != "$0"))
                    {
                        frmDiscountReason frmDiscReason = new frmDiscountReason();
                        frmDiscReason.ShowDialog();
                        if (frmDiscReason.IsSuccessful)
                            discountReason = frmDiscReason.DiscountReason;
                        else
                            return;
                    }
                    #endregion

                    success = pos.ApplyDiscountLevel2(Discount2, LineID, out status);
                    if (success)
                    {
                        pos.ApplyAuthorizedBy(LineID, authorizedBy);
                        pos.ApplyDiscountReason(LineID, discountReason);
                    }
                }
                if (success)
                {
                    IsSuccessful = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(status);
                }
            }
        }

        private void btnFinalPrice_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            frmKeypad f = new frmKeypad();
            f.textMessage = "Input Final Price";
            f.IsInteger = false;

            string authorizedBy = "";
            string discountReason = "";

            f.ShowDialog();
            decimal discount;
            if (!string.IsNullOrEmpty(f.value) && decimal.TryParse(f.value.ToString(), out discount))
            {
                string status = "";
                bool success = false;
                if (IsApplyToAll)
                {
                    //this code should not be executed
                    #region *) Check Privileges To All
                    bool isAllowed = pos.CheckFinalPrivilegesToAll(UserInfo.username, discount, out status);
                    authorizedBy = UserInfo.username;
                    if (!isAllowed)
                    {
                        if (IsPromtPasswordOnDiscLimit)
                        {
                            MessageBox.Show(status);
                            frmSupervisorLogin fSpv = new frmSupervisorLogin();
                            fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                            fSpv.ShowDialog();
                            if (fSpv.IsAuthorized)
                            {
                                isAllowed = pos.CheckFinalPrivilegesToAll(UserInfo.username, discount, out status);
                                if (isAllowed)
                                    authorizedBy = fSpv.mySupervisor;
                            }
                        }
                        if (!isAllowed)
                        {
                            MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                            return;
                        }
                    }
                    #endregion

                    #region *) Discount Reason
                    if (UseDiscountReason && (discount > 0))
                    {
                        frmDiscountReason frmDiscReason = new frmDiscountReason();
                        frmDiscReason.ShowDialog();
                        if (frmDiscReason.IsSuccessful)
                            discountReason = frmDiscReason.DiscountReason;
                        else
                            return;
                    }
                    #endregion

                    List<string> listStatus = new List<string>();
                    for (int i = 0; i < pos.myOrderDet.Count; i++)
                    {
                        if (pos.myOrderDet[i].ItemNo == "LINE_DISCOUNT")
                            continue;

                        if (pos.myOrderDet[i].UnitPrice == 0)
                        {
                            listStatus.Add("Cannot apply disc to the $0 item");
                        }
                        else
                        {
                            string theStatus = "";

                            string Discount2 = "";
                            if (discount < 0)
                                Discount2 = "";
                            else
                            {
                                decimal discountedPrice = pos.myOrderDet[i].UnitPrice - ((pos.myOrderDet[i].UnitPrice * pos.myOrderDet[i].Discount) / 100);
                                if (pos.myOrderDet[i].IsPromo)
                                    discountedPrice = pos.myOrderDet[i].UnitPrice - ((pos.myOrderDet[i].UnitPrice * (decimal)pos.myOrderDet[i].PromoDiscount) / 100);

                                decimal discountFinal = (discountedPrice * (pos.myOrderDet[i].Quantity ?? 0)) - (discount * (pos.myOrderDet[i].Quantity ?? 0));

                                if (discountFinal > 0)
                                    Discount2 = "$" + discountFinal.ToString("N2");
                                else
                                {
                                    listStatus.Add("Final Price should be less than Discounted Price");
                                }
                            }

                            pos.ApplyDiscountLevel2FinalPrice(discount, pos.myOrderDet[i].OrderDetID, out theStatus);
                            if (!string.IsNullOrEmpty(theStatus))
                                listStatus.Add(theStatus);
                            else
                            {
                                pos.ApplyAuthorizedBy(pos.myOrderDet[i].OrderDetID, authorizedBy);
                                pos.ApplyDiscountReason(pos.myOrderDet[i].OrderDetID, discountReason);
                            }
                        }
                    }
                    success = listStatus.Count == 0;
                    if (!success)
                        status = string.Join("\n", listStatus.ToArray());
                }
                else
                {
                    string Discount2 = "";
                    var myDet = pos.GetLine(LineID, out status);
                    if (myDet.UnitPrice == 0)
                    {
                        MessageBox.Show("Cannot apply disc to the $0 item");
                        return;
                    }
                    #region *) Check Privileges To Single LineID

                    if (discount < 0)
                        Discount2 = "";
                    else
                    {
                        decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
                        if (myDet.IsPromo)
                            discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * (decimal)myDet.PromoDiscount) / 100);

                        decimal discountFinal = discountedPrice - discount;

                        if (discountFinal > 0)
                            Discount2 = "$" + discountFinal.ToString("N2");
                        else
                        {
                            MessageBox.Show("Final Price should be less than Discounted Price");
                            return;
                        }
                    }

                    bool isAllowed = pos.CheckDiscount2Privileges(UserInfo.username, LineID, Discount2, out status);
                    authorizedBy = UserInfo.username;
                    if (!isAllowed)
                    {
                        if (IsPromtPasswordOnDiscLimit)
                        {
                            MessageBox.Show(status);
                            frmSupervisorLogin fSpv = new frmSupervisorLogin();
                            fSpv.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                            fSpv.ShowDialog();
                            if (fSpv.IsAuthorized)
                            {
                                isAllowed = pos.CheckDiscount2Privileges(fSpv.mySupervisor, LineID, Discount2, out status);
                                if (isAllowed)
                                    authorizedBy = fSpv.mySupervisor;
                            }
                        }
                        if (!isAllowed)
                        {
                            MessageBox.Show("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                            return;
                        }
                    }
                    #endregion

                    #region *) Discount Reason
                    if (UseDiscountReason && (Discount2 != "0%" || Discount2 != "$0"))
                    {
                        frmDiscountReason frmDiscReason = new frmDiscountReason();
                        frmDiscReason.ShowDialog();
                        if (frmDiscReason.IsSuccessful)
                            discountReason = frmDiscReason.DiscountReason;
                        else
                            return;
                    }
                    #endregion

                    success = pos.ApplyDiscountLevel2FinalPrice(discount, LineID, out status);
                    if (success)
                    {
                        pos.ApplyAuthorizedBy(LineID, authorizedBy);
                        pos.ApplyDiscountReason(LineID, discountReason);
                    }
                }
                if (success)
                {
                    IsSuccessful = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(status);
                }
            }
        }
    }
}
