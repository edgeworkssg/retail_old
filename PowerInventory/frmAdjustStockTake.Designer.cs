namespace PowerInventory
{
    partial class frmAdjustStockTake
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdjustStockTake));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemBalQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryLocationCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.SN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OnHand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactoryPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockTakeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryDetRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockTakeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.btnDeleteChecked = new System.Windows.Forms.Button();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.llInvert = new System.Windows.Forms.LinkLabel();
            this.llSelectNone = new System.Windows.Forms.LinkLabel();
            this.llSelectAll = new System.Windows.Forms.LinkLabel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtQtyDisc = new System.Windows.Forms.TextBox();
            this.txtValueDisc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bgDownload = new System.ComponentModel.BackgroundWorker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.pnlProgress.SuspendLayout();
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
            resources.ApplyResources(this.ItemName, "ItemName");
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
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
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            this.dgvStock.AllowUserToResizeColumns = false;
            this.dgvStock.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvStock, "dgvStock");
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SN,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.OnHand,
            this.Quantity,
            this.defi,
            this.FactoryPrice,
            this.TotalCost,
            this.StockTakeDate,
            this.Remark,
            this.InventoryDetRefNo,
            this.StockTakeID});
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.TabStop = false;
            this.dgvStock.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStock_CellValueChanged);
            this.dgvStock.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStock_CellMouseClick);
            this.dgvStock.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStock_CellMouseMove);
            this.dgvStock.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStock_CellClick);
            this.dgvStock.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvStock_DataBindingComplete);
            // 
            // SN
            // 
            this.SN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SN.DataPropertyName = "Marked";
            this.SN.FalseValue = "false";
            resources.ApplyResources(this.SN, "SN");
            this.SN.IndeterminateValue = "false";
            this.SN.Name = "SN";
            this.SN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SN.TrueValue = "true";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ItemNo";
            this.dataGridViewTextBoxColumn1.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ItemName";
            this.dataGridViewTextBoxColumn2.FillWeight = 14.71503F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // OnHand
            // 
            this.OnHand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OnHand.DataPropertyName = "SystemBalQty";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "0.####";
            this.OnHand.DefaultCellStyle = dataGridViewCellStyle6;
            this.OnHand.FillWeight = 1F;
            resources.ApplyResources(this.OnHand, "OnHand");
            this.OnHand.Name = "OnHand";
            this.OnHand.ReadOnly = true;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Quantity.DataPropertyName = "StockTakeQty";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Format = "0.####";
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.Quantity, "Quantity");
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // defi
            // 
            this.defi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.defi.DataPropertyName = "defi";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Format = "0.####";
            this.defi.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.defi, "defi");
            this.defi.Name = "defi";
            this.defi.ReadOnly = true;
            // 
            // FactoryPrice
            // 
            this.FactoryPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.FactoryPrice.DataPropertyName = "CostOfGoods";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            dataGridViewCellStyle9.NullValue = "0";
            this.FactoryPrice.DefaultCellStyle = dataGridViewCellStyle9;
            this.FactoryPrice.FillWeight = 14.71503F;
            resources.ApplyResources(this.FactoryPrice, "FactoryPrice");
            this.FactoryPrice.Name = "FactoryPrice";
            this.FactoryPrice.ReadOnly = true;
            // 
            // TotalCost
            // 
            this.TotalCost.DataPropertyName = "TotalCost";
            dataGridViewCellStyle10.Format = "N2";
            dataGridViewCellStyle10.NullValue = null;
            this.TotalCost.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.TotalCost, "TotalCost");
            this.TotalCost.Name = "TotalCost";
            this.TotalCost.ReadOnly = true;
            // 
            // StockTakeDate
            // 
            this.StockTakeDate.DataPropertyName = "StockTakeDate";
            resources.ApplyResources(this.StockTakeDate, "StockTakeDate");
            this.StockTakeDate.Name = "StockTakeDate";
            this.StockTakeDate.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Remark.DataPropertyName = "TakenBy";
            this.Remark.FillWeight = 14.71503F;
            resources.ApplyResources(this.Remark, "Remark");
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // InventoryDetRefNo
            // 
            this.InventoryDetRefNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.InventoryDetRefNo.DataPropertyName = "VerifiedBy";
            resources.ApplyResources(this.InventoryDetRefNo, "InventoryDetRefNo");
            this.InventoryDetRefNo.Name = "InventoryDetRefNo";
            this.InventoryDetRefNo.ReadOnly = true;
            // 
            // StockTakeID
            // 
            this.StockTakeID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StockTakeID.DataPropertyName = "StockTakeID";
            resources.ApplyResources(this.StockTakeID, "StockTakeID");
            this.StockTakeID.Name = "StockTakeID";
            this.StockTakeID.ReadOnly = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            // btnDeleteChecked
            // 
            resources.ApplyResources(this.btnDeleteChecked, "btnDeleteChecked");
            this.btnDeleteChecked.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnDeleteChecked.ForeColor = System.Drawing.Color.White;
            this.btnDeleteChecked.Name = "btnDeleteChecked";
            this.btnDeleteChecked.UseVisualStyleBackColor = true;
            this.btnDeleteChecked.Click += new System.EventHandler(this.btnDeleteChecked_Click);
            // 
            // pnlProgress
            // 
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.lblStatus);
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Name = "pnlProgress";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Name = "lblStatus";
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
            // llInvert
            // 
            resources.ApplyResources(this.llInvert, "llInvert");
            this.llInvert.BackColor = System.Drawing.Color.Transparent;
            this.llInvert.Name = "llInvert";
            this.llInvert.TabStop = true;
            this.llInvert.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llInvert.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llInvert_LinkClicked);
            // 
            // llSelectNone
            // 
            resources.ApplyResources(this.llSelectNone, "llSelectNone");
            this.llSelectNone.BackColor = System.Drawing.Color.Transparent;
            this.llSelectNone.Name = "llSelectNone";
            this.llSelectNone.TabStop = true;
            this.llSelectNone.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llSelectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectNone_LinkClicked);
            // 
            // llSelectAll
            // 
            resources.ApplyResources(this.llSelectAll, "llSelectAll");
            this.llSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.llSelectAll.Name = "llSelectAll";
            this.llSelectAll.TabStop = true;
            this.llSelectAll.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectAll_LinkClicked);
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = true;
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
            // bgDownload
            // 
            this.bgDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDownload_DoWork);
            this.bgDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgDownload_RunWorkerCompleted);
            this.bgDownload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtFilter
            // 
            resources.ApplyResources(this.txtFilter, "txtFilter");
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            this.txtFilter.Validated += new System.EventHandler(this.txtFilter_Validated);
            // 
            // frmAdjustStockTake
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtValueDisc);
            this.Controls.Add(this.txtQtyDisc);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.llInvert);
            this.Controls.Add(this.llSelectNone);
            this.Controls.Add(this.llSelectAll);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.btnDeleteChecked);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvStock);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbLocation);
            this.Name = "frmAdjustStockTake";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAdjustStockTake_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAdjustStockTake_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        protected System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemBalQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryLocationCol;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.Button btnDeleteChecked;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
        private System.ComponentModel.BackgroundWorker bgSearch;
        private System.Windows.Forms.LinkLabel llInvert;
        private System.Windows.Forms.LinkLabel llSelectNone;
        private System.Windows.Forms.LinkLabel llSelectAll;
        protected System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtQtyDisc;
        private System.Windows.Forms.TextBox txtValueDisc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker bgDownload;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn OnHand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn defi;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactoryPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockTakeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryDetRefNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockTakeID;
      
    }
}