namespace PowerInventory
{
    partial class frmCategoryList
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAccountCategory = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtCategoryName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteChecked = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnNewCategory = new System.Windows.Forms.Button();
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.btnNewDepartment = new System.Windows.Forms.Button();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(706, 23);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 40);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(622, 23);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(78, 40);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.Transparent;
            this.groupBox9.Controls.Add(this.btnClose);
            this.groupBox9.Controls.Add(this.txtRemark);
            this.groupBox9.Controls.Add(this.btnSearch);
            this.groupBox9.Controls.Add(this.cmbDepartment);
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.label11);
            this.groupBox9.Controls.Add(this.txtAccountCategory);
            this.groupBox9.Controls.Add(this.label15);
            this.groupBox9.Controls.Add(this.txtCategoryName);
            this.groupBox9.Controls.Add(this.label16);
            this.groupBox9.Location = new System.Drawing.Point(2, 0);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(792, 74);
            this.groupBox9.TabIndex = 36;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "FILTER";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(412, 13);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(180, 21);
            this.txtRemark.TabIndex = 4;
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new System.Drawing.Point(411, 40);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(181, 23);
            this.cmbDepartment.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(326, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "Department";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(326, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 15);
            this.label11.TabIndex = 19;
            this.label11.Text = "Remark";
            // 
            // txtAccountCategory
            // 
            this.txtAccountCategory.Location = new System.Drawing.Point(111, 40);
            this.txtAccountCategory.Name = "txtAccountCategory";
            this.txtAccountCategory.Size = new System.Drawing.Size(175, 21);
            this.txtAccountCategory.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(6, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(82, 15);
            this.label15.TabIndex = 17;
            this.label15.Text = "Account Cat";
            // 
            // txtCategoryName
            // 
            this.txtCategoryName.Location = new System.Drawing.Point(111, 13);
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.Size = new System.Drawing.Size(175, 21);
            this.txtCategoryName.TabIndex = 0;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(6, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(105, 15);
            this.label16.TabIndex = 15;
            this.label16.Text = "Category Name";
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
            this.remark,
            this.displayname,
            this.groupname});
            this.dgvItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvItems.Location = new System.Drawing.Point(2, 122);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowHeadersWidth = 23;
            this.dgvItems.Size = new System.Drawing.Size(793, 467);
            this.dgvItems.TabIndex = 34;
            this.dgvItems.TabStop = false;
            this.dgvItems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellClick);
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
            this.CategoryName.DataPropertyName = "CategoryName";
            this.CategoryName.HeaderText = "Category";
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.ReadOnly = true;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.remark.DataPropertyName = "DepartmentName";
            this.remark.HeaderText = "Department";
            this.remark.Name = "remark";
            this.remark.ReadOnly = true;
            // 
            // displayname
            // 
            this.displayname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.displayname.DataPropertyName = "isforsaleyesno";
            dataGridViewCellStyle1.Format = "@";
            this.displayname.DefaultCellStyle = dataGridViewCellStyle1;
            this.displayname.HeaderText = "Is For Sale";
            this.displayname.Name = "displayname";
            this.displayname.ReadOnly = true;
            this.displayname.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.displayname.Visible = false;
            // 
            // groupname
            // 
            this.groupname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupname.DataPropertyName = "accountcategory";
            this.groupname.HeaderText = "Account Category";
            this.groupname.Name = "groupname";
            this.groupname.ReadOnly = true;
            // 
            // btnDeleteChecked
            // 
            this.btnDeleteChecked.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnDeleteChecked.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteChecked.ForeColor = System.Drawing.Color.White;
            this.btnDeleteChecked.Location = new System.Drawing.Point(2, 80);
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
            this.btnExport.Location = new System.Drawing.Point(656, 80);
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
            this.btnNewCategory.Location = new System.Drawing.Point(290, 80);
            this.btnNewCategory.Name = "btnNewCategory";
            this.btnNewCategory.Size = new System.Drawing.Size(138, 40);
            this.btnNewCategory.TabIndex = 39;
            this.btnNewCategory.Text = "New Category";
            this.btnNewCategory.UseVisualStyleBackColor = true;
            this.btnNewCategory.Click += new System.EventHandler(this.btnNewUser_Click);
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            this.fsdExportToExcel.Title = "Save File";
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // btnNewDepartment
            // 
            this.btnNewDepartment.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnNewDepartment.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewDepartment.ForeColor = System.Drawing.Color.White;
            this.btnNewDepartment.Location = new System.Drawing.Point(146, 80);
            this.btnNewDepartment.Name = "btnNewDepartment";
            this.btnNewDepartment.Size = new System.Drawing.Size(138, 40);
            this.btnNewDepartment.TabIndex = 40;
            this.btnNewDepartment.Text = "New Department";
            this.btnNewDepartment.UseVisualStyleBackColor = true;
            this.btnNewDepartment.Click += new System.EventHandler(this.btnNewDepartment_Click);
            // 
            // frmCategoryList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(799, 592);
            this.Controls.Add(this.btnNewDepartment);
            this.Controls.Add(this.btnNewCategory);
            this.Controls.Add(this.btnDeleteChecked);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.dgvItems);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(1, 1);
            this.Name = "frmCategoryList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Category Management";
            this.Load += new System.EventHandler(this.frmUserMst_Load);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtAccountCategory;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtCategoryName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.TextBox txtRemark;
        internal System.Windows.Forms.Button btnDeleteChecked;
        internal System.Windows.Forms.Button btnExport;
        internal System.Windows.Forms.Button btnNewCategory;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        internal System.Windows.Forms.Button btnNewDepartment;
        private System.Windows.Forms.DataGridViewCheckBoxColumn btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayname;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupname;
    }
}