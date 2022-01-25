using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using PowerPOS.Container;
using System.Collections;
using POSDevices;
using PowerPOS;
using PowerPOSLib.PowerPOSSync;
using SubSonic;
using WinPowerPOS.MembershipForms;
using Newtonsoft.Json;
using System.Diagnostics;
namespace WinPowerPOS.OrderForms
{
    public partial class frmAddMember : Form
    {
        public string searchReq;

        public decimal PreferedDiscount;
        public string membershipNo;


        #region "Form Initialization and loading"
        public frmAddMember()
        {
            InitializeComponent();
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
            {
                this.Text = "Search " + AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString();
                labelProgressBarSyncMembership.Text = "Downloading new created " + AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString();
            }
            else
            {
                this.Text = "Search Member";
                labelProgressBarSyncMembership.Text = "Downloading new created members";
            }
        }
        private void frmAddMember_Load(object sender, EventArgs e)
        {
            try
            {
                Process[] oskProcessArray = Process.GetProcessesByName("TabTip");

                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }

                //find item using the given text
                MembershipController mbr = new MembershipController();
                ViewMembershipCollection coll = mbr.SearchMembership(searchReq);

                if (coll.Count == 0)
                {
                    labelSearchNotification.ForeColor = Color.Red;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
                    {
                        labelSearchNotification.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " cannot be found!";
                    }
                    else 
                    {
                        labelSearchNotification.Text = "Members cannot be found!";
                    }
                    //MessageBox.Show("Search produce no result.");
                    //this.Close();
                }
                else
                {
                    labelSearchNotification.ForeColor = Color.Blue;
                    labelSearchNotification.ForeColor = Color.Red;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
                    {
                        labelSearchNotification.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " found.";
                    }
                    else
                    {
                        labelSearchNotification.Text = "Members found.";
                    }
                    labelSearchNotification.Text = coll.Count + " members found.";
                }

                dgvMembersList.AutoGenerateColumns = false;
                this.dgvMembersList.DataSource = coll;
                this.dgvMembersList.Refresh();

                //Initialize progress bar 
                progressBarSyncMembership.Maximum = 100;
                progressBarSyncMembership.Value = 0;



                string searchText = this.searchReq;
                if (!string.IsNullOrEmpty(searchText))
                {
                    txtSearch.Text = searchText;
                }

                //Sync membership data
                bool isLocalhost = SyncClientController.WS_URL.StartsWith("http://localhost");
                string MembershipSync = AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipSyncSegmentSize);
                
                //##Commented on 2015.11.26 --- Using new Method
                //if (MembershipSync != "")
                //    if (!isLocalhost)
                //        SyncMemberData();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        private String getServerMembershipPrefix()
        {
            String membershipPrefixCode = "";
            Synchronization _ws = new Synchronization();
            SyncClientController.Load_WS_URL();
            string serverUrl = SyncClientController.WS_URL.ToLower().Replace("synchronization/synchronization.asmx", "");
            string url = serverUrl + "API/Membership/GetServerMembershipPrefix.ashx";
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection parameters = new NameValueCollection();
                byte[] responsebytes = webClient.UploadValues(url, "POST", parameters);
                string jsonDataRaw = Encoding.UTF8.GetString(responsebytes);
                membershipPrefixCode = jsonDataRaw;
            }

            return membershipPrefixCode;
        }

        private int getTotalUnSyncMembershipThatNotCreatedInCurrentPOS()
        {
            string lastTimeStamp = getLastMembershipTimeStampThatNotCreatedInCurrentPOS();

            int totalMembership = 0;
            Synchronization _ws = new Synchronization();
            SyncClientController.Load_WS_URL();
            string serverUrl = SyncClientController.WS_URL.ToLower().Replace("synchronization/synchronization.asmx", "");
            string url = serverUrl + "API/Membership/GetTotalMembership.ashx";
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("ParamCreatedOn", lastTimeStamp);
                parameters.Add("ParamMembershipNo", getLastMembershipNoThatNotCreatedInCurrentPOS());
                parameters.Add("ParamMembershipPrefixCode", PointOfSaleInfo.MembershipPrefixCode);
                byte[] responsebytes = webClient.UploadValues(url, "POST", parameters);
                string jsonDataRaw = Encoding.UTF8.GetString(responsebytes);
                var jsonData = (JArray)JsonConvert.DeserializeObject(jsonDataRaw);

                foreach (var item in jsonData)
                {
                    totalMembership = (int)(item["TotalMembership"] ?? "0");
                }
            }

