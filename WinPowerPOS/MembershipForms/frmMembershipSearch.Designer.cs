namespace WinPowerPOS.MembershipForm
{
    partial class frmMembershipSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtpStartExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartBirthDay = new System.Windows.Forms.DateTimePicker();
            this.dtpEndBirthDay = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtMembershipNo = new System.Windows.Forms.TextBox();
            this.txtNameToAppear = new System.Windows.Forms.TextBox();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtNRIC = new System.Windows.Forms.TextBox();
            this.txtHome = new System.Windows.Forms.TextBox();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.cmbGroupName = new System.Windows.Forms.ComboBox();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.dgvMember = new System.Windows.Forms.DataGridView();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cmbBirthdayMonth = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnNewMember = new System.Windows.Forms.Button();
            this.txtMembershipNoTo = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BirthDae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostalCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpStartExpiryDate
            // 
            this.dtpStartExpiryDate.Checked = false;
            this.dtpStartExpiryDate.Location = new System.Drawing.Point(89, 2);
            this.dtpStartExpiryDate.Name = "dtpStartExpiryDate";
            this.dtpStartExpiryDate.ShowCheckBox = true;
            this.dtpStartExpiryDate.Size = new System.Drawing.Size(200, 20);
            this.dtpStartExpiryDate.TabIndex = 0;
            // 
            // dtpEndExpiryDate
            // 
            this.dtpEndExpiryDate.Checked = false;
            this.dtpEndExpiryDate.Location = new System.Drawing.Point(89, 31);
            this.dtpEndExpiryDate.Name = "dtpEndExpiryDate";
            this.dtpEndExpiryDate.ShowCheckBox = true;
            this.dtpEndExpiryDate.Size = new System.Drawing.Size(200, 20);
            this.dtpEndExpiryDate.TabIndex = 1;
            // 
            // dtpStartBirthDay
            // 
            this.dtpStartBirthDay.Checked = false;
            this.dtpStartBirthDay.Location = new System.Drawing.Point(89, 65);
            this.dtpStartBirthDay.Name = "dtpStartBirthDay";
            this.dtpStartBirthDay.ShowCheckBox = true;
            this.dtpStartBirthDay.Size = new System.Drawing.Size(200, 20);
            this.dtpStartBirthDay.TabIndex = 2;
            // 
            // dtpEndBirthDay
            // 
            this.dtpEndBirthDay.Checked = false;
            this.dtpEndBirthDay.Location = new System.Drawing.Point(89, 96);
            this.dtpEndBirthDay.Name = "dtpEndBirthDay";
            this.dtpEndBirthDay.ShowCheckBox = true;
            this.dtpEndBirthDay.Size = new System.Drawing.Size(200, 20);
            this.dtpEndBirthDay.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Signup Date Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(0, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Birthday Start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(304, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Membership No";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(532, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Card Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(304, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(304, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "NRIC";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(304, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Mobile";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(304, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Home";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(532, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Address";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(532, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "E-Mail";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(532, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Gender";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(0, 37);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "Signup Date End";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(0, 99);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Birthday End";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // txtMembershipNo
            // 
            this.txtMembershipNo.Location = new System.Drawing.Point(396, 3);
            this.txtMembershipNo.Name = "txtMembershipNo";
            this.txtMembershipNo.Size = new System.Drawing.Size(121, 20);
            this.txtMembershipNo.TabIndex = 17;
            // 
            // txtNameToAppear
            // 
            this.txtNameToAppear.Location = new System.Drawing.Point(396, 34);
            this.txtNameToAppear.Name = "txtNameToAppear";
            this.txtNameToAppear.Size = new System.Drawing.Size(121, 20);
            this.txtNameToAppear.TabIndex = 18;
            // 
            // txtMobile
            // 
            this.txtMobile.Location = new System.Drawing.Point(396, 96);
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(121, 20);
            this.txtMobile.TabIndex = 19;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(611, 36);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(132, 20);
            this.txtAddress.TabIndex = 20;
            // 
            // txtNRIC
            // 
            this.txtNRIC.Location = new System.Drawing.Point(396, 65);
            this.txtNRIC.Name = "txtNRIC";
            this.txtNRIC.Size = new System.Drawing.Size(121, 20);
            this.txtNRIC.TabIndex = 21;
            // 
            // txtHome
            // 
            this.txtHome.Location = new System.Drawing.Point(396, 129);
            this.txtHome.Name = "txtHome";
            this.txtHome.Size = new System.Drawing.Size(121, 20);
            this.txtHome.TabIndex = 22;
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.Location = new System.Drawing.Point(611, 66);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(132, 20);
            this.txtPostalCode.TabIndex = 23;
            // 
            // cmbGroupName
            // 
            this.cmbGroupName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupName.FormattingEnabled = true;
            this.cmbGroupName.Location = new System.Drawing.Point(611, 127);
            this.cmbGroupName.Name = "cmbGroupName";
            this.cmbGroupName.Size = new System.Drawing.Size(132, 21);
            this.cmbGroupName.TabIndex = 24;
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            "ALL",
            "M",
            "F"});
            this.cmbGender.Location = new System.Drawing.Point(611, 99);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(132, 21);
            this.cmbGender.TabIndex = 25;
            // 
            // dgvMember
            // 
            this.dgvMember.AllowUserToAddRows = false;
            this.dgvMember.AllowUserToDeleteRows = false;
            this.dgvMember.AllowUserToResizeColumns = false;
            this.dgvMember.AllowUserToResizeRows = false;
            this.dgvMember.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.MembershipNo,
            this.GroupName,
            this.NameToAppear,
            this.NRIC,
            this.ExpiryDate,
            this.BirthDae,
            this.FirstName,
            this.LastName,
            this.Mobile,
            this.EMail,
            this.Address,
            this.PostalCode,
            this.Country,
            this.City});
            this.dgvMember.Location = new System.Drawing.Point(3, 200);
            this.dgvMember.Name = "dgvMember";
            this.dgvMember.ReadOnly = true;
            this.dgvMember.RowHeadersVisible = false;
            this.dgvMember.RowHeadersWidth = 20;
            this.dgvMember.Size = new System.Drawing.Size(781, 338);
            this.dgvMember.TabIndex = 26;
            this.dgvMember.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvMember_RowPostPaint);
            this.dgvMember.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMember_CellClick);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(551, 158);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(93, 36);
            this.btnSearch.TabIndex = 28;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClear.CausesValidation = false;
            this.btnClear.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClear.Location = new System.Drawing.Point(650, 158);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(93, 36);
            this.btnClear.TabIndex = 29;
            this.btnClear.Text = "CLOSE";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cmbBirthdayMonth
            // 
            this.cmbBirthdayMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBirthdayMonth.FormattingEnabled = true;
            this.cmbBirthdayMonth.Items.AddRange(new object[] {
            "-Select-",
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cmbBirthdayMonth.Location = new System.Drawing.Point(89, 128);
            this.cmbBirthdayMonth.Name = "cmbBirthdayMonth";
            this.cmbBirthdayMonth.Size = new System.Drawing.Size(200, 21);
            this.cmbBirthdayMonth.TabIndex = 31;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Location = new System.Drawing.Point(0, 132);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 13);
            this.label14.TabIndex = 30;
            this.label14.Text = "Birthday Month";
            // 
            // btnNewMember
            // 
            this.btnNewMember.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnNewMember.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnNewMember.ForeColor = System.Drawing.Color.White;
            this.btnNewMember.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNewMember.Location = new System.Drawing.Point(3, 158);
            this.btnNewMember.Name = "btnNewMember";
            this.btnNewMember.Size = new System.Drawing.Size(118, 36);
            this.btnNewMember.TabIndex = 32;
            this.btnNewMember.Text = "NEW MEMBER";
            this.btnNewMember.UseVisualStyleBackColor = true;
            this.btnNewMember.Visible = false;
            this.btnNewMember.Click += new System.EventHandler(this.btnNewMember_Click);
            // 
            // txtMembershipNoTo
            // 
            this.txtMembershipNoTo.Location = new System.Drawing.Point(611, 5);
            this.txtMembershipNoTo.Name = "txtMembershipNoTo";
            this.txtMembershipNoTo.Size = new System.Drawing.Size(132, 20);
            this.txtMembershipNoTo.TabIndex = 34;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(532, 8);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(20, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "To";
            // 
            // btnExport
            // 
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExport.Location = new System.Drawing.Point(424, 158);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(93, 35);
            this.btnExport.TabIndex = 35;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            this.fsdExportToExcel.Title = "Save File";
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Text = "View";
            this.Column1.UseColumnTextForButtonValue = true;
            this.Column1.Width = 60;
            // 
            // MembershipNo
            // 
            this.MembershipNo.DataPropertyName = "MembershipNo";
            this.MembershipNo.HeaderText = "No";
            this.MembershipNo.Name = "MembershipNo";
            this.MembershipNo.ReadOnly = true;
            // 
            // GroupName
            // 
            this.GroupName.DataPropertyName = "GroupName";
            this.GroupName.HeaderText = "Group";
            this.GroupName.Name = "GroupName";
            this.GroupName.ReadOnly = true;
            // 
            // NameToAppear
            // 
            this.NameToAppear.DataPropertyName = "NameToAppear";
            this.NameToAppear.HeaderText = "Name To Appear";
            this.NameToAppear.Name = "NameToAppear";
            this.NameToAppear.ReadOnly = true;
            this.NameToAppear.Width = 140;
            // 
            // NRIC
            // 
            this.NRIC.DataPropertyName = "NRIC";
            this.NRIC.HeaderText = "NRIC";
            this.NRIC.Name = "NRIC";
            this.NRIC.ReadOnly = true;
            // 
            // ExpiryDate
            // 
            this.ExpiryDate.DataPropertyName = "ExpiryDate";
            dataGridViewCellStyle3.Format = "dd MMM yyyy";
            dataGridViewCellStyle3.NullValue = null;
            this.ExpiryDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.ExpiryDate.HeaderText = "ExpiryDate";
            this.ExpiryDate.Name = "ExpiryDate";
            this.ExpiryDate.ReadOnly = true;
            // 
            // BirthDae
            // 
            this.BirthDae.DataPropertyName = "DateOfBirth";
            dataGridViewCellStyle4.Format = "dd MMM yyyy";
            dataGridViewCellStyle4.NullValue = null;
            this.BirthDae.DefaultCellStyle = dataGridViewCellStyle4;
            this.BirthDae.HeaderText = "Birthday";
            this.BirthDae.Name = "BirthDae";
            this.BirthDae.ReadOnly = true;
            // 
            // FirstName
            // 
            this.FirstName.DataPropertyName = "FirstName";
            this.FirstName.HeaderText = "First Name";
            this.FirstName.Name = "FirstName";
            this.FirstName.ReadOnly = true;
            this.FirstName.Width = 140;
            // 
            // LastName
            // 
            this.LastName.DataPropertyName = "lastName";
            this.LastName.HeaderText = "Last Name";
            this.LastName.Name = "LastName";
            this.LastName.ReadOnly = true;
            this.LastName.Width = 140;
            // 
            // Mobile
            // 
            this.Mobile.DataPropertyName = "Mobile";
            this.Mobile.HeaderText = "Mobile";
            this.Mobile.Name = "Mobile";
            this.Mobile.ReadOnly = true;
            // 
            // EMail
            // 
            this.EMail.DataPropertyName = "email";
            this.EMail.HeaderText = "E-Mail";
            this.EMail.Name = "EMail";
            this.EMail.ReadOnly = true;
            this.EMail.Width = 140;
            // 
            // Address
            // 
            this.Address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Address.DataPropertyName = "Address";
            this.Address.HeaderText = "Address";
            this.Address.MinimumWidth = 100;
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // PostalCode
            // 
            this.PostalCode.DataPropertyName = "ZipCode";
            this.PostalCode.HeaderText = "Zip Code";
            this.PostalCode.Name = "PostalCode";
            this.PostalCode.ReadOnly = true;
            // 
            // Country
            // 
            this.Country.DataPropertyName = "Country";
            this.Country.HeaderText = "Country";
            this.Country.Name = "Country";
            this.Country.ReadOnly = true;
            // 
            // City
            // 
            this.City.DataPropertyName = "City";
            this.City.HeaderText = "City";
            this.City.Name = "City";
            this.City.ReadOnly = true;
            // 
            // frmMembershipSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(796, 550);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtMembershipNoTo);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnNewMember);
            this.Controls.Add(this.cmbBirthdayMonth);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.dgvMember);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.cmbGroupName);
            this.Controls.Add(this.txtPostalCode);
            this.Controls.Add(this.txtHome);
            this.Controls.Add(this.txtNRIC);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtMobile);
            this.Controls.Add(this.txtNameToAppear);
            this.Controls.Add(this.txtMembershipNo);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEndBirthDay);
            this.Controls.Add(this.dtpStartBirthDay);
            this.Controls.Add(this.dtpEndExpiryDate);
            this.Controls.Add(this.dtpStartExpiryDate);
            this.Location = new System.Drawing.Point(1, 1);
            this.Name = "frmMembershipSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMassMailer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStartExpiryDate;
        private System.Windows.Forms.DateTimePicker dtpEndExpiryDate;
        private System.Windows.Forms.DateTimePicker dtpStartBirthDay;
        private System.Windows.Forms.DateTimePicker dtpEndBirthDay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtMembershipNo;
        private System.Windows.Forms.TextBox txtNameToAppear;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtNRIC;
        private System.Windows.Forms.TextBox txtHome;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.ComboBox cmbGroupName;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.DataGridView dgvMember;
        internal System.Windows.Forms.Button btnSearch;
        internal System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox cmbBirthdayMonth;
        private System.Windows.Forms.Label label14;
        internal System.Windows.Forms.Button btnNewMember;
        private System.Windows.Forms.TextBox txtMembershipNoTo;
        private System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BirthDae;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mobile;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn PostalCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Country;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;

    }
}

