namespace PowerInventory.ItemForms
{
    partial class frmItemMst
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnNewCategory = new System.Windows.Forms.Button();
            this.lblAttributes5 = new System.Windows.Forms.Label();
            this.txtAttributes5 = new System.Windows.Forms.TextBox();
            this.lblAttributes3 = new System.Windows.Forms.Label();
            this.txtAttributes4 = new System.Windows.Forms.TextBox();
            this.lblAttributes4 = new System.Windows.Forms.Label();
            this.txtAttributes3 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAttributes2 = new System.Windows.Forms.TextBox();
            this.lblAttributes2 = new System.Windows.Forms.Label();
            this.txtAttributes1 = new System.Windows.Forms.TextBox();
            this.lblAttributes1 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.txtItemDesc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCategoryName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.btnNewItem = new System.Windows.Forms.Button();
            this.btnDeleteChecked = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.llInvert = new System.Windows.Forms.LinkLabel();
            this.llSelectNone = new System.Windows.Forms.LinkLabel();
            this.llSelectAll = new System.Windows.Forms.LinkLabel();
            this.btnEdit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.view = new System.Windows.Forms.DataGridViewButtonColumn();
            this.itemno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(704, 159);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 40);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(250, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(78, 40);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.Transparent;
            this.groupBox9.Controls.Add(this.btnNewCategory);
            this.groupBox9.Controls.Add(this.btnClose);
            this.groupBox9.Controls.Add(this.lblAttributes5);
            this.groupBox9.Controls.Add(this.txtAttributes5);
            this.groupBox9.Controls.Add(this.lblAttributes3);
            this.groupBox9.Controls.Add(this.txtAttributes4);
            this.groupBox9.Controls.Add(this.lblAttributes4);
            this.groupBox9.Controls.Add(this.txtAttributes3);
            this.groupBox9.Controls.Add(this.label8);
            this.groupBox9.Controls.Add(this.txtAttributes2);
            this.groupBox9.Controls.Add(this.lblAttributes2);
            this.groupBox9.Controls.Add(this.txtAttributes1);
            this.groupBox9.Controls.Add(this.lblAttributes1);
            this.groupBox9.Controls.Add(this.txtItemName);
            this.groupBox9.Controls.Add(this.txtItemDesc);
            this.groupBox9.Controls.Add(this.label1);
            this.groupBox9.Controls.Add(this.cmbCategoryName);
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.label11);
            this.groupBox9.Controls.Add(this.txtBarcode);
            this.groupBox9.Controls.Add(this.label15);
            this.groupBox9.Controls.Add(this.txtRefNo);
            this.groupBox9.Controls.Add(this.label16);
            this.groupBox9.Location = new System.Drawing.Point(899, 6);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(792, 205);
            this.groupBox9.TabIndex = 36;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "FILTER";
            this.groupBox9.Visible = false;
            // 
            // btnNewCategory
            // 
            this.btnNewCategory.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnNewCategory.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCategory.ForeColor = System.Drawing.Color.White;
            this.btnNewCategory.Location = new System.Drawing.Point(540, 67);
            this.btnNewCategory.Name = "btnNewCategory";
            this.btnNewCategory.Size = new System.Drawing.Size(138, 40);
            this.btnNewCategory.TabIndex = 43;
            this.btnNewCategory.Text = "New Category";
            this.btnNewCategory.UseVisualStyleBackColor = true;
            this.btnNewCategory.Click += new System.EventHandler(this.btnNewCategory_Click);
            // 
            // lblAttributes5
            // 
            this.lblAttributes5.AutoSize = true;
            this.lblAttributes5.BackColor = System.Drawing.Color.Transparent;
            this.lblAttributes5.Location = new System.Drawing.Point(6, 177);
            this.lblAttributes5.Name = "lblAttributes5";
            this.lblAttributes5.Size = new System.Drawing.Size(75, 15);
            this.lblAttributes5.TabIndex = 43;
            this.lblAttributes5.Text = "Attributes5";
            // 
            // txtAttributes5
            // 
            this.txtAttributes5.Location = new System.Drawing.Point(257, 174);
            this.txtAttributes5.Name = "txtAttributes5";
            this.txtAttributes5.Size = new System.Drawing.Size(218, 21);
            this.txtAttributes5.TabIndex = 10;
            this.txtAttributes5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // lblAttributes3
            // 
            this.lblAttributes3.AutoSize = true;
            this.lblAttributes3.BackColor = System.Drawing.Color.Transparent;
            this.lblAttributes3.Location = new System.Drawing.Point(6, 123);
            this.lblAttributes3.Name = "lblAttributes3";
            this.lblAttributes3.Size = new System.Drawing.Size(75, 15);
            this.lblAttributes3.TabIndex = 41;
            this.lblAttributes3.Text = "Attributes3";
            // 
            // txtAttributes4
            // 
            this.txtAttributes4.Location = new System.Drawing.Point(257, 147);
            this.txtAttributes4.Name = "txtAttributes4";
            this.txtAttributes4.Size = new System.Drawing.Size(218, 21);
            this.txtAttributes4.TabIndex = 7;
            this.txtAttributes4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // lblAttributes4
            // 
            this.lblAttributes4.AutoSize = true;
            this.lblAttributes4.BackColor = System.Drawing.Color.Transparent;
            this.lblAttributes4.Location = new System.Drawing.Point(6, 149);
            this.lblAttributes4.Name = "lblAttributes4";
            this.lblAttributes4.Size = new System.Drawing.Size(75, 15);
            this.lblAttributes4.TabIndex = 39;
            this.lblAttributes4.Text = "Attributes4";
            // 
            // txtAttributes3
            // 
            this.txtAttributes3.Location = new System.Drawing.Point(257, 120);
            this.txtAttributes3.Name = "txtAttributes3";
            this.txtAttributes3.Size = new System.Drawing.Size(218, 21);
            this.txtAttributes3.TabIndex = 6;
            this.txtAttributes3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(254, 83);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 15);
            this.label8.TabIndex = 37;
            // 
            // txtAttributes2
            // 
            this.txtAttributes2.Location = new System.Drawing.Point(257, 94);
            this.txtAttributes2.Name = "txtAttributes2";
            this.txtAttributes2.Size = new System.Drawing.Size(218, 21);
            this.txtAttributes2.TabIndex = 3;
            this.txtAttributes2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // lblAttributes2
            // 
            this.lblAttributes2.AutoSize = true;
            this.lblAttributes2.BackColor = System.Drawing.Color.Transparent;
            this.lblAttributes2.Location = new System.Drawing.Point(6, 96);
            this.lblAttributes2.Name = "lblAttributes2";
            this.lblAttributes2.Size = new System.Drawing.Size(75, 15);
            this.lblAttributes2.TabIndex = 35;
            this.lblAttributes2.Text = "Attributes2";
            // 
            // txtAttributes1
            // 
            this.txtAttributes1.Location = new System.Drawing.Point(257, 67);
            this.txtAttributes1.Name = "txtAttributes1";
            this.txtAttributes1.Size = new System.Drawing.Size(218, 21);
            this.txtAttributes1.TabIndex = 2;
            this.txtAttributes1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // lblAttributes1
            // 
            this.lblAttributes1.AutoSize = true;
            this.lblAttributes1.BackColor = System.Drawing.Color.Transparent;
            this.lblAttributes1.Location = new System.Drawing.Point(6, 70);
            this.lblAttributes1.Name = "lblAttributes1";
            this.lblAttributes1.Size = new System.Drawing.Size(75, 15);
            this.lblAttributes1.TabIndex = 33;
            this.lblAttributes1.Text = "Attributes1";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(341, 13);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(180, 21);
            this.txtItemName.TabIndex = 4;
            this.txtItemName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // txtItemDesc
            // 
            this.txtItemDesc.Location = new System.Drawing.Point(633, 15);
            this.txtItemDesc.Name = "txtItemDesc";
            this.txtItemDesc.Size = new System.Drawing.Size(151, 21);
            this.txtItemDesc.TabIndex = 9;
            this.txtItemDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(537, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 15);
            this.label1.TabIndex = 26;
            this.label1.Text = "Description";
            // 
            // cmbCategoryName
            // 
            this.cmbCategoryName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryName.FormattingEnabled = true;
            this.cmbCategoryName.Location = new System.Drawing.Point(340, 39);
            this.cmbCategoryName.Name = "cmbCategoryName";
            this.cmbCategoryName.Size = new System.Drawing.Size(181, 23);
            this.cmbCategoryName.TabIndex = 5;
            this.cmbCategoryName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(254, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "Category";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(254, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 15);
            this.label11.TabIndex = 19;
            this.label11.Text = "Item Name";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(73, 39);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(175, 21);
            this.txtBarcode.TabIndex = 1;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(6, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 15);
            this.label15.TabIndex = 17;
            this.label15.Text = "Barcode:";
            // 
            // txtRefNo
            // 
            this.txtRefNo.Location = new System.Drawing.Point(73, 12);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Size = new System.Drawing.Size(175, 21);
            this.txtRefNo.TabIndex = 0;
            this.txtRefNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(6, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(61, 15);
            this.label16.TabIndex = 15;
            this.label16.Text = "Item No:";
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnEdit,
            this.view,
            this.itemno,
            this.barcode,
            this.itemname,
            this.categoryname,
            this.price,
            this.ItemDesc,
            this.Attributes1,
            this.Attributes2,
            this.Attributes3,
            this.Attributes4,
            this.Attributes5});
            this.dgvItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvItems.Location = new System.Drawing.Point(2, 103);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowHeadersWidth = 23;
            this.dgvItems.Size = new System.Drawing.Size(915, 563);
            this.dgvItems.TabIndex = 34;
            this.dgvItems.TabStop = false;
            this.dgvItems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellClick);
            this.dgvItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvItems_KeyDown);
            // 
            // btnNewItem
            // 
            this.btnNewItem.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnNewItem.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewItem.ForeColor = System.Drawing.Color.White;
            this.btnNewItem.Location = new System.Drawing.Point(146, 55);
            this.btnNewItem.Name = "btnNewItem";
            this.btnNewItem.Size = new System.Drawing.Size(138, 40);
            this.btnNewItem.TabIndex = 42;
            this.btnNewItem.Text = "New Item";
            this.btnNewItem.UseVisualStyleBackColor = true;
            this.btnNewItem.Click += new System.EventHandler(this.btnNewItem_Click);
            // 
            // btnDeleteChecked
            // 
            this.btnDeleteChecked.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnDeleteChecked.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteChecked.ForeColor = System.Drawing.Color.White;
            this.btnDeleteChecked.Location = new System.Drawing.Point(2, 55);
            this.btnDeleteChecked.Name = "btnDeleteChecked";
            this.btnDeleteChecked.Size = new System.Drawing.Size(138, 40);
            this.btnDeleteChecked.TabIndex = 41;
            this.btnDeleteChecked.Text = "Delete Checked";
            this.btnDeleteChecked.UseVisualStyleBackColor = true;
            this.btnDeleteChecked.Click += new System.EventHandler(this.btnDeleteChecked_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(290, 55);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(138, 40);
            this.btnExport.TabIndex = 40;
            this.btnExport.Text = "Export To Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            this.fsdExportToExcel.Title = "Save File";
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(232, 21);
            this.txtSearch.TabIndex = 43;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // bgSearch
            // 
            this.bgSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSearch_DoWork);
            this.bgSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgSearch_RunWorkerCompleted);
            this.bgSearch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // pnlProgress
            // 
            this.pnlProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Location = new System.Drawing.Point(338, 262);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(200, 100);
            this.pnlProgress.TabIndex = 44;
            this.pnlProgress.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(55, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Please Wait...";
            // 
            // pgb1
            // 
            this.pgb1.Location = new System.Drawing.Point(25, 42);
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Size = new System.Drawing.Size(159, 23);
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgb1.TabIndex = 0;
            // 
            // llInvert
            // 
            this.llInvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llInvert.AutoSize = true;
            this.llInvert.BackColor = System.Drawing.Color.Transparent;
            this.llInvert.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.llInvert.Location = new System.Drawing.Point(851, 80);
            this.llInvert.Name = "llInvert";
            this.llInvert.Size = new System.Drawing.Size(42, 15);
            this.llInvert.TabIndex = 110;
            this.llInvert.TabStop = true;
            this.llInvert.Text = "Invert";
            this.llInvert.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llInvert_LinkClicked);
            // 
            // llSelectNone
            // 
            this.llSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSelectNone.AutoSize = true;
            this.llSelectNone.BackColor = System.Drawing.Color.Transparent;
            this.llSelectNone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.llSelectNone.Location = new System.Drawing.Point(750, 80);
            this.llSelectNone.Name = "llSelectNone";
            this.llSelectNone.Size = new System.Drawing.Size(85, 15);
            this.llSelectNone.TabIndex = 109;
            this.llSelectNone.TabStop = true;
            this.llSelectNone.Text = "Select None";
            this.llSelectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectNone_LinkClicked);
            // 
            // llSelectAll
            // 
            this.llSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSelectAll.AutoSize = true;
            this.llSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.llSelectAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.llSelectAll.Location = new System.Drawing.Point(665, 79);
            this.llSelectAll.Name = "llSelectAll";
            this.llSelectAll.Size = new System.Drawing.Size(67, 15);
            this.llSelectAll.TabIndex = 108;
            this.llSelectAll.TabStop = true;
            this.llSelectAll.Text = "Select All";
            this.llSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectAll_LinkClicked);
            // 
            // btnEdit
            // 
            this.btnEdit.HeaderText = "";
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnEdit.Width = 60;
            // 
            // view
            // 
            this.view.HeaderText = "";
            this.view.Name = "view";
            this.view.Text = "View";
            this.view.UseColumnTextForButtonValue = true;
            this.view.Width = 60;
            // 
            // itemno
            // 
            this.itemno.DataPropertyName = "itemno";
            this.itemno.HeaderText = "Item No";
            this.itemno.MinimumWidth = 120;
            this.itemno.Name = "itemno";
            this.itemno.ReadOnly = true;
            this.itemno.Width = 120;
            // 
            // barcode
            // 
            this.barcode.DataPropertyName = "barcode";
            this.barcode.HeaderText = "Barcode";
            this.barcode.Name = "barcode";
            this.barcode.ReadOnly = true;
            this.barcode.Visible = false;
            // 
            // itemname
            // 
            this.itemname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.itemname.DataPropertyName = "itemname";
            this.itemname.HeaderText = "Item Name";
            this.itemname.MinimumWidth = 200;
            this.itemname.Name = "itemname";
            this.itemname.ReadOnly = true;
            // 
            // categoryname
            // 
            this.categoryname.DataPropertyName = "categoryname";
            this.categoryname.HeaderText = "Category";
            this.categoryname.Name = "categoryname";
            this.categoryname.ReadOnly = true;
            this.categoryname.Visible = false;
            this.categoryname.Width = 140;
            // 
            // price
            // 
            this.price.DataPropertyName = "retailprice";
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.price.DefaultCellStyle = dataGridViewCellStyle1;
            this.price.HeaderText = "R.Price";
            this.price.Name = "price";
            this.price.ReadOnly = true;
            this.price.Visible = false;
            this.price.Width = 80;
            // 
            // ItemDesc
            // 
            this.ItemDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDesc.DataPropertyName = "ItemDesc";
            this.ItemDesc.HeaderText = "Desc";
            this.ItemDesc.MinimumWidth = 200;
            this.ItemDesc.Name = "ItemDesc";
            // 
            // Attributes1
            // 
            this.Attributes1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Attributes1.DataPropertyName = "Attributes1";
            this.Attributes1.HeaderText = "Attributes1";
            this.Attributes1.Name = "Attributes1";
            this.Attributes1.ReadOnly = true;
            // 
            // Attributes2
            // 
            this.Attributes2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Attributes2.DataPropertyName = "Attributes2";
            this.Attributes2.HeaderText = "Attributes2";
            this.Attributes2.Name = "Attributes2";
            this.Attributes2.ReadOnly = true;
            // 
            // Attributes3
            // 
            this.Attributes3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Attributes3.DataPropertyName = "Attributes3";
            this.Attributes3.HeaderText = "Attributes3";
            this.Attributes3.Name = "Attributes3";
            this.Attributes3.ReadOnly = true;
            // 
            // Attributes4
            // 
            this.Attributes4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Attributes4.DataPropertyName = "Attributes4";
            this.Attributes4.HeaderText = "Attributes4";
            this.Attributes4.Name = "Attributes4";
            this.Attributes4.ReadOnly = true;
            // 
            // Attributes5
            // 
            this.Attributes5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Attributes5.DataPropertyName = "Attributes5";
            this.Attributes5.HeaderText = "Attributes5";
            this.Attributes5.Name = "Attributes5";
            this.Attributes5.ReadOnly = true;
            // 
            // frmItemMst
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(921, 678);
            this.Controls.Add(this.llInvert);
            this.Controls.Add(this.llSelectNone);
            this.Controls.Add(this.llSelectAll);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnNewItem);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnDeleteChecked);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.groupBox9);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(1, 1);
            this.Name = "frmItemMst";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Products";
            this.Load += new System.EventHandler(this.frmItemMst_Load);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.ComboBox cmbCategoryName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRefNo;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.TextBox txtItemDesc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.TextBox txtAttributes4;
        private System.Windows.Forms.Label lblAttributes4;
        private System.Windows.Forms.TextBox txtAttributes3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAttributes2;
        private System.Windows.Forms.Label lblAttributes2;
        private System.Windows.Forms.TextBox txtAttributes1;
        private System.Windows.Forms.Label lblAttributes1;
        private System.Windows.Forms.Label lblAttributes5;
        private System.Windows.Forms.TextBox txtAttributes5;
        private System.Windows.Forms.Label lblAttributes3;
        internal System.Windows.Forms.Button btnNewItem;
        internal System.Windows.Forms.Button btnDeleteChecked;
        internal System.Windows.Forms.Button btnExport;
        internal System.Windows.Forms.Button btnNewCategory;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.ComponentModel.BackgroundWorker bgSearch;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.ProgressBar pgb1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel llInvert;
        private System.Windows.Forms.LinkLabel llSelectNone;
        private System.Windows.Forms.LinkLabel llSelectAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn btnEdit;
        private System.Windows.Forms.DataGridViewButtonColumn view;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemno;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemname;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryname;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes5;
    }
}