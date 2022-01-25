namespace PowerInventory
{
    partial class frmInventoryLocationList
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
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteChecked = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnNewCategory = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToResizeRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnEdit,
            this.CategoryName,
            this.remark});
            this.dgvItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvItems.Location = new System.Drawing.Point(2, 58);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowHeadersWidth = 23;
            this.dgvItems.Size = new System.Drawing.Size(793, 531);
            this.dgvItems.TabIndex = 34;
            this.dgvItems.TabStop = false;
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.btnEdit.HeaderText = "";
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.btnEdit.Width = 40;
            // 
            // CategoryName
            // 
            this.CategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CategoryName.DataPropertyName = "InventoryLocationName";
            this.CategoryName.HeaderText = "Inventory Location";
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.ReadOnly = true;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.remark.DataPropertyName = "InventoryLocationID";
            this.remark.HeaderText = "InventoryLocationID";
            this.remark.Name = "remark";
            this.remark.ReadOnly = true;
            this.remark.Visible = false;
            // 
            // btnDeleteChecked
            // 
            this.btnDeleteChecked.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnDeleteChecked.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteChecked.ForeColor = System.Drawing.Color.White;
            this.btnDeleteChecked.Location = new System.Drawing.Point(-3, 12);
            this.btnDeleteChecked.Name = "btnDeleteChecked";
            this.btnDeleteChecked.Size = new System.Drawing.Size(138, 40);
            this.btnDeleteChecked.TabIndex = 38;
            this.btnDeleteChecked.Text = "Delete Checked";
            this.btnDeleteChecked.UseVisualStyleBackColor = true;
            this.btnDeleteChecked.Click += new System.EventHandler(this.btnDeleteChecked_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnExport.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(657, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(138, 40);
            this.btnExport.TabIndex = 37;
            this.btnExport.Text = "Export To Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnNewCategory
            // 
            this.btnNewCategory.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnNewCategory.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCategory.ForeColor = System.Drawing.Color.White;
            this.btnNewCategory.Location = new System.Drawing.Point(141, 12);
            this.btnNewCategory.Name = "btnNewCategory";
            this.btnNewCategory.Size = new System.Drawing.Size(138, 40);
            this.btnNewCategory.TabIndex = 39;
            this.btnNewCategory.Text = "New Inventory Location";
            this.btnNewCategory.UseVisualStyleBackColor = true;
            this.btnNewCategory.Click += new System.EventHandler(this.btnNewUser_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            this.fsdExportToExcel.Title = "Save File";
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // frmInventoryLocationList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(799, 592);
            this.Controls.Add(this.btnNewCategory);
            this.Controls.Add(this.btnDeleteChecked);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgvItems);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(1, 1);
            this.Name = "frmInventoryLocationList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Inventory Location Management";
            this.Load += new System.EventHandler(this.frmUserMst_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItems;
        internal System.Windows.Forms.Button btnDeleteChecked;
        internal System.Windows.Forms.Button btnExport;
        internal System.Windows.Forms.Button btnNewCategory;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
    }
}