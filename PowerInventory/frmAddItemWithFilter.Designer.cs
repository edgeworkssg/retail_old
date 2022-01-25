namespace PowerInventory
{
    partial class frmAddItemWithFilter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlAddItems = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pnlAttributes = new System.Windows.Forms.Panel();
            this.btnAttrCancel = new System.Windows.Forms.Button();
            this.btnAttrOK = new System.Windows.Forms.Button();
            this.clbAttributes = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkShowSales = new System.Windows.Forms.CheckBox();
            this.txtNumOfDays = new System.Windows.Forms.TextBox();
            this.btnShowAttr = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblSupplierName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancelAdd = new System.Windows.Forms.Button();
            this.dgvItemList = new System.Windows.Forms.DataGridView();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOnHand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSales1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSales2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSales3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConvRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlAddItems.SuspendLayout();
            this.pnlAttributes.SuspendLayout();
            this.pnlLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlAddItems
            // 
            this.pnlAddItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAddItems.BackColor = System.Drawing.Color.White;
            this.pnlAddItems.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.pnlAddItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAddItems.Controls.Add(this.btnExport);
            this.pnlAddItems.Controls.Add(this.btnSearch);
            this.pnlAddItems.Controls.Add(this.pnlAttributes);
            this.pnlAddItems.Controls.Add(this.label4);
            this.pnlAddItems.Controls.Add(this.chkShowSales);
            this.pnlAddItems.Controls.Add(this.txtNumOfDays);
            this.pnlAddItems.Controls.Add(this.btnShowAttr);
            this.pnlAddItems.Controls.Add(this.txtFilter);
            this.pnlAddItems.Controls.Add(this.lblSupplierName);
            this.pnlAddItems.Controls.Add(this.label2);
            this.pnlAddItems.Controls.Add(this.label1);
            this.pnlAddItems.Controls.Add(this.pnlLoading);
            this.pnlAddItems.Controls.Add(this.btnOK);
            this.pnlAddItems.Controls.Add(this.btnCancelAdd);
            this.pnlAddItems.Controls.Add(this.dgvItemList);
            this.pnlAddItems.Location = new System.Drawing.Point(1, 1);
            this.pnlAddItems.Name = "pnlAddItems";
            this.pnlAddItems.Size = new System.Drawing.Size(786, 560);
            this.pnlAddItems.TabIndex = 17;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(14, 507);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(112, 41);
            this.btnExport.TabIndex = 122;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(289, 25);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 30);
            this.btnSearch.TabIndex = 121;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pnlAttributes
            // 
            this.pnlAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAttributes.Controls.Add(this.btnAttrCancel);
            this.pnlAttributes.Controls.Add(this.btnAttrOK);
            this.pnlAttributes.Controls.Add(this.clbAttributes);
            this.pnlAttributes.Location = new System.Drawing.Point(574, 52);
            this.pnlAttributes.Name = "pnlAttributes";
            this.pnlAttributes.Size = new System.Drawing.Size(195, 157);
            this.pnlAttributes.TabIndex = 120;
            this.pnlAttributes.Visible = false;
            // 
            // btnAttrCancel
            // 
            this.btnAttrCancel.Location = new System.Drawing.Point(99, 130);
            this.btnAttrCancel.Name = "btnAttrCancel";
            this.btnAttrCancel.Size = new System.Drawing.Size(91, 23);
            this.btnAttrCancel.TabIndex = 118;
            this.btnAttrCancel.Text = "Cancel";
            this.btnAttrCancel.UseVisualStyleBackColor = true;
            this.btnAttrCancel.Click += new System.EventHandler(this.btnAttrCancel_Click);
            // 
            // btnAttrOK
            // 
            this.btnAttrOK.Location = new System.Drawing.Point(5, 130);
            this.btnAttrOK.Name = "btnAttrOK";
            this.btnAttrOK.Size = new System.Drawing.Size(91, 23);
            this.btnAttrOK.TabIndex = 117;
            this.btnAttrOK.Text = "OK";
            this.btnAttrOK.UseVisualStyleBackColor = true;
            this.btnAttrOK.Click += new System.EventHandler(this.btnAttrOK_Click);
            // 
            // clbAttributes
            // 
            this.clbAttributes.CheckOnClick = true;
            this.clbAttributes.FormattingEnabled = true;
            this.clbAttributes.Items.AddRange(new object[] {
            "Attributes 1",
            "Attributes 2",
            "Attributes 3",
            "Attributes 4",
            "Attributes 5",
            "Attributes 6",
            "Attributes 7",
            "Attributes 8"});
            this.clbAttributes.Location = new System.Drawing.Point(5, 4);
            this.clbAttributes.Name = "clbAttributes";
            this.clbAttributes.Size = new System.Drawing.Size(185, 124);
            this.clbAttributes.TabIndex = 116;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(740, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 119;
            this.label4.Text = "days";
            // 
            // chkShowSales
            // 
            this.chkShowSales.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowSales.AutoSize = true;
            this.chkShowSales.BackColor = System.Drawing.Color.Transparent;
            this.chkShowSales.Location = new System.Drawing.Point(575, 7);
            this.chkShowSales.Name = "chkShowSales";
            this.chkShowSales.Size = new System.Drawing.Size(126, 17);
            this.chkShowSales.TabIndex = 118;
            this.chkShowSales.Text = "Show Previous Sales";
            this.chkShowSales.UseVisualStyleBackColor = false;
            this.chkShowSales.CheckedChanged += new System.EventHandler(this.chkShowSales_CheckedChanged);
            // 
            // txtNumOfDays
            // 
            this.txtNumOfDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNumOfDays.Location = new System.Drawing.Point(703, 5);
            this.txtNumOfDays.Name = "txtNumOfDays";
            this.txtNumOfDays.Size = new System.Drawing.Size(34, 20);
            this.txtNumOfDays.TabIndex = 117;
            // 
            // btnShowAttr
            // 
            this.btnShowAttr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowAttr.Location = new System.Drawing.Point(574, 30);
            this.btnShowAttr.Name = "btnShowAttr";
            this.btnShowAttr.Size = new System.Drawing.Size(195, 23);
            this.btnShowAttr.TabIndex = 115;
            this.btnShowAttr.Text = "Show Attributes";
            this.btnShowAttr.UseVisualStyleBackColor = true;
            this.btnShowAttr.Click += new System.EventHandler(this.btnShowAttr_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(68, 30);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(213, 20);
            this.txtFilter.TabIndex = 114;
            this.txtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_KeyPress);
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.BackColor = System.Drawing.Color.Transparent;
            this.lblSupplierName.Location = new System.Drawing.Point(65, 9);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(73, 13);
            this.lblSupplierName.TabIndex = 113;
            this.lblSupplierName.Text = "SupplierName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(11, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 112;
            this.label2.Text = "Filter:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 111;
            this.label1.Text = "Supplier:";
            // 
            // pnlLoading
            // 
            this.pnlLoading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlLoading.BackColor = System.Drawing.Color.White;
            this.pnlLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoading.Controls.Add(this.label3);
            this.pnlLoading.Location = new System.Drawing.Point(227, 220);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(331, 118);
            this.pnlLoading.TabIndex = 109;
            this.pnlLoading.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(55, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 31);
            this.label3.TabIndex = 0;
            this.label3.Text = "PLEASE WAIT...";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnOK.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(540, 507);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(112, 41);
            this.btnOK.TabIndex = 21;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancelAdd
            // 
            this.btnCancelAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelAdd.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancelAdd.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelAdd.ForeColor = System.Drawing.Color.White;
            this.btnCancelAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelAdd.Location = new System.Drawing.Point(657, 507);
            this.btnCancelAdd.Name = "btnCancelAdd";
            this.btnCancelAdd.Size = new System.Drawing.Size(112, 41);
            this.btnCancelAdd.TabIndex = 20;
            this.btnCancelAdd.Text = "CANCEL";
            this.btnCancelAdd.UseVisualStyleBackColor = false;
            this.btnCancelAdd.Click += new System.EventHandler(this.btnCancelAdd_Click);
            // 
            // dgvItemList
            // 
            this.dgvItemList.AllowUserToAddRows = false;
            this.dgvItemList.AllowUserToDeleteRows = false;
            this.dgvItemList.AllowUserToResizeRows = false;
            this.dgvItemList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItemList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemNo,
            this.colItemName,
            this.colOnHand,
            this.colOrderQty,
            this.OrderUOM,
            this.colSales1,
            this.colSales2,
            this.colSales3,
            this.colCost,
            this.colUOM,
            this.colAttributes1,
            this.colAttributes2,
            this.colAttributes3,
            this.colAttributes4,
            this.colAttributes5,
            this.colAttributes6,
            this.colAttributes7,
            this.colAttributes8,
            this.colConvRate});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemList.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvItemList.Location = new System.Drawing.Point(10, 64);
            this.dgvItemList.Name = "dgvItemList";
            this.dgvItemList.RowHeadersVisible = false;
            this.dgvItemList.Size = new System.Drawing.Size(759, 437);
            this.dgvItemList.TabIndex = 0;
            this.dgvItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemList_CellClick);
            // 
            // saveFileDialogExport
            // 
            this.saveFileDialogExport.DefaultExt = "xls";
            this.saveFileDialogExport.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm\"";
            this.saveFileDialogExport.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogExport_FileOk);
            // 
            // colItemNo
            // 
            this.colItemNo.DataPropertyName = "ItemNo";
            this.colItemNo.HeaderText = "Item No";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.ReadOnly = true;
            this.colItemNo.Width = 120;
            // 
            // colItemName
            // 
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.MinimumWidth = 350;
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colOnHand
            // 
            this.colOnHand.DataPropertyName = "OnHandQty";
            dataGridViewCellStyle2.Format = "#,0.####";
            this.colOnHand.DefaultCellStyle = dataGridViewCellStyle2;
            this.colOnHand.HeaderText = "On Hand";
            this.colOnHand.Name = "colOnHand";
            this.colOnHand.ReadOnly = true;
            // 
            // colOrderQty
            // 
            this.colOrderQty.DataPropertyName = "OrderQty";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LemonChiffon;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = "0";
            this.colOrderQty.DefaultCellStyle = dataGridViewCellStyle3;
            this.colOrderQty.HeaderText = "Order";
            this.colOrderQty.Name = "colOrderQty";
            this.colOrderQty.ReadOnly = true;
            // 
            // OrderUOM
            // 
            this.OrderUOM.DataPropertyName = "UOM";
            this.OrderUOM.HeaderText = "Order UOM";
            this.OrderUOM.Name = "OrderUOM";
            // 
            // colSales1
            // 
            this.colSales1.DataPropertyName = "Sales1";
            dataGridViewCellStyle4.Format = "#,0.####";
            this.colSales1.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSales1.HeaderText = "10 days";
            this.colSales1.Name = "colSales1";
            this.colSales1.ReadOnly = true;
            // 
            // colSales2
            // 
            this.colSales2.DataPropertyName = "Sales2";
            dataGridViewCellStyle5.Format = "#,0.####";
            this.colSales2.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSales2.HeaderText = "20 days";
            this.colSales2.Name = "colSales2";
            this.colSales2.ReadOnly = true;
            // 
            // colSales3
            // 
            this.colSales3.DataPropertyName = "Sales3";
            dataGridViewCellStyle6.Format = "#,0.####";
            this.colSales3.DefaultCellStyle = dataGridViewCellStyle6;
            this.colSales3.HeaderText = "30 days";
            this.colSales3.Name = "colSales3";
            this.colSales3.ReadOnly = true;
            // 
            // colCost
            // 
            this.colCost.DataPropertyName = "CostPrice";
            this.colCost.HeaderText = "Cost";
            this.colCost.Name = "colCost";
            this.colCost.ReadOnly = true;
            // 
            // colUOM
            // 
            this.colUOM.DataPropertyName = "UOM";
            this.colUOM.HeaderText = "Sales UOM";
            this.colUOM.Name = "colUOM";
            this.colUOM.ReadOnly = true;
            // 
            // colAttributes1
            // 
            this.colAttributes1.DataPropertyName = "Attributes1";
            this.colAttributes1.HeaderText = "Attributes1";
            this.colAttributes1.Name = "colAttributes1";
            this.colAttributes1.ReadOnly = true;
            this.colAttributes1.Visible = false;
            // 
            // colAttributes2
            // 
            this.colAttributes2.DataPropertyName = "Attributes2";
            this.colAttributes2.HeaderText = "Attributes2";
            this.colAttributes2.Name = "colAttributes2";
            this.colAttributes2.ReadOnly = true;
            this.colAttributes2.Visible = false;
            // 
            // colAttributes3
            // 
            this.colAttributes3.DataPropertyName = "Attributes3";
            this.colAttributes3.HeaderText = "Attributes3";
            this.colAttributes3.Name = "colAttributes3";
            this.colAttributes3.ReadOnly = true;
            this.colAttributes3.Visible = false;
            // 
            // colAttributes4
            // 
            this.colAttributes4.DataPropertyName = "Attributes4";
            this.colAttributes4.HeaderText = "Attributes4";
            this.colAttributes4.Name = "colAttributes4";
            this.colAttributes4.ReadOnly = true;
            this.colAttributes4.Visible = false;
            // 
            // colAttributes5
            // 
            this.colAttributes5.DataPropertyName = "Attributes5";
            this.colAttributes5.HeaderText = "Attributes5";
            this.colAttributes5.Name = "colAttributes5";
            this.colAttributes5.ReadOnly = true;
            this.colAttributes5.Visible = false;
            // 
            // colAttributes6
            // 
            this.colAttributes6.DataPropertyName = "Attributes6";
            this.colAttributes6.HeaderText = "Attributes6";
            this.colAttributes6.Name = "colAttributes6";
            this.colAttributes6.ReadOnly = true;
            this.colAttributes6.Visible = false;
            // 
            // colAttributes7
            // 
            this.colAttributes7.DataPropertyName = "Attributes7";
            this.colAttributes7.HeaderText = "Attributes7";
            this.colAttributes7.Name = "colAttributes7";
            this.colAttributes7.ReadOnly = true;
            this.colAttributes7.Visible = false;
            // 
            // colAttributes8
            // 
            this.colAttributes8.DataPropertyName = "Attributes8";
            this.colAttributes8.HeaderText = "Attributes8";
            this.colAttributes8.Name = "colAttributes8";
            this.colAttributes8.ReadOnly = true;
            this.colAttributes8.Visible = false;
            // 
            // colConvRate
            // 
            this.colConvRate.DataPropertyName = "ConvRate";
            this.colConvRate.HeaderText = "ConvRate";
            this.colConvRate.Name = "colConvRate";
            this.colConvRate.Visible = false;
            // 
            // frmAddItemWithFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 562);
            this.Controls.Add(this.pnlAddItems);
            this.Name = "frmAddItemWithFilter";
            this.Text = "Add Item";
            this.Load += new System.EventHandler(this.frmAddItemWithFilter_Load);
            this.pnlAddItems.ResumeLayout(false);
            this.pnlAddItems.PerformLayout();
            this.pnlAttributes.ResumeLayout(false);
            this.pnlLoading.ResumeLayout(false);
            this.pnlLoading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlAddItems;
        protected System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancelAdd;
        private System.Windows.Forms.DataGridView dgvItemList;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblSupplierName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnShowAttr;
        private System.Windows.Forms.CheckedListBox clbAttributes;
        private System.Windows.Forms.CheckBox chkShowSales;
        private System.Windows.Forms.TextBox txtNumOfDays;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlAttributes;
        private System.Windows.Forms.Button btnAttrCancel;
        private System.Windows.Forms.Button btnAttrOK;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn col30days;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOnHand;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSales1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSales2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSales3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes6;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes7;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes8;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConvRate;
    }
}