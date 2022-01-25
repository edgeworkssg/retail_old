namespace PowerInventory
{
    partial class frmAddMember
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddMember));
            this.pnlAddItems = new System.Windows.Forms.Panel();
            this.labelSearchNotification = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvMembersList = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColMembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BirthDae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcSalesPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostalCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancelAdd = new System.Windows.Forms.Button();
            this.pnlAddItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembersList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlAddItems
            // 
            resources.ApplyResources(this.pnlAddItems, "pnlAddItems");
            this.pnlAddItems.BackColor = System.Drawing.Color.White;
            this.pnlAddItems.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.pnlAddItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAddItems.Controls.Add(this.labelSearchNotification);
            this.pnlAddItems.Controls.Add(this.txtSearch);
            this.pnlAddItems.Controls.Add(this.btnSearch);
            this.pnlAddItems.Controls.Add(this.dgvMembersList);
            this.pnlAddItems.Controls.Add(this.btnCancelAdd);
            this.pnlAddItems.Name = "pnlAddItems";
            // 
            // labelSearchNotification
            // 
            this.labelSearchNotification.BackColor = System.Drawing.Color.Transparent;
            this.labelSearchNotification.ForeColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.labelSearchNotification, "labelSearchNotification");
            this.labelSearchNotification.Name = "labelSearchNotification";
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.Name = "txtSearch";
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvMembersList
            // 
            this.dgvMembersList.AllowUserToAddRows = false;
            this.dgvMembersList.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvMembersList, "dgvMembersList");
            this.dgvMembersList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembersList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnAdd,
            this.ColMembershipNo,
            this.GroupName,
            this.NameToAppear,
            this.NRIC,
            this.ExpiryDate,
            this.BirthDae,
            this.dgcSalesPerson,
            this.FirstName,
            this.LastName,
            this.Mobile,
            this.EMail,
            this.Address,
            this.PostalCode,
            this.Country,
            this.City});
            this.dgvMembersList.Name = "dgvMembersList";
            this.dgvMembersList.ReadOnly = true;
            this.dgvMembersList.RowHeadersVisible = false;
            this.dgvMembersList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMembersList_CellClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ReadOnly = true;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseColumnTextForButtonValue = true;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            // 
            // ColMembershipNo
            // 
            this.ColMembershipNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
            // Mobile
            // 
            this.Mobile.DataPropertyName = "Mobile";
            resources.ApplyResources(this.Mobile, "Mobile");
            this.Mobile.Name = "Mobile";
            this.Mobile.ReadOnly = true;
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
            // btnCancelAdd
            // 
            this.btnCancelAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelAdd.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            resources.ApplyResources(this.btnCancelAdd, "btnCancelAdd");
            this.btnCancelAdd.ForeColor = System.Drawing.Color.White;
            this.btnCancelAdd.Name = "btnCancelAdd";
            this.btnCancelAdd.UseVisualStyleBackColor = false;
            this.btnCancelAdd.Click += new System.EventHandler(this.btnCancelAdd_Click);
            // 
            // frmAddMember
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlAddItems);
            this.MinimizeBox = false;
            this.Name = "frmAddMember";
            this.pnlAddItems.ResumeLayout(false);
            this.pnlAddItems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembersList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlAddItems;
        internal System.Windows.Forms.Button btnCancelAdd;
        private System.Windows.Forms.DataGridView dgvMembersList;
        private System.Windows.Forms.DataGridViewButtonColumn btnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BirthDae;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcSalesPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mobile;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn PostalCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Country;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label labelSearchNotification;
    }
}