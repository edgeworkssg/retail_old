namespace PowerInventory.InventoryForms
{
    partial class frmStockOnHand
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockOnHand));
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
            this.bgDownload = new System.ComponentModel.BackgroundWorker();
            this.lblTotal = new System.Windows.Forms.Label();
            this.dgvPurchase = new FixReEntrantDataGridView();
            this.lblItemCount = new System.Windows.Forms.Label();
            this.view = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactoryPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemBalQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.txtItemName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItemName_KeyDown);
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
            // bgDownload
            // 
            this.bgDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDownload_DoWork);
            this.bgDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgDownload_RunWorkerCompleted);
            this.bgDownload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // lblTotal
            // 
            resources.ApplyResources(this.lblTotal, "lblTotal");
            this.lblTotal.Name = "lblTotal";
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
            this.FactoryPrice,
            this.UOM,
            this.SystemBalQty,
            this.ItemDesc,
            this.ID});
            this.dgvPurchase.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvPurchase.Name = "dgvPurchase";
            this.dgvPurchase.RowHeadersVisible = false;
            this.dgvPurchase.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPurchase.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchase_CellContentClick);
            // 
            // lblItemCount
            // 
            resources.ApplyResources(this.lblItemCount, "lblItemCount");
            this.lblItemCount.Name = "lblItemCount";
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
            this.ItemNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ItemName.FillWeight = 200F;
            resources.ApplyResources(this.ItemName, "ItemName");
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            this.ItemName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // FactoryPrice
            // 
            this.FactoryPrice.DataPropertyName = "COG";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "0";
            this.FactoryPrice.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.FactoryPrice, "FactoryPrice");
            this.FactoryPrice.Name = "FactoryPrice";
            this.FactoryPrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            resources.ApplyResources(this.UOM, "UOM");
            this.UOM.Name = "UOM";
            // 
            // SystemBalQty
            // 
            this.SystemBalQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SystemBalQty.DataPropertyName = "Quantity";
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.SystemBalQty.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.SystemBalQty, "SystemBalQty");
            this.SystemBalQty.Name = "SystemBalQty";
            this.SystemBalQty.ReadOnly = true;
            // 
            // ItemDesc
            // 
            this.ItemDesc.DataPropertyName = "ItemDesc";
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemDesc.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.ItemDesc, "ItemDesc");
            this.ItemDesc.Name = "ItemDesc";
            this.ItemDesc.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "StockTakeID";
            resources.ApplyResources(this.ID, "ID");
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // frmStockOnHand
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblItemCount);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dgvPurchase);
            this.KeyPreview = true;
            this.Name = "frmStockOnHand";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.OrderTaking_Load);
            this.Activated += new System.EventHandler(this.OrderTaking_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStockOnHand_FormClosed);
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
        private System.ComponentModel.BackgroundWorker bgDownload;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblItemCount;
        private System.Windows.Forms.DataGridViewButtonColumn view;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactoryPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemBalQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
    }
}

