namespace WinPowerPOS
{
    partial class frmTodayDiscount
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.ItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountedPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AMCPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AMCPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.Set = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SetPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SetAMCPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemId,
            this.Description,
            this.Price,
            this.DiscountedPrice,
            this.DiscountPercent,
            this.AMCPrice,
            this.AMCPercent,
            this.EndDate});
            this.dataGridView1.Location = new System.Drawing.Point(12, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(690, 147);
            this.dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Schedule Discount";
            // 
            // ItemId
            // 
            this.ItemId.HeaderText = "Item Id";
            this.ItemId.Name = "ItemId";
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            // 
            // DiscountedPrice
            // 
            this.DiscountedPrice.HeaderText = "DiscountedPrice";
            this.DiscountedPrice.Name = "DiscountedPrice";
            // 
            // DiscountPercent
            // 
            this.DiscountPercent.HeaderText = "%";
            this.DiscountPercent.Name = "DiscountPercent";
            this.DiscountPercent.Width = 20;
            // 
            // AMCPrice
            // 
            this.AMCPrice.HeaderText = "AMC Price";
            this.AMCPrice.Name = "AMCPrice";
            // 
            // AMCPercent
            // 
            this.AMCPercent.HeaderText = "%";
            this.AMCPercent.Name = "AMCPercent";
            this.AMCPercent.Width = 20;
            // 
            // EndDate
            // 
            this.EndDate.HeaderText = "End Date";
            this.EndDate.Name = "EndDate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Group Discount";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Set,
            this.SetPrice,
            this.Column1,
            this.SetAMCPrice,
            this.Column2});
            this.dataGridView2.Location = new System.Drawing.Point(12, 214);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(385, 117);
            this.dataGridView2.TabIndex = 3;
            // 
            // Set
            // 
            this.Set.HeaderText = "Set";
            this.Set.Name = "Set";
            // 
            // SetPrice
            // 
            this.SetPrice.HeaderText = "Set Price";
            this.SetPrice.Name = "SetPrice";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "%";
            this.Column1.Name = "Column1";
            this.Column1.Width = 20;
            // 
            // SetAMCPrice
            // 
            this.SetAMCPrice.HeaderText = "AMC Price";
            this.SetAMCPrice.Name = "SetAMCPrice";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "%";
            this.Column2.Name = "Column2";
            this.Column2.Width = 20;
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column3});
            this.dataGridView3.Location = new System.Drawing.Point(426, 214);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.Size = new System.Drawing.Size(276, 117);
            this.dataGridView3.TabIndex = 4;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Item Id";
            this.Column4.Name = "Column4";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Item In Set";
            this.Column3.Name = "Column3";
            // 
            // frmTodayDiscount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 345);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmTodayDiscount";
            this.Text = "Today\'s Discount";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountedPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn AMCPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn AMCPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Set;
        private System.Windows.Forms.DataGridViewTextBoxColumn SetPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SetAMCPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}