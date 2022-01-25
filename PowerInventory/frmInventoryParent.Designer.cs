namespace PowerInventory
{
    partial class frmInventoryParent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInventoryParent));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tbControl = new System.Windows.Forms.TabControl();
            this.tbHeader = new System.Windows.Forms.TabPage();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.tblInventory = new System.Windows.Forms.TableLayoutPanel();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpInventoryDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbBody = new System.Windows.Forms.TabPage();
            this.cmbCriteria = new System.Windows.Forms.ComboBox();
            this.lblTotalQuantity = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTotalItem = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDefaultQty = new System.Windows.Forms.TextBox();
            this.cbDefaultQty = new System.Windows.Forms.CheckBox();
            this.btnPage = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.lblTotalCostPriceAmount = new System.Windows.Forms.Label();
            this.lblTotalCostPrice = new System.Windows.Forms.Label();
            this.llInvert = new System.Windows.Forms.LinkLabel();
            this.llSelectNone = new System.Windows.Forms.LinkLabel();
            this.llSelectAll = new System.Windows.Forms.LinkLabel();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnScanItemNo = new System.Windows.Forms.Button();
            this.txtItemNoBarcode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.SN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OnHand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Currency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactoryPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AlternateCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CalcFactoryPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryDetRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SerialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLoadingMessage = new System.Windows.Forms.Label();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.saveProduct = new System.Windows.Forms.SaveFileDialog();
            this.btnDownloadTemplate = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSaveToDisk = new System.Windows.Forms.Button();
            this.btnDeleteChecked = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.tbControl.SuspendLayout();
            this.tbHeader.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.tblInventory.SuspendLayout();
            this.tbBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.pnlLoading.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbControl
            // 
            resources.ApplyResources(this.tbControl, "tbControl");
            this.tbControl.Controls.Add(this.tbHeader);
            this.tbControl.Controls.Add(this.tbBody);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 0;
            this.tbControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            // 
            // tbHeader
            // 
            this.tbHeader.BackColor = System.Drawing.Color.Gainsboro;
            this.tbHeader.Controls.Add(this.pnlProgress);
            this.tbHeader.Controls.Add(this.tblInventory);
            resources.ApplyResources(this.tbHeader, "tbHeader");
            this.tbHeader.Name = "tbHeader";
            // 
            // pnlProgress
            // 
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.label9);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Name = "pnlProgress";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Name = "label9";
            // 
            // pgb1
            // 
            resources.ApplyResources(this.pgb1, "pgb1");
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // tblInventory
            // 
            resources.ApplyResources(this.tblInventory, "tblInventory");
            this.tblInventory.BackColor = System.Drawing.Color.Gainsboro;
            this.tblInventory.Controls.Add(this.txtRemark, 4, 2);
            this.tblInventory.Controls.Add(this.label2, 0, 0);
            this.tblInventory.Controls.Add(this.label7, 3, 1);
            this.tblInventory.Controls.Add(this.label4, 0, 1);
            this.tblInventory.Controls.Add(this.txtRefNo, 1, 1);
            this.tblInventory.Controls.Add(this.label6, 0, 3);
            this.tblInventory.Controls.Add(this.dtpInventoryDate, 4, 1);
            this.tblInventory.Controls.Add(this.label5, 3, 2);
            this.tblInventory.Controls.Add(this.cmbLocation, 1, 2);
            this.tblInventory.Controls.Add(this.label1, 0, 2);
            this.tblInventory.Name = "tblInventory";
            // 
            // txtRemark
            // 
            resources.ApplyResources(this.txtRemark, "txtRemark");
            this.txtRemark.Name = "txtRemark";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Navy;
            this.tblInventory.SetColumnSpan(this.label2, 5);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Gainsboro;
            this.label7.Name = "label7";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Gainsboro;
            this.label4.Name = "label4";
            // 
            // txtRefNo
            // 
            resources.ApplyResources(this.txtRefNo, "txtRefNo");
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Navy;
            this.tblInventory.SetColumnSpan(this.label6, 5);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // dtpInventoryDate
            // 
            resources.ApplyResources(this.dtpInventoryDate, "dtpInventoryDate");
            this.dtpInventoryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInventoryDate.Name = "dtpInventoryDate";
            this.dtpInventoryDate.ValueChanged += new System.EventHandler(this.dtpInventoryDate_ValueChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Gainsboro;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // cmbLocation
            // 
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Name = "label1";
            // 
            // tbBody
            // 
            this.tbBody.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgnd;
            this.tbBody.Controls.Add(this.cmbCriteria);
            this.tbBody.Controls.Add(this.lblTotalQuantity);
            this.tbBody.Controls.Add(this.label14);
            this.tbBody.Controls.Add(this.lblTotalItem);
            this.tbBody.Controls.Add(this.label12);
            this.tbBody.Controls.Add(this.label10);
            this.tbBody.Controls.Add(this.txtDefaultQty);
            this.tbBody.Controls.Add(this.cbDefaultQty);
            this.tbBody.Controls.Add(this.btnPage);
            this.tbBody.Controls.Add(this.btnLastPage);
            this.tbBody.Controls.Add(this.btnNextPage);
            this.tbBody.Controls.Add(this.btnPreviousPage);
            this.tbBody.Controls.Add(this.btnFirstPage);
            this.tbBody.Controls.Add(this.lblTotalCostPriceAmount);
            this.tbBody.Controls.Add(this.lblTotalCostPrice);
            this.tbBody.Controls.Add(this.llInvert);
            this.tbBody.Controls.Add(this.llSelectNone);
            this.tbBody.Controls.Add(this.llSelectAll);
            this.tbBody.Controls.Add(this.btnAddItem);
            this.tbBody.Controls.Add(this.btnScanItemNo);
            this.tbBody.Controls.Add(this.txtItemNoBarcode);
            this.tbBody.Controls.Add(this.label8);
            this.tbBody.Controls.Add(this.dgvStock);
            resources.ApplyResources(this.tbBody, "tbBody");
            this.tbBody.Name = "tbBody";
            this.tbBody.UseVisualStyleBackColor = true;
            this.tbBody.Enter += new System.EventHandler(this.tbBody_Enter);
            // 
            // cmbCriteria
            // 
            this.cmbCriteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCriteria.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCriteria, "cmbCriteria");
            this.cmbCriteria.Name = "cmbCriteria";
            // 
            // lblTotalQuantity
            // 
            resources.ApplyResources(this.lblTotalQuantity, "lblTotalQuantity");
            this.lblTotalQuantity.Name = "lblTotalQuantity";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // lblTotalItem
            // 
            resources.ApplyResources(this.lblTotalItem, "lblTotalItem");
            this.lblTotalItem.Name = "lblTotalItem";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // txtDefaultQty
            // 
            resources.ApplyResources(this.txtDefaultQty, "txtDefaultQty");
            this.txtDefaultQty.Name = "txtDefaultQty";
            this.txtDefaultQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDefaultQty_KeyPress);
            // 
            // cbDefaultQty
            // 
            resources.ApplyResources(this.cbDefaultQty, "cbDefaultQty");
            this.cbDefaultQty.Name = "cbDefaultQty";
            this.cbDefaultQty.UseVisualStyleBackColor = true;
            // 
            // btnPage
            // 
            resources.ApplyResources(this.btnPage, "btnPage");
            this.btnPage.Name = "btnPage";
            this.btnPage.UseVisualStyleBackColor = true;
            // 
            // btnLastPage
            // 
            resources.ApplyResources(this.btnLastPage, "btnLastPage");
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnNextPage
            // 
            resources.ApplyResources(this.btnNextPage, "btnNextPage");
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPreviousPage
            // 
            resources.ApplyResources(this.btnPreviousPage, "btnPreviousPage");
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.UseVisualStyleBackColor = true;
            this.btnPreviousPage.Click += new System.EventHandler(this.btnPreviousPage_Click);
            // 
            // btnFirstPage
            // 
            resources.ApplyResources(this.btnFirstPage, "btnFirstPage");
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // lblTotalCostPriceAmount
            // 
            resources.ApplyResources(this.lblTotalCostPriceAmount, "lblTotalCostPriceAmount");
            this.lblTotalCostPriceAmount.Name = "lblTotalCostPriceAmount";
            // 
            // lblTotalCostPrice
            // 
            resources.ApplyResources(this.lblTotalCostPrice, "lblTotalCostPrice");
            this.lblTotalCostPrice.Name = "lblTotalCostPrice";
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
            // btnAddItem
            // 
            resources.ApplyResources(this.btnAddItem, "btnAddItem");
            this.btnAddItem.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnAddItem.ForeColor = System.Drawing.Color.White;
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnScanItemNo
            // 
            this.btnScanItemNo.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnScanItemNo, "btnScanItemNo");
            this.btnScanItemNo.ForeColor = System.Drawing.Color.White;
            this.btnScanItemNo.Name = "btnScanItemNo";
            this.btnScanItemNo.UseVisualStyleBackColor = true;
            this.btnScanItemNo.Click += new System.EventHandler(this.btnScanItemNo_Click);
            // 
            // txtItemNoBarcode
            // 
            resources.ApplyResources(this.txtItemNoBarcode, "txtItemNoBarcode");
            this.txtItemNoBarcode.Name = "txtItemNoBarcode";
            this.txtItemNoBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItemNoBarcode_KeyDown);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            this.dgvStock.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvStock, "dgvStock");
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvStock.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SN,
            this.ItemNo,
            this.ItemName,
            this.CategoryName,
            this.UOM,
            this.OnHand,
            this.Quantity,
            this.RetailPrice,
            this.Currency,
            this.FactoryPrice,
            this.AlternateCostPrice,
            this.Discount,
            this.CalcFactoryPrice,
            this.TotalCostPrice,
            this.ItemDesc,
            this.Remark,
            this.InventoryDetRefNo,
            this.SerialNo});
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.TabStop = false;
            this.dgvStock.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStock_CellMouseClick);
            this.dgvStock.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvStock_RowPrePaint);
            this.dgvStock.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStock_CellMouseMove);
            this.dgvStock.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStock_CellClick);
            this.dgvStock.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvStock_DataBindingComplete);
            // 
            // SN
            // 
            this.SN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SN.DataPropertyName = "Deleted";
            this.SN.FalseValue = "false";
            resources.ApplyResources(this.SN, "SN");
            this.SN.IndeterminateValue = "false";
            this.SN.Name = "SN";
            this.SN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SN.TrueValue = "true";
            // 
            // ItemNo
            // 
            this.ItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.FillWeight = 1F;
            resources.ApplyResources(this.ItemNo, "ItemNo");
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            this.ItemNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ItemName.FillWeight = 14.71503F;
            resources.ApplyResources(this.ItemName, "ItemName");
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            this.ItemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CategoryName
            // 
            this.CategoryName.DataPropertyName = "CategoryName";
            resources.ApplyResources(this.CategoryName, "CategoryName");
            this.CategoryName.Name = "CategoryName";
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            resources.ApplyResources(this.UOM, "UOM");
            this.UOM.Name = "UOM";
            // 
            // OnHand
            // 
            this.OnHand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OnHand.DataPropertyName = "OnHand";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "0.####";
            this.OnHand.DefaultCellStyle = dataGridViewCellStyle2;
            this.OnHand.FillWeight = 1F;
            resources.ApplyResources(this.OnHand, "OnHand");
            this.OnHand.Name = "OnHand";
            this.OnHand.ReadOnly = true;
            this.OnHand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Quantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "0.####";
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Quantity, "Quantity");
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RetailPrice
            // 
            this.RetailPrice.DataPropertyName = "RetailPrice";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N";
            dataGridViewCellStyle4.NullValue = "0";
            this.RetailPrice.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.RetailPrice, "RetailPrice");
            this.RetailPrice.Name = "RetailPrice";
            this.RetailPrice.ReadOnly = true;
            // 
            // Currency
            // 
            this.Currency.DataPropertyName = "Currency";
            resources.ApplyResources(this.Currency, "Currency");
            this.Currency.Name = "Currency";
            // 
            // FactoryPrice
            // 
            this.FactoryPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.FactoryPrice.DataPropertyName = "InitialFactoryPrice";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N4";
            dataGridViewCellStyle5.NullValue = "0";
            this.FactoryPrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.FactoryPrice.FillWeight = 14.71503F;
            resources.ApplyResources(this.FactoryPrice, "FactoryPrice");
            this.FactoryPrice.Name = "FactoryPrice";
            this.FactoryPrice.ReadOnly = true;
            this.FactoryPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AlternateCostPrice
            // 
            this.AlternateCostPrice.DataPropertyName = "AlternateCostPrice";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N4";
            dataGridViewCellStyle6.NullValue = "0";
            this.AlternateCostPrice.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.AlternateCostPrice, "AlternateCostPrice");
            this.AlternateCostPrice.Name = "AlternateCostPrice";
            // 
            // Discount
            // 
            this.Discount.DataPropertyName = "Discount";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = "0";
            this.Discount.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.Discount, "Discount");
            this.Discount.Name = "Discount";
            // 
            // CalcFactoryPrice
            // 
            this.CalcFactoryPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CalcFactoryPrice.DataPropertyName = "FactoryPrice";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N4";
            dataGridViewCellStyle8.NullValue = "0";
            this.CalcFactoryPrice.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.CalcFactoryPrice, "CalcFactoryPrice");
            this.CalcFactoryPrice.Name = "CalcFactoryPrice";
            this.CalcFactoryPrice.ReadOnly = true;
            this.CalcFactoryPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TotalCostPrice
            // 
            this.TotalCostPrice.DataPropertyName = "TotalCostPrice";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N";
            dataGridViewCellStyle9.NullValue = "0";
            this.TotalCostPrice.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.TotalCostPrice, "TotalCostPrice");
            this.TotalCostPrice.Name = "TotalCostPrice";
            this.TotalCostPrice.ReadOnly = true;
            // 
            // ItemDesc
            // 
            this.ItemDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDesc.DataPropertyName = "ItemDesc";
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemDesc.DefaultCellStyle = dataGridViewCellStyle10;
            this.ItemDesc.FillWeight = 14.71503F;
            resources.ApplyResources(this.ItemDesc, "ItemDesc");
            this.ItemDesc.Name = "ItemDesc";
            this.ItemDesc.ReadOnly = true;
            this.ItemDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Remark.DataPropertyName = "Remark";
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Remark.DefaultCellStyle = dataGridViewCellStyle11;
            this.Remark.FillWeight = 14.71503F;
            resources.ApplyResources(this.Remark, "Remark");
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            this.Remark.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // InventoryDetRefNo
            // 
            this.InventoryDetRefNo.DataPropertyName = "InventoryDetRefNo";
            resources.ApplyResources(this.InventoryDetRefNo, "InventoryDetRefNo");
            this.InventoryDetRefNo.Name = "InventoryDetRefNo";
            this.InventoryDetRefNo.ReadOnly = true;
            this.InventoryDetRefNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SerialNo
            // 
            this.SerialNo.DataPropertyName = "SerialNo";
            resources.ApplyResources(this.SerialNo, "SerialNo");
            this.SerialNo.Name = "SerialNo";
            // 
            // pnlLoading
            // 
            this.pnlLoading.BackColor = System.Drawing.Color.Yellow;
            this.pnlLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoading.Controls.Add(this.label3);
            this.pnlLoading.Controls.Add(this.lblLoadingMessage);
            resources.ApplyResources(this.pnlLoading, "pnlLoading");
            this.pnlLoading.Name = "pnlLoading";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lblLoadingMessage
            // 
            resources.ApplyResources(this.lblLoadingMessage, "lblLoadingMessage");
            this.lblLoadingMessage.Name = "lblLoadingMessage";
            // 
            // saveFileDialogExport
            // 
            this.saveFileDialogExport.DefaultExt = "csv";
            resources.ApplyResources(this.saveFileDialogExport, "saveFileDialogExport");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.csv";
            this.openFileDialog1.FileName = "openFileDialog1";
            resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
            // 
            // bgSearch
            // 
            this.bgSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSearch_DoWork);
            this.bgSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgSearch_RunWorkerCompleted);
            this.bgSearch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // saveProduct
            // 
            this.saveProduct.DefaultExt = "csv";
            resources.ApplyResources(this.saveProduct, "saveProduct");
            // 
            // btnDownloadTemplate
            // 
            resources.ApplyResources(this.btnDownloadTemplate, "btnDownloadTemplate");
            this.btnDownloadTemplate.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnDownloadTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.UseVisualStyleBackColor = true;
            this.btnDownloadTemplate.Click += new System.EventHandler(this.btnDownloadTemplate_Click);
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSaveToDisk
            // 
            resources.ApplyResources(this.btnSaveToDisk, "btnSaveToDisk");
            this.btnSaveToDisk.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSaveToDisk.ForeColor = System.Drawing.Color.White;
            this.btnSaveToDisk.Name = "btnSaveToDisk";
            this.btnSaveToDisk.UseVisualStyleBackColor = true;
            this.btnSaveToDisk.Click += new System.EventHandler(this.btnSaveToDisk_Click);
            // 
            // btnDeleteChecked
            // 
            resources.ApplyResources(this.btnDeleteChecked, "btnDeleteChecked");
            this.btnDeleteChecked.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnDeleteChecked.ForeColor = System.Drawing.Color.White;
            this.btnDeleteChecked.Name = "btnDeleteChecked";
            this.btnDeleteChecked.TabStop = false;
            this.btnDeleteChecked.UseVisualStyleBackColor = true;
            this.btnDeleteChecked.Click += new System.EventHandler(this.btnDeleteChecked_Click);
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
            // btnSetting
            // 
            resources.ApplyResources(this.btnSetting, "btnSetting");
            this.btnSetting.BackgroundImage = global::PowerInventory.Properties.Resources.lightorange;
            this.btnSetting.ForeColor = System.Drawing.Color.Black;
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // frmInventoryParent
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.btnDownloadTemplate);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnSaveToDisk);
            this.Controls.Add(this.btnDeleteChecked);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbControl);
            this.Controls.Add(this.pnlLoading);
            this.Controls.Add(this.btnSetting);
            this.Name = "frmInventoryParent";
            this.Load += new System.EventHandler(this.frmInventoryParent_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInventoryParent_FormClosed);
            this.tbControl.ResumeLayout(false);
            this.tbHeader.ResumeLayout(false);
            this.tbHeader.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.tblInventory.ResumeLayout(false);
            this.tblInventory.PerformLayout();
            this.tbBody.ResumeLayout(false);
            this.tbBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.pnlLoading.ResumeLayout(false);
            this.pnlLoading.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tbHeader;
        private System.Windows.Forms.TabPage tbBody;
        protected System.Windows.Forms.TabControl tbControl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnScanItemNo;
        protected System.Windows.Forms.TextBox txtItemNoBarcode;
        protected System.Windows.Forms.TextBox txtRemark;
        protected System.Windows.Forms.DateTimePicker dtpInventoryDate;
        protected System.Windows.Forms.TextBox txtRefNo;
        protected System.Windows.Forms.ComboBox cmbLocation;
        protected System.Windows.Forms.Button btnSave;
        protected System.Windows.Forms.TableLayoutPanel tblInventory;
        protected System.Windows.Forms.Button btnDeleteChecked;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        protected System.Windows.Forms.DataGridView dgvStock;
        protected System.Windows.Forms.Button btnSaveToDisk;
        protected System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.LinkLabel llInvert;
        private System.Windows.Forms.LinkLabel llSelectNone;
        private System.Windows.Forms.LinkLabel llSelectAll;
        private System.Windows.Forms.Label lblLoadingMessage;
        protected System.Windows.Forms.Panel pnlLoading;
        protected System.Windows.Forms.Button btnExport;
        protected System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ProgressBar pgb1;
        protected System.ComponentModel.BackgroundWorker bgSearch;
        protected System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        protected System.Windows.Forms.Button btnDownloadTemplate;
        private System.Windows.Forms.SaveFileDialog saveProduct;
        protected System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Label lblTotalCostPriceAmount;
        private System.Windows.Forms.Label lblTotalCostPrice;
		private System.Windows.Forms.Button btnPage;
		private System.Windows.Forms.Button btnLastPage;
		private System.Windows.Forms.Button btnNextPage;
		private System.Windows.Forms.Button btnPreviousPage;
		private System.Windows.Forms.Button btnFirstPage;
        protected System.Windows.Forms.CheckBox cbDefaultQty;
        private System.Windows.Forms.Label label10;
        protected System.Windows.Forms.TextBox txtDefaultQty;
        private System.Windows.Forms.Label lblTotalItem;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblTotalQuantity;
        private System.Windows.Forms.Label label14;
        protected System.Windows.Forms.ComboBox cmbCriteria;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn OnHand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn RetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Currency;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactoryPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn AlternateCostPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CalcFactoryPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCostPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryDetRefNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNo;
    }
}