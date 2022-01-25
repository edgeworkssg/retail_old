using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using SubSonic;
using PowerPOS.Container;

namespace PowerPOS
{
    public partial class POSController
    {
        bool isNewMember;
        private decimal membershipDiscount;
        public Membership CurrentMember;
        public string redemptionMembershipNo;

        private DateTime newExpiryDate; //For renewals...
        private int newMembershipGroupID = -1;

        public AttachedParticularCollection CurrentAttachedParticular;

        public decimal OutstandingBalanceOverall = 0;
        public decimal OutstandingBalanceOrder = 0;

        /*
        public bool EligibleForUpgrade()
        {
            if (!MembershipApplied()) return false;

            //if already gold member, upgrade no longer needed
            if (CurrentMember.MembershipGroupId == MembershipController.GOLD_GROUPID) return false;

            bool IsVitaMix = false;
            bool IsWaterFilter = false;
            bool IsJuicePlus = false;
            bool IsYoung = false;

            //existing purchase from the past
            if (CurrentMember.IsVitaMix.HasValue) IsVitaMix = CurrentMember.IsVitaMix.Value;
            if (CurrentMember.IsWaterFilter.HasValue) IsWaterFilter = CurrentMember.IsWaterFilter.Value;
            if (CurrentMember.IsJuicePlus.HasValue) IsJuicePlus = CurrentMember.IsJuicePlus.Value;
            if (CurrentMember.IsYoung.HasValue) IsYoung = CurrentMember.IsYoung.Value;

            //scan through orderdet list to determine if the guy eligible for upgrade
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!myOrderDet[i].IsVoided)
                {
                    if (myOrderDet[i].Item.IsVitaMix.HasValue &&
                        myOrderDet[i].Item.IsVitaMix == true)
                        IsVitaMix = true;

                    if (myOrderDet[i].Item.IsWaterFilter.HasValue &&
                        myOrderDet[i].Item.IsWaterFilter == true)
                        IsWaterFilter = true;

                    if (myOrderDet[i].Item.IsYoung.HasValue &&
                        myOrderDet[i].Item.IsYoung == true)
                        IsYoung = true;

                    if (myOrderDet[i].Item.IsJuicePlus.HasValue &&
                        myOrderDet[i].Item.IsJuicePlus == true)
                        IsJuicePlus = true;
                }
            }

            return IsVitaMix & IsJuicePlus & IsYoung & IsWaterFilter;
        }
        */

        //Load membership info from the value assigned from orderhdr
        //use this when 
        /*
        public void LoadMembership()
        {
            if (myOrderHdr.MembershipNo != null && myOrderHdr.MembershipNo != "")
            {
                CurrentMember = new Membership(Membership.Columns.MembershipNo, myOrderHdr.MembershipNo);
            }
            else
            {
                CurrentMember = new Membership();
            }
        }
        */

        //return whether membership that assign to this receipt
        public bool MembershipApplied()
        {
            return ((CurrentMember != null && 
                CurrentMember.IsLoaded && !CurrentMember.IsNew) | isNewMember);
        }

        //remove membership from receipt
        public bool RemoveMemberFromReceipt()
        {
            //set membership variables
            CurrentMember = null;
            isNewMember = false;
            //void lne item for new signup & renewal if any..
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                //remove new sign up item and renewal item
                if (myOrderDet[i].ItemNo ==
                    MembershipController.MEMBERSHIP_SIGNUP_BARCODE
                    || myOrderDet[i].ItemNo ==
                    MembershipController.RENEWAL_BARCODE)
                {
                    myOrderDet[i].IsVoided = true;
                }

                myOrderDet[i].IsPromoPossibilityChecked = false;

            }
            
            //apply the discounts
            promoCtrl.UndoPromoToOrder();
            promoCtrl.ApplyPromoToOrder();
            ApplyMembershipDiscount();
            return true;
        }

        // Cancel creating new member
        public bool CancelNewMember()
        {
            //set membership variables
            CurrentMember = null;
            isNewMember = false;
            return true;
        }

