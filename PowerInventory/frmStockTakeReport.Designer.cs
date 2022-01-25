namespace PowerInventory
{
    partial class frmStockTakeReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockTakeReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OnHand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactoryPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockTakeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Verified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemBalQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryLocationCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExport = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.txtQtyDisc = new System.Windows.Forms.TextBox();
            this.txtValueDisc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtVerifiedBy = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTakenBy = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bgDownload = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.pnlProgress.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_SelectedIndexChanged);
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            this.dgvStock.AllowUserToResizeColumns = false;
            this.dgvStock.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvStock, "dgvStock");
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemNo,
            this.ItemName,
            this.OnHand,
            this.Quantity,
            this.defi,
            this.FactoryPrice,
            this.TotalCost,
            this.StockTakeDate,
            this.Remark,
            this.Verified});
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.TabStop = false;
            this.dgvStock.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStock_CellMouseClick);
            // 
            // ItemNo
            // 
            this.ItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ItemNo.DataPropertyName = "ItemNo";
            resources.ApplyResources(this.ItemNo, "ItemNo");
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.FillWeight = 14F;
            resources.ApplyResources(this.ItemName, "ItemName");
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // OnHand
            // 
            this.OnHand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OnHand.DataPropertyName = "BalQtyAtEntry";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "0.####";
            this.OnHand.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.OnHand, "OnHand");
            this.OnHand.Name = "OnHand";
            this.OnHand.ReadOnly = true;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Quantity.DataPropertyName = "StockTakeQty";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "0.####";
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.Quantity, "Quantity");
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // defi
            // 
            this.defi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.defi.DataPropertyName = "AdjustmentQty";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "0.####";
            this.defi.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.defi, "defi");
            this.defi.Name = "defi";
            this.defi.ReadOnly = true;
            // 
            // FactoryPrice
            // 
            this.FactoryPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.FactoryPrice.DataPropertyName = "CostOfGoods";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = "0";
            this.FactoryPrice.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.FactoryPrice, "FactoryPrice");
            this.FactoryPrice.Name = "FactoryPrice";
            this.FactoryPrice.ReadOnly = true;
            // 
            // TotalCost
            // 
            this.TotalCost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TotalCost.DataPropertyName = "TotalDiscrepancyValue";
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.TotalCost.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.TotalCost, "TotalCost");
            this.TotalCost.Name = "TotalCost";
            this.TotalCost.ReadOnly = true;
            // 
            // StockTakeDate
            // 
            this.StockTakeDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StockTakeDate.DataPropertyName = "StockTakeDate";
            resources.ApplyResources(this.StockTakeDate, "StockTakeDate");
            this.StockTakeDate.Name = "StockTakeDate";
            this.StockTakeDate.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Remark.DataPropertyName = "TakenBy";
            resources.ApplyResources(this.Remark, "Remark");
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // Verified
            // 
            this.Verified.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Verified.DataPropertyName = "VerifiedBy";
            resources.ApplyResources(this.Verified, "Verified");
            this.Verified.Name = "Verified";
            this.Verified.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "StockTakeID";
            resources.ApplyResources(this.ID, "ID");
            this.ID.Name = "ID";
            // 
            // SystemBalQty
            // 
            this.SystemBalQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SystemBalQty.DataPropertyName = "OnHand";
            resources.ApplyResources(this.SystemBalQty, "SystemBalQty");
            this.SystemBalQty.Name = "SystemBalQty";
            // 
            // InventoryLocationCol
            // 
            this.InventoryLocationCol.DataPropertyName = "InventoryLocationName";
            resources.ApplyResources(this.InventoryLocationCol, "InventoryLocationCol");
            this.InventoryLocationCol.Name = "InventoryLocationCol";
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            resources.ApplyResources(this.fsdExportToExcel, "fsdExportToExcel");
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // pnlProgress
            // 
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Name = "pnlProgress";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // pgb1
            // 
            resources.ApplyResources(this.pgb1, "pgb1");
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // bgSearch
            // 
            this.bgSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSearch_DoWork);
            this.bgSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgSearch_RunWorkerCompleted);
            this.bgSearch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // txtQtyDisc
            // 
            resources.ApplyResources(this.txtQtyDisc, "txtQtyDisc");
            this.txtQtyDisc.Name = "txtQtyDisc";
            this.txtQtyDisc.ReadOnly = true;
            // 
            // txtValueDisc
            // 
            resources.ApplyResources(this.txtValueDisc, "txtValueDisc");
            this.txtValueDisc.Name = "txtValueDisc";
            this.txtValueDisc.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtVerifiedBy);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtTakenBy);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.cmbLocation);
            this.groupBox1.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Name = "label9";
            // 
            // txtVerifiedBy
            // 
            resources.ApplyResources(this.txtVerifiedBy, "txtVerifiedBy");
            this.txtVerifiedBy.Name = "txtVerifiedBy";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // txtTakenBy
            // 
            resources.ApplyResources(this.txtTakenBy, "txtTakenBy");
            this.txtTakenBy.Name = "txtTakenBy";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dtpEndDate
            // 
            resources.ApplyResources(this.dtpEndDate, "dtpEndDate");
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
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
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Name = "label7";
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.Name = "txtSearch";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // dtpStartDate
            // 
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // bgDownload
            // 
            this.bgDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDownload_DoWork);
            this.bgDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgDownload_RunWorkerCompleted);
            this.bgDownload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // frmStockTakeReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtValueDisc);
            this.Controls.Add(this.txtQtyDisc);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.dgvStock);
            this.Name = "frmStockTakeReport";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmStockTakeReport_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStockTakeReport_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        protected System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemBalQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryLocationCol;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
        private System.ComponentModel.BackgroundWorker bgSearch;
        private System.Windows.Forms.TextBox txtQtyDisc;
        private System.Windows.Forms.TextBox txtValueDisc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtVerifiedBy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTakenBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn VerifiedBy;
        private System.ComponentModel.BackgroundWorker bgDownload;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OnHand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn defi;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactoryPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockTakeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Verified;
      
    }
}