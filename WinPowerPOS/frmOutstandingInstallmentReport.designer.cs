namespace WinPowerPOS
{
    partial class frmOutstandingInstallmentReport
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
            this.dgvInstallment = new System.Windows.Forms.DataGridView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.Search = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.dgvcOMemberNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOMemberName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOPhoneNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstallment)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvInstallment
            // 
            this.dgvInstallment.AllowUserToAddRows = false;
            this.dgvInstallment.AllowUserToDeleteRows = false;
            this.dgvInstallment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstallment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcOMemberNo,
            this.dgvcOMemberName,
            this.dgvcOPhoneNum,
            this.Mobile,
            this.Credit,
            this.dgvcOAmount,
            this.Balance});
            this.dgvInstallment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInstallment.Location = new System.Drawing.Point(0, 98);
            this.dgvInstallment.Name = "dgvInstallment";
            this.dgvInstallment.ReadOnly = true;
            this.dgvInstallment.RowHeadersVisible = false;
            this.dgvInstallment.Size = new System.Drawing.Size(738, 224);
            this.dgvInstallment.TabIndex = 3;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSearch.Controls.Add(this.Search);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnExport);
            this.pnlSearch.Controls.Add(this.btnConfirm);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(738, 98);
            this.pnlSearch.TabIndex = 4;
            // 
            // Search
            // 
            this.Search.AutoSize = true;
            this.Search.Location = new System.Drawing.Point(22, 25);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(51, 16);
            this.Search.TabIndex = 65;
            this.Search.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(95, 25);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(170, 22);
            this.txtSearch.TabIndex = 64;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExport.Location = new System.Drawing.Point(578, 20);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(132, 35);
            this.btnExport.TabIndex = 63;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnConfirm.CausesValidation = false;
            this.btnConfirm.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfirm.Location = new System.Drawing.Point(282, 20);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(0);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(128, 35);
            this.btnConfirm.TabIndex = 62;
            this.btnConfirm.Text = "SHOW";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            this.fsdExportToExcel.Title = "Save File";
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // dgvcOMemberNo
            // 
            this.dgvcOMemberNo.DataPropertyName = "MembershipNo";
            this.dgvcOMemberNo.HeaderText = "Membership No";
            this.dgvcOMemberNo.Name = "dgvcOMemberNo";
            this.dgvcOMemberNo.ReadOnly = true;
            // 
            // dgvcOMemberName
            // 
            this.dgvcOMemberName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcOMemberName.DataPropertyName = "NameToAppear";
            this.dgvcOMemberName.HeaderText = "Member Name";
            this.dgvcOMemberName.MinimumWidth = 150;
            this.dgvcOMemberName.Name = "dgvcOMemberName";
            this.dgvcOMemberName.ReadOnly = true;
            // 
            // dgvcOPhoneNum
            // 
            this.dgvcOPhoneNum.DataPropertyName = "Home";
            dataGridViewCellStyle1.NullValue = null;
            this.dgvcOPhoneNum.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvcOPhoneNum.HeaderText = "Home";
            this.dgvcOPhoneNum.Name = "dgvcOPhoneNum";
            this.dgvcOPhoneNum.ReadOnly = true;
            this.dgvcOPhoneNum.Width = 150;
            // 
            // Mobile
            // 
            this.Mobile.DataPropertyName = "Mobile";
            this.Mobile.HeaderText = "Mobile";
            this.Mobile.Name = "Mobile";
            this.Mobile.ReadOnly = true;
            // 
            // Credit
            // 
            this.Credit.DataPropertyName = "Credit";
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = null;
            this.Credit.DefaultCellStyle = dataGridViewCellStyle2;
            this.Credit.HeaderText = "Credit";
            this.Credit.Name = "Credit";
            this.Credit.ReadOnly = true;
            // 
            // dgvcOAmount
            // 
            this.dgvcOAmount.DataPropertyName = "Debit";
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = null;
            this.dgvcOAmount.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvcOAmount.HeaderText = "Debit";
            this.dgvcOAmount.Name = "dgvcOAmount";
            this.dgvcOAmount.ReadOnly = true;
            this.dgvcOAmount.Width = 150;
            // 
            // Balance
            // 
            this.Balance.DataPropertyName = "Balance";
            dataGridViewCellStyle4.Format = "C2";
            dataGridViewCellStyle4.NullValue = null;
            this.Balance.DefaultCellStyle = dataGridViewCellStyle4;
            this.Balance.HeaderText = "Balance";
            this.Balance.Name = "Balance";
            this.Balance.ReadOnly = true;
            // 
            // frmOutstandingInstallmentReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 322);
            this.Controls.Add(this.dgvInstallment);
            this.Controls.Add(this.pnlSearch);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmOutstandingInstallmentReport";
            this.Text = "Outstanding Installment Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstallment)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInstallment;
        private System.Windows.Forms.Panel pnlSearch;
        internal System.Windows.Forms.Button btnConfirm;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label Search;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOMemberNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOMemberName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOPhoneNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mobile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Credit;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
    }
}