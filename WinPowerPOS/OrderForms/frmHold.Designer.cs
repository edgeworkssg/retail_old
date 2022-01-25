namespace WinPowerPOS.OrderForms
{
    partial class frmHold
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
            this.Tabel = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvcHoldNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcAppTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcMembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabel
            // 
            this.Tabel.AllowUserToAddRows = false;
            this.Tabel.AllowUserToDeleteRows = false;
            this.Tabel.AllowUserToResizeRows = false;
            this.Tabel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.Tabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Tabel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tabel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcHoldNo,
            this.dgvcAppTime,
            this.dgvcMembershipNo,
            this.dgvcName,
            this.NRIC,
            this.Total,
            this.LineInfo});
            this.Tabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.Tabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabel.Location = new System.Drawing.Point(11, 53);
            this.Tabel.Margin = new System.Windows.Forms.Padding(4);
            this.Tabel.Name = "Tabel";
            this.Tabel.ReadOnly = true;
            this.Tabel.RowHeadersVisible = false;
            this.Tabel.Size = new System.Drawing.Size(664, 389);
            this.Tabel.TabIndex = 0;
            this.Tabel.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellMouseLeave);
            this.Tabel.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellMouseEnter);
            this.Tabel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellContentClick);
            this.Tabel.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(11, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 43);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(625, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "HOLD";
            // 
            // dgvcHoldNo
            // 
            this.dgvcHoldNo.DataPropertyName = "HoldNo";
            this.dgvcHoldNo.HeaderText = "Hold No";
            this.dgvcHoldNo.Name = "dgvcHoldNo";
            this.dgvcHoldNo.ReadOnly = true;
            this.dgvcHoldNo.Visible = false;
            // 
            // dgvcAppTime
            // 
            this.dgvcAppTime.DataPropertyName = "AppTime";
            this.dgvcAppTime.HeaderText = "Hold Time";
            this.dgvcAppTime.MinimumWidth = 100;
            this.dgvcAppTime.Name = "dgvcAppTime";
            this.dgvcAppTime.ReadOnly = true;
            this.dgvcAppTime.Width = 140;
            // 
            // dgvcMembershipNo
            // 
            this.dgvcMembershipNo.DataPropertyName = "MembershipNo";
            this.dgvcMembershipNo.HeaderText = "Membership No";
            this.dgvcMembershipNo.Name = "dgvcMembershipNo";
            this.dgvcMembershipNo.ReadOnly = true;
            this.dgvcMembershipNo.Visible = false;
            // 
            // dgvcName
            // 
            this.dgvcName.DataPropertyName = "MembershipName";
            this.dgvcName.HeaderText = "Name";
            this.dgvcName.MinimumWidth = 100;
            this.dgvcName.Name = "dgvcName";
            this.dgvcName.ReadOnly = true;
            this.dgvcName.Width = 200;
            // 
            // NRIC
            // 
            this.NRIC.DataPropertyName = "NRIC";
            this.NRIC.HeaderText = "NRIC";
            this.NRIC.Name = "NRIC";
            this.NRIC.ReadOnly = true;
            // 
            // Total
            // 
            this.Total.DataPropertyName = "TotalAmount";
            this.Total.HeaderText = "Total";
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            // 
            // LineInfo
            // 
            this.LineInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LineInfo.DataPropertyName = "LineInfo";
            this.LineInfo.HeaderText = "Remarks";
            this.LineInfo.Name = "LineInfo";
            this.LineInfo.ReadOnly = true;
            this.LineInfo.Width = 88;
            // 
            // frmHold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.longdarkbg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(686, 452);
            this.ControlBox = false;
            this.Controls.Add(this.Tabel);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmHold";
            this.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmHold_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Tabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcHoldNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcAppTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcMembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineInfo;

    }
}