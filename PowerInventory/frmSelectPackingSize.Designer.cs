namespace PowerInventory
{
    partial class frmSelectPackingSize
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.dgvPacking = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ItemSupplierMapID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplierID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplierName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackingSizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackingSizeUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackingSizeCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPacking)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSelect.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSelect.ForeColor = System.Drawing.Color.White;
            this.btnSelect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelect.Location = new System.Drawing.Point(360, 171);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 30);
            this.btnSelect.TabIndex = 32;
            this.btnSelect.Text = "SELECT";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // dgvPacking
            // 
            this.dgvPacking.AllowUserToAddRows = false;
            this.dgvPacking.AllowUserToDeleteRows = false;
            this.dgvPacking.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPacking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPacking.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemSupplierMapID,
            this.ItemNo,
            this.SupplierID,
            this.SupplierName,
            this.PackingSizeName,
            this.PackingSizeUOM,
            this.PackingSizeCostPrice});
            this.dgvPacking.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvPacking.Location = new System.Drawing.Point(0, 0);
            this.dgvPacking.MultiSelect = false;
            this.dgvPacking.Name = "dgvPacking";
            this.dgvPacking.ReadOnly = true;
            this.dgvPacking.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvPacking.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPacking.Size = new System.Drawing.Size(435, 165);
            this.dgvPacking.TabIndex = 31;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(282, 171);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ItemSupplierMapID
            // 
            this.ItemSupplierMapID.DataPropertyName = "ItemSupplierMapID";
            this.ItemSupplierMapID.HeaderText = "ItemSupplierMapID";
            this.ItemSupplierMapID.Name = "ItemSupplierMapID";
            this.ItemSupplierMapID.ReadOnly = true;
            this.ItemSupplierMapID.Visible = false;
            // 
            // ItemNo
            // 
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.HeaderText = "ItemNo";
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            this.ItemNo.Visible = false;
            // 
            // SupplierID
            // 
            this.SupplierID.DataPropertyName = "SupplierID";
            this.SupplierID.HeaderText = "SupplierID";
            this.SupplierID.Name = "SupplierID";
            this.SupplierID.ReadOnly = true;
            this.SupplierID.Visible = false;
            // 
            // SupplierName
            // 
            this.SupplierName.DataPropertyName = "SupplierName";
            this.SupplierName.HeaderText = "SupplierName";
            this.SupplierName.Name = "SupplierName";
            this.SupplierName.ReadOnly = true;
            this.SupplierName.Visible = false;
            // 
            // PackingSizeName
            // 
            this.PackingSizeName.DataPropertyName = "PackingSizeName";
            this.PackingSizeName.HeaderText = "Packing Size";
            this.PackingSizeName.Name = "PackingSizeName";
            this.PackingSizeName.ReadOnly = true;
            // 
            // PackingSizeUOM
            // 
            this.PackingSizeUOM.DataPropertyName = "PackingSizeUOM";
            dataGridViewCellStyle1.Format = "N2";
            this.PackingSizeUOM.DefaultCellStyle = dataGridViewCellStyle1;
            this.PackingSizeUOM.HeaderText = "Conversion Rate";
            this.PackingSizeUOM.Name = "PackingSizeUOM";
            this.PackingSizeUOM.ReadOnly = true;
            // 
            // PackingSizeCostPrice
            // 
            this.PackingSizeCostPrice.DataPropertyName = "PackingSizeCostPrice";
            dataGridViewCellStyle2.Format = "N2";
            this.PackingSizeCostPrice.DefaultCellStyle = dataGridViewCellStyle2;
            this.PackingSizeCostPrice.HeaderText = "Packing Size Cost";
            this.PackingSizeCostPrice.Name = "PackingSizeCostPrice";
            this.PackingSizeCostPrice.ReadOnly = true;
            // 
            // frmSelectPackingSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 205);
            this.ControlBox = false;
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.dgvPacking);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSelectPackingSize";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Packing Size";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPacking)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridView dgvPacking;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemSupplierMapID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupplierID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupplierName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackingSizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackingSizeUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackingSizeCostPrice;
    }
}