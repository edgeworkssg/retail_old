using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AppointmentBook;
using AppointmentBook.Model;
using PowerPOS;
using WinPowerPOS.EditBillForms;
using WinPowerPOS.MembershipForms;
using System.Data;
using PowerPOS.Container;
using System.ComponentModel;

namespace WinPowerPOS.AppointmentForms
{
	public partial class frmAppointmentEditor : Form
	{
		private AppointmentBookControlOptions _options;
		private DateTime _day;
		private Membership _member;
		private readonly List<AppointmentItem> _items = new List<AppointmentItem>();
		private Appointment _appointment;
        public TimeSpan ProposedTime;
		private frmSelectServices _selectServicesForm;
		private frmSelectMember _selectMemberForm;
        private frmRoomListing _roomListing;

        public BackgroundWorker SyncSalesThread;
        public BackgroundWorker SyncAppointmentThread;

		public int SelectedEmployeeIndex
		{
			get { return cbSalesPerson.SelectedIndex; }
		}

        public int SelectedResourcesIndex
        {
            get { return cbResources.SelectedIndex; }
        }

		public int Duration
		{
			get { return (int) nudDuration.Value; }
		}

		public string Description
		{
			get { return tbDescription.Text; }
			set { tbDescription.Text = value; }
		}

		public Color Color
		{
			get { return btnColor.BackColor; }
		}

		public Color FontColor
		{
			get { return btnFontColor.BackColor; }
		}

		public Membership Member
		{
			get { return _member; }
			set { SetMember(value); }
		}

		public List<AppointmentItem> Items
		{
			get { return _items; }
		}

		public frmAppointmentEditor()
		{
			InitializeComponent();
			dgvItemList.AutoGenerateColumns = false;
			UpdateServicesView();
            ShowHideBasePrivileges();
		}

        public void ShowHideBasePrivileges()
        {
            bool isCreateAppointment = false;
            DataRow[] dr3 = UserInfo.privileges.Select("privilegeName='" + PrivilegesController.CREATE_APPOINTMENT + "'");
            if (UserInfo.username.ToLower() == "edgeworks" || dr3.Length > 0)
                isCreateAppointment = true;

            if (!isCreateAppointment)
            {
                btnClearMember.Visible = false;
                btnOk.Visible = false;

                dgvItemList.Columns[0].Visible = false;
            }
            else
            {
                btnClearMember.Visible = true;
                btnOk.Visible = true;

                dgvItemList.Columns[0].Visible = true;
            }
        }

		public void SetOptions(AppointmentBookControlOptions options)
		{
			_options = options;
		}

