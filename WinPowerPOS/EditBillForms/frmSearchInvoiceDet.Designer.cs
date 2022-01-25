namespace WinPowerPOS.EditBillForms
{
    partial class frmSearchInvoiceDet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchInvoiceDet));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvRcpt = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.OrderDetDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderHdrId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Disc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderDetID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsVoided = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryHdrRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRcpt)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.txtSearch);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txtSearch
            // 
            this.txtSearch.AccessibleDescription = null;
            this.txtSearch.AccessibleName = null;
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.BackgroundImage = null;
            this.txtSearch.Font = null;
            this.txtSearch.Name = "txtSearch";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = null;
            this.label5.Name = "label5";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.AccessibleDescription = null;
            this.dtpEndDate.AccessibleName = null;
            resources.ApplyResources(this.dtpEndDate, "dtpEndDate");
            this.dtpEndDate.BackgroundImage = null;
            this.dtpEndDate.CalendarFont = null;
            this.dtpEndDate.Font = null;
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Name = "dtpEndDate";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.AccessibleDescription = null;
            this.dtpStartDate.AccessibleName = null;
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.BackgroundImage = null;
            this.dtpStartDate.CalendarFont = null;
            this.dtpStartDate.Font = null;
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Name = "dtpStartDate";
            // 
            // btnExport
            // 
            this.btnExport.AccessibleDescription = null;
            this.btnExport.AccessibleName = null;
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleDescription = null;
            this.btnClose.AccessibleName = null;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleDescription = null;
            this.btnSearch.AccessibleName = null;
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvRcpt
            // 
            this.dgvRcpt.AccessibleDescription = null;
            this.dgvRcpt.AccessibleName = null;
            this.dgvRcpt.AllowUserToAddRows = false;
            this.dgvRcpt.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvRcpt.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvRcpt, "dgvRcpt");
            this.dgvRcpt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvRcpt.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvRcpt.BackgroundImage = null;
            this.dgvRcpt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRcpt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderDetDate,
            this.OrderHdrId,
            this.Department,
            this.Category,
            this.ItemNo,
            this.ItemName,
            this.Quantity,
            this.UnitPrice,
            this.Disc,
            this.Amount,
            this.orderDetID,
            this.IsVoided,
            this.InventoryHdrRefNo,
            this.Remark,
            this.Remark1});
            this.dgvRcpt.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvRcpt.Font = null;
            this.dgvRcpt.Name = "dgvRcpt";
            this.dgvRcpt.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRcpt.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRcpt.RowHeadersVisible = false;
            this.dgvRcpt.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvRcpt_RowPrePaint);
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleDescription = null;
            this.groupBox3.AccessibleName = null;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.BackgroundImage = null;
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Font = null;
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label7
            // 
            this.label7.AccessibleDescription = null;
            this.label7.AccessibleName = null;
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = null;
            this.label7.Name = "label7";
            // 
            // label8
            // 
            this.label8.AccessibleDescription = null;
            this.label8.AccessibleName = null;
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = null;
            this.label8.Name = "label8";
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            resources.ApplyResources(this.fsdExportToExcel, "fsdExportToExcel");
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // OrderDetDate
            // 
            this.OrderDetDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OrderDetDate.DataPropertyName = "OrderDetDate";
            resources.ApplyResources(this.OrderDetDate, "OrderDetDate");
            this.OrderDetDate.Name = "OrderDetDate";
            this.OrderDetDate.ReadOnly = true;
            // 
            // OrderHdrId
            // 
            this.OrderHdrId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OrderHdrId.DataPropertyName = "OrderRefNo";
            resources.ApplyResources(this.OrderHdrId, "OrderHdrId");
            this.OrderHdrId.Name = "OrderHdrId";
            this.OrderHdrId.ReadOnly = true;
            // 
            // Department
            // 
            this.Department.DataPropertyName = "ItemDepartmentID";
            resources.ApplyResources(this.Department, "Department");
            this.Department.Name = "Department";
            this.Department.ReadOnly = true;
            // 
            // Category
            // 
            this.Category.DataPropertyName = "CategoryName";
            resources.ApplyResources(this.Category, "Category");
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            // 
            // ItemNo
            // 
            this.ItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Quantity.DataPropertyName = "Quantity";
            resources.ApplyResources(this.Quantity, "Quantity");
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // UnitPrice
            // 
            this.UnitPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UnitPrice.DataPropertyName = "UnitPrice";
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = null;
            this.UnitPrice.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.UnitPrice, "UnitPrice");
            this.UnitPrice.Name = "UnitPrice";
            this.UnitPrice.ReadOnly = true;
            // 
            // Disc
            // 
            this.Disc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Disc.DataPropertyName = "Discount";
            resources.ApplyResources(this.Disc, "Disc");
            this.Disc.Name = "Disc";
            this.Disc.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Amount.DataPropertyName = "Amount";
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = null;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Amount, "Amount");
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // orderDetID
            // 
            this.orderDetID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.orderDetID.DataPropertyName = "orderDetID";
            resources.ApplyResources(this.orderDetID, "orderDetID");
            this.orderDetID.Name = "orderDetID";
            this.orderDetID.ReadOnly = true;
            // 
            // IsVoided
            // 
            this.IsVoided.DataPropertyName = "IsVoided";
            resources.ApplyResources(this.IsVoided, "IsVoided");
            this.IsVoided.Name = "IsVoided";
            this.IsVoided.ReadOnly = true;
            // 
            // InventoryHdrRefNo
            // 
            this.InventoryHdrRefNo.DataPropertyName = "InventoryHdrRefNo";
            resources.ApplyResources(this.InventoryHdrRefNo, "InventoryHdrRefNo");
            this.InventoryHdrRefNo.Name = "InventoryHdrRefNo";
            this.InventoryHdrRefNo.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Remark.DataPropertyName = "Remark";
            this.Remark.FillWeight = 130F;
            resources.ApplyResources(this.Remark, "Remark");
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // Remark1
            // 
            this.Remark1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Remark1.DataPropertyName = "Remark1";
            this.Remark1.FillWeight = 130F;
            resources.ApplyResources(this.Remark1, "Remark1");
            this.Remark1.Name = "Remark1";
            this.Remark1.ReadOnly = true;
            // 
            // frmSearchInvoiceDet
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvRcpt);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.DoubleBuffered = true;
            this.Font = null;
            this.Icon = null;
            this.Name = "frmSearchInvoiceDet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSearchInvoiceDet_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRcpt)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvRcpt;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDetDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderHdrId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Department;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Disc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderDetID;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsVoided;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryHdrRefNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark1;
    }
}