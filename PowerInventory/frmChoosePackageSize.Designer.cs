namespace PowerInventory
{
    partial class frmChoosePackageSize
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
            this.PackingSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnNoPacking = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PackingSize,
            this.UOM,
            this.Qty,
            this.Cost});
            this.dataGridView1.Location = new System.Drawing.Point(12, 63);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(512, 266);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // PackingSize
            // 
            this.PackingSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PackingSize.DataPropertyName = "PackingSize";
            this.PackingSize.HeaderText = "Packing Size";
            this.PackingSize.Name = "PackingSize";
            this.PackingSize.ReadOnly = true;
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            this.UOM.HeaderText = "Size ";
            this.UOM.Name = "UOM";
            this.UOM.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.DataPropertyName = "Qty";
            this.Qty.HeaderText = "UOM";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            // 
            // Cost
            // 
            this.Cost.DataPropertyName = "Cost";
            this.Cost.HeaderText = "Cost Price";
            this.Cost.Name = "Cost";
            this.Cost.ReadOnly = true;
            // 
            // btnNoPacking
            // 
            this.btnNoPacking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNoPacking.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnNoPacking.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnNoPacking.ForeColor = System.Drawing.Color.White;
            this.btnNoPacking.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNoPacking.Location = new System.Drawing.Point(12, 10);
            this.btnNoPacking.Margin = new System.Windows.Forms.Padding(1);
            this.btnNoPacking.Name = "btnNoPacking";
            this.btnNoPacking.Size = new System.Drawing.Size(124, 43);
            this.btnNoPacking.TabIndex = 112;
            this.btnNoPacking.Text = "NO PACKING";
            this.btnNoPacking.UseVisualStyleBackColor = true;
            this.btnNoPacking.Click += new System.EventHandler(this.btnNoPacking_Click);
            // 
            // frmChoosePackageSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 341);
            this.Controls.Add(this.btnNoPacking);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmChoosePackageSize";
            this.Text = "Choose Package Size";
            this.Load += new System.EventHandler(this.frmChoosePackageSize_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackingSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
        protected System.Windows.Forms.Button btnNoPacking;

    }
}