        public void ResetForm()
        {
            try
            {
                try
                {
                    cbSalesPerson.SelectedIndex = 0;
                    cbResources.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                nudDuration.Value = 0;
                btnClearMember_Click(btnClearMember, new EventArgs());
                tbDescription.Text = "";
                btnFontColor.ForeColor = Color.White;
                btnColor.ForeColor = Color.Chocolate;
            }
            catch (Exception e)
            {
                Logger.writeLog(e);
            }
        }

		public DialogResult ShowNew(AppointmentBookData data, DateTime day, int employeeIndex)
		{
            ResetForm();
			lblSalesPerson.Enabled = cbSalesPerson.Enabled = cbResources.Enabled = true;
			btnCreateInvoice.Enabled = false;

			_day = day;

			nudDuration.Increment = _options.TimeResolution;

			AdjustDurationButtons();

			cbSalesPerson.Items.Clear();
			
			foreach (var employee in data.Employees)
				cbSalesPerson.Items.Add(employee);

			if (cbSalesPerson.Items.Count == 0)
				employeeIndex = -1;
			
			if (cbSalesPerson.Items.Count > employeeIndex)
				cbSalesPerson.SelectedIndex = employeeIndex;

            cbSalesPerson_SelectedIndexChanged(cbSalesPerson, null);

			cbSalesPerson.SelectedIndexChanged += cbSalesPerson_SelectedIndexChanged;

            cbResources.Items.Clear();

            foreach (var resource in data.Resources)
                cbResources.Items.Add(resource);
            
            if(cbResources.Items.Count > 0)
                cbResources.SelectedIndex = 0;

			return ShowDialog();
		}

        public DialogResult ShowNewResource(AppointmentBookData data, DateTime day, int resourceIndex)
        {
            ResetForm();
            lblSalesPerson.Enabled = cbSalesPerson.Enabled = cbResources.Enabled = true;
            btnCreateInvoice.Enabled = false;

            _day = day;

            nudDuration.Increment = _options.TimeResolution;

            AdjustDurationButtons();

            cbSalesPerson.Items.Clear();

            foreach (var employee in data.Employees)
                cbSalesPerson.Items.Add(employee);

            cbSalesPerson_SelectedIndexChanged(cbSalesPerson, null);

            cbSalesPerson.SelectedIndexChanged += cbSalesPerson_SelectedIndexChanged;

            cbResources.Items.Clear();

            foreach (var resource in data.Resources)
                cbResources.Items.Add(resource);

            if (cbResources.Items.Count > 0)
                cbResources.SelectedIndex = resourceIndex;

            return ShowDialog();
        }

		private void AdjustDurationButton(Button button, int minutes)
		{
			button.Tag = minutes;
			button.Text = string.Format("+{0} min", minutes);
			button.Enabled = nudDuration.Value + minutes <= nudDuration.Maximum;
		}

		private void AdjustDurationButtons()
		{
			btnClearDuration.Enabled = nudDuration.Enabled && nudDuration.Maximum >= _options.TimeResolution;
			AdjustDurationButton(btnDuration1, _options.TimeResolution * 1);
			AdjustDurationButton(btnDuration2, _options.TimeResolution * 2);
			AdjustDurationButton(btnDuration3, _options.TimeResolution * 3);
			AdjustDurationButton(btnDuration4, _options.TimeResolution * 4);
			AdjustDurationButton(btnDuration5, _options.TimeResolution * 6);
		}

		private void cbSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				var employee = (Employee) cbSalesPerson.SelectedItem;

				var max = (employee != null) ? TimeHelper.FindLongestFreeInterval(employee, _options, _day).TotalMinutes : 0;

				if (max < _options.TimeResolution)
				{
					nudDuration.Enabled = lblDuration.Enabled = false;
					nudDuration.Value = nudDuration.Maximum = 0;

				}
				else
				{
					nudDuration.Enabled = lblDuration.Enabled = true;

					var value = nudDuration.Value;
					if (value < _options.TimeResolution)
						value = _options.TimeResolution;
					
					if (value > (decimal) max)
						value = (decimal) max;

					nudDuration.Value = 0;
					nudDuration.Maximum = (decimal) max;
					nudDuration.Value = value;
				}
				AdjustDurationButtons();

				UpdateControls();
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void UpdateControls()
		{
			btnOk.Enabled = cbSalesPerson.SelectedIndex >= 0 && nudDuration.Value > 0;
			AdjustDurationButtons();
		}

		private void nudDuration_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateControls();
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void tbDescription_TextChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateControls();
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void btnColor_Click(object sender, EventArgs e)
		{
			try
			{
				colorDialog.Color = btnColor.BackColor;
				if (colorDialog.ShowDialog() == DialogResult.OK)
					btnColor.BackColor = colorDialog.Color;
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void btnFontColor_Click(object sender, EventArgs e)
		{
			try
			{
				colorDialog.Color = btnFontColor.BackColor;
				if (colorDialog.ShowDialog() == DialogResult.OK)
					btnFontColor.BackColor = colorDialog.Color;
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}
        private AppointmentBookData _data;
		public DialogResult ShowEdit(AppointmentBookData data, TimeSlot timeSlot)
		{
            _data = data;
			lblSalesPerson.Enabled = cbSalesPerson.Enabled = cbResources.Enabled = false;
            btnCreateInvoice.Enabled = true;
			
			_appointment = new Appointment(timeSlot.Id);

			if (!string.IsNullOrEmpty(_appointment.OrderHdrID) && !(new OrderHdr(_appointment.OrderHdrID).IsVoided))
				btnCreateInvoice.Text = "SHOW INVOICE ...";
			else
				btnCreateInvoice.Text = "CREATE INVOICE ...";

			nudDuration.Increment = _options.TimeResolution;

			var maxDuration = TimeHelper.GetMaxDuration(timeSlot.Employee, _options, timeSlot.StartTime);
			var value = timeSlot.Duration;
			if (value > maxDuration)
				value = maxDuration;
			
			nudDuration.Value = 0;
			nudDuration.Maximum = (decimal)maxDuration.TotalMinutes;
			nudDuration.Value = (decimal)value.TotalMinutes;

			AdjustDurationButtons();

            cbSalesPerson.Items.Clear();

			foreach (var employee in data.Employees)
				cbSalesPerson.Items.Add(employee);

			cbSalesPerson.SelectedItem = timeSlot.Employee;

            cbResources.Items.Clear();

            foreach (var resource in data.Resources)
                cbResources.Items.Add(resource);

            if (cbResources.Items.Count > 0)
                cbResources.SelectedItem = timeSlot.Resource;

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false))
            {
                //for old appointment that don't have rooom
                if(timeSlot.Resource == null || timeSlot.Resource.ResourceID == "ROOM_DEFAULT")
                {
                    Resource Default = new Resource("ROOM_DEFAULT");
                    AppointmentResource reso = new AppointmentResource() { ResourceID = Default.ResourceID, Name = Default.ResourceName, Capacity = Default.Capacity ?? 0 };

                    cbResources.Items.Add(reso);
                    cbResources.SelectedItem = reso;
                }
            }

            btnColor.BackColor = timeSlot.BackColor;
			btnFontColor.BackColor = timeSlot.FontColor;

			UpdateServicesView();
			UpdateControls();

			return ShowDialog();
		}

        public DialogResult ShowReadOnly(AppointmentBookData data, TimeSlot timeSlot)
        {
            _data = data;
            lblSalesPerson.Enabled = cbSalesPerson.Enabled = cbResources.Enabled = false;
            btnCreateInvoice.Enabled = true;

            _appointment = new Appointment(timeSlot.Id);

            if (!string.IsNullOrEmpty(_appointment.OrderHdrID) && !(new OrderHdr(_appointment.OrderHdrID).IsVoided))
                btnCreateInvoice.Text = "SHOW INVOICE ...";
            else
                btnCreateInvoice.Text = "CREATE INVOICE ...";

            nudDuration.Increment = _options.TimeResolution == null ? 10 : _options.TimeResolution;

            var maxDuration = TimeHelper.GetMaxDuration(timeSlot.Employee, _options, timeSlot.StartTime);
            var value = timeSlot.Duration;
            if (value > maxDuration)
                value = maxDuration;

            nudDuration.Value = 0;
            nudDuration.Maximum = (decimal)maxDuration.TotalMinutes;
            nudDuration.Value = (decimal)value.TotalMinutes;

            AdjustDurationButtons();

            cbSalesPerson.Items.Clear();

            foreach (var employee in data.Employees)
                cbSalesPerson.Items.Add(employee);

            cbSalesPerson.SelectedItem = timeSlot.Employee;

            cbResources.Items.Clear();

            foreach (var resource in data.Resources)
                cbResources.Items.Add(resource);

            cbResources.SelectedItem = timeSlot.Resource;

            btnColor.BackColor = timeSlot.BackColor;
            btnFontColor.BackColor = timeSlot.FontColor;

            UpdateServicesView();
            UpdateControls();

            btnCreateInvoice.Enabled = btnRoomListing.Visible = btnCancel.Visible = btnSelectMember.Enabled = btnSelectServices.Enabled = btnClearMember.Visible = false;
            dgvItemList.Columns[0].Visible = false;
            dgvPreviousServices.Columns["colReadd"].Visible = false;

            return ShowDialog();
        }

       	private void btnSelectMember_Click(object sender, EventArgs e)
		{
			try
			{
				if (_selectMemberForm == null)
					_selectMemberForm = new frmSelectMember();

				if (_selectMemberForm.ShowDialog() == DialogResult.OK)
					SetMember(_selectMemberForm.SelectedMember);
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void SetMember(Membership member)
		{
			_member = member;

			dgvPreviousServices.AutoGenerateColumns = false;

			btnMemberInfo.Enabled = btnClearMember.Enabled = member != null;
            btnAddNewMember.Enabled = !(member != null);

			if (member == null)
			{
				lblMemberName.Text = "No Member selected";
				dgvPreviousServices.DataSource = null;
				lblMemberInfo.Text = string.Empty;
			}
			else
			{
				lblMemberName.Text = _member.NameToAppear;

				var sb = new StringBuilder();

				if (!string.IsNullOrEmpty(_member.FirstName) || !string.IsNullOrEmpty(_member.LastName))
					sb.AppendFormat("FullName: {0} {1}", _member.FirstName, _member.LastName);


				if (!string.IsNullOrEmpty(_member.Mobile))
				{
					if (sb.Length > 0) sb.Append(",\n");
					sb.AppendFormat("Mobile: {0}", _member.Mobile);
				}

				if (!string.IsNullOrEmpty(_member.Email))
				{
					if (sb.Length > 0) sb.Append(",\n");
					sb.AppendFormat("Email: {0}", _member.Email);
				}

				if (_member.DateOfBirth > DateTime.MinValue)
				{
					if (sb.Length > 0) sb.Append(",\n");
					sb.AppendFormat("Birthday: {0}", _member.DateOfBirth);
				}

				if (!string.IsNullOrEmpty(_member.StreetName) || !string.IsNullOrEmpty(_member.StreetName2))
				{
					if (sb.Length > 0) sb.Append(",\n");
					sb.AppendFormat("Address: {0} {1}", _member.StreetName, _member.StreetName2);
				}

				if (!string.IsNullOrEmpty(_member.Nric))
				{
					if (sb.Length > 0) sb.Append(",\n");
					sb.AppendFormat("NRIC: {0}", _member.Nric);
				}

				if (!string.IsNullOrEmpty(_member.Remarks))
				{
					if (sb.Length > 0) sb.Append(",\n");
					sb.AppendFormat("Remarks: {0}", _member.Remarks);
				}

				lblMemberInfo.Text = sb.ToString();

                int rowtotal = 0;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipSummaryRowNumber), out rowtotal))
                {
                    rowtotal = 5000;
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.DownloadAllRecentPurchase), false))
                {
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    SyncClientController.Load_WS_URL();
                    ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.GetPastTransaction(_member.MembershipNo, rowtotal);
                    if (data != null)
                    {
                        DataSet ds = SyncClientController.DecompressDataSetFromByteArray(data);
                        dgvPreviousServices.DataSource = ds.Tables[0];
                    }
                }
                else
                {
                    dgvPreviousServices.DataSource = _member.GetPastTransaction(rowtotal, false);
                }
			}
		}

