namespace PowerInventory.InventoryForms
{
    partial class frmCheckStockOnHandCorrectness
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheckStockOnHandCorrectness));
            this.epOrder = new System.Windows.Forms.ErrorProvider(this.components);
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.btnExport = new System.Windows.Forms.Button();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.dgvPurchase = new FixReEntrantDataGridView();
            this.view = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemBalQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemainingQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Diff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryLocationCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.epOrder)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).BeginInit();
            this.SuspendLayout();
            // 
            // epOrder
            // 
            this.epOrder.ContainerControl = this;
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            resources.ApplyResources(this.fsdExportToExcel, "fsdExportToExcel");
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
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
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.Name = "cmbLocation";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbLocation);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtItemName);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
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
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            // bgSearch
            // 
            this.bgSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSearch_DoWork);
            this.bgSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgSearch_RunWorkerCompleted);
            this.bgSearch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
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
            // dgvPurchase
            // 
            this.dgvPurchase.AllowUserToAddRows = false;
            this.dgvPurchase.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvPurchase, "dgvPurchase");
            this.dgvPurchase.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPurchase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.view,
            this.ItemNo,
            this.ItemName,
            this.SystemBalQty,
            this.RemainingQty,
            this.Diff,
            this.ItemDesc,
            this.ID,
            this.InventoryLocationCol});
            this.dgvPurchase.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvPurchase.Name = "dgvPurchase";
            this.dgvPurchase.RowHeadersVisible = false;
            this.dgvPurchase.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPurchase.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvPurchase_RowPrePaint);
            this.dgvPurchase.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchase_CellContentClick);
            // 
            // view
            // 
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            this.view.Text = "View";
            this.view.UseColumnTextForButtonValue = true;
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
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ItemName.FillWeight = 36.06557F;
            resources.ApplyResources(this.ItemName, "ItemName");
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // SystemBalQty
            // 
            this.SystemBalQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SystemBalQty.DataPropertyName = "OnHand";
            resources.ApplyResources(this.SystemBalQty, "SystemBalQty");
            this.SystemBalQty.Name = "SystemBalQty";
            // 
            // RemainingQty
            // 
            this.RemainingQty.DataPropertyName = "RemainingQty";
            resources.ApplyResources(this.RemainingQty, "RemainingQty");
            this.RemainingQty.Name = "RemainingQty";
            // 
            // Diff
            // 
            this.Diff.DataPropertyName = "Diff";
            resources.ApplyResources(this.Diff, "Diff");
            this.Diff.Name = "Diff";
            // 
            // ItemDesc
            // 
            this.ItemDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDesc.DataPropertyName = "ItemDesc";
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemDesc.DefaultCellStyle = dataGridViewCellStyle2;
            this.ItemDesc.FillWeight = 163.9344F;
            resources.ApplyResources(this.ItemDesc, "ItemDesc");
            this.ItemDesc.Name = "ItemDesc";
            // 
            // ID
            // 
            this.ID.DataPropertyName = "StockTakeID";
            resources.ApplyResources(this.ID, "ID");
            this.ID.Name = "ID";
            // 
            // InventoryLocationCol
            // 
            this.InventoryLocationCol.DataPropertyName = "InventoryLocationName";
            resources.ApplyResources(this.InventoryLocationCol, "InventoryLocationCol");
            this.InventoryLocationCol.Name = "InventoryLocationCol";
            // 
            // frmCheckStockOnHandCorrectness
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.dgvPurchase);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.groupBox2);
            this.KeyPreview = true;
            this.Name = "frmCheckStockOnHandCorrectness";
            this.Load += new System.EventHandler(this.OrderTaking_Load);
            this.Activated += new System.EventHandler(this.OrderTaking_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmOrderTaking_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.epOrder)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider epOrder;
        private FixReEntrantDataGridView dgvPurchase;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        protected System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtItemName;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.ComponentModel.BackgroundWorker bgSearch;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
        private System.Windows.Forms.DataGridViewButtonColumn view;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemBalQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemainingQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Diff;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryLocationCol;
    }
}

