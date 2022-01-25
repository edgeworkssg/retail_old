namespace WinPowerPOS
{
    partial class frmReceiptList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReceiptList));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvRcpt = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLineInfo = new System.Windows.Forms.TextBox();
            this.lblLineInfo = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCashier = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnVoid = new System.Windows.Forms.Button();
            this.btnReprint = new System.Windows.Forms.Button();
            this.btnRefund = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.cbSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnColView = new System.Windows.Forms.DataGridViewButtonColumn();
            this.OrderRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CashierID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderHdrID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsVoided = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRcpt)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvRcpt
            // 
            this.dgvRcpt.AllowUserToAddRows = false;
            this.dgvRcpt.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvRcpt.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvRcpt, "dgvRcpt");
            this.dgvRcpt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRcpt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cbSelect,
            this.btnColView,
            this.OrderRefNo,
            this.LineInfo,
            this.Amount,
            this.TotalQty,
            this.OrderDate,
            this.Remark,
            this.CashierID,
            this.orderHdrID,
            this.IsVoided,
            this.PaymentMode,
            this.CardNo,
            this.NameToAppear});
            this.dgvRcpt.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvRcpt.Name = "dgvRcpt";
            this.dgvRcpt.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRcpt.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvRcpt.RowHeadersVisible = false;
            this.dgvRcpt.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRcpt_RowPostPaint);
            this.dgvRcpt.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRcpt_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // dtpEndDate
            // 
            resources.ApplyResources(this.dtpEndDate, "dtpEndDate");
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Name = "dtpEndDate";
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
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.txtLineInfo);
            this.groupBox2.Controls.Add(this.lblLineInfo);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtRemark);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbStatus);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtCashier);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtRefNo);
            this.groupBox2.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // txtLineInfo
            // 
            resources.ApplyResources(this.txtLineInfo, "txtLineInfo");
            this.txtLineInfo.Name = "txtLineInfo";
            // 
            // lblLineInfo
            // 
            resources.ApplyResources(this.lblLineInfo, "lblLineInfo");
            this.lblLineInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblLineInfo.Name = "lblLineInfo";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Name = "label7";
            // 
            // txtRemark
            // 
            resources.ApplyResources(this.txtRemark, "txtRemark");
            this.txtRemark.Name = "txtRemark";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            resources.GetString("cmbStatus.Items"),
            resources.GetString("cmbStatus.Items1"),
            resources.GetString("cmbStatus.Items2")});
            resources.ApplyResources(this.cmbStatus, "cmbStatus");
            this.cmbStatus.Name = "cmbStatus";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // txtCashier
            // 
            resources.ApplyResources(this.txtCashier, "txtCashier");
            this.txtCashier.Name = "txtCashier";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // txtRefNo
            // 
            resources.ApplyResources(this.txtRefNo, "txtRefNo");
            this.txtRefNo.Name = "txtRefNo";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.btnExport);
            this.groupBox4.Controls.Add(this.btnVoid);
            this.groupBox4.Controls.Add(this.btnReprint);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // btnExport
            // 
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnVoid
            // 
            resources.ApplyResources(this.btnVoid, "btnVoid");
            this.btnVoid.ForeColor = System.Drawing.Color.White;
            this.btnVoid.Name = "btnVoid";
            this.btnVoid.UseVisualStyleBackColor = true;
            this.btnVoid.Click += new System.EventHandler(this.btnVoid_Click);
            // 
            // btnReprint
            // 
            resources.ApplyResources(this.btnReprint, "btnReprint");
            this.btnReprint.ForeColor = System.Drawing.Color.White;
            this.btnReprint.Name = "btnReprint";
            this.btnReprint.UseVisualStyleBackColor = true;
            this.btnReprint.Click += new System.EventHandler(this.btnReprint_Click);
            // 
            // btnRefund
            // 
            resources.ApplyResources(this.btnRefund, "btnRefund");
            this.btnRefund.ForeColor = System.Drawing.Color.White;
            this.btnRefund.Name = "btnRefund";
            this.btnRefund.UseVisualStyleBackColor = true;
            this.btnRefund.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.btnRefund);
            this.groupBox5.Controls.Add(this.btnClose);
            this.groupBox5.Controls.Add(this.btnSearch);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            resources.ApplyResources(this.fsdExportToExcel, "fsdExportToExcel");
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // cbSelect
            // 
            this.cbSelect.Frozen = true;
            resources.ApplyResources(this.cbSelect, "cbSelect");
            this.cbSelect.Name = "cbSelect";
            this.cbSelect.ReadOnly = true;
            // 
            // btnColView
            // 
            this.btnColView.Frozen = true;
            resources.ApplyResources(this.btnColView, "btnColView");
            this.btnColView.Name = "btnColView";
            this.btnColView.ReadOnly = true;
            this.btnColView.Text = "View";
            this.btnColView.UseColumnTextForButtonValue = true;
            // 
            // OrderRefNo
            // 
            this.OrderRefNo.DataPropertyName = "OrderRefNo";
            this.OrderRefNo.Frozen = true;
            resources.ApplyResources(this.OrderRefNo, "OrderRefNo");
            this.OrderRefNo.Name = "OrderRefNo";
            this.OrderRefNo.ReadOnly = true;
            // 
            // LineInfo
            // 
            this.LineInfo.DataPropertyName = "LineInfo";
            this.LineInfo.Frozen = true;
            resources.ApplyResources(this.LineInfo, "LineInfo");
            this.LineInfo.Name = "LineInfo";
            this.LineInfo.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Amount.DataPropertyName = "Amount";
            dataGridViewCellStyle2.Format = "N";
            dataGridViewCellStyle2.NullValue = null;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle2;
            this.Amount.Frozen = true;
            resources.ApplyResources(this.Amount, "Amount");
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // TotalQty
            // 
            this.TotalQty.DataPropertyName = "Quantity";
            dataGridViewCellStyle3.Format = "N2";
            this.TotalQty.DefaultCellStyle = dataGridViewCellStyle3;
            this.TotalQty.FillWeight = 60F;
            this.TotalQty.Frozen = true;
            resources.ApplyResources(this.TotalQty, "TotalQty");
            this.TotalQty.Name = "TotalQty";
            this.TotalQty.ReadOnly = true;
            // 
            // OrderDate
            // 
            this.OrderDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OrderDate.DataPropertyName = "OrderDate";
            dataGridViewCellStyle4.Format = "dd MMM yyyy HH:mm:ss";
            dataGridViewCellStyle4.NullValue = "dd MMM yyyy HH:mm:ss";
            this.OrderDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.OrderDate.Frozen = true;
            resources.ApplyResources(this.OrderDate, "OrderDate");
            this.OrderDate.Name = "OrderDate";
            this.OrderDate.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Remark.DataPropertyName = "Remark";
            resources.ApplyResources(this.Remark, "Remark");
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // CashierID
            // 
            this.CashierID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CashierID.DataPropertyName = "CashierID";
            resources.ApplyResources(this.CashierID, "CashierID");
            this.CashierID.Name = "CashierID";
            this.CashierID.ReadOnly = true;
            // 
            // orderHdrID
            // 
            this.orderHdrID.DataPropertyName = "orderHdrID";
            resources.ApplyResources(this.orderHdrID, "orderHdrID");
            this.orderHdrID.Name = "orderHdrID";
            this.orderHdrID.ReadOnly = true;
            // 
            // IsVoided
            // 
            this.IsVoided.DataPropertyName = "IsVoided";
            resources.ApplyResources(this.IsVoided, "IsVoided");
            this.IsVoided.Name = "IsVoided";
            this.IsVoided.ReadOnly = true;
            // 
            // PaymentMode
            // 
            this.PaymentMode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PaymentMode.DataPropertyName = "PaymentType";
            resources.ApplyResources(this.PaymentMode, "PaymentMode");
            this.PaymentMode.Name = "PaymentMode";
            this.PaymentMode.ReadOnly = true;
            // 
            // CardNo
            // 
            this.CardNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CardNo.DataPropertyName = "MembershipNo";
            resources.ApplyResources(this.CardNo, "CardNo");
            this.CardNo.Name = "CardNo";
            this.CardNo.ReadOnly = true;
            // 
            // NameToAppear
            // 
            this.NameToAppear.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NameToAppear.DataPropertyName = "NameToAppear";
            resources.ApplyResources(this.NameToAppear, "NameToAppear");
            this.NameToAppear.Name = "NameToAppear";
            this.NameToAppear.ReadOnly = true;
            // 
            // frmReceiptList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dgvRcpt);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmReceiptList";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmReceiptList_Load);
            this.Activated += new System.EventHandler(this.frmReceiptList_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRcpt)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRcpt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtCashier;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRefNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        internal System.Windows.Forms.Button btnSearch;
        internal System.Windows.Forms.Button btnReprint;
        private System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnVoid;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLineInfo;
        private System.Windows.Forms.Label lblLineInfo;
        internal System.Windows.Forms.Button btnRefund;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cbSelect;
        private System.Windows.Forms.DataGridViewButtonColumn btnColView;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderRefNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn CashierID;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderHdrID;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsVoided;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
    }
}