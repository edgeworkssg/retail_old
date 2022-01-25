namespace WinPowerPOS.AppointmentForms
{
    partial class frmCalendarEdit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Tabel = new System.Windows.Forms.DataGridView();
            this.dgvbEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvbDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvcID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcMembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcMembershipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcAppointmentTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcRoomID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcRoomName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.iEventID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.iAppointment = new System.Windows.Forms.DateTimePicker();
            this.iDuration = new System.Windows.Forms.ComboBox();
            this.iRoom = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.iMembership = new System.Windows.Forms.TextBox();
            this.lblMembershipInfo = new System.Windows.Forms.Label();
            this.cmdMembershipSearch = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.myCalendar = new System.Windows.Forms.MonthCalendar();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.iTime = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabel
            // 
            this.Tabel.AllowUserToAddRows = false;
            this.Tabel.AllowUserToDeleteRows = false;
            this.Tabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabel.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.Tabel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.Tabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Tabel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tabel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvbEdit,
            this.dgvbDelete,
            this.dgvcID,
            this.dgvcMembershipNo,
            this.dgvcMembershipName,
            this.dgvcAppointmentTime,
            this.dgvcDuration,
            this.dgvcRoomID,
            this.dgvcRoomName,
            this.dgvcType});
            this.Tabel.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.Tabel.Location = new System.Drawing.Point(12, 14);
            this.Tabel.MultiSelect = false;
            this.Tabel.Name = "Tabel";
            this.Tabel.ReadOnly = true;
            this.Tabel.RowHeadersVisible = false;
            this.Tabel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Tabel.Size = new System.Drawing.Size(882, 316);
            this.Tabel.TabIndex = 11;
            this.Tabel.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellContentClick);
            // 
            // dgvbEdit
            // 
            this.dgvbEdit.HeaderText = "";
            this.dgvbEdit.Name = "dgvbEdit";
            this.dgvbEdit.ReadOnly = true;
            this.dgvbEdit.Text = "Edit";
            this.dgvbEdit.UseColumnTextForButtonValue = true;
            this.dgvbEdit.Width = 50;
            // 
            // dgvbDelete
            // 
            this.dgvbDelete.HeaderText = "";
            this.dgvbDelete.Name = "dgvbDelete";
            this.dgvbDelete.ReadOnly = true;
            this.dgvbDelete.Text = "Del";
            this.dgvbDelete.UseColumnTextForButtonValue = true;
            this.dgvbDelete.Width = 50;
            // 
            // dgvcID
            // 
            this.dgvcID.DataPropertyName = "ID";
            this.dgvcID.HeaderText = "Appointment ID";
            this.dgvcID.Name = "dgvcID";
            this.dgvcID.ReadOnly = true;
            this.dgvcID.Visible = false;
            this.dgvcID.Width = 125;
            // 
            // dgvcMembershipNo
            // 
            this.dgvcMembershipNo.DataPropertyName = "MembershipNo";
            this.dgvcMembershipNo.HeaderText = "Membership No";
            this.dgvcMembershipNo.Name = "dgvcMembershipNo";
            this.dgvcMembershipNo.ReadOnly = true;
            this.dgvcMembershipNo.Visible = false;
            this.dgvcMembershipNo.Width = 150;
            // 
            // dgvcMembershipName
            // 
            this.dgvcMembershipName.DataPropertyName = "MembershipName";
            this.dgvcMembershipName.HeaderText = "Membership Name";
            this.dgvcMembershipName.Name = "dgvcMembershipName";
            this.dgvcMembershipName.ReadOnly = true;
            this.dgvcMembershipName.Width = 200;
            // 
            // dgvcAppointmentTime
            // 
            this.dgvcAppointmentTime.DataPropertyName = "AppointmentTime";
            dataGridViewCellStyle19.Format = "f";
            dataGridViewCellStyle19.NullValue = null;
            this.dgvcAppointmentTime.DefaultCellStyle = dataGridViewCellStyle19;
            this.dgvcAppointmentTime.HeaderText = "Time";
            this.dgvcAppointmentTime.Name = "dgvcAppointmentTime";
            this.dgvcAppointmentTime.ReadOnly = true;
            // 
            // dgvcDuration
            // 
            this.dgvcDuration.DataPropertyName = "Duration";
            dataGridViewCellStyle20.Format = "N0";
            this.dgvcDuration.DefaultCellStyle = dataGridViewCellStyle20;
            this.dgvcDuration.HeaderText = "Duration";
            this.dgvcDuration.Name = "dgvcDuration";
            this.dgvcDuration.ReadOnly = true;
            // 
            // dgvcRoomID
            // 
            this.dgvcRoomID.DataPropertyName = "RoomID";
            dataGridViewCellStyle21.Format = "N0";
            this.dgvcRoomID.DefaultCellStyle = dataGridViewCellStyle21;
            this.dgvcRoomID.HeaderText = "Room";
            this.dgvcRoomID.Name = "dgvcRoomID";
            this.dgvcRoomID.ReadOnly = true;
            this.dgvcRoomID.Visible = false;
            // 
            // dgvcRoomName
            // 
            this.dgvcRoomName.DataPropertyName = "RoomName";
            this.dgvcRoomName.HeaderText = "Room Name";
            this.dgvcRoomName.Name = "dgvcRoomName";
            this.dgvcRoomName.ReadOnly = true;
            this.dgvcRoomName.Width = 150;
            // 
            // dgvcType
            // 
            this.dgvcType.HeaderText = "Type";
            this.dgvcType.Name = "dgvcType";
            this.dgvcType.ReadOnly = true;
            this.dgvcType.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(258, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Event ID";
            // 
            // iEventID
            // 
            this.iEventID.Location = new System.Drawing.Point(381, 12);
            this.iEventID.Name = "iEventID";
            this.iEventID.ReadOnly = true;
            this.iEventID.Size = new System.Drawing.Size(158, 22);
            this.iEventID.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(258, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Membership";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(258, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Appointment Time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(258, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "End Time";
            // 
            // iAppointment
            // 
            this.iAppointment.CustomFormat = "dd MMM yyyy";
            this.iAppointment.Enabled = false;
            this.iAppointment.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.iAppointment.Location = new System.Drawing.Point(381, 40);
            this.iAppointment.Name = "iAppointment";
            this.iAppointment.Size = new System.Drawing.Size(112, 22);
            this.iAppointment.TabIndex = 22;
            this.iAppointment.Value = new System.DateTime(2011, 9, 18, 10, 38, 56, 0);
            this.iAppointment.Validating += new System.ComponentModel.CancelEventHandler(this.iAppointment_Validating);
            // 
            // iDuration
            // 
            this.iDuration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iDuration.FormattingEnabled = true;
            this.iDuration.Location = new System.Drawing.Point(381, 68);
            this.iDuration.Name = "iDuration";
            this.iDuration.Size = new System.Drawing.Size(312, 24);
            this.iDuration.TabIndex = 23;
            // 
            // iRoom
            // 
            this.iRoom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.iRoom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.iRoom.FormattingEnabled = true;
            this.iRoom.Location = new System.Drawing.Point(381, 143);
            this.iRoom.Name = "iRoom";
            this.iRoom.Size = new System.Drawing.Size(312, 24);
            this.iRoom.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(258, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 16);
            this.label6.TabIndex = 24;
            this.label6.Text = "Room";
            // 
            // iMembership
            // 
            this.iMembership.Location = new System.Drawing.Point(381, 98);
            this.iMembership.Name = "iMembership";
            this.iMembership.Size = new System.Drawing.Size(283, 22);
            this.iMembership.TabIndex = 27;
            // 
            // lblMembershipInfo
            // 
            this.lblMembershipInfo.AutoEllipsis = true;
            this.lblMembershipInfo.Location = new System.Drawing.Point(378, 124);
            this.lblMembershipInfo.Name = "lblMembershipInfo";
            this.lblMembershipInfo.Size = new System.Drawing.Size(315, 16);
            this.lblMembershipInfo.TabIndex = 67;
            this.lblMembershipInfo.Text = "KV00001 - Adeline Chng Yi Yun";
            // 
            // cmdMembershipSearch
            // 
            this.cmdMembershipSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdMembershipSearch.Location = new System.Drawing.Point(670, 98);
            this.cmdMembershipSearch.Name = "cmdMembershipSearch";
            this.cmdMembershipSearch.Size = new System.Drawing.Size(23, 23);
            this.cmdMembershipSearch.TabIndex = 68;
            this.cmdMembershipSearch.Text = "...";
            this.cmdMembershipSearch.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.iTime);
            this.panel4.Controls.Add(this.myCalendar);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.btnNew);
            this.panel4.Controls.Add(this.cmdMembershipSearch);
            this.panel4.Controls.Add(this.iEventID);
            this.panel4.Controls.Add(this.lblMembershipInfo);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.iMembership);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.iRoom);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.iAppointment);
            this.panel4.Controls.Add(this.iDuration);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(906, 189);
            this.panel4.TabIndex = 69;
            // 
            // myCalendar
            // 
            this.myCalendar.Location = new System.Drawing.Point(12, 12);
            this.myCalendar.MaxSelectionCount = 1;
            this.myCalendar.Name = "myCalendar";
            this.myCalendar.ShowTodayCircle = false;
            this.myCalendar.TabIndex = 75;
            this.myCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.myCalendar_DateChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(622, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 50);
            this.btnSave.TabIndex = 74;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.Color.Black;
            this.btnNew.Location = new System.Drawing.Point(545, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(71, 50);
            this.btnNew.TabIndex = 73;
            this.btnNew.Text = "NEW";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.panel3.Controls.Add(this.Tabel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.ForeColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(0, 189);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(906, 342);
            this.panel3.TabIndex = 70;
            // 
            // iTime
            // 
            this.iTime.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.iTime.Location = new System.Drawing.Point(499, 40);
            this.iTime.Mask = "00:00";
            this.iTime.Name = "iTime";
            this.iTime.Size = new System.Drawing.Size(40, 22);
            this.iTime.TabIndex = 77;
            this.iTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.iTime.ValidatingType = typeof(System.DateTime);
            this.iTime.Validating += new System.ComponentModel.CancelEventHandler(this.iTime_Validating);
            // 
            // frmCalendarEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.ClientSize = new System.Drawing.Size(906, 531);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmCalendarEdit";
            this.Text = "Appointment List";
            this.Load += new System.EventHandler(this.frmCalendarEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Tabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox iEventID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker iAppointment;
        private System.Windows.Forms.ComboBox iDuration;
        private System.Windows.Forms.ComboBox iRoom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox iMembership;
        private System.Windows.Forms.Label lblMembershipInfo;
        private System.Windows.Forms.Button cmdMembershipSearch;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewButtonColumn dgvbEdit;
        private System.Windows.Forms.DataGridViewButtonColumn dgvbDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcMembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcMembershipName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcAppointmentTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcRoomID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcRoomName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcType;
        private System.Windows.Forms.MonthCalendar myCalendar;
        private System.Windows.Forms.MaskedTextBox iTime;

    }
}