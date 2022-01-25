namespace MassMailer
{
    partial class frmMassMailer
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
            this.dtpStartExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartBirthDay = new System.Windows.Forms.DateTimePicker();
            this.dtpEndBirthDay = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtMembershipNoFrom = new System.Windows.Forms.TextBox();
            this.txtMembershipNoTo = new System.Windows.Forms.TextBox();
            this.cmbGroupName = new System.Windows.Forms.ComboBox();
            this.dgvMember = new System.Windows.Forms.DataGridView();
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.pnlSendOrder = new System.Windows.Forms.Panel();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnlSendOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpStartExpiryDate
            // 
            this.dtpStartExpiryDate.Checked = false;
            this.dtpStartExpiryDate.Location = new System.Drawing.Point(124, 14);
            this.dtpStartExpiryDate.Name = "dtpStartExpiryDate";
            this.dtpStartExpiryDate.ShowCheckBox = true;
            this.dtpStartExpiryDate.Size = new System.Drawing.Size(200, 20);
            this.dtpStartExpiryDate.TabIndex = 0;
            // 
            // dtpEndExpiryDate
            // 
            this.dtpEndExpiryDate.Checked = false;
            this.dtpEndExpiryDate.Location = new System.Drawing.Point(461, 14);
            this.dtpEndExpiryDate.Name = "dtpEndExpiryDate";
            this.dtpEndExpiryDate.ShowCheckBox = true;
            this.dtpEndExpiryDate.Size = new System.Drawing.Size(200, 20);
            this.dtpEndExpiryDate.TabIndex = 1;
            // 
            // dtpStartBirthDay
            // 
            this.dtpStartBirthDay.Checked = false;
            this.dtpStartBirthDay.Location = new System.Drawing.Point(124, 41);
            this.dtpStartBirthDay.Name = "dtpStartBirthDay";
            this.dtpStartBirthDay.ShowCheckBox = true;
            this.dtpStartBirthDay.Size = new System.Drawing.Size(200, 20);
            this.dtpStartBirthDay.TabIndex = 2;
            // 
            // dtpEndBirthDay
            // 
            this.dtpEndBirthDay.Checked = false;
            this.dtpEndBirthDay.Location = new System.Drawing.Point(461, 41);
            this.dtpEndBirthDay.Name = "dtpEndBirthDay";
            this.dtpEndBirthDay.ShowCheckBox = true;
            this.dtpEndBirthDay.Size = new System.Drawing.Size(200, 20);
            this.dtpEndBirthDay.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Expiry Date Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Birthday Start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Membership No From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(683, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Group Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(348, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Membership No To";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(348, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "Expiry Date End";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(348, 47);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Birthday End";
            // 
            // txtMembershipNoFrom
            // 
            this.txtMembershipNoFrom.Location = new System.Drawing.Point(124, 76);
            this.txtMembershipNoFrom.Name = "txtMembershipNoFrom";
            this.txtMembershipNoFrom.Size = new System.Drawing.Size(200, 20);
            this.txtMembershipNoFrom.TabIndex = 17;
            // 
            // txtMembershipNoTo
            // 
            this.txtMembershipNoTo.Location = new System.Drawing.Point(461, 72);
            this.txtMembershipNoTo.Name = "txtMembershipNoTo";
            this.txtMembershipNoTo.Size = new System.Drawing.Size(200, 20);
            this.txtMembershipNoTo.TabIndex = 22;
            // 
            // cmbGroupName
            // 
            this.cmbGroupName.FormattingEnabled = true;
            this.cmbGroupName.Location = new System.Drawing.Point(757, 12);
            this.cmbGroupName.Name = "cmbGroupName";
            this.cmbGroupName.Size = new System.Drawing.Size(200, 21);
            this.cmbGroupName.TabIndex = 24;
            // 
            // dgvMember
            // 
            this.dgvMember.AllowUserToAddRows = false;
            this.dgvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MembershipNo,
            this.GroupName,
            this.NameToAppear,
            this.FirstName,
            this.LastName,
            this.EMail,
            this.NRIC,
            this.ExpiryDate});
            this.dgvMember.Location = new System.Drawing.Point(3, 153);
            this.dgvMember.Name = "dgvMember";
            this.dgvMember.RowHeadersWidth = 20;
            this.dgvMember.Size = new System.Drawing.Size(977, 287);
            this.dgvMember.TabIndex = 26;
            this.dgvMember.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvMember_RowPostPaint);
            this.dgvMember.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMember_CellContentClick);
            // 
            // MembershipNo
            // 
            this.MembershipNo.DataPropertyName = "MembershipNo";
            this.MembershipNo.HeaderText = "Membership No";
            this.MembershipNo.Name = "MembershipNo";
            this.MembershipNo.Width = 140;
            // 
            // GroupName
            // 
            this.GroupName.DataPropertyName = "GroupName";
            this.GroupName.HeaderText = "Group";
            this.GroupName.Name = "GroupName";
            this.GroupName.Width = 140;
            // 
            // NameToAppear
            // 
            this.NameToAppear.DataPropertyName = "NameToAppear";
            this.NameToAppear.HeaderText = "Name To Appear";
            this.NameToAppear.Name = "NameToAppear";
            this.NameToAppear.Width = 140;
            // 
            // FirstName
            // 
            this.FirstName.DataPropertyName = "FirstName";
            this.FirstName.HeaderText = "First Name";
            this.FirstName.Name = "FirstName";
            this.FirstName.Width = 140;
            // 
            // LastName
            // 
            this.LastName.DataPropertyName = "lastName";
            this.LastName.HeaderText = "Last Name";
            this.LastName.Name = "LastName";
            this.LastName.Width = 140;
            // 
            // EMail
            // 
            this.EMail.DataPropertyName = "email";
            this.EMail.HeaderText = "E-Mail";
            this.EMail.Name = "EMail";
            this.EMail.Width = 140;
            // 
            // NRIC
            // 
            this.NRIC.DataPropertyName = "NRIC";
            this.NRIC.HeaderText = "NRIC";
            this.NRIC.Name = "NRIC";
            this.NRIC.Width = 140;
            // 
            // ExpiryDate
            // 
            this.ExpiryDate.DataPropertyName = "ExpiryDate";
            this.ExpiryDate.HeaderText = "ExpiryDate";
            this.ExpiryDate.Name = "ExpiryDate";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.Green;
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(753, 105);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(117, 32);
            this.btnSearch.TabIndex = 28;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.CausesValidation = false;
            this.btnClear.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.DarkRed;
            this.btnClear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClear.Location = new System.Drawing.Point(876, 105);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(81, 32);
            this.btnClear.TabIndex = 29;
            this.btnClear.Text = "CLOSE";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.Green;
            this.btnSend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSend.Location = new System.Drawing.Point(194, 196);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(117, 41);
            this.btnSend.TabIndex = 30;
            this.btnSend.Text = "SEND >";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(246, 41);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 31;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(77, 43);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(163, 20);
            this.txtFileName.TabIndex = 32;
            // 
            // pbPreview
            // 
            this.pbPreview.Location = new System.Drawing.Point(42, 93);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(141, 144);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPreview.TabIndex = 33;
            this.pbPreview.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtSubject);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.btnSend);
            this.groupBox1.Controls.Add(this.pbPreview);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Location = new System.Drawing.Point(3, 464);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 249);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Picture Message";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(6, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 13);
            this.label16.TabIndex = 37;
            this.label16.Text = "Picture File";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(6, 18);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(43, 13);
            this.label15.TabIndex = 36;
            this.label15.Text = "Subject";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(77, 15);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(244, 20);
            this.txtSubject.TabIndex = 35;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(6, 77);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(216, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "Preview (This is not the original size)";
            // 
            // pnlSendOrder
            // 
            this.pnlSendOrder.BackColor = System.Drawing.Color.White;
            this.pnlSendOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSendOrder.Controls.Add(this.txtStatus);
            this.pnlSendOrder.Controls.Add(this.pictureBox2);
            this.pnlSendOrder.Controls.Add(this.label18);
            this.pnlSendOrder.Location = new System.Drawing.Point(80, 115);
            this.pnlSendOrder.Name = "pnlSendOrder";
            this.pnlSendOrder.Size = new System.Drawing.Size(775, 560);
            this.pnlSendOrder.TabIndex = 70;
            this.pnlSendOrder.Visible = false;
            // 
            // txtStatus
            // 
            this.txtStatus.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.txtStatus.Location = new System.Drawing.Point(36, 122);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(702, 403);
            this.txtStatus.TabIndex = 4;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MassMailer.Properties.Resources.progressbar_long_green;
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(235, 15);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(307, 63);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(194, 81);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(386, 26);
            this.label18.TabIndex = 1;
            this.label18.Text = "SENDING EMAILS. PLEASE WAIT";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.DarkBlue;
            this.label17.Location = new System.Drawing.Point(776, 678);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(117, 35);
            this.label17.TabIndex = 71;
            this.label17.Text = "Power";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label19.Location = new System.Drawing.Point(889, 678);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(82, 35);
            this.label19.TabIndex = 72;
            this.label19.Text = "POS";
            // 
            // frmMassMailer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MassMailer.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(984, 725);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.dgvMember);
            this.Controls.Add(this.cmbGroupName);
            this.Controls.Add(this.txtMembershipNoTo);
            this.Controls.Add(this.txtMembershipNoFrom);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEndBirthDay);
            this.Controls.Add(this.dtpStartBirthDay);
            this.Controls.Add(this.dtpEndExpiryDate);
            this.Controls.Add(this.dtpStartExpiryDate);
            this.Controls.Add(this.pnlSendOrder);
            this.Name = "frmMassMailer";
            this.Text = "Search Member";
            this.Load += new System.EventHandler(this.frmMassMailer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlSendOrder.ResumeLayout(false);
            this.pnlSendOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
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
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtMembershipNoFrom;
        private System.Windows.Forms.TextBox txtMembershipNoTo;
        private System.Windows.Forms.ComboBox cmbGroupName;
        private System.Windows.Forms.DataGridView dgvMember;
        internal System.Windows.Forms.Button btnSearch;
        internal System.Windows.Forms.Button btnClear;
        internal System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMail;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryDate;
        private System.Windows.Forms.Panel pnlSendOrder;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtStatus;
    }
}

