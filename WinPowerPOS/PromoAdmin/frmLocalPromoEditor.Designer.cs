namespace WinPowerPOS.PromoAdmin
{
    partial class frmLocalPromoEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLocalPromoEditor));
            this.DateTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPromo = new System.Windows.Forms.DataGridView();
            this.PromoCampaignHdrId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoCampaignHdrName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CampaignType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromo)).BeginInit();
            this.SuspendLayout();
            // 
            // DateTo
            // 
            this.DateTo.DataPropertyName = "DateTo";
            this.DateTo.HeaderText = "结束日期 Date To";
            this.DateTo.Name = "DateTo";
            this.DateTo.Width = 130;
            // 
            // dgvPromo
            // 
            this.dgvPromo.AllowUserToAddRows = false;
            this.dgvPromo.AllowUserToDeleteRows = false;
            this.dgvPromo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPromo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPromo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPromo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PromoCampaignHdrId,
            this.PromoCampaignHdrName,
            this.CampaignType,
            this.DateFrom,
            this.DateTo,
            this.Enabled});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPromo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPromo.Location = new System.Drawing.Point(12, 63);
            this.dgvPromo.Name = "dgvPromo";
            this.dgvPromo.RowHeadersVisible = false;
            this.dgvPromo.Size = new System.Drawing.Size(989, 651);
            this.dgvPromo.TabIndex = 26;
            this.dgvPromo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPromo_CellContentClick);
            // 
            // PromoCampaignHdrId
            // 
            this.PromoCampaignHdrId.DataPropertyName = "PromoCampaignHdrId";
            this.PromoCampaignHdrId.HeaderText = "PromoCampaignHdrId";
            this.PromoCampaignHdrId.Name = "PromoCampaignHdrId";
            this.PromoCampaignHdrId.Visible = false;
            this.PromoCampaignHdrId.Width = 250;
            // 
            // PromoCampaignHdrName
            // 
            this.PromoCampaignHdrName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PromoCampaignHdrName.DataPropertyName = "PromoCampaignName";
            this.PromoCampaignHdrName.HeaderText = "促销名字 Campaign Name";
            this.PromoCampaignHdrName.Name = "PromoCampaignHdrName";
            // 
            // CampaignType
            // 
            this.CampaignType.DataPropertyName = "CampaignType";
            this.CampaignType.HeaderText = "促销类型 Campaign Type";
            this.CampaignType.Name = "CampaignType";
            this.CampaignType.Width = 180;
            // 
            // DateFrom
            // 
            this.DateFrom.DataPropertyName = "DateFrom";
            this.DateFrom.HeaderText = "开始日期 Date From";
            this.DateFrom.Name = "DateFrom";
            this.DateFrom.Width = 130;
            // 
            // Enabled
            // 
            this.Enabled.DataPropertyName = "Enabled";
            this.Enabled.HeaderText = "启动 Enabled";
            this.Enabled.Name = "Enabled";
            this.Enabled.Width = 80;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(12, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 45);
            this.btnClose.TabIndex = 27;
            this.btnClose.Text = "关闭 CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmLocalPromoEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1013, 726);
            this.ControlBox = false;
            this.Controls.Add(this.dgvPromo);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmLocalPromoEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmLocalPromoEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn DateTo;
        private System.Windows.Forms.DataGridView dgvPromo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoCampaignHdrId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoCampaignHdrName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CampaignType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateFrom;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enabled;
        internal System.Windows.Forms.Button btnClose;
    }
}