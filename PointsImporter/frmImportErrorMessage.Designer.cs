namespace PointsImporter
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
            this.dgvErrorMessage = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.row = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dgvErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvErrorMessage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErrorMessage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.row,
            this.ItemNo,
            this.Barcode,
            this.Timestamp,
            this.Error});
            this.dgvErrorMessage.Location = new System.Drawing.Point(3, 36);
            this.dgvErrorMessage.Name = "dgvErrorMessage";
            this.dgvErrorMessage.RowHeadersVisible = false;
            this.dgvErrorMessage.Size = new System.Drawing.Size(719, 546);
            this.dgvErrorMessage.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Error found when importing the file";
            // 
            // row
            // 
            this.row.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.row.DataPropertyName = "row";
            this.row.HeaderText = "Row No";
            this.row.Name = "row";
            this.row.ReadOnly = true;
            this.row.Width = 80;
            // 
            // ItemNo
            // 
            this.ItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemNo.DataPropertyName = "itemno";
            this.ItemNo.HeaderText = "Item No";
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            // 
            // Barcode
            // 
            this.Barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Barcode.DataPropertyName = "membershipno";
            this.Barcode.HeaderText = "Membership No";
            this.Barcode.Name = "Barcode";
            this.Barcode.ReadOnly = true;
            // 
            // Timestamp
            // 
            this.Timestamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Timestamp.DataPropertyName = "timestamp";
            //this.Timestamp.HeaderText = global::PowerInventory.Properties.InventoryLanguage_th_TH.String1;
            this.Timestamp.Name = "Timestamp";
            this.Timestamp.ReadOnly = true;
            // 
            // Error
            // 
            this.Error.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Error.DataPropertyName = "error";
            this.Error.FillWeight = 200F;
            this.Error.HeaderText = "Error Message";
            this.Error.Name = "Error";
            this.Error.ReadOnly = true;
            // 
            // frmImportErrorMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 585);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvErrorMessage);
            this.Name = "frmImportErrorMessage";
            this.Text = "Import Error Message";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Timestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
    }
}