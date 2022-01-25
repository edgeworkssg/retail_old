namespace WinPowerPOS.Package
{
    partial class frmRedeemPackage
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
            this.cmbStylist = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMembershipNo = new System.Windows.Forms.TextBox();
            this.txtNameOf = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRedeem = new System.Windows.Forms.Button();
            this.dgvRedeem = new System.Windows.Forms.DataGridView();
            this.PackageRedeemID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RedeemDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StylistID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmountPerVisit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedeem)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbStylist
            // 
            this.cmbStylist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStylist.FormattingEnabled = true;
            this.cmbStylist.Location = new System.Drawing.Point(403, 26);
            this.cmbStylist.Name = "cmbStylist";
            this.cmbStylist.Size = new System.Drawing.Size(168, 21);
            this.cmbStylist.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(282, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "STYLIST";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(11, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "MEMBERSHIP NO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(11, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "NAME";
            // 
            // txtMembershipNo
            // 
            this.txtMembershipNo.Location = new System.Drawing.Point(115, 26);
            this.txtMembershipNo.Name = "txtMembershipNo";
            this.txtMembershipNo.Size = new System.Drawing.Size(136, 20);
            this.txtMembershipNo.TabIndex = 4;
            // 
            // txtNameOf
            // 
            this.txtNameOf.Location = new System.Drawing.Point(115, 57);
            this.txtNameOf.Name = "txtNameOf";
            this.txtNameOf.Size = new System.Drawing.Size(136, 20);
            this.txtNameOf.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(282, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "AMOUNT PER VISIT";
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(403, 53);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(168, 20);
            this.txtAmount.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnRedeem);
            this.groupBox1.Controls.Add(this.txtAmount);
            this.groupBox1.Controls.Add(this.cmbStylist);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtNameOf);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMembershipNo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(584, 125);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Redeem Packages";
            // 
            // btnRedeem
            // 
            this.btnRedeem.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnRedeem.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnRedeem.ForeColor = System.Drawing.Color.White;
            this.btnRedeem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRedeem.Location = new System.Drawing.Point(486, 82);
            this.btnRedeem.Name = "btnRedeem";
            this.btnRedeem.Size = new System.Drawing.Size(85, 37);
            this.btnRedeem.TabIndex = 32;
            this.btnRedeem.Text = "Redeem";
            this.btnRedeem.UseVisualStyleBackColor = true;
            this.btnRedeem.Click += new System.EventHandler(this.btnRedeem_Click);
            // 
            // dgvRedeem
            // 
            this.dgvRedeem.AllowUserToAddRows = false;
            this.dgvRedeem.AllowUserToDeleteRows = false;
            this.dgvRedeem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRedeem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRedeem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PackageRedeemID,
            this.RedeemDate,
            this.StylistID,
            this.AmountPerVisit,
            this.MembershipNo,
            this.NameCol});
            this.dgvRedeem.Location = new System.Drawing.Point(12, 143);
            this.dgvRedeem.Name = "dgvRedeem";
            this.dgvRedeem.RowHeadersVisible = false;
            this.dgvRedeem.Size = new System.Drawing.Size(585, 434);
            this.dgvRedeem.TabIndex = 9;
            // 
            // PackageRedeemID
            // 
            this.PackageRedeemID.HeaderText = "ID";
            this.PackageRedeemID.Name = "PackageRedeemID";
            this.PackageRedeemID.Visible = false;
            // 
            // RedeemDate
            // 
            this.RedeemDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RedeemDate.DataPropertyName = "PackageRedeemDate";
            this.RedeemDate.HeaderText = "Redeem Date";
            this.RedeemDate.Name = "RedeemDate";
            // 
            // StylistID
            // 
            this.StylistID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StylistID.DataPropertyName = "DisplayName";
            this.StylistID.HeaderText = "Stylist";
            this.StylistID.Name = "StylistID";
            // 
            // AmountPerVisit
            // 
            this.AmountPerVisit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AmountPerVisit.DataPropertyName = "Amount";
            this.AmountPerVisit.HeaderText = "Amount Per Visit";
            this.AmountPerVisit.Name = "AmountPerVisit";
            // 
            // MembershipNo
            // 
            this.MembershipNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MembershipNo.DataPropertyName = "MembershipNo";
            this.MembershipNo.HeaderText = "Membership No";
            this.MembershipNo.Name = "MembershipNo";
            // 
            // NameCol
            // 
            this.NameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NameCol.DataPropertyName = "Name";
            this.NameCol.HeaderText = "Customer Name";
            this.NameCol.Name = "NameCol";
            // 
            // frmRedeemPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(609, 589);
            this.Controls.Add(this.dgvRedeem);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmRedeemPackage";
            this.Text = "Redeem Package";
            this.Load += new System.EventHandler(this.frmRedeemPackage_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedeem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbStylist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMembershipNo;
        private System.Windows.Forms.TextBox txtNameOf;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button btnRedeem;
        private System.Windows.Forms.DataGridView dgvRedeem;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackageRedeemID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RedeemDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn StylistID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmountPerVisit;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameCol;
    }
}