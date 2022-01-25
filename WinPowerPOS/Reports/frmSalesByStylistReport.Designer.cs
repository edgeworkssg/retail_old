namespace WinPowerPOS.Reports
{
    partial class frmSalesByStylistReport
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbInventory = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.cbServiceItem = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.OrderHdrID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ORDERDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExport = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbWithCommision = new System.Windows.Forms.ComboBox();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.cmbWithCommision);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbInventory);
            this.groupBox3.Controls.Add(this.btnGenerate);
            this.groupBox3.Controls.Add(this.cbServiceItem);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.dtpEndDate);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.dtpStartDate);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(663, 90);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Search";
            // 
            // cbInventory
            // 
            this.cbInventory.AutoSize = true;
            this.cbInventory.Checked = true;
            this.cbInventory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInventory.Location = new System.Drawing.Point(319, 44);
            this.cbInventory.Name = "cbInventory";
            this.cbInventory.Size = new System.Drawing.Size(68, 17);
            this.cbInventory.TabIndex = 33;
            this.cbInventory.Text = "Products";
            this.cbInventory.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnGenerate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGenerate.Location = new System.Drawing.Point(572, 47);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(85, 37);
            this.btnGenerate.TabIndex = 31;
            this.btnGenerate.Text = "Search";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cbServiceItem
            // 
            this.cbServiceItem.AutoSize = true;
            this.cbServiceItem.Checked = true;
            this.cbServiceItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbServiceItem.Location = new System.Drawing.Point(319, 21);
            this.cbServiceItem.Name = "cbServiceItem";
            this.cbServiceItem.Size = new System.Drawing.Size(85, 17);
            this.cbServiceItem.TabIndex = 32;
            this.cbServiceItem.Text = "Service Item";
            this.cbServiceItem.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(6, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Start Date:";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "dd - MMM - yyyy HH:mm:ss";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(70, 47);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(193, 20);
            this.dtpEndDate.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(6, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "End Date:";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "dd - MMM - yyyy HH:mm:ss";
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(70, 21);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(193, 20);
            this.dtpStartDate.TabIndex = 18;
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderHdrID,
            this.ORDERDATE});
            this.dgvReport.Location = new System.Drawing.Point(13, 108);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.Size = new System.Drawing.Size(827, 360);
            this.dgvReport.TabIndex = 30;
            this.dgvReport.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvReport_RowPrePaint);
            // 
            // OrderHdrID
            // 
            this.OrderHdrID.DataPropertyName = "OrderHdrID";
            this.OrderHdrID.HeaderText = "REF NO";
            this.OrderHdrID.Name = "OrderHdrID";
            this.OrderHdrID.ReadOnly = true;
            // 
            // ORDERDATE
            // 
            this.ORDERDATE.DataPropertyName = "orderdate";
            this.ORDERDATE.HeaderText = "DATE";
            this.ORDERDATE.Name = "ORDERDATE";
            this.ORDERDATE.ReadOnly = true;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExport.Location = new System.Drawing.Point(708, 67);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(132, 35);
            this.btnExport.TabIndex = 31;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(424, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Commision";
            // 
            // cmbWithCommision
            // 
            this.cmbWithCommision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWithCommision.FormattingEnabled = true;
            this.cmbWithCommision.Items.AddRange(new object[] {
            "ALL",
            "Without Commision",
            "With Commision"});
            this.cmbWithCommision.Location = new System.Drawing.Point(427, 42);
            this.cmbWithCommision.Name = "cmbWithCommision";
            this.cmbWithCommision.Size = new System.Drawing.Size(121, 21);
            this.cmbWithCommision.TabIndex = 35;
            // 
            // frmSalesByStylistReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(852, 480);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.groupBox3);
            this.Name = "frmSalesByStylistReport";
            this.Text = "Sales By Stylist";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DataGridView dgvReport;
        internal System.Windows.Forms.Button btnGenerate;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.CheckBox cbInventory;
        private System.Windows.Forms.CheckBox cbServiceItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderHdrID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORDERDATE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbWithCommision;

    }
}