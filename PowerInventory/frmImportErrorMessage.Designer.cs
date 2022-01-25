namespace PowerInventory
{
    partial class frmImportErrorMessage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportErrorMessage));
			this.dgvErrorMessage = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.row = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgvErrorMessage)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvErrorMessage
			// 
			this.dgvErrorMessage.AllowUserToAddRows = false;
			this.dgvErrorMessage.AllowUserToDeleteRows = false;
			this.dgvErrorMessage.AllowUserToResizeRows = false;
			resources.ApplyResources(this.dgvErrorMessage, "dgvErrorMessage");
			this.dgvErrorMessage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvErrorMessage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.row,
            this.ItemNo,
            this.Barcode,
            this.Quantity,
            this.Timestamp,
            this.Error});
			this.dgvErrorMessage.Name = "dgvErrorMessage";
			this.dgvErrorMessage.RowHeadersVisible = false;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = System.Drawing.Color.DarkRed;
			this.label1.Name = "label1";
			// 
			// row
			// 
			this.row.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.row.DataPropertyName = "row";
			resources.ApplyResources(this.row, "row");
			this.row.Name = "row";
			this.row.ReadOnly = true;
			// 
			// ItemNo
			// 
			this.ItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ItemNo.DataPropertyName = "itemno";
			resources.ApplyResources(this.ItemNo, "ItemNo");
			this.ItemNo.Name = "ItemNo";
			this.ItemNo.ReadOnly = true;
			// 
			// Barcode
			// 
			this.Barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Barcode.DataPropertyName = "barcode";
			resources.ApplyResources(this.Barcode, "Barcode");
			this.Barcode.Name = "Barcode";
			this.Barcode.ReadOnly = true;
			// 
			// Quantity
			// 
			this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Quantity.DataPropertyName = "quantity";
			resources.ApplyResources(this.Quantity, "Quantity");
			this.Quantity.Name = "Quantity";
			this.Quantity.ReadOnly = true;
			// 
			// Timestamp
			// 
			this.Timestamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Timestamp.DataPropertyName = "timestamp";
			resources.ApplyResources(this.Timestamp, "Timestamp");
			this.Timestamp.Name = "Timestamp";
			this.Timestamp.ReadOnly = true;
			// 
			// Error
			// 
			this.Error.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Error.DataPropertyName = "error";
			this.Error.FillWeight = 200F;
			resources.ApplyResources(this.Error, "Error");
			this.Error.Name = "Error";
			this.Error.ReadOnly = true;
			// 
			// frmImportErrorMessage
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dgvErrorMessage);
			this.MinimizeBox = false;
			this.Name = "frmImportErrorMessage";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Load += new System.EventHandler(this.frmImportErrorMessage_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvErrorMessage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvErrorMessage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridViewTextBoxColumn row;
		private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Barcode;
		private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
		private System.Windows.Forms.DataGridViewTextBoxColumn Timestamp;
		private System.Windows.Forms.DataGridViewTextBoxColumn Error;
    }
}