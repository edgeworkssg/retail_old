namespace WinPowerPOS.PromoAdmin
{
    partial class frmPromotionList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPromotionList));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LblOutlet = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvPromotionList = new System.Windows.Forms.DataGridView();
            this.ViewColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.PromoCampaignHdrID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsRestrictHour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApplicableDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApplicableTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Outlet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotionList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.LblOutlet);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.dtpDate);
            this.groupBox2.Controls.Add(this.txtSearch);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(827, 113);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter";
            // 
            // LblOutlet
            // 
            this.LblOutlet.AutoSize = true;
            this.LblOutlet.Location = new System.Drawing.Point(79, 84);
            this.LblOutlet.Name = "LblOutlet";
            this.LblOutlet.Size = new System.Drawing.Size(35, 13);
            this.LblOutlet.TabIndex = 18;
            this.LblOutlet.Text = "label4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Outlet";
            // 
            // dtpDate
            // 
            this.dtpDate.Checked = false;
            this.dtpDate.CustomFormat = "dddd, dd MMMM yyyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(80, 50);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.ShowCheckBox = true;
            this.dtpDate.Size = new System.Drawing.Size(250, 20);
            this.dtpDate.TabIndex = 6;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(80, 21);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(399, 20);
            this.txtSearch.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 56);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Valid On";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Search";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.btnClose);
            this.groupBox5.Controls.Add(this.btnSearch);
            this.groupBox5.Location = new System.Drawing.Point(13, 129);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(827, 57);
            this.groupBox5.TabIndex = 27;
            this.groupBox5.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(100, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 35);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(9, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 35);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvPromotionList
            // 
            this.dgvPromotionList.AllowUserToAddRows = false;
            this.dgvPromotionList.AllowUserToDeleteRows = false;
            this.dgvPromotionList.AllowUserToResizeRows = false;
            this.dgvPromotionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPromotionList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPromotionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPromotionList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ViewColumn,
            this.PromoCampaignHdrID,
            this.PromoCode,
            this.PromoName,
            this.Priority,
            this.DateFrom,
            this.DateTo,
            this.IsRestrictHour,
            this.ApplicableDays,
            this.ApplicableTo,
            this.PriceDiscount,
            this.CategoryName,
            this.ItemName,
            this.UnitQty,
            this.ItemNo,
            this.PromoPrice,
            this.PromoDiscount,
            this.Outlet});
            this.dgvPromotionList.Location = new System.Drawing.Point(13, 194);
            this.dgvPromotionList.MultiSelect = false;
            this.dgvPromotionList.Name = "dgvPromotionList";
            this.dgvPromotionList.Size = new System.Drawing.Size(827, 360);
            this.dgvPromotionList.TabIndex = 28;
            this.dgvPromotionList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPromotionList_CellFormatting);
            this.dgvPromotionList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPromotionList_CellContentClick);
            // 
            // ViewColumn
            // 
            this.ViewColumn.HeaderText = "view";
            this.ViewColumn.Name = "ViewColumn";
            this.ViewColumn.Text = "View";
            this.ViewColumn.UseColumnTextForButtonValue = true;
            this.ViewColumn.Width = 35;
            // 
            // PromoCampaignHdrID
            // 
            this.PromoCampaignHdrID.DataPropertyName = "PromoCampaignHdrID";
            this.PromoCampaignHdrID.HeaderText = "PromoCampaignHdrID";
            this.PromoCampaignHdrID.Name = "PromoCampaignHdrID";
            this.PromoCampaignHdrID.Visible = false;
            this.PromoCampaignHdrID.Width = 137;
            // 
            // PromoCode
            // 
            this.PromoCode.DataPropertyName = "PromoCode";
            this.PromoCode.HeaderText = "Promo Code";
            this.PromoCode.Name = "PromoCode";
            this.PromoCode.Width = 83;
            // 
            // PromoName
            // 
            this.PromoName.DataPropertyName = "PromoCampaignName";
            this.PromoName.HeaderText = "Promo Name";
            this.PromoName.Name = "PromoName";
            this.PromoName.Width = 86;
            // 
            // Priority
            // 
            this.Priority.DataPropertyName = "Priority";
            this.Priority.HeaderText = "Priority";
            this.Priority.Name = "Priority";
            this.Priority.Width = 63;
            // 
            // DateFrom
            // 
            this.DateFrom.DataPropertyName = "DateFrom";
            this.DateFrom.HeaderText = "Start";
            this.DateFrom.Name = "DateFrom";
            this.DateFrom.Width = 54;
            // 
            // DateTo
            // 
            this.DateTo.DataPropertyName = "DateTo";
            this.DateTo.HeaderText = "End";
            this.DateTo.Name = "DateTo";
            this.DateTo.Width = 51;
            // 
            // IsRestrictHour
            // 
            this.IsRestrictHour.DataPropertyName = "IsRestrictHour";
            this.IsRestrictHour.HeaderText = "Hour";
            this.IsRestrictHour.Name = "IsRestrictHour";
            this.IsRestrictHour.Width = 55;
            // 
            // ApplicableDays
            // 
            this.ApplicableDays.DataPropertyName = "ApplicableDays";
            this.ApplicableDays.HeaderText = "Days";
            this.ApplicableDays.Name = "ApplicableDays";
            this.ApplicableDays.Width = 56;
            // 
            // ApplicableTo
            // 
            this.ApplicableTo.DataPropertyName = "ForNonMembersAlso";
            this.ApplicableTo.HeaderText = "Applicable To";
            this.ApplicableTo.Name = "ApplicableTo";
            this.ApplicableTo.Width = 89;
            // 
            // PriceDiscount
            // 
            this.PriceDiscount.DataPropertyName = "PriceDiscount";
            this.PriceDiscount.HeaderText = "Disc (%) / Price ($)";
            this.PriceDiscount.Name = "PriceDiscount";
            this.PriceDiscount.Visible = false;
            this.PriceDiscount.Width = 120;
            // 
            // CategoryName
            // 
            this.CategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CategoryName.DataPropertyName = "CategoryName";
            this.CategoryName.HeaderText = "Category Name";
            this.CategoryName.Name = "CategoryName";
            // 
            // ItemName
            // 
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.HeaderText = "Product Name";
            this.ItemName.Name = "ItemName";
            this.ItemName.Width = 92;
            // 
            // UnitQty
            // 
            this.UnitQty.DataPropertyName = "UnitQty";
            this.UnitQty.HeaderText = "Qty";
            this.UnitQty.Name = "UnitQty";
            this.UnitQty.Width = 48;
            // 
            // ItemNo
            // 
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.HeaderText = "ItemNo";
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.Visible = false;
            this.ItemNo.Width = 66;
            // 
            // PromoPrice
            // 
            this.PromoPrice.DataPropertyName = "PromoPrice";
            this.PromoPrice.HeaderText = "PromoPrice";
            this.PromoPrice.Name = "PromoPrice";
            this.PromoPrice.Visible = false;
            this.PromoPrice.Width = 86;
            // 
            // PromoDiscount
            // 
            this.PromoDiscount.DataPropertyName = "PromoDiscount";
            this.PromoDiscount.HeaderText = "PromoDiscount";
            this.PromoDiscount.Name = "PromoDiscount";
            this.PromoDiscount.Visible = false;
            this.PromoDiscount.Width = 104;
            // 
            // Outlet
            // 
            this.Outlet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Outlet.DataPropertyName = "OutletName";
            this.Outlet.HeaderText = "Outlet";
            this.Outlet.Name = "Outlet";
            // 
            // frmPromotionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 566);
            this.Controls.Add(this.dgvPromotionList);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Name = "frmPromotionList";
            this.Text = "Promotion List";
            this.Load += new System.EventHandler(this.frmPromotionList_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotionList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvPromotionList;
        private System.Windows.Forms.Label LblOutlet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewButtonColumn ViewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoCampaignHdrID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsRestrictHour;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApplicableDays;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApplicableTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Outlet;
    }
}