namespace WinPowerPOS.PromoAdmin
{
    partial class frmViewPromotionDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewPromotionDetails));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbDays = new System.Windows.Forms.CheckedListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lblRestrictHourEnd = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblRestrictHourStart = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbIsRestrictHour = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.lblPriority = new System.Windows.Forms.Label();
            this.lblPromoName = new System.Windows.Forms.Label();
            this.lblPromoCode = new System.Windows.Forms.Label();
            this.lblOutlet = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rbMemberOnly = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AnyQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.lblBarcode);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.cbDays);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.lblRestrictHourEnd);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.lblRestrictHourStart);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cbIsRestrictHour);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lblDateTo);
            this.groupBox2.Controls.Add(this.lblDateFrom);
            this.groupBox2.Controls.Add(this.lblPriority);
            this.groupBox2.Controls.Add(this.lblPromoName);
            this.groupBox2.Controls.Add(this.lblPromoCode);
            this.groupBox2.Controls.Add(this.lblOutlet);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.rbMemberOnly);
            this.groupBox2.Controls.Add(this.rbAll);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(877, 245);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DETAILS";
            // 
            // cbDays
            // 
            this.cbDays.ColumnWidth = 40;
            this.cbDays.Enabled = false;
            this.cbDays.FormattingEnabled = true;
            this.cbDays.Items.AddRange(new object[] {
            "M",
            "Th",
            "W",
            "Tu",
            "F",
            "Sa",
            "Su"});
            this.cbDays.Location = new System.Drawing.Point(156, 149);
            this.cbDays.MultiColumn = true;
            this.cbDays.Name = "cbDays";
            this.cbDays.Size = new System.Drawing.Size(310, 19);
            this.cbDays.TabIndex = 41;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 149);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 13);
            this.label13.TabIndex = 40;
            this.label13.Text = "Days Applicable";
            // 
            // lblRestrictHourEnd
            // 
            this.lblRestrictHourEnd.AutoSize = true;
            this.lblRestrictHourEnd.Location = new System.Drawing.Point(427, 109);
            this.lblRestrictHourEnd.Name = "lblRestrictHourEnd";
            this.lblRestrictHourEnd.Size = new System.Drawing.Size(10, 13);
            this.lblRestrictHourEnd.TabIndex = 39;
            this.lblRestrictHourEnd.Text = "-";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(354, 109);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "End";
            // 
            // lblRestrictHourStart
            // 
            this.lblRestrictHourStart.AutoSize = true;
            this.lblRestrictHourStart.Location = new System.Drawing.Point(255, 109);
            this.lblRestrictHourStart.Name = "lblRestrictHourStart";
            this.lblRestrictHourStart.Size = new System.Drawing.Size(10, 13);
            this.lblRestrictHourStart.TabIndex = 37;
            this.lblRestrictHourStart.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(206, 109);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Start";
            // 
            // cbIsRestrictHour
            // 
            this.cbIsRestrictHour.AutoSize = true;
            this.cbIsRestrictHour.Enabled = false;
            this.cbIsRestrictHour.Location = new System.Drawing.Point(159, 109);
            this.cbIsRestrictHour.Name = "cbIsRestrictHour";
            this.cbIsRestrictHour.Size = new System.Drawing.Size(15, 14);
            this.cbIsRestrictHour.TabIndex = 35;
            this.cbIsRestrictHour.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(9, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 50);
            this.label8.TabIndex = 34;
            this.label8.Text = "Restrict to the hour of the day only";
            // 
            // lblDateTo
            // 
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.BackColor = System.Drawing.Color.Transparent;
            this.lblDateTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDateTo.Location = new System.Drawing.Point(424, 77);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(10, 13);
            this.lblDateTo.TabIndex = 33;
            this.lblDateTo.Text = "-";
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.BackColor = System.Drawing.Color.Transparent;
            this.lblDateFrom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDateFrom.Location = new System.Drawing.Point(156, 80);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(10, 13);
            this.lblDateFrom.TabIndex = 32;
            this.lblDateFrom.Text = "-";
            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.BackColor = System.Drawing.Color.Transparent;
            this.lblPriority.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPriority.Location = new System.Drawing.Point(424, 50);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(10, 13);
            this.lblPriority.TabIndex = 31;
            this.lblPriority.Text = "-";
            // 
            // lblPromoName
            // 
            this.lblPromoName.AutoSize = true;
            this.lblPromoName.BackColor = System.Drawing.Color.Transparent;
            this.lblPromoName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPromoName.Location = new System.Drawing.Point(156, 50);
            this.lblPromoName.Name = "lblPromoName";
            this.lblPromoName.Size = new System.Drawing.Size(10, 13);
            this.lblPromoName.TabIndex = 30;
            this.lblPromoName.Text = "-";
            // 
            // lblPromoCode
            // 
            this.lblPromoCode.AutoSize = true;
            this.lblPromoCode.BackColor = System.Drawing.Color.Transparent;
            this.lblPromoCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPromoCode.Location = new System.Drawing.Point(156, 24);
            this.lblPromoCode.Name = "lblPromoCode";
            this.lblPromoCode.Size = new System.Drawing.Size(10, 13);
            this.lblPromoCode.TabIndex = 29;
            this.lblPromoCode.Text = "-";
            // 
            // lblOutlet
            // 
            this.lblOutlet.AutoSize = true;
            this.lblOutlet.BackColor = System.Drawing.Color.Transparent;
            this.lblOutlet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblOutlet.Location = new System.Drawing.Point(153, 207);
            this.lblOutlet.Name = "lblOutlet";
            this.lblOutlet.Size = new System.Drawing.Size(10, 13);
            this.lblOutlet.TabIndex = 28;
            this.lblOutlet.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(9, 207);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Outlet";
            // 
            // rbMemberOnly
            // 
            this.rbMemberOnly.AutoSize = true;
            this.rbMemberOnly.Location = new System.Drawing.Point(207, 175);
            this.rbMemberOnly.Name = "rbMemberOnly";
            this.rbMemberOnly.Size = new System.Drawing.Size(87, 17);
            this.rbMemberOnly.TabIndex = 26;
            this.rbMemberOnly.TabStop = true;
            this.rbMemberOnly.Text = "Member Only";
            this.rbMemberOnly.UseVisualStyleBackColor = true;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(156, 175);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(36, 17);
            this.rbAll.TabIndex = 25;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(6, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Applicable To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(354, 77);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "End";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(354, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Priority";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(8, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Promo Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(8, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Promo Code";
            // 
            // dgvDetails
            // 
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.AllowUserToResizeRows = false;
            this.dgvDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDetails.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CategoryName,
            this.ItemNo,
            this.ItemName,
            this.UnitQty,
            this.AnyQty,
            this.RetailPrice,
            this.PromoPrice,
            this.DiscPercent,
            this.DiscAmount});
            this.dgvDetails.Location = new System.Drawing.Point(13, 263);
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.Size = new System.Drawing.Size(876, 264);
            this.dgvDetails.TabIndex = 28;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(803, 533);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 35);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CategoryName
            // 
            this.CategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CategoryName.DataPropertyName = "CategoryName";
            this.CategoryName.HeaderText = "Category";
            this.CategoryName.Name = "CategoryName";
            // 
            // ItemNo
            // 
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.HeaderText = "Item No";
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.Width = 69;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.HeaderText = "Product Name";
            this.ItemName.Name = "ItemName";
            // 
            // UnitQty
            // 
            this.UnitQty.DataPropertyName = "UnitQty";
            this.UnitQty.HeaderText = "QTY";
            this.UnitQty.Name = "UnitQty";
            this.UnitQty.Width = 54;
            // 
            // AnyQty
            // 
            this.AnyQty.DataPropertyName = "AnyQty";
            this.AnyQty.HeaderText = "ANY";
            this.AnyQty.Name = "AnyQty";
            this.AnyQty.Width = 54;
            // 
            // RetailPrice
            // 
            this.RetailPrice.DataPropertyName = "RetailPrice";
            this.RetailPrice.HeaderText = "RetailPrice";
            this.RetailPrice.Name = "RetailPrice";
            this.RetailPrice.Width = 83;
            // 
            // PromoPrice
            // 
            this.PromoPrice.DataPropertyName = "PromoPrice";
            this.PromoPrice.HeaderText = "Promo Price";
            this.PromoPrice.Name = "PromoPrice";
            this.PromoPrice.Width = 89;
            // 
            // DiscPercent
            // 
            this.DiscPercent.DataPropertyName = "DiscPercent";
            this.DiscPercent.HeaderText = "Disc %";
            this.DiscPercent.Name = "DiscPercent";
            this.DiscPercent.Width = 64;
            // 
            // DiscAmount
            // 
            this.DiscAmount.DataPropertyName = "DiscAmount";
            this.DiscAmount.HeaderText = "Disc $";
            this.DiscAmount.Name = "DiscAmount";
            this.DiscAmount.Width = 62;
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.BackColor = System.Drawing.Color.Transparent;
            this.lblBarcode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblBarcode.Location = new System.Drawing.Point(424, 24);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(10, 13);
            this.lblBarcode.TabIndex = 43;
            this.lblBarcode.Text = "-";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(354, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 42;
            this.label12.Text = "Barcode";
            // 
            // frmViewPromotionDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 580);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvDetails);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.Name = "frmViewPromotionDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Promotion Details";
            this.Load += new System.EventHandler(this.frmViewPromotion_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbMemberOnly;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Label lblOutlet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.Label lblPromoName;
        private System.Windows.Forms.Label lblPromoCode;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.Label lblDateFrom;
        internal System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbIsRestrictHour;
        private System.Windows.Forms.Label lblRestrictHourEnd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblRestrictHourStart;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckedListBox cbDays;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn AnyQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn RetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscAmount;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.Label label12;
    }
}