namespace PowerInventory.PurchaseOrder
{
    partial class frmPurchaseOrder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.OnHand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactoryPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryDetRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLoadingMessage = new System.Windows.Forms.Label();
            this.saveProduct = new System.Windows.Forms.SaveFileDialog();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.btnDownloadTemplate = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSaveToDisk = new System.Windows.Forms.Button();
            this.btnDeleteChecked = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
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
            this.tbControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbControl.Controls.Add(this.tbHeader);
            this.tbControl.Controls.Add(this.tbBody);
            this.tbControl.Location = new System.Drawing.Point(0, 4);
            this.tbControl.Margin = new System.Windows.Forms.Padding(4);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 0;
            this.tbControl.Size = new System.Drawing.Size(846, 540);
            this.tbControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tbControl.TabIndex = 111;
            // 
            // tbHeader
            // 
            this.tbHeader.BackColor = System.Drawing.Color.Gainsboro;
            this.tbHeader.Controls.Add(this.pnlProgress);
            this.tbHeader.Controls.Add(this.tblInventory);
            this.tbHeader.Location = new System.Drawing.Point(4, 25);
            this.tbHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.Size = new System.Drawing.Size(838, 511);
            this.tbHeader.TabIndex = 0;
            this.tbHeader.Text = "HEADER";
            // 
            // pnlProgress
            // 
            this.pnlProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.label9);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Location = new System.Drawing.Point(319, 201);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(200, 100);
            this.pnlProgress.TabIndex = 70;
            this.pnlProgress.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(55, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "Please Wait...";
            // 
            // pgb1
            // 
            this.pgb1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pgb1.Location = new System.Drawing.Point(25, 42);
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Size = new System.Drawing.Size(159, 23);
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgb1.TabIndex = 0;
            // 
            // tblInventory
            // 
            this.tblInventory.AutoSize = true;
            this.tblInventory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblInventory.BackColor = System.Drawing.Color.Gainsboro;
            this.tblInventory.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tblInventory.ColumnCount = 5;
            this.tblInventory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tblInventory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tblInventory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tblInventory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tblInventory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
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
            this.tblInventory.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblInventory.Location = new System.Drawing.Point(0, 0);
            this.tblInventory.Margin = new System.Windows.Forms.Padding(0);
            this.tblInventory.Name = "tblInventory";
            this.tblInventory.RowCount = 4;
            this.tblInventory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tblInventory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tblInventory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tblInventory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tblInventory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblInventory.Size = new System.Drawing.Size(838, 178);
            this.tblInventory.TabIndex = 0;
            // 
            // txtRemark
            // 
            this.txtRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemark.Location = new System.Drawing.Point(653, 53);
            this.txtRemark.Margin = new System.Windows.Forms.Padding(0);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(184, 98);
            this.txtRemark.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Navy;
            this.tblInventory.SetColumnSpan(this.label2, 5);
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(1, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(836, 25);
            this.label2.TabIndex = 42;
            this.label2.Text = "Basic Information";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Gainsboro;
            this.label7.Location = new System.Drawing.Point(469, 27);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(183, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "Date";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Gainsboro;
            this.label4.Location = new System.Drawing.Point(1, 27);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ref No";
            // 
            // txtRefNo
            // 
            this.txtRefNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRefNo.Location = new System.Drawing.Point(185, 27);
            this.txtRefNo.Margin = new System.Windows.Forms.Padding(0);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.ReadOnly = true;
            this.txtRefNo.Size = new System.Drawing.Size(183, 22);
            this.txtRefNo.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Navy;
            this.tblInventory.SetColumnSpan(this.label6, 5);
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(1, 152);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(836, 25);
            this.label6.TabIndex = 41;
            this.label6.Text = "Additional Information";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpInventoryDate
            // 
            this.dtpInventoryDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpInventoryDate.CustomFormat = "dd MMMM yyyy HH:mm";
            this.dtpInventoryDate.Enabled = false;
            this.dtpInventoryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInventoryDate.Location = new System.Drawing.Point(656, 30);
            this.dtpInventoryDate.Name = "dtpInventoryDate";
            this.dtpInventoryDate.Size = new System.Drawing.Size(178, 22);
            this.dtpInventoryDate.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Gainsboro;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(469, 53);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(183, 98);
            this.label5.TabIndex = 39;
            this.label5.Text = "Remark";
            // 
            // cmbLocation
            // 
            this.cmbLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(185, 53);
            this.cmbLocation.Margin = new System.Windows.Forms.Padding(0);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(183, 24);
            this.cmbLocation.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(1, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 98);
            this.label1.TabIndex = 0;
            this.label1.Text = "Location";
            // 
            // tbBody
            // 
            this.tbBody.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgnd;
            this.tbBody.Controls.Add(this.llInvert);
            this.tbBody.Controls.Add(this.llSelectNone);
            this.tbBody.Controls.Add(this.llSelectAll);
            this.tbBody.Controls.Add(this.btnAddItem);
            this.tbBody.Controls.Add(this.btnScanItemNo);
            this.tbBody.Controls.Add(this.txtItemNoBarcode);
            this.tbBody.Controls.Add(this.label8);
            this.tbBody.Controls.Add(this.dgvStock);
            this.tbBody.Location = new System.Drawing.Point(4, 25);
            this.tbBody.Margin = new System.Windows.Forms.Padding(0);
            this.tbBody.Name = "tbBody";
            this.tbBody.Size = new System.Drawing.Size(838, 511);
            this.tbBody.TabIndex = 1;
            this.tbBody.Text = "DETAILS";
            this.tbBody.UseVisualStyleBackColor = true;
            // 
            // llInvert
            // 
            this.llInvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llInvert.AutoSize = true;
            this.llInvert.BackColor = System.Drawing.Color.Transparent;
            this.llInvert.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.llInvert.Location = new System.Drawing.Point(793, 18);
            this.llInvert.Name = "llInvert";
            this.llInvert.Size = new System.Drawing.Size(40, 16);
            this.llInvert.TabIndex = 107;
            this.llInvert.TabStop = true;
            this.llInvert.Text = "Invert";
            this.llInvert.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // llSelectNone
            // 
            this.llSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSelectNone.AutoSize = true;
            this.llSelectNone.BackColor = System.Drawing.Color.Transparent;
            this.llSelectNone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.llSelectNone.Location = new System.Drawing.Point(712, 18);
            this.llSelectNone.Name = "llSelectNone";
            this.llSelectNone.Size = new System.Drawing.Size(82, 16);
            this.llSelectNone.TabIndex = 106;
            this.llSelectNone.TabStop = true;
            this.llSelectNone.Text = "Select None";
            this.llSelectNone.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // llSelectAll
            // 
            this.llSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSelectAll.AutoSize = true;
            this.llSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.llSelectAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.llSelectAll.Location = new System.Drawing.Point(641, 18);
            this.llSelectAll.Name = "llSelectAll";
            this.llSelectAll.Size = new System.Drawing.Size(64, 16);
            this.llSelectAll.TabIndex = 105;
            this.llSelectAll.TabStop = true;
            this.llSelectAll.Text = "Select All";
            this.llSelectAll.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItem.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddItem.ForeColor = System.Drawing.Color.White;
            this.btnAddItem.Location = new System.Drawing.Point(344, -1);
            this.btnAddItem.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(89, 41);
            this.btnAddItem.TabIndex = 104;
            this.btnAddItem.Text = "NEW ITEM";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Visible = false;
            // 
            // btnScanItemNo
            // 
            this.btnScanItemNo.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnScanItemNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanItemNo.ForeColor = System.Drawing.Color.White;
            this.btnScanItemNo.Location = new System.Drawing.Point(275, 4);
            this.btnScanItemNo.Margin = new System.Windows.Forms.Padding(4);
            this.btnScanItemNo.Name = "btnScanItemNo";
            this.btnScanItemNo.Size = new System.Drawing.Size(52, 30);
            this.btnScanItemNo.TabIndex = 102;
            this.btnScanItemNo.Text = "OK";
            this.btnScanItemNo.UseVisualStyleBackColor = true;
            // 
            // txtItemNoBarcode
            // 
            this.txtItemNoBarcode.Location = new System.Drawing.Point(81, 7);
            this.txtItemNoBarcode.Name = "txtItemNoBarcode";
            this.txtItemNoBarcode.Size = new System.Drawing.Size(187, 22);
            this.txtItemNoBarcode.TabIndex = 101;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 16);
            this.label8.TabIndex = 27;
            this.label8.Text = "SEARCH";
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            this.dgvStock.AllowUserToResizeRows = false;
            this.dgvStock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvStock.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SN,
            this.ItemNo,
            this.ItemName,
            this.OnHand,
            this.Quantity,
            this.FactoryPrice,
            this.ItemDesc,
            this.Remark,
            this.InventoryDetRefNo});
            this.dgvStock.Location = new System.Drawing.Point(3, 41);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.Size = new System.Drawing.Size(832, 473);
            this.dgvStock.TabIndex = 26;
            this.dgvStock.TabStop = false;
            // 
            // SN
            // 
            this.SN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SN.DataPropertyName = "Deleted";
            this.SN.FalseValue = "false";
            this.SN.HeaderText = "";
            this.SN.IndeterminateValue = "false";
            this.SN.Name = "SN";
            this.SN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SN.TrueValue = "true";
            this.SN.Width = 45;
            // 
            // ItemNo
            // 
            this.ItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.FillWeight = 1F;
            this.ItemNo.HeaderText = "Item No";
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            this.ItemNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ItemNo.Width = 120;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ItemName.FillWeight = 14.71503F;
            this.ItemName.HeaderText = "Item Name";
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            this.ItemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // OnHand
            // 
            this.OnHand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OnHand.DataPropertyName = "OnHand";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.OnHand.DefaultCellStyle = dataGridViewCellStyle2;
            this.OnHand.FillWeight = 1F;
            this.OnHand.HeaderText = "Bal Qty";
            this.OnHand.Name = "OnHand";
            this.OnHand.ReadOnly = true;
            this.OnHand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.OnHand.Width = 80;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Quantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle3;
            this.Quantity.HeaderText = "Qty";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FactoryPrice
            // 
            this.FactoryPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.FactoryPrice.DataPropertyName = "FactoryPrice";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = "0";
            this.FactoryPrice.DefaultCellStyle = dataGridViewCellStyle4;
            this.FactoryPrice.FillWeight = 14.71503F;
            this.FactoryPrice.HeaderText = "Cost Price";
            this.FactoryPrice.Name = "FactoryPrice";
            this.FactoryPrice.ReadOnly = true;
            this.FactoryPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ItemDesc
            // 
            this.ItemDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDesc.DataPropertyName = "ItemDesc";
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemDesc.DefaultCellStyle = dataGridViewCellStyle5;
            this.ItemDesc.FillWeight = 14.71503F;
            this.ItemDesc.HeaderText = "Description";
            this.ItemDesc.Name = "ItemDesc";
            this.ItemDesc.ReadOnly = true;
            this.ItemDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Remark.DataPropertyName = "Remark";
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Remark.DefaultCellStyle = dataGridViewCellStyle6;
            this.Remark.FillWeight = 14.71503F;
            this.Remark.HeaderText = "Remark";
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            this.Remark.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // InventoryDetRefNo
            // 
            this.InventoryDetRefNo.DataPropertyName = "InventoryDetRefNo";
            this.InventoryDetRefNo.HeaderText = "InventoryDetRefNo";
            this.InventoryDetRefNo.Name = "InventoryDetRefNo";
            this.InventoryDetRefNo.ReadOnly = true;
            this.InventoryDetRefNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.InventoryDetRefNo.Visible = false;
            this.InventoryDetRefNo.Width = 128;
            // 
            // pnlLoading
            // 
            this.pnlLoading.BackColor = System.Drawing.Color.Yellow;
            this.pnlLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoading.Controls.Add(this.label3);
            this.pnlLoading.Controls.Add(this.lblLoadingMessage);
            this.pnlLoading.Location = new System.Drawing.Point(275, 168);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(331, 118);
            this.pnlLoading.TabIndex = 119;
            this.pnlLoading.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(214, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "This may take a few minutes...";
            // 
            // lblLoadingMessage
            // 
            this.lblLoadingMessage.AutoSize = true;
            this.lblLoadingMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadingMessage.Location = new System.Drawing.Point(51, 42);
            this.lblLoadingMessage.Name = "lblLoadingMessage";
            this.lblLoadingMessage.Size = new System.Drawing.Size(232, 31);
            this.lblLoadingMessage.TabIndex = 0;
            this.lblLoadingMessage.Text = "PLEASE WAIT...";
            // 
            // saveProduct
            // 
            this.saveProduct.DefaultExt = "csv";
            this.saveProduct.Filter = "Comma Delimited|*.csv";
            this.saveProduct.Title = "Save Export File";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.csv";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Excel Files|*.csv;*xls|Data Collector|*.txt";
            // 
            // saveFileDialogExport
            // 
            this.saveFileDialogExport.DefaultExt = "csv";
            this.saveFileDialogExport.Filter = "Comma Delimited|*.csv";
            this.saveFileDialogExport.Title = "Save Export File";
            // 
            // btnDownloadTemplate
            // 
            this.btnDownloadTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDownloadTemplate.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnDownloadTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadTemplate.Location = new System.Drawing.Point(473, 552);
            this.btnDownloadTemplate.Margin = new System.Windows.Forms.Padding(1);
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.Size = new System.Drawing.Size(93, 43);
            this.btnDownloadTemplate.TabIndex = 120;
            this.btnDownloadTemplate.Text = "DOWNLOAD TEMPLATE";
            this.btnDownloadTemplate.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Location = new System.Drawing.Point(391, 552);
            this.btnImport.Margin = new System.Windows.Forms.Padding(1);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 43);
            this.btnImport.TabIndex = 118;
            this.btnImport.Text = "IMPORT";
            this.btnImport.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(309, 552);
            this.btnExport.Margin = new System.Windows.Forms.Padding(1);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 43);
            this.btnExport.TabIndex = 117;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(145, 552);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 43);
            this.btnPrint.TabIndex = 116;
            this.btnPrint.Text = "PRINT";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnSaveToDisk
            // 
            this.btnSaveToDisk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveToDisk.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSaveToDisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveToDisk.ForeColor = System.Drawing.Color.White;
            this.btnSaveToDisk.Location = new System.Drawing.Point(227, 552);
            this.btnSaveToDisk.Margin = new System.Windows.Forms.Padding(1);
            this.btnSaveToDisk.Name = "btnSaveToDisk";
            this.btnSaveToDisk.Size = new System.Drawing.Size(80, 43);
            this.btnSaveToDisk.TabIndex = 115;
            this.btnSaveToDisk.Text = "SAVE";
            this.btnSaveToDisk.UseVisualStyleBackColor = true;
            // 
            // btnDeleteChecked
            // 
            this.btnDeleteChecked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteChecked.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnDeleteChecked.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteChecked.ForeColor = System.Drawing.Color.White;
            this.btnDeleteChecked.Location = new System.Drawing.Point(4, 552);
            this.btnDeleteChecked.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteChecked.Name = "btnDeleteChecked";
            this.btnDeleteChecked.Size = new System.Drawing.Size(133, 43);
            this.btnDeleteChecked.TabIndex = 112;
            this.btnDeleteChecked.TabStop = false;
            this.btnDeleteChecked.Text = "DELETE CHECKED";
            this.btnDeleteChecked.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(568, 552);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(133, 43);
            this.btnCancel.TabIndex = 113;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSetting
            // 
            this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetting.BackgroundImage = global::PowerInventory.Properties.Resources.lightorange;
            this.btnSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetting.ForeColor = System.Drawing.Color.Black;
            this.btnSetting.Location = new System.Drawing.Point(227, 552);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(1);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(80, 43);
            this.btnSetting.TabIndex = 121;
            this.btnSetting.Text = "SETTINGS";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(709, 552);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(133, 43);
            this.btnSave.TabIndex = 114;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // frmPurchaseOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 599);
            this.Controls.Add(this.tbControl);
            this.Controls.Add(this.pnlLoading);
            this.Controls.Add(this.btnDownloadTemplate);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnSaveToDisk);
            this.Controls.Add(this.btnDeleteChecked);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPurchaseOrder";
            this.Text = "Purchase Order";
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

        private System.Windows.Forms.LinkLabel llInvert;
        private System.Windows.Forms.LinkLabel llSelectAll;
        private System.Windows.Forms.LinkLabel llSelectNone;
        protected System.Windows.Forms.TabControl tbControl;
        private System.Windows.Forms.TabPage tbHeader;
        protected System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ProgressBar pgb1;
        protected System.Windows.Forms.TableLayoutPanel tblInventory;
        protected System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        protected System.Windows.Forms.TextBox txtRefNo;
        private System.Windows.Forms.Label label6;
        protected System.Windows.Forms.DateTimePicker dtpInventoryDate;
        private System.Windows.Forms.Label label5;
        protected System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tbBody;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnScanItemNo;
        private System.Windows.Forms.TextBox txtItemNoBarcode;
        private System.Windows.Forms.Label label8;
        protected System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OnHand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactoryPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryDetRefNo;
        protected System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLoadingMessage;
        protected System.Windows.Forms.Button btnDownloadTemplate;
        private System.Windows.Forms.SaveFileDialog saveProduct;
        protected System.ComponentModel.BackgroundWorker bgSearch;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        protected System.Windows.Forms.Button btnImport;
        protected System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
        protected System.Windows.Forms.Button btnPrint;
        protected System.Windows.Forms.Button btnSaveToDisk;
        protected System.Windows.Forms.Button btnDeleteChecked;
        private System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Button btnSetting;
        protected System.Windows.Forms.Button btnSave;
    }
}