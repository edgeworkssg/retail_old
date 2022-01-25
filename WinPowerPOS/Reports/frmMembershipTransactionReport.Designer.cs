namespace WinPowerPOS.Reports
{
    partial class frmMembershipTransactionReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMembershipTransactionReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtAppearName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dgvRcpt = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbPosID = new System.Windows.Forms.ComboBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.cmbOutlet = new System.Windows.Forms.ComboBox();
            this.cmbGroupName = new System.Windows.Forms.ComboBox();
            this.txtMembershipNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNRIC = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.cbSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnColView = new System.Windows.Forms.DataGridViewButtonColumn();
            this.OrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PointOfSaleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutletName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderHdrId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRcpt)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAppearName
            // 
            this.txtAppearName.Location = new System.Drawing.Point(96, 44);
            this.txtAppearName.Name = "txtAppearName";
            this.txtAppearName.Size = new System.Drawing.Size(110, 20);
            this.txtAppearName.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(212, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Membership No:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(6, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Start Date:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(212, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Group Name:";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "dd - MMM - yyyy HH:mm:ss";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(81, 44);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.Size = new System.Drawing.Size(170, 20);
            this.dtpEndDate.TabIndex = 20;
            // 
            // dgvRcpt
            // 
            this.dgvRcpt.AllowUserToAddRows = false;
            this.dgvRcpt.AllowUserToDeleteRows = false;
            this.dgvRcpt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRcpt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRcpt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cbSelect,
            this.btnColView,
            this.OrderDate,
            this.MembershipNo,
            this.NameToAppear,
            this.CategoryName,
            this.ItemName,
            this.LineAmount,
            this.PointOfSaleName,
            this.OutletName,
            this.OrderHdrId,
            this.Remark});
            this.dgvRcpt.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvRcpt.Location = new System.Drawing.Point(5, 201);
            this.dgvRcpt.Name = "dgvRcpt";
            this.dgvRcpt.ReadOnly = true;
            this.dgvRcpt.RowHeadersVisible = false;
            this.dgvRcpt.RowHeadersWidth = 23;
            this.dgvRcpt.Size = new System.Drawing.Size(910, 357);
            this.dgvRcpt.TabIndex = 27;
            this.dgvRcpt.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRcpt_RowPostPaint);
            this.dgvRcpt.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRcpt_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "End Date:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(6, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Name To Appear:";
            // 
            // txtRefNo
            // 
            this.txtRefNo.Location = new System.Drawing.Point(96, 21);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Size = new System.Drawing.Size(110, 20);
            this.txtRefNo.TabIndex = 16;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExport.Location = new System.Drawing.Point(533, 16);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(132, 35);
            this.btnExport.TabIndex = 10;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Ref No:";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.btnExport);
            this.groupBox4.Location = new System.Drawing.Point(244, 135);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(671, 57);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.cmbPosID);
            this.groupBox2.Controls.Add(this.cmbCategory);
            this.groupBox2.Controls.Add(this.cmbDepartment);
            this.groupBox2.Controls.Add(this.cmbOutlet);
            this.groupBox2.Controls.Add(this.cmbGroupName);
            this.groupBox2.Controls.Add(this.txtMembershipNo);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtItemName);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtLastName);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtNRIC);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtFirstName);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtAppearName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtRefNo);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(277, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(638, 123);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter";
            // 
            // cmbPosID
            // 
            this.cmbPosID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPosID.FormattingEnabled = true;
            this.cmbPosID.Location = new System.Drawing.Point(498, 43);
            this.cmbPosID.Name = "cmbPosID";
            this.cmbPosID.Size = new System.Drawing.Size(133, 21);
            this.cmbPosID.TabIndex = 42;
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "ALL",
            "Voided",
            "Not Voided"});
            this.cmbCategory.Location = new System.Drawing.Point(498, 19);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(133, 21);
            this.cmbCategory.TabIndex = 41;
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Items.AddRange(new object[] {
            "ALL",
            "Voided",
            "Not Voided"});
            this.cmbDepartment.Location = new System.Drawing.Point(498, 93);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(133, 21);
            this.cmbDepartment.TabIndex = 40;
            // 
            // cmbOutlet
            // 
            this.cmbOutlet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutlet.FormattingEnabled = true;
            this.cmbOutlet.Items.AddRange(new object[] {
            "ALL",
            "Voided",
            "Not Voided"});
            this.cmbOutlet.Location = new System.Drawing.Point(498, 67);
            this.cmbOutlet.Name = "cmbOutlet";
            this.cmbOutlet.Size = new System.Drawing.Size(133, 21);
            this.cmbOutlet.TabIndex = 39;
            // 
            // cmbGroupName
            // 
            this.cmbGroupName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupName.FormattingEnabled = true;
            this.cmbGroupName.Items.AddRange(new object[] {
            "ALL",
            "Voided",
            "Not Voided"});
            this.cmbGroupName.Location = new System.Drawing.Point(302, 44);
            this.cmbGroupName.Name = "cmbGroupName";
            this.cmbGroupName.Size = new System.Drawing.Size(119, 21);
            this.cmbGroupName.TabIndex = 38;
            // 
            // txtMembershipNo
            // 
            this.txtMembershipNo.Location = new System.Drawing.Point(302, 21);
            this.txtMembershipNo.Name = "txtMembershipNo";
            this.txtMembershipNo.Size = new System.Drawing.Size(119, 20);
            this.txtMembershipNo.TabIndex = 37;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(427, 97);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "Department:";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(302, 94);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(119, 20);
            this.txtItemName.TabIndex = 34;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(212, 97);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 13);
            this.label13.TabIndex = 33;
            this.label13.Text = "Item Name:";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(96, 94);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(110, 20);
            this.txtLastName.TabIndex = 32;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(6, 97);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Last Name:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(427, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Outlet:";
            // 
            // txtNRIC
            // 
            this.txtNRIC.Location = new System.Drawing.Point(302, 68);
            this.txtNRIC.Name = "txtNRIC";
            this.txtNRIC.Size = new System.Drawing.Size(119, 20);
            this.txtNRIC.TabIndex = 28;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(212, 71);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "NRIC:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(96, 68);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(110, 20);
            this.txtFirstName.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(6, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "First Name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(427, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "POS ID:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(427, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Category:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 75);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date / Time Filter";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "dd - MMM - yyyy HH:mm:ss";
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(81, 21);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            this.dtpStartDate.Size = new System.Drawing.Size(170, 20);
            this.dtpStartDate.TabIndex = 18;
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            this.fsdExportToExcel.Title = "Save File";
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.btnClose);
            this.groupBox5.Controls.Add(this.btnSearch);
            this.groupBox5.Location = new System.Drawing.Point(5, 135);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(233, 57);
            this.groupBox5.TabIndex = 31;
            this.groupBox5.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(100, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 35);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(9, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 35);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSelect
            // 
            this.cbSelect.Frozen = true;
            this.cbSelect.HeaderText = "";
            this.cbSelect.Name = "cbSelect";
            this.cbSelect.ReadOnly = true;
            this.cbSelect.Visible = false;
            this.cbSelect.Width = 30;
            // 
            // btnColView
            // 
            this.btnColView.HeaderText = "";
            this.btnColView.Name = "btnColView";
            this.btnColView.ReadOnly = true;
            this.btnColView.Text = "View";
            this.btnColView.ToolTipText = "View Order Detail";
            this.btnColView.UseColumnTextForButtonValue = true;
            this.btnColView.Width = 60;
            // 
            // OrderDate
            // 
            this.OrderDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OrderDate.DataPropertyName = "OrderDate";
            dataGridViewCellStyle1.Format = "g";
            dataGridViewCellStyle1.NullValue = "dd MMM yyyy HH:mm";
            this.OrderDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.OrderDate.HeaderText = "Order Date";
            this.OrderDate.MinimumWidth = 120;
            this.OrderDate.Name = "OrderDate";
            this.OrderDate.ReadOnly = true;
            this.OrderDate.Width = 120;
            // 
            // MembershipNo
            // 
            this.MembershipNo.DataPropertyName = "MembershipNo";
            this.MembershipNo.HeaderText = "Membership No";
            this.MembershipNo.MinimumWidth = 105;
            this.MembershipNo.Name = "MembershipNo";
            this.MembershipNo.ReadOnly = true;
            this.MembershipNo.Width = 105;
            // 
            // NameToAppear
            // 
            this.NameToAppear.DataPropertyName = "NameToAppear";
            this.NameToAppear.HeaderText = "Name";
            this.NameToAppear.MinimumWidth = 120;
            this.NameToAppear.Name = "NameToAppear";
            this.NameToAppear.ReadOnly = true;
            this.NameToAppear.Width = 120;
            // 
            // CategoryName
            // 
            this.CategoryName.DataPropertyName = "CategoryName";
            this.CategoryName.HeaderText = "Category";
            this.CategoryName.MinimumWidth = 100;
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.ReadOnly = true;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.HeaderText = "Item Name";
            this.ItemName.MinimumWidth = 150;
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // LineAmount
            // 
            this.LineAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LineAmount.DataPropertyName = "LineAmount";
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = null;
            this.LineAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.LineAmount.HeaderText = "Amount";
            this.LineAmount.MinimumWidth = 80;
            this.LineAmount.Name = "LineAmount";
            this.LineAmount.ReadOnly = true;
            this.LineAmount.Width = 80;
            // 
            // PointOfSaleName
            // 
            this.PointOfSaleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PointOfSaleName.DataPropertyName = "PointOfSaleName";
            this.PointOfSaleName.HeaderText = "Point Of Sale";
            this.PointOfSaleName.MinimumWidth = 100;
            this.PointOfSaleName.Name = "PointOfSaleName";
            this.PointOfSaleName.ReadOnly = true;
            // 
            // OutletName
            // 
            this.OutletName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OutletName.DataPropertyName = "OutletName";
            this.OutletName.HeaderText = "Outlet";
            this.OutletName.MinimumWidth = 100;
            this.OutletName.Name = "OutletName";
            this.OutletName.ReadOnly = true;
            // 
            // OrderHdrId
            // 
            this.OrderHdrId.DataPropertyName = "orderhdrid";
            this.OrderHdrId.HeaderText = "Order Hdr ID";
            this.OrderHdrId.MinimumWidth = 2;
            this.OrderHdrId.Name = "OrderHdrId";
            this.OrderHdrId.ReadOnly = true;
            this.OrderHdrId.Visible = false;
            this.OrderHdrId.Width = 2;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Remark.DataPropertyName = "Remark";
            this.Remark.HeaderText = "Remark";
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // frmMembershipTransactionReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(920, 566);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.dgvRcpt);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Name = "frmMembershipTransactionReport";
            this.Text = "Membership Transaction Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvRcpt)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtAppearName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DataGridView dgvRcpt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRefNo;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNRIC;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtMembershipNo;
        private System.Windows.Forms.ComboBox cmbPosID;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.ComboBox cmbOutlet;
        private System.Windows.Forms.ComboBox cmbGroupName;
        private System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cbSelect;
        private System.Windows.Forms.DataGridViewButtonColumn btnColView;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn PointOfSaleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutletName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderHdrId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;

    }
}