        //return membership information
        public Membership GetMemberInfo()
        {
            if (MembershipApplied())
            {
                return CurrentMember;
            }
            else
            {
                return null;
            }
        }
        //apply membership discount to receipt
        //TODO: assign a different membership discount for product and service
        public void ApplyMembershipDiscount()
        {
            string status = "";
            try
            {
                OrderDet myOrderDetItem;

                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    myOrderDetItem = myOrderDet[i];
                    //if it is not special
                    //if (myOrderDetItem.IsSpecial == false)
                    //{
                        if (MembershipApplied() && GetMemberInfo().MembershipGroup.Discount != 0)
                        {
                            //if it is not non discountable
                            if (!myOrderDetItem.Item.IsNonDiscountable)
                            {
                                //if it is not promo
                                //if (!myOrderDetItem.IsPromo && !myOrderDetItem.IsSpecial && !myOrderDetItem.IsExchange)
                                if (!myOrderDetItem.IsPromo && !myOrderDetItem.IsExchange)
                                {
                                    //apply the discount
                                    //apply product or service option here
                                    // add to load discount from attributes4
                                    if (myOrderDetItem.Discount < preferredDiscount)
                                    {
                                        myOrderDetItem.Discount = preferredDiscount;
                                    }

                                    if (MembershipApplied() && myOrderDetItem.Discount < membershipDiscount)
                                    {
                                        myOrderDetItem.Discount = membershipDiscount;//(decimal)CurrentMember.MembershipGroup.Discount;
                                        myOrderDetItem.DiscountDetail = membershipDiscount.ToString("N0") + "%";
                                        //myOrderDetItem.IsSpecial = true;
                                    }
                                }
                            }
                            else
                            {
                                myOrderDetItem.DiscountDetail = "0%";
                                myOrderDetItem.Discount = 0;
                            }
                        }
                    //}
                    string pricelevel = "";
                    if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null && GetMemberInfo().MembershipGroup.Userfld1 != "")
                        pricelevel = GetMemberInfo().MembershipGroup.Userfld1;
                    if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                    {
                        SpecialDiscount sd = new SpecialDiscount(pricelevel);
                        try
                        {
                            clearDiscount(0);
                            applyDiscount(pricelevel);
                        }
                        catch (Exception ex) { Logger.writeLog(ex.Message); }
                    }
                    CalculateLineAmount(ref myOrderDetItem);
                }
                return;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return;
            }
        }

        public bool IsNewMembersRegistered()
        {           
            return isNewMember;
        }

        //return bool isHavePasscode
        public bool IsHavePasscode()
        {
            if (!MembershipApplied())
            {
                return false;
            }
            else
            {
                return CurrentMember.PassCode != null && CurrentMember.PassCode != "";
            }
        }

        public bool IsPassCodeMatch(string passcode)
        {
            return CurrentMember.PassCode == passcode;
        }

        public string GetAssignedMembershipStaff()
        {
            if (MembershipApplied())
            {
                return CurrentMember.SalesPersonID;
            }
            else
            {
                return "";
            }
        }

        /*
        public bool hasRenewal()
        {

            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].Item.Barcode == MembershipController.RENEWAL_BARCODE)
                    return true;
            }
            return false;

        }
        */

        //assign new member that first sign up to the pos
        public bool AssignNewMember
            (string membershipNo, string nametoappear, string nric, string mobileno, string homeno,
             int membershipgroupid, DateTime ExpiryDate,
             string address, string address2, string postalcode,
             string city, string country,
                bool applyPromo, string email)
        {

            //registers the members
            CurrentMember = new Membership();
            CurrentMember.NameToAppear = nametoappear;
            CurrentMember.FirstName = "";
            CurrentMember.LastName = "";
            CurrentMember.ChineseName = "";
            CurrentMember.ChristianName = "";
            CurrentMember.Nric = nric;
            CurrentMember.MembershipGroupId = membershipgroupid;
            CurrentMember.Mobile = mobileno;
            CurrentMember.Home = homeno;
            CurrentMember.ExpiryDate = ExpiryDate;
            CurrentMember.MembershipNo = membershipNo;
            CurrentMember.StreetName = address;
            CurrentMember.ZipCode = postalcode;
            CurrentMember.City = city;
            CurrentMember.StreetName2 = address2;
            CurrentMember.Country = country;
            CurrentMember.SubscriptionDate = DateTime.Now;
            CurrentMember.DateOfBirth = DateTime.Now.AddMonths(-2);
            CurrentMember.Email = email;
            isNewMember = true;
            CurrentMember.Deleted = false;
            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
            if (applyPromo)
            {
                promoCtrl.UndoPromoToOrder();
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    myOrderDet[i].IsPromoPossibilityChecked = false;
                }
                promoCtrl.ApplyPromoToOrder();
            }

            //ApplyMembershipDiscount();
            return true;
        }

        /// <summary>
        /// To Add membership together with detail info. (created by John Harries)
        /// </summary>
        /// <returns>True for success process and false for unsuccess process.</returns>
        public bool AssignNewMember
            (string membershipNo, string nametoappear, string nric, string mobileno, string homeno,
             int membershipgroupid, DateTime ExpiryDate,
             string address, string address2, string postalcode,
             string city, string country,
                bool applyPromo, string email, string firstName, string lastName, string chineseName,
            string christianName,DateTime dateOfBirth, DateTime subscriptionDate, bool isVitaMix, bool isYoung,
            bool isWaterFilter, bool isJuicePlus, string remarks, string occupation, string fax, string office)
        {

            //registers the members
            CurrentMember = new Membership();
            CurrentMember.NameToAppear = nametoappear;
            CurrentMember.FirstName = firstName;
            CurrentMember.LastName = lastName;
            CurrentMember.ChineseName = chineseName;
            CurrentMember.ChristianName = christianName;
            CurrentMember.Nric = nric;
            CurrentMember.MembershipGroupId = membershipgroupid;
            CurrentMember.Mobile = mobileno;
            CurrentMember.Home = homeno;
            CurrentMember.ExpiryDate = ExpiryDate;
            CurrentMember.MembershipNo = membershipNo;
            CurrentMember.StreetName = address;
            CurrentMember.ZipCode = postalcode;
            CurrentMember.City = city;
            CurrentMember.StreetName2 = address2;
            CurrentMember.Country = country;
            CurrentMember.SubscriptionDate = subscriptionDate.Date;
            if (DateTime.Compare(dateOfBirth.Date, DateTime.Today.Date) < 0) { CurrentMember.DateOfBirth = dateOfBirth.Date; }
            CurrentMember.Email = email;
            CurrentMember.IsJuicePlus = isJuicePlus;
            CurrentMember.IsVitaMix = isVitaMix;
            CurrentMember.IsWaterFilter = isWaterFilter;
            CurrentMember.IsYoung = isYoung;
            CurrentMember.Remarks = remarks;
            CurrentMember.Occupation = occupation;
            CurrentMember.Fax = fax;
            CurrentMember.Office = office;
            isNewMember = true;
            CurrentMember.Deleted = false;
            CurrentMember.UniqueID = Guid.NewGuid();
            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
            if (applyPromo)
            {
                promoCtrl.UndoPromoToOrder();
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    myOrderDet[i].IsPromoPossibilityChecked = false;
                }
                promoCtrl.ApplyPromoToOrder();
            }

            //ApplyMembershipDiscount();
            return true;
        }

        /// <summary>
        /// To Add membership together with detail info. (created by John Harries)
        /// </summary>
        /// <returns>True for success process and false for unsuccess process.</returns>
        public bool AssignNewMember
            (string membershipNo, string nametoappear, string nric, string mobileno, string homeno,
             int membershipgroupid, DateTime ExpiryDate,
             string address, string address2, string postalcode,
             string city, string country,
                bool applyPromo, string email, string firstName, string lastName, string chineseName,
            string christianName, DateTime dateOfBirth, DateTime subscriptionDate, bool isVitaMix, bool isYoung,
            bool isWaterFilter, bool isJuicePlus, string remarks, string occupation, string fax, string office, string customfield1name,
            string customField1value, string customField2name, string customField2value, string gender, string passCode)
        {

            //registers the members
            CurrentMember = new Membership();
            CurrentMember.NameToAppear = nametoappear;
            CurrentMember.FirstName = firstName;
            CurrentMember.LastName = lastName;
            CurrentMember.ChineseName = chineseName;
            CurrentMember.ChristianName = christianName;
            CurrentMember.Nric = nric;
            CurrentMember.MembershipGroupId = membershipgroupid;
            CurrentMember.Mobile = mobileno;
            CurrentMember.Home = homeno;
            CurrentMember.ExpiryDate = ExpiryDate;
            CurrentMember.MembershipNo = membershipNo;
            CurrentMember.StreetName = address;
            CurrentMember.ZipCode = postalcode;
            CurrentMember.City = city;
            CurrentMember.StreetName2 = address2;
            CurrentMember.Country = country;
            CurrentMember.SubscriptionDate = subscriptionDate.Date;
            if (DateTime.Compare(dateOfBirth.Date, DateTime.Today.Date) < 0) { CurrentMember.DateOfBirth = dateOfBirth.Date; }
            CurrentMember.Email = email;
            CurrentMember.IsJuicePlus = isJuicePlus;
            CurrentMember.IsVitaMix = isVitaMix;
            CurrentMember.IsWaterFilter = isWaterFilter;
            CurrentMember.IsYoung = isYoung;
            CurrentMember.Remarks = remarks;
            CurrentMember.Occupation = occupation;
            CurrentMember.Fax = fax;
            CurrentMember.Office = office;
            CurrentMember.Gender = gender;
            CurrentMember.PassCode = passCode;
            if (!customfield1name.Equals(""))
                CurrentMember.SetColumnValue(customfield1name, customField1value);
            if (!customField2name.Equals(""))
                CurrentMember.SetColumnValue(customField2name, customField2value);
            
            isNewMember = true;
            CurrentMember.Deleted = false;
            CurrentMember.UniqueID = Guid.NewGuid();
            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
            if (CurrentMember.IsNew)
                CurrentMember.IsCreatedInWeb = false;
            
            if (applyPromo)
            {
                promoCtrl.UndoPromoToOrder();
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    myOrderDet[i].IsPromoPossibilityChecked = false;
                }
                promoCtrl.ApplyPromoToOrder();
            }

            //ApplyMembershipDiscount();
            return true;
        }

        ////assign new member that first sign up to the pos
        //public bool AssignNewMember
        //    (string nametoappear, string nric, string mobileno, string homeno,
        //     int membershipgroupid, DateTime ExpiryDate,
        //     string address, string address2, string postalcode,
        //     string city, string country,
        //        bool applyPromo, string email)
        //{

        //    //registers the members
        //    CurrentMember = new Membership();
        //    CurrentMember.NameToAppear = nametoappear;
        //    CurrentMember.FirstName = "";
        //    CurrentMember.LastName = "";
        //    CurrentMember.ChineseName = "";
        //    CurrentMember.ChristianName = "";
        //    CurrentMember.Nric = nric;
        //    CurrentMember.MembershipGroupId = membershipgroupid;
        //    CurrentMember.Mobile = mobileno;
        //    CurrentMember.Home = homeno;
        //    CurrentMember.ExpiryDate = ExpiryDate;
        //    CurrentMember.MembershipNo = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
        //    CurrentMember.StreetName = address;
        //    CurrentMember.ZipCode = postalcode;
        //    CurrentMember.City = city;
        //    CurrentMember.StreetName2 = address2;
        //    CurrentMember.Country = country;
        //    CurrentMember.SubscriptionDate = DateTime.Now;
        //    CurrentMember.DateOfBirth = DateTime.Now.AddMonths(-2);
        //    CurrentMember.Email = email;
        //    isNewMember = true;
        //    CurrentMember.Deleted = false;
        //    membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
        //    if (applyPromo)
        //    {
        //        promoCtrl.UndoPromoToOrder();
        //        promoCtrl.ApplyPromoToOrder();
        //    }

        //    //ApplyMembershipDiscount();
        //    return true;
        //}

        /*
        public decimal getMembershipDiscount()
        {
            return membershipDiscount;
        }
        
        public bool ClearMembershipDiscount()
        {
            membershipDiscount = 0;
            return true;
        }
        */
        //Load new member from DB
        //Allow member to be assign to CurrentMember record
        //even when members has already expired
        public bool AssignExpiredMember(string membershipno, out string status)
        {
            status = "";
            CurrentMember = new Membership(Membership.Columns.MembershipNo, membershipno);
            if (CurrentMember == null || CurrentMember.ExpiryDate == null)
            {
                status = "Membership number " + membershipno + " does not exist";
                return false;
            }
            promoCtrl.UndoPromoToOrder(); //UndoCurrentPromo();
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                myOrderDet[i].IsPromoPossibilityChecked = false;
            }
            promoCtrl.ApplyPromoToOrder();
            status = "";
            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
            //Apply Discount....
            //ApplyMembershipDiscount();
            isNewMember = false;
            return true;
        }
        /*
        public bool AssignOtherMembership(string membershipNo)
        {
            CurrentMember = new Membership();
            CurrentMember.MembershipNo = membershipNo;
            CurrentMember.MembershipGroupId = MembershipController.DEFAULT_GROUPID;
            CurrentMember.NameToAppear = "Other";
            CurrentMember.IsLoaded = true;
            CurrentMember.IsNew = false;
            promoCtrl.UndoPromoToOrder(); //UndoCurrentPromo();
            promoCtrl.ApplyPromoToOrder();

            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
            //Apply Discount....
            //ApplyMembershipDiscount();
            isNewMember = false;
            return true;
        }
        */

        //Assign membership to receipt
        //Load data from DB and assign to local variable CurrentMember
        //reject if expired
        public bool AssignMembership(string membershipno, out string status)
        {
            DateTime ExpiryDate;

            if (MembershipController.CheckMembershipValid
                (membershipno, out CurrentMember, out ExpiryDate))
            {
                if (membershipno != "WALK-IN")
                {
                    foreach (OrderDet od in myOrderDet)
                    {
                        od.IsPromoPossibilityChecked = false;
                    }
                    promoCtrl.UndoPromoToOrder(); //UndoCurrentPromo();
                    promoCtrl.ApplyPromoToOrder();
                }
                status = "";
                membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;

                //Apply Discount....
                //ApplyMembershipDiscount();
                isNewMember = false;
                return true;
            }
            else
            {
                if (ExpiryDate != DateTime.MinValue)
                {
                    //Prompt that membership has expired
                    status = "Dear customer, membership has expired on " + ExpiryDate + ". Kindly renew your membership. Thank you.";
                    return false;
                }
                else
                {
                    //Prompt that this is invalid membership number 
                    status = "This is not valid membership number. No membership information is recorded in the system.";
                    return false;
                }
            }
        }
        //record new expiry date upon renewal 
        public bool SetNewExpiryDate(DateTime myExpiryDate)
        {
            newExpiryDate = myExpiryDate;
            return true;
        }
        //record new expiry date upon renewal 
        public bool SetNewMembershipGroupID(int myGroupID)
        {
            newMembershipGroupID = myGroupID;
            return true;
        }

        public void UpdateExistedMembership(Membership membership)
        {

        }

        public bool ValidateMembership(Membership member, out List<string> EmptyMandatoryFields)
        {
            EmptyMandatoryFields = new List<string>();
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowMembershipWarning), false))
            {
                string fields = AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipWarningFields);
                if (fields != null && fields != "")
                {
                    string[] result = fields.Split(';');
                    foreach (string s in result)
                    {
                        if (member.GetColumnValue(s) == null || member.GetColumnValue(s).ToString() == "")
                        {
                            EmptyMandatoryFields.Add(s);
                        }
                    }

                    if (EmptyMandatoryFields.Count > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
