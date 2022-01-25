using System;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using WinPowerPOS.MembershipForms;
using PowerPOSLib.PowerPOSSync;
using System.Net;
using System.Collections.Specialized;
using PowerPOS.Container;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SubSonic;
using System.Threading;
using System.Globalization;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmSelectMember: Form
    {
	    private const string _query = "SELECT * FROM ViewMembership WHERE DELETED = 0";
	    private const string _sortingColumn = "NameToAppear";

	    public Membership SelectedMember = null;

		public frmSelectMember()
        {
            InitializeComponent();
			dgvMembersList.AutoGenerateColumns = false;
			UpdateControls();

			pagingControl.DataGrid = dgvMembersList;
			pagingControl.SetQuery(_query, _sortingColumn);
        }

	    private void frmAddMember_Load(object sender, EventArgs e)
	    {
		    try
		    {
			    ActiveControl = tbSearch;
			    tbSearch.Focus();

                //Sync membership data
                bool isLocalhost = SyncClientController.WS_URL.StartsWith("http://localhost");
                string MembershipSync = AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipSyncSegmentSize);
                
                progressBarSyncMembership.Width = dgvMembersList.Width;
                //SyncMemberData();
                //if (MembershipSync != "")
                //    if (!isLocalhost)
                //        SyncMemberData();
		    }
		    catch (Exception exception)
		    {
			    frmError.ShowError(this, exception);
		    }
		}

	    private void btSearch_TextChanged(object sender, EventArgs e)
	    {
		    try
		    {
			    tmrFilter.Stop();
			    tmrFilter.Start();
		    }
		    catch (Exception exception)
		    {
			    frmError.ShowError(this, exception);
		    }
	    }

	    private void tmrFilter_Tick(object sender, EventArgs e)
	    {
		    try
		    {
			    tmrFilter.Stop();

				var query = new StringBuilder(_query);

				query.Append(" AND (");

			    var parts = tbSearch.Text.Split(new[] {' ', '.', ',', ';', ':', '-', '\t', '/'});
			    for (int i = 0; i < parts.Length; i++)
			    {
				    var part = parts[i];

				    if (i > 0) query.Append(" AND ");
					query.Append("(ISNULL(nametoappear,'') + " +
				    "ISNULL(firstname,'') + " +
				    "ISNULL(lastname,'') + " +
				    "ISNULL(chinesename,'') + " +
				    "ISNULL(christianname,'') + " +
				    "ISNULL(salespersonid,'') + " +
				    "ISNULL(groupname,'') + " +
				    "ISNULL(office,'') + " +
				    "ISNULL(home,'') + " +
				    "ISNULL(mobile,'') + " +
				    "ISNULL(email,'') + " +
				    "ISNULL(nric,'') + " +
				    "ISNULL(streetname,'') + " +
				    "ISNULL(streetname2,'') + " +
				    "ISNULL(zipcode,'') " +
				    "like N'%" + part + "%')");
			    }

			    query.Append(")");

				pagingControl.SetQuery(query.ToString(), _sortingColumn);
		    }
		    catch (Exception exception)
		    {
			    frmError.ShowError(this, exception);
		    }
		    finally
		    {
				UpdateControls();
		    }
	    }

	    private void dgvMembersList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	    {
		    try
		    {
			    if (e.ColumnIndex > 0)
			    {
					var memberNo = dgvMembersList.Rows[e.RowIndex].Cells[ColMembershipNo.Index].Value.ToString();
					SelectedMember = new Membership(memberNo);
				    UpdateControls();
				    DialogResult = DialogResult.OK;
			    }
		    }
		    catch (Exception exception)
		    {
			    frmError.ShowError(this, exception);
		    }
	    }

	    private void UpdateControls()
	    {
			btnOk.Enabled = SelectedMember != null;
	    }

		private void dgvMembersList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				var memberNo = dgvMembersList.Rows[e.RowIndex].Cells[ColMembershipNo.Index].Value.ToString();
				SelectedMember = new Membership(memberNo);
				UpdateControls();
				
				if (e.ColumnIndex == 0)
				{
					var infoView = new frmMembershipViewInfo(memberNo);
					infoView.ShowDialog();
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}


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
                        if (membershipSyncSegmentSizeRaw == null || string.IsNullOrEmpty(membershipSyncSegmentSizeRaw))
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
                                    string firstName = ((string)item["FirstName"] ?? "").Replace("'", "''");
                                    string lastName = ((string)item["LastName"] ?? "").Replace("'", "''");
                                    string nameToAppear = ((string)item["NameToAppear"] ?? "").Replace("'", "''");
                                    string email = (string)item["Email"] ?? "";
                                    string streetName = ((string)item["StreetName"] ?? "").Replace("'", "''");
                                    string mobile = ((string)item["Mobile"] ?? "").Replace("'", "''");
                                    string nric = (string)item["NRIC"] ?? "";
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
                                    string expiryDateRaw = (string)item["MemberExpiryDate"] ?? "";
                                    string deleted = ((string)(item["Deleted"] ?? "False")).ToLower() == "false" ? "0" : "1";
                                    int membershipGroupId = (int)(item["MembershipGroupId"] ?? "0");
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
                                            , FirstName
                                            , LastName
                                            , NameToAppear
                                            , ExpiryDate
                                            , Email
                                            , Mobile
                                            , StreetName
                                            , Nric
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
                                            , '" + firstName + @"'
                                            , '" + lastName + @"'
                                            , '" + nameToAppear + @"'
                                            , '" + expiryDate + @"'
                                            , '" + email + @"'
                                            , '" + mobile + @"'
                                            , '" + streetName + @"'
                                            , '" + nric + @"'
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
                                             , FirstName = '" + firstName + @"'
                                             , LastName = '" + lastName + @"'
                                             , NameToAppear = '" + nameToAppear + @"'
                                             , ExpiryDate = '" + expiryDate + @"'
                                             , Email = '" + email + @"'
                                             , Mobile = '" + mobile + @"'
                                             , StreetName = '" + streetName + @"'
                                             , Nric = '" + nric + @"'
                                             , Userfld1 = '" + userfld1 + @"'
                                             , Userfld2 = '" + userfld2 + @"'
                                             , Userfld3 = '" + userfld3 + @"'
                                             , Userfld4 = '" + userfld4 + @"'
                                             , Userfld5 = '" + userfld5 + @"'
                                             , Userfld6 = '" + userfld6 + @"'
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
                            btSearch_TextChanged(_this, new EventArgs());
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

    }
}
