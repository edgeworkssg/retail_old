namespace WinPowerPOS.Reports
{
    partial class LineCommissionReport
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Service = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OpenPriceProduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NonCommission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalProductAndService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PointSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PointSoldValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackageSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackageRedeemed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackageRedeemedValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.btnGenerate);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.dtpEndDate);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.dtpStartDate);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(508, 90);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Search";
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnGenerate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGenerate.Location = new System.Drawing.Point(417, 47);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(85, 37);
            this.btnGenerate.TabIndex = 31;
            this.btnGenerate.Text = "Search";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
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
            this.UserName,
            this.Service,
            this.Product,
            this.OpenPriceProduct,
            this.SystemItem,
            this.NonCommission,
            this.TotalProductAndService,
            this.PointSold,
            this.PointSoldValue,
            this.PackageSold,
            this.PackageRedeemed,
            this.PackageRedeemedValue});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Format = "C2";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport.Location = new System.Drawing.Point(13, 108);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.Size = new System.Drawing.Size(827, 360);
            this.dgvReport.TabIndex = 30;
            this.dgvReport.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvReport_RowPrePaint);
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
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.HeaderText = "Sales Person";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            // 
            // Service
            // 
            this.Service.DataPropertyName = "Service";
            this.Service.HeaderText = "Service";
            this.Service.Name = "Service";
            this.Service.ReadOnly = true;
            // 
            // Product
            // 
            this.Product.DataPropertyName = "Product";
            this.Product.HeaderText = "Product";
            this.Product.Name = "Product";
            this.Product.ReadOnly = true;
            // 
            // OpenPriceProduct
            // 
            this.OpenPriceProduct.DataPropertyName = "OpenPriceProduct";
            this.OpenPriceProduct.HeaderText = "O.P Product";
            this.OpenPriceProduct.Name = "OpenPriceProduct";
            this.OpenPriceProduct.ReadOnly = true;
            // 
            // SystemItem
            // 
            this.SystemItem.DataPropertyName = "SystemItem";
            this.SystemItem.HeaderText = "System Item";
            this.SystemItem.Name = "SystemItem";
            this.SystemItem.ReadOnly = true;
            // 
            // NonCommission
            // 
            this.NonCommission.DataPropertyName = "NonCommission";
            this.NonCommission.HeaderText = "Non Commission";
            this.NonCommission.Name = "NonCommission";
            this.NonCommission.ReadOnly = true;
            // 
            // TotalProductAndService
            // 
            this.TotalProductAndService.DataPropertyName = "TotalProductAndService";
            this.TotalProductAndService.HeaderText = "Total Product & Service";
            this.TotalProductAndService.Name = "TotalProductAndService";
            this.TotalProductAndService.ReadOnly = true;
            // 
            // PointSold
            // 
            this.PointSold.DataPropertyName = "PointSold";
            this.PointSold.HeaderText = "Point Sold";
            this.PointSold.Name = "PointSold";
            this.PointSold.ReadOnly = true;
            // 
            // PointSoldValue
            // 
            this.PointSoldValue.DataPropertyName = "PointSoldValue";
            this.PointSoldValue.HeaderText = "Point Sold Value";
            this.PointSoldValue.Name = "PointSoldValue";
            this.PointSoldValue.ReadOnly = true;
            // 
            // PackageSold
            // 
            this.PackageSold.DataPropertyName = "PackageSold";
            this.PackageSold.HeaderText = "Package Sold";
            this.PackageSold.Name = "PackageSold";
            this.PackageSold.ReadOnly = true;
            // 
            // PackageRedeemed
            // 
            this.PackageRedeemed.DataPropertyName = "PackageRedeemed";
            this.PackageRedeemed.HeaderText = "Package Redeemed";
            this.PackageRedeemed.Name = "PackageRedeemed";
            this.PackageRedeemed.ReadOnly = true;
            // 
            // PackageRedeemedValue
            // 
            this.PackageRedeemedValue.DataPropertyName = "PackageRedeemedValue";
            this.PackageRedeemedValue.HeaderText = "Package Redeemed Value";
            this.PackageRedeemedValue.Name = "PackageRedeemedValue";
            this.PackageRedeemedValue.ReadOnly = true;
            // 
            // LineCommissionReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(852, 480);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.groupBox3);
            this.Name = "LineCommissionReport";
            this.Text = "Sales Person Performance Report";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Service;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenPriceProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn NonCommission;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalProductAndService;
        private System.Windows.Forms.DataGridViewTextBoxColumn PointSold;
        private System.Windows.Forms.DataGridViewTextBoxColumn PointSoldValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackageSold;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackageRedeemed;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackageRedeemedValue;

    }
}