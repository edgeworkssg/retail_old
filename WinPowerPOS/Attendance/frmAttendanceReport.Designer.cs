namespace WinPowerPOS.Attendance
{
    partial class frmAttendanceReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgCheckOut = new System.Windows.Forms.DataGridView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cbPOS = new System.Windows.Forms.ComboBox();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.dgvcOCardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOMemberName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcODuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgCheckOut)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgCheckOut
            // 
            this.dgCheckOut.AllowUserToAddRows = false;
            this.dgCheckOut.AllowUserToDeleteRows = false;
            this.dgCheckOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCheckOut.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcOCardNo,
            this.dgvcOMemberName,
            this.dgvcOStartTime,
            this.dgvcODuration,
            this.dgvcOEndTime});
            this.dgCheckOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCheckOut.Location = new System.Drawing.Point(0, 98);
            this.dgCheckOut.Name = "dgCheckOut";
            this.dgCheckOut.ReadOnly = true;
            this.dgCheckOut.RowHeadersVisible = false;
            this.dgCheckOut.Size = new System.Drawing.Size(738, 224);
            this.dgCheckOut.TabIndex = 3;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSearch.Controls.Add(this.btnExport);
            this.pnlSearch.Controls.Add(this.btnConfirm);
            this.pnlSearch.Controls.Add(this.label8);
            this.pnlSearch.Controls.Add(this.cbPOS);
            this.pnlSearch.Controls.Add(this.dtEnd);
            this.pnlSearch.Controls.Add(this.label7);
            this.pnlSearch.Controls.Add(this.dtStart);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(738, 98);
            this.pnlSearch.TabIndex = 4;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnConfirm.CausesValidation = false;
            this.btnConfirm.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfirm.Location = new System.Drawing.Point(369, 45);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(0);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(124, 44);
            this.btnConfirm.TabIndex = 62;
            this.btnConfirm.Text = "SEARCH";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 16);
            this.label8.TabIndex = 5;
            this.label8.Text = "P.O.S";
            // 
            // cbPOS
            // 
            this.cbPOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPOS.FormattingEnabled = true;
            this.cbPOS.Location = new System.Drawing.Point(88, 62);
            this.cbPOS.Name = "cbPOS";
            this.cbPOS.Size = new System.Drawing.Size(262, 24);
            this.cbPOS.TabIndex = 4;
            // 
            // dtEnd
            // 
            this.dtEnd.Checked = false;
            this.dtEnd.CustomFormat = "dd MMM yyyy HH:mm:ss";
            this.dtEnd.Location = new System.Drawing.Point(88, 34);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.ShowCheckBox = true;
            this.dtEnd.Size = new System.Drawing.Size(262, 22);
            this.dtEnd.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "End Date";
            // 
            // dtStart
            // 
            this.dtStart.Checked = false;
            this.dtStart.CustomFormat = "dd MMM yyyy HH:mm:ss";
            this.dtStart.Location = new System.Drawing.Point(88, 6);
            this.dtStart.Name = "dtStart";
            this.dtStart.ShowCheckBox = true;
            this.dtStart.Size = new System.Drawing.Size(262, 22);
            this.dtStart.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Date";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExport.Location = new System.Drawing.Point(599, 54);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(132, 35);
            this.btnExport.TabIndex = 63;
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
            // dgvcOCardNo
            // 
            this.dgvcOCardNo.DataPropertyName = "MembershipNo";
            this.dgvcOCardNo.HeaderText = "Card No";
            this.dgvcOCardNo.Name = "dgvcOCardNo";
            this.dgvcOCardNo.ReadOnly = true;
            // 
            // dgvcOMemberName
            // 
            this.dgvcOMemberName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcOMemberName.DataPropertyName = "MembershipName";
            this.dgvcOMemberName.HeaderText = "Member Name";
            this.dgvcOMemberName.MinimumWidth = 150;
            this.dgvcOMemberName.Name = "dgvcOMemberName";
            this.dgvcOMemberName.ReadOnly = true;
            // 
            // dgvcOStartTime
            // 
            this.dgvcOStartTime.DataPropertyName = "LoginTime";
            dataGridViewCellStyle1.Format = "dd MMM yyyy HH:mm:ss";
            dataGridViewCellStyle1.NullValue = null;
            this.dgvcOStartTime.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvcOStartTime.HeaderText = "Start";
            this.dgvcOStartTime.Name = "dgvcOStartTime";
            this.dgvcOStartTime.ReadOnly = true;
            this.dgvcOStartTime.Width = 150;
            // 
            // dgvcODuration
            // 
            this.dgvcODuration.DataPropertyName = "Duration";
            dataGridViewCellStyle2.Format = "HH\'h\' mm\'m\' ss\'s\'";
            this.dgvcODuration.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvcODuration.HeaderText = "Duration";
            this.dgvcODuration.Name = "dgvcODuration";
            this.dgvcODuration.ReadOnly = true;
            this.dgvcODuration.Width = 150;
            // 
            // dgvcOEndTime
            // 
            this.dgvcOEndTime.DataPropertyName = "LogoutTime";
            dataGridViewCellStyle3.Format = "dd MMM yyyy HH:mm:ss";
            this.dgvcOEndTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvcOEndTime.HeaderText = "End";
            this.dgvcOEndTime.Name = "dgvcOEndTime";
            this.dgvcOEndTime.ReadOnly = true;
            this.dgvcOEndTime.Width = 150;
            // 
            // frmAttendanceReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 322);
            this.Controls.Add(this.dgCheckOut);
            this.Controls.Add(this.pnlSearch);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAttendanceReport";
            this.Text = "Attendance Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAttendanceReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgCheckOut)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgCheckOut;
        private System.Windows.Forms.Panel pnlSearch;
        internal System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbPOS;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOCardNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOMemberName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcODuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOEndTime;
    }
}