		private void btnSelectServices_Click(object sender, EventArgs e)
		{
			try
			{
				if (_selectServicesForm == null)
					_selectServicesForm = new frmSelectServices();

				_selectServicesForm.Clear();

				if (_selectServicesForm.ShowDialog() == DialogResult.OK)
				{
					foreach (var item in _selectServicesForm.SelectedItems)
						_items.Add(new AppointmentItem {ItemNo = item.Value.ItemNo, Quantity = 1, UnitPrice = item.Value.RetailPrice, Deleted = false});

					UpdateServicesView();
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void dgvItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex == colRemove.Index)
				{
					_items.RemoveAt(e.RowIndex);
					UpdateServicesView();
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void dgvPreviousServices_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex == colReadd.Index)
				{
					var itemNo = (string)dgvPreviousServices.Rows[e.RowIndex].Cells[dgvcItemNo.Index].Value;
					var price = (decimal)dgvPreviousServices.Rows[e.RowIndex].Cells[dgvcUnitPrice.Index].Value;
					_items.Add(new AppointmentItem { ItemNo = itemNo, Quantity = 1, UnitPrice = price, Deleted = false});
					UpdateServicesView();
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void UpdateServicesView()
		{
            DataTable dt = _items.ToDataTable<AppointmentItem>();
            dt.Columns.Add("ItemName");
            dt.Columns.Add("CategoryName");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ItemNo"] != null && dr["ItemNo"] != "")
                {
                    Item i = new Item(dr["ItemNo"].ToString());
                    if (i != null && i.ItemNo != "")
                    {
                        dr["ItemName"] = i.ItemName;
                        dr["CategoryName"] = i.CategoryName;
                    }
                }
            }
			dgvItemList.DataSource = dt;
		}

		private void dgvPreviousServices_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex != colReadd.Index)
				{
					var itemNo = (string) dgvPreviousServices.Rows[e.RowIndex].Cells[dgvcItemNo.Index].Value;
					var price  = (decimal) dgvPreviousServices.Rows[e.RowIndex].Cells[dgvcUnitPrice.Index].Value;
					_items.Add(new AppointmentItem { ItemNo = itemNo, Quantity = 1, UnitPrice = price, Deleted = false });
					UpdateServicesView();
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void dgvItemList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex != colRemove.Index)
				{
					_items.RemoveAt(e.RowIndex);
					UpdateServicesView();
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		public void ClearServices()
		{
			_items.Clear();
			UpdateServicesView();
		}

		private void btnDuration_Click(object sender, EventArgs e)
		{
			try
			{
				var minutes = (int) ((Button) sender).Tag;

				if (nudDuration.Value + minutes > nudDuration.Maximum)
					minutes = (int)(nudDuration.Maximum - nudDuration.Value);

				nudDuration.Value += minutes;
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void btnMemberInfo_Click(object sender, EventArgs e)
		{
			try
			{
				if (_member != null)
				{
					var infoForm = new frmMembershipViewInfo(_member.MembershipNo);
					infoForm.ShowDialog(this);
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void btnClearMember_Click(object sender, EventArgs e)
		{
			try
			{
				SetMember(null);
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void btnCreateInvoice_Click(object sender, EventArgs e)
		{
			try
			{
                if (!string.IsNullOrEmpty(_appointment.OrderHdrID) && !(new OrderHdr(_appointment.OrderHdrID).IsVoided))
                    (new frmViewBillDetail { OrderHdrID = _appointment.OrderHdrID }).ShowDialog();
                else
                {
                    if (FormController.ShowInvoiceWithReturn(_appointment.Id, SyncSalesThread, SyncAppointmentThread))
                    {
                        this.Close();
                    }
                }
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void btnClearDuration_Click(object sender, EventArgs e)
		{
			nudDuration.Value = _options.TimeResolution;
		}

        public bool FindNewTime = true;
        private void frmAppointmentEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                bool isValid = true;
                bool membershipMandatory = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.MembershipMandatory), false);
                bool serviceMandatory = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.ServicesMandatory), false);
                string message = "Save Appointment Failed!";
                Employee selEmp = (Employee)cbSalesPerson.SelectedItem;
                AppointmentResource selResource = (AppointmentResource)cbResources.SelectedItem;
                string status = "";

                Guid id = Guid.NewGuid();
                if (_appointment != null && _appointment.Id != null)
                {
                    Logger.writeLog(">>> App collide not null"+ FindNewTime.ToString());
                    id = _appointment.Id;
                }

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.DisableTimeCollisionCheck), false))
                {
                    if (!AppointmentController.CheckCollision(id.ToString()
                        , selEmp.Id, _day.Date.Add(ProposedTime), this.Duration, out status))
                    {
                        Logger.writeLog(">>> App collide " + FindNewTime.ToString());
                        if (!FindNewTime)
                        {
                            message += Environment.NewLine + "- Appointment collide with other.";
                            isValid = false;
                        }
                        else
                        {
                            MessageBox.Show("Appointment collide with other");
                        }
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false))
                {
                    if (!AppointmentController.CheckCollisionResource(id.ToString()
                        , selEmp.Id, selResource.ResourceID , _day.Date.Add(ProposedTime), this.Duration, out status))
                    {
                        Logger.writeLog(">>> App collide " + FindNewTime.ToString());
                        if (!FindNewTime)
                        {
                            message += Environment.NewLine + "- Resource is full.";
                            isValid = false;
                        }
                        else
                        {
                            MessageBox.Show("Resource is full");
                        }
                    }
                }

                if (membershipMandatory && this.Member == null)
                {
                    message += Environment.NewLine + "- Please select member.";
                    isValid = false;
                }

                if (serviceMandatory && (this.Items == null || this.Items.Count == 0))
                {
                    message += Environment.NewLine + "- Please select services.";
                    isValid = false;
                }

                if (!isValid)
                {
                    MessageBox.Show(message);
                    e.Cancel = true;
                }
            }
        }

        private void frmAppointmentEditor_Load(object sender, EventArgs e)
        {
            ShowHideBasePrivileges();

            btnRoomListing.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false);
            labelRoom.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false);
            cbResources.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false);

            btnSelectMember.TextImageRelation = TextImageRelation.ImageAboveText;
            btnSelectMember.Text = string.Format("Select {0}", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Member");
            btnAddNewMember.Text = string.Format("Add New {0}", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Member");
            btnMemberInfo.Text = string.Format("{0} Info", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Member");
            btnClearMember.Text = string.Format("Clear {0}", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Member");
            lblMemberName.Text = string.Format("Clear {0}", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Member");

            
            
        }

        private void btnRoomListing_Click(object sender, EventArgs e)
        {
            if (_roomListing == null)
                _roomListing = new frmRoomListing();

            _roomListing.ShowDialog();
        }

        private void btnAddNewMember_Click(object sender, EventArgs e)
        {
            frmNewMembershipEdit f = new frmNewMembershipEdit();
            f.StartPosition = FormStartPosition.CenterScreen;
            //f.membershipNo = "";
            f.IsReadOnly = false;
            //f.EnableToSave = true;
            f.ShowDialog();
            if (f.IsUpdated)
            {
                SetMember(f.EditedMember);
            }
            f.Dispose();
        }
	}

}
