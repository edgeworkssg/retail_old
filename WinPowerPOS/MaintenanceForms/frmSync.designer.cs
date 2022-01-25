namespace WinPowerPOS
{
    partial class frmSync
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSync));
            this.cbProducts = new System.Windows.Forms.CheckBox();
            this.cbUserAccounts = new System.Windows.Forms.CheckBox();
            this.cbHotkeys = new System.Windows.Forms.CheckBox();
            this.cbPromotions = new System.Windows.Forms.CheckBox();
            this.btnSyncSales = new System.Windows.Forms.Button();
            this.dtpStartDateSales = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndDateSales = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbProject = new System.Windows.Forms.CheckBox();
            this.cbLineInfo = new System.Windows.Forms.CheckBox();
            this.btnGetAll = new System.Windows.Forms.Button();
            this.btnGetRecentlyModified = new System.Windows.Forms.Button();
            this.cbMemberships = new System.Windows.Forms.CheckBox();
            this.cbPointOfSales = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSyncLogs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEndDateLogs = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDateLogs = new System.Windows.Forms.DateTimePicker();
            this.cbMembershipRenewal = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.cbRedemptionLogs = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEndDateSyncAll = new System.Windows.Forms.DateTimePicker();
            this.btnSyncAll = new System.Windows.Forms.Button();
            this.dtpStartDateSyncAll = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSendTo = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btnClearMsg = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlWait = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnSyncAppointment = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.pnlWait.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProducts
            // 
            this.cbProducts.AutoSize = true;
            this.cbProducts.Checked = true;
            this.cbProducts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbProducts.Location = new System.Drawing.Point(10, 25);
            this.cbProducts.Name = "cbProducts";
            this.cbProducts.Size = new System.Drawing.Size(74, 19);
            this.cbProducts.TabIndex = 0;
            this.cbProducts.Text = "Products";
            this.cbProducts.UseVisualStyleBackColor = true;
            // 
            // cbUserAccounts
            // 
            this.cbUserAccounts.AutoSize = true;
            this.cbUserAccounts.Checked = true;
            this.cbUserAccounts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUserAccounts.Location = new System.Drawing.Point(10, 52);
            this.cbUserAccounts.Name = "cbUserAccounts";
            this.cbUserAccounts.Size = new System.Drawing.Size(104, 19);
            this.cbUserAccounts.TabIndex = 1;
            this.cbUserAccounts.Text = "User Accounts";
            this.cbUserAccounts.UseVisualStyleBackColor = true;
            // 
            // cbHotkeys
            // 
            this.cbHotkeys.AutoSize = true;
            this.cbHotkeys.Checked = true;
            this.cbHotkeys.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHotkeys.Location = new System.Drawing.Point(10, 78);
            this.cbHotkeys.Name = "cbHotkeys";
            this.cbHotkeys.Size = new System.Drawing.Size(72, 19);
            this.cbHotkeys.TabIndex = 4;
            this.cbHotkeys.Text = "Hot keys";
            this.cbHotkeys.UseVisualStyleBackColor = true;
            // 
            // cbPromotions
            // 
            this.cbPromotions.AutoSize = true;
            this.cbPromotions.Checked = true;
            this.cbPromotions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPromotions.Location = new System.Drawing.Point(10, 105);
            this.cbPromotions.Name = "cbPromotions";
            this.cbPromotions.Size = new System.Drawing.Size(89, 19);
            this.cbPromotions.TabIndex = 5;
            this.cbPromotions.Text = "Promotions";
            this.cbPromotions.UseVisualStyleBackColor = true;
            // 
            // btnSyncSales
            // 
            this.btnSyncSales.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSyncSales.BackgroundImage")));
            this.btnSyncSales.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnSyncSales.ForeColor = System.Drawing.Color.White;
            this.btnSyncSales.Location = new System.Drawing.Point(145, 98);
            this.btnSyncSales.Name = "btnSyncSales";
            this.btnSyncSales.Size = new System.Drawing.Size(87, 36);
            this.btnSyncSales.TabIndex = 9;
            this.btnSyncSales.Text = "Sync Sales";
            this.btnSyncSales.UseVisualStyleBackColor = true;
            this.btnSyncSales.Click += new System.EventHandler(this.btnSyncSales_Click);
            // 
            // dtpStartDateSales
            // 
            this.dtpStartDateSales.CustomFormat = "dd MMM yyyy hh:mm:ss";
            this.dtpStartDateSales.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDateSales.Location = new System.Drawing.Point(67, 25);
            this.dtpStartDateSales.Name = "dtpStartDateSales";
            this.dtpStartDateSales.Size = new System.Drawing.Size(159, 21);
            this.dtpStartDateSales.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.dtpEndDateSales);
            this.groupBox1.Controls.Add(this.btnSyncSales);
            this.groupBox1.Controls.Add(this.dtpStartDateSales);
            this.groupBox1.Location = new System.Drawing.Point(2, 299);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 140);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Synchronize Sales By Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "End Date";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 15);
            this.label7.TabIndex = 14;
            this.label7.Text = "Start Date";
            // 
            // dtpEndDateSales
            // 
            this.dtpEndDateSales.CustomFormat = "dd MMM yyyy hh:mm:ss";
            this.dtpEndDateSales.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDateSales.Location = new System.Drawing.Point(67, 70);
            this.dtpEndDateSales.Name = "dtpEndDateSales";
            this.dtpEndDateSales.Size = new System.Drawing.Size(159, 21);
            this.dtpEndDateSales.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbProject);
            this.groupBox2.Controls.Add(this.cbLineInfo);
            this.groupBox2.Controls.Add(this.btnGetAll);
            this.groupBox2.Controls.Add(this.btnGetRecentlyModified);
            this.groupBox2.Controls.Add(this.cbMemberships);
            this.groupBox2.Controls.Add(this.cbPointOfSales);
            this.groupBox2.Controls.Add(this.cbProducts);
            this.groupBox2.Controls.Add(this.cbPromotions);
            this.groupBox2.Controls.Add(this.cbUserAccounts);
            this.groupBox2.Controls.Add(this.cbHotkeys);
            this.groupBox2.Location = new System.Drawing.Point(2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(232, 214);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Get Information from Server";
            // 
            // cbProject
            // 
            this.cbProject.AutoSize = true;
            this.cbProject.Checked = true;
            this.cbProject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbProject.Location = new System.Drawing.Point(122, 105);
            this.cbProject.Name = "cbProject";
            this.cbProject.Size = new System.Drawing.Size(70, 19);
            this.cbProject.TabIndex = 13;
            this.cbProject.Text = "Projects";
            this.cbProject.UseVisualStyleBackColor = true;
            // 
            // cbLineInfo
            // 
            this.cbLineInfo.AutoSize = true;
            this.cbLineInfo.Checked = true;
            this.cbLineInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLineInfo.Location = new System.Drawing.Point(122, 78);
            this.cbLineInfo.Name = "cbLineInfo";
            this.cbLineInfo.Size = new System.Drawing.Size(73, 19);
            this.cbLineInfo.TabIndex = 12;
            this.cbLineInfo.Text = "Line Info";
            this.cbLineInfo.UseVisualStyleBackColor = true;
            // 
            // btnGetAll
            // 
            this.btnGetAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetAll.BackgroundImage")));
            this.btnGetAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnGetAll.ForeColor = System.Drawing.Color.White;
            this.btnGetAll.Location = new System.Drawing.Point(122, 157);
            this.btnGetAll.Name = "btnGetAll";
            this.btnGetAll.Size = new System.Drawing.Size(104, 49);
            this.btnGetAll.TabIndex = 11;
            this.btnGetAll.Text = "Get ALL";
            this.btnGetAll.UseVisualStyleBackColor = true;
            this.btnGetAll.Click += new System.EventHandler(this.btnGetAll_Click);
            // 
            // btnGetRecentlyModified
            // 
            this.btnGetRecentlyModified.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnGetRecentlyModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnGetRecentlyModified.ForeColor = System.Drawing.Color.White;
            this.btnGetRecentlyModified.Location = new System.Drawing.Point(12, 157);
            this.btnGetRecentlyModified.Name = "btnGetRecentlyModified";
            this.btnGetRecentlyModified.Size = new System.Drawing.Size(104, 49);
            this.btnGetRecentlyModified.TabIndex = 10;
            this.btnGetRecentlyModified.Text = "Get Recently Modified";
            this.btnGetRecentlyModified.UseVisualStyleBackColor = true;
            this.btnGetRecentlyModified.Visible = false;
            this.btnGetRecentlyModified.Click += new System.EventHandler(this.btnGetRecentlyModified_Click);
            // 
            // cbMemberships
            // 
            this.cbMemberships.AutoSize = true;
            this.cbMemberships.Checked = true;
            this.cbMemberships.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMemberships.Location = new System.Drawing.Point(122, 52);
            this.cbMemberships.Name = "cbMemberships";
            this.cbMemberships.Size = new System.Drawing.Size(102, 19);
            this.cbMemberships.TabIndex = 7;
            this.cbMemberships.Text = "Memberships";
            this.cbMemberships.UseVisualStyleBackColor = true;
            // 
            // cbPointOfSales
            // 
            this.cbPointOfSales.AutoSize = true;
            this.cbPointOfSales.Checked = true;
            this.cbPointOfSales.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPointOfSales.Location = new System.Drawing.Point(122, 25);
            this.cbPointOfSales.Name = "cbPointOfSales";
            this.cbPointOfSales.Size = new System.Drawing.Size(101, 19);
            this.cbPointOfSales.TabIndex = 6;
            this.cbPointOfSales.Text = "Point of Sales";
            this.cbPointOfSales.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSyncLogs);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.dtpEndDateLogs);
            this.groupBox3.Controls.Add(this.dtpStartDateLogs);
            this.groupBox3.Controls.Add(this.cbMembershipRenewal);
            this.groupBox3.Controls.Add(this.checkBox9);
            this.groupBox3.Controls.Add(this.cbRedemptionLogs);
            this.groupBox3.Controls.Add(this.checkBox11);
            this.groupBox3.Controls.Add(this.checkBox12);
            this.groupBox3.Location = new System.Drawing.Point(240, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(240, 290);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Send Logs";
            // 
            // btnSyncLogs
            // 
            this.btnSyncLogs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSyncLogs.BackgroundImage")));
            this.btnSyncLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnSyncLogs.ForeColor = System.Drawing.Color.White;
            this.btnSyncLogs.Location = new System.Drawing.Point(145, 238);
            this.btnSyncLogs.Name = "btnSyncLogs";
            this.btnSyncLogs.Size = new System.Drawing.Size(87, 46);
            this.btnSyncLogs.TabIndex = 20;
            this.btnSyncLogs.Text = "Sync Logs";
            this.btnSyncLogs.UseVisualStyleBackColor = true;
            this.btnSyncLogs.Click += new System.EventHandler(this.btnSyncLogs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "End Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Start Date";
            // 
            // dtpEndDateLogs
            // 
            this.dtpEndDateLogs.CustomFormat = "dd MMM yyyy hh:mm:ss";
            this.dtpEndDateLogs.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDateLogs.Location = new System.Drawing.Point(73, 54);
            this.dtpEndDateLogs.Name = "dtpEndDateLogs";
            this.dtpEndDateLogs.Size = new System.Drawing.Size(159, 21);
            this.dtpEndDateLogs.TabIndex = 17;
            // 
            // dtpStartDateLogs
            // 
            this.dtpStartDateLogs.CustomFormat = "dd MMM yyyy hh:mm:ss";
            this.dtpStartDateLogs.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDateLogs.Location = new System.Drawing.Point(73, 22);
            this.dtpStartDateLogs.Name = "dtpStartDateLogs";
            this.dtpStartDateLogs.Size = new System.Drawing.Size(159, 21);
            this.dtpStartDateLogs.TabIndex = 16;
            // 
            // cbMembershipRenewal
            // 
            this.cbMembershipRenewal.AutoSize = true;
            this.cbMembershipRenewal.Checked = true;
            this.cbMembershipRenewal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMembershipRenewal.Location = new System.Drawing.Point(12, 200);
            this.cbMembershipRenewal.Name = "cbMembershipRenewal";
            this.cbMembershipRenewal.Size = new System.Drawing.Size(180, 19);
            this.cbMembershipRenewal.TabIndex = 12;
            this.cbMembershipRenewal.Text = "New Signup && Renewal Log";
            this.cbMembershipRenewal.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Checked = true;
            this.checkBox9.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox9.Location = new System.Drawing.Point(10, 93);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(95, 19);
            this.checkBox9.TabIndex = 8;
            this.checkBox9.Text = "Login Activity";
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // cbRedemptionLogs
            // 
            this.cbRedemptionLogs.AutoSize = true;
            this.cbRedemptionLogs.Checked = true;
            this.cbRedemptionLogs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRedemptionLogs.Location = new System.Drawing.Point(12, 173);
            this.cbRedemptionLogs.Name = "cbRedemptionLogs";
            this.cbRedemptionLogs.Size = new System.Drawing.Size(118, 19);
            this.cbRedemptionLogs.TabIndex = 11;
            this.cbRedemptionLogs.Text = "Redemption Log";
            this.cbRedemptionLogs.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Checked = true;
            this.checkBox11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox11.Location = new System.Drawing.Point(12, 120);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(120, 19);
            this.checkBox11.TabIndex = 9;
            this.checkBox11.Text = "Cash Recordings";
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Checked = true;
            this.checkBox12.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox12.Location = new System.Drawing.Point(12, 147);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(135, 19);
            this.checkBox12.TabIndex = 10;
            this.checkBox12.Text = "Special Activity Logs";
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.dtpEndDateSyncAll);
            this.groupBox4.Controls.Add(this.btnSyncAll);
            this.groupBox4.Controls.Add(this.dtpStartDateSyncAll);
            this.groupBox4.Location = new System.Drawing.Point(240, 299);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(240, 140);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Synchronize All";
            this.groupBox4.Visible = false;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(6, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 36);
            this.button1.TabIndex = 16;
            this.button1.Text = "Sync Inventory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "End Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Start Date";
            // 
            // dtpEndDateSyncAll
            // 
            this.dtpEndDateSyncAll.CustomFormat = "dd MMM yyyy hh:mm:ss";
            this.dtpEndDateSyncAll.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDateSyncAll.Location = new System.Drawing.Point(70, 70);
            this.dtpEndDateSyncAll.Name = "dtpEndDateSyncAll";
            this.dtpEndDateSyncAll.Size = new System.Drawing.Size(159, 21);
            this.dtpEndDateSyncAll.TabIndex = 13;
            // 
            // btnSyncAll
            // 
            this.btnSyncAll.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSyncAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnSyncAll.ForeColor = System.Drawing.Color.White;
            this.btnSyncAll.Location = new System.Drawing.Point(142, 98);
            this.btnSyncAll.Name = "btnSyncAll";
            this.btnSyncAll.Size = new System.Drawing.Size(87, 36);
            this.btnSyncAll.TabIndex = 9;
            this.btnSyncAll.Text = "Sync All";
            this.btnSyncAll.UseVisualStyleBackColor = true;
            this.btnSyncAll.Visible = false;
            this.btnSyncAll.Click += new System.EventHandler(this.btnSyncAll_Click);
            // 
            // dtpStartDateSyncAll
            // 
            this.dtpStartDateSyncAll.CustomFormat = "dd MMM yyyy hh:mm:ss";
            this.dtpStartDateSyncAll.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDateSyncAll.Location = new System.Drawing.Point(70, 25);
            this.dtpStartDateSyncAll.Name = "dtpStartDateSyncAll";
            this.dtpStartDateSyncAll.Size = new System.Drawing.Size(159, 21);
            this.dtpStartDateSyncAll.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 442);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "Send to:";
            // 
            // lblSendTo
            // 
            this.lblSendTo.AutoSize = true;
            this.lblSendTo.Location = new System.Drawing.Point(58, 442);
            this.lblSendTo.Name = "lblSendTo";
            this.lblSendTo.Size = new System.Drawing.Size(11, 15);
            this.lblSendTo.TabIndex = 18;
            this.lblSendTo.Text = "-";
            // 
            // txtMsg
            // 
            this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsg.Location = new System.Drawing.Point(486, 3);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(277, 385);
            this.txtMsg.TabIndex = 20;
            this.txtMsg.Visible = false;
            // 
            // btnClearMsg
            // 
            this.btnClearMsg.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClearMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnClearMsg.ForeColor = System.Drawing.Color.White;
            this.btnClearMsg.Location = new System.Drawing.Point(538, 394);
            this.btnClearMsg.Name = "btnClearMsg";
            this.btnClearMsg.Size = new System.Drawing.Size(124, 43);
            this.btnClearMsg.TabIndex = 21;
            this.btnClearMsg.Text = "Clear Message";
            this.btnClearMsg.UseVisualStyleBackColor = true;
            this.btnClearMsg.Click += new System.EventHandler(this.btnClearMsg_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(668, 394);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 43);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlWait
            // 
            this.pnlWait.BackgroundImage = global::WinPowerPOS.Properties.Resources.longyellowbackground2;
            this.pnlWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlWait.Controls.Add(this.label6);
            this.pnlWait.Location = new System.Drawing.Point(150, 100);
            this.pnlWait.Name = "pnlWait";
            this.pnlWait.Size = new System.Drawing.Size(409, 167);
            this.pnlWait.TabIndex = 23;
            this.pnlWait.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(102, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(205, 31);
            this.label6.TabIndex = 0;
            this.label6.Text = "PLEASE WAIT";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnSyncAppointment);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.dateTimePicker2);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.dateTimePicker1);
            this.groupBox5.Location = new System.Drawing.Point(2, 223);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(232, 70);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Appointment Sync";
            // 
            // btnSyncAppointment
            // 
            this.btnSyncAppointment.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSyncAppointment.BackgroundImage")));
            this.btnSyncAppointment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnSyncAppointment.ForeColor = System.Drawing.Color.White;
            this.btnSyncAppointment.Location = new System.Drawing.Point(10, 21);
            this.btnSyncAppointment.Name = "btnSyncAppointment";
            this.btnSyncAppointment.Size = new System.Drawing.Size(211, 36);
            this.btnSyncAppointment.TabIndex = 10;
            this.btnSyncAppointment.Text = "Sync Appointment";
            this.btnSyncAppointment.UseVisualStyleBackColor = true;
            this.btnSyncAppointment.Click += new System.EventHandler(this.btnSyncAppointment_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 15);
            this.label9.TabIndex = 7;
            this.label9.Text = "End Date";
            this.label9.Visible = false;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(86, 42);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(135, 21);
            this.dateTimePicker2.TabIndex = 6;
            this.dateTimePicker2.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 15);
            this.label10.TabIndex = 5;
            this.label10.Text = "Start Date";
            this.label10.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(86, 18);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(135, 21);
            this.dateTimePicker1.TabIndex = 4;
            this.dateTimePicker1.Visible = false;
            // 
            // frmSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 466);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClearMsg);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.lblSendTo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlWait);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F);
            this.Name = "frmSync";
            this.Text = "Synchronization";
            this.Load += new System.EventHandler(this.frmSync_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSync_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.pnlWait.ResumeLayout(false);
            this.pnlWait.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbProducts;
        private System.Windows.Forms.CheckBox cbUserAccounts;
        private System.Windows.Forms.CheckBox cbHotkeys;
        private System.Windows.Forms.CheckBox cbPromotions;
        private System.Windows.Forms.Button btnSyncSales;
        private System.Windows.Forms.DateTimePicker dtpStartDateSales;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpEndDateSales;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbPointOfSales;
        private System.Windows.Forms.CheckBox cbMemberships;
        private System.Windows.Forms.Button btnGetAll;
        private System.Windows.Forms.Button btnGetRecentlyModified;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbMembershipRenewal;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox cbRedemptionLogs;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.Button btnSyncLogs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpEndDateLogs;
        private System.Windows.Forms.DateTimePicker dtpStartDateLogs;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpEndDateSyncAll;
        private System.Windows.Forms.Button btnSyncAll;
        private System.Windows.Forms.DateTimePicker dtpStartDateSyncAll;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblSendTo;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btnClearMsg;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlWait;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbLineInfo;
        private System.Windows.Forms.CheckBox cbProject;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnSyncAppointment;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}