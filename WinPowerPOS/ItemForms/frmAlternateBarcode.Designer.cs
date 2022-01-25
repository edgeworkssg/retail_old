namespace WinPowerPOS.ItemForms
{
    partial class frmAlternateBarcode
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblItemNoName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAltBarcode = new System.Windows.Forms.TextBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.BarcodeIDCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item";
            // 
            // lblItemNoName
            // 
            this.lblItemNoName.AutoSize = true;
            this.lblItemNoName.Location = new System.Drawing.Point(46, 13);
            this.lblItemNoName.Name = "lblItemNoName";
            this.lblItemNoName.Size = new System.Drawing.Size(10, 13);
            this.lblItemNoName.TabIndex = 1;
            this.lblItemNoName.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(13, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Alternate Barcode";
            // 
            // txtAltBarcode
            // 
            this.txtAltBarcode.Location = new System.Drawing.Point(113, 44);
            this.txtAltBarcode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAltBarcode.Name = "txtAltBarcode";
            this.txtAltBarcode.Size = new System.Drawing.Size(232, 20);
            this.txtAltBarcode.TabIndex = 20;
            this.txtAltBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAltBarcode_KeyDown);
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToResizeRows = false;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnDelete,
            this.BarcodeIDCol,
            this.CategoryName});
            this.dgvItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvItems.Location = new System.Drawing.Point(2, 76);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowHeadersWidth = 23;
            this.dgvItems.Size = new System.Drawing.Size(529, 349);
            this.dgvItems.TabIndex = 35;
            this.dgvItems.TabStop = false;
            this.dgvItems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellClick);
            // 
            // btnAdd
            // 
            this.btnAdd.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(352, 36);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 34);
            this.btnAdd.TabIndex = 36;
            this.btnAdd.Text = "ADD";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.HeaderText = "";
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseColumnTextForButtonValue = true;
            this.btnDelete.Width = 80;
            // 
            // BarcodeIDCol
            // 
            this.BarcodeIDCol.DataPropertyName = "BarcodeID";
            this.BarcodeIDCol.HeaderText = "BarcodeId";
            this.BarcodeIDCol.Name = "BarcodeIDCol";
            this.BarcodeIDCol.Visible = false;
            // 
            // CategoryName
            // 
            this.CategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CategoryName.DataPropertyName = "Barcode";
            this.CategoryName.HeaderText = "Alternate Barcode";
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.ReadOnly = true;
            // 
            // frmAlternateBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 432);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAltBarcode);
            this.Controls.Add(this.lblItemNoName);
            this.Controls.Add(this.label1);
            this.Name = "frmAlternateBarcode";
            this.Text = "Alternate Barcode";
            this.Load += new System.EventHandler(this.frmAlternateBarcode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblItemNoName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAltBarcode;
        private System.Windows.Forms.DataGridView dgvItems;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridViewButtonColumn btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn BarcodeIDCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
    }
}