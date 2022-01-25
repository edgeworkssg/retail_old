namespace WinPowerPOS.PromoAdmin
{
    partial class frmListOfDiscountByItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListOfDiscountByItem));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnEndDate = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnStartDate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvCampaignList = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnName = new System.Windows.Forms.Button();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.txtMisc = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CampaignName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.PromoCampaignDetID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoCampaignHdrID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCampaignList)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.btnEndDate);
            this.groupBox2.Controls.Add(this.dtpTo);
            this.groupBox2.Controls.Add(this.dtpFrom);
            this.groupBox2.Controls.Add(this.btnStartDate);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(2, 3);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(320, 114);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Date Filter";
            // 
            // btnEndDate
            // 
            this.btnEndDate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEndDate.BackgroundImage")));
            this.btnEndDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEndDate.ForeColor = System.Drawing.Color.White;
            this.btnEndDate.Location = new System.Drawing.Point(207, 77);
            this.btnEndDate.Margin = new System.Windows.Forms.Padding(4);
            this.btnEndDate.Name = "btnEndDate";
            this.btnEndDate.Size = new System.Drawing.Size(100, 29);
            this.btnEndDate.TabIndex = 8;
            this.btnEndDate.Text = "End Date";
            this.btnEndDate.UseVisualStyleBackColor = true;
            this.btnEndDate.Click += new System.EventHandler(this.btnEndDate_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dddd, dd MMMM yyyy HH:mm:ss";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(57, 49);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(250, 20);
            this.dtpTo.TabIndex = 7;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dddd, dd MMMM yyyy HH:mm:ss";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(57, 21);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(250, 20);
            this.dtpFrom.TabIndex = 6;
            // 
            // btnStartDate
            // 
            this.btnStartDate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStartDate.BackgroundImage")));
            this.btnStartDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartDate.ForeColor = System.Drawing.Color.White;
            this.btnStartDate.Location = new System.Drawing.Point(99, 77);
            this.btnStartDate.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartDate.Name = "btnStartDate";
            this.btnStartDate.Size = new System.Drawing.Size(100, 29);
            this.btnStartDate.TabIndex = 5;
            this.btnStartDate.Text = "Start Date";
            this.btnStartDate.UseVisualStyleBackColor = true;
            this.btnStartDate.Click += new System.EventHandler(this.btnStartDate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 53);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "To";
            // 
            // dgvCampaignList
            // 
            this.dgvCampaignList.AllowUserToAddRows = false;
            this.dgvCampaignList.AllowUserToDeleteRows = false;
            this.dgvCampaignList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCampaignList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCampaignList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Delete,
            this.CampaignName,
            this.ItemNo,
            this.ItemName,
            this.PromoDiscount,
            this.DateFrom,
            this.DateEnd,
            this.Column1,
            this.PromoCampaignDetID,
            this.PromoCampaignHdrID});
            this.dgvCampaignList.Location = new System.Drawing.Point(2, 125);
            this.dgvCampaignList.Margin = new System.Windows.Forms.Padding(4);
            this.dgvCampaignList.Name = "dgvCampaignList";
            this.dgvCampaignList.RowHeadersVisible = false;
            this.dgvCampaignList.Size = new System.Drawing.Size(891, 450);
            this.dgvCampaignList.TabIndex = 8;
            this.dgvCampaignList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCampaignList_CellClick);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.btnName);
            this.groupBox3.Controls.Add(this.btnDiscount);
            this.groupBox3.Controls.Add(this.txtMisc);
            this.groupBox3.Location = new System.Drawing.Point(330, 3);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(284, 114);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Misc Filter";
            // 
            // btnName
            // 
            this.btnName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnName.BackgroundImage")));
            this.btnName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnName.ForeColor = System.Drawing.Color.White;
            this.btnName.Location = new System.Drawing.Point(56, 60);
            this.btnName.Margin = new System.Windows.Forms.Padding(4);
            this.btnName.Name = "btnName";
            this.btnName.Size = new System.Drawing.Size(100, 29);
            this.btnName.TabIndex = 6;
            this.btnName.Text = "Name";
            this.btnName.UseVisualStyleBackColor = true;
            this.btnName.Click += new System.EventHandler(this.btnName_Click);
            // 
            // btnDiscount
            // 
            this.btnDiscount.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDiscount.BackgroundImage")));
            this.btnDiscount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiscount.ForeColor = System.Drawing.Color.White;
            this.btnDiscount.Location = new System.Drawing.Point(164, 60);
            this.btnDiscount.Margin = new System.Windows.Forms.Padding(4);
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(100, 29);
            this.btnDiscount.TabIndex = 5;
            this.btnDiscount.Text = "Discount";
            this.btnDiscount.UseVisualStyleBackColor = true;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // txtMisc
            // 
            this.txtMisc.Location = new System.Drawing.Point(8, 32);
            this.txtMisc.Margin = new System.Windows.Forms.Padding(4);
            this.txtMisc.Name = "txtMisc";
            this.txtMisc.Size = new System.Drawing.Size(255, 20);
            this.txtMisc.TabIndex = 2;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNew.BackgroundImage")));
            this.btnNew.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(793, 80);
            this.btnNew.Margin = new System.Windows.Forms.Padding(4);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(100, 37);
            this.btnNew.TabIndex = 10;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // Delete
            // 
            this.Delete.HeaderText = "";
            this.Delete.Name = "Delete";
            this.Delete.Text = "Delete";
            this.Delete.UseColumnTextForButtonValue = true;
            this.Delete.Width = 70;
            // 
            // CampaignName
            // 
            this.CampaignName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CampaignName.DataPropertyName = "PromoCampaignName";
            this.CampaignName.FillWeight = 1F;
            this.CampaignName.HeaderText = "Campaign Name";
            this.CampaignName.MinimumWidth = 180;
            this.CampaignName.Name = "CampaignName";
            // 
            // ItemNo
            // 
            this.ItemNo.DataPropertyName = "ItemNo";
            dataGridViewCellStyle1.Format = "C2";
            dataGridViewCellStyle1.NullValue = null;
            this.ItemNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.ItemNo.HeaderText = "Item No";
            this.ItemNo.MinimumWidth = 120;
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.Width = 120;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.HeaderText = "Item Name";
            this.ItemName.MinimumWidth = 160;
            this.ItemName.Name = "ItemName";
            // 
            // PromoDiscount
            // 
            this.PromoDiscount.DataPropertyName = "PromoDiscount";
            this.PromoDiscount.HeaderText = "Disc (%)";
            this.PromoDiscount.MinimumWidth = 50;
            this.PromoDiscount.Name = "PromoDiscount";
            this.PromoDiscount.Width = 50;
            // 
            // DateFrom
            // 
            this.DateFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DateFrom.DataPropertyName = "DateFrom";
            this.DateFrom.HeaderText = "Date Start";
            this.DateFrom.MinimumWidth = 100;
            this.DateFrom.Name = "DateFrom";
            // 
            // DateEnd
            // 
            this.DateEnd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DateEnd.DataPropertyName = "DateTo";
            this.DateEnd.HeaderText = "Date End";
            this.DateEnd.MinimumWidth = 100;
            this.DateEnd.Name = "DateEnd";
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Text = "Location";
            this.Column1.UseColumnTextForButtonValue = true;
            this.Column1.Width = 70;
            // 
            // PromoCampaignDetID
            // 
            this.PromoCampaignDetID.DataPropertyName = "PromoCampaignDetID";
            this.PromoCampaignDetID.HeaderText = "PromoCampaignDetID";
            this.PromoCampaignDetID.Name = "PromoCampaignDetID";
            this.PromoCampaignDetID.Visible = false;
            // 
            // PromoCampaignHdrID
            // 
            this.PromoCampaignHdrID.DataPropertyName = "PromoCampaignHdrID";
            this.PromoCampaignHdrID.HeaderText = "PromoCampaignHdrID";
            this.PromoCampaignHdrID.Name = "PromoCampaignHdrID";
            this.PromoCampaignHdrID.Visible = false;
            // 
            // frmListOfDiscountByItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(895, 588);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.dgvCampaignList);
            this.Controls.Add(this.groupBox2);
            this.Name = "frmListOfDiscountByItem";
            this.Text = "Discount By Item";
            this.Load += new System.EventHandler(this.frmListOfDiscountByItem_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCampaignList)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnEndDate;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnStartDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvCampaignList;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnName;
        private System.Windows.Forms.Button btnDiscount;
        private System.Windows.Forms.TextBox txtMisc;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn CampaignName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateEnd;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoCampaignDetID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoCampaignHdrID;
    }
}