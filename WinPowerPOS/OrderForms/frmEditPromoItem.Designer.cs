namespace PowerPOS
{
    partial class frmEditPromoItem
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblPromoName = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.dgvPurchase = new FixReEntrantDataGridView();
            this.sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colMin = new System.Windows.Forms.DataGridViewButtonColumn();
            this.IsVoided = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DiscDollar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceLevel = new System.Windows.Forms.DataGridViewButtonColumn();
            this.GSTCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSpecial = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsPreOrder = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FOC = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineCommission = new System.Windows.Forms.DataGridViewButtonColumn();
            this.IsPromo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsNonDiscountable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsExchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPromoName
            // 
            this.lblPromoName.AutoSize = true;
            this.lblPromoName.BackColor = System.Drawing.Color.Transparent;
            this.lblPromoName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromoName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPromoName.Location = new System.Drawing.Point(14, 9);
            this.lblPromoName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPromoName.Name = "lblPromoName";
            this.lblPromoName.Size = new System.Drawing.Size(111, 20);
            this.lblPromoName.TabIndex = 54;
            this.lblPromoName.Text = "Promo Name";
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(387, 276);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(106, 59);
            this.btnOk.TabIndex = 55;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(505, 276);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 59);
            this.btnCancel.TabIndex = 56;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.ForeColor = System.Drawing.Color.Red;
            this.lblResult.Location = new System.Drawing.Point(15, 65);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 16);
            this.lblResult.TabIndex = 57;
            // 
            // dgvPurchase
            // 
            this.dgvPurchase.AllowUserToAddRows = false;
            this.dgvPurchase.AllowUserToDeleteRows = false;
            this.dgvPurchase.AllowUserToResizeColumns = false;
            this.dgvPurchase.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvPurchase.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPurchase.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPurchase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sno,
            this.ItemNo,
            this.ItemName,
            this.Quantity,
            this.Price,
            this.colAdd,
            this.colMin,
            this.IsVoided,
            this.DiscDollar,
            this.Discount,
            this.DiscountDetail,
            this.PriceLevel,
            this.GSTCol,
            this.Amount,
            this.IsSpecial,
            this.IsPreOrder,
            this.FOC,
            this.ID,
            this.LineCommission,
            this.IsPromo,
            this.IsNonDiscountable,
            this.IsExchange});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPurchase.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvPurchase.Location = new System.Drawing.Point(12, 34);
            this.dgvPurchase.Name = "dgvPurchase";
            this.dgvPurchase.RowHeadersVisible = false;
            this.dgvPurchase.RowHeadersWidth = 23;
            this.dgvPurchase.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPurchase.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.RowTemplate.Height = 50;
            this.dgvPurchase.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvPurchase.Size = new System.Drawing.Size(601, 233);
            this.dgvPurchase.TabIndex = 58;
            this.dgvPurchase.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvPurchase_RowPostPaint);
            this.dgvPurchase.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchase_CellClick);
            this.dgvPurchase.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchase_CellContentClick);
            // 
            // sno
            // 
            this.sno.HeaderText = "sno";
            this.sno.Name = "sno";
            this.sno.Width = 27;
            // 
            // ItemNo
            // 
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.HeaderText = "ItemNo";
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            this.ItemNo.Width = 150;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemName.DefaultCellStyle = dataGridViewCellStyle2;
            this.ItemName.HeaderText = "Description";
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "quantity";
            this.Quantity.HeaderText = "Qty";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.Width = 40;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "price";
            this.Price.HeaderText = "R.Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Visible = false;
            this.Price.Width = 60;
            // 
            // colAdd
            // 
            this.colAdd.HeaderText = "+";
            this.colAdd.Name = "colAdd";
            this.colAdd.Text = "+";
            this.colAdd.UseColumnTextForButtonValue = true;
            this.colAdd.Width = 50;
            // 
            // colMin
            // 
            this.colMin.HeaderText = "-";
            this.colMin.Name = "colMin";
            this.colMin.Text = "-";
            this.colMin.UseColumnTextForButtonValue = true;
            this.colMin.Width = 50;
            // 
            // IsVoided
            // 
            this.IsVoided.DataPropertyName = "IsVoided";
            this.IsVoided.HeaderText = "IsVoided";
            this.IsVoided.Name = "IsVoided";
            this.IsVoided.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.IsVoided.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsVoided.Text = "X";
            this.IsVoided.UseColumnTextForButtonValue = true;
            this.IsVoided.Width = 30;
            // 
            // DiscDollar
            // 
            this.DiscDollar.DataPropertyName = "DPrice";
            this.DiscDollar.HeaderText = "D.Price";
            this.DiscDollar.Name = "DiscDollar";
            this.DiscDollar.ReadOnly = true;
            this.DiscDollar.Visible = false;
            this.DiscDollar.Width = 60;
            // 
            // Discount
            // 
            this.Discount.DataPropertyName = "Disc(%)";
            this.Discount.HeaderText = "Disc(%)";
            this.Discount.Name = "Discount";
            this.Discount.ReadOnly = true;
            this.Discount.Visible = false;
            this.Discount.Width = 60;
            // 
            // DiscountDetail
            // 
            this.DiscountDetail.DataPropertyName = "DiscountDetail";
            this.DiscountDetail.HeaderText = "Discount";
            this.DiscountDetail.Name = "DiscountDetail";
            this.DiscountDetail.ReadOnly = true;
            this.DiscountDetail.Visible = false;
            // 
            // PriceLevel
            // 
            this.PriceLevel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PriceLevel.DataPropertyName = "PriceLevelName";
            this.PriceLevel.FillWeight = 75F;
            this.PriceLevel.HeaderText = "Apply";
            this.PriceLevel.MinimumWidth = 75;
            this.PriceLevel.Name = "PriceLevel";
            this.PriceLevel.Text = "Disc";
            this.PriceLevel.Visible = false;
            this.PriceLevel.Width = 75;
            // 
            // GSTCol
            // 
            this.GSTCol.DataPropertyName = "GSTAmount";
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = null;
            this.GSTCol.DefaultCellStyle = dataGridViewCellStyle3;
            this.GSTCol.HeaderText = "Tax";
            this.GSTCol.MinimumWidth = 60;
            this.GSTCol.Name = "GSTCol";
            this.GSTCol.Visible = false;
            this.GSTCol.Width = 60;
            // 
            // Amount
            // 
            this.Amount.DataPropertyName = "Amount";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Amount.DefaultCellStyle = dataGridViewCellStyle4;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Visible = false;
            this.Amount.Width = 70;
            // 
            // IsSpecial
            // 
            this.IsSpecial.DataPropertyName = "IsSpecial";
            this.IsSpecial.HeaderText = "S";
            this.IsSpecial.Name = "IsSpecial";
            this.IsSpecial.Visible = false;
            this.IsSpecial.Width = 30;
            // 
            // IsPreOrder
            // 
            this.IsPreOrder.DataPropertyName = "IsPreOrder";
            this.IsPreOrder.HeaderText = "O";
            this.IsPreOrder.Name = "IsPreOrder";
            this.IsPreOrder.Visible = false;
            this.IsPreOrder.Width = 30;
            // 
            // FOC
            // 
            this.FOC.DataPropertyName = "IsFreeOfCharge";
            this.FOC.HeaderText = "FOC";
            this.FOC.Name = "FOC";
            this.FOC.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FOC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.FOC.Visible = false;
            this.FOC.Width = 40;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // LineCommission
            // 
            this.LineCommission.HeaderText = "LineCommission";
            this.LineCommission.Name = "LineCommission";
            this.LineCommission.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LineCommission.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LineCommission.Text = "+";
            this.LineCommission.UseColumnTextForButtonValue = true;
            this.LineCommission.Visible = false;
            this.LineCommission.Width = 35;
            // 
            // IsPromo
            // 
            this.IsPromo.DataPropertyName = "IsPromo";
            this.IsPromo.HeaderText = "Promo";
            this.IsPromo.Name = "IsPromo";
            this.IsPromo.Visible = false;
            // 
            // IsNonDiscountable
            // 
            this.IsNonDiscountable.DataPropertyName = "IsNonDiscountable";
            this.IsNonDiscountable.HeaderText = "IsNonDiscountable";
            this.IsNonDiscountable.Name = "IsNonDiscountable";
            this.IsNonDiscountable.Visible = false;
            // 
            // IsExchange
            // 
            this.IsExchange.DataPropertyName = "IsExchange";
            this.IsExchange.HeaderText = "IsExchange";
            this.IsExchange.Name = "IsExchange";
            this.IsExchange.Visible = false;
            // 
            // frmEditPromoItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 350);
            this.Controls.Add(this.dgvPurchase);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblPromoName);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditPromoItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Promo Item";
            this.Load += new System.EventHandler(this.frmLineInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPromoName;
        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblResult;
        private FixReEntrantDataGridView dgvPurchase;
        private System.Windows.Forms.DataGridViewTextBoxColumn sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewButtonColumn colAdd;
        private System.Windows.Forms.DataGridViewButtonColumn colMin;
        private System.Windows.Forms.DataGridViewButtonColumn IsVoided;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscDollar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountDetail;
        private System.Windows.Forms.DataGridViewButtonColumn PriceLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn GSTCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSpecial;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsPreOrder;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewButtonColumn LineCommission;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsPromo;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsNonDiscountable;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsExchange;
    }
}