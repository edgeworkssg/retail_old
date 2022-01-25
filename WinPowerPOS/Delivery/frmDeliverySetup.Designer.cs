namespace WinPowerPOS.Delivery
{
    partial class frmDeliverySetup
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
            this.dgvDeliverySetup = new System.Windows.Forms.DataGridView();
            this.cbColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepositPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDetID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecipientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MobileNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostalCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.cmbDeliveryTime = new System.Windows.Forms.ComboBox();
            this.dtpDeliveryDate = new System.Windows.Forms.DateTimePicker();
            this.txtMembershipNo = new System.Windows.Forms.TextBox();
            this.label51 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.lblReceiptNo = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.txtReceiptNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChooseMember = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.txtMobileNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHomeNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUnitNo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAssign = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lblUnassignedDeposit = new System.Windows.Forms.Label();
            this.lblTickItems = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.gnVendorDelivery = new System.Windows.Forms.GroupBox();
            this.rbNoVendorDelivery = new System.Windows.Forms.RadioButton();
            this.rbYesVendorDelivery = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbDeliveryOutlet = new System.Windows.Forms.ComboBox();
            this.pnlDeliveryOutlet = new System.Windows.Forms.Panel();
            this.pnlVendorDelivery = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliverySetup)).BeginInit();
            this.gnVendorDelivery.SuspendLayout();
            this.pnlDeliveryOutlet.SuspendLayout();
            this.pnlVendorDelivery.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDeliverySetup
            // 
            this.dgvDeliverySetup.AllowUserToAddRows = false;
            this.dgvDeliverySetup.AllowUserToDeleteRows = false;
            this.dgvDeliverySetup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDeliverySetup.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvDeliverySetup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeliverySetup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cbColumn,
            this.ItemNo,
            this.ItemName,
            this.Quantity,
            this.ItemPrice,
            this.DepositPaid,
            this.OrderDetID,
            this.RecipientName,
            this.MobileNo,
            this.HomeNo,
            this.DeliveryAddress,
            this.DeliveryDateTime,
            this.Remarks,
            this.PostalCode,
            this.UnitNo,
            this.DeliveryDate,
            this.DeliveryTime});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDeliverySetup.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDeliverySetup.Location = new System.Drawing.Point(7, 362);
            this.dgvDeliverySetup.Name = "dgvDeliverySetup";
            this.dgvDeliverySetup.RowHeadersVisible = false;
            this.dgvDeliverySetup.Size = new System.Drawing.Size(873, 192);
            this.dgvDeliverySetup.TabIndex = 14;
            this.dgvDeliverySetup.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeliverySetup_CellClick);
            // 
            // cbColumn
            // 
            this.cbColumn.DataPropertyName = "IsTicked";
            this.cbColumn.HeaderText = "Check";
            this.cbColumn.Name = "cbColumn";
            this.cbColumn.Width = 50;
            // 
            // ItemNo
            // 
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.HeaderText = "Item No";
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            this.ItemNo.Visible = false;
            // 
            // ItemName
            // 
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.HeaderText = "Item Name";
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            this.ItemName.Width = 150;
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle1;
            this.Quantity.HeaderText = "Qty";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.Width = 50;
            // 
            // ItemPrice
            // 
            this.ItemPrice.DataPropertyName = "ItemPrice";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "0.00";
            this.ItemPrice.DefaultCellStyle = dataGridViewCellStyle2;
            this.ItemPrice.HeaderText = "Item Price";
            this.ItemPrice.Name = "ItemPrice";
            this.ItemPrice.ReadOnly = true;
            // 
            // DepositPaid
            // 
            this.DepositPaid.DataPropertyName = "DepositPaid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "0.00";
            this.DepositPaid.DefaultCellStyle = dataGridViewCellStyle3;
            this.DepositPaid.HeaderText = "Deposit Paid";
            this.DepositPaid.Name = "DepositPaid";
            this.DepositPaid.ReadOnly = true;
            // 
            // OrderDetID
            // 
            this.OrderDetID.DataPropertyName = "OrderDetID";
            this.OrderDetID.HeaderText = "OrderDetID";
            this.OrderDetID.Name = "OrderDetID";
            this.OrderDetID.Visible = false;
            // 
            // RecipientName
            // 
            this.RecipientName.DataPropertyName = "RecipientName";
            this.RecipientName.HeaderText = "Recipient Name";
            this.RecipientName.Name = "RecipientName";
            this.RecipientName.ReadOnly = true;
            // 
            // MobileNo
            // 
            this.MobileNo.DataPropertyName = "MobileNo";
            this.MobileNo.HeaderText = "Mobile No";
            this.MobileNo.Name = "MobileNo";
            this.MobileNo.ReadOnly = true;
            // 
            // HomeNo
            // 
            this.HomeNo.DataPropertyName = "HomeNo";
            this.HomeNo.HeaderText = "Home No";
            this.HomeNo.Name = "HomeNo";
            this.HomeNo.ReadOnly = true;
            // 
            // DeliveryAddress
            // 
            this.DeliveryAddress.DataPropertyName = "DeliveryAddress";
            this.DeliveryAddress.HeaderText = "Delivery Address";
            this.DeliveryAddress.Name = "DeliveryAddress";
            this.DeliveryAddress.ReadOnly = true;
            this.DeliveryAddress.Width = 200;
            // 
            // DeliveryDateTime
            // 
            this.DeliveryDateTime.DataPropertyName = "DeliveryDateTime";
            this.DeliveryDateTime.HeaderText = "Delivery Date/Time";
            this.DeliveryDateTime.Name = "DeliveryDateTime";
            this.DeliveryDateTime.ReadOnly = true;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            this.Remarks.Width = 200;
            // 
            // PostalCode
            // 
            this.PostalCode.DataPropertyName = "PostalCode";
            this.PostalCode.HeaderText = "PostalCode";
            this.PostalCode.Name = "PostalCode";
            this.PostalCode.ReadOnly = true;
            this.PostalCode.Visible = false;
            // 
            // UnitNo
            // 
            this.UnitNo.DataPropertyName = "UnitNo";
            this.UnitNo.HeaderText = "UnitNo";
            this.UnitNo.Name = "UnitNo";
            this.UnitNo.ReadOnly = true;
            this.UnitNo.Visible = false;
            // 
            // DeliveryDate
            // 
            this.DeliveryDate.DataPropertyName = "DeliveryDate";
            this.DeliveryDate.HeaderText = "DeliveryDate";
            this.DeliveryDate.Name = "DeliveryDate";
            this.DeliveryDate.ReadOnly = true;
            this.DeliveryDate.Visible = false;
            // 
            // DeliveryTime
            // 
            this.DeliveryTime.DataPropertyName = "DeliveryTime";
            this.DeliveryTime.HeaderText = "DeliveryTime";
            this.DeliveryTime.Name = "DeliveryTime";
            this.DeliveryTime.ReadOnly = true;
            this.DeliveryTime.Visible = false;
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(202, 199);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(237, 80);
            this.txtAddress.TabIndex = 7;
            // 
            // cmbDeliveryTime
            // 
            this.cmbDeliveryTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeliveryTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDeliveryTime.FormattingEnabled = true;
            this.cmbDeliveryTime.Items.AddRange(new object[] {
            global::WinPowerPOS.Properties.Language.String1,
            "10am - 1pm",
            "12pm - 3pm",
            "2pm - 5pm"});
            this.cmbDeliveryTime.Location = new System.Drawing.Point(639, 74);
            this.cmbDeliveryTime.Name = "cmbDeliveryTime";
            this.cmbDeliveryTime.Size = new System.Drawing.Size(237, 28);
            this.cmbDeliveryTime.TabIndex = 10;
            // 
            // dtpDeliveryDate
            // 
            this.dtpDeliveryDate.Checked = false;
            this.dtpDeliveryDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDeliveryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDeliveryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDeliveryDate.Location = new System.Drawing.Point(639, 42);
            this.dtpDeliveryDate.Name = "dtpDeliveryDate";
            this.dtpDeliveryDate.ShowCheckBox = true;
            this.dtpDeliveryDate.Size = new System.Drawing.Size(237, 26);
            this.dtpDeliveryDate.TabIndex = 9;
            this.dtpDeliveryDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpDeliveryDate_KeyUp);
            this.dtpDeliveryDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dtpDeliveryDate_MouseUp);
            // 
            // txtMembershipNo
            // 
            this.txtMembershipNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMembershipNo.Location = new System.Drawing.Point(202, 39);
            this.txtMembershipNo.Name = "txtMembershipNo";
            this.txtMembershipNo.ReadOnly = true;
            this.txtMembershipNo.Size = new System.Drawing.Size(237, 26);
            this.txtMembershipNo.TabIndex = 1;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label51.Location = new System.Drawing.Point(507, 78);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(102, 20);
            this.label51.TabIndex = 60;
            this.label51.Text = "Delivery Time";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.Location = new System.Drawing.Point(507, 45);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(103, 20);
            this.label50.TabIndex = 59;
            this.label50.Text = "Delivery Date";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.Location = new System.Drawing.Point(9, 42);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(120, 20);
            this.label48.TabIndex = 57;
            this.label48.Text = "Membership No";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 65);
            this.label1.TabIndex = 67;
            this.label1.Text = "Delivery Address ";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(202, 71);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(237, 26);
            this.txtName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 20);
            this.label2.TabIndex = 74;
            this.label2.Text = "Recipient Name";
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.Location = new System.Drawing.Point(778, 560);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(102, 47);
            this.btnDone.TabIndex = 16;
            this.btnDone.Text = "CONFIRM DELIVERY";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblReceiptNo
            // 
            this.lblReceiptNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReceiptNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptNo.Location = new System.Drawing.Point(602, 6);
            this.lblReceiptNo.Name = "lblReceiptNo";
            this.lblReceiptNo.Size = new System.Drawing.Size(278, 26);
            this.lblReceiptNo.TabIndex = 77;
            this.lblReceiptNo.Text = "PENDING";
            this.lblReceiptNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(670, 560);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 47);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(562, 560);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(102, 47);
            this.button6.TabIndex = 83;
            this.button6.Text = "VOID";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            // 
            // txtReceiptNumber
            // 
            this.txtReceiptNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReceiptNumber.Location = new System.Drawing.Point(202, 7);
            this.txtReceiptNumber.Name = "txtReceiptNumber";
            this.txtReceiptNumber.Size = new System.Drawing.Size(237, 26);
            this.txtReceiptNumber.TabIndex = 0;
            this.txtReceiptNumber.Leave += new System.EventHandler(this.txtReceiptNumber_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 20);
            this.label4.TabIndex = 84;
            this.label4.Text = "Receipt Number";
            // 
            // btnChooseMember
            // 
            this.btnChooseMember.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseMember.Location = new System.Drawing.Point(445, 39);
            this.btnChooseMember.Name = "btnChooseMember";
            this.btnChooseMember.Size = new System.Drawing.Size(35, 26);
            this.btnChooseMember.TabIndex = 2;
            this.btnChooseMember.Text = "...";
            this.btnChooseMember.UseVisualStyleBackColor = true;
            this.btnChooseMember.Visible = false;
            this.btnChooseMember.Click += new System.EventHandler(this.btnChooseMember_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(507, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 20);
            this.label3.TabIndex = 87;
            this.label3.Text = "Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(639, 106);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(237, 80);
            this.txtRemarks.TabIndex = 11;
            // 
            // txtMobileNo
            // 
            this.txtMobileNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMobileNo.Location = new System.Drawing.Point(202, 103);
            this.txtMobileNo.Name = "txtMobileNo";
            this.txtMobileNo.Size = new System.Drawing.Size(237, 26);
            this.txtMobileNo.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 90;
            this.label5.Text = "Mobile No";
            // 
            // txtHomeNo
            // 
            this.txtHomeNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHomeNo.Location = new System.Drawing.Point(202, 135);
            this.txtHomeNo.Name = "txtHomeNo";
            this.txtHomeNo.Size = new System.Drawing.Size(237, 26);
            this.txtHomeNo.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 20);
            this.label6.TabIndex = 92;
            this.label6.Text = "Home No";
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostalCode.Location = new System.Drawing.Point(202, 167);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(144, 26);
            this.txtPostalCode.TabIndex = 6;
            this.txtPostalCode.Leave += new System.EventHandler(this.txtPostalCode_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 20);
            this.label7.TabIndex = 94;
            this.label7.Text = "Postal Code";
            // 
            // txtUnitNo
            // 
            this.txtUnitNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnitNo.Location = new System.Drawing.Point(202, 285);
            this.txtUnitNo.Name = "txtUnitNo";
            this.txtUnitNo.Size = new System.Drawing.Size(237, 26);
            this.txtUnitNo.TabIndex = 8;
            this.txtUnitNo.Leave += new System.EventHandler(this.txtUnitNo_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 20);
            this.label8.TabIndex = 96;
            this.label8.Text = "Unit No";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackgroundImage = global::WinPowerPOS.Properties.Resources.longyellowbackground2;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(747, 276);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(134, 47);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAssign
            // 
            this.btnAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAssign.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnAssign.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssign.ForeColor = System.Drawing.Color.White;
            this.btnAssign.Location = new System.Drawing.Point(607, 276);
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.Size = new System.Drawing.Size(134, 47);
            this.btnAssign.TabIndex = 12;
            this.btnAssign.Text = "Assign Address";
            this.btnAssign.UseVisualStyleBackColor = true;
            this.btnAssign.Click += new System.EventHandler(this.btnAssign_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 563);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 20);
            this.label9.TabIndex = 97;
            this.label9.Text = "Unassigned Deposit";
            // 
            // lblUnassignedDeposit
            // 
            this.lblUnassignedDeposit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUnassignedDeposit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUnassignedDeposit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnassignedDeposit.Location = new System.Drawing.Point(202, 560);
            this.lblUnassignedDeposit.Name = "lblUnassignedDeposit";
            this.lblUnassignedDeposit.Size = new System.Drawing.Size(237, 26);
            this.lblUnassignedDeposit.TabIndex = 98;
            this.lblUnassignedDeposit.Text = "0.00";
            this.lblUnassignedDeposit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTickItems
            // 
            this.lblTickItems.AutoSize = true;
            this.lblTickItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTickItems.Location = new System.Drawing.Point(9, 334);
            this.lblTickItems.Name = "lblTickItems";
            this.lblTickItems.Size = new System.Drawing.Size(639, 20);
            this.lblTickItems.TabIndex = 99;
            this.lblTickItems.Text = "Please tick the items that are required for delivery and press the \"Confirm Deliv" +
                "ery\" button.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 20);
            this.label10.TabIndex = 100;
            this.label10.Text = "Vendor Delivery";
            // 
            // gnVendorDelivery
            // 
            this.gnVendorDelivery.Controls.Add(this.rbNoVendorDelivery);
            this.gnVendorDelivery.Controls.Add(this.rbYesVendorDelivery);
            this.gnVendorDelivery.Location = new System.Drawing.Point(141, 0);
            this.gnVendorDelivery.Name = "gnVendorDelivery";
            this.gnVendorDelivery.Size = new System.Drawing.Size(237, 38);
            this.gnVendorDelivery.TabIndex = 101;
            this.gnVendorDelivery.TabStop = false;
            // 
            // rbNoVendorDelivery
            // 
            this.rbNoVendorDelivery.AutoSize = true;
            this.rbNoVendorDelivery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNoVendorDelivery.Location = new System.Drawing.Point(86, 10);
            this.rbNoVendorDelivery.Name = "rbNoVendorDelivery";
            this.rbNoVendorDelivery.Size = new System.Drawing.Size(47, 24);
            this.rbNoVendorDelivery.TabIndex = 1;
            this.rbNoVendorDelivery.TabStop = true;
            this.rbNoVendorDelivery.Text = "No";
            this.rbNoVendorDelivery.UseVisualStyleBackColor = true;
            // 
            // rbYesVendorDelivery
            // 
            this.rbYesVendorDelivery.AutoSize = true;
            this.rbYesVendorDelivery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbYesVendorDelivery.Location = new System.Drawing.Point(6, 8);
            this.rbYesVendorDelivery.Name = "rbYesVendorDelivery";
            this.rbYesVendorDelivery.Size = new System.Drawing.Size(55, 24);
            this.rbYesVendorDelivery.TabIndex = 0;
            this.rbYesVendorDelivery.TabStop = true;
            this.rbYesVendorDelivery.Text = "Yes";
            this.rbYesVendorDelivery.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(16, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 20);
            this.label11.TabIndex = 102;
            this.label11.Text = "Delivery Outlet";
            // 
            // cmbDeliveryOutlet
            // 
            this.cmbDeliveryOutlet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeliveryOutlet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDeliveryOutlet.FormattingEnabled = true;
            this.cmbDeliveryOutlet.Items.AddRange(new object[] {
            global::WinPowerPOS.Properties.Language.String1,
            "10am - 1pm",
            "12pm - 3pm",
            "2pm - 5pm"});
            this.cmbDeliveryOutlet.Location = new System.Drawing.Point(141, 8);
            this.cmbDeliveryOutlet.Name = "cmbDeliveryOutlet";
            this.cmbDeliveryOutlet.Size = new System.Drawing.Size(237, 28);
            this.cmbDeliveryOutlet.TabIndex = 103;
            // 
            // pnlDeliveryOutlet
            // 
            this.pnlDeliveryOutlet.Controls.Add(this.label11);
            this.pnlDeliveryOutlet.Controls.Add(this.cmbDeliveryOutlet);
            this.pnlDeliveryOutlet.Location = new System.Drawing.Point(495, 236);
            this.pnlDeliveryOutlet.Name = "pnlDeliveryOutlet";
            this.pnlDeliveryOutlet.Size = new System.Drawing.Size(381, 43);
            this.pnlDeliveryOutlet.TabIndex = 104;
            this.pnlDeliveryOutlet.Visible = false;
            // 
            // pnlVendorDelivery
            // 
            this.pnlVendorDelivery.Controls.Add(this.label10);
            this.pnlVendorDelivery.Controls.Add(this.gnVendorDelivery);
            this.pnlVendorDelivery.Location = new System.Drawing.Point(495, 192);
            this.pnlVendorDelivery.Name = "pnlVendorDelivery";
            this.pnlVendorDelivery.Size = new System.Drawing.Size(381, 39);
            this.pnlVendorDelivery.TabIndex = 105;
            this.pnlVendorDelivery.Visible = false;
            // 
            // frmDeliverySetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 617);
            this.Controls.Add(this.pnlVendorDelivery);
            this.Controls.Add(this.pnlDeliveryOutlet);
            this.Controls.Add(this.lblTickItems);
            this.Controls.Add(this.lblUnassignedDeposit);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAssign);
            this.Controls.Add(this.txtUnitNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtPostalCode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtHomeNo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMobileNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnChooseMember);
            this.Controls.Add(this.txtReceiptNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvDeliverySetup);
            this.Controls.Add(this.lblReceiptNo);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.cmbDeliveryTime);
            this.Controls.Add(this.dtpDeliveryDate);
            this.Controls.Add(this.txtMembershipNo);
            this.Controls.Add(this.label51);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.label48);
            this.Name = "frmDeliverySetup";
            this.Text = "Delivery Setup";
            this.Load += new System.EventHandler(this.frmDeliverySetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliverySetup)).EndInit();
            this.gnVendorDelivery.ResumeLayout(false);
            this.gnVendorDelivery.PerformLayout();
            this.pnlDeliveryOutlet.ResumeLayout(false);
            this.pnlDeliveryOutlet.PerformLayout();
            this.pnlVendorDelivery.ResumeLayout(false);
            this.pnlVendorDelivery.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDeliverySetup;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.ComboBox cmbDeliveryTime;
        private System.Windows.Forms.DateTimePicker dtpDeliveryDate;
        private System.Windows.Forms.TextBox txtMembershipNo;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Label lblReceiptNo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtReceiptNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnChooseMember;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.TextBox txtMobileNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHomeNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUnitNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAssign;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblUnassignedDeposit;
        private System.Windows.Forms.Label lblTickItems;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cbColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepositPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDetID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecipientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MobileNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn PostalCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox gnVendorDelivery;
        private System.Windows.Forms.RadioButton rbNoVendorDelivery;
        private System.Windows.Forms.RadioButton rbYesVendorDelivery;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbDeliveryOutlet;
        private System.Windows.Forms.Panel pnlDeliveryOutlet;
        private System.Windows.Forms.Panel pnlVendorDelivery;
    }
}