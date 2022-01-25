using WinPowerPOS.OrderForms;

namespace WinPowerPOS.AppointmentForms
{
	partial class frmSelectMember
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectMember));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMembersList = new System.Windows.Forms.DataGridView();
            this.btnDetails = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColMembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BirthDae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcSalesPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostalCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.tmrFilter = new System.Windows.Forms.Timer(this.components);
            this.lblTitle = new System.Windows.Forms.Label();
            this.labelTotalNewCreatedMembers = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTotalDownloadedMembers = new System.Windows.Forms.Label();
            this.labelProgressBarSyncMembership = new System.Windows.Forms.Label();
            this.progressBarSyncMembership = new System.Windows.Forms.ProgressBar();
            this.pagingControl = new WinPowerPOS.DataGridViewPagingControl();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembersList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMembersList
            // 
            this.dgvMembersList.AllowUserToAddRows = false;
            this.dgvMembersList.AllowUserToDeleteRows = false;
            this.dgvMembersList.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvMembersList, "dgvMembersList");
            this.dgvMembersList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvMembersList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvMembersList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvMembersList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembersList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnDetails,
            this.ColMembershipNo,
            this.GroupName,
            this.NameToAppear,
            this.NRIC,
            this.ExpiryDate,
            this.BirthDae,
            this.dgcSalesPerson,
            this.FirstName,
            this.LastName,
            this.EMail,
            this.Address,
            this.PostalCode,
            this.Country,
            this.City});
            this.dgvMembersList.Name = "dgvMembersList";
            this.dgvMembersList.ReadOnly = true;
            this.dgvMembersList.RowHeadersVisible = false;
            this.dgvMembersList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMembersList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMembersList_CellDoubleClick);
            this.dgvMembersList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMembersList_CellClick);
            // 
            // btnDetails
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.btnDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.btnDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.btnDetails, "btnDetails");
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.ReadOnly = true;
            this.btnDetails.Text = "View";
            this.btnDetails.UseColumnTextForButtonValue = true;
            // 
            // ColMembershipNo
            // 
            this.ColMembershipNo.DataPropertyName = "MembershipNo";
            resources.ApplyResources(this.ColMembershipNo, "ColMembershipNo");
            this.ColMembershipNo.Name = "ColMembershipNo";
            this.ColMembershipNo.ReadOnly = true;
            // 
            // GroupName
            // 
            this.GroupName.DataPropertyName = "GroupName";
            resources.ApplyResources(this.GroupName, "GroupName");
            this.GroupName.Name = "GroupName";
            this.GroupName.ReadOnly = true;
            // 
            // NameToAppear
            // 
            this.NameToAppear.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NameToAppear.DataPropertyName = "NameToAppear";
            resources.ApplyResources(this.NameToAppear, "NameToAppear");
            this.NameToAppear.Name = "NameToAppear";
            this.NameToAppear.ReadOnly = true;
            // 
            // NRIC
            // 
            this.NRIC.DataPropertyName = "NRIC";
            resources.ApplyResources(this.NRIC, "NRIC");
            this.NRIC.Name = "NRIC";
            this.NRIC.ReadOnly = true;
            // 
            // ExpiryDate
            // 
            this.ExpiryDate.DataPropertyName = "ExpiryDate";
            resources.ApplyResources(this.ExpiryDate, "ExpiryDate");
            this.ExpiryDate.Name = "ExpiryDate";
            this.ExpiryDate.ReadOnly = true;
            // 
            // BirthDae
            // 
            this.BirthDae.DataPropertyName = "DateOfBirth";
            resources.ApplyResources(this.BirthDae, "BirthDae");
            this.BirthDae.Name = "BirthDae";
            this.BirthDae.ReadOnly = true;
            // 
            // dgcSalesPerson
            // 
            this.dgcSalesPerson.DataPropertyName = "SalesPersonID";
            resources.ApplyResources(this.dgcSalesPerson, "dgcSalesPerson");
            this.dgcSalesPerson.Name = "dgcSalesPerson";
            this.dgcSalesPerson.ReadOnly = true;
            // 
            // FirstName
            // 
            this.FirstName.DataPropertyName = "FirstName";
            resources.ApplyResources(this.FirstName, "FirstName");
            this.FirstName.Name = "FirstName";
            this.FirstName.ReadOnly = true;
            // 
            // LastName
            // 
            this.LastName.DataPropertyName = "lastName";
            resources.ApplyResources(this.LastName, "LastName");
            this.LastName.Name = "LastName";
            this.LastName.ReadOnly = true;
            // 
            // EMail
            // 
            this.EMail.DataPropertyName = "email";
            resources.ApplyResources(this.EMail, "EMail");
            this.EMail.Name = "EMail";
            this.EMail.ReadOnly = true;
            // 
            // Address
            // 
            this.Address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Address.DataPropertyName = "streetname";
            resources.ApplyResources(this.Address, "Address");
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // PostalCode
            // 
            this.PostalCode.DataPropertyName = "ZipCode";
            resources.ApplyResources(this.PostalCode, "PostalCode");
            this.PostalCode.Name = "PostalCode";
            this.PostalCode.ReadOnly = true;
            // 
            // Country
            // 
            this.Country.DataPropertyName = "Country";
            resources.ApplyResources(this.Country, "Country");
            this.Country.Name = "Country";
            this.Country.ReadOnly = true;
            // 
            // City
            // 
            this.City.DataPropertyName = "City";
            resources.ApplyResources(this.City, "City");
            this.City.Name = "City";
            this.City.ReadOnly = true;
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.TextChanged += new System.EventHandler(this.btSearch_TextChanged);
            // 
            // tmrFilter
            // 
            this.tmrFilter.Interval = 300;
            this.tmrFilter.Tick += new System.EventHandler(this.tmrFilter_Tick);
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // labelTotalNewCreatedMembers
            // 
            this.labelTotalNewCreatedMembers.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelTotalNewCreatedMembers, "labelTotalNewCreatedMembers");
            this.labelTotalNewCreatedMembers.Name = "labelTotalNewCreatedMembers";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labelTotalDownloadedMembers
            // 
            this.labelTotalDownloadedMembers.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelTotalDownloadedMembers, "labelTotalDownloadedMembers");
            this.labelTotalDownloadedMembers.Name = "labelTotalDownloadedMembers";
            // 
            // labelProgressBarSyncMembership
            // 
            this.labelProgressBarSyncMembership.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelProgressBarSyncMembership, "labelProgressBarSyncMembership");
            this.labelProgressBarSyncMembership.Name = "labelProgressBarSyncMembership";
            // 
            // progressBarSyncMembership
            // 
            resources.ApplyResources(this.progressBarSyncMembership, "progressBarSyncMembership");
            this.progressBarSyncMembership.Name = "progressBarSyncMembership";
            // 
            // pagingControl
            // 
            resources.ApplyResources(this.pagingControl, "pagingControl");
            this.pagingControl.AutoGenerateColumns = false;
            this.pagingControl.CurrentPage = 0;
            this.pagingControl.DataGrid = null;
            this.pagingControl.Name = "pagingControl";
            this.pagingControl.PageSize = 50;
            // 
            // frmSelectMember
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.labelTotalNewCreatedMembers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelTotalDownloadedMembers);
            this.Controls.Add(this.labelProgressBarSyncMembership);
            this.Controls.Add(this.progressBarSyncMembership);
            this.Controls.Add(this.pagingControl);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.dgvMembersList);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "frmSelectMember";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAddMember_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembersList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.DataGridView dgvMembersList;
		internal System.Windows.Forms.Button btnOk;
		internal System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox tbSearch;
		private System.Windows.Forms.Timer tmrFilter;
		private System.Windows.Forms.Label lblTitle;
        private DataGridViewPagingControl pagingControl;
        private System.Windows.Forms.DataGridViewButtonColumn btnDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BirthDae;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcSalesPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn PostalCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Country;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        private System.Windows.Forms.Label labelTotalNewCreatedMembers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTotalDownloadedMembers;
        private System.Windows.Forms.Label labelProgressBarSyncMembership;
        private System.Windows.Forms.ProgressBar progressBarSyncMembership;

	}
}