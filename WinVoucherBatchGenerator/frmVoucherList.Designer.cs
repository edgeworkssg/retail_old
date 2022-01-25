namespace WinVoucherBatchGenerator
{
    partial class frmVoucherList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVoucherList));
            this.dgvVoucherList = new System.Windows.Forms.DataGridView();
            this.VoucherNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateIssued = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateRedeemed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVoucherNoFrom = new System.Windows.Forms.TextBox();
            this.txtVoucherNoTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnExpiry = new System.Windows.Forms.Button();
            this.btnRedeemed = new System.Windows.Forms.Button();
            this.btnSold = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnIssue = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAmount = new System.Windows.Forms.Button();
            this.btnRemark = new System.Windows.Forms.Button();
            this.txtMisc = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVoucherList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvVoucherList
            // 
            this.dgvVoucherList.AllowUserToAddRows = false;
            this.dgvVoucherList.AllowUserToDeleteRows = false;
            this.dgvVoucherList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVoucherList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVoucherList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VoucherNo,
            this.Amount,
            this.Remark,
            this.DateIssued,
            this.DateSold,
            this.DateRedeemed,
            this.ExpiryDate,
            this.Status});
            this.dgvVoucherList.Location = new System.Drawing.Point(4, 154);
            this.dgvVoucherList.Margin = new System.Windows.Forms.Padding(4);
            this.dgvVoucherList.Name = "dgvVoucherList";
            this.dgvVoucherList.RowHeadersVisible = false;
            this.dgvVoucherList.Size = new System.Drawing.Size(1069, 649);
            this.dgvVoucherList.TabIndex = 0;
            // 
            // VoucherNo
            // 
            this.VoucherNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.VoucherNo.DataPropertyName = "VoucherNo";
            this.VoucherNo.HeaderText = "Voucher No";
            this.VoucherNo.Name = "VoucherNo";
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Amount.DataPropertyName = "Amount";
            dataGridViewCellStyle1.Format = "C2";
            dataGridViewCellStyle1.NullValue = null;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle1;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Remark.DataPropertyName = "Remark";
            this.Remark.HeaderText = "Remark";
            this.Remark.Name = "Remark";
            // 
            // DateIssued
            // 
            this.DateIssued.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DateIssued.DataPropertyName = "DateIssued";
            this.DateIssued.HeaderText = "Date Issued";
            this.DateIssued.Name = "DateIssued";
            // 
            // DateSold
            // 
            this.DateSold.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DateSold.DataPropertyName = "DateSold";
            this.DateSold.HeaderText = "Date Sold";
            this.DateSold.Name = "DateSold";
            // 
            // DateRedeemed
            // 
            this.DateRedeemed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DateRedeemed.DataPropertyName = "DateRedeemed";
            this.DateRedeemed.HeaderText = "Date Redeemed";
            this.DateRedeemed.Name = "DateRedeemed";
            // 
            // ExpiryDate
            // 
            this.ExpiryDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ExpiryDate.DataPropertyName = "ExpiryDate";
            this.ExpiryDate.HeaderText = "Expiry Date";
            this.ExpiryDate.Name = "ExpiryDate";
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.DataPropertyName = "VoucherStatusId";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "From";
            // 
            // txtVoucherNoFrom
            // 
            this.txtVoucherNoFrom.Location = new System.Drawing.Point(57, 32);
            this.txtVoucherNoFrom.Margin = new System.Windows.Forms.Padding(4);
            this.txtVoucherNoFrom.Name = "txtVoucherNoFrom";
            this.txtVoucherNoFrom.Size = new System.Drawing.Size(205, 26);
            this.txtVoucherNoFrom.TabIndex = 2;
            // 
            // txtVoucherNoTo
            // 
            this.txtVoucherNoTo.Location = new System.Drawing.Point(57, 68);
            this.txtVoucherNoTo.Margin = new System.Windows.Forms.Padding(4);
            this.txtVoucherNoTo.Name = "txtVoucherNoTo";
            this.txtVoucherNoTo.Size = new System.Drawing.Size(205, 26);
            this.txtVoucherNoTo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 72);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "To";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtVoucherNoTo);
            this.groupBox1.Controls.Add(this.txtVoucherNoFrom);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(284, 143);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Voucher No";
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(164, 105);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 29);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnExpiry);
            this.groupBox2.Controls.Add(this.btnRedeemed);
            this.groupBox2.Controls.Add(this.btnSold);
            this.groupBox2.Controls.Add(this.dtpTo);
            this.groupBox2.Controls.Add(this.dtpFrom);
            this.groupBox2.Controls.Add(this.btnIssue);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(296, 3);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(456, 143);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Date Filter";
            // 
            // btnExpiry
            // 
            this.btnExpiry.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExpiry.BackgroundImage")));
            this.btnExpiry.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpiry.ForeColor = System.Drawing.Color.White;
            this.btnExpiry.Location = new System.Drawing.Point(336, 105);
            this.btnExpiry.Margin = new System.Windows.Forms.Padding(4);
            this.btnExpiry.Name = "btnExpiry";
            this.btnExpiry.Size = new System.Drawing.Size(100, 29);
            this.btnExpiry.TabIndex = 10;
            this.btnExpiry.Text = "Expiry";
            this.btnExpiry.UseVisualStyleBackColor = true;
            this.btnExpiry.Click += new System.EventHandler(this.btnExpiry_Click);
            // 
            // btnRedeemed
            // 
            this.btnRedeemed.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRedeemed.BackgroundImage")));
            this.btnRedeemed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRedeemed.ForeColor = System.Drawing.Color.White;
            this.btnRedeemed.Location = new System.Drawing.Point(228, 105);
            this.btnRedeemed.Margin = new System.Windows.Forms.Padding(4);
            this.btnRedeemed.Name = "btnRedeemed";
            this.btnRedeemed.Size = new System.Drawing.Size(100, 29);
            this.btnRedeemed.TabIndex = 9;
            this.btnRedeemed.Text = "Redeemed";
            this.btnRedeemed.UseVisualStyleBackColor = true;
            this.btnRedeemed.Click += new System.EventHandler(this.btnRedeemed_Click);
            // 
            // btnSold
            // 
            this.btnSold.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSold.BackgroundImage")));
            this.btnSold.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSold.ForeColor = System.Drawing.Color.White;
            this.btnSold.Location = new System.Drawing.Point(120, 105);
            this.btnSold.Margin = new System.Windows.Forms.Padding(4);
            this.btnSold.Name = "btnSold";
            this.btnSold.Size = new System.Drawing.Size(100, 29);
            this.btnSold.TabIndex = 8;
            this.btnSold.Text = "Sold";
            this.btnSold.UseVisualStyleBackColor = true;
            this.btnSold.Click += new System.EventHandler(this.btnSold_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dddd, dd MMMM yyyy HH:mm:ss";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(57, 68);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(377, 26);
            this.dtpTo.TabIndex = 7;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dddd, dd MMMM yyyy HH:mm:ss";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(57, 32);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(377, 26);
            this.dtpFrom.TabIndex = 6;
            // 
            // btnIssue
            // 
            this.btnIssue.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnIssue.BackgroundImage")));
            this.btnIssue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIssue.ForeColor = System.Drawing.Color.White;
            this.btnIssue.Location = new System.Drawing.Point(12, 105);
            this.btnIssue.Margin = new System.Windows.Forms.Padding(4);
            this.btnIssue.Name = "btnIssue";
            this.btnIssue.Size = new System.Drawing.Size(100, 29);
            this.btnIssue.TabIndex = 5;
            this.btnIssue.Text = "Issued";
            this.btnIssue.UseVisualStyleBackColor = true;
            this.btnIssue.Click += new System.EventHandler(this.btnIssue_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 36);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 72);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "To";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAmount);
            this.groupBox3.Controls.Add(this.btnRemark);
            this.groupBox3.Controls.Add(this.txtMisc);
            this.groupBox3.Location = new System.Drawing.Point(760, 3);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(284, 143);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Misc Filter";
            // 
            // btnAmount
            // 
            this.btnAmount.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAmount.BackgroundImage")));
            this.btnAmount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAmount.ForeColor = System.Drawing.Color.White;
            this.btnAmount.Location = new System.Drawing.Point(56, 72);
            this.btnAmount.Margin = new System.Windows.Forms.Padding(4);
            this.btnAmount.Name = "btnAmount";
            this.btnAmount.Size = new System.Drawing.Size(100, 29);
            this.btnAmount.TabIndex = 6;
            this.btnAmount.Text = "Amount";
            this.btnAmount.UseVisualStyleBackColor = true;
            this.btnAmount.Click += new System.EventHandler(this.btnAmount_Click);
            // 
            // btnRemark
            // 
            this.btnRemark.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemark.BackgroundImage")));
            this.btnRemark.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemark.ForeColor = System.Drawing.Color.White;
            this.btnRemark.Location = new System.Drawing.Point(164, 72);
            this.btnRemark.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemark.Name = "btnRemark";
            this.btnRemark.Size = new System.Drawing.Size(100, 29);
            this.btnRemark.TabIndex = 5;
            this.btnRemark.Text = "Remark";
            this.btnRemark.UseVisualStyleBackColor = true;
            this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
            // 
            // txtMisc
            // 
            this.txtMisc.Location = new System.Drawing.Point(8, 32);
            this.txtMisc.Margin = new System.Windows.Forms.Padding(4);
            this.txtMisc.Name = "txtMisc";
            this.txtMisc.Size = new System.Drawing.Size(255, 26);
            this.txtMisc.TabIndex = 2;
            // 
            // frmVoucherList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 820);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvVoucherList);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmVoucherList";
            this.Text = "Voucher List";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmVoucherList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVoucherList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVoucherList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVoucherNoFrom;
        private System.Windows.Forms.TextBox txtVoucherNoTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExpiry;
        private System.Windows.Forms.Button btnRedeemed;
        private System.Windows.Forms.Button btnSold;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnIssue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnAmount;
        private System.Windows.Forms.Button btnRemark;
        private System.Windows.Forms.TextBox txtMisc;
        private System.Windows.Forms.DataGridViewTextBoxColumn VoucherNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateIssued;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateSold;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateRedeemed;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}