            return totalMembership;
        }

        private String getLastMembershipTimeStampThatNotCreatedInCurrentPOS()
        {
            string strSql = "select top 1 CreatedOn from Membership where MembershipNo not like '" + PointOfSaleInfo.MembershipPrefixCode + "%' order by CreatedOn desc;";
            QueryCommand cmd = new QueryCommand(strSql);
            var paramCreatedOnRaw = DataService.ExecuteScalar(cmd);
            String paramCreatedOn = null;
            if (paramCreatedOnRaw != null)
            {
                paramCreatedOn = ((DateTime)paramCreatedOnRaw).ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else
            {
                paramCreatedOn = "1900-01-01";
            }

            return paramCreatedOn;
        }

        private String getLastMembershipNoThatNotCreatedInCurrentPOS()
        {
            string strSql = "select top 1 MembershipNo from Membership where MembershipNo not like '" + PointOfSaleInfo.MembershipPrefixCode + "%' order by CreatedOn desc;";
            QueryCommand cmd = new QueryCommand(strSql);
            var paramMembershipNoRaw = DataService.ExecuteScalar(cmd);
            String paramMembershipNo = null;
            if (paramMembershipNoRaw != null)
            {
                paramMembershipNo = paramMembershipNoRaw.ToString();
            }
            else
            {
                paramMembershipNo = "";
            }

            return paramMembershipNo;
        }

        private void SyncMemberData()
        {
            try
            {
                labelTotalDownloadedMembers.Text = "0";
                labelTotalNewCreatedMembers.Text = "0";

                Thread syncThread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        var _this = this;
                        int totalMembership = getTotalUnSyncMembershipThatNotCreatedInCurrentPOS();
                        int progressBarMaxValue = totalMembership;
                        progressBarSyncMembership.Maximum = totalMembership;
                        MethodInvoker setupMI = new MethodInvoker(() =>
                        {
                            progressBarSyncMembership.Maximum = progressBarMaxValue;
                            progressBarSyncMembership.Value = 0;

                            labelTotalNewCreatedMembers.Text = totalMembership.ToString();
                            labelTotalDownloadedMembers.Text = "0";
                        });

                        if (progressBarSyncMembership.InvokeRequired)
                        {
                            progressBarSyncMembership.Invoke(setupMI);
                        }
                        else
                        {
                            setupMI.Invoke();
                        }

                        bool loopParameter = false;
                        int progressBarValue = 0;
                        var membershipSyncSegmentSizeRaw = AppSetting.GetSetting("MembershipSyncSegmentSize");
                        int membershipSyncSegmentSize = 0;
                        int membershipLoopIterator = 0;
                        if (membershipSyncSegmentSizeRaw == null)
                        {
                            membershipSyncSegmentSize = 100;
                        }
                        else
                        {
                            membershipSyncSegmentSize = Convert.ToInt32(membershipSyncSegmentSizeRaw);
                        }
                       
                        while (loopParameter == false && totalMembership > 0)
                        {
                            using (WebClient webClient = new WebClient())
                            {
                                string serverUrl = SyncClientController.WS_URL.ToLower().Replace("synchronization/synchronization.asmx", "");
                                String url = serverUrl + "API/Membership/GetMembership.ashx";
                                NameValueCollection parameters = new NameValueCollection();
                                parameters.Add("Count", membershipSyncSegmentSize.ToString());
                                parameters.Add("ParamMembershipNo", getLastMembershipNoThatNotCreatedInCurrentPOS());
                                parameters.Add("ParamCreatedOn", getLastMembershipTimeStampThatNotCreatedInCurrentPOS());
                                parameters.Add("ParamMembershipPrefixCode", PointOfSaleInfo.MembershipPrefixCode);
                                byte[] responsebytes = webClient.UploadValues(url, "POST", parameters);
                                string jsonDataRaw = Encoding.UTF8.GetString(responsebytes);
                                var jsonData = (JArray)JsonConvert.DeserializeObject(jsonDataRaw);
                                membershipLoopIterator = 0;
                                foreach (var item in jsonData)
                                {
                                    membershipLoopIterator += 1;
                                    string membershipNo = (string)item["MembershipNo"] ?? "";
                                    int membershipGroupId = (int)(item["MembershipGroupId"] ?? "0");
                                    string title = (string)item["Title"] ?? "";
                                    string lastName = ((string)item["LastName"] ?? "").Replace("'", "''");
                                    string firstName = ((string)item["FirstName"] ?? "").Replace("'", "''");
                                    string christianName = ((string)item["ChristianName"] ?? "").Replace("'", "''");
                                    string chineseName = ((string)item["ChineseName"] ?? "").Replace("'", "''");
                                    string nameToAppear = ((string)item["NameToAppear"] ?? "").Replace("'", "''");
                                    string gender = (string)item["Gender"] ?? "";
                                    string dateOfBirthRaw = (string)item["MemberDateOfBirth"] ?? "";
                                    string nationality = (string)item["Nationality"] ?? "";
                                    string nric = (string)item["NRIC"] ?? "";
                                    string occupation = (string)item["Occupation"] ?? "";
                                    string maritalStatus = (string)item["MaritalStatus"] ?? "";
                                    string email = (string)item["Email"] ?? "";
                                    string block = (string)item["Block"] ?? "";
                                    string buildingName = (string)item["BuildingName"] ?? "";
                                    string streetName = ((string)item["StreetName"] ?? "").Replace("'", "''");
                                    string streetName2 = ((string)item["StreetName2"] ?? "").Replace("'", "''");
                                    string unitNo = ((string)item["UnitNo"] ?? "").Replace("'", "''");
                                    string city = ((string)item["City"] ?? "").Replace("'", "''");
                                    string country = ((string)item["Country"] ?? "").Replace("'", "''");
                                    string zipCode = ((string)item["ZipCode"] ?? "").Replace("'", "''");
                                    string mobile = ((string)item["Mobile"] ?? "").Replace("'", "''");
                                    string office = ((string)item["Office"] ?? "").Replace("'", "''");
                                    string fax = ((string)item["Fax"] ?? "").Replace("'", "''");
                                    string home = ((string)item["Home"] ?? "").Replace("'", "''");
                                    string expiryDateRaw = (string)item["MemberExpiryDate"] ?? "";
                                    string remarks = ((string)item["Remarks"] ?? "").Replace("'", "''");
                                    string subscriptionDateRaw = (string)item["MemberSubscriptionDate"] ?? "";
                                    string userfld1 = ((string)item["userfld1"] ?? "").Replace("'", "''");
                                    string userfld2 = ((string)item["userfld2"] ?? "").Replace("'", "''");
                                    string userfld3 = ((string)item["userfld3"] ?? "").Replace("'", "''");
                                    string userfld4 = ((string)item["userfld4"] ?? "").Replace("'", "''");
                                    string userfld5 = ((string)item["userfld5"] ?? "").Replace("'", "''");
                                    string userfld6 = ((string)item["userfld6"] ?? "").Replace("'", "''");
                                    string userfld7 = ((string)item["userfld7"] ?? "").Replace("'", "''");
                                    string userfld8 = ((string)item["userfld8"] ?? "").Replace("'", "''");
                                    string userfld9 = ((string)item["userfld9"] ?? "").Replace("'", "''");
                                    string userfld10 = ((string)item["userfld10"] ?? "").Replace("'", "''");
                                    string userint1 = ((string)item["userint1"] ?? "").Replace("'", "''");
                                    string userint2 = ((string)item["userint2"] ?? "").Replace("'", "''");
                                    string userint3 = ((string)item["userint3"] ?? "").Replace("'", "''");
                                    string userint4 = ((string)item["userint4"] ?? "").Replace("'", "''");
                                    string userint5 = ((string)item["userint5"] ?? "").Replace("'", "''");
                                    string userflag1 = ((string)item["userflag1"] ?? "").Replace("'", "''");
                                    string userflag2 = ((string)item["userflag2"] ?? "").Replace("'", "''");
                                    string userflag3 = ((string)item["userflag3"] ?? "").Replace("'", "''");
                                    string userflag4 = ((string)item["userflag4"] ?? "").Replace("'", "''");
                                    string userflag5 = ((string)item["userflag5"] ?? "").Replace("'", "''");
                                    string userfloat1 = ((string)item["userfloat1"] ?? "").Replace("'", "''");
                                    string userfloat2 = ((string)item["userfloat2"] ?? "").Replace("'", "''");
                                    string userfloat3 = ((string)item["userfloat3"] ?? "").Replace("'", "''");
                                    string userfloat4 = ((string)item["userfloat4"] ?? "").Replace("'", "''");
                                    string userfloat5 = ((string)item["userfloat5"] ?? "").Replace("'", "''");
                                    string deleted = ((string)(item["Deleted"] ?? "False")).ToLower() == "false" ? "0" : "1";
                                    string createdOnRaw = (string)item["CreatedOn"] ?? "";
                                    string modifiedOnRaw = (string)item["ModifiedOn"] ?? "";
                                    //DateTime createdOn = DateTime.ParseExact(createdOnRaw, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    //DateTime modifiedOn = DateTime.ParseExact(modifiedOnRaw, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    DateTime expiryDate = new DateTime(1900, 1, 1);
                                    if (string.IsNullOrEmpty(expiryDateRaw) == false)
                                    {
                                        expiryDate = DateTime.ParseExact(expiryDateRaw, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    }
                                    string tempDeleted = (string)item["Deleted"];

                                    QueryCommand cmd = new QueryCommand("select count(*) from Membership a where a.MembershipNo='" + membershipNo + "'", "PowerPOS");
                                    int memberCount = (int)DataService.ExecuteScalar(cmd);
                                    string strSql = "";
                                    if (memberCount < 1)
                                    {
                                        strSql = @"
                                        insert into Membership (
                                              MembershipNo
                                            , MembershipGroupId
                                            , Title
                                            , LastName
                                            , FirstName
                                            , ChristianName
                                            , ChineseName
                                            , NameToAppear
                                            , Gender
                                            , DateOfBirth
                                            , Nationality
                                            , Nric
                                            , Occupation
                                            , MaritalStatus
                                            , Email
                                            , Block
                                            , BuildingName
                                            , StreetName
                                            , StreetName2
                                            , UnitNo
                                            , City
                                            , Country
                                            , ZipCode
                                            , Mobile
                                            , Office
                                            , Fax
                                            , Home
                                            , ExpiryDate
                                            , Remarks
                                            , SubscriptionDate
                                            , Userfld1
                                            , Userfld2
                                            , Userfld3
                                            , Userfld4
                                            , Userfld5
                                            , Userfld6
                                            , Userfld7
                                            , Userfld8
                                            , Userfld9
                                            , Userfld10
                                            , Userflag1
                                            , Userflag2
                                            , Userflag3
                                            , Userflag4
                                            , Userflag5
                                            , Userfloat1
                                            , Userfloat2
                                            , Userfloat3
                                            , Userfloat4
                                            , Userfloat5
                                            , Userint1
                                            , Userint2
                                            , Userint3
                                            , Userint4
                                            , Userint5
                                            , Deleted
                                            , CreatedOn
                                            , ModifiedOn
                                            , CreatedBy
                                            , ModifiedBy
                                        )
                                        values (
                                              '" + membershipNo + @"'
                                            , " + membershipGroupId + @"
                                            , '" + title + @"'
                                            , '" + lastName + @"'
                                            , '" + firstName + @"'
                                            , '" + christianName + @"'
                                            , '" + chineseName + @"'
                                            , '" + nameToAppear + @"'
                                            , '" + gender + @"'
                                            , '" + dateOfBirthRaw + @"'
                                            , '" + nationality + @"'
                                            , '" + nric + @"'
                                            , '" + occupation + @"'
                                            , '" + maritalStatus + @"'
                                            , '" + email + @"'
                                            , '" + block + @"'
                                            , '" + buildingName + @"'
                                            , '" + streetName + @"'
                                            , '" + streetName2 + @"'
                                            , '" + unitNo + @"'
                                            , '" + city + @"'
                                            , '" + country + @"'
                                            , '" + zipCode + @"'
                                            , '" + mobile + @"'
                                            , '" + office + @"'
                                            , '" + fax + @"'
                                            , '" + home + @"'
                                            , '" + expiryDate + @"'
                                            , '" + remarks + @"'
                                            , '" + subscriptionDateRaw + @"'
                                            , '" + userfld1 + @"'
                                            , '" + userfld2 + @"'
                                            , '" + userfld3 + @"'
                                            , '" + userfld4 + @"'
                                            , '" + userfld5 + @"'
                                            , '" + userfld6 + @"'
                                            , '" + userfld7 + @"'
                                            , '" + userfld8 + @"'
                                            , '" + userfld9 + @"'
                                            , '" + userfld10 + @"'
                                            , '" + userflag1 + @"'
                                            , '" + userflag2 + @"'
                                            , '" + userflag3 + @"'
                                            , '" + userflag4 + @"'
                                            , '" + userflag5 + @"'
                                            , '" + userfloat1 + @"'
                                            , '" + userfloat2 + @"'
                                            , '" + userfloat3 + @"'
                                            , '" + userfloat4 + @"'
                                            , '" + userfloat5 + @"'
                                            , '" + userint1 + @"'
                                            , '" + userint2 + @"'
                                            , '" + userint3 + @"'
                                            , '" + userint4 + @"'
                                            , '" + userint5 + @"'
                                            , " + deleted + @"
                                            , '" + createdOnRaw + @"'
                                            , '" + modifiedOnRaw + @"'
                                            , 'Synchronizer'
                                            , 'Synchronizer'
                                        )
                                    ";
                                    }
                                    else
                                    {
                                        strSql = @"
                                        update Membership
                                           set MembershipNo = '" + membershipNo + @"'
                                             , MembershipGroupId = " + membershipGroupId + @"
                                             , Title = '" + title + @"'
                                             , LastName = '" + lastName + @"'
                                             , FirstName = '" + firstName + @"'
                                             , ChristianName = '" + christianName + @"'
                                             , ChineseName = '" + chineseName + @"'
                                             , NameToAppear = '" + nameToAppear + @"'
                                             , Gender = '" + gender + @"'
                                             , DateOfBirth = '" + dateOfBirthRaw + @"'
                                             , Nationality = '" + nationality + @"'
                                             , Nric = '" + nric + @"'
                                             , Occupation = '" + occupation + @"'
                                             , MaritalStatus = '" + maritalStatus + @"'
                                             , Email = '" + email + @"'
                                             , Block = '" + block + @"'
                                             , BuildingName = '" + buildingName + @"'
                                             , StreetName = '" + streetName + @"'
                                             , StreetName2 = '" + streetName2 + @"'
                                             , UnitNo = '" + unitNo + @"'
                                             , City = '" + city + @"'
                                             , Country = '" + country + @"'
                                             , ZipCode = '" + zipCode + @"'
                                             , Mobile = '" + mobile + @"'
                                             , Office = '" + office + @"'
                                             , Fax = '" + fax + @"'
                                             , Home = '" + home + @"'
                                             , ExpiryDate = '" + expiryDate + @"'
                                             , Remarks = '" + remarks + @"'
                                             , SubscriptionDate = '" + subscriptionDateRaw + @"'
                                             , Userfld1 = '" + userfld1 + @"'
                                             , Userfld2 = '" + userfld2 + @"'
                                             , Userfld3 = '" + userfld3 + @"'
                                             , Userfld4 = '" + userfld4 + @"'
                                             , Userfld5 = '" + userfld5 + @"'
                                             , Userfld6 = '" + userfld6 + @"'
                                             , Userfld7 = '" + userfld7 + @"'
                                             , Userfld8 = '" + userfld8 + @"'
                                             , Userfld9 = '" + userfld9 + @"'
                                             , Userfld10 = '" + userfld10 + @"'
                                             , userflag1 = '" + userflag1 + @"'
                                             , userflag2 = '" + userflag2 + @"'
                                             , userflag3 = '" + userflag3 + @"'
                                             , userflag4 = '" + userflag4 + @"'
                                             , userflag5 = '" + userflag5 + @"'
                                             , userfloat1 = '" + userfloat1 + @"'
                                             , userfloat2 = '" + userfloat2 + @"'
                                             , userfloat3 = '" + userfloat3 + @"'
                                             , userfloat4 = '" + userfloat4 + @"'
                                             , userfloat5 = '" + userfloat5 + @"'
                                             , userint1 = '" + userint1 + @"'
                                             , userint2 = '" + userint2 + @"'
                                             , userint3 = '" + userint3 + @"'
                                             , userint4 = '" + userint4 + @"'
                                             , userint5 = '" + userint5 + @"'
                                             , Deleted = " + (deleted.ToLower() == "false" || deleted.ToLower() == "0" ? "0" : "1") + @"
                                             , CreatedOn = '" + createdOnRaw + @"'
                                             , ModifiedOn = '" + modifiedOnRaw + @"'
                                             , CreatedBy = 'Synchronizer'
                                             , ModifiedBy = 'Synchronizer'
                                         where MembershipNo = '" + membershipNo + @"'
                                        ";
                                    }

                                    QueryCommand cmdMember = new QueryCommand(strSql, "PowerPOS");

                                    DataService.ExecuteQuery(cmdMember);
                                    Logger.writeLog("Synchronizing member : " + membershipNo);
                                }
                            }

                            progressBarValue = progressBarSyncMembership.Value + membershipLoopIterator;
                            totalMembership = getTotalUnSyncMembershipThatNotCreatedInCurrentPOS();
                            if (progressBarValue >= progressBarSyncMembership.Maximum)
                            {
                                progressBarValue = progressBarSyncMembership.Maximum;
                                loopParameter = true;
                            }

                            MethodInvoker setProgressBarMI = new MethodInvoker(() =>
                            {
                                progressBarSyncMembership.Value = progressBarValue;
                                labelTotalDownloadedMembers.Text = progressBarValue.ToString();
                                //btnSearch_Click(_this, new EventArgs());
                            });
                            if (progressBarSyncMembership.InvokeRequired)
                            {
                                progressBarSyncMembership.Invoke(setProgressBarMI);
                            }
                            else
                            {
                                setProgressBarMI.Invoke();
                            }
                            //btnSearch_Click(this, new EventArgs());
                        }

                        MethodInvoker setSearchMemberInvoker = new MethodInvoker(() =>
                        {
                            btnSearch_Click(_this, new EventArgs());
                        });
                        if (progressBarSyncMembership.InvokeRequired)
                        {
                            progressBarSyncMembership.Invoke(setSearchMemberInvoker);
                        }
                        else
                        {
                            setSearchMemberInvoker.Invoke();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                    }
                }));
                syncThread.Start();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(this, "Cannot sync members from the server.\nPlease, contact your administrator or support team!");
            }
        }

        #region "Close Form"
        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                membershipNo = "";
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void dgvMembersList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    //Get the item no            
                    membershipNo = dgvMembersList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    this.Close();
                }
                else
                {
                    //pop up detail window....
                    string membershipNo = dgvMembersList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    Membership member = new Membership(Membership.Columns.MembershipNo, membershipNo);
                    if (!member.IsNew)
                    {
                        frmNewMembershipEdit f = new frmNewMembershipEdit(member);
                        f.IsReadOnly = true;
                        f.ShowDialog();
                        f.Dispose();
                    }
                }
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                return;
            }

            MethodInvoker searchNotification = new MethodInvoker(() =>
            {
                labelSearchNotification.ForeColor = Color.Blue;
                labelSearchNotification.Text = "Searching members ....";
            });

            if (labelSearchNotification.InvokeRequired)
            {
                progressBarSyncMembership.Invoke(searchNotification);
            }
            else
            {
                searchNotification.Invoke();
            }
            
            

            string searchText = txtSearch.Text;
            //find item using the given text
            MembershipController mbr = new MembershipController();
            ViewMembershipCollection coll = mbr.SearchMembership(searchText);

            if (coll.Count == 0)
            {
                MethodInvoker searchNotificationA = new MethodInvoker(() =>
                {
                    labelSearchNotification.ForeColor = Color.Red;
                    labelSearchNotification.Text = "Members cannot be found!";
                });

                if (labelSearchNotification.InvokeRequired)
                {
                    labelSearchNotification.Invoke(searchNotificationA);
                }
                else
                {
                    searchNotificationA.Invoke();
                }
            }
            else
            {
                MethodInvoker searchNotificationA = new MethodInvoker(() =>
                {
                    labelSearchNotification.ForeColor = Color.Blue;
                    labelSearchNotification.Text = coll.Count + " members found.";
                });

                if (labelSearchNotification.InvokeRequired)
                {
                    labelSearchNotification.Invoke(searchNotificationA);
                }
                else
                {
                    searchNotificationA.Invoke();
                }
            }

            dgvMembersList.AutoGenerateColumns = false;
            this.dgvMembersList.DataSource = coll;
            this.dgvMembersList.Refresh();
        }
    }
}
