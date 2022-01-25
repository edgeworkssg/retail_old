namespace PowerInventory
{
    partial class frmStockCardReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockCardReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dgvRcpt = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.bgDownload = new System.ComponentModel.BackgroundWorker();
            this.DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BalanceBefore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockIn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockOut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransferIn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransferOut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdjustmentIn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdjustmentOut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnOut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BalanceAfter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryLocationCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRcpt)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.cmbCategory);
            this.groupBox2.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCategory, "cmbCategory");
            this.cmbCategory.Name = "cmbCategory";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.Name = "cmbLocation";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // txtItemName
            // 
            resources.ApplyResources(this.txtItemName, "txtItemName");
            this.txtItemName.Name = "txtItemName";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.cmbLocation);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtItemName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnExport
            // 
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // dtpStartDate
            // 
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Name = "dtpStartDate";
            // 
            // dgvRcpt
            // 
            this.dgvRcpt.AllowUserToAddRows = false;
            this.dgvRcpt.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvRcpt, "dgvRcpt");
            this.dgvRcpt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRcpt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DepartmentName,
            this.CategoryName,
            this.ItemNo,
            this.ItemName,
            this.BalanceBefore,
            this.StockIn,
            this.StockOut,
            this.TransferIn,
            this.TransferOut,
            this.AdjustmentIn,
            this.AdjustmentOut,
            this.ReturnOut,
            this.BalanceAfter,
            this.UOM,
            this.InventoryLocationCol});
            this.dgvRcpt.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvRcpt.Name = "dgvRcpt";
            this.dgvRcpt.ReadOnly = true;
            this.dgvRcpt.RowHeadersVisible = false;
            this.dgvRcpt.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRcpt_CellContentClick);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
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
            // bgDownload
            // 
            this.bgDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDownload_DoWork);
            this.bgDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgDownload_RunWorkerCompleted);
            this.bgDownload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // DepartmentName
            // 
            this.DepartmentName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DepartmentName.DataPropertyName = "DepartmentName";
            resources.ApplyResources(this.DepartmentName, "DepartmentName");
            this.DepartmentName.Name = "DepartmentName";
            this.DepartmentName.ReadOnly = true;
            // 
            // CategoryName
            // 
            this.CategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CategoryName.DataPropertyName = "CategoryName";
            resources.ApplyResources(this.CategoryName, "CategoryName");
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.ReadOnly = true;
            // 
            // ItemNo
            // 
            this.ItemNo.DataPropertyName = "ItemNo";
            resources.ApplyResources(this.ItemNo, "ItemNo");
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            resources.ApplyResources(this.ItemName, "ItemName");
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // BalanceBefore
            // 
            this.BalanceBefore.DataPropertyName = "BalanceBefore";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "0.####";
            this.BalanceBefore.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.BalanceBefore, "BalanceBefore");
            this.BalanceBefore.Name = "BalanceBefore";
            this.BalanceBefore.ReadOnly = true;
            // 
            // StockIn
            // 
            this.StockIn.DataPropertyName = "StockIn";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "0.####";
            dataGridViewCellStyle2.NullValue = null;
            this.StockIn.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.StockIn, "StockIn");
            this.StockIn.Name = "StockIn";
            this.StockIn.ReadOnly = true;
            // 
            // StockOut
            // 
            this.StockOut.DataPropertyName = "StockOut";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "0.####";
            this.StockOut.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.StockOut, "StockOut");
            this.StockOut.Name = "StockOut";
            this.StockOut.ReadOnly = true;
            // 
            // TransferIn
            // 
            this.TransferIn.DataPropertyName = "TransferIn";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "0.####";
            this.TransferIn.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.TransferIn, "TransferIn");
            this.TransferIn.Name = "TransferIn";
            this.TransferIn.ReadOnly = true;
            // 
            // TransferOut
            // 
            this.TransferOut.DataPropertyName = "TransferOut";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "0.####";
            this.TransferOut.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.TransferOut, "TransferOut");
            this.TransferOut.Name = "TransferOut";
            this.TransferOut.ReadOnly = true;
            // 
            // AdjustmentIn
            // 
            this.AdjustmentIn.DataPropertyName = "AdjustmentIn";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "0.####";
            this.AdjustmentIn.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.AdjustmentIn, "AdjustmentIn");
            this.AdjustmentIn.Name = "AdjustmentIn";
            this.AdjustmentIn.ReadOnly = true;
            // 
            // AdjustmentOut
            // 
            this.AdjustmentOut.DataPropertyName = "AdjustmentOut";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "0.####";
            this.AdjustmentOut.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.AdjustmentOut, "AdjustmentOut");
            this.AdjustmentOut.Name = "AdjustmentOut";
            this.AdjustmentOut.ReadOnly = true;
            // 
            // ReturnOut
            // 
            this.ReturnOut.DataPropertyName = "ReturnOut";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "0.####";
            this.ReturnOut.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.ReturnOut, "ReturnOut");
            this.ReturnOut.Name = "ReturnOut";
            this.ReturnOut.ReadOnly = true;
            // 
            // BalanceAfter
            // 
            this.BalanceAfter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.BalanceAfter.DataPropertyName = "BalanceAfter";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "0.####";
            dataGridViewCellStyle9.NullValue = null;
            this.BalanceAfter.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.BalanceAfter, "BalanceAfter");
            this.BalanceAfter.Name = "BalanceAfter";
            this.BalanceAfter.ReadOnly = true;
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            resources.ApplyResources(this.UOM, "UOM");
            this.UOM.Name = "UOM";
            this.UOM.ReadOnly = true;
            // 
            // InventoryLocationCol
            // 
            this.InventoryLocationCol.DataPropertyName = "InventoryLocationName";
            resources.ApplyResources(this.InventoryLocationCol, "InventoryLocationCol");
            this.InventoryLocationCol.Name = "InventoryLocationCol";
            this.InventoryLocationCol.ReadOnly = true;
            // 
            // frmStockCardReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.dgvRcpt);
            this.DoubleBuffered = true;
            this.Name = "frmStockCardReport";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSearchInvoiceDet_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStockCardReport_FormClosed);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRcpt)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvRcpt;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.Label label2;
        protected System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
        private System.ComponentModel.BackgroundWorker bgSearch;
        private System.ComponentModel.BackgroundWorker bgDownload;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalanceBefore;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransferIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransferOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdjustmentIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdjustmentOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalanceAfter;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryLocationCol;
    }
}