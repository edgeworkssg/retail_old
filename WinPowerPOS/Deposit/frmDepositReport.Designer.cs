namespace WinPowerPOS.DepositForms
{
    partial class frmDepositReport
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
            this.btnName = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvInstallment = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRefNo = new System.Windows.Forms.Button();
            this.btnNRIC = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Office = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstallment)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnName
            // 
            this.btnName.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnName.ForeColor = System.Drawing.Color.White;
            this.btnName.Location = new System.Drawing.Point(11, 51);
            this.btnName.Name = "btnName";
            this.btnName.Size = new System.Drawing.Size(92, 31);
            this.btnName.TabIndex = 54;
            this.btnName.Text = "Name";
            this.btnName.UseVisualStyleBackColor = true;
            this.btnName.Click += new System.EventHandler(this.btnName_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(11, 19);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(258, 26);
            this.txtSearch.TabIndex = 11;
            // 
            // dgvInstallment
            // 
            this.dgvInstallment.AllowUserToAddRows = false;
            this.dgvInstallment.AllowUserToDeleteRows = false;
            this.dgvInstallment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInstallment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstallment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.View,
            this.NRIC,
            this.MembershipNo,
            this.NameToAppear,
            this.Amount,
            this.Mobile,
            this.Office,
            this.email,
            this.Address});
            this.dgvInstallment.Location = new System.Drawing.Point(1, 108);
            this.dgvInstallment.Name = "dgvInstallment";
            this.dgvInstallment.ReadOnly = true;
            this.dgvInstallment.RowHeadersVisible = false;
            this.dgvInstallment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvInstallment.Size = new System.Drawing.Size(901, 451);
            this.dgvInstallment.TabIndex = 51;
            this.dgvInstallment.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvInstallment_RowPrePaint);
            this.dgvInstallment.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInstallment_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnRefNo);
            this.groupBox1.Controls.Add(this.btnNRIC);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.btnName);
            this.groupBox1.Location = new System.Drawing.Point(1, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 90);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SEARCH";
            // 
            // btnRefNo
            // 
            this.btnRefNo.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnRefNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefNo.ForeColor = System.Drawing.Color.White;
            this.btnRefNo.Location = new System.Drawing.Point(207, 51);
            this.btnRefNo.Name = "btnRefNo";
            this.btnRefNo.Size = new System.Drawing.Size(153, 31);
            this.btnRefNo.TabIndex = 57;
            this.btnRefNo.Text = "Membership No";
            this.btnRefNo.UseVisualStyleBackColor = true;
            this.btnRefNo.Click += new System.EventHandler(this.btnRefNo_Click);
            // 
            // btnNRIC
            // 
            this.btnNRIC.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnNRIC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNRIC.ForeColor = System.Drawing.Color.White;
            this.btnNRIC.Location = new System.Drawing.Point(109, 51);
            this.btnNRIC.Name = "btnNRIC";
            this.btnNRIC.Size = new System.Drawing.Size(92, 31);
            this.btnNRIC.TabIndex = 55;
            this.btnNRIC.Text = "NRIC";
            this.btnNRIC.UseVisualStyleBackColor = true;
            this.btnNRIC.Click += new System.EventHandler(this.btnNRIC_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.btnExport);
            this.groupBox2.Location = new System.Drawing.Point(465, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 90);
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            // 
            // btnExport
            // 
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(307, 51);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(124, 33);
            this.btnExport.TabIndex = 58;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            this.fsdExportToExcel.Title = "Save File";
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // View
            // 
            this.View.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.View.HeaderText = "";
            this.View.Name = "View";
            this.View.ReadOnly = true;
            this.View.Text = "View";
            this.View.UseColumnTextForButtonValue = true;
            this.View.Width = 70;
            // 
            // NRIC
            // 
            this.NRIC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NRIC.DataPropertyName = "NRIC";
            this.NRIC.HeaderText = "NRIC";
            this.NRIC.MinimumWidth = 80;
            this.NRIC.Name = "NRIC";
            this.NRIC.ReadOnly = true;
            this.NRIC.Width = 80;
            // 
            // MembershipNo
            // 
            this.MembershipNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MembershipNo.DataPropertyName = "MembershipNo";
            this.MembershipNo.HeaderText = "Membership No";
            this.MembershipNo.MinimumWidth = 120;
            this.MembershipNo.Name = "MembershipNo";
            this.MembershipNo.ReadOnly = true;
            this.MembershipNo.Width = 120;
            // 
            // NameToAppear
            // 
            this.NameToAppear.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NameToAppear.DataPropertyName = "NameToAppear";
            this.NameToAppear.HeaderText = "Name";
            this.NameToAppear.Name = "NameToAppear";
            this.NameToAppear.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Amount.DataPropertyName = "Amount";
            dataGridViewCellStyle1.Format = "C2";
            dataGridViewCellStyle1.NullValue = "0";
            this.Amount.DefaultCellStyle = dataGridViewCellStyle1;
            this.Amount.HeaderText = "Amount";
            this.Amount.MinimumWidth = 70;
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Width = 70;
            // 
            // Mobile
            // 
            this.Mobile.HeaderText = "Mobile";
            this.Mobile.MinimumWidth = 100;
            this.Mobile.Name = "Mobile";
            this.Mobile.ReadOnly = true;
            // 
            // Office
            // 
            this.Office.DataPropertyName = "Office";
            this.Office.HeaderText = "Office No";
            this.Office.MinimumWidth = 100;
            this.Office.Name = "Office";
            this.Office.ReadOnly = true;
            // 
            // email
            // 
            this.email.DataPropertyName = "email";
            this.email.HeaderText = "email";
            this.email.MinimumWidth = 150;
            this.email.Name = "email";
            this.email.ReadOnly = true;
            this.email.Width = 150;
            // 
            // Address
            // 
            this.Address.DataPropertyName = "streetname";
            this.Address.HeaderText = "Address";
            this.Address.MinimumWidth = 150;
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Width = 150;
            // 
            // frmDepositReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(914, 571);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvInstallment);
            this.Name = "frmDepositReport";
            this.Text = "Deposit Balance Report";
            this.Load += new System.EventHandler(this.frmDepositReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstallment)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnName;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvInstallment;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRefNo;
        private System.Windows.Forms.Button btnNRIC;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.DataGridViewButtonColumn View;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mobile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Office;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
    }
}