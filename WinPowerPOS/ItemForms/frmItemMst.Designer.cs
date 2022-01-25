namespace WinPowerPOS.ItemForms
{
    partial class frmItemMst
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ItemDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attributes5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnNewItem = new System.Windows.Forms.Button();
            this.btnDeleteChecked = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnNewCategory = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.optBarcodeError = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.tSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(771, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(138, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(363, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 33);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "GO";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToResizeRows = false;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnEdit,
            this.ItemDepartment,
            this.categoryname,
            this.itemno,
            this.itemname,
            this.barcode,
            this.price,
            this.ItemDesc,
            this.Attributes1,
            this.Attributes2,
            this.Attributes3,
            this.Attributes4,
            this.Attributes5});
            this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvItems.Location = new System.Drawing.Point(0, 106);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowHeadersWidth = 23;
            this.dgvItems.Size = new System.Drawing.Size(921, 286);
            this.dgvItems.TabIndex = 1;
            this.dgvItems.TabStop = false;
            this.dgvItems.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvItems_CellValidating);
            this.dgvItems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellClick);
            this.dgvItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvItems_KeyDown);
            // 
            // btnEdit
            // 
            this.btnEdit.HeaderText = "";
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnEdit.Width = 60;
            // 
            // ItemDepartment
            // 
            this.ItemDepartment.DataPropertyName = "DepartmentName";
            this.ItemDepartment.HeaderText = "Department";
            this.ItemDepartment.Name = "ItemDepartment";
            this.ItemDepartment.ReadOnly = true;
            // 
            // categoryname
            // 
            this.categoryname.DataPropertyName = "CategoryName";
            this.categoryname.HeaderText = "Category";
            this.categoryname.Name = "categoryname";
            this.categoryname.ReadOnly = true;
            this.categoryname.Width = 140;
            // 
            // itemno
            // 
            this.itemno.DataPropertyName = "ItemNo";
            this.itemno.HeaderText = "Item No";
            this.itemno.Name = "itemno";
            this.itemno.ReadOnly = true;
            this.itemno.Width = 140;
            // 
            // itemname
            // 
            this.itemname.DataPropertyName = "ItemName";
            this.itemname.HeaderText = "Item Name";
            this.itemname.Name = "itemname";
            this.itemname.Width = 150;
            // 
            // barcode
            // 
            this.barcode.DataPropertyName = "Barcode";
            this.barcode.HeaderText = "Barcode";
            this.barcode.Name = "barcode";
            // 
            // price
            // 
            this.price.DataPropertyName = "RetailPrice";
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = null;
            this.price.DefaultCellStyle = dataGridViewCellStyle2;
            this.price.HeaderText = "R.Price";
            this.price.Name = "price";
            this.price.Width = 80;
            // 
            // ItemDesc
            // 
            this.ItemDesc.DataPropertyName = "ItemDesc";
            this.ItemDesc.HeaderText = "Desc";
            this.ItemDesc.Name = "ItemDesc";
            // 
            // Attributes1
            // 
            this.Attributes1.DataPropertyName = "Attributes1";
            this.Attributes1.HeaderText = "Attributes1";
            this.Attributes1.Name = "Attributes1";
            this.Attributes1.Width = 120;
            // 
            // Attributes2
            // 
            this.Attributes2.DataPropertyName = "Attributes2";
            this.Attributes2.HeaderText = "Attributes2";
            this.Attributes2.Name = "Attributes2";
            this.Attributes2.Width = 120;
            // 
            // Attributes3
            // 
            this.Attributes3.DataPropertyName = "Attributes3";
            this.Attributes3.HeaderText = "Attributes3";
            this.Attributes3.Name = "Attributes3";
            // 
            // Attributes4
            // 
            this.Attributes4.DataPropertyName = "Attributes4";
            this.Attributes4.HeaderText = "Attributes4";
            this.Attributes4.Name = "Attributes4";
            // 
            // Attributes5
            // 
            this.Attributes5.DataPropertyName = "Attributes5";
            this.Attributes5.HeaderText = "Attributes5";
            this.Attributes5.Name = "Attributes5";
            // 
            // btnNewItem
            // 
            this.btnNewItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewItem.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnNewItem.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewItem.ForeColor = System.Drawing.Color.White;
            this.btnNewItem.Location = new System.Drawing.Point(627, 63);
            this.btnNewItem.Name = "btnNewItem";
            this.btnNewItem.Size = new System.Drawing.Size(138, 40);
            this.btnNewItem.TabIndex = 3;
            this.btnNewItem.Text = "New Item";
            this.btnNewItem.UseVisualStyleBackColor = true;
            this.btnNewItem.Visible = false;
            this.btnNewItem.Click += new System.EventHandler(this.btnNewItem_Click);
            // 
            // btnDeleteChecked
            // 
            this.btnDeleteChecked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteChecked.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnDeleteChecked.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteChecked.ForeColor = System.Drawing.Color.White;
            this.btnDeleteChecked.Location = new System.Drawing.Point(339, 63);
            this.btnDeleteChecked.Name = "btnDeleteChecked";
            this.btnDeleteChecked.Size = new System.Drawing.Size(138, 40);
            this.btnDeleteChecked.TabIndex = 1;
            this.btnDeleteChecked.Text = "Delete Checked";
            this.btnDeleteChecked.UseVisualStyleBackColor = true;
            this.btnDeleteChecked.Visible = false;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnExport.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.Black;
            this.btnExport.Location = new System.Drawing.Point(771, 63);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(138, 40);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export To Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnNewCategory
            // 
            this.btnNewCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewCategory.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnNewCategory.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCategory.ForeColor = System.Drawing.Color.White;
            this.btnNewCategory.Location = new System.Drawing.Point(483, 63);
            this.btnNewCategory.Name = "btnNewCategory";
            this.btnNewCategory.Size = new System.Drawing.Size(138, 40);
            this.btnNewCategory.TabIndex = 2;
            this.btnNewCategory.Text = "New Category";
            this.btnNewCategory.UseVisualStyleBackColor = true;
            this.btnNewCategory.Visible = false;
            this.btnNewCategory.Click += new System.EventHandler(this.btnNewCategory_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            this.fsdExportToExcel.Title = "Save File";
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnNewCategory);
            this.panel1.Controls.Add(this.optBarcodeError);
            this.panel1.Controls.Add(this.btnDeleteChecked);
            this.panel1.Controls.Add(this.btnNewItem);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(921, 106);
            this.panel1.TabIndex = 0;
            // 
            // optBarcodeError
            // 
            this.optBarcodeError.Appearance = System.Windows.Forms.Appearance.Button;
            this.optBarcodeError.Location = new System.Drawing.Point(12, 63);
            this.optBarcodeError.Name = "optBarcodeError";
            this.optBarcodeError.Size = new System.Drawing.Size(138, 40);
            this.optBarcodeError.TabIndex = 8;
            this.optBarcodeError.Text = "Show all item with error Barcode";
            this.optBarcodeError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optBarcodeError.UseVisualStyleBackColor = true;
            this.optBarcodeError.CheckedChanged += new System.EventHandler(this.optBarcodeError_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.tSearch);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(921, 61);
            this.panel2.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(627, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 40);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tSearch
            // 
            this.tSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tSearch.Location = new System.Drawing.Point(84, 18);
            this.tSearch.Name = "tSearch";
            this.tSearch.Size = new System.Drawing.Size(262, 26);
            this.tSearch.TabIndex = 1;
            this.tSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tSearch_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Search : ";
            // 
            // frmItemMst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(921, 392);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(1, 1);
            this.Name = "frmItemMst";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Products";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmItemMst_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvItems;
        internal System.Windows.Forms.Button btnNewItem;
        internal System.Windows.Forms.Button btnDeleteChecked;
        internal System.Windows.Forms.Button btnExport;
        internal System.Windows.Forms.Button btnNewCategory;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tSearch;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox optBarcodeError;
        private System.Windows.Forms.DataGridViewCheckBoxColumn btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryname;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemno;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemname;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attributes5;
    }
}