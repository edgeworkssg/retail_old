namespace WinPowerPOS.AppointmentForms
{
	partial class frmAppointmentEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSalesPerson = new System.Windows.Forms.Label();
            this.cbSalesPerson = new System.Windows.Forms.ComboBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.nudDuration = new System.Windows.Forms.NumericUpDown();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.lblColor = new System.Windows.Forms.Label();
            this.btnColor = new System.Windows.Forms.Button();
            this.btnFontColor = new System.Windows.Forms.Button();
            this.lblFontColor = new System.Windows.Forms.Label();
            this.lblMemberName = new System.Windows.Forms.Label();
            this.gbMember = new System.Windows.Forms.GroupBox();
            this.btnAddNewMember = new System.Windows.Forms.Button();
            this.btnClearMember = new System.Windows.Forms.Button();
            this.btnMemberInfo = new System.Windows.Forms.Button();
            this.lblMemberInfo = new System.Windows.Forms.Label();
            this.gbServices = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvItemList = new System.Windows.Forms.DataGridView();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPreviousServices = new System.Windows.Forms.DataGridView();
            this.colReadd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvcItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcTransTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcSalesPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcSumAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelectServices = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.labelRoom = new System.Windows.Forms.Label();
            this.cbResources = new System.Windows.Forms.ComboBox();
            this.btnClearDuration = new System.Windows.Forms.Button();
            this.btnDuration5 = new System.Windows.Forms.Button();
            this.btnDuration4 = new System.Windows.Forms.Button();
            this.btnDuration3 = new System.Windows.Forms.Button();
            this.btnDuration2 = new System.Windows.Forms.Button();
            this.btnDuration1 = new System.Windows.Forms.Button();
            this.btnSelectMember = new System.Windows.Forms.Button();
            this.btnCreateInvoice = new System.Windows.Forms.Button();
            this.btnRoomListing = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).BeginInit();
            this.gbMember.SuspendLayout();
            this.gbServices.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreviousServices)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOk.Location = new System.Drawing.Point(804, 687);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(120, 48);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(930, 687);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 48);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblSalesPerson
            // 
            this.lblSalesPerson.AutoSize = true;
            this.lblSalesPerson.Location = new System.Drawing.Point(21, 27);
            this.lblSalesPerson.Name = "lblSalesPerson";
            this.lblSalesPerson.Size = new System.Drawing.Size(88, 16);
            this.lblSalesPerson.TabIndex = 0;
            this.lblSalesPerson.Text = "Sales person";
            // 
            // cbSalesPerson
            // 
            this.cbSalesPerson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSalesPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSalesPerson.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSalesPerson.FormattingEnabled = true;
            this.cbSalesPerson.Location = new System.Drawing.Point(139, 24);
            this.cbSalesPerson.Name = "cbSalesPerson";
            this.cbSalesPerson.Size = new System.Drawing.Size(862, 24);
            this.cbSalesPerson.TabIndex = 1;
            this.cbSalesPerson.SelectedIndexChanged += new System.EventHandler(this.cbSalesPerson_SelectedIndexChanged);
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(25, 116);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(110, 16);
            this.lblDuration.TabIndex = 2;
            this.lblDuration.Text = "Duration, minutes";
            // 
            // nudDuration
            // 
            this.nudDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudDuration.Location = new System.Drawing.Point(143, 111);
            this.nudDuration.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDuration.Name = "nudDuration";
            this.nudDuration.Size = new System.Drawing.Size(68, 26);
            this.nudDuration.TabIndex = 3;
            this.nudDuration.ValueChanged += new System.EventHandler(this.nudDuration_ValueChanged);
            // 
            // tbDescription
            // 
            this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDescription.Location = new System.Drawing.Point(20, 20);
            this.tbDescription.MaxLength = 500;
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(652, 47);
            this.tbDescription.TabIndex = 0;
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            this.colorDialog.FullOpen = true;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(21, 43);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(40, 16);
            this.lblColor.TabIndex = 0;
            this.lblColor.Text = "Color";
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.Chocolate;
            this.btnColor.Location = new System.Drawing.Point(67, 35);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(62, 32);
            this.btnColor.TabIndex = 1;
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // btnFontColor
            // 
            this.btnFontColor.BackColor = System.Drawing.Color.White;
            this.btnFontColor.Location = new System.Drawing.Point(238, 35);
            this.btnFontColor.Name = "btnFontColor";
            this.btnFontColor.Size = new System.Drawing.Size(62, 32);
            this.btnFontColor.TabIndex = 3;
            this.btnFontColor.UseVisualStyleBackColor = false;
            this.btnFontColor.Click += new System.EventHandler(this.btnFontColor_Click);
            // 
            // lblFontColor
            // 
            this.lblFontColor.AutoSize = true;
            this.lblFontColor.Location = new System.Drawing.Point(163, 43);
            this.lblFontColor.Name = "lblFontColor";
            this.lblFontColor.Size = new System.Drawing.Size(69, 16);
            this.lblFontColor.TabIndex = 2;
            this.lblFontColor.Text = "Font Color";
            // 
            // lblMemberName
            // 
            this.lblMemberName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMemberName.AutoEllipsis = true;
            this.lblMemberName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMemberName.Location = new System.Drawing.Point(24, 31);
            this.lblMemberName.Name = "lblMemberName";
            this.lblMemberName.Size = new System.Drawing.Size(276, 25);
            this.lblMemberName.TabIndex = 1;
            this.lblMemberName.Text = "No Member selected";
            // 
            // gbMember
            // 
            this.gbMember.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbMember.Controls.Add(this.btnAddNewMember);
            this.gbMember.Controls.Add(this.btnClearMember);
            this.gbMember.Controls.Add(this.btnMemberInfo);
            this.gbMember.Controls.Add(this.lblMemberInfo);
            this.gbMember.Controls.Add(this.lblMemberName);
            this.gbMember.Location = new System.Drawing.Point(28, 336);
            this.gbMember.Name = "gbMember";
            this.gbMember.Size = new System.Drawing.Size(323, 258);
            this.gbMember.TabIndex = 1;
            this.gbMember.TabStop = false;
            this.gbMember.Text = " Member ";
            // 
            // btnAddNewMember
            // 
            this.btnAddNewMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNewMember.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddNewMember.Location = new System.Drawing.Point(2, 193);
            this.btnAddNewMember.Name = "btnAddNewMember";
            this.btnAddNewMember.Size = new System.Drawing.Size(97, 47);
            this.btnAddNewMember.TabIndex = 8;
            this.btnAddNewMember.Text = "Add New Member";
            this.btnAddNewMember.UseVisualStyleBackColor = true;
            this.btnAddNewMember.Click += new System.EventHandler(this.btnAddNewMember_Click);
            // 
            // btnClearMember
            // 
            this.btnClearMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearMember.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearMember.Location = new System.Drawing.Point(208, 193);
            this.btnClearMember.Name = "btnClearMember";
            this.btnClearMember.Size = new System.Drawing.Size(104, 47);
            this.btnClearMember.TabIndex = 7;
            this.btnClearMember.Text = "Clear Member";
            this.btnClearMember.UseVisualStyleBackColor = true;
            this.btnClearMember.Click += new System.EventHandler(this.btnClearMember_Click);
            // 
            // btnMemberInfo
            // 
            this.btnMemberInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMemberInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnMemberInfo.Location = new System.Drawing.Point(105, 193);
            this.btnMemberInfo.Name = "btnMemberInfo";
            this.btnMemberInfo.Size = new System.Drawing.Size(97, 47);
            this.btnMemberInfo.TabIndex = 6;
            this.btnMemberInfo.Text = "Member info";
            this.btnMemberInfo.UseVisualStyleBackColor = true;
            this.btnMemberInfo.Click += new System.EventHandler(this.btnMemberInfo_Click);
            // 
            // lblMemberInfo
            // 
            this.lblMemberInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMemberInfo.AutoEllipsis = true;
            this.lblMemberInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblMemberInfo.Location = new System.Drawing.Point(41, 56);
            this.lblMemberInfo.Name = "lblMemberInfo";
            this.lblMemberInfo.Size = new System.Drawing.Size(259, 125);
            this.lblMemberInfo.TabIndex = 2;
            // 
            // gbServices
            // 
            this.gbServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbServices.Controls.Add(this.tableLayoutPanel2);
            this.gbServices.Location = new System.Drawing.Point(357, 189);
            this.gbServices.Name = "gbServices";
            this.gbServices.Size = new System.Drawing.Size(693, 405);
            this.gbServices.TabIndex = 2;
            this.gbServices.TabStop = false;
            this.gbServices.Text = " Services ";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.dgvItemList, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dgvPreviousServices, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(20, 33);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(652, 354);
            this.tableLayoutPanel2.TabIndex = 21;
            // 
            // dgvItemList
            // 
            this.dgvItemList.AllowUserToAddRows = false;
            this.dgvItemList.AllowUserToDeleteRows = false;
            this.dgvItemList.AllowUserToResizeRows = false;
            this.dgvItemList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvItemList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvItemList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRemove,
            this.colItemNo,
            this.colItemName,
            this.colPrice,
            this.colCategoryName});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItemList.Location = new System.Drawing.Point(3, 23);
            this.dgvItemList.Name = "dgvItemList";
            this.dgvItemList.RowHeadersVisible = false;
            this.dgvItemList.Size = new System.Drawing.Size(646, 151);
            this.dgvItemList.TabIndex = 1;
            this.dgvItemList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemList_CellContentDoubleClick);
            this.dgvItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemList_CellContentClick);
            // 
            // colRemove
            // 
            this.colRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.colRemove.DefaultCellStyle = dataGridViewCellStyle1;
            this.colRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.colRemove.HeaderText = "Action";
            this.colRemove.Name = "colRemove";
            this.colRemove.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colRemove.Text = "Remove";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 51;
            // 
            // colItemNo
            // 
            this.colItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colItemNo.DataPropertyName = "ItemNo";
            this.colItemNo.HeaderText = "Item No";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.ReadOnly = true;
            this.colItemNo.Width = 79;
            // 
            // colItemName
            // 
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.MinimumWidth = 350;
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colPrice
            // 
            this.colPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPrice.DataPropertyName = "UnitPrice";
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = "0";
            this.colPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.colPrice.HeaderText = "Price";
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            this.colPrice.Width = 64;
            // 
            // colCategoryName
            // 
            this.colCategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colCategoryName.DataPropertyName = "CategoryName";
            this.colCategoryName.HeaderText = "Category";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.ReadOnly = true;
            this.colCategoryName.Width = 88;
            // 
            // dgvPreviousServices
            // 
            this.dgvPreviousServices.AllowUserToAddRows = false;
            this.dgvPreviousServices.AllowUserToDeleteRows = false;
            this.dgvPreviousServices.AllowUserToResizeRows = false;
            this.dgvPreviousServices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPreviousServices.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPreviousServices.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPreviousServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreviousServices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colReadd,
            this.dgvcItemNo,
            this.dgvcTransTime,
            this.ItemName,
            this.dgvcSalesPerson,
            this.dgvcUnitPrice,
            this.dgvcQuantity,
            this.dgvcSumAmount});
            this.dgvPreviousServices.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPreviousServices.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvPreviousServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPreviousServices.Location = new System.Drawing.Point(4, 201);
            this.dgvPreviousServices.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPreviousServices.Name = "dgvPreviousServices";
            this.dgvPreviousServices.ReadOnly = true;
            this.dgvPreviousServices.RowHeadersVisible = false;
            this.dgvPreviousServices.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvPreviousServices.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvPreviousServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPreviousServices.Size = new System.Drawing.Size(644, 149);
            this.dgvPreviousServices.TabIndex = 3;
            this.dgvPreviousServices.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPreviousServices_CellContentDoubleClick);
            this.dgvPreviousServices.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPreviousServices_CellContentClick);
            // 
            // colReadd
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.colReadd.DefaultCellStyle = dataGridViewCellStyle5;
            this.colReadd.HeaderText = "Action";
            this.colReadd.Name = "colReadd";
            this.colReadd.ReadOnly = true;
            this.colReadd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colReadd.Text = "Re-add";
            this.colReadd.UseColumnTextForButtonValue = true;
            this.colReadd.Width = 80;
            // 
            // dgvcItemNo
            // 
            this.dgvcItemNo.DataPropertyName = "ItemNo";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvcItemNo.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvcItemNo.HeaderText = "Item No";
            this.dgvcItemNo.Name = "dgvcItemNo";
            this.dgvcItemNo.ReadOnly = true;
            this.dgvcItemNo.Visible = false;
            // 
            // dgvcTransTime
            // 
            this.dgvcTransTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvcTransTime.DataPropertyName = "OrderDate";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.Format = "dd MMM yyyy -- HH:mm";
            this.dgvcTransTime.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvcTransTime.FillWeight = 120F;
            this.dgvcTransTime.HeaderText = "Transaction Time";
            this.dgvcTransTime.MinimumWidth = 130;
            this.dgvcTransTime.Name = "dgvcTransTime";
            this.dgvcTransTime.ReadOnly = true;
            this.dgvcTransTime.Width = 130;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.HeaderText = "Item Name";
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // dgvcSalesPerson
            // 
            this.dgvcSalesPerson.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcSalesPerson.DataPropertyName = "SalesPersonDisplayName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = "--";
            this.dgvcSalesPerson.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvcSalesPerson.HeaderText = "Sales";
            this.dgvcSalesPerson.Name = "dgvcSalesPerson";
            this.dgvcSalesPerson.ReadOnly = true;
            this.dgvcSalesPerson.Width = 68;
            // 
            // dgvcUnitPrice
            // 
            this.dgvcUnitPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvcUnitPrice.DataPropertyName = "UnitPrice";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "C2";
            dataGridViewCellStyle9.NullValue = null;
            this.dgvcUnitPrice.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvcUnitPrice.HeaderText = "Unit Price";
            this.dgvcUnitPrice.Name = "dgvcUnitPrice";
            this.dgvcUnitPrice.ReadOnly = true;
            this.dgvcUnitPrice.Width = 70;
            // 
            // dgvcQuantity
            // 
            this.dgvcQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvcQuantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N2";
            this.dgvcQuantity.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvcQuantity.HeaderText = "Quantity";
            this.dgvcQuantity.Name = "dgvcQuantity";
            this.dgvcQuantity.ReadOnly = true;
            this.dgvcQuantity.Width = 60;
            // 
            // dgvcSumAmount
            // 
            this.dgvcSumAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvcSumAmount.DataPropertyName = "Amount";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "C2";
            this.dgvcSumAmount.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvcSumAmount.HeaderText = "Amount";
            this.dgvcSumAmount.Name = "dgvcSumAmount";
            this.dgvcSumAmount.ReadOnly = true;
            this.dgvcSumAmount.Width = 60;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Selected services";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = "Previous orders";
            // 
            // btnSelectServices
            // 
            this.btnSelectServices.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSelectServices.Image = global::WinPowerPOS.Properties.Resources.Service;
            this.btnSelectServices.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectServices.Location = new System.Drawing.Point(199, 201);
            this.btnSelectServices.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.btnSelectServices.Name = "btnSelectServices";
            this.btnSelectServices.Size = new System.Drawing.Size(135, 122);
            this.btnSelectServices.TabIndex = 0;
            this.btnSelectServices.Text = "\r\n\r\n\r\n\r\n\r\nSelect services";
            this.btnSelectServices.UseVisualStyleBackColor = true;
            this.btnSelectServices.Click += new System.EventHandler(this.btnSelectServices_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tbDescription);
            this.groupBox3.Location = new System.Drawing.Point(357, 600);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(693, 78);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Description ";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.lblFontColor);
            this.groupBox4.Controls.Add(this.lblColor);
            this.groupBox4.Controls.Add(this.btnColor);
            this.groupBox4.Controls.Add(this.btnFontColor);
            this.groupBox4.Location = new System.Drawing.Point(28, 600);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(323, 78);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = " Appearance ";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.labelRoom);
            this.groupBox5.Controls.Add(this.cbResources);
            this.groupBox5.Controls.Add(this.btnClearDuration);
            this.groupBox5.Controls.Add(this.btnDuration5);
            this.groupBox5.Controls.Add(this.btnDuration4);
            this.groupBox5.Controls.Add(this.btnDuration3);
            this.groupBox5.Controls.Add(this.btnDuration2);
            this.groupBox5.Controls.Add(this.btnDuration1);
            this.groupBox5.Controls.Add(this.lblSalesPerson);
            this.groupBox5.Controls.Add(this.cbSalesPerson);
            this.groupBox5.Controls.Add(this.lblDuration);
            this.groupBox5.Controls.Add(this.nudDuration);
            this.groupBox5.Location = new System.Drawing.Point(28, 21);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1022, 162);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Appointment ";
            // 
            // labelRoom
            // 
            this.labelRoom.AutoSize = true;
            this.labelRoom.Location = new System.Drawing.Point(25, 66);
            this.labelRoom.Name = "labelRoom";
            this.labelRoom.Size = new System.Drawing.Size(45, 16);
            this.labelRoom.TabIndex = 11;
            this.labelRoom.Text = "Room";
            // 
            // cbResources
            // 
            this.cbResources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbResources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResources.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbResources.FormattingEnabled = true;
            this.cbResources.Location = new System.Drawing.Point(139, 63);
            this.cbResources.Name = "cbResources";
            this.cbResources.Size = new System.Drawing.Size(862, 24);
            this.cbResources.TabIndex = 10;
            // 
            // btnClearDuration
            // 
            this.btnClearDuration.Location = new System.Drawing.Point(225, 102);
            this.btnClearDuration.Name = "btnClearDuration";
            this.btnClearDuration.Size = new System.Drawing.Size(75, 45);
            this.btnClearDuration.TabIndex = 9;
            this.btnClearDuration.Text = "Clear";
            this.btnClearDuration.UseVisualStyleBackColor = true;
            this.btnClearDuration.Click += new System.EventHandler(this.btnClearDuration_Click);
            // 
            // btnDuration5
            // 
            this.btnDuration5.Location = new System.Drawing.Point(642, 102);
            this.btnDuration5.Name = "btnDuration5";
            this.btnDuration5.Size = new System.Drawing.Size(75, 45);
            this.btnDuration5.TabIndex = 8;
            this.btnDuration5.UseVisualStyleBackColor = true;
            this.btnDuration5.Click += new System.EventHandler(this.btnDuration_Click);
            // 
            // btnDuration4
            // 
            this.btnDuration4.Location = new System.Drawing.Point(561, 102);
            this.btnDuration4.Name = "btnDuration4";
            this.btnDuration4.Size = new System.Drawing.Size(75, 45);
            this.btnDuration4.TabIndex = 7;
            this.btnDuration4.UseVisualStyleBackColor = true;
            this.btnDuration4.Click += new System.EventHandler(this.btnDuration_Click);
            // 
            // btnDuration3
            // 
            this.btnDuration3.Location = new System.Drawing.Point(480, 102);
            this.btnDuration3.Name = "btnDuration3";
            this.btnDuration3.Size = new System.Drawing.Size(75, 45);
            this.btnDuration3.TabIndex = 6;
            this.btnDuration3.UseVisualStyleBackColor = true;
            this.btnDuration3.Click += new System.EventHandler(this.btnDuration_Click);
            // 
            // btnDuration2
            // 
            this.btnDuration2.Location = new System.Drawing.Point(399, 102);
            this.btnDuration2.Name = "btnDuration2";
            this.btnDuration2.Size = new System.Drawing.Size(75, 45);
            this.btnDuration2.TabIndex = 5;
            this.btnDuration2.UseVisualStyleBackColor = true;
            this.btnDuration2.Click += new System.EventHandler(this.btnDuration_Click);
            // 
            // btnDuration1
            // 
            this.btnDuration1.Location = new System.Drawing.Point(318, 102);
            this.btnDuration1.Name = "btnDuration1";
            this.btnDuration1.Size = new System.Drawing.Size(75, 45);
            this.btnDuration1.TabIndex = 4;
            this.btnDuration1.UseVisualStyleBackColor = true;
            this.btnDuration1.Click += new System.EventHandler(this.btnDuration_Click);
            // 
            // btnSelectMember
            // 
            this.btnSelectMember.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSelectMember.Image = global::WinPowerPOS.Properties.Resources.Member;
            this.btnSelectMember.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectMember.Location = new System.Drawing.Point(48, 201);
            this.btnSelectMember.Name = "btnSelectMember";
            this.btnSelectMember.Size = new System.Drawing.Size(135, 122);
            this.btnSelectMember.TabIndex = 0;
            this.btnSelectMember.Text = "\r\n\r\n\r\n\r\n\r\nSelect member";
            this.btnSelectMember.UseVisualStyleBackColor = true;
            this.btnSelectMember.Click += new System.EventHandler(this.btnSelectMember_Click);
            // 
            // btnCreateInvoice
            // 
            this.btnCreateInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCreateInvoice.Location = new System.Drawing.Point(28, 687);
            this.btnCreateInvoice.Name = "btnCreateInvoice";
            this.btnCreateInvoice.Size = new System.Drawing.Size(185, 48);
            this.btnCreateInvoice.TabIndex = 7;
            this.btnCreateInvoice.Text = "CREATE INVOICE ...";
            this.btnCreateInvoice.UseVisualStyleBackColor = true;
            this.btnCreateInvoice.Click += new System.EventHandler(this.btnCreateInvoice_Click);
            // 
            // btnRoomListing
            // 
            this.btnRoomListing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRoomListing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRoomListing.Location = new System.Drawing.Point(219, 687);
            this.btnRoomListing.Name = "btnRoomListing";
            this.btnRoomListing.Size = new System.Drawing.Size(150, 48);
            this.btnRoomListing.TabIndex = 8;
            this.btnRoomListing.Text = "ROOM LISTING";
            this.btnRoomListing.UseVisualStyleBackColor = true;
            this.btnRoomListing.Click += new System.EventHandler(this.btnRoomListing_Click);
            // 
            // frmAppointmentEditor
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1070, 746);
            this.Controls.Add(this.btnRoomListing);
            this.Controls.Add(this.btnCreateInvoice);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnSelectServices);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSelectMember);
            this.Controls.Add(this.gbServices);
            this.Controls.Add(this.gbMember);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "frmAppointmentEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Appointment Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAppointmentEditor_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAppointmentEditor_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).EndInit();
            this.gbMember.ResumeLayout(false);
            this.gbServices.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreviousServices)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblSalesPerson;
		private System.Windows.Forms.ComboBox cbSalesPerson;
		private System.Windows.Forms.Label lblDuration;
		private System.Windows.Forms.NumericUpDown nudDuration;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Label lblColor;
		private System.Windows.Forms.Button btnColor;
		private System.Windows.Forms.Button btnFontColor;
		private System.Windows.Forms.Label lblFontColor;
		private System.Windows.Forms.Label lblMemberName;
		private System.Windows.Forms.Button btnSelectMember;
		private System.Windows.Forms.GroupBox gbMember;
		private System.Windows.Forms.GroupBox gbServices;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label lblMemberInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnSelectServices;
		private System.Windows.Forms.DataGridView dgvPreviousServices;
        private System.Windows.Forms.DataGridView dgvItemList;
		private System.Windows.Forms.Button btnDuration4;
		private System.Windows.Forms.Button btnDuration3;
		private System.Windows.Forms.Button btnDuration2;
		private System.Windows.Forms.Button btnDuration1;
		private System.Windows.Forms.Button btnDuration5;
		private System.Windows.Forms.Button btnClearMember;
		private System.Windows.Forms.Button btnMemberInfo;
		private System.Windows.Forms.Button btnCreateInvoice;
		private System.Windows.Forms.Button btnClearDuration;
		private System.Windows.Forms.DataGridViewButtonColumn colRemove;
		private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
		private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
		private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryName;
        private System.Windows.Forms.Label labelRoom;
        private System.Windows.Forms.ComboBox cbResources;
        private System.Windows.Forms.Button btnRoomListing;
        private System.Windows.Forms.Button btnAddNewMember;
        private System.Windows.Forms.DataGridViewButtonColumn colReadd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcTransTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcSalesPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcSumAmount;
	}
}