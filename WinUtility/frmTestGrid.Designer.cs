namespace WinUtility
{
    partial class frmTestGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnLoadSample = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLogDetail = new System.Windows.Forms.RichTextBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.dgvPurchase = new FixReEntrantDataGridView();
            this.sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.longDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscDollar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceLevel = new System.Windows.Forms.DataGridViewButtonColumn();
            this.GSTCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSpecial = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsPreOrder = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FOC = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcSalesPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcSalesPerson2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsVoided = new System.Windows.Forms.DataGridViewButtonColumn();
            this.LineCommission = new System.Windows.Forms.DataGridViewButtonColumn();
            this.IsPromo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsNonDiscountable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsExchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadSample
            // 
            this.btnLoadSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSample.Location = new System.Drawing.Point(417, 12);
            this.btnLoadSample.Name = "btnLoadSample";
            this.btnLoadSample.Size = new System.Drawing.Size(77, 29);
            this.btnLoadSample.TabIndex = 58;
            this.btnLoadSample.Text = "SAMPLE";
            this.btnLoadSample.UseVisualStyleBackColor = true;
            this.btnLoadSample.Click += new System.EventHandler(this.btnLoadSample_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(334, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(77, 29);
            this.btnClear.TabIndex = 57;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(251, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(77, 29);
            this.btnAdd.TabIndex = 56;
            this.btnAdd.Text = "ADD";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtBarcode
            // 
            this.txtBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.Location = new System.Drawing.Point(12, 12);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(233, 29);
            this.txtBarcode.TabIndex = 55;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            this.txtBarcode.Leave += new System.EventHandler(this.txtBarcode_Leave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.txtLogDetail, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLog, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 338);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(482, 219);
            this.tableLayoutPanel1.TabIndex = 59;
            // 
            // txtLogDetail
            // 
            this.txtLogDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogDetail.Location = new System.Drawing.Point(244, 3);
            this.txtLogDetail.Name = "txtLogDetail";
            this.txtLogDetail.Size = new System.Drawing.Size(235, 213);
            this.txtLogDetail.TabIndex = 1;
            this.txtLogDetail.Text = "  ";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(235, 213);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "   ";
            // 
            // dgvPurchase
            // 
            this.dgvPurchase.AllowUserToAddRows = false;
            this.dgvPurchase.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvPurchase.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPurchase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPurchase.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPurchase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sno,
            this.ItemNo,
            this.ItemName,
            this.longDesc,
            this.Quantity,
            this.Price,
            this.DiscDollar,
            this.Discount,
            this.DiscountDetail,
            this.PriceLevel,
            this.GSTCol,
            this.Amount,
            this.IsSpecial,
            this.IsPreOrder,
            this.FOC,
            this.dgvcSalesPerson,
            this.dgvcSalesPerson2,
            this.ID,
            this.IsVoided,
            this.LineCommission,
            this.IsPromo,
            this.IsNonDiscountable,
            this.IsExchange});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvPurchase.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvPurchase.Location = new System.Drawing.Point(12, 47);
            this.dgvPurchase.Name = "dgvPurchase";
            this.dgvPurchase.RowHeadersVisible = false;
            this.dgvPurchase.RowHeadersWidth = 23;
            this.dgvPurchase.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.RowTemplate.Height = 33;
            this.dgvPurchase.Size = new System.Drawing.Size(479, 288);
            this.dgvPurchase.TabIndex = 60;
            this.dgvPurchase.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvPurchase_RowPrePaint);
            this.dgvPurchase.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvPurchase_RowPostPaint);
            this.dgvPurchase.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvPurchase_CellPainting);
            this.dgvPurchase.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPurchase_DataBindingComplete);
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
            this.ItemNo.Width = 175;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemName.DefaultCellStyle = dataGridViewCellStyle7;
            this.ItemName.HeaderText = "Description";
            this.ItemName.MinimumWidth = 150;
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // longDesc
            // 
            this.longDesc.DataPropertyName = "itemdesc";
            this.longDesc.HeaderText = "Description";
            this.longDesc.Name = "longDesc";
            this.longDesc.ReadOnly = true;
            this.longDesc.Visible = false;
            this.longDesc.Width = 150;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Quantity.DataPropertyName = "quantity";
            this.Quantity.HeaderText = "Qty";
            this.Quantity.MinimumWidth = 40;
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.Width = 48;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "price";
            this.Price.HeaderText = "R.Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Width = 60;
            // 
            // DiscDollar
            // 
            this.DiscDollar.DataPropertyName = "DPrice";
            this.DiscDollar.HeaderText = "D.Price";
            this.DiscDollar.Name = "DiscDollar";
            this.DiscDollar.ReadOnly = true;
            this.DiscDollar.Width = 60;
            // 
            // Discount
            // 
            this.Discount.DataPropertyName = "Disc(%)";
            this.Discount.HeaderText = "Disc(%)";
            this.Discount.Name = "Discount";
            this.Discount.ReadOnly = true;
            this.Discount.Width = 60;
            // 
            // DiscountDetail
            // 
            this.DiscountDetail.DataPropertyName = "DiscountDetail";
            this.DiscountDetail.HeaderText = "Discount";
            this.DiscountDetail.Name = "DiscountDetail";
            this.DiscountDetail.ReadOnly = true;
            this.DiscountDetail.Width = 80;
            // 
            // PriceLevel
            // 
            this.PriceLevel.DataPropertyName = "PriceLevelName";
            this.PriceLevel.FillWeight = 75F;
            this.PriceLevel.HeaderText = "Apply";
            this.PriceLevel.MinimumWidth = 75;
            this.PriceLevel.Name = "PriceLevel";
            this.PriceLevel.Text = "Disc";
            this.PriceLevel.Width = 75;
            // 
            // GSTCol
            // 
            this.GSTCol.DataPropertyName = "GSTAmount";
            dataGridViewCellStyle8.Format = "C2";
            dataGridViewCellStyle8.NullValue = null;
            this.GSTCol.DefaultCellStyle = dataGridViewCellStyle8;
            this.GSTCol.HeaderText = "Tax";
            this.GSTCol.MinimumWidth = 60;
            this.GSTCol.Name = "GSTCol";
            this.GSTCol.Width = 60;
            // 
            // Amount
            // 
            this.Amount.DataPropertyName = "Amount";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Amount.DefaultCellStyle = dataGridViewCellStyle9;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Width = 70;
            // 
            // IsSpecial
            // 
            this.IsSpecial.DataPropertyName = "IsSpecial";
            this.IsSpecial.HeaderText = "S";
            this.IsSpecial.Name = "IsSpecial";
            this.IsSpecial.Width = 30;
            // 
            // IsPreOrder
            // 
            this.IsPreOrder.DataPropertyName = "IsPreOrder";
            this.IsPreOrder.HeaderText = "O";
            this.IsPreOrder.Name = "IsPreOrder";
            this.IsPreOrder.Width = 30;
            // 
            // FOC
            // 
            this.FOC.DataPropertyName = "IsFreeOfCharge";
            this.FOC.HeaderText = "FOC";
            this.FOC.Name = "FOC";
            this.FOC.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FOC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.FOC.Width = 40;
            // 
            // dgvcSalesPerson
            // 
            this.dgvcSalesPerson.DataPropertyName = "SalesPerson";
            this.dgvcSalesPerson.HeaderText = "Sales Person";
            this.dgvcSalesPerson.Name = "dgvcSalesPerson";
            // 
            // dgvcSalesPerson2
            // 
            this.dgvcSalesPerson2.DataPropertyName = "SalesPerson2";
            this.dgvcSalesPerson2.HeaderText = "2nd Sales Person";
            this.dgvcSalesPerson2.Name = "dgvcSalesPerson2";
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
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
            // frmTestGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 569);
            this.Controls.Add(this.dgvPurchase);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnLoadSample);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtBarcode);
            this.Name = "frmTestGrid";
            this.Text = "frmTestGrid";
            this.Load += new System.EventHandler(this.OrderTaking_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadSample;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox txtLogDetail;
        private System.Windows.Forms.RichTextBox txtLog;
        private FixReEntrantDataGridView dgvPurchase;
        private System.Windows.Forms.DataGridViewTextBoxColumn sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn longDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscDollar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountDetail;
        private System.Windows.Forms.DataGridViewButtonColumn PriceLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn GSTCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSpecial;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsPreOrder;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcSalesPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcSalesPerson2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewButtonColumn IsVoided;
        private System.Windows.Forms.DataGridViewButtonColumn LineCommission;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsPromo;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsNonDiscountable;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsExchange;
